# SQL Interview Q&A

---

1. **Difference between TRUNCATE, DROP, and DELETE?**
TRUNCATE — removes all rows quickly, table structure remains, cannot use WHERE, minimal logging, cannot rollback in most DBs (in SQL Server can rollback if in explicit transaction).
DROP — removes the entire table along with all associated objects (indexes, constraints, triggers). Cannot rollback easily.
DELETE — removes rows with optional WHERE clause, fully transaction-logged, slow on large data, can rollback.

2. **Difference between Temporary Tables and Table Variables?**
Temporary Table (#temp) — stored in tempdb, supports indexes, supports TRUNCATE, visible to nested stored procedures, persists until session ends or explicitly dropped.
Table Variable (@var) — stored in tempdb, no indexes (except primary key), no TRUNCATE, scope is the current batch only, automatically deleted after batch. Table variables cause fewer recompilations.

3. **Local vs Global Temporary Tables?**
Local Temp Table (#table) — visible only to the current session/connection. Dropped when the session ends.
Global Temp Table (##table) — visible to all sessions/connections. Dropped when the last session using it disconnects.
Table Variable (@table) — no global equivalent. Scope is the current batch only.
All three are physically created in tempdb.

4. **CTE vs Temp Table?**
CTE (Common Table Expression) — defined with WITH keyword, scope is a single query only. Cannot be reused. Good for readability and recursive queries.
Temp Table — persists for the session or batch, can be used multiple times, can be indexed. Better for large datasets or reuse across multiple queries.
```sql
-- CTE
WITH CTE AS (SELECT * FROM Employees WHERE Dept = 'IT')
SELECT * FROM CTE;

-- Temp Table
SELECT * INTO #TempEmp FROM Employees WHERE Dept = 'IT';
SELECT * FROM #TempEmp;
```

5. **Difference between UNION, UNION ALL, and INTERSECT?**
UNION — combines results of two queries, removes duplicates. Slower due to deduplication.
UNION ALL — combines results including duplicates. Faster than UNION.
INTERSECT — returns only rows that appear in both query results (common rows).
EXCEPT — returns rows from the first query that do not appear in the second.

6. **Difference between Stored Procedure and Function?**
Stored Procedure — can have input/output params, can perform DML (INSERT/UPDATE/DELETE), cannot be used in SELECT statement, can use transactions.
Function — must return a value, can be used in SELECT/WHERE/JOIN, cannot perform DML (scalar functions), no output params. Two types: Scalar (returns single value) and Table-valued (returns a table).

7. **Difference between Primary Key, Unique Key, Foreign Key, Composite Key?**
Primary Key — uniquely identifies each row, no NULLs, one per table, creates clustered index by default.
Unique Key — enforces uniqueness, allows one NULL, can have multiple per table.
Foreign Key — enforces referential integrity between two tables. Values must exist in the referenced primary key column.
Composite Key — a key made up of more than one column. Can be primary or unique.

8. **Difference between CROSS JOIN, SELF JOIN, OUTER JOIN, INNER JOIN?**
INNER JOIN — returns rows where there is a match in both tables.
LEFT OUTER JOIN — all rows from left table, matching rows from right (NULL if no match).
RIGHT OUTER JOIN — all rows from right table, matching rows from left.
FULL OUTER JOIN — all rows from both tables, NULLs where no match.
CROSS JOIN — cartesian product of both tables. Every row from left joined with every row from right. No join condition.
SELF JOIN — a table joined with itself using aliases. Used for hierarchical data (e.g., employee-manager).

9. **Transactions — difference between COMMIT and ROLLBACK?**
A transaction is a unit of work that must be fully completed or fully undone.
COMMIT — saves all changes made in the transaction permanently.
ROLLBACK — undoes all changes made since the transaction began.
SAVEPOINT — marks a point within a transaction to rollback to partially.
```sql
BEGIN TRANSACTION;
UPDATE Accounts SET Balance = Balance - 100 WHERE Id = 1;
UPDATE Accounts SET Balance = Balance + 100 WHERE Id = 2;
COMMIT; -- or ROLLBACK on error
```

10. **Clustered vs Non-Clustered Index?**
Clustered Index — determines the physical order of data in the table. Only one per table. Created automatically on primary key. Fast for range queries.
Non-Clustered Index — separate structure pointing to row locations. Multiple allowed per table. Good for columns used in WHERE or JOIN but not the primary key.

11. **How to bulk insert data into tables?**
BULK INSERT — loads data from a flat file directly into a table.
SELECT INTO — copies data from one table to a new table.
INSERT INTO ... SELECT — copies data from one table into an existing table.
SqlBulkCopy — .NET class for high-performance bulk inserts from code.
```sql
BULK INSERT Employees FROM 'C:\data\employees.csv'
WITH (FIELDTERMINATOR = ',', ROWTERMINATOR = '\n', FIRSTROW = 2);
```

12. **Difference between WHERE and HAVING clause?**
WHERE — filters rows before grouping. Cannot use aggregate functions.
HAVING — filters groups after GROUP BY. Can use aggregate functions like SUM, COUNT, AVG.
```sql
SELECT Dept, COUNT(*) as Total FROM Employees
WHERE Age > 25
GROUP BY Dept
HAVING COUNT(*) > 5;
```

13. **Query optimization techniques?**
Use indexes on columns in WHERE, JOIN, and ORDER BY. Use stored procedures instead of ad-hoc queries. Use JOINs instead of subqueries where possible. Avoid SELECT * — select only needed columns. Use EXISTS instead of IN for large datasets. Avoid functions on indexed columns in WHERE clause. Use query execution plan (SET STATISTICS IO ON or SSMS Execution Plan) to identify bottlenecks. Normalize tables. Use partitioning for very large tables. Fetch from views for pre-optimized complex queries.

14. **How to optimize stored procedures?**
Use SET NOCOUNT ON to suppress row count messages. Keep transactions short. Avoid cursors — use set-based operations. Use sp_executesql instead of EXEC for parameterized dynamic SQL. Avoid recompilation triggers (schema changes inside SP). Use appropriate indexes on tables the SP queries.

15. **How to analyze performance of a stored procedure?**
Use SQL Server Execution Plan (SSMS — Ctrl+M or Estimated/Actual Execution Plan). Use SET STATISTICS TIME ON and SET STATISTICS IO ON to see CPU time and logical reads. Use SQL Server Profiler or Extended Events to trace slow queries. Use sys.dm_exec_query_stats DMV to find expensive queries. Structure Map / Dependency Viewer in SSMS shows SP dependencies.

16. **What is an UPSERT operation? How to do it?**
UPSERT means INSERT if the row doesn't exist, UPDATE if it does. In SQL Server use MERGE statement.
```sql
MERGE INTO Employees AS target
USING (SELECT 1 AS Id, 'John' AS Name) AS source
ON target.Id = source.Id
WHEN MATCHED THEN UPDATE SET target.Name = source.Name
WHEN NOT MATCHED THEN INSERT (Id, Name) VALUES (source.Id, source.Name);
```

17. **What are SQL clauses?**
WHERE — filters rows based on a condition.
GROUP BY — groups rows sharing a value for aggregate functions.
HAVING — filters groups after GROUP BY.
ORDER BY — sorts the result set (ASC/DESC). Cannot be used in subqueries in SQL Server.
DISTINCT — removes duplicate rows from result.
TOP / LIMIT — limits the number of rows returned.

18. **What are triggers? Types?**
Triggers are special stored procedures that automatically execute when an event occurs.
DML Triggers — fire on INSERT, UPDATE, DELETE on a table. AFTER (or FOR) trigger runs after the operation. INSTEAD OF trigger replaces the operation.
DDL Triggers — fire on CREATE, ALTER, DROP events.
Logon Triggers — fire on LOGON events to the SQL Server instance.
Use cases: auditing, enforcing business rules, maintaining derived data.

19. **Execution order of triggers?**
For DML: INSTEAD OF triggers fire before the operation. AFTER triggers fire after the operation and after constraint checks. If multiple AFTER triggers exist on the same event, you can set which runs first and last using sp_settriggerorder, but middle ones have no guaranteed order.

20. **What is a transaction? Why is it needed?**
A transaction is a sequence of operations treated as a single unit — either all succeed or all fail. Needed to maintain data consistency and integrity, especially when multiple related changes must happen together (e.g., debit one account, credit another). Transactions follow ACID properties.

21. **What are ACID properties?**
Atomicity — all operations in a transaction complete or none do.
Consistency — database moves from one valid state to another.
Isolation — concurrent transactions don't interfere with each other.
Durability — committed changes are permanent even after system failure.

22. **Difference between CHAR, VARCHAR, NCHAR, NVARCHAR?**
CHAR(n) — fixed-length non-Unicode. Pads with spaces. Fast for fixed-size data.
VARCHAR(n) — variable-length non-Unicode. Stores only used bytes.
NCHAR(n) — fixed-length Unicode. Uses 2 bytes per character.
NVARCHAR(n) — variable-length Unicode. Use for multilingual data. Prefix string with N: N'हिंदी'.
Rule of thumb: use VARCHAR for English-only, NVARCHAR for international text.

23. **Can we use ORDER BY in a subquery?**
No. ORDER BY is not allowed inside a subquery in SQL Server unless TOP, OFFSET-FETCH, or FOR XML is also used. Ordering is only meaningful in the outermost query.

24. **How to pass data from a CSV file to a Stored Procedure?**
Option 1 — BULK INSERT into a staging table first, then process from there. Option 2 — Use Table-Valued Parameters (TVP): define a table type, pass it as a parameter to the SP. Option 3 — Read CSV in application code, build DataTable, use SqlBulkCopy or pass as TVP via ADO.NET.

25. **What is COALESCE()?**
Returns the first non-NULL value from a list of expressions. Used to replace NULLs with a default value.
```sql
SELECT COALESCE(MiddleName, 'N/A') FROM Employees;
-- Returns 'N/A' if MiddleName is NULL
```

26. **Difference between ExecuteReader, ExecuteScalar, and ExecuteNonQuery?**
ExecuteReader — executes a SELECT query and returns a SqlDataReader for reading multiple rows.
ExecuteScalar — executes a query and returns a single value (first column of first row). Used for COUNT, SUM, etc.
ExecuteNonQuery — executes INSERT, UPDATE, DELETE, or DDL. Returns the number of rows affected.

27. **Stored procedure to return Nth highest salary?**
```sql
CREATE PROCEDURE GetNthHighestSalary @N INT
AS
BEGIN
    SELECT DISTINCT Salary FROM Employees
    ORDER BY Salary DESC
    OFFSET @N - 1 ROWS FETCH NEXT 1 ROW ONLY;
END
-- Or using DENSE_RANK:
SELECT Salary FROM (
    SELECT Salary, DENSE_RANK() OVER (ORDER BY Salary DESC) AS Rnk FROM Employees
) T WHERE Rnk = @N;
```

28. **Query to find employees who earn more than their manager?**
```sql
SELECT e.Name, e.Salary
FROM Employees e
JOIN Employees m ON e.ManagerId = m.Id
WHERE e.Salary > m.Salary;
```

29. **Identify and delete duplicate rows?**
```sql
-- Identify duplicates
SELECT Name, COUNT(*) FROM Employees GROUP BY Name HAVING COUNT(*) > 1;

-- Delete duplicates keeping one
WITH CTE AS (
    SELECT *, ROW_NUMBER() OVER (PARTITION BY Name, Email ORDER BY Id) AS rn FROM Employees
)
DELETE FROM CTE WHERE rn > 1;
```

30. **How to get the ID of the last inserted row?**
@@IDENTITY — returns last identity value inserted in the current session, any table.
SCOPE_IDENTITY() — returns last identity in the current scope (same SP/trigger). Preferred.
IDENT_CURRENT('TableName') — returns last identity for a specific table regardless of session or scope.

31. **Find tables from a database with the same prefix?**
```sql
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME LIKE 'prefix_%' AND TABLE_TYPE = 'BASE TABLE';
```

32. **CTE vs Subquery — when to use which?**
Subquery — use for simple, one-off filtering or scalar lookups inline. Good when used once.
CTE — use when the logic needs to be referenced multiple times in the same query, for recursive queries (hierarchical data), or when readability matters. CTEs are not materialized (re-executed each time referenced) unless the optimizer caches them.

33. **Query using HAVING clause?**
```sql
SELECT DepartmentId, AVG(Salary) AS AvgSalary
FROM Employees
GROUP BY DepartmentId
HAVING AVG(Salary) > 60000;
```

34. **Query to retrieve highest and lowest salary of employees aged 22-30?**
```sql
SELECT MAX(Salary) AS HighestSalary, MIN(Salary) AS LowestSalary
FROM Employees
WHERE Age BETWEEN 22 AND 30;
```

35. **Query to swap employee gender (M→F) where salary > 50000 and age > 30?**
Assuming Employee, Salary, and Age are separate tables joined by EmployeeId.
```sql
UPDATE e
SET e.Gender = CASE WHEN e.Gender = 'M' THEN 'F' ELSE 'M' END
FROM Employee e
JOIN Salary s ON e.Id = s.EmployeeId
JOIN Age a ON e.Id = a.EmployeeId
WHERE s.Amount > 50000 AND a.Age > 30;
```

36. **How to copy data from one table to another?**
```sql
-- Copy to a new table (creates it)
SELECT * INTO NewTable FROM OldTable;

-- Copy to an existing table
INSERT INTO ExistingTable (Col1, Col2)
SELECT Col1, Col2 FROM OldTable;

-- Copy with condition
INSERT INTO ArchiveEmployees SELECT * FROM Employees WHERE Status = 'Inactive';
```