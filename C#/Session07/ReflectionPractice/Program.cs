namespace ReflectionPractice
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Type type = typeof(DateTime);
            // Console.WriteLine("Type: " + type.Name);
            // Console.WriteLine("NameSpace: " + type.Namespace);
            // foreach (var method in type.GetMethods())
            // {
            //     Console.WriteLine(method.Name);
            // }
            Type t = typeof(List<string>);

            Console.WriteLine(t.Name); // returns List`1
            //as we know list is a generic type so we see the generic argument
            Console.WriteLine(t.GetGenericArguments()[0]);

            object instance = Activator.CreateInstance(t)!;

            var list = (List<string>)instance;

            list.Add("Hello reflection");
            Console.WriteLine(list[0]);
        }
    }
}