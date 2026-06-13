create database PG
--drop database PG
use PG
create table Customers(
customerid int ,
customername varchar(40),
expense money,
createdate datetime)

create table Products(
productid int primary key ,
productname varchar(20) not null,
productbuyerid int foreign key references Customers(customerid) ,
productprice money not null
)

create table Transactions(
transactionid int primary key,
trnsactiondate Date not null,
transactionto varchar(20) default 'Empty',
transactionfrom varchar(20) default 'Empty')

create table Credentials(
credid int identity primary key,
credusername varchar(30) unique not null,
credpassword varchar(30) not null)

alter table Customers alter column customerid int not null
alter table Customers add primary key(customerid)

insert into Customers values(8,'Rahul8',80,'2023-05-13')
delete from Customers where createdate='2023-05-13'
insert into Products values(4,'Soap2',3,70)
insert into Transactions values(2,'2023-05-13',default,default)

insert into Credentials values('pg','pgpg')

select top 3 * from Customers 
select * from Customers
select * from Products
select * from Transactions
select * from Credentials

create view Customermoneyview as 
select * from Customers where expense=80

select * from Customers where expense>70 or expense<60 and not expense=50
select * from Customers order by expense desc

update Customers set customername='Gaurav' where expense=70
delete from Customers where expense=70

select count(expense) as Outcome from Customers

select * from Customers where expense like '5_.%'
select * from Customers where expense in (50,70)
select * from Customers where expense between 20 and 90

select c.customername,p.productprice from Customers c, Products p where c.customerid=p.productid

select * from Customers as c inner join Products as p on c.customerid=p.productid
select * from Customers as c full join Products as p on c.customerid=p.productid
select * from Customers as c left join Products as p on c.customerid=p.productid
select * from Customers as c right join Products as p on c.customerid=p.productid
select * from Customers as c cross join Products as p  
select * from Customers a,Customers b where a.customerid=b.customerid

select c.customername as name from Customers c union all select p.productname as name from Products p

select count(p.productid),productname from Products p group by p.productprice,productname having productprice>80

select * from Customers where exists (select * from Products p where p.productprice=70)

begin transaction 
insert into Customers values (7,'Rahul6',88,'2023-05-13')
IF(@@ERROR > 0)  
BEGIN  
    ROLLBACK TRANSACTION  
END  
else
Begin
commit transaction
END

--stored procedure start
create procedure sp
@cid int,
@cname varchar (20),
@ex money,
@date Date
as 
begin

declare @hi int;
set @hi=4;
insert into Customers values(@cid,@cname,@ex,@date)

end
-- end

execute sp @cid=6,@cname='Purushottam',@ex=120,@date='2023-05-13'

create trigger tmodify
on Customers 
for insert
as begin
insert into Credentials values('uttam','password')
end

select * from Customers

select min(expense) from (select top 2 count(customerid) as id,expense from Customers group by expense order by expense desc) as expense

---Delete duplicate entries Ques 1
delete from products  
where productid not in 
(
select  max(a.productid)
from products a
group by  a.productname,a.productbuyerid,a.productprice
)


---Sp to take a value and find nth highest price using CTE  Ques 2
create procedure nhighest
(@nth int)
as

begin

with cte(productid,productname,productbuyerid,productprice) as

(select Top(@nth) * from products order by productprice desc)

select top(1) * from cte
order by productprice desc
end

execute nhighest 2

--- Ques 3 Select employees who earn more than their managers

SELECT *
FROM employees e,
     employees m
WHERE e.manager_id = m.emp_id
  AND e.salary> m.salary;

  --------------------------------------------------------------------
  DECLARE @id INT=15,
@name varchar(50)='Omkar',
@age int =23
IF ((select count(*) from students where ID=@id ) =1)
BEGIN
UPDATE students
 SET
     Name = @name,
  age= @age
 WHERE ID = @id;
END
ELSE
BEGIN
INSERT INTO students(ID,Name,age)
 VALUES (@id,@name,@age )
END

select * from students
------------------------------------------------------------------
declare @temptry table
(id int,
name nvarchar(20))
-------------------------------------------------------------------
CREATE TABLE #indian_authors (
    author_id INT PRIMARY KEY,
    author_name VARCHAR(255)
);

--------------------------------------------------------------------
select * from customers

select ROW_NUMBER() over (partition by expense order by expense) row_num, customername,expense,createdate from customers
--------------------------------------------------

select schema_name(t.schema_id) as schema_name,
       t.name as table_name
from sys.tables t
where t.name like 'hr%'
order by table_name,
         schema_name;


select SCOPE_IDENTITY() as sp ;
select IDENT_CURRENT('dbo.Customers') as s