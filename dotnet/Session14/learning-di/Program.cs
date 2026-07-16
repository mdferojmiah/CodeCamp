using learning_di;

var services = new CustomServiceCollection();
//services.AddTransient<IEmailService, EmailService>();
//services.AddTransient<NotificationService, NotificationService>();
services.AddTransient<ITransientService, TransientService>();
services.AddSingleton<ISingletonService, SingletonService>();
services.AddScoped<IScopedService,ScopedService>();


var serviceProvider = services.BuildServiceProvider();

var notificationService1 = serviceProvider.GetRequiredService<ITransientService>();
var notificationService2 = serviceProvider.GetRequiredService<ITransientService>();
var notificationService3 = serviceProvider.GetRequiredService<ITransientService>();
notificationService1.Print();
notificationService2.Print();
notificationService3.Print();
Console.WriteLine();

var singletonService1 = serviceProvider.GetRequiredService<ISingletonService>();
var singletonService2 = serviceProvider.GetRequiredService<ISingletonService>();
var singletonService3 = serviceProvider.GetRequiredService<ISingletonService>();
singletonService1.Print();
singletonService2.Print();
singletonService3.Print();
Console.WriteLine();

using (var scope = serviceProvider.CreateScope())
{
    var scopedProvider1 = scope.ServiceProvider.GetRequiredService<IScopedService>();
    var scopedProvider2 = scope.ServiceProvider.GetRequiredService<IScopedService>();
    var scopedProvider3 = scope.ServiceProvider.GetRequiredService<IScopedService>();
    scopedProvider1.Print();
    scopedProvider2.Print();
    scopedProvider3.Print();
    Console.WriteLine();
}

using (var scope2 = serviceProvider.CreateScope())
{
    var scopedProvider1 = scope2.ServiceProvider.GetRequiredService<IScopedService>();
    var scopedProvider2 = scope2.ServiceProvider.GetRequiredService<IScopedService>();
    var scopedProvider3 = scope2.ServiceProvider.GetRequiredService<IScopedService>();
    scopedProvider1.Print();
    scopedProvider2.Print();
    scopedProvider3.Print();
    Console.WriteLine();
}