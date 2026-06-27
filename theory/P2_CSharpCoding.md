# 200 Coding Interview Questions

> Every question includes the problem, example, and a clean C# solution.
> Focus: Strings, Arrays, Numbers, Patterns, OOP, Data Structures

---

## SECTION 1: STRING QUESTIONS (Q1–Q50)

---

### Q1. Reverse a String

**Problem:** Reverse the given string.

**Example:** `"hello"` → `"olleh"`

```csharp
string Reverse(string s) => new string(s.Reverse().ToArray());

// Manual approach
string ReverseManual(string s) {
    char[] arr = s.ToCharArray();
    int l = 0, r = arr.Length - 1;
    while (l < r) { (arr[l], arr[r]) = (arr[r], arr[l]); l++; r--; }
    return new string(arr);
}
```

---

### Q2. Check if a String is a Palindrome

**Problem:** Return true if the string reads the same forwards and backwards (ignore case).

**Example:** `"Madam"` → `true`, `"hello"` → `false`

```csharp
bool IsPalindrome(string s) {
    s = s.ToLower();
    int l = 0, r = s.Length - 1;
    while (l < r) {
        if (s[l] != s[r]) return false;
        l++; r--;
    }
    return true;
}
```

---

### Q3. Count Vowels and Consonants

**Problem:** Count the number of vowels and consonants in a string.

**Example:** `"Hello World"` → Vowels: 3, Consonants: 7

```csharp
void CountVowelsConsonants(string s) {
    s = s.ToLower();
    string vowels = "aeiou";
    int v = 0, c = 0;
    foreach (char ch in s) {
        if (char.IsLetter(ch)) {
            if (vowels.Contains(ch)) v++;
            else c++;
        }
    }
    Console.WriteLine($"Vowels: {v}, Consonants: {c}");
}
```

---

### Q4. Check if Two Strings are Anagrams

**Problem:** Two strings are anagrams if they contain the same characters in different order.

**Example:** `"listen"`, `"silent"` → `true`

```csharp
bool IsAnagram(string s1, string s2) {
    if (s1.Length != s2.Length) return false;
    return s1.ToLower().OrderBy(c => c)
             .SequenceEqual(s2.ToLower().OrderBy(c => c));
}
```

---

### Q5. Count Occurrences of a Character

**Problem:** Count how many times a given character appears in a string.

**Example:** `"programming"`, `'g'` → `2`

```csharp
int CountChar(string s, char c) => s.Count(x => x == c);
```

---

### Q6. Remove All Spaces from a String

**Problem:** Remove all whitespace characters from a string.

**Example:** `"Hello World How"` → `"HelloWorldHow"`

```csharp
string RemoveSpaces(string s) => new string(s.Where(c => c != ' ').ToArray());
// Or: string.Concat(s.Split(' '))
```

---

### Q7. Reverse Words in a Sentence

**Problem:** Reverse the order of words in a sentence (not the letters).

**Example:** `"Hello World"` → `"World Hello"`

```csharp
string ReverseWords(string s) {
    var words = s.Split(' ');
    return string.Join(" ", words.Reverse());
}
```

---

### Q8. Find the First Non-Repeating Character

**Problem:** Return the first character that does not repeat in the string.

**Example:** `"aabbcde"` → `'c'`

```csharp
char FirstNonRepeating(string s) {
    return s.GroupBy(c => c)
            .FirstOrDefault(g => g.Count() == 1)?.Key ?? '\0';
}
```

---

### Q9. Find Duplicate Characters in a String

**Problem:** Print all characters that appear more than once.

**Example:** `"programming"` → `r, g, m`

```csharp
void FindDuplicates(string s) {
    s.GroupBy(c => c)
     .Where(g => g.Count() > 1)
     .ToList()
     .ForEach(g => Console.Write(g.Key + " "));
}
```

---

### Q10. Check if a String Contains Only Digits

**Problem:** Return true if all characters in the string are digits.

**Example:** `"12345"` → `true`, `"123a5"` → `false`

```csharp
bool IsAllDigits(string s) => s.All(char.IsDigit);
```

---

### Q11. Count Words in a String

**Problem:** Count the number of words in a sentence.

**Example:** `"Hello World How Are You"` → `5`

```csharp
int CountWords(string s) =>
    s.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries).Length;
```

---

### Q12. Convert String to Title Case

**Problem:** Capitalize the first letter of every word.

**Example:** `"hello world"` → `"Hello World"`

```csharp
string ToTitleCase(string s) {
    return string.Join(" ", s.Split(' ')
        .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));
}
```

---

### Q13. Check if a String is a Rotation of Another

**Problem:** String B is a rotation of A if it contains all characters in the same circular order.

**Example:** `"abcde"`, `"cdeab"` → `true`

```csharp
bool IsRotation(string a, string b) {
    if (a.Length != b.Length) return false;
    return (a + a).Contains(b);
}
```

---

### Q14. Remove Duplicate Characters from a String

**Problem:** Keep only the first occurrence of each character.

**Example:** `"programming"` → `"progamin"`

```csharp
string RemoveDuplicates(string s) =>
    new string(s.Distinct().ToArray());
```

---

### Q15. Find the Longest Word in a Sentence

**Problem:** Return the longest word from a sentence.

**Example:** `"The quick brown fox"` → `"quick"`

```csharp
string LongestWord(string s) =>
    s.Split(' ').OrderByDescending(w => w.Length).First();
```

---

### Q16. Count the Frequency of Each Character

**Problem:** Print each character and how many times it appears.

**Example:** `"aabbc"` → `a:2, b:2, c:1`

```csharp
void CharFrequency(string s) {
    s.GroupBy(c => c)
     .OrderBy(g => g.Key)
     .ToList()
     .ForEach(g => Console.WriteLine($"{g.Key}: {g.Count()}"));
}
```

---

### Q17. Replace All Occurrences of a Word

**Problem:** Replace every occurrence of a target word with another.

**Example:** `"I love cats, cats are great"`, replace `"cats"` with `"dogs"` → `"I love dogs, dogs are great"`

```csharp
string ReplaceWord(string s, string from, string to) => s.Replace(from, to);
```

---

### Q18. Check if a String is a Substring of Another

**Problem:** Return true if `sub` exists inside `main`.

**Example:** `"hello world"`, `"world"` → `true`

```csharp
bool IsSubstring(string main, string sub) => main.Contains(sub);
```

---

### Q19. Compress a String (Run-Length Encoding)

**Problem:** Replace consecutive repeated characters with the character and its count.

**Example:** `"aaabbbccdddd"` → `"a3b3c2d4"`

```csharp
string Compress(string s) {
    var sb = new StringBuilder();
    int i = 0;
    while (i < s.Length) {
        char c = s[i];
        int count = 0;
        while (i < s.Length && s[i] == c) { count++; i++; }
        sb.Append(c).Append(count > 1 ? count.ToString() : "");
    }
    return sb.ToString();
}
```

---

### Q20. Check if Parentheses are Balanced

**Problem:** Return true if every opening bracket has a matching closing bracket in correct order.

**Example:** `"({[]})"` → `true`, `"({[})"` → `false`

```csharp
bool IsBalanced(string s) {
    var stack = new Stack<char>();
    foreach (char c in s) {
        if (c == '(' || c == '{' || c == '[') stack.Push(c);
        else {
            if (stack.Count == 0) return false;
            char top = stack.Pop();
            if (c == ')' && top != '(') return false;
            if (c == '}' && top != '{') return false;
            if (c == ']' && top != '[') return false;
        }
    }
    return stack.Count == 0;
}
```

---

### Q21. Find the Most Frequent Character

**Problem:** Return the character that appears the most times.

**Example:** `"mississippi"` → `'s'`

```csharp
char MostFrequent(string s) =>
    s.GroupBy(c => c).OrderByDescending(g => g.Count()).First().Key;
```

---

### Q22. Check if a String has All Unique Characters

**Problem:** Return true if no character repeats.

**Example:** `"abcdef"` → `true`, `"abcdea"` → `false`

```csharp
bool AllUnique(string s) => s.Length == s.Distinct().Count();
```

---

### Q23. Reverse Only the Vowels in a String

**Problem:** Reverse only the vowels, leave consonants in place.

**Example:** `"hello"` → `"holle"`

```csharp
string ReverseVowels(string s) {
    string vowels = "aeiouAEIOU";
    char[] arr = s.ToCharArray();
    int l = 0, r = arr.Length - 1;
    while (l < r) {
        while (l < r && !vowels.Contains(arr[l])) l++;
        while (l < r && !vowels.Contains(arr[r])) r--;
        (arr[l], arr[r]) = (arr[r], arr[l]);
        l++; r--;
    }
    return new string(arr);
}
```

---

### Q24. Count Uppercase and Lowercase Letters

**Problem:** Count how many uppercase and lowercase letters are in a string.

**Example:** `"Hello World"` → Upper: 2, Lower: 8

```csharp
void CountCase(string s) {
    int upper = s.Count(char.IsUpper);
    int lower = s.Count(char.IsLower);
    Console.WriteLine($"Upper: {upper}, Lower: {lower}");
}
```

---

### Q25. Find All Permutations of a String

**Problem:** Print all possible arrangements of a string's characters.

**Example:** `"abc"` → `abc, acb, bac, bca, cab, cba`

```csharp
void Permutations(string s, string result = "") {
    if (s.Length == 0) { Console.WriteLine(result); return; }
    for (int i = 0; i < s.Length; i++)
        Permutations(s.Remove(i, 1), result + s[i]);
}
```

---

### Q26. Convert a String to Integer (without `int.Parse`)

**Problem:** Manually convert a numeric string to an integer.

**Example:** `"12345"` → `12345`

```csharp
int StringToInt(string s) {
    int result = 0;
    bool negative = s[0] == '-';
    int start = negative ? 1 : 0;
    for (int i = start; i < s.Length; i++)
        result = result * 10 + (s[i] - '0');
    return negative ? -result : result;
}
```

---

### Q27. Check if Two Strings are Equal Ignoring Case

**Problem:** Compare two strings case-insensitively.

**Example:** `"Hello"`, `"hello"` → `true`

```csharp
bool EqualIgnoreCase(string a, string b) =>
    string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
```

---

### Q28. Find the Longest Common Prefix

**Problem:** Find the longest prefix shared by all strings in an array.

**Example:** `["flower", "flow", "flight"]` → `"fl"`

```csharp
string LongestCommonPrefix(string[] strs) {
    if (strs.Length == 0) return "";
    string prefix = strs[0];
    foreach (var s in strs)
        while (!s.StartsWith(prefix))
            prefix = prefix.Substring(0, prefix.Length - 1);
    return prefix;
}
```

---

### Q29. Truncate a String to N Words

**Problem:** Return only the first N words of a sentence.

**Example:** `"The quick brown fox"`, N=2 → `"The quick"`

```csharp
string TruncateToNWords(string s, int n) =>
    string.Join(" ", s.Split(' ').Take(n));
```

---

### Q30. Check if a String is a Valid Email (Basic Check)

**Problem:** Return true if the string contains `@` and a `.` after `@`.

**Example:** `"user@example.com"` → `true`, `"userexample.com"` → `false`

```csharp
bool IsValidEmail(string s) {
    int atIndex = s.IndexOf('@');
    if (atIndex < 1) return false;
    int dotIndex = s.LastIndexOf('.');
    return dotIndex > atIndex + 1 && dotIndex < s.Length - 1;
}
```

---

### Q31. Count the Number of Sentences in a Paragraph

**Problem:** Count sentences by counting `.`, `!`, and `?` characters.

**Example:** `"Hello. How are you? I am fine!"` → `3`

```csharp
int CountSentences(string s) => s.Count(c => c == '.' || c == '!' || c == '?');
```

---

### Q32. Check if a String is a Pangram

**Problem:** A pangram contains every letter of the alphabet at least once.

**Example:** `"The quick brown fox jumps over the lazy dog"` → `true`

```csharp
bool IsPangram(string s) {
    s = s.ToLower();
    return Enumerable.Range('a', 26).All(c => s.Contains((char)c));
}
```

---

### Q33. Toggle Case of Each Character

**Problem:** Convert uppercase to lowercase and vice versa for every character.

**Example:** `"Hello World"` → `"hELLO wORLD"`

```csharp
string ToggleCase(string s) =>
    new string(s.Select(c => char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c)).ToArray());
```

---

### Q34. Find the Shortest Word in a Sentence

**Problem:** Return the word with the fewest characters.

**Example:** `"The quick brown fox"` → `"The"`

```csharp
string ShortestWord(string s) => s.Split(' ').OrderBy(w => w.Length).First();
```

---

### Q35. Remove All Non-Alphanumeric Characters

**Problem:** Strip out everything that is not a letter or digit.

**Example:** `"Hello, World! 123"` → `"HelloWorld123"`

```csharp
string RemoveNonAlphanumeric(string s) =>
    new string(s.Where(char.IsLetterOrDigit).ToArray());
```

---

### Q36. Check if One String is a Prefix of Another

**Problem:** Return true if string A starts with string B.

**Example:** `"programming"`, `"prog"` → `true`

```csharp
bool IsPrefix(string s, string prefix) =>
    s.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
```

---

### Q37. Check if One String is a Suffix of Another

**Problem:** Return true if string A ends with string B.

**Example:** `"programming"`, `"ing"` → `true`

```csharp
bool IsSuffix(string s, string suffix) =>
    s.EndsWith(suffix, StringComparison.OrdinalIgnoreCase);
```

---

### Q38. Repeat a String N Times

**Problem:** Return the string repeated N times.

**Example:** `"ab"`, N=3 → `"ababab"`

```csharp
string RepeatString(string s, int n) => string.Concat(Enumerable.Repeat(s, n));
```

---

### Q39. Capitalize the First Letter Only

**Problem:** Make the first character uppercase and the rest lowercase.

**Example:** `"hELLO wORLD"` → `"Hello world"`

```csharp
string CapitalizeFirst(string s) =>
    char.ToUpper(s[0]) + s.Substring(1).ToLower();
```

---

### Q40. Find All Words That Appear More Than Once

**Problem:** Return a list of words that repeat in a sentence (case-insensitive).

**Example:** `"the cat sat on the mat the"` → `["the"]`

```csharp
List<string> RepeatedWords(string s) =>
    s.ToLower().Split(' ')
     .GroupBy(w => w)
     .Where(g => g.Count() > 1)
     .Select(g => g.Key)
     .ToList();
```

---

### Q41. Left Pad a String to a Fixed Width

**Problem:** Pad the string on the left with spaces (or a character) until it reaches length N.

**Example:** `"42"`, N=6 → `"    42"`

```csharp
string LeftPad(string s, int n, char padChar = ' ') => s.PadLeft(n, padChar);
```

---

### Q42. Right Pad a String to a Fixed Width

**Problem:** Pad the string on the right with a character until it reaches length N.

**Example:** `"hi"`, N=6, padChar=`'-'` → `"hi----"`

```csharp
string RightPad(string s, int n, char padChar = ' ') => s.PadRight(n, padChar);
```

---

### Q43. Extract All Numbers from a String

**Problem:** Pull out all numeric substrings from a mixed string.

**Example:** `"abc 123 def 456"` → `[123, 456]`

```csharp
List<int> ExtractNumbers(string s) =>
    System.Text.RegularExpressions.Regex.Matches(s, @"\d+")
        .Select(m => int.Parse(m.Value))
        .ToList();
```

---

### Q44. Find the Middle Character(s) of a String

**Problem:** Return the middle character if odd length, or middle two if even length.

**Example:** `"abcde"` → `"c"`, `"abcd"` → `"bc"`

```csharp
string MiddleChar(string s) {
    int mid = s.Length / 2;
    return s.Length % 2 == 0 ? s.Substring(mid - 1, 2) : s.Substring(mid, 1);
}
```

---

### Q45. Convert a Sentence to Camel Case

**Problem:** Convert words to camelCase (first word lowercase, rest capitalized).

**Example:** `"hello world foo"` → `"helloWorldFoo"`

```csharp
string ToCamelCase(string s) {
    var words = s.Split(' ');
    return words[0].ToLower() + string.Concat(
        words.Skip(1).Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));
}
```

---

### Q46. Count Matching Characters Between Two Strings (Same Position)

**Problem:** Count positions where both strings have the same character.

**Example:** `"abcde"`, `"axcye"` → `3` (a, c, e match)

```csharp
int CountMatchingChars(string a, string b) =>
    a.Zip(b, (x, y) => x == y).Count(match => match);
```

---

### Q47. Remove All Occurrences of a Given Character

**Problem:** Remove every occurrence of a specific character.

**Example:** `"hello world"`, remove `'l'` → `"heo word"`

```csharp
string RemoveChar(string s, char c) => new string(s.Where(x => x != c).ToArray());
```

---

### Q48. Check if String Contains Only Alphabets

**Problem:** Return true if the string has only letters (no digits or symbols).

**Example:** `"HelloWorld"` → `true`, `"Hello1"` → `false`

```csharp
bool IsAlphaOnly(string s) => s.All(char.IsLetter);
```

---

### Q49. Split a String by Multiple Delimiters

**Problem:** Split a string by comma, semicolon, and pipe.

**Example:** `"a,b;c|d"` → `["a", "b", "c", "d"]`

```csharp
string[] SplitMultiple(string s) =>
    s.Split(new char[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
```

---

### Q50. Count How Many Times a Substring Appears

**Problem:** Count occurrences of a substring inside another string.

**Example:** `"abcabcabc"`, `"abc"` → `3`

```csharp
int CountSubstring(string s, string sub) {
    int count = 0, index = 0;
    while ((index = s.IndexOf(sub, index)) != -1) { count++; index += sub.Length; }
    return count;
}
```

---

## SECTION 2: ARRAY QUESTIONS (Q51–Q110)

---

### Q51. Find the Maximum Element in an Array

**Problem:** Return the largest number in the array.

**Example:** `[3, 1, 7, 2, 9]` → `9`

```csharp
int FindMax(int[] arr) => arr.Max();

// Manual
int FindMaxManual(int[] arr) {
    int max = arr[0];
    foreach (int n in arr) if (n > max) max = n;
    return max;
}
```

---

### Q52. Find the Minimum Element in an Array

**Problem:** Return the smallest number in the array.

**Example:** `[3, 1, 7, 2, 9]` → `1`

```csharp
int FindMin(int[] arr) => arr.Min();
```

---

### Q53. Calculate the Sum of All Array Elements

**Problem:** Return the total sum.

**Example:** `[1, 2, 3, 4, 5]` → `15`

```csharp
int Sum(int[] arr) => arr.Sum();
```

---

### Q54. Find the Average of an Array

**Problem:** Return the average of all elements.

**Example:** `[10, 20, 30]` → `20.0`

```csharp
double Average(int[] arr) => arr.Average();
```

---

### Q55. Reverse an Array

**Problem:** Reverse the array in-place.

**Example:** `[1, 2, 3, 4, 5]` → `[5, 4, 3, 2, 1]`

```csharp
void ReverseArray(int[] arr) {
    int l = 0, r = arr.Length - 1;
    while (l < r) { (arr[l], arr[r]) = (arr[r], arr[l]); l++; r--; }
}
```

---

### Q56. Remove Duplicates from an Array

**Problem:** Return the array with duplicate values removed.

**Example:** `[1, 2, 2, 3, 3, 4]` → `[1, 2, 3, 4]`

```csharp
int[] RemoveDuplicates(int[] arr) => arr.Distinct().ToArray();
```

---

### Q57. Find the Second Largest Element

**Problem:** Return the second largest number (not a duplicate of max).

**Example:** `[10, 5, 20, 15]` → `15`

```csharp
int SecondLargest(int[] arr) => arr.Distinct().OrderByDescending(x => x).Skip(1).First();
```

---

### Q58. Check if an Array is Sorted (Ascending)

**Problem:** Return true if array is in non-decreasing order.

**Example:** `[1, 2, 3, 4]` → `true`, `[1, 3, 2]` → `false`

```csharp
bool IsSorted(int[] arr) {
    for (int i = 1; i < arr.Length; i++)
        if (arr[i] < arr[i - 1]) return false;
    return true;
}
```

---

### Q59. Count Even and Odd Numbers

**Problem:** Count how many numbers are even and how many are odd.

**Example:** `[1, 2, 3, 4, 5]` → Even: 2, Odd: 3

```csharp
void CountEvenOdd(int[] arr) {
    int even = arr.Count(x => x % 2 == 0);
    Console.WriteLine($"Even: {even}, Odd: {arr.Length - even}");
}
```

---

### Q60. Find the Two Numbers That Add Up to a Target (Two Sum)

**Problem:** Return the indices of two numbers that sum to the target.

**Example:** `[2, 7, 11, 15]`, target=9 → `[0, 1]`

```csharp
int[] TwoSum(int[] nums, int target) {
    var map = new Dictionary<int, int>();
    for (int i = 0; i < nums.Length; i++) {
        int complement = target - nums[i];
        if (map.ContainsKey(complement)) return new[] { map[complement], i };
        map[nums[i]] = i;
    }
    return Array.Empty<int>();
}
```

---

### Q61. Rotate an Array to the Right by K Steps

**Problem:** Shift all elements to the right by K positions. Elements that fall off the end wrap to the front.

**Example:** `[1,2,3,4,5]`, K=2 → `[4,5,1,2,3]`

```csharp
int[] RotateRight(int[] arr, int k) {
    k = k % arr.Length;
    return arr.Skip(arr.Length - k).Concat(arr.Take(arr.Length - k)).ToArray();
}
```

---

### Q62. Find the Missing Number in a Sequence

**Problem:** Array contains numbers 1 to N with one missing. Find it.

**Example:** `[1, 2, 4, 5]` → `3`

```csharp
int MissingNumber(int[] arr) {
    int n = arr.Length + 1;
    int expected = n * (n + 1) / 2;
    return expected - arr.Sum();
}
```

---

### Q63. Find All Duplicate Numbers in an Array

**Problem:** Return numbers that appear more than once.

**Example:** `[1, 2, 3, 2, 4, 3]` → `[2, 3]`

```csharp
int[] FindDuplicates(int[] arr) =>
    arr.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
```

---

### Q64. Merge Two Sorted Arrays

**Problem:** Merge two sorted arrays into a single sorted array.

**Example:** `[1, 3, 5]`, `[2, 4, 6]` → `[1, 2, 3, 4, 5, 6]`

```csharp
int[] MergeSorted(int[] a, int[] b) => a.Concat(b).OrderBy(x => x).ToArray();

// Manual O(n+m) merge
int[] MergeSortedManual(int[] a, int[] b) {
    int[] result = new int[a.Length + b.Length];
    int i = 0, j = 0, k = 0;
    while (i < a.Length && j < b.Length)
        result[k++] = a[i] <= b[j] ? a[i++] : b[j++];
    while (i < a.Length) result[k++] = a[i++];
    while (j < b.Length) result[k++] = b[j++];
    return result;
}
```

---

### Q65. Move All Zeros to the End

**Problem:** Move all zeros to the end while keeping the order of non-zero elements.

**Example:** `[0, 1, 0, 3, 12]` → `[1, 3, 12, 0, 0]`

```csharp
int[] MoveZeros(int[] arr) =>
    arr.Where(x => x != 0).Concat(arr.Where(x => x == 0)).ToArray();
```

---

### Q66. Find the Majority Element (appears more than n/2 times)

**Problem:** Return the element that appears more than half the time.

**Example:** `[3, 2, 3]` → `3`

```csharp
int MajorityElement(int[] nums) {
    int candidate = nums[0], count = 1;
    for (int i = 1; i < nums.Length; i++) {
        count += nums[i] == candidate ? 1 : -1;
        if (count == 0) { candidate = nums[i]; count = 1; }
    }
    return candidate;
}
```

---

### Q67. Find the Intersection of Two Arrays

**Problem:** Return elements that exist in both arrays.

**Example:** `[1, 2, 3, 4]`, `[3, 4, 5, 6]` → `[3, 4]`

```csharp
int[] Intersection(int[] a, int[] b) => a.Intersect(b).ToArray();
```

---

### Q68. Find the Union of Two Arrays

**Problem:** Return all unique elements from both arrays combined.

**Example:** `[1, 2, 3]`, `[3, 4, 5]` → `[1, 2, 3, 4, 5]`

```csharp
int[] Union(int[] a, int[] b) => a.Union(b).ToArray();
```

---

### Q69. Check if Array Contains a Specific Value

**Problem:** Return true if the value exists in the array.

**Example:** `[1, 2, 3, 4]`, value=3 → `true`

```csharp
bool Contains(int[] arr, int val) => arr.Contains(val);
```

---

### Q70. Sort an Array in Ascending and Descending Order

**Problem:** Sort the array both ways.

**Example:** `[5, 2, 8, 1]` → Asc: `[1, 2, 5, 8]`, Desc: `[8, 5, 2, 1]`

```csharp
int[] SortAsc(int[] arr) => arr.OrderBy(x => x).ToArray();
int[] SortDesc(int[] arr) => arr.OrderByDescending(x => x).ToArray();
```

---

### Q71. Find the Maximum Consecutive Ones

**Problem:** Return the maximum number of consecutive 1s in a binary array.

**Example:** `[1,1,0,1,1,1]` → `3`

```csharp
int MaxConsecutiveOnes(int[] nums) {
    int max = 0, count = 0;
    foreach (int n in nums) {
        count = n == 1 ? count + 1 : 0;
        max = Math.Max(max, count);
    }
    return max;
}
```

---

### Q72. Find the Subarray with the Maximum Sum (Kadane's Algorithm)

**Problem:** Return the largest possible sum of any contiguous subarray.

**Example:** `[-2,1,-3,4,-1,2,1,-5,4]` → `6` (subarray `[4,-1,2,1]`)

```csharp
int MaxSubarraySum(int[] nums) {
    int maxSum = nums[0], current = nums[0];
    for (int i = 1; i < nums.Length; i++) {
        current = Math.Max(nums[i], current + nums[i]);
        maxSum = Math.Max(maxSum, current);
    }
    return maxSum;
}
```

---

### Q73. Count Frequency of Each Element in an Array

**Problem:** Print each element and how many times it appears.

**Example:** `[1, 2, 2, 3, 3, 3]` → `1:1, 2:2, 3:3`

```csharp
void CountFrequency(int[] arr) {
    arr.GroupBy(x => x)
       .OrderBy(g => g.Key)
       .ToList()
       .ForEach(g => Console.WriteLine($"{g.Key}: {g.Count()}"));
}
```

---

### Q74. Find the Product of All Elements Except Self

**Problem:** Return an array where each index holds the product of all other elements. No division.

**Example:** `[1,2,3,4]` → `[24,12,8,6]`

```csharp
int[] ProductExceptSelf(int[] nums) {
    int n = nums.Length;
    int[] result = new int[n];
    result[0] = 1;
    for (int i = 1; i < n; i++) result[i] = result[i-1] * nums[i-1];
    int right = 1;
    for (int i = n - 1; i >= 0; i--) { result[i] *= right; right *= nums[i]; }
    return result;
}
```

---

### Q75. Print All Pairs with a Given Sum

**Problem:** Print all pairs of numbers from the array that add up to the target.

**Example:** `[1, 5, 3, 2, 4]`, target=6 → `(1,5), (2,4)`

```csharp
void PrintPairsWithSum(int[] arr, int target) {
    var seen = new HashSet<int>();
    foreach (int n in arr) {
        int complement = target - n;
        if (seen.Contains(complement))
            Console.WriteLine($"({complement}, {n})");
        seen.Add(n);
    }
}
```

---

### Q76. Check if Array is a Subset of Another

**Problem:** Return true if all elements of array A exist in array B.

**Example:** A=`[1, 2, 3]`, B=`[1, 2, 3, 4, 5]` → `true`

```csharp
bool IsSubset(int[] a, int[] b) => a.All(x => b.Contains(x));
```

---

### Q77. Flatten a 2D Array into 1D

**Problem:** Convert a jagged/2D array into a single flat array.

**Example:** `[[1,2],[3,4],[5]]` → `[1,2,3,4,5]`

```csharp
int[] Flatten(int[][] arr) => arr.SelectMany(x => x).ToArray();
```

---

### Q78. Find the Smallest and Largest in One Pass

**Problem:** Find both min and max in a single loop.

**Example:** `[3, 1, 7, 2, 9, 4]` → Min: 1, Max: 9

```csharp
void MinMax(int[] arr) {
    int min = arr[0], max = arr[0];
    foreach (int n in arr) { if (n < min) min = n; if (n > max) max = n; }
    Console.WriteLine($"Min: {min}, Max: {max}");
}
```

---

### Q79. Remove a Specific Element from an Array

**Problem:** Remove all occurrences of a given value.

**Example:** `[1, 2, 3, 2, 4]`, remove `2` → `[1, 3, 4]`

```csharp
int[] RemoveElement(int[] arr, int val) => arr.Where(x => x != val).ToArray();
```

---

### Q80. Find the Index of the First Occurrence of a Value

**Problem:** Return the index where the value first appears, or -1 if not found.

**Example:** `[5, 3, 7, 3, 9]`, value=3 → `1`

```csharp
int FirstIndex(int[] arr, int val) => Array.IndexOf(arr, val);
```

---

### Q81. Find the Kth Smallest Element

**Problem:** Return the Kth smallest value in an unsorted array.

**Example:** `[7, 10, 4, 3, 20, 15]`, K=3 → `7`

```csharp
int KthSmallest(int[] arr, int k) =>
    arr.OrderBy(x => x).ElementAt(k - 1);
```

---

### Q82. Find the Kth Largest Element

**Problem:** Return the Kth largest value.

**Example:** `[3, 2, 1, 5, 6, 4]`, K=2 → `5`

```csharp
int KthLargest(int[] arr, int k) =>
    arr.OrderByDescending(x => x).ElementAt(k - 1);
```

---

### Q83. Find the Longest Increasing Subarray

**Problem:** Return the length of the longest contiguous subarray that is strictly increasing.

**Example:** `[1, 2, 3, 1, 2, 3, 4]` → `4`

```csharp
int LongestIncreasing(int[] arr) {
    int max = 1, cur = 1;
    for (int i = 1; i < arr.Length; i++) {
        cur = arr[i] > arr[i - 1] ? cur + 1 : 1;
        max = Math.Max(max, cur);
    }
    return max;
}
```

---

### Q84. Separate Even and Odd Numbers

**Problem:** Rearrange so all even numbers come before all odd numbers.

**Example:** `[1, 2, 3, 4, 5]` → `[2, 4, 1, 3, 5]`

```csharp
int[] SeparateEvenOdd(int[] arr) =>
    arr.Where(x => x % 2 == 0).Concat(arr.Where(x => x % 2 != 0)).ToArray();
```

---

### Q85. Find the Smallest Positive Missing Number

**Problem:** Return the smallest positive integer not in the array.

**Example:** `[3, 4, -1, 1]` → `2`

```csharp
int SmallestMissingPositive(int[] arr) {
    var set = new HashSet<int>(arr);
    for (int i = 1; ; i++) if (!set.Contains(i)) return i;
}
```

---

### Q86. Chunk an Array into Groups of N

**Problem:** Split an array into sub-arrays of size N.

**Example:** `[1,2,3,4,5,6,7]`, N=3 → `[[1,2,3],[4,5,6],[7]]`

```csharp
List<int[]> Chunk(int[] arr, int n) {
    var result = new List<int[]>();
    for (int i = 0; i < arr.Length; i += n)
        result.Add(arr.Skip(i).Take(n).ToArray());
    return result;
}
```

---

### Q87. Find the Index of the Maximum Element

**Problem:** Return the index of the largest value in the array.

**Example:** `[10, 5, 20, 15]` → `2`

```csharp
int IndexOfMax(int[] arr) => Array.IndexOf(arr, arr.Max());
```

---

### Q88. Count Elements Greater Than Average

**Problem:** Return how many elements are above the array's average value.

**Example:** `[1, 2, 3, 4, 5]` → `2` (4 and 5 are above avg 3)

```csharp
int CountAboveAverage(int[] arr) {
    double avg = arr.Average();
    return arr.Count(x => x > avg);
}
```

---

### Q89. Find All Elements That Appear Exactly Once

**Problem:** Return elements that have no duplicates.

**Example:** `[1, 2, 2, 3, 4, 4]` → `[1, 3]`

```csharp
int[] UniqueElements(int[] arr) =>
    arr.GroupBy(x => x).Where(g => g.Count() == 1).Select(g => g.Key).ToArray();
```

---

### Q90. Shift All Negative Numbers to the Front

**Problem:** Move negatives to the front, positives to the back (order within each group doesn't matter).

**Example:** `[1, -2, 3, -4, 5]` → `[-2, -4, 1, 3, 5]`

```csharp
int[] NegativesFirst(int[] arr) =>
    arr.Where(x => x < 0).Concat(arr.Where(x => x >= 0)).ToArray();
```

---

### Q91. Find the Range of an Array (Max - Min)

**Problem:** Return the difference between the largest and smallest values.

**Example:** `[3, 1, 7, 2, 9]` → `8`

```csharp
int Range(int[] arr) => arr.Max() - arr.Min();
```

---

### Q92. Check if Two Arrays Are Equal

**Problem:** Return true if both arrays have the same elements in the same order.

**Example:** `[1,2,3]`, `[1,2,3]` → `true`

```csharp
bool AreEqual(int[] a, int[] b) => a.SequenceEqual(b);
```

---

### Q93. Zip Two Arrays Together

**Problem:** Combine corresponding elements of two arrays into pairs.

**Example:** `[1,2,3]`, `["a","b","c"]` → `[(1,a),(2,b),(3,c)]`

```csharp
var pairs = nums.Zip(letters, (n, l) => $"({n},{l})").ToList();
```

---

### Q94. Find the Pair with the Maximum Sum

**Problem:** Return the two numbers from the array that produce the highest sum.

**Example:** `[1, 5, 3, 9, 2]` → `(9, 5)` sum = 14

```csharp
(int, int) MaxSumPair(int[] arr) {
    var sorted = arr.OrderByDescending(x => x).ToArray();
    return (sorted[0], sorted[1]);
}
```

---

### Q95. Remove All Elements Below a Threshold

**Problem:** Return only elements greater than or equal to the threshold.

**Example:** `[1, 5, 2, 8, 3]`, threshold=4 → `[5, 8]`

```csharp
int[] FilterBelowThreshold(int[] arr, int threshold) =>
    arr.Where(x => x >= threshold).ToArray();
```

---

### Q96. Find the Cumulative Sum Array

**Problem:** Return an array where each index holds the running total up to that point.

**Example:** `[1, 2, 3, 4]` → `[1, 3, 6, 10]`

```csharp
int[] CumulativeSum(int[] arr) {
    int[] result = new int[arr.Length];
    result[0] = arr[0];
    for (int i = 1; i < arr.Length; i++) result[i] = result[i - 1] + arr[i];
    return result;
}
```

---

### Q97. Transpose a 2D Matrix

**Problem:** Swap rows and columns of a square matrix.

**Example:** `[[1,2],[3,4]]` → `[[1,3],[2,4]]`

```csharp
int[,] Transpose(int[,] m) {
    int n = m.GetLength(0);
    var t = new int[n, n];
    for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++) t[j, i] = m[i, j];
    return t;
}
```

---

### Q98. Find All Numbers in Array Divisible by K

**Problem:** Return all elements divisible by K.

**Example:** `[1,2,3,4,5,6,7,8,9,10]`, K=3 → `[3,6,9]`

```csharp
int[] DivisibleByK(int[] arr, int k) => arr.Where(x => x % k == 0).ToArray();
```

---

### Q99. Swap the First and Last Elements of an Array

**Problem:** Exchange the element at index 0 with the element at the last index.

**Example:** `[1,2,3,4,5]` → `[5,2,3,4,1]`

```csharp
void SwapFirstLast(int[] arr) =>
    (arr[0], arr[arr.Length - 1]) = (arr[arr.Length - 1], arr[0]);
```

---

### Q100. Replace Every Element with the Next Greater Element

**Problem:** For each element, replace it with the next larger element to its right. Use -1 if none exists.

**Example:** `[4, 5, 2, 10, 8]` → `[5, 10, 10, -1, -1]`

```csharp
int[] NextGreaterElement(int[] arr) {
    int n = arr.Length;
    int[] result = Enumerable.Repeat(-1, n).ToArray();
    var stack = new Stack<int>();
    for (int i = 0; i < n; i++) {
        while (stack.Count > 0 && arr[i] > arr[stack.Peek()])
            result[stack.Pop()] = arr[i];
        stack.Push(i);
    }
    return result;
}
```

---

## SECTION 3: NUMBER & MATH QUESTIONS (Q101–Q140)

---

### Q101. Check if a Number is Prime

**Problem:** Return true if the number has no divisors other than 1 and itself.

**Example:** `7` → `true`, `9` → `false`

```csharp
bool IsPrime(int n) {
    if (n < 2) return false;
    for (int i = 2; i <= Math.Sqrt(n); i++)
        if (n % i == 0) return false;
    return true;
}
```

---

### Q102. Find the Factorial of a Number

**Problem:** Return N! (product of all integers from 1 to N).

**Example:** `5` → `120`

```csharp
long Factorial(int n) => n <= 1 ? 1 : n * Factorial(n - 1);

// Iterative
long FactorialLoop(int n) {
    long result = 1;
    for (int i = 2; i <= n; i++) result *= i;
    return result;
}
```

---

### Q103. Find the Fibonacci Number at Position N

**Problem:** Return the Nth number in the Fibonacci sequence (0,1,1,2,3,5,8...).

**Example:** N=6 → `8`

```csharp
int Fibonacci(int n) {
    if (n <= 1) return n;
    int a = 0, b = 1;
    for (int i = 2; i <= n; i++) { int temp = a + b; a = b; b = temp; }
    return b;
}
```

---

### Q104. Check if a Number is Armstrong

**Problem:** A number is Armstrong if the sum of its digits each raised to the power of the digit count equals the number.

**Example:** `153` = 1³+5³+3³ = 153 → `true`

```csharp
bool IsArmstrong(int n) {
    string s = n.ToString();
    int power = s.Length;
    return s.Sum(c => (int)Math.Pow(c - '0', power)) == n;
}
```

---

### Q105. Reverse a Number

**Problem:** Reverse the digits of an integer.

**Example:** `12345` → `54321`, `-123` → `-321`

```csharp
int ReverseNumber(int n) {
    bool neg = n < 0;
    string s = new string(Math.Abs(n).ToString().Reverse().ToArray());
    return neg ? -int.Parse(s) : int.Parse(s);
}
```

---

### Q106. Check if a Number is a Palindrome

**Problem:** Return true if the number reads the same forwards and backwards.

**Example:** `121` → `true`, `123` → `false`

```csharp
bool IsNumberPalindrome(int n) {
    if (n < 0) return false;
    string s = n.ToString();
    return s.SequenceEqual(s.Reverse());
}
```

---

### Q107. Find GCD (Greatest Common Divisor) of Two Numbers

**Problem:** Return the largest number that divides both.

**Example:** `12, 18` → `6`

```csharp
int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);
```

---

### Q108. Find LCM (Least Common Multiple) of Two Numbers

**Problem:** Return the smallest number divisible by both.

**Example:** `4, 6` → `12`

```csharp
int LCM(int a, int b) => a / GCD(a, b) * b;
int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);
```

---

### Q109. Find the Sum of Digits of a Number

**Problem:** Add up all the individual digits.

**Example:** `12345` → `15`

```csharp
int SumOfDigits(int n) => Math.Abs(n).ToString().Sum(c => c - '0');
```

---

### Q110. Check if a Number is Even or Odd Without Using %

**Problem:** Check even/odd using bitwise AND.

**Example:** `4` → Even, `7` → Odd

```csharp
string EvenOrOdd(int n) => (n & 1) == 0 ? "Even" : "Odd";
```

---

### Q111. Print the Multiplication Table of a Number

**Problem:** Print the table up to 10.

**Example:** N=5 → `5x1=5, 5x2=10, ... 5x10=50`

```csharp
void MultiplicationTable(int n) {
    for (int i = 1; i <= 10; i++)
        Console.WriteLine($"{n} x {i} = {n * i}");
}
```

---

### Q112. Count the Number of Digits in an Integer

**Problem:** Return how many digits the number has.

**Example:** `12345` → `5`

```csharp
int CountDigits(int n) => Math.Abs(n).ToString().Length;
```

---

### Q113. Find All Prime Numbers up to N (Sieve of Eratosthenes)

**Problem:** Print all prime numbers from 2 to N.

**Example:** N=10 → `2, 3, 5, 7`

```csharp
void PrintPrimesUpTo(int n) {
    bool[] isComposite = new bool[n + 1];
    for (int i = 2; i * i <= n; i++)
        if (!isComposite[i])
            for (int j = i * i; j <= n; j += i) isComposite[j] = true;
    for (int i = 2; i <= n; i++)
        if (!isComposite[i]) Console.Write(i + " ");
}
```

---

### Q114. Power of a Number (x raised to n)

**Problem:** Calculate x to the power of n without using `Math.Pow`.

**Example:** `2^10` → `1024`

```csharp
double Power(double x, int n) {
    if (n == 0) return 1;
    if (n < 0) { x = 1 / x; n = -n; }
    double result = 1;
    while (n > 0) {
        if (n % 2 == 1) result *= x;
        x *= x;
        n /= 2;
    }
    return result;
}
```

---

### Q115. Convert Decimal to Binary

**Problem:** Convert a decimal integer to its binary string representation.

**Example:** `10` → `"1010"`

```csharp
string DecimalToBinary(int n) => Convert.ToString(n, 2);

// Manual
string DecimalToBinaryManual(int n) {
    if (n == 0) return "0";
    var bits = new Stack<int>();
    while (n > 0) { bits.Push(n % 2); n /= 2; }
    return string.Join("", bits);
}
```

---

### Q116. Find All Factors (Divisors) of a Number

**Problem:** Return all numbers that divide evenly into N.

**Example:** `12` → `[1, 2, 3, 4, 6, 12]`

```csharp
List<int> Factors(int n) =>
    Enumerable.Range(1, n).Where(i => n % i == 0).ToList();
```

---

### Q117. Check if a Number is a Perfect Square

**Problem:** Return true if the square root of N is a whole number.

**Example:** `16` → `true`, `15` → `false`

```csharp
bool IsPerfectSquare(int n) {
    int root = (int)Math.Sqrt(n);
    return root * root == n;
}
```

---

### Q118. Find the Nth Triangular Number

**Problem:** The Nth triangular number is the sum of 1 to N.

**Example:** N=5 → `15` (1+2+3+4+5)

```csharp
int TriangularNumber(int n) => n * (n + 1) / 2;
```

---

### Q119. Convert Binary to Decimal

**Problem:** Convert a binary string to its decimal value.

**Example:** `"1010"` → `10`

```csharp
int BinaryToDecimal(string binary) => Convert.ToInt32(binary, 2);

// Manual
int BinaryToDecimalManual(string binary) {
    int result = 0;
    for (int i = 0; i < binary.Length; i++)
        result = result * 2 + (binary[i] - '0');
    return result;
}
```

---

### Q120. Check if a Number is a Power of 2

**Problem:** Return true if N is a power of 2 (1, 2, 4, 8, 16...).

**Example:** `16` → `true`, `18` → `false`

```csharp
bool IsPowerOfTwo(int n) => n > 0 && (n & (n - 1)) == 0;
```

---

### Q121. Find the Absolute Difference Between Two Numbers

**Problem:** Return |a - b|.

**Example:** `a=10, b=15` → `5`

```csharp
int AbsDiff(int a, int b) => Math.Abs(a - b);
```

---

### Q122. Print a Number in Words (Units to Thousands)

**Problem:** Convert numbers 1-19 to their English word.

**Example:** `7` → `"Seven"`, `13` → `"Thirteen"`

```csharp
string NumberToWord(int n) {
    string[] words = { "", "One","Two","Three","Four","Five","Six","Seven",
        "Eight","Nine","Ten","Eleven","Twelve","Thirteen","Fourteen","Fifteen",
        "Sixteen","Seventeen","Eighteen","Nineteen" };
    return n >= 1 && n < 20 ? words[n] : "Out of range";
}
```

---

### Q123. Check if a Year is a Leap Year

**Problem:** A year is a leap year if divisible by 4, except centuries which must be divisible by 400.

**Example:** `2024` → `true`, `1900` → `false`, `2000` → `true`

```csharp
bool IsLeapYear(int year) =>
    (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
```

---

### Q124. Find the Sum of All Even Numbers Between 1 and N

**Problem:** Sum all even numbers from 1 to N inclusive.

**Example:** N=10 → `30` (2+4+6+8+10)

```csharp
int SumOfEvens(int n) => Enumerable.Range(1, n).Where(x => x % 2 == 0).Sum();
```

---

### Q125. Find the Largest of Three Numbers

**Problem:** Return the maximum among three integers.

**Example:** `3, 7, 5` → `7`

```csharp
int MaxOfThree(int a, int b, int c) => Math.Max(a, Math.Max(b, c));
```

---

### Q126. Check if a Number is Positive, Negative, or Zero

**Problem:** Return a string describing the sign.

**Example:** `5` → `"Positive"`, `-3` → `"Negative"`, `0` → `"Zero"`

```csharp
string Sign(int n) => n > 0 ? "Positive" : n < 0 ? "Negative" : "Zero";
```

---

### Q127. Calculate Simple Interest

**Problem:** SI = (P × R × T) / 100.

**Example:** P=1000, R=5, T=2 → `100`

```csharp
double SimpleInterest(double p, double r, double t) => (p * r * t) / 100;
```

---

### Q128. Calculate Compound Interest

**Problem:** CI = P × (1 + R/100)^T - P.

**Example:** P=1000, R=5, T=2 → `102.5`

```csharp
double CompoundInterest(double p, double r, double t) =>
    p * Math.Pow(1 + r / 100, t) - p;
```

---

### Q129. Find the Number of Trailing Zeros in N Factorial

**Problem:** Count zeros at the end of N!. Each zero comes from a factor of 10 = 2×5. Count 5s.

**Example:** N=25 → `6`

```csharp
int TrailingZeros(int n) {
    int count = 0;
    while (n >= 5) { n /= 5; count += n; }
    return count;
}
```

---

### Q130. Check if a Number is a Kaprekar Number

**Problem:** A number is Kaprekar if squaring it and splitting the result sums back to the original.

**Example:** `9`: 9²=81, 8+1=9 → `true`. `45`: 45²=2025, 20+25=45 → `true`

```csharp
bool IsKaprekar(int n) {
    long sq = (long)n * n;
    string s = sq.ToString();
    for (int i = 1; i < s.Length; i++) {
        int left = int.Parse(s.Substring(0, i));
        int right = int.Parse(s.Substring(i));
        if (left + right == n) return true;
    }
    return false;
}
```

---

### Q131. Print All Prime Numbers in a Range

**Problem:** Print all prime numbers between A and B (inclusive).

**Example:** A=10, B=20 → `11, 13, 17, 19`

```csharp
void PrimesInRange(int a, int b) {
    bool IsPrime(int n) {
        if (n < 2) return false;
        for (int i = 2; i <= Math.Sqrt(n); i++) if (n % i == 0) return false;
        return true;
    }
    var primes = Enumerable.Range(a, b - a + 1).Where(IsPrime);
    Console.WriteLine(string.Join(", ", primes));
}
```

---

### Q132. Find if a Number Can Be Expressed as Sum of Two Primes

**Problem:** Return true if the number can be written as the sum of exactly two prime numbers.

**Example:** `10` → true (3+7 or 5+5)

```csharp
bool SumOfTwoPrimes(int n) {
    bool IsPrime(int x) {
        if (x < 2) return false;
        for (int i = 2; i <= Math.Sqrt(x); i++) if (x % i == 0) return false;
        return true;
    }
    for (int i = 2; i <= n / 2; i++)
        if (IsPrime(i) && IsPrime(n - i)) return true;
    return false;
}
```

---

### Q133. Check if a Number is Perfect

**Problem:** A perfect number equals the sum of its proper divisors.

**Example:** `28` → divisors: 1+2+4+7+14=28 → `true`

```csharp
bool IsPerfect(int n) {
    if (n < 2) return false;
    int sum = Enumerable.Range(1, n / 2).Where(i => n % i == 0).Sum();
    return sum == n;
}
```

---

### Q134. Find the N-th Term of an Arithmetic Sequence

**Problem:** Given first term `a`, common difference `d`, find the N-th term.

**Example:** a=2, d=3, N=5 → `14`

```csharp
int NthArithmeticTerm(int a, int d, int n) => a + (n - 1) * d;
```

---

### Q135. Find the Number of Ways to Climb N Stairs (1 or 2 steps)

**Problem:** You can take 1 or 2 steps. How many distinct ways are there to reach step N?

**Example:** N=4 → `5` (1+1+1+1, 1+1+2, 1+2+1, 2+1+1, 2+2)

```csharp
int ClimbStairs(int n) {
    if (n <= 2) return n;
    int a = 1, b = 2;
    for (int i = 3; i <= n; i++) { int temp = a + b; a = b; b = temp; }
    return b;
}
```

---

### Q136. Calculate Simple Interest

**Problem:** SI = (P × R × T) / 100.

> See Q127.

---

### Q137. Find All Numbers Divisible by 3 and 5 in a Range

**Problem:** Print all numbers from 1 to N divisible by both 3 and 5.

**Example:** N=30 → `15, 30`

```csharp
void DivisibleBy3And5(int n) {
    var result = Enumerable.Range(1, n).Where(x => x % 3 == 0 && x % 5 == 0);
    Console.WriteLine(string.Join(", ", result));
}
```

---

### Q138. Swap Two Numbers Without a Temporary Variable

**Problem:** Swap two integers without using a third variable.

**Example:** `a=5, b=10` → `a=10, b=5`

```csharp
void Swap(ref int a, ref int b) {
    a = a + b;
    b = a - b;
    a = a - b;
    // Or with XOR: a ^= b; b ^= a; a ^= b;
}
```

---

### Q139. Generate Pascal's Triangle up to N Rows

**Problem:** Print Pascal's triangle.

**Example:** N=4:
```
1
1 1
1 2 1
1 3 3 1
```

```csharp
void PascalsTriangle(int n) {
    for (int i = 0; i < n; i++) {
        int val = 1;
        for (int j = 0; j <= i; j++) {
            Console.Write(val + " ");
            val = val * (i - j) / (j + 1);
        }
        Console.WriteLine();
    }
}
```

---

### Q140. Word Frequency Counter

**Problem:** Given a paragraph, count the frequency of each word (case-insensitive), and return the top 3 most frequent words.

**Example:** `"the cat sat on the mat the cat"` → `the:3, cat:2, sat:1`

```csharp
List<(string Word, int Count)> TopNWords(string text, int n) {
    return text.ToLower()
        .Split(new char[]{' ', '.', ',', '!', '?'}, StringSplitOptions.RemoveEmptyEntries)
        .GroupBy(w => w)
        .OrderByDescending(g => g.Count())
        .Take(n)
        .Select(g => (g.Key, g.Count()))
        .ToList();
}
```

---

## SECTION 4: PATTERN & LOGIC QUESTIONS (Q141–Q150)

---

### Q141. Print a Right Triangle Star Pattern

**Problem:** Print a right-angled triangle of stars with N rows.

**Example:** N=4:
```
*
* *
* * *
* * * *
```

```csharp
void RightTriangle(int n) {
    for (int i = 1; i <= n; i++)
        Console.WriteLine(string.Join(" ", Enumerable.Repeat("*", i)));
}
```

---

### Q142. Print a Pyramid Star Pattern

**Problem:** Print a centered pyramid of stars.

**Example:** N=4:
```
   *
  ***
 *****
*******
```

```csharp
void Pyramid(int n) {
    for (int i = 1; i <= n; i++) {
        Console.Write(new string(' ', n - i));
        Console.WriteLine(new string('*', 2 * i - 1));
    }
}
```

---

### Q143. Print a Number Pattern (Floyd's Triangle)

**Problem:** Print a triangle filled with consecutive numbers.

**Example:** N=4:
```
1
2 3
4 5 6
7 8 9 10
```

```csharp
void FloydsTriangle(int n) {
    int num = 1;
    for (int i = 1; i <= n; i++) {
        for (int j = 1; j <= i; j++) Console.Write(num++ + " ");
        Console.WriteLine();
    }
}
```

---

### Q144. FizzBuzz (Classic)

**Problem:** Print 1 to N. For multiples of 3 print "Fizz", multiples of 5 print "Buzz", multiples of both print "FizzBuzz".

**Example:** N=15 → `1 2 Fizz 4 Buzz Fizz 7 8 Fizz Buzz 11 Fizz 13 14 FizzBuzz`

```csharp
void FizzBuzz(int n) {
    for (int i = 1; i <= n; i++) {
        if (i % 15 == 0) Console.Write("FizzBuzz ");
        else if (i % 3 == 0) Console.Write("Fizz ");
        else if (i % 5 == 0) Console.Write("Buzz ");
        else Console.Write(i + " ");
    }
}
```

---

### Q145. Find the Longest Substring Without Repeating Characters

**Problem:** Return the length of the longest substring where no character repeats.

**Example:** `"abcabcbb"` → `3` (abc)

```csharp
int LongestUniqueSubstring(string s) {
    var seen = new Dictionary<char, int>();
    int max = 0, start = 0;
    for (int i = 0; i < s.Length; i++) {
        if (seen.ContainsKey(s[i]) && seen[s[i]] >= start)
            start = seen[s[i]] + 1;
        seen[s[i]] = i;
        max = Math.Max(max, i - start + 1);
    }
    return max;
}
```

---

### Q146. Find the Missing Letter in an Alphabetical Sequence

**Problem:** A string contains all letters in order except one. Find the missing letter.

**Example:** `"abcdefghijklmnopqrstuvwxyz".Remove(7, 1)` missing `'h'`

```csharp
char FindMissingLetter(string s) {
    for (int i = 0; i < s.Length - 1; i++)
        if (s[i + 1] - s[i] > 1) return (char)(s[i] + 1);
    return '\0';
}
```

---

### Q147. Find the Longest Chain of Characters

**Problem:** Return the maximum number of times any single character appears consecutively anywhere in the string.

**Example:** `"aabbbccddddee"` → `4` (dddd)

```csharp
int LongestChain(string s) {
    int max = 1, cur = 1;
    for (int i = 1; i < s.Length; i++) {
        cur = s[i] == s[i - 1] ? cur + 1 : 1;
        max = Math.Max(max, cur);
    }
    return max;
}
```

---

### Q148. Calculate the Hamming Distance Between Two Strings

**Problem:** Count positions where the characters differ (same length strings).

**Example:** `"karolin"`, `"kathrin"` → `3`

```csharp
int HammingDistance(string a, string b) =>
    a.Zip(b, (x, y) => x != y).Count(diff => diff);
```

---

### Q149. Find the First Repeated Word in a Sentence

**Problem:** Return the first word that appears more than once.

**Example:** `"the cat sat on the mat"` → `"the"`

```csharp
string FirstRepeatedWord(string s) {
    var seen = new HashSet<string>();
    foreach (var word in s.ToLower().Split(' '))
        if (!seen.Add(word)) return word;
    return "";
}
```

---

### Q150. Validate a Password Strength

**Problem:** A strong password must be 8+ characters, contain at least one uppercase, one lowercase, one digit, and one special character.

**Example:** `"Pass@123"` → `true`, `"password"` → `false`

```csharp
bool IsStrongPassword(string p) =>
    p.Length >= 8 &&
    p.Any(char.IsUpper) &&
    p.Any(char.IsLower) &&
    p.Any(char.IsDigit) &&
    p.Any(c => "!@#$%^&*".Contains(c));
```

---

## SECTION 5: COLLECTIONS & LINQ (Q151–Q165)

---

### Q151. Find the Top 3 Highest Salaries from a List

**Problem:** Given a list of employee objects, return the top 3 distinct salaries.

```csharp
var top3 = employees
    .Select(e => e.Salary)
    .Distinct()
    .OrderByDescending(s => s)
    .Take(3)
    .ToList();
```

---

### Q152. Group Employees by Department

**Problem:** Group a list of employees by their department name.

```csharp
var grouped = employees
    .GroupBy(e => e.Department)
    .Select(g => new { Dept = g.Key, Count = g.Count(), Employees = g.ToList() });
```

---

### Q153. Find Employees Whose Name Starts with a Letter

**Problem:** Filter employees whose name starts with 'A'.

```csharp
var result = employees
    .Where(e => e.Name.StartsWith("A", StringComparison.OrdinalIgnoreCase))
    .OrderBy(e => e.Name)
    .ToList();
```

---

### Q154. Calculate Total Salary by Department

**Problem:** Return the sum of salaries for each department.

```csharp
var deptTotals = employees
    .GroupBy(e => e.Department)
    .Select(g => new { Department = g.Key, TotalSalary = g.Sum(e => e.Salary) })
    .OrderByDescending(x => x.TotalSalary);
```

---

### Q155. Find Duplicates in a List of Strings

**Problem:** Return all strings that appear more than once.

```csharp
var duplicates = list
    .GroupBy(s => s)
    .Where(g => g.Count() > 1)
    .Select(g => g.Key)
    .ToList();
```

---

### Q156. Convert a List of Strings to Uppercase

**Problem:** Return a new list with all strings in uppercase.

```csharp
var upper = list.Select(s => s.ToUpper()).ToList();
```

---

### Q157. Find the Average Age of Employees Over 30

**Problem:** Filter employees older than 30 and calculate their average age.

```csharp
double avgAge = employees
    .Where(e => e.Age > 30)
    .Average(e => e.Age);
```

---

### Q158. Check if Any Employee Earns More Than 100,000

**Problem:** Return true if at least one employee has a salary above 100k.

```csharp
bool highEarner = employees.Any(e => e.Salary > 100_000);
```

---

### Q159. Remove Null and Empty Strings from a List

**Problem:** Filter out all null, empty, or whitespace strings.

```csharp
var clean = list.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
```

---

### Q160. Sort a Dictionary by Value

**Problem:** Given a `Dictionary<string, int>`, sort it by value descending.

```csharp
var sorted = dict.OrderByDescending(kv => kv.Value)
                 .ToDictionary(kv => kv.Key, kv => kv.Value);
```

---

### Q161. Group Words by Their Length

**Problem:** Group a list of words into a dictionary where the key is the word length.

**Example:** `["cat", "dog", "elephant", "ant"]` → `{3: [cat, dog, ant], 8: [elephant]}`

```csharp
Dictionary<int, List<string>> GroupByLength(string[] words) =>
    words.GroupBy(w => w.Length)
         .ToDictionary(g => g.Key, g => g.ToList());
```

---

### Q162. Flatten Nested Strings to a Flat List

**Problem:** Given a `List<List<string>>`, flatten it into a single `List<string>`.

**Example:** `[["a","b"],["c"],["d","e"]]` → `["a","b","c","d","e"]`

```csharp
List<string> Flatten(List<List<string>> nested) => nested.SelectMany(x => x).ToList();
```

---

### Q163. Parse a CSV Line into a List of Values

**Problem:** Split a CSV-formatted string into its fields, handling quoted fields.

**Example:** `"John,25,\"New York\",Engineer"` → `["John", "25", "New York", "Engineer"]`

```csharp
List<string> ParseCsvLine(string line) {
    var result = new List<string>();
    bool inQuotes = false;
    var current = new StringBuilder();
    foreach (char c in line) {
        if (c == '"') { inQuotes = !inQuotes; continue; }
        if (c == ',' && !inQuotes) { result.Add(current.ToString()); current.Clear(); continue; }
        current.Append(c);
    }
    result.Add(current.ToString());
    return result;
}
```

---

### Q164. Demonstrate LINQ with a Custom Class

**Problem:** Use LINQ to filter, sort, and project a list of `Product` objects.

```csharp
class Product { public string Name = ""; public decimal Price; public string Category = ""; }

var products = new List<Product> {
    new() { Name = "Laptop", Price = 999, Category = "Electronics" },
    new() { Name = "Phone", Price = 499, Category = "Electronics" },
    new() { Name = "Desk", Price = 299, Category = "Furniture" }
};

var result = products
    .Where(p => p.Price > 300)
    .OrderBy(p => p.Price)
    .Select(p => new { p.Name, p.Price });
```

---

### Q165. Rotate a Matrix 90 Degrees Clockwise

**Problem:** Rotate an N×N matrix in-place by 90 degrees clockwise.

**Example:** `[[1,2],[3,4]]` → `[[3,1],[4,2]]`

```csharp
void RotateMatrix(int[,] m) {
    int n = m.GetLength(0);
    // Transpose
    for (int i = 0; i < n; i++)
        for (int j = i + 1; j < n; j++)
            (m[i, j], m[j, i]) = (m[j, i], m[i, j]);
    // Reverse each row
    for (int i = 0; i < n; i++)
        for (int j = 0; j < n / 2; j++)
            (m[i, j], m[i, n - 1 - j]) = (m[i, n - 1 - j], m[i, j]);
}
```

---

## SECTION 6: DATA STRUCTURES (Q166–Q185)

---

### Q166. Implement a Basic Stack Using an Array

**Problem:** Implement push, pop, peek, and isEmpty without using the built-in Stack class.

```csharp
class MyStack {
    private int[] data = new int[100];
    private int top = -1;

    public void Push(int val) => data[++top] = val;
    public int Pop() => top < 0 ? throw new Exception("Empty") : data[top--];
    public int Peek() => top < 0 ? throw new Exception("Empty") : data[top];
    public bool IsEmpty() => top == -1;
}
```

---

### Q167. Implement a Queue Using Two Stacks

**Problem:** Build a Queue (FIFO) using only Stack (LIFO) operations.

```csharp
class MyQueue {
    private Stack<int> inbox = new(), outbox = new();

    public void Enqueue(int val) => inbox.Push(val);

    public int Dequeue() {
        if (outbox.Count == 0)
            while (inbox.Count > 0) outbox.Push(inbox.Pop());
        return outbox.Pop();
    }

    public int Peek() {
        if (outbox.Count == 0)
            while (inbox.Count > 0) outbox.Push(inbox.Pop());
        return outbox.Peek();
    }
}
```

---

### Q168. Implement a Min Stack (Stack with getMin in O(1))

**Problem:** Design a stack that supports `push`, `pop`, `peek`, and `getMin` all in O(1).

```csharp
class MinStack {
    private Stack<int> stack = new();
    private Stack<int> minStack = new();

    public void Push(int val) {
        stack.Push(val);
        if (minStack.Count == 0 || val <= minStack.Peek()) minStack.Push(val);
    }

    public void Pop() {
        if (stack.Pop() == minStack.Peek()) minStack.Pop();
    }

    public int GetMin() => minStack.Peek();
    public int Peek() => stack.Peek();
}
```

---

### Q169. Reverse a Linked List

**Problem:** Reverse a singly linked list iteratively.

```csharp
class Node { public int Val; public Node? Next; }

Node? ReverseList(Node? head) {
    Node? prev = null, curr = head;
    while (curr != null) {
        Node? next = curr.Next;
        curr.Next = prev;
        prev = curr;
        curr = next;
    }
    return prev;
}
```

---

### Q170. Detect a Cycle in a Linked List

**Problem:** Return true if the linked list has a cycle (loop).

```csharp
bool HasCycle(Node head) {
    var slow = head;
    var fast = head;
    while (fast?.Next != null) {
        slow = slow!.Next!;
        fast = fast.Next.Next;
        if (slow == fast) return true;
    }
    return false;
}
```

---

### Q171. Find the Middle Node of a Linked List

**Problem:** Return the middle node. If even length, return the second middle.

```csharp
Node? FindMiddle(Node head) {
    var slow = head;
    var fast = head;
    while (fast?.Next != null) { slow = slow!.Next!; fast = fast.Next.Next; }
    return slow;
}
```

---

### Q172. Flatten a Linked List (Convert to Array and Back)

**Problem:** Convert linked list values to an array, and convert an array back to a linked list.

```csharp
// To array
int[] ToArray(Node? head) {
    var list = new List<int>();
    while (head != null) { list.Add(head.Val); head = head.Next; }
    return list.ToArray();
}

// From array
Node? FromArray(int[] arr) {
    if (arr.Length == 0) return null;
    var head = new Node { Val = arr[0] };
    var cur = head;
    for (int i = 1; i < arr.Length; i++) { cur.Next = new Node { Val = arr[i] }; cur = cur.Next; }
    return head;
}
```

---

### Q173. Implement a Simple HashMap Without Built-in Dictionary

**Problem:** Build a key-value store using an array of buckets (basic hash map).

```csharp
class SimpleHashMap {
    private List<(int key, int val)>[] buckets = new List<(int, int)>[1000];

    private int Hash(int key) => Math.Abs(key % 1000);

    public void Put(int key, int val) {
        int h = Hash(key);
        buckets[h] ??= new();
        var entry = buckets[h].FindIndex(e => e.key == key);
        if (entry >= 0) buckets[h][entry] = (key, val);
        else buckets[h].Add((key, val));
    }

    public int Get(int key) {
        int h = Hash(key);
        return buckets[h]?.FirstOrDefault(e => e.key == key).val ?? -1;
    }
}
```

---

### Q174. Check if a Binary Tree is Balanced

**Problem:** A balanced tree has no subtree whose left and right height differ by more than 1.

```csharp
class TreeNode { public int Val; public TreeNode? Left, Right; }

bool IsBalanced(TreeNode? root) {
    int Height(TreeNode? node) {
        if (node == null) return 0;
        int l = Height(node.Left), r = Height(node.Right);
        if (l == -1 || r == -1 || Math.Abs(l - r) > 1) return -1;
        return Math.Max(l, r) + 1;
    }
    return Height(root) != -1;
}
```

---

### Q175. Level-Order Traversal of a Binary Tree (BFS)

**Problem:** Print nodes level by level, left to right.

**Example:** Tree with root 1, left 2, right 3 → `[[1],[2,3]]`

```csharp
List<List<int>> LevelOrder(TreeNode? root) {
    var result = new List<List<int>>();
    if (root == null) return result;
    var queue = new Queue<TreeNode>();
    queue.Enqueue(root);
    while (queue.Count > 0) {
        var level = new List<int>();
        int size = queue.Count;
        for (int i = 0; i < size; i++) {
            var node = queue.Dequeue();
            level.Add(node.Val);
            if (node.Left != null) queue.Enqueue(node.Left);
            if (node.Right != null) queue.Enqueue(node.Right);
        }
        result.Add(level);
    }
    return result;
}
```

---

### Q176. Find the Maximum Depth of a Binary Tree

**Problem:** Return the number of nodes on the longest path from root to leaf.

**Example:** Tree of height 3 → `3`

```csharp
int MaxDepth(TreeNode? root) =>
    root == null ? 0 : 1 + Math.Max(MaxDepth(root.Left), MaxDepth(root.Right));
```

---

### Q177. Check if a Binary Search Tree is Valid

**Problem:** Return true if the tree satisfies the BST property (left < root < right at every node).

```csharp
bool IsValidBST(TreeNode? node, long min = long.MinValue, long max = long.MaxValue) {
    if (node == null) return true;
    if (node.Val <= min || node.Val >= max) return false;
    return IsValidBST(node.Left, min, node.Val) && IsValidBST(node.Right, node.Val, max);
}
```

---

### Q178. Binary Search on a Sorted Array

**Problem:** Return the index of the target in a sorted array, or -1 if not found.

**Example:** `[1,3,5,7,9,11]`, target=7 → `3`

```csharp
int BinarySearch(int[] arr, int target) {
    int l = 0, r = arr.Length - 1;
    while (l <= r) {
        int mid = l + (r - l) / 2;
        if (arr[mid] == target) return mid;
        if (arr[mid] < target) l = mid + 1;
        else r = mid - 1;
    }
    return -1;
}
```

---

### Q179. Implement Bubble Sort

**Problem:** Sort an array using the bubble sort algorithm.

**Example:** `[5,3,1,4,2]` → `[1,2,3,4,5]`

```csharp
void BubbleSort(int[] arr) {
    for (int i = 0; i < arr.Length - 1; i++)
        for (int j = 0; j < arr.Length - 1 - i; j++)
            if (arr[j] > arr[j + 1]) (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
}
```

---

### Q180. Implement Selection Sort

**Problem:** Sort by finding the minimum each time and placing it at the front.

```csharp
void SelectionSort(int[] arr) {
    for (int i = 0; i < arr.Length - 1; i++) {
        int minIdx = i;
        for (int j = i + 1; j < arr.Length; j++)
            if (arr[j] < arr[minIdx]) minIdx = j;
        (arr[i], arr[minIdx]) = (arr[minIdx], arr[i]);
    }
}
```

---

### Q181. Implement Insertion Sort

**Problem:** Build the sorted list one element at a time by inserting into the correct position.

```csharp
void InsertionSort(int[] arr) {
    for (int i = 1; i < arr.Length; i++) {
        int key = arr[i], j = i - 1;
        while (j >= 0 && arr[j] > key) { arr[j + 1] = arr[j]; j--; }
        arr[j + 1] = key;
    }
}
```

---

### Q182. Count Islands in a Grid (DFS)

**Problem:** A grid of '1' (land) and '0' (water). Count the number of islands (connected land regions).

**Example:** Grid with two separate land masses → `2`

```csharp
int CountIslands(char[,] grid) {
    int rows = grid.GetLength(0), cols = grid.GetLength(1), count = 0;
    void DFS(int r, int c) {
        if (r < 0 || r >= rows || c < 0 || c >= cols || grid[r, c] != '1') return;
        grid[r, c] = '0';
        DFS(r+1, c); DFS(r-1, c); DFS(r, c+1); DFS(r, c-1);
    }
    for (int i = 0; i < rows; i++)
        for (int j = 0; j < cols; j++)
            if (grid[i, j] == '1') { DFS(i, j); count++; }
    return count;
}
```

---

### Q183. Generate All Subsets of an Array

**Problem:** Return all possible subsets (the power set) of an array.

**Example:** `[1,2,3]` → `[[], [1], [2], [3], [1,2], [1,3], [2,3], [1,2,3]]`

```csharp
List<List<int>> Subsets(int[] nums) {
    var result = new List<List<int>> { new() };
    foreach (int n in nums) {
        var newSubsets = result.Select(s => new List<int>(s) { n }).ToList();
        result.AddRange(newSubsets);
    }
    return result;
}
```

---

### Q184. Decode a Run-Length Encoded String

**Problem:** Expand a run-length encoded string back to its original form.

**Example:** `"a3b2c4"` → `"aaabbcccc"`

```csharp
string Decode(string s) {
    var sb = new StringBuilder();
    int i = 0;
    while (i < s.Length) {
        char c = s[i++];
        var numStr = new StringBuilder();
        while (i < s.Length && char.IsDigit(s[i])) numStr.Append(s[i++]);
        int count = numStr.Length > 0 ? int.Parse(numStr.ToString()) : 1;
        sb.Append(new string(c, count));
    }
    return sb.ToString();
}
```

---

### Q185. Implement a Simple LRU Cache (Least Recently Used)

**Problem:** Design a cache with a fixed capacity. When full, evict the least recently used item.

```csharp
class LRUCache {
    private int _capacity;
    private Dictionary<int, LinkedListNode<(int key, int val)>> _map = new();
    private LinkedList<(int key, int val)> _list = new();

    public LRUCache(int capacity) => _capacity = capacity;

    public int Get(int key) {
        if (!_map.ContainsKey(key)) return -1;
        var node = _map[key];
        _list.Remove(node);
        _list.AddFirst(node);
        return node.Value.val;
    }

    public void Put(int key, int val) {
        if (_map.ContainsKey(key)) _list.Remove(_map[key]);
        else if (_map.Count == _capacity) {
            _map.Remove(_list.Last!.Value.key);
            _list.RemoveLast();
        }
        _map[key] = _list.AddFirst((key, val));
    }
}
```

---

## SECTION 7: OOP & C# SPECIFIC (Q186–Q200)

---

### Q186. Implement a Generic Swap Method

**Problem:** Write a generic method that swaps two values of any type.

```csharp
void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);

// Usage
int x = 5, y = 10;
Swap(ref x, ref y); // x=10, y=5

string s1 = "hello", s2 = "world";
Swap(ref s1, ref s2);
```

---

### Q187. Implement the Singleton Pattern

**Problem:** Ensure a class has only one instance throughout the application.

```csharp
public sealed class AppConfig {
    private static AppConfig? _instance;
    private static readonly object _lock = new();

    private AppConfig() { }

    public static AppConfig Instance {
        get {
            if (_instance == null)
                lock (_lock) { _instance ??= new AppConfig(); }
            return _instance;
        }
    }

    public string ConnectionString { get; set; } = "";
}
```

---

### Q188. Implement the Factory Pattern

**Problem:** Create objects without specifying the exact class to instantiate.

```csharp
interface IShape { double Area(); }
class Circle : IShape { double r; public Circle(double r) => this.r = r; public double Area() => Math.PI * r * r; }
class Square : IShape { double s; public Square(double s) => this.s = s; public double Area() => s * s; }

static IShape CreateShape(string type, double size) => type switch {
    "circle" => new Circle(size),
    "square" => new Square(size),
    _ => throw new ArgumentException("Unknown shape")
};
```

---

### Q189. Implement the Observer Pattern

**Problem:** Allow multiple objects to subscribe to and receive notifications from a subject.

```csharp
interface IObserver { void Update(string message); }

class EventPublisher {
    private List<IObserver> _subscribers = new();
    public void Subscribe(IObserver o) => _subscribers.Add(o);
    public void Notify(string msg) => _subscribers.ForEach(o => o.Update(msg));
}

class Logger : IObserver {
    public void Update(string message) => Console.WriteLine($"LOG: {message}");
}
```

---

### Q190. Demonstrate Method Overloading

**Problem:** Write a class with three `Add` methods — for int, double, and string concatenation.

```csharp
class Calculator {
    public int Add(int a, int b) => a + b;
    public double Add(double a, double b) => a + b;
    public string Add(string a, string b) => a + b;
}
```

---

### Q191. Demonstrate Method Overriding with Virtual and Override

**Problem:** Show how a derived class can override a base class method.

```csharp
class Animal {
    public virtual string Speak() => "...";
}

class Dog : Animal {
    public override string Speak() => "Woof!";
}

class Cat : Animal {
    public override string Speak() => "Meow!";
}

// Usage
Animal a = new Dog();
Console.WriteLine(a.Speak()); // "Woof!" — runtime polymorphism
```

---

### Q192. Implement an Interface with Multiple Classes

**Problem:** Define an `IPayment` interface and implement it with two different classes.

```csharp
interface IPayment {
    bool ProcessPayment(decimal amount);
    string GetMethod();
}

class CreditCard : IPayment {
    public bool ProcessPayment(decimal amount) { Console.WriteLine($"Credit: {amount}"); return true; }
    public string GetMethod() => "Credit Card";
}

class PayPal : IPayment {
    public bool ProcessPayment(decimal amount) { Console.WriteLine($"PayPal: {amount}"); return true; }
    public string GetMethod() => "PayPal";
}
```

---

### Q193. Demonstrate Abstract Class vs Interface

**Problem:** Show when to use abstract class (shared state/behavior) vs interface (contract only).

```csharp
// Abstract class — has shared logic and state
abstract class Vehicle {
    public string Brand { get; set; } = "";
    public abstract int GetSpeed(); // must override
    public string Describe() => $"{Brand} goes {GetSpeed()} km/h"; // shared logic
}

// Interface — pure contract, no state
interface IFuelable {
    void Refuel(int liters);
    int FuelLevel { get; }
}

class Car : Vehicle, IFuelable {
    private int _fuel = 0;
    public override int GetSpeed() => 120;
    public void Refuel(int liters) => _fuel += liters;
    public int FuelLevel => _fuel;
}
```

---

### Q194. Write a Generic Stack Class

**Problem:** Build a type-safe Stack from scratch using generics.

```csharp
class Stack<T> {
    private List<T> _items = new();

    public void Push(T item) => _items.Add(item);

    public T Pop() {
        if (_items.Count == 0) throw new InvalidOperationException("Stack is empty");
        T item = _items[^1];
        _items.RemoveAt(_items.Count - 1);
        return item;
    }

    public T Peek() => _items.Count == 0
        ? throw new InvalidOperationException("Stack is empty")
        : _items[^1];

    public bool IsEmpty => _items.Count == 0;
    public int Count => _items.Count;
}
```

---

### Q195. Implement IComparable to Sort Custom Objects

**Problem:** Allow a list of `Employee` objects to be sorted by salary using `List.Sort()`.

```csharp
class Employee : IComparable<Employee> {
    public string Name { get; set; } = "";
    public int Salary { get; set; }

    public int CompareTo(Employee? other) => Salary.CompareTo(other?.Salary);
}

// Usage
var employees = new List<Employee> {
    new() { Name = "Alice", Salary = 80000 },
    new() { Name = "Bob", Salary = 60000 }
};
employees.Sort(); // sorts by salary ascending
```

---

### Q196. Use Extension Methods to Add Functionality to String

**Problem:** Add an `IsNullOrEmpty` and `ToTitleCase` extension method to `string`.

```csharp
static class StringExtensions {
    public static bool IsNullOrEmpty(this string? s) => string.IsNullOrEmpty(s);

    public static string ToTitleCase(this string s) =>
        string.Join(" ", s.Split(' ')
            .Select(w => w.Length > 0
                ? char.ToUpper(w[0]) + w.Substring(1).ToLower()
                : w));
}

// Usage
"hello world".ToTitleCase(); // "Hello World"
```

---

### Q197. Demonstrate `async/await` with Task

**Problem:** Write an async method that simulates fetching data with a delay.

```csharp
async Task<string> FetchDataAsync(int id) {
    await Task.Delay(1000); // simulate network call
    return $"Data for id {id}";
}

async Task RunAsync() {
    Console.WriteLine("Fetching...");
    string data = await FetchDataAsync(42);
    Console.WriteLine(data);
}
```

---

### Q198. Write a Custom Exception Class

**Problem:** Create a domain-specific exception with a custom message and error code.

```csharp
public class ValidationException : Exception {
    public int ErrorCode { get; }

    public ValidationException(string message, int errorCode)
        : base(message) {
        ErrorCode = errorCode;
    }
}

// Usage
throw new ValidationException("Email is required", 1001);

try { /* ... */ }
catch (ValidationException ex) {
    Console.WriteLine($"[{ex.ErrorCode}] {ex.Message}");
}
```

---

### Q199. Implement the Builder Pattern

**Problem:** Build a complex `Request` object step by step using a fluent builder.

```csharp
class HttpRequest {
    public string Url { get; set; } = "";
    public string Method { get; set; } = "GET";
    public Dictionary<string, string> Headers { get; set; } = new();
    public string? Body { get; set; }
}

class HttpRequestBuilder {
    private HttpRequest _req = new();

    public HttpRequestBuilder WithUrl(string url) { _req.Url = url; return this; }
    public HttpRequestBuilder WithMethod(string method) { _req.Method = method; return this; }
    public HttpRequestBuilder WithHeader(string key, string val) { _req.Headers[key] = val; return this; }
    public HttpRequestBuilder WithBody(string body) { _req.Body = body; return this; }
    public HttpRequest Build() => _req;
}

// Usage
var request = new HttpRequestBuilder()
    .WithUrl("https://api.example.com/data")
    .WithMethod("POST")
    .WithHeader("Authorization", "Bearer token")
    .WithBody("{\"key\":\"value\"}")
    .Build();
```

---

### Q200. Design a Vending Machine (State Machine)

**Problem:** Implement a simple vending machine that accepts coins, lets users select items, and dispenses items or returns change.

```csharp
class VendingMachine {
    private decimal _balance = 0;
    private Dictionary<string, (decimal price, int qty)> _items = new() {
        { "Water", (1.00m, 10) },
        { "Chips", (1.50m, 5) },
        { "Soda",  (2.00m, 8) }
    };

    public void InsertCoin(decimal amount) {
        _balance += amount;
        Console.WriteLine($"Balance: {_balance:C}");
    }

    public string SelectItem(string item) {
        if (!_items.ContainsKey(item)) return "Item not found";
        var (price, qty) = _items[item];
        if (qty == 0) return "Out of stock";
        if (_balance < price) return $"Insert {price - _balance:C} more";

        _balance -= price;
        _items[item] = (price, qty - 1);
        string change = _balance > 0 ? $" Change: {_balance:C}" : "";
        _balance = 0;
        return $"Dispensing {item}.{change}";
    }

    public decimal ReturnCoins() {
        decimal refund = _balance;
        _balance = 0;
        return refund;
    }
}
```

---

## Quick Reference: Complexity Cheat Sheet

| Operation | Time Complexity |
|---|---|
| Array access by index | O(1) |
| Array search (unsorted) | O(n) |
| Array search (sorted, binary) | O(log n) |
| Dictionary/HashSet lookup | O(1) avg |
| Sorting (LINQ OrderBy / built-in) | O(n log n) |
| Bubble / Selection / Insertion Sort | O(n²) |
| String reverse | O(n) |
| String Contains | O(n) |
| Linked list insert at head | O(1) |
| Linked list search | O(n) |
| Binary tree search (balanced) | O(log n) |
| Recursive Fibonacci (naive) | O(2ⁿ) |
| Recursive Fibonacci (memoized) | O(n) |

---

## Tips for MNC Coding Rounds

- **Always handle edge cases first** — null input, empty array, single element, negative numbers.
- **Use LINQ confidently** — MNC interviewers expect .NET developers to know LINQ.
- **Know both ways** — built-in method (LINQ) and manual loop. Interviewers often ask "now do it without LINQ."
- **Think out loud** — explain your approach before typing a single line.
- **Discuss time and space complexity** — even a rough answer shows maturity.
- **Write clean, readable code** — meaningful names, no magic numbers, proper indentation.
- **Test with the given example** — trace through your code manually before submitting.
- **Ask clarifying questions** — "Can the array be empty?", "Are duplicates allowed?", "Should it be case-sensitive?"
- **Start simple, then optimize** — brute force first, then improve if asked.
- **OOP questions are common in MNCs** — know your SOLID principles and design patterns.