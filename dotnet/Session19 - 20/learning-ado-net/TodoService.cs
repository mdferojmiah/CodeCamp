using Npgsql;

namespace learning_ado_net;

public class TodoService
{
    public async Task AddTodoAsync(string title, NpgsqlConnection connection)
    {
        await connection.OpenAsync();
        string query = "INSERT INTO Todos (title) VALUES (@title)";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@title", title);

        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();
    }
}