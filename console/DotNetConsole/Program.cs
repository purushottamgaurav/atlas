using DotNetConsole;

// Reflection example
//Basics.Reflection(typeof(Basics));

//Basics.EqualsVsDoubleEquals();

// OOps example
//Oops.ConstructorTest();

// Coding example

Console.WriteLine("Please enter the coding number to execute");
int quesNo = int.Parse(Console.ReadLine());
Type type = typeof(Coding);
var methods = type.GetMethods();

var method = methods.First(x => x.Name.Contains($"Q{quesNo}_"));

method.Invoke(null, null);
