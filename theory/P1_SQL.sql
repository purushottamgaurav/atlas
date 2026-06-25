-- ============================================================
--   SQL QUICK REFERENCE + MNC INTERVIEW QUESTIONS
--   Database: PG  |  SQL Server (T-SQL)
--   Schemas : Customers, Products, Transactions, Credentials
-- ============================================================
--  HOW TO USE
--    Ctrl+F  a section heading (e.g. "-- ## 3.") to jump fast.
--    Every live query uses the PG schema tables directly.
-- ============================================================

-- ============================================================
-- SECTION 0 : DATABASE & TABLE SETUP
-- ============================================================

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

-- ── Transactions ─────────────────────────────────────────────
create table Transactions (
    transactionid   int  primary key,
    trnsactiondate  date not null,
    transactionto   varchar(20) default 'Empty',
    transactionfrom varchar(20) default 'Empty'
)

-- ── Credentials ──────────────────────────────────────────────
create table Credentials (
    credid       int  identity primary key,   -- auto-increment
    credusername varchar(30) unique not null,
    credpassword varchar(30) not null
)

-- ── Sample DML ───────────────────────────────────────────────
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


-- ============================================================
-- ## 1.  SELECT  —  Basics
-- ============================================================

select * from Customers                          -- all rows & cols
select top 3 * from Customers                    -- first 3 rows
select customername, expense from Customers      -- specific cols
select distinct expense from Customers           -- unique values
select customername as Name, expense as Amount   -- column alias
    from Customers


-- ============================================================
-- ## 2.  WHERE  —  Filtering
-- ============================================================

-- Comparison operators
select * from Customers where expense > 70
select * from Customers where expense = 80
select * from Customers where expense <> 50      -- not equal

-- Logical operators  (AND / OR / NOT)
select * from Customers
    where expense > 70 or expense < 60 and not expense = 50

-- LIKE  — pattern matching
-- %  = zero or more chars    _  = exactly one char
select * from Customers where customername like 'R%'   -- starts with R
select * from Customers where customername like '%ul'  -- ends   with ul
select * from Customers where customername like '_r%'  -- 2nd char = r
select * from Customers where expense like '5_.%'      -- e.g. 5X.YZ

-- IN  — match a list
select * from Customers where expense in (50, 70, 90)

-- BETWEEN  — inclusive range
select * from Customers where expense between 20 and 90

-- IS NULL / IS NOT NULL
select * from Customers where customername is null
select * from Customers where customername is not null


-- ============================================================
-- ## 3.  ORDER BY  /  TOP  /  OFFSET-FETCH
-- ============================================================

select * from Customers order by expense desc         -- descending
select * from Customers order by expense asc          -- ascending (default)
select * from Customers order by expense desc, customername asc  -- multi-col

-- Pagination (SQL Server 2012+)
select * from Customers
    order by customerid
    offset 2 rows fetch next 3 rows only   -- skip 2, take next 3


-- ============================================================
-- ## 4.  INSERT  /  UPDATE  /  DELETE  /  TRUNCATE
-- ============================================================

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

-- DELETE  — row-level, logged, can be rolled back
delete from Customers where createdate = '2023-05-13'
delete from Customers where expense = 70

-- TRUNCATE  — removes all rows, faster, minimal logging
truncate table Transactions   -- cannot use WHERE


-- ============================================================
-- ## 5.  AGGREGATE FUNCTIONS  &  GROUP BY  /  HAVING
-- ============================================================

-- Aggregates
select count(*)          as TotalRows   from Customers
select count(expense)    as NonNullRows from Customers
select sum(expense)      as TotalSpend  from Customers
select avg(expense)      as AvgSpend    from Customers
select min(expense)      as MinSpend    from Customers
select max(expense)      as MaxSpend    from Customers

-- GROUP BY  (aggregate per group)
select productname, count(productid) as Qty, sum(productprice) as Revenue
    from Products
    group by productname

-- HAVING  — filter on aggregate  (WHERE cannot use aggregates)
select productname, count(productid) as Qty
    from Products
    group by productname, productprice
    having productprice > 80

-- GROUP BY + ORDER BY
select expense, count(*) as Cnt
    from Customers
    group by expense
    order by Cnt desc


-- ============================================================
-- ## 6.  JOINS
-- ============================================================

-- INNER JOIN  — only matching rows
select c.customername, p.productname, p.productprice
    from Customers c
    inner join Products p on c.customerid = p.productbuyerid

-- LEFT JOIN  — all from left, matching from right (NULL if no match)
select c.customername, p.productname
    from Customers c
    left join Products p on c.customerid = p.productbuyerid

-- RIGHT JOIN  — all from right, matching from left
select c.customername, p.productname
    from Customers c
    right join Products p on c.customerid = p.productbuyerid

-- FULL OUTER JOIN  — all rows from both sides
select c.customername, p.productname
    from Customers c
    full join Products p on c.customerid = p.productbuyerid

-- CROSS JOIN  — cartesian product (every row x every row)
select c.customername, p.productname
    from Customers c
    cross join Products p

-- SELF JOIN  — table joined with itself (e.g. manager hierarchy)
select a.customername as Customer, b.customername as Referrer
    from Customers a, Customers b
    where a.customerid = b.customerid

-- Old-style implicit join (avoid in new code, prefer explicit JOIN)
select c.customername, p.productprice
    from Customers c, Products p
    where c.customerid = p.productid


-- ============================================================
-- ## 7.  UNION  /  INTERSECT  /  EXCEPT
-- ============================================================

-- UNION      — combines results, removes duplicates
select customername as Name from Customers
union
select productname  as Name from Products

-- UNION ALL  — keeps duplicates (faster)
select customername as Name from Customers
union all
select productname as Name from Products

-- INTERSECT  — rows that appear in BOTH result sets
select customerid from Customers
intersect
select productbuyerid from Products

-- EXCEPT     — rows in first set but NOT in second
select customerid from Customers
except
select productbuyerid from Products


-- ============================================================
-- ## 8.  SUBQUERIES  /  EXISTS  /  CTE
-- ============================================================

-- Subquery in WHERE
select * from Customers
    where expense > (select avg(expense) from Customers)

-- Subquery in FROM  (derived table)
select * from (
    select customername, expense from Customers where expense > 60
) as HighSpenders

-- EXISTS  — true if subquery returns any row
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

-- CTE  (Common Table Expression)  — readable, reusable within query
with HighSpendersCTE as (
    select * from Customers where expense > 70
)
select * from HighSpendersCTE

-- CTE to find Nth highest price  (see also SP version in Section 9)
with RankedPrices as (
    select productprice,
           dense_rank() over (order by productprice desc) as rnk
    from Products
)
select productprice from RankedPrices where rnk = 2   -- 2nd highest


-- ============================================================
-- ## 9.  WINDOW FUNCTIONS
-- ============================================================

-- ROW_NUMBER — unique sequential number per partition
select row_number() over (partition by expense order by expense) as row_num,
       customername, expense, createdate
    from Customers

-- RANK       — gaps allowed when tied
select rank() over (order by expense desc) as rnk,
       customername, expense
    from Customers

-- DENSE_RANK — no gaps when tied
select dense_rank() over (order by expense desc) as dense_rnk,
       customername, expense
    from Customers

-- NTILE(n)   — divide rows into n buckets
select ntile(3) over (order by expense) as bucket,
       customername, expense
    from Customers

-- LAG / LEAD  — previous / next row value
select customername, expense,
       lag(expense,  1, 0) over (order by customerid) as PrevExpense,
       lead(expense, 1, 0) over (order by customerid) as NextExpense
    from Customers

-- Running total with SUM OVER
select customername, expense,
       sum(expense) over (order by customerid rows unbounded preceding) as RunningTotal
    from Customers


-- ============================================================
-- ## 10. DDL  —  ALTER TABLE
-- ============================================================

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


-- ============================================================
-- ## 11. VIEWS
-- ============================================================

-- Create view
create view CustomerMoneyView as
    select * from Customers where expense = 80

-- Use view
select * from CustomerMoneyView

-- Create or replace  (SQL Server uses ALTER VIEW)
alter view CustomerMoneyView as
    select * from Customers where expense >= 80

-- View with JOIN
create view CustomerProductView as
    select c.customername, p.productname, p.productprice
    from Customers c
    inner join Products p on c.customerid = p.productbuyerid

-- Drop view
drop view CustomerMoneyView


-- ============================================================
-- ## 12. STORED PROCEDURES
-- ============================================================

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

-- SP using CTE  — Nth highest product price
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


-- ============================================================
-- ## 13. TRANSACTIONS
-- ============================================================

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

-- TRY / CATCH pattern  (preferred modern approach)
begin try
    begin transaction
        insert into Customers values (12,'TryUser',55,'2023-08-01')
        update  Customers set expense = -1 where customerid = 12  -- will fail CHK
    commit transaction
end try
begin catch
    rollback transaction
    select error_message() as ErrorMsg, error_number() as ErrNo
end catch

-- SAVEPOINT  — partial rollback
begin transaction
    insert into Customers values (20,'A',10,'2024-01-01')
    save transaction SavePoint1
    insert into Customers values (21,'B',20,'2024-01-02')
    rollback transaction SavePoint1   -- undo only the 2nd insert
commit transaction


-- ============================================================
-- ## 14. TRIGGERS
-- ============================================================

-- AFTER INSERT trigger
create trigger trig_AfterInsertCustomer
on Customers
after insert
as
begin
    insert into Credentials values ('auto_user','auto_pass')
end

-- INSTEAD OF trigger  (fires instead of the DML)
create trigger trig_InsteadOfDelete
on Customers
instead of delete
as
begin
    print 'Direct delete is not allowed on Customers.'
end

-- Drop trigger
drop trigger trig_AfterInsertCustomer


-- ============================================================
-- ## 15. VARIABLES  /  CONTROL FLOW  /  TEMP OBJECTS
-- ============================================================

-- Variables
declare @id   int         = 15
declare @name varchar(50) = 'Omkar'
declare @age  int         = 23

-- IF / ELSE  — upsert pattern
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

-- Table variable  (scope = batch/SP)
declare @TempTable table (
    id   int,
    name nvarchar(20)
)
insert into @TempTable values (1,'Test')
select * from @TempTable

-- Local Temp Table  (scope = session, dropped when session ends)
create table #LocalTemp (
    author_id   int primary key,
    author_name varchar(255)
)
drop table #LocalTemp

-- Global Temp Table  (visible to all sessions)
create table ##GlobalTemp (id int, val varchar(50))
drop table ##GlobalTemp


-- ============================================================
-- ## 16. IDENTITY  /  SYSTEM FUNCTIONS  /  USEFUL QUERIES
-- ============================================================

-- Get last inserted identity value (session-scoped, safest)
select scope_identity() as LastInsertedId

-- Get current identity value of a specific table
select ident_current('dbo.Customers') as CurrentIdentity

-- System info
select getdate()    as Now
select newid()      as NewGUID
select db_name()    as CurrentDB
select user_name()  as CurrentUser
select @@version    as SQLServerVersion
select @@rowcount   as RowsAffected   -- rows affected by last statement
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
select charindex('l','hello')      -- 3  (first occurrence)
select left('hello',3)             -- hel
select right('hello',3)            -- llo
select concat('SQL',' ','Server')  -- SQL Server

-- Date functions
select getdate()                         -- current datetime
select year(getdate())                   -- current year
select month(getdate())                  -- current month
select day(getdate())                    -- current day
select dateadd(day,  7, getdate())       -- 7 days from now
select datediff(day,'2023-01-01',getdate()) -- days since date
select format(getdate(),'dd/MM/yyyy')   -- formatted string

-- NULL handling
select isnull(null, 'Default')           -- Default
select coalesce(null, null, 'Found')     -- Found
select nullif(5, 5)                      -- NULL  (equal)

-- CASE expression
select customername,
       expense,
       case
           when expense >= 100 then 'High'
           when expense >= 70  then 'Medium'
           else                     'Low'
       end as SpendCategory
    from Customers

-- IIF  (shorthand CASE)
select customername,
       iif(expense > 80, 'VIP', 'Regular') as Status
    from Customers

-- CAST / CONVERT
select cast(expense as int)                        from Customers
select convert(varchar, createdate, 103)           from Customers  -- dd/mm/yyyy


-- ============================================================
-- ## 17. INDEXES  (Performance)
-- ============================================================

-- Clustered index  (one per table, physically sorts data)
create clustered index IX_Customers_Expense
    on Customers (expense)

-- Non-clustered index  (separate lookup structure)
create nonclustered index IX_Customers_Name
    on Customers (customername)

-- Composite index
create nonclustered index IX_Products_BuyerPrice
    on Products (productbuyerid, productprice)

-- Drop index
drop index IX_Customers_Name on Customers

-- View indexes on a table
select * from sys.indexes where object_id = object_id('Customers')


-- ============================================================
-- ## 18. NORMALIZATION QUICK CHEAT SHEET (Comments only)
-- ============================================================
-- 1NF  : Atomic columns, no repeating groups.
-- 2NF  : 1NF + every non-key column depends on the WHOLE PK.
-- 3NF  : 2NF + no transitive dependencies (non-key → non-key).
-- BCNF : Every determinant is a candidate key.
-- 4NF  : No multi-valued dependencies.
-- 5NF  : No join dependencies that can't be inferred from PKs.


-- ============================================================
-- ██████████████████████████████████████████████████████████
--  MNC INTERVIEW QUESTIONS (50)
--  Every answer runs against the PG schema above.
-- ██████████████████████████████████████████████████████████
-- ============================================================

-- ─────────────────────────────────────────────────────────────
-- Q1. Retrieve all customers.
-- ─────────────────────────────────────────────────────────────
select * from Customers

-- ─────────────────────────────────────────────────────────────
-- Q2. Find the customer with the highest expense.
-- ─────────────────────────────────────────────────────────────
select top 1 * from Customers order by expense desc

-- ─────────────────────────────────────────────────────────────
-- Q3. Find the 2nd highest expense (no LIMIT / no TOP 2 trick).
-- ─────────────────────────────────────────────────────────────
select max(expense) as SecondHighest
    from Customers
    where expense < (select max(expense) from Customers)

-- ─────────────────────────────────────────────────────────────
-- Q4. Find the Nth highest expense using DENSE_RANK (N = 3).
-- ─────────────────────────────────────────────────────────────
with Ranked as (
    select expense,
           dense_rank() over (order by expense desc) as rnk
    from Customers
)
select distinct expense from Ranked where rnk = 3

-- ─────────────────────────────────────────────────────────────
-- Q5. Count the total number of customers.
-- ─────────────────────────────────────────────────────────────
select count(*) as TotalCustomers from Customers

-- ─────────────────────────────────────────────────────────────
-- Q6. Find customers whose expense is above the average.
-- ─────────────────────────────────────────────────────────────
select * from Customers
    where expense > (select avg(expense) from Customers)

-- ─────────────────────────────────────────────────────────────
-- Q7. List customers along with their purchased products.
--     (INNER JOIN — only customers who bought something)
-- ─────────────────────────────────────────────────────────────
select c.customername, p.productname, p.productprice
    from Customers c
    inner join Products p on c.customerid = p.productbuyerid

-- ─────────────────────────────────────────────────────────────
-- Q8. List ALL customers, including those without any product.
--     (LEFT JOIN)
-- ─────────────────────────────────────────────────────────────
select c.customername, p.productname
    from Customers c
    left join Products p on c.customerid = p.productbuyerid
     
-- ─────────────────────────────────────────────────────────────
-- Q9. Find customers who have NOT bought any product.
-- ─────────────────────────────────────────────────────────────
select c.*
    from Customers c
    left join Products p on c.customerid = p.productbuyerid
    where p.productid is null

-- ─────────────────────────────────────────────────────────────
-- Q10. Delete duplicate rows from Products,
--      keeping the row with the highest productid.
-- ─────────────────────────────────────────────────────────────
delete from Products
    where productid not in (
        select max(productid)
        from Products
        group by productname, productbuyerid, productprice
    )

-- ─────────────────────────────────────────────────────────────
-- Q11. Find products with price greater than 70.
-- ─────────────────────────────────────────────────────────────
select * from Products where productprice > 70

-- ─────────────────────────────────────────────────────────────
-- Q12. Get the total revenue per product name.
-- ─────────────────────────────────────────────────────────────
select productname, sum(productprice) as TotalRevenue
    from Products
    group by productname

-- ─────────────────────────────────────────────────────────────
-- Q13. List products where more than one unit exists (count > 1).
-- ─────────────────────────────────────────────────────────────
select productname, count(*) as Units
    from Products
    group by productname
    having count(*) > 1

-- ─────────────────────────────────────────────────────────────
-- Q14. Find the minimum and maximum product price.
-- ─────────────────────────────────────────────────────────────
select min(productprice) as MinPrice,
       max(productprice) as MaxPrice
    from Products

-- ─────────────────────────────────────────────────────────────
-- Q15. Rank products by price descending.
-- ─────────────────────────────────────────────────────────────
select productname,
       productprice,
       rank()       over (order by productprice desc) as rnk,
       dense_rank() over (order by productprice desc) as dense_rnk,
       row_number() over (order by productprice desc) as row_num
    from Products

-- ─────────────────────────────────────────────────────────────
-- Q16. Find customers whose name starts with 'R'.
-- ─────────────────────────────────────────────────────────────
select * from Customers where customername like 'R%'

-- ─────────────────────────────────────────────────────────────
-- Q17. Get total expense per customer, sorted highest first.
-- ─────────────────────────────────────────────────────────────
select customername, sum(expense) as TotalExpense
    from Customers
    group by customername
    order by TotalExpense desc

-- ─────────────────────────────────────────────────────────────
-- Q18. Retrieve customer names and their expense category
--      using CASE (High / Medium / Low).
-- ─────────────────────────────────────────────────────────────
select customername,
       expense,
       case
           when expense >= 100 then 'High'
           when expense >= 70  then 'Medium'
           else                     'Low'
       end as Category
    from Customers

-- ─────────────────────────────────────────────────────────────
-- Q19. Show customers with a running total of expenses.
-- ─────────────────────────────────────────────────────────────
select customername,
       expense,
       sum(expense) over (order by customerid
                          rows unbounded preceding) as RunningTotal
    from Customers

-- ─────────────────────────────────────────────────────────────
-- Q20. Get each customer's expense and the previous customer's
--      expense using LAG.
-- ─────────────────────────────────────────────────────────────
select customername,
       expense,
       lag(expense, 1, 0) over (order by customerid) as PreviousExpense
    from Customers

-- ─────────────────────────────────────────────────────────────
-- Q21. Display customers with row numbers per expense group.
-- ─────────────────────────────────────────────────────────────
select row_number() over (partition by expense order by customerid) as rn,
       customername, expense
    from Customers

-- ─────────────────────────────────────────────────────────────
-- Q22. Find all transactions that have no sender or receiver
--      (still showing the default 'Empty' value).
-- ─────────────────────────────────────────────────────────────
select * from Transactions
    where transactionto = 'Empty' or transactionfrom = 'Empty'

-- ─────────────────────────────────────────────────────────────
-- Q23. Upsert: update customer if exists, insert if not.
--      (Classic IF-EXISTS pattern)
-- ─────────────────────────────────────────────────────────────
declare @uid   int         = 15
declare @uname varchar(50) = 'Omkar'
declare @uage  money       = 23

if ((select count(*) from Customers where customerid = @uid) = 1)
    update Customers
        set customername = @uname,
            expense      = @uage
        where customerid = @uid
else
    insert into Customers values (@uid, @uname, @uage, getdate())

-- ─────────────────────────────────────────────────────────────
-- Q24. Write a stored procedure to insert a customer.
-- ─────────────────────────────────────────────────────────────
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

-- ─────────────────────────────────────────────────────────────
-- Q25. Write a SP to find the Nth highest product price.
-- ─────────────────────────────────────────────────────────────
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

-- ─────────────────────────────────────────────────────────────
-- Q26. Write a transaction that inserts and rolls back on error.
-- ─────────────────────────────────────────────────────────────
begin try
    begin transaction
        insert into Customers values (99,'TxnTest',50,'2024-01-01')
        -- simulate error: insert duplicate PK
        insert into Customers values (99,'Duplicate',60,'2024-01-01')
    commit transaction
end try
begin catch
    rollback transaction
    select error_message() as ErrorMsg
end catch

-- ─────────────────────────────────────────────────────────────
-- Q27. What is a VIEW? Create one to show high spenders.
-- ─────────────────────────────────────────────────────────────
-- A VIEW is a named, stored SELECT query. It doesn't store data.
-- Use cases: security (hide columns), simplify complex queries.

create view vw_HighSpenders as
    select customername, expense
    from Customers
    where expense > 80

select * from vw_HighSpenders

-- ─────────────────────────────────────────────────────────────
-- Q28. Difference between WHERE and HAVING — demonstrate both.
-- ─────────────────────────────────────────────────────────────
-- WHERE  : filters ROWS   before grouping (cannot use aggregates)
-- HAVING : filters GROUPS after  grouping (can use aggregates)

select expense, count(*) as Cnt
    from Customers
    where expense > 40          -- row-level filter
    group by expense
    having count(*) >= 1        -- group-level filter

-- ─────────────────────────────────────────────────────────────
-- Q29. Difference between RANK, DENSE_RANK, ROW_NUMBER.
-- ─────────────────────────────────────────────────────────────
-- ROW_NUMBER : always unique (1,2,3,4,5)
-- RANK       : ties get same rank, next rank skips (1,1,3,4)
-- DENSE_RANK : ties get same rank, next rank does NOT skip (1,1,2,3)

select customername, expense,
       row_number() over (order by expense desc) as row_num,
       rank()       over (order by expense desc) as rnk,
       dense_rank() over (order by expense desc) as dense_rnk
    from Customers

-- ─────────────────────────────────────────────────────────────
-- Q30. What is a CTE? Write one to list customers above average.
-- ─────────────────────────────────────────────────────────────
-- CTE = Common Table Expression. Temporary result set defined
-- with WITH. Improves readability. Exists only for the query.

with AvgCTE as (
    select avg(expense) as AvgExp from Customers
)
select c.*
    from Customers c
    cross join AvgCTE a
    where c.expense > a.AvgExp

-- ─────────────────────────────────────────────────────────────
-- Q31. Difference between TRUNCATE and DELETE.
-- ─────────────────────────────────────────────────────────────
-- DELETE  : DML, row-by-row, logged, can use WHERE, can ROLLBACK.
-- TRUNCATE: DDL, removes all rows, minimal logging, no WHERE,
--           resets identity counter, faster on large tables.

-- delete from Customers where customerid = 99   -- selective
-- truncate table Transactions                   -- all rows

-- ─────────────────────────────────────────────────────────────
-- Q32. Find customers who share the same expense amount.
--      (Duplicate expense values)
-- ─────────────────────────────────────────────────────────────
select expense, count(*) as Count
    from Customers
    group by expense
    having count(*) > 1

-- ─────────────────────────────────────────────────────────────
-- Q33. Find customers who registered in 2023.
-- ─────────────────────────────────────────────────────────────
select * from Customers where year(createdate) = 2023

-- ─────────────────────────────────────────────────────────────
-- Q34. Find the most recently registered customer.
-- ─────────────────────────────────────────────────────────────
select top 1 * from Customers order by createdate desc

-- ─────────────────────────────────────────────────────────────
-- Q35. Add a column 'email' to Customers and then drop it.
-- ─────────────────────────────────────────────────────────────
alter table Customers add email varchar(100)
alter table Customers drop column email

-- ─────────────────────────────────────────────────────────────
-- Q36. What is a TRIGGER? Create one that logs on insert.
-- ─────────────────────────────────────────────────────────────
-- TRIGGER : code that fires automatically on INSERT/UPDATE/DELETE.
-- Types   : AFTER (post-DML), INSTEAD OF (replaces DML).

create trigger trig_LogInsert
on Customers
after insert
as
begin
    declare @newid int
    select @newid = customerid from inserted     -- 'inserted' pseudo-table
    print 'New customer added with ID: ' + cast(@newid as varchar)
end

-- ─────────────────────────────────────────────────────────────
-- Q37. Explain PRIMARY KEY vs UNIQUE KEY with examples.
-- ─────────────────────────────────────────────────────────────
-- PRIMARY KEY : one per table, no NULL allowed, clustered index by default.
-- UNIQUE KEY  : multiple per table, one NULL allowed.

-- PK already on Customers(customerid), Products(productid)
-- UNIQUE already on Credentials(credusername)

-- ─────────────────────────────────────────────────────────────
-- Q38. What is a FOREIGN KEY? Show cascading delete setup.
-- ─────────────────────────────────────────────────────────────
-- Enforces referential integrity between two tables.

-- Example: if we recreate the FK with ON DELETE CASCADE:
alter table Products drop constraint FK_buyer   -- drop existing if any
alter table Products
    add constraint FK_buyer_cascade
    foreign key (productbuyerid) references Customers(customerid)
    on delete cascade    -- auto-delete products when customer deleted
    on update cascade    -- auto-update FK when PK changes

-- ─────────────────────────────────────────────────────────────
-- Q39. Difference between INNER JOIN, LEFT JOIN, RIGHT JOIN.
-- ─────────────────────────────────────────────────────────────
-- INNER : only rows with a match in BOTH tables.
-- LEFT  : all rows from left table + matched from right (NULL if no match).
-- RIGHT : all rows from right table + matched from left (NULL if no match).
-- FULL  : all rows from both (NULL where no match on either side).

-- ─────────────────────────────────────────────────────────────
-- Q40. Find products that no customer has purchased.
-- ─────────────────────────────────────────────────────────────
select p.*
    from Products p
    left join Customers c on p.productbuyerid = c.customerid
    where c.customerid is null

-- ─────────────────────────────────────────────────────────────
-- Q41. Get customer count per month in 2023.
-- ─────────────────────────────────────────────────────────────
select month(createdate) as Month,
       count(*)           as NewCustomers
    from Customers
    where year(createdate) = 2023
    group by month(createdate)
    order by Month

-- ─────────────────────────────────────────────────────────────
-- Q42. Pivot: show total expense for each customer as columns.
--      (Simple PIVOT using CASE)
-- ─────────────────────────────────────────────────────────────
select
    sum(case when customername = 'Rahul1'      then expense else 0 end) as Rahul1,
    sum(case when customername = 'Priya'       then expense else 0 end) as Priya,
    sum(case when customername = 'Gaurav'      then expense else 0 end) as Gaurav
    from Customers

-- ─────────────────────────────────────────────────────────────
-- Q43. Employees who earn more than their manager.
--      (Self-join pattern — applied to Customers as stand-in)
-- ─────────────────────────────────────────────────────────────
-- Standard SQL pattern (assumes employees table with manager_id):
-- SELECT e.* FROM employees e JOIN employees m
--   ON e.manager_id = m.emp_id WHERE e.salary > m.salary;

-- Applied to Customers (self-join on customerid for demo):
select a.customername as Employee,
       a.expense      as EmpExpense,
       b.customername as Manager,
       b.expense      as MgrExpense
    from Customers a
    inner join Customers b on a.customerid = b.customerid + 1
    where a.expense > b.expense

-- ─────────────────────────────────────────────────────────────
-- Q44. What is IDENTITY? Reset it.
-- ─────────────────────────────────────────────────────────────
-- IDENTITY(seed, increment) auto-generates PK values.
-- Credentials.credid is IDENTITY(1,1).

select ident_current('dbo.Credentials') as CurrentSeed

-- Reseed to 100
dbcc checkident ('dbo.Credentials', reseed, 100)

-- ─────────────────────────────────────────────────────────────
-- Q45. What is SCOPE_IDENTITY vs @@IDENTITY vs IDENT_CURRENT?
-- ─────────────────────────────────────────────────────────────
-- SCOPE_IDENTITY() : last identity in current scope/session (safest)
-- @@IDENTITY        : last identity across all scopes (affected by triggers)
-- IDENT_CURRENT('T'): last identity for table T, any session

insert into Credentials values ('newuser','pass123')
select scope_identity()            as ScopeIdentity,
       @@identity                  as AtAtIdentity,
       ident_current('Credentials') as IdentCurrent

-- ─────────────────────────────────────────────────────────────
-- Q46. Use COALESCE and ISNULL to handle NULLs.
-- ─────────────────────────────────────────────────────────────
select customername,
       isnull(customername, 'Unknown')         as WithIsNull,
       coalesce(null, customername, 'Default') as WithCoalesce
    from Customers

-- ─────────────────────────────────────────────────────────────
-- Q47. Convert expense to a formatted string with CAST/CONVERT.
-- ─────────────────────────────────────────────────────────────
select customername,
       cast(expense as int)                            as IntExpense,
       '$' + cast(cast(expense as int) as varchar)     as FormattedExpense,
       convert(varchar, createdate, 103)               as DateDMY
    from Customers

-- ─────────────────────────────────────────────────────────────
-- Q48. What is UNION vs UNION ALL? Demonstrate the difference.
-- ─────────────────────────────────────────────────────────────
-- UNION     removes duplicates (slower – needs sorting/hashing).
-- UNION ALL keeps duplicates  (faster).

select customername as Name from Customers
union                              -- duplicates removed
select productname  as Name from Products

select customername as Name from Customers
union all                          -- duplicates kept
select productname  as Name from Products

-- ─────────────────────────────────────────────────────────────
-- Q49. What are indexes? Show clustered vs non-clustered.
-- ─────────────────────────────────────────────────────────────
-- CLUSTERED   : defines physical row order; one per table;
--               the table IS the index.
-- NON-CLUSTERED: separate B-tree structure pointing to data rows;
--               many per table.

create clustered index    IX_C_Expense on Customers(expense)
create nonclustered index IX_NC_Name   on Customers(customername)

-- Drop them
drop index IX_C_Expense on Customers
drop index IX_NC_Name   on Customers

-- ─────────────────────────────────────────────────────────────
-- Q50. What are the ACID properties? (Interview theory question)
-- ─────────────────────────────────────────────────────────────
-- A – Atomicity   : All operations in a transaction succeed or all fail.
-- C – Consistency : DB moves from one valid state to another.
-- I – Isolation   : Concurrent transactions don't interfere.
-- D – Durability  : Committed data survives system failures.

-- Demonstration: the TRY/CATCH transaction in Q26 shows Atomicity.
-- Isolation levels in SQL Server:
--   READ UNCOMMITTED (dirty reads allowed)
--   READ COMMITTED   (default)
--   REPEATABLE READ
--   SNAPSHOT
--   SERIALIZABLE     (strictest)

set transaction isolation level read committed   -- set isolation level
begin transaction
    select * from Customers with (nolock)        -- READ UNCOMMITTED hint
commit transaction

-- ============================================================
-- END OF FILE
-- ============================================================