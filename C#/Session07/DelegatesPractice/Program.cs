namespace DelegatesPractice
{
    public class Program
    {
        //Delegates
        public delegate int Calculate(int x, int y);
        static int Add(int a, int b) => a + b;
        static int Multiply(int a, int b) => a * b; 

        static void Greet(string name, int id) => Console.WriteLine($"Hi, {name}! Your ID is {id}");

        static double Divide(int a, int b) => a / (double)b;

        static bool IsEven(int x) => x % 2  == 0;

        static void Main(string[] args)
        {
            Calculate calc = Add;
            Console.WriteLine(calc(3, 4));
            calc = Multiply;
            Console.WriteLine(calc(3, 4));
            Console.WriteLine();

            //Action - returns void and accepts params
            Action<string, int>  say = Greet;
            say("Feroj", 37);
            Console.WriteLine();

            //Func - returns value and accepts params
            Func<int, int, double> div = Divide;
            Console.WriteLine("Division result: " + div(5, 2));
            Console.WriteLine();

            //Predicate - restuns bool and accepts one param
            Predicate<int> evenChecker = IsEven;
            List<int> nums = new () {1, 2, 3, 4, 5, 6};

            var evenNums = nums.FindAll(evenChecker);

            foreach(var num in evenNums)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
        }

    }
}