namespace DotNetConsole
{
    internal static class Basics
    {
        static Basics()
        {

        }

        // Reflection example
        public static string ReflectionTest { get; set; }
        internal static void Reflection(Type type)
        {
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                Console.WriteLine(property.Name);
            }
        }

        // .Equals vs ==
        internal static void EqualsVsDoubleEquals()
        {
            string a = "hello";
            string b = "hello";
            Console.WriteLine(a == b); // True, because string literals are interned
            Console.WriteLine(a.Equals(b)); // True, because the content is the same

            // Reference Type example
            object c = new string(new char[] { 'h', 'e', 'l', 'l', 'o' });
            object d = new string(new char[] { 'h', 'e', 'l', 'l', 'o' });
            Console.WriteLine(c == d); // False, because they are different object references
            Console.WriteLine(a.Equals(c)); // True, because the content is the same
        }
    }
}
