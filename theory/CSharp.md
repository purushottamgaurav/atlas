# C# Interview Q&A

---

1. **What is C# and CLR?**
C# is a strongly-typed OOP language that compiles to MSIL/CIL bytecode. CLR (Common Language Runtime) is the execution engine that JIT-compiles MSIL to native code and provides memory management, type safety, security, and exception handling. CLS ensures cross-language interoperability.

2. **What is a C# Assembly? How to secure .exe and .dll?**
An assembly is a compiled, versioned unit of code (.exe or .dll) containing MSIL, metadata, and a manifest. Secure it using strong-naming (sn.exe), obfuscation tools (Dotfuscator, ConfuserEx), and Authenticode signing.

3. **Assembly vs Namespace?**
Assembly is a physical compiled unit (.dll/.exe) — a deployment boundary. Namespace is a logical grouping of types in code — no physical boundary.

4. **What is JIT Compilation? Types?**
JIT converts MSIL to native code at runtime. Normal JIT compiles on first call and caches the result. Pre-JIT/AOT compiles the entire assembly upfront via ngen.exe. Econo-JIT compiles only what's needed and discards after use — low memory, used in older .NET.

5. **Can we have more than one Main method?**
Yes. Multiple classes can each have a Main. You pick which one runs via Project Properties → Startup object or the -main compiler flag.

6. **What is Reflection? How is it used in projects?**
Reflection lets you inspect and invoke types, methods, and properties at runtime via System.Reflection. Used in ORMs (mapping DB columns to properties), DI containers (assembly scanning), serializers, and plugin systems.
```csharp
Type t = typeof(MyClass);
t.GetMethod("Run").Invoke(instance, null);
```

7. **What is a static constructor? Why and how is it used?**
A static constructor runs once automatically before the class is first used. It initializes static members. It takes no parameters and no access modifier because the runtime calls it — there is no caller to pass arguments or respect access control.

8. **Types of constructors?**
Default (no params), Parameterized, Private (used in Singleton — prevents external instantiation), Static (initializes static members, runs once, no params/modifier), Copy (takes same type as param to clone), Constructor chaining (use this(...) to call another constructor in the same class, base(...) for parent).

9. **Synchronous vs asynchronous programming?**
Synchronous — each line waits for the previous to finish (blocking). Asynchronous — async/await lets the thread do other work while waiting for I/O to complete. Improves scalability, especially in web apps.

10. **What are async and await keywords?**
async marks a method as asynchronous. await suspends the method until the Task completes without blocking the thread. To call async from sync: Task.Run(async () => { await FetchAsync(); }).GetAwaiter().GetResult();

11. **Parallel class library in C#?**
System.Threading.Tasks.Parallel provides CPU-bound parallelism. Parallel.For, Parallel.ForEach, Parallel.Invoke. Use when work is independent and CPU-heavy. Different from async which is for I/O-bound work.

12. **Thread.Sleep() vs Thread.Join()?**
Thread.Sleep(ms) pauses the current thread. thread.Join() blocks the calling thread until the specified thread finishes.

13. **Race condition vs deadlock?**
Race condition — two threads access shared data concurrently; result depends on timing. Fix with lock or Interlocked. Deadlock — two threads each hold a lock the other needs; both wait forever. Fix with consistent lock ordering or Monitor.TryEnter with timeout.

14. **Producer-consumer problem?**
Producer generates data, consumer processes it. Use BlockingCollection<T> or Channel<T> in modern .NET.

15. **IEnumerable vs IQueryable?**
IEnumerable<T> iterates in-memory — LINQ runs client-side. IQueryable<T> builds an expression tree — LINQ translates to SQL and runs server-side. Use IQueryable for DB queries to avoid loading everything into memory.

16. **Difference between PUT, PATCH, POST?**
POST creates a new resource (not idempotent). PUT replaces the entire resource (idempotent). PATCH partially updates a resource (idempotent).

17. **Func vs Action vs Predicate?**
Func<T, TResult> returns a value. Action<T> returns void. Predicate<T> returns bool and is essentially Func<T, bool>.

18. **Call by value vs call by reference?**
By value — a copy is passed; changes don't affect the original. By reference (ref/out) — the memory address is passed; changes affect the original. Reference types passed by value still allow mutation of the object — the reference is copied, not the object.

19. **What are delegates? Singlecast vs multicast?**
Delegates are type-safe function pointers. Singlecast points to one method. Multicast points to multiple methods (+=) — all are called on Invoke(). Func, Action, and Predicate are built-in generic delegate types.

20. **What are events?**
Events wrap multicast delegates with add/remove encapsulation. Subscribers register handlers; the publisher raises the event. External code cannot invoke the event directly — only subscribe/unsubscribe.

21. **Difference between ref and out?**
ref — variable must be initialized before passing; two-way. out — need not be initialized before passing; must be assigned inside the method. Used when a method needs to return multiple values.

22. **What are properties and accessors?**
Properties expose private fields via get/set accessors, enabling validation and encapsulation. Auto-property: public string Name { get; set; }. Init-only (C# 9): public string Id { get; init; }

23. **Finally vs Finalize vs Dispose?**
finally always runs after try/catch regardless of exception. Finalize (~Destructor) is called by GC before object is collected — non-deterministic, avoid for critical cleanup. Dispose is explicit deterministic cleanup via IDisposable; use with using block. Always prefer Dispose over Finalize.

24. **How to ensure unmanaged resources don't leak?**
Implement IDisposable. Release unmanaged resources in Dispose(), add a finalizer as a safety net, and call GC.SuppressFinalize(this) inside Dispose. Consumers wrap usage in a using block.

25. **const vs readonly vs static readonly?**
const is a compile-time constant, inlined at call sites, only primitives/strings. readonly is set at declaration or in constructor, per-instance runtime constant. static readonly is one value for all instances, set once at class load. Prefer static readonly over const for values that might change across versions.

26. **Hashtable vs Dictionary?**
Hashtable is non-generic, stores object, not type-safe, has boxing overhead. Dictionary<TKey, TValue> is generic, type-safe, and faster. For thread safety use ConcurrentDictionary.

27. **What are indexers?**
Let objects be indexed like arrays. Use when a class wraps a collection and index-access feels natural.
```csharp
public string this[int i] { get => _data[i]; set => _data[i] = value; }
```

28. **String vs StringBuilder?**
string is immutable — every modification creates a new object in memory. StringBuilder is mutable — modifies in place. Use StringBuilder when doing many concatenations or modifications in loops.

29. **Types of classes in C#?**
Abstract — cannot be instantiated, can be inherited. Sealed — can be instantiated, cannot be inherited. Static — cannot be instantiated or inherited, derives from object only. Partial — splits class definition across files. Interface — pure contract, no instance state.

30. **Abstract class vs Interface?**
Abstract class — use when subclasses share a common base implementation. Supports fields, constructors, access modifiers. Interface — use when unrelated classes share a contract. Supports multiple implementation. From C# 8, interfaces can have default implementations.

31. **What are extension methods?**
Add methods to existing types without modifying them. Defined as static methods in a static class with this as the first parameter.
```csharp
public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);
```

32. **Early binding vs late binding?**
Early binding — type resolved at compile time; direct method calls; faster. Late binding — type resolved at runtime via dynamic, reflection, or virtual dispatch; flexible but slower.

33. **Method overloading vs overriding vs hiding?**
Overloading — same name, different parameters; compile-time polymorphism. Overriding — subclass redefines a virtual/abstract method using override; runtime polymorphism. Hiding — use new keyword to hide base method; base reference still calls base version (no polymorphic dispatch).

34. **Managed vs unmanaged code?**
Managed code runs under CLR; GC handles memory. Unmanaged code runs outside CLR with manual memory management (C/C++ via P/Invoke or COM). Wrap unmanaged resources in IDisposable.

35. **What are generics?**
Allow type-safe reusable code without boxing. List<T>, Dictionary<TK,TV>. Benefits: compile-time type checking, no casting, better performance than non-generic collections.

36. **Boxing and unboxing?**
Boxing — value type to object (heap allocation). Unboxing — object back to value type. Avoid in hot paths; use generics instead.

37. **What is Kestrel vs IIS?**
Kestrel is a cross-platform lightweight HTTP server built into ASP.NET Core — the default server. IIS is Windows-only and typically used as a reverse proxy in front of Kestrel in production Windows deployments.

38. **SOLID principles?**
- **S**ingle Responsibility: One reason to change
- **O**pen/Closed: Open for extension, closed for modification
- **L**iskov Substitution: Subtypes must be substitutable
- **I**nterface Segregation: Clients shouldn't depend on interfaces they don't use
- **D**ependency Inversion: Depend on abstractions, not concrete types

39. **What happens when Liskov Substitution is violated?**
Subclass breaks the base class contract (e.g., Square overriding Rectangle setters breaks area logic). Errors appear in unit tests or at runtime. Mitigate by preferring composition over inheritance or redesigning the abstraction.

40. **Design patterns — types?**
Creational (object creation) — Singleton, Factory, Abstract Factory, Builder. Structural (composition) — Adapter, Decorator, Facade, Proxy. Behavioral (communication) — Strategy, Observer, Command, Mediator.

41. **Singleton pattern?**
Ensures only one instance exists. Thread-safe using static readonly initialization.
```csharp
public sealed class Singleton {
    private static readonly Singleton _instance = new();
    private Singleton() { }
    public static Singleton Instance => _instance;
}
```

42. **Repository pattern?**
Abstracts data access behind an interface (IRepository<T>). Services talk to the interface, not directly to DbContext. Enables unit testing with mocks and swappable data sources.

43. **Dependency Injection vs new keyword?**
new creates tight coupling. DI inverts control — dependencies are injected externally. Benefits: loose coupling, testability (swap mocks), maintainability.

44. **throw vs throw ex in exception handling?**
throw re-throws preserving the original stack trace. throw ex resets the stack trace to the current line, hiding the original origin. Always use throw.

45. **Order of catch blocks?**
Most specific exception first, most general last. FileNotFoundException before IOException before Exception.

46. **var vs dynamic?**
var is statically typed — type resolved at compile time; IntelliSense works. dynamic is resolved at runtime — no compile-time checking; can throw RuntimeBinderException.

47. **What are records?**
Reference types (record class) or value types (record struct) designed for immutable data models. Auto-generate Equals, GetHashCode, ToString, and with-expression support. Use for DTOs and value objects.
```csharp
record Person(string Name, int Age);
var p2 = p1 with { Age = 30 };
```

48. **Why does C# not support multiple inheritance?**
To avoid the diamond problem — ambiguity when B and C both inherit A and both override the same method, then D inherits B and C. C# uses interfaces for multiple-type contracts instead.

49. **Convert.ToString() vs .ToString()?**
Convert.ToString() is null-safe — returns empty string if value is null. .ToString() throws NullReferenceException on null.

50. **== vs .Equals()?**
For primitives and types that override ==, both compare values. For objects, == checks reference equality by default; .Equals() can be overridden to check value equality. For integers both behave the same.

51. **Covariance vs contravariance?**
Covariance (out) — use a more derived type than specified. IEnumerable<Dog> assignable to IEnumerable<Animal>. Contravariance (in) — use a less derived type. Action<Animal> assignable to Action<Dog>.

52. **How to resolve ambiguity when two interfaces have the same method?**
Use explicit interface implementation.
```csharp
void IA.Show() => Console.WriteLine("IA");
void IB.Show() => Console.WriteLine("IB");
// Call: ((IA)obj).Show();
```

53. **is-a vs has-a relationship?**
is-a is inheritance (Dog : Animal). has-a is composition/aggregation (Car has an Engine). Prefer composition when behavior can vary independently.

54. **Serialization and deserialization?**
Converting an object to a storable/transmittable format (JSON, XML, binary) and back. Used for APIs, caching, file persistence, message queues.
```csharp
var json = JsonSerializer.Serialize(obj);
var obj = JsonSerializer.Deserialize<MyType>(json);
```

55. **Stack vs heap memory?**
Stack stores value types and method frames — LIFO, fast, auto-reclaimed. Heap stores reference types — managed by GC.

56. **What are accessibility modifiers?**
public, private, protected, internal, protected internal (same assembly OR derived classes), private protected (derived classes in same assembly only).

57. **virtual vs abstract vs sealed override?**
virtual has a default implementation and can be overridden. abstract has no implementation and must be overridden. sealed override prevents further overriding down the hierarchy.

58. **enum vs struct vs class?**
enum is named integer constants — value type. struct is a value type, stack-allocated, no inheritance, good for small data like Point. class is a reference type, heap-allocated, supports full OOP.

59. **What are code smells?**
Indicators of poor design: Long Method, God Class, Duplicate Code, Feature Envy, Primitive Obsession. Refactor using SOLID principles or design patterns. Reference: refactoring.guru/refactoring/smells

60. **Inheritance, Polymorphism, Encapsulation, Abstraction with examples?**
Encapsulation — private int _balance exposed via a property. Inheritance — Dog : Animal reuses Animal's members. Polymorphism — animal.Speak() calls Dog's or Cat's version at runtime via virtual dispatch. Abstraction — abstract class Shape with abstract Area() hides implementation details.

61. **What are sealed classes and when would you use them?**
Sealed classes cannot be inherited. Use when:
- Design doesn't support inheritance
- Security/integrity concerns
- Performance optimization
- Preventing accidental misuse

62. **Explain the difference between IEnumerable and IEnumerator**
- **IEnumerable**: Container, provides GetEnumerator() method
- **IEnumerator**: Iterator, implements Current, MoveNext(), Reset()

```csharp
// IEnumerable - the collection
public interface IEnumerable {
    IEnumerator GetEnumerator();
}

// IEnumerator - the pointer/position
public interface IEnumerator {
    object Current { get; }
    bool MoveNext();
    void Reset();
}
```

63. **What is LINQ and how does it work?**
 Language Integrated Query - provides unified syntax for querying various data sources. Uses deferred execution (lazy evaluation).

```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };
var query = numbers.Where(x => x > 2).Select(x => x * 2);
// Not executed until enumerated
foreach (var item in query) { }  // Execution happens here
```

64.  **What's the difference between IDisposable and IAsyncDisposable?**
- **IDisposable**: Synchronous resource cleanup
- **IAsyncDisposable**: Asynchronous resource cleanup (databases, HTTP)

```csharp
public class DatabaseConnection : IAsyncDisposable {
    public async ValueTask DisposeAsync() {
        await connection.CloseAsync();
    }
}

// Usage
await using var connection = new DatabaseConnection();
// Auto-disposes when scope exits