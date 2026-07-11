using learning_di;

var services = new CustomServiceCollection();
services.AddTransient<IEmailService, EmailService>();
services.AddTransient<NotificationService, NotificationService>();


var serviceProvider = services.BuildServiceProvider();

var notificationService = serviceProvider.GetRequiredService<NotificationService>();
notificationService.Notify("f@test.com", "hey all. how are you?");