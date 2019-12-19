using BusinessObjects;
using EntitySpaces.Interfaces;
using System;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            esProviderFactory.Factory = new EntitySpaces.Loader.esDataProviderFactory();

            // Add a connection
            esConnectionElement conn = new esConnectionElement();
            conn.Provider = "EntitySpaces.SqlClientProvider";
            conn.DatabaseVersion = "2012";
            conn.ConnectionString = "User ID=sa;Password=blank;Initial Catalog=Northwind;Data Source=localhost";
            esConfigSettings.ConnectionInfo.Connections.Add(conn);

            // Quick test on new syntax
            EmployeesQuery q = new EmployeesQuery("q");
            q.Where(q.EmployeeID > (() =>
                {
                    return new EmployeesQuery("e", out var q1)
                    .Select(q1.EmployeeID)
                    .Where(q1.EmployeeID.IsNotNull()).es.Any();
                })
            );

            EmployeesCollection coll = new EmployeesCollection();
            if (coll.Load(q))
            {

            }

            AddLoadSaveDeleteSingleEntity();
            StreamlinedDynamicQueryAPI();
            CollectionLoadAll();

            SaveEntity();
            UpdateEntity();
            DeleteEntity();

            CollectionSave();
            CollectionSave_BulkInsert();
            CollectionSaveHierarchical();

            GetTheCount();
            GroupBy();
            Concatenation();
            Paging();
            WhereExists();
            CorrelatedSubQuery();
            CorrelatedSubQueryEmbeddedSubQuery();
            SelectAllExcept();
            SelectDistinctTop();
            AliasColumn();
            AndOr();
            Filter();

            Query_Join();
            Subquery();
            CaseWhenThenEnd();
            HavingClause();
        }

        static private void AddLoadSaveDeleteSingleEntity()
        {
            // Add
            Employees newEmp = new Employees();
            newEmp.FirstName = "Joe";
            newEmp.LastName = "Smith";
            newEmp.Save();

            // Load
            Employees employee = new Employees();
            if (employee.LoadByPrimaryKey(newEmp.EmployeeID.Value))
            {
                // Save
                employee.FirstName = "Bob";
                employee.Save();

                // Delete
                employee.MarkAsDeleted();
                employee.Save();
            }
        }

        static private void StreamlinedDynamicQueryAPI()
        {
            Employees emp = new EmployeesQuery("e", out var q)
                .Select(q.EmployeeID, q.FirstName, q.LastName)
                .Where(q.EmployeeID == 5)
                .ToEntity<Employees>();

            EmployeesCollection coll = new EmployeesQuery("e", out var c)
                .Select(c.EmployeeID, c.FirstName, c.LastName)
                .ToCollection<EmployeesCollection>();
        }

        static private void CollectionLoadAll()
        {
            EmployeesCollection coll = new EmployeesCollection();
            if (coll.LoadAll())
            {
                foreach (Employees emp in coll)
                {
                    
                }

            }
        }

        static private void GroupBy()
        {
            OrderDetailsQuery od = new OrderDetailsQuery("od");
            od.Select(od.OrderID, (od.UnitPrice * od.Quantity).Sum().As("OrderTotal"));
            od.GroupBy(od.OrderID);

            OrderDetailsCollection coll = new OrderDetailsCollection();
            if (coll.Load(od))
            {
               
            }
        }

        static private void Concatenation()
        {
            EmployeesQuery q = new EmployeesQuery("e");
            q.Select(q.EmployeeID, (q.LastName + ", " + q.FirstName).As("FullName"));

            EmployeesCollection coll = new EmployeesCollection();
            if (coll.Load(q))
            {
                
            }
        }

        static private void Query_Join()
        {
            OrdersQuery oq = new OrdersQuery("oq");
            oq.InnerJoin<OrderDetailsQuery>("odq", out var odq).On(oq.OrderID == odq.OrderID)
            .Where(odq.Discount > 0)
            .Select(oq.OrderID, odq.Discount);

            OrdersCollection coll = new OrdersCollection();
            if (coll.Load(oq))
            {
                // Lazy loads ...
                foreach (Orders order in coll)
                {
                    /*
                    // LAZY LOADS
                    foreach (OrderDetails orderItem in order.OrderDetailsCollection)
                    {

                    }
                    */
                }
            }
        }

        static private void SaveEntity()
        {
            // The transaction isn't necessary here but demonstrates it's usage
            using (esTransactionScope scope = new esTransactionScope())
            {
                Employees employee = new Employees();
                employee.FirstName = "Mike";
                employee.LastName = "Griffin";
                employee.Save();

                scope.Complete(); // last line of using statement
            }
        }

        static private void UpdateEntity()
        {
            Employees employee = new Employees();
            employee.FirstName = "Mike";
            employee.LastName = "Griffin";
            employee.Save();

            Employees emp = new Employees();
            if (emp.LoadByPrimaryKey(employee.EmployeeID.Value))
            {
                emp.FirstName = "Joe";

                emp.Save();
            }
        }

        static private void DeleteEntity()
        {
            Employees employee = new Employees();
            employee.FirstName = "Mike";
            employee.LastName = "Griffin";
            employee.Save();

            employee.MarkAsDeleted();
            employee.Save();
        }

        static private void CollectionSave()
        {
            EmployeesCollection coll = new EmployeesCollection();
            Employees emp1 = coll.AddNew();
            emp1.FirstName = "Cindi";
            emp1.LastName = "Griffin";

            Employees emp2 = new Employees();
            emp2.FirstName = "Frank";
            emp2.LastName = "Smith";
            emp2.HireDate = DateTime.Now;
            coll.Add(emp2);

            coll.Save();
        }

        static private void CollectionSave_BulkInsert()
        {
            EmployeesCollection coll = new EmployeesCollection();
            Employees emp1 = coll.AddNew();
            emp1.FirstName = "Cindi";
            emp1.LastName = "Griffin";

            Employees emp2 = coll.AddNew();
            emp2.FirstName = "Frank";
            emp2.LastName = "Smith";

            coll.BulkInsert();
        }

        static private void CollectionSaveHierarchical()
        {
            OrdersCollection coll = new OrdersCollection();
            Orders order = coll.AddNew();
            order.OrderDate = DateTime.Now;

            OrderDetails detail1 = order.OrderDetailsCollection.AddNew();
            detail1.UnitPrice = 55.00M;
            detail1.Quantity = 4;
            detail1.ProductID = 8;

            OrderDetails detail2 = order.OrderDetailsCollection.AddNew();
            detail2.UnitPrice = 25.00M;
            detail2.Quantity = 3;
            detail2.ProductID = 4;

            coll.Save();

            int orderId = order.OrderID.Value;
            int detail1_orderId = detail1.OrderID.Value;
            int detail2_orderId = detail2.OrderID.Value;
        }

        static private void GetTheCount()
        {
            EmployeesQuery q = new EmployeesQuery();
            q.Where(q.LastName.Like("%a"));
            q.es.CountAll();

            int count = q.ExecuteScalar<int>();
        }

        static private void AndOr()
        {
            EmployeesQuery q = new EmployeesQuery("e");
            q.Where((q.EmployeeID > 4 && q.EmployeeID < 10) || q.EmployeeID == 100);
            q.Select(q.EmployeeID, (q.LastName + ", " + q.FirstName).As("FullName"));

            EmployeesCollection coll = new EmployeesCollection();
            if (coll.Load(q))
            {
   
            }
        }

        static private void Paging()
        {
            // PageSize and PageNumber
            EmployeesQuery q = new EmployeesQuery("e");
            q.OrderBy(q.HireDate.Descending).es.PageSize(5).es.PageNumber(2);

            EmployeesCollection coll1 = new EmployeesCollection();
            if (coll1.Load(q))
            {
            }

            // Skip and Take
            EmployeesQuery q1 = new EmployeesQuery("e");
            q1.OrderBy(q.HireDate.Descending).Skip(5).Take(20);

            EmployeesCollection coll2 = new EmployeesCollection();
            if (coll2.Load(q1))
            {
            }
        }

        static private void SelectAllExcept()
        {
            EmployeesQuery q = new EmployeesQuery("e");
            q.SelectAllExcept(q.Photo);

            EmployeesCollection coll = new EmployeesCollection();
            if (coll.Load(q))
            {

            }
        }

        static private void SelectDistinctTop()
        {
            EmployeesQuery q = new EmployeesQuery("e");
            q.Select(q.EmployeeID).es.Top(5).es.Distinct();

            EmployeesCollection coll = new EmployeesCollection();
            if (coll.Load(q))
            {
 
            }
        }

        static private void WhereExists()
        {
            // Find all Employees who have no ReportsTo. We could do this via a simple
            // join as well but are demonstrating the Exists() functionality
            EmployeesQuery eq = new EmployeesQuery("e");
            eq.Select(eq.EmployeeID, eq.ReportsTo)
            .Where(eq.Exists(() =>
                {
                    EmployeesQuery subquery = new EmployeesQuery("s");
                    subquery.Select(subquery.EmployeeID)
                    .Where(subquery.ReportsTo.IsNotNull() && subquery.EmployeeID == eq.EmployeeID)
                    .es.Distinct();
                    return subquery;
                })
            );

            EmployeesCollection coll = new EmployeesCollection();
            if (coll.Load(eq))
            {
                // Then we loaded at least one record
            }
        }

        static private void AliasColumn()
        {
            EmployeesQuery q = new EmployeesQuery("e");
            q.Select(q.FirstName.As("MyAlias"));

            EmployeesCollection coll = new EmployeesCollection();
            if (coll.Load(q))
            {

            }
        }

        static private void Filter()
        {
            EmployeesCollection coll = new EmployeesCollection();
            if (coll.LoadAll())
            {
                // Filter on FirstName containing an "a"
                coll.Filter = coll.AsQueryable().Where(d => d.FirstName.Contains("a"));

                foreach (Employees employee in coll)
                {
                    // Each employee's FirstName has an 'a' in
                }

                // Clear the filter
                coll.Filter = null;

                foreach (Employees employee in coll)
                {
                    // All employees are now back in the list
                }
            }
        }

        static private void Subquery()
        {
            OrdersQuery orders = new OrdersQuery("o");
            OrderDetailsQuery details = new OrderDetailsQuery("oi");

            orders.Select
            (
                orders.OrderID,
                orders.OrderDate,
                details.Select
                (
                    details.UnitPrice.Max()
                )
                .Where(orders.OrderID == details.OrderID).As("MaxUnitPrice")
            );

            OrdersCollection coll = new OrdersCollection();
            if (coll.Load(orders))
            {
                foreach (Orders order in coll)
                {

                }
            }
        }

        static private void CorrelatedSubQuery()
        {
            OrderDetailsQuery oiq = new OrderDetailsQuery("oi");
            ProductsQuery pq = new ProductsQuery("p");

            oiq.Select(oiq.OrderID, (oiq.Quantity * oiq.UnitPrice).Sum().As("Total"))
            .Where(oiq.ProductID
                .In(
                    pq.Select(pq.ProductID).Where(oiq.ProductID == pq.ProductID).es.Distinct()
                )
            )
            .GroupBy(oiq.OrderID);

            OrderDetailsCollection coll = new OrderDetailsCollection();
            if (coll.Load(oiq))
            {

            }
        }

        static private void CorrelatedSubQueryEmbeddedSubQuery()
        {
            OrderDetailsQuery oiq = new OrderDetailsQuery("oi");
            oiq.Select(oiq.OrderID, (oiq.Quantity * oiq.UnitPrice).Sum().As("Total"))
            .Where(oiq.ProductID.In(() =>
                {
                    ProductsQuery pq = new ProductsQuery("p");
                    pq.Select(pq.ProductID).Where(oiq.ProductID == pq.ProductID)
                    .es.Distinct();
                    return pq;
                })
            )
            .GroupBy(oiq.OrderID);

            OrderDetailsCollection coll = new OrderDetailsCollection();
            if (coll.Load(oiq))
            {

            }
        }

        static private void CaseWhenThenEnd()
        {
            OrderDetailsQuery oq = new OrderDetailsQuery();

            oq.Select
            (
                oq.Quantity,
                oq.UnitPrice,
                oq.UnitPrice
                    .Case()
                        .When(oq.Quantity < 50).Then(oq.UnitPrice)
                        .When(oq.Quantity >= 50 && oq.Quantity < 70).Then(oq.UnitPrice * .90)
                        .When(oq.Quantity >= 70 && oq.Quantity < 99).Then(oq.UnitPrice * .80)
                        .Else(oq.UnitPrice * .70)
                    .End().As("Adjusted Unit Price")
            ).OrderBy(oq.Quantity.Descending);

            OrderDetailsCollection coll = new OrderDetailsCollection();
            if (coll.Load(oq))
            {

            }

        }

        static private void HavingClause()
        {
            OrderDetailsQuery q = new OrderDetailsQuery();
            q.Select(q.OrderID, q.UnitPrice.Sum().As("TotalUnitPrice"))
            .Where(q.Discount.IsNotNull())
            .GroupBy(q.OrderID)
            .Having(q.UnitPrice.Sum() > 100)
            .OrderBy(q.OrderID.Descending)
            .es.WithNoLock();

            OrderDetailsCollection coll = new OrderDetailsCollection();
            if (coll.Load(q))
            {

            }
        }
    }
}
