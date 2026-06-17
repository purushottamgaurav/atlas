namespace DotNetConsole
{
    internal class DesignPattern
    {
        // Singleton Design pattern -- 1- Private constructor, 2- Static instance, 3- One access point
        // same as services.AddSingleton<Database>();
        public void SingletonCheck()
        {
            // both are same
            var a = DatabaseSingleton.GetInstance();
            var b = DatabaseSingleton.GetInstance();
        }

        // Factory Design pattern - method,class decide which object to create , you just ask for it.
        // 1- Common Interface, 2- Concrete classes, 3- Factory class

        public void FactoryCheck()
        {
            var notification = NotificationFactory.Create("email");
            notification.Send("Order confirmed!");

            var sms = NotificationFactory.Create("sms");
            sms.Send("Your OTP is 1234");
        }

    }

    // Singleton
    public class DatabaseSingleton
    {
        public static DatabaseSingleton _instance;

        private DatabaseSingleton() { }

        public static DatabaseSingleton GetInstance()
        {
            if (_instance == null)
                _instance = new DatabaseSingleton();

            return _instance;
        }

    }

    // Factory
    // 1. Common interface
    public interface INotification
    {
        void Send(string message);
    }

    // 2. Concrete classes
    public class EmailNotification : INotification
    {
        public void Send(string message) => Console.WriteLine($"Email: {message}");
    }

    public class SmsNotification : INotification
    {
        public void Send(string message) => Console.WriteLine($"SMS: {message}");
    }

    public class PushNotification : INotification
    {
        public void Send(string message) => Console.WriteLine($"Push: {message}");
    }

    // 3. Factory — one place that decides
    public class NotificationFactory
    {
        public static INotification Create(string type)
        {
            return type switch
            {
                "email" => new EmailNotification(),
                "sms" => new SmsNotification(),
                "push" => new PushNotification(),
                _ => throw new ArgumentException("Unknown type")
            };
        }
    }



}
