using learning_ado_net;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

string connectionString = "Host=localhost;Port=5433;Username=postgres;Password=12345;Database=codecampdb";
var npgsqlConnection = new NpgsqlConnection(connectionString);

var app = builder.Build();

app.MapGet("/todos", async () =>
{
    var todoService = new TodoService();
    await todoService.AddTodoAsync("coding", npgsqlConnection);
});


app.Run();

