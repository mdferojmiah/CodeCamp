/*public class Rectangle
{
    public double Width { get; set; }
    public double Height { get; set; }
    //computed property
    public double area => Width * Height;
    //data validation
    private double _width;
    public double ValidateWidth
    {
        get => _width;
        set
        {
            if(value < 0)
            {
                throw new ArgumentException("Width cannot be negative.");
            }
            _width = value;
        }
    }
}*/

//inheritance & method overriding
/*public class Person
{
    public string Name { get; set; } = string.Empty;

    public Person(string name) => Name = name;

    public virtual void Introduce() => Console.WriteLine($"Hi, I'm {Name}.");
}

public class Student : Person
{
    public string Major { get; set; } = string.Empty;

    public Student(string name, string major) : base(name)
    {
        Major = major;
    }
    
    public override void Introduce()
    {
        base.Introduce();
        Console.WriteLine($"I'm studying {Major}.");
    }
}*/

//method overriding & hiding
/*public class Animal
{
    public virtual void Speak() => Console.WriteLine("Animal is braking!");
    public void Move() => Console.WriteLine("Animal Moves!");
}

public class Dog: Animal
{
    public override void Speak() => Console.WriteLine("Dog is barking!");
    public new void Move() => Console.WriteLine("Dog runs!");   
}*/

//Encapsulation
/*public class BankAccount
{
    private decimal _balance;

    public  decimal Balance
    {
        get => _balance;
        private set
        {
            if(value < 0)
                throw new ArgumentException("Balance cannot be negative.");
            _balance = value;
        }
    }

    public void Deposit(decimal amount)
    {
        if(amount <= 0)
        {
            throw new ArgumentException("Deposit amount must be positive.");
        }
        Balance += amount;
        Console.WriteLine($"Deposit successful. Current balance: {Balance}");
    }
}*/


//Protected Access Modifier
/*public class Account
{
    protected decimal Balance{get; set;}

    public Account(decimal initialBalance)
    {
        Balance = initialBalance;
    }

    protected void LogTransaction(string message) => Console.WriteLine($"Log: {message}");
}

public class SavingsAccount: Account
{
    public SavingsAccount(decimal initialBalance): base(initialBalance) {}

    public void AddInterest(decimal interestRate)
    {
        decimal interest = Balance * interestRate;
        Balance += interest;
        LogTransaction($"Interest added: {interest}");
    }

    public void CheckBalance() => LogTransaction($"Current balance: {Balance}");
}*/

//has-a relationship
public class Engine
{
    public void start() => Console.WriteLine("Engine started.");
}

public class Car
{
    private readonly Engine _engine = new Engine();

    public void start()
    {
        _engine.start();
        Console.WriteLine("Car is ready to go!");
    }
}

class Program
{
    static void Main()
    {
        /*Student student = new Student("Alice", "Computer Science");
        student.Introduce();*/

        /*Animal a = new Dog();
        a.Speak(); // Dog is barking!
        a.Move();  // Animal Moves! hides the Dog's Move method, calls Animal's Move method

        Dog d = new Dog();
        d.Move(); // Dog runs! calls Dog's Move method*/

        /*BankAccount account = new BankAccount();
        account.Deposit(1000);
        Console.WriteLine($"Current Balance: {account.Balance}");
        //account.Balance = 500; // This will cause a compile-time error because the setter is private*/
        
        /*SavingsAccount savings = new SavingsAccount(1000);
        savings.AddInterest(0.05m); // Adds 5% interest
        savings.CheckBalance(); // Logs the current balance*/

        Car car = new Car();
        car.start();
    }
}