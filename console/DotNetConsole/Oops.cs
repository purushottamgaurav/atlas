
namespace DotNetConsole
{
    public static class Oops
    {
        static Oops()
        {
            
        }

        public static void ConstructorTest()
        {
            // 1. Default Constructor
            var defaultConn = new DatabaseConnection();
            Console.WriteLine($"Default Constructor: Server={defaultConn.Server}, Database={defaultConn.Database}");
            // 2. Parameterized Constructor
            var paramConn = new DatabaseConnection("myServer", "myDatabase");
            Console.WriteLine($"Parameterized Constructor: Server={paramConn.Server}, Database={paramConn.Database}");
            // 3. Copy Constructor
            var copyConn = new DatabaseConnection(paramConn);
            Console.WriteLine($"Copy Constructor: Server={copyConn.Server}, Database={copyConn.Database}");
        }
    }

    public class DatabaseConnection
    {
        public string Server { get; set; }
        public string Database { get; set; }

        // 1. Default Constructor
        public DatabaseConnection()
        {
            Server = "localhost";
            Database = "Master";
        }

        // 2. Parameterized Constructor
        public DatabaseConnection(string server, string database)
        {
            Server = server;
            Database = database;
        }

        // 3. Copy Constructor
        public DatabaseConnection(DatabaseConnection other)
        {
            Server = other.Server;
            Database = other.Database;
        }

        // 4. Static Constructor
        static DatabaseConnection()
        {
            Console.WriteLine("Static constructor called once.");
        }

        // 5. Private Constructor
        private DatabaseConnection(string connectionString)
        {
            // Used internally, and restrict object creation with this parameter
        }
    }
}
