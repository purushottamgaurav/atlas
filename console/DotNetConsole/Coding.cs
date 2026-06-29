using System.Text;

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

            //LinQ
            //Console.WriteLine(new string(name.Reverse().ToArray()));

            //char[] charsOld = name.ToCharArray();

            //int stringLenth = charsOld.Length;

            //char[] charsNew = new char[stringLenth];

            //for (int i = stringLenth; i > 0; i--)
            //{
            //    charsNew[stringLenth - i] = charsOld[i - 1];
            //}

            //var newString = new String(charsNew);
            //Console.WriteLine(newString);

            // I 2
            string reverse = string.Empty;
            for(int i = 0; i< name.Length;i++)
            {
                reverse += name[name.Length - i-1];
            }

            Console.WriteLine(reverse);
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




        public static void Q3_CountVowels_Consonants()
        {
            string word = "My Name is Purushottam Gaurav";
            char[] wordArray = word.ToArray();

            int vcount = 0, ccount = 0;

            for (int i = 0; i < wordArray.Length; i++)
            {
                if (wordArray[i] == ' ')
                    continue;
                else if (wordArray[i] == 'a' || wordArray[i] == 'e' || wordArray[i] == 'i' || wordArray[i] == 'o' || wordArray[i] == 'u')
                    vcount++;
                else
                    ccount++;

            }

            Console.WriteLine($"Consonants- {ccount}, Vowels- {vcount}");
        }

        public static void Q4_Anagram()
        {
            string word = "listen";
            string word2 = "silentt";

            char[] a = word.ToArray();
            char[] b = word2.ToArray();

            Array.Sort(a);
            Array.Sort(b);

            if (new string(a) != new string(b))
            {
                Console.WriteLine("Not an anagram");
            }
            else
            {
                Console.WriteLine("Anagram");
            }
        }

        public static void Q5_CountOccurance()
        {
            string word = "programming";

            Console.WriteLine($"Which character you want to count in {word}");
            char c = char.Parse(Console.ReadLine());

            char[] array = word.ToArray();
            int count = 0;
            foreach (char ch in array)
            {
                if (ch == c)
                    count++;
            }

            Console.WriteLine(count);
        }

        public static void Q6_RemoveSpaces()
        {
            string word = " My name is Purushottam Gaurav";
            var final = new StringBuilder();

            char[] wordArray = word.ToArray();

            foreach (char c in wordArray)
            {
                if (c == ' ')
                    continue;
                else
                    final.Append(c);
            }

            Console.WriteLine(final);
        }

        public static void Q7_ReverseWords()
        {
            string word = "My name is Purushottam Gaurav.";
            char[] wordArray = word.ToArray();
            char[] reverseWord = new char[wordArray.Length];

            for (int i = 0; i < wordArray.Length; i++)
            {
                reverseWord[i] = wordArray[wordArray.Length - i - 1];
            }

            var result = new String(reverseWord);
            Console.WriteLine(result);
        }

        public static void Q8_FirstNonRepeating()
        {
            string word = "aaabbccdee";
            char[] wordArray = word.ToArray();
            char previous = wordArray[0];

            for (int i = 0; i < wordArray.Length; i++)
            {
                if (i > 0)
                    previous = wordArray[i - 1];

                if (wordArray[i] != wordArray[i + 1] && wordArray[i] != previous)
                {
                    Console.WriteLine(wordArray[i]);
                    break;
                }

            }
        }

        public static void Q9_FindDuplicateCharacters()
        {
            string word = "Purushottam Gaurav";
            char[] wordArray = word.ToArray();

            //char[] duplicateArray = new char[wordArray.Length];
            //int count = 0;
            //for (int i = 0; i < wordArray.Length; i++)
            //{
            //    if (isDuplicate(wordArray[i]))
            //        continue;

            //    for (int j = i + 1; j < wordArray.Length; j++)
            //    {
            //        if (wordArray[i] == wordArray[j])
            //        {
            //            duplicateArray[count] = wordArray[i];
            //            count++;
            //            break;
            //        }
            //    }
            //}

            //bool isDuplicate(char c)
            //{
            //    foreach (char cd in duplicateArray)
            //    {
            //        if (cd == c)
            //            return true;
            //    }
            //    return false;
            //}

            //Array.ForEach(duplicateArray, x => Console.WriteLine(x));

            // I 2
            Dictionary<char, int> unique = new Dictionary<char, int>();

            foreach( char c in word)
            {
                if (unique.ContainsKey(c))
                {
                    unique[c]++;
                }
                else
                    unique[c] = 1;
            }

            foreach(var v in unique)
            {
                if(v.Value>1)
                {
                    Console.WriteLine($"The duplicate entries are {v.Key}: Count {v.Value}");
                }
            }

        }

        public static void Q10_OnlyDigitString()
        {
            string word = "156787";
            char[] wordArray = word.ToArray();
            bool digit = true;

            foreach (char c in wordArray)
            {
                if (char.IsLetter(c))
                {
                    digit = false;
                    break;
                }
            }

            Console.WriteLine($"It contains only digit: {digit}");
        }


        public static void Q11_CountWords()
        {
            string sentence = " My name is PUrushottam Gaurav";
            int count = 0;
            string sanity = sentence.Trim();

            char[] wordArray = sentence.ToArray();

            foreach (char c in wordArray)
            {
                if (c == ' ')
                    count++;
            }

            Console.WriteLine($"No of words in the sentence is {count}");
        }

        public static void Q12_TitleCase()
        {
            string word = "my name is purushottam gaurav";
            string sanitedWord = word.Trim();

            char[] wordArray = word.ToArray();
            bool spaceDetected = true;

            for (int i = 0; i < wordArray.Length; i++)
            {
                if (spaceDetected)
                    wordArray[i] = char.ToUpper(wordArray[i]);

                if (wordArray[i] == ' ')
                {
                    spaceDetected = true;
                }
                else
                {
                    spaceDetected = false;
                }

            }

            Console.WriteLine(new string(wordArray));
        }

        public static void Q14_RemoveDuplicates()
        {
            string name = "Purushottam";

            //LinQ
            //var a = new string(name.Distinct().ToArray());
            //Console.WriteLine(a);

            Dictionary<char, int> lookup = new Dictionary<char, int>();
            string result = "";

            foreach (char c in name)
            {
                if (!lookup.ContainsKey(c))
                {
                    lookup[c] = 1;
                    result += c;
                }
            }

            Console.WriteLine(result);
        }

        public static void Q15_LongestWord()
        {
            string word = " My name is Purushottam Gaurav";

            //LinQ
            //var longest = word.Split(' ').OrderByDescending(x=>x.Length).First();
            //Console.WriteLine(longest);

            var allWords = word.Split(' ');
            int max = 0;

            foreach (string s in allWords)
            {
                if (s.Length > max)
                    max = s.Length;
            }

            Console.WriteLine(new string(allWords.Where(x => x.Length == max).First()));
        }

        public static void Q16_CountFrequency()
        {
            string word = " Purushottam";

            //LinQ
            //word.GroupBy(x => x).OrderBy(g=>g.Key).ToList().ForEach(x=>Console.WriteLine());

            Dictionary<char, int> lookUp = new Dictionary<char, int>();

            foreach (char c in word)
            {
                if (lookUp.ContainsKey(c))
                {
                    lookUp[c]++;
                }
                else
                    lookUp[c] = 1;

            }

            foreach (var k in lookUp)
            {
                Console.WriteLine($"{k.Key} : {k.Value}");
            }
        }

        public static void Q17_ReplaceOccurance()
        {
            string word = "I love cats, cats are amazing";

            //LinQ
            //Console.WriteLine( word.Replace("cats", "dogs"));


        }

        public static void Q18_SubString()
        {
            string subString = "Helloo";
            string word = "Hello World";

            //if(word.Contains(subString))
            //{
            //    Console.WriteLine("Is a sub string");
            //}

            // T - 2

        }

        public static void Q19_Compress()
        {
            string word = "aaaabbccccccdd";

            Dictionary<char, int> freq = new();

            foreach( char c in word)
            {
                if (freq.ContainsKey(c))
                    freq[c]++;
                else freq[c] = 1;
            }

            foreach( var v in freq)
            {
                Console.Write(v.Key + "" + v.Value);
            }
        }

        public static void Q20_ParantehsisBalanced()
        {
            string paranthesis = "[({})]";
            var stack = new Stack<char>();

            foreach( char c in paranthesis)
            {
                if (c == '(' || c == '{' || c == '[')
                    stack.Push(c);
                else
                {
                    if (stack.Count == 0)
                    {
                        Console.WriteLine("Not Balanced");
                        return;
                    }

                    char poped = stack.Pop();

                    if (c==')'&& poped!='(')
                    {
                        Console.WriteLine("Not Balanced");
                        break;
                    }
                    else if (c=='}' && poped!='{')
                    {
                        Console.WriteLine("Not Balanced");
                        break;
                    }
                    else if(c==']' && poped!='[')
                    {
                        Console.WriteLine("Not Balanced");
                        break;
                    }

                }
            }

            Console.WriteLine("Balanced");

        }


        public static void Q21_Most_Frequent()
        {
            string word = "mississippi";

            Dictionary<char, int> freq = new();
            foreach(char c in word)
            {
                if (freq.ContainsKey(c))
                    freq[c]++;
                else freq[c] = 1;
            }

            var highest = freq.OrderByDescending(x => x.Value).First();
            Console.WriteLine(highest.Key + "" + highest.Value);
        }

        public static void Q22_UniqueChara()
        {
            string word = "puru";

            if (word.Distinct().Count() == word.Length)
                Console.WriteLine("Is Unique");
        }

        public static void Q23_ReverseVowels()
        {
              
        }

        public static void Q24_CountUpperLower()
        {
            string word = "Hello World";

            int upper=0, lower=0;

            foreach(char c in word)
            {
                char lowerCase = char.ToLower(c);
                if (c == lowerCase)
                    lower++;
                else
                    upper++;
            }

            Console.WriteLine("Lower : {0}, Upper : {1}",lower,upper);
        }

        public static void Q51_Largest()
        {
            int[] array = new int[] { 4, 5, 9, 6, 6, 15 };
            //LinQ
            //Console.WriteLine(array.Max());

            int length = array.Length;
            int largest = array[0];

            for (int i = 0; i < length; i++)
            {
                if (array[i] > largest)
                    largest = array[i];
            }

            Console.WriteLine($"Largest number is {largest}");
        }

        public static void Q52_LowestNumber()
        {
            int[] nums = new int[] { 5, 8, 1, 9, 16, 54 };

            //LinQ
            //Console.WriteLine(nums.Min());

            int lowest = nums[0];

            for (int i = 0; i < nums.Length; i++)
            {
                if (lowest > nums[i])
                    lowest = nums[i];
            }

            Console.WriteLine($"The smallest number is {lowest}");
        }

        public static void Q53_ArraySum()
        {
            int[] numbers = new int[] { 2, 5, 9, 7, 6 };

            //LinQ
            //Console.WriteLine(numbers.Sum());

            int sum = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                sum += numbers[i];
            }

            Console.WriteLine($"The sum is {sum}");
        }

 

        public static void Q54_AverageArray()
        {
            int[] numbers = { 5, 8, 6, 8, 9, 4, 35 };
            double average = numbers.Average();

            Console.WriteLine($"The average is {average}");
        }

        public static void Q55_ReverseArray()
        {
            int[] numbers = { 1, 5, 8, 6, 3, 7, 9 };
            int[] reverse = new int[numbers.Length];

            for (int i = 0; i < numbers.Length; i++)
            {
                reverse[numbers.Length - 1 - i] = numbers[i];
            }

            Array.ForEach(reverse, x => Console.WriteLine(x));
        }

        public static void Q56_RemoveDuplicates()
        {
            //int[] nums = { 1, 6, 2, 2, 4, 4, 1 };
            //int[] result = nums.Distinct().ToArray();

            int[] nums = { 1, 6, 2, 2, 4, 4, 1 };
            int[] newArray = new int[nums.Length];
            newArray[0] = nums[0];
            int uniquecount = 1;

            bool IsUnique(int number)
            {
                for (int i = 0; i < newArray.Length; i++)
                {
                    if (newArray[i] == number)
                        return false;
                }

                return true;
            }

            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = i + 1; j <= nums.Length - 1; j++)
                {
                    if (nums[i] != nums[j])
                    {
                        if (!IsUnique(nums[j]))
                            continue;

                        newArray[uniquecount] = nums[j];
                        uniquecount++;
                    }
                }
            }

            int[] result = new int[uniquecount];
            for (int i = 0; i < uniquecount; i++)
            {
                result[i] = newArray[i];
            }

            Array.ForEach(result, x => Console.WriteLine(x));

        }

        public static void Q60_TwoNumbersTarget()
        {
            int[] array = [2,7,11,15];
            int target = 18;
            for(int i = 0; i<array.Length;i++)
            {
                for( int j = i+ 1; j<array.Length;j++)
                {
                    if (array[i] + array[j] == target)
                    {
                        Console.WriteLine("[{0},{1}]", array[i], array[j]);
                        return;
                    }
                    
                }
            }

        }

        public static void Q63_FindDuplicateNumber()
        {
            int[] array = [1, 2, 3, 2, 4, 3];

            HashSet<int> duplicate = new();

            for( int i = 0;i<array.Length;i++)
            {
                for(int j =1+i; j<array.Length;j++)
                {
                    if (array[i] == array[j])
                        duplicate.Add(array[i]);
                }
            }

            foreach( var c in duplicate)
            {
                Console.Write(c);

            }
        }

        public static void Q65_ZerosToEnd()
        {
            int[] array = [0, 1, 0, 3, 12];
            int[] newArray = new int[array.Length];

            int count = 0;
            for(int i = 0; i<array.Length;i++)
            {
                if (array[i] == 0)
                {
                    newArray[array.Length - 1 - i] = 0;
                }
                else
                {
                    newArray[count] = array[i];
                    count++;
                }

            }

            foreach( var v in newArray)
            {
                Console.Write(v);
            }
        }

        public static void Q68_UnionOfArray()
        {
            int[] array1 = [1,2,3];
            int[] array2 = [3,4,5];

            var length = array1.Length + array2.Length;

            var set = new HashSet<int>();

            foreach(var c in array1)
            {
                set.Add(c);
            }


            foreach (var c in array2)
            {
                set.Add(c);
            }

            foreach( var c in set.ToArray())
            {
                Console.Write(c);
            }
        }

        public static void Q69_ArraySpecificValue()
        {
            int[] array = [1, 2, 3, 4];

            Console.WriteLine(array.Contains(3));

        public static void Q57_SecondLargest()
        {
            int[] array = [1, 3, 5, 8, 9, 2];

            var a = array.OrderByDescending(c => c).ToArray();

            Console.WriteLine(a[1]);
        }

        public static void Q58_CheckArraySort()
        {
            int[] array = [1,2, 3, 5, 8, 9];

            for(int i = 0;i<array.Length-1;i++)
            {
                if (array[i] > array[i+1])
                {
                    Console.WriteLine("Not Sorted");
                    return;
                }
            }
            Console.WriteLine("Sorted");

        }

        public static void Q59_CountEvenOdd()
        {
            int[] array = [1, 5, 8, 4, 7, 9];

            int even = 0,odd = 0;

            foreach( var v in array)
            {
                if (v % 2 == 0)
                {
                    even++;
                }
                else odd++;
            }

            Console.WriteLine("Even: {0}, Odd: {1}",even, odd);
        }

        public static void Q76_RightTriangle()
        {
            Console.WriteLine("Print the number of rows");
            int n = int.Parse(Console.ReadLine());

            for (int i = 1; i < n + 1; i++)
            {
                for (int j = 1; j <= i; j++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
        }

        /*
         *    *
         *   ***
         *  *****
         * 
         * 
         */

        public static void Q77_PyramidStar()
        {
            Console.WriteLine("Print the number of rows");
            int n = int.Parse(Console.ReadLine());

            for (int i = n; i > 0; i--)
            {
                for (int j = 1; j < i; j++)
                {
                    Console.Write(" ");
                }

                for (int m = 0; m <= (n - i) * 2; m++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
        }

        public static void Q78_FloydsTraingle()
        {

            Console.WriteLine("Print the number of rows of Floyd's traingle");
            int n = int.Parse(Console.ReadLine());

            int count = 1;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    Console.Write(count + " ");
                    count++;
                }
                Console.WriteLine();
            }

        }

        public static void Q79_FizzBuzz()
        {
            // 1 to N ,,, multiple of 3 ,, print Fizz and multiple of 5 print Buzz multipleof both print fIzzBuzz
            int n = 20;

            for( int i = 1; i<n;i++)
            {
                if ((i % 3) == 0 && (i % 5) == 0)
                    Console.Write("FizzBuzz "); 
                else if ((i % 3) == 0)
                    Console.Write("Fizz ");
                else if ((i % 5) == 0)
                    Console.Write("Buzz ");
                else Console.Write(i+ " ");
            }

        }

        public static void Q101_PrimeNumber()
        {
            for (int i = 3; i < 100; i++)
            {
                bool prime = true;

                for (int j = 2; j < i; j++)
                {
                    if ((i % j) == 0)
                    {
                        prime = false;
                        break;
                    }

                }
                if (prime)
                    Console.Write(i + " ");
            }
        }

        public static void Q102_Factorial()
        {
            int n = 5;
            int factorial = 1;

            for ( int i = 1; i <= n; i ++)
            {
                factorial = factorial * i;
            }

            Console.WriteLine(factorial);
        }



    }
}
