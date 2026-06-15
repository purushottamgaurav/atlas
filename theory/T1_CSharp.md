# C# Interview Questions & Answers

---

## 🔷 Section 1: Core Language Fundamentals

---

**Q1. What is C#? What is the CLR?**

C# is a statically typed, object-oriented language by Microsoft that runs on the .NET runtime. The **CLR (Common Language Runtime)** is the engine that runs .NET programs — it handles memory management, garbage collection, exception handling, and security.

---

**Q2. What is IL, CLR, and JIT? How do they relate?**

- **IL (Intermediate Language):** When you compile C# code, it doesn't go directly to machine code — it becomes IL (also called MSIL or CIL), a platform-neutral bytecode.
- **CLR:** Loads and runs this IL.
- **JIT (Just-In-Time Compiler):** At runtime, CLR's JIT converts IL into native machine code for the current CPU.

```
C# Code → [C# Compiler] → IL (.dll/.exe) → [JIT at runtime] → Native Machine Code
```

---

**Q3. What are the types of JIT compilation?**

| Type | Description |
|------|-------------|
| **Normal JIT** | Compiles methods on first call, caches result |
| **Econo JIT** | Compiles method, discards after use (low memory) |
| **Pre-JIT (NGEN/AOT)** | Compiles entire assembly before execution (faster startup) |

---

**Q4. What is a C# Assembly? Difference between Assembly and Namespace?**

- **Assembly** is the physical file (`.dll` or `.exe`) — the unit of deployment and versioning.
- **Namespace** is a logical grouping of types inside code to avoid name conflicts.

One assembly can contain many namespaces. One namespace can span multiple assemblies.

```csharp
// Namespace is logical grouping
namespace MyApp.Services { public class OrderService {} }

// Assembly is the compiled output: MyApp.dll
```

---

**Q5. What is Reflection in C#? How is it used?**

Reflection lets you inspect and invoke types, methods, and properties **at runtime**, even if you don't know them at compile time.

```csharp
Type type = typeof(MyClass);
MethodInfo method = type.GetMethod("DoWork");
method.Invoke(Activator.CreateInstance(type), null);
```

**Used in:** Dependency injection containers, ORMs (EF Core), serializers (JSON.NET), plugin systems.

---

**Q6. What is a Namespace vs using directive?**

A **namespace** declares where a type lives. A **using directive** lets you use that type without typing the full path.

```csharp
namespace MyApp.Models { public class User {} }

// Without using:
MyApp.Models.User u = new MyApp.Models.User();

// With using:
using MyApp.Models;
User u = new User();
```

---

**Q7. What are keywords vs identifiers vs literals?**

- **Keyword:** Reserved words by C# (`class`, `int`, `return`, `async`).
- **Identifier:** Name you give to things (`MyClass`, `userName`, `DoWork`).
- **Literal:** A fixed value written in code (`42`, `"hello"`, `true`, `3.14`).

---

**Q8. Difference between value types and reference types?**

| | Value Type | Reference Type |
|--|--|--|
| Stored on | Stack | Heap |
| Contains | The actual data | A pointer to data |
| Copy behavior | Full copy | Copies the reference |
| Examples | `int`, `bool`, `struct`, `enum`, `double` | `class`, `string`, `array`, `delegate` |

**5 Value types:** `int`, `double`, `bool`, `char`, `struct`  
**5 Reference types:** `string`, `class`, `array`, `delegate`, `object`

---

**Q9. What is boxing and unboxing? How to avoid it?**

- **Boxing:** Converting a value type to `object` (heap allocation).
- **Unboxing:** Casting it back to value type.

```csharp
int x = 42;
object boxed = x;       // Boxing — heap allocation
int unboxed = (int)boxed; // Unboxing
```

**Avoid by:** Using **generics** (`List<int>` instead of `ArrayList`), which avoids heap allocation for value types.

---

**Q10. What is the difference between `==` and `.Equals()`?**

- `==` compares **values** for value types and **references** for reference types by default (unless the operator is overloaded).
- `.Equals()` compares **logical equality** as defined by the type; it can be overridden to compare object contents instead of references.

#### Example

```csharp
class Person
{
    public string Name { get; set; }

    public override bool Equals(object obj)
    {
        return obj is Person p && Name == p.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}

var p1 = new Person { Name = "John" };
var p2 = new Person { Name = "John" };

Console.WriteLine(p1 == p2);      // False (different references)
Console.WriteLine(p1.Equals(p2)); // True  (same logical value)
```
---

**Q11. What is `Convert.ToString()` vs `.ToString()`?**

- `.ToString()` throws `NullReferenceException` if called on null.
- `Convert.ToString()` returns an **empty string** for null — safer.

```csharp
string s = null;
s.ToString();           // ❌ NullReferenceException
Convert.ToString(s);    // ✅ returns ""
```

---

**Q12. What are the types of constructors in C#?**

| Type | Description |
|------|-------------|
| **Default** | No params, auto-generated if none defined |
| **Parameterized** | Takes arguments to init fields |
| **Static** | Runs once, initializes static members, no access modifier |
| **Copy Constructor** | Takes same class as param, copies values |
| **Private** | Prevents instantiation (used in Singleton) |

```csharp
public class Car {
    public Car() {}                        // Default
    public Car(string model) {}            // Parameterized
    public Car(Car other) { Model = other.Model; } // Copy
    static Car() { /* static init */ }    // Static
    private Car(int secret) {}             // Private
}
```

---

**Q13. What is a Deconstructor in C#?**

A deconstructor lets you "unpack" an object into multiple variables using a `Deconstruct` method.

```csharp
public class Point {
    public int X { get; } public int Y { get; }
    public Point(int x, int y) { X = x; Y = y; }
    public void Deconstruct(out int x, out int y) { x = X; y = Y; }
}

var p = new Point(3, 5);
var (x, y) = p; // x = 3, y = 5
```

---

**Q14. What are properties and accessors?**

Properties are members that expose fields with controlled access using `get` and `set`.

```csharp
public class Person {
    private string _name;
    public string Name {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException();
    }
    public int Age { get; init; } // init-only (C# 9)
    public string Id { get; } = Guid.NewGuid().ToString(); // computed
}
```

---

**Q15. What is the difference between a field and a property?**

- **Field:** A raw variable stored in the class.
- **Property:** A controlled accessor with `get`/`set` logic, supports data binding, interfaces, and validation.

```csharp
public class Order {
    public decimal _total;         // Field — direct access, no control
    public decimal Total           // Property — controlled
    {
        get => _total;
        set => _total = value >= 0 ? value : throw new ArgumentException();
    }
}
```

---

**Q16. What is `const` vs `readonly` vs `static readonly`?**

| | `const` | `readonly` | `static readonly` |
|--|--|--|--|
| Value set at | Compile time | Runtime (constructor) | Runtime (static constructor) |
| Scope | Implicitly static | Per-instance | Shared across all instances |
| Type restriction | Primitives only | Any type | Any type |

```csharp
const double Pi = 3.14;                     // compile-time
readonly DateTime CreatedAt;                // set in constructor
static readonly HttpClient Client = new();  // shared, set once
```

---

**Q17. What is `ref` vs `out` vs `in` parameter?**

| Keyword | Must be initialized before? | Caller can read after? |
|---------|-----|----|
| `ref` | Yes | Yes |
| `out` | No | Yes (method must assign) |
| `in` | Yes | Read-only inside method |

```csharp
void WithRef(ref int x) { x += 1; }
void WithOut(out int x) { x = 10; }   // must assign
void WithIn(in int x) { /* x is read-only */ }

int a = 5; WithRef(ref a);  // a = 6
int b;     WithOut(out b);  // b = 10
int c = 3; WithIn(in c);    // c unchanged
```

---

**Q18. What is the `params` keyword?**

Allows passing a variable number of arguments as an array.

```csharp
int Sum(params int[] nums) => nums.Sum();

Sum(1, 2, 3);       // ✅
Sum(1, 2, 3, 4, 5); // ✅
Sum(new int[]{1,2}); // ✅ also works with array
```

---

**Q19. What is the difference between `var`, `dynamic`, and `object`?**

| | `var` | `dynamic` | `object` |
|--|--|--|--|
| Typed at | Compile time | Runtime | Compile time |
| Type safe | Yes | No | No (requires cast) |
| Performance | Fast | Slow (DLR) | Slow (boxing/casting) |

```csharp
var x = 42;          // int — inferred at compile time
dynamic d = "hello"; // any type — resolved at runtime
object o = 42;       // need to cast back: (int)o
```

---

**Q20. What are `string` vs `StringBuilder` vs string concatenation?**

- `string` is **immutable** — each `+` creates a new string.
- In a loop, use `StringBuilder` — it mutates in-place.

```csharp
// Bad: O(n²) allocations
string result = "";
for (int i = 0; i < 1000; i++) result += i;

// Good: O(n) single buffer
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) sb.Append(i);
string result = sb.ToString();
```

---

## 🔷 Section 2: OOP Concepts

---

**Q21. What are the 4 pillars of OOP with examples?**

- **Encapsulation:** Hide internal state, expose via properties.
- **Inheritance:** Child class inherits from parent.
- **Polymorphism:** Same method, different behavior at runtime.
- **Abstraction:** Hide complexity, expose what matters.

```csharp
// Encapsulation
class BankAccount { private decimal _balance; public void Deposit(decimal amt) => _balance += amt; }

// Inheritance
class Animal { public virtual void Speak() => Console.WriteLine("..."); }
class Dog : Animal { public override void Speak() => Console.WriteLine("Woof"); }

// Polymorphism
Animal a = new Dog(); a.Speak(); // "Woof"

// Abstraction
abstract class Shape { public abstract double Area(); }
```

---

**Q22. Abstract class (Is-A) vs Interface (Can-Do)?**

| | Abstract Class | Interface |
|--|--|--|
| Relationship | Is-A | Can-Do |
| Has implementation | Yes | Yes (C# 8+ default methods) |
| Multiple inheritance | ❌ | ✅ |
| Fields allowed | Yes | No |
| Use when | Shared base behavior | Defining capabilities/contracts |

```csharp
abstract class Animal { public abstract void Speak(); }  // Is-A
interface ISwimmable { void Swim(); }                    // Can-Do

class Duck : Animal, ISwimmable {
    public override void Speak() => Console.WriteLine("Quack");
    public void Swim() => Console.WriteLine("Swimming...");
}
```

---

**Q23. Why doesn't C# support multiple class inheritance?**

To avoid the **Diamond Problem** — if two parent classes define the same method, the child wouldn't know which to use. C# solves this by allowing multiple **interface** inheritance instead.

---

**Q24. What is the `base` keyword?**

Used to call the parent class constructor or method.

```csharp
class Vehicle {
    public Vehicle(string type) => Console.WriteLine($"Vehicle: {type}");
    public virtual void Start() => Console.WriteLine("Starting...");
}

class Car : Vehicle {
    public Car() : base("Car") {}  // call parent constructor
    public override void Start() {
        base.Start();              // call parent method
        Console.WriteLine("Car engine on.");
    }
}
```

---

**Q25. What are `virtual`, `abstract`, `override`, and `sealed` keywords?**

| Keyword | Meaning |
|---------|---------|
| `virtual` | Method can be overridden |
| `abstract` | Must be overridden, no body |
| `override` | Replaces parent's virtual/abstract method |
| `sealed` | Prevents further overriding (or class from being inherited) |

```csharp
class Base {
    public virtual void Run() {}
    public abstract void Init();   // Must override
}
class Child : Base {
    public override void Run() {}
    public sealed override void Init() {} // No one can override this further
}
```

---

**Q26. Method overloading vs overriding vs hiding?**

| | Overloading | Overriding | Hiding |
|--|--|--|--|
| Same name | Yes | Yes | Yes |
| Same signature | No | Yes | Yes |
| Polymorphism | No | Yes | No |
| Keyword | None | `override` | `new` |

```csharp
class Base { public virtual void Print() => Console.WriteLine("Base"); }

class Child : Base {
    public void Print(int x) {}     // Overloading
    public override void Print() {} // Overriding — polymorphic
    public new void Print() {}      // Hiding — hides base, NOT polymorphic
}
```

---

**Q27. What is method hiding (`new` keyword)?**

When a derived class defines a method with the same signature using `new`, it **hides** the base method rather than overriding it. The method called depends on the **reference type**, not the object type.

```csharp
class A { public void Show() => Console.WriteLine("A"); }
class B : A { public new void Show() => Console.WriteLine("B"); }

A obj = new B();
obj.Show(); // "A" — uses reference type (hiding, not polymorphism)
```

---

**Q28. What is early binding vs late binding?**

- **Early Binding:** Type resolved at **compile time** (normal method calls, overloaded methods).
- **Late Binding:** Type resolved at **runtime** (`virtual`/`override`, `dynamic`, reflection).

```csharp
// Early binding
Dog d = new Dog(); d.Speak();

// Late binding — resolved at runtime
dynamic obj = new Dog(); obj.Speak();
Animal a = new Dog(); a.Speak(); // virtual dispatch
```

---

**Q29. What is Is-A vs Has-A relationship?**

- **Is-A:** Inheritance — a `Dog` is an `Animal`.
- **Has-A:** Composition — a `Car` has an `Engine`.

```csharp
class Animal {}
class Dog : Animal {}          // Is-A

class Engine {}
class Car { Engine _engine = new Engine(); }  // Has-A (composition)
```

Prefer **composition over inheritance** for flexibility.

---

**Q30. What are extension methods?**

Add methods to existing types **without modifying** them or inheriting.

```csharp
public static class StringExtensions {
    public static bool IsEmail(this string s) => s.Contains("@");
}

// Usage
"user@mail.com".IsEmail(); // true
```

---

**Q31. What are sealed, static, partial, and abstract classes?**

| Class Type | Description |
|--|--|
| `sealed` | Cannot be inherited |
| `static` | Cannot be instantiated, only static members |
| `partial` | Split across multiple files |
| `abstract` | Cannot be instantiated, must be subclassed |

```csharp
sealed class Config {}          // No subclassing
static class MathHelper {}      // No instances
partial class MyForm {}         // Split across files
abstract class Shape {}         // Must be subclassed
```

---

**Q32. Covariance vs Contravariance vs Invariance?**

- **Covariance (`out`):** Use a more derived type than specified (read-only).
- **Contravariance (`in`):** Use a less derived type (write-only).
- **Invariance:** Only the exact type works.

```csharp
// Covariance — IEnumerable<Dog> can be used as IEnumerable<Animal>
IEnumerable<Dog> dogs = new List<Dog>();
IEnumerable<Animal> animals = dogs; // ✅ covariant

// Contravariance — Action<Animal> can be used as Action<Dog>
Action<Animal> act = a => Console.WriteLine(a);
Action<Dog> dogAct = act; // ✅ contravariant

// List<Dog> cannot be used as List<Animal> — invariant
List<Animal> list = new List<Dog>(); // ❌ compile error
```

---

**Q33. What are indexers in C#?**

Let you access objects using array-like syntax `[]`.

```csharp
class WordCollection {
    private string[] _words = new string[10];
    public string this[int i] {
        get => _words[i];
        set => _words[i] = value;
    }
}

var wc = new WordCollection();
wc[0] = "hello";
Console.WriteLine(wc[0]); // hello
```

---

**Q34. How to resolve ambiguity when two interfaces have the same method?**

Use **explicit interface implementation**.

```csharp
interface IFoo { void Print(); }
interface IBar { void Print(); }

class MyClass : IFoo, IBar {
    void IFoo.Print() => Console.WriteLine("Foo");
    void IBar.Print() => Console.WriteLine("Bar");
}

MyClass obj = new MyClass();
((IFoo)obj).Print(); // Foo
((IBar)obj).Print(); // Bar
```

---

## 🔷 Section 3: Memory, Types & Casting

---

**Q35. Stack vs Heap memory in C#?**

| | Stack | Heap |
|--|--|--|
| Stores | Value types, method frames | Reference types, boxed values |
| Managed by | Compiler (auto) | GC |
| Speed | Very fast | Slower |
| Size limit | Small (~1MB) | Large |

---

**Q36. Implicit vs Explicit casting?**

- **Implicit:** Safe, no data loss, done automatically.
- **Explicit:** May lose data, must be done manually.

```csharp
int i = 100;
double d = i;     // implicit — safe
int back = (int)d; // explicit — may truncate
```

---

**Q37. `as` operator vs `is` operator?**

- `is` — checks type, returns `bool`.
- `as` — tries cast, returns `null` if fails (no exception).

```csharp
object obj = "hello";

if (obj is string s) Console.WriteLine(s.ToUpper()); // pattern match + cast

string result = obj as string; // null if can't cast
if (result != null) Console.WriteLine(result);
```

---

**Q38. What is pattern matching in C#?**

Cleaner way to test and cast types using `is`, `switch`, or property patterns.

```csharp
// Type pattern
if (obj is Dog dog) dog.Bark();

// Switch pattern
string Describe(object o) => o switch {
    int i when i > 0 => "Positive int",
    string s => $"String: {s}",
    null => "null",
    _ => "Unknown"
};

// Property pattern
if (person is { Age: > 18, Name: "Alice" }) Console.WriteLine("Adult Alice");
```

---

**Q39. What is `Span<T>` and `Memory<T>`?**

They represent **slices of memory** without allocations — great for performance.

- `Span<T>` — stack-only, can't be stored in class fields.
- `Memory<T>` — can be used in async code and stored.

```csharp
byte[] buffer = new byte[1024];
Span<byte> slice = buffer.AsSpan(0, 100);  // No allocation
slice.Fill(0);                              // Zero out first 100 bytes

// Memory<T> for async
Memory<byte> mem = buffer.AsMemory(0, 100);
await stream.WriteAsync(mem);
```

---

**Q40. What are nullable reference types?**

Introduced in C# 8 — makes the compiler warn you when a reference type might be `null`, helping prevent `NullReferenceException`.

```csharp
#nullable enable

string name = "Alice";     // non-nullable — can't be null
string? nickname = null;   // nullable — might be null

Console.WriteLine(nickname?.ToUpper()); // safe with ?.
Console.WriteLine(nickname!.ToUpper()); // force — you promise it's not null
```

---

## 🔷 Section 4: Delegates, Events & Lambdas

---

**Q41. What are delegates? Singlecast vs Multicast?**

A delegate is a **type-safe function pointer** — it holds a reference to a method.

- **Singlecast:** Points to one method.
- **Multicast:** Points to multiple methods (using `+=`).

```csharp
delegate void Notify(string msg);

void Email(string msg) => Console.WriteLine($"Email: {msg}");
void SMS(string msg) => Console.WriteLine($"SMS: {msg}");

Notify n = Email;   // singlecast
n += SMS;           // multicast
n("Alert!");        // calls both
```

---

**Q42. What are `Func`, `Action`, and `Predicate`?**

Built-in delegate types so you don't need to define your own.

| Type | Returns | Use |
|--|--|--|
| `Action<T>` | void | For side-effect methods |
| `Func<T, TResult>` | TResult | For transformations |
| `Predicate<T>` | bool | For condition checks |

```csharp
Action<string> print = msg => Console.WriteLine(msg);
Func<int, int, int> add = (a, b) => a + b;
Predicate<int> isEven = n => n % 2 == 0;

print("hi");           // "hi"
Console.WriteLine(add(2, 3)); // 5
Console.WriteLine(isEven(4)); // true
```

---

**Q43. What are events in C#?**

Events are **delegate wrappers** that only allow `+=` and `-=` from outside the class — prevents accidental overwriting.

```csharp
public class Button {
    public event Action Clicked;
    public void Click() => Clicked?.Invoke();
}

var btn = new Button();
btn.Clicked += () => Console.WriteLine("Button clicked!");
btn.Click();
```

---

**Q44. What is a closure in C#?**

A lambda that **captures variables** from its enclosing scope — the variable stays alive as long as the lambda does.

```csharp
int multiplier = 3;
Func<int, int> triple = x => x * multiplier; // captures 'multiplier'
Console.WriteLine(triple(5)); // 15

// Pitfall in loops — captures the variable, not the value
var actions = new List<Action>();
for (int i = 0; i < 3; i++) {
    int captured = i;    // fix: capture a local copy
    actions.Add(() => Console.WriteLine(captured));
}
actions.ForEach(a => a()); // 0, 1, 2 ✅
```

---

## 🔷 Section 5: Exception Handling

---

**Q45. `throw` vs `throw ex` in exception handling?**

- `throw` re-throws with the **original stack trace** (correct).
- `throw ex` re-throws but **resets the stack trace** (loses origin info).

```csharp
try { DoWork(); }
catch (Exception ex) {
    Log(ex);
    throw;     // ✅ preserves stack trace
    // throw ex; ❌ loses original stack trace
}
```

---

**Q46. What is the order of catch blocks?**

Most specific exceptions first, most general last. `Exception` must be last.

```csharp
try { /* ... */ }
catch (FileNotFoundException ex) { /* most specific */ }
catch (IOException ex) { }
catch (Exception ex) { /* most general — must be last */ }
finally { /* always runs */ }
```

---

**Q47. `finally` vs `Finalize` vs `Dispose`?**

| | `finally` | `Finalize` | `Dispose` |
|--|--|--|--|
| What | Block in try/catch | Destructor (~ClassName) | `IDisposable` method |
| When runs | Always after try/catch | When GC collects | When called explicitly |
| Use for | Cleanup code | Unmanaged cleanup fallback | Releasing resources |

```csharp
// finally
try { } finally { Console.WriteLine("always runs"); }

// Finalize (destructor)
class Resource { ~Resource() { /* called by GC */ } }

// Dispose
class Resource : IDisposable {
    public void Dispose() { /* free resources now */ }
}
using (var r = new Resource()) { } // Dispose called automatically
```

---

**Q48. How to ensure unmanaged resources don't leak?**

Implement `IDisposable` and use the dispose pattern. Use `using` statement to auto-call `Dispose`.

```csharp
public class FileWrapper : IDisposable {
    private FileStream _stream;
    private bool _disposed = false;

    public FileWrapper(string path) => _stream = File.OpenRead(path);

    public void Dispose() {
        if (!_disposed) {
            _stream?.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }

    ~FileWrapper() => Dispose(); // fallback
}

using var fw = new FileWrapper("data.txt"); // auto-disposed
```

---

## 🔷 Section 6: Generics & Collections

---

**Q49. What are generics? What are constraints?**

Generics let you write type-safe code that works with any type, avoiding boxing and casting.

```csharp
// Generic method
T Max<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b) > 0 ? a : b;

Max(3, 5);       // int
Max("a", "b");   // string

// Constraints
void Print<T>(T item) where T : class, new() { }
// where T : struct       — value type only
// where T : class        — reference type only
// where T : new()        — has parameterless constructor
// where T : SomeClass    — must inherit SomeClass
// where T : ISomeInterface — must implement interface
```

---

**Q50. Generic collections vs non-generic collections?**

| Generic | Non-Generic | Difference |
|--|--|--|
| `List<T>` | `ArrayList` | Generics are type-safe, no boxing |
| `Dictionary<K,V>` | `Hashtable` | Type safe, faster |
| `Queue<T>` | `Queue` | Type safe |
| `Stack<T>` | `Stack` | Type safe |

Always prefer generic collections.

---

**Q51. `IEnumerable` vs `ICollection` vs `IList` vs `IQueryable`?**

| Interface | Can enumerate | Count | Add/Remove | Index | Query on DB |
|--|--|--|--|--|--|
| `IEnumerable<T>` | ✅ | ❌ | ❌ | ❌ | ❌ |
| `ICollection<T>` | ✅ | ✅ | ✅ | ❌ | ❌ |
| `IList<T>` | ✅ | ✅ | ✅ | ✅ | ❌ |
| `IQueryable<T>` | ✅ | ✅ | ❌ | ❌ | ✅ |

`IQueryable` translates queries to SQL. `IEnumerable` evaluates in-memory.

---

**Q52. `IEnumerable<T>` vs `IEnumerator<T>`?**

- `IEnumerable<T>` — a collection that *can be iterated* (has `GetEnumerator()`).
- `IEnumerator<T>` — the *iterator itself* (has `Current`, `MoveNext()`, `Reset()`).

```csharp
// IEnumerable gives you the enumerator
IEnumerable<int> list = new List<int> { 1, 2, 3 };

// IEnumerator does the actual iteration
IEnumerator<int> e = list.GetEnumerator();
while (e.MoveNext()) Console.WriteLine(e.Current);
```

---

**Q53. `List<T>` vs `LinkedList<T>` vs `Array`?**

| | Array | List<T> | LinkedList<T> |
|--|--|--|--|
| Size | Fixed | Dynamic | Dynamic |
| Access by index | O(1) | O(1) | O(n) |
| Insert/Delete (middle) | O(n) | O(n) | O(1) |
| Memory | Compact | Compact | Node overhead |

---

**Q54. `HashSet<T>` vs `List<T>` and `Dictionary<K,V>` vs `Hashtable`?**

- `HashSet<T>` — no duplicates, O(1) lookup.
- `List<T>` — allows duplicates, O(n) lookup.
- `Dictionary<K,V>` — generic, type-safe, O(1) lookup.
- `Hashtable` — non-generic, boxing, not type-safe.

```csharp
var set = new HashSet<int> { 1, 2, 3 };
set.Add(2); // ignored — already exists

var dict = new Dictionary<string, int> { ["age"] = 25 };
dict["age"]; // 25 — O(1)
```

---

**Q55. What are concurrent collections in C#?**

Thread-safe collections in `System.Collections.Concurrent`:

| Collection | Use Case |
|--|--|
| `ConcurrentDictionary<K,V>` | Thread-safe key-value store |
| `ConcurrentQueue<T>` | Thread-safe FIFO |
| `ConcurrentStack<T>` | Thread-safe LIFO |
| `ConcurrentBag<T>` | Unordered, thread-safe bag |
| `BlockingCollection<T>` | Producer-consumer with blocking |

```csharp
var dict = new ConcurrentDictionary<string, int>();
dict.AddOrUpdate("count", 1, (key, old) => old + 1);
```

---

**Q56. Shallow copy vs deep copy?**

- **Shallow copy:** Copies the object but reference members still point to the same objects.
- **Deep copy:** Copies everything, including referenced objects.

```csharp
// Shallow — Address is shared
Person a = new Person { Name = "Alice", Address = new Address { City = "NY" } };
Person b = (Person)a.MemberwiseClone(); // shallow
b.Address.City = "LA"; // also changes a.Address.City!

// Deep — Address is duplicated
Person c = new Person { Name = a.Name, Address = new Address { City = a.Address.City } };
```

---

## 🔷 Section 7: LINQ

---

**Q57. What is LINQ? What is deferred vs immediate execution?**

LINQ (Language Integrated Query) lets you query collections using SQL-like syntax.

- **Deferred execution:** Query is not run until you iterate it (`Where`, `Select`).
- **Immediate execution:** Runs right away (`ToList()`, `Count()`, `First()`).

```csharp
var nums = new[] { 1, 2, 3, 4, 5 };

// Deferred — not executed yet
var query = nums.Where(x => x > 2);

nums = new[] { 10, 20 }; // change source

foreach (var n in query) Console.Write(n); // evaluates NOW, with new source

// Immediate
var result = nums.Where(x => x > 2).ToList(); // runs immediately
```

---

**Q58. Types of LINQ operators?**

| Category | Examples |
|--|--|
| Filtering | `Where`, `OfType` |
| Projection | `Select`, `SelectMany` |
| Sorting | `OrderBy`, `ThenBy`, `OrderByDescending` |
| Grouping | `GroupBy` |
| Joining | `Join`, `GroupJoin` |
| Aggregation | `Count`, `Sum`, `Min`, `Max`, `Average` |
| Set | `Distinct`, `Union`, `Intersect`, `Except` |
| Element | `First`, `FirstOrDefault`, `Single`, `SingleOrDefault` |
| Conversion | `ToList`, `ToArray`, `ToDictionary` |

---

**Q59. `FirstOrDefault` vs `SingleOrDefault`?**

- `FirstOrDefault` — returns the **first** match or null. OK with multiple matches.
- `SingleOrDefault` — returns the match, but **throws** if more than one match.

```csharp
var list = new[] { 1, 2, 2, 3 };

list.FirstOrDefault(x => x == 2);   // 2 — first match
list.SingleOrDefault(x => x == 2);  // ❌ throws — more than one match
list.SingleOrDefault(x => x == 3);  // 3 — exactly one match ✅
```

---

**Q60. `IEnumerable<T>` vs `IQueryable<T>` in LINQ?**

- `IEnumerable<T>` — in-memory, all data loaded then filtered.
- `IQueryable<T>` — translates `Where`/`Select` to SQL via expression trees, filters at DB.

```csharp
// IEnumerable — loads ALL users, then filters in C#
IEnumerable<User> users = db.Users.ToList();
var result = users.Where(u => u.Age > 18);

// IQueryable — sends WHERE age > 18 to the database
IQueryable<User> query = db.Users.Where(u => u.Age > 18);
var result = query.ToList();
```

Always use `IQueryable` for database queries to avoid loading unnecessary data.

---

**Q61. How to do pagination with LINQ?**

```csharp
int page = 2, pageSize = 10;

var pagedResults = db.Products
    .OrderBy(p => p.Id)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToList();
```

---

**Q62. A LINQ query is slow. How to troubleshoot?**

- Check if it's `IEnumerable` loading all data (switch to `IQueryable`).
- Look for N+1 query — use `.Include()` for EF Core.
- Use `.AsNoTracking()` for read-only queries.
- Check if indexes exist on filtered/sorted columns.
- Log SQL with `db.Database.Log` or a profiler.

```csharp
// Bad — N+1
foreach (var order in db.Orders) { var user = db.Users.Find(order.UserId); }

// Good — single query with join
var result = db.Orders.Include(o => o.User).ToList();
```

---

## 🔷 Section 8: Async & Threading

---

**Q63. Synchronous vs Asynchronous programming?**

- **Sync:** Waits for each operation before moving on. Blocks the thread.
- **Async:** Starts an operation, **does other work** while waiting, resumes when done.

```csharp
// Sync — thread blocked waiting for file
string content = File.ReadAllText("big.txt");

// Async — thread freed while waiting
string content = await File.ReadAllTextAsync("big.txt");
```

---

**Q64. What are `async` and `await`? How do they work under the hood?**

`async` marks a method as asynchronous. `await` suspends execution until the awaited task completes — **without blocking the thread**.

Under the hood, the compiler converts `async` methods into a **state machine** that resumes from where it left off.

```csharp
public async Task<string> FetchDataAsync() {
    var client = new HttpClient();
    string data = await client.GetStringAsync("https://api.example.com");
    return data.ToUpper();
}
```

**Pitfalls:**
- `async void` — can't be awaited, swallows exceptions.
- Deadlock with `.Result` or `.Wait()` in sync context.
- Forgetting `await` makes the task run but errors go unobserved.

---

**Q65. `async void` vs `async Task`? When to use each?**

| | `async Task` | `async void` |
|--|--|--|
| Can be awaited | ✅ | ❌ |
| Exceptions propagate | ✅ | ❌ (crashes app) |
| Use case | All async methods | Event handlers only |

```csharp
// ❌ Avoid — exceptions silently swallowed
async void LoadData() { await FetchAsync(); }

// ✅ Correct
async Task LoadDataAsync() { await FetchAsync(); }

// ✅ Only valid use of async void
button.Click += async (s, e) => { await LoadDataAsync(); };
```

---

**Q66. What is the difference between `Task`, `Task<T>`, and `ValueTask<T>`?**

| | `Task` | `Task<T>` | `ValueTask<T>` |
|--|--|--|--|
| Returns | Nothing (like void) | A value | A value, lower allocation |
| When to use | Fire and wait | Return a result | Result often available synchronously |

```csharp
async Task SaveAsync() { await db.SaveChangesAsync(); }
async Task<User> GetUserAsync(int id) { return await db.Users.FindAsync(id); }
async ValueTask<int> CachedCountAsync() {
    if (_cached) return _count; // no heap allocation
    return await db.Items.CountAsync();
}
```

---

**Q67. `Task.Run`, `Task.Delay`, and `Thread.Sleep` — when to use each?**

| | `Task.Run` | `Task.Delay` | `Thread.Sleep` |
|--|--|--|--|
| Use for | CPU-bound work on thread pool | Async wait (non-blocking) | Blocking wait (avoid) |

```csharp
// CPU-bound — push to thread pool
await Task.Run(() => HeavyComputation());

// Non-blocking wait
await Task.Delay(1000); // frees thread while waiting

// Blocking — holds thread
Thread.Sleep(1000); // ❌ avoid in async code
```

---

**Q68. Difference between `Task`, `Thread`, and `Process`?**

| | Thread | Task | Process |
|--|--|--|--|
| Level | OS thread | Abstraction over thread pool | OS process |
| Weight | Heavy | Light | Very heavy |
| Returns value | ❌ | ✅ | ❌ |

```csharp
// Thread — manual, low-level
var t = new Thread(() => DoWork()); t.Start();

// Task — preferred, uses thread pool
var task = Task.Run(() => DoWork());

// Process — separate program
Process.Start("notepad.exe");
```

---

**Q69. `Task.WhenAll` vs `Task.WhenAny`?**

- `WhenAll` — waits for **all** tasks to complete.
- `WhenAny` — waits until **any one** completes.

```csharp
// Run all in parallel, wait for all
var t1 = FetchUserAsync();
var t2 = FetchOrdersAsync();
await Task.WhenAll(t1, t2);

// Return whichever finishes first (e.g., timeout pattern)
var fetch = FetchDataAsync();
var timeout = Task.Delay(5000);
var done = await Task.WhenAny(fetch, timeout);
if (done == timeout) throw new TimeoutException();
```

---

**Q70. `Task.FromResult`, `Task.FromException`, `Task.FromCanceled`?**

Create already-completed tasks without async overhead.

```csharp
// Cached value — no async needed
Task<int> GetCount() => Task.FromResult(42);

// Already failed
Task Fail() => Task.FromException(new InvalidOperationException("Oops"));

// Already canceled
Task Cancel(CancellationToken ct) => Task.FromCanceled(ct);
```

---

**Q71. Can you use `await` in `catch` or `finally` blocks?**

Yes, since C# 6.

```csharp
try {
    await DoWorkAsync();
}
catch (Exception ex) {
    await LogErrorAsync(ex);   // ✅ since C# 6
}
finally {
    await CleanupAsync();      // ✅ since C# 6
}
```

---

**Q72. What is `Thread.Sleep()` vs `Thread.Join()`?**

- `Thread.Sleep(ms)` — pauses the **current** thread for ms milliseconds.
- `Thread.Join()` — makes the **calling thread wait** until another thread finishes.

```csharp
Thread t = new Thread(() => { Thread.Sleep(1000); Console.WriteLine("Done"); });
t.Start();
t.Join(); // main thread waits here until t finishes
```

---

**Q73. Race condition vs Deadlock?**

- **Race condition:** Two threads access shared data simultaneously — result depends on timing.
- **Deadlock:** Two threads each wait for a lock the other holds — both stuck forever.

```csharp
// Race condition — unsynchronized counter
int count = 0;
Parallel.For(0, 1000, _ => count++); // wrong result!

// Fixed with Interlocked
Parallel.For(0, 1000, _ => Interlocked.Increment(ref count));

// Deadlock scenario
lock(A) { lock(B) {} }  // Thread 1
lock(B) { lock(A) {} }  // Thread 2 — deadlock!
```

**Avoid with:** consistent lock ordering, `SemaphoreSlim`, `Monitor.TryEnter` with timeout.

---

**Q74. What is the Producer-Consumer problem?**

One or more producers add items, one or more consumers remove items from a shared buffer. The challenge: don't consume empty buffer, don't overflow full buffer.

```csharp
var collection = new BlockingCollection<int>(boundedCapacity: 10);

// Producer
Task.Run(() => {
    for (int i = 0; i < 100; i++) { collection.Add(i); }
    collection.CompleteAdding();
});

// Consumer
Task.Run(() => {
    foreach (var item in collection.GetConsumingEnumerable())
        Console.WriteLine(item);
});
```

---

**Q75. What is a Semaphore in C#?**

A semaphore controls access to a resource by allowing only N threads at a time (like a bouncer with N slots).

```csharp
var semaphore = new SemaphoreSlim(3); // max 3 concurrent

async Task ProcessAsync(int id) {
    await semaphore.WaitAsync();
    try { await DoWorkAsync(id); }
    finally { semaphore.Release(); }
}

// Only 3 run at once
await Task.WhenAll(Enumerable.Range(1, 10).Select(ProcessAsync));
```

---

**Q76. What is a CancellationToken?**

Used to cooperatively cancel long-running or async operations.

```csharp
async Task FetchAsync(CancellationToken ct) {
    for (int i = 0; i < 1000; i++) {
        ct.ThrowIfCancellationRequested(); // check each iteration
        await Task.Delay(100, ct);
    }
}

var cts = new CancellationTokenSource();
cts.CancelAfter(2000); // cancel after 2s
try { await FetchAsync(cts.Token); }
catch (OperationCanceledException) { Console.WriteLine("Cancelled!"); }
```

---

**Q77. What is `Parallel.ForEach` vs PLINQ vs `Parallel.For`?**

| | Use For |
|--|--|
| `Parallel.For` | Numeric index loops |
| `Parallel.ForEach` | Collections |
| PLINQ (`.AsParallel()`) | LINQ queries |

```csharp
// Parallel.For
Parallel.For(0, 100, i => Process(i));

// Parallel.ForEach
Parallel.ForEach(items, item => Process(item));

// PLINQ
var results = items.AsParallel().Where(x => x > 5).ToList();
```

Use when work is **CPU-bound and independent**. Not for I/O.

---

**Q78. Fire-and-forget tasks failing silently. How to fix?**

Unobserved exceptions in fire-and-forget tasks swallow silently. Fix:

```csharp
// ❌ Bad — exception lost
Task.Run(() => DoWork());

// ✅ Good — handle exceptions
_ = Task.Run(async () => {
    try { await DoWorkAsync(); }
    catch (Exception ex) { await LogAsync(ex); }
});

// Or extension method
public static async void FireAndForget(this Task task, Action<Exception> onError = null) {
    try { await task; }
    catch (Exception ex) { onError?.Invoke(ex); }
}
```

---

## 🔷 Section 9: Design Principles & Patterns

---

**Q79. SOLID Principles?**

| Letter | Principle | Meaning |
|--|--|--|
| S | Single Responsibility | A class should have one reason to change |
| O | Open/Closed | Open for extension, closed for modification |
| L | Liskov Substitution | Subtypes must be substitutable for base types |
| I | Interface Segregation | Don't force clients to depend on methods they don't use |
| D | Dependency Inversion | Depend on abstractions, not concretions |

```csharp
// D — DI example
// ❌ Bad
class OrderService { var db = new SqlDatabase(); }

// ✅ Good — depend on interface
class OrderService {
    private readonly IDatabase _db;
    public OrderService(IDatabase db) { _db = db; }
}
```

---

**Q80. What happens when Liskov Substitution is violated?**

Subclasses that break expected behavior cause bugs when used polymorphically.

```csharp
// Violation — Square breaks Rectangle's contract
class Rectangle { public virtual int Width { get; set; } public virtual int Height { get; set; } }
class Square : Rectangle {
    public override int Width { set { base.Width = base.Height = value; } }
}

// Caller expects Width and Height independent
Rectangle r = new Square();
r.Width = 5; r.Height = 3;
Console.WriteLine(r.Width * r.Height); // Expected 15, got 9!
```

---

**Q81. Singleton Pattern?**

Ensures only one instance of a class exists.

```csharp
public sealed class AppConfig {
    private static readonly Lazy<AppConfig> _instance =
        new Lazy<AppConfig>(() => new AppConfig());

    private AppConfig() {}

    public static AppConfig Instance => _instance.Value;
    public string Theme { get; set; } = "Dark";
}

// Usage
AppConfig.Instance.Theme = "Light";
```

---

**Q82. Repository Pattern?**

Abstracts data access behind an interface — swappable implementations (DB, memory, API).

```csharp
public interface IUserRepository {
    Task<User> GetByIdAsync(int id);
    Task SaveAsync(User user);
}

public class SqlUserRepository : IUserRepository {
    private readonly AppDbContext _db;
    public SqlUserRepository(AppDbContext db) => _db = db;
    public async Task<User> GetByIdAsync(int id) => await _db.Users.FindAsync(id);
    public async Task SaveAsync(User user) { _db.Users.Add(user); await _db.SaveChangesAsync(); }
}
```

---

**Q83. Dependency Injection vs `new` keyword?**

| | `new` | DI |
|--|--|--|
| Coupling | Tight | Loose |
| Testable | Hard to mock | Easy to mock |
| Lifecycle | You manage | Framework manages |

```csharp
// ❌ Tight coupling — can't swap or mock
class OrderService { var repo = new SqlRepository(); }

// ✅ DI — inject via constructor
class OrderService {
    private readonly IRepository _repo;
    public OrderService(IRepository repo) { _repo = repo; } // inject
}
```

---

**Q84. What are Design Pattern types?**

| Category | Examples |
|--|--|
| **Creational** | Singleton, Factory, Builder, Prototype |
| **Structural** | Adapter, Decorator, Proxy, Facade, Composite |
| **Behavioral** | Observer, Strategy, Command, Iterator, Template Method |

---

## 🔷 Section 10: Access Modifiers & More

---

**Q85. Access Modifiers in C#?**

| Modifier | Accessible From |
|--|--|
| `public` | Anywhere |
| `private` | Same class only |
| `protected` | Same class + subclasses |
| `internal` | Same assembly |
| `protected internal` | Same assembly OR subclasses |
| `private protected` | Same class + subclasses in same assembly |

---

**Q86. What are Records in C#? When to use them?**

Records are immutable reference types with value-based equality, ideal for DTOs and data models.

```csharp
public record Person(string Name, int Age);

var p1 = new Person("Alice", 30);
var p2 = new Person("Alice", 30);

Console.WriteLine(p1 == p2); // true — value equality
Console.WriteLine(p1);       // Person { Name = Alice, Age = 30 }

// Non-destructive mutation
var p3 = p1 with { Age = 31 };
```

Use for: API responses, domain events, value objects.

---

**Q87. `enum` vs `struct` vs `class`?**

| | `enum` | `struct` | `class` |
|--|--|--|--|
| Type | Value | Value | Reference |
| Inherits | `Enum` | `ValueType` | `object` |
| Use for | Named constants | Small, immutable data | Complex objects |

```csharp
enum Status { Active, Inactive }
struct Point { public int X, Y; }
class Person { public string Name; public string Email; }
```

---

**Q88. What are attributes in C#? How to create custom attributes?**

Attributes add **metadata** to code, inspectable via reflection.

```csharp
// Custom attribute
[AttributeUsage(AttributeTargets.Method)]
public class LogAttribute : Attribute {
    public string Level { get; }
    public LogAttribute(string level) => Level = level;
}

// Usage
[Log("Info")]
public void DoWork() { }

// Read via reflection
var method = typeof(MyClass).GetMethod("DoWork");
var attr = method.GetCustomAttribute<LogAttribute>();
Console.WriteLine(attr.Level); // "Info"
```

---

**Q89. Null-coalescing `??` and null-conditional `?.` operators?**

```csharp
string name = null;

// ?. — safe navigation (returns null instead of throwing)
int? length = name?.Length; // null, not NullReferenceException

// ?? — default if null
string display = name ?? "Anonymous"; // "Anonymous"

// ??= — assign if null
name ??= "Default"; // assigns "Default" if name is null

// Chaining
string city = user?.Address?.City ?? "Unknown";
```

---

**Q90. `readonly` property vs `init`-only property?**

- `readonly` field — can only be set in constructor or declaration.
- `init` property — can be set during object initialization but not after.

```csharp
public class Config {
    public readonly string Version = "1.0"; // set at declaration or constructor only

    public string Name { get; init; } // set only at object init
}

var c = new Config { Name = "MyApp" }; // ✅
c.Name = "Other"; // ❌ compile error — init only
```

---

**Q91. Managed vs Unmanaged code?**

- **Managed:** Runs under CLR control — GC, type safety, exceptions. All normal C# code.
- **Unmanaged:** Native code (C/C++), COM objects, OS APIs. No GC — you manage memory.

```csharp
// Unmanaged resource — requires manual cleanup
[DllImport("kernel32.dll")]
static extern IntPtr OpenProcess(int access, bool inherit, int pid);

// Always wrap in IDisposable
```

---

**Q92. What is Serialization and Deserialization?**

Converting objects to a storable/transmittable format (JSON, XML, binary) and back.

```csharp
using System.Text.Json;

var user = new User { Name = "Alice", Age = 30 };

// Serialize to JSON
string json = JsonSerializer.Serialize(user);
// {"Name":"Alice","Age":30}

// Deserialize back
User restored = JsonSerializer.Deserialize<User>(json);
```

---

**Q93. What are code smells? Give examples.**

Code smells are patterns that signal poorly written code — not bugs, but hints for refactoring.

| Smell | Example |
|--|--|
| Long method | 200-line method doing too much |
| God class | Class with 50+ responsibilities |
| Magic numbers | `if (status == 3)` — what's 3? |
| Deep nesting | 5 levels of if/for |
| Duplicate code | Same logic in 3 places |
| Feature envy | Class using another class's data more than its own |

---

**Q94. What is `IDisposable` vs `IAsyncDisposable`?**

- `IDisposable.Dispose()` — synchronous cleanup.
- `IAsyncDisposable.DisposeAsync()` — async cleanup (e.g., flushing async streams).

```csharp
// Sync
class Conn : IDisposable { public void Dispose() => Close(); }
using (var conn = new Conn()) { }

// Async
class AsyncConn : IAsyncDisposable {
    public async ValueTask DisposeAsync() => await CloseAsync();
}
await using (var conn = new AsyncConn()) { }
```

---

**Q95. `String.Intern` and String interning?**

The CLR maintains a string pool. Interned strings with the same value share the same memory reference.

```csharp
string a = "hello";
string b = "hello"; // same literal — same reference
Console.WriteLine(ReferenceEquals(a, b)); // true (interned)

string c = new string("hello".ToCharArray());
Console.WriteLine(ReferenceEquals(a, c)); // false (heap allocated)

string interned = string.Intern(c);
Console.WriteLine(ReferenceEquals(a, interned)); // true
```

---

**Q96. What is `yield return` and iterator methods?**

`yield return` produces values one at a time (lazy), without building a full collection.

```csharp
IEnumerable<int> GetNumbers() {
    for (int i = 0; i < 1000000; i++)
        yield return i; // pauses here, resumes on next iteration
}

foreach (var n in GetNumbers()) {
    if (n > 10) break; // only generates 11 items — lazy!
}
```

---

**Q97. What is the difference between `static` class and `Singleton`?**

| | Static Class | Singleton |
|--|--|--|
| Instance | None | One instance |
| Interface | Can't implement | Can implement |
| DI injectable | ❌ | ✅ |
| Testable | Hard | Yes |
| State | Global | Controlled |

Use Singleton over static when you need DI, interfaces, or testability.

---

**Q98. What are expression trees in C#?**

Expression trees represent code **as data** (a tree of objects), used by LINQ-to-SQL to translate queries.

```csharp
// Lambda
Func<int, bool> func = x => x > 5;

// Expression tree — inspectable, translatable
Expression<Func<int, bool>> expr = x => x > 5;

// EF Core uses expression trees to convert to SQL:
// db.Users.Where(u => u.Age > 18) → SELECT * WHERE Age > 18
```

---

**Q99. What is `dynamic` dispatch vs reflection?**

Both allow runtime behavior, but differ in performance and usage.

```csharp
// dynamic — uses DLR, cleaner syntax
dynamic obj = GetSomeObject();
obj.DoWork(); // resolved at runtime

// Reflection — more control, more verbose
Type type = obj.GetType();
type.GetMethod("DoWork").Invoke(obj, null);
```

`dynamic` is faster to write. Reflection gives more control (discover, inspect, invoke dynamically).

---

**Q100. What are `Task.Factory` and `Task.Yield`?**

- `Task.Factory.StartNew` — creates tasks with fine-grained control (scheduler, options, parent).
- `Task.Yield` — yields control back to the caller, allowing other work to run, then resumes.

```csharp
// Task.Factory — use when you need custom scheduler or long-running task
Task.Factory.StartNew(
    () => LongRunningWork(),
    TaskCreationOptions.LongRunning // hint: use dedicated thread
);

// Task.Yield — relinquish control briefly (useful in tight async loops)
async Task ProcessAsync() {
    for (int i = 0; i < 1000000; i++) {
        DoWork(i);
        if (i % 100 == 0)
            await Task.Yield(); // let other tasks run
    }
}
```

---

*End of 100 C# Interview Questions — 5 Years Experience*