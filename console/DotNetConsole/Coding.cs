
namespace DotNetConsole
{
    public static class Coding
    {
        static Coding()
        {

        }


        public static void Q1_ReverseString()
        {
            var name = "hello";
            char[] charsOld = name.ToCharArray();

            int stringLenth = charsOld.Length;

            char[] charsNew = new char[stringLenth];

            for (int i = stringLenth; i > 0; i--)
            {
                charsNew[stringLenth - i] = charsOld[i - 1];
            }

            var newString = new String(charsNew);
            Console.WriteLine(newString);

        }


        public static void Q2_IsPalindrome()
        {
            string word = "aba";
            char[] wordArray = word.ToCharArray();
            for (int i = 0; i < wordArray.Length / 2; i++)
            {
                if (wordArray[i] != wordArray[wordArray.Length - 1])
                {
                    Console.WriteLine("Not a palindrome");
                }
            }

            Console.WriteLine(" Word is a palindrome");
        }


        public static void Q31_Largest()
        {
            int[] array = new int[] { 4, 5, 9, 6, 6, 15 };
            int length = array.Length;
            int largest = array[0];

            for (int i = 0; i < length; i++)
            {
                if (array[i] > largest)
                    largest = array[i];
            }

            Console.WriteLine($"Largest number is {largest}");
        }

        public static void Q33_ArraySum()
        {
            int[] numbers = new int[] { 2, 5, 9, 7, 6 };

            int sum = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                sum += numbers[i];
            }

            Console.WriteLine($"The sum is {sum}");
        }

        public static void Q32_LowestNumber()
        {
            int[] nums = new int[] { 5, 8, 1, 9, 16, 54 };

            int lowest = nums[0];

            for (int i = 0; i < nums.Length; i++)
            {
                if (lowest > nums[i])
                    lowest = nums[i];
            }

            Console.WriteLine($"The smallest number is {lowest}");
        }

        public static void Q34_AverageArray()
        {
            int[] numbers = { 5, 8, 6, 8, 9, 4, 35 };
            double average = numbers.Average();

            Console.WriteLine($"The average is {average}");
        }

        public static void Q35_ReverseArray()
        {
            int[] numbers = { 1, 5, 8, 6, 3, 7, 9 };
            int[] reverse = new int[numbers.Length];

            for (int i = 0; i < numbers.Length; i++)
            {
                reverse[numbers.Length - 1 - i] = numbers[i];
            }

            Array.ForEach(reverse, x => Console.WriteLine(x));
        }

        //public static void Q36_RemoveDuplicates()
        //{
        //    int[] nums = { 1, 6, 2, 8, 6, 8 };
        //    for (int i = 0; i < nums.Length; i++)
        //    {
        //        for (int j = i + 1; j < nums.Length - 1; j++)
        //        {
        //            if (nums[i] == nums[j])
        //                 nums.RemoveAt(j);
        //        }
        //    }
        //}
    }
}
