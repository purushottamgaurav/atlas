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

**Q3. How does Garbage Collector work in C#?**

- Uses **.NET CLR GC** — automatic, generational
- **3 Generations:**
  - **Gen 0** → new objects, collected most often
  - **Gen 1** → short-lived survivors
  - **Gen 2** → long-lived objects, collected rarely
- **LOH (Large Object Heap)** → objects > 85KB, collected with Gen 2
- Internally runs **Mark → Compact → Update references**
- GC handles **managed memory only** — use `IDisposable` + `using` for unmanaged resources

```csharp
using (var conn = new SqlConnection(connStr))
{
    // auto-disposed at end of block
}
```

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

| Type | Description | Example | Use case |
|------|-------------|---------|----------|
| **Default** | No params; auto-generated if none defined | `new Car()` | Objects with sensible defaults |
| **Parameterized** | Takes arguments to initialize fields | `new Car("BMW", 2024)` | Enforcing required data at creation |
| **Static** | Runs once; initializes static members; no access modifier | `static Car() { … }` | One-time setup like config loading |
| **Copy** | Takes same class instance, copies its values | `new Car(existingCar)` | Cloning objects before mutation |
| **Private** | Prevents instantiation from outside the class | `private Car() { … }` | Singleton / factory pattern |


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

| Keyword | Must be initialized before? | Caller can read after? | Use case |
|---------|-----------------------------|------------------------|----------|
| `ref` | Yes | Yes | Pass a value to modify in place — e.g. swapping two variables |
| `out` | No | Yes (method must assign) | Return multiple values — e.g. `int.TryParse(str, out int result)` |
| `in` | Yes | Read-only inside method | Like `ref` but method can't modify it — e.g. pass a `DateTime` struct to a `Calculate()` method by reference without copying or risking changes |

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
| Use case | Everyday local variables where type is obvious — e.g. `var list = new List<int>()` | Interop with COM, JSON, or Python/JS — e.g. reading Excel via COM API where return type is unknown | Pre-generics collections like `ArrayList` stored everything as `object` — must cast back manually, can crash at runtime |

```csharp
var x = 42;          // int — inferred at compile time
dynamic d = "hello"; // any type — resolved at runtime
object o = 42;       // need to cast back: (int)o
```

---

## Q20. What are `string` vs `String` vs `StringBuilder` vs string concatenation?
 
| | `string` / `String` | `StringBuilder` | Concatenation (`+`) |
|--|--|--|--|
| Mutable | No | Yes | No |
| Performance | Fast for few ops | Fast for many ops | Slow in loops — O(n²) |
| Use case | Fixed text, comparisons | Building strings in loops | Joining 2–3 values only |
 
> `string` and `String` are identical — `string` is just a C# alias for `System.String`.
 
```csharp
// Bad: creates a new string object on every iteration — O(n²) allocations
string result = "";
for (int i = 0; i < 1000; i++) result += i;
 
// Good: mutates a single buffer in-place — O(n)
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) sb.Append(i);
string result = sb.ToString();
```
 
**Simple rule:** Use `+` for 2–3 values. Use `StringBuilder` the moment you're in a loop.

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

**Q23. Why doesn't C# support multiple class inheritance? What are the types?**
 
C# blocks multiple class inheritance to avoid the **Diamond Problem** — ambiguity when two parents define the same method. Use interfaces instead.
 
#### Types of Inheritance in C#
 
| Type | Supported | Example |
|------|-----------|---------|
| **Single** | Yes | `class Dog : Animal` |
| **Multilevel** | Yes | `class Puppy : Dog` |
| **Hierarchical** | Yes | `Dog : Animal`, `Cat : Animal` |
| **Multiple (class)** | No ❌ | `class Dog : Animal, Pet` |
| **Multiple (interface)** | Yes ✅ | `class Dog : IAnimal, IPet` |

---

**Q24. What is `base` and `this` keyword?**


| | `base` | `this` |
|--|--|--|
| Refers to | Parent class | Current class |
| Use case | Call parent constructor/method | Chain constructors, current instance |
 
```csharp
class Dog : Animal
{
    public Dog(string name) : base(name) { }       // base — calls Animal's constructor
    public Dog() : this("Unknown") { }              // this — chains to Dog(string)
 
    public override void Speak()
    {
        base.Speak();          // runs Animal.Speak() first
        Console.WriteLine("Woof!");
    }
}
```
 
#### `this` in extension methods
Marks which type the method extends.
 
```csharp
public static bool IsEven(this int n) => n % 2 == 0;
4.IsEven();  // true
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

**Q27. Class vs Struct vs Record vs Enum in C#**
  
| | `class` | `struct` | `record` | `enum` |
|--|--|--|--|--|
| Type | Reference | Value | Reference (`record`) / Value (`record struct`) | Value |
| Stored in | Heap | Stack | Heap / Stack | Stack |
| Mutable | Yes | Yes | No (default) | No |
| Inheritance | Yes | No | Yes | No |
| Equality | Reference | Value | Value (auto) | Value |
| `null` allowed | Yes | No | Yes | No |
| Best for | Objects & behavior | Small, short-lived data | Immutable data models | Named constants |
 
```csharp
class Person { public string Name { get; set; } }           // mutable, heap
struct Point { public int X, Y; }                           // copied on assignment
record Product(string Name, decimal Price);                  // reference type, value equality
record struct Point3D(int X, int Y, int Z);                  // value type, value equality
enum Status { Active, Inactive, Pending }                    // named constants
```
 
#### Key rules
- `struct` is **copied** on assignment; `class` shares the same reference
- `record` → reference type; `record struct` → value type — both get **value equality** and **immutability**
- `enum` — no methods, no properties; just named integer constants

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

**Q29. What is Is-A vs Has-A vs Can-Do relationship?**

| | Is-a | Has-a | Can-do |
|--|--|--|--|
| Relationship | Inheritance | Composition | Interface |
| Keyword | `class Dog : Animal` | `class Car { Engine _engine; }` | `class Bird : IFlyable` |
| Meaning | Dog **is a** type of Animal | Car **has an** Engine inside it | Bird **can** fly |
| Couples tightly? | Yes | No | No |
| Use case | Shared base behavior — e.g. `Animal` has `Eat()`, `Dog` inherits it | Building complex objects from smaller parts — e.g. `Car` owns `Engine`, `Wheels` | Adding capabilities across unrelated types — e.g. both `Bird` and `Plane` can implement `IFlyable` |
 
```csharp
// Is-a — inheritance
class Animal { public void Eat() { } }
class Dog : Animal { }  // Dog is an Animal
 
// Has-a — composition
class Engine { public void Start() { } }
class Car { private Engine _engine = new Engine(); }  // Car has an Engine
 
// Can-do — interface
interface IFlyable { void Fly(); }
class Bird : IFlyable { public void Fly() { } }   // Bird can fly
class Plane : IFlyable { public void Fly() { } }  // Plane can fly too
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

#### Pros & Cons
 
| Pros | Cons |
|------|------|
| Extend sealed/third-party types | No access to private members |
| Fluent, clean syntax | Clutters intellisense if overused |
| LINQ is built on them | Hard to find where defined |

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
// 1. DECLARE — define the signature blueprint
delegate void Notify(string msg);
 
// 2. METHODS — must match delegate signature
static void Email(string msg) => Console.WriteLine($"Email: {msg}");
static void SMS(string msg)   => Console.WriteLine($"SMS: {msg}");
 
// 3. INITIALIZE — assign a method
Notify n = Email;
 
// 4. CHAIN — add more methods
n += SMS;
 
// 5. CALL — fires all methods in order
n("Alert!");
// Email: Alert!
// SMS: Alert!
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

 **Q45. Lambda Expression vs Anonymous Method**

| | Anonymous Method | Lambda Expression |
|---|---|---|
| Syntax | Verbose (`delegate`) | Concise (`=>`) |
| Introduced | C# 2.0 | C# 3.0 |
| Expression body | ❌ | ✅ |

```csharp
// Anonymous Method
Func<int, int> square = delegate(int x) { return x * x; };

// Lambda — same thing, shorter
Func<int, int> square = x => x * x; // ✅ cleaner

// Real world — filtering orders
var big = orders.Where(delegate(Order o) { return o.Amount > 1000; }); // ❌ verbose
var big = orders.Where(o => o.Amount > 1000);                          // ✅ lambda
```

**Rule:** Always prefer lambda — same thing, cleaner syntax. Anonymous methods are rarely used today.

---

**Q46. Expression-Bodied Member**

> Replace `{ return ... }` with `=>` for single-line members

```csharp
// ❌ Verbose
public string GetName() { return "Alice"; }
public int    Age      { get { return 25; } }

// ✅ Expression-bodied
public string GetName() => "Alice";
public int    Age       => 25;

// Works on methods, properties, constructors
public string FullName => $"{First} {Last}";
public void   Print()  => Console.WriteLine(Name);
```

> **Rule:** Single line that returns something → use `=>`

---

**Q47. What is Tuple Unpacking**

> Destructure a tuple into individual named variables in one line

```csharp
// Return multiple values from a method
(string Name, int Age) GetUser() => ("Alice", 25);

// Unpack — instead of .Item1, .Item2
var (name, age) = GetUser();
Console.WriteLine(name); // Alice
Console.WriteLine(age);  // 25

// Real world — swap two variables
int a = 1, b = 2;
(a, b) = (b, a); // ✅ no temp variable needed
```

> **Rule:** Method returns multiple values → return tuple → unpack with `var (x, y) =`

## 🔷 Section 5: Exception Handling

---

**Q48. `throw` vs `throw ex` in exception handling?**

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

**Q49. What is the order of catch blocks?**

Most specific exceptions first, most general last. `Exception` must be last.

```csharp
try { /* ... */ }
catch (FileNotFoundException ex) { /* most specific */ }
catch (IOException ex) { }
catch (Exception ex) { /* most general — must be last */ }
finally { /* always runs */ }
```

---

**Q50. `finally` vs `Finalize` vs `Dispose`?**

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

**Q51. How to ensure unmanaged resources don't leak?**

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

**Q52. What are generics? Give a real use case?**

Generics let you write type-safe code that works with any type, avoiding boxing and casting.

```csharp
// 1. INTERFACE — generic contract
public interface IRepository<T>
{
    T GetById(int id);
    void Add(T entity);
    void Delete(T entity);
}

// 2. IMPLEMENTATION — one class for any entity
public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _db;

    public T GetById(int id) => _db.Set<T>().Find(id);
    public void Add(T entity)    => _db.Set<T>().Add(entity);
    public void Delete(T entity) => _db.Set<T>().Remove(entity);
}

// 3. USAGE — same repo, any entity
var userRepo    = new Repository<User>();
var productRepo = new Repository<Product>();

userRepo.GetById(1);
productRepo.GetById(5);
```

> **Benefit:** One repository class, works for **any entity** — no duplicate code per table.

---

**Q53. Generic collections vs non-generic collections?**

Non-generics store everything as `object` — causing **boxing, casting, and runtime bugs**. Generics are type-safe and faster.
 
| Generic | Non-Generic |
|--|--|
| `List<T>` | `ArrayList` |
| `Dictionary<K,V>` | `Hashtable` |
| `Queue<T>` | `Queue` |
| `Stack<T>` | `Stack` |
 
```csharp
// Non-generic — runtime bug
ArrayList list = new ArrayList();
list.Add("oops");        // no error at compile time
int x = (int)list[0];   // 💥 crashes at runtime
 
// Generic — compile-time safety
List<int> list = new List<int>();
list.Add("oops");        // ❌ caught at compile time
```
 
Non-generics are legacy (pre .NET 2.0) — always prefer generics.

---
**Q54 What are Generic Parameters & Constraints?**

A placeholder for a type defined at **compile time** — write once, work for any type.
 
```csharp
// T is the type parameter — decided when called
List<int> nums = new List<int>();     // T = int
List<string> names = new List<string>(); // T = string
```
 
```csharp
T Max<T>(T a, T b) where T : IComparable<T>
    => a.CompareTo(b) > 0 ? a : b;
```
  
| Constraint | Meaning |
|------------|---------|
| `where T : class` | Reference type only |
| `where T : struct` | Value type only |
| `where T : new()` | Must have parameterless constructor |
| `where T : BaseClass` | Must inherit `BaseClass` |
| `where T : IInterface` | Must implement `IInterface` |
| `where T : notnull` | Non-nullable only |
 
> Without constraints, `T` is treated as `object`.

---

**Q55. `IEnumerable` vs `IReadOnlyCollection` vs `IReadOnlyList` vs `ICollection` vs `IList` vs `IQueryable`?**

> Each interface below **adds more capability** than the previous one
 
| Interface | Count | Add/Remove | Index | Read Only | DB Query | Use When |
|---|---|---|---|---|---|---|
| `IEnumerable<T>`        | ❌ | ❌ | ❌ | ✅ | ❌ | Just loop / read-only |
| `IReadOnlyCollection<T>`| ✅ | ❌ | ❌ | ✅ | ❌ | Count, no modify |
| `IReadOnlyList<T>`      | ✅ | ❌ | ✅ | ✅ | ❌ | Count + index, no modify |
| `ICollection<T>`        | ✅ | ✅ | ❌ | ❌ | ❌ | Count + add/remove |
| `IList<T>`              | ✅ | ✅ | ✅ | ❌ | ❌ | Full control + index |
| `IQueryable<T>`         | ✅ | ❌ | ❌ | ✅ | ✅ | Filter on DB before loading |
 
```csharp
// All are interfaces — assign List<T> to them, never new() directly
var list = new List<User> { new User { Name = "Alice" }, new User { Name = "Bob" } };
 
IEnumerable<User>         e  = list; // just loop
IReadOnlyCollection<User> rc = list; // count, no modify
IReadOnlyList<User>       rl = list; // count + index, no modify
ICollection<User>         c  = list; // count + add/remove
IList<User>               l  = list; // full control
 
c.Add(new User());   // ✅
rc.Add(new User());  // ❌ compile error — read only
 
// IQueryable — only from DB context, not from List
IQueryable<User> q = db.Users;
q.Where(u => u.Age > 18); // SELECT * WHERE Age > 18 ✅
```
 
 **Rule:** All are just references to a `List<T>` — the interface controls **what the caller is allowed to do** with it.

---

**Q56. `Array` vs `ArrayList` vs `List<T>` vs `LinkedList<T>`**

| | Array | ArrayList | List\<T\> | LinkedList\<T\> |
|---|---|---|---|---|
| Generic | ✅ | ❌ | ✅ | ✅ |
| Type Safe | ✅ | ❌ (boxing) | ✅ | ✅ |
| Size | Fixed | Dynamic | Dynamic | Dynamic |
| Index Access | O(1) | O(1) | O(1) | O(n) |
| Insert/Delete (middle) | O(n) | O(n) | O(n) | O(1) |
| Use When | Size known | Legacy only | General purpose | Frequent insert/delete |
 
```csharp
// Array — fixed size
int[] arr = new int[3] { 1, 2, 3 };
var x = arr[0]; // O(1) ✅
 
// ArrayList — non-generic, needs casting (avoid in modern code)
ArrayList al = new ArrayList();
al.Add(1);
al.Add("oops"); // ❌ no type safety — mixes int and string
int n = (int)al[0]; // needs cast, boxing overhead
 
// List<T> — generic, type-safe, dynamic (prefer this always)
var list = new List<int> { 1, 2, 3 };
list.Add(4);       // ✅
list.Insert(1, 9); // O(n) — shifts elements
 
// LinkedList — fast insert/delete in middle
var linked = new LinkedList<int>(new[] { 1, 2, 3 });
linked.AddAfter(linked.First, 9); // O(1) ✅ no shifting
```
 
**Rule:** `ArrayList` → legacy, avoid. `List<T>` → always prefer. `LinkedList<T>` → only when frequent middle inserts/deletes.

---

**Q57.  `List<T>` vs `HashSet<T>` vs `SortedSet<T>`?**

| | Duplicates | Ordered | Type-safe | Lookup | Use case |
|--|--|--|--|--|--|
| `List<T>` | Yes | Yes | Yes | O(n) | General ordered collection |
| `HashSet<T>` | No | No | Yes | O(1) | Unique values, fast lookup |
| `SortedSet<T>` | No | Yes (auto) | Yes | O(log n) | Unique + sorted values |
 
```csharp
var list = new List<int> { 1, 2, 2 };           // [1, 2, 2] — duplicates allowed
 
var set = new HashSet<int> { 1, 2, 3 };
set.Add(2);                                       // ignored — already exists
 
var sorted = new SortedSet<int> { 3, 1, 2 };     // auto-sorts → [1, 2, 3]
```
 
**Rule:** Use `HashSet<T>` for unique values, `SortedSet<T>` when you also need sorting.

---

**Q58. What are concurrent collections in C#?**

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

**Q59. Types of Dictionary Collections**


| | Ordered | Thread-safe | Performance | Use case |
|--|--|--|--|--|
| `Dictionary<K,V>` | No | No | O(1) | General fast lookup |
| `SortedDictionary<K,V>` | Yes (Red-Black Tree) | No | O(log n) | Frequent insert/delete |
| `SortedList<K,V>` | Yes (Two arrays) | No | O(log n) | Built once, read many times |
| `ConcurrentDictionary<K,V>` | No | Yes | O(1) | Multi-thread access |
| `Hashtable` | No | No | O(1) | Legacy code only |
 
 Default to `Dictionary<K,V>` — switch only when you need sorting or thread safety.


---

## 🔷 Section 7: LINQ

---

**Q60. What is LINQ? What is deferred vs immediate execution?**

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

**Q61. Types of LINQ operators?**

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

**Q62. `FirstOrDefault` vs `SingleOrDefault`?**

- `FirstOrDefault` — returns the **first** match or null. OK with multiple matches.
- `SingleOrDefault` — returns the match, but **throws** if more than one match.

```csharp
var list = new[] { 1, 2, 2, 3 };

list.FirstOrDefault(x => x == 2);   // 2 — first match
list.SingleOrDefault(x => x == 2);  // ❌ throws — more than one match
list.SingleOrDefault(x => x == 3);  // 3 — exactly one match ✅
```

---

**Q63. `IEnumerable<T>` vs `IQueryable<T>` in LINQ?**

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
 **Q64. `Select` vs `SelectMany` in LINQ**
 
 `Select` = transform. `SelectMany` = flatten nested collections into one.
 
```csharp
var orders = new List<Order> {
    new Order { Id = 1, Items = new List<string> { "Laptop", "Mouse" } },
    new Order { Id = 2, Items = new List<string> { "Phone",  "Case"  } }
};
 
// Select — one result per order
orders.Select(o => o.Id);
// [ 1, 2 ]
 
// SelectMany — all items across all orders, flat
orders.SelectMany(o => o.Items);
// [ "Laptop", "Mouse", "Phone", "Case" ] ✅
 
// Real world — unique items sold
orders.SelectMany(o => o.Items).Distinct();
// [ "Laptop", "Mouse", "Phone", "Case" ]
```
 
> **Rule:** List inside a list → use `SelectMany` to flatten.


---

**Q65. How to do pagination with LINQ?**

```csharp
int page = 2, pageSize = 10;

var pagedResults = db.Products
    .OrderBy(p => p.Id)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToList();
```

---

**Q66. A LINQ query is slow. How to troubleshoot?**

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

**Q67. Synchronous vs Asynchronous programming?**

- **Sync:** Waits for each operation before moving on. Blocks the thread.
- **Async:** Starts an operation, **does other work** while waiting, resumes when done.

```csharp
// Sync — thread blocked waiting for file
string content = File.ReadAllText("big.txt");

// Async — thread freed while waiting
string content = await File.ReadAllTextAsync("big.txt");
```

---

**Q68. What are `async` and `await`? How do they work under the hood?**

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

**Q69. `async void` vs `async Task`? When to use each?**

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

**Q70. What is the difference between `Task`, `Task<T>`, and `ValueTask<T>`?**

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

**Q71. `Task.Run`, `Task.Delay`, and `Thread.Sleep` — when to use each?**

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

**Q72. Difference between `Task`, `Thread`, and `Process`?**

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

**Q73. `Task.WhenAll` vs `Task.WhenAny`?**

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

**Q74. `Task.FromResult`, `Task.FromException`, `Task.FromCanceled`?**


 Create **already-completed tasks** — no async work, no thread used.

```csharp
Task<int> GetCount() => Task.FromResult(42);                              // ✅ value ready
Task Fail()          => Task.FromException(new InvalidOperationException()); // ❌ error ready
Task Cancel(CancellationToken ct) => Task.FromCanceled(ct);               // 🚫 cancelled
```

Interface forces `Task` return but answer is already in memory:

```csharp
public interface ICache { Task<int> GetCount(); }

// DB — real async
public class DbCache : ICache
{
    public async Task<int> GetCount() => await _db.Items.CountAsync();
}

// Memory — no async needed
public class MemoryCache : ICache
{
    public Task<int> GetCount() => Task.FromResult(42); // ✅ skip async overhead
}
```

---

**Q75. Can you use `await` in `catch` or `finally` blocks?**

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

**Q76. What is `Thread.Sleep()` vs `Thread.Join()`?**

- `Thread.Sleep(ms)` — pauses the **current** thread for ms milliseconds.
- `Thread.Join()` — makes the **calling thread wait** until another thread finishes.

```csharp
Thread t = new Thread(() => { Thread.Sleep(1000); Console.WriteLine("Done"); });
t.Start();
t.Join(); // main thread waits here until t finishes
```

---

**Q77. Race condition vs Deadlock?**

**Race Condition** : Two threads modify shared data simultaneously → wrong result
 
```csharp
// ❌ Two people withdraw from same account at once — balance goes negative
Parallel.Invoke(() => balance -= 80, () => balance -= 80);
 
// ✅ Fix
Parallel.Invoke(() => Interlocked.Add(ref balance, -80), () => Interlocked.Add(ref balance, -80));
```
 
**Deadlock** : Two threads wait for each other's lock → both stuck forever
 
```csharp
lock(Pen) { lock(Paper) { } }  // Thread 1 — holds Pen, waits for Paper
lock(Paper) { lock(Pen) { } }  // Thread 2 — holds Paper, waits for Pen ❌
 
// ✅ Fix — always lock in same order
lock(Pen) { lock(Paper) { } }  // Both threads
```
 
| | Race Condition | Deadlock |
|---|---|---|
| Problem | Threads overlap | Threads wait for each other |
| Fix | `Interlocked` / `lock` | Consistent lock order |

---

**Q78. What is the Producer-Consumer problem?**

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

**Q79. What is a Semaphore in C#?**

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

**Q80. What is a CancellationToken?**

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

**Q81. What is `Parallel.ForEach` vs PLINQ vs `Parallel.For`?**

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

**Q82. Fire-and-forget tasks failing silently. How to fix?**

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

**Q83. Explain SOLID Principles?**

| Letter | Principle | Meaning |
|--|--|--|
| S | Single Responsibility | A class should have one reason to change |
| O | Open/Closed | Open for extension, closed for modification |
| L | Liskov Substitution | Subtypes must be substitutable for base types |
| I | Interface Segregation | Don't force clients to depend on methods they don't use |
| D | Dependency Inversion | Depend on abstractions, not concretions |

**S — Single Responsibility** : One class, one job
```csharp
// ❌ Bad
class OrderService { void Save() {} void SendEmail() {} }
 
// ✅ Good
class OrderRepository     { void Save(Order o) {} }
class NotificationService { void Send(Order o) {} }
```
 
**O — Open/Closed** : Extend via new class, never modify old
```csharp
interface INotifier { void Send(Order o); }
 
class EmailNotifier : INotifier { public void Send(Order o) => Console.WriteLine("Email"); }
class SmsNotifier   : INotifier { public void Send(Order o) => Console.WriteLine("SMS");   }
// ✅ Add PushNotifier without touching EmailNotifier or SmsNotifier
class PushNotifier  : INotifier { public void Send(Order o) => Console.WriteLine("Push");  }
```
 
**L — Liskov Substitution** : Swap child for parent, nothing breaks
```csharp
// ✅ Every INotifier child honours Send() — swap freely
INotifier n = new SmsNotifier();
n.Send(order); // always works ✅
 
// ❌ Bad — breaks promise
class BrokenNotifier : INotifier {
    public void Send(Order o) => throw new NotImplementedException(); // caller crashes!
}
```
 
**I — Interface Segregation** : Don't implement what you don't need
```csharp
// ❌ Bad — SmsNotifier forced to implement SendEmail it doesn't support
interface INotifier { void SendEmail(Order o); void SendSms(Order o); }
 
// ✅ Good — split
interface IEmailNotifier { void SendEmail(Order o); }
interface ISmsNotifier   { void SendSms(Order o);   }
 
class SmsNotifier : ISmsNotifier { public void SendSms(Order o) => Console.WriteLine("SMS"); } // ✅ no junk
```
 
**D — Dependency Inversion** : Depend on interface, not concrete class
```csharp
// ❌ Bad
class OrderProcessor { private SmsNotifier _n = new SmsNotifier(); }
 
// ✅ Good — inject INotifier, swap freely
class OrderProcessor {
    private readonly ISmsNotifier _notifier;
    public OrderProcessor(ISmsNotifier notifier) { _notifier = notifier; }
    public void Process(Order o) => _notifier.SendSms(o);
}
 
new OrderProcessor(new SmsNotifier()); // ✅
```

---

**Q84. Exaplin `DRY` and `KISS` Principles ?**

#### DRY — Don't Repeat Yourself
Same logic should exist in **one place only**.
 
```csharp
// Bad — duplicated tax logic
public decimal OrderTotal(Order o) => o.Items.Sum(i => i.Price) * 1.18m;
public decimal InvoiceTotal(Order o) => o.Items.Sum(i => i.Price) * 1.18m;
 
// Good — one method
public decimal CalculateTotal(Order o) => o.Items.Sum(i => i.Price) * 1.18m;
```
 
#### KISS — Keep It Simple, Stupid
Write the **simplest solution** that works.
 
```csharp
// Bad
public bool IsEven(int n) => n % 2 == 0 ? true : false;
 
// Good
public bool IsEven(int n) => n % 2 == 0;
```
 
 DRY = no duplication · KISS = no unnecessary complexity

---

**Q85. What are Design Pattern types?**

| Category | About | Examples |
|---|---|---|
| **Creational** | How objects are **created** | Singleton, Factory, Builder, Prototype |
| **Structural** | How objects are **composed/connected** | Adapter, Decorator, Facade, Proxy, Repository |
| **Behavioral** | How objects **communicate/interact** | Mediator, Observer, Strategy, Command, CQRS |

---

## 🔷 Section 10: Access Modifiers & More

---

**Q86. Access Modifiers in C#?**

| Modifier | Accessible From |
|--|--|
| `public` | Anywhere |
| `private` | Same class only |
| `protected` | Same class + subclasses |
| `internal` | Same assembly |
| `protected internal` | Same assembly OR subclasses |
| `private protected` | Same class + subclasses in same assembly |

---

**Q87. What is `checked` vs `unchecked` in C#?**

Controls what happens when an **integer overflows**.
 
| | `checked` | `unchecked` |
|--|--|--|
| On overflow | Throws `OverflowException` | Wraps around silently |
| Use when | Data accuracy matters | Performance, intentional wrap |
| Real use | Financial calculations | Hash codes, bit manipulation |
 
```csharp
int max = int.MaxValue;  // 2,147,483,647
 
checked   { int x = max + 1; }  // 💥 OverflowException
unchecked { int x = max + 1; }  // -2,147,483,648 — wraps around
```
 
> Default is `unchecked` — use `checked` when wrong numbers are worse than a crash.


#### Why do both keywords exist?
 
To **override the current default** — if the project is globally `checked`, you need `unchecked` to opt out for specific blocks like hash codes, and vice versa.

---

**Q88. What is Interlop in C# ?**

Allows C# to call **unmanaged code** (C, C++, Win32 APIs).
 
 
| Keyword | Purpose |
|---------|---------|
| `extern` | Method implemented outside C# |
| `unsafe` | Enables pointer manipulation |
| `fixed` | Pins memory so GC won't move it |
| `volatile` | Always read fresh from memory — no thread caching |
 
 
```csharp
// extern — call a Windows API
[DllImport("user32.dll")]
public static extern int MessageBox(IntPtr h, string msg, string title, int type);
 
// unsafe + fixed — pointer manipulation
unsafe {
    fixed (int* p = &value) { *p = 100; }
}
 
// volatile — prevent thread caching
volatile bool _running = true;
```

---

**Q89. What are attributes in C#? How to create custom attributes?**

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

**Q90. Null-coalescing `??` and null-conditional `?.` operators?**

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

**Q91. `readonly` property vs `init`-only property?**

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

**Q92. Managed vs Unmanaged code?**

- **Managed:** Runs under CLR control — GC, type safety, exceptions. All normal C# code.
- **Unmanaged:** Native code (C/C++), COM objects, OS APIs. No GC — you manage memory.

```csharp
// Unmanaged resource — requires manual cleanup
[DllImport("kernel32.dll")]
static extern IntPtr OpenProcess(int access, bool inherit, int pid);

// Always wrap in IDisposable
```

---

**Q93. What is Serialization and Deserialization?**

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

**Q94. What is ArrayPool vs MemoryPool vs stackalloc?**

All three reduce heap allocations for better performance.

| | `ArrayPool<T>` | `MemoryPool<T>` | `stackalloc` |
|--|--|--|--|
| Where | Heap (rented) | Heap (rented) | Stack |
| Size limit | Large OK | Large OK | Small only (~1KB) |
| Use with async | ✅ | ✅ | ❌ (Span only) |

```csharp
var pool = ArrayPool<byte>.Shared;
byte[] buffer = pool.Rent(4096);
try { await stream.ReadAsync(buffer); }
finally { pool.Return(buffer); }

Span<int> nums = stackalloc int[10];
nums[0] = 42;

using IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(1024);
Memory<byte> mem = owner.Memory;
await stream.ReadAsync(mem);
```
---

**Q95. What is `IDisposable` vs `IAsyncDisposable`?**

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

**Q96. `String.Intern` and String interning?**

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

**Q97. What is `yield return` and iterator methods?**

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

**Q98. What is the difference between `static` class and `Singleton`?**

| | Static Class | Singleton |
|--|--|--|
| Instance | None | One instance |
| Interface | Can't implement | Can implement |
| DI injectable | ❌ | ✅ |
| Testable | Hard | Yes |
| State | Global | Controlled |

Use Singleton over static when you need DI, interfaces, or testability.

---

**Q99. What are expression trees in C#?**

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

**Q100. What is `dynamic` dispatch vs reflection?**

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
