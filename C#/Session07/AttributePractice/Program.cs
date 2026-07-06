using System.Reflection;

namespace AttributePractice
{
    public class Program
    {
        static void Main(string[] args)
        {
            PrintAttributeInfo(typeof(FirstClass));
            PrintAttributeInfo(typeof(SecoundClass));
        }

        private static void PrintAttributeInfo(Type type)
        {
            Console.WriteLine($"Information for {type}");

            Attribute[] attributes = Attribute.GetCustomAttributes(type);

            foreach (Attribute attribute in attributes)
            {
                if(attribute is DeveloperAttribute d)
                {
                    Console.WriteLine(d.Name);
                    Console.WriteLine(d.Version);
                }
            }
        }
    }
}