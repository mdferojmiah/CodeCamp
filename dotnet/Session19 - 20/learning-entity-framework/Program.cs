using learning_entity_framework.Data;
using learning_entity_framework.Interceptors;
using learning_entity_framework.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

var builder = WebApplication.CreateBuilder(args);

string connectionString = "Host=localhost;Port=5433;Username=postgres;Password=12345;Database=codecamp_db";

builder.Services.AddDbContext<AppDbContext>(options =>
                                            options.UseLazyLoadingProxies()
                                                .UseNpgsql(connectionString)
                                                .AddInterceptors(new SavechangesTimingInterceptor(), 
                                                                new QueryCheckInInterceptor()));    
               

var app = builder.Build();

app.MapGet("/users", (AppDbContext dbContext) =>
{
    var newPerson = new Person
    {
        Name = "tiger chan",
        Email = "tiger@test.com"  
    };

    dbContext.Persons.Add(newPerson);

    // //change tracker state checking
    // Console.WriteLine("-----After Add-----");
    // var afterAdd = dbContext.ChangeTracker.Entries().Select(x => new
    // {
    //     EntityName = x.Entity.GetType().Name,
    //     State = x.State.ToString()
    // });
    // foreach(var entity in afterAdd)
    // {
    //     Console.WriteLine($"EntityName: {entity.EntityName} | State: {entity.State}");
    // }

    dbContext.SaveChanges();

    return Results.Ok(newPerson);
});


app.MapGet("/one-to-one", (AppDbContext dbContext) =>
{
    var person1 = new Person
    {
        Name = "Jhon doe 1",
        Email = "jhondoe1@test.com",
        PersonAddress = new PersonAddress
        {
            Address = "123 Rose valey, Atlanta"
        }
    };

    var person2 = new Person
    {
        Name = "Rocky Macho",
        Email = "rocky@test.com",
        PersonAddress = new PersonAddress
        {
            Address = "123 Garman way, BuLand"
        }
    };
    var person3 = new Person
    {
        Name = "Abdur Rahman",
        Email = "rahman@test.com",
        PersonAddress = new PersonAddress
        {
            Address = "123 new Road, Makka"
        }
    };

    dbContext.Add(person1);
    dbContext.Add(person2);
    dbContext.Add(person3);
    dbContext.SaveChanges();

    return Results.Ok("Added!");
});


app.MapGet("/users-with-address", (AppDbContext dbContext) =>
{
    var users = dbContext.Persons
                        .Include(x => x.PersonAddress)
                        .ToList();
    
    return Results.Ok(users);
});


app.MapGet("/address", (AppDbContext dbContext) =>
{
    var address = dbContext.PersonAddresses
                        .Include(x => x.Person)
                        .FirstOrDefault();

    return Results.Ok(new
    {
        AddressId = address?.Id,
        AddressName = address?.Address,
        PersonId = address?.Person?.Id,
        PersonName = address?.Person?.Name,
        PersonEmail = address?.Person?.Email
    });
});


app.MapGet("/one-to-many", (AppDbContext dbContext) =>
{
    var user = dbContext.Persons
                        .FirstOrDefault(x => 
                            x.Id == Guid.Parse("019fedce-6fde-7278-8e89-cb4385722826"));
    
    if(user == null) return Results.NotFound("User not found");

    var order1 = new Order
    {
        ToTal = 1000,
        PersonId = user.Id
    };
    var order2 = new Order
    {
        ToTal = 1500,
        PersonId = user.Id
    };
    var order3 = new Order
    {
        ToTal = 800,
        PersonId = user.Id
    };

    dbContext.Orders.AddRange(order1, order2, order3);
    dbContext.SaveChanges();
    return Results.Ok("Orders added successfully");
});


app.MapGet("/persons-orders", (AppDbContext dbContext) =>
{
    var personWithOrders = dbContext.Persons
                        .Include(x => x.Orders)
                        .FirstOrDefault(x => 
                            x.Id == Guid.Parse("019fedce-6fde-7278-8e89-cb4385722826"));
    
    if(personWithOrders == null) return Results.NotFound($"No person found!");

    var orders = personWithOrders?.Orders?
                                .Select(x => new {
                                    Id = x.Id,
                                    ToTal = x.ToTal
                                });
    

    return Results.Ok(new
    {
        Id = personWithOrders?.Id,
        Name = personWithOrders?.Name,
        Email = personWithOrders?.Email,
        Orders = orders
    });
});


app.MapGet("/many-to-many", (AppDbContext dbContext) =>
{
    var student = new Student
    {
        Name = "Feroj Miah"
    };

    var course = new Course
    {
        Name = "Introduction to C#"
    };

    var enrollment = new Enrollment
    {
        Student = student,
        Course = course,
        EnrollmentAt = DateTime.UtcNow
    };

    dbContext.Add(enrollment);
    dbContext.SaveChanges();

    return Results.Ok("Enrollment added successfully.");
});

app.MapGet("/explict-loading", (AppDbContext dbContext) =>
{
    var student = dbContext.Students.FirstOrDefault();
    dbContext.Entry(student).Collection(e => e.Enrollments).Load(); //explicit loading

    return Results.Ok(new
    {
        StudentId = student?.Id,
        Name = student?.Name,
        Enrollment = student?.Enrollments
                            .Select(e => new 
                            {
                                CourseId =  e.CouseId,
                                EnrollmentAt = e.EnrollmentAt
                            })
    });
});

app.MapGet("/lazy-loading", (AppDbContext dbContext) =>
{
    //need to intall a package: microsoft.efcore.proxies
    //need to declare all the navigation property as virtual
    var student = dbContext.Students.FirstOrDefault();
    var enrollments = student?.Enrollments;

    return Results.Ok(new
    {
        StudenId = student?.Id,
        Name = student?.Name,
        Enrollment = enrollments?.Select(e => new
        {
            CourseId = e.CouseId,
            EnrollmentAt = e.EnrollmentAt
        })
    });
});

app.MapGet("/user", async (AppDbContext dbContext) =>
{
    var user = await dbContext.Persons.FirstOrDefaultAsync();
    return Results.Ok(new
    {
        UserId = user?.Id,
        UserName = user?.Name,
        UserEmail = user?.Email
    });
});
app.Run();
