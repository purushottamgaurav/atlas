# SQL Complete Reference & Interview Q&A

> Schema setup + SQL Server (T-SQL) syntax reference, followed by 50 theory questions and 50 query questions (beginner → intermediate) on the PG schema.

---

# Part A — Schema Setup

Database: **PG** | Schemas: **Customers, Products, Transactions, Credentials**

```sql
-- ── Database ────────────────────────────────────────────────
create database PG
-- drop database PG
use PG

-- ── Customers ───────────────────────────────────────────────
create table Customers (
    customerid   int,
    customername varchar(40),
    expense      money,
    createdate   datetime
)

-- add PK after creation
alter table Customers alter column customerid int not null
alter table Customers add primary key (customerid)

-- ── Products ────────────────────────────────────────────────
create table Products (
    productid      int   primary key,
    productname    varchar(20) not null,
    productbuyerid int   foreign key references Customers(customerid),
    productprice   money not null
)

-- ── Transactions ────────────────────────────────────────────
create table Transactions (
    transactionid   int  primary key,
    trnsactiondate  date not null,
    transactionto   varchar(20) default 'Empty',
    transactionfrom varchar(20) default 'Empty'
)

-- ── Credentials ─────────────────────────────────────────────
create table Credentials (
    credid       int  identity primary key,   -- auto-increment
    credusername varchar(30) unique not null,
    credpassword varchar(30) not null
)

-- ── Sample DML ──────────────────────────────────────────────
insert into Customers values (1,'Rahul1',50,'2023-01-01')
insert into Customers values (2,'Priya',  60,'2023-02-01')
insert into Customers values (3,'Gaurav', 70,'2023-03-01')
insert into Customers values (4,'Sneha',  80,'2023-04-01')
insert into Customers values (5,'Arjun',  90,'2023-05-01')
insert into Customers values (6,'Purushottam',120,'2023-05-13')

insert into Products values (1,'Soap',   1, 50)
insert into Products values (2,'Shampoo',2, 70)
insert into Products values (3,'Cream',  3, 90)
insert into Products values (4,'Soap2',  3, 70)

insert into Transactions values (1,'2023-01-10','Alice','Bob')
insert into Transactions values (2,'2023-05-13',default,default)

insert into Credentials values ('pg','pgpg')
insert into Credentials values ('uttam','password')
```

---

# Part B — SQL Syntax Reference

## 1. SELECT — Basics

```sql
select * from Customers                          -- all rows & cols
select top 3 * from Customers                    -- first 3 rows
select customername, expense from Customers      -- specific cols
select distinct expense from Customers           -- unique values
select customername as Name, expense as Amount   -- column alias
    from Customers
```

## 2. WHERE — Filtering

```sql
-- Comparison operators
select * from Customers where expense > 70
select * from Customers where expense = 80
select * from Customers where expense <> 50      -- not equal

-- Logical operators (AND / OR / NOT)
select * from Customers
    where expense > 70 or expense < 60 and not expense = 50

-- LIKE — pattern matching
-- %  = zero or more chars    _  = exactly one char
select * from Customers where customername like 'R%'   -- starts with R
select * from Customers where customername like '%ul'  -- ends   with ul
select * from Customers where customername like '_r%'  -- 2nd char = r
select * from Customers where expense like '5_.%'      -- e.g. 5X.YZ

-- IN — match a list
select * from Customers where expense in (50, 70, 90)

-- BETWEEN — inclusive range
select * from Customers where expense between 20 and 90

-- IS NULL / IS NOT NULL
select * from Customers where customername is null
select * from Customers where customername is not null
```

## 3. ORDER BY / TOP / OFFSET-FETCH

```sql
select * from Customers order by expense desc                    -- descending
select * from Customers order by expense asc                     -- ascending (default)
select * from Customers order by expense desc, customername asc  -- multi-col

-- Pagination (SQL Server 2012+)
select * from Customers
    order by customerid
    offset 2 rows fetch next 3 rows only   -- skip 2, take next 3
```

## 4. INSERT / UPDATE / DELETE / TRUNCATE

```sql
-- INSERT
insert into Customers values (8,'Rahul8',80,'2023-05-13')

insert into Customers (customerid,customername,expense,createdate)
    values (9,'TestUser',55,'2023-06-01')

-- INSERT multiple rows
insert into Customers values
    (10,'UserA',40,'2023-07-01'),
    (11,'UserB',60,'2023-07-02')

-- UPDATE
update Customers set customername = 'Gaurav' where expense = 70

-- UPDATE multiple cols
update Customers
    set customername = 'UpdatedName',
        expense      = 999
    where customerid = 8

-- DELETE — row-level, logged, can be rolled back
delete from Customers where createdate = '2023-05-13'
delete from Customers where expense = 70

-- TRUNCATE — removes all rows, faster, minimal logging
truncate table Transactions   -- cannot use WHERE
```

## 5. Aggregate Functions & GROUP BY / HAVING

```sql
-- Aggregates
select count(*)          as TotalRows   from Customers
select count(expense)    as NonNullRows from Customers
select sum(expense)      as TotalSpend  from Customers
select avg(expense)      as AvgSpend    from Customers
select min(expense)      as MinSpend    from Customers
select max(expense)      as MaxSpend    from Customers

-- GROUP BY (aggregate per group)
select productname, count(productid) as Qty, sum(productprice) as Revenue
    from Products
    group by productname

-- HAVING — filter on aggregate (WHERE cannot use aggregates)
select productname, count(productid) as Qty
    from Products
    group by productname, productprice
    having productprice > 80

-- GROUP BY + ORDER BY
select expense, count(*) as Cnt
    from Customers
    group by expense
    order by Cnt desc
```

## 6. JOINs

```sql
-- INNER JOIN — only matching rows
select c.customername, p.productname, p.productprice
    from Customers c
    inner join Products p on c.customerid = p.productbuyerid

-- LEFT JOIN — all from left, matching from right (NULL if no match)
select c.customername, p.productname
    from Customers c
    left join Products p on c.customerid = p.productbuyerid

-- RIGHT JOIN — all from right, matching from left
select c.customername, p.productname
    from Customers c
    right join Products p on c.customerid = p.productbuyerid

-- FULL OUTER JOIN — all rows from both sides
select c.customername, p.productname
    from Customers c
    full join Products p on c.customerid = p.productbuyerid

-- CROSS JOIN — cartesian product
select c.customername, p.productname
    from Customers c
    cross join Products p

-- SELF JOIN — table joined with itself (e.g. manager hierarchy)
select a.customername as Customer, b.customername as Referrer
    from Customers a, Customers b
    where a.customerid = b.customerid
```

## 7. UNION / INTERSECT / EXCEPT

```sql
-- UNION — combines results, removes duplicates
select customername as Name from Customers
union
select productname  as Name from Products

-- UNION ALL — keeps duplicates (faster)
select customername as Name from Customers
union all
select productname as Name from Products

-- INTERSECT — rows that appear in BOTH result sets
select customerid from Customers
intersect
select productbuyerid from Products

-- EXCEPT — rows in first set but NOT in second
select customerid from Customers
except
select productbuyerid from Products
```

## 8. Subqueries / EXISTS / CTE

```sql
-- Subquery in WHERE
select * from Customers
    where expense > (select avg(expense) from Customers)

-- Subquery in FROM (derived table)
select * from (
    select customername, expense from Customers where expense > 60
) as HighSpenders

-- EXISTS — true if subquery returns any row
select * from Customers
    where exists (
        select 1 from Products p
        where p.productbuyerid = Customers.customerid
    )

-- NOT EXISTS
select * from Customers
    where not exists (
        select 1 from Products p
        where p.productbuyerid = Customers.customerid
    )

-- IN with subquery
select * from Customers
    where customerid in (select productbuyerid from Products)

-- CTE (Common Table Expression) — readable, reusable within query
with HighSpendersCTE as (
    select * from Customers where expense > 70
)
select * from HighSpendersCTE

-- CTE to find Nth highest price
with RankedPrices as (
    select productprice,
           dense_rank() over (order by productprice desc) as rnk
    from Products
)
select productprice from RankedPrices where rnk = 2   -- 2nd highest
```

## 9. Window Functions

```sql
-- ROW_NUMBER — unique sequential number per partition
select row_number() over (partition by expense order by expense) as row_num,
       customername, expense, createdate
    from Customers

-- RANK — gaps allowed when tied
select rank() over (order by expense desc) as rnk,
       customername, expense
    from Customers

-- DENSE_RANK — no gaps when tied
select dense_rank() over (order by expense desc) as dense_rnk,
       customername, expense
    from Customers

-- NTILE(n) — divide rows into n buckets
select ntile(3) over (order by expense) as bucket,
       customername, expense
    from Customers

-- LAG / LEAD — previous / next row value
select customername, expense,
       lag(expense,  1, 0) over (order by customerid) as PrevExpense,
       lead(expense, 1, 0) over (order by customerid) as NextExpense
    from Customers

-- Running total with SUM OVER
select customername, expense,
       sum(expense) over (order by customerid rows unbounded preceding) as RunningTotal
    from Customers
```

## 10. DDL — ALTER TABLE

```sql
-- Add column
alter table Customers add email varchar(50)

-- Drop column
alter table Customers drop column email

-- Change data type
alter table Customers alter column customername varchar(60)

-- Add NOT NULL constraint
alter table Customers alter column customername varchar(60) not null

-- Add UNIQUE constraint
alter table Credentials add constraint UQ_username unique (credusername)

-- Add CHECK constraint
alter table Products add constraint CHK_price check (productprice > 0)

-- Add DEFAULT
alter table Transactions add constraint DF_to default 'Empty' for transactionto

-- Drop constraint
alter table Products drop constraint CHK_price

-- Add Foreign Key
alter table Products
    add constraint FK_buyer
    foreign key (productbuyerid) references Customers(customerid)
```

## 11. Views

```sql
-- Create view
create view CustomerMoneyView as
    select * from Customers where expense = 80

-- Use view
select * from CustomerMoneyView

-- Alter view (SQL Server)
alter view CustomerMoneyView as
    select * from Customers where expense >= 80

-- View with JOIN
create view CustomerProductView as
    select c.customername, p.productname, p.productprice
    from Customers c
    inner join Products p on c.customerid = p.productbuyerid

-- Drop view
drop view CustomerMoneyView
```

## 12. Stored Procedures

```sql
-- Basic SP with parameters
create procedure sp_InsertCustomer
    @cid   int,
    @cname varchar(20),
    @ex    money,
    @date  date
as
begin
    insert into Customers values (@cid, @cname, @ex, @date)
end

execute sp_InsertCustomer @cid=6, @cname='Purushottam', @ex=120, @date='2023-05-13'

-- SP with OUTPUT parameter
create procedure sp_GetCustomerCount
    @count int output
as
begin
    select @count = count(*) from Customers
end

declare @total int
exec sp_GetCustomerCount @count = @total output
select @total as TotalCustomers

-- SP using CTE — Nth highest product price
create procedure sp_NthHighestPrice
    @nth int
as
begin
    with cte as (
        select top (@nth) * from Products order by productprice desc
    )
    select top 1 * from cte order by productprice asc
end

execute sp_NthHighestPrice 2

-- Drop SP
drop procedure sp_InsertCustomer
```

## 13. Transactions

```sql
begin transaction
    insert into Customers values (7,'Rahul6',88,'2023-05-13')
    if (@@error > 0)
    begin
        rollback transaction
    end
    else
    begin
        commit transaction
    end

-- TRY / CATCH pattern (preferred modern approach)
begin try
    begin transaction
        insert into Customers values (12,'TryUser',55,'2023-08-01')
        update  Customers set expense = -1 where customerid = 12
    commit transaction
end try
begin catch
    rollback transaction
    select error_message() as ErrorMsg, error_number() as ErrNo
end catch

-- SAVEPOINT — partial rollback
begin transaction
    insert into Customers values (20,'A',10,'2024-01-01')
    save transaction SavePoint1
    insert into Customers values (21,'B',20,'2024-01-02')
    rollback transaction SavePoint1   -- undo only the 2nd insert
commit transaction
```

## 14. Triggers

```sql
-- AFTER INSERT trigger
create trigger trig_AfterInsertCustomer
on Customers
after insert
as
begin
    insert into Credentials values ('auto_user','auto_pass')
end

-- INSTEAD OF trigger
create trigger trig_InsteadOfDelete
on Customers
instead of delete
as
begin
    print 'Direct delete is not allowed on Customers.'
end

-- Drop trigger
drop trigger trig_AfterInsertCustomer
```

## 15. Variables / Control Flow / Temp Objects

```sql
-- Variables
declare @id   int         = 15
declare @name varchar(50) = 'Omkar'
declare @age  int         = 23

-- IF / ELSE — upsert pattern
if ((select count(*) from Customers where customerid = @id) = 1)
begin
    update Customers
        set customername = @name
        where customerid = @id
end
else
begin
    insert into Customers (customerid, customername, expense, createdate)
        values (@id, @name, 0, getdate())
end

-- WHILE loop
declare @i int = 1
while @i <= 5
begin
    print 'Row ' + cast(@i as varchar)
    set @i = @i + 1
end

-- Table variable (scope = batch/SP)
declare @TempTable table (
    id   int,
    name nvarchar(20)
)
insert into @TempTable values (1,'Test')
select * from @TempTable

-- Local Temp Table (scope = session)
create table #LocalTemp (
    author_id   int primary key,
    author_name varchar(255)
)
drop table #LocalTemp

-- Global Temp Table (visible to all sessions)
create table ##GlobalTemp (id int, val varchar(50))
drop table ##GlobalTemp
```

## 16. Identity / System Functions / Useful Queries

```sql
-- Get last inserted identity value (session-scoped, safest)
select scope_identity() as LastInsertedId
select ident_current('dbo.Customers') as CurrentIdentity

-- System info
select getdate()    as Now
select newid()      as NewGUID
select db_name()    as CurrentDB
select user_name()  as CurrentUser
select @@version    as SQLServerVersion
select @@rowcount   as RowsAffected
select @@error      as LastErrorCode

-- List tables matching a pattern
select schema_name(t.schema_id) as schema_name,
       t.name                   as table_name
    from sys.tables t
    where t.name like 'hr%'
    order by table_name, schema_name

-- String functions
select upper('hello')              -- HELLO
select lower('HELLO')              -- hello
select ltrim(rtrim('  hi  '))      -- trim both sides
select len('hello')                -- 5
select substring('hello',1,3)      -- hel
select replace('hello','l','r')    -- herro
select charindex('l','hello')      -- 3
select left('hello',3)             -- hel
select right('hello',3)            -- llo
select concat('SQL',' ','Server')  -- SQL Server

-- Date functions
select getdate()
select year(getdate())
select month(getdate())
select day(getdate())
select dateadd(day,  7, getdate())
select datediff(day,'2023-01-01',getdate())
select format(getdate(),'dd/MM/yyyy')

-- NULL handling
select isnull(null, 'Default')           -- Default
select coalesce(null, null, 'Found')     -- Found
select nullif(5, 5)                      -- NULL

-- CASE expression
select customername,
       expense,
       case
           when expense >= 100 then 'High'
           when expense >= 70  then 'Medium'
           else                     'Low'
       end as SpendCategory
    from Customers

-- IIF (shorthand CASE)
select customername,
       iif(expense > 80, 'VIP', 'Regular') as Status
    from Customers

-- CAST / CONVERT
select cast(expense as int) from Customers
select convert(varchar, createdate, 103) from Customers  -- dd/mm/yyyy
```

## 17. Indexes (Performance)

```sql
-- Clustered index (one per table, physically sorts data)
create clustered index IX_Customers_Expense
    on Customers (expense)

-- Non-clustered index (separate lookup structure)
create nonclustered index IX_Customers_Name
    on Customers (customername)

-- Composite index
create nonclustered index IX_Products_BuyerPrice
    on Products (productbuyerid, productprice)

-- Drop index
drop index IX_Customers_Name on Customers

-- View indexes on a table
select * from sys.indexes where object_id = object_id('Customers')
```

## 18. Normalization Quick Cheat Sheet

- **1NF** — Atomic columns, no repeating groups.
- **2NF** — 1NF + every non-key column depends on the WHOLE PK.
- **3NF** — 2NF + no transitive dependencies (non-key → non-key).
- **BCNF** — Every determinant is a candidate key.
- **4NF** — No multi-valued dependencies.
- **5NF** — No join dependencies that can't be inferred from PKs.

---

# Part C — 50 Theory Questions (Basics → In-Depth)

---

**Q1. What is SQL? What are DDL, DML, DCL, and TCL?**

SQL (Structured Query Language) is the standard language for managing relational databases.

- **DDL (Data Definition):** `CREATE`, `ALTER`, `DROP`, `TRUNCATE` — defines schema.
- **DML (Data Manipulation):** `SELECT`, `INSERT`, `UPDATE`, `DELETE` — manipulates data.
- **DCL (Data Control):** `GRANT`, `REVOKE` — controls permissions.
- **TCL (Transaction Control):** `BEGIN`, `COMMIT`, `ROLLBACK`, `SAVEPOINT` — manages transactions.

---

**Q2. What is the difference between DBMS and RDBMS?**

- **DBMS** stores data as files; no relational integrity (e.g., FoxPro, MS Access older versions).
- **RDBMS** stores data in tables with relationships, enforces ACID and referential integrity (SQL Server, Oracle, PostgreSQL, MySQL).

All modern systems are RDBMS.

---

**Q3. What is the difference between DELETE, TRUNCATE, and DROP?**

- **DELETE** — removes rows one by one, can use `WHERE`, fully logged, can be rolled back.
- **TRUNCATE** — removes all rows at once, no `WHERE`, minimal logging, much faster, resets identity, can be rolled back inside an explicit transaction in SQL Server.
- **DROP** — removes the entire table and all its objects (indexes, constraints, triggers).

Use DELETE when you need conditions or rollback. Use TRUNCATE to wipe a table fast. Use DROP when you want the table gone completely.

---

**Q4. What is the difference between WHERE and HAVING?**

- **WHERE** — filters rows **before** grouping. Cannot use aggregate functions.
- **HAVING** — filters groups **after** `GROUP BY`. Can use aggregate functions.

```sql
SELECT Dept, COUNT(*) AS Total
FROM Employees
WHERE Age > 25            -- filters rows first
GROUP BY Dept
HAVING COUNT(*) > 5;      -- filters groups after
```

---

**Q5. What is the difference between UNION, UNION ALL, INTERSECT, and EXCEPT?**

- **UNION** — combines two queries and removes duplicates. Slower (needs sort/hash).
- **UNION ALL** — combines two queries and keeps duplicates. Faster.
- **INTERSECT** — returns only rows that appear in **both** result sets.
- **EXCEPT** — returns rows from the first query that are **not** in the second.

All require matching column counts and compatible data types.

---

**Q6. What is the difference between Clustered and Non-Clustered Index?**

- **Clustered Index** — physically sorts and stores table data in index order. Only **one** per table. Created automatically on Primary Key. Fast for range queries.
- **Non-Clustered Index** — a separate structure that points to the actual rows. You can have **many** per table. Good for columns frequently used in `WHERE` or `JOIN`.

Think of clustered index as the book itself (sorted), non-clustered as the index at the back of the book (pointers).

---

**Q7. What are ACID properties?**

- **Atomicity** — all steps in a transaction succeed or none do.
- **Consistency** — the database moves from one valid state to another.
- **Isolation** — concurrent transactions don't interfere with each other.
- **Durability** — committed data is permanent, even after a crash.

---

**Q8. What is the difference between a Stored Procedure and a Function?**

| Feature | Stored Procedure | Function |
|---|---|---|
| Returns | Optional (output params) | Must return a value |
| DML (INSERT/UPDATE/DELETE) | Yes | No (scalar functions) |
| Use in SELECT | No | Yes |
| Transactions | Yes | No |
| Exception handling | Yes | Limited |

Use functions for calculations used inside queries. Use stored procedures for business logic and data modifications.

---

**Q9. What is normalization? Explain 1NF, 2NF, 3NF, and BCNF.**

Normalization organizes tables to reduce data redundancy and improve integrity.

- **1NF** — each column holds atomic (single) values. No repeating groups. Each row is unique.
- **2NF** — meets 1NF + every non-key column depends on the **whole** primary key (no partial dependency).
- **3NF** — meets 2NF + no non-key column depends on another non-key column (no transitive dependency).
- **BCNF** — stricter version of 3NF. Every determinant must be a candidate key.

---

**Q10. What is denormalization and when do you use it?**

Denormalization deliberately introduces redundancy into a normalized database to improve read performance. You duplicate data or pre-compute joins so queries run faster — at the cost of more storage and more complex writes.

Use it for:
- Read-heavy reporting/analytics tables.
- Data warehouses (star/snowflake schemas).
- When normalized joins are too slow to meet SLAs.

---

**Q11. What are SQL Constraints?**

Constraints enforce rules on data in a table:

- **PRIMARY KEY** — uniquely identifies each row. No NULLs. One per table.
- **FOREIGN KEY** — enforces referential integrity between two tables.
- **UNIQUE** — all values in a column must be different. Allows one NULL.
- **CHECK** — ensures values meet a condition (e.g., `Age > 0`).
- **DEFAULT** — sets a default value when none is provided.
- **NOT NULL** — column cannot store NULL.

---

**Q12. What is the difference between Primary Key and Unique Key?**

| Feature | Primary Key | Unique Key |
|---|---|---|
| NULLs allowed | No | Yes (one NULL) |
| Per table | Only one | Multiple allowed |
| Creates index | Clustered (default) | Non-clustered |
| Purpose | Row identifier | Enforce uniqueness |

---

**Q13. What is a Foreign Key and what is referential integrity?**

A **Foreign Key** is a column in one table that references the primary key of another table, enforcing **referential integrity** — you can't insert a child row pointing to a non-existent parent.

Cascade options on FKs:
- `ON DELETE CASCADE` — deleting parent deletes children.
- `ON UPDATE CASCADE` — updating parent PK updates children.
- `ON DELETE SET NULL` — children's FK set to NULL.
- `ON DELETE NO ACTION` — block delete if children exist (default).

---

**Q14. What are the different types of JOINs?**

- **INNER JOIN** — rows with a match in both tables.
- **LEFT JOIN** — all rows from left, matching from right (NULL if no match).
- **RIGHT JOIN** — all rows from right, matching from left.
- **FULL OUTER JOIN** — all rows from both, NULLs where no match.
- **CROSS JOIN** — cartesian product. Every row × every row. No join condition.
- **SELF JOIN** — a table joined with itself. Used for hierarchical data like employee-manager.

---

**Q15. What is the difference between EXISTS and IN?**

- **IN** compares the column to a list/subquery of values.
- **EXISTS** checks whether the subquery returns at least one row; doesn't care about the value.

```sql
-- IN
SELECT * FROM Customers WHERE customerid IN (SELECT productbuyerid FROM Products);

-- EXISTS (often faster on large subqueries; stops at first match)
SELECT * FROM Customers c
WHERE EXISTS (SELECT 1 FROM Products p WHERE p.productbuyerid = c.customerid);
```

`EXISTS` handles NULLs better than `NOT IN`.

---

**Q16. What is the difference between a Subquery and a Correlated Subquery?**

- **Subquery** — runs once, independently of the outer query.
- **Correlated Subquery** — references the outer query and runs **once per outer row**. Slower but useful for row-by-row checks.

```sql
-- Correlated
SELECT * FROM Customers c
WHERE c.expense > (SELECT AVG(expense) FROM Customers WHERE customerid <= c.customerid);
```

---

**Q17. What is a CTE and when would you use it over a Subquery?**

A **CTE (Common Table Expression)** is a named temporary result set defined with `WITH` at the top of a query.

Use CTE when:
- The same subquery logic is reused multiple times.
- You need **recursive** queries (e.g., org charts, hierarchies).
- You want better readability.

Use a subquery for simple one-off inline filtering.

---

**Q18. What is a Recursive CTE?**

A CTE that references itself. It has two parts: an **anchor** member (base case) and a **recursive** member, combined with `UNION ALL`. Used for hierarchies (employee-manager), tree traversal, generating sequences.

```sql
WITH OrgChart AS (
    SELECT Id, ManagerId, 0 AS Lvl FROM Employees WHERE ManagerId IS NULL
    UNION ALL
    SELECT e.Id, e.ManagerId, oc.Lvl + 1
    FROM Employees e JOIN OrgChart oc ON e.ManagerId = oc.Id
)
SELECT * FROM OrgChart;
```

---

**Q19. What are Window Functions? Explain RANK, DENSE_RANK, ROW_NUMBER.**

Window functions perform calculations across a set of rows related to the current row, without collapsing rows like `GROUP BY`.

- **ROW_NUMBER()** — unique sequential number. No ties. 1,2,3,4.
- **RANK()** — same rank for ties, then skips numbers. 1,1,3,4.
- **DENSE_RANK()** — same rank for ties, no gaps. 1,1,2,3.

Other useful ones: `LAG`, `LEAD`, `NTILE`, `SUM/AVG OVER (...)`.

---

**Q20. What is PIVOT and UNPIVOT?**

- **PIVOT** — rotates row data into columns (e.g., monthly totals as columns).
- **UNPIVOT** — does the reverse, columns into rows.

```sql
SELECT * FROM (SELECT DepartmentId, Salary FROM Employees) src
PIVOT (SUM(Salary) FOR DepartmentId IN ([1],[2],[3])) pvt;
```

---

**Q21. What is the difference between CHAR, VARCHAR, NCHAR, NVARCHAR?**

- **CHAR(n)** — fixed length, non-Unicode. Always uses n bytes.
- **VARCHAR(n)** — variable length, non-Unicode. Uses only what's needed.
- **NCHAR(n)** — fixed length, Unicode. 2 bytes per character.
- **NVARCHAR(n)** — variable length, Unicode. Use for international text.

Use `VARCHAR` for English-only data, `NVARCHAR` for any language. Prefix Unicode strings with `N`: `N'हिंदी'`.

---

**Q22. What is COALESCE() and how is it different from ISNULL()?**

`COALESCE()` returns the first non-NULL value from a list of expressions.

```sql
SELECT COALESCE(MiddleName, NickName, 'N/A') FROM Employees;
```

- **COALESCE** — ANSI standard, takes any number of arguments, return type based on highest precedence.
- **ISNULL** — SQL Server-specific, takes exactly two arguments, return type matches the first.

---

**Q23. What is the difference between Local and Global Temporary Tables?**

- **Local (#table)** — visible only to the current session. Dropped when the session ends.
- **Global (##table)** — visible to all sessions. Dropped when the last session using it disconnects.

Both are physically stored in `tempdb`.

---

**Q24. What is the difference between Temp Tables and Table Variables?**

| Feature | Temp Table (`#table`) | Table Variable (`@table`) |
|---|---|---|
| Stored in | tempdb | tempdb |
| Indexes | Yes (additional) | Only PK/unique |
| Scope | Session | Current batch only |
| TRUNCATE | Yes | No |
| Statistics | Yes | No |
| Recompilation | More likely | Less likely |

Use temp tables for large datasets or when you need indexes/stats. Use table variables for small, short-lived data in a batch.

---

**Q25. What is UPSERT and how do you do it in SQL Server?**

UPSERT = "insert if the row doesn't exist, update if it does." In SQL Server, use the `MERGE` statement.

```sql
MERGE INTO Employees AS target
USING (SELECT 1 AS Id, 'John' AS Name) AS source ON target.Id = source.Id
WHEN MATCHED THEN UPDATE SET target.Name = source.Name
WHEN NOT MATCHED THEN INSERT (Id, Name) VALUES (source.Id, source.Name);
```

---

**Q26. What is a View and when would you use it?**

A view is a saved SQL query that acts like a virtual table. It doesn't store data itself — it runs the underlying query each time.

Use views to:
- Simplify complex queries for reuse.
- Restrict access to specific columns/rows for security.
- Present data in a business-friendly format.

```sql
CREATE VIEW ActiveEmployees AS
SELECT Id, Name, Dept FROM Employees WHERE IsActive = 1;
```

---

**Q27. What is an Indexed View / Materialized View?**

A regular view runs its query every time. An **indexed view** (SQL Server's term for a materialized view) physically stores the result on disk with a unique clustered index. Reads are fast, but every base table change must update the indexed view.

```sql
CREATE VIEW dbo.vw_Sales WITH SCHEMABINDING AS
SELECT ProductId, SUM(Qty) AS TotalQty, COUNT_BIG(*) AS Cnt
FROM dbo.Sales GROUP BY ProductId;

CREATE UNIQUE CLUSTERED INDEX IX_vw_Sales ON dbo.vw_Sales (ProductId);
```

---

**Q28. What are Triggers and what types exist?**

Triggers are stored procedures that automatically run when a specific event happens.

- **AFTER trigger** — runs after `INSERT`, `UPDATE`, or `DELETE`. Used for auditing, logging.
- **INSTEAD OF trigger** — replaces the operation. Useful on views.
- **DDL trigger** — fires on schema changes (`CREATE`, `ALTER`, `DROP`).
- **Logon trigger** — fires when a user logs into SQL Server.

Triggers have access to `INSERTED` and `DELETED` virtual tables with new and old row values.

---

**Q29. What is a Cursor and when should you use one?**

A cursor lets you process rows one at a time, like an iterator. Useful for row-by-row logic that can't be expressed in set-based SQL — but **avoid cursors when a set-based query works**, they are much slower.

```sql
DECLARE cur CURSOR FOR SELECT Id FROM Customers;
OPEN cur;
FETCH NEXT FROM cur INTO @id;
WHILE @@FETCH_STATUS = 0 BEGIN
    -- per-row logic
    FETCH NEXT FROM cur INTO @id;
END
CLOSE cur; DEALLOCATE cur;
```

---

**Q30. What is a Transaction and why is it needed?**

A transaction is a group of SQL statements that must all succeed or all fail together. Needed to keep data consistent — e.g., transferring money: debit one account and credit another must both happen or neither.

```sql
BEGIN TRANSACTION;
    UPDATE Accounts SET Balance -= 500 WHERE Id = 1;
    UPDATE Accounts SET Balance += 500 WHERE Id = 2;
COMMIT;  -- or ROLLBACK if something fails
```

---

**Q31. What are the Isolation Levels in SQL Server?**

| Level | Dirty Read | Non-repeatable Read | Phantom Read |
|---|---|---|---|
| READ UNCOMMITTED | ✅ allowed | ✅ allowed | ✅ allowed |
| READ COMMITTED (default) | ❌ | ✅ allowed | ✅ allowed |
| REPEATABLE READ | ❌ | ❌ | ✅ allowed |
| SNAPSHOT | ❌ | ❌ | ❌ |
| SERIALIZABLE | ❌ | ❌ | ❌ |

```sql
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
```

Stricter level = more consistency but more blocking.

---

**Q32. What is a Deadlock and how do you prevent it?**

A deadlock happens when two transactions hold locks the other needs, and neither can proceed. SQL Server detects deadlocks and kills the lower-priority one (the "deadlock victim", error 1205).

Prevention:
- Always access tables in the **same order** across transactions.
- Keep transactions **short**.
- Use **lower isolation levels** where safe.
- Add proper **indexes** to reduce locking scope.
- Retry the victim transaction in app code.

---

**Q33. What is Optimistic vs Pessimistic Concurrency?**

- **Pessimistic** — locks rows when read so no one else can modify them. Safe but blocks readers/writers. Use when conflicts are frequent.
- **Optimistic** — no locks; checks at save time whether the row was modified since it was read (using a `RowVersion` / `Timestamp`). If yes, rejects with concurrency error. Use when conflicts are rare (most web apps).

---

**Q34. What is the NOLOCK hint and when should you avoid it?**

`WITH (NOLOCK)` is equivalent to `READ UNCOMMITTED` for a single table — it ignores locks and may return **dirty data** (uncommitted, may be rolled back), missing rows, or duplicate rows.

Use sparingly for reports that tolerate inaccuracy. **Never use** on financial, billing, or audit data.

```sql
SELECT * FROM Orders WITH (NOLOCK);
```

---

**Q35. What is Query Optimization? Name common techniques.**

Making queries run faster and use fewer resources:

- Add **indexes** on columns used in `WHERE`, `JOIN`, `ORDER BY`.
- Avoid **SELECT *** — select only needed columns.
- Use **EXISTS** instead of `IN` for large subqueries.
- Avoid **functions on indexed columns** in `WHERE` (prevents index use).
- Use **JOINs** instead of subqueries where possible.
- Inspect the **execution plan** (SSMS: Ctrl+M).
- Use `SET STATISTICS IO ON` to check logical reads.
- Use stored procedures (plan cached) over ad-hoc queries.
- **Paginate** instead of returning huge datasets.

---

**Q36. What is a SARGable query?**

SARG = Search ARGument. A query is **SARGable** if it can use indexes effectively. Wrapping an indexed column in a function makes it **non-SARGable**.

```sql
-- ❌ Non-SARGable (function on column)
SELECT * FROM Orders WHERE YEAR(OrderDate) = 2023;

-- ✅ SARGable (range on bare column)
SELECT * FROM Orders WHERE OrderDate >= '2023-01-01' AND OrderDate < '2024-01-01';
```

---

**Q37. What is database indexing and what are the downsides of too many indexes?**

An index is a data structure that speeds up data retrieval on a column. Without it, SQL Server does a full table scan.

Downsides of too many indexes:
- **Slower writes** — every `INSERT/UPDATE/DELETE` must update all indexes.
- **More storage** — each index takes disk space.
- **Higher maintenance** — fragmented indexes need rebuilding.

Add indexes on columns used in `WHERE`, `JOIN`, `ORDER BY`. Remove unused ones.

---

**Q38. What is a Composite Index? When to use it?**

An index on multiple columns. The **column order matters** — it's only used efficiently when the leading column appears in `WHERE`.

```sql
CREATE INDEX IX_Orders_CustomerDate ON Orders(CustomerId, OrderDate);
-- Helps: WHERE CustomerId = 5 AND OrderDate > '2023-01-01'
-- Helps: WHERE CustomerId = 5
-- Does NOT help: WHERE OrderDate > '2023-01-01'  (skips leading col)
```

---

**Q39. What is the difference between @@IDENTITY, SCOPE_IDENTITY(), and IDENT_CURRENT()?**

All return the last auto-generated identity value, but scope differs:

- **@@IDENTITY** — last identity inserted in the current session, **any table including triggers**.
- **SCOPE_IDENTITY()** — last identity in the current scope (same SP/batch). **Preferred** — safer.
- **IDENT_CURRENT('TableName')** — last identity for a specific table regardless of session or scope.

---

**Q40. What is a Composite Key vs Surrogate Key?**

- **Composite Key** — primary key made of two or more columns (e.g., `(OrderId, ProductId)` in an order-items table).
- **Surrogate Key** — an artificial single-column key (usually an IDENTITY/UUID) with no business meaning. Preferred for ease of joins and changes.

---

**Q41. What is the difference between CASE and IIF?**

Both return one value from multiple options.

```sql
-- CASE — ANSI standard, supports multiple branches
SELECT CASE WHEN expense > 80 THEN 'VIP' ELSE 'Regular' END FROM Customers;

-- IIF — SQL Server shorthand for two-branch CASE (since 2012)
SELECT IIF(expense > 80, 'VIP', 'Regular') FROM Customers;
```

Use `CASE` for more than two outcomes; `IIF` for quick binary checks.

---

**Q42. What is the GO statement?**

`GO` is not a SQL command — it's a **batch separator** used in SQL Server tools (SSMS, sqlcmd). It tells the client to send everything before it as one batch to the server.

```sql
USE PG;
GO
CREATE TABLE Foo (Id INT);
GO 5  -- run the preceding batch 5 times
```

---

**Q43. What are the system databases in SQL Server?**

- **master** — instance-level info: logins, linked servers, settings.
- **model** — template for new databases (any object here is copied to new DBs).
- **msdb** — SQL Agent jobs, schedules, backup history.
- **tempdb** — temp tables, table variables, intermediate sorts. Recreated on every restart.
- **resource** — read-only DB with system objects (hidden).

---

**Q44. What is a Schema?**

A schema is a logical container/namespace for database objects (tables, views, SPs). It groups related objects and lets you control permissions at the schema level.

```sql
CREATE SCHEMA Sales;
CREATE TABLE Sales.Orders (Id INT);
SELECT * FROM Sales.Orders;
```

Default schema is `dbo`.

---

**Q45. What is the difference between INNER JOIN and SELF JOIN?**

`SELF JOIN` is not a different join type — it's an `INNER JOIN` (or any join) where a table is joined to itself, typically to traverse relationships within the same table (e.g., employee-manager).

```sql
SELECT e.Name AS Employee, m.Name AS Manager
FROM Employees e JOIN Employees m ON e.ManagerId = m.Id;
```

---

**Q46. What is the difference between CROSS APPLY and OUTER APPLY?**

`APPLY` lets you call a table-valued function or correlated subquery for each row of the outer table.

- **CROSS APPLY** — like INNER JOIN; rows with no match are dropped.
- **OUTER APPLY** — like LEFT JOIN; rows with no match are kept (NULLs from the right).

```sql
SELECT c.customername, p.productname
FROM Customers c
CROSS APPLY (SELECT TOP 1 productname FROM Products WHERE productbuyerid = c.customerid) p;
```

---

**Q47. What is SQL Injection and how do you prevent it?**

SQL injection is when malicious input is inserted into a SQL query to manipulate the database.

Vulnerable: `"SELECT * FROM Users WHERE Name = '" + input + "'"`
Attacker passes `' OR '1'='1` and gets all rows.

Prevention:
- Use **parameterized queries** or stored procedures — never concatenate user input.
- Use an **ORM** like Entity Framework (parameterizes automatically).
- Validate and sanitize inputs.
- Apply **least privilege** on the DB account.

```sql
EXEC sp_executesql N'SELECT * FROM Users WHERE Name = @Name', N'@Name NVARCHAR(100)', @Name = @input;
```

---

**Q48. What is the difference between ExecuteReader, ExecuteScalar, and ExecuteNonQuery in ADO.NET?**

- **ExecuteReader** — runs a `SELECT` and returns a `SqlDataReader` for reading multiple rows.
- **ExecuteScalar** — runs a query and returns a **single value** (first column, first row). Good for `COUNT`, `SUM`.
- **ExecuteNonQuery** — runs `INSERT`, `UPDATE`, `DELETE`, or DDL. Returns the **number of rows affected**.

---

**Q49. What are the differences between BCP, BULK INSERT, and OPENROWSET for bulk loading?**

- **BCP** — command-line utility, fast import/export between files and tables.
- **BULK INSERT** — T-SQL statement that loads a file into a table.
- **OPENROWSET(BULK ...)** — table-valued function; loads file data as a rowset for queries.

For very large loads, use minimally-logged BULK INSERT with `TABLOCK` and bulk-logged recovery model.

---

**Q50. What is the order of execution of a SELECT statement?**

Logical order (not the order you write it):

1. `FROM` — pick tables, apply joins
2. `WHERE` — filter rows
3. `GROUP BY` — group rows
4. `HAVING` — filter groups
5. `SELECT` — pick columns / compute expressions
6. `DISTINCT`
7. `ORDER BY` — sort
8. `TOP / OFFSET-FETCH` — limit

This is why you can't reference a column alias defined in `SELECT` inside `WHERE` — `WHERE` runs first.

---

# Part D — 50 SQL Query Questions (Beginner → Intermediate)

> All queries run against the PG schema (Customers, Products, Transactions, Credentials).

---

## Beginner (Q1–Q20)

---

**Q1. Retrieve all customers.**

```sql
select * from Customers
```

---

**Q2. Get only customer name and expense columns.**

```sql
select customername, expense from Customers
```

---

**Q3. Get distinct expense values from Customers.**

```sql
select distinct expense from Customers
```

---

**Q4. Find the customer with the highest expense.**

```sql
select top 1 * from Customers order by expense desc
```

---

**Q5. Find products with price greater than 70.**

```sql
select * from Products where productprice > 70
```

---

**Q6. Find customers whose name starts with 'R'.**

```sql
select * from Customers where customername like 'R%'
```

---

**Q7. Find customers whose expense is between 60 and 90 (inclusive).**

```sql
select * from Customers where expense between 60 and 90
```

---

**Q8. Find customers whose expense is one of 50, 70, or 90.**

```sql
select * from Customers where expense in (50, 70, 90)
```

---

**Q9. Count the total number of customers.**

```sql
select count(*) as TotalCustomers from Customers
```

---

**Q10. Get the minimum and maximum product price.**

```sql
select min(productprice) as MinPrice,
       max(productprice) as MaxPrice
from Products
```

---

**Q11. Get the total expense across all customers.**

```sql
select sum(expense) as TotalExpense from Customers
```

---

**Q12. Get the average expense per customer.**

```sql
select avg(expense) as AvgExpense from Customers
```

---

**Q13. List customers ordered by expense descending.**

```sql
select * from Customers order by expense desc
```

---

**Q14. Find customers registered in 2023.**

```sql
select * from Customers where year(createdate) = 2023
```

---

**Q15. Find the most recently registered customer.**

```sql
select top 1 * from Customers order by createdate desc
```

---

**Q16. List customers along with the products they purchased (INNER JOIN).**

```sql
select c.customername, p.productname, p.productprice
from Customers c
inner join Products p on c.customerid = p.productbuyerid
```

---

**Q17. List ALL customers, including those without any product (LEFT JOIN).**

```sql
select c.customername, p.productname
from Customers c
left join Products p on c.customerid = p.productbuyerid
```

---

**Q18. Find customers who have NOT bought any product.**

```sql
select c.*
from Customers c
left join Products p on c.customerid = p.productbuyerid
where p.productid is null
```

---

**Q19. Get total revenue per product name.**

```sql
select productname, sum(productprice) as TotalRevenue
from Products
group by productname
```

---

**Q20. List product names that have more than one unit.**

```sql
select productname, count(*) as Units
from Products
group by productname
having count(*) > 1
```

---

## Intermediate (Q21–Q50)

---

**Q21. Find customers whose expense is above the average expense.**

```sql
select * from Customers
where expense > (select avg(expense) from Customers)
```

---

**Q22. Find the 2nd highest expense (without TOP).**

```sql
select max(expense) as SecondHighest
from Customers
where expense < (select max(expense) from Customers)
```

---

**Q23. Find the Nth highest expense using DENSE_RANK (N = 3).**

```sql
with Ranked as (
    select expense,
           dense_rank() over (order by expense desc) as rnk
    from Customers
)
select distinct expense from Ranked where rnk = 3
```

---

**Q24. Categorize customers by expense (High / Medium / Low) using CASE.**

```sql
select customername,
       expense,
       case
           when expense >= 100 then 'High'
           when expense >= 70  then 'Medium'
           else                     'Low'
       end as Category
from Customers
```

---

**Q25. Show customers with a running total of expenses ordered by customerid.**

```sql
select customername,
       expense,
       sum(expense) over (order by customerid
                          rows unbounded preceding) as RunningTotal
from Customers
```

---

**Q26. Show each customer's expense and the previous customer's expense using LAG.**

```sql
select customername,
       expense,
       lag(expense, 1, 0) over (order by customerid) as PreviousExpense
from Customers
```

---

**Q27. Rank products by price descending using RANK, DENSE_RANK, ROW_NUMBER.**

```sql
select productname,
       productprice,
       rank()       over (order by productprice desc) as rnk,
       dense_rank() over (order by productprice desc) as dense_rnk,
       row_number() over (order by productprice desc) as row_num
from Products
```

---

**Q28. Show customers with row numbers per expense group.**

```sql
select row_number() over (partition by expense order by customerid) as rn,
       customername, expense
from Customers
```

---

**Q29. Find customers who share the same expense amount (duplicates).**

```sql
select expense, count(*) as Count
from Customers
group by expense
having count(*) > 1
```

---

**Q30. Total expense per customer, sorted highest first.**

```sql
select customername, sum(expense) as TotalExpense
from Customers
group by customername
order by TotalExpense desc
```

---

**Q31. Get the customer count per month in 2023.**

```sql
select month(createdate) as Month,
       count(*)           as NewCustomers
from Customers
where year(createdate) = 2023
group by month(createdate)
order by Month
```

---

**Q32. Find products that no customer has purchased.**

```sql
select p.*
from Products p
left join Customers c on p.productbuyerid = c.customerid
where c.customerid is null
```

---

**Q33. Get top 2 most expensive products per buyer.**

```sql
select * from (
    select productname, productbuyerid, productprice,
           dense_rank() over (partition by productbuyerid
                              order by productprice desc) as rnk
    from Products
) t
where rnk <= 2
```

---

**Q34. Find all transactions that have no sender or receiver (still 'Empty').**

```sql
select * from Transactions
where transactionto = 'Empty' or transactionfrom = 'Empty'
```

---

**Q35. Use COALESCE and ISNULL to handle NULLs on Customer name.**

```sql
select customername,
       isnull(customername, 'Unknown')         as WithIsNull,
       coalesce(null, customername, 'Default') as WithCoalesce
from Customers
```

---

**Q36. Format expense and date using CAST and CONVERT.**

```sql
select customername,
       cast(expense as int)                            as IntExpense,
       '$' + cast(cast(expense as int) as varchar)     as FormattedExpense,
       convert(varchar, createdate, 103)               as DateDMY
from Customers
```

---

**Q37. UPSERT — update customer if exists, insert if not.**

```sql
declare @uid   int         = 15
declare @uname varchar(50) = 'Omkar'
declare @uex   money       = 23

if ((select count(*) from Customers where customerid = @uid) = 1)
    update Customers
    set customername = @uname, expense = @uex
    where customerid = @uid
else
    insert into Customers values (@uid, @uname, @uex, getdate())
```

---

**Q38. Write a stored procedure to insert a customer.**

```sql
create procedure sp_AddCustomer
    @cid   int,
    @cname varchar(20),
    @ex    money,
    @date  date
as
begin
    insert into Customers values (@cid, @cname, @ex, @date)
end

execute sp_AddCustomer @cid=30, @cname='Vikram', @ex=75, @date='2024-01-01'
```

---

**Q39. Stored procedure to return the Nth highest product price.**

```sql
create procedure sp_NthHighest
    @nth int
as
begin
    with cte as (
        select productprice,
               dense_rank() over (order by productprice desc) as rnk
        from Products
    )
    select distinct productprice from cte where rnk = @nth
end

execute sp_NthHighest 2
```

---

**Q40. Transaction with TRY/CATCH that rolls back on error.**

```sql
begin try
    begin transaction
        insert into Customers values (99,'TxnTest',50,'2024-01-01')
        -- simulate error: duplicate PK
        insert into Customers values (99,'Duplicate',60,'2024-01-01')
    commit transaction
end try
begin catch
    rollback transaction
    select error_message() as ErrorMsg
end catch
```

---

**Q41. Create a view for high spenders (expense > 80).**

```sql
create view vw_HighSpenders as
    select customername, expense
    from Customers
    where expense > 80

select * from vw_HighSpenders
```

---

**Q42. CTE to list customers above average expense.**

```sql
with AvgCTE as (
    select avg(expense) as AvgExp from Customers
)
select c.*
from Customers c
cross join AvgCTE a
where c.expense > a.AvgExp
```

---

**Q43. AFTER-INSERT trigger that logs the new customer id.**

```sql
create trigger trig_LogInsert
on Customers
after insert
as
begin
    declare @newid int
    select @newid = customerid from inserted
    print 'New customer added with ID: ' + cast(@newid as varchar)
end
```

---

**Q44. Delete duplicate products keeping the one with the highest productid.**

```sql
delete from Products
where productid not in (
    select max(productid)
    from Products
    group by productname, productbuyerid, productprice
)
```

---

**Q45. Add an `email` column to Customers and then drop it.**

```sql
alter table Customers add email varchar(100)
alter table Customers drop column email
```

---

**Q46. Create a clustered index on expense and a non-clustered index on customername.**

```sql
create clustered index    IX_C_Expense on Customers(expense)
create nonclustered index IX_NC_Name   on Customers(customername)

drop index IX_C_Expense on Customers
drop index IX_NC_Name   on Customers
```

---

**Q47. Recreate the Products FK with ON DELETE CASCADE.**

```sql
alter table Products drop constraint FK_buyer
alter table Products
    add constraint FK_buyer_cascade
    foreign key (productbuyerid) references Customers(customerid)
    on delete cascade
    on update cascade
```

---

**Q48. Pivot — show total expense per customer as columns.**

```sql
select
    sum(case when customername = 'Rahul1' then expense else 0 end) as Rahul1,
    sum(case when customername = 'Priya'  then expense else 0 end) as Priya,
    sum(case when customername = 'Gaurav' then expense else 0 end) as Gaurav
from Customers
```

---

**Q49. Compare SCOPE_IDENTITY, @@IDENTITY, and IDENT_CURRENT after a Credentials insert.**

```sql
insert into Credentials values ('newuser','pass123')
select scope_identity()            as ScopeIdentity,
       @@identity                  as AtAtIdentity,
       ident_current('Credentials') as IdentCurrent
```

---

**Q50. Comma-separated list of product names per buyer using STRING_AGG (SQL Server 2017+).**

```sql
select productbuyerid,
       string_agg(productname, ', ') within group (order by productname) as Products
from Products
group by productbuyerid
```

---

