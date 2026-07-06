using System.Diagnostics.Contracts;

namespace BuilderPattern
{
    public class Computer
    {
        public string CPU { get; }
        public int RAM { get; }
        public int Storage { get; } 
        public bool HasGPU { get; }

        private Computer(Builder builder)
        {
            CPU = builder.CPU;
            RAM = builder.RAM;
            Storage = builder.Storage;
            HasGPU = builder.HasGPU;
        }

        public void ShowComputerDetails()
        {
            Console.WriteLine($"CPU: {CPU} | RAM: {RAM} GB | Storage: {Storage} GB | HasGPU: {HasGPU}");
        }

        public class Builder
        {
            public string CPU { get; private set; } = string.Empty;
            public int RAM { get; private set;}
            public int Storage { get; private set; } 
            public bool HasGPU { get; private set; }

            public Builder HasCPU(string cpu)
            {
                CPU = cpu;
                return this;
            }

            public Builder HasRAM(int ram)
            {
                RAM = ram;
                return this;
            }

            public Builder HasStorage(int storage)
            {
                Storage = storage;
                return this;
            }

            public Builder HasGpu(bool hasGpu)
            {
                HasGPU = hasGpu;
                return this;
            }

            public Computer Build() => new Computer(this);
        }
    }
}