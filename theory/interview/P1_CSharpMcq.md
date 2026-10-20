# .NET Developer MCQ Exam
### Beginner → Intermediate | Heavy on Code / Output Prediction

> Solve every question first. Scroll to the **Answer Key** at the bottom for answers + brief explanations.
> Focus is on logical, output-prediction, and code-trace questions over pure theory.

---

## SECTION 1: C# BASICS & OUTPUT PREDICTION (Q1–Q35)

---

**Q1.** What is the output?
```csharp
int x = 5;
Console.WriteLine(x++ + " " + ++x);
```
- A) 5 7
- B) 6 7
- C) 5 6
- D) 6 6

---

**Q2.** What is the output?
```csharp
int a = 10, b = 3;
Console.Write(a / b + " " + a % b);
```
- A) 3.33 1
- B) 3 1
- C) 3.33 0.33
- D) 4 1

---

**Q3.** What is the output?
```csharp
int i = 1;
while (i < 5) { i *= 2; }
Console.Write(i);
```
- A) 4
- B) 8
- C) 16
- D) 5

---

**Q4.** What is the output?
```csharp
string s = "abc";
s += "def";
Console.Write(s.Length);
```
- A) 3
- B) 6
- C) 7
- D) Error

---

**Q5.** What is the output?
```csharp
int[] arr = { 1, 2, 3, 4 };
Console.Write(arr.Length + " " + arr[arr.Length - 1]);
```
- A) 3 4
- B) 4 4
- C) 4 3
- D) 5 4

---

**Q6.** Which access modifier makes a member accessible only within the same assembly?
- A) private
- B) protected
- C) internal
- D) public

---

**Q7.** What is the output?
```csharp
bool b = true;
Console.Write(!b || (1 == 1));
```
- A) True
- B) False
- C) 1
- D) Error

---

**Q8.** What is the output?
```csharp
int x = 5;
int y = x;
y = 10;
Console.Write(x + " " + y);
```
- A) 10 10
- B) 5 10
- C) 5 5
- D) 10 5

---

**Q9.** What is the output?
```csharp
string a = "hello";
string b = "hello";
Console.Write(object.ReferenceEquals(a, b));
```
- A) False
- B) True
- C) Compile error
- D) Runtime exception

---

**Q10.** What does the `??` operator do?
- A) Checks for null and throws an exception
- B) Returns the left operand if not null, otherwise returns the right operand
- C) Converts null to a boolean false
- D) Performs a null-safe method call

---

**Q11.** What is the output?
```csharp
int? a = null;
int b = a ?? 99;
Console.Write(b);
```
- A) 0
- B) null
- C) 99
- D) Exception

---

**Q12.** What is the output?
```csharp
int? a = null;
int? b = 5;
Console.WriteLine((a + b)?.ToString() ?? "null");
```
- A) 5
- B) null
- C) 0
- D) Throws NullReferenceException

---

**Q13.** What does the `sealed` keyword do when applied to a class?
- A) Prevents the class from being instantiated
- B) Prevents the class from being inherited
- C) Makes all members private
- D) Makes the class static

---

**Q14.** What is the difference between `ref` and `out` parameters?
- A) `ref` requires initialization before passing; `out` does not
- B) `out` requires initialization before passing; `ref` does not
- C) Both require initialization
- D) Neither requires initialization

---

**Q15.** What is the output?
```csharp
void Increment(int x) { x++; }
int n = 5;
Increment(n);
Console.Write(n);
```
- A) 5
- B) 6
- C) 0
- D) Error

---

**Q16.** What is the output?
```csharp
void Increment(ref int x) { x++; }
int n = 5;
Increment(ref n);
Console.Write(n);
```
- A) 5
- B) 6
- C) 0
- D) Error

---

**Q17.** What is the output?
```csharp
int Foo(int x) => x switch {
    < 0 => -1,
    0 => 0,
    > 0 and < 10 => 1,
    _ => 10
};
Console.Write(Foo(-5) + " " + Foo(5) + " " + Foo(50));
```
- A) -1 1 10
- B) 0 5 50
- C) -5 5 50
- D) Compile error

---

**Q18.** What is boxing?
- A) Converting a value type to a reference type (wrapping it in an object)
- B) Converting a string to an integer
- C) Wrapping a method in a delegate
- D) Casting between two reference types

---

**Q19.** Which of the following is a value type?
- A) string
- B) object
- C) struct
- D) class

---

**Q20.** What does `nameof(myVar)` return at compile time?
- A) "var"
- B) "myVar"
- C) Type of myVar
- D) Memory address

---

**Q21.** What is the output?
```csharp
public static int Test() {
    try { return 1; }
    finally { Console.Write("F"); }
}
Console.Write(Test());
```
- A) 1F
- B) F1
- C) 1
- D) Compile error

---

**Q22.** What happens when you throw an exception inside a `finally` block?
- A) The original exception is preserved
- B) The finally exception replaces the original exception
- C) Compile error
- D) The finally block is skipped

---

**Q23.** What is the difference between `throw` and `throw ex` in a catch block?
- A) Identical
- B) `throw` preserves the original stack trace; `throw ex` resets it
- C) `throw ex` preserves stack trace; `throw` resets
- D) `throw` only works for system exceptions

---

**Q24.** What is the output?
```csharp
try {
    Console.Write("A");
    throw new Exception();
} catch {
    Console.Write("B");
} finally {
    Console.Write("C");
}
```
- A) AB
- B) ABC
- C) AC
- D) BCA

---

**Q25.** What is the output?
```csharp
int x = 10;
if (x > 5)
    if (x > 20)
        Console.Write("A");
    else
        Console.Write("B");
```
- A) A
- B) B
- C) AB
- D) (no output)

---

**Q26.** What is the output?
```csharp
for (int i = 0; i < 3; i++) {
    if (i == 1) continue;
    Console.Write(i);
}
```
- A) 012
- B) 02
- C) 12
- D) 0

---

**Q27.** What is the output?
```csharp
int sum = 0;
for (int i = 1; i <= 5; i++) {
    if (i == 3) break;
    sum += i;
}
Console.Write(sum);
```
- A) 6
- B) 3
- C) 10
- D) 15

---

**Q28.** What is the output?
```csharp
string s = null;
Console.Write(s?.Length ?? -1);
```
- A) 0
- B) -1
- C) null
- D) NullReferenceException

---

**Q29.** What is the output?
```csharp
var list = new List<Action>();
for (int i = 0; i < 3; i++)
    list.Add(() => Console.Write(i));
list.ForEach(a => a());
```
- A) 012
- B) 333
- C) 000
- D) Compile error

---

**Q30.** What is the output?
```csharp
Action act = null;
for (int i = 0; i < 3; i++) {
    int local = i;
    act += () => Console.Write(local);
}
act();
```
- A) 012
- B) 333
- C) 000
- D) NullReferenceException

---

**Q31.** What does `var` do in C#?
- A) Declares a dynamic type
- B) Compile-time inferred static type
- C) A reference to a value type
- D) Declares a constant

---

**Q32.** What is the output?
```csharp
int x = 10;
object o = x;
int y = (int)o;
Console.Write(y);
```
- A) 10
- B) 0
- C) Compile error
- D) InvalidCastException

---

**Q33.** What is the output?
```csharp
string s1 = "hello";
string s2 = "HELLO";
Console.Write(s1.Equals(s2, StringComparison.OrdinalIgnoreCase));
```
- A) True
- B) False
- C) Throws
- D) null

---

**Q34.** What is the difference between `==` and `.Equals()` for strings?
- A) `==` compares references; `.Equals()` compares values
- B) For strings, both compare content (because `==` is overloaded)
- C) `.Equals()` is case-sensitive; `==` is not
- D) They are different for all types

---

**Q35.** What is the output?
```csharp
char c = 'A';
Console.Write((int)c);
```
- A) A
- B) 65
- C) 97
- D) 0

---

## SECTION 2: OOP, GENERICS & C# FEATURES (Q36–Q55)

---

**Q36.** Which keyword allows a method to be overridden in a derived class?
- A) static
- B) virtual
- C) sealed
- D) override

---

**Q37.** What is the output?
```csharp
class A { public virtual void Show() => Console.Write("A"); }
class B : A { public override void Show() => Console.Write("B"); }
A obj = new B();
obj.Show();
```
- A) A
- B) B
- C) AB
- D) Compile error

---

**Q38.** What is the output?
```csharp
class A { public void Show() => Console.Write("A"); }
class B : A { public new void Show() => Console.Write("B"); }
A obj = new B();
obj.Show();
```
- A) A
- B) B
- C) AB
- D) Compile error

---

**Q39.** Can an abstract class have a constructor?
- A) No, abstract classes cannot have constructors
- B) Yes, called by derived classes during instantiation
- C) Only if it's parameterless
- D) Only static constructors

---

**Q40.** What is the difference between abstract class and interface (C# 8+)?
- A) Both are identical
- B) Abstract class can have state and constructors; interface can have default methods but no instance state
- C) Interfaces support multiple inheritance; abstract classes don't (true)
- D) Both B and C

---

**Q41.** What is the output?
```csharp
interface IShape { void Draw(); }
class Circle : IShape { public void Draw() => Console.Write("Circle"); }
IShape s = new Circle();
s.Draw();
```
- A) Circle
- B) IShape
- C) Compile error
- D) (no output)

---

**Q42.** A `Func<int, int, bool>` delegate represents:
- A) A function taking two booleans returning an int
- B) A function taking two ints returning a bool
- C) A function taking a bool and returning two ints
- D) A function with no parameters

---

**Q43.** What does `params` keyword do?
- A) Makes a parameter optional
- B) Allows a method to accept a variable number of arguments as an array
- C) Passes a parameter by reference
- D) Declares named parameters

---

**Q44.** What is the output?
```csharp
int Add(params int[] nums) {
    int sum = 0;
    foreach (var n in nums) sum += n;
    return sum;
}
Console.Write(Add(1, 2, 3, 4));
```
- A) 10
- B) 6
- C) 4
- D) Error

---

**Q45.** What is the result of `typeof(int) == typeof(Int32)`?
- A) False
- B) True — `int` is an alias for `System.Int32`
- C) Compile error
- D) Runtime exception

---

**Q46.** What is the output?
```csharp
class Box<T> { public T Value; }
var b = new Box<int> { Value = 42 };
Console.Write(b.Value);
```
- A) 42
- B) 0
- C) Compile error
- D) null

---

**Q47.** What does `nameof()` return?
- A) The full namespace
- B) The compile-time string name of a variable/type/member
- C) The runtime type name
- D) The assembly name

---

**Q48.** What is a `partial` class?
- A) A class that cannot be fully instantiated
- B) A class definition split across multiple files
- C) A class with abstract methods
- D) A sealed class

---

**Q49.** What is the output?
```csharp
record Point(int X, int Y);
var p1 = new Point(1, 2);
var p2 = new Point(1, 2);
Console.Write(p1 == p2);
```
- A) True
- B) False
- C) Throws
- D) Compile error

---

**Q50.** What is the output?
```csharp
record Point(int X, int Y);
var p1 = new Point(1, 2);
var p2 = p1 with { Y = 3 };
Console.Write(p1.X + " " + p2.Y);
```
- A) 1 3
- B) 1 2
- C) 0 3
- D) Compile error

---

**Q51.** What does `init` accessor do?
- A) Makes the property write-only
- B) Allows setting only during object construction / initializer
- C) Initializes the property to default
- D) Marks it as ignored by serialisers

---

**Q52.** What is a nullable value type in C#?
- A) A reference type that holds null
- B) A value type wrapped with `Nullable<T>` that can hold null
- C) A string defaulting to null
- D) An interface returning null

---

**Q53.** What is the output?
```csharp
int? a = null;
Console.Write(a.HasValue);
```
- A) True
- B) False
- C) null
- D) Throws

---

**Q54.** What does the `in` parameter modifier do (C# 7.2+)?
- A) Passes by value
- B) Passes by read-only reference (caller's variable cannot be modified)
- C) Output parameter
- D) Optional parameter

---

**Q55.** What is the output?
```csharp
enum Color { Red = 1, Green, Blue }
Console.Write((int)Color.Blue);
```
- A) 0
- B) 2
- C) 3
- D) Blue

---

## SECTION 3: COLLECTIONS & LINQ (Q56–Q90)

---

**Q56.** Which collection should you use for fast key-value lookups?
- A) List<T>
- B) Queue<T>
- C) Dictionary<TKey, TValue>
- D) LinkedList<T>

---

**Q57.** What is the output?
```csharp
var list = new List<int> { 1, 2, 3 };
list.Add(4);
list.Remove(2);
Console.Write(string.Join(",", list));
```
- A) 1,2,3,4
- B) 1,3,4
- C) 1,2,3
- D) 2,3,4

---

**Q58.** What is the output?
```csharp
var dict = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
dict["a"] = 10;
Console.Write(dict["a"]);
```
- A) 1
- B) 10
- C) 0
- D) Throws — duplicate key

---

**Q59.** What is the output?
```csharp
var set = new HashSet<int> { 1, 2, 3 };
set.Add(2);
Console.Write(set.Count);
```
- A) 3
- B) 4
- C) 2
- D) Throws

---

**Q60.** What is the output?
```csharp
var q = new Queue<int>();
q.Enqueue(1); q.Enqueue(2); q.Enqueue(3);
Console.Write(q.Dequeue());
```
- A) 1
- B) 2
- C) 3
- D) Throws

---

**Q61.** What is the output?
```csharp
var s = new Stack<int>();
s.Push(1); s.Push(2); s.Push(3);
Console.Write(s.Pop());
```
- A) 1
- B) 2
- C) 3
- D) Throws

---

**Q62.** What is the output?
```csharp
List<int> list = new() { 1, 2, 3 };
foreach (var i in list) {
    if (i == 2) list.Remove(i);
}
```
- A) list becomes {1, 3}
- B) Throws InvalidOperationException
- C) Loop silently breaks
- D) No change

---

**Q63.** What is deferred execution in LINQ?
- A) Running LINQ queries on a background thread
- B) The query is not executed when defined — it executes when iterated
- C) Caching query results
- D) Postponing compilation

---

**Q64.** What is the output?
```csharp
var nums = new[] { 1, 2, 3, 4, 5 };
var q = nums.Where(n => n > 2);
Console.Write(string.Join(",", q));
```
- A) 1,2
- B) 3,4,5
- C) 1,2,3,4,5
- D) (deferred, no output)

---

**Q65.** What is the output?
```csharp
var nums = new[] { 1, 2, 3, 4, 5 };
var result = nums.Where(n => n > 2).Select(n => n * 2);
Console.Write(string.Join(",", result));
```
- A) 3,4,5
- B) 6,8,10
- C) 2,4,6,8,10
- D) Deferred — no output

---

**Q66.** What is the output?
```csharp
var nums = new[] { 1, 2, 3 };
IEnumerable<int> q = nums.Select(n => { Console.Write(n); return n * 2; });
var first = q.First();
Console.Write("|");
var list = q.ToList();
```
- A) 123|123
- B) 1|123
- C) 1|1
- D) 123|

---

**Q67.** What is the difference between `First()` and `FirstOrDefault()`?
- A) `First()` returns null if no match; `FirstOrDefault()` throws
- B) `First()` throws if no match; `FirstOrDefault()` returns default value
- C) Identical
- D) `FirstOrDefault()` is slower

---

**Q68.** What is the output?
```csharp
var nums = new[] { 1, 2, 3, 4 };
Console.Write(nums.Sum() + " " + nums.Average());
```
- A) 10 2.5
- B) 10 2
- C) 4 2
- D) 24 2.5

---

**Q69.** What does `Any()` do?
- A) Returns true if every element matches
- B) Returns true if at least one element matches (or sequence is non-empty)
- C) Counts matches
- D) Returns the first match

---

**Q70.** What does `All()` do?
- A) Returns true if every element matches the predicate
- B) Returns true if any element matches
- C) Returns all elements
- D) Same as `Where`

---

**Q71.** What is the output?
```csharp
var nums = new[] { 1, 2, 3, 4 };
Console.Write(nums.All(n => n > 0) + " " + nums.Any(n => n > 5));
```
- A) True False
- B) False True
- C) True True
- D) False False

---

**Q72.** What does `SelectMany()` do?
- A) Selects multiple properties
- B) Flattens a collection of collections into a single sequence
- C) Selects elements with multiple conditions
- D) Selects first and last elements

---

**Q73.** What is the output?
```csharp
var lists = new[] { new[] {1, 2}, new[] {3, 4} };
var flat = lists.SelectMany(x => x);
Console.Write(string.Join(",", flat));
```
- A) 1,2,3,4
- B) 1,3,2,4
- C) {1,2},{3,4}
- D) Compile error

---

**Q74.** What is the output?
```csharp
var nums = new[] { 5, 3, 1, 4, 2 };
var sorted = nums.OrderBy(n => n);
Console.Write(string.Join(",", sorted));
```
- A) 5,3,1,4,2
- B) 1,2,3,4,5
- C) 5,4,3,2,1
- D) Deferred

---

**Q75.** What does `GroupBy()` return?
- A) A sorted sequence
- B) `IEnumerable<IGrouping<TKey, TElement>>`
- C) A Dictionary
- D) A flat sequence

---

**Q76.** What is the output?
```csharp
var words = new[] { "apple", "banana", "avocado" };
var grouped = words.GroupBy(w => w[0]).Count();
Console.Write(grouped);
```
- A) 1
- B) 2
- C) 3
- D) 0

---

**Q77.** What is the output?
```csharp
var nums = new[] { 1, 2, 3, 4, 5 };
Console.Write(nums.Take(2).Sum() + " " + nums.Skip(2).Sum());
```
- A) 3 12
- B) 1 14
- C) 6 9
- D) 5 10

---

**Q78.** What does `Distinct()` do?
- A) Returns elements satisfying a condition
- B) Returns unique elements based on default equality
- C) Sorts the sequence
- D) Reverses the sequence

---

**Q79.** What is the output?
```csharp
var nums = new[] { 1, 2, 2, 3, 3, 3 };
Console.Write(nums.Distinct().Count());
```
- A) 6
- B) 3
- C) 2
- D) 1

---

**Q80.** What is the difference between `IEnumerable.Count()` and `List.Count`?
- A) Identical
- B) `IEnumerable.Count()` may iterate the whole sequence; `List.Count` is O(1)
- C) `List.Count` is slower
- D) `IEnumerable.Count()` is cached

---

**Q81.** What is the output?
```csharp
var nums = new[] { 1, 2, 3, 4, 5 };
Console.Write(nums.Aggregate(0, (acc, n) => acc + n));
```
- A) 0
- B) 15
- C) 5
- D) 1

---

**Q82.** What does `TakeWhile()` do?
- A) Takes elements while a condition is true, stopping at the first false
- B) Same as `Where`
- C) Random subset
- D) All elements until end

---

**Q83.** What is the output?
```csharp
var nums = new[] { 1, 2, 3, 1, 4 };
var r = nums.TakeWhile(n => n < 3);
Console.Write(string.Join(",", r));
```
- A) 1,2
- B) 1,2,1
- C) 1,2,3
- D) 1,2,1,4

---

**Q84.** What is the output?
```csharp
var a = new[] { 1, 2, 3 };
var b = new[] { 10, 20, 30 };
var z = a.Zip(b, (x, y) => x + y);
Console.Write(string.Join(",", z));
```
- A) 11,22,33
- B) 1,2,3,10,20,30
- C) 10,40,90
- D) Error

---

**Q85.** What is the output?
```csharp
var nums = Enumerable.Range(1, 5);
Console.Write(string.Join(",", nums));
```
- A) 0,1,2,3,4
- B) 1,2,3,4,5
- C) 1,2,3,4
- D) 1,5

---

**Q86.** What does `ToList()` do that `AsEnumerable()` does not?
- A) ToList materialises (executes) the query; AsEnumerable just changes the static type
- B) Identical
- C) AsEnumerable runs the query
- D) ToList returns IEnumerable

---

**Q87.** What is the output?
```csharp
var nums = new List<int> { 1, 2, 3 };
var r = nums.Reverse<int>();
Console.Write(string.Join(",", r));
```
- A) 1,2,3
- B) 3,2,1
- C) Empty
- D) Throws

---

**Q88.** What is `IEnumerable<T>` vs `IQueryable<T>`?
- A) `IEnumerable` runs in memory; `IQueryable` allows expression trees translated (e.g., to SQL)
- B) `IQueryable` is faster for in-memory ops
- C) `IEnumerable` is for databases
- D) Identical

---

**Q89.** What does `Single()` do?
- A) Returns the first element
- B) Throws if zero OR more than one element matches
- C) Returns default for empty
- D) Returns last element

---

**Q90.** What is the output?
```csharp
var nums = new[] { 10, 20, 30 };
Console.Write(nums.Min() + " " + nums.Max());
```
- A) 0 30
- B) 10 30
- C) 10 20
- D) 20 30

---

## SECTION 4: ASYNC/AWAIT & THREADING (Q91–Q115)

---

**Q91.** What is the output?
```csharp
async Task<int> GetAsync() => await Task.FromResult(42);
Console.Write(await GetAsync());
```
- A) 42
- B) 0
- C) Task<int>
- D) Compile error

---

**Q92.** What does `async/await` do under the hood?
- A) Creates a new thread
- B) Transforms the method into a state machine for non-blocking waiting
- C) Runs on the thread pool always
- D) Spawns a background process

---

**Q93.** What is `Task.Run()` used for?
- A) Running async code synchronously
- B) Offloading CPU-bound work to a thread pool thread
- C) Creating a cancelled task
- D) Scheduling for a specific time

---

**Q94.** What is the output?
```csharp
async Task<int> A() => 1 + await Task.FromResult(2);
async Task<int> B() {
    var x = A();
    return await x + await x;
}
Console.Write(await B());
```
- A) 6
- B) 3
- C) Throws on second await
- D) Hangs

---

**Q95.** Predict the output:
```csharp
int x = 0;
Parallel.For(0, 100_000, _ => x++);
Console.Write(x);
```
- A) 100000
- B) Usually less than 100000 due to race condition
- C) 0
- D) Compile error

---

**Q96.** Predict the output:
```csharp
int x = 0;
var tasks = Enumerable.Range(0, 1000)
    .Select(_ => Task.Run(() => Interlocked.Increment(ref x)))
    .ToArray();
await Task.WhenAll(tasks);
Console.Write(x);
```
- A) 1000
- B) Less than 1000
- C) 0
- D) Compile error

---

**Q97.** What is `Task.WhenAll` vs `Task.WhenAny`?
- A) `WhenAll` returns first; `WhenAny` returns all
- B) `WhenAll` completes when all tasks complete; `WhenAny` when the first completes
- C) Identical
- D) `WhenAny` waits with a timeout

---

**Q98.** What is the output?
```csharp
async Task<int> Run() {
    using var cts = new CancellationTokenSource(100);
    try { await Task.Delay(1000, cts.Token); return 1; }
    catch (TaskCanceledException) { return 2; }
}
Console.Write(await Run());
```
- A) 1
- B) 2
- C) Throws
- D) Hangs

---

**Q99.** Why is `async void` an anti-pattern (outside event handlers)?
- A) Compile error
- B) Cannot be awaited; exceptions escape and can crash the process
- C) Slower than async Task
- D) Disables Task.Run

---

**Q100.** What is the difference between `Task` and `ValueTask`?
- A) ValueTask always allocates
- B) ValueTask avoids heap allocation when the result is synchronously available
- C) Identical
- D) ValueTask supports cancellation; Task does not

---

**Q101.** What is `CancellationToken` used for?
- A) Cancelling DB transactions
- B) Cooperative cancellation of async operations
- C) Setting a timeout
- D) Cancelling a thread

---

**Q102.** What is the output?
```csharp
int counter = 0;
var tasks = new Task[5];
for (int i = 0; i < 5; i++)
    tasks[i] = Task.Run(() => Interlocked.Increment(ref counter));
Task.WaitAll(tasks);
Console.Write(counter);
```
- A) 0
- B) 5
- C) Random < 5
- D) Throws

---

**Q103.** Why is `lock(this)` problematic?
- A) Compile error
- B) External code can take the same lock, causing deadlocks
- C) Only on structs
- D) It's the recommended pattern

---

**Q104.** What is `SemaphoreSlim` used for?
- A) Mutual exclusion for one thread
- B) Limiting the number of concurrent accessors of a resource (with async wait support)
- C) Thread-local storage
- D) Inter-thread signalling

---

**Q105.** What is `Interlocked.Increment()` used for?
- A) Incrementing a loop counter
- B) Atomic increment, thread-safe, without a lock
- C) Incrementing a Semaphore
- D) Incrementing a concurrent collection

---

**Q106.** What is `ConfigureAwait(false)` used for?
- A) Cancelling a task
- B) Telling the awaiter not to resume on the original synchronization context
- C) Running on a specific thread
- D) Making await fault-tolerant

---

**Q107.** What is a deadlock?
- A) A task that never starts
- B) Two operations waiting indefinitely for each other to release resources
- C) A race condition
- D) An exception in a finally block

---

**Q108.** Why is `Task.Result` dangerous in WinForms/legacy ASP.NET?
- A) Throws immediately
- B) Blocks the captured sync-context thread the continuation needs → deadlock
- C) Uses too much memory
- D) Disables Task scheduling

---

**Q109.** What's the difference between `lock` and `Mutex`?
- A) Identical
- B) `lock` is process-local (Monitor); `Mutex` is cross-process (kernel object, slower)
- C) `lock` is cross-process; `Mutex` is process-local
- D) `Mutex` is faster

---

**Q110.** What is `ThreadLocal<T>` vs `AsyncLocal<T>`?
- A) Identical
- B) `ThreadLocal` is per OS thread; `AsyncLocal` flows across async/await continuations
- C) `AsyncLocal` is process-wide
- D) `ThreadLocal` flows with await

---

**Q111.** What is `Parallel.ForEach`?
- A) Async foreach
- B) Parallel loop distributing work across thread-pool threads concurrently
- C) Runs on UI thread
- D) A LINQ operator

---

**Q112.** What is `Channel<T>`?
- A) Inter-process pipe
- B) High-performance, thread-safe producer-consumer data structure for async pipelines
- C) Network socket
- D) Pub/Sub event bus

---

**Q113.** What is the output?
```csharp
async Task<int> AddAsync(int a, int b) => a + b;
var t1 = AddAsync(1, 2);
var t2 = AddAsync(3, 4);
Console.Write(await t1 + await t2);
```
- A) 10
- B) 3
- C) 7
- D) Compile error

---

**Q114.** What does `Volatile.Read` provide?
- A) Atomic 64-bit reads
- B) An acquire-fence read so subsequent reads/writes can't be reordered before it
- C) A lock
- D) Thread-local read

---

**Q115.** What is `Monitor` in C#?
- A) CPU monitor
- B) The sync primitive underneath the `lock` keyword (`Monitor.Enter` / `Monitor.Exit`)
- C) UI component
- D) Diagnostic tool

---

## SECTION 5: .NET / CLR FUNDAMENTALS (Q116–Q135)

---

**Q116.** What is the CLR?
- A) Common Language Runtime — the execution engine for .NET
- B) Common Library Repository
- C) Component Layer Reference
- D) Cross-Language Runtime

---

**Q117.** What is IL (Intermediate Language)?
- A) Native machine code
- B) CPU-independent bytecode compiled from C#, JIT-compiled to native at runtime
- C) A markup language
- D) An interpreted scripting language

---

**Q118.** What is JIT compilation?
- A) Compiling C# source to IL at build
- B) Translating IL to native machine code at runtime, just before execution
- C) Ahead-of-time during install
- D) A minifier

---

**Q119.** What's the difference between .NET Framework and .NET (Core)?
- A) .NET Framework is cross-platform; .NET Core is Windows-only
- B) .NET (Core) is cross-platform, open-source, modular; .NET Framework is Windows-only legacy
- C) Identical
- D) .NET Core is Linux-only

---

**Q120.** How does the Garbage Collector work?
- A) Deletes objects when they go out of scope
- B) Periodically identifies unreachable objects and reclaims them with a generational algorithm
- C) Reference counting like C++
- D) Manual via GC.Collect

---

**Q121.** What is the Large Object Heap (LOH)?
- A) A separate heap for objects > 85,000 bytes; not compacted by default
- B) Heap for static variables
- C) Reserved for string interning
- D) For COM interop objects

---

**Q122.** What is the difference between Stack and Heap?
- A) Stack stores references; Heap stores values
- B) Stack stores value types and method frames (LIFO); Heap stores reference type instances (GC managed)
- C) Stack is for threads; Heap is single-threaded
- D) They are the same

---

**Q123.** What is an Assembly?
- A) A compiled `.exe` or `.dll` — unit of deployment and versioning
- B) A collection of source files
- C) A NuGet package
- D) A `.csproj` file

---

**Q124.** What does `GC.SuppressFinalize(this)` do?
- A) Prevents GC from collecting the object
- B) Tells GC not to call the finalizer (cleanup done via Dispose)
- C) Forces immediate collection
- D) Removes all references

---

**Q125.** What is `IDisposable` used for?
- A) Marking a class as GC-eligible
- B) Providing deterministic cleanup of unmanaged resources
- C) Preventing multiple instantiation
- D) Serialization

---

**Q126.** What does `using` statement do?
- A) Imports namespaces
- B) Ensures IDisposable objects are disposed at end of scope
- C) Both
- D) Declares global variables

---

**Q127.** What is Reflection?
- A) Mirror design pattern
- B) Ability to inspect and manipulate types/methods/properties at runtime
- C) Compile-time code generation
- D) Optical networking

---

**Q128.** What does `Activator.CreateInstance()` do?
- A) Starts the runtime
- B) Creates an instance of a type dynamically at runtime using reflection
- C) Activates a background service
- D) Initializes a static class

---

**Q129.** What is `WeakReference<T>`?
- A) A reference that increments refcount
- B) A reference that does not prevent GC from collecting the target
- C) A reference auto-nulled on access
- D) Thread-local reference

---

**Q130.** What does the `volatile` keyword do?
- A) Prevents serialization
- B) Ensures reads/writes go to main memory (preventing certain caching/reordering)
- C) Makes the field thread-safe (atomic)
- D) Prevents GC

---

**Q131.** What does `Environment.Exit(0)` do?
- A) Exits the current method
- B) Terminates the process immediately with exit code 0
- C) Disposes all IDisposable objects
- D) Soft restart

---

**Q132.** What is the output?
```csharp
struct Point { public int X; }
Point p1 = new Point { X = 5 };
Point p2 = p1;
p2.X = 10;
Console.Write(p1.X + " " + p2.X);
```
- A) 10 10
- B) 5 10
- C) 5 5
- D) 10 5

---

**Q133.** What is the output?
```csharp
class Point { public int X; }
Point p1 = new Point { X = 5 };
Point p2 = p1;
p2.X = 10;
Console.Write(p1.X + " " + p2.X);
```
- A) 10 10
- B) 5 10
- C) 5 5
- D) 10 5

---

**Q134.** What does `[Serializable]` attribute do?
- A) Makes a class JSON-serializable
- B) Marks a class compatible with binary serialization (legacy BinaryFormatter)
- C) Enables XML serialization
- D) Prevents serialization

---

**Q135.** What is `Span<T>`?
- A) A thread-safe array
- B) A type-safe, memory-safe slice of contiguous memory with no heap allocation
- C) A lazy wrapper
- D) Thread-local storage

---

## SECTION 6: ASP.NET CORE (Q136–Q160)

---

**Q136.** Where in the middleware pipeline should `UseAuthentication()` and `UseAuthorization()` sit?
- A) Anywhere
- B) After `UseRouting()` and before endpoints; Authentication before Authorization
- C) Before UseRouting
- D) After endpoints

---

**Q137.** What is the difference between `app.Use()` and `app.Run()`?
- A) Use is terminal; Run is not
- B) Use can call next middleware; Run is terminal (no next)
- C) Identical
- D) Run is for routing only

---

**Q138.** What does `[ApiController]` enable?
- A) MVC view rendering
- B) Auto model validation (400 ProblemDetails), `[FromBody]` inference, required binding sources
- C) Registers controller in DI
- D) Razor Pages routing

---

**Q139.** What is the difference between `IActionResult` and `ActionResult<T>`?
- A) Identical
- B) `ActionResult<T>` is typed, allowing OpenAPI/Swagger to infer response shape and implicit conversion
- C) `IActionResult` is for async only
- D) `ActionResult<T>` only returns JSON

---

**Q140.** What is the correct DI lifetime for `DbContext`?
- A) Singleton
- B) Scoped (one per HTTP request)
- C) Transient
- D) Static field

---

**Q141.** What is the difference between `AddSingleton`, `AddScoped`, and `AddTransient`?
- A) Singleton = per request; Scoped = per app; Transient = new
- B) Singleton = per app lifetime; Scoped = per HTTP request; Transient = new every resolution
- C) All create new instances each time
- D) Scoped is for DBs only

---

**Q142.** What is `appsettings.json` for?
- A) Compiled binaries
- B) Hierarchical app configuration (connection strings, feature flags, etc.)
- C) HTTP routes
- D) NuGet package references

---

**Q143.** How do you bind a configuration section to a strongly-typed class?
- A) `[Bind]` attribute
- B) `services.Configure<T>(config.GetSection("X"))`
- C) Manually read each key
- D) `[Configuration]` attribute

---

**Q144.** What does `[HttpGet]`, `[HttpPost]` do on action methods?
- A) Restricts the action to a specific HTTP method
- B) Sets the content type
- C) Defines authorization
- D) Sets the status code

---

**Q145.** What is Model Binding in ASP.NET Core?
- A) Connecting DB to model classes
- B) Mapping HTTP request data (route, query, body) to action parameters
- C) Binding views to models
- D) Linking model classes with FK

---

**Q146.** What does `[FromBody]` indicate?
- A) Parameter is read from the route
- B) Parameter is read from the HTTP request body (typically JSON)
- C) Parameter is read from the query string
- D) Parameter is from headers

---

**Q147.** What does `UseHttpsRedirection()` do?
- A) Encrypts the DB connection
- B) Automatically redirects HTTP requests to HTTPS
- C) Validates SSL certs
- D) Configures HSTS

---

**Q148.** What is middleware in ASP.NET Core?
- A) A DB abstraction layer
- B) Components in the HTTP request/response pipeline that can process, modify, or short-circuit requests
- C) Server-loaded JS libraries
- D) Auth tokens

---

**Q149.** What is CORS?
- A) Static file caching
- B) Cross-Origin Resource Sharing — controls which browser origins can call your API
- C) A compression algorithm
- D) SQL injection prevention

---

**Q150.** What does `IHostedService` do?
- A) Hosts static files
- B) Interface for background services in ASP.NET Core
- C) IIS hosting config
- D) Manages HTTP sessions

---

**Q151.** What is Content Negotiation?
- A) Negotiating SSL certs
- B) Selecting response format (JSON/XML) based on `Accept` header
- C) Compression
- D) Caching

---

**Q152.** What does `app.UseExceptionHandler("/Error")` do?
- A) Logs to file
- B) Catches unhandled exceptions and routes to the specified error path
- C) Prevents propagation
- D) Returns 500 with no body

---

**Q153.** What is the Minimal API in .NET 6+?
- A) Stripped-down ASP.NET with no routing
- B) Lightweight way to define HTTP endpoints with minimal ceremony, no controllers
- C) An API without authentication
- D) Performance-only mode

---

**Q154.** What does `IHttpClientFactory` solve?
- A) Required for HTTPS
- B) Manual `new HttpClient()` causes socket exhaustion; factory pools handlers safely
- C) HttpClient is not thread-safe
- D) HttpClient doesn't support JSON

---

**Q155.** What is `IHttpContextAccessor`?
- A) An outbound HTTP client
- B) Access to the current HTTP context outside controllers (e.g., in services)
- C) Reads HTTP headers from config
- D) Factory for HttpClient

---

**Q156.** What is the role of `Program.cs` in ASP.NET Core 6+?
- A) Routing-only configuration
- B) Entry point that configures services (DI) and the HTTP middleware pipeline
- C) Main controller
- D) IIS startup script

---

**Q157.** What is the purpose of `[Route]` attribute?
- A) Registers the controller in DI
- B) Defines URL patterns for controller actions
- C) Specifies HTTP methods
- D) Configures caching

---

**Q158.** What is the order of MVC filters around an action?
- A) Action → Authorization → Resource → Result
- B) Authorization → Resource → Action → (action runs) → Result → Exception (on error)
- C) Result → Action → Resource → Authorization
- D) Random

---

**Q159.** What is Rate Limiting in ASP.NET Core 7+?
- A) Limiting response size
- B) Throttling incoming requests to protect the server
- C) CPU caps per request
- D) Limiting DB connections

---

**Q160.** What does `[FromServices]` do?
- A) Marks param as session value
- B) Resolves the parameter from DI instead of from request data
- C) Triggers a service call
- D) Only works on Razor Pages

---

## SECTION 7: ENTITY FRAMEWORK CORE (Q161–Q180)

---

**Q161.** What is `DbContext`?
- A) A connection string
- B) Primary class for DB operations — represents a session with the database
- C) A config file
- D) Migration runner

---

**Q162.** What does `SaveChanges()` do?
- A) Writes to a local cache
- B) Generates and executes SQL INSERT/UPDATE/DELETE for all tracked changes
- C) Manually commits a transaction
- D) Refreshes entities from DB

---

**Q163.** What is the difference between `Add` and `Attach`?
- A) Add marks as new (INSERT); Attach marks as existing (tracked, no immediate SQL)
- B) Attach marks as new; Add as existing
- C) Identical
- D) Add is bulk; Attach is single

---

**Q164.** What is Eager Loading?
- A) Loading all data upfront regardless
- B) Loading related entities as part of the initial query using `.Include()`
- C) Caching entities
- D) Precompiling LINQ

---

**Q165.** What is Lazy Loading?
- A) Deferring all DB ops
- B) Loading related entities automatically when navigation property is first accessed
- C) Loading top 10 rows
- D) Result caching

---

**Q166.** What is the N+1 query problem?
- A) Running the same query 1000 times
- B) 1 query for a list, then N additional queries to fetch related data per item
- C) A query with N joins
- D) Returns N+1 rows than expected

---

**Q167.** What does `.AsNoTracking()` do?
- A) Disables LINQ translation
- B) Disables change tracking, improving read-only query performance
- C) Prevents deletes
- D) Disables lazy loading

---

**Q168.** What is a Migration in EF Core?
- A) Moving DB to a different server
- B) Code-generated file describing incremental schema changes
- C) Transferring data between tables
- D) Converting DB engines

---

**Q169.** What does `.Find()` do that `FirstOrDefault()` doesn't?
- A) `.Find()` checks the change tracker cache first (PK lookup); `FirstOrDefault()` always queries DB
- B) FirstOrDefault checks cache
- C) Identical
- D) Find returns all matches

---

**Q170.** What does the Fluent API in EF Core do?
- A) A JS query syntax
- B) Configures entity mappings in code via method chaining in `OnModelCreating()`
- C) REST query API
- D) Auto-generated API layer

---

**Q171.** What is `ExecuteUpdateAsync()` in EF Core 7+?
- A) Runs migrations async
- B) Bulk UPDATE in SQL without loading entities into ChangeTracker
- C) Updates connection string at runtime
- D) Schedules deferred updates

---

**Q172.** What does `HasQueryFilter()` do?
- A) Validates query syntax
- B) Adds an implicit WHERE clause to all queries for that entity (e.g., soft delete, multi-tenancy)
- C) Limits result size
- D) Caches results

---

**Q173.** What does `.Include()` do?
- A) Includes a column
- B) Eager loads related entities via JOIN
- C) Adds a filter
- D) Adds an index

---

**Q174.** What is `AsSplitQuery()` used for?
- A) Sharding
- B) Splitting a query with multiple Includes into separate SQL queries — avoids cartesian explosion
- C) Parallel execution
- D) Pagination

---

**Q175.** What does `[DatabaseGenerated(DatabaseGeneratedOption.Identity)]` mean?
- A) Marks the column as auto-incremented by the DB
- B) Makes it read-only
- C) Sets as primary key
- D) Generates a GUID

---

**Q176.** What is Connection Resiliency in EF Core?
- A) Auto-scaling connections
- B) Automatic retry logic for transient DB failures
- C) Load balancing
- D) Encrypting the connection string

---

**Q177.** What is an Owned Entity in EF Core?
- A) An always-private entity
- B) A type whose lifetime is tied to an owner — by default mapped into the owner's table (value object)
- C) Composite key entity
- D) Cannot be queried

---

**Q178.** What is a Shadow Property?
- A) Visible only in debug
- B) A property in the EF model but not in the entity class — exists only in DB schema
- C) Calculated, not persisted
- D) A private backing field

---

**Q179.** What's the difference between `ExecuteSqlRaw` and `ExecuteSqlInterpolated`?
- A) Identical
- B) `ExecuteSqlInterpolated($"... {value}")` parameterises interpolated values safely; `ExecuteSqlRaw` needs manual `SqlParameter`
- C) Raw is safer
- D) Interpolated runs LINQ

---

**Q180.** What is the issue with using EF Core `InMemory` provider for testing?
- A) Nothing
- B) Doesn't enforce relational constraints, transactions, or raw SQL — may behave differently from a real DB
- C) Cannot be installed
- D) Slower than SQL Server

---

## SECTION 8: DEPENDENCY INJECTION & PATTERNS (Q181–Q190)

---

**Q181.** What is Dependency Injection?
- A) Injecting JS into HTML
- B) A design pattern where dependencies are provided externally rather than created internally
- C) Injecting SQL into queries
- D) Compile-time code gen

---

**Q182.** What is the Captive Dependency problem?
- A) Dependency can't be released from memory
- B) A longer-lived service (Singleton) holding a shorter-lived (Scoped/Transient) service — keeps it alive too long
- C) Circular dependency
- D) Authentication-required service

---

**Q183.** What does `GetRequiredService<T>()` do differently from `GetService<T>()`?
- A) GetRequiredService throws if not registered; GetService returns null
- B) Identical
- C) GetService throws
- D) GetRequiredService is Singleton-only

---

**Q184.** What is the Service Locator anti-pattern?
- A) Locating physical servers
- B) Resolving dependencies from a container inside classes — hides dependencies, harder to test
- C) Caching services
- D) Improved DI

---

**Q185.** What is `IServiceScope` used for?
- A) Limits service namespace
- B) Manually created DI scope — needed in background workers / console apps to use scoped services
- C) Unit testing only
- D) Auth scope

---

**Q186.** What is the Repository Pattern?
- A) Version control storage
- B) Abstraction between business logic and data access; collection-like interface for domain objects
- C) Factory for DB connections
- D) HTTP cache management

---

**Q187.** What is the Unit of Work pattern?
- A) Sprint effort estimation
- B) Maintains a list of objects affected by a business transaction and writes them in a single coordinated operation
- C) Time-boxing technique
- D) Rate limiting

---

**Q188.** What is the Singleton pattern (not DI lifetime)?
- A) Class in a single assembly
- B) Restricts instantiation to one instance with a global access point
- C) Static-members-only class
- D) Lazy loader

---

**Q189.** What is the Decorator pattern?
- A) UI styling
- B) Wrapping an object to add behaviour without modifying the original class
- C) Logging decorative messages
- D) Adding attributes

---

**Q190.** What is the Mediator pattern (used by MediatR)?
- A) Service broker for APIs
- B) Decouples senders and handlers — components communicate through a central mediator
- C) Load balancing
- D) Query dispatcher

---

## SECTION 9: TESTING (Q191–Q200)

---

**Q191.** What is the AAA pattern?
- A) Authentication–Authorization–Audit
- B) Arrange–Act–Assert — a structure for organising test code
- C) Async–Await–Assert
- D) Abstract–Abstract–Assert

---

**Q192.** What is `[Fact]` vs `[Theory]` in xUnit?
- A) Fact runs multiple times; Theory runs once
- B) Fact runs once with no parameters; Theory runs multiple times with `[InlineData]` / `[MemberData]`
- C) Identical
- D) Theory is async-only

---

**Q193.** What is Mocking?
- A) Writing fake business logic
- B) Creating controlled fakes of dependencies that simulate real behaviour without side effects
- C) Mock database
- D) Hiding implementation

---

**Q194.** What is the difference between Unit, Integration, and E2E tests?
- A) Just different frameworks
- B) Unit = isolated units; Integration = multiple components together; E2E = complete user flows
- C) E2E is fastest
- D) Integration doesn't need a DB

---

**Q195.** What is `WebApplicationFactory<T>`?
- A) Test DB factory
- B) In-memory test server bootstrapping the full ASP.NET Core pipeline for integration tests
- C) Mock for IWebHostEnvironment
- D) Test data generator

---

**Q196.** What does Test-Driven Development (TDD) describe?
- A) Writing tests after the code
- B) Red–Green–Refactor: failing test → minimum code to pass → refactor
- C) Automated integration testing
- D) Code coverage analysis

---

**Q197.** What is the output?
```csharp
var nums = new[] { 1, 2, 3, 4 };
var sum = 0;
foreach (var n in nums) sum += n;
Console.Write(sum);
```
- A) 10
- B) 4
- C) 24
- D) 0

---

**Q198.** What is the output?
```csharp
var nums = new List<int> { 1, 2, 3 };
nums.Insert(1, 99);
Console.Write(string.Join(",", nums));
```
- A) 1,99,2,3
- B) 99,1,2,3
- C) 1,2,99,3
- D) 1,2,3,99

---

**Q199.** What is Code Coverage?
- A) Percentage of code that is commented
- B) Percentage of production code executed by tests
- C) Number of tests per file
- D) Linting metric

---

**Q200.** What is the output?
```csharp
var dict = new Dictionary<string, int>();
dict.TryAdd("a", 1);
dict.TryAdd("a", 2);
Console.Write(dict["a"]);
```
- A) 1
- B) 2
- C) 3
- D) Throws

---

# 🎯 ANSWER KEY (Q1 – Q200)

> Correct option + brief explanation.

---

## Section 1: C# Basics & Output Prediction

**Q1. ✅ A) 5 7** — `x++` uses 5 then increments to 6; `++x` increments to 7 and uses 7.

**Q2. ✅ B) 3 1** — Integer division: `10/3 = 3`. Modulo: `10%3 = 1`.

**Q3. ✅ B) 8** — i: 1 → 2 → 4 → 8. Loop ends (8 not < 5).

**Q4. ✅ B) 6** — Concatenation yields `"abcdef"` (length 6).

**Q5. ✅ B) 4 4** — Length is 4 and `arr[3]` is the last element (4).

**Q6. ✅ C) internal** — Restricts access to current assembly.

**Q7. ✅ A) True** — `!true = false`; `false || true = true`.

**Q8. ✅ B) 5 10** — `int` is a value type — `y = x` copies value; changing y doesn't affect x.

**Q9. ✅ B) True** — String literals are interned by the compiler; same reference.

**Q10. ✅ B) Returns left operand if not null, else right.** Null-coalescing.

**Q11. ✅ C) 99** — `null ?? 99` returns 99.

**Q12. ✅ B) null** — Nullable arithmetic with null yields null; `null?.ToString() ?? "null"` returns "null".

**Q13. ✅ B) Prevents the class from being inherited.**

**Q14. ✅ A) `ref` requires initialization; `out` does not.**

**Q15. ✅ A) 5** — Value type passed by value; method-local change doesn't affect caller.

**Q16. ✅ B) 6** — `ref` passes by reference; caller's variable changes.

**Q17. ✅ A) -1 1 10** — Switch expression: -5 < 0 → -1; 5 is `>0 and <10` → 1; 50 falls to `_` → 10.

**Q18. ✅ A) Converting a value type to a reference type.**

**Q19. ✅ C) struct** — value type. The others are reference types.

**Q20. ✅ B) "myVar"** — Compile-time string of the identifier.

**Q21. ✅ A) 1F** — Return value is computed (1), `finally` runs (prints F), then 1 is returned and printed by caller. Net: `F1`. **Correct: B) F1.**

**Q22. ✅ B) The finally exception replaces the original.** Avoid throwing from finally.

**Q23. ✅ B) `throw` preserves stack trace; `throw ex` resets it.**

**Q24. ✅ B) ABC** — Try prints A, throws, catch prints B, finally prints C.

**Q25. ✅ B) B** — Inner `else` binds to the nearest `if` (the `x > 20` one).

**Q26. ✅ B) 02** — `continue` skips the `i==1` iteration.

**Q27. ✅ B) 3** — 1 + 2 then break at i=3.

**Q28. ✅ B) -1** — `s?.Length` is null; `?? -1` returns -1.

**Q29. ✅ B) 333** — Classic capture bug: all lambdas share the same `i`; after loop `i` is 3.

**Q30. ✅ A) 012** — `local` is a new variable per iteration; each lambda captures its own.

**Q31. ✅ B) Compile-time inferred static type.**

**Q32. ✅ A) 10** — Box (int → object), unbox back to int.

**Q33. ✅ A) True** — `OrdinalIgnoreCase` ignores case.

**Q34. ✅ B) For strings, both compare content (== is overloaded for string).**

**Q35. ✅ B) 65** — ASCII code for 'A'.

---

## Section 2: OOP, Generics & C# Features

**Q36. ✅ B) virtual**

**Q37. ✅ B) B** — Virtual + override → polymorphism: calls overridden method at runtime.

**Q38. ✅ A) A** — `new` hides; method bound by the static (compile-time) type of `obj` (A).

**Q39. ✅ B) Yes — called by derived classes' constructors.**

**Q40. ✅ D) Both B and C.**

**Q41. ✅ A) Circle** — Polymorphism through the interface.

**Q42. ✅ B) Two ints → bool.** Last generic param is the return type.

**Q43. ✅ B) Variable number of arguments as an array.**

**Q44. ✅ A) 10** — 1+2+3+4 = 10.

**Q45. ✅ B) True** — `int` is an alias for `System.Int32`.

**Q46. ✅ A) 42** — Generic class with `T = int`.

**Q47. ✅ B) Compile-time string name of a variable/type/member.**

**Q48. ✅ B) A class definition split across multiple files.**

**Q49. ✅ A) True** — Records implement value-based equality (and override `==`).

**Q50. ✅ A) 1 3** — `with` creates a copy with Y changed; original p1 unchanged.

**Q51. ✅ B) Set only during construction / initializer.**

**Q52. ✅ B) A value type wrapped with `Nullable<T>` that can hold null.**

**Q53. ✅ B) False** — `a` is null; `HasValue` is false.

**Q54. ✅ B) Read-only reference parameter.**

**Q55. ✅ C) 3** — Enum starts at 1 (Red); Green=2, Blue=3.

---

## Section 3: Collections & LINQ

**Q56. ✅ C) Dictionary<TKey, TValue>** — Average O(1) lookup.

**Q57. ✅ B) 1,3,4** — After add/remove sequence.

**Q58. ✅ B) 10** — Indexer assignment replaces existing value.

**Q59. ✅ A) 3** — HashSet ignores duplicates.

**Q60. ✅ A) 1** — FIFO.

**Q61. ✅ C) 3** — LIFO.

**Q62. ✅ B) Throws InvalidOperationException** — Modifying List during iteration invalidates the enumerator.

**Q63. ✅ B) Query is not executed until iterated.**

**Q64. ✅ B) 3,4,5** — `string.Join` iterates, executing the deferred query.

**Q65. ✅ B) 6,8,10** — Filter then double.

**Q66. ✅ B) 1|123** — `First()` short-circuits after first; then `ToList()` re-enumerates everything.

**Q67. ✅ B) `First()` throws if no match; `FirstOrDefault()` returns default.**

**Q68. ✅ A) 10 2.5** — Sum 10, Average 2.5.

**Q69. ✅ B) Returns true if at least one matches (or sequence is non-empty).**

**Q70. ✅ A) True only if every element matches.**

**Q71. ✅ A) True False** — All > 0 → True; Any > 5 → False.

**Q72. ✅ B) Flattens a collection of collections.**

**Q73. ✅ A) 1,2,3,4** — Flattened.

**Q74. ✅ B) 1,2,3,4,5** — OrderBy ascending.

**Q75. ✅ B) IEnumerable<IGrouping<TKey, TElement>>**

**Q76. ✅ B) 2** — Groups by first char: 'a' (apple, avocado), 'b' (banana). Count = 2.

**Q77. ✅ A) 3 12** — Take(2) → 1+2=3; Skip(2) → 3+4+5=12.

**Q78. ✅ B) Returns unique elements based on default equality.**

**Q79. ✅ B) 3** — {1, 2, 3} unique values.

**Q80. ✅ B) IEnumerable.Count() may iterate; List.Count is O(1).**

**Q81. ✅ B) 15** — Folds with sum (1+2+3+4+5).

**Q82. ✅ A) Takes while condition true, stops at first false.**

**Q83. ✅ A) 1,2** — Stops at 3 (first not < 3).

**Q84. ✅ A) 11,22,33** — Pairs and sums element-by-element.

**Q85. ✅ B) 1,2,3,4,5** — Range(1, count=5).

**Q86. ✅ A) ToList materialises; AsEnumerable just changes the static type.**

**Q87. ✅ B) 3,2,1** — Reversed.

**Q88. ✅ A) IEnumerable in memory; IQueryable allows expression tree translation (e.g., SQL).**

**Q89. ✅ B) Throws if zero OR more than one match.**

**Q90. ✅ B) 10 30** — Min and Max.

---

## Section 4: Async/Await & Threading

**Q91. ✅ A) 42**

**Q92. ✅ B) State machine for non-blocking waiting.**

**Q93. ✅ B) Offloads CPU-bound work to a thread pool thread.**

**Q94. ✅ A) 6** — `A()` returns Task<int>=3; awaiting it twice yields 3+3=6 (task result is cached).

**Q95. ✅ B) Less than 100000** — `x++` is non-atomic; race condition.

**Q96. ✅ A) 1000** — `Interlocked.Increment` is atomic.

**Q97. ✅ B) WhenAll = all complete; WhenAny = first completes.**

**Q98. ✅ B) 2** — Cancelled after 100 ms; catch returns 2.

**Q99. ✅ B) Cannot be awaited; exceptions escape and may crash the process.**

**Q100. ✅ B) ValueTask avoids heap allocation when result is synchronously available.**

**Q101. ✅ B) Cooperative cancellation of async ops.**

**Q102. ✅ B) 5** — Atomic increments.

**Q103. ✅ B) External code can lock the same object → deadlocks/starvation.**

**Q104. ✅ B) Limits the number of concurrent accessors (with async wait support).**

**Q105. ✅ B) Atomic increment, thread-safe, lock-free.**

**Q106. ✅ B) Don't capture / resume on the original synchronization context.**

**Q107. ✅ B) Two operations waiting indefinitely for each other's resources.**

**Q108. ✅ B) Blocks the sync-context thread the continuation needs → deadlock.**

**Q109. ✅ B) `lock` is process-local; Mutex is cross-process (slower).**

**Q110. ✅ B) ThreadLocal = per OS thread; AsyncLocal flows across async continuations.**

**Q111. ✅ B) Parallel loop on thread-pool threads.**

**Q112. ✅ B) High-performance, thread-safe producer-consumer for async pipelines.**

**Q113. ✅ A) 10** — 3 + 7 = 10.

**Q114. ✅ B) Acquire-fence read preventing reordering after the read.**

**Q115. ✅ B) Underlying primitive of the `lock` keyword.**

---

## Section 5: .NET / CLR Fundamentals

**Q116. ✅ A) Common Language Runtime.**

**Q117. ✅ B) CPU-independent bytecode JIT-compiled at runtime.**

**Q118. ✅ B) IL → native at runtime, just-in-time.**

**Q119. ✅ B) .NET (Core) is cross-platform, modern; .NET Framework is Windows-only, legacy.**

**Q120. ✅ B) Generational GC reclaims unreachable objects periodically.**

**Q121. ✅ A) Heap for objects > 85,000 bytes; not compacted by default.**

**Q122. ✅ B) Stack stores value types/frames (LIFO); Heap stores reference type instances.**

**Q123. ✅ A) Compiled .exe or .dll — deployment/versioning unit.**

**Q124. ✅ B) Skip the finalizer (Dispose already cleaned up).**

**Q125. ✅ B) Deterministic cleanup of unmanaged resources.**

**Q126. ✅ C) Both directives (`using System;`) and disposal (`using var x = ...`).**

**Q127. ✅ B) Inspecting/manipulating types/members at runtime.**

**Q128. ✅ B) Creates an instance dynamically via reflection.**

**Q129. ✅ B) Doesn't prevent GC from collecting the target.**

**Q130. ✅ B) Ensures reads/writes hit main memory; prevents some reordering. Doesn't make compound ops atomic.**

**Q131. ✅ B) Terminates the process immediately.**

**Q132. ✅ B) 5 10** — Struct is a value type; `p2 = p1` copies.

**Q133. ✅ A) 10 10** — Class is a reference type; both variables point to the same instance.

**Q134. ✅ B) Marks compatibility with binary serialization (legacy BinaryFormatter).**

**Q135. ✅ B) Type/memory-safe slice of contiguous memory; no heap allocation.**

---

## Section 6: ASP.NET Core

**Q136. ✅ B) After UseRouting, before endpoints; Authentication before Authorization.**

**Q137. ✅ B) Use can call next; Run is terminal.**

**Q138. ✅ B) Auto validation (400 ProblemDetails), [FromBody] inference, required binding sources.**

**Q139. ✅ B) ActionResult<T> is typed — OpenAPI infers response shape; allows implicit conversion.**

**Q140. ✅ B) Scoped — one per HTTP request.**

**Q141. ✅ B) Singleton per app lifetime; Scoped per request; Transient new every time.**

**Q142. ✅ B) Hierarchical app configuration.**

**Q143. ✅ B) `services.Configure<T>(config.GetSection("X"))`.**

**Q144. ✅ A) Restricts the action to a specific HTTP method.**

**Q145. ✅ B) Maps HTTP data (route/query/body) to action parameters.**

**Q146. ✅ B) Read from request body (typically JSON).**

**Q147. ✅ B) Auto-redirects HTTP to HTTPS.**

**Q148. ✅ B) Components in the HTTP pipeline that can process / short-circuit requests.**

**Q149. ✅ B) Controls which browser origins can call your API.**

**Q150. ✅ B) Interface for background services in ASP.NET Core.**

**Q151. ✅ B) Selects response format (JSON/XML) based on `Accept` header.**

**Q152. ✅ B) Catches unhandled exceptions and re-executes a request to the error path.**

**Q153. ✅ B) Lightweight HTTP endpoint definitions without controllers.**

**Q154. ✅ B) Pools HttpMessageHandler so manual `new HttpClient()` doesn't exhaust sockets.**

**Q155. ✅ B) Access to HttpContext outside controllers (be careful with lifetime).**

**Q156. ✅ B) Entry point configuring DI and the middleware pipeline (minimal hosting model).**

**Q157. ✅ B) Defines URL patterns for controller actions.**

**Q158. ✅ B) Authorization → Resource → Action → Result; Exception only on error.**

**Q159. ✅ B) Throttling incoming requests.**

**Q160. ✅ B) Resolves the parameter from DI rather than the request.**

---

## Section 7: Entity Framework Core

**Q161. ✅ B) Primary class for DB operations / session with the database.**

**Q162. ✅ B) Generates and executes SQL for tracked changes in a transaction.**

**Q163. ✅ A) Add = new (INSERT); Attach = existing (tracked, no immediate SQL).**

**Q164. ✅ B) Loading related entities via `.Include()` in the initial query (JOIN).**

**Q165. ✅ B) Auto-loading related entities on first access to a navigation property.**

**Q166. ✅ B) 1 query + N additional queries for related data per item.**

**Q167. ✅ B) Disables change tracking for read-only performance.**

**Q168. ✅ B) Code-generated file describing incremental schema changes.**

**Q169. ✅ A) Find checks the change tracker cache first; FirstOrDefault always hits the DB.**

**Q170. ✅ B) Configures entity mappings via method chaining in `OnModelCreating`.**

**Q171. ✅ B) Bulk UPDATE SQL without loading entities into the ChangeTracker.**

**Q172. ✅ B) Adds an implicit WHERE clause to all queries for an entity (soft delete, multi-tenancy).**

**Q173. ✅ B) Eager loading via JOIN.**

**Q174. ✅ B) Splits queries with multiple includes to avoid cartesian explosion.**

**Q175. ✅ A) DB auto-increments the column on INSERT.**

**Q176. ✅ B) Auto retry for transient DB failures.**

**Q177. ✅ B) Value-object-like entity tied to an owner — mapped into the owner's table by default.**

**Q178. ✅ B) Property in the model but not in the entity class.**

**Q179. ✅ B) `ExecuteSqlInterpolated` parameterises automatically — safer from SQL injection.**

**Q180. ✅ B) Doesn't enforce relational constraints / transactions; behaviour can differ from a real DB.**

---

## Section 8: Dependency Injection & Patterns

**Q181. ✅ B) Dependencies provided externally instead of created internally.**

**Q182. ✅ B) Singleton holding Scoped/Transient → keeps the shorter-lived service alive too long.**

**Q183. ✅ A) GetRequiredService throws if missing; GetService returns null.**

**Q184. ✅ B) Resolving from a container inside classes — hides dependencies, hurts testability.**

**Q185. ✅ B) Manually created DI scope — needed in background workers / console apps.**

**Q186. ✅ B) Abstraction between business logic and data access; collection-like interface.**

**Q187. ✅ B) Coordinates writing all changes in a single transaction.**

**Q188. ✅ B) One instance per type with a global access point.**

**Q189. ✅ B) Wraps an object to add behaviour without modifying it.**

**Q190. ✅ B) Senders and handlers communicate via a central mediator (e.g., MediatR).**

---

## Section 9: Testing & Misc

**Q191. ✅ B) Arrange–Act–Assert.**

**Q192. ✅ B) Fact runs once; Theory runs many times with data sets.**

**Q193. ✅ B) Controlled fakes simulating real behaviour without side effects.**

**Q194. ✅ B) Unit (isolated) → Integration (multiple components) → E2E (user flows).**

**Q195. ✅ B) In-memory test server running the full ASP.NET Core pipeline.**

**Q196. ✅ B) Red–Green–Refactor.**

**Q197. ✅ A) 10** — 1+2+3+4.

**Q198. ✅ A) 1,99,2,3** — Insert at index 1.

**Q199. ✅ B) Percentage of production code executed by tests.**

**Q200. ✅ A) 1** — `TryAdd` returns false on duplicate keys without overwriting.

---

## Summary of Topics

| Section | Topic | Questions |
|---|---|---|
| 1 | C# Basics & Output Prediction | Q1–Q35 |
| 2 | OOP, Generics & C# Features | Q36–Q55 |
| 3 | Collections & LINQ | Q56–Q90 |
| 4 | Async/Await & Threading | Q91–Q115 |
| 5 | .NET / CLR Fundamentals | Q116–Q135 |
| 6 | ASP.NET Core | Q136–Q160 |
| 7 | Entity Framework Core | Q161–Q180 |
| 8 | Dependency Injection & Patterns | Q181–Q190 |
| 9 | Testing & Misc | Q191–Q200 |

---

