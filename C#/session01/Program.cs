using System;

namespace HelloWorld
{
    class Program
    {

        static void Main(string[] args)
        {
            string input = "123";
            int result = Convert.ToInt32(input);
            System.Console.WriteLine(input + ": " + input.GetType());
            System.Console.WriteLine(result + ": " + result.GetType());
        }
    }
}

