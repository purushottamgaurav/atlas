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

| DBMS | RDBMS |
|------|--------|
| Stores data as files or standalone tables. | Stores data in related tables. |
| No relationships between tables. | Uses **Primary Keys** and **Foreign Keys** for relationships. |
| Does not enforce referential integrity. | Enforces referential integrity. |
| Limited transaction support. | Supports **ACID** transactions. |
| Best for small/single-user applications. | Best for large, multi-user enterprise applications. |
| **Examples:** FoxPro, dBase | **Examples:** SQL Server, Oracle, PostgreSQL, MySQL |

---

**Q3. What is the difference between DELETE, TRUNCATE, and DROP?**

- **DELETE** — removes rows one by one, can use `WHERE`, fully logged, can be rolled back.
- **TRUNCATE** — removes all rows at once, no `WHERE`, minimal logging, much faster, resets identity, can be rolled back inside an explicit transaction in SQL Server.
- **DROP** — removes the entire table and all its objects (indexes, constraints, triggers).

Use DELETE when you need conditions or rollback. Use TRUNCATE to wipe a table fast. Use DROP when you want the table gone completely.

---

**Q4. What is the difference between WHERE and HAVING?**

| **WHERE** | **HAVING** |
|-----------|------------|
| Filters **rows** before grouping. | Filters **groups** after `GROUP BY`. |
| Applied **before** aggregation. | Applied **after** aggregation. |
| **Cannot** use aggregate functions (`SUM`, `COUNT`, `AVG`, etc.). | **Can** use aggregate functions. |
| Can be used with or without `GROUP BY`. | Typically used with `GROUP BY`. |

```sql
SELECT Expense, COUNT(*) AS TotalCustomers
FROM Customers
WHERE Expense > 25000          -- Filters rows first
GROUP BY Expense
HAVING COUNT(*) > 5;           -- Filters groups after
```

---

**Q5. What is the difference between UNION, UNION ALL, INTERSECT, and EXCEPT?**

- **UNION** — combines two queries and removes duplicates. Slower (needs sort/hash).
- **UNION ALL** — combines two queries and keeps duplicates. Faster.
- **INTERSECT** — returns only rows that appear in **both** result sets.
- **EXCEPT** — returns rows from the first query that are **not** in the second.

All require matching column counts and compatible data types.

---

**Q6. What are ACID properties?**

- **Atomicity** – Either all operations in a transaction succeed or none do.  
  **Example:** During a bank transfer, money is debited from Account A and credited to Account B. If the credit fails, the debit is rolled back.

- **Consistency** – A transaction always leaves the database in a valid state by enforcing all rules and constraints.  
  **Example:** An order cannot be created for a customer that doesn't exist (Foreign Key constraint).

- **Isolation** – Concurrent transactions execute independently without affecting each other.  
  **Example:** While User A is updating a product's price, User B cannot read the uncommitted new price.

- **Durability** – Once a transaction is committed, the changes are permanently saved.  
  **Example:** After an order is successfully placed and committed, it remains in the database even if the server crashes immediately afterward.

---

**Q7. What is the difference between a Stored Procedure and a Function?**

| Feature | Stored Procedure | Function |
|---|---|---|
| Returns | Optional (output params) | Must return a value |
| DML (INSERT/UPDATE/DELETE) | Yes | No (scalar functions) |
| Use in SELECT | No | Yes |
| Transactions | Yes | No |
| Exception handling | Yes | Limited |

Use functions for calculations used inside queries. Use stored procedures for business logic and data modifications.

---

**Q8. What is normalization? Explain 1NF, 2NF, 3NF, and BCNF.**

Normalization organizes tables to reduce data redundancy and improve integrity.

- **1NF** — each column holds atomic (single) values. No repeating groups. Each row is unique.
- **2NF** — meets 1NF + every non-key column depends on the **whole** primary key (no partial dependency).
- **3NF** — meets 2NF + no non-key column depends on another non-key column (no transitive dependency).
- **BCNF** — stricter version of 3NF. Every determinant must be a candidate key.

---

**Q9. What is denormalization and when do you use it?**

Denormalization deliberately introduces redundancy into a normalized database to improve read performance. You duplicate data or pre-compute joins so queries run faster — at the cost of more storage and more complex writes.

Use it for:
- Read-heavy reporting/analytics tables.
- Data warehouses (star/snowflake schemas).
- When normalized joins are too slow to meet SLAs.

---

**Q10. What are SQL Constraints?**

Constraints enforce rules on data in a table:

- **PRIMARY KEY** — uniquely identifies each row. No NULLs. One per table.
- **FOREIGN KEY** — enforces referential integrity between two tables.
- **UNIQUE** — all values in a column must be different. Allows one NULL.
- **CHECK** — ensures values meet a condition (e.g., `Age > 0`).
- **DEFAULT** — sets a default value when none is provided.
- **NOT NULL** — column cannot store NULL.

---

**Q11. What is the difference between Primary Key and Unique Key?**

| Feature | Primary Key | Unique Key |
|---|---|---|
| NULLs allowed | No | Yes (one NULL) |
| Per table | Only one | Multiple allowed |
| Creates index | Clustered (default) | Non-clustered |
| Purpose | Row identifier | Enforce uniqueness |

---

**Q12. What is a Foreign Key and what is referential integrity?**

A **Foreign Key** is a column in one table that references the primary key of another table, enforcing **referential integrity** — you can't insert a child row pointing to a non-existent parent.

Cascade options on FKs:
- `ON DELETE CASCADE` — deleting parent deletes children.
- `ON UPDATE CASCADE` — updating parent PK updates children.
- `ON DELETE SET NULL` — children's FK set to NULL.
- `ON DELETE NO ACTION` — block delete if children exist (default).

---

**Q13. What are the different types of JOINs?**

- **INNER JOIN** — rows with a match in both tables.
- **LEFT JOIN** — all rows from left, matching from right (NULL if no match).
- **RIGHT JOIN** — all rows from right, matching from left.
- **FULL OUTER JOIN** — all rows from both, NULLs where no match.
- **CROSS JOIN** — cartesian product. Every row × every row. No join condition.
- **SELF JOIN** — a table joined with itself. Used for hierarchical data like employee-manager.

---

**Q14. What is the difference between EXISTS and IN?**

- **IN** compares the column to a list/subquery of values.
- **EXISTS** checks whether the subquery returns at least one row; doesn't care about the value.

```sql
SELECT *
FROM Customers
WHERE CustomerId IN (
    SELECT ProductBuyerId
    FROM Products
);
```

```sql
SELECT *
FROM Customers c
WHERE EXISTS (
    SELECT 1
    FROM Products p
    WHERE p.ProductBuyerId = c.CustomerId
);
```

`EXISTS` handles NULLs better than `NOT IN`.

---

**Q15. What is the difference between a Subquery and a Correlated Subquery?**


| **Subquery** | **Correlated Subquery** |
|--------------|-------------------------|
| Runs **once**. | Runs **once per outer row**. |
| Independent of the outer query. | References the outer query. |
| Faster. | Slower. |


```sql
SELECT *
FROM Customers
WHERE Expense > (
    SELECT AVG(Expense)
    FROM Customers
);
```

> `AVG(Expense)` is calculated **once**.

```sql
SELECT *
FROM Customers c
WHERE c.Expense > (
    SELECT AVG(Expense)
    FROM Customers
    WHERE CustomerId <= c.CustomerId
);
```

> The inner query runs **for each customer**.

---

**Q16. What is a CTE and when would you use it over a Subquery?**

A **CTE (Common Table Expression)** is a named temporary result set defined with `WITH` at the top of a query.

Use CTE when:
- The same subquery logic is reused multiple times.
- You need **recursive** queries (e.g., org charts, hierarchies).
- You want better readability.

Use a subquery for simple one-off inline filtering.

---

**Q17. What is a Recursive CTE?**

A **Recursive CTE** is a CTE that **calls itself** to retrieve hierarchical data like **employee-manager**, **folders**, or **categories**.

It has **2 parts**:
- **Anchor Member** – Starting point (e.g., CEO).
- **Recursive Member** – Repeatedly finds the next level using `UNION ALL`.

```sql
WITH OrgChart AS (
    -- Anchor
    SELECT Id, ManagerId
    FROM Employees
    WHERE ManagerId IS NULL

    UNION ALL

    -- Recursive
    SELECT e.Id, e.ManagerId
    FROM Employees e
    JOIN OrgChart oc ON e.ManagerId = oc.Id
)
SELECT * FROM OrgChart;
```

---

**Q18. What are Window Functions? Explain RANK, DENSE_RANK, ROW_NUMBER.**

Window functions perform calculations across a set of rows related to the current row, without collapsing rows like `GROUP BY`.

- **ROW_NUMBER()** — unique sequential number. No ties. 1,2,3,4.
- **RANK()** — same rank for ties, then skips numbers. 1,1,3,4.
- **DENSE_RANK()** — same rank for ties, no gaps. 1,1,2,3.

Other useful ones: `LAG`, `LEAD`, `NTILE`, `SUM/AVG OVER (...)`.

---

**Q19. What is PIVOT and UNPIVOT?**

- **PIVOT** — rotates row data into columns (e.g., monthly totals as columns).
- **UNPIVOT** — does the reverse, columns into rows.

```sql
SELECT *
FROM Products
PIVOT (
    SUM(ProductPrice)
    FOR ProductBuyerId IN ([1], [2])
) p;
```

```sql
SELECT *
FROM BuyerProductPrice
UNPIVOT (
    ProductPrice
    FOR ProductBuyerId IN ([1], [2])
) u;
```

---

**Q20. What is the difference between CHAR, VARCHAR, NCHAR, NVARCHAR?**

- **CHAR(n)** — fixed length, non-Unicode. Always uses n bytes.
- **VARCHAR(n)** — variable length, non-Unicode. Uses only what's needed.
- **NCHAR(n)** — fixed length, Unicode. 2 bytes per character.
- **NVARCHAR(n)** — variable length, Unicode. Use for international text.

Use `VARCHAR` for English-only data, `NVARCHAR` for any language. Prefix Unicode strings with `N`: `N'हिंदी'`.

---

**Q21. What is COALESCE() and how is it different from ISNULL()?**

`COALESCE()` returns the first non-NULL value from a list of expressions.

```sql
SELECT COALESCE(MiddleName, NickName, 'N/A') FROM Employees;
```

- **COALESCE** — ANSI standard, takes any number of arguments, return type based on highest precedence.
- **ISNULL** — SQL Server-specific, takes exactly two arguments, return type matches the first.

---

**Q22. What is the difference between Local and Global Temporary Tables?**

| **Local Temp Table (`#`)** | **Global Temp Table (`##`)** |
|----------------------------|------------------------------|
| Visible only to the **current session**. | Visible to **all sessions**. |
| Created with a single `#`. | Created with a double `##`. |
| Automatically dropped when the **creating session ends**. | Dropped when the **last session using it ends**. |
| Used for session-specific temporary data. | Used to share temporary data across sessions. |

Both are physically stored in `tempdb`.

---

**Q23. What is the difference between Temp Tables and Table Variables?**

| Feature | Temp Table (`#table`) | Table Variable (`@table`) |
|---|---|---|
| Stored in | tempdb | tempdb |
| Indexes | Yes (additional) | Only PK/unique |
| Scope | Session | Current batch only |
| TRUNCATE | Yes | No |
| Statistics | Yes | No |
| Recompilation | More likely | Less likely |

Use temp tables for large datasets or when you need indexes/stats. 
Use table variables for small, short-lived data in a batch.

---

**Q24. What is UPSERT and how do you do it in SQL Server?**

UPSERT = "insert if the row doesn't exist, update if it does." In SQL Server, use the `MERGE` statement.

```sql
MERGE INTO Customers AS target
USING (
    SELECT 1 AS CustomerId,
           'John' AS CustomerName,
           5000 AS Expense,
           GETDATE() AS CreateDate
) AS source
ON target.CustomerId = source.CustomerId

WHEN MATCHED THEN
    UPDATE SET
        target.CustomerName = source.CustomerName,
        target.Expense = source.Expense,
        target.CreateDate = source.CreateDate

WHEN NOT MATCHED THEN
    INSERT (CustomerId, CustomerName, Expense, CreateDate)
    VALUES (source.CustomerId, source.CustomerName, source.Expense, source.CreateDate);
```

---

**Q25. What is a View and when would you use it?**

A view is a saved SQL query that acts like a virtual table. It doesn't store data itself — it runs the underlying query each time.

Use views to:
- Simplify complex queries for reuse.
- Restrict access to specific columns/rows for security.
- Present data in a business-friendly format.

```sql
CREATE VIEW CustomerDetails AS
SELECT CustomerId,
       CustomerName,
       Expense,
       CreateDate
FROM Customers;
```

---

**Q26. What is an Indexed View / Materialized View?**


| **Regular View** | **Indexed View** |
|------------------|------------------|
| Stores only the SQL query. | Stores the query result on disk. |
| Executes the query every time it is accessed. | Reads precomputed data, so queries are faster. |
| No extra storage. | Requires additional storage. |
| Faster updates. | Slower updates because the view must also be updated. |

```sql
CREATE VIEW dbo.vw_ProductSales
WITH SCHEMABINDING
AS
SELECT
    ProductBuyerId,
    SUM(ProductPrice) AS TotalAmount,
    COUNT_BIG(*) AS ProductCount
FROM dbo.Products
GROUP BY ProductBuyerId;
GO

CREATE UNIQUE CLUSTERED INDEX IX_vw_ProductSales
ON dbo.vw_ProductSales (ProductBuyerId);
```

---

**Q27. What are Triggers and what types exist?**

Triggers are stored procedures that automatically run when a specific event happens.

- **AFTER trigger** — runs after `INSERT`, `UPDATE`, or `DELETE`. Used for auditing, logging.
- **INSTEAD OF trigger** — replaces the operation. Useful on views.
- **DDL trigger** — fires on schema changes (`CREATE`, `ALTER`, `DROP`).
- **Logon trigger** — fires when a user logs into SQL Server.

Triggers have access to `INSERTED` and `DELETED` virtual tables with new and old row values.

---

**Q28. What is a Cursor and when should you use one?**

A cursor lets you process rows one at a time, like an iterator. Useful for row-by-row logic that can't be expressed in set-based SQL — but **avoid cursors when a set-based query works**, they are much slower.

```sql
DECLARE @CustomerId INT;

DECLARE CustomerCursor CURSOR FOR
SELECT CustomerId
FROM Customers;

OPEN CustomerCursor;

FETCH NEXT FROM CustomerCursor INTO @CustomerId;

WHILE @@FETCH_STATUS = 0
BEGIN
    PRINT 'Processing Customer: ' + CAST(@CustomerId AS VARCHAR);

    FETCH NEXT FROM CustomerCursor INTO @CustomerId;
END

CLOSE CustomerCursor;
DEALLOCATE CustomerCursor;
```

---

**Q29. What is a Transaction and why is it needed?**

A transaction is a group of SQL statements that must all succeed or all fail together. Needed to keep data consistent — e.g., transferring money: debit one account and credit another must both happen or neither.

```sql
BEGIN TRY
    BEGIN TRANSACTION;

    UPDATE Accounts
    SET Balance = Balance - 500
    WHERE Id = 1;

    UPDATE Accounts
    SET Balance = Balance + 500
    WHERE Id = 2;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH;
```

---

**Q30. What are the Isolation Levels in SQL Server?**

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

**Q31. What is a Deadlock and how do you prevent it?**

A deadlock happens when two transactions hold locks the other needs, and neither can proceed. SQL Server detects deadlocks and kills the lower-priority one (the "deadlock victim", error 1205).

Prevention:
- Always access tables in the **same order** across transactions.
- Keep transactions **short**.
- Use **lower isolation levels** where safe.
- Add proper **indexes** to reduce locking scope.
- Retry the victim transaction in app code.

---

**Q32. What is Optimistic vs Pessimistic Concurrency?**

- **Pessimistic** — locks rows when read so no one else can modify them. Safe but blocks readers/writers. Use when conflicts are frequent.
- **Optimistic** — no locks; checks at save time whether the row was modified since it was read (using a `RowVersion` / `Timestamp`). If yes, rejects with concurrency error. Use when conflicts are rare (most web apps).

#### Pessimistic (Locks the row)

```sql
SELECT *
FROM Customers
WITH (UPDLOCK)
WHERE CustomerId = 1;
```

#### Optimistic (Uses `ROWVERSION`)

```sql
UPDATE Customers
SET Expense = 5000
WHERE CustomerId = 1
  AND RowVersion = @OldRowVersion;
```

---

**Q33. What is the NOLOCK hint and when should you avoid it?**

`WITH (NOLOCK)` is equivalent to `READ UNCOMMITTED` for a single table — it ignores locks and may return **dirty data** (uncommitted, may be rolled back), missing rows, or duplicate rows.

Use sparingly for reports that tolerate inaccuracy. **Never use** on financial, billing, or audit data.

```sql
SELECT *
FROM Customers
WITH (NOLOCK);
```

---

**Q34. What is Query Optimization? Name common techniques.**

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

**Q35. What is a SARGable query?**

**SARGable (Search ARGument Able)** means a query is written so SQL Server can **use an index efficiently**. This usually means comparing the **column directly** instead of applying functions to it.

❌ **Non-SARGable** (can't efficiently use the index)

```sql
SELECT *
FROM Customers
WHERE YEAR(CreateDate) = 2023;
```

✅ **SARGable** (can use the index)

```sql
SELECT *
FROM Customers
WHERE CreateDate >= '2023-01-01'
  AND CreateDate < '2024-01-01';
```

---

**Q36. What is database indexing and what are the upsides and downsides of too many indexes?**

A **database index** is a data structure that helps SQL Server **find rows faster** without scanning the entire table. It is similar to an **index in a book**—instead of reading every page, you jump directly to the required information.

#### Advantages

- **Faster `SELECT` queries**.
- Improves performance of **`WHERE`**, **`JOIN`**, **`ORDER BY`**, and **`GROUP BY`** operations.
- Reduces full table scans by enabling **index seeks**.

#### Disadvantages of Too Many Indexes

- **Slower writes** – Every `INSERT`, `UPDATE`, and `DELETE` must update all related indexes.
- **More storage** – Each index consumes additional disk space.
- **Higher maintenance** – Indexes can become fragmented and require rebuilding or reorganizing.

#### Best Practices

- ✅ Create indexes on columns frequently used in **`WHERE`**, **`JOIN`**, **`ORDER BY`**, and **`GROUP BY`**.
- ❌ Avoid indexing every column.
- ❌ Remove unused or duplicate indexes.

---
**Q37. What is the difference between Clustered and Non-Clustered Index?**

| **Clustered Index** | **Non-Clustered Index** |
|----------------------|-------------------------|
| Physically stores table data in the index order. | Stores only the index; points to the actual data rows. |
| Only **one** clustered index per table. | Multiple non-clustered indexes can exist on a table. |
| Faster for **range queries**, sorting, and ordered retrieval. | Faster for **searching**, filtering (`WHERE`), and `JOIN`s. |
| Usually created on the **Primary Key** by default (unless specified otherwise). | Created on frequently searched columns. |


Think of clustered index as the book itself (sorted), non-clustered as the index at the back of the book (pointers).

---

**Q38. What is a Composite Index? When to use it?**

A **Composite Index** is an index on **multiple columns**. It speeds up queries that use those columns together.

> **Column order matters.** SQL Server efficiently uses the index only when the **leading (leftmost) column** is included in the query.

```sql
CREATE INDEX IX_Products_BuyerPrice
ON Products(ProductBuyerId, ProductPrice);
```

- ✅ `WHERE ProductBuyerId = 1`
- ✅ `WHERE ProductBuyerId = 1 AND ProductPrice > 1000`
- ❌ `WHERE ProductPrice > 1000`

> Follows the **leftmost prefix rule**.

---

**Q39. What is the difference between @@IDENTITY, SCOPE_IDENTITY(), and IDENT_CURRENT()?**


| Function | Returns | Use Case |
|----------|---------|----------|
| **@@IDENTITY** | Last identity generated in the **current session**, including **triggers**. | Rarely used; can return unexpected values if triggers insert into other tables. |
| **SCOPE_IDENTITY()** | Last identity generated in the **current scope** (same procedure/batch). | ✅ **Preferred** after an `INSERT` to get the ID you just inserted. |
| **IDENT_CURRENT('Table')** | Last identity generated for a **specific table**, regardless of session or scope. | Used to check the latest identity value of a table (not safe for retrieving your inserted row). |

```sql
INSERT INTO Credentials (CredUsername, CredPassword)
VALUES ('john', 'pass123');

SELECT @@IDENTITY;
SELECT SCOPE_IDENTITY();            -- Current scope
SELECT IDENT_CURRENT('Credentials'); -- Last identity in table
```

---

**Q40. What is a Composite Key vs Surrogate Key?**

| **Composite Key** | **Surrogate Key** |
|-------------------|-------------------|
| A **Primary Key** made of **two or more columns**. | An **artificial single-column key** (e.g., `IDENTITY` or `UUID`) with no business meaning. |
| Uses existing business data to uniquely identify a row. | Used only to uniquely identify a row. |
| Can make joins and foreign keys more complex. | Simpler joins and easier maintenance. |
| **Example:** `(OrderId, ProductId)` in `OrderItems`. | **Example:** `OrderItemId` as an `IDENTITY` column. |

```sql
-- Composite Key
PRIMARY KEY (ProductId, ProductBuyerId)
```

```sql
-- Surrogate Key
CredId INT IDENTITY PRIMARY KEY
```

---

**Q41. What is the difference between CASE and IIF?**

Both return one value from multiple options.

```sql
-- CASE
SELECT CASE
         WHEN Expense > 5000 THEN 'VIP'
         ELSE 'Regular'
       END
FROM Customers;
```

```sql
-- IIF
SELECT IIF(Expense > 5000, 'VIP', 'Regular')
FROM Customers;
```

Use `CASE` for more than two outcomes; `IIF` for quick binary checks.

---

**Q42. What is the GO statement?**


`GO` is **not a SQL command**. It is a **batch separator** used by SQL Server tools (SSMS, `sqlcmd`) to execute the previous statements as one batch.

```sql
USE MyDatabase;
GO

CREATE TABLE Employees (Id INT);
GO
```

#### Advantages
- Separates SQL scripts into batches.
- `GO n` executes the previous batch **n** times.

#### Disadvantages
- Not recognized by ADO.NET or SQL Server itself.
- Variables declared before `GO` cannot be used after it.

---

**Q43. What are the system databases in SQL Server?**

- **master** — instance-level info: logins, linked servers, settings.
- **model** — template for new databases (any object here is copied to new DBs).
- **msdb** — SQL Agent jobs, schedules, backup history.
- **tempdb** — temp tables, table variables, intermediate sorts. Recreated on every restart.
- **resource** — read-only DB with system objects (hidden).

---

**Q44. What is a Schema?**

A **Schema** is a **logical container (namespace)** that groups database objects like **tables, views, and stored procedures**.

```sql
CREATE SCHEMA Sales;

CREATE TABLE Sales.Orders (Id INT);
```

#### Benefits
- Organizes related objects.
- Avoids name conflicts.
- Simplifies permission management.

> **Default schema:** `dbo`

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
SELECT c.CustomerName, p.ProductName
FROM Customers c
CROSS APPLY (
    SELECT TOP 1 ProductName
    FROM Products
    WHERE ProductBuyerId = c.CustomerId
) p;
```

> Returns **only** customers with at least one matching product.

```sql
SELECT c.CustomerName, p.ProductName
FROM Customers c
OUTER APPLY (
    SELECT TOP 1 ProductName
    FROM Products
    WHERE ProductBuyerId = c.CustomerId
) p;
```
> Returns **all** customers. If no matching product exists, `ProductName` is `NULL`.

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
DECLARE @Name VARCHAR(40) = 'John';

EXEC sp_executesql
    N'SELECT * FROM Customers WHERE CustomerName = @Name',
    N'@Name VARCHAR(40)',
    @Name = @Name;
```

---

**Q48. What is the difference between ExecuteReader, ExecuteScalar, and ExecuteNonQuery in ADO.NET?**

| Method | Returns | Used For |
|--------|---------|----------|
| **ExecuteReader()** | `SqlDataReader` | Executes a `SELECT` query and reads **multiple rows** one at a time. |
| **ExecuteScalar()** | Single value (first row, first column) | Used when only **one value** is needed, such as `COUNT`, `SUM`, `MAX`, or an identity value. |
| **ExecuteNonQuery()** | Number of rows affected (`int`) | Executes `INSERT`, `UPDATE`, `DELETE`, and DDL statements like `CREATE` or `DROP`. |


```csharp
// ExecuteReader
var reader = cmd.ExecuteReader();    // Read multiple rows
```

```csharp
// ExecuteScalar
int count = (int)cmd.ExecuteScalar();   // e.g., SELECT COUNT(*)
```

```csharp
// ExecuteNonQuery
int rows = cmd.ExecuteNonQuery();       // e.g., UPDATE Employees...
```

---

**Q49. What are the differences between BCP, BULK INSERT, and OPENROWSET for bulk loading?**

- **BCP** — command-line utility, fast import/export between files and tables.
- **BULK INSERT** — T-SQL statement that loads a file into a table.
- **OPENROWSET(BULK ...)** — table-valued function; loads file data as a rowset for queries.

```sql
BULK INSERT Customers
FROM 'C:\Data\Customers.csv';
```

```sql
SELECT *
FROM OPENROWSET(
    BULK 'C:\Data\Customers.csv',
    FORMAT = 'CSV'
) AS C;
```

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

**Q41. Insert multiple rows in a transaction. If one insert fails, rollback only that part using a savepoint, not the whole transaction.**

```sql
BEGIN TRY
    BEGIN TRANSACTION

        INSERT INTO Customers VALUES (101, 'A', 100, '2024-01-01')
        SAVE TRANSACTION sp1

        INSERT INTO Customers VALUES (102, 'B', 200, '2024-01-01')
        SAVE TRANSACTION sp2

        -- this will fail
        INSERT INTO Customers VALUES (101, 'Duplicate', 300, '2024-01-01')

    COMMIT TRANSACTION

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION sp2

    SELECT ERROR_MESSAGE() AS ErrorMessage
END CATCH
```

---

**Q42. Create a view for high spenders (expense > 80).**

```sql
create view vw_HighSpenders as
    select customername, expense
    from Customers
    where expense > 80

select * from vw_HighSpenders
```

---

**Q43. CTE to list customers above average expense.**

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

**Q44. AFTER-INSERT trigger that logs the customer info in Audit table.**

```sql
CREATE TABLE CustomerAudit
(
    AuditId INT IDENTITY PRIMARY KEY,
    CustomerId INT,
    ActionType VARCHAR(20),
    ActionDate DATETIME
);

CREATE TRIGGER trg_Customer_Insert
ON Customers
AFTER INSERT
AS
BEGIN
    INSERT INTO CustomerAudit(CustomerId, ActionType, ActionDate)
    SELECT CustomerId,
           'INSERT',
           GETDATE()
    FROM inserted;
END;
```

---

**Q45. Delete duplicate products keeping the one with the highest productid.**

```sql
delete from Products
where productid not in (
    select max(productid)
    from Products
    group by productname, productbuyerid, productprice
)
```

---

**Q46. Add an `Email` column to `Customers` and make it unique.**

```sql
ALTER TABLE Customers
ADD Email VARCHAR(100);

ALTER TABLE Customers
ADD CONSTRAINT UQ_Customers_Email UNIQUE (Email);
```

---

**Q47. Create a filtered index for customers with expense greater than 5000.**

```sql
CREATE INDEX IX_Customers_HighExpense
ON Customers(Expense)
WHERE Expense > 5000;
```

---

**Q48. Recreate the Products FK with ON DELETE CASCADE.**

```sql
alter table Products drop constraint FK_buyer
alter table Products
    add constraint FK_buyer_cascade
    foreign key (productbuyerid) references Customers(customerid)
    on delete cascade
    on update cascade
```

---

**Q49. Display the total product price for each buyer as separate columns using `PIVOT`**

```sql
SELECT *
FROM (
    SELECT ProductBuyerId, ProductPrice
    FROM Products
) AS SourceTable
PIVOT (
    SUM(ProductPrice)
    FOR ProductBuyerId IN ([1], [2], [3])
) AS PivotTable;
```

---

**Q50. Insert a new credential and retrieve the generated identity value.**

```sql
INSERT INTO Credentials (CredUsername, CredPassword)
VALUES ('newuser', 'pass123');

SELECT SCOPE_IDENTITY() AS NewCredentialId;
```

---

>End Of Document

---

