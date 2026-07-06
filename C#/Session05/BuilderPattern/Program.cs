namespace BuilderPattern
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("-----Builder Pattern-----");
            var pc = new Computer.Builder()
                        .HasCPU("Inter Core i7")
                        .HasRAM(16)
                        .HasStorage(1024)
                        .HasGpu(true)
                        .Build();
            
            pc.ShowComputerDetails();
        }
    }
}