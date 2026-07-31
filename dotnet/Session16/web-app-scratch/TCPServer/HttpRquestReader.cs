using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace web_app_scratch.TCPServer;

public class HttpRequestReader
{
    private const int MaxHeaderSize = 32 * 1024;
    private const int MaxBodySize = 10 * 1024 * 1024;

    public static async Task<(byte[] rawHeader, byte[] rawBody)> ReadAsync(NetworkStream stream)
    {
        using var memoryBuffer = new MemoryStream();
        var tempBuffer = new byte[4096]; // 4kb buffer
        var headerEnd = -1;

        // running the loop untill the header end found
        while (true)
        {
            //reading string 4kb at a time
            var read = await stream.ReadAsync(tempBuffer);
            if(read == 0)
            {
                throw new EndOfStreamException("Client disconnected before completing request.");
            }

            // adder the 4kb buffer to the memoryBuffer
            memoryBuffer.Write(tempBuffer, 0, read);
            var raw = memoryBuffer.ToArray(); // converting into bytes

            // checking if the end of header found or not
            for(int i = 0; i < raw.Length - 3; i++)
            {
                // if found break the loop and set headerEnd index
                if(raw[i] == '\r' && raw[i + 1] == '\n' && raw[i + 2] == '\r' && raw[i + 3] == '\n')
                {
                    headerEnd = i + 4;
                    break;
                }
            }

            //if headerEnd is set then stop reading the stream by ending the while loop
            if(headerEnd != -1) break;

            //checking if the header has crossed the maxHeaderSize
            if (memoryBuffer.Length > MaxHeaderSize)
            {
                throw new HttpException(413, "Request Header too large.");
            }
        }
        
        // converting memoryBuffer to Bytes
        var headerBytes = memoryBuffer.ToArray()[..headerEnd];
        var headerText = Encoding.UTF8.GetString(headerBytes); // converting bytes to string
        int contentLength = 0;

        foreach(var line in headerText.Split("\r\n")) //spliting the header by new lines
        {
            //getting the content-length to parse body from strem
            if (line.StartsWith("Content-Length", StringComparison.OrdinalIgnoreCase))
            {
                _ = int.TryParse(line["Content-Length".Length..].Trim(), out contentLength);
                break;
            }
        }

        byte[] bodyBytes = [];
        if(contentLength > 0)
        {
            //cheking if the requested content length is allowed or not
            if(contentLength > MaxBodySize)
            {
                throw new HttpException(413, "Request Body too large.");
            }

            //reading the body from the stream
            using var bodyBuffer = new MemoryStream();
            var alreadyReaded = (int)memoryBuffer.Length - headerEnd; // if some body content has already been readed it will contain value

            //writing the readed body content to bodyBuffer
            if(alreadyReaded > 0) bodyBuffer.Write(memoryBuffer.ToArray(), headerEnd, alreadyReaded);
            var totalRead = alreadyReaded;

            while(totalRead < contentLength)
            {
                // reading rest of the body content from stream
                var read = await stream.ReadAsync(tempBuffer, 0, Math.Min(contentLength - totalRead, tempBuffer.Length));

                // throwing exception when the body couldn't be read
                if(read == 0) throw new EndOfStreamException("Client disconnected before reading body.");

                // writing the readed stream to the bodyBuffer
                bodyBuffer.Write(tempBuffer, 0, read);
                totalRead += read;
            }

            // converting bodyBuffer to Bytes
            bodyBytes = bodyBuffer.ToArray();
        }
        return (headerBytes, bodyBytes);
    }
}