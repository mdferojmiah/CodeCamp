using System.Security.Cryptography;
using System.Text;

namespace learning_identity;

public class EncryptionManager
{
    private static readonly byte[] _key = Encoding.UTF8.GetBytes("my32BitSecretKeyForEncryption123");

    public static (string? encryptedText, string? initializedVector) Encrypt(string text)
    {
        using Aes aes = Aes.Create();
        aes.Key = _key;

        var ivBytes = aes.IV;
        
        var plainBytes = Encoding.UTF8.GetBytes(text);
        var encryptedBytes = aes.EncryptCbc(plainBytes, ivBytes);

        var encryptedText = Convert.ToBase64String(encryptedBytes).TrimEnd('=');
        var initializedVector = Convert.ToBase64String(ivBytes).TrimEnd('=');

        return(encryptedText, initializedVector);
    }

    public static string? Decrypt(string encryptedText, string initializedVector)
    {
        using Aes aes = Aes.Create();
        aes.Key = _key;

        var base64Text = AddBase64Padding(encryptedText);
        var base64IV = AddBase64Padding(initializedVector);

        var encryptedBytes = Convert.FromBase64String(base64Text);
        var ivBytes = Convert.FromBase64String(base64IV);

        var decryptedBytes = aes.DecryptCbc(encryptedBytes, ivBytes);
        var decryptedText = Encoding.UTF8.GetString(decryptedBytes);

        return decryptedText;
    }

    private static string AddBase64Padding(string base64WithoutPadding)
    {
        // Base64 strings should have length divisible by 4
        // Add '=' characters to make it valid
        int paddingNeeded = (4 - (base64WithoutPadding.Length % 4)) % 4;
        return base64WithoutPadding + new string('=', paddingNeeded);
    }
}