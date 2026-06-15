
namespace DotNetConsole
{
    public static class Coding
    {
        static Coding()
        {
            
        }

        //Q_1 - "hello" -> "olleh"
        public static void Q1_ReverseString()
        {
            var name = "hello";
            char[] charsOld = name.ToCharArray();

            int stringLenth = charsOld.Length;

            char[] charsNew = new char[stringLenth] ;

            for (int i = stringLenth; i > 0; i--)
            {
                charsNew[stringLenth -i]= charsOld[i-1];
            }

            var newString = new String(charsNew);
            Console.WriteLine(newString);

        }
    }
}
