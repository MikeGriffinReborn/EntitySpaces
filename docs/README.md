<img src="https://repository-images.githubusercontent.com/194275145/55b5b080-1ccf-11ea-8609-15b9de0d2351" alt="EntitySpaces" width="531" height="268">

Available on Nuget @ [EntitySpaces.ORM.SqlServer](https://www.nuget.org/packages/EntitySpaces.ORM.SqlServer) See the [Setup Section](#setup) for more details ...

# EntitySpaces - A Fluent SQL API
EntitySpaces is a Fluent API for SQL server. If you are familiar with the SQL syntax then you are already an expert in EntitySpaces. EntitiySpaces is also high performance, transactional, and very intuitive. EntitySpaces Studio is used to generate your C# classes from your database schema.

## Example Query
In this example we are going to sum the total # of items for each order. Each order can have many order detail records so we group our query by OrderId and sum up the quantity as 'TotalQuantity'. Notice that we can access the derived 'TotalQuantity' column through the dynamic property.

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

The out is as follows is ...

|OrderID | TotalQuantity |
|-|-|
|10248	 |27|
|10249	 |49|
|10250	 |60|

## Transaction Support
EntitySpaces is both Hiearchical and Transactional. If you are saving a nested set of hierarchical objects then a transaction is implicitly created for you. However, if you need to save two disparate objects as shown in the sample below then you can use an esTransactionScope to ensure they both succeed or fail as a unit.

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

In this case we are using the hierarchical object model and there is no need to declare an esTransactionScope.

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

## JSON Serialization is Smooth
EntitySpaces will serialize any extra columns which are brought back by a query via a JOIN, aggregates, or by creating an extra column on the fly via concatenation such as is done with "fullName" column shown in the example below. Even though there is not a "fullName" property on the Employees object the "fullName" value will still serialize correctly. 

```c#
EmployeesCollection coll = new EmployeesQuery("e", out var e)
.Select
(
    e.EmployeeID, e.LastName, e.FirstName,
    (e.LastName + ", " + e.FirstName).As("fullName") // extra column 
)
.OrderBy(e.LastName.Descending)
.ToCollection<EmployeesCollection>();

if (coll.Count > 0)
{
    string s = JsonConvert.SerializeObject(coll);
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
  },
  {
    "EmployeeID": 191,
    "LastName": "Smith",
    "FirstName": "Frank",
    "fullName": "Smith, Frank"
  }
]
``` 

## InnerJoin, RightJoin, LeftJoin, CrossJoin, and FullJoin

The sample below demonstrates a self join on the Employees table which is looking for all employees with an 'a' in their last name who have people reporting to them. Kind of silly but it shows off the syntax. 

```c#
EmployeesCollection coll = new EmployeesQuery("e", out var e)   // Employees
    .InnerJoin<EmployeesQuery>("r", out var reportsTo).On(e.ReportsTo == reportsTo.EmployeeID)
    .Select(e.EmployeeID, e.LastName, reportsTo.LastName.As("SupervisorName"))
    .Where(reportsTo.LastName.Like("%a%"))
    .OrderBy(reportsTo.LastName.Descending)
    .es.Distinct()
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

# More Dynamic Query Samples

## Select Top

```c#
EmployeesQuery q = new EmployeesQuery();
q.Where(q.ReportsTo.IsNotNull()).OrderBy(q.LastName.Descending).es.Top(1);

EmployeesCollection emp = new EmployeesCollection();
if (emp.Load(q))
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT  TOP 1 * 
FROM [Employees] 
WHERE [ReportsTo] IS NOT NULL 
ORDER BY [LastName] DESC
```

## SelectAllExcept

SelectAllExcept() is not really a SubQuery, just a convenient enhancement that allows you to select all except one or more listed columns.

```c#
// We don't want to bring back the huge photo
EmployeesQuery q = new EmployeesQuery();
q.SelectAllExcept(q.Photo);

EmployeesCollection coll = new EmployeesCollection();
if (coll.Load(q))
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT [EmployeeID],[LastName],[FirstName],[Supervisor],[Age], -- ... not [Photo]
FROM [dbo].[Employee]
```

## Getting the Count

```c#
EmployeesQuery q = new EmployeesQuery();
q.Where(q.ReportsTo.IsNull()).es.CountAll();

int count = q.ExecuteScalar<int>();
```

SQL Generated:

```sql
SELECT COUNT(*) AS 'Count' 
FROM [Employees] 
WHERE [ReportsTo] IS NULL
```

Let's get the count 

## Paging

**Using PageSize and PageNumber**

This is the traditional way of paging and works on all versions of SQL Server. You always need an OrderBy when sorting.

```c#
EmployeesQuery q = new EmployeesQuery();
q.Select(q.EmployeeID, q.LastName)
.OrderBy(q.LastName.Ascending).es.PageNumber(2).es.PageSize(20);

EmployeesCollection coll = new EmployeesCollection();
if (coll.Load(q))
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

**Skip and Take**

Skip and Take Require Microsoft SQL 2012 at a minimum and is a much nicer syntax.

```c#
EmployeesQuery q = new EmployeesQuery();
q.Select(q.EmployeeID, q.LastName).OrderBy(q.LastName.Ascending).Skip(40).Take(20);

EmployeesCollection coll = new EmployeesCollection();
if (coll.Load(q))
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

## With NoLock

```c#
EmployeesQuery e = new EmployeesQuery("e");

e.Select(e.EmployeeID)
.InnerJoin<OrdersQuery>("o", out var o).On(e.EmployeeID == o.EmployeeID)
.Where(o.Freight > 20)
.es.WithNoLock();

EmployeesCollection coll = new EmployeesCollection();
if(coll.Load(oq))
{
    // Then we loaded at least one record
}
```

Notice that even though many query objects are being used you only need to set WithNoLock to true for the parent or main query object. The SQL generated is as follows:

SQL Generated: (Notice that "WITH (NOLOCK)" was applied on both tables involved in the query)

```sql
SELECT e.[EmployeeID]  
FROM [Employees] e WITH (NOLOCK) 
INNER JOIN [Orders] o WITH (NOLOCK) ON e.[EmployeeID] = o.[EmployeeID] 
WHERE o.[Freight] > @Freight1
```

## Distinct

SelectT DISTINCT clause to retrieve the only distinct values in a specified list of columns.

```c#
EmployeesQuery e = new EmployeesQuery("e");

// Employee's who have orders ...
e.Select(e.EmployeeID)
.InnerJoin<OrdersQuery>("o", out var o).On(e.EmployeeID == o.EmployeeID)
.es.Distinct();
```

SQL Generated:

```sql
SELECT DISTINCT e.[EmployeeID]
FROM [Employees] e 
INNER JOIN [Orders] o ON e.[EmployeeID] = o.[EmployeeID]
```

## Any, All, and Some

```c#
CustomersQuery c2 = new CustomersQuery("c2");
c2.Select(c2.PostalCode).Where(c2.Region == "OR").es.All();

CustomersQuery c1 = new CustomersQuery("c1");
c1.Select(c1.CustomerID, c1.CompanyName, c1.PostalCode);
c1.Where(c1.PostalCode > c2); // NOTICE the > on the query C2

CustomersCollection coll = new CustomersCollection();
if(coll.Load(c1))
{

}
```

SQL Generated:

```sql
SELECT c1.[CustomerID], c1.[CompanyName], c1.[PostalCode]
FROM [Customers] c1
WHERE c1.[PostalCode] > ALL
(
    SELECT c2.[PostalCode]
     FROM [Customers] c2
     WHERE c2.[Region] = @Region1
)
```

## The In() and NotIn() - Nesting clauses

```c#
OrdersQuery oQuery = new OrdersQuery("o");
oQuery.Select(oQuery.OrderID, oQuery.EmployeeID)
.InnerJoin<OrderDetailsQuery>("od", out var od).On(oQuery.OrderID == od.OrderID)
.InnerJoin<EmployeesQuery>("e", out var e)
  .On(e.EmployeeID == oQuery.EmployeeID && oQuery.EmployeeID.In(() =>
  {
    EmployeesQuery ee = new EmployeesQuery("ee");
    ee.InnerJoin<OrdersQuery>("eo", out var eo).On(ee.EmployeeID == eo.EmployeeID)
      .InnerJoin<OrderDetailsQuery>("eod", out var eod).On(eo.OrderID == eod.OrderID)
      .Select(eo.EmployeeID)
      .es.Distinct();
    return ee;
  })
);

OrdersCollection coll = new OrdersCollection();
if (coll.Load(oQuery))
{

}
```

SQL Generated:

```sql
SELECT o.[OrderID],  o.[EmployeeID]
FROM [Orders] o
INNER JOIN [Order Details] od ON o.[OrderID] = od.[OrderID]
INNER JOIN [Employees] e ON (e.[EmployeeID] = o.[EmployeeID] AND o.[EmployeeID] IN (
	SELECT DISTINCT eo.[EmployeeID]
	FROM [Employees] ee
	INNER JOIN [Orders] eo ON ee.[EmployeeID] = eo.[EmployeeID]
	INNER JOIN [Order Details] eod ON eo.[OrderID] = eod.[OrderID])
)
```

## The Exists() clause

Exists evaluates to true, if the SubQuery returns a result set.

```c#
EmployeesQuery eq = new EmployeesQuery("e");
eq.Select(eq.EmployeeID, eq.ReportsTo)
.Where(eq.Exists(() =>
{
    // SubQuery of Employees with a null Supervisor column.
    EmployeesQuery sq = new EmployeesQuery("s");
    sq.Select(sq.EmployeeID).Where(sq.ReportsTo.IsNull()).es.Distinct();
    return sq;
}));

EmployeesCollection coll = new EmployeesCollection();
if (coll.Load(eq))
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

## The From() clause

```c#
OrderDetailsQuery od = null;

OrdersQuery o = new OrdersQuery("o");
o.Select(o.CustomerID, o.OrderDate, "<sub.OrderTotal>");
o.From(() =>
{
    od = new OrderDetailsQuery("od");
    od.Select(od.OrderID, (od.UnitPrice * od.Quantity).Sum().As("OrderTotal"))
    .GroupBy(od.OrderID);
    return od;
}).As("sub");
o.InnerJoin(o).On(o.OrderID == od.OrderID);

OrdersCollection collection = new OrdersCollection();
if(collection.Load(o))
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT o.[CustomerID], o.[OrderDate], sub.OrderTotal
FROM  
(
   SELECT od.[OrderID], SUM((od.[UnitPrice] * od.[Quantity])) AS 'OrderTotal'
   FROM [Order Details] od
   GROUP BY od.[OrderID]
) AS sub
INNER JOIN [Orders] o ON o.[OrderID] = sub.[OrderID]
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

## Select SubQuery

A SubQuery in a Select clause must return a single value.

```c#
OrdersCollection coll = new OrdersQuery("o", out var orders)
.Select
(
    orders.OrderID, orders.OrderDate,
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

This is the same as the query above, but returns all columns in the Order table, instead of just OrderID and OrderDate. Notice that the Select clause contains orders, not orders.. The SQL produced will use the supplied alias o..

```c#
OrderQuery orders = new OrderQuery("o");
OrderItemQuery details = new OrderItemQuery("oi");

orders.Select
(
    orders, // this means orders.*
    details.Select
    (
        details.UnitPrice.Max()
    )
    .Where(orders.OrderID == details.OrderID).As("MaxUnitPrice")
);

OrderCollection coll = new OrderCollection();
if(coll.Load(orders))
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT o.* 
(
    SELECT MAX(oi.[UnitPrice]) AS 'UnitPrice'  
    FROM [dbo].[OrderItem] oi 
    WHERE o.[OrderID] = oi.[OrderID]
) AS MaxUnitPrice  
FROM [ForeignKeyTest].[dbo].[Order] o
```

## From SubQuery

An aggregate requires a GROUP BY for each column in the SELECT that is not an aggregate. Sometimes you wish to include columns in your result set that you do not wish to group by. One way to accomplish this is by using a SubQuery in the From clause that contains the aggregate the way you want it grouped. The outer query contains the results of the aggregate, plus any additional columns.

If you use a SubQuery in a From clause, you must give the From clause its own alias (shown below as "sub"). In the outer query, to refer to an aliased element in the From SubQuery, use the inline raw SQL technique to qualify the aggregate's alias with the From clause alias, i.e., "".

```c#
OrderQuery oq = new OrderQuery("o");
OrderItemQuery oiq = new OrderItemQuery("oi");

oq.Select(oq.CustID, oq.OrderDate, "<sub.OrderTotal>");
oq.From
(
    oiq.Select(oiq.OrderID, (oiq.UnitPrice * oiq.Quantity).Sum().As("OrderTotal"))
    .GroupBy(oiq.OrderID)
).As("sub");
oq.InnerJoin(oq).On(oq.OrderID == oiq.OrderID);

OrderCollection coll = new OrderCollection();
if(coll.Load(oq))
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT o.[CustID],o.[OrderDate],sub.OrderTotal  
FROM 
(
    SELECT oi.[OrderID],
    SUM((oi.[UnitPrice]*oi.[Quantity])) AS 'OrderTotal'  
    FROM [dbo].[OrderItem] oi 
    GROUP BY oi.[OrderID]
) AS sub 
INNER JOIN [dbo].[Order] o ON o.[OrderID] = sub.[OrderID]
```

## Where SubQuery

In and NotIn are two of the most common operators used in a Where SubQuery. The following produces a result set containing Territories that an Employee is not associated with.

```c#
// Territories that Employee 1 is not assigned to.
TerritoriesQuery tq = new TerritoriesQuery("t");
tq.Select(tq.TerritoryID, tq.TerritoryDescription);
tq.Where(tq.TerritoryID.NotIn(() =>
{
    EmployeeTerritoriesQuery etq = new EmployeeTerritoriesQuery("et");
    etq.Select(etq.TerritoryID);
    etq.Where(etq.EmployeeID == 1);
    return etq;
}));

TerritoriesCollection coll = new TerritoriesCollection();
if (coll.Load(tq))
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

Exists evaluates to true, if the SubQuery returns a result set.


```c#
// If even one employee has a null supervisor,
// i.e., the above query has a result set,
// then run a list of all employees.
EmployeesQuery eq = new EmployeesQuery("e");
eq.Select(eq.EmployeeID, eq.ReportsTo)
.Where(eq.Exists(() =>
{
    // SubQuery of Employees with a null Supervisor column.
    EmployeesQuery sq = new EmployeesQuery("s");
    sq.Select(sq.EmployeeID).Where(sq.ReportsTo.IsNull()).es.Distinct();
    return sq;
}));

EmployeesCollection coll = new EmployeesCollection();
if (coll.Load(eq))
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT e.[EmployeeID],e.[Supervisor]  
FROM [dbo].[Employee] e 
WHERE EXISTS 
(
    SELECT DISTINCT s.[EmployeeID]  
    FROM [dbo].[Employee] s 
    WHERE s.[Supervisor] IS NULL
)
```

SubQueries cannot be used directly within a Join(SubQuery) clause, but they can be used within a Join(query).On(SubQuery) clause.

```c#
// Query for the Join
OrderItemQuery oiq = new OrderItemQuery("oi");

// SubQuery of OrderItems with a discount
OrderItemQuery oisq = new OrderItemQuery("ois");
oisq.Select(oisq.Discount).Where(oisq.Discount > 0).es.Distinct();

// Orders with discounted items
OrderQuery oq = new OrderQuery("o");
oq.Select(oq.OrderID, oiq.Discount);
oq.InnerJoin(oiq). On(oq.OrderID == oiq.OrderID && oiq.Discount.In(oisq));

OrderCollection coll = new OrderCollection();
if(coll.Load(oq))
{
    // Then we loaded at least one record
}   
```

SQL Generated:

```sql
SELECT o.[OrderID],oi.[Discount]  
FROM [dbo].[Order] o 
INNER JOIN [dbo].[OrderItem] oi 
ON (o.[OrderID] = oi.[OrderID] AND oi.[Discount] IN  
(
    SELECT  DISTINCT ois.[Discount]  
    FROM [dbo].[OrderItem] ois 
    WHERE ois.[Discount] > @Discount1)
)
```

## Correlated SubQuery

A correlated SubQuery is where the inner query relies on an element of the outer query. The inner select cannot run on its own. Below, the inner pq query uses the outer query's oiq.ProductID in the Where() clause.

```c#
OrderItemQuery oiq = new OrderItemQuery("oi");
ProductQuery pq = new ProductQuery("p");

oiq.Select(
    oiq.OrderID,
    (oiq.Quantity * oiq.UnitPrice).Sum().As("Total")
);
oiq.Where(oiq.ProductID
    .In(
        pq.Select(pq.ProductID)
        .Where(oiq.ProductID == pq.ProductID)
    )
);
oiq.GroupBy(oiq.OrderID);

OrderItemCollection coll = new OrderItemCollection();
if(coll.Load(oiq))
{
    // Then we loaded at least one record
}   
```

SQL Generated:

```sql
SELECT oi.[OrderID],SUM((oi.[Quantity]*oi.[UnitPrice])) AS 'Total'  
FROM [dbo].[OrderItem] oi 
WHERE oi.[ProductID] IN 
(
    SELECT p.[ProductID]  
    FROM [dbo].[Product] p 
    WHERE oi.[ProductID] = p.[ProductID]
)  
GROUP BY oi.[OrderID]
```

## Nested SubQuery

EntitySpaces supports nesting of SubQueries. Each database vendor has their own limits on just how deep the nesting can go. EntitySpaces supports two different syntax approaches to nested SubQueries.

Traditional SQL-style syntax is most useful if you already have a query designed using standard SQL, and are just converting it to a DynamicQuery.

```c#
OrderQuery oq = new OrderQuery("o");
CustomerQuery cq = new CustomerQuery("c");
EmployeeQuery eq = new EmployeeQuery("e");

// OrderID and CustID for customers who ordered on the same date
// a customer was added, and have a manager whose 
// last name starts with 'S'.
oq.Select(
    oq.OrderID,
    oq.CustID
);
oq.Where(oq.OrderDate
    .In(
        cq.Select(cq.DateAdded)
        .Where(cq.Manager.In(
            eq.Select(eq.EmployeeID)
            .Where(eq.LastName.Like("S%"))
            )
        )
    )
);

OrderCollection coll = new OrderCollection();
if(coll.Load(oq))
{
    // Then we loaded at least one record
}   
```

SQL Generated:

```sql
SELECT o.[OrderID],o.[CustID]  
FROM [dbo].[Order] o 
WHERE o.[OrderDate] IN 
(
    SELECT c.[DateAdded]  
    FROM [dbo].[Customer] c 
    WHERE c.[Manager] IN 
    (
        SELECT e.[EmployeeID]  
        FROM [dbo].[Employee] e 
        WHERE e.[LastName] LIKE @LastName1
    ) 
)
```
Nesting by query instance name can be easier to understand and construct, if you are starting from scratch, and have no pre-existing SQL to go by. The trick is to start with the inner-most SubQuery and work your way out. The query below produces the same results as the traditional SQL-style query above. The instance names are color coded to emphasize how they are nested.

```c#
// Employees whose LastName begins with 'S'.
EmployeeQuery eq = new EmployeeQuery("e");
eq.Select(eq.EmployeeID);
eq.Where(eq.LastName.Like("S%"));

// DateAdded for Customers whose Managers are in the
// EmployeeQuery above.
CustomerQuery cq = new CustomerQuery("c");
cq.Select(cq.DateAdded);
cq.Where(cq.Manager.In(eq));

// OrderID and CustID where the OrderDate is in the
// CustomerQuery above.
OrderQuery oq = new OrderQuery("o");
oq.Select(oq.OrderID, oq.CustID);
oq.Where(oq.OrderDate.In(cq));

OrderCollection coll = new OrderCollection();
if(coll.Load(oq))
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT o.[OrderID],o.[CustID]  
FROM [dbo].[Order] o 
WHERE o.[OrderDate] IN 
(
    SELECT c.[DateAdded]  
    FROM [dbo].[Customer] c 
    WHERE c.[Manager] IN 
    (
        SELECT e.[EmployeeID]  
        FROM [dbo].[Employee] e 
        WHERE e.[LastName] LIKE @LastName1
    )
)
```

## Any, All, and Some

ANY, ALL, and SOME are SubQuery qualifiers. They precede the SubQuery they apply to. For most databases, ANY and SOME are synonymous. Usually, if you use an operator (>, >=, =, <, <=) in a Where clause against a SubQuery, then the SubQuery must return a single value. By applying a qualifier to the SubQuery, you can use operators against SubQueries that return multiple results.

Notice, below, that the ALL qualifier is set to true for the SubQuery with "cq.es.All = true;".

```c#
// DateAdded for Customers whose Manager  = 3
CustomerQuery cq = new CustomerQuery("c");
cq.Select(cq.DateAdded).Where(cq.Manager == 3).es.All();

// OrderID and CustID where the OrderDate is 
// less than all of the dates in the CustomerQuery above.
OrderQuery oq = new OrderQuery("o");
oq.Select(oq.OrderID, oq.CustID);
oq.Where(oq.OrderDate < cq);

OrderCollection coll = new OrderCollection();
if(coll.Load(oq))
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT o.[OrderID],o.[CustID]  
FROM [dbo].[Order] o 
WHERE o.[OrderDate] < ALL 
(
    SELECT c.[DateAdded]  
    FROM [ForeignKeyTest].[dbo].[Customer] c 
    WHERE c.[Manager] = @Manager1
)
```
Below, is a nested SubQuery. The ANY qualifier is set to true for the middle SubQuery with "cq.es.Any = true;".

```c#
// Employees whose LastName begins with 'S'.
EmployeeQuery eq = new EmployeeQuery("e");
eq.Select(eq.EmployeeID).Where(eq.LastName.Like("S%"));

// DateAdded for Customers whose Managers are in the
// EmployeeQuery above.
CustomerQuery cq = new CustomerQuery("c");
cq.Select(cq.DateAdded).Where(cq.Manager.In(eq)).es.Any();

// OrderID and CustID where the OrderDate is 
// less than any one of the dates in the CustomerQuery above.
OrderQuery oq = new OrderQuery("o");
oq.Select(oq.OrderID, oq.CustID).Where(oq.OrderDate < cq);

OrderCollection coll = new OrderCollection();
if(coll.Load(oq))
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT o.[OrderID],o.[CustID]  
FROM [dbo].[Order] o 
WHERE o.[OrderDate] < ANY 
(
    SELECT c.[DateAdded]  
    FROM [dbo].[Customer] c 
    WHERE c.[Manager] IN 
    (
        SELECT e.[EmployeeID]  
        FROM [dbo].[Employee] e 
        WHERE e.[LastName] LIKE @LastName1
    )
)
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
EmployeeQuery q = new EmployeeQuery();
q.Select(q.EmployeeID, q.Age.Sum().As("TotalAge"))
.Where(q.EmployeeID.IsNotNull())
.GroupBy(q.EmployeeID)
.Having(q.Age.Sum() > 5)
.OrderBy(q.EmployeeID.Descending);

EmployeeCollection coll = new EmployeeCollection();
if(coll.Load(q))
{
    // Then we loaded at least one record
}
```

SQL Generated:

```sql
SELECT [EmployeeID] AS 'EmployeeID',SUM([Age]) AS 'TotalAge' 
FROM [dbo].[Employee] 
WHERE[EmployeeID] IS NOT NULL 
GROUP BY [EmployeeID] 
HAVING SUM([Age]) > @Age2 
ORDER BY [EmployeeID] DESC
```

## Union, Intersect, and Except
These might be kind of silly but they demonstrate syntax.

## Union

```c#
EmployeeQuery eq1 = new EmployeeQuery("eq1");
EmployeeQuery eq2 = new EmployeeQuery("eq2");

// This leaves out the record with Age 30
eq1.Where(eq1.Age < 30);
eq1.Union(eq2);
eq2.Where(eq2.Age > 30);
```

## Intersect

```c#
EmployeeQuery eq1 = new EmployeeQuery("eq1");
EmployeeQuery eq2 = new EmployeeQuery("eq2");

// This leaves out the record with Age 30
eq1.Where(eq1.FirstName.Like("%n%"));
eq1.Intersect(eq2);
eq2.Where(eq2.FirstName.Like("%a%"));
```

## Except

```c#
EmployeeQuery eq1 = new EmployeeQuery("eq1");
EmployeeQuery eq2 = new EmployeeQuery("eq2");

// This leaves out the record with Age 30
eq1.Where(eq1.FirstName.Like("%J%"));
eq1.Except(eq2);
eq2.Where(eq2.FirstName == "Jim");
```

## Raw SQL Injection Everywhere
There may be times when you need to access some SQL feature that is not supported by the DynamicQuery API. But, now having used and fallen in love with DynamicQuery, the last thing you want to do is stop and go write a stored procedure or create a view. We have always supported the raw injection feature in our Select statement, but it will soon be available almost everywhere. The way it works is you pass in raw SQL in the form of a string surrounded by < > angle brackets. That indicates that you want the raw SQL passed directly to the database engine “as is”.

Here is an example query. You would never write a query like this in reality. Tiraggo supports this simple query without having to use < > angle brackets. This is just to show all of the places that can accept the raw SQL injection technique:

```c#
EmployeesQuery q = new EmployeesQuery();
q.Select("<FirstName>", q.HireDate)
.Where("<EmployeeID = 1>")
.GroupBy("<FirstName>", q.HireDate)
.OrderBy("<FirstName ASC>"); 

EmployeeCollection coll = new EmployeeCollection();
if(coll.Load(q))
{
    // Then we loaded at least one record
}
```

The SQL Generated is as follows (and works)

SQL Generated:

```sql
SELECT FirstName,[HireDate] AS 'HireDate'  
FROM [Employees] WHERE (EmployeeID = 1) 
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

# Setup

1. Install [EntitySpaces Studio](https://github.com/MikeGriffinReborn/EntitySpaces/blob/master/EntitySpaces.Studio/EntitySpacesStudio_2019.0.1214.0.zip?raw=true/ "Zip File")

2. Install the [EntitySpaces.ORM.SqlServer](https://www.nuget.org/packages/EntitySpaces.ORM.SqlServer/ "NuGet") for the SQL Server NuGet package into your Visual Studio project.

**Generating your Classes via EntitySpaces Studio**
It's very simple. You only need to execute two templates. The Custom classes are generated only once, that is where you can add custom code and overide EntitySpaces functionality if need be. The Generated classes are generated any time your database schema changes, you never edit these classes.

However, first you will need to go to the "Settings" tab and then the "Connection" tab and connect to your database, there is a dialog box that can help you do that, it's very simple.

<img src="docs\Studio.PNG" alt="EntitySpaces Studio" width="632" height="406">

**Setup SQL Connection in your C# .NET Project**

```c#
// esDataProviderFactory is a one time setup 
esProviderFactory.Factory = new EntitySpaces.Loader.esDataProviderFactory();

// Add a connection
esConnectionElement conn = new esConnectionElement();
conn.Name = "RemoteDb";
conn.ProviderMetadataKey = "esDefault";
conn.Provider = "EntitySpaces.SqlClientProvider";
conn.ProviderClass = "DataProvider";
conn.SqlAccessType = esSqlAccessType.DynamicSQL;
conn.ConnectionString = 
   "User ID=mydmin;Password=abc123;Initial Catalog=Northwind;Data Source=localhost";
conn.DatabaseVersion = "2017";
esConfigSettings.ConnectionInfo.Connections.Add(conn);

// Assign the Default Connection
esConfigSettings.ConnectionInfo.Default = "RemoteDb";
```
