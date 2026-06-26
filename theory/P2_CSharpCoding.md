# 🎯 100 .NET Interview Questions — 5 Years Experience
> **How to use:** Paste any code block into [dotnetfiddle.net](https://dotnetfiddle.net) or [onlinegdb.com/online_csharp_compiler](https://onlinegdb.com/online_csharp_compiler) and run it instantly.
> Each question has a **self-contained `Main()` method** so you can copy-paste and run without any setup.
> Practice 5 questions daily → finish in 20 days → crack the interview.

---

## 📋 TABLE OF CONTENTS
| Section | Topics | Questions |
|---|---|---|
| 1 | Strings — Core Logic | Q1–Q20 |
| 2 | Arrays & Collections | Q21–Q40 |
| 3 | Numbers & Math Logic | Q41–Q55 |
| 4 | LINQ Deep Dive | Q56–Q70 |
| 5 | Data Structures | Q71–Q80 |
| 6 | OOP & C# Concepts | Q81–Q90 |
| 7 | Mixed Scenario / Real-World | Q91–Q100 |

---

## ⏱ COMPLEXITY CHEAT SHEET
| Structure | Access | Search | Insert | Delete |
|---|---|---|---|---|
| Array | O(1) | O(n) | O(n) | O(n) |
| Dictionary / HashSet | O(1) avg | O(1) avg | O(1) avg | O(1) avg |
| List\<T\> | O(1) | O(n) | O(1) end | O(n) |
| Sorted array (binary search) | O(1) | O(log n) | O(n) | O(n) |
| LinkedList | O(n) | O(n) | O(1) head | O(1) known node |
| Stack / Queue | O(1) top | O(n) | O(1) | O(1) |

---

---

# SECTION 1 — STRINGS (Q1–Q20)

---

### Q1. Find the First Non-Repeating Character
**Logic:** Character that appears exactly once, in its original order.
**Example:** `"aabbcde"` → `'c'`

```csharp
using System;
using System.Linq;

class Q1 {
    // LINQ
    static char LinqWay(string s) =>
        s.GroupBy(c => c).FirstOrDefault(g => g.Count() == 1)?.Key ?? '\0';

    // Manual
    static char ManualWay(string s) {
        int[] freq = new int[256];
        foreach (char c in s) freq[c]++;
        foreach (char c in s) if (freq[c] == 1) return c;
        return '\0';
    }

    static void Main() {
        string input = "aabbcde";
        Console.WriteLine("LINQ  : " + LinqWay(input));   // c
        Console.WriteLine("Manual: " + ManualWay(input)); // c
    }
}
```

---

### Q2. Check if Two Strings are Anagrams
**Logic:** Same characters, different order. Sort and compare.
**Example:** `"listen"`, `"silent"` → `true`

```csharp
using System;
using System.Linq;

class Q2 {
    // LINQ
    static bool LinqWay(string a, string b) =>
        a.Length == b.Length &&
        a.ToLower().OrderBy(c => c).SequenceEqual(b.ToLower().OrderBy(c => c));

    // Manual
    static bool ManualWay(string a, string b) {
        if (a.Length != b.Length) return false;
        int[] count = new int[26];
        a = a.ToLower(); b = b.ToLower();
        foreach (char c in a) count[c - 'a']++;
        foreach (char c in b) count[c - 'a']--;
        return count.All(x => x == 0);
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("listen", "silent"));  // True
        Console.WriteLine("Manual: " + ManualWay("listen", "silent")); // True
        Console.WriteLine("LINQ  : " + LinqWay("hello", "world"));    // False
    }
}
```

---

### Q3. Longest Substring Without Repeating Characters
**Logic:** Sliding window — expand right, shrink left when duplicate found.
**Example:** `"abcabcbb"` → `3` (abc)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q3 {
    // LINQ-assisted (uses dictionary, LINQ for max)
    static int LinqWay(string s) {
        var seen = new Dictionary<char, int>();
        int start = 0, max = 0;
        for (int i = 0; i < s.Length; i++) {
            if (seen.ContainsKey(s[i]) && seen[s[i]] >= start)
                start = seen[s[i]] + 1;
            seen[s[i]] = i;
            max = Math.Max(max, i - start + 1);
        }
        return max;
    }

    // Manual (pure loop, no dictionary)
    static int ManualWay(string s) {
        int max = 0;
        for (int i = 0; i < s.Length; i++) {
            bool[] seen = new bool[256];
            int count = 0;
            for (int j = i; j < s.Length; j++) {
                if (seen[s[j]]) break;
                seen[s[j]] = true;
                count++;
            }
            if (count > max) max = count;
        }
        return max;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("abcabcbb")); // 3
        Console.WriteLine("Manual: " + ManualWay("abcabcbb")); // 3
        Console.WriteLine("LINQ  : " + LinqWay("pwwkew")); // 3
    }
}
```

---

### Q4. Reverse Only the Words, Not the Letters
**Logic:** Split on space, reverse the list of words, rejoin.
**Example:** `"Hello World How"` → `"How World Hello"`

```csharp
using System;
using System.Linq;

class Q4 {
    // LINQ
    static string LinqWay(string s) =>
        string.Join(" ", s.Split(' ').Reverse());

    // Manual
    static string ManualWay(string s) {
        string[] words = s.Split(' ');
        int l = 0, r = words.Length - 1;
        while (l < r) { (words[l], words[r]) = (words[r], words[l]); l++; r--; }
        return string.Join(" ", words);
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("Hello World How"));   // How World Hello
        Console.WriteLine("Manual: " + ManualWay("Hello World How")); // How World Hello
    }
}
```

---

### Q5. String Compression (Run-Length Encoding)
**Logic:** Count consecutive chars. Append char + count. If count = 1, skip the number.
**Example:** `"aaabbbccdddd"` → `"a3b3c2d4"`

```csharp
using System;
using System.Text;
using System.Linq;

class Q5 {
    // LINQ-assisted
    static string LinqWay(string s) {
        var sb = new StringBuilder();
        foreach (var g in s.Select((c, i) => (c, i))
                            .GroupBy(x => {
                                int k = x.i;
                                while (k > 0 && s[k - 1] == x.c) k--;
                                return k;
                            })) {
            sb.Append(g.First().c);
            if (g.Count() > 1) sb.Append(g.Count());
        }
        return sb.ToString();
    }

    // Manual (cleaner for interviews)
    static string ManualWay(string s) {
        var sb = new StringBuilder();
        int i = 0;
        while (i < s.Length) {
            char c = s[i];
            int count = 0;
            while (i < s.Length && s[i] == c) { count++; i++; }
            sb.Append(c);
            if (count > 1) sb.Append(count);
        }
        return sb.ToString();
    }

    static void Main() {
        Console.WriteLine("Manual: " + ManualWay("aaabbbccdddd")); // a3b3c2d4
        Console.WriteLine("Manual: " + ManualWay("abcd"));         // abcd
    }
}
```

---

### Q6. Check if a String is a Rotation of Another
**Logic:** If B is a rotation of A, then B will always be a substring of A+A.
**Example:** `"abcde"`, `"cdeab"` → `true`

```csharp
using System;

class Q6 {
    // LINQ (String.Contains is the key insight)
    static bool LinqWay(string a, string b) =>
        a.Length == b.Length && (a + a).Contains(b);

    // Manual (KMP-style search inside a+a)
    static bool ManualWay(string a, string b) {
        if (a.Length != b.Length) return false;
        string doubled = a + a;
        // simple substring search
        for (int i = 0; i <= doubled.Length - b.Length; i++) {
            bool match = true;
            for (int j = 0; j < b.Length; j++) {
                if (doubled[i + j] != b[j]) { match = false; break; }
            }
            if (match) return true;
        }
        return false;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("abcde", "cdeab"));  // True
        Console.WriteLine("Manual: " + ManualWay("abcde", "cdeab")); // True
        Console.WriteLine("LINQ  : " + LinqWay("abcde", "abced"));  // False
    }
}
```

---

### Q7. Find All Permutations of a String
**Logic:** Recursively fix one character and permute the rest.
**Example:** `"abc"` → `abc acb bac bca cab cba`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q7 {
    // LINQ (recursive, functional style)
    static IEnumerable<string> LinqWay(string s) =>
        s.Length <= 1 ? new[] { s } :
        s.SelectMany((c, i) =>
            LinqWay(s.Remove(i, 1)).Select(p => c + p));

    // Manual
    static void ManualWay(string s, string result = "") {
        if (s.Length == 0) { Console.Write(result + " "); return; }
        for (int i = 0; i < s.Length; i++)
            ManualWay(s.Remove(i, 1), result + s[i]);
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + string.Join(" ", LinqWay("abc")));
        Console.Write("Manual: "); ManualWay("abc"); Console.WriteLine();
    }
}
```

---

### Q8. Longest Common Prefix Among an Array of Strings
**Logic:** Start with first string as prefix. Keep trimming from end until all strings start with it.
**Example:** `["flower","flow","flight"]` → `"fl"`

```csharp
using System;
using System.Linq;

class Q8 {
    // LINQ
    static string LinqWay(string[] arr) {
        if (arr.Length == 0) return "";
        return arr.Aggregate((prefix, s) => {
            while (!s.StartsWith(prefix)) prefix = prefix[..^1];
            return prefix;
        });
    }

    // Manual
    static string ManualWay(string[] arr) {
        if (arr.Length == 0) return "";
        string prefix = arr[0];
        for (int i = 1; i < arr.Length; i++)
            while (!arr[i].StartsWith(prefix))
                prefix = prefix.Substring(0, prefix.Length - 1);
        return prefix;
    }

    static void Main() {
        string[] input = { "flower", "flow", "flight" };
        Console.WriteLine("LINQ  : " + LinqWay(input));   // fl
        Console.WriteLine("Manual: " + ManualWay(input)); // fl
    }
}
```

---

### Q9. Decode a Run-Length Encoded String
**Logic:** Read char, then read all following digits as the repeat count.
**Example:** `"a3b2c4"` → `"aaabbcccc"`

```csharp
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

class Q9 {
    // LINQ + Regex
    static string LinqWay(string s) {
        var matches = Regex.Matches(s, @"([a-zA-Z])(\d*)");
        return string.Concat(matches.Cast<Match>()
            .Select(m => new string(m.Groups[1].Value[0],
                m.Groups[2].Value == "" ? 1 : int.Parse(m.Groups[2].Value))));
    }

    // Manual
    static string ManualWay(string s) {
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

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("a3b2c4"));   // aaabbcccc
        Console.WriteLine("Manual: " + ManualWay("a3b2c4")); // aaabbcccc
    }
}
```

---

### Q10. Check if Parentheses / Brackets are Balanced
**Logic:** Stack-based. Push on opening, pop on closing, check they match.
**Example:** `"({[]})"` → `true`, `"({[})"` → `false`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q10 {
    // LINQ-assisted (uses stack, LINQ for pair matching)
    static bool LinqWay(string s) {
        var pairs = new Dictionary<char, char> { [')']=  '(', ['}'] = '{', [']'] = '[' };
        var stack = new Stack<char>();
        return s.All(c => {
            if ("({[".Contains(c)) { stack.Push(c); return true; }
            if (")}]".Contains(c)) return stack.Count > 0 && stack.Pop() == pairs[c];
            return true;
        });
    }

    // Manual
    static bool ManualWay(string s) {
        var stack = new Stack<char>();
        foreach (char c in s) {
            if (c == '(' || c == '{' || c == '[') stack.Push(c);
            else if (c == ')' || c == '}' || c == ']') {
                if (stack.Count == 0) return false;
                char top = stack.Pop();
                if (c == ')' && top != '(') return false;
                if (c == '}' && top != '{') return false;
                if (c == ']' && top != '[') return false;
            }
        }
        return stack.Count == 0;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("({[]})"));  // True
        Console.WriteLine("Manual: " + ManualWay("({[})"));  // False
    }
}
```

---

### Q11. Find All Words That Appear More Than Once
**Logic:** Group by word, filter count > 1.
**Example:** `"the cat sat on the mat the"` → `["the"]`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q11 {
    // LINQ
    static List<string> LinqWay(string s) =>
        s.ToLower().Split(' ')
         .GroupBy(w => w)
         .Where(g => g.Count() > 1)
         .Select(g => g.Key)
         .ToList();

    // Manual
    static List<string> ManualWay(string s) {
        var freq = new Dictionary<string, int>();
        foreach (var word in s.ToLower().Split(' '))
            freq[word] = freq.ContainsKey(word) ? freq[word] + 1 : 1;
        var result = new List<string>();
        foreach (var kv in freq) if (kv.Value > 1) result.Add(kv.Key);
        return result;
    }

    static void Main() {
        string input = "the cat sat on the mat the";
        Console.WriteLine("LINQ  : " + string.Join(", ", LinqWay(input)));
        Console.WriteLine("Manual: " + string.Join(", ", ManualWay(input)));
    }
}
```

---

### Q12. Reverse Only Vowels in a String
**Logic:** Two-pointer approach. Move inward, swap only when both pointers are on vowels.
**Example:** `"hello"` → `"holle"`

```csharp
using System;
using System.Linq;

class Q12 {
    static readonly string Vowels = "aeiouAEIOU";

    // LINQ-assisted (collect vowels, place in reverse)
    static string LinqWay(string s) {
        var vowelList = s.Where(c => Vowels.Contains(c)).Reverse().ToList();
        int idx = 0;
        return new string(s.Select(c => Vowels.Contains(c) ? vowelList[idx++] : c).ToArray());
    }

    // Manual (two pointers)
    static string ManualWay(string s) {
        char[] arr = s.ToCharArray();
        int l = 0, r = arr.Length - 1;
        while (l < r) {
            while (l < r && !Vowels.Contains(arr[l])) l++;
            while (l < r && !Vowels.Contains(arr[r])) r--;
            (arr[l], arr[r]) = (arr[r], arr[l]);
            l++; r--;
        }
        return new string(arr);
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("hello"));   // holle
        Console.WriteLine("Manual: " + ManualWay("hello")); // holle
        Console.WriteLine("LINQ  : " + LinqWay("leetcode")); // leotcede
    }
}
```

---

### Q13. Count Occurrences of Each Word (Word Frequency)
**Logic:** Split, group, count. Classic frequency map.
**Example:** `"the cat sat on the mat"` → `the:2, cat:1, ...`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q13 {
    // LINQ
    static Dictionary<string, int> LinqWay(string text) =>
        text.ToLower().Split(' ')
            .GroupBy(w => w)
            .ToDictionary(g => g.Key, g => g.Count());

    // Manual
    static Dictionary<string, int> ManualWay(string text) {
        var freq = new Dictionary<string, int>();
        foreach (var word in text.ToLower().Split(' '))
            freq[word] = freq.ContainsKey(word) ? freq[word] + 1 : 1;
        return freq;
    }

    static void Main() {
        string text = "the cat sat on the mat";
        var linqResult = LinqWay(text);
        Console.WriteLine("LINQ  : " + string.Join(", ", linqResult.Select(kv => $"{kv.Key}:{kv.Value}")));
        var manualResult = ManualWay(text);
        Console.WriteLine("Manual: " + string.Join(", ", manualResult.Select(kv => $"{kv.Key}:{kv.Value}")));
    }
}
```

---

### Q14. Find the Longest Palindromic Substring
**Logic:** Expand around each character (and between characters) as center.
**Example:** `"babad"` → `"bab"`, `"racecar"` → `"racecar"`

```csharp
using System;
using System.Linq;

class Q14 {
    // LINQ-assisted (brute force with LINQ filter)
    static string LinqWay(string s) {
        bool IsPalin(string t) => t.SequenceEqual(t.Reverse());
        string best = "";
        for (int i = 0; i < s.Length; i++)
            for (int j = i + 1; j <= s.Length; j++) {
                var sub = s.Substring(i, j - i);
                if (IsPalin(sub) && sub.Length > best.Length) best = sub;
            }
        return best;
    }

    // Manual (expand-around-center, O(n²) but optimal for interviews)
    static string ManualWay(string s) {
        string Expand(int l, int r) {
            while (l >= 0 && r < s.Length && s[l] == s[r]) { l--; r++; }
            return s.Substring(l + 1, r - l - 1);
        }
        string best = "";
        for (int i = 0; i < s.Length; i++) {
            string odd  = Expand(i, i);
            string even = Expand(i, i + 1);
            if (odd.Length  > best.Length) best = odd;
            if (even.Length > best.Length) best = even;
        }
        return best;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("racecar"));   // racecar
        Console.WriteLine("Manual: " + ManualWay("babad"));   // bab
        Console.WriteLine("Manual: " + ManualWay("cbbd"));    // bb
    }
}
```

---

### Q15. Check if a String is a Pangram
**Logic:** Contains every letter a-z at least once.
**Example:** `"The quick brown fox jumps over the lazy dog"` → `true`

```csharp
using System;
using System.Linq;

class Q15 {
    // LINQ
    static bool LinqWay(string s) {
        s = s.ToLower();
        return Enumerable.Range('a', 26).All(c => s.Contains((char)c));
    }

    // Manual
    static bool ManualWay(string s) {
        bool[] seen = new bool[26];
        foreach (char c in s.ToLower())
            if (c >= 'a' && c <= 'z') seen[c - 'a'] = true;
        foreach (bool b in seen) if (!b) return false;
        return true;
    }

    static void Main() {
        string pangram = "The quick brown fox jumps over the lazy dog";
        Console.WriteLine("LINQ  : " + LinqWay(pangram));   // True
        Console.WriteLine("Manual: " + ManualWay(pangram)); // True
        Console.WriteLine("LINQ  : " + LinqWay("Hello World")); // False
    }
}
```

---

### Q16. Find the Hamming Distance Between Two Strings
**Logic:** Count positions where characters differ (same-length strings).
**Example:** `"karolin"`, `"kathrin"` → `3`

```csharp
using System;
using System.Linq;

class Q16 {
    // LINQ
    static int LinqWay(string a, string b) =>
        a.Zip(b, (x, y) => x != y).Count(diff => diff);

    // Manual
    static int ManualWay(string a, string b) {
        int count = 0;
        for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            if (a[i] != b[i]) count++;
        return count;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("karolin", "kathrin")); // 3
        Console.WriteLine("Manual: " + ManualWay("karolin", "kathrin")); // 3
    }
}
```

---

### Q17. Convert a Sentence to camelCase
**Logic:** First word lowercase, each subsequent word capitalized.
**Example:** `"hello world foo"` → `"helloWorldFoo"`

```csharp
using System;
using System.Linq;

class Q17 {
    // LINQ
    static string LinqWay(string s) {
        var words = s.Split(' ');
        return words[0].ToLower() + string.Concat(
            words.Skip(1).Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));
    }

    // Manual
    static string ManualWay(string s) {
        var words = s.Split(' ');
        string result = words[0].ToLower();
        for (int i = 1; i < words.Length; i++)
            result += char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
        return result;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("hello world foo"));   // helloWorldFoo
        Console.WriteLine("Manual: " + ManualWay("hello world foo")); // helloWorldFoo
    }
}
```

---

### Q18. Find the First Repeated Word in a Sentence
**Logic:** Use a HashSet — first word that fails Add() is the first repeat.
**Example:** `"the cat sat on the mat"` → `"the"`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q18 {
    // LINQ
    static string LinqWay(string s) {
        var seen = new HashSet<string>();
        return s.ToLower().Split(' ').FirstOrDefault(w => !seen.Add(w)) ?? "none";
    }

    // Manual
    static string ManualWay(string s) {
        var seen = new HashSet<string>();
        foreach (var word in s.ToLower().Split(' '))
            if (!seen.Add(word)) return word;
        return "none";
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("the cat sat on the mat"));   // the
        Console.WriteLine("Manual: " + ManualWay("the cat sat on the mat")); // the
    }
}
```

---

### Q19. Validate a Strong Password
**Logic:** 8+ chars, at least 1 uppercase, 1 lowercase, 1 digit, 1 special character.
**Example:** `"Pass@123"` → `true`, `"password"` → `false`

```csharp
using System;
using System.Linq;

class Q19 {
    // LINQ
    static bool LinqWay(string p) =>
        p.Length >= 8 &&
        p.Any(char.IsUpper) &&
        p.Any(char.IsLower) &&
        p.Any(char.IsDigit) &&
        p.Any(c => "!@#$%^&*".Contains(c));

    // Manual
    static bool ManualWay(string p) {
        if (p.Length < 8) return false;
        bool hasUpper = false, hasLower = false, hasDigit = false, hasSpecial = false;
        foreach (char c in p) {
            if (char.IsUpper(c)) hasUpper = true;
            else if (char.IsLower(c)) hasLower = true;
            else if (char.IsDigit(c)) hasDigit = true;
            else if ("!@#$%^&*".Contains(c)) hasSpecial = true;
        }
        return hasUpper && hasLower && hasDigit && hasSpecial;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("Pass@123"));   // True
        Console.WriteLine("Manual: " + ManualWay("password"));  // False
        Console.WriteLine("LINQ  : " + LinqWay("P@ssw0rd"));   // True
    }
}
```

---

### Q20. Count How Many Times a Substring Appears (Non-Overlapping)
**Logic:** Move index forward by substring length after each find.
**Example:** `"abcabcabc"`, `"abc"` → `3`

```csharp
using System;
using System.Text.RegularExpressions;
using System.Linq;

class Q20 {
    // LINQ + Regex
    static int LinqWay(string s, string sub) =>
        Regex.Matches(s, Regex.Escape(sub)).Count;

    // Manual
    static int ManualWay(string s, string sub) {
        int count = 0, index = 0;
        while ((index = s.IndexOf(sub, index)) != -1) {
            count++;
            index += sub.Length;
        }
        return count;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay("abcabcabc", "abc"));   // 3
        Console.WriteLine("Manual: " + ManualWay("abcabcabc", "abc")); // 3
        Console.WriteLine("Manual: " + ManualWay("aaaa", "aa"));        // 2
    }
}
```

---

---

# SECTION 2 — ARRAYS & COLLECTIONS (Q21–Q40)

---

### Q21. Two Sum — Return Indices of Two Numbers That Add to Target
**Logic:** HashMap stores value→index. For each element, check if complement exists.
**Example:** `[2,7,11,15]`, target=9 → `[0,1]`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q21 {
    // LINQ (brute force with LINQ)
    static int[] LinqWay(int[] nums, int target) =>
        (from i in Enumerable.Range(0, nums.Length)
         from j in Enumerable.Range(i + 1, nums.Length - i - 1)
         where nums[i] + nums[j] == target
         select new[] { i, j }).First();

    // Manual (O(n) with HashMap)
    static int[] ManualWay(int[] nums, int target) {
        var map = new Dictionary<int, int>();
        for (int i = 0; i < nums.Length; i++) {
            int comp = target - nums[i];
            if (map.ContainsKey(comp)) return new[] { map[comp], i };
            map[nums[i]] = i;
        }
        return Array.Empty<int>();
    }

    static void Main() {
        int[] result1 = LinqWay(new[] { 2, 7, 11, 15 }, 9);
        Console.WriteLine("LINQ  : [" + string.Join(",", result1) + "]"); // [0,1]
        int[] result2 = ManualWay(new[] { 3, 2, 4 }, 6);
        Console.WriteLine("Manual: [" + string.Join(",", result2) + "]"); // [1,2]
    }
}
```

---

### Q22. Find the Missing Number in 1 to N
**Logic:** Expected sum = N*(N+1)/2. Subtract actual sum.
**Example:** `[1,2,4,5]` → `3`

```csharp
using System;
using System.Linq;

class Q22 {
    // LINQ
    static int LinqWay(int[] arr) {
        int n = arr.Length + 1;
        return n * (n + 1) / 2 - arr.Sum();
    }

    // Manual
    static int ManualWay(int[] arr) {
        int n = arr.Length + 1;
        int expected = n * (n + 1) / 2;
        int actual = 0;
        foreach (int x in arr) actual += x;
        return expected - actual;
    }

    static void Main() {
        int[] arr = { 1, 2, 4, 5 };
        Console.WriteLine("LINQ  : " + LinqWay(arr));   // 3
        Console.WriteLine("Manual: " + ManualWay(arr)); // 3
    }
}
```

---

### Q23. Find the Maximum Subarray Sum (Kadane's Algorithm)
**Logic:** Track current sum; reset to current element if adding makes it worse.
**Example:** `[-2,1,-3,4,-1,2,1,-5,4]` → `6`

```csharp
using System;
using System.Linq;

class Q23 {
    // LINQ (brute force O(n²) using LINQ)
    static int LinqWay(int[] nums) =>
        Enumerable.Range(0, nums.Length)
            .SelectMany(i => Enumerable.Range(1, nums.Length - i)
                .Select(len => nums.Skip(i).Take(len).Sum()))
            .Max();

    // Manual (Kadane's O(n))
    static int ManualWay(int[] nums) {
        int max = nums[0], current = nums[0];
        for (int i = 1; i < nums.Length; i++) {
            current = Math.Max(nums[i], current + nums[i]);
            max = Math.Max(max, current);
        }
        return max;
    }

    static void Main() {
        int[] arr = { -2, 1, -3, 4, -1, 2, 1, -5, 4 };
        Console.WriteLine("LINQ  : " + LinqWay(arr));   // 6
        Console.WriteLine("Manual: " + ManualWay(arr)); // 6
    }
}
```

---

### Q24. Move All Zeros to the End (Preserve Order)
**Logic:** Collect non-zeros first, then fill remaining with zeros.
**Example:** `[0,1,0,3,12]` → `[1,3,12,0,0]`

```csharp
using System;
using System.Linq;

class Q24 {
    // LINQ
    static int[] LinqWay(int[] arr) =>
        arr.Where(x => x != 0).Concat(arr.Where(x => x == 0)).ToArray();

    // Manual (in-place, two-pointer)
    static void ManualWay(int[] arr) {
        int pos = 0;
        foreach (int x in arr) if (x != 0) arr[pos++] = x;
        while (pos < arr.Length) arr[pos++] = 0;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + string.Join(",", LinqWay(new[] { 0, 1, 0, 3, 12 })));
        int[] arr = { 0, 1, 0, 3, 12 };
        ManualWay(arr);
        Console.WriteLine("Manual: " + string.Join(",", arr)); // 1,3,12,0,0
    }
}
```

---

### Q25. Find the Majority Element (Appears > n/2 Times)
**Logic:** Boyer-Moore Voting — cancel opposites, last standing is majority.
**Example:** `[3,2,3]` → `3`

```csharp
using System;
using System.Linq;

class Q25 {
    // LINQ
    static int LinqWay(int[] nums) =>
        nums.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;

    // Manual (Boyer-Moore O(n) time O(1) space)
    static int ManualWay(int[] nums) {
        int candidate = nums[0], count = 1;
        for (int i = 1; i < nums.Length; i++) {
            count += nums[i] == candidate ? 1 : -1;
            if (count == 0) { candidate = nums[i]; count = 1; }
        }
        return candidate;
    }

    static void Main() {
        int[] arr = { 2, 2, 1, 1, 1, 2, 2 };
        Console.WriteLine("LINQ  : " + LinqWay(arr));   // 2
        Console.WriteLine("Manual: " + ManualWay(arr)); // 2
    }
}
```

---

### Q26. Product of Array Except Self (No Division)
**Logic:** Left pass stores prefix products; right pass multiplies suffix products.
**Example:** `[1,2,3,4]` → `[24,12,8,6]`

```csharp
using System;
using System.Linq;

class Q26 {
    // LINQ-assisted (uses Aggregate for prefix/suffix)
    static int[] LinqWay(int[] nums) {
        int n = nums.Length;
        int[] result = new int[n];
        result[0] = 1;
        for (int i = 1; i < n; i++) result[i] = result[i - 1] * nums[i - 1];
        int right = 1;
        for (int i = n - 1; i >= 0; i--) { result[i] *= right; right *= nums[i]; }
        return result;
    }

    // Manual (same logic, just explicit)
    static int[] ManualWay(int[] nums) {
        int n = nums.Length;
        int[] left = new int[n], right = new int[n], result = new int[n];
        left[0] = right[n - 1] = 1;
        for (int i = 1; i < n; i++) left[i] = left[i - 1] * nums[i - 1];
        for (int i = n - 2; i >= 0; i--) right[i] = right[i + 1] * nums[i + 1];
        for (int i = 0; i < n; i++) result[i] = left[i] * right[i];
        return result;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + string.Join(",", LinqWay(new[] { 1, 2, 3, 4 })));
        Console.WriteLine("Manual: " + string.Join(",", ManualWay(new[] { 1, 2, 3, 4 }))); // 24,12,8,6
    }
}
```

---

### Q27. Find the Next Greater Element for Each Array Element
**Logic:** Stack-based — for each element, pop stack items smaller than it. They found their NGE.
**Example:** `[4,5,2,10,8]` → `[5,10,10,-1,-1]`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q27 {
    // LINQ (brute force)
    static int[] LinqWay(int[] arr) =>
        arr.Select((val, i) =>
            arr.Skip(i + 1).FirstOrDefault(x => x > val, -1)).ToArray();

    // Manual (Stack O(n))
    static int[] ManualWay(int[] arr) {
        int n = arr.Length;
        int[] result = Enumerable.Repeat(-1, n).ToArray();
        var stack = new Stack<int>(); // stores indices
        for (int i = 0; i < n; i++) {
            while (stack.Count > 0 && arr[i] > arr[stack.Peek()])
                result[stack.Pop()] = arr[i];
            stack.Push(i);
        }
        return result;
    }

    static void Main() {
        int[] arr = { 4, 5, 2, 10, 8 };
        Console.WriteLine("LINQ  : " + string.Join(",", LinqWay(arr)));   // 5,10,10,-1,-1
        Console.WriteLine("Manual: " + string.Join(",", ManualWay(arr))); // 5,10,10,-1,-1
    }
}
```

---

### Q28. Find All Pairs with a Given Sum
**Logic:** HashSet — for each element, check if complement was seen before.
**Example:** `[1,5,3,2,4]`, target=6 → `(1,5) (2,4)`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q28 {
    // LINQ (all combinations)
    static void LinqWay(int[] arr, int target) {
        var pairs = from i in Enumerable.Range(0, arr.Length)
                    from j in Enumerable.Range(i + 1, arr.Length - i - 1)
                    where arr[i] + arr[j] == target
                    select $"({arr[i]},{arr[j]})";
        Console.WriteLine("LINQ  : " + string.Join(" ", pairs));
    }

    // Manual (HashSet O(n))
    static void ManualWay(int[] arr, int target) {
        var seen = new HashSet<int>();
        var printed = new HashSet<string>();
        foreach (int n in arr) {
            int comp = target - n;
            if (seen.Contains(comp)) {
                int a = Math.Min(n, comp), b = Math.Max(n, comp);
                printed.Add($"({a},{b})");
            }
            seen.Add(n);
        }
        Console.WriteLine("Manual: " + string.Join(" ", printed));
    }

    static void Main() {
        int[] arr = { 1, 5, 3, 2, 4 };
        LinqWay(arr, 6);
        ManualWay(arr, 6);
    }
}
```

---

### Q29. Find the Smallest Positive Missing Number
**Logic:** Use HashSet. Count up from 1 until a miss is found.
**Example:** `[3,4,-1,1]` → `2`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q29 {
    // LINQ
    static int LinqWay(int[] arr) {
        var set = new HashSet<int>(arr);
        return Enumerable.Range(1, int.MaxValue).First(i => !set.Contains(i));
    }

    // Manual
    static int ManualWay(int[] arr) {
        var set = new HashSet<int>(arr);
        for (int i = 1; ; i++) if (!set.Contains(i)) return i;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(new[] { 3, 4, -1, 1 }));  // 2
        Console.WriteLine("Manual: " + ManualWay(new[] { 1, 2, 0 }));    // 3
    }
}
```

---

### Q30. Find the Longest Increasing Subarray (Contiguous)
**Logic:** Track current streak; reset when order breaks.
**Example:** `[1,2,3,1,2,3,4]` → `4`

```csharp
using System;
using System.Linq;

class Q30 {
    // LINQ
    static int LinqWay(int[] arr) {
        int max = 1, cur = 1;
        return arr.Skip(1).Select((x, i) => {
            cur = x > arr[i] ? cur + 1 : 1;
            max = Math.Max(max, cur);
            return max;
        }).DefaultIfEmpty(1).Last();
    }

    // Manual
    static int ManualWay(int[] arr) {
        int max = 1, cur = 1;
        for (int i = 1; i < arr.Length; i++) {
            cur = arr[i] > arr[i - 1] ? cur + 1 : 1;
            if (cur > max) max = cur;
        }
        return max;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(new[] { 1, 2, 3, 1, 2, 3, 4 }));   // 4
        Console.WriteLine("Manual: " + ManualWay(new[] { 1, 2, 3, 1, 2, 3, 4 })); // 4
    }
}
```

---

### Q31. Rotate Array to the Right by K Steps
**Logic:** Take last K elements, prepend to first N-K elements.
**Example:** `[1,2,3,4,5]`, K=2 → `[4,5,1,2,3]`

```csharp
using System;
using System.Linq;

class Q31 {
    // LINQ
    static int[] LinqWay(int[] arr, int k) {
        k = k % arr.Length;
        return arr.Skip(arr.Length - k).Concat(arr.Take(arr.Length - k)).ToArray();
    }

    // Manual (three-reverse trick)
    static void Reverse(int[] arr, int l, int r) {
        while (l < r) { (arr[l], arr[r]) = (arr[r], arr[l]); l++; r--; }
    }
    static int[] ManualWay(int[] arr, int k) {
        k = k % arr.Length;
        Reverse(arr, 0, arr.Length - 1);
        Reverse(arr, 0, k - 1);
        Reverse(arr, k, arr.Length - 1);
        return arr;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + string.Join(",", LinqWay(new[] { 1, 2, 3, 4, 5 }, 2)));
        Console.WriteLine("Manual: " + string.Join(",", ManualWay(new[] { 1, 2, 3, 4, 5 }, 2)));
        // Both: 4,5,1,2,3
    }
}
```

---

### Q32. Find the Kth Largest Element Without Full Sort
**Logic:** Partial sort using a min-heap of size K. Much faster for large arrays.
**Example:** `[3,2,1,5,6,4]`, K=2 → `5`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q32 {
    // LINQ
    static int LinqWay(int[] arr, int k) =>
        arr.OrderByDescending(x => x).ElementAt(k - 1);

    // Manual (min-heap of size k)
    static int ManualWay(int[] arr, int k) {
        var minHeap = new SortedList<int, int>(); // simulate heap
        foreach (int x in arr) {
            if (!minHeap.ContainsKey(x)) minHeap[x] = 0;
            minHeap[x]++;
            // keep only k largest by removing smallest when over k total
            int total = minHeap.Values.Sum();
            while (total > k) {
                int smallest = minHeap.Keys[0];
                if (minHeap[smallest] == 1) minHeap.Remove(smallest);
                else minHeap[smallest]--;
                total--;
            }
        }
        return minHeap.Keys[0];
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(new[] { 3, 2, 1, 5, 6, 4 }, 2));   // 5
        Console.WriteLine("Manual: " + ManualWay(new[] { 3, 2, 1, 5, 6, 4 }, 2)); // 5
    }
}
```

---

### Q33. Generate All Subsets (Power Set)
**Logic:** For each existing subset, create a new one with the current element added.
**Example:** `[1,2,3]` → `[], [1], [2], [1,2], [3], [1,3], [2,3], [1,2,3]`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q33 {
    // LINQ
    static List<List<int>> LinqWay(int[] nums) {
        var result = new List<List<int>> { new() };
        foreach (int n in nums)
            result = result.Concat(result.Select(s => new List<int>(s) { n })).ToList();
        return result;
    }

    // Manual (bit masking)
    static List<List<int>> ManualWay(int[] nums) {
        var result = new List<List<int>>();
        int total = 1 << nums.Length;
        for (int mask = 0; mask < total; mask++) {
            var subset = new List<int>();
            for (int i = 0; i < nums.Length; i++)
                if ((mask & (1 << i)) != 0) subset.Add(nums[i]);
            result.Add(subset);
        }
        return result;
    }

    static void Main() {
        var r1 = LinqWay(new[] { 1, 2, 3 });
        Console.WriteLine("LINQ  subsets: " + r1.Count); // 8
        var r2 = ManualWay(new[] { 1, 2, 3 });
        Console.WriteLine("Manual subsets: " + r2.Count); // 8
        foreach (var s in r2) Console.Write("[" + string.Join(",", s) + "] ");
    }
}
```

---

### Q34. Find the Cumulative Sum Array
**Logic:** Each element = itself + all previous elements.
**Example:** `[1,2,3,4]` → `[1,3,6,10]`

```csharp
using System;
using System.Linq;

class Q34 {
    // LINQ (Aggregate with index trick)
    static int[] LinqWay(int[] arr) {
        int running = 0;
        return arr.Select(x => running += x).ToArray();
    }

    // Manual
    static int[] ManualWay(int[] arr) {
        int[] result = new int[arr.Length];
        result[0] = arr[0];
        for (int i = 1; i < arr.Length; i++) result[i] = result[i - 1] + arr[i];
        return result;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + string.Join(",", LinqWay(new[] { 1, 2, 3, 4 })));   // 1,3,6,10
        Console.WriteLine("Manual: " + string.Join(",", ManualWay(new[] { 1, 2, 3, 4 }))); // 1,3,6,10
    }
}
```

---

### Q35. Find the Maximum Consecutive Ones in a Binary Array
**Logic:** Running counter; reset to 0 on 0, track max.
**Example:** `[1,1,0,1,1,1]` → `3`

```csharp
using System;
using System.Linq;

class Q35 {
    // LINQ
    static int LinqWay(int[] nums) {
        int count = 0;
        return nums.Select(x => count = x == 1 ? count + 1 : 0).Max();
    }

    // Manual
    static int ManualWay(int[] nums) {
        int max = 0, count = 0;
        foreach (int n in nums) {
            count = n == 1 ? count + 1 : 0;
            if (count > max) max = count;
        }
        return max;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(new[] { 1, 1, 0, 1, 1, 1 }));   // 3
        Console.WriteLine("Manual: " + ManualWay(new[] { 1, 1, 0, 1, 1, 1 })); // 3
    }
}
```

---

### Q36. Separate Even and Odd Numbers (Evens First)
**Logic:** Partition by modulo. Stable partition preserving order.
**Example:** `[1,2,3,4,5]` → `[2,4,1,3,5]`

```csharp
using System;
using System.Linq;

class Q36 {
    // LINQ
    static int[] LinqWay(int[] arr) =>
        arr.Where(x => x % 2 == 0).Concat(arr.Where(x => x % 2 != 0)).ToArray();

    // Manual (stable partition)
    static int[] ManualWay(int[] arr) {
        var evens = new System.Collections.Generic.List<int>();
        var odds  = new System.Collections.Generic.List<int>();
        foreach (int x in arr) { if (x % 2 == 0) evens.Add(x); else odds.Add(x); }
        evens.AddRange(odds);
        return evens.ToArray();
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + string.Join(",", LinqWay(new[] { 1, 2, 3, 4, 5 })));
        Console.WriteLine("Manual: " + string.Join(",", ManualWay(new[] { 1, 2, 3, 4, 5 })));
        // 2,4,1,3,5
    }
}
```

---

### Q37. Check if Array is a Subset of Another
**Logic:** Every element of A must exist in B.
**Example:** A=`[1,2,3]`, B=`[1,2,3,4,5]` → `true`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q37 {
    // LINQ
    static bool LinqWay(int[] a, int[] b) => a.All(x => b.Contains(x));

    // Manual (HashSet for O(n))
    static bool ManualWay(int[] a, int[] b) {
        var set = new HashSet<int>(b);
        foreach (int x in a) if (!set.Contains(x)) return false;
        return true;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4, 5 }));   // True
        Console.WriteLine("Manual: " + ManualWay(new[] { 1, 2, 6 }, new[] { 1, 2, 3, 4, 5 })); // False
    }
}
```

---

### Q38. Count Elements Greater Than Average
**Logic:** Compute average, then count elements strictly above it.
**Example:** `[1,2,3,4,5]` → `2` (4 and 5 are above avg 3)

```csharp
using System;
using System.Linq;

class Q38 {
    // LINQ
    static int LinqWay(int[] arr) {
        double avg = arr.Average();
        return arr.Count(x => x > avg);
    }

    // Manual
    static int ManualWay(int[] arr) {
        double avg = 0;
        foreach (int x in arr) avg += x;
        avg /= arr.Length;
        int count = 0;
        foreach (int x in arr) if (x > avg) count++;
        return count;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(new[] { 1, 2, 3, 4, 5 }));   // 2
        Console.WriteLine("Manual: " + ManualWay(new[] { 1, 2, 3, 4, 5 })); // 2
    }
}
```

---

### Q39. Flatten a 2D Array into 1D
**Logic:** SelectMany in LINQ; nested loop manually.
**Example:** `[[1,2],[3,4],[5]]` → `[1,2,3,4,5]`

```csharp
using System;
using System.Linq;

class Q39 {
    // LINQ
    static int[] LinqWay(int[][] arr) => arr.SelectMany(x => x).ToArray();

    // Manual
    static int[] ManualWay(int[][] arr) {
        var result = new System.Collections.Generic.List<int>();
        foreach (var row in arr)
            foreach (int x in row) result.Add(x);
        return result.ToArray();
    }

    static void Main() {
        int[][] arr = { new[] { 1, 2 }, new[] { 3, 4 }, new[] { 5 } };
        Console.WriteLine("LINQ  : " + string.Join(",", LinqWay(arr)));   // 1,2,3,4,5
        Console.WriteLine("Manual: " + string.Join(",", ManualWay(arr))); // 1,2,3,4,5
    }
}
```

---

### Q40. Find the Pair with the Maximum Product
**Logic:** Sort descending, the top two give max product (for positives). For negatives, check last two also.
**Example:** `[1,5,3,9,2]` → `(5,9)` = 45

```csharp
using System;
using System.Linq;

class Q40 {
    // LINQ
    static (int, int) LinqWay(int[] arr) {
        var sorted = arr.OrderByDescending(x => x).ToArray();
        long posProduct = (long)sorted[0] * sorted[1];
        var asc = arr.OrderBy(x => x).ToArray();
        long negProduct = (long)asc[0] * asc[1];
        return negProduct > posProduct ? (asc[0], asc[1]) : (sorted[0], sorted[1]);
    }

    // Manual
    static (int, int) ManualWay(int[] arr) {
        Array.Sort(arr);
        long negProd = (long)arr[0] * arr[1];
        long posProd = (long)arr[^1] * arr[^2];
        return negProd > posProd ? (arr[0], arr[1]) : (arr[^1], arr[^2]);
    }

    static void Main() {
        var r1 = LinqWay(new[] { 1, 5, 3, 9, 2 });
        Console.WriteLine("LINQ  : " + r1); // (9, 5)
        var r2 = ManualWay(new[] { -10, -3, 1, 5 });
        Console.WriteLine("Manual: " + r2); // (-10, -3) product=30
    }
}
```

---

---

# SECTION 3 — NUMBERS & MATH (Q41–Q55)

---

### Q41. Check if a Number is Prime
**Logic:** No divisor from 2 to √n means prime.
**Example:** `7` → `true`, `9` → `false`

```csharp
using System;
using System.Linq;

class Q41 {
    // LINQ
    static bool LinqWay(int n) =>
        n >= 2 && Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i != 0);

    // Manual
    static bool ManualWay(int n) {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) return false;
        return true;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(7));    // True
        Console.WriteLine("Manual: " + ManualWay(9));  // False
        Console.WriteLine("Manual: " + ManualWay(97)); // True
    }
}
```

---

### Q42. Find the Nth Fibonacci Number
**Logic:** Iterative (O(n) time, O(1) space). Avoid naive recursion O(2ⁿ).
**Example:** N=10 → `55`

```csharp
using System;
using System.Linq;

class Q42 {
    // LINQ (generate sequence)
    static int LinqWay(int n) =>
        Enumerable.Range(0, n + 1)
            .Aggregate((0, 1), (state, _) => (state.Item2, state.Item1 + state.Item2))
            .Item1;

    // Manual
    static int ManualWay(int n) {
        if (n <= 1) return n;
        int a = 0, b = 1;
        for (int i = 2; i <= n; i++) { int t = a + b; a = b; b = t; }
        return b;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(10));   // 55
        Console.WriteLine("Manual: " + ManualWay(10)); // 55
    }
}
```

---

### Q43. Print All Primes up to N (Sieve of Eratosthenes)
**Logic:** Mark all multiples of each prime as composite.
**Example:** N=20 → `2 3 5 7 11 13 17 19`

```csharp
using System;
using System.Linq;

class Q43 {
    // LINQ
    static int[] LinqWay(int n) {
        bool[] sieve = new bool[n + 1];
        for (int i = 2; i * i <= n; i++)
            if (!sieve[i]) for (int j = i * i; j <= n; j += i) sieve[j] = true;
        return Enumerable.Range(2, n - 1).Where(i => !sieve[i]).ToArray();
    }

    // Manual
    static int[] ManualWay(int n) {
        bool[] isComposite = new bool[n + 1];
        for (int i = 2; i * i <= n; i++)
            if (!isComposite[i])
                for (int j = i * i; j <= n; j += i) isComposite[j] = true;
        var primes = new System.Collections.Generic.List<int>();
        for (int i = 2; i <= n; i++) if (!isComposite[i]) primes.Add(i);
        return primes.ToArray();
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + string.Join(" ", LinqWay(20)));
        Console.WriteLine("Manual: " + string.Join(" ", ManualWay(20)));
        // 2 3 5 7 11 13 17 19
    }
}
```

---

### Q44. Find GCD and LCM of Two Numbers
**Logic:** GCD via Euclidean algorithm. LCM = (a * b) / GCD.
**Example:** `12, 18` → GCD=6, LCM=36

```csharp
using System;

class Q44 {
    // Both LINQ and Manual use same algorithm — key is the formula
    static int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);
    static int LCM(int a, int b) => a / GCD(a, b) * b; // divide first to avoid overflow

    // Iterative GCD
    static int GCDManual(int a, int b) {
        while (b != 0) { int t = b; b = a % b; a = t; }
        return a;
    }

    static void Main() {
        Console.WriteLine("GCD(12,18) = " + GCD(12, 18));        // 6
        Console.WriteLine("LCM(12,18) = " + LCM(12, 18));        // 36
        Console.WriteLine("GCD Manual = " + GCDManual(48, 18));  // 6
    }
}
```

---

### Q45. Check if a Number is Armstrong (Narcissistic)
**Logic:** Sum of each digit raised to the power of total digits = number.
**Example:** `153` = 1³+5³+3³ = 153 → `true`

```csharp
using System;
using System.Linq;

class Q45 {
    // LINQ
    static bool LinqWay(int n) {
        string s = n.ToString();
        return s.Sum(c => (int)Math.Pow(c - '0', s.Length)) == n;
    }

    // Manual
    static bool ManualWay(int n) {
        string s = n.ToString();
        int sum = 0;
        foreach (char c in s) sum += (int)Math.Pow(c - '0', s.Length);
        return sum == n;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(153));   // True
        Console.WriteLine("Manual: " + ManualWay(9474)); // True (9⁴+4⁴+7⁴+4⁴)
        Console.WriteLine("Manual: " + ManualWay(100));  // False
    }
}
```

---

### Q46. Count Trailing Zeros in N Factorial
**Logic:** Each zero = factor of 10 = 2×5. Count 5s in prime factorization (there are always enough 2s).
**Example:** `25!` → `6`

```csharp
using System;

class Q46 {
    // Both ways use same math
    static int LinqWay(int n) {
        int count = 0;
        for (int p = 5; p <= n; p *= 5) count += n / p;
        return count;
    }

    static int ManualWay(int n) {
        int count = 0;
        while (n >= 5) { n /= 5; count += n; }
        return count;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(25));   // 6
        Console.WriteLine("Manual: " + ManualWay(100)); // 24
    }
}
```

---

### Q47. Find All Factors of a Number
**Logic:** Check divisibility from 1 to √n, add both i and n/i.
**Example:** `12` → `[1,2,3,4,6,12]`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q47 {
    // LINQ
    static List<int> LinqWay(int n) =>
        Enumerable.Range(1, n).Where(i => n % i == 0).ToList();

    // Manual (O(√n))
    static List<int> ManualWay(int n) {
        var factors = new List<int>();
        for (int i = 1; i * i <= n; i++) {
            if (n % i == 0) {
                factors.Add(i);
                if (i != n / i) factors.Add(n / i);
            }
        }
        factors.Sort();
        return factors;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + string.Join(",", LinqWay(12)));   // 1,2,3,4,6,12
        Console.WriteLine("Manual: " + string.Join(",", ManualWay(12))); // 1,2,3,4,6,12
    }
}
```

---

### Q48. Check if a Number is a Power of 2
**Logic:** Powers of 2 in binary have exactly one bit set. `n & (n-1)` clears that bit → 0.
**Example:** `16` → `true`, `18` → `false`

```csharp
using System;

class Q48 {
    // Bitwise (elegant, same for both)
    static bool LinqWay(int n) => n > 0 && (n & (n - 1)) == 0;

    // Manual (divide by 2 repeatedly)
    static bool ManualWay(int n) {
        if (n <= 0) return false;
        while (n % 2 == 0) n /= 2;
        return n == 1;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(16));  // True
        Console.WriteLine("Manual: " + ManualWay(18)); // False
        Console.WriteLine("Manual: " + ManualWay(1));  // True
    }
}
```

---

### Q49. Check if a Year is a Leap Year
**Logic:** Divisible by 4 AND (not divisible by 100 OR divisible by 400).
**Example:** `2024`→`true`, `1900`→`false`, `2000`→`true`

```csharp
using System;

class Q49 {
    static bool LinqWay(int y) => (y % 4 == 0 && y % 100 != 0) || y % 400 == 0;
    static bool ManualWay(int y) {
        if (y % 400 == 0) return true;
        if (y % 100 == 0) return false;
        return y % 4 == 0;
    }

    static void Main() {
        Console.WriteLine("2024: " + LinqWay(2024));  // True
        Console.WriteLine("1900: " + LinqWay(1900));  // False
        Console.WriteLine("2000: " + ManualWay(2000)); // True
    }
}
```

---

### Q50. Find the Number of Ways to Climb N Stairs (1 or 2 Steps)
**Logic:** Same pattern as Fibonacci. Ways(n) = Ways(n-1) + Ways(n-2).
**Example:** N=5 → `8`

```csharp
using System;

class Q50 {
    // Memoized (top-down)
    static int LinqWay(int n, int[] memo = null) {
        memo ??= new int[n + 1];
        if (n <= 2) return n;
        if (memo[n] != 0) return memo[n];
        memo[n] = LinqWay(n - 1, memo) + LinqWay(n - 2, memo);
        return memo[n];
    }

    // Manual (bottom-up DP)
    static int ManualWay(int n) {
        if (n <= 2) return n;
        int a = 1, b = 2;
        for (int i = 3; i <= n; i++) { int t = a + b; a = b; b = t; }
        return b;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(5));   // 8
        Console.WriteLine("Manual: " + ManualWay(10)); // 89
    }
}
```

---

### Q51. Check if a Number is a Perfect Number
**Logic:** Sum of proper divisors (excluding itself) equals the number.
**Example:** `28` → `1+2+4+7+14=28` → `true`

```csharp
using System;
using System.Linq;

class Q51 {
    // LINQ
    static bool LinqWay(int n) =>
        n > 1 && Enumerable.Range(1, n / 2).Where(i => n % i == 0).Sum() == n;

    // Manual
    static bool ManualWay(int n) {
        if (n < 2) return false;
        int sum = 1;
        for (int i = 2; i * i <= n; i++) {
            if (n % i == 0) {
                sum += i;
                if (i != n / i) sum += n / i;
            }
        }
        return sum == n;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(28));    // True
        Console.WriteLine("Manual: " + ManualWay(496)); // True
        Console.WriteLine("Manual: " + ManualWay(12));  // False
    }
}
```

---

### Q52. Fast Power (x^n) Without Math.Pow
**Logic:** Binary exponentiation — halve the exponent each step.
**Example:** `2^10` → `1024`

```csharp
using System;

class Q52 {
    // Recursive (clean)
    static double LinqWay(double x, int n) {
        if (n == 0) return 1;
        if (n < 0) { x = 1 / x; n = -n; }
        return n % 2 == 0 ? LinqWay(x * x, n / 2) : x * LinqWay(x * x, n / 2);
    }

    // Iterative
    static double ManualWay(double x, int n) {
        if (n < 0) { x = 1 / x; n = -n; }
        double result = 1;
        while (n > 0) {
            if (n % 2 == 1) result *= x;
            x *= x;
            n /= 2;
        }
        return result;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(2, 10));   // 1024
        Console.WriteLine("Manual: " + ManualWay(3, 5));  // 243
    }
}
```

---

### Q53. Find the Sum of Digits Until Single Digit (Digital Root)
**Logic:** Repeatedly sum digits until result is 0–9. Math shortcut: result = 1 + (n-1)%9.
**Example:** `9875` → 9+8+7+5=29 → 2+9=11 → 1+1=2

```csharp
using System;
using System.Linq;

class Q53 {
    // LINQ (uses math formula)
    static int LinqWay(int n) => n == 0 ? 0 : 1 + (n - 1) % 9;

    // Manual (repeated digit sum)
    static int ManualWay(int n) {
        while (n >= 10) {
            int sum = 0;
            while (n > 0) { sum += n % 10; n /= 10; }
            n = sum;
        }
        return n;
    }

    static void Main() {
        Console.WriteLine("LINQ  : " + LinqWay(9875));   // 2
        Console.WriteLine("Manual: " + ManualWay(9875)); // 2
        Console.WriteLine("Manual: " + ManualWay(0));    // 0
    }
}
```

---

### Q54. Convert Decimal to Binary, Octal, and Hex
**Logic:** Convert.ToString(n, base) handles all. Manual: use modulo of target base.
**Example:** `255` → Binary:`11111111`, Octal:`377`, Hex:`FF`

```csharp
using System;

class Q54 {
    // Built-in
    static void LinqWay(int n) {
        Console.WriteLine($"Binary : {Convert.ToString(n, 2)}");
        Console.WriteLine($"Octal  : {Convert.ToString(n, 8)}");
        Console.WriteLine($"Hex    : {Convert.ToString(n, 16).ToUpper()}");
    }

    // Manual (custom base converter)
    static string ManualConvert(int n, int baseN) {
        string digits = "0123456789ABCDEF";
        string result = "";
        while (n > 0) { result = digits[n % baseN] + result; n /= baseN; }
        return result == "" ? "0" : result;
    }

    static void Main() {
        LinqWay(255);
        Console.WriteLine("Manual Binary : " + ManualConvert(255, 2));  // 11111111
        Console.WriteLine("Manual Hex    : " + ManualConvert(255, 16)); // FF
    }
}
```

---

### Q55. Calculate Simple and Compound Interest
**Logic:** SI = P×R×T/100; CI = P×(1+R/100)^T - P.
**Example:** P=1000, R=5%, T=2yr → SI=100, CI=102.5

```csharp
using System;

class Q55 {
    static double SimpleInterest(double p, double r, double t) => p * r * t / 100;
    static double CompoundInterest(double p, double r, double t) =>
        p * Math.Pow(1 + r / 100, t) - p;

    static void Main() {
        Console.WriteLine($"SI  = {SimpleInterest(1000, 5, 2):F2}");   // 100.00
        Console.WriteLine($"CI  = {CompoundInterest(1000, 5, 2):F2}"); // 102.50
        Console.WriteLine($"Diff= {CompoundInterest(1000, 5, 2) - SimpleInterest(1000, 5, 2):F2}"); // 2.50
    }
}
```

---

---

# SECTION 4 — LINQ DEEP DIVE (Q56–Q70)

---

### Q56. Group Employees by Department & Count Each
**Logic:** GroupBy department, select key + count.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q56 {
    record Employee(string Name, string Dept, int Salary);

    static void Main() {
        var employees = new List<Employee> {
            new("Alice", "IT", 80000), new("Bob", "HR", 60000),
            new("Carol", "IT", 90000), new("Dave", "HR", 55000),
            new("Eve",   "IT", 70000)
        };

        // LINQ
        var grouped = employees
            .GroupBy(e => e.Dept)
            .Select(g => new { Dept = g.Key, Count = g.Count(), AvgSalary = g.Average(e => e.Salary) });

        foreach (var g in grouped)
            Console.WriteLine($"{g.Dept}: {g.Count} employees, avg salary {g.AvgSalary:F0}");

        // Manual
        var manual = new Dictionary<string, (int count, double total)>();
        foreach (var e in employees) {
            if (!manual.ContainsKey(e.Dept)) manual[e.Dept] = (0, 0);
            manual[e.Dept] = (manual[e.Dept].count + 1, manual[e.Dept].total + e.Salary);
        }
        Console.WriteLine("--- Manual ---");
        foreach (var kv in manual)
            Console.WriteLine($"{kv.Key}: {kv.Value.count} employees, avg {kv.Value.total / kv.Value.count:F0}");
    }
}
```

---

### Q57. Find Top 3 Distinct Salaries
**Logic:** Distinct salaries, order descending, take 3.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q57 {
    record Employee(string Name, int Salary);

    static void Main() {
        var employees = new List<Employee> {
            new("A", 90000), new("B", 90000), new("C", 70000),
            new("D", 80000), new("E", 60000), new("F", 50000)
        };

        // LINQ
        var top3 = employees.Select(e => e.Salary).Distinct()
                             .OrderByDescending(s => s).Take(3).ToList();
        Console.WriteLine("LINQ Top 3: " + string.Join(", ", top3)); // 90000, 80000, 70000

        // Manual
        var distinct = new HashSet<int>();
        var sorted = new List<int>();
        foreach (var e in employees.OrderByDescending(e => e.Salary))
            if (distinct.Add(e.Salary)) sorted.Add(e.Salary);
        Console.WriteLine("Manual Top 3: " + string.Join(", ", sorted.Take(3)));
    }
}
```

---

### Q58. Find Employees Earning Above Department Average
**Logic:** Join each employee to their department average, filter.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q58 {
    record Employee(string Name, string Dept, int Salary);

    static void Main() {
        var employees = new List<Employee> {
            new("Alice", "IT", 80000), new("Bob", "IT", 60000),
            new("Carol", "HR", 55000), new("Dave", "HR", 75000)
        };

        // LINQ
        var result = employees
            .GroupBy(e => e.Dept)
            .SelectMany(g => {
                double avg = g.Average(e => e.Salary);
                return g.Where(e => e.Salary > avg).Select(e => new { e.Name, e.Dept, e.Salary, DeptAvg = avg });
            });

        foreach (var r in result)
            Console.WriteLine($"{r.Name} ({r.Dept}): {r.Salary} > dept avg {r.DeptAvg:F0}");

        // Manual
        var deptAvg = new Dictionary<string, double>();
        var deptCount = new Dictionary<string, int>();
        foreach (var e in employees) {
            deptAvg[e.Dept] = deptAvg.GetValueOrDefault(e.Dept) + e.Salary;
            deptCount[e.Dept] = deptCount.GetValueOrDefault(e.Dept) + 1;
        }
        Console.WriteLine("--- Manual ---");
        foreach (var e in employees)
            if (e.Salary > deptAvg[e.Dept] / deptCount[e.Dept])
                Console.WriteLine($"{e.Name} earns above dept avg");
    }
}
```

---

### Q59. Flatten and Deduplicate a List of Lists
**Logic:** SelectMany to flatten, Distinct to remove duplicates.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q59 {
    static void Main() {
        var nested = new List<List<int>> {
            new() { 1, 2, 3 }, new() { 2, 3, 4 }, new() { 4, 5, 6 }
        };

        // LINQ
        var flat = nested.SelectMany(x => x).Distinct().OrderBy(x => x).ToList();
        Console.WriteLine("LINQ  : " + string.Join(",", flat)); // 1,2,3,4,5,6

        // Manual
        var seen = new HashSet<int>();
        var manual = new List<int>();
        foreach (var list in nested)
            foreach (int x in list)
                if (seen.Add(x)) manual.Add(x);
        manual.Sort();
        Console.WriteLine("Manual: " + string.Join(",", manual));
    }
}
```

---

### Q60. Zip Two Lists into a Dictionary
**Logic:** Zip pairs corresponding elements. ToDictionary makes it a map.

```csharp
using System;
using System.Linq;
using System.Collections.Generic;

class Q60 {
    static void Main() {
        string[] keys = { "name", "age", "city" };
        string[] values = { "Alice", "30", "NYC" };

        // LINQ
        var dict = keys.Zip(values, (k, v) => new { k, v })
                        .ToDictionary(x => x.k, x => x.v);
        foreach (var kv in dict) Console.WriteLine($"{kv.Key} = {kv.Value}");

        // Manual
        var manual = new Dictionary<string, string>();
        for (int i = 0; i < Math.Min(keys.Length, values.Length); i++)
            manual[keys[i]] = values[i];
        Console.WriteLine("Manual: " + string.Join(", ", manual.Select(kv => $"{kv.Key}={kv.Value}")));
    }
}
```

---

### Q61. Find Elements Present in First List but NOT in Second (Except)
**Logic:** Set difference. Elements in A that are not in B.

```csharp
using System;
using System.Linq;
using System.Collections.Generic;

class Q61 {
    static void Main() {
        int[] a = { 1, 2, 3, 4, 5 };
        int[] b = { 3, 4, 5, 6, 7 };

        // LINQ
        var onlyInA = a.Except(b).ToArray();
        Console.WriteLine("LINQ  : " + string.Join(",", onlyInA)); // 1,2

        // Manual
        var setB = new HashSet<int>(b);
        var manual = new List<int>();
        foreach (int x in a) if (!setB.Contains(x)) manual.Add(x);
        Console.WriteLine("Manual: " + string.Join(",", manual)); // 1,2
    }
}
```

---

### Q62. Sort a Dictionary by Value (Descending)
**Logic:** OrderByDescending on values, rebuild dictionary.

```csharp
using System;
using System.Linq;
using System.Collections.Generic;

class Q62 {
    static void Main() {
        var scores = new Dictionary<string, int> {
            ["Alice"] = 85, ["Bob"] = 92, ["Carol"] = 78, ["Dave"] = 92
        };

        // LINQ
        var sorted = scores.OrderByDescending(kv => kv.Value)
                            .ThenBy(kv => kv.Key); // tie-break by name
        foreach (var kv in sorted)
            Console.WriteLine($"{kv.Key}: {kv.Value}");

        // Manual
        var list = new List<KeyValuePair<string, int>>(scores);
        list.Sort((a, b) => b.Value != a.Value ? b.Value.CompareTo(a.Value) : a.Key.CompareTo(b.Key));
        Console.WriteLine("Manual: " + string.Join(", ", list.Select(kv => $"{kv.Key}:{kv.Value}")));
    }
}
```

---

### Q63. Partition a List into Two Groups Using a Predicate
**Logic:** ToLookup or GroupBy with bool key. Cleaner than two Where calls.

```csharp
using System;
using System.Linq;
using System.Collections.Generic;

class Q63 {
    static void Main() {
        int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // LINQ
        var lookup = numbers.ToLookup(n => n % 2 == 0 ? "even" : "odd");
        Console.WriteLine("Evens: " + string.Join(",", lookup["even"]));
        Console.WriteLine("Odds : " + string.Join(",", lookup["odd"]));

        // Manual
        var evens = new List<int>(); var odds = new List<int>();
        foreach (int n in numbers) { if (n % 2 == 0) evens.Add(n); else odds.Add(n); }
        Console.WriteLine("Manual Evens: " + string.Join(",", evens));
    }
}
```

---

### Q64. Check if All / Any / None Elements Satisfy a Condition
**Logic:** LINQ All/Any/None are the most readable ways. Manual uses early exit loops.

```csharp
using System;
using System.Linq;

class Q64 {
    static void Main() {
        int[] nums = { 2, 4, 6, 8, 10 };

        // LINQ
        Console.WriteLine("All even  : " + nums.All(n => n % 2 == 0));     // True
        Console.WriteLine("Any > 9   : " + nums.Any(n => n > 9));          // True
        Console.WriteLine("None odd  : " + !nums.Any(n => n % 2 != 0));    // True

        // Manual
        bool allEven = true;
        foreach (int n in nums) if (n % 2 != 0) { allEven = false; break; }
        Console.WriteLine("Manual All even: " + allEven); // True
    }
}
```

---

### Q65. Build a Frequency Map and Find Top N Most Common
**Logic:** GroupBy + OrderByDescending + Take(N).

```csharp
using System;
using System.Linq;
using System.Collections.Generic;

class Q65 {
    static List<(string word, int count)> LinqWay(string text, int n) =>
        text.ToLower().Split(' ')
            .GroupBy(w => w)
            .OrderByDescending(g => g.Count())
            .Take(n)
            .Select(g => (g.Key, g.Count()))
            .ToList();

    static List<(string, int)> ManualWay(string text, int n) {
        var freq = new Dictionary<string, int>();
        foreach (var w in text.ToLower().Split(' '))
            freq[w] = freq.GetValueOrDefault(w) + 1;
        var sorted = new List<(string, int)>();
        foreach (var kv in freq) sorted.Add((kv.Key, kv.Value));
        sorted.Sort((a, b) => b.Item2.CompareTo(a.Item2));
        return sorted.Take(n).ToList();
    }

    static void Main() {
        string text = "the cat sat on the mat the cat";
        var r1 = LinqWay(text, 3);
        Console.WriteLine("LINQ  : " + string.Join(", ", r1.Select(x => $"{x.word}:{x.count}")));
        var r2 = ManualWay(text, 3);
        Console.WriteLine("Manual: " + string.Join(", ", r2.Select(x => $"{x.Item1}:{x.Item2}")));
    }
}
```

---

### Q66. Find Employees with Both Highest and Lowest Salary
**Logic:** MinBy and MaxBy (C# 6+). Or OrderBy + First/Last.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q66 {
    record Employee(string Name, int Salary);

    static void Main() {
        var employees = new List<Employee> {
            new("Alice", 80000), new("Bob", 45000), new("Carol", 95000), new("Dave", 60000)
        };

        // LINQ
        var highest = employees.MaxBy(e => e.Salary);
        var lowest  = employees.MinBy(e => e.Salary);
        Console.WriteLine($"LINQ Highest: {highest!.Name} ({highest.Salary})");
        Console.WriteLine($"LINQ Lowest : {lowest!.Name} ({lowest.Salary})");

        // Manual
        Employee maxE = employees[0], minE = employees[0];
        foreach (var e in employees) {
            if (e.Salary > maxE.Salary) maxE = e;
            if (e.Salary < minE.Salary) minE = e;
        }
        Console.WriteLine($"Manual Highest: {maxE.Name}, Lowest: {minE.Name}");
    }
}
```

---

### Q67. Cross-Join Two Lists (All Combinations)
**Logic:** SelectMany on one list for each element of the other — Cartesian product.

```csharp
using System;
using System.Linq;

class Q67 {
    static void Main() {
        string[] colors = { "Red", "Blue" };
        string[] sizes  = { "S", "M", "L" };

        // LINQ
        var combos = colors.SelectMany(c => sizes.Select(s => $"{c}-{s}"));
        Console.WriteLine("LINQ  : " + string.Join(", ", combos));

        // Manual
        foreach (var c in colors)
            foreach (var s in sizes)
                Console.Write($"{c}-{s} ");
        Console.WriteLine();
    }
}
```

---

### Q68. Use Aggregate to Build a Custom Summary String
**Logic:** Aggregate is LINQ's fold/reduce — powerful for custom accumulations.

```csharp
using System;
using System.Linq;

class Q68 {
    static void Main() {
        string[] words = { "C#", "is", "awesome", "and", "fast" };

        // LINQ Aggregate (custom separator)
        string sentence = words.Aggregate((acc, w) => acc + " " + w);
        Console.WriteLine("Sentence: " + sentence);

        // Aggregate with seed and result selector
        var stats = new[] { 5, 3, 8, 1, 9, 2 }.Aggregate(
            seed: (min: int.MaxValue, max: int.MinValue, sum: 0),
            func: (acc, x) => (Math.Min(acc.min, x), Math.Max(acc.max, x), acc.sum + x),
            resultSelector: acc => $"Min={acc.min}, Max={acc.max}, Sum={acc.sum}"
        );
        Console.WriteLine(stats); // Min=1, Max=9, Sum=28
    }
}
```

---

### Q69. Find Intersection and Union of Two Lists
**Logic:** Intersect (common elements) and Union (all unique elements).

```csharp
using System;
using System.Linq;
using System.Collections.Generic;

class Q69 {
    static void Main() {
        int[] a = { 1, 2, 3, 4, 5 };
        int[] b = { 3, 4, 5, 6, 7 };

        // LINQ
        Console.WriteLine("Intersection: " + string.Join(",", a.Intersect(b))); // 3,4,5
        Console.WriteLine("Union       : " + string.Join(",", a.Union(b)));      // 1,2,3,4,5,6,7

        // Manual
        var setA = new HashSet<int>(a);
        var intersect = new List<int>(); var union = new HashSet<int>(a);
        foreach (int x in b) { if (setA.Contains(x)) intersect.Add(x); union.Add(x); }
        Console.WriteLine("Manual Intersect: " + string.Join(",", intersect));
        Console.WriteLine("Manual Union    : " + string.Join(",", union.OrderBy(x => x)));
    }
}
```

---

### Q70. Group Words by First Letter (Dictionary of Lists)
**Logic:** GroupBy first char, project to Dictionary<char, List<string>>.

```csharp
using System;
using System.Linq;
using System.Collections.Generic;

class Q70 {
    static void Main() {
        string[] words = { "apple", "ant", "bat", "banana", "cherry", "cat", "avocado" };

        // LINQ
        var grouped = words.GroupBy(w => w[0])
                           .ToDictionary(g => g.Key, g => g.ToList());
        foreach (var kv in grouped.OrderBy(kv => kv.Key))
            Console.WriteLine($"'{kv.Key}': {string.Join(", ", kv.Value)}");

        // Manual
        var manual = new Dictionary<char, List<string>>();
        foreach (var w in words) {
            if (!manual.ContainsKey(w[0])) manual[w[0]] = new List<string>();
            manual[w[0]].Add(w);
        }
        Console.WriteLine("Manual 'a': " + string.Join(", ", manual['a']));
    }
}
```

---

---

# SECTION 5 — DATA STRUCTURES (Q71–Q80)

---

### Q71. Implement a Min Stack (O(1) getMin)
**Logic:** Maintain a second stack tracking minimums. Only push to minStack if value ≤ current min.

```csharp
using System;
using System.Collections.Generic;

class Q71 {
    class MinStack {
        Stack<int> stack = new(), minStack = new();

        public void Push(int val) {
            stack.Push(val);
            if (minStack.Count == 0 || val <= minStack.Peek()) minStack.Push(val);
        }
        public int Pop() {
            int val = stack.Pop();
            if (val == minStack.Peek()) minStack.Pop();
            return val;
        }
        public int GetMin() => minStack.Peek();
        public int Peek() => stack.Peek();
    }

    static void Main() {
        var ms = new MinStack();
        ms.Push(5); ms.Push(3); ms.Push(7); ms.Push(2); ms.Push(6);
        Console.WriteLine("Min: " + ms.GetMin()); // 2
        ms.Pop(); ms.Pop();
        Console.WriteLine("Min: " + ms.GetMin()); // 3
    }
}
```

---

### Q72. Implement a Queue Using Two Stacks
**Logic:** Enqueue to inbox. To dequeue, if outbox is empty, flush inbox to outbox (reverses order → FIFO).

```csharp
using System;
using System.Collections.Generic;

class Q72 {
    class MyQueue {
        Stack<int> inbox = new(), outbox = new();

        public void Enqueue(int val) => inbox.Push(val);

        public int Dequeue() {
            if (outbox.Count == 0)
                while (inbox.Count > 0) outbox.Push(inbox.Pop());
            if (outbox.Count == 0) throw new Exception("Queue is empty");
            return outbox.Pop();
        }

        public int Peek() {
            if (outbox.Count == 0)
                while (inbox.Count > 0) outbox.Push(inbox.Pop());
            return outbox.Peek();
        }
    }

    static void Main() {
        var q = new MyQueue();
        q.Enqueue(1); q.Enqueue(2); q.Enqueue(3);
        Console.WriteLine(q.Dequeue()); // 1
        Console.WriteLine(q.Dequeue()); // 2
        q.Enqueue(4);
        Console.WriteLine(q.Dequeue()); // 3
        Console.WriteLine(q.Peek());    // 4
    }
}
```

---

### Q73. Reverse a Linked List (Iterative + Recursive)
**Logic:** Iterative: track prev, curr, next. Recursive: reverse rest, point last to head.

```csharp
using System;

class Q73 {
    class Node { public int Val; public Node Next; public Node(int v) { Val = v; } }

    static Node BuildList(params int[] vals) {
        Node head = new(vals[0]), cur = head;
        for (int i = 1; i < vals.Length; i++) { cur.Next = new(vals[i]); cur = cur.Next; }
        return head;
    }
    static string Print(Node head) {
        string s = "";
        while (head != null) { s += head.Val + (head.Next != null ? "->" : ""); head = head.Next; }
        return s;
    }

    // Iterative
    static Node ReverseIterative(Node head) {
        Node prev = null, curr = head;
        while (curr != null) { var next = curr.Next; curr.Next = prev; prev = curr; curr = next; }
        return prev;
    }

    // Recursive
    static Node ReverseRecursive(Node head) {
        if (head?.Next == null) return head;
        Node newHead = ReverseRecursive(head.Next);
        head.Next.Next = head;
        head.Next = null;
        return newHead;
    }

    static void Main() {
        var list1 = BuildList(1, 2, 3, 4, 5);
        Console.WriteLine("Original  : " + Print(list1));
        Console.WriteLine("Iterative : " + Print(ReverseIterative(list1)));

        var list2 = BuildList(1, 2, 3, 4, 5);
        Console.WriteLine("Recursive : " + Print(ReverseRecursive(list2)));
    }
}
```

---

### Q74. Detect a Cycle in a Linked List (Floyd's Algorithm)
**Logic:** Slow pointer moves 1 step, fast moves 2. If they meet, there's a cycle.

```csharp
using System;

class Q74 {
    class Node { public int Val; public Node Next; public Node(int v) { Val = v; } }

    static bool HasCycle(Node head) {
        var slow = head; var fast = head;
        while (fast?.Next != null) {
            slow = slow.Next;
            fast = fast.Next.Next;
            if (slow == fast) return true;
        }
        return false;
    }

    static void Main() {
        // No cycle
        var n1 = new Node(1); var n2 = new Node(2); var n3 = new Node(3);
        n1.Next = n2; n2.Next = n3;
        Console.WriteLine("No cycle: " + HasCycle(n1)); // False

        // With cycle: 1->2->3->back to n2
        n3.Next = n2;
        Console.WriteLine("Has cycle: " + HasCycle(n1)); // True
    }
}
```

---

### Q75. Find the Middle Node of a Linked List
**Logic:** Slow/fast pointers. When fast reaches end, slow is at middle.

```csharp
using System;

class Q75 {
    class Node { public int Val; public Node Next; public Node(int v) { Val = v; } }

    static Node FindMiddle(Node head) {
        var slow = head; var fast = head;
        while (fast?.Next != null) { slow = slow.Next; fast = fast.Next.Next; }
        return slow;
    }

    static void Main() {
        Node head = null, cur = null;
        foreach (int v in new[] { 1, 2, 3, 4, 5 }) {
            var node = new Node(v);
            if (head == null) { head = node; cur = node; }
            else { cur.Next = node; cur = node; }
        }
        Console.WriteLine("Middle (odd list) : " + FindMiddle(head).Val); // 3

        // Even list: 1,2,3,4
        head = null; cur = null;
        foreach (int v in new[] { 1, 2, 3, 4 }) {
            var node = new Node(v);
            if (head == null) { head = node; cur = node; }
            else { cur.Next = node; cur = node; }
        }
        Console.WriteLine("Middle (even list): " + FindMiddle(head).Val); // 3 (second middle)
    }
}
```

---

### Q76. Binary Search on a Sorted Array
**Logic:** Halve the search space each step. O(log n) vs O(n) linear search.
**Example:** `[1,3,5,7,9,11]`, target=7 → index `3`

```csharp
using System;

class Q76 {
    // Iterative
    static int BinarySearchIterative(int[] arr, int target) {
        int l = 0, r = arr.Length - 1;
        while (l <= r) {
            int mid = l + (r - l) / 2; // avoids overflow vs (l+r)/2
            if (arr[mid] == target) return mid;
            if (arr[mid] < target) l = mid + 1;
            else r = mid - 1;
        }
        return -1;
    }

    // Recursive
    static int BinarySearchRecursive(int[] arr, int target, int l, int r) {
        if (l > r) return -1;
        int mid = l + (r - l) / 2;
        if (arr[mid] == target) return mid;
        return arr[mid] < target
            ? BinarySearchRecursive(arr, target, mid + 1, r)
            : BinarySearchRecursive(arr, target, l, mid - 1);
    }

    static void Main() {
        int[] arr = { 1, 3, 5, 7, 9, 11 };
        Console.WriteLine("Iterative: " + BinarySearchIterative(arr, 7));                    // 3
        Console.WriteLine("Recursive: " + BinarySearchRecursive(arr, 7, 0, arr.Length - 1)); // 3
        Console.WriteLine("Not found: " + BinarySearchIterative(arr, 4));                    // -1
    }
}
```

---

### Q77. Level-Order Traversal of a Binary Tree (BFS)
**Logic:** Queue-based. Process all nodes at current level before moving to next.

```csharp
using System;
using System.Collections.Generic;

class Q77 {
    class TreeNode { public int Val; public TreeNode Left, Right; public TreeNode(int v) { Val = v; } }

    static List<List<int>> LevelOrder(TreeNode root) {
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
                if (node.Left  != null) queue.Enqueue(node.Left);
                if (node.Right != null) queue.Enqueue(node.Right);
            }
            result.Add(level);
        }
        return result;
    }

    static void Main() {
        //       1
        //      / \
        //     2   3
        //    / \
        //   4   5
        var root = new TreeNode(1);
        root.Left = new TreeNode(2); root.Right = new TreeNode(3);
        root.Left.Left = new TreeNode(4); root.Left.Right = new TreeNode(5);

        var levels = LevelOrder(root);
        foreach (var level in levels)
            Console.WriteLine("[" + string.Join(",", level) + "]");
        // [1] [2,3] [4,5]
    }
}
```

---

### Q78. Check if a Binary Tree is Balanced
**Logic:** Check height of left and right subtrees differ by at most 1, recursively.

```csharp
using System;

class Q78 {
    class TreeNode { public int Val; public TreeNode Left, Right; public TreeNode(int v) { Val = v; } }

    static bool IsBalanced(TreeNode root) {
        int Height(TreeNode node) {
            if (node == null) return 0;
            int l = Height(node.Left);
            if (l == -1) return -1;
            int r = Height(node.Right);
            if (r == -1) return -1;
            if (Math.Abs(l - r) > 1) return -1;
            return Math.Max(l, r) + 1;
        }
        return Height(root) != -1;
    }

    static void Main() {
        // Balanced
        var root = new TreeNode(1);
        root.Left = new TreeNode(2); root.Right = new TreeNode(3);
        root.Left.Left = new TreeNode(4);
        Console.WriteLine("Balanced tree: " + IsBalanced(root)); // True

        // Unbalanced (right side empty, left deep)
        var root2 = new TreeNode(1);
        root2.Left = new TreeNode(2);
        root2.Left.Left = new TreeNode(3);
        root2.Left.Left.Left = new TreeNode(4);
        Console.WriteLine("Unbalanced tree: " + IsBalanced(root2)); // False
    }
}
```

---

### Q79. Implement Bubble, Selection, and Insertion Sort
**Logic:** Three classic O(n²) sorts. Interviewers often ask you to code these from scratch.

```csharp
using System;

class Q79 {
    static void Bubble(int[] arr) {
        for (int i = 0; i < arr.Length - 1; i++)
            for (int j = 0; j < arr.Length - 1 - i; j++)
                if (arr[j] > arr[j + 1]) (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
    }

    static void Selection(int[] arr) {
        for (int i = 0; i < arr.Length - 1; i++) {
            int minIdx = i;
            for (int j = i + 1; j < arr.Length; j++)
                if (arr[j] < arr[minIdx]) minIdx = j;
            (arr[i], arr[minIdx]) = (arr[minIdx], arr[i]);
        }
    }

    static void Insertion(int[] arr) {
        for (int i = 1; i < arr.Length; i++) {
            int key = arr[i], j = i - 1;
            while (j >= 0 && arr[j] > key) { arr[j + 1] = arr[j]; j--; }
            arr[j + 1] = key;
        }
    }

    static void Main() {
        int[] a = { 5, 3, 1, 4, 2 }; Bubble(a);
        Console.WriteLine("Bubble   : " + string.Join(",", a)); // 1,2,3,4,5

        int[] b = { 5, 3, 1, 4, 2 }; Selection(b);
        Console.WriteLine("Selection: " + string.Join(",", b)); // 1,2,3,4,5

        int[] c = { 5, 3, 1, 4, 2 }; Insertion(c);
        Console.WriteLine("Insertion: " + string.Join(",", c)); // 1,2,3,4,5
    }
}
```

---

### Q80. Implement a Simple LRU Cache
**Logic:** LinkedList (ordered by recency) + Dictionary (O(1) lookup). Evict tail on overflow.

```csharp
using System;
using System.Collections.Generic;

class Q80 {
    class LRUCache {
        int capacity;
        Dictionary<int, LinkedListNode<(int key, int val)>> map = new();
        LinkedList<(int key, int val)> list = new();

        public LRUCache(int cap) { capacity = cap; }

        public int Get(int key) {
            if (!map.ContainsKey(key)) return -1;
            var node = map[key];
            list.Remove(node); list.AddFirst(node);
            return node.Value.val;
        }

        public void Put(int key, int val) {
            if (map.ContainsKey(key)) list.Remove(map[key]);
            else if (map.Count == capacity) { map.Remove(list.Last!.Value.key); list.RemoveLast(); }
            map[key] = list.AddFirst((key, val));
        }
    }

    static void Main() {
        var cache = new LRUCache(2);
        cache.Put(1, 10); cache.Put(2, 20);
        Console.WriteLine(cache.Get(1));   // 10 (1 is now most recent)
        cache.Put(3, 30);                   // evicts 2 (least recent)
        Console.WriteLine(cache.Get(2));   // -1 (evicted)
        Console.WriteLine(cache.Get(3));   // 30
    }
}
```

---

---

# SECTION 6 — OOP & C# CONCEPTS (Q81–Q90)

---

### Q81. Singleton Pattern (Thread-Safe)
**Logic:** Private constructor + static instance + double-checked locking.

```csharp
using System;

class Q81 {
    sealed class AppConfig {
        static AppConfig _instance;
        static readonly object _lock = new();
        public string ConnectionString { get; set; } = "Server=prod;DB=main";

        AppConfig() { } // private

        public static AppConfig Instance {
            get {
                if (_instance == null)
                    lock (_lock) { _instance ??= new AppConfig(); }
                return _instance;
            }
        }
    }

    static void Main() {
        var c1 = AppConfig.Instance;
        var c2 = AppConfig.Instance;
        Console.WriteLine("Same instance: " + ReferenceEquals(c1, c2)); // True
        Console.WriteLine("Connection: " + c1.ConnectionString);
    }
}
```

---

### Q82. Factory Pattern
**Logic:** Create objects through a factory method — caller doesn't need to know the concrete type.

```csharp
using System;

class Q82 {
    interface IShape { double Area(); string Name(); }

    class Circle   : IShape { double r; public Circle(double r)   { this.r = r; }
        public double Area() => Math.PI * r * r; public string Name() => "Circle"; }
    class Rectangle : IShape { double w, h; public Rectangle(double w, double h) { this.w=w; this.h=h; }
        public double Area() => w * h; public string Name() => "Rectangle"; }
    class Triangle  : IShape { double b, h; public Triangle(double b, double h) { this.b=b; this.h=h; }
        public double Area() => 0.5 * b * h; public string Name() => "Triangle"; }

    static IShape CreateShape(string type, double a, double b = 0) => type.ToLower() switch {
        "circle"    => new Circle(a),
        "rectangle" => new Rectangle(a, b),
        "triangle"  => new Triangle(a, b),
        _ => throw new ArgumentException("Unknown: " + type)
    };

    static void Main() {
        IShape[] shapes = {
            CreateShape("circle", 5),
            CreateShape("rectangle", 4, 6),
            CreateShape("triangle", 3, 8)
        };
        foreach (var s in shapes)
            Console.WriteLine($"{s.Name()}: area = {s.Area():F2}");
    }
}
```

---

### Q83. Observer Pattern (Event-Driven)
**Logic:** Publisher raises events. Multiple subscribers listen without tight coupling.

```csharp
using System;
using System.Collections.Generic;

class Q83 {
    // Using delegates and events
    class OrderService {
        public event Action<string> OrderPlaced;

        public void PlaceOrder(string item) {
            Console.WriteLine($"Order placed: {item}");
            OrderPlaced?.Invoke(item);
        }
    }

    class EmailService {
        public void OnOrderPlaced(string item) =>
            Console.WriteLine($"[Email] Confirmation sent for: {item}");
    }

    class InventoryService {
        public void OnOrderPlaced(string item) =>
            Console.WriteLine($"[Inventory] Stock reduced for: {item}");
    }

    static void Main() {
        var orders = new OrderService();
        var email = new EmailService();
        var inventory = new InventoryService();

        orders.OrderPlaced += email.OnOrderPlaced;
        orders.OrderPlaced += inventory.OnOrderPlaced;

        orders.PlaceOrder("Laptop");
        orders.PlaceOrder("Mouse");
    }
}
```

---

### Q84. Builder Pattern (Fluent Interface)
**Logic:** Construct complex objects step-by-step. Each method returns `this` for chaining.

```csharp
using System;
using System.Collections.Generic;

class Q84 {
    class SqlQuery {
        public string Table { get; set; }
        public List<string> Columns { get; set; } = new();
        public string WhereClause { get; set; }
        public string OrderByColumn { get; set; }
        public int Limit { get; set; } = -1;

        public override string ToString() {
            var cols = Columns.Count > 0 ? string.Join(", ", Columns) : "*";
            var sql = $"SELECT {cols} FROM {Table}";
            if (WhereClause != null) sql += $" WHERE {WhereClause}";
            if (OrderByColumn != null) sql += $" ORDER BY {OrderByColumn}";
            if (Limit > 0) sql += $" LIMIT {Limit}";
            return sql;
        }
    }

    class QueryBuilder {
        SqlQuery _q = new();
        public QueryBuilder From(string table)  { _q.Table = table; return this; }
        public QueryBuilder Select(params string[] cols) { _q.Columns.AddRange(cols); return this; }
        public QueryBuilder Where(string clause) { _q.WhereClause = clause; return this; }
        public QueryBuilder OrderBy(string col)  { _q.OrderByColumn = col; return this; }
        public QueryBuilder Take(int n)          { _q.Limit = n; return this; }
        public SqlQuery Build() => _q;
    }

    static void Main() {
        var query = new QueryBuilder()
            .From("Employees")
            .Select("Name", "Salary", "Department")
            .Where("Salary > 50000")
            .OrderBy("Salary")
            .Take(10)
            .Build();

        Console.WriteLine(query);
    }
}
```

---

### Q85. Demonstrate Abstract Class vs Interface (When to Use Each)
**Logic:** Abstract class = shared state + behavior. Interface = pure contract / multiple inheritance.

```csharp
using System;

class Q85 {
    // Abstract class — has shared code and state
    abstract class Animal {
        public string Name { get; }
        protected Animal(string name) { Name = name; }
        public abstract string MakeSound(); // must override
        public string Describe() => $"{Name} says: {MakeSound()}"; // shared logic
    }

    // Interface — pure contract, no implementation (pre-C#8)
    interface ISwimmable { void Swim(); }
    interface IFlyable   { void Fly();  }

    class Duck : Animal, ISwimmable, IFlyable {
        public Duck() : base("Duck") { }
        public override string MakeSound() => "Quack";
        public void Swim() => Console.WriteLine($"{Name} is swimming");
        public void Fly()  => Console.WriteLine($"{Name} is flying");
    }

    class Dog : Animal, ISwimmable {
        public Dog() : base("Dog") { }
        public override string MakeSound() => "Woof";
        public void Swim() => Console.WriteLine($"{Name} is swimming (slowly)");
    }

    static void Main() {
        Animal[] animals = { new Duck(), new Dog() };
        foreach (var a in animals) Console.WriteLine(a.Describe());

        // Polymorphism via interface
        ISwimmable[] swimmers = { new Duck(), new Dog() };
        foreach (var s in swimmers) s.Swim();
    }
}
```

---

### Q86. Extension Methods
**Logic:** Static methods in a static class with `this` as first parameter. Extend any type without subclassing.

```csharp
using System;
using System.Linq;

class Q86 {
    static class Extensions {
        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);
        public static string ToTitleCase(this string s) =>
            string.Join(" ", s.Split(' ').Select(w => w.Length > 0 ? char.ToUpper(w[0]) + w[1..].ToLower() : w));
        public static bool IsPalindrome(this string s) {
            s = s.ToLower();
            return s.SequenceEqual(s.Reverse());
        }
        public static int[] TakeLast(this int[] arr, int n) => arr.Skip(arr.Length - n).ToArray();
    }

    static void Main() {
        Console.WriteLine("".IsNullOrEmpty());                        // True
        Console.WriteLine("hello world".ToTitleCase());              // Hello World
        Console.WriteLine("racecar".IsPalindrome());                 // True
        Console.WriteLine(string.Join(",", new[]{1,2,3,4,5}.TakeLast(3))); // 3,4,5
    }
}
```

---

### Q87. Generics — Generic Repository Pattern
**Logic:** Write once, use for any type. Type parameter T makes code reusable and type-safe.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q87 {
    interface IRepository<T> {
        void Add(T item);
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Delete(int id);
    }

    class InMemoryRepository<T> : IRepository<T> where T : class {
        Dictionary<int, T> _store = new();
        int _nextId = 1;
        Func<T, int> _getId;
        Action<T, int> _setId;

        public InMemoryRepository(Func<T, int> getId, Action<T, int> setId) {
            _getId = getId; _setId = setId;
        }
        public void Add(T item) { _setId(item, _nextId); _store[_nextId++] = item; }
        public T GetById(int id) => _store.GetValueOrDefault(id);
        public IEnumerable<T> GetAll() => _store.Values;
        public void Delete(int id) => _store.Remove(id);
    }

    class Product { public int Id { get; set; } public string Name { get; set; } }

    static void Main() {
        var repo = new InMemoryRepository<Product>(p => p.Id, (p, id) => p.Id = id);
        repo.Add(new Product { Name = "Laptop" });
        repo.Add(new Product { Name = "Mouse"  });
        foreach (var p in repo.GetAll())
            Console.WriteLine($"[{p.Id}] {p.Name}");
        repo.Delete(1);
        Console.WriteLine("After delete: " + repo.GetAll().Count()); // 1
    }
}
```

---

### Q88. async/await and Task Parallelism
**Logic:** Async methods return Task. Await suspends without blocking the thread.

```csharp
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

class Q88 {
    static async Task<string> FetchUserAsync(int id) {
        await Task.Delay(100); // simulate DB/API call
        return $"User_{id}";
    }

    static async Task<int> GetOrderCountAsync(int userId) {
        await Task.Delay(80);
        return userId * 3; // fake data
    }

    static async Task Main() {
        // Sequential (slow)
        var user1 = await FetchUserAsync(1);
        var count1 = await GetOrderCountAsync(1);
        Console.WriteLine($"Sequential: {user1}, orders={count1}");

        // Parallel (faster — both run at the same time)
        var userTask   = FetchUserAsync(2);
        var orderTask  = GetOrderCountAsync(2);
        await Task.WhenAll(userTask, orderTask);
        Console.WriteLine($"Parallel: {userTask.Result}, orders={orderTask.Result}");

        // WhenAll with a list
        var tasks = new List<Task<string>>();
        for (int i = 1; i <= 5; i++) tasks.Add(FetchUserAsync(i));
        var users = await Task.WhenAll(tasks);
        Console.WriteLine("All users: " + string.Join(", ", users));
    }
}
```

---

### Q89. Custom Exception Handling with Exception Hierarchy
**Logic:** Derive from Exception. Catch specific before general. Use finally for cleanup.

```csharp
using System;

class Q89 {
    class AppException : Exception {
        public int ErrorCode { get; }
        public AppException(string msg, int code) : base(msg) { ErrorCode = code; }
    }

    class ValidationException : AppException {
        public string Field { get; }
        public ValidationException(string field, string msg)
            : base(msg, 400) { Field = field; }
    }

    class NotFoundException : AppException {
        public NotFoundException(string resource)
            : base($"{resource} not found", 404) { }
    }

    static void ProcessRequest(string action) {
        switch (action) {
            case "validate": throw new ValidationException("Email", "Email is required");
            case "notfound": throw new NotFoundException("User");
            case "error":    throw new AppException("Internal error", 500);
        }
    }

    static void Main() {
        foreach (var action in new[] { "validate", "notfound", "error", "ok" }) {
            try {
                ProcessRequest(action);
                Console.WriteLine($"'{action}' succeeded");
            }
            catch (ValidationException ex) {
                Console.WriteLine($"[{ex.ErrorCode}] Validation failed on '{ex.Field}': {ex.Message}");
            }
            catch (NotFoundException ex) {
                Console.WriteLine($"[{ex.ErrorCode}] {ex.Message}");
            }
            catch (AppException ex) {
                Console.WriteLine($"[{ex.ErrorCode}] App error: {ex.Message}");
            }
            finally {
                Console.WriteLine($"  -> cleanup done for '{action}'");
            }
        }
    }
}
```

---

### Q90. IComparable, IComparer, and Custom Sorting
**Logic:** IComparable on the class itself. IComparer for external/alternate sort strategies.

```csharp
using System;
using System.Collections.Generic;

class Q90 {
    class Employee : IComparable<Employee> {
        public string Name { get; set; }
        public int Salary  { get; set; }
        public int Age     { get; set; }

        // Default sort: by Salary ascending
        public int CompareTo(Employee other) => Salary.CompareTo(other.Salary);
        public override string ToString() => $"{Name}(salary={Salary},age={Age})";
    }

    // Alternative sort strategy: by Name
    class NameComparer : IComparer<Employee> {
        public int Compare(Employee a, Employee b) => string.Compare(a.Name, b.Name);
    }

    // Alternative: by Age descending
    class AgeDescComparer : IComparer<Employee> {
        public int Compare(Employee a, Employee b) => b.Age.CompareTo(a.Age);
    }

    static void Main() {
        var employees = new List<Employee> {
            new() { Name="Carol", Salary=90000, Age=35 },
            new() { Name="Alice", Salary=60000, Age=28 },
            new() { Name="Bob",   Salary=75000, Age=42 }
        };

        employees.Sort();
        Console.WriteLine("By Salary : " + string.Join(", ", employees));

        employees.Sort(new NameComparer());
        Console.WriteLine("By Name   : " + string.Join(", ", employees));

        employees.Sort(new AgeDescComparer());
        Console.WriteLine("By Age↓   : " + string.Join(", ", employees));
    }
}
```

---

---

# SECTION 7 — MIXED SCENARIO / REAL-WORLD (Q91–Q100)

---

### Q91. Count Islands in a Grid (DFS)
**Logic:** When '1' is found, DFS to mark all connected land as visited. Count each DFS start.

```csharp
using System;

class Q91 {
    static int CountIslands(char[,] grid) {
        int rows = grid.GetLength(0), cols = grid.GetLength(1), count = 0;

        void DFS(int r, int c) {
            if (r < 0 || r >= rows || c < 0 || c >= cols || grid[r, c] != '1') return;
            grid[r, c] = '0'; // mark visited
            DFS(r+1,c); DFS(r-1,c); DFS(r,c+1); DFS(r,c-1);
        }

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                if (grid[i, j] == '1') { DFS(i, j); count++; }

        return count;
    }

    static void Main() {
        char[,] grid1 = {
            {'1','1','0','0'},
            {'1','1','0','0'},
            {'0','0','1','0'},
            {'0','0','0','1'}
        };
        Console.WriteLine("Islands: " + CountIslands(grid1)); // 3

        char[,] grid2 = {
            {'1','1','1'},
            {'0','1','0'},
            {'1','0','1'}
        };
        Console.WriteLine("Islands: " + CountIslands(grid2)); // 3
    }
}
```

---

### Q92. Parse a CSV Line (Handle Quoted Fields)
**Logic:** State-machine approach — toggle `inQuotes` on `"`, treat `,` inside quotes as data.

```csharp
using System;
using System.Collections.Generic;
using System.Text;

class Q92 {
    static List<string> ParseCsvLine(string line) {
        var result = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in line) {
            if (c == '"') { inQuotes = !inQuotes; continue; }
            if (c == ',' && !inQuotes) {
                result.Add(current.ToString().Trim());
                current.Clear();
                continue;
            }
            current.Append(c);
        }
        result.Add(current.ToString().Trim());
        return result;
    }

    static void Main() {
        string csv = "Alice,30,\"New York, NY\",Engineer,\"C#, .NET\"";
        var fields = ParseCsvLine(csv);
        for (int i = 0; i < fields.Count; i++)
            Console.WriteLine($"[{i}] {fields[i]}");
        // [0] Alice  [1] 30  [2] New York, NY  [3] Engineer  [4] C#, .NET
    }
}
```

---

### Q93. Implement a Simple Vending Machine (State Machine)
**Logic:** State machine with explicit states. Transitions are valid only in certain states.

```csharp
using System;
using System.Collections.Generic;

class Q93 {
    enum State { Idle, HasMoney, Dispensing }

    class VendingMachine {
        State _state = State.Idle;
        decimal _balance = 0;
        Dictionary<string, (decimal price, int qty)> _items = new() {
            ["Water"] = (1.00m, 5),
            ["Chips"] = (1.50m, 3),
            ["Soda"]  = (2.00m, 4)
        };

        public void InsertCoin(decimal amount) {
            _balance += amount;
            _state = State.HasMoney;
            Console.WriteLine($"Balance: {_balance:C}");
        }

        public void Select(string item) {
            if (_state != State.HasMoney) { Console.WriteLine("Insert money first"); return; }
            if (!_items.ContainsKey(item)) { Console.WriteLine("Not found"); return; }
            var (price, qty) = _items[item];
            if (qty == 0)           { Console.WriteLine("Out of stock"); return; }
            if (_balance < price)   { Console.WriteLine($"Need {price - _balance:C} more"); return; }

            _balance -= price;
            _items[item] = (price, qty - 1);
            Console.WriteLine($"Dispensing {item}");
            if (_balance > 0) { Console.WriteLine($"Change: {_balance:C}"); _balance = 0; }
            _state = State.Idle;
        }

        public void Cancel() {
            if (_balance > 0) Console.WriteLine($"Returned: {_balance:C}");
            _balance = 0; _state = State.Idle;
        }
    }

    static void Main() {
        var vm = new VendingMachine();
        vm.Select("Chips");          // Insert money first
        vm.InsertCoin(1.00m);
        vm.Select("Chips");          // Need $0.50 more
        vm.InsertCoin(0.75m);
        vm.Select("Chips");          // Dispensing, Change: $0.25
        vm.InsertCoin(2.00m);
        vm.Select("Soda");           // Dispensing, no change
    }
}
```

---

### Q94. Find the Longest Chain of Consecutive Numbers
**Logic:** Use HashSet. For each number that is a "start" (n-1 not in set), expand the chain.
**Example:** `[100,4,200,1,3,2]` → `4` (1→2→3→4)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q94 {
    // LINQ (brute force O(n log n))
    static int LinqWay(int[] nums) {
        if (nums.Length == 0) return 0;
        var sorted = nums.Distinct().OrderBy(x => x).ToArray();
        int max = 1, cur = 1;
        for (int i = 1; i < sorted.Length; i++) {
            cur = sorted[i] == sorted[i - 1] + 1 ? cur + 1 : 1;
            if (cur > max) max = cur;
        }
        return max;
    }

    // Manual (O(n) with HashSet)
    static int ManualWay(int[] nums) {
        var set = new HashSet<int>(nums);
        int max = 0;
        foreach (int n in set) {
            if (!set.Contains(n - 1)) { // n is a chain start
                int cur = 1;
                while (set.Contains(n + cur)) cur++;
                if (cur > max) max = cur;
            }
        }
        return max;
    }

    static void Main() {
        int[] arr = { 100, 4, 200, 1, 3, 2 };
        Console.WriteLine("LINQ  : " + LinqWay(arr));   // 4
        Console.WriteLine("Manual: " + ManualWay(arr)); // 4
    }
}
```

---

### Q95. Design a Rate Limiter (Token Bucket)
**Logic:** Bucket refills tokens over time. Request is allowed only if a token is available.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

class Q95 {
    class TokenBucketRateLimiter {
        int _capacity;
        int _tokens;
        int _refillPerSecond;
        DateTime _lastRefill;
        readonly object _lock = new();

        public TokenBucketRateLimiter(int capacity, int refillPerSecond) {
            _capacity = capacity;
            _tokens = capacity;
            _refillPerSecond = refillPerSecond;
            _lastRefill = DateTime.UtcNow;
        }

        void Refill() {
            var now = DateTime.UtcNow;
            double secondsPassed = (now - _lastRefill).TotalSeconds;
            int newTokens = (int)(secondsPassed * _refillPerSecond);
            if (newTokens > 0) {
                _tokens = Math.Min(_capacity, _tokens + newTokens);
                _lastRefill = now;
            }
        }

        public bool AllowRequest(string requestId) {
            lock (_lock) {
                Refill();
                if (_tokens > 0) { _tokens--; Console.WriteLine($"[ALLOW] {requestId} | tokens left: {_tokens}"); return true; }
                Console.WriteLine($"[BLOCK] {requestId} | rate limit hit");
                return false;
            }
        }
    }

    static void Main() {
        var limiter = new TokenBucketRateLimiter(capacity: 3, refillPerSecond: 1);
        // Burst of 5 requests
        for (int i = 1; i <= 5; i++) limiter.AllowRequest($"req-{i}");
        Console.WriteLine("Waiting 2 seconds for refill...");
        Thread.Sleep(2000);
        limiter.AllowRequest("req-after-wait"); // should be allowed
    }
}
```

---

### Q96. Implement Merge Sort
**Logic:** Divide array in half recursively, merge sorted halves. O(n log n) always.

```csharp
using System;

class Q96 {
    static void MergeSort(int[] arr, int left, int right) {
        if (left >= right) return;
        int mid = left + (right - left) / 2;
        MergeSort(arr, left, mid);
        MergeSort(arr, mid + 1, right);
        Merge(arr, left, mid, right);
    }

    static void Merge(int[] arr, int left, int mid, int right) {
        int n1 = mid - left + 1, n2 = right - mid;
        int[] L = new int[n1], R = new int[n2];
        Array.Copy(arr, left, L, 0, n1);
        Array.Copy(arr, mid + 1, R, 0, n2);

        int i = 0, j = 0, k = left;
        while (i < n1 && j < n2) arr[k++] = L[i] <= R[j] ? L[i++] : R[j++];
        while (i < n1) arr[k++] = L[i++];
        while (j < n2) arr[k++] = R[j++];
    }

    static void Main() {
        int[] arr = { 38, 27, 43, 3, 9, 82, 10 };
        Console.WriteLine("Before: " + string.Join(",", arr));
        MergeSort(arr, 0, arr.Length - 1);
        Console.WriteLine("After : " + string.Join(",", arr)); // 3,9,10,27,38,43,82
    }
}
```

---

### Q97. Generate and Validate a Sudoku Board
**Logic:** Valid = no repeats in any row, column, or 3×3 box. Use HashSets per unit.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Q97 {
    static bool IsValid(char[,] board) {
        bool CheckGroup(IEnumerable<char> cells) {
            var digits = cells.Where(c => c != '.').ToList();
            return digits.Count == digits.Distinct().Count();
        }

        for (int i = 0; i < 9; i++) {
            // Row
            if (!CheckGroup(Enumerable.Range(0, 9).Select(j => board[i, j]))) return false;
            // Column
            if (!CheckGroup(Enumerable.Range(0, 9).Select(j => board[j, i]))) return false;
        }
        // 3x3 boxes
        for (int br = 0; br < 3; br++)
            for (int bc = 0; bc < 3; bc++)
                if (!CheckGroup(from r in Enumerable.Range(br * 3, 3)
                                from c in Enumerable.Range(bc * 3, 3)
                                select board[r, c])) return false;
        return true;
    }

    static void Main() {
        char[,] valid = {
            {'5','3','.','.','7','.','.','.','.'},
            {'6','.','.','1','9','5','.','.','.'},
            {'.','9','8','.','.','.','.','6','.'},
            {'8','.','.','.','6','.','.','.','3'},
            {'4','.','.','8','.','3','.','.','1'},
            {'7','.','.','.','2','.','.','.','6'},
            {'.','6','.','.','.','.','2','8','.'},
            {'.','.','.','4','1','9','.','.','5'},
            {'.','.','.','.','8','.','.','7','9'}
        };
        Console.WriteLine("Valid board: " + IsValid(valid)); // True
    }
}
```

---

### Q98. Find All Combinations That Sum to Target (Backtracking)
**Logic:** At each step, try including the current element, recurse, then exclude it.
**Example:** `[2,3,6,7]`, target=7 → `[7]`, `[2,2,3]`

```csharp
using System;
using System.Collections.Generic;

class Q98 {
    static List<List<int>> CombinationSum(int[] candidates, int target) {
        var result = new List<List<int>>();
        Array.Sort(candidates);

        void Backtrack(int start, int remaining, List<int> current) {
            if (remaining == 0) { result.Add(new List<int>(current)); return; }
            for (int i = start; i < candidates.Length; i++) {
                if (candidates[i] > remaining) break; // pruning
                current.Add(candidates[i]);
                Backtrack(i, remaining - candidates[i], current); // can reuse same element
                current.RemoveAt(current.Count - 1);
            }
        }

        Backtrack(0, target, new List<int>());
        return result;
    }

    static void Main() {
        var result = CombinationSum(new[] { 2, 3, 6, 7 }, 7);
        foreach (var combo in result)
            Console.WriteLine("[" + string.Join(",", combo) + "]");
        // [2,2,3]  [7]
    }
}
```

---

### Q99. Design a Thread-Safe Counter with Interlocked
**Logic:** Interlocked.Increment is atomic — no race conditions without lock overhead.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

class Q99 {
    class Counter {
        int _count = 0;
        public void Increment() => Interlocked.Increment(ref _count);
        public void Decrement() => Interlocked.Decrement(ref _count);
        public int Value => _count;
    }

    static void Main() {
        var counter = new Counter();
        var tasks = new Task[100];

        for (int i = 0; i < 50; i++) tasks[i]     = Task.Run(() => { for (int j = 0; j < 1000; j++) counter.Increment(); });
        for (int i = 50; i < 100; i++) tasks[i]   = Task.Run(() => { for (int j = 0; j < 500;  j++) counter.Decrement(); });

        Task.WaitAll(tasks);
        Console.WriteLine("Expected: " + (50 * 1000 - 50 * 500)); // 25000
        Console.WriteLine("Actual  : " + counter.Value);           // 25000 (always correct)
    }
}
```

---

### Q100. Implement a Mini Expression Evaluator (+ - * /)
**Logic:** Two stacks — numbers and operators. Respect operator precedence by processing `*` and `/` immediately.
**Example:** `"3+2*2"` → `7`, `"10-3*2+1"` → `5`

```csharp
using System;
using System.Collections.Generic;

class Q100 {
    static int Evaluate(string s) {
        var nums = new Stack<int>();
        var ops  = new Stack<char>();

        int ApplyOp(int b, int a, char op) => op switch {
            '+' => a + b, '-' => a - b,
            '*' => a * b, '/' => a / b,
            _ => 0
        };

        bool HigherPrecedence(char a, char b) =>
            (a == '*' || a == '/') && (b == '+' || b == '-');

        int i = 0;
        while (i < s.Length) {
            if (char.IsDigit(s[i])) {
                int num = 0;
                while (i < s.Length && char.IsDigit(s[i])) num = num * 10 + (s[i++] - '0');
                nums.Push(num);
                continue;
            }
            while (ops.Count > 0 && HigherPrecedence(ops.Peek(), s[i]))
                nums.Push(ApplyOp(nums.Pop(), nums.Pop(), ops.Pop()));
            ops.Push(s[i++]);
        }
        while (ops.Count > 0)
            nums.Push(ApplyOp(nums.Pop(), nums.Pop(), ops.Pop()));

        return nums.Pop();
    }

    static void Main() {
        Console.WriteLine("3+2*2     = " + Evaluate("3+2*2"));      // 7
        Console.WriteLine("10-3*2+1  = " + Evaluate("10-3*2+1"));   // 5
        Console.WriteLine("14/7*2-1  = " + Evaluate("14/7*2-1"));   // 3
        Console.WriteLine("100+200   = " + Evaluate("100+200"));    // 300
    }
}
```

---

---

## 📌 QUICK-REFERENCE: DAILY PRACTICE PLAN

| Day | Questions | Focus |
|---|---|---|
| 1 | Q1–Q5 | String fundamentals |
| 2 | Q6–Q10 | String patterns + Stack |
| 3 | Q11–Q15 | String + Palindrome logic |
| 4 | Q16–Q20 | String — real interview types |
| 5 | Q21–Q25 | Array — Two pointer + HashMap |
| 6 | Q26–Q30 | Array — Prefix/Sliding window |
| 7 | Q31–Q35 | Array — Rotation, Subarray, Binary |
| 8 | Q36–Q40 | Array — Partition, Products |
| 9 | Q41–Q45 | Numbers — Primes, Fibonacci, Sieve |
| 10 | Q46–Q50 | Numbers — Math tricks |
| 11 | Q51–Q55 | Numbers — Perfect, Power, Interest |
| 12 | Q56–Q60 | LINQ — GroupBy, Zip, Flatten |
| 13 | Q61–Q65 | LINQ — Set ops, Frequency |
| 14 | Q66–Q70 | LINQ — MinBy/MaxBy, Aggregate |
| 15 | Q71–Q75 | DS — Stack, Queue, LinkedList |
| 16 | Q76–Q80 | DS — Binary Search, Tree, Sort |
| 17 | Q81–Q85 | OOP — Patterns, Polymorphism |
| 18 | Q86–Q90 | OOP — Generics, async, Exceptions |
| 19 | Q91–Q95 | Real World — DFS, CSV, StateMachine |
| 20 | Q96–Q100 | Real World — Merge Sort, Backtrack |

---

## 🎯 KEY INTERVIEW TIPS FOR 5 YRS EXPERIENCE

- **Always state time/space complexity** before coding — interviewers expect this at your level.
- **LINQ first, manual second** — show you know the idiomatic way, then show you understand the internals.
- **Two-pointer and sliding window** are asked constantly for array/string problems.
- **HashMap/HashSet is your best friend** — most O(n²) → O(n) optimizations come from it.
- **Design patterns come up** — Singleton, Factory, Builder, Observer are the MNC favourites.
- **async/await + thread safety** is critical for senior roles — know Interlocked, lock, Task.WhenAll.
- **Edge cases to always mention**: null input, empty collection, single element, negative numbers, overflow.
- **SOLID principles** — be ready to discuss why your code follows them.