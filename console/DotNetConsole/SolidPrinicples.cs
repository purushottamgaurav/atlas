namespace DotNetConsole
{
    internal class SolidPrinicples
    {
        public void SolidPrincipal()
        {
            var order = new Order { Id = 1, Product = "Laptop", Amount = 1000 };

            // S — each class has one job
            var repo = new OrderRepository();
            repo.Save(order);

            // O — swap discount without touching OrderProcessor
            Discount discount = new LoyaltyDiscount();
            Console.WriteLine($"Final price: {discount.Apply(order.Amount)}");

            // L — swap child notifier, parent still works fine
            OrderNotifier notifier = new SmsOrderNotifier();
            notifier.Notify(order);   // works perfectly — no crash ✅

            // D — swap notifier without changing OrderProcessor
            INotifier channel = new SmsChannel();
            var processor = new OrderProcessor(channel);
            processor.Process(order);
        }
    }


    // ============================================================
    //  SOLID PRINCIPLES — One simple real-world example
    //  Scenario: Sending order notifications
    // ============================================================


    // ─────────────────────────────────────────────────────────────
    // S — Single Responsibility Principle
    //     Each class does ONE thing only.
    // ─────────────────────────────────────────────────────────────

    public class Order
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }
        public string CustomerEmail { get; set; }
    }

    // Only responsible for saving orders
    public class OrderRepository
    {
        public void Save(Order order)
            => Console.WriteLine($"Order {order.Id} saved.");
    }

    // Only responsible for sending notifications
    public class NotificationService
    {
        public void Notify(string message)
            => Console.WriteLine($"Notification: {message}");
    }

    // ❌ BAD — one class doing both (violates SRP)
    // public class OrderManager {
    //     public void Save(Order o) { }
    //     public void SendEmail(Order o) { }   ← two responsibilities
    // }


    // ─────────────────────────────────────────────────────────────
    // O — Open/Closed Principle
    //     Open for extension, closed for modification.
    //     Add new behavior without changing existing code.
    // ─────────────────────────────────────────────────────────────

    public abstract class Discount
    {
        public abstract decimal Apply(decimal price);
    }

    public class NoDiscount : Discount
    {
        public override decimal Apply(decimal price) => price;
    }

    public class SeasonalDiscount : Discount
    {
        public override decimal Apply(decimal price) => price * 0.9m; // 10% off
    }

    // ✅ Adding a new discount = new class, no existing code touched
    public class LoyaltyDiscount : Discount
    {
        public override decimal Apply(decimal price) => price * 0.8m; // 20% off
    }


    // ─────────────────────────────────────────────────────────────
    // L — Liskov Substitution Principle
    //     Child class must honour the parent's promise.
    //     Swap child in place of parent → should still work.
    // ─────────────────────────────────────────────────────────────

    // Parent makes a promise: every OrderNotifier CAN notify
    public abstract class OrderNotifier
    {
        public abstract void Notify(Order order);
    }

    public class EmailOrderNotifier : OrderNotifier
    {
        public override void Notify(Order order)
            => Console.WriteLine($"Email sent for order {order.Id}"); // ✅ keeps promise
    }

    public class SmsOrderNotifier : OrderNotifier
    {
        public override void Notify(Order order)
            => Console.WriteLine($"SMS sent for order {order.Id}");   // ✅ keeps promise
    }

    // ❌ BAD — breaks LSP, child throws on parent's method
    // public class FreeOrderNotifier : OrderNotifier
    // {
    //     public override void Notify(Order order)
    //         => throw new NotImplementedException("Free orders skip notification!");
    //     // Parent promised Notify() works — this child broke that promise ❌
    //     // Caller using OrderNotifier has no idea this will crash
    // }

    // ✅ GOOD — every child works wherever parent is expected
    // OrderNotifier notifier = new SmsOrderNotifier();  // or EmailOrderNotifier
    // notifier.Notify(order);  // always works, no surprises ✅

    // 💡 Memory tip: "Can I swap the child without the caller noticing?"
    //    YES = LSP satisfied ✅    NO (crashes/does nothing) = LSP violated ❌


    // ─────────────────────────────────────────────────────────────
    // I — Interface Segregation Principle
    //     Don't force a class to implement methods it doesn't need.
    //     LSP = child must keep ALL parent methods working
    //     ISP = class should only SIGN UP for what it can actually do
    // ─────────────────────────────────────────────────────────────

    // ❌ BAD — fat interface forces every notifier to implement all channels
    // public interface INotificationSender
    // {
    //     void SendEmail(Order order);
    //     void SendSms(Order order);
    //     void SendPush(Order order);
    // }
    //
    // SmsOnlyNotifier is FORCED to implement Email and Push it doesn't support
    // public class SmsOnlyNotifier : INotificationSender
    // {
    //     public void SendSms(Order order)   => Console.WriteLine("SMS sent");
    //     public void SendEmail(Order order) => throw new NotImplementedException(); // ❌ forced
    //     public void SendPush(Order order)  => throw new NotImplementedException(); // ❌ forced
    // }

    // ✅ GOOD — split into small focused interfaces
    //    Each class signs up only for what it actually supports
    public interface IEmailSender { void SendEmail(Order order); }
    public interface ISmsSender { void SendSms(Order order); }
    public interface IPushSender { void SendPush(Order order); }

    // SmsOnlyNotifier only signs ISmsSender — not forced to implement anything else
    public class SmsOnlyNotifier : ISmsSender
    {
        public void SendSms(Order order)
            => Console.WriteLine($"SMS sent for order {order.Id}"); // ✅ no junk methods
    }

    // FullNotifier supports all channels — signs all three interfaces
    public class FullNotifier : IEmailSender, ISmsSender, IPushSender
    {
        public void SendEmail(Order order) => Console.WriteLine($"Email sent for order {order.Id}");
        public void SendSms(Order order) => Console.WriteLine($"SMS sent for order {order.Id}");
        public void SendPush(Order order) => Console.WriteLine($"Push sent for order {order.Id}");
    }

    // 💡 Memory tip: "Am I writing a method just to throw NotImplementedException?"
    //    YES = split the interface ❌    NO = you're good ✅

    // ─────────────────────────────────────────────────────────────
    // LSP vs ISP — they look similar but solve different problems:
    //
    //  LSP → about CLASS HIERARCHY  — child must not break parent's behaviour
    //  ISP → about INTERFACE DESIGN — don't bundle unrelated methods together
    //
    //  Same symptom (NotImplementedException) — different cause:
    //  LSP — wrong inheritance    → FreeOrderNotifier shouldn't extend OrderNotifier
    //  ISP — wrong interface      → fat INotificationSender has too many methods
    // ─────────────────────────────────────────────────────────────


    // ─────────────────────────────────────────────────────────────
    // D — Dependency Inversion Principle
    //     Depend on abstractions, not concrete classes.
    // ─────────────────────────────────────────────────────────────

    // Abstraction
    public interface INotifier
    {
        void Send(string message);
    }

    // Concrete implementations
    public class EmailChannel : INotifier
    {
        public void Send(string message)
            => Console.WriteLine($"Email: {message}");
    }

    public class SmsChannel : INotifier
    {
        public void Send(string message)
            => Console.WriteLine($"SMS: {message}");
    }

    // OrderProcessor depends on INotifier (abstraction), NOT EmailChannel directly
    public class OrderProcessor
    {
        private readonly INotifier _notifier;

        // Inject whatever notifier you want
        public OrderProcessor(INotifier notifier)
        {
            _notifier = notifier;
        }

        public void Process(Order order)
        {
            Console.WriteLine($"Processing order {order.Id}");
            _notifier.Send($"Order {order.Id} confirmed!");
        }
    }

    // ❌ BAD — tightly coupled to EmailChannel
    // public class OrderProcessor {
    //     private EmailChannel _email = new EmailChannel(); ← hard dependency
    // }
}