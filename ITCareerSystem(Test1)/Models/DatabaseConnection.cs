using System.Data.SqlClient;
public sealed class DatabaseConnection
{
    private static readonly object padlock = new object();
    private static DatabaseConnection instance = null;
    private readonly SqlConnection connection;

    private DatabaseConnection(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DataBaseConnection");
        connection = new SqlConnection(connectionString);
    }

    public static DatabaseConnection Instance(IConfiguration configuration)
    {
        lock (padlock)
        {
            if (instance == null)
            {
                instance = new DatabaseConnection(configuration);
            }
            return instance;
        }
    }

    public SqlConnection Connection
    {
        get { return connection; }
    }
}

