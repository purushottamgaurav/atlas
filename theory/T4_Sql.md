# SQL Interview Q&A

> 25 Theory Questions + 25 Coding Questions with short answers and clean SQL examples.

---

## Part 1: Theory Questions

---

**Q1. What is the difference between DELETE, TRUNCATE, and DROP?**

- **DELETE** — removes rows one by one, can use `WHERE`, fully logged, can be rolled back.
- **TRUNCATE** — removes all rows at once, no `WHERE`, minimal logging, much faster, can be rolled back inside an explicit transaction in SQL Server.
- **DROP** — removes the entire table and all its objects (indexes, constraints, triggers). Cannot be easily rolled back.

Use DELETE when you need conditions or rollback. Use TRUNCATE to wipe a table fast. Use DROP when you want the table gone completely.

---

**Q2. What is the difference between WHERE and HAVING?**

- **WHERE** — filters rows **before** grouping. Cannot use aggregate functions like `SUM()`, `COUNT()`.
- **HAVING** — filters groups **after** `GROUP BY`. Can use aggregate functions.

```sql
SELECT Dept, COUNT(*) AS Total
FROM Employees
WHERE Age > 25            -- filters rows first
GROUP BY Dept
HAVING COUNT(*) > 5;      -- filters groups after
```

---

**Q3. What is the difference between UNION, UNION ALL, and INTERSECT?**

- **UNION** — combines two queries and removes duplicates. Slower.
- **UNION ALL** — combines two queries and keeps duplicates. Faster.
- **INTERSECT** — returns only rows that appear in **both** query results.
- **EXCEPT** — returns rows from the first query that are **not** in the second.

---

**Q4. What is the difference between Clustered and Non-Clustered Index?**

- **Clustered Index** — physically sorts and stores table data in index order. Only **one** per table. Created automatically on Primary Key. Fast for range queries.
- **Non-Clustered Index** — a separate structure that points to the actual rows. You can have **many** per table. Good for columns frequently used in `WHERE` or `JOIN`.

Think of clustered index as the book itself (sorted), non-clustered as the index at the back of the book (pointers).

---

**Q5. What are ACID properties?**

- **Atomicity** — all steps in a transaction succeed or none do.
- **Consistency** — the database moves from one valid state to another.
- **Isolation** — concurrent transactions don't interfere with each other.
- **Durability** — committed data is permanent, even after a crash.

---

**Q6. What is the difference between a Stored Procedure and a Function?**

| Feature | Stored Procedure | Function |
|---|---|---|
| Returns | Optional (output params) | Must return a value |
| DML (INSERT/UPDATE/DELETE) | Yes | No (scalar functions) |
| Use in SELECT | No | Yes |
| Transactions | Yes | No |
| Exception handling | Yes | Limited |

Use functions for calculations used inside queries. Use stored procedures for business logic and data modifications.

---

**Q7. What is normalization? Explain 1NF, 2NF, 3NF.**

Normalization organizes tables to reduce data redundancy and improve integrity.

- **1NF** — each column holds atomic (single) values. No repeating groups. Each row is unique.
- **2NF** — meets 1NF + every non-key column depends on the **whole** primary key (no partial dependency). Applies to composite keys.
- **3NF** — meets 2NF + no non-key column depends on another non-key column (no transitive dependency).
- **BCNF** — stricter version of 3NF. Every determinant must be a candidate key.

---

**Q8. What are SQL Constraints?**

Constraints enforce rules on data in a table:

- **PRIMARY KEY** — uniquely identifies each row. No NULLs. One per table.
- **FOREIGN KEY** — enforces referential integrity between two tables.
- **UNIQUE** — all values in a column must be different. Allows one NULL.
- **CHECK** — ensures values meet a condition (e.g., `Age > 0`).
- **DEFAULT** — sets a default value when none is provided.
- **NOT NULL** — column cannot store NULL.

---

**Q9. What is a CTE and when would you use it over a Subquery?**

A **CTE (Common Table Expression)** is a named temporary result set defined with `WITH` at the top of a query.

Use CTE when:
- The same subquery logic is reused multiple times in the query.
- You need recursive queries (e.g., org charts, hierarchies).
- You want better readability.

Use a subquery for simple one-off inline filtering.

---

**Q10. What is the difference between Temporary Tables and Table Variables?**

| Feature | Temp Table (#table) | Table Variable (@table) |
|---|---|---|
| Stored in | tempdb | tempdb |
| Indexes | Yes | Only primary key |
| Scope | Session | Current batch only |
| TRUNCATE | Yes | No |
| Recompilation | More likely | Less likely |

Use temp tables for large datasets or when you need indexes. Use table variables for small, short-lived data in a batch.

---

**Q11. What are triggers and what types exist?**

Triggers are stored procedures that automatically run when a specific event happens.

- **AFTER trigger** — runs after `INSERT`, `UPDATE`, or `DELETE`. Used for auditing, logging.
- **INSTEAD OF trigger** — replaces the operation. Useful on views.
- **DDL trigger** — fires on schema changes like `CREATE`, `ALTER`, `DROP`.
- **Logon trigger** — fires when a user logs into SQL Server.

Triggers have access to `INSERTED` and `DELETED` virtual tables that hold the new and old row values.

---

**Q12. What is the difference between CHAR, VARCHAR, NCHAR, NVARCHAR?**

- **CHAR(n)** — fixed length, non-Unicode. Always uses n bytes. Good for fixed-size codes.
- **VARCHAR(n)** — variable length, non-Unicode. Uses only what's needed.
- **NCHAR(n)** — fixed length, Unicode. 2 bytes per character.
- **NVARCHAR(n)** — variable length, Unicode. Use for international/multilingual text.

Rule: use `VARCHAR` for English-only data, `NVARCHAR` for any language. Always prefix Unicode strings with `N`: `N'हिंदी'`.

---

**Q13. What is COALESCE() and when do you use it?**

`COALESCE()` returns the first non-NULL value from a list of expressions. Used to replace NULLs with a fallback value.

```sql
SELECT COALESCE(MiddleName, 'N/A') AS MiddleName FROM Employees;
-- returns 'N/A' if MiddleName is NULL
```

`ISNULL(value, default)` is the SQL Server-specific version that takes only two arguments. `COALESCE` is ANSI standard and more flexible.

---

**Q14. What is the difference between Local and Global Temporary Tables?**

- **Local (#table)** — visible only to the current session. Dropped when the session ends.
- **Global (##table)** — visible to all sessions. Dropped when the last session using it disconnects.

Both are physically stored in `tempdb`.

---

**Q15. What is UPSERT and how do you do it in SQL Server?**

UPSERT means "insert if the row doesn't exist, update if it does." In SQL Server, use the `MERGE` statement.

```sql
MERGE INTO Employees AS target
USING (SELECT 1 AS Id, 'John' AS Name) AS source ON target.Id = source.Id
WHEN MATCHED THEN UPDATE SET target.Name = source.Name
WHEN NOT MATCHED THEN INSERT (Id, Name) VALUES (source.Id, source.Name);
```

---

**Q16. What are the different types of JOINs?**

- **INNER JOIN** — rows with a match in both tables.
- **LEFT JOIN** — all rows from left, matching from right (NULL if no match).
- **RIGHT JOIN** — all rows from right, matching from left.
- **FULL OUTER JOIN** — all rows from both, NULLs where no match.
- **CROSS JOIN** — cartesian product. Every row from left × every row from right. No join condition.
- **SELF JOIN** — a table joined with itself. Used for hierarchical data like employee-manager.

---

**Q17. What is the difference between @@IDENTITY, SCOPE_IDENTITY(), and IDENT_CURRENT()?**

All return the last auto-generated identity value, but scope differs:

- **@@IDENTITY** — last identity inserted in the current session, any table including triggers.
- **SCOPE_IDENTITY()** — last identity in the current scope only (same stored procedure). **Preferred** — safer.
- **IDENT_CURRENT('TableName')** — last identity for a specific table regardless of session or scope.

---

**Q18. What is query optimization? Name common techniques.**

Making queries run faster and use fewer resources:

- Add **indexes** on columns used in `WHERE`, `JOIN`, `ORDER BY`.
- Avoid **SELECT *** — select only needed columns.
- Use **EXISTS** instead of `IN` for large datasets.
- Avoid **functions on indexed columns** in `WHERE` (prevents index use).
- Use **JOINs** instead of subqueries where possible.
- Use **execution plan** (SSMS: Ctrl+M) to find bottlenecks.
- Use `SET STATISTICS IO ON` to check logical reads.
- Use stored procedures instead of ad-hoc queries.
- Paginate results instead of returning huge datasets.

---

**Q19. What is a transaction and why is it needed?**

A transaction is a group of SQL statements that must all succeed or all fail together. Needed to keep data consistent — for example, deducting from one account and adding to another must both happen or neither should.

```sql
BEGIN TRANSACTION;
    UPDATE Accounts SET Balance -= 500 WHERE Id = 1;
    UPDATE Accounts SET Balance += 500 WHERE Id = 2;
COMMIT;  -- or ROLLBACK if something fails
```

---

**Q20. What is a View and when would you use it?**

A view is a saved SQL query that acts like a virtual table. It doesn't store data itself — it runs the underlying query each time.

Use views to:
- Simplify complex queries for reuse.
- Restrict access to specific columns or rows for security.
- Present data in a business-friendly format.

```sql
CREATE VIEW ActiveEmployees AS
SELECT Id, Name, Dept FROM Employees WHERE IsActive = 1;
```

---

**Q21. What is the difference between ExecuteReader, ExecuteScalar, and ExecuteNonQuery in ADO.NET?**

- **ExecuteReader** — runs a `SELECT` and returns a `SqlDataReader` for reading multiple rows.
- **ExecuteScalar** — runs a query and returns a **single value** (first column, first row). Good for `COUNT`, `SUM`.
- **ExecuteNonQuery** — runs `INSERT`, `UPDATE`, `DELETE`, or DDL. Returns the **number of rows affected**.

---

**Q22. What is SQL injection and how do you prevent it?**

SQL injection is when malicious input is inserted into a SQL query to manipulate the database.

Example of vulnerable code: `"SELECT * FROM Users WHERE Name = '" + input + "'"`
An attacker passes `' OR '1'='1` and gets all rows.

Prevention:
- Use **parameterized queries** or stored procedures — never concatenate user input.
- Use an **ORM** like Entity Framework which parameterizes automatically.
- Validate and sanitize inputs.

```sql
-- Safe parameterized query in SQL Server
EXEC sp_executesql N'SELECT * FROM Users WHERE Name = @Name', N'@Name NVARCHAR(100)', @Name = @input;
```

---

**Q23. What is the difference between a Primary Key and a Unique Key?**

| Feature | Primary Key | Unique Key |
|---|---|---|
| NULLs allowed | No | Yes (one NULL) |
| Per table | Only one | Multiple allowed |
| Creates index | Clustered (default) | Non-clustered |
| Purpose | Row identifier | Enforce uniqueness |

---

**Q24. What are window functions and what is the difference between RANK, DENSE_RANK, and ROW_NUMBER?**

Window functions perform calculations across a set of rows related to the current row, without collapsing rows like `GROUP BY`.

- **ROW_NUMBER()** — unique sequential number. No ties. 1,2,3,4.
- **RANK()** — same rank for ties, then skips numbers. 1,1,3,4.
- **DENSE_RANK()** — same rank for ties, no gaps. 1,1,2,3.

```sql
SELECT Name, Salary,
    ROW_NUMBER() OVER (ORDER BY Salary DESC) AS RowNum,
    RANK()       OVER (ORDER BY Salary DESC) AS Rnk,
    DENSE_RANK() OVER (ORDER BY Salary DESC) AS DenseRnk
FROM Employees;
```

---

**Q25. What is database indexing and what are the downsides of too many indexes?**

An index is a data structure that speeds up data retrieval on a column. Without it, SQL Server does a full table scan.

Downsides of too many indexes:
- **Slower writes** — every `INSERT`, `UPDATE`, `DELETE` must update all indexes on the table.
- **More storage** — each index takes disk space.
- **Higher maintenance** — fragmented indexes need regular rebuilding.

Add indexes on columns used in `WHERE`, `JOIN`, and `ORDER BY`. Remove unused indexes.

---

## Part 2: Coding Questions

---

**Q26. Find the second highest salary.**

```sql
-- Using OFFSET
SELECT DISTINCT Salary FROM Employees
ORDER BY Salary DESC
OFFSET 1 ROWS FETCH NEXT 1 ROW ONLY;

-- Using subquery
SELECT MAX(Salary) FROM Employees
WHERE Salary < (SELECT MAX(Salary) FROM Employees);
```

---

**Q27. Find the Nth highest salary.**

```sql
-- Works for any N using DENSE_RANK
SELECT Salary FROM (
    SELECT Salary, DENSE_RANK() OVER (ORDER BY Salary DESC) AS Rnk
    FROM Employees
) T
WHERE Rnk = 3;  -- change 3 to any N
```

---

**Q28. Find duplicate rows in a table.**

```sql
-- Show duplicate names and how many times they appear
SELECT Name, Email, COUNT(*) AS DuplicateCount
FROM Employees
GROUP BY Name, Email
HAVING COUNT(*) > 1;
```

---

**Q29. Delete duplicate rows but keep one.**

```sql
WITH CTE AS (
    SELECT *,
        ROW_NUMBER() OVER (PARTITION BY Name, Email ORDER BY Id) AS rn
    FROM Employees
)
DELETE FROM CTE WHERE rn > 1;
-- rn = 1 is kept (first occurrence), rest are deleted
```

---

**Q30. Find employees who earn more than their manager.**

```sql
SELECT e.Name AS Employee, e.Salary AS EmpSalary,
       m.Name AS Manager, m.Salary AS ManagerSalary
FROM Employees e
JOIN Employees m ON e.ManagerId = m.Id
WHERE e.Salary > m.Salary;
```

---

**Q31. Find all employees who have no manager (top-level).**

```sql
SELECT * FROM Employees WHERE ManagerId IS NULL;
```

---

**Q32. Get the department with the highest average salary.**

```sql
SELECT TOP 1 DepartmentId, AVG(Salary) AS AvgSalary
FROM Employees
GROUP BY DepartmentId
ORDER BY AvgSalary DESC;
```

---

**Q33. Get employees whose salary is above the department average.**

```sql
SELECT e.Name, e.Salary, e.DepartmentId
FROM Employees e
WHERE e.Salary > (
    SELECT AVG(Salary) FROM Employees
    WHERE DepartmentId = e.DepartmentId
);
```

---

**Q34. Get the top 3 earners per department.**

```sql
SELECT Name, DepartmentId, Salary FROM (
    SELECT Name, DepartmentId, Salary,
        DENSE_RANK() OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS Rnk
    FROM Employees
) T
WHERE Rnk <= 3;
```

---

**Q35. Find employees hired in the last 30 days.**

```sql
SELECT * FROM Employees
WHERE HireDate >= DATEADD(DAY, -30, GETDATE());
```

---

**Q36. Swap values in a column (e.g., swap M and F in Gender).**

```sql
UPDATE Employees
SET Gender = CASE WHEN Gender = 'M' THEN 'F' ELSE 'M' END;

-- With a condition
UPDATE Employees
SET Gender = CASE WHEN Gender = 'M' THEN 'F' ELSE 'M' END
WHERE Salary > 50000;
```

---

**Q37. Get the count of employees per department, including departments with no employees.**

```sql
-- Use LEFT JOIN so departments with 0 employees still appear
SELECT d.DepartmentName, COUNT(e.Id) AS EmployeeCount
FROM Departments d
LEFT JOIN Employees e ON d.Id = e.DepartmentId
GROUP BY d.DepartmentName;
```

---

**Q38. Find the highest and lowest salary per department.**

```sql
SELECT DepartmentId,
    MAX(Salary) AS HighestSalary,
    MIN(Salary) AS LowestSalary
FROM Employees
GROUP BY DepartmentId;
```

---

**Q39. Get all employees whose name starts with 'A' and has exactly 5 characters.**

```sql
SELECT * FROM Employees
WHERE Name LIKE 'A____';  -- A + exactly 4 underscores = 5 chars total
```

---

**Q40. Get a running total (cumulative sum) of salary ordered by hire date.**

```sql
SELECT Name, HireDate, Salary,
    SUM(Salary) OVER (ORDER BY HireDate ROWS UNBOUNDED PRECEDING) AS RunningTotal
FROM Employees;
```

---

**Q41. Find employees who share the same salary.**

```sql
SELECT e1.Name, e1.Salary
FROM Employees e1
WHERE e1.Salary IN (
    SELECT Salary FROM Employees
    GROUP BY Salary HAVING COUNT(*) > 1
)
ORDER BY e1.Salary;
```

---

**Q42. Pivot — show total salary per department as columns.**

```sql
SELECT * FROM (
    SELECT DepartmentId, Salary FROM Employees
) src
PIVOT (
    SUM(Salary) FOR DepartmentId IN ([1], [2], [3])
) pvt;
```

---

**Q43. Get the first and last record from a table.**

```sql
-- First record
SELECT TOP 1 * FROM Employees ORDER BY Id ASC;

-- Last record
SELECT TOP 1 * FROM Employees ORDER BY Id DESC;

-- Both in one query
SELECT * FROM Employees WHERE Id = (SELECT MIN(Id) FROM Employees)
UNION ALL
SELECT * FROM Employees WHERE Id = (SELECT MAX(Id) FROM Employees);
```

---

**Q44. Find employees who joined in the same year as their manager.**

```sql
SELECT e.Name AS Employee, m.Name AS Manager
FROM Employees e
JOIN Employees m ON e.ManagerId = m.Id
WHERE YEAR(e.HireDate) = YEAR(m.HireDate);
```

---

**Q45. Copy data from one table to a new table and an existing table.**

```sql
-- Copy to a new table (creates it automatically)
SELECT * INTO EmployeesBackup FROM Employees;

-- Copy to an existing table
INSERT INTO EmployeesArchive (Id, Name, Salary)
SELECT Id, Name, Salary FROM Employees WHERE IsActive = 0;
```

---

**Q46. Find the total number of orders and total revenue per customer, only for customers with more than 5 orders.**

```sql
SELECT CustomerId,
    COUNT(*) AS TotalOrders,
    SUM(Amount) AS TotalRevenue
FROM Orders
GROUP BY CustomerId
HAVING COUNT(*) > 5
ORDER BY TotalRevenue DESC;
```

---

**Q47. Find all products that have never been ordered.**

```sql
-- Using LEFT JOIN
SELECT p.ProductName
FROM Products p
LEFT JOIN OrderItems oi ON p.Id = oi.ProductId
WHERE oi.ProductId IS NULL;

-- Using NOT EXISTS (faster on large data)
SELECT ProductName FROM Products p
WHERE NOT EXISTS (
    SELECT 1 FROM OrderItems oi WHERE oi.ProductId = p.Id
);
```

---

**Q48. Get the median salary from the Employees table.**

```sql
SELECT AVG(Salary * 1.0) AS MedianSalary
FROM (
    SELECT Salary,
        ROW_NUMBER() OVER (ORDER BY Salary) AS RowAsc,
        ROW_NUMBER() OVER (ORDER BY Salary DESC) AS RowDesc
    FROM Employees
) T
WHERE RowAsc IN (RowDesc, RowDesc - 1, RowDesc + 1);
-- This handles both odd and even counts
```

---

**Q49. Recursive CTE — get all employees in a manager's hierarchy.**

```sql
WITH OrgChart AS (
    -- Anchor: start with the top manager
    SELECT Id, Name, ManagerId, 0 AS Level
    FROM Employees WHERE ManagerId IS NULL

    UNION ALL

    -- Recursive: join each employee to their manager
    SELECT e.Id, e.Name, e.ManagerId, oc.Level + 1
    FROM Employees e
    JOIN OrgChart oc ON e.ManagerId = oc.Id
)
SELECT * FROM OrgChart ORDER BY Level;
```

---

**Q50. Get a comma-separated list of employee names per department.**

```sql
SELECT DepartmentId,
    STRING_AGG(Name, ', ') WITHIN GROUP (ORDER BY Name) AS EmployeeNames
FROM Employees
GROUP BY DepartmentId;
-- STRING_AGG is available in SQL Server 2017+
```

---

