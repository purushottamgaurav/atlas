# .NET Developer MCQ Exam (3 Years Experience)
### 200 Questions with Detailed Explanations

---

## SECTION 1: C# FUNDAMENTALS & ADVANCED CONCEPTS (Q1–Q35)

---

**Q1.** What is the output of the following code?
```csharp
int x = 5;
Console.WriteLine(x++ + " " + ++x);
```
- A) 5 7
- B) 6 7
- C) 5 6
- D) 6 6

**✅ Answer: A) 5 7**

> `x++` is post-increment: uses current value (5), then increments x to 6. `++x` is pre-increment: increments x from 6 to 7, then uses it. Result: "5 7".

---

**Q2.** Which access modifier makes a member accessible only within the same assembly?
- A) private
- B) protected
- C) internal
- D) public

**✅ Answer: C) internal**

> `internal` restricts access to the current assembly. `private` is class-only, `protected` allows derived classes (even in other assemblies), and `public` is unrestricted.

---

**Q3.** What does the `sealed` keyword do when applied to a class?
- A) Prevents the class from being instantiated
- B) Prevents the class from being inherited
- C) Makes all members private
- D) Makes the class static

**✅ Answer: B) Prevents the class from being inherited**

> `sealed` prevents inheritance. Unlike `abstract` (which can't be instantiated), `sealed` classes can be instantiated but not subclassed. `string` in .NET is a classic example of a sealed class.

---

**Q4.** What is the difference between `ref` and `out` parameters in C#?
- A) `ref` requires initialization before passing; `out` does not
- B) `out` requires initialization before passing; `ref` does not
- C) Both require initialization before passing
- D) Neither requires initialization

**✅ Answer: A) `ref` requires initialization before passing; `out` does not**

> `ref` passes an already-initialized variable by reference. `out` is used when a method returns multiple values — the caller doesn't need to initialize, but the method must assign a value before returning.

---

**Q5.** What is boxing and unboxing in C#?
- A) Converting a value type to a reference type and vice versa
- B) Converting a string to an integer and vice versa
- C) Wrapping a method in a delegate
- D) Casting between two reference types

**✅ Answer: A) Converting a value type to a reference type and vice versa**

> Boxing wraps a value type (e.g., `int`) into a heap-allocated `object`. Unboxing extracts it back. This involves memory allocation and type checking, making it a performance concern in loops.

---

**Q6.** Which of the following is a value type in C#?
- A) string
- B) object
- C) struct
- D) class

**✅ Answer: C) struct**

> `struct` is a value type stored on the stack (or inline). `string`, `object`, and `class` types are reference types stored on the heap.

---

**Q7.** What keyword is used to define an extension method?
- A) virtual
- B) static
- C) override
- D) partial

**✅ Answer: B) static**

> Extension methods are defined in a `static` class using a `static` method, with the first parameter prefixed with `this`. Example: `public static int WordCount(this string str)`.

---

**Q8.** What is a delegate in C#?
- A) A base class for all types
- B) A type-safe function pointer
- C) A keyword for async programming
- D) A type of interface

**✅ Answer: B) A type-safe function pointer**

> A delegate is a type that holds a reference to a method with a compatible signature. It enables callback patterns, events, and LINQ lambdas. `Func<>`, `Action<>`, and `Predicate<>` are built-in generic delegates.

---

**Q9.** What is the output?
```csharp
string a = "hello";
string b = "hello";
Console.WriteLine(object.ReferenceEquals(a, b));
```
- A) False
- B) True
- C) Compile error
- D) Runtime exception

**✅ Answer: B) True**

> C# interns string literals at compile time. Both `a` and `b` point to the same interned string object in memory, so `ReferenceEquals` returns `true`.

---

**Q10.** What does the `?? ` operator do?
- A) Checks for null and throws an exception
- B) Returns the left operand if not null, otherwise returns the right operand
- C) Converts null to a boolean false
- D) Performs a null-safe method call

**✅ Answer: B) Returns the left operand if not null, otherwise returns the right operand**

> The null-coalescing operator `??` returns the left-hand value if it's not null; otherwise it returns the right-hand value. E.g., `string name = input ?? "default"`.

---

**Q11.** Which of the following correctly describes a `record` type in C# 9+?
- A) A mutable reference type with auto-generated constructors
- B) An immutable reference type with value-based equality
- C) A value type similar to a struct
- D) An abstract class with required members

**✅ Answer: B) An immutable reference type with value-based equality**

> `record` types in C# 9+ are reference types that use value-based equality (comparing property values, not references), support `with` expressions for non-destructive mutation, and auto-generate `ToString`, `GetHashCode`, and `Equals`.

---

**Q12.** What is the purpose of the `using` statement in C#?
- A) To import namespaces
- B) To ensure IDisposable objects are properly disposed
- C) Both A and B
- D) To declare global variables

**✅ Answer: C) Both A and B**

> `using` serves two purposes: as a directive to import namespaces (`using System;`), and as a statement to ensure `IDisposable` objects are disposed after the block (`using (var conn = new SqlConnection(...)) { }`).

---

**Q13.** What is the difference between `IEnumerable<T>` and `IQueryable<T>`?
- A) `IEnumerable` is for in-memory collections; `IQueryable` allows expression trees to be translated (e.g., to SQL)
- B) `IQueryable` is faster for in-memory operations
- C) `IEnumerable` works with databases; `IQueryable` works with lists
- D) There is no difference

**✅ Answer: A) `IEnumerable` is for in-memory collections; `IQueryable` allows expression trees to be translated (e.g., to SQL)**

> `IEnumerable` processes data in memory (C# code). `IQueryable` builds expression trees that providers (like EF Core) translate to SQL, allowing server-side filtering. Using `IEnumerable` on a DB query fetches all rows first, then filters — a serious performance pitfall.

---

**Q14.** What does `virtual` keyword allow?
- A) A method to be called without an object instance
- B) A method to be overridden in derived classes
- C) A method to be hidden in derived classes
- D) A method to run asynchronously

**✅ Answer: B) A method to be overridden in derived classes**

> `virtual` marks a method as overridable. Derived classes use `override` to provide a new implementation. Without `virtual`, a method cannot be polymorphically overridden (only hidden with `new`).

---

**Q15.** What is the difference between `abstract` class and `interface` in C# 8+?
- A) Interfaces can have default implementations; abstract classes cannot have constructors
- B) Abstract classes can have state and constructors; interfaces can have default method implementations but not state
- C) They are identical in C# 8+
- D) Interfaces support multiple inheritance; abstract classes do not

**✅ Answer: B) Abstract classes can have state and constructors; interfaces can have default method implementations but not state**

> In C# 8+, interfaces gained default implementations. However, abstract classes can have fields, constructors, and full state. Interfaces still cannot have instance fields. A class can implement multiple interfaces but inherit from only one abstract class.

---

**Q16.** What is a `Func<int, int, bool>` delegate?
- A) A function taking two booleans and returning an int
- B) A function taking two ints and returning a bool
- C) A function taking a bool and returning two ints
- D) A function taking no parameters and returning a bool

**✅ Answer: B) A function taking two ints and returning a bool**

> In `Func<T1, T2, TResult>`, the last type parameter is always the return type. So `Func<int, int, bool>` takes two `int` parameters and returns a `bool`.

---

**Q17.** What is covariance in C# generics?
- A) Allowing a derived type where a base type is expected (using `out`)
- B) Allowing a base type where a derived type is expected (using `in`)
- C) Converting value types to reference types
- D) Enabling method overloading with generic constraints

**✅ Answer: A) Allowing a derived type where a base type is expected (using `out`)**

> Covariance (`out`) allows using a more derived type. E.g., `IEnumerable<Dog>` can be assigned to `IEnumerable<Animal>` because `IEnumerable<T>` is covariant. Contravariance (`in`) works the other direction.

---

**Q18.** What does the `params` keyword do?
- A) Makes a parameter optional with a default value
- B) Allows a method to accept a variable number of arguments as an array
- C) Passes a parameter by reference
- D) Declares named parameters

**✅ Answer: B) Allows a method to accept a variable number of arguments as an array**

> `params` lets callers pass any number of arguments: `void Log(params string[] messages)`. Internally, they're treated as an array. Only one `params` parameter is allowed per method, and it must be the last parameter.

---

**Q19.** What is the result of `typeof(int) == typeof(Int32)`?
- A) False — they are different types
- B) True — `int` is an alias for `System.Int32`
- C) Compile error
- D) Runtime exception

**✅ Answer: B) True — `int` is an alias for `System.Int32`**

> In C#, `int` is syntactic sugar for `System.Int32`. They are the same type, so `typeof(int) == typeof(Int32)` is `true`.

---

**Q20.** What is a `partial` class?
- A) A class that cannot be fully instantiated
- B) A class definition split across multiple files
- C) A class with some abstract methods
- D) A class sealed from inheritance

**✅ Answer: B) A class definition split across multiple files**

> `partial` allows a class, struct, or interface to be split across multiple files. This is commonly used with code-generated files (e.g., EF migrations, WinForms designer) to separate auto-generated code from hand-written code.

---

**Q21.** Which collection should you use for fast key-value lookups?
- A) List<T>
- B) Queue<T>
- C) Dictionary<TKey, TValue>
- D) LinkedList<T>

**✅ Answer: C) Dictionary<TKey, TValue>**

> `Dictionary<TKey, TValue>` uses a hash table internally, giving O(1) average-case lookup, insertion, and deletion. `List<T>` is O(n) for lookups unless sorted.

---

**Q22.** What is the purpose of `IDisposable`?
- A) To mark a class as garbage-collected
- B) To provide a deterministic cleanup mechanism for unmanaged resources
- C) To prevent an object from being created more than once
- D) To make a class serializable

**✅ Answer: B) To provide a deterministic cleanup mechanism for unmanaged resources**

> `IDisposable.Dispose()` is called explicitly (or via `using`) to release unmanaged resources (file handles, DB connections, etc.) before GC runs. GC handles managed memory but doesn't know about unmanaged resources.

---

**Q23.** What happens when you throw an exception inside a `finally` block?
- A) The original exception is preserved and both are thrown
- B) The finally exception replaces the original exception
- C) Compile error
- D) The finally block is skipped

**✅ Answer: B) The finally exception replaces the original exception**

> If an exception is thrown inside `finally`, it replaces the original exception. The original exception is lost. This is a common gotcha — avoid throwing from `finally` blocks.

---

**Q24.** What is the difference between `==` and `.Equals()` for strings?
- A) `==` compares references; `.Equals()` compares values
- B) For strings, both compare values (content), but `==` is overloaded
- C) `.Equals()` is case-sensitive; `==` is not
- D) They are completely identical for all types

**✅ Answer: B) For strings, both compare values (content), but `==` is overloaded**

> The `string` class overloads `==` to compare content, not references. So `"abc" == "abc"` is `true`. But for general objects, `==` compares references. Always use `string.Equals(a, b, StringComparison.OrdinalIgnoreCase)` for culture-safe comparison.

---

**Q25.** What is a nullable value type in C#?
- A) A reference type that can hold null
- B) A value type wrapped with `Nullable<T>` that can hold null
- C) A string that defaults to null
- D) An interface that returns null

**✅ Answer: B) A value type wrapped with `Nullable<T>` that can hold null**

> `int?` is shorthand for `Nullable<int>`. It adds a `HasValue` property and allows `null` assignment. Useful for database scenarios where numeric columns may be NULL.

---

**Q26.** Which statement about `static` constructors is correct?
- A) They can take parameters
- B) They are called once per object instantiation
- C) They are called once per type, automatically, before first use
- D) They must be explicitly invoked

**✅ Answer: C) They are called once per type, automatically, before first use**

> A static constructor runs exactly once — the first time the type is used. It initializes static fields and cannot have access modifiers or parameters. You cannot control when it runs.

---

**Q27.** What does `nameof()` return?
- A) The full namespace path of a type
- B) The compile-time string name of a variable, type, or member
- C) The runtime type name
- D) The assembly name

**✅ Answer: B) The compile-time string name of a variable, type, or member**

> `nameof(myVariable)` returns `"myVariable"` as a string at compile time. This is useful for argument validation (`throw new ArgumentNullException(nameof(param))`) because it's refactor-safe.

---

**Q28.** What is pattern matching in C#?
- A) Matching regex patterns against strings
- B) Testing whether a value has a certain shape or type and extracting data from it
- C) A design pattern for observer implementations
- D) Matching method signatures to delegates

**✅ Answer: B) Testing whether a value has a certain shape or type and extracting data from it**

> Pattern matching (C# 7+) allows `is` type patterns (`if (obj is string s)`), switch expressions with patterns, property patterns (`obj is { Name: "Alice" }`), and more. It enables expressive, concise type discrimination.

---

**Q29.** What is the difference between `throw` and `throw ex` in a catch block?
- A) They are identical
- B) `throw` rethrows the original exception preserving the stack trace; `throw ex` resets the stack trace
- C) `throw ex` preserves the stack trace; `throw` resets it
- D) `throw` only works for system exceptions

**✅ Answer: B) `throw` rethrows the original exception preserving the stack trace; `throw ex` resets the stack trace**

> `throw;` (bare rethrow) preserves the original stack trace, making debugging easier. `throw ex;` starts a new stack trace from the current location, hiding the real origin of the error.

---

**Q30.** What does `Span<T>` provide?
- A) Thread-safe access to arrays
- B) A type-safe, memory-safe slice of contiguous memory with no heap allocation
- C) A lazy evaluation wrapper around collections
- D) A thread-local storage mechanism

**✅ Answer: B) A type-safe, memory-safe slice of contiguous memory with no heap allocation**

> `Span<T>` is a stack-only struct that represents a contiguous region of memory (array slice, stack memory, native memory) without allocating on the heap. It enables high-performance, zero-copy operations.

---

**Q31.** What does `yield return` do?
- A) Exits the current method with a value
- B) Creates a lazy, stateful iterator without manually implementing IEnumerator
- C) Returns a Task with a result
- D) Yields CPU time to another thread

**✅ Answer: B) Creates a lazy, stateful iterator without manually implementing IEnumerator**

> `yield return` pauses the method, returns a value, and resumes from where it left off on the next call. The compiler generates a state machine that implements `IEnumerator<T>`. Useful for memory-efficient sequences.

---

**Q32.** What is the `dynamic` type in C#?
- A) A type resolved at compile time
- B) A type that bypasses compile-time type checking and is resolved at runtime
- C) A synonym for `object`
- D) A type that always returns the fastest available overload

**✅ Answer: B) A type that bypasses compile-time type checking and is resolved at runtime**

> `dynamic` defers type checking to runtime via the DLR (Dynamic Language Runtime). Useful for interop with COM, reflection-heavy code, or dynamic languages. It trades compile-time safety for flexibility.

---

**Q33.** What is the purpose of the `volatile` keyword?
- A) Prevents a field from being serialized
- B) Ensures a field is always read/written directly to main memory, preventing CPU caching optimizations
- C) Makes a field thread-safe
- D) Prevents the garbage collector from collecting the field

**✅ Answer: B) Ensures a field is always read/written directly to main memory, preventing CPU caching optimizations**

> `volatile` tells the compiler and CPU not to cache the field value in a register. It prevents instruction reordering for that field. However, it does not guarantee atomicity for compound operations — use `Interlocked` or `lock` for that.

---

**Q34.** What is the output?
```csharp
var list = new List<Action>();
for (int i = 0; i < 3; i++)
    list.Add(() => Console.Write(i));
list.ForEach(a => a());
```
- A) 0 1 2
- B) 3 3 3
- C) 0 0 0
- D) Compile error

**✅ Answer: B) 3 3 3**

> This is the classic closure-over-loop-variable bug. All lambdas capture the same variable `i` by reference, not by value. By the time they execute, `i` is 3. To fix: `int copy = i; list.Add(() => Console.Write(copy));`

---

**Q35.** What does the `in` parameter modifier do in C# 7.2+?
- A) Passes a parameter by value
- B) Passes a parameter by read-only reference (cannot be modified)
- C) Passes a parameter as an output parameter
- D) Marks a parameter as optional

**✅ Answer: B) Passes a parameter by read-only reference (cannot be modified)**

> `in` passes large value types (like big structs) by reference for performance, while guaranteeing the callee cannot modify the value. It combines the efficiency of `ref` with the safety guarantee that the original value won't change.

---

## SECTION 2: .NET FUNDAMENTALS & CLR (Q36–Q55)

---

**Q36.** What is the CLR?
- A) Common Language Runtime — the execution engine for .NET code
- B) Common Library Repository — a package manager
- C) Component Layer Reference — a design pattern
- D) Cross-Language Runtime — a cross-platform OS

**✅ Answer: A) Common Language Runtime — the execution engine for .NET code**

> The CLR is the virtual machine component of .NET. It handles JIT compilation of IL to native code, memory management (GC), exception handling, type safety, and security.

---

**Q37.** What is IL (Intermediate Language)?
- A) Native machine code produced by the C# compiler
- B) A CPU-independent bytecode compiled from C#, then JIT-compiled to native code at runtime
- C) A markup language for .NET configuration
- D) An interpreted scripting language

**✅ Answer: B) A CPU-independent bytecode compiled from C#, then JIT-compiled to native code at runtime**

> When you compile C#, the compiler produces IL (also called MSIL or CIL). At runtime, the JIT compiler translates IL to native machine code for the target CPU. This enables cross-language interoperability and cross-platform targeting.

---

**Q38.** What is the difference between .NET Framework and .NET Core?
- A) .NET Framework is cross-platform; .NET Core is Windows-only
- B) .NET Core is cross-platform, open-source, and modular; .NET Framework is Windows-only and legacy
- C) They are the same product with different version numbers
- D) .NET Core only runs on Linux

**✅ Answer: B) .NET Core is cross-platform, open-source, and modular; .NET Framework is Windows-only and legacy**

> .NET Framework is the original Windows-only implementation. .NET Core (now just ".NET" from v5 onwards) is cross-platform, open-source, and the future of .NET. New projects should target .NET 6/7/8+.

---

**Q39.** How does the Garbage Collector (GC) work in .NET?
- A) It immediately deletes objects when they go out of scope
- B) It periodically identifies unreachable objects and reclaims their memory using a generational algorithm
- C) It uses reference counting like C++
- D) Developers must manually call GC.Collect() for cleanup

**✅ Answer: B) It periodically identifies unreachable objects and reclaims their memory using a generational algorithm**

> .NET GC uses three generations (0, 1, 2). Short-lived objects in Gen 0 are collected frequently. Surviving objects are promoted to Gen 1, then Gen 2. The GC traces object reachability from roots (stack, statics) — unreachable objects are collected.

---

**Q40.** What is the Large Object Heap (LOH)?
- A) A separate heap for objects larger than 85,000 bytes, not compacted by default
- B) The heap used for static variables
- C) A heap reserved for string interning
- D) A garbage-collected heap for COM interop objects

**✅ Answer: A) A separate heap for objects larger than 85,000 bytes, not compacted by default**

> Objects ≥ 85,000 bytes go to the LOH. The LOH is collected during Gen 2 GC and historically wasn't compacted (causing fragmentation). In .NET 4.5.1+, you can enable LOH compaction with `GCSettings.LargeObjectHeapCompactionMode`.

---

**Q41.** What is JIT compilation?
- A) Compiling C# source code to IL at build time
- B) Translating IL to native machine code at runtime, just before execution
- C) Compiling all code ahead-of-time during installation
- D) A tool for minifying .NET assemblies

**✅ Answer: B) Translating IL to native machine code at runtime, just before execution**

> JIT (Just-In-Time) compiles IL to native code the first time each method is called. The compiled code is cached for subsequent calls. AOT (Ahead-of-Time) compilation (like Native AOT in .NET 7+) pre-compiles to native code for faster startup.

---

**Q42.** What is an Assembly in .NET?
- A) A compiled .exe or .dll file that is the unit of deployment and versioning in .NET
- B) A collection of C# source files
- C) A NuGet package
- D) A project file (.csproj)

**✅ Answer: A) A compiled .exe or .dll file that is the unit of deployment and versioning in .NET**

> An assembly is the basic unit of deployment in .NET. It contains IL code, a manifest (metadata), and optionally resources. Assemblies are versioned and can be strongly named for GAC deployment.

---

**Q43.** What is the GAC (Global Assembly Cache)?
- A) A central repository for storing shared .NET assemblies across multiple applications
- B) A NuGet package cache folder
- C) The garbage collector's internal memory cache
- D) A compiled cache of JIT-compiled native code

**✅ Answer: A) A central repository for storing shared .NET assemblies across multiple applications**

> The GAC stores strongly named assemblies that can be shared by multiple applications on the same machine. It's less relevant in modern .NET (Core) which prefers self-contained deployments.

---

**Q44.** What does `AppDomain` represent?
- A) A logical isolation boundary for running .NET code within a process
- B) The URL domain of a web application
- C) A thread pool partition
- D) A network domain for authentication

**✅ Answer: A) A logical isolation boundary for running .NET code within a process**

> In .NET Framework, `AppDomain` provided isolation within a process (separate heaps, security boundaries). In .NET Core/.NET 5+, AppDomains are not supported for isolation — use separate processes instead.

---

**Q45.** What is the purpose of `GC.SuppressFinalize(this)`?
- A) Prevents the GC from ever collecting the object
- B) Tells the GC not to call the finalizer since cleanup was already done in Dispose()
- C) Forces an immediate garbage collection
- D) Removes an object from all references

**✅ Answer: B) Tells the GC not to call the finalizer since cleanup was already done in Dispose()**

> In the Dispose pattern, after calling `Dispose()`, you call `GC.SuppressFinalize(this)` to prevent the finalizer from running again, which would be wasteful since resources are already released.

---

**Q46.** What is the difference between `Stack` and `Heap` in .NET?
- A) Stack stores reference types; Heap stores value types
- B) Stack stores value types and method frames (LIFO); Heap stores reference type instances (GC-managed)
- C) Stack is for multi-threading; Heap is single-threaded
- D) They are the same memory region

**✅ Answer: B) Stack stores value types and method frames (LIFO); Heap stores reference type instances (GC-managed)**

> The stack holds local value types and method call frames. The heap holds all objects (reference types). When a method exits, stack frames are popped automatically. Heap objects persist until the GC collects them.

---

**Q47.** What does `WeakReference<T>` allow?
- A) A reference that increases reference count but not GC eligibility
- B) A reference to an object that doesn't prevent GC from collecting it
- C) A reference that is automatically nulled when the object is accessed
- D) A thread-local reference

**✅ Answer: B) A reference to an object that doesn't prevent GC from collecting it**

> `WeakReference<T>` lets you hold a reference to an object without rooting it. The GC can still collect the object. Useful for caches — you can check `TryGetTarget()` and rebuild if collected. Prevents memory leaks in cache scenarios.

---

**Q48.** What is Reflection in .NET?
- A) A design pattern for mirroring object state
- B) The ability to inspect and manipulate types, methods, and properties at runtime
- C) A compile-time code generation feature
- D) An optical networking protocol

**✅ Answer: B) The ability to inspect and manipulate types, methods, and properties at runtime**

> Reflection allows examining assemblies, types, and members at runtime via `System.Reflection`. It powers DI containers, ORMs, serializers, and test frameworks. Performance overhead is significant; cache `MethodInfo` objects if using frequently.

---

**Q49.** What is the purpose of `Activator.CreateInstance()`?
- A) Starts the .NET runtime
- B) Creates an instance of a type dynamically at runtime using reflection
- C) Activates a background service
- D) Initializes a static class

**✅ Answer: B) Creates an instance of a type dynamically at runtime using reflection**

> `Activator.CreateInstance(type)` creates an object of the given type without knowing it at compile time. Used in DI containers, plugin systems, and factories. It invokes the parameterless constructor by default.

---

**Q50.** What is a strongly-typed Assembly?
- A) An assembly compiled with strict type checking
- B) An assembly signed with a public/private key pair to guarantee identity and version
- C) An assembly with no external dependencies
- D) An assembly stored in the GAC by default

**✅ Answer: B) An assembly signed with a public/private key pair to guarantee identity and version**

> Strong naming gives an assembly a unique identity (name + version + culture + public key token). This prevents assembly spoofing and is required for GAC deployment.

---

**Q51.** What is the difference between `System.Exception` and `System.ApplicationException`?
- A) `ApplicationException` is for CLR errors; `Exception` is for application errors
- B) They are functionally equivalent — `ApplicationException` was intended for app-level errors but the distinction is no longer recommended
- C) `ApplicationException` cannot be caught
- D) `System.Exception` is for fatal errors only

**✅ Answer: B) They are functionally equivalent — `ApplicationException` was intended for app-level errors but the distinction is no longer recommended**

> `ApplicationException` was originally meant to distinguish app-level from system-level exceptions, but this convention was abandoned. Best practice is to derive custom exceptions directly from `Exception`.

---

**Q52.** What does the `[Serializable]` attribute do?
- A) Makes a class JSON-serializable
- B) Marks a class as compatible with binary serialization (BinaryFormatter)
- C) Enables XML serialization
- D) Prevents the class from being serialized

**✅ Answer: B) Marks a class as compatible with binary serialization (BinaryFormatter)**

> `[Serializable]` enables `BinaryFormatter` and `SoapFormatter` serialization. Note: `BinaryFormatter` is obsolete and removed in .NET 9 due to security vulnerabilities. Use `System.Text.Json` or `Newtonsoft.Json` instead.

---

**Q53.** What is `CultureInfo` used for?
- A) Managing database connection cultures
- B) Providing locale-specific formatting for dates, numbers, currencies, and strings
- C) Setting the UI language of Windows
- D) Configuring time zone information

**✅ Answer: B) Providing locale-specific formatting for dates, numbers, currencies, and strings**

> `CultureInfo` controls formatting rules for dates (`dd/MM/yyyy` vs `MM/dd/yyyy`), numbers (decimal separators), and currencies. Use `CultureInfo.InvariantCulture` for serialization/parsing to avoid locale-dependent bugs.

---

**Q54.** What does `Environment.Exit(0)` do?
- A) Exits the current method
- B) Terminates the process immediately with exit code 0
- C) Disposes all IDisposable objects
- D) Causes a soft restart of the application

**✅ Answer: B) Terminates the process immediately with exit code 0**

> `Environment.Exit()` terminates the process. Exit code 0 conventionally means success. Unlike an exception, it does not unwind the stack, so `finally` blocks may not run. Avoid in production code — prefer graceful shutdown.

---

**Q55.** What is `Marshal` class used for?
- A) Coordinating parallel tasks
- B) Interoperating with unmanaged (native) code — allocating/freeing unmanaged memory, converting types
- C) Serializing objects to JSON
- D) Managing thread synchronization

**✅ Answer: B) Interoperating with unmanaged (native) code — allocating/freeing unmanaged memory, converting types**

> `System.Runtime.InteropServices.Marshal` provides methods for P/Invoke interop: converting between managed and unmanaged types, allocating unmanaged memory, and copying data between managed and native buffers.

---

## SECTION 3: ASP.NET CORE (Q56–Q80)

---

**Q56.** What is the role of `Program.cs` in ASP.NET Core 6+?
- A) The entry point that configures services (DI) and the HTTP middleware pipeline
- B) A configuration file for routing only
- C) The main controller class
- D) The startup script for IIS

**✅ Answer: A) The entry point that configures services (DI) and the HTTP middleware pipeline**

> In .NET 6+, `Program.cs` uses the minimal hosting model: `builder.Services` for DI registration, and `app.Use*` for middleware pipeline configuration. `Startup.cs` is no longer required.

---

**Q57.** What is middleware in ASP.NET Core?
- A) A database abstraction layer
- B) Software components in the HTTP request/response pipeline that can process, modify, or short-circuit requests
- C) JavaScript libraries loaded by the server
- D) Authentication tokens

**✅ Answer: B) Software components in the HTTP request/response pipeline that can process, modify, or short-circuit requests**

> Middleware forms a pipeline. Each component calls `next()` to pass control. Examples: authentication, logging, exception handling, CORS, routing. Order matters — middleware is executed in registration order.

---

**Q58.** What is the difference between `app.Use()` and `app.Run()` in middleware?
- A) `app.Use()` is for terminal middleware; `app.Run()` passes to the next component
- B) `app.Use()` can call next middleware; `app.Run()` is terminal and does not call next
- C) They are identical
- D) `app.Run()` is for routing only

**✅ Answer: B) `app.Use()` can call next middleware; `app.Run()` is terminal and does not call next**

> `app.Use()` takes a `next` parameter and can continue the pipeline. `app.Run()` is a terminal delegate — nothing runs after it. Using `app.Run()` before other middleware will short-circuit all subsequent middleware.

---

**Q59.** What does `[ApiController]` attribute do in ASP.NET Core?
- A) Marks a class as a view controller for MVC
- B) Enables automatic model validation, binding source inference, and problem details responses
- C) Registers the controller in DI
- D) Enables routing for Razor Pages

**✅ Answer: B) Enables automatic model validation, binding source inference, and problem details responses**

> `[ApiController]` automatically returns 400 responses when `ModelState` is invalid, infers `[FromBody]` for complex types, and formats error responses using `ProblemDetails`. It's required for proper Web API behavior.

---

**Q60.** What is the difference between `IActionResult` and `ActionResult<T>`?
- A) They are identical
- B) `ActionResult<T>` provides type-safe return values enabling Swagger/OpenAPI to document response types; `IActionResult` is untyped
- C) `IActionResult` is for async controllers; `ActionResult<T>` is for sync
- D) `ActionResult<T>` only works with JSON

**✅ Answer: B) `ActionResult<T>` provides type-safe return values enabling Swagger/OpenAPI to document response types; `IActionResult` is untyped**

> `ActionResult<T>` allows returning either an `ActionResult` (e.g., `NotFound()`) or a `T` directly, and OpenAPI/Swagger can automatically detect the `T` type for documentation.

---

**Q61.** What is Razor Pages vs MVC in ASP.NET Core?
- A) Razor Pages uses a page-centric model (page + code-behind); MVC uses a Controller-View-Model pattern
- B) Razor Pages is for APIs; MVC is for web pages
- C) They are the same pattern
- D) MVC is deprecated in favor of Razor Pages

**✅ Answer: A) Razor Pages uses a page-centric model (page + code-behind); MVC uses a Controller-View-Model pattern**

> Razor Pages organizes page-specific logic into `PageModel` classes co-located with the `.cshtml` view. MVC separates controllers, views, and models. Razor Pages is simpler for page-focused UIs; MVC offers more flexibility.

---

**Q62.** What is the purpose of `appsettings.json`?
- A) To store compiled application binaries
- B) To provide hierarchical application configuration (connection strings, feature flags, etc.)
- C) To define HTTP routes
- D) To store NuGet package references

**✅ Answer: B) To provide hierarchical application configuration (connection strings, feature flags, etc.)**

> `appsettings.json` stores configuration in JSON format. It can be overridden by `appsettings.{Environment}.json`, environment variables, or command-line args. Accessed via `IConfiguration` in DI.

---

**Q63.** How do you bind configuration sections to strongly-typed classes?
- A) Using `[Bind]` attribute on the class
- B) Using `services.Configure<T>(configuration.GetSection("SectionName"))`
- C) Manually reading each key from IConfiguration
- D) Using `[Configuration]` attribute

**✅ Answer: B) Using `services.Configure<T>(configuration.GetSection("SectionName"))`**

> `Configure<T>` binds a config section to a POCO class and registers it as `IOptions<T>`. Inject `IOptions<T>` (static snapshot), `IOptionsSnapshot<T>` (reloaded per request), or `IOptionsMonitor<T>` (change notifications).

---

**Q64.** What does `UseRouting()` and `UseEndpoints()` do?
- A) `UseRouting()` matches incoming requests to route patterns; `UseEndpoints()` executes the matched endpoint
- B) `UseRouting()` sends responses; `UseEndpoints()` reads requests
- C) They configure SSL routing
- D) They are only needed for Razor Pages

**✅ Answer: A) `UseRouting()` matches incoming requests to route patterns; `UseEndpoints()` executes the matched endpoint**

> `UseRouting()` analyzes the request and selects an endpoint. Middleware between `UseRouting` and `UseEndpoints` can inspect the selected endpoint (e.g., for authorization). `UseEndpoints()` executes the matched controller action or route handler.

---

**Q65.** What is Content Negotiation in ASP.NET Core Web API?
- A) Negotiating SSL certificates
- B) The process of selecting the response format (JSON, XML, etc.) based on the `Accept` header
- C) Compressing HTTP responses
- D) Caching API responses

**✅ Answer: B) The process of selecting the response format (JSON, XML, etc.) based on the `Accept` header**

> When a client sends `Accept: application/xml`, the server uses output formatters to serialize the response as XML if the formatter is registered. By default, ASP.NET Core only includes JSON formatter. XML requires `AddXmlSerializerFormatters()`.

---

**Q66.** What is the purpose of `[Route]` attribute in ASP.NET Core?
- A) Registers the controller in the service container
- B) Defines URL patterns for controller actions
- C) Specifies allowed HTTP methods
- D) Configures response caching

**✅ Answer: B) Defines URL patterns for controller actions**

> `[Route("api/[controller]")]` on a controller sets the base route. `[Route("{id}")]` on an action adds a template. Route tokens like `[controller]` and `[action]` are replaced with the class/method names.

---

**Q67.** What is Model Binding in ASP.NET Core?
- A) Connecting the database to model classes
- B) The process of mapping HTTP request data (query strings, route values, body) to action method parameters
- C) Binding views to models in MVC
- D) Linking two model classes with a foreign key

**✅ Answer: B) The process of mapping HTTP request data (query strings, route values, body) to action method parameters**

> Model binding automatically maps incoming request data to action parameters. Sources include route data, query string, form data, and body (JSON). `[FromBody]`, `[FromQuery]`, `[FromRoute]` control the source explicitly.

---

**Q68.** What does `[HttpGet]`, `[HttpPost]` etc. do?
- A) Restricts the action to a specific HTTP method
- B) Specifies the response content type
- C) Defines authorization requirements
- D) Sets the HTTP response status code

**✅ Answer: A) Restricts the action to a specific HTTP method**

> HTTP method attributes restrict which HTTP verb triggers the action. `[HttpGet]` only responds to GET, `[HttpPost]` to POST, etc. They can also define route templates: `[HttpGet("{id:int}")]`.

---

**Q69.** What is IHostedService?
- A) An interface for creating background services in ASP.NET Core
- B) A service for hosting static files
- C) An IIS hosting configuration interface
- D) A service for managing HTTP sessions

**✅ Answer: A) An interface for creating background services in ASP.NET Core**

> `IHostedService` (or `BackgroundService`) allows long-running background tasks (message queue consumers, schedulers, health probes). Register with `services.AddHostedService<T>()`. `BackgroundService` is an abstract base that simplifies implementation.

---

**Q70.** What is CORS in ASP.NET Core?
- A) A caching strategy for static files
- B) Cross-Origin Resource Sharing — a mechanism that controls which domains can make browser requests to your API
- C) A compression algorithm
- D) A SQL injection prevention technique

**✅ Answer: B) Cross-Origin Resource Sharing — a mechanism that controls which domains can make browser requests to your API**

> Browsers enforce the same-origin policy. CORS headers (`Access-Control-Allow-Origin`) tell the browser which cross-origin requests are allowed. In ASP.NET Core, configure with `services.AddCors()` and `app.UseCors()`.

---

**Q71.** What is the purpose of `IActionFilter`?
- A) Filters invalid model states automatically
- B) Intercepts controller action execution — runs before and after an action
- C) Validates JWT tokens
- D) Compresses action results

**✅ Answer: B) Intercepts controller action execution — runs before and after an action**

> Action filters implement `OnActionExecuting` (before) and `OnActionExecuted` (after) the action runs. Used for logging, validation, caching, and modifying results. Other filter types: Authorization, Resource, Exception, Result.

---

**Q72.** What is the Minimal API feature in .NET 6+?
- A) A stripped-down version of ASP.NET with no routing
- B) A lightweight approach to define HTTP endpoints with minimal ceremony, without controllers
- C) An API with no authentication
- D) A performance-only API mode

**✅ Answer: B) A lightweight approach to define HTTP endpoints with minimal ceremony, without controllers**

> Minimal APIs define endpoints directly: `app.MapGet("/products", () => db.Products.ToList())`. No controllers or action attributes needed. Excellent for microservices and small APIs.

---

**Q73.** What does `app.UseExceptionHandler("/Error")` do?
- A) Logs all exceptions to a file
- B) Catches unhandled exceptions and redirects to the specified error path
- C) Prevents exceptions from propagating
- D) Sends 500 status without a body

**✅ Answer: B) Catches unhandled exceptions and redirects to the specified error path**

> `UseExceptionHandler` is production-grade exception middleware. In development, `UseDeveloperExceptionPage()` is preferred instead (shows full stack trace). In production, redirect to an error page or use `UseExceptionHandler(opts => opts.Run(...))` for JSON error responses.

---

**Q74.** What is Response Caching in ASP.NET Core?
- A) Storing database query results in memory
- B) Caching HTTP responses on the server or client based on response headers (`Cache-Control`)
- C) Caching authentication tokens
- D) Caching static files on CDN

**✅ Answer: B) Caching HTTP responses on the server or client based on response headers (`Cache-Control`)**

> `[ResponseCache]` attribute adds `Cache-Control` headers. The `UseResponseCaching()` middleware enables server-side caching. `IMemoryCache` or `IDistributedCache` provide in-process/Redis caching for more control.

---

**Q75.** What is Health Checks in ASP.NET Core?
- A) A code quality tool
- B) A mechanism to expose endpoints reporting the health status of dependencies (DB, external APIs, etc.)
- C) A unit test framework
- D) A monitoring tool for CPU usage

**✅ Answer: B) A mechanism to expose endpoints reporting the health status of dependencies (DB, external APIs, etc.)**

> Health checks are registered via `services.AddHealthChecks()` and exposed at `/health`. Kubernetes liveness/readiness probes, load balancers, and monitoring tools use these endpoints to check if the app is ready to serve traffic.

---

**Q76.** What is `IHttpContextAccessor`?
- A) Provides access to the current HTTP context outside of controllers (e.g., in services)
- B) A client for making outbound HTTP requests
- C) An accessor for reading HTTP headers from config
- D) A factory for creating HttpClient instances

**✅ Answer: A) Provides access to the current HTTP context outside of controllers (e.g., in services)**

> Controllers have `HttpContext` directly. Services don't. `IHttpContextAccessor` lets services access the request context (user identity, headers). Register with `services.AddHttpContextAccessor()`. Avoid in concurrent scenarios.

---

**Q77.** What does `UseHttpsRedirection()` do?
- A) Encrypts the database connection
- B) Automatically redirects HTTP requests to HTTPS
- C) Validates SSL certificates
- D) Configures HSTS headers

**✅ Answer: B) Automatically redirects HTTP requests to HTTPS**

> `UseHttpsRedirection()` returns a 301/307 redirect from HTTP to HTTPS. Combined with `UseHsts()` (which sets the Strict-Transport-Security header), it ensures clients always use HTTPS.

---

**Q78.** What is Output Caching in .NET 7+?
- A) Identical to Response Caching
- B) A new, more powerful caching middleware that caches full response bodies server-side with support for cache invalidation and policies
- C) A front-end caching mechanism
- D) Caching of compiled Razor views

**✅ Answer: B) A new, more powerful caching middleware that caches full response bodies server-side with support for cache invalidation and policies**

> Output Caching (added in .NET 7) improves on Response Caching with cache invalidation tags, configurable policies, and `vary-by` conditions. It stores the full response and can be invalidated programmatically.

---

**Q79.** What is Rate Limiting in ASP.NET Core 7+?
- A) Limiting API response size
- B) Throttling incoming requests to protect the server from overload
- C) Setting CPU usage caps for each request
- D) Limiting the number of database connections

**✅ Answer: B) Throttling incoming requests to protect the server from overload**

> ASP.NET Core 7+ includes built-in rate limiting middleware (`UseRateLimiter()`). Supports fixed window, sliding window, token bucket, and concurrency limiters. Configured via `services.AddRateLimiter()`.

---

**Q80.** What is the difference between `AddSingleton`, `AddScoped`, and `AddTransient`?
- A) Singleton = one per request; Scoped = one per app; Transient = always new
- B) Singleton = one per app lifetime; Scoped = one per HTTP request; Transient = new instance every time
- C) They all create new instances on each resolution
- D) Scoped is for databases only

**✅ Answer: B) Singleton = one per app lifetime; Scoped = one per HTTP request; Transient = new instance every time**

> This is critical for DI lifetime management. Singletons are shared application-wide. Scoped instances are shared within a request but new across requests. Transient instances are always new — best for lightweight, stateless services.

---

## SECTION 4: ENTITY FRAMEWORK CORE (Q81–Q100)

---

**Q81.** What is Entity Framework Core?
- A) A JavaScript ORM for Node.js
- B) A lightweight, cross-platform Object-Relational Mapper for .NET
- C) A SQL query parser
- D) A database migration tool only

**✅ Answer: B) A lightweight, cross-platform Object-Relational Mapper for .NET**

> EF Core maps .NET objects to database tables, allowing CRUD operations via LINQ without writing raw SQL. It supports code-first, database-first, and migrations.

---

**Q82.** What is the difference between `Add` and `Attach` in EF Core?
- A) `Add` marks an entity as new (INSERT); `Attach` marks it as existing (no immediate SQL, just tracking)
- B) `Attach` marks an entity as new; `Add` is for existing entities
- C) They are identical
- D) `Add` is for bulk inserts; `Attach` is for single records

**✅ Answer: A) `Add` marks an entity as new (INSERT); `Attach` marks it as existing (no immediate SQL, just tracking)**

> `Add` puts the entity in `EntityState.Added` — EF will INSERT it on `SaveChanges`. `Attach` puts it in `EntityState.Unchanged` — useful when you have a disconnected entity you want to track without inserting.

---

**Q83.** What is Lazy Loading in EF Core?
- A) Deferring database operations until needed
- B) Automatically loading related entities when a navigation property is first accessed
- C) Loading only the first 10 rows of a result set
- D) Caching query results for later use

**✅ Answer: B) Automatically loading related entities when a navigation property is first accessed**

> With lazy loading enabled (requires `Microsoft.EntityFrameworkCore.Proxies`), accessing `order.Customer` will automatically query the database if `Customer` hasn't been loaded. This can cause N+1 query problems.

---

**Q84.** What is Eager Loading in EF Core?
- A) Loading all data upfront regardless of need
- B) Loading related entities as part of the initial query using `.Include()`
- C) Caching entities for fast access
- D) Pre-compiling LINQ queries

**✅ Answer: B) Loading related entities as part of the initial query using `.Include()`**

> `dbContext.Orders.Include(o => o.Customer).Include(o => o.Items).ToList()` generates a JOIN query fetching orders with their customers and items in a single round trip, avoiding N+1.

---

**Q85.** What is N+1 query problem in EF Core?
- A) Running the same query 1000 times
- B) Executing 1 query to get a list, then N additional queries to get related data for each item
- C) A query with N joins
- D) A query that returns N+1 rows than expected

**✅ Answer: B) Executing 1 query to get a list, then N additional queries to get related data for each item**

> Example: Querying 100 orders without `.Include(o => o.Customer)` then accessing `order.Customer` in a loop fires 100 additional DB queries. Solve with Eager Loading (`.Include()`) or Explicit Loading.

---

**Q86.** What is a Migration in EF Core?
- A) Moving the database to a different server
- B) A code-generated file that describes incremental schema changes to apply to the database
- C) A data transfer between tables
- D) Converting from SQL Server to PostgreSQL

**✅ Answer: B) A code-generated file that describes incremental schema changes to apply to the database**

> Migrations (via `dotnet ef migrations add`) generate `Up()` and `Down()` methods representing schema changes. `dotnet ef database update` applies pending migrations to the database.

---

**Q87.** What does `.AsNoTracking()` do?
- A) Disables LINQ query translation
- B) Tells EF Core not to track the returned entities, improving read-only query performance
- C) Prevents entities from being deleted
- D) Disables lazy loading for the query

**✅ Answer: B) Tells EF Core not to track the returned entities, improving read-only query performance**

> By default, EF Core tracks all loaded entities in the `ChangeTracker`. `.AsNoTracking()` skips this for read-only scenarios, reducing memory usage and improving performance — ideal for GET endpoints.

---

**Q88.** What is the `DbContext` in EF Core?
- A) A database connection string
- B) The primary class for database operations — represents a session with the database
- C) A configuration file for database settings
- D) A migration runner

**✅ Answer: B) The primary class for database operations — represents a session with the database**

> `DbContext` exposes `DbSet<T>` properties for each entity, manages change tracking, and handles the `SaveChanges()` unit-of-work pattern. It should be registered as `Scoped` in DI.

---

**Q89.** What does `SaveChanges()` do?
- A) Writes changes to a local cache
- B) Generates and executes SQL INSERT/UPDATE/DELETE statements for all tracked entity changes
- C) Commits a transaction manually
- D) Refreshes the entities from the database

**✅ Answer: B) Generates and executes SQL INSERT/UPDATE/DELETE statements for all tracked entity changes**

> `SaveChanges()` inspects the ChangeTracker, generates SQL for Added/Modified/Deleted entities, and executes them in a transaction. `SaveChangesAsync()` is the async version.

---

**Q90.** What is the Fluent API in EF Core?
- A) A JavaScript query syntax for EF
- B) A way to configure entity mappings in code using method chaining in `OnModelCreating()`
- C) A REST API style for querying data
- D) An auto-generated API layer on top of EF

**✅ Answer: B) A way to configure entity mappings in code using method chaining in `OnModelCreating()`**

> Fluent API (`modelBuilder.Entity<Order>().HasKey(o => o.Id)`) configures table names, column types, relationships, indexes, etc. It's more powerful than Data Annotations and keeps entity classes clean.

---

**Q91.** What is the difference between `.Find()` and `.FirstOrDefault()`?
- A) `.Find()` uses primary key and checks the cache first; `.FirstOrDefault()` always queries the database
- B) `.FirstOrDefault()` checks cache; `.Find()` always queries
- C) They are identical
- D) `.Find()` returns all matches; `.FirstOrDefault()` returns one

**✅ Answer: A) `.Find()` uses primary key and checks the cache first; `.FirstOrDefault()` always queries the database**

> `Find(id)` checks the ChangeTracker first before hitting the database — perfect for cache-friendly lookups by primary key. `FirstOrDefault()` always translates to SQL.

---

**Q92.** What is Explicit Loading in EF Core?
- A) Loading entities using raw SQL
- B) Manually loading related entities after the main entity is already loaded, using `.Entry().Collection().Load()`
- C) Using `Include()` in the initial query
- D) Loading all tables at application startup

**✅ Answer: B) Manually loading related entities after the main entity is already loaded, using `.Entry().Collection().Load()`**

> Explicit loading: `context.Entry(order).Collection(o => o.Items).Load()`. Useful when you need related data conditionally — load only if needed, avoiding unnecessary joins.

---

**Q93.** What does `[DatabaseGenerated(DatabaseGeneratedOption.Identity)]` do?
- A) Marks the column as auto-incremented by the database
- B) Makes the column read-only
- C) Sets the column as the primary key
- D) Generates a GUID automatically

**✅ Answer: A) Marks the column as auto-incremented by the database**

> This tells EF Core that the database generates the value on INSERT (e.g., `IDENTITY` in SQL Server). EF won't try to insert a value for this column and will read back the generated value after INSERT.

---

**Q94.** What is Connection Resiliency in EF Core?
- A) Automatically scaling database connections
- B) Automatic retry logic for transient database failures
- C) Load balancing database connections
- D) Encrypting the connection string

**✅ Answer: B) Automatic retry logic for transient database failures**

> Configure with `options.UseNpgsql(cs, o => o.EnableRetryOnFailure())`. EF Core's execution strategy retries failed operations (network blips, timeout) up to a specified number of times — critical for cloud databases.

---

**Q95.** What is the purpose of `IEntityTypeConfiguration<T>`?
- A) Configuring HTTP endpoints for entities
- B) Separating Fluent API entity configuration into individual classes for organization
- C) Auto-generating APIs for entity types
- D) Validating entity data before saving

**✅ Answer: B) Separating Fluent API entity configuration into individual classes for organization**

> Instead of putting all mappings in `OnModelCreating()`, you create separate classes: `class OrderConfig : IEntityTypeConfiguration<Order>`. Apply with `modelBuilder.ApplyConfigurationsFromAssembly(assembly)`.

---

**Q96.** What does `HasQueryFilter()` do in EF Core?
- A) Adds a global WHERE clause to all queries for that entity type (e.g., soft delete, multi-tenancy)
- B) Validates query syntax at startup
- C) Limits query result size
- D) Caches query results

**✅ Answer: A) Adds a global WHERE clause to all queries for that entity type (e.g., soft delete, multi-tenancy)**

> `modelBuilder.Entity<Post>().HasQueryFilter(p => !p.IsDeleted)` automatically appends `WHERE IsDeleted = 0` to every query for `Post`. Essential for soft-delete patterns. Override with `.IgnoreQueryFilters()`.

---

**Q97.** What is Split Query in EF Core?
- A) Running queries on multiple databases simultaneously
- B) Splitting a single LINQ query with multiple includes into separate SQL queries to avoid cartesian explosion
- C) Partitioning large queries for parallel execution
- D) Breaking queries into pages

**✅ Answer: B) Splitting a single LINQ query with multiple includes into separate SQL queries to avoid cartesian explosion**

> `AsSplitQuery()` tells EF to execute multiple SELECT statements instead of one with JOINs. Without it, joining multiple collection navigations causes row multiplication (cartesian explosion). Each query is efficient but requires multiple round trips.

---

**Q98.** What is Owned Entity in EF Core?
- A) An entity that is always private
- B) A type whose lifetime is tied to an owner entity — mapped as part of the owner's table by default
- C) An entity with a composite key
- D) An entity that cannot be queried independently

**✅ Answer: B) A type whose lifetime is tied to an owner entity — mapped as part of the owner's table by default**

> Owned entities (configured with `OwnsOne` / `OwnsMany`) represent value objects stored within the owner's table. Example: `Address` owned by `Customer` — `Address` columns live in the `Customers` table.

---

**Q99.** What is `ExecuteUpdateAsync()` in EF Core 7+?
- A) Runs migrations asynchronously
- B) Performs a bulk UPDATE via SQL without loading entities into memory
- C) Updates the connection string at runtime
- D) Schedules deferred updates

**✅ Answer: B) Performs a bulk UPDATE via SQL without loading entities into memory**

> EF Core 7 introduced `ExecuteUpdateAsync()` and `ExecuteDeleteAsync()` for bulk operations. Unlike `SaveChanges()`, they don't load entities into the ChangeTracker — generating a single efficient UPDATE/DELETE SQL statement.

---

**Q100.** What is a Shadow Property in EF Core?
- A) A property visible only in debug mode
- B) A property defined in the EF model but not in the entity class — exists only in the database schema
- C) A calculated property that isn't persisted
- D) A private backing field

**✅ Answer: B) A property defined in the EF model but not in the entity class — exists only in the database schema**

> Shadow properties are configured via Fluent API: `modelBuilder.Entity<Blog>().Property<DateTime>("LastUpdated")`. EF manages them transparently. Useful for audit fields without polluting domain models.

---

## SECTION 5: LINQ (Q101–Q115)

---

**Q101.** What is deferred execution in LINQ?
- A) Running LINQ queries on a background thread
- B) The query is not executed when defined — it executes when iterated (e.g., `foreach`, `.ToList()`)
- C) Caching query results for later use
- D) Postponing compilation of LINQ expressions

**✅ Answer: B) The query is not executed when defined — it executes when iterated (e.g., `foreach`, `.ToList()`)**

> LINQ methods like `Where`, `Select`, `OrderBy` build an expression tree (or iterator chain) but don't execute. Execution is triggered by `ToList()`, `ToArray()`, `First()`, `foreach`, etc. Immediate methods: `Count()`, `Sum()`, `Any()`.

---

**Q102.** What is the difference between `First()` and `FirstOrDefault()`?
- A) `First()` returns null if no match; `FirstOrDefault()` throws an exception
- B) `First()` throws `InvalidOperationException` if no match; `FirstOrDefault()` returns the default value
- C) They are identical
- D) `FirstOrDefault()` is slower than `First()`

**✅ Answer: B) `First()` throws `InvalidOperationException` if no match; `FirstOrDefault()` returns the default value**

> `First()` assumes at least one element exists. `FirstOrDefault()` safely returns `null` (for reference types) or `default(T)` if nothing matches. Use `FirstOrDefault()` when absence is expected.

---

**Q103.** What does `SelectMany()` do?
- A) Selects multiple properties from each element
- B) Flattens a collection of collections into a single sequence
- C) Selects elements matching multiple conditions
- D) Selects the first and last elements

**✅ Answer: B) Flattens a collection of collections into a single sequence**

> `orders.SelectMany(o => o.Items)` returns all items across all orders as a flat `IEnumerable<Item>`. It's equivalent to a nested `foreach` that yields each inner element.

---

**Q104.** What is the difference between `Any()` and `All()`?
- A) `Any()` returns true if ALL elements match; `All()` if ANY match
- B) `Any()` returns true if at least one element matches; `All()` returns true only if every element matches
- C) They are identical
- D) `Any()` counts elements; `All()` validates types

**✅ Answer: B) `Any()` returns true if at least one element matches; `All()` returns true only if every element matches**

> `Any(predicate)` short-circuits at the first match. `All(predicate)` short-circuits at the first non-match. `Any()` (no predicate) just checks if the sequence is non-empty.

---

**Q105.** What does `GroupBy()` return?
- A) A sorted sequence of elements
- B) An `IEnumerable<IGrouping<TKey, TElement>>` — groups of elements sharing the same key
- C) A Dictionary<TKey, List<T>>
- D) A flat sequence with group indexes

**✅ Answer: B) An `IEnumerable<IGrouping<TKey, TElement>>` — groups of elements sharing the same key**

> `products.GroupBy(p => p.Category)` returns groups where each `IGrouping` has a `Key` (the category) and contains the matching products. Access via `group.Key` and iterating the group.

---

**Q106.** What is the difference between `Join` and `GroupJoin` in LINQ?
- A) `Join` is an inner join; `GroupJoin` is a left outer join equivalent
- B) `Join` is a left join; `GroupJoin` is an inner join
- C) They are identical
- D) `GroupJoin` filters nulls; `Join` does not

**✅ Answer: A) `Join` is an inner join; `GroupJoin` is a left outer join equivalent**

> `Join` returns only matching pairs (inner join). `GroupJoin` correlates each left element with a (possibly empty) collection of right elements — enabling left outer join behavior when combined with `SelectMany` and `DefaultIfEmpty`.

---

**Q107.** What does `Aggregate()` do?
- A) Counts aggregate statistics like SUM and AVG
- B) Applies an accumulator function over a sequence, building a single result
- C) Groups elements for aggregation
- D) Produces a Tuple of aggregated results

**✅ Answer: B) Applies an accumulator function over a sequence, building a single result**

> `Aggregate` is a fold/reduce operation. `numbers.Aggregate(0, (acc, n) => acc + n)` sums the numbers. It's the general form; `Sum()`, `Max()`, `Min()`, `Count()` are specialized aggregates.

---

**Q108.** What does `Zip()` do in LINQ?
- A) Compresses the sequence using ZIP compression
- B) Merges two sequences element-by-element using a selector function
- C) Removes duplicates from a sequence
- D) Pairs each element with its index

**✅ Answer: B) Merges two sequences element-by-element using a selector function**

> `Zip(second, selector)` pairs elements at matching indices: `names.Zip(scores, (n, s) => $"{n}: {s}")`. Stops at the shorter sequence. Useful for combining parallel arrays.

---

**Q109.** What is the difference between `Distinct()` and `DistinctBy()` in .NET 6+?
- A) `Distinct()` uses `.Equals()`; `DistinctBy()` deduplicates by a key selector
- B) `DistinctBy()` is case-insensitive; `Distinct()` is not
- C) They are identical
- D) `DistinctBy()` only works on strings

**✅ Answer: A) `Distinct()` uses `.Equals()`; `DistinctBy()` deduplicates by a key selector**

> `Distinct()` returns unique elements using equality. `DistinctBy(x => x.Id)` (added in .NET 6) deduplicates by a key — useful when you want the first occurrence of each unique key in a complex object sequence.

---

**Q110.** What does `TakeWhile()` do?
- A) Takes elements while a condition is true, stopping at the first false
- B) Takes elements that match a condition (same as `Where`)
- C) Takes a random subset of elements
- D) Takes elements until the sequence ends

**✅ Answer: A) Takes elements while a condition is true, stopping at the first false**

> `TakeWhile(predicate)` returns elements from the start as long as the predicate is true — stops at the first false. Contrast with `Where()` which filters throughout the entire sequence.

---

**Q111.** What is the difference between `IEnumerable.Count()` and `List.Count`?
- A) They are identical in performance
- B) `IEnumerable.Count()` iterates the entire sequence; `List.Count` is O(1) from a stored field
- C) `List.Count` is slower due to locking
- D) `IEnumerable.Count()` returns a cached count

**✅ Answer: B) `IEnumerable.Count()` iterates the entire sequence; `List.Count` is O(1) from a stored field**

> LINQ's `Count()` iterates the sequence (unless the underlying collection implements `ICollection<T>`, in which case it optimizes). `List<T>.Count` is a pre-computed property — always O(1).

---

**Q112.** What does `let` keyword do in a LINQ query expression?
- A) Declares a loop variable
- B) Introduces an intermediate range variable storing a sub-expression result for reuse
- C) Releases resources in the query
- D) Defines a lambda in query syntax

**✅ Answer: B) Introduces an intermediate range variable storing a sub-expression result for reuse**

> In query syntax: `let total = item.Qty * item.Price` computes and names a value inside the query, avoiding repeated computation. In method syntax, this is done with a `Select` that produces an anonymous type.

---

**Q113.** What does `AsParallel()` do?
- A) Runs queries on multiple databases
- B) Enables PLINQ (Parallel LINQ), processing elements concurrently using multiple threads
- C) Caches query results for parallel access
- D) Converts a query to async

**✅ Answer: B) Enables PLINQ (Parallel LINQ), processing elements concurrently using multiple threads**

> `data.AsParallel().Where(x => HeavyFilter(x))` runs the query on multiple CPU cores in parallel. Useful for CPU-bound operations on large datasets. Overhead makes it counterproductive for small collections.

---

**Q114.** What is the output?
```csharp
var nums = new[] { 1, 2, 3, 4, 5 };
var result = nums.Where(n => n > 2).Select(n => n * 2);
```
- A) `{ 3, 4, 5 }`
- B) `{ 6, 8, 10 }`
- C) `{ 2, 4, 6, 8, 10 }`
- D) Query is not executed yet — it's deferred

**✅ Answer: D) Query is not executed yet — it's deferred**

> The LINQ chain builds an iterator but doesn't execute until iterated. Adding `.ToList()` or using `foreach` would produce `{ 6, 8, 10 }` due to deferred execution.

---

**Q115.** What is `Lookup<TKey, TElement>` and how does it differ from `Dictionary`?
- A) `Lookup` is identical to `Dictionary` with a different name
- B) `Lookup` maps each key to multiple values (one-to-many); `Dictionary` maps each key to a single value
- C) `Dictionary` supports multiple values; `Lookup` supports only one
- D) `Lookup` is mutable; `Dictionary` is immutable

**✅ Answer: B) `Lookup` maps each key to multiple values (one-to-many); `Dictionary` maps each key to a single value**

> `ToLookup(x => x.Category)` creates a `Lookup` where each key has a collection of values — like a `Dictionary<TKey, IEnumerable<TElement>>` but returns empty collections for missing keys instead of throwing.

---

## SECTION 6: ASYNC/AWAIT & THREADING (Q116–Q130)

---

**Q116.** What does `async` and `await` do?
- A) Creates a new thread for the operation
- B) Transforms the method into a state machine, allowing non-blocking waiting on asynchronous operations
- C) Runs the method on the thread pool
- D) Creates a background task that runs independently

**✅ Answer: B) Transforms the method into a state machine, allowing non-blocking waiting on asynchronous operations**

> `await` suspends the method without blocking the thread, allowing the thread to process other work. The compiler generates a state machine that resumes the method when the awaited task completes.

---

**Q117.** What is `Task.Run()` used for?
- A) Running async code synchronously
- B) Offloading CPU-bound work to a thread pool thread
- C) Creating a canceled task
- D) Scheduling a task for a specific time

**✅ Answer: B) Offloading CPU-bound work to a thread pool thread**

> `Task.Run(() => HeavyComputation())` queues work to the thread pool. Don't use it for I/O-bound work (use `async/await` directly); use it for CPU-bound work that would block the calling thread.

---

**Q118.** What is the difference between `Task` and `ValueTask`?
- A) `ValueTask` always allocates on the heap; `Task` does not
- B) `ValueTask` avoids heap allocation when the result is synchronously available; `Task` always allocates
- C) They are identical
- D) `ValueTask` supports cancellation; `Task` does not

**✅ Answer: B) `ValueTask` avoids heap allocation when the result is synchronously available; `Task` always allocates**

> `ValueTask<T>` is a struct that avoids the heap allocation of `Task<T>` in hot paths where the value is often already available (e.g., cached results). Downside: can only be awaited once. Use for high-performance scenarios.

---

**Q119.** What is `ConfigureAwait(false)` used for?
- A) Canceling a task if it doesn't complete
- B) Tells the awaiter not to resume on the original synchronization context (useful in library code to avoid deadlocks)
- C) Configures the task to run on a specific thread
- D) Makes the await fault-tolerant

**✅ Answer: B) Tells the awaiter not to resume on the original synchronization context (useful in library code to avoid deadlocks)**

> In ASP.NET Core (which has no synchronization context), `ConfigureAwait(false)` has no effect. But in WinForms/WPF or .NET Framework ASP.NET (which has sync contexts), it prevents deadlocks in library code and can improve performance.

---

**Q120.** What is a deadlock in async C# code?
- A) Two tasks waiting indefinitely for each other to complete
- B) A task that never starts
- C) A race condition between two threads
- D) An exception that occurs in a finally block

**✅ Answer: A) Two tasks waiting indefinitely for each other to complete**

> Classic async deadlock: calling `.Result` or `.Wait()` on a Task in code with a synchronization context (ASP.NET, WinForms). The continuation needs the context to resume, but the context is blocked waiting for the task. Solution: use `async/await` throughout.

---

**Q121.** What does `CancellationToken` do?
- A) Cancels a database transaction
- B) Provides a cooperative cancellation mechanism for async operations
- C) Sets a timeout for async operations
- D) Cancels a thread

**✅ Answer: B) Provides a cooperative cancellation mechanism for async operations**

> `CancellationToken` allows callers to signal cancellation. Methods check `token.IsCancellationRequested` or call `token.ThrowIfCancellationRequested()`. Long-running operations should accept and honor `CancellationToken`.

---

**Q122.** What is `Parallel.ForEach`?
- A) An async version of foreach
- B) A parallel loop that distributes work across thread pool threads concurrently
- C) A foreach that runs on the UI thread
- D) A LINQ operator for parallel projection

**✅ Answer: B) A parallel loop that distributes work across thread pool threads concurrently**

> `Parallel.ForEach(list, item => Process(item))` processes items concurrently. Best for CPU-bound, independent workloads. Does not support `async` delegates natively — use `Task.WhenAll` + `Select` for async parallel execution.

---

**Q123.** What is `SemaphoreSlim` used for?
- A) Mutual exclusion for a single thread
- B) Limiting the number of threads that can access a resource concurrently
- C) Thread-local storage
- D) Signaling between threads

**✅ Answer: B) Limiting the number of threads that can access a resource concurrently**

> `SemaphoreSlim(3)` allows up to 3 concurrent accesses. It has async support (`WaitAsync()`), unlike the heavier `Semaphore`. Used for rate limiting, connection pooling, or throttling parallel HTTP requests.

---

**Q124.** What is `Interlocked.Increment()` used for?
- A) Incrementing a loop counter safely
- B) Performing an atomic increment on a variable, thread-safely, without a lock
- C) Incrementing a Semaphore
- D) Incrementing a concurrent collection

**✅ Answer: B) Performing an atomic increment on a variable, thread-safely, without a lock**

> `Interlocked.Increment(ref counter)` uses CPU-level atomic instructions to safely increment without a lock. Faster than `lock` for simple numeric operations. Part of `System.Threading`.

---

**Q125.** What is `Task.WhenAll()` vs `Task.WhenAny()`?
- A) `WhenAll` completes when any task finishes; `WhenAny` when all finish
- B) `WhenAll` completes when all tasks complete; `WhenAny` completes when the first task completes
- C) They are identical
- D) `WhenAny` waits for all with a timeout

**✅ Answer: B) `WhenAll` completes when all tasks complete; `WhenAny` completes when the first task completes**

> `await Task.WhenAll(t1, t2, t3)` runs all tasks concurrently and waits for all to finish. `await Task.WhenAny(t1, t2, t3)` returns when the fastest task completes — useful for timeout racing.

---

**Q126.** What is `ThreadLocal<T>`?
- A) A thread-safe collection
- B) A variable that has a separate value for each thread
- C) A lock used for thread synchronization
- D) A parameter passed across async calls

**✅ Answer: B) A variable that has a separate value for each thread**

> `ThreadLocal<T>` stores per-thread values — each thread sees its own instance. Useful for performance counters, parsers, or connection-like objects that shouldn't be shared. Contrast with `AsyncLocal<T>` which flows across async continuations.

---

**Q127.** What is a `Monitor` in C#?
- A) A class for monitoring CPU usage
- B) A synchronization primitive that provides exclusive lock ownership, used internally by the `lock` keyword
- C) A UI component for displaying thread status
- D) A diagnostic tool for memory

**✅ Answer: B) A synchronization primitive that provides exclusive lock ownership, used internally by the `lock` keyword**

> `lock(obj) { }` compiles to `Monitor.Enter(obj)` / `Monitor.Exit(obj)`. `Monitor` also provides `Wait()`, `Pulse()`, `PulseAll()` for condition-variable-style synchronization.

---

**Q128.** What is `async void` and why is it dangerous?
- A) A valid pattern for async event handlers that doesn't propagate exceptions properly
- B) A method that runs synchronously
- C) An optimization for fire-and-forget tasks
- D) Identical to `async Task`

**✅ Answer: A) A valid pattern for async event handlers that doesn't propagate exceptions properly**

> `async void` methods cannot be awaited, so exceptions are unobserved and crash the process. The only legitimate use is event handlers. Always prefer `async Task` — it allows awaiting, exception handling, and cancellation.

---

**Q129.** What is `Channel<T>` in .NET?
- A) A named pipe for inter-process communication
- B) A high-performance, thread-safe producer-consumer data structure for async data pipelines
- C) A network socket abstraction
- D) A Pub/Sub event bus

**✅ Answer: B) A high-performance, thread-safe producer-consumer data structure for async data pipelines**

> `Channel<T>` (in `System.Threading.Channels`) provides bounded/unbounded async queues. Producers write to `channel.Writer`, consumers read from `channel.Reader`. Significantly more efficient than `BlockingCollection<T>`.

---

**Q130.** What is the difference between `lock` and `Mutex`?
- A) `lock` works across processes; `Mutex` is process-local
- B) `lock` is process-local and uses Monitor; `Mutex` is cross-process and is slower
- C) They are identical
- D) `Mutex` is managed; `lock` uses unmanaged code

**✅ Answer: B) `lock` is process-local and uses Monitor; `Mutex` is cross-process and is slower**

> `lock` compiles to `Monitor.Enter/Exit` — fast, managed, in-process only. `Mutex` is a Windows kernel object usable across processes (e.g., single-instance app detection). `Mutex` is significantly slower due to kernel transitions.

---

## SECTION 7: DEPENDENCY INJECTION (Q131–Q140)

---

**Q131.** What is Dependency Injection (DI)?
- A) Injecting JavaScript into HTML pages
- B) A design pattern where dependencies are provided to a class externally rather than created internally
- C) A way to inject SQL into database queries
- D) A compile-time code generation technique

**✅ Answer: B) A design pattern where dependencies are provided to a class externally rather than created internally**

> DI decouples classes from their dependencies. Instead of `new EmailService()` inside a class, the service is injected via constructor. This enables testability, loose coupling, and swappable implementations.

---

**Q132.** What is the Service Locator pattern and why is it considered an anti-pattern?
- A) It's a pattern for locating physical servers
- B) Resolving dependencies from a container inside classes — hides dependencies and makes testing harder
- C) A caching pattern for services
- D) An improved version of DI

**✅ Answer: B) Resolving dependencies from a container inside classes — hides dependencies and makes testing harder**

> Service Locator (`serviceProvider.GetService<T>()` inside business logic) hides dependencies from class signatures, making them invisible to consumers and hard to mock in tests. Prefer explicit constructor injection.

---

**Q133.** What is the Captive Dependency problem in DI?
- A) A dependency that cannot be released from memory
- B) A longer-lived service (Singleton) holding a shorter-lived service (Scoped/Transient), causing the shorter-lived service to live too long
- C) A circular dependency between two services
- D) A service that requires authentication

**✅ Answer: B) A longer-lived service (Singleton) holding a shorter-lived service (Scoped/Transient), causing the shorter-lived service to live too long**

> If a Singleton captures a Scoped service, that Scoped instance won't be released per-request — it lives for the app lifetime. ASP.NET Core detects this and throws at startup in development. Solution: use `IServiceScopeFactory`.

---

**Q134.** How do you register multiple implementations of the same interface?
- A) It's not possible in .NET DI
- B) Register multiple times with `AddTransient/Scoped/Singleton`; inject as `IEnumerable<IInterface>`
- C) Only the last registration is used
- D) Use `AddSingleton` with a factory function

**✅ Answer: B) Register multiple implementations with `Add*`; inject as `IEnumerable<IInterface>`**

> Registering the same interface multiple times with `.AddTransient<INotifier, EmailNotifier>()` and `.AddTransient<INotifier, SmsNotifier>()` allows injecting `IEnumerable<INotifier>` to get all implementations.

---

**Q135.** What is `IServiceScope`?
- A) A scope that limits service registration namespaces
- B) A manually created DI scope — used when you need Scoped services outside of an HTTP request (e.g., in background services)
- C) A scope for unit testing
- D) An authentication scope

**✅ Answer: B) A manually created DI scope — used when you need Scoped services outside of an HTTP request**

> In `IHostedService`, there's no HTTP scope. Use `IServiceScopeFactory.CreateScope()` to create a scope, resolve Scoped services, use them, then dispose the scope to release resources.

---

**Q136.** What is `IOptions<T>`, `IOptionsSnapshot<T>`, and `IOptionsMonitor<T>`?
- A) They are identical
- B) `IOptions<T>` is a static snapshot; `IOptionsSnapshot<T>` reloads per request; `IOptionsMonitor<T>` provides change notifications
- C) `IOptionsMonitor` is obsolete; use `IOptions<T>`
- D) `IOptionsSnapshot<T>` is for Singleton services only

**✅ Answer: B) `IOptions<T>` is a static snapshot; `IOptionsSnapshot<T>` reloads per request; `IOptionsMonitor<T>` provides change notifications**

> `IOptions<T>`: configured once at startup. `IOptionsSnapshot<T>`: reloaded from config per request (Scoped). `IOptionsMonitor<T>`: can push change notifications to running singletons when config changes.

---

**Q137.** What is a Factory pattern in DI?
- A) A design pattern where DI creates objects via factories instead of direct resolution
- B) Registering a delegate that creates service instances, useful for runtime parameter injection
- C) A pattern for creating unit tests
- D) A way to create database connections

**✅ Answer: B) Registering a delegate that creates service instances, useful for runtime parameter injection**

> `services.AddTransient<IEmailService>(sp => new EmailService(sp.GetRequiredService<ILogger<EmailService>>(), "smtp.host"))` — factory registrations allow injecting parameters not available in DI (like connection strings or runtime config).

---

**Q138.** What does `GetRequiredService<T>()` do differently from `GetService<T>()`?
- A) `GetRequiredService` throws `InvalidOperationException` if the service isn't registered; `GetService` returns null
- B) They are identical
- C) `GetService` throws; `GetRequiredService` returns null
- D) `GetRequiredService` only works with Singleton services

**✅ Answer: A) `GetRequiredService` throws `InvalidOperationException` if the service isn't registered; `GetService` returns null**

> `GetRequiredService<T>()` is preferred when the service is mandatory — it fails fast with a clear error rather than a `NullReferenceException` later. `GetService<T>()` is for optional dependencies.

---

**Q139.** What is Property Injection vs Constructor Injection?
- A) Constructor injection is optional; property injection is mandatory
- B) Constructor injection provides dependencies via the constructor (preferred); property injection sets them via properties (less explicit)
- C) They are identical
- D) Property injection is faster than constructor injection

**✅ Answer: B) Constructor injection is preferred; property injection sets them via properties**

> Constructor injection is the standard pattern in .NET DI — it makes dependencies explicit and immutable. Property injection can be used for optional dependencies but hides them and makes the class harder to test.

---

**Q140.** Can you inject `IServiceProvider` and resolve services from it?
- A) No — it's not registered in DI
- B) Yes, but it's generally an anti-pattern (Service Locator); use it only in infrastructure code or when the service type is known only at runtime
- C) Yes, and it's the recommended approach
- D) Only in Singleton services

**✅ Answer: B) Yes, but it's generally an anti-pattern; use it only in infrastructure code or when the service type is known only at runtime**

> `IServiceProvider` is registered and can be injected. But using it inside business logic is the Service Locator anti-pattern. Legitimate uses: plugin systems, generic factories, or when the type is dynamic.

---

## SECTION 8: DESIGN PATTERNS (Q141–Q150)

---

**Q141.** What is the Repository Pattern?
- A) A pattern for storing code in version control
- B) An abstraction layer between business logic and data access, providing a collection-like interface for domain objects
- C) A factory for creating database connections
- D) A design pattern for managing HTTP caches

**✅ Answer: B) An abstraction layer between business logic and data access, providing a collection-like interface for domain objects**

> The Repository pattern isolates data access logic. Business code calls `repository.GetById(id)` without knowing if data comes from SQL, MongoDB, or a cache. Enables unit testing by mocking the repository interface.

---

**Q142.** What is the Unit of Work Pattern?
- A) Measuring the effort required to complete a task
- B) A pattern that maintains a list of objects affected by a business transaction and coordinates writing changes in a single operation
- C) A time-boxing technique for sprints
- D) A pattern for rate limiting API calls

**✅ Answer: B) A pattern that coordinates writing changes in a single operation**

> Unit of Work groups multiple repository operations into a single transaction. EF Core's `DbContext` is an implementation of the Unit of Work pattern — `SaveChanges()` commits all changes atomically.

---

**Q143.** What is the Singleton Design Pattern (not DI lifetime)?
- A) A class that can only be created in a single assembly
- B) A pattern that restricts instantiation of a class to a single instance, providing a global access point
- C) A class with only static members
- D) A thread-safe lazy loader

**✅ Answer: B) A pattern that restricts instantiation of a class to a single instance**

> Classic Singleton uses a private constructor and a static `Instance` property. Thread-safe implementations use `Lazy<T>` or double-check locking. In modern .NET apps, DI singletons are preferred over manual Singletons.

---

**Q144.** What is the Strategy Pattern?
- A) Defining a family of algorithms, encapsulating each one, making them interchangeable at runtime
- B) A business strategy planning tool
- C) Selecting the fastest database query strategy
- D) Configuring middleware pipeline strategies

**✅ Answer: A) Defining a family of algorithms, encapsulating each one, making them interchangeable at runtime**

> Strategy encapsulates algorithms behind an interface. Example: `ISortStrategy` with `BubbleSortStrategy`, `QuickSortStrategy`. The context holds a reference and delegates to whichever is injected/set. Eliminates switch statements.

---

**Q145.** What is the Decorator Pattern?
- A) Adding UI styling to classes
- B) Wrapping an object to add behavior without modifying the original class
- C) A pattern for logging decorative messages
- D) Adding attributes to method signatures

**✅ Answer: B) Wrapping an object to add behavior without modifying the original class**

> The Decorator wraps an interface implementation to add cross-cutting behavior (logging, caching, validation). `CachingRepository` wraps `SqlRepository` — DI injects the decorator transparently. Follows the Open/Closed Principle.

---

**Q146.** What is the Observer Pattern?
- A) Monitoring application performance
- B) A pattern where one object (subject) notifies multiple dependent objects (observers) when its state changes
- C) Watching file system changes
- D) Logging all method calls

**✅ Answer: B) One object notifies multiple dependent objects when state changes**

> C#'s `event` keyword is an implementation of the Observer pattern. Subjects fire events; observers subscribe. IObservable/IObserver (Rx.NET) provides a more powerful reactive version.

---

**Q147.** What is CQRS (Command Query Responsibility Segregation)?
- A) A database normalization technique
- B) Separating read (query) and write (command) operations into distinct models
- C) A REST API design pattern
- D) A caching invalidation strategy

**✅ Answer: B) Separating read and write operations into distinct models**

> CQRS uses separate models (and sometimes separate databases) for reads and writes. Commands mutate state; queries return data. Enables optimizing each side independently. Often combined with Event Sourcing and MediatR.

---

**Q148.** What is the Mediator Pattern (as used by MediatR)?
- A) A third-party service broker for APIs
- B) Reduces direct dependencies between components by having them communicate through a central mediator object
- C) A load balancing pattern
- D) A database query dispatcher

**✅ Answer: B) Reduces direct dependencies between components by communicating through a central mediator**

> MediatR implements this — controllers call `mediator.Send(command)` without knowing which handler processes it. This decouples controllers from application logic, enabling pipeline behaviors (logging, validation) via `IPipelineBehavior`.

---

**Q149.** What is the Factory Method Pattern?
- A) A factory class with a `Create()` method
- B) Defines an interface for creating objects but lets subclasses decide which classes to instantiate
- C) Creating objects using reflection
- D) A static class with constructor methods

**✅ Answer: B) Defines an interface for creating objects but lets subclasses decide which classes to instantiate**

> Factory Method defers instantiation to subclasses. A `LoggerFactory.Create()` can return `FileLogger` or `ConsoleLogger` based on configuration. DI's service registrations with factory delegates are a modern implementation.

---

**Q150.** What is the Command Pattern?
- A) Running shell commands from .NET
- B) Encapsulating a request as an object, enabling parameterization, queuing, logging, and undo operations
- C) A design pattern for HTTP commands
- D) Defining CLI arguments

**✅ Answer: B) Encapsulating a request as an object**

> The Command pattern turns method calls into objects. Each `ICommand` has an `Execute()` and optionally `Undo()`. Used in undo/redo systems, MediatR commands, transaction logs, and task queues.

---

## SECTION 9: REST API & WEB (Q151–Q160)

---

**Q151.** What is the difference between REST and SOAP?
- A) SOAP is newer; REST is legacy
- B) REST is an architectural style using HTTP natively; SOAP is a protocol with rigid XML messaging and WS-* standards
- C) REST uses XML only; SOAP uses JSON
- D) They are identical

**✅ Answer: B) REST is an architectural style using HTTP natively; SOAP is a protocol with rigid XML messaging**

> REST leverages HTTP verbs (GET/POST/PUT/DELETE), status codes, and URLs for resources. SOAP is protocol-based with WSDL contracts, XML envelopes, and built-in security/transactions. REST is simpler and dominant in modern web APIs.

---

**Q152.** What HTTP status code represents a successful resource creation?
- A) 200 OK
- B) 204 No Content
- C) 201 Created
- D) 202 Accepted

**✅ Answer: C) 201 Created**

> 201 Created indicates a resource was successfully created. The response should include a `Location` header pointing to the new resource's URL. Use `CreatedAtAction()` in ASP.NET Core to return 201.

---

**Q153.** What is idempotency in REST?
- A) An API that returns the same response regardless of input
- B) Multiple identical requests produce the same result as a single request — the side effects are the same
- C) A security mechanism preventing duplicate requests
- D) Caching API responses for idempotent results

**✅ Answer: B) Multiple identical requests produce the same result as a single request**

> GET, PUT, DELETE are idempotent — calling them multiple times has the same effect. POST is not idempotent — creating the same order twice creates two orders. PATCH can be idempotent depending on implementation.

---

**Q154.** What is versioning in REST APIs and what are common strategies?
- A) A git versioning system for APIs
- B) Managing backward-incompatible changes — strategies include URL path (`/v1/`), query string (`?version=1`), header versioning
- C) Semantic versioning of NuGet packages
- D) Rotating API keys

**✅ Answer: B) Managing backward-incompatible changes — URL, query string, or header versioning**

> API versioning lets clients use older versions while new clients use newer ones. URL versioning (`/api/v1/products`) is most visible. Header versioning (`Api-Version: 1`) is cleaner. Use `Microsoft.AspNetCore.Mvc.Versioning` NuGet for implementation.

---

**Q155.** What is the difference between PUT and PATCH?
- A) PUT creates; PATCH updates
- B) PUT replaces the entire resource; PATCH applies partial updates to the resource
- C) PATCH replaces; PUT partially updates
- D) They are identical

**✅ Answer: B) PUT replaces the entire resource; PATCH applies partial updates**

> PUT sends the complete representation — fields not included are set to null/default. PATCH sends only the fields that change (using JSON Patch or a partial DTO). Use PATCH for large resources where you only need to update a few fields.

---

**Q156.** What is Swagger/OpenAPI in ASP.NET Core?
- A) A testing framework for REST APIs
- B) A specification and tooling for documenting REST APIs — generates interactive documentation from code
- C) A mock server for API testing
- D) A security scanner for APIs

**✅ Answer: B) A specification and tooling for documenting REST APIs**

> Swagger (OpenAPI) auto-generates interactive API documentation from controller annotations. Swashbuckle or NSwag integrate with ASP.NET Core to produce a UI (`/swagger`) and a machine-readable OpenAPI JSON spec.

---

**Q157.** What is ProblemDetails (RFC 7807)?
- A) A .NET exception type
- B) A standardized JSON format for HTTP error responses
- C) An HTTP status code for validation errors
- D) A logging format for API errors

**✅ Answer: B) A standardized JSON format for HTTP error responses**

> RFC 7807 defines a JSON problem response with `type`, `title`, `status`, `detail`, and `instance` fields. ASP.NET Core returns `ProblemDetails` by default when `[ApiController]` is used, providing consistent error responses.

---

**Q158.** What is `IHttpClientFactory`?
- A) A factory for creating `HttpClient` instances that manages lifetimes and avoids socket exhaustion
- B) A mock factory for testing HTTP calls
- C) A factory for creating `HttpContext` objects
- D) A factory for HTTP middleware

**✅ Answer: A) A factory for creating `HttpClient` instances that manages lifetimes and avoids socket exhaustion**

> Creating `new HttpClient()` per request exhausts sockets (port exhaustion). `IHttpClientFactory` pools `HttpMessageHandler` instances, handles disposal, and supports named/typed clients. Register with `services.AddHttpClient()`.

---

**Q159.** What is a Polly policy in .NET HTTP clients?
- A) A privacy policy generator
- B) A resilience and transient fault-handling library — provides retry, circuit breaker, timeout, and fallback policies
- C) A certificate policy for HTTPS
- D) A test isolation policy

**✅ Answer: B) A resilience library providing retry, circuit breaker, timeout, and fallback policies**

> Polly wraps `IHttpClientFactory` calls with retry logic, circuit breakers (stop calling a failing service), and timeouts. `services.AddHttpClient().AddTransientHttpErrorPolicy(p => p.RetryAsync(3))` is a typical setup.

---

**Q160.** What is OData in ASP.NET Core?
- A) A database schema definition language
- B) An open protocol for RESTful APIs that enables $filter, $select, $expand, $orderby query options
- C) An authentication protocol
- D) A binary data transfer format

**✅ Answer: B) An open protocol enabling $filter, $select, $expand, $orderby query options**

> OData adds a standard query language to REST APIs. Clients can request `GET /api/products?$filter=Price gt 100&$orderby=Name` without custom endpoint logic. Microsoft OData library integrates with EF Core for database-level filtering.

---

## SECTION 10: AUTHENTICATION & AUTHORIZATION (Q161–Q170)

---

**Q161.** What is JWT (JSON Web Token)?
- A) A JSON-based database query format
- B) A compact, self-contained token format for transmitting claims between parties, signed with HMAC or RSA
- C) A JavaScript testing framework
- D) A compression format for JSON

**✅ Answer: B) A compact, self-contained token for transmitting signed claims**

> JWT has three parts: Header (algorithm), Payload (claims), Signature. The server signs tokens with a secret key; clients include them in `Authorization: Bearer <token>`. The server validates the signature without a database lookup.

---

**Q162.** What is the difference between Authentication and Authorization?
- A) They are the same concept
- B) Authentication verifies who you are (identity); Authorization verifies what you're allowed to do (permissions)
- C) Authorization verifies identity; Authentication verifies permissions
- D) Authentication is for APIs; Authorization is for web apps

**✅ Answer: B) Authentication verifies identity; Authorization verifies permissions**

> Authentication: "Are you who you claim to be?" (login, JWT validation). Authorization: "Are you allowed to do this?" (role checks, policy evaluation). In ASP.NET Core: `UseAuthentication()` before `UseAuthorization()`.

---

**Q163.** What is OAuth 2.0?
- A) A password encryption standard
- B) An authorization framework that enables third-party applications to obtain limited access on behalf of a user
- C) An authentication protocol
- D) A JWT signing algorithm

**✅ Answer: B) An authorization framework for delegated access**

> OAuth 2.0 allows users to grant third-party apps access to their resources without sharing credentials. Flows: Authorization Code (web apps), Client Credentials (machine-to-machine), PKCE (SPAs). Note: OAuth is authorization, not authentication — OIDC adds authentication.

---

**Q164.** What is OpenID Connect (OIDC)?
- A) An open-source database connection protocol
- B) An authentication layer built on top of OAuth 2.0 that adds identity — provides an ID token (JWT) with user info
- C) A certificate management protocol
- D) A federated database protocol

**✅ Answer: B) An authentication layer on top of OAuth 2.0**

> OIDC adds an `id_token` (JWT containing user identity claims) to OAuth 2.0's access tokens. Used for SSO — "Login with Google/Microsoft" flows use OIDC. Libraries like IdentityServer, Azure AD, and Auth0 implement OIDC.

---

**Q165.** What is `[Authorize]` attribute in ASP.NET Core?
- A) Marks a controller/action as requiring authenticated users
- B) Grants all users access to an endpoint
- C) Validates the request body automatically
- D) Enables CORS for the endpoint

**✅ Answer: A) Marks a controller/action as requiring authenticated users**

> `[Authorize]` requires an authenticated user. `[Authorize(Roles = "Admin")]` requires a role. `[Authorize(Policy = "MustBeOver18")]` applies a named policy. `[AllowAnonymous]` overrides to allow unauthenticated access.

---

**Q166.** What is Claims-based identity in ASP.NET Core?
- A) Insurance claims processing
- B) Identity represented as a collection of claims (key-value pairs) about the user — name, email, role, etc.
- C) A database table of user permissions
- D) An HTTP header format for identity

**✅ Answer: B) Identity represented as a collection of claims about the user**

> A `ClaimsPrincipal` contains `ClaimsIdentity` objects, each with a collection of `Claim` objects (`ClaimType`, `ClaimValue`). JWT payload claims are mapped to `ClaimsPrincipal` automatically after token validation.

---

**Q167.** What is `IAuthorizationHandler` in ASP.NET Core?
- A) The built-in cookie handler
- B) A class that implements custom authorization requirements evaluation for policies
- C) An HTTP filter for authentication
- D) A JWT validator

**✅ Answer: B) A class implementing custom authorization requirement evaluation**

> Custom policies use `IAuthorizationRequirement` (data) + `IAuthorizationHandler` (logic). The handler's `HandleRequirementAsync` calls `context.Succeed(requirement)` or `Fail()`. Registered in DI as `IAuthorizationHandler`.

---

**Q168.** What is Refresh Token?
- A) A token that refreshes the UI
- B) A long-lived token used to obtain a new access token when the current one expires, without requiring re-login
- C) A token used to refresh cache
- D) An extension of the access token lifetime

**✅ Answer: B) A long-lived token for obtaining new access tokens**

> Access tokens are short-lived (minutes). Refresh tokens are long-lived and stored securely (HTTP-only cookies or secure storage). When the access token expires, the client exchanges the refresh token for a new access token.

---

**Q169.** What is ASP.NET Core Identity?
- A) A JWT library
- B) A membership system providing user registration, login, password hashing, roles, and claims management
- C) An authentication middleware for OAuth
- D) A database-free identity provider

**✅ Answer: B) A membership system providing user registration, login, password hashing, roles, and claims**

> ASP.NET Core Identity handles user management: creating/storing users, hashing passwords (PBKDF2), role management, email confirmation, two-factor authentication. Built on EF Core for persistence.

---

**Q170.** What is CSRF (Cross-Site Request Forgery)?
- A) A SQL injection variant
- B) An attack where a malicious site tricks a user's browser into making authenticated requests to another site
- C) A cross-site scripting attack
- D) A CORS misconfiguration exploit

**✅ Answer: B) An attack tricking browsers into making authenticated requests to another site**

> CSRF exploits the fact that browsers automatically send cookies. Mitigations: Anti-forgery tokens (`[ValidateAntiForgeryToken]`), SameSite cookie attribute, and custom request headers (APIs). SPAs using JWT in Authorization headers are naturally immune.

---

## SECTION 11: TESTING (Q171–Q180)

---

**Q171.** What is the difference between Unit, Integration, and E2E tests?
- A) They differ only in the testing framework used
- B) Unit tests test isolated units; Integration tests test multiple components together; E2E tests test complete user flows
- C) E2E is fastest; Unit is slowest
- D) Integration tests don't require a database

**✅ Answer: B) Unit = isolated; Integration = multiple components; E2E = complete flows**

> Unit tests are fast and isolated (no I/O). Integration tests verify components work together (with real DB, HTTP). E2E tests simulate real user behavior through the full stack. Follow the test pyramid: many units, fewer integration, minimal E2E.

---

**Q172.** What is Mocking in unit testing?
- A) Writing fake business logic
- B) Creating controlled fake implementations of dependencies that simulate real behavior without side effects
- C) Running tests against a mock database
- D) Hiding implementation details from tests

**✅ Answer: B) Creating controlled fake implementations of dependencies**

> Mocking frameworks (Moq, NSubstitute) create fake objects that simulate dependencies. `mock.Setup(m => m.GetById(1)).Returns(product)` controls what the mock returns. Tests then verify the system-under-test behaves correctly.

---

**Q173.** What is the AAA pattern in unit testing?
- A) Authentication-Authorization-Audit
- B) Arrange-Act-Assert — a structure for organizing test code
- C) Async-Await-Assert
- D) Abstract-Abstract-Assert

**✅ Answer: B) Arrange-Act-Assert**

> AAA is the standard test structure: **Arrange** (set up test data/mocks), **Act** (call the method under test), **Assert** (verify the result). Clear separation makes tests readable and maintainable.

---

**Q174.** What is `WebApplicationFactory<T>` in ASP.NET Core testing?
- A) A factory for creating test databases
- B) An in-memory test server that bootstraps the full ASP.NET Core pipeline for integration testing
- C) A mock for `IWebHostEnvironment`
- D) A factory for generating test data

**✅ Answer: B) An in-memory test server for integration testing**

> `WebApplicationFactory<Program>` spins up the full application (middleware, DI, routing) in memory. Tests use `CreateClient()` to send real HTTP requests. You can override services with `WithWebHostBuilder(builder => ...)`.

---

**Q175.** What is `[Theory]` vs `[Fact]` in xUnit?
- A) `[Fact]` runs multiple test cases; `[Theory]` runs once
- B) `[Fact]` runs once with no parameters; `[Theory]` runs multiple times with different data sets via `[InlineData]`
- C) They are identical
- D) `[Theory]` is for async tests

**✅ Answer: B) `[Fact]` runs once; `[Theory]` runs with `[InlineData]` parameter sets**

> `[Fact]` marks a simple test. `[Theory]` with `[InlineData(1, 2, 3)]` parameterizes the test — xUnit runs it once per data set. Also supports `[MemberData]` and `[ClassData]` for complex test data.

---

**Q176.** What is Test Driven Development (TDD)?
- A) Developing tests after the code is written
- B) Writing failing tests first, then writing minimum code to pass, then refactoring (Red-Green-Refactor)
- C) Automated integration testing
- D) Code coverage analysis

**✅ Answer: B) Writing failing tests first, then code to pass, then refactoring**

> TDD cycle: Write a failing test (Red) → Write minimal code to pass (Green) → Refactor without breaking tests (Refactor). Results in high test coverage, better API design, and confidence in changes.

---

**Q177.** What is `FluentAssertions` used for?
- A) Writing fluent builder patterns
- B) Providing expressive, readable assertion syntax for unit tests with better failure messages
- C) A mocking framework
- D) A performance testing tool

**✅ Answer: B) Providing expressive assertion syntax with better failure messages**

> `result.Should().Be(42)` is more readable than `Assert.Equal(42, result)`. FluentAssertions provides rich assertions for collections, exceptions, dates, and objects, with detailed failure descriptions.

---

**Q178.** What is an In-Memory database provider in EF Core used for?
- A) Storing production data when no SQL server is available
- B) Fast, ephemeral database for unit/integration testing without a real database connection
- C) Caching EF queries in memory
- D) Running SQL queries without a database engine

**✅ Answer: B) Fast, ephemeral database for unit/integration testing**

> `UseInMemoryDatabase("TestDb")` provides an in-memory EF Core store for tests. Warning: It doesn't enforce database constraints or mimic SQL behavior. For more realistic integration tests, use Testcontainers with a real Docker database.

---

**Q179.** What is Testcontainers in .NET?
- A) A Docker container management library
- B) A library that spins up real Docker containers (SQL Server, Redis, RabbitMQ) for integration tests
- C) A test isolation tool
- D) A test data generator

**✅ Answer: B) A library spinning up Docker containers for integration tests**

> `Testcontainers.MsSql` starts a real SQL Server Docker container for integration tests, giving you a real database engine. Tests run against the actual database behavior, not an in-memory approximation.

---

**Q180.** What is Code Coverage?
- A) The percentage of code that is commented
- B) The percentage of production code executed by tests
- C) The number of test files per production file
- D) A metric for code quality linting

**✅ Answer: B) The percentage of production code executed by tests**

> Code coverage measures which code lines/branches tests exercise. High coverage (80%+) reduces regression risk but doesn't guarantee quality — tests must assert meaningful things. Tools: Coverlet, Visual Studio Coverage.

---

## SECTION 12: SQL & DATABASES (Q181–Q190)

---

**Q181.** What is the difference between `INNER JOIN` and `LEFT JOIN`?
- A) `INNER JOIN` returns all rows; `LEFT JOIN` returns matching rows
- B) `INNER JOIN` returns only matching rows from both tables; `LEFT JOIN` returns all rows from the left table and matching rows from the right (null if no match)
- C) They are identical
- D) `LEFT JOIN` is faster than `INNER JOIN`

**✅ Answer: B) INNER JOIN returns matching rows only; LEFT JOIN returns all left rows**

> `INNER JOIN` excludes unmatched rows from both sides. `LEFT JOIN` includes all rows from the left table — null columns for unmatched right rows. Use `LEFT JOIN` when right-side data is optional.

---

**Q182.** What is a database index and when should you add one?
- A) A sorted copy of entire table data
- B) A data structure that speeds up data retrieval on specific columns at the cost of write performance and storage
- C) A database backup mechanism
- D) A unique constraint on columns

**✅ Answer: B) A data structure speeding up reads at the cost of write performance**

> Indexes (B-tree by default) allow O(log n) lookups vs O(n) full table scans. Add indexes on: frequently filtered columns (WHERE), JOIN columns, ORDER BY columns. Avoid indexing rarely-queried columns — each index slows INSERTs/UPDATEs.

---

**Q183.** What is a transaction in SQL?
- A) A single SQL statement execution
- B) A unit of work that either all succeeds or all fails, guaranteeing ACID properties
- C) A stored procedure call
- D) A database migration

**✅ Answer: B) A unit of work with ACID guarantees**

> ACID: Atomicity (all or nothing), Consistency (valid state), Isolation (concurrent transactions don't interfere), Durability (committed changes persist). `BEGIN TRANSACTION`, `COMMIT`, `ROLLBACK`.

---

**Q184.** What is the N+1 problem in SQL?
- A) Querying N+1 tables in a single query
- B) Running 1 query to get a list, then N separate queries to get related data for each item
- C) A SQL syntax error
- D) A constraint violation

**✅ Answer: B) 1 query + N queries for related data**

> Example: Fetching 100 orders then querying each order's customer separately = 101 queries. Solution: JOINs or eager loading in ORMs. See EF Core Q85.

---

**Q185.** What is a stored procedure?
- A) A compiled, parameterized SQL script stored in the database and executed on demand
- B) A SQL query stored in application code
- C) A database trigger
- D) A view with parameters

**✅ Answer: A) A compiled SQL script stored in the database**

> Stored procedures are pre-compiled, reducing parse overhead. They support parameters, transactions, and complex logic. Call from EF Core: `dbContext.Database.ExecuteSqlRaw("EXEC GetOrders @customerId", param)`.

---

**Q186.** What is database normalization?
- A) Encrypting database columns
- B) Organizing data to reduce redundancy and improve integrity — typically into Normal Forms (1NF, 2NF, 3NF)
- C) Indexing all columns for performance
- D) Converting all tables to lowercase names

**✅ Answer: B) Organizing data to reduce redundancy and improve integrity**

> 1NF: Atomic columns. 2NF: No partial dependencies. 3NF: No transitive dependencies. Denormalization trades integrity for read performance (common in read-heavy reporting databases or NoSQL).

---

**Q187.** What is a CTE (Common Table Expression)?
- A) A client-side table expression in LINQ
- B) A named temporary result set defined with `WITH` that can be referenced in a SELECT/INSERT/UPDATE
- C) A cross-table expression for joining multiple databases
- D) A type of view

**✅ Answer: B) A named temporary result set defined with `WITH`**

> CTEs improve query readability and enable recursive queries: `WITH cte AS (SELECT ...) SELECT * FROM cte`. Recursive CTEs (using `UNION ALL`) traverse hierarchical data like org charts or category trees.

---

**Q188.** What are Window Functions in SQL?
- A) Functions specific to Windows Server databases
- B) Functions that perform calculations across a set of related rows (window) without collapsing them, using `OVER()`
- C) SQL functions for UI rendering
- D) Functions applied to a single row

**✅ Answer: B) Calculations across related rows using `OVER()`**

> `ROW_NUMBER() OVER(PARTITION BY category ORDER BY price)` assigns row numbers within each category partition. Other window functions: `RANK()`, `DENSE_RANK()`, `LAG()`, `LEAD()`, `SUM() OVER()`. Powerful for analytics without GROUP BY collapsing.

---

**Q189.** What is a deadlock in SQL?
- A) A table that can't be updated
- B) A situation where two transactions wait for each other's locked resources, causing both to be blocked indefinitely
- C) A query that runs forever
- D) A constraint that prevents deletion

**✅ Answer: B) Two transactions waiting for each other's locks indefinitely**

> SQL Server detects deadlocks and kills one transaction (the deadlock victim), which receives error 1205. Prevent with: consistent lock ordering, short transactions, appropriate isolation levels, and indexing.

---

**Q190.** What is the difference between `TRUNCATE` and `DELETE`?
- A) `DELETE` removes all rows; `TRUNCATE` removes specific rows
- B) `TRUNCATE` removes all rows faster (no logging, resets identity) and cannot be rolled back in some DBs; `DELETE` is logged, can be WHERE-filtered, and can be rolled back
- C) They are identical
- D) `TRUNCATE` supports WHERE clause; `DELETE` does not

**✅ Answer: B) `TRUNCATE` is faster, unfiltered, resets identity; `DELETE` is logged, filterable, rollback-safe**

> `TRUNCATE` removes all rows as a minimal-log operation. `DELETE` logs each row deletion, supports `WHERE`, fires triggers, and is fully transactional. `TRUNCATE` resets the identity counter; `DELETE` does not.

---

## SECTION 13: MICROSERVICES, CLOUD & MISCELLANEOUS (Q191–Q200)

---

**Q191.** What is the difference between a Monolith and Microservices architecture?
- A) Monolith uses multiple databases; Microservices uses one
- B) Monolith is a single deployable unit; Microservices decompose functionality into small, independently deployable services
- C) Microservices are always faster
- D) Monolith is for small apps; Microservices is for large apps only

**✅ Answer: B) Monolith = single unit; Microservices = independently deployable services**

> Monoliths are simpler but scale as a whole. Microservices scale individually, deploy independently, and use different tech stacks — but add complexity (distributed tracing, network latency, data consistency). Not always better; start with a monolith unless scale demands otherwise.

---

**Q192.** What is an API Gateway in Microservices?
- A) A database router
- B) A single entry point for all clients that routes requests, handles cross-cutting concerns (auth, rate limiting, logging), and aggregates APIs
- C) A load balancer for databases
- D) A caching layer for APIs

**✅ Answer: B) A single entry point handling routing and cross-cutting concerns**

> API Gateways (AWS API Gateway, Kong, Azure APIM, YARP) route client requests to backend services, handle auth, rate limiting, SSL termination, and can aggregate multiple service responses into one.

---

**Q193.** What is Event-Driven Architecture?
- A) Logging every application event
- B) An architectural pattern where services communicate through events (messages) rather than direct synchronous calls
- C) A design pattern for UI event handlers
- D) An event-sourced database pattern

**✅ Answer: B) Services communicate through events/messages**

> Services publish events (e.g., `OrderPlaced`) to a message broker (RabbitMQ, Azure Service Bus, Kafka). Other services subscribe and react. Decouples services, enables resilience, but adds eventual consistency complexity.

---

**Q194.** What is the Saga pattern?
- A) A long-running SQL stored procedure
- B) A pattern for managing distributed transactions across multiple services using a sequence of local transactions with compensating actions on failure
- C) A story-driven development methodology
- D) A read model pattern in CQRS

**✅ Answer: B) Distributed transaction management with compensating actions**

> Sagas handle long-running business processes across services. Choreography: services react to events. Orchestration: a central coordinator (Saga Orchestrator) directs steps. On failure, compensating transactions undo completed steps.

---

**Q195.** What is Docker and why is it relevant for .NET developers?
- A) A Windows-only deployment tool
- B) A containerization platform that packages applications with their dependencies, ensuring consistent environments across dev/test/prod
- C) A .NET code minification tool
- D) A virtual machine manager

**✅ Answer: B) Containerization for consistent environments**

> Docker containers package the .NET runtime + app + dependencies into a portable image. Same image runs on dev laptop, CI pipeline, and production. .NET has official Docker images. `Dockerfile` defines the build steps.

---

**Q196.** What is Kubernetes (K8s)?
- A) A .NET deployment framework
- B) An open-source container orchestration platform that automates deployment, scaling, and management of containerized applications
- C) A Docker image registry
- D) A CI/CD pipeline tool

**✅ Answer: B) Container orchestration platform**

> Kubernetes manages Docker containers at scale: auto-scaling, load balancing, rolling updates, self-healing (restarts crashed containers), service discovery, and secrets management. Essential for production microservices.

---

**Q197.** What is Application Insights?
- A) An IDE code analysis tool
- B) A Microsoft Azure APM (Application Performance Management) service for monitoring, logging, and diagnostics of .NET applications
- C) A SQL query analyzer
- D) A code coverage tool

**✅ Answer: B) Azure APM service for monitoring and diagnostics**

> Application Insights collects telemetry (requests, dependencies, exceptions, custom events), enables distributed tracing, alerting, and dashboards. Integrate via `services.AddApplicationInsightsTelemetry()`.

---

**Q198.** What is the Outbox Pattern?
- A) A design pattern for email outboxes
- B) A pattern ensuring that database changes and message publishing happen atomically by storing messages in an "outbox" table within the same transaction
- C) An email queue for notification services
- D) A retry pattern for failed HTTP requests

**✅ Answer: B) Atomic database changes + message publishing via an outbox table**

> Problem: saving to DB and publishing to a message bus aren't atomic — you can save but fail to publish. The Outbox pattern saves the message to a table in the same DB transaction. A background process reads and publishes outbox messages, ensuring delivery.

---

**Q199.** What is gRPC and when would you use it over REST?
- A) A Google REST API standard
- B) A high-performance RPC framework using HTTP/2 and Protocol Buffers — better for internal service-to-service communication where speed and contract strictness matter
- C) A GraphQL variant
- D) A REST API versioning tool

**✅ Answer: B) High-performance RPC using HTTP/2 and Protocol Buffers**

> gRPC offers: binary serialization (smaller payloads than JSON), bidirectional streaming, HTTP/2 multiplexing, and strongly typed contracts via `.proto` files. Prefer gRPC for internal microservice communication; REST for public APIs.

---

**Q200.** What is SignalR in ASP.NET Core?
- A) A digital signing library
- B) A library for adding real-time, bidirectional communication between server and clients using WebSockets (with fallbacks)
- C) A signal processing library
- D) A serverless function framework

**✅ Answer: B) Real-time bidirectional communication library**

> SignalR abstracts WebSockets (with Long Polling and Server-Sent Events as fallbacks). The server can push updates to connected clients. Use cases: live dashboards, chat applications, collaborative tools, real-time notifications. Supports Azure SignalR Service for scaling.

---

## Summary of Topics Covered

| Section | Topics | Questions |
|---|---|---|
| 1 | C# Fundamentals & Advanced | Q1–Q35 |
| 2 | .NET Fundamentals & CLR | Q36–Q55 |
| 3 | ASP.NET Core | Q56–Q80 |
| 4 | Entity Framework Core | Q81–Q100 |
| 5 | LINQ | Q101–Q115 |
| 6 | Async/Await & Threading | Q116–Q130 |
| 7 | Dependency Injection | Q131–Q140 |
| 8 | Design Patterns | Q141–Q150 |
| 9 | REST API & Web | Q151–Q160 |
| 10 | Authentication & Authorization | Q161–Q170 |
| 11 | Testing | Q171–Q180 |
| 12 | SQL & Databases | Q181–Q190 |
| 13 | Microservices, Cloud & Misc | Q191–Q200 |

---
*Exam prepared for .NET Developer (3 Years Experience) — June 2026*
