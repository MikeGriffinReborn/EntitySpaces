Click here for the [Glossy Site ...](https://mikegriffinreborn.github.io/EntitySpaces/)

<img src="https://repository-images.githubusercontent.com/194275145/55b5b080-1ccf-11ea-8609-15b9de0d2351" alt="EntitySpaces" width="531" height="268">

Available on Nuget for [SqlServer](https://www.nuget.org/packages/EntitySpaces.ORM.SqlServer), [SQLite](https://www.nuget.org/packages/EntitySpaces.ORM.SQLite/ "NuGet"), [MySQL](https://www.nuget.org/packages/EntitySpaces.ORM.MySQL/ "NuGet") or [PostgreSQL](https://www.nuget.org/packages/EntitySpaces.ORM.PostgreSQL)

See the [Setup Section](#setup) for more details ...

# EntitySpaces - A Fluent SQL API
EntitySpaces is a Fluent API for SQL Server, SQLite, MySQL, PostgreSQL and more on the way. If you are familiar with the SQL syntax then you are already an expert in EntitySpaces. EntitySpaces is also high performance, transactional, and very intuitive. EntitySpaces Studio is used to generate your C# classes from your database schema.

## Example Query
In this example we are going to sum the total # of items for each order. Each order can have many order detail records so we group our query by OrderId and sum up the quantity as 'TotalQuantity'. Notice that we can access the derived 'TotalQuantity' column through the dynamic property.

**Use of 'out var'**

*Notice the judicial use of the "our var" syntax of C# in the example code below. The "out var" syntax allows you to delcare the variable that is created for you such as the 'OrderDetailQuery' object 'od' in the InnerJoin() below. Then you are free to use the 'od' variable throughout the query as we do in the Select() statement. This is also true for constructors. For example, notice how we declare "out var o" on the creation of the OrdersQuery() and then are free to use it throughout as well.*

```c#
OrdersCollection coll = new OrdersQuery("o", out var o)
    .InnerJoin<OrderDetailsQuery>("od", out var od).On(o.OrderID == od.OrderID)
    .Select(o.OrderID, od.Quantity.Sum().As("TotalQuantity"))
    .GroupBy(o.OrderID)
    .OrderBy(o.OrderID.Ascending)
    .ToCollection<OrdersCollection>();

foreach(Orders order in coll)
{
    Console.WriteLine(order.OrderID + " : " + order.dynamic.TotalQuantity);
}
```

The SQL generated is just as you would expect.

```sql
SELECT o.[OrderID], SUM(od.[Quantity]) AS 'TotalQuantity'  
FROM [Orders] o 
INNER JOIN [Order Details] od ON o.[OrderID] = od.[OrderID] 
GROUP BY o.[OrderID] 
ORDER BY o.[OrderID] ASC
```

The output is as follows is ...

|OrderID | TotalQuantity |
|-|-|
|10248	 |27|
|10249	 |49|
|10250	 |60|

## InnerJoin, RightJoin, LeftJoin, CrossJoin, and FullJoin

The sample below demonstrates a self join on the Employees table which is looking for all employees whose Supervisor has an 'a' in their last name. Kind of silly but it shows off the syntax. 

```c#
EmployeesCollection coll = new EmployeesQuery("e", out var e)   // Employees
    .InnerJoin<EmployeesQuery>("r", out var reportsTo).On(e.ReportsTo == reportsTo.EmployeeID)
    .Select(e.EmployeeID, e.LastName, reportsTo.LastName.As("SupervisorName"))
    .Where(reportsTo.LastName.Like("%a%"))
    .OrderBy(reportsTo.LastName.Descending).Distinct()
    .ToCollection<EmployeesCollection>();

if (coll.Count > 0)
{
    // Then we loaded at least one record
}
```

Notice that the SQL is extremely lean.

Results from the Query Above. SQL Parameters are always used to avoid SQL Injection Attacks.

```sql
SELECT  DISTINCT e.[EmployeeID],e.[LastName],r.[LastName] AS 'SupervisorName'  
FROM [Employees] e 
INNER JOIN [Employees] r ON e.[ReportsTo] = r.[EmployeeID] 
WHERE r.[LastName] LIKE @LastName1 
ORDER BY r.[LastName] DESC
```

## Any, All, and Some 
Any, All, and Some all follow the same rules. You them with operators (==, !=, >, >=, <, or <=) in the "nested" syntax as shown below.

```c#
EmployeesCollection coll = new EmployeesQuery("q", out var q)
.Where(q.EmployeeID > (() =>
    {
        return new EmployeesQuery("e", out var q1)
        .Select(q1.EmployeeID)
        .Where(q1.EmployeeID.IsNotNull()).Any();  // <= Any indicated here !
    })
)
.ToCollection<EmployeesCollection>();
```

SQL Generated:

```sql
SELECT * FROM [Employees] q 
WHERE q.[EmployeeID] > ANY 
(
    SELECT e.[EmployeeID] 
    FROM [Employees] e 
    WHERE e.[EmployeeID] IS NOT NULL
)
```

## CrossApply and OuterApply
This example uses OuterApply to select each customer and their last 2 orders.

```c#
CustomersCollection coll = new CustomersQuery("c", out var c)
    .OuterApply<OrdersQuery>(out var o, () =>
    {
        return new OrdersQuery("o", out var subQuery)
        .Select(subQuery.OrderID, subQuery.OrderDate)
        .Top(2)
        .Where(subQuery.CustomerID == c.CustomerID)
        .OrderBy(subQuery.OrderDate.Descending, subQuery.OrderID.Ascending);

    })
    .Select(c.CustomerID, c.CompanyName, o.OrderID, o.OrderDate)
    .ToCollection<CustomersCollection>();

// Notice the "dynamic" property accessor for accessing the columns brought 
// back from the Orders table.
foreach(Customers cust in coll)
{
    Console.WriteLine(cust.CustomerID);
    Console.WriteLine(cust.CompanyName);
    Console.WriteLine(cust.dynamic.OrderID);
    Console.WriteLine(cust.dynamic.OrderDate);
}    
```
SQL Generated:

```sql
SELECT c.[CustomerID],c.[CompanyName],o.[OrderID],o.[OrderDate]
FROM [Customers] c 
OUTER APPLY 
(
    SELECT TOP 2 o.[OrderID],o.[OrderDate]
	FROM [Orders] o 
	WHERE o.[CustomerID] = c.[CustomerID] 
	ORDER BY o.[OrderDate] DESC,o.[OrderID] ASC
) AS o
```

Each customer and their last 2 orders.


|CustomerID | CompanyName | OrderID | OrderDate|
|:-|:-|:-|:-|
|ALFKI|Alfreds Futterkiste|11011|04/09/1998 12:00:00 AM|
|ALFKI|Alfreds Futterkiste|10952|03/16/1998 12:00:00 AM|
|ANATR|Ana Trujillo Emparedados y helados|10926|03/04/1998 12:00:00 AM|
|ANATR|Ana Trujillo Emparedados y helados|10759|11/28/1997 12:00:00 AM|
|ANTON|Antonio Moreno Taquería|10856|01/28/1998 12:00:00 AM|
|ANTON|Antonio Moreno Taquería|10682|09/25/1997 12:00:00 AM|
|AROUT|Around the Horn|11016|04/10/1998 12:00:00 AM|
|AROUT|Around the Horn|10953|03/16/1998 12:00:00 AM|

## Union, Intersect, and Except
Here we use Union to find employees whose first name begins with F, C, or M. Of course, this isn't a great way to determine this data but it demonstrate syntax.

```c#
EmployeesCollection coll = new EmployeesQuery("q1", out var q1)
    .Select(q1.EmployeeID, q1.FirstName, q1.LastName)
    .Where(q1.FirstName.Like("F%"))
    .Union(() =>
    {
        return new EmployeesQuery("q2", out var q2)
        .Select(q2.EmployeeID, q2.FirstName, q2.LastName)
        .Where(q2.FirstName.Like("C%"));
    })
    .Union(() =>
    {
        return new EmployeesQuery("q3", out var q3)
        .Select(q3.EmployeeID, q3.FirstName, q3.LastName)
        .Where(q3.FirstName.Like("M%"));
    })
    .ToCollection<EmployeesCollection>();

if (coll.Count > 0)
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT q1.[EmployeeID],q1.[FirstName],q1.[LastName]  
FROM [Employees] q1 WHERE q1.[FirstName] LIKE @FirstName1 
  UNION SELECT q2.[EmployeeID],q2.[FirstName],q2.[LastName]  
  FROM [Employees] q2 WHERE q2.[FirstName] LIKE @FirstName2 
  UNION SELECT q3.[EmployeeID],q3.[FirstName],q3.[LastName]  
  FROM [Employees] q3 WHERE q3.[FirstName] LIKE @FirstName3
```

## Using In() and NotIn() via Nested Queries

```c#
OrdersCollection coll = new OrdersQuery("o", out var oQuery)
.Select(oQuery.OrderID, oQuery.EmployeeID)
.InnerJoin<OrderDetailsQuery>("od", out var od).On(oQuery.OrderID == od.OrderID)
.InnerJoin<EmployeesQuery>("e", out var e).On(e.EmployeeID == oQuery.EmployeeID 
  && oQuery.EmployeeID.In(() =>
  {
     return new EmployeesQuery("ee", out var ee)
      .InnerJoin<OrdersQuery>("eo", out var eo).On(ee.EmployeeID == eo.EmployeeID)
      .InnerJoin<OrderDetailsQuery>("eod", out var eod).On(eo.OrderID == eod.OrderID)
      .Select(eo.EmployeeID)
      .Distinct();
  })
)
.ToCollection<OrdersCollection>();

if (coll.Count > 0)
{
    // We loaded some records
}
```

SQL Generated:

```sql
SELECT o.[OrderID],  o.[EmployeeID]
FROM [Orders] o
INNER JOIN [Order Details] od ON o.[OrderID] = od.[OrderID]
INNER JOIN [Employees] e ON (e.[EmployeeID] = o.[EmployeeID] AND o.[EmployeeID] IN 
(
    SELECT DISTINCT eo.[EmployeeID]
    FROM [Employees] ee
    INNER JOIN [Orders] eo ON ee.[EmployeeID] = eo.[EmployeeID]
    INNER JOIN [Order Details] eod ON eo.[OrderID] = eod.[OrderID]
)
```

## Exists() 

Exists evaluates to true if the SubQuery returns a result set.

```c#
EmployeesCollection coll = new EmployeesQuery("e", out var eq)
.Select(eq.EmployeeID, eq.ReportsTo)
.Where(eq.Exists(() =>
{
    // SubQuery of Employees with a null Supervisor column.
    return new EmployeesQuery("s", out var sq)
    .Select(sq.EmployeeID).Where(sq.ReportsTo.IsNull()).Distinct();
}))
.ToCollection<EmployeesCollection>();

if (coll.Count > 0)
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT e.[EmployeeID], e.[ReportsTo]
FROM [Employees] e
WHERE EXISTS (
    SELECT DISTINCT s.[EmployeeID]
    FROM [Employees] s
    WHERE s.[ReportsTo] IS NULL
)
```

## Where() with Nested Query

In and NotIn are two of the most common operators used in a Where SubQuery. The following produces a result set containing Territories that an Employee is not associated with.

```c#
// Territories that Employee 1 is not assigned to.
TerritoriesCollection coll = new TerritoriesQuery("t", out var tq)
  .Select(tq.TerritoryID, tq.TerritoryDescription);
  .Where(tq.TerritoryID.NotIn(() =>
  {
      return new EmployeeTerritoriesQuery("et", out var etq)
      .Select(etq.TerritoryID)
      .Where(etq.EmployeeID == 1);
  }))
  .ToCollection<TerritoriesCollection>();

if (coll.Count > 0)
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT t.[Description]  
FROM [dbo].[Territory] t 
WHERE t.[TerritoryID] NOT IN 
(
    SELECT et.[TerrID]  
    FROM .[dbo].[EmployeeTerritory] et 
    WHERE et.[EmpID] = @EmpID1
) 
```

## From() with Nested Query
Notice how in the Select() statement we use the "escape hatch" mechanism and declare "<sub.OrderTotal>" as a string. What does this do? Anything you pass in within "<>" brackets is take "as-is". We need to do this here because the nested query in the From() clause is aliased as "sub" and we need to access the derived "OrderTotal" column. In an upcoming version the "out var" syntax will be supported on the Alias and you will no longer have to use the escape hatch. This isn't always true of the From clause it only has to do with this particular query.

```c#
OrdersCollection coll = new OrdersQuery("o", out var o)
    .Select(o.CustomerID, o.OrderDate, "<sub.OrderTotal>")
    .From<OrderDetailsQuery>(out var od, () =>
    {
        return new OrderDetailsQuery("od", out var subQuery)
        .Select(subQuery.OrderID, (subQuery.UnitPrice * subQuery.Quantity).Sum().As("OrderTotal"))
        .GroupBy(subQuery.OrderID);
    }).As("sub")
    .InnerJoin(o).On(o.OrderID == od.OrderID)
    .ToCollection<OrdersCollection>();

if (coll.Count > 0)
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT o.[CustomerID], o.[OrderDate], sub.OrderTotal
FROM 
(
    SELECT od.[OrderID],SUM((od.[UnitPrice] * od.[Quantity])) AS 'OrderTotal'  
	FROM [Order Details] od 
	GROUP BY od.[OrderID]
) AS sub 
INNER JOIN [Orders] o ON o.[OrderID] = sub.[OrderID]
```

## Nested Query within Select Clause

A Nested Query in a Select clause must return a single value.

```c#
OrdersCollection coll = new OrdersQuery("o", out var orders)
.Select
(
    orders.OrderID, 
    orders.OrderDate,
    // Embed another query (see 'SQL Generated' below)
    new OrderDetailsQuery("oi", out var details).Select(details.UnitPrice.Max())
    .Where(orders.OrderID == details.OrderID).As("MaxUnitPrice")
)
.ToCollection<OrdersCollection>();

if (coll.Count > 0)
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT o.[OrderID],o.[OrderDate], 
(
   SELECT MAX(oi.[UnitPrice]) AS 'UnitPrice'  
   FROM [Order Details] oi 
   WHERE o.[OrderID] = oi.[OrderID]
) AS MaxUnitPrice  
FROM [Orders] o
```

## AND and OR and Concatentation
And and Or work just as you would expect, use parenthesis to control the order of precedence. You can also concatentat and use all kinds of operators in your queries. See the tables at the end of this document.

```c#
EmployeesCollection coll = new EmployeesQuery("e", out var q)
    .Select(q.EmployeeID, (q.LastName + ", " + q.FirstName).As("FullName"))
    .Where(q.EmployeeID > 4 && (q.EmployeeID < 10 || q.EmployeeID == 100))
    .ToCollection<EmployeesCollection>();

if (coll.Count > 0)
{

}
```

SQL Generated:

```sql
SELECT 
   e.[EmployeeID],
  (e.[LastName] + ', ' + e.[FirstName]) AS 'FullName'  
FROM [Employees] e 
WHERE e.[EmployeeID] > @EmployeeID1 
  AND 
  (
      e.[EmployeeID] < @EmployeeID2 OR e.[EmployeeID] = @EmployeeID3
  )
```

## Select * from a Joined Table
Here the Orders table is joined with the OrderDetails table. The Orders.OrderID column is brought back along with all columns from the OrderDetails table. Notice how the Select() statement uses 'od' without a column declared. This results in 'od.*' in the SQL.

```c#
OrdersCollection coll = new OrdersQuery("oq", out var o)
.InnerJoin<OrderDetailsQuery>("od", out var od).On(o.OrderID == od.OrderID)
.Select(o.OrderID, od) // Notice the 'od' results in 'od.*'
.Where(od.Discount > 0)
.ToCollection<OrdersCollection>();

if (coll.Count > 0)
{
    // data was loaded
}
```

SQL Generated:

```sql
SELECT oq.[OrderID], od.*
FROM [Orders] oq 
INNER JOIN [Order Details] od ON oq.[OrderID] = od.[OrderID]
WHERE od.[Discount] > @Discount1
```

## Select Top

```c#
Employees emp = new EmployeesQuery("q", out var q)
   .Where(q.ReportsTo.IsNotNull())
   .OrderBy(q.LastName.Descending).Top(1)
   .ToEntity<Employees>();

if (emp != null)
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT TOP 1 * 
FROM [Employees] 
WHERE [ReportsTo] IS NOT NULL 
ORDER BY [LastName] DESC
```


## SelectAllExcept

SelectAllExcept() is just a convenient way to select all columns except one or more listed columns.

```c#
// We don't want to bring back the huge photo
EmployeesCollection coll = new EmployeesQuery("q", out var q)
    .SelectAllExcept(q.Photo)
    .ToCollection<EmployeesCollection>();

if (coll.Count > 0)
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT q.[EmployeeID],q.[LastName],q.[FirstName],q.[Title], -- all except q.Photo
FROM [Employees] q
```

## Paging

**PageSize / PageNumber**

This is the traditional way of paging and works on all versions of SQL Server. You always need an OrderBy when sorting.

```c#
EmployeesCollection coll = new EmployeesQuery("q", out var q)
 .Select(q.EmployeeID, q.LastName)
 .OrderBy(q.LastName.Ascending)
 .PageNumber(2).PageSize(20)
 .ToCollection<EmployeesCollection>();

if (coll.Count > 0)
{

}
```

SQL Generated:

```sql
WITH [withStatement] AS 
(
   SELECT [EmployeeID],[LastName],
      ROW_NUMBER() OVER( ORDER BY [LastName] ASC) AS ESRN 
	  FROM [Employees]
)
SELECT * 
FROM [withStatement] 
WHERE ESRN BETWEEN 21 AND 40 
ORDER BY ESRN ASC
```

**Skip / Take**

Skip and Take Require Microsoft SQL 2012 at a minimum and is a much nicer syntax.

```c#
EmployeesCollection coll = new EmployeesQuery("q", out var q)
 .Select(q.EmployeeID, q.LastName)
 .OrderBy(q.LastName.Ascending)
 .Skip(40).Take(20)
 .ToCollection<EmployeesCollection>();

if (coll.Count > 0)
{

}
```

SQL Generated:

```sql
SELECT [EmployeeID],[LastName]
FROM [Employees] 
ORDER BY [LastName] ASC 
OFFSET 40 ROWS  
FETCH NEXT 20 ROWS ONLY 
```

## Distinct

SelectT DISTINCT clause to retrieve the only distinct values in a specified list of columns.

```c#
// Distinct list of Employee's who have orders ...
EmployeesCollection coll = new EmployeesQuery("e", out var e)
  .Select(e.EmployeeID)
  .InnerJoin<OrdersQuery>("o", out var o).On(e.EmployeeID == o.EmployeeID)
  .Distinct()
  .ToCollection<EmployeesCollection>();
```

SQL Generated:

```sql
SELECT DISTINCT e.[EmployeeID]
FROM [Employees] e 
INNER JOIN [Orders] o ON e.[EmployeeID] = o.[EmployeeID]
```

## With NoLock

```c#
EmployeesCollection coll = new EmployeesQuery("e", out var e)
  .Select(e.EmployeeID)
  .InnerJoin<OrdersQuery>("o", out var o).On(e.EmployeeID == o.EmployeeID)
  .Where(o.Freight > 20)
  .es.WithNoLock()
  .ToCollection<EmployeesCollection>();
```

Notice that even though many query objects are being used you only need to set WithNoLock to true for the parent or main query object. The SQL generated is as follows:

SQL Generated: (Notice that "WITH (NOLOCK)" was applied on both tables involved in the query)

```sql
SELECT e.[EmployeeID]  
FROM [Employees] e WITH (NOLOCK) 
INNER JOIN [Orders] o WITH (NOLOCK) ON e.[EmployeeID] = o.[EmployeeID] 
WHERE o.[Freight] > @Freight1
```

## Full Expressions

This query doesn’t really make sense, but we wanted to show you what will is possible.

```c#
EmployeesQuery q = new EmployeesQuery(); 
q.Select(q.LastName.Substring(2, 4).ToLower()); 
q.OrderBy(q.LastName.Substring(2, 4).ToLower().Descending); 
q.GroupBy(q.LastName.Substring(2, 4).ToLower());

EmployeesCollection coll = new EmployeesCollection();
if(coll.Load(q))
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT SUBSTRING(LOWER([LastName]),2,4) AS 'LastName' 
FROM [Employees] 
GROUP BY SUBSTRING(LOWER([LastName]),2,4) 
ORDER BY SUBSTRING(LOWER([LastName]),2,4) DESC
```

## Case().When().Then().End() Syntax

```c#
EmployeesQuery q = new EmployeesQuery();
q.Select(q.EmployeeID, q.FirstName);
q.Where(q.EmployeeID == 2);

OrderDetailsQuery oq = new OrderDetailsQuery();
oq.Select
(
  oq.UnitPrice.Case()
    .When("yay").Then("wow")
    .When(oq.Exists(q)).Then("Exists!!")
    .When(oq.Quantity >= 50).Then(oq.UnitPrice)
    .When(oq.Quantity  / 50 / 50 == 0).Then(oq.UnitPrice)
    .When(oq.Quantity >= 50 && oq.Quantity < 250).Then(1)
    .When(oq.Quantity >= 250 && 
            oq.Quantity < 1000).Then(oq.UnitPrice * .80)
    .Else("Huh?")
    .End()
);
oq.Where(oq.Quantity.Sum() >= 50 && oq.Quantity.Avg() < 250);
oq.OrderBy(oq.OrderID.Descending, oq.Quantity.Descending);

OrderDetailsCollection coll = new OrderDetailsCollection();
if(coll.Load(OrderDetails))
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT 
  CASE UnitPrice  
    WHEN 'yay' THEN 'wow' 
    WHEN  EXISTS 
    (
        SELECT [EmployeeID],[FirstName]  
        FROM [Employees] 
        WHERE [EmployeeID] = @EmployeeID1
    ) THEN 'Exists!!' 
    WHEN [Quantity] >= @Quantity2 THEN [UnitPrice] 
    WHEN (([Quantity] / 50) / 50) = @Expr3 THEN [UnitPrice] 
    WHEN ([Quantity] >= @Quantity4 AND [Quantity] < @Quantity5) THEN 1 
    WHEN ([Quantity] >= @Quantity6 AND [Quantity] < @Quantity7) THEN 
         ([UnitPrice] * 0.8) 
    ELSE 'Huh?'  
    END    
FROM [Order Details] 
WHERE (SUM([Quantity]) >= @Quantity8 AND AVG([Quantity]) < @Quantity9) 
ORDER BY [OrderID] DESC,[Quantity] DESC
```

## Another Case/When Query

```c#
EmployeeQuery q = new EmployeeQuery();
q.Select
(
    q.LastName
        .Case()
            .When(q.LastName.Like("%a%"))
            .Then("Last Name Contains an A")
            .Else("Last Name Doesnt Contain an A")
        .End().As("SpecialLastName")
);

EmployeeCollection coll = new EmployeeCollection();
if(coll.Load(q))
{
    // Then we loaded at least one record
}
```

## Having Clause

```c#
EmployeeCollection coll = new EmployeeQuery("e", out var q)
  .Select(q.EmployeeID, q.Age.Sum().As("TotalAge"))
  .Where(q.EmployeeID.IsNotNull())
  .GroupBy(q.EmployeeID)
  .Having(q.Age.Sum() > 5)
  .OrderBy(q.EmployeeID.Descending)
  .ToCollection<EmployeeCollection>();

if(coll.Count > 0)
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT e.[EmployeeID] AS 'EmployeeID', SUM([Age]) AS 'TotalAge' 
FROM [dbo].[Employee] e 
WHERE e.[EmployeeID] IS NOT NULL 
GROUP BY e.[EmployeeID] 
HAVING SUM([Age]) > @Age2 
ORDER BY e.[EmployeeID] DESC
```

## Getting the Count
Here we are getting the count of Employees who have NULL as their ReportsTo ...
```c#
int count = new EmployeesQuery("e", out var q)
    .Where(q.ReportsTo.IsNull())
    .es.CountAll().ExecuteScalar<int>();
```

SQL Generated:

```sql
SELECT COUNT(*) AS 'Count' 
FROM [Employees] e 
WHERE e.[ReportsTo] IS NULL
```

## Raw SQL Injection Everywhere
There may be times when you need to access some SQL feature that is not supported by the DynamicQuery API. But, now having used and fallen in love with DynamicQuery, the last thing you want to do is stop and go write a stored procedure or create a view. We have always supported the raw injection feature in our Select statement, but it will soon be available almost everywhere. The way it works is you pass in raw SQL in the form of a string surrounded by < > angle brackets. That indicates that you want the raw SQL passed directly to the database engine “as is”.

Here is an example query. You would never write a query like this in reality. Tiraggo supports this simple query without having to use < > angle brackets. This is just to show all of the places that can accept the raw SQL injection technique:

```c#
EmployeesCollection coll = new EmployeesQuery("e", out var q)
    .Select("<FirstName>", q.HireDate)
    .Where("<EmployeeID = 1>")
    .GroupBy("<FirstName>", q.HireDate)
    .OrderBy("<FirstName ASC>")
    .ToCollection<EmployeesCollection>();

if (coll.Count > 0)
{
    // Then we loaded at least one record
}
```

The SQL Generated is as follows (and works)

SQL Generated:

```sql
SELECT FirstName, e.[HireDate]
FROM [Employees] e 
WHERE (EmployeeID = 1) 
GROUP BY FirstName,[HireDate] 
ORDER BY FirstName ASC
```

Of course, you could easily write the above query without injection, but you get the idea. The escape hatch will be available to you almost everywhere ….

```c#
EmployeesQuery q = new EmployeesQuery();
q.Select(q.FirstName);
.Where(q.EmployeeID == 1)
.OrderBy(q.FirstName.Ascending)
.GroupBy(q.FirstName, q.HireDate)
```

Using the raw SQL injection techniques above will allow you to invoke SQL functions that we don’t support, including database vender specific SQL, and so on. Hopefully, you will almost never have to resort to writing a custom load method to invoke a stored procedure or an entirely hand written SQL statement. Of course, you can use our native API everywhere and just inject the raw SQL on the GroupBy for instance. You can mix and match to get the desired SQL.

## Old School Syntax
If you prefer you can use the old school syntax which doesn't use the generic methods with the "out var" technique. See the example below:

```c#
EmployeesQuery eQuery = new EmployeesQuery("e");
OrdersQuery o = new OrdersQuery("o");
OrderDetailsQuery od = new OrderDetailsQuery("od");

 eQuery.Select(eQuery.EmployeeID)
.InnerJoin(o).On(eQuery.EmployeeID == o.EmployeeID)
.InnerJoin(od).On(o.OrderID == od.OrderID)
.Where(o.Freight > 20);

EmployeesCollection coll = new EmployeesCollection();
if(coll.Load(eQuery))
{
    // The data was loaded
}
```

## JSON Serialization of Derived Columns
EntitySpaces will serialize any derived columns which are brought back by a query via a JOIN, aggregates, or by creating an extra column on the fly via concatenation such as is done with "fullName" column shown in the example below. Even though there is not a "fullName" property on the Employees object the "fullName" value will still serialize correctly. 

```c#
EmployeesCollection coll = new EmployeesQuery("e", out var e)
.Select
(
    e.EmployeeID, e.LastName, e.FirstName,
    (e.LastName + ", " + e.FirstName).As("fullName") // derived column 
)
.OrderBy(e.LastName.Descending)
.ToCollection<EmployeesCollection>();

if (coll.Count > 0)
{
    string json = JsonConvert.SerializeObject(coll);
}
```

Notice the "fullName" column is present in the JSON, no need for intermediate classes or "newing" up anonymous objects.

```json
[
  {
    "EmployeeID": 6,
    "LastName": "Suyama",
    "FirstName": "Michael",
    "fullName": "Suyama, Michael"
  },
  {
    "EmployeeID": 193,
    "LastName": "Smith",
    "FirstName": "Frank",
    "fullName": "Smith, Frank"
  }
]
``` 

# Modifying Data

## Transaction Support
EntitySpaces is both Hiearchical and Transactional. If you are saving a nested set of hierarchical objects then a transaction is implicitly created for you. However, if you need to save two disparate unrelated objects as shown in the sample below then you should use an esTransactionScope to ensure they both succeed or fail as a unit.

```c#
using (esTransactionScope scope = new esTransactionScope())
{
    Employees employee = new Employees();
    employee.FirstName = "Mike";
    employee.LastName = "Griffin";
    employee.Save();

    Products product = new Products();
    product.ProductName = "Some Gadget";
    product.Save();

    scope.Complete(); // last line of using statement
}
```

In this example below we are using the EntitySpaces hierarchical model and there is no need to declare an esTransactionScope.

```c#
// Create an order
Orders order = new Orders
{
    OrderDate = DateTime.Now
};

// Add an OrderDetails Record to the Order
order.OrderDetailsCollection.Add(new OrderDetails
{
    UnitPrice = 55.00M,
    Quantity = 4,
    ProductID = 8
});

order.Save(); // Saves hierarchically
```

## CRUD Example
```c#
// Create a new Employee
Employees newEmp = new Employees();
newEmp.FirstName = "Joe";
newEmp.LastName = "Smith";
newEmp.Save();

// Load that same Employee
Employees employee = new Employees();
if (employee.LoadByPrimaryKey(newEmp.EmployeeID.Value))
{
    // Modify that Employee
    employee.FirstName = "Bob";
    employee.Save();

    // Delete that Employee
    employee.MarkAsDeleted();
    employee.Save();
}
```

## Collections
Collection are simple enumerable lists of single entities.
```c#
EmployeesCollection coll = new EmployeesCollection();
if (coll.LoadAll())
{
    foreach (Employees emp in coll)
    {
        
    }
}
```

## Supported Operators

Use the native language syntax, it works as you expect it would.

|Operator | Description |
|:-|:-|
| + |plus operator|
| - |minus operator|
| * |multiple operator|
| / |divison operator|
| % |mod operator|
| > |greater-than operator|
| < |less-than operator|
| <= |less-than or equal-to operator|
| >= |greater-than or equal to operator|
| == |equal to operator|
| != |not-equal to operator|
| && |and operator|
| \|\| |or operator|

## Sub Operators

|Sub Operator | Description |
|:-|:-|
| ToUpper() | Convert to lower case|
| ToLower() | Left trim any leading spaces|
| LTrim() | Left trim any trailing spaces|
| RTrim() | Right trim any trailing spaces|
| Trim() | Trim both leading and trailing spaces|
| SubString() | Return a sub-string|
| Coalesce() | Return the first non null evaluating expression|
| Date() | Returns only the date of a datetime type|
| DatePart() | Returns the value of part of a datetime value|
| Length() | Return the length|
| Round() | Rounds the numeric-expression to the desired places after the decimal point|
| Avg() | Average|
| Count() | Count operator|
| Max() | Maximum Value|
| Min() | Minimum Value|
| StdDev() | Standard Deviation|
| Var() | Variance|
| Sum() | Summation|
| Cast() | SQL Cast|

# Setup

1. Install [EntitySpaces Studio](https://github.com/MikeGriffinReborn/EntitySpaces/raw/master/EntitySpaces.Studio/EntitySpacesStudio_20191.1218.0.zip?raw=true/ "Zip File")

## NuGet Package(s)

* SQL Server - [EntitySpaces.ORM.SqlServer](https://www.nuget.org/packages/EntitySpaces.ORM.SqlServer/ "NuGet") 
* SQLite - [EntitySpaces.ORM.SQLite](https://www.nuget.org/packages/EntitySpaces.ORM.SQLite/ "NuGet") 
* MySQL - [EntitySpaces.ORM.MySQL](https://www.nuget.org/packages/EntitySpaces.ORM.MySQL/ "NuGet")
* PostgreSQL - [EntitySpaces.ORM.PostgreSQL](https://www.nuget.org/packages/EntitySpaces.ORM.PostgreSQL)

**Generating your Classes via EntitySpaces Studio**
It's very simple. You only need to execute two templates. The Custom classes are generated only once, that is where you can add custom code and overide EntitySpaces functionality if need be. The Generated classes are generated any time your database schema changes, you never edit these classes.

However, first you will need to go to the "Settings" tab and then the "Connection" tab and connect to your database, there is a dialog box that can help you do that, it's very simple.

<img src="https://raw.githubusercontent.com/MikeGriffinReborn/EntitySpaces/master/docs/Studio.PNG" alt="EntitySpaces Studio" width="632" height="406">

**Setup SQL Server connection string in your C# .NET Project**

```c#
// esDataProviderFactory is a one time setup 
esProviderFactory.Factory = new EntitySpaces.Loader.esDataProviderFactory();

// Add a connection
esConnectionElement conn = new esConnectionElement();
conn.Provider = "EntitySpaces.SqlClientProvider";
conn.DatabaseVersion = "2012";
conn.ConnectionString = "User ID=sa;Password=blank;Initial Catalog=Northwind;Data Source=localhost";
esConfigSettings.ConnectionInfo.Connections.Add(conn);
```

**Setup SQLite connection string in your C# .NET Project**

```c#
// esDataProviderFactory is a one time setup 
esProviderFactory.Factory = new EntitySpaces.Loader.esDataProviderFactory();

// Add a connection
esConnectionElement conn = new esConnectionElement();
conn.Provider = "EntitySpaces.SQLiteProvider";
conn.DatabaseVersion = "2012";
conn.ConnectionString = @"Data Source=C:\MyFolder\Northwind.db3;Version=3;";
esConfigSettings.ConnectionInfo.Connections.Add(conn);
```

**Setup MySQL connection string in your C# .NET Project**

```c#
// esDataProviderFactory is a one time setup 
esProviderFactory.Factory = new EntitySpaces.Loader.esDataProviderFactory();

// Add a connection
esConnectionElement conn = new esConnectionElement();
conn.Provider = "EntitySpaces.MySqlProvider";
conn.DatabaseVersion = "2012";
conn.ConnectionString = "Database=Northwind;Data Source=localhost;User Id=myuser;Password=mypassword;";
esConfigSettings.ConnectionInfo.Connections.Add(conn);
```
