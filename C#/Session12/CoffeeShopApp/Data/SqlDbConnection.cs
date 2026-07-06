using Microsoft.Data.SqlClient;

namespace CoffeeShopApp.Data;

public sealed class SqlDbConnection
{
    private static SqlDbConnection? _instance;
    private static readonly object _lock = new object();
    private readonly string _connectionString;

    public SqlDbConnection()
    {
        _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=CoffeeShopDB;Integrated Security=True;TrustServerCertificate=True;";
    }

    public static SqlDbConnection instance
    {
        get
        {
            lock (_lock)
            {
                if(_instance == null)
                {
                    _instance = new SqlDbConnection();
                }
                return _instance;
            }
        }
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}