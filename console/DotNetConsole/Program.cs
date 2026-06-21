using DotNetConsole;

Console.WriteLine("Select one from the below options: - (1/2/3/4)");
Console.WriteLine("1 - Coding");
Console.WriteLine("2 - Oops");
Console.WriteLine("3 - Design Pattern");
Console.WriteLine("4 - Solid Prinicpal");

int number = IsValid();

switch (number)
{
    case 1:
        Console.WriteLine("Please enter the coding number to execute");
        int quesNo = int.Parse(Console.ReadLine());
        Type type = typeof(Coding);
        var methods = type.GetMethods();

        var method = methods.First(x => x.Name.Contains($"Q{quesNo}_"));

        method.Invoke(null, null);
        break;

    case 2:
        //StaticTest.StaticTestt();
        AbstractTest.Test();
        break;

    case 3:
        break;
}

int IsValid()
{
    while (true)
    {
        if (int.TryParse(Console.ReadLine(), out int number))
        {
            return number;
        }

        Console.WriteLine("Please enter a value like 1, 2, or 3");
    }
}
