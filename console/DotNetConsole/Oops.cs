
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

    #region Static Class
    // Static Class - cannot create object, cannot be inherited, cannot inherit others
    public static class ConstantPG
    {
        public const string procName = "dsadad";

        public static int GetSalary(string employee)
        {
            if (employee == "PG")
                return 80;
            else return 50;
        }
    }

    public static class Extensions
    {
        public static string ToStringg(this string word)
        {
            return word.ToUpper();
        }
    }

    public static class StaticTest
    {
        public static void StaticTestt()
        {
            string procName = ConstantPG.procName;

            //DSADAD
            var modifyWord = procName.ToStringg();

            int getSalary = ConstantPG.GetSalary("PG");
            Console.WriteLine(getSalary);
        }
    }

    #endregion

    #region Abstract Class 

    // Abstract Class - cannot create object, can be inherited, can inherit others, can contain abstract and non - abstract methods
    public abstract class Employee
    {
        public int EmployeeId { get; set; }

        //Abstract Method
        public abstract int PhoneNumber { get;}

        //Virtual Method
        public virtual int GetSalary()
        {
            return 50000;
        }

        //Normal Method
        public string GetGender(char c)
        {
            return c == 'M' ? "Male" : "Female";           
        }
    }

    public class Manager: Employee  
    {
        public override int PhoneNumber => 2131231;

        public override int GetSalary()
        {
            return 80000;
        }
       
        public virtual int sdcasa()
        {
            return 5;
        }
    }


    public static class AbstractTest 
    {
        public static void Test()
        {
            Manager a = new Manager();
            string gender= a.GetGender('F');
            int salary = a.GetSalary();
        }
    }

    #endregion
}
