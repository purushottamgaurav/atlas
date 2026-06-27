# SQL Server + NoSQL Interview Q&A — 100 Questions
> 70 SQL Server Theory + 30 NoSQL (Cosmos DB / MongoDB / Snowflake / Redis)  
> Targeted at a 5-Year .NET Full Stack Developer

---

## Part 1: SQL Server — Basics & Fundamentals (Q1–Q15)

---

**Q1. What is the difference between DELETE, TRUNCATE, and DROP?**

- **DELETE** — removes rows one by one. Supports `WHERE`. Fully logged. Can be rolled back.
- **TRUNCATE** — removes all rows at once. No `WHERE`. Minimally logged. Much faster. Can be rolled back inside an explicit transaction.
- **DROP** — removes the entire table structure along with all indexes, constraints, and triggers. Cannot be easily rolled back.

Use DELETE when you need to filter rows or might need to roll back. Use TRUNCATE to wipe a table quickly. Use DROP when you want the table completely gone.

---

**Q2. What is the difference between WHERE and HAVING?**

- **WHERE** — filters rows **before** grouping. Cannot use aggregate functions like `SUM()` or `COUNT()`.
- **HAVING** — filters groups **after** `GROUP BY`. Can use aggregate functions.

```sql
SELECT Dept, COUNT(*) AS Total
FROM Employees
WHERE Age > 25
GROUP BY Dept
HAVING COUNT(*) > 5;
```

---

**Q3. What is the difference between UNION, UNION ALL, INTERSECT, and EXCEPT?**

- **UNION** — combines two result sets and removes duplicates. Slower due to deduplication.
- **UNION ALL** — combines two result sets and keeps all duplicates. Faster.
- **INTERSECT** — returns only rows that appear in **both** result sets.
- **EXCEPT** — returns rows from the first query that do **not** appear in the second.

All require the same number of columns and compatible data types.

---

**Q4. What are SQL Server constraints? List all types.**

Constraints enforce rules on column data:

- **PRIMARY KEY** — uniquely identifies each row. No NULLs. One per table.
- **FOREIGN KEY** — enforces referential integrity between two tables.
- **UNIQUE** — all values in the column must be different. Allows one NULL.
- **CHECK** — values must satisfy a condition (e.g., `Age > 0`).
- **DEFAULT** — provides a default value when none is supplied.
- **NOT NULL** — the column cannot store NULL values.

---

**Q5. What are the different types of JOINs in SQL Server?**

- **INNER JOIN** — returns rows with a match in both tables.
- **LEFT JOIN** — all rows from the left table, matching rows from the right (NULL if no match).
- **RIGHT JOIN** — all rows from the right table, matching rows from the left.
- **FULL OUTER JOIN** — all rows from both tables, NULLs where there is no match.
- **CROSS JOIN** — cartesian product: every row in the left table paired with every row in the right. No join condition.
- **SELF JOIN** — a table joined to itself. Common for employee-manager hierarchies.

---

**Q6. What is the difference between a Primary Key and a Unique Key?**

| Feature | Primary Key | Unique Key |
|---|---|---|
| NULLs allowed | No | Yes (one NULL per column) |
| Per table | Only one | Multiple allowed |
| Index created | Clustered (by default) | Non-clustered |
| Purpose | Uniquely identifies each row | Enforces uniqueness on non-PK columns |

---

**Q7. What is normalization? Explain 1NF, 2NF, and 3NF.**

Normalization organizes tables to reduce data redundancy and improve data integrity.

- **1NF** — each column holds atomic (single, indivisible) values. No repeating groups. Each row must be unique.
- **2NF** — meets 1NF + every non-key column depends on the **whole** primary key, not just part of it. Applies when the primary key is composite.
- **3NF** — meets 2NF + no non-key column depends on another non-key column (no transitive dependency).
- **BCNF** — a stricter version of 3NF where every determinant must be a candidate key.

---

**Q8. What is denormalization and when would you use it?**

Denormalization intentionally introduces redundancy into a normalized schema by merging tables or adding duplicate columns. It trades storage efficiency for query performance.

Use denormalization when:
- Read performance is critical and JOIN-heavy queries are too slow.
- The data is read far more often than it is written.
- You are building a reporting or analytical layer where aggregated data is pre-computed.

Be cautious — denormalization makes updates more complex and risks data inconsistency.

---

**Q9. What is the difference between CHAR, VARCHAR, NCHAR, and NVARCHAR?**

- **CHAR(n)** — fixed-length, non-Unicode. Always uses exactly n bytes. Good for codes like country codes or status flags.
- **VARCHAR(n)** — variable-length, non-Unicode. Uses only as many bytes as needed (+ 2 overhead).
- **NCHAR(n)** — fixed-length, Unicode (2 bytes per character). For multilingual fixed values.
- **NVARCHAR(n)** — variable-length, Unicode. Use this for any text that may contain non-English characters.

Rule of thumb: use `VARCHAR` for English-only data, `NVARCHAR` for any language. Always prefix Unicode string literals with `N`: `N'नमस्ते'`.

---

**Q10. What is NULL in SQL and how is it different from 0 or an empty string?**

`NULL` means the absence of a value — it is unknown. It is not the same as 0 (which is a number) or `''` (which is an empty string).

Key behaviors:
- Any arithmetic with NULL returns NULL: `5 + NULL = NULL`.
- Any comparison with NULL returns UNKNOWN, not TRUE or FALSE: `NULL = NULL` is UNKNOWN.
- Use `IS NULL` or `IS NOT NULL` to check for NULL values.
- Use `COALESCE()` or `ISNULL()` to substitute a default value when NULL is encountered.

---

**Q11. What is the difference between COALESCE() and ISNULL()?**

Both return a fallback value when the first expression is NULL.

- **ISNULL(value, replacement)** — SQL Server-specific. Takes exactly two arguments. Slightly faster.
- **COALESCE(val1, val2, val3, ...)** — ANSI SQL standard. Takes multiple arguments and returns the first non-NULL value. More flexible and portable.

```sql
SELECT COALESCE(MiddleName, NickName, 'N/A') AS DisplayName FROM Employees;
```

---

**Q12. What are aggregate functions in SQL Server?**

Aggregate functions perform a calculation on a set of rows and return a single value. They are used with `GROUP BY`.

Common aggregate functions: `COUNT()`, `SUM()`, `AVG()`, `MIN()`, `MAX()`, `STRING_AGG()`, `COUNT(DISTINCT col)`.

Important: aggregate functions ignore NULL values (except `COUNT(*)`).

---

**Q13. What is the difference between COUNT(*), COUNT(column), and COUNT(DISTINCT column)?**

- **COUNT(*)** — counts all rows including NULLs.
- **COUNT(column)** — counts rows where that column is NOT NULL.
- **COUNT(DISTINCT column)** — counts distinct non-NULL values in that column.

```sql
SELECT COUNT(*), COUNT(ManagerId), COUNT(DISTINCT DepartmentId) FROM Employees;
```

---

**Q14. What are scalar, inline table-valued, and multi-statement table-valued functions?**

- **Scalar function** — takes parameters, returns a single value. Can be used in `SELECT`, `WHERE`. Beware: scalar UDFs can be slow on large data sets (row-by-row execution).
- **Inline Table-Valued Function (iTVF)** — returns a table from a single `SELECT` statement. Treated by the optimizer like a parameterized view. Very efficient.
- **Multi-Statement Table-Valued Function (mTVF)** — builds and returns a table using multiple T-SQL statements. More flexible but slower because the optimizer can't see inside it.

Prefer iTVFs over mTVFs for performance.

---

**Q15. What is the CASE expression and how is it used?**

`CASE` is SQL Server's conditional expression, similar to `if-else`. It returns a value based on conditions.

```sql
-- Simple CASE
SELECT Name,
  CASE Gender
    WHEN 'M' THEN 'Male'
    WHEN 'F' THEN 'Female'
    ELSE 'Other'
  END AS GenderLabel
FROM Employees;

-- Searched CASE
SELECT Name, Salary,
  CASE
    WHEN Salary > 100000 THEN 'High'
    WHEN Salary > 50000 THEN 'Medium'
    ELSE 'Low'
  END AS SalaryBand
FROM Employees;
```

---

## Part 2: SQL Server — Indexes (Q16–Q22)

---

**Q16. What is a database index and why is it needed?**

An index is a separate data structure (B-tree by default) that SQL Server maintains alongside a table to speed up data retrieval. Without an index, SQL Server performs a **full table scan** — reading every row to find matching records.

The trade-off: indexes make reads faster but slow down writes (`INSERT`, `UPDATE`, `DELETE`) because the index must also be updated. Use indexes on columns frequently used in `WHERE`, `JOIN`, and `ORDER BY` clauses.

---

**Q17. What is the difference between a Clustered and a Non-Clustered Index?**

- **Clustered Index** — physically sorts and stores the table rows in index key order. Only **one** per table (the table IS the index). By default, created on the Primary Key. Best for range queries on the key column.
- **Non-Clustered Index** — a separate B-tree structure where the leaf nodes contain the index key + a pointer (RID or clustered key) to the actual row. You can have **many** per table.

Think of a clustered index as a dictionary (words sorted alphabetically = data sorted on disk), and a non-clustered index as the index section at the back of a textbook (pointers to page numbers).

---

**Q18. What is a covering index and what is the purpose of INCLUDE columns?**

A **covering index** is one that contains all the columns a specific query needs, so SQL Server can answer the query entirely from the index without visiting the base table (no key lookup).

The `INCLUDE` clause adds non-key columns to the leaf level of a non-clustered index. This avoids the cost of a Key Lookup without widening the B-tree key itself.

```sql
-- Covers: SELECT Name, Email FROM Employees WHERE DepartmentId = 5
CREATE NONCLUSTERED INDEX IX_Dept_Cover
ON Employees (DepartmentId)
INCLUDE (Name, Email);
```

---

**Q19. What is a composite index and what is the importance of column order?**

A **composite index** (or multi-column index) is an index on two or more columns. SQL Server builds the B-tree key from the columns in the order they are specified.

Column order matters because:
- The index is useful for queries that filter on the **leading column(s)** of the index.
- A filter on the second column alone (without the first) typically cannot use the index efficiently.
- A common rule: put the most selective column first, then order by query usage patterns.

```sql
-- Useful for: WHERE LastName = ? AND FirstName = ?  OR  WHERE LastName = ?
-- NOT useful for: WHERE FirstName = ? alone
CREATE INDEX IX_Name ON Employees (LastName, FirstName);
```

---

**Q20. What is a Filtered Index?**

A **filtered index** is a non-clustered index with a `WHERE` clause, covering only a subset of rows in the table. It is smaller, cheaper to maintain, and more efficient for queries on that specific subset.

```sql
-- Index only active employees — much smaller and faster for active-user queries
CREATE NONCLUSTERED INDEX IX_ActiveEmployees
ON Employees (DepartmentId, Salary)
WHERE IsActive = 1;
```

Use filtered indexes when a column has low cardinality (e.g., a status flag) and most queries target one specific value.

---

**Q21. What is index fragmentation and how do you fix it?**

Over time, as rows are inserted, updated, and deleted, index pages become fragmented — the logical order of leaf pages no longer matches physical disk order, increasing I/O for range scans.

Measure fragmentation using `sys.dm_db_index_physical_stats`.

Fix based on fragmentation level:
- **< 5%** — do nothing.
- **5–30%** — `ALTER INDEX ... REORGANIZE` (online, low overhead, incremental).
- **> 30%** — `ALTER INDEX ... REBUILD` (rebuilds from scratch; use `WITH (ONLINE = ON)` in Enterprise Edition to avoid blocking).

Automate with a SQL Server Agent job scheduled weekly/nightly.

---

**Q22. What is the difference between an Index Seek, Index Scan, and Key Lookup in an execution plan?**

- **Index Seek** — SQL Server navigates the B-tree to find exactly the matching rows. Very efficient. This is what you want.
- **Index Scan** — SQL Server reads all rows in the index from start to end. Acceptable for small tables or when most rows qualify; bad for large tables with selective filters.
- **Table Scan** — reads the entire base table (no index used). Usually indicates a missing index.
- **Key Lookup** — happens when a non-clustered index seek finds matching rows but the query needs columns not in the index. SQL Server fetches those extra columns from the clustered index row by row. Expensive at scale — fix with INCLUDE columns.

---

## Part 3: SQL Server — Stored Procedures, Views, Functions & Triggers (Q23–Q30)

---

**Q23. What is a View and when would you use one?**

A view is a saved SQL `SELECT` query stored as a named object that acts like a virtual table. It does not store data itself — it re-executes the underlying query each time it is referenced.

Use views to:
- Simplify complex queries for reuse across the application.
- Restrict access to specific columns or rows (security layer).
- Present a business-friendly data model without exposing underlying tables.

```sql
CREATE VIEW ActiveEmployees AS
SELECT Id, Name, DepartmentId FROM Employees WHERE IsActive = 1;
```

---

**Q24. What is an indexed view (materialized view) in SQL Server?**

An **indexed view** (also called a materialized view) creates a clustered index on a view, physically storing the result set on disk. Unlike a regular view, it does not re-execute the query on every access.

Benefits: dramatic read performance improvement for aggregations on large tables.  
Restrictions: the view must use `WITH SCHEMABINDING`, cannot reference other views, cannot use `*`, `OUTER JOIN`, subqueries, `DISTINCT`, `TOP`, `UNION`, or non-deterministic functions.

SQL Server automatically uses the indexed view in queries even without explicitly referencing it (with Enterprise Edition or the `NOEXPAND` hint on Standard Edition).

---

**Q25. What is a Stored Procedure and what are its advantages?**

A stored procedure is a named, precompiled set of T-SQL statements stored in the database.

Advantages:
- **Performance** — the execution plan is compiled and cached on first run, reused on subsequent calls.
- **Security** — users can be granted `EXECUTE` permission without direct table access.
- **Maintainability** — business logic lives in one place; no need to redeploy the application to change a query.
- **Reduced network traffic** — only the procedure call is sent over the network, not the full SQL.
- **Prevents SQL injection** — parameters are handled safely by the database engine.

---

**Q26. What is the difference between a Stored Procedure and a User-Defined Function?**

| Feature | Stored Procedure | User-Defined Function |
|---|---|---|
| Returns | Optional (output params or result sets) | Must return a value or table |
| DML (INSERT/UPDATE/DELETE) | Yes | Not in scalar/iTVF; limited in mTVF |
| Use inside SELECT | No | Yes |
| Transactions | Yes | No |
| Exception handling (TRY/CATCH) | Yes | Limited |
| Can call SP from function | No | No |

Use functions for reusable computations inside queries. Use stored procedures for business logic, data modification, and complex workflows.

---

**Q27. What are triggers and what types exist in SQL Server?**

Triggers are special stored procedures that automatically execute in response to specific events on a table or database.

Types:
- **AFTER trigger (FOR trigger)** — fires after an `INSERT`, `UPDATE`, or `DELETE` completes. Used for auditing, logging, enforcing business rules.
- **INSTEAD OF trigger** — fires in place of the original DML operation. Useful for making views updatable or intercepting operations.
- **DDL trigger** — fires on schema changes like `CREATE`, `ALTER`, `DROP`. Used for auditing or preventing schema changes.
- **Logon trigger** — fires when a user session is established.

Triggers have access to two virtual tables: `INSERTED` (new/updated row values) and `DELETED` (old/removed row values).

---

**Q28. What are the disadvantages of triggers?**

- **Hidden logic** — triggers execute invisibly; developers may not realize side effects are occurring.
- **Difficult to debug** — hard to trace in complex systems.
- **Performance overhead** — every qualifying DML operation pays the trigger's execution cost.
- **Recursive/cascading triggers** — if a trigger causes another trigger to fire, it can create unexpected chains.
- **Testing complexity** — unit testing code that relies on trigger behavior is harder.

For auditing and logging, consider explicit application-level logging or change data capture (CDC) over triggers for maintainability.

---

**Q29. What is a CTE (Common Table Expression) and when would you use it?**

A CTE is a named temporary result set defined at the top of a query using the `WITH` keyword. It exists only for the duration of the query.

```sql
WITH SeniorEmployees AS (
    SELECT * FROM Employees WHERE YearsOfService > 10
)
SELECT Department, COUNT(*) FROM SeniorEmployees GROUP BY Department;
```

Use a CTE when:
- The same subquery logic is needed more than once in the query.
- You need a **recursive query** (org charts, bill of materials, hierarchies).
- You want cleaner, more readable code than deeply nested subqueries.

---

**Q30. What is a recursive CTE and when would you use it?**

A recursive CTE references itself to process hierarchical or tree-structured data. It has two parts:

- **Anchor member** — the base case (starting rows).
- **Recursive member** — joins back to the CTE itself to get child rows.

```sql
WITH OrgChart AS (
    SELECT Id, Name, ManagerId, 0 AS Level
    FROM Employees WHERE ManagerId IS NULL   -- anchor: top-level managers

    UNION ALL

    SELECT e.Id, e.Name, e.ManagerId, oc.Level + 1
    FROM Employees e
    JOIN OrgChart oc ON e.ManagerId = oc.Id  -- recursive: find direct reports
)
SELECT * FROM OrgChart ORDER BY Level;
```

Use for: employee hierarchies, category trees, bill of materials, folder structures.

---

## Part 4: SQL Server — Transactions & Concurrency (Q31–Q40)

---

**Q31. What is a transaction and why is it needed?**

A transaction is a logical unit of work containing one or more SQL statements that must all succeed or all fail together. Transactions enforce the ACID properties.

```sql
BEGIN TRANSACTION;
    UPDATE Accounts SET Balance -= 500 WHERE Id = 1;
    UPDATE Accounts SET Balance += 500 WHERE Id = 2;
COMMIT;
-- If anything fails, ROLLBACK to undo both updates
```

Without transactions, a failure between two related operations (like a bank transfer) could leave data in an inconsistent state.

---

**Q32. What are ACID properties?**

- **Atomicity** — all operations in a transaction succeed or none are applied.
- **Consistency** — the database moves from one valid state to another; all constraints remain satisfied.
- **Isolation** — concurrent transactions cannot see each other's intermediate states.
- **Durability** — once committed, the transaction's changes survive crashes and restarts.

---

**Q33. What are the SQL Server transaction isolation levels?**

Isolation levels control how much one transaction is affected by other concurrent transactions:

| Level | Dirty Read | Non-Repeatable Read | Phantom Read |
|---|---|---|---|
| READ UNCOMMITTED | Possible | Possible | Possible |
| READ COMMITTED (default) | Prevented | Possible | Possible |
| REPEATABLE READ | Prevented | Prevented | Possible |
| SERIALIZABLE | Prevented | Prevented | Prevented |
| SNAPSHOT | Prevented | Prevented | Prevented (uses versioning) |

- **Dirty Read** — reading uncommitted changes from another transaction.
- **Non-Repeatable Read** — the same row returns different values when re-read within the same transaction.
- **Phantom Read** — a range query returns different rows when re-run because another transaction inserted/deleted rows.

---

**Q34. What is a deadlock and how does SQL Server handle it?**

A deadlock occurs when two or more transactions each hold a lock the other needs, creating a circular wait that neither can break.

SQL Server's **deadlock monitor** (runs every 5 seconds) detects this cycle and automatically terminates one transaction — the **deadlock victim** (usually the one that costs least to roll back). The victim receives error 1205.

Prevention tips:
- Access tables in a **consistent order** across all transactions.
- Keep transactions **short** — commit as quickly as possible.
- Add proper indexes to reduce lock duration.
- Use `SNAPSHOT` isolation to eliminate shared read locks.

---

**Q35. What is the difference between optimistic and pessimistic concurrency?**

- **Pessimistic concurrency** — assumes conflicts will happen. Acquires locks when data is read, preventing others from modifying it until the transaction finishes. Standard SQL Server locking. Safe but reduces throughput under high concurrency.
- **Optimistic concurrency** — assumes conflicts are rare. Reads data without locking. At update time, it checks whether the data has changed since it was read (using a `rowversion`/`timestamp` column). If it has changed, the update fails and the application retries.

In .NET, Entity Framework Core supports optimistic concurrency via `[Timestamp]` or `[ConcurrencyCheck]` attributes.

---

**Q36. What is NOLOCK and when should you use it carefully?**

`WITH (NOLOCK)` is a table hint that tells SQL Server to read data without acquiring shared locks, equivalent to `READ UNCOMMITTED` isolation for that specific table.

Benefits: avoids being blocked by writers; can improve read performance in busy systems.

Risks:
- **Dirty reads** — you may read data that was never committed (if the writing transaction is rolled back).
- **Phantom rows** — you may see rows that don't exist or miss rows that do.
- **Non-repeatable reads**.

Use `NOLOCK` only for non-critical reads where slightly stale or inconsistent data is acceptable (e.g., dashboards, approximate counts). Never use it for financial data or any calculation that must be accurate.

---

**Q37. What is SNAPSHOT isolation and how does it differ from READ COMMITTED?**

`SNAPSHOT` isolation uses **row versioning** — when a row is modified, the old version is stored in `tempdb`. Readers access the old version rather than waiting for the write lock to be released.

- **READ COMMITTED** (default) — readers wait for a write lock to be released. Each statement sees the latest committed data at the time that statement runs.
- **READ COMMITTED SNAPSHOT ISOLATION (RCSI)** — a database-level setting that makes READ COMMITTED use row versioning automatically. Readers never block writers and writers never block readers. Most commonly enabled setting for high-concurrency OLTP in SQL Server.
- **SNAPSHOT** — the transaction sees a consistent snapshot of data as it was at the start of the transaction (not just each statement).

Enable RCSI: `ALTER DATABASE MyDB SET READ_COMMITTED_SNAPSHOT ON;`

---

**Q38. What is the difference between @@IDENTITY, SCOPE_IDENTITY(), and IDENT_CURRENT()?**

All return an auto-generated identity value, but their scope differs:

- **@@IDENTITY** — last identity generated in the current session, including inside triggers. Dangerous if a trigger inserts into another identity table.
- **SCOPE_IDENTITY()** — last identity generated in the current scope (same stored procedure/batch). **Preferred** — safe and predictable.
- **IDENT_CURRENT('TableName')** — last identity value for a specific table, regardless of session or scope. Useful to check the last inserted ID for a table from outside the inserting session.

---

**Q39. What is the difference between BEGIN TRANSACTION, SAVE TRANSACTION, and ROLLBACK?**

- **BEGIN TRANSACTION** — starts a new transaction. All subsequent statements are part of it.
- **COMMIT** — saves all changes made since `BEGIN TRANSACTION`.
- **ROLLBACK** — undoes all changes made since `BEGIN TRANSACTION` (or the last savepoint).
- **SAVE TRANSACTION savepoint_name** — creates a named checkpoint within a transaction. You can roll back to this savepoint without rolling back the entire transaction.

```sql
BEGIN TRANSACTION;
    INSERT INTO Orders ...;
    SAVE TRANSACTION AfterOrder;
    INSERT INTO OrderItems ...;
    IF @@ERROR <> 0
        ROLLBACK TRANSACTION AfterOrder;  -- only rolls back OrderItems
COMMIT;
```

---

**Q40. What is TRY...CATCH in SQL Server and how does error handling work?**

`TRY...CATCH` is SQL Server's structured error handling mechanism (introduced in SQL Server 2005).

```sql
BEGIN TRY
    BEGIN TRANSACTION;
        UPDATE Accounts SET Balance -= 500 WHERE Id = 1;
        UPDATE Accounts SET Balance += 500 WHERE Id = 2;
    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    SELECT ERROR_NUMBER() AS ErrNum,
           ERROR_MESSAGE() AS ErrMsg,
           ERROR_SEVERITY() AS ErrSeverity;
END CATCH;
```

Key error functions inside `CATCH`: `ERROR_NUMBER()`, `ERROR_MESSAGE()`, `ERROR_LINE()`, `ERROR_SEVERITY()`, `ERROR_STATE()`, `ERROR_PROCEDURE()`.

Note: `TRY...CATCH` does not catch errors with severity 20 or higher (fatal connection errors) or compile errors.

---

## Part 5: SQL Server — Performance & Query Optimization (Q41–Q52)

---

**Q41. What is a query execution plan and why is it important?**

An execution plan is a visual/textual map of the steps SQL Server takes to execute a query — which indexes it uses, which join strategy it picks, estimated vs actual row counts, and where the most cost is concentrated.

How to access:
- **SSMS**: `Ctrl+M` for actual execution plan, `Ctrl+L` for estimated.
- Look for: table scans (missing index), key lookups (add INCLUDE columns), fat arrows (high row count), warnings (implicit type conversions, missing statistics).

Use alongside `SET STATISTICS IO ON` to see logical reads — fewer reads = faster query.

---

**Q42. What are some common query optimization techniques?**

- Add **indexes** on columns used in `WHERE`, `JOIN`, and `ORDER BY`.
- Avoid **SELECT *** — retrieve only the columns you need.
- Avoid **functions on indexed columns** in WHERE (prevents index seek): use `WHERE HireDate >= '2024-01-01'` instead of `WHERE YEAR(HireDate) = 2024`.
- Use **EXISTS instead of IN** for large subqueries — EXISTS stops at the first match.
- Avoid **implicit type conversions** — ensure comparison values match column data types.
- Use **JOINs instead of correlated subqueries** where possible.
- Use **pagination** (`OFFSET/FETCH`) instead of returning huge result sets.
- Use stored procedures for repeated queries (plan caching).
- Review the **execution plan** to identify bottlenecks.

---

**Q43. What are SQL Server wait statistics and why do they matter?**

Wait statistics record what SQL Server threads are waiting for when they cannot immediately proceed. They are the most useful starting point for diagnosing performance problems.

DMV: `sys.dm_os_wait_stats`

Common wait types:

| Wait Type | Indicates |
|---|---|
| CXPACKET | Parallel query imbalance |
| LCK_M_X / LCK_M_S | Lock contention / blocking |
| PAGEIOLATCH_SH | Disk I/O — missing index or low RAM |
| SOS_SCHEDULER_YIELD | CPU pressure |
| ASYNC_NETWORK_IO | Application reading results slowly |

---

**Q44. What is sp_executesql and why is it preferred over EXEC for dynamic SQL?**

Both execute dynamically built SQL strings, but `sp_executesql` is safer and more performant:

- **`EXEC(@sql)`** — concatenates strings. No parameter support. Vulnerable to SQL injection. New execution plan generated every time.
- **`sp_executesql`** — supports parameterized queries. Safe from injection. Plans are cached and reused.

```sql
EXEC sp_executesql
    N'SELECT * FROM Employees WHERE DeptId = @DeptId',
    N'@DeptId INT',
    @DeptId = 5;
```

Always use `sp_executesql` for dynamic SQL in production.

---

**Q45. What is parameter sniffing and how can it cause problems?**

When SQL Server first compiles a stored procedure, it **sniffs** the parameter values being passed and builds an execution plan optimized for those specific values. This plan is then cached and reused for all subsequent calls — even with very different parameter values.

Problem: a plan optimized for `DeptId = 1` (10,000 rows) may be terrible for `DeptId = 99` (2 rows), causing unexpected slow queries.

Solutions:
- `OPTION (RECOMPILE)` on the query — generates a fresh plan every execution (CPU cost).
- `OPTIMIZE FOR (@param UNKNOWN)` — uses average statistics instead of sniffed value.
- `WITH RECOMPILE` on the stored procedure — recompiles every call (use sparingly).
- Local variables — assign parameter to a local variable; SQL Server can't sniff local vars.

---

**Q46. What is the difference between a temp table and a table variable?**

| Feature | Temp Table (#table) | Table Variable (@table) |
|---|---|---|
| Stored in | tempdb | tempdb (mostly) |
| Indexes | Yes (explicit) | Only primary key or unique constraint |
| Statistics | Yes | No (can cause poor plans) |
| Scope | Session | Current batch/procedure only |
| TRUNCATE support | Yes | No |
| Transactions | Participates | Does not participate in rollback |

Use temp tables for larger data sets, complex operations, or when you need indexes. Use table variables for small, short-lived result sets within a single batch.

---

**Q47. What is the Query Store in SQL Server?**

Query Store (introduced in SQL Server 2016) automatically captures query text, execution plans, and runtime performance statistics inside the database itself.

What it solves:
- **Plan regression** — when SQL Server silently switches to a worse execution plan after a statistics update, index rebuild, or upgrade, causing sudden slowdowns.
- Allows you to **force a specific plan** for a query.
- Identifies **top resource-consuming queries** over time.
- Compare plan performance across time windows.

Enable it: `ALTER DATABASE [MyDB] SET QUERY_STORE = ON;`

---

**Q48. What is a Columnstore Index and when would you use it?**

A Columnstore Index stores data column-by-column rather than row-by-row. Each column segment is highly compressed together, and queries use batch-mode processing (processes ~900 rows at a time instead of one).

Benefits:
- 5–10× compression over row-based storage.
- Massive performance gains for analytical queries (`SUM`, `AVG`, `COUNT`) over millions of rows.

When to use:
- Reporting databases, data warehouses, fact tables with analytical workloads.
- **Not** ideal for OLTP tables with frequent single-row writes.

SQL Server 2016+ supports updatable clustered columnstore indexes via a delta store for inserts.

---

**Q49. What is the difference between MERGE and separate INSERT/UPDATE/DELETE?**

`MERGE` performs all three DML operations in a single atomic statement by comparing a source to a target table. Commonly used for ETL loads and table synchronization (UPSERT).

```sql
MERGE INTO Employees AS target
USING (SELECT 1 AS Id, 'Alice' AS Name) AS source ON target.Id = source.Id
WHEN MATCHED THEN UPDATE SET target.Name = source.Name
WHEN NOT MATCHED THEN INSERT (Id, Name) VALUES (source.Id, source.Name);
```

Use MERGE when loading staging data into production or synchronizing tables. For simple upserts, a conditional `IF EXISTS ... UPDATE ELSE INSERT` is also common and sometimes simpler to debug.

---

**Q50. What is the difference between ROW_NUMBER(), RANK(), and DENSE_RANK()?**

All are window functions that assign numbers to rows within a result set.

- **ROW_NUMBER()** — unique sequential number. No ties. 1, 2, 3, 4.
- **RANK()** — same number for tied rows, then skips. 1, 1, 3, 4.
- **DENSE_RANK()** — same number for ties, no gaps. 1, 1, 2, 3.

```sql
SELECT Name, Salary,
    ROW_NUMBER()  OVER (ORDER BY Salary DESC) AS RowNum,
    RANK()        OVER (ORDER BY Salary DESC) AS Rnk,
    DENSE_RANK()  OVER (ORDER BY Salary DESC) AS DenseRnk
FROM Employees;
```

---

**Q51. What are other useful window functions in SQL Server?**

Window functions compute values over a "window" of rows related to the current row without collapsing results like `GROUP BY`.

- **LAG(col, n)** — returns the value of `col` from `n` rows before the current row.
- **LEAD(col, n)** — returns the value from `n` rows after.
- **FIRST_VALUE(col)** — returns the value from the first row in the window.
- **LAST_VALUE(col)** — value from the last row in the window.
- **NTILE(n)** — divides rows into `n` equal buckets and assigns a bucket number.
- **SUM/AVG/COUNT OVER()** — running totals and moving averages without GROUP BY.

---

**Q52. What is the difference between CROSS APPLY and OUTER APPLY?**

`APPLY` allows you to call a table-valued function for each row of a table, or join a subquery that references columns from the outer table.

- **CROSS APPLY** — like an INNER JOIN: only returns rows from the left table where the right expression returns results.
- **OUTER APPLY** — like a LEFT JOIN: returns all rows from the left table; NULLs if the right expression returns no rows.

```sql
-- Get the top 3 orders for each customer
SELECT c.Name, o.OrderDate, o.Amount
FROM Customers c
CROSS APPLY (
    SELECT TOP 3 * FROM Orders WHERE CustomerId = c.Id ORDER BY Amount DESC
) o;
```

---

## Part 6: SQL Server — Security, Backup & Advanced Features (Q53–Q70)

---

**Q53. What are the types of backups in SQL Server?**

- **Full Backup** — backs up the entire database. Foundation for any restore sequence.
- **Differential Backup** — backs up only data changed since the last full backup. Faster than full, reduces restore time.
- **Transaction Log Backup** — backs up the transaction log. Allows **point-in-time recovery**. Required for databases in FULL or BULK-LOGGED recovery model.
- **File/Filegroup Backup** — backs up individual files or filegroups. Used for very large databases.
- **Copy-Only Backup** — a full backup that does not reset the differential base. Used for ad-hoc copies without disrupting the backup chain.

---

**Q54. What are the SQL Server recovery models and how do they differ?**

The recovery model controls how the transaction log is managed and what backup/restore options are available.

- **SIMPLE** — transaction log is automatically truncated after each checkpoint. No log backups. Recovery only to the last full or differential backup. Use for dev/test or non-critical databases.
- **FULL** — all transactions are fully logged. Transaction log backups allow **point-in-time recovery**. Log must be regularly backed up or it grows indefinitely. Required for production databases.
- **BULK-LOGGED** — like FULL but minimally logs bulk operations (`BULK INSERT`, `SELECT INTO`, index rebuilds). Less log growth during bulk loads but loses point-in-time recovery during bulk operations.

---

**Q55. What is SQL Server Always On Availability Groups?**

**Always On Availability Groups** (introduced in SQL Server 2012) is the primary high availability and disaster recovery solution for SQL Server.

It maintains one or more secondary replicas of a primary database on separate SQL Server instances. Supports:
- **Automatic failover** — secondary becomes primary if the primary fails (synchronous mode).
- **Manual failover** — for planned maintenance.
- **Readable secondaries** — offload read queries (reports, backups) to secondary replicas.
- **Multiple secondaries** — up to 8 secondary replicas.

In Azure, this is often implemented as part of **SQL Managed Instance** or configured manually on Azure VMs.

---

**Q56. What is Change Data Capture (CDC) in SQL Server?**

**CDC** is a SQL Server feature that captures row-level changes (INSERT, UPDATE, DELETE) made to tables and records them in change tables in the database. It uses the transaction log to track changes with minimal overhead.

Use CDC when:
- You need to audit all data changes over time.
- You are building an ETL pipeline that needs to process only changed data (incremental loads).
- You want to feed changes to downstream systems (event-driven architectures).

Compared to triggers: CDC is more reliable, has less overhead, and doesn't require trigger logic on every table.

---

**Q57. What is Row-Level Security (RLS) in SQL Server?**

**Row-Level Security** (introduced in SQL Server 2016) allows you to control which rows in a table a user can see or modify, based on a predicate (filter) function — without changing application code.

```sql
-- Only return rows where SalesRegion matches the logged-in user's region
CREATE FUNCTION dbo.fn_SecurityFilter(@Region NVARCHAR(50))
RETURNS TABLE WITH SCHEMABINDING AS
RETURN SELECT 1 AS Result
WHERE @Region = USER_NAME();

CREATE SECURITY POLICY RegionPolicy
ADD FILTER PREDICATE dbo.fn_SecurityFilter(SalesRegion) ON dbo.Sales;
```

Use RLS for multi-tenant applications where each tenant should only see their own data, without building filter logic into every query.

---

**Q58. What is Dynamic Data Masking in SQL Server?**

**Dynamic Data Masking** (DDM) limits sensitive data exposure to non-privileged users by masking it at query time — without actually changing the stored data.

Mask types:
- `default()` — full mask (shows XXXX for strings, 0 for numbers).
- `partial(prefix, padding, suffix)` — shows part of the value: e.g., `aXXX@XXXX.com`.
- `email()` — masks email format.
- `random(from, to)` — returns a random number.

Users with `UNMASK` permission see real data. Useful for giving developers/support access to production-like data without exposing sensitive values.

---

**Q59. What is the difference between authentication and authorization in SQL Server?**

- **Authentication** — verifying who you are. SQL Server supports:
  - **Windows Authentication** — uses Active Directory credentials. Preferred (more secure, no passwords in connection strings).
  - **SQL Server Authentication** — username/password stored in SQL Server. Used for cross-domain or cloud scenarios.
  - **Azure AD Authentication** — for Azure SQL / Managed Instance.

- **Authorization** — controlling what you can do after you authenticate. Managed via:
  - **Roles** — `db_datareader`, `db_datawriter`, `db_owner`, custom roles.
  - **GRANT / DENY / REVOKE** — permission on specific objects (tables, procedures, schemas).
  - **Schema-level permissions** — grant access to all objects in a schema at once.

---

**Q60. What is the principle of least privilege in the context of SQL Server?**

The principle of least privilege means granting users and applications only the minimum permissions they need to perform their function — nothing more.

In practice for .NET applications:
- The application's database user should have `EXECUTE` on stored procedures only — not direct `SELECT`/`INSERT`/`UPDATE`/`DELETE` on tables.
- Developers should not have `db_owner` on production databases.
- Use a separate read-only user for reporting queries.
- Avoid using `sa` or `sysadmin` accounts in application connection strings.

---

**Q61. What is In-Memory OLTP (Hekaton) in SQL Server?**

Introduced in SQL Server 2014, **In-Memory OLTP** allows you to create memory-optimized tables that reside entirely in RAM with a lock-free, latch-free concurrency model.

Benefits:
- Dramatically faster INSERT/UPDATE/DELETE for high-throughput workloads.
- No row locking or page latching — uses optimistic multi-version concurrency.
- **Natively compiled stored procedures** execute directly as machine code.

When to use: high-frequency inserts (telemetry, queuing, session state), when locking is a confirmed bottleneck. Not suitable for large analytical queries or tables needing complex indexes.

---

**Q62. What is a schema in SQL Server and why is it useful?**

A **schema** is a namespace that groups database objects (tables, views, procedures) under a named container. The default schema is `dbo`.

Benefits:
- **Organization** — group related objects: `Sales.Orders`, `HR.Employees`, `Finance.Accounts`.
- **Security** — grant/deny permissions at the schema level rather than on each individual object.
- **Name collision avoidance** — two tables can share the same name in different schemas.

In large databases or multi-tenant applications, schemas are an important tool for logical separation and access control.

---

**Q63. What is a sequence object in SQL Server and how does it differ from IDENTITY?**

A **SEQUENCE** is a database object that generates a sequential numeric series, independent of any table (introduced in SQL Server 2012).

| Feature | IDENTITY | SEQUENCE |
|---|---|---|
| Scope | Tied to one column in one table | Independent object, reusable |
| Generate values before INSERT | No | Yes (`NEXT VALUE FOR`) |
| Cycle / min / max / increment | Limited | Fully configurable |
| Share across tables | No | Yes |

Use SEQUENCE when you need to generate a number before inserting (e.g., to use in multiple tables), or when you need to share a number series across tables.

---

**Q64. What are computed columns and persisted computed columns?**

A **computed column** is a virtual column whose value is derived from an expression involving other columns in the same row. It is calculated on the fly and does not store data.

A **persisted computed column** physically stores the result on disk. Required if you want to index the computed column. The expression must be deterministic.

```sql
ALTER TABLE Employees
ADD FullName AS (FirstName + ' ' + LastName) PERSISTED;

CREATE INDEX IX_FullName ON Employees (FullName);
```

---

**Q65. What is the difference between a heap and a clustered table in SQL Server?**

- **Heap** — a table with no clustered index. Rows are stored in no particular order. Data pages are linked in a list. Lookups require a full scan (or use a non-clustered index with a RID lookup). Inserts are fast (no ordering needed) but range queries are slow.
- **Clustered table** — a table with a clustered index. Rows are physically sorted by the clustered key. Range queries on the clustered key are very efficient. A key lookup from a non-clustered index follows the clustered key (more efficient than a RID lookup in a heap).

Most production tables should have a clustered index. Heaps are occasionally useful for staging/load tables.

---

**Q66. What is linked server in SQL Server?**

A **linked server** is a configured connection from one SQL Server instance to another data source (SQL Server, Oracle, Excel, etc.), allowing you to query remote data using four-part naming: `[ServerName].[Database].[Schema].[Table]`.

```sql
SELECT * FROM [RemoteServer].[SalesDB].[dbo].[Orders];
```

Use linked servers for cross-server reporting or data migration. Avoid for performance-critical production queries — network latency and remote data transfer make them slow. Prefer ETL processes or replication for regular cross-server data movement.

---

**Q67. What is SQL Server Agent and what is it used for?**

**SQL Server Agent** is a Windows service that runs scheduled jobs, alerts, and operators.

Common uses:
- Scheduled backups (full, differential, log).
- Index maintenance (reorganize/rebuild).
- Statistics updates.
- ETL jobs (running SSIS packages).
- Database integrity checks (`DBCC CHECKDB`).
- Sending alert emails when jobs fail.

Jobs consist of one or more **steps** (T-SQL, SSIS, PowerShell, etc.) and **schedules** (time-based or event-triggered).

---

**Q68. What is DBCC CHECKDB and why should it be run regularly?**

`DBCC CHECKDB` is a Database Console Command that performs a consistency check on an entire database — it validates the structural integrity of all objects, checks for corruption in pages, indexes, and constraints.

Why run it regularly:
- Detects hardware-level data corruption (disk errors, RAID issues) before it causes data loss.
- Recommended to run at least weekly on production databases.
- Run after restoring a backup to verify the backup is clean.

```sql
DBCC CHECKDB ('YourDatabase') WITH NO_INFOMSGS, ALL_ERRORMSGS;
```

---

**Q69. What is the difference between SQL Server Replication and Always On Availability Groups?**

| Feature | Replication | Always On AG |
|---|---|---|
| Purpose | Data distribution / publishing | HA and DR |
| Granularity | Table or row/column-level filtering | Entire database(s) |
| Topology | One publisher → many subscribers | One primary → 1–8 secondaries |
| Latency | Asynchronous (transactional replication) | Sync or async |
| Readable secondary | Yes (subscribers) | Yes (readable secondary replicas) |
| Failover | Manual only | Automatic (synchronous mode) |

Use Replication when you need to selectively distribute subsets of data to multiple consumers. Use Always On AG for full database HA/DR with automatic failover.

---

**Q70. What is Extended Events (XEvents) in SQL Server and why is it preferred over SQL Profiler?**

**Extended Events** is SQL Server's lightweight, modern event tracing framework, the recommended replacement for the deprecated SQL Profiler.

Why XEvents over Profiler:
- **Much lower overhead** — Profiler can consume 15–25% CPU on busy servers; XEvents is near-zero impact.
- **More events** — over 1,000 trackable events vs ~180 in Profiler.
- **Flexible output targets** — ring buffer (memory), file, histogram, event counter.
- **Always-on** — the built-in `system_health` session captures deadlocks, non-yielding schedulers, and memory warnings by default.

Common uses: capturing slow queries, deadlocks, blocking, login failures, missing indexes.

---

## Part 7: Snowflake — 10 Questions (Q71–Q80)

---

**Q71. What is Snowflake and how is its architecture different from SQL Server?**

Snowflake is a cloud-native data warehouse with a **multi-cluster shared data architecture** that separates storage and compute — unlike SQL Server where they are tightly coupled on the same server.

Three layers:
- **Storage** — data stored in compressed columnar format in cloud object storage (S3, Azure Blob, GCS). You pay only for storage used.
- **Compute (Virtual Warehouses)** — independent MPP clusters that execute queries against the storage. Multiple warehouses can query the same data simultaneously without contention.
- **Cloud Services** — handles metadata, query optimization, authentication, and transactions.

This separation means you can scale compute up/down or pause it when idle, without affecting stored data.

---

**Q72. What is a Virtual Warehouse in Snowflake?**

A Virtual Warehouse is an independently scalable MPP compute cluster that executes SQL queries. Sizes range from XS (1 node) to 6XL (512 nodes) — each size doubles the compute.

Key features:
- **Auto-suspend** — pauses after a configured idle period, stopping compute billing.
- **Auto-resume** — starts automatically when a query arrives.
- **Multi-cluster warehouses** — spin up additional clusters during peak load to prevent queue wait.

Multiple warehouses can simultaneously read the same data without locking each other — a major advantage over traditional databases.

---

**Q73. What is Time Travel in Snowflake?**

Time Travel allows you to query data as it existed at a past point in time, restore dropped tables/schemas/databases, or clone objects from a historical state.

```sql
-- Query data from 1 hour ago
SELECT * FROM Orders AT (OFFSET => -3600);

-- Restore a dropped table
UNDROP TABLE Orders;
```

Retention period: 0–90 days (Standard edition supports 1 day; Enterprise supports up to 90). After Time Travel expires, Snowflake provides an additional 7-day **Fail-Safe** window accessible only by Snowflake support for disaster recovery.

---

**Q74. What is Zero-Copy Cloning in Snowflake?**

Zero-copy cloning creates an instant copy of a table, schema, or database **without duplicating the underlying data**. The clone initially shares the same micro-partitions as the source. Only when data in the clone or source changes are new partitions written.

```sql
CREATE DATABASE dev_db CLONE prod_db;
CREATE TABLE orders_backup CLONE orders;
```

Benefits: instant creation regardless of size, no storage cost until data diverges. Commonly used to create dev/test environments from production snapshots.

---

**Q75. What is the difference between COPY INTO and INSERT for loading data in Snowflake?**

- **`COPY INTO`** — the preferred bulk load mechanism. Reads files in parallel directly from cloud stages (S3, Azure Blob, GCS) into a Snowflake table across all virtual warehouse nodes. Supports CSV, JSON, Parquet, Avro, ORC. Tracks load history to prevent duplicate loads. Much faster than row-by-row inserts for large volumes.
- **`INSERT INTO`** — standard SQL insert. Suitable for small volumes or transformations within Snowflake (e.g., `INSERT INTO ... SELECT FROM staging`).

For real-time ingestion, **Snowpipe** continuously loads files from a stage using cloud event notifications as they arrive.

---

**Q76. What are micro-partitions in Snowflake?**

Micro-partitions are the fundamental storage unit in Snowflake. Data is automatically divided into immutable columnar partitions of 50–500 MB (compressed), each storing metadata (min/max values, row count, null count per column).

At query time, Snowflake uses this metadata to **prune** (skip) entire partitions that cannot contain matching rows — dramatically reducing I/O without requiring explicit indexes. This is Snowflake's equivalent of index-based access.

---

**Q77. What are Snowflake stages?**

A **stage** in Snowflake is a named location where data files are stored for loading or unloading.

- **Internal stage** — Snowflake-managed storage (user stage, table stage, or named internal stage).
- **External stage** — points to cloud storage you own (S3, Azure Blob, GCS). You manage the files; Snowflake reads from them.

Stages are used with `COPY INTO` for loading data and `COPY INTO <location>` for unloading query results back to files.

---

**Q78. What is the difference between Snowflake's warehouse sizes and when would you scale up vs. scale out?**

- **Scale up** — use a larger warehouse size (e.g., M → L → XL) when a single complex query needs more compute power or memory. Larger warehouses have more nodes working on the same query, reducing single-query execution time.
- **Scale out** — use a **multi-cluster warehouse** to add more warehouses of the same size when there are many concurrent users/queries queuing. Each cluster handles a subset of the concurrent queries.

Rule: scale up for slow individual queries; scale out for concurrency problems.

---

**Q79. What is Snowflake's data sharing feature?**

**Secure Data Sharing** allows you to share live, read-only access to data in your Snowflake account with other Snowflake accounts — without copying or moving the data. The consumer queries the provider's storage directly.

Benefits:
- Real-time data (no ETL lag).
- No data duplication or transfer cost.
- Consumer can be a different company or another internal account.

This is how Snowflake's **Data Marketplace** works — providers publish live data sets that consumers can query immediately after gaining access.

---

**Q80. What is the difference between a Snowflake Table, Transient Table, and Temporary Table?**

| Type | Time Travel | Fail-Safe | Persistence |
|---|---|---|---|
| **Permanent Table** | Up to 90 days | 7 days | Persists until dropped |
| **Transient Table** | Up to 1 day | None | Persists until dropped |
| **Temporary Table** | Up to 1 day | None | Session-scoped, auto-dropped |

Use **transient tables** for staging/intermediate data where you don't need Time Travel or Fail-Safe (saves storage cost). Use **temporary tables** for session-scoped intermediate results within a pipeline or session.

---

## Part 8: MongoDB & Cosmos DB — 10 Questions (Q81–Q90)

---

**Q81. What is a document database and how does it differ from a relational database?**

A document database stores data as self-describing JSON-like documents rather than rows in fixed-schema tables.

| Feature | Relational (SQL Server) | Document (MongoDB) |
|---|---|---|
| Data model | Tables, fixed schema | JSON documents, flexible schema |
| Relationships | Foreign keys, JOINs | Embedded documents or references |
| Schema changes | ALTER TABLE (can be disruptive) | Add new fields to documents freely |
| Scaling | Primarily vertical | Horizontal sharding built-in |

Best for: hierarchical data, rapidly changing schemas, user profiles, product catalogs.

---

**Q82. What is the difference between embedding and referencing in MongoDB?**

When modeling related data in MongoDB:

- **Embedding** — store related data as nested documents inside the parent. Best when data is always accessed together, has a 1-to-few relationship, and doesn't grow unboundedly. (e.g., order line items embedded inside an order.)
- **Referencing** — store the `_id` of a related document. Best when data is large, changes independently, or is shared across many documents. Requires a separate query or `$lookup` to fetch. Similar to a foreign key.

General rule: **embed** data you always read together; **reference** data that changes independently or could grow without bound.

---

**Q83. What is the MongoDB Aggregation Pipeline?**

The aggregation pipeline processes documents through a sequence of stages, each transforming the data.

Common stages:
- `$match` — filter documents (like WHERE).
- `$group` — group and aggregate (like GROUP BY + SUM/COUNT).
- `$project` — reshape documents, include/exclude fields.
- `$lookup` — left outer join to another collection.
- `$sort`, `$limit`, `$skip` — ordering and pagination.
- `$unwind` — deconstruct an array into separate documents.

```js
db.orders.aggregate([
  { $match: { status: "completed" } },
  { $group: { _id: "$customerId", total: { $sum: "$amount" } } },
  { $sort: { total: -1 } }
]);
```

---

**Q84. What is Azure Cosmos DB and how does it relate to MongoDB?**

**Cosmos DB** is Microsoft's globally distributed, multi-model NoSQL database service. It is not MongoDB but offers a **MongoDB-compatible API**, so existing MongoDB drivers and code work with minimal changes.

Key Cosmos DB advantages:
- **Turnkey global distribution** — replicate data across any Azure regions with a few clicks.
- **Multiple APIs** — MongoDB, SQL (Core), Cassandra, Gremlin (graph), Table.
- **Five consistency levels** — choose the right balance of consistency vs. performance.
- **SLA-backed** — 99.999% availability with multi-region writes.

Pricing is based on **Request Units (RUs)** — a combined measure of CPU, memory, and I/O consumed by each operation.

---

**Q85. What are the five Cosmos DB consistency levels?**

Cosmos DB lets you choose a consistency level per request:

1. **Strong** — reads always return the latest committed write. Highest consistency, highest latency.
2. **Bounded Staleness** — reads lag behind writes by at most K versions or T seconds. Good for globally distributed near-real-time scenarios.
3. **Session** (default) — within a single client session, you always read your own writes. Most practical for web applications.
4. **Consistent Prefix** — you never see out-of-order writes. Reads may be stale but always in order.
5. **Eventual** — no ordering guarantees. Maximum throughput, lowest latency. For non-critical data (likes, view counts).

---

**Q86. What is a partition key in Cosmos DB and why does it matter?**

Cosmos DB distributes data across physical partitions based on the **partition key** you choose. All documents with the same partition key value land in the same logical partition.

Why it matters:
- **Queries with a partition key filter** are routed to a single partition — fast and cheap.
- **Cross-partition queries** fan out to all partitions — expensive in RUs.
- A **hot partition** (one key getting most traffic) is a throughput bottleneck.

A good partition key has **high cardinality** and results in even distribution of reads and writes. Common choices: `userId`, `orderId`, `tenantId`.

---

**Q87. What is the Request Unit (RU) model in Cosmos DB?**

A **Request Unit (RU)** is Cosmos DB's currency for measuring the cost of database operations. It abstracts away CPU, memory, and I/O into a single unit.

- Reading a 1 KB document costs approximately 1 RU.
- Writes cost more than reads (~5 RUs for a 1 KB document).
- Complex queries and cross-partition queries cost more RUs.

You provision RUs per second (RU/s) for a container. If your workload exceeds provisioned RUs, requests are throttled (429 errors). Cosmos DB also offers **serverless** mode (pay per actual RU consumed) for intermittent workloads.

---

**Q88. How does indexing work in Cosmos DB?**

By default, Cosmos DB **automatically indexes every property** of every document. Unlike relational databases, you don't create indexes manually for common queries.

The default indexing policy includes all paths (`/*`) with range indexes. You can customize it to:
- **Exclude paths** you never query to save RU cost on writes.
- **Include composite indexes** for queries with ORDER BY on multiple fields.
- **Add spatial indexes** for geospatial queries.

Over-indexing increases write cost (every write must update all indexes), so tuning the index policy is important for write-heavy workloads.

---

**Q89. What is the difference between SQL API and MongoDB API in Cosmos DB?**

| Feature | SQL (Core) API | MongoDB API |
|---|---|---|
| Query language | SQL-like (SELECT, FROM, WHERE) | MongoDB query language (BSON, aggregation pipeline) |
| Best for | New Cosmos DB projects | Migrating existing MongoDB applications |
| Driver | Azure Cosmos DB SDK | Standard MongoDB driver (with Cosmos DB endpoint) |
| Feature parity | Full Cosmos DB features | Most MongoDB features; some advanced features differ |

If you are building a new project on Cosmos DB, the SQL API gives you the most native Cosmos DB experience. If you are migrating a MongoDB application, the MongoDB API minimizes code changes.

---

**Q90. How do you handle transactions in MongoDB and Cosmos DB?**

**MongoDB:**
- Single-document operations are always atomic (ACID at the document level).
- Multi-document ACID transactions are supported since MongoDB 4.0 on replica sets, using `session.startTransaction()`. Keep transactions short (60-second default timeout).

**Cosmos DB:**
- Supports stored procedures and triggers (JavaScript) that execute atomically within a single logical partition.
- Multi-item transactions are supported within a single partition.
- Cross-partition transactions are not natively supported — use the **Saga pattern** or optimistic concurrency with ETags.

---

## Part 9: Redis — 10 Questions (Q91–Q100)

---

**Q91. What is Redis and what are its primary use cases in a .NET application?**

Redis (Remote Dictionary Server) is an open-source, in-memory data structure store used primarily as a cache, message broker, and session store.

Common use cases in .NET applications:
- **Distributed cache** — cache expensive database results across multiple app servers using `IDistributedCache` in ASP.NET Core.
- **Session storage** — store user session state shared across stateless app instances.
- **Rate limiting** — atomic INCR operations to count and throttle API requests.
- **Pub/Sub messaging** — lightweight event broadcasting between services.
- **Distributed locks** — coordinate exclusive access across multiple service instances.
- **Leaderboards** — sorted sets maintain ranked scores efficiently.

Primary .NET library: `StackExchange.Redis`.

---

**Q92. What are the main Redis data types?**

Redis supports several data structures beyond simple key-value pairs:

| Type | Description | Use Case |
|---|---|---|
| **String** | Binary-safe value up to 512 MB | Cache objects, counters (INCR), flags |
| **List** | Doubly linked list | Message queues, activity feeds |
| **Hash** | Field-value pairs within a key | User profile, settings per entity |
| **Set** | Unordered unique strings | Tags, unique visitors |
| **Sorted Set (ZSet)** | Unique strings scored by a float | Leaderboards, priority queues |
| **Stream** | Append-only log of messages | Event sourcing, reliable message queues |

---

**Q93. What is the difference between cache-aside and write-through caching?**

- **Cache-aside (lazy loading)** — on a cache miss, the application reads from the database, stores the result in Redis, and returns it. Future requests are served from cache. Risk: stale data if the database is updated without invalidating the cache key.
- **Write-through** — every write to the database also updates the cache simultaneously. Cache is always consistent. Downside: cache may store data that is never read (wasted memory) and write latency is slightly higher.

Cache-aside is the most common pattern in .NET applications with Redis. Always set an expiry (`AbsoluteExpiration` or `SlidingExpiration`) to control staleness.

---

**Q94. What is Redis persistence and what are RDB and AOF?**

By default, Redis is in-memory only — a restart loses all data. For durability:

- **RDB (Redis Database Snapshot)** — takes point-in-time snapshots at configured intervals (e.g., every 5 minutes if 100+ keys changed). Fast restarts, compact files. Data written since the last snapshot is lost on crash.
- **AOF (Append Only File)** — logs every write operation to disk. Much less data loss risk (can fsync every second or every command). Larger files, slower restart.

For a Redis cache, persistence is often disabled — losing the cache on restart is acceptable. For Redis used as a session store or primary data store, enable AOF.

---

**Q95. What is Redis TTL (Time To Live) and why is it important?**

TTL sets an expiry on a Redis key, after which it is automatically deleted. It is fundamental to preventing unbounded memory growth in a cache.

```csharp
// Set a key with 30-minute expiry using StackExchange.Redis
db.StringSet("user:123:profile", json, TimeSpan.FromMinutes(30));
```

Strategies:
- **Absolute expiry** — expires at a fixed time regardless of access.
- **Sliding expiry** — resets the timer each time the key is accessed. Good for session data.

Always set a TTL on cache keys. Keys without TTL accumulate indefinitely and can exhaust Redis memory.

---

**Q96. What is Redis pub/sub and how is it different from a message queue?**

- **Pub/Sub** — a publisher sends a message to a channel; all current subscribers receive it. Messages are not stored — if a subscriber is offline, it misses the message. Fire-and-forget. Best for real-time notifications, live dashboards, chat.

- **Message Queue (Redis Lists or Streams)** — messages are stored in Redis and consumers pull them. Messages persist until consumed. Supports reliable delivery (with Streams and consumer groups). Best for task queues and reliable event processing.

In .NET, use Redis pub/sub for lightweight broadcasting (e.g., cache invalidation across app servers). Use Redis Streams or Azure Service Bus for reliable message processing.

---

**Q97. What is a distributed lock in Redis and when do you need one?**

A distributed lock prevents multiple service instances from executing the same critical section simultaneously (e.g., scheduling a job, sending a notification, processing a payment).

A simple Redis lock uses `SET key value NX PX milliseconds`:
- `NX` — only set if the key does not exist.
- `PX` — set expiry in milliseconds (auto-releases if the holder crashes).

The value should be a unique token per caller. To release, use a Lua script to check the token before deleting (atomic compare-and-delete).

For multi-node Redis setups, the **RedLock algorithm** acquires the lock on a majority of independent Redis nodes for safety. In .NET, the `RedLock.net` library implements this.

---

**Q98. What is Redis eviction and what are the common eviction policies?**

When Redis reaches its configured `maxmemory` limit, it must evict keys to free space. The eviction policy controls which keys are removed:

- **noeviction** — returns an error on write when memory is full. Use when data loss is unacceptable.
- **allkeys-lru** — evicts the least recently used key across all keys. Most common for caches.
- **volatile-lru** — evicts the least recently used key among keys with a TTL set.
- **allkeys-random** — evicts a random key. Simple but not smart.
- **volatile-ttl** — evicts the key with the shortest remaining TTL first.

For a cache use case, `allkeys-lru` is the most sensible default.

---

**Q99. How do you use Redis as a distributed cache in ASP.NET Core?**

ASP.NET Core has built-in support for Redis via the `IDistributedCache` interface.

1. Install: `Microsoft.Extensions.Caching.StackExchangeRedis`
2. Register in `Program.cs`:
```csharp
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "localhost:6379";
    options.InstanceName = "MyApp:";
});
```
3. Inject and use `IDistributedCache` in your services:
```csharp
var cached = await _cache.GetStringAsync("key");
if (cached == null) {
    var data = await _dbService.GetDataAsync();
    await _cache.SetStringAsync("key", JsonSerializer.Serialize(data),
        new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });
}
```

---

**Q100. What is the difference between Redis Cluster and Redis Sentinel?**

Both address Redis availability but solve different problems:

- **Redis Sentinel** — monitors a primary Redis node and automatically promotes a replica to primary if the primary fails. Provides high availability for a single logical Redis instance. No data sharding — all data lives on one primary. Simple to set up.

- **Redis Cluster** — horizontally shards data across multiple primary nodes using hash slots (16,384 total slots distributed across nodes). Each primary can have replicas. Provides both **high availability** and **horizontal scalability**. More complex setup. Use when a single node cannot hold all data in memory.

For most ASP.NET Core applications (session cache, output cache), Sentinel is sufficient. Use Cluster only when data volume or throughput exceeds what a single Redis node can handle.

---

*End of document — 70 SQL Server Theory Questions + 10 Snowflake + 10 MongoDB/Cosmos DB + 10 Redis*