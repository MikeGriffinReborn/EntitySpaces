/*  New BSD License
-------------------------------------------------------------------------------
Copyright (c) 2006-2012, EntitySpaces, LLC
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the EntitySpaces, LLC nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL EntitySpaces, LLC BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
-------------------------------------------------------------------------------
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

#if (WCF)
using System.Runtime.Serialization;
#endif


namespace EntitySpaces.DynamicQuery
{
    /// <summary>
    /// This provides the Dynamic Query mechanism used by your Business object (Employees),
    /// collection (EmployeesCollection), and query caching (EmployeesQuery).
    /// </summary>
    /// <example>
    /// DynamicQuery allows you to (without writing any stored procedures)
    /// query your database on the fly. All selection criteria are passed in
    /// via Parameters (SAParameter, OleDbParameter) in order to prevent
    /// sql injection techniques often attempted by hackers.  
    /// Additional examples are provided here:
    /// <code>
    /// http://www.entityspaces.net/portal/QueryAPISamples/tabid/80/Default.aspx
    /// </code>
    /// <code>
    /// EmployeesCollection emps = new EmployeesCollection;
    /// 
    /// emps.Query.es.CountAll = true;
    /// emps.Query.Select
    /// (
    ///		emps.Query.LastName,
    ///		emps.Query.FirstName
    /// )
    /// .Where
    /// (
    ///		emps.Query.Or
    ///		(
    ///			emps.Query.LastName.Like("%A%"),
    ///			emps.Query.LastName.Like("%O%")
    ///		),
    ///		emps.Query.BirthDate.Between("1940-01-01", "2006-12-31")
    /// )
    /// .GroupBy
    /// (
    ///		emps.Query.LastName,
    ///		emps.Query.FirstName
    /// )
    /// .OrderBy
    /// (
    ///		emps.Query.LastName.Descending,
    ///		emps.Query.FirstName.Ascending
    /// );
    /// 
    /// emps.Query.Load();
    /// </code>
    /// </example>
#if !SILVERLIGHT    
    [Serializable]
#endif
#if (WCF)
    [DataContract(Namespace = "es", IsReference = true)]
#endif
    public class esDynamicQuerySerializable : IDynamicQuerySerializableInternal
    {
        /// <summary>
        /// The Constructor
        /// </summary>
        public esDynamicQuerySerializable()
        {

        }

        /// <summary>
        /// The Constructor used when using this query in a "Join"
        /// </summary>
        /// <param name="joinAlias">The alias of the associated Table to be used in the "Join"</param>
        public esDynamicQuerySerializable(string joinAlias)
        {
            this.joinAlias = joinAlias;
        }

        /// <summary>
        /// Called whenever the Entity needs a connection. This can be used to override the default connection 
        /// per object manually, or automatically by filling in the "Connection Name" on the "Generated Master"
        /// template. 
        /// </summary>
        /// <returns></returns>
        virtual protected string GetConnectionName()
        {
            return null;
        }

        /// <summary>
        /// Used during DataContract Serializaton
        /// </summary>
        /// <returns>The Name of the Query such as EmployeesQuery and not EmployeesQueryProxyStub</returns>
        virtual protected string GetQueryName()
        {
            return null;
        }

        virtual protected esQueryItem QueryItemFromName(string name)
        {
            return null;
        }

        /// <summary>
        /// Used by EntitySpaces internally
        /// </summary>
        /// <param name="query">The SubQuery</param>
        internal void AddQueryToList(esDynamicQuerySerializable query)
        {
            if (!queries.ContainsKey(query.joinAlias) && query != this)
            {
                if (subQueryNames == null) subQueryNames += string.Empty;
                if (subQueryNames.Length > 0) subQueryNames += ";";
                subQueryNames += query.GetQueryName();

                queries[query.joinAlias] = query;
            }
        }

        /// <summary>
        /// The query's SELECT clause. SELECT * by default (all columns).
        /// </summary>
        /// <example>
        /// <code>
        /// emps.Query.Select();
        /// </code>
        /// </example>
        /// <returns>An esDynamicQueryTransport containing a select clause.</returns>
        public esDynamicQuerySerializable Select()
        {
            return this;
        }

        /// <summary>
        /// A query SELECT clause for the specified column.
        /// </summary>
        /// <example>
        /// <code>
        /// emps.Query.Select(emps.Query.LastName);
        /// </code>
        /// </example>
        /// <param name="column">The column to place in the select statement.</param>
        /// <returns>An esDynamicQueryTransport containing a select clause.</returns>
        public esDynamicQuerySerializable Select(object obj)
        {
            if (this.selectColumns == null)
            {
                this.selectColumns = new List<esExpression>();
            }

            esExpression sItem = obj as esExpression;
            if (sItem == null)
            {
                esQueryItem queryItem = null;

                esCast cast = obj as esCast;
                if (cast != null)
                {
                    queryItem = cast.item;
                }
                else
                {
                    queryItem = obj as esQueryItem;
                }

                // This if conditional is this way intentionally
                if(queryItem is object)
                {
                    sItem = queryItem;
                }
                else
                {
                    sItem = new esExpression();
                    esDynamicQuerySerializable query = obj as esDynamicQuerySerializable;
                    if (query != null)
                    {
                        AddQueryToList(query);
                        sItem.Query = query;
                    }
                    else
                    {
                        sItem.Column.Name = obj as string;
                    }
                }
            }

            this.selectColumns.Add(sItem);

            return this;
        }

        /// <summary>
        /// A query SELECT clause for columns or aggregates.
        /// </summary>
        /// <example>
        /// See <see cref="esQuerySubOperatorType"/> Enumeration.
        /// <code>
        /// emps.Query.Select
        /// (
        ///		emps.Query.LastName,
        ///		emps.Query.Age.Avg("Average Age")
        /// );
        /// </code>
        /// </example>
        /// <param name="columns">The list of columns or aggregates to place in the select statement.</param>
        /// <returns>An esDynamicQueryTransport containing a select clause.</returns>
        public esDynamicQuerySerializable Select(params object[] columns)
        {
            if (this.selectColumns == null)
            {
                this.selectColumns = new List<esExpression>();
            }

            foreach (object obj in columns)
            {
                esQueryItem queryItem = null;
                esExpression sItem;

                esCast cast = obj as esCast;
                if (cast != null)
                {
                    queryItem = cast.item;
                }
                else
                {
                    queryItem = obj as esQueryItem;
                }

                // This if conditional is this way intentionally
                if(queryItem is object)
                {
                    sItem = queryItem;
                }
                else
                {
                    sItem = new esExpression();
                    esDynamicQuerySerializable query = obj as esDynamicQuerySerializable;
                    if (query != null)
                    {
                        AddQueryToList(query);
                        sItem.Query = query;
                    }
                    else
                    {
                        sItem.Column.Name = obj as string;
                    }
                }

                this.selectColumns.Add(sItem);
            }

            return this;
        }

        /// <summary>
        /// This method will create a Select statement for all of the columns in the entity except for the ones passed in.
        /// This is very useful when you want to eliminate blobs and other fields for performance.
        /// </summary>
        /// <param name="columns">The columns which you wish to exclude from the Select statement</param>
        /// <returns></returns>
        virtual public esDynamicQuerySerializable SelectAllExcept(params esQueryItem[] columns)
        {
            if (m_selectAllExcept == null)
            {
                m_selectAllExcept = new List<esQueryItem>();
            }

            foreach (esQueryItem item in columns)
            {
                m_selectAllExcept.Add(item);
            }
            return this;
        }

        /// <summary>
        /// This method will select all of the columns explicity by name that were present when you generated your
        /// classes as opposed to doing a SELECT *
        /// </summary>
        /// <returns></returns>
        virtual public esDynamicQuerySerializable SelectAll()
        {
            m_selectAll = true;
            return this;
        }

        /// <summary>
        /// Use this method in conjuction with Take()
        /// </summary>
        /// <param name="count">The number of rows to skip</param>
        /// <returns></returns>
        public esDynamicQuerySerializable Skip(int count)
        {
            this.skip = count;
            return this;
        }

        /// <summary>
        /// Use this method in conjuction with Skip()
        /// </summary>
        /// <param name="count">The number of rows to return</param>
        /// <returns></returns>
        public esDynamicQuerySerializable Take(int count)
        {
            this.take = count;
            return this;
        }

        /// <summary>
        /// Creates an INNER JOIN 
        /// </summary>
        /// <param name="joinQuery">This query represents the table you are joining to</param>
        /// <returns>An esJoinItem, which you then call the On() method.</returns>
        public esJoinItem InnerJoin(esDynamicQuerySerializable joinQuery)
        {
            return JoinCommon(joinQuery, esJoinType.InnerJoin);
        }

        /// <summary>
        /// Creates a LEFT JOIN 
        /// </summary>
        /// <param name="joinQuery">This query represents the table you are joining to</param>
        /// <returns>An esJoinItem, which you then call the On() method.</returns>
        public esJoinItem LeftJoin(esDynamicQuerySerializable joinQuery)
        {
            return JoinCommon(joinQuery, esJoinType.LeftJoin);
        }

        /// <summary>
        /// Creates a RIGHT JOIN 
        /// </summary>
        /// <param name="joinQuery">This query represents the table you are joining to</param>
        /// <returns>An esJoinItem, which you then call the On() method.</returns>
        public esJoinItem RightJoin(esDynamicQuerySerializable joinQuery)
        {
            return JoinCommon(joinQuery, esJoinType.RightJoin);
        }

        /// <summary>
        /// Creates a FULL JOIN 
        /// </summary>
        /// <param name="joinQuery">This query represents the table you are joining to</param>
        /// <returns>An esJoinItem, which you then call the On() method.</returns>
        public esJoinItem FullJoin(esDynamicQuerySerializable joinQuery)
        {
            return JoinCommon(joinQuery, esJoinType.FullJoin);
        }

        /// <summary>
        /// Creates an INNER JOIN 
        /// </summary>
        /// <param name="joinQuery">This query represents the table you are joining to</param>
        /// <returns>An esJoinItem, which you then call the On() method.</returns>
        public esJoinItem CrossJoin(esDynamicQuerySerializable joinQuery)
        {
            return JoinCommon(joinQuery, esJoinType.CrossJoin);
        }

        /// <summary>
        /// Used by all of the join functions as all of the logic is the same except for 
        /// the type of join
        /// </summary>
        /// <param name="joinQuery">This query represents the table you are joining to</param>
        /// <param name="joinType">The type of join, ie, Inner, Left, Right, Full</param>
        /// <returns></returns>
        private esJoinItem JoinCommon(esDynamicQuerySerializable joinQuery, esJoinType joinType)
        {
            if (joinQuery.joinAlias == " ")
            {
                throw new Exception("Your DynamicQuery must have an Alias, use the alternate constructor");
            }

            if (this.joinItems == null)
            {
                this.joinItems = new List<esJoinItem>();
            }

            esJoinItem joinItem = new esJoinItem(this);
            joinItem.data.Query = joinQuery;
            joinItem.data.JoinType = joinType;

            this.joinItems.Add(joinItem);

            AddQueryToList(joinQuery);

            return joinItem;
        }

        /// <summary>
        /// Allows you to pass in a SubQuery for your FROM statement
        /// </summary>
        /// <param name="fromQuery">The subquery to use as your FROM statement</param>
        /// <returns></returns>
        public esDynamicQuerySerializable From(esDynamicQuerySerializable fromQuery)
        {
            this.fromQuery = fromQuery;
            AddQueryToList(fromQuery);
            return this;
        }

        /// <summary>
        /// Performs a UNION between two statements
        /// </summary>
        /// <returns></returns>
        public esDynamicQuerySerializable Union(esDynamicQuerySerializable query)
        {
            esSetOperation setOperation = new esSetOperation(query);
            setOperation.SetOperationType = esSetOperationType.Union;

            if(setOperations == null)
            {
                setOperations = new List<esSetOperation>();
            }

            setOperations.Add(setOperation);

            return this;
        }

        /// <summary>
        /// Performs a UNION ALL between two statements
        /// </summary>
        /// <returns></returns>
        public esDynamicQuerySerializable UnionAll(esDynamicQuerySerializable query)
        {
            esSetOperation setOperation = new esSetOperation(query);
            setOperation.SetOperationType = esSetOperationType.UnionAll;

            if (setOperations == null)
            {
                setOperations = new List<esSetOperation>();
            }

            setOperations.Add(setOperation);

            return this;
        }

        /// <summary>
        /// Peforms an INTERSECT between two statements
        /// </summary>
        /// <returns></returns>
        public esDynamicQuerySerializable Intersect(esDynamicQuerySerializable query)
        {
            esSetOperation setOperation = new esSetOperation(query);
            setOperation.SetOperationType = esSetOperationType.Intersect;

            if (setOperations == null)
            {
                setOperations = new List<esSetOperation>();
            }

            setOperations.Add(setOperation);

            return this;
        }

        /// <summary>
        /// Performs an EXCEPT between two statements
        /// </summary>
        /// <returns></returns>
        public esDynamicQuerySerializable Except(esDynamicQuerySerializable query)
        {
            esSetOperation setOperation = new esSetOperation(query);
            setOperation.SetOperationType = esSetOperationType.Except;

            if (setOperations == null)
            {
                setOperations = new List<esSetOperation>();
            }

            setOperations.Add(setOperation);

            return this;
        }

        /// <summary>
        /// Provide an Alias for your query or subquery
        /// </summary>
        /// <param name="subQueryAlias"></param>
        /// <returns></returns>
        public esDynamicQuerySerializable As(string subQueryAlias)
        {
            if (this.fromQuery == null)
            {
                this.subQueryAlias = subQueryAlias;
            }
            else
            {
                this.fromQuery.subQueryAlias = subQueryAlias;
            }
            return this;
        }

        /// <summary>
        /// A query WHERE clause. The default conjunction is "AND".
        /// </summary>
        /// <example>
        /// See <see cref="esConjunction"/> Enumeration.
        /// <code>
        /// emps.Query.Where
        /// (
        ///		emps.Query.LastName == "Doe" &amp;&amp; emps.Query.FirstName.Like("J%")
        /// );
        /// </code>
        /// </example>
        /// <param name="items">The list of objects to place in the where statement.</param>
        /// <returns>An esDynamicQueryTransport containing a where clause.</returns>
        public esDynamicQuerySerializable Where(params object[] items)
        {
            bool first = true;

            if (this.whereItems == null)
            {
                this.whereItems = new List<esComparison>();
            }
            else
            {
                if (!isExplicitParenthesis)
                {
                    whereItems.Add(new esComparison(this.defaultConjunction));
                }
                else
                {
                    isExplicitParenthesis = false;
                }
            }

            foreach (object item in items)
            {
                esComparison wi = item as esComparison;

                if (wi != null)
                {
                    if (wi.Parenthesis == esParenthesis.Open)
                    {
                        isExplicitParenthesis = true;
                    }
                    else if (wi.Parenthesis == esParenthesis.Close)
                    {
                        this.whereItems.RemoveAt(this.whereItems.Count - 1);
                    }

                    if (wi.data.WhereExpression != null)
                    {
                        foreach (esComparison expItem in wi.data.WhereExpression)
                        {
                            if (expItem.Value != null)
                            {
                                esDynamicQuerySerializable q = expItem.data.Value as esDynamicQuerySerializable;
                                if (q != null)
                                {
                                    AddQueryToList(q);
                                }
                            }
                        }

                        this.whereItems.AddRange(wi.data.WhereExpression);
                    }
                    else
                    {
                        if (!first)
                        {
                            this.whereItems.Add(new esComparison(this.defaultConjunction));
                        }
                        this.whereItems.Add(wi);
                    }

                    esDynamicQuerySerializable query = wi.Value as esDynamicQuerySerializable;

                    if (query != null)
                    {
                        AddQueryToList(query);
                    }
                }
                else
                {
                    if (!first)
                    {
                        this.whereItems.Add(new esComparison(this.defaultConjunction));
                    }
                    this.whereItems.AddRange(ProcessWhereItems(this.defaultConjunction, item));
                }
                first = false;
            }

            return this;
        }

        /// <summary>
        /// This is not the preferred way to build a where clause. The preferred way is to use the Where() method. This is typically used when the UI allows for the user to pick any number of filters to operate on.
        /// </summary>
        /// <param name="column">This is actual the Propery name of the Entity, not the physical column name in the database table, althought they might be the same</param>
        /// <param name="operation">"EQUAL", "NOTEQUAL", "GREATERTHAN", "GREATERTHANOREQUAL", "LESSTHAN", "LESSTHANOREQUAL", "LIKE", "ISNULL", "ISNOTNULL", "BETWEEN", "IN", "NOTIN", "NOTLIKE", "CONTAINS"</param>
        /// <param name="criteria">The value the operation is to act upon</param>
        /// <param name="criteria2">the 2nd value of the "BETWEEN" operation</param>
        /// <param name="conjuction">Either "AND" or "OR"</param>
        /// <returns></returns>
        public esComparison ManualWhere(string column, string operation, object criteria, object criteria2, string conjuction)
        {
            var v = null as esComparison;

            switch (operation.ToUpper())
            {
                case "EQUAL":
                    v = this.QueryItemFromName(column) == criteria;
                    break;

                case "NOTEQUAL":
                    v = this.QueryItemFromName(column) != criteria;
                    break;

                case "GREATERTHAN":
                    v = this.QueryItemFromName(column) > criteria;
                    break;

                case "GREATERTHANOREQUAL":
                    v = this.QueryItemFromName(column) >= criteria;
                    break;

                case "LESSTHAN":
                    v = this.QueryItemFromName(column) < criteria;
                    break;

                case "LESSTHANOREQUAL":
                    v = this.QueryItemFromName(column) <= criteria;
                    break;

                case "LIKE":
                    v = this.QueryItemFromName(column).Like(criteria);
                    break;

                case "ISNULL":
                    v = this.QueryItemFromName(column).IsNull();
                    break;

                case "ISNOTNULL":
                    v = this.QueryItemFromName(column).IsNotNull();
                    break;

                case "BETWEEN":
                    v = this.QueryItemFromName(column).Between(criteria, criteria2);
                    break;

                case "IN":
                    v = this.QueryItemFromName(column).In(criteria);
                    break;

                case "NOTIN":
                    v = this.QueryItemFromName(column).NotIn(criteria);
                    break;

                case "NOTLIKE":
                    v = this.QueryItemFromName(column).NotLike(criteria);
                    break;

                case "CONTAINS":
                    v = this.QueryItemFromName(column).Contains(criteria);
                    break;
            }

            if (prevComp != null)
            {
                if (conjuction.ToUpper().StartsWith("A"))
                {
                    v = prevComp && v;
                }
                else
                {
                    v = prevComp || v;
                }
            }

            prevComp = v;

            return v;
        }


        /// <summary>
        /// A query WHERE clause. The default conjunction is "AND".
        /// </summary>
        /// <example>
        /// See <see cref="esConjunction"/> Enumeration.
        /// <code>
        /// emps.Query.Where
        /// (
        ///		emps.Query.LastName == "Doe" &amp;&amp; emps.Query.FirstName.Like("J%")
        /// );
        /// </code>
        /// </example>
        /// <param name="items">The list of objects to place in the where statement.</param>
        /// <returns>An esDynamicQueryTransport containing a where clause.</returns>
        public esDynamicQuerySerializable Having(params object[] items)
        {
            bool first = true;

            if (this.havingItems == null)
            {
                this.havingItems = new List<esComparison>();
            }
            else
            {
                if (!isExplicitParenthesis)
                {
                    havingItems.Add(new esComparison(this.defaultConjunction));
                }
                else
                {
                    isExplicitParenthesis = false;
                }
            }

            foreach (object item in items)
            {
                esComparison wi = item as esComparison;

                if (wi != null)
                {
                    if (wi.Parenthesis == esParenthesis.Open)
                    {
                        isExplicitParenthesis = true;
                    }
                    else if (wi.Parenthesis == esParenthesis.Close)
                    {
                        this.havingItems.RemoveAt(this.havingItems.Count - 1);
                    }

                    if (wi.data.WhereExpression != null)
                    {
                        foreach (esComparison expItem in wi.data.WhereExpression)
                        {
                            if (expItem.Value != null)
                            {
                                esDynamicQuerySerializable q = expItem.data.Value as esDynamicQuerySerializable;
                                if (q != null)
                                {
                                    AddQueryToList(q);
                                }
                            }
                        }

                        this.havingItems.AddRange(wi.data.WhereExpression);
                    }
                    else
                    {
                        if (!first)
                        {
                            this.havingItems.Add(new esComparison(this.defaultConjunction));
                        }
                        this.havingItems.Add(wi);
                    }

                    esDynamicQuerySerializable query = wi.Value as esDynamicQuerySerializable;

                    if (query != null)
                    {
                        AddQueryToList(query);
                    }
                }
                else
                {
                    if (!first)
                    {
                        this.havingItems.Add(new esComparison(this.defaultConjunction));
                    }
                    this.havingItems.AddRange(ProcessWhereItems(this.defaultConjunction, item));
                }
                first = false;
            }

            return this;
        }

        /// <summary>
        /// Pass in a subquery to use in an EXISTS statement
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public esComparison Exists(esDynamicQuerySerializable query)
        {
            AddQueryToList(query);

            esComparison where = new esComparison(query);
            where.Operand = esComparisonOperand.Exists;
            where.Value = query;
            return where;
        }

        /// <summary>
        /// Pass in a subquery to use in an NOT EXISTS statement
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public esComparison NotExists(esDynamicQuerySerializable query)
        {
            AddQueryToList(query);

            esComparison where = new esComparison(query);
            where.Operand = esComparisonOperand.NotExists;
            where.Value = query;
            return where;
        }

        /// <summary>
        /// A query WHERE clause joined by "AND". Natural language alternative, C# = &amp;&amp; operator, VB.NET = AND Keyword
        /// </summary>
        /// <example>
        /// See <see cref="esConjunction"/> Enumeration.
        /// <code>
        /// emps.Query.Where
        /// (
        ///		emps.Query.And
        ///		(
        ///			emps.Query.LastName == "Doe",
        ///			emps.Query.FirstName.Like("J%")
        ///		),
        ///		emps.Query.Age.LessThen(50)
        /// );
        /// </code>
        /// </example>
        /// <param name="items">The list of objects to place in the where statement.</param>
        /// <returns>An esDynamicQueryTransport containing a where clause.</returns>
        [Obsolete("For more readable code use '&&' in C# or 'AndAlso' in VB.NET rather than this method")]
        public List<esComparison> And(params object[] items)
        {
            return ProcessWhereItems(esConjunction.And, items);
        }

        /// <summary>
        /// A query WHERE clause joined by "AND NOT". Natural language alternative, C# = &amp;&amp; ! operator, VB.NET = AND NOT Keyword
        /// </summary>
        /// <example>
        /// See <see cref="esConjunction"/> Enumeration.
        /// <code>
        /// emps.Query.Where
        /// (
        ///		emps.Query.And
        ///		(
        ///			emps.Query.LastName == "Doe",
        ///			emps.Query.FirstName.Like("J%")
        ///		),
        ///		emps.Query.Age.LessThen(50)
        /// );
        /// </code>
        /// </example>
        /// <param name="items">The list of objects to place in the where statement.</param>
        /// <returns>An esDynamicQueryTransport containing a where clause.</returns>
        public List<esComparison> AndNot(params object[] items)
        {
            return ProcessWhereItems(esConjunction.AndNot, items);
        }

        /// <summary>
        /// A query WHERE clause joined by "OR". Natural language alternative, C# = || operator, VB.NET = OR Keyword
        /// </summary>
        /// <example>
        /// See <see cref="esConjunction"/> Enumeration.
        /// <code>
        /// emps.Query.Where
        /// (
        ///		emps.Query.Or
        ///		(
        ///			emps.Query.LastName == "Doe",
        ///			emps.Query.FirstName.Like("J%")
        ///		),
        ///		emps.Query.Age.LessThen(50)
        /// );
        /// </code>
        /// </example>
        /// <param name="items">The list of objects to place in the where statement.</param>
        /// <returns>An esDynamicQueryTransport containing a where clause.</returns>
        [Obsolete("For more readable code use '||' in C# or 'OrElse' in VB.NET rather than this method")]
        public List<esComparison> Or(params object[] items)
        {
            return ProcessWhereItems(esConjunction.Or, items);
        }

        /// <summary>
        /// A query WHERE clause joined by "OR NOT". Natural language alternative, C# = || ! operator, VB.NET = OR NOT Keyword
        /// </summary>
        /// <example>
        /// See <see cref="esConjunction"/> Enumeration.
        /// <code>
        /// emps.Query.Where
        /// (
        ///		emps.Query.Or
        ///		(
        ///			emps.Query.LastName == "Doe",
        ///			emps.Query.FirstName.Like("J%")
        ///		),
        ///		emps.Query.Age.LessThen(50)
        /// );
        /// </code>
        /// </example>
        /// <param name="items">The list of objects to place in the where statement.</param>
        /// <returns>An esDynamicQueryTransport containing a where clause.</returns>
        public List<esComparison> OrNot(params object[] items)
        {
            return ProcessWhereItems(esConjunction.OrNot, items);
        }

        /// <summary>
        /// Your ORDER BY statement.
        /// </summary>
        /// <example>
        /// <code>
        /// emps.Query.OrderBy
        /// (
        ///		emps.Query.LastName.Descending
        /// );
        /// </code>
        /// </example>
        /// <param name="orderByItems">The list of objects to place in the OrderBy statement.</param>
        /// <returns>An esDynamicQueryTransport containing a OrderBy clause.</returns>
        public esDynamicQuerySerializable OrderBy(params esOrderByItem[] orderByItems)
        {
            if (this.orderByItems == null)
            {
                this.orderByItems = new List<esOrderByItem>();
            }

            foreach (esOrderByItem orderByItem in orderByItems)
            {
                this.orderByItems.Add(orderByItem);
            }
            return this;
        }

        /// <summary>
        /// Your ORDER BY statement
        /// </summary>
        /// <example>
        /// You can order by an aggregate by passing the aggregate alias.
        /// See <see cref="esOrderByDirection"/> Enumeration.
        /// <code>
        /// emps.Query.OrderBy
        /// (
        ///		emps.Query.LastName.Descending
        /// );
        /// </code>
        /// </example>
        /// <param name="columnName">The column to place in the OrderBy statement.</param>
        /// <param name="direction">Sort direction.</param>
        /// <returns>An esDynamicQueryTransport containing a OrderBy clause.</returns>
        public esDynamicQuerySerializable OrderBy(string columnName, esOrderByDirection direction)
        {
            if (this.orderByItems == null)
            {
                this.orderByItems = new List<esOrderByItem>();
            }

            esOrderByItem item = new esOrderByItem();

            item.Expression = new esExpression();
            item.Expression.Query = this;
            item.Expression.Column.Name = columnName;
            item.Expression.Column.Alias = columnName;
            item.Direction = direction;

            this.orderByItems.Add(item);

            return this;
        }

        /// <summary>
        /// Your GROUP BY statement
        /// </summary>
        /// <example>
        /// <code>
        /// emps.Query.es.CountAll = true;
        /// emps.Query.Select
        /// (
        ///		emps.Query.LastName,
        ///		emps.Query.FirstName
        /// )
        /// .GroupBy
        /// (
        ///		emps.Query.LastName,
        ///		emps.Query.FirstName
        /// );
        /// </code>
        /// </example>
        /// <param name="columns">The columns to place in the OrderBy statement.</param>
        /// <returns>An esDynamicQueryTransport containing a GroupBy clause.</returns>
        public esDynamicQuerySerializable GroupBy(params esQueryItem[] columns)
        {
            if (this.groupByItems == null)
            {
                this.groupByItems = new List<esGroupByItem>();
            }

            foreach (esQueryItem item in columns)
            {
                esGroupByItem groupBy = new esGroupByItem();
                groupBy.Expression = item;
                this.groupByItems.Add(groupBy);
            }
            return this;
        }

        /// <summary>
        /// Your GROUP BY statement
        /// </summary>
        /// <example>
        /// <code>
        /// emps.Query.es.CountAll = true;
        /// emps.Query.Select
        /// (
        ///		emps.Query.LastName,
        ///		emps.Query.FirstName
        /// )
        /// .GroupBy
        /// (
        ///		emps.Query.LastName,
        ///		emps.Query.FirstName
        /// );
        /// </code>
        /// </example>
        /// <param name="columns">The columns to place in the OrderBy statement.</param>
        /// <returns>An esDynamicQueryTransport containing a GroupBy clause.</returns>
        public esDynamicQuerySerializable GroupBy(params string[] columns)
        {
            if (this.groupByItems == null)
            {
                this.groupByItems = new List<esGroupByItem>();
            }

            foreach (string column in columns)
            {
                esGroupByItem item = new esGroupByItem();
                item.Expression = new esExpression();
                item.Expression.Column.Name = column;
                item.Expression.Column.Alias = column;
                item.Expression.Query = this;
                this.groupByItems.Add(item);
            }
            return this;
        }

        /// <summary>
        /// Allows you to pull a SubQuery out of this Query
        /// </summary>
        /// <typeparam name="T">The Type of the desired Query</typeparam>
        /// <returns>The Query if found, otherwise null</returns>
        public T GetQuery<T>() where T : esDynamicQuerySerializable
        {
            Type type = typeof(T);
            string strType = type.ToString();

            foreach (esJoinItem joinItem in this.joinItems)
            {
                esJoinItem.esJoinItemData data = (esJoinItem.esJoinItemData)joinItem;

                if (data.Query.GetType().ToString() == strType)
                {
                    return (T)data.Query;
                }
            }

            if (this.GetType().ToString() == strType)
            {
                return (T)this;
            }

            return null;
        }

        #region Helper Routine
        private List<esComparison> ProcessWhereItems(esConjunction conj, params object[] theItems)
        {
            List<esComparison> items = new List<esComparison>();

            items.Add(new esComparison(esParenthesis.Open));

            bool first = true;

            esComparison whereItem;
            int count = theItems.Length;

            for (int i = 0; i < count; i++)
            {
                object o = theItems[i];

                whereItem = o as esComparison;
                if (whereItem != null)
                {
                    if (!first)
                    {
                        items.Add(new esComparison(conj));
                    }
                    items.Add(whereItem);
                    first = false;
                }
                else
                {
                    List<esComparison> listItem = o as List<esComparison>;
                    if (listItem != null)
                    {
                        if (!first)
                        {
                            items.Add(new esComparison(conj));
                        }
                        items.AddRange(listItem);
                        first = false;
                    }
                    else
                    {
                        if (o is string)
                        {
                            whereItem = new esComparison(this);
                            whereItem.ColumnName = o as string;
                            whereItem.IsLiteral = true;
                            items.Add(whereItem);
                        }
                    }
                }
            }

            items.Add(new esComparison(esParenthesis.Close));

            return items;
        }

        protected void HookupWithNoLock(esDynamicQuerySerializable query)
        {
            if (query.es.WithNoLock.HasValue && query.es.WithNoLock == true)
            {
                if (query.queries != null)
                {
                    foreach (esDynamicQuerySerializable nestedQuery in query.queries.Values)
                    {
                        nestedQuery.es.WithNoLock = true;

                        HookupWithNoLock(nestedQuery);
                    }
                }
            }
        }
        #endregion Helper Routine

        #region es

        /// <summary>
        /// This is to help hide some details from Intellisense.
        /// </summary>
        public DynamicQueryProps es
        {
            get
            {
                if (this.props == null)
                {
                    this.props = new DynamicQueryProps(this);
                }

                return this.props;
            }
        }

#if !SILVERLIGHT    
        [NonSerialized]
#endif
        private DynamicQueryProps props;

        /// <summary>
        /// The Dynamic Query properties.
        /// </summary>
        public class DynamicQueryProps
        {
            /// <summary>
            /// The Dynamic Query properties.
            /// </summary>
            /// <param name="query">The esDynamicQueryTransport's properties.</param>
            public DynamicQueryProps(esDynamicQuerySerializable query)
            {
                this.dynamicQuery = query;
            }

            /// <summary>
            /// string QuerySource.
            /// </summary>
            /// <returns>string QuerySource.</returns>
            public string QuerySource
            {
                get { return this.dynamicQuery.querySource; }
                set { this.dynamicQuery.querySource = value; }
            }

            /// <summary>
            /// string JoinAlias.
            /// </summary>
            /// <returns>string JoinAlias.</returns>
            public string JoinAlias
            {
                get { return this.dynamicQuery.joinAlias; }
                set { this.dynamicQuery.joinAlias = value; }
            }

            /// <summary>
            /// esConjunction DefaultConjunction.
            /// </summary>
            /// <returns>esConjunction DefaultConjunction.</returns>
            public esConjunction DefaultConjunction
            {
                get { return this.dynamicQuery.defaultConjunction; }
                set { this.dynamicQuery.defaultConjunction = value; }
            }

            /// <summary>
            /// This will limit the number of rows returned, after sorting.
            /// Setting Top to 10 will return the top ten rows after sorting.
            /// </summary>
            public int? Top
            {
                get { return this.dynamicQuery.top; }
                set { this.dynamicQuery.top = value; }
            }

            /// <summary>
            /// This will use the WITH (NOLOCK) syntax on all tables joined in the query. Currently
            /// only implemented for Microsoft SQL Server.
            /// </summary>
            public bool? WithNoLock
            {
                get { return this.dynamicQuery.withNoLock; }
                set { this.dynamicQuery.withNoLock = value; }
            }

            /// <summary>
            /// Used in SubQueries. The Any qualifier works just like the In except it allows for >, >=, 
            /// <, <= as well as the = (In) and != (Not In) operators
            /// </summary>
            public bool Any
            {
                get { return this.dynamicQuery.subquerySearchCondition == esSubquerySearchCondition.Any; }
                set { this.dynamicQuery.subquerySearchCondition = esSubquerySearchCondition.Any; }
            }

            /// <summary>
            /// Used in SubQueries. Used like 'Any' except for one key exception - any operator applied must be true for
            /// ALL the values returned in our subquery.
            /// </summary>
            public bool All
            {
                get { return this.dynamicQuery.subquerySearchCondition == esSubquerySearchCondition.All; }
                set { this.dynamicQuery.subquerySearchCondition = esSubquerySearchCondition.All; }
            }

            /// <summary>
            /// Used in SubQueries. The word SOME is an alias for ANY, and may be used anywhere that ANY is used. 
            /// The SQL standard defines these two words with the same meaning to overcome a limitation in the English language, 
            /// particularly for inequality comparisons.
            /// </summary>
            public bool Some
            {
                get { return this.dynamicQuery.subquerySearchCondition == esSubquerySearchCondition.Some; }
                set { this.dynamicQuery.subquerySearchCondition = esSubquerySearchCondition.Some; }
            }

            /// <summary>
            /// This will retrieve a specific row number from a select.
            /// This is useful when paging large sets of data
            /// </summary>
            public int? PageNumber
            {
                get { return this.dynamicQuery.pageNumber; }
                set { this.dynamicQuery.pageNumber = value; }
            }

            /// <summary>
            /// This will retrieve a specific row number from a select.
            /// This is useful when paging large sets of data
            /// </summary>
            public int? PageSize
            {
                get { return this.dynamicQuery.pageSize; }
                set { this.dynamicQuery.pageSize = value; }
            }

            /// <summary>
            /// Setting Distinct = True will elimate duplicate rows from the data.
            /// </summary>
            public bool Distinct
            {
                get { return this.dynamicQuery.distinct; }
                set { this.dynamicQuery.distinct = value; }
            }

            /// <summary>
            /// If true, add a COUNT(*) Aggregate to the selected columns list.
            /// </summary>
            public bool CountAll
            {
                get { return this.dynamicQuery.countAll; }
                set 
                { 
                    this.dynamicQuery.countAll = value;
                    if (String.IsNullOrEmpty(this.dynamicQuery.countAllAlias))
                    {
                        this.dynamicQuery.countAllAlias = "Count";
                    }
                }
            }

            /// <summary>
            /// If CountAll is set to true, use this to add a user-friendly column name.
            /// </summary>
            public string CountAllAlias
            {
                get { return this.dynamicQuery.countAllAlias; }
                set { this.dynamicQuery.countAllAlias = value; }
            }

            /// <summary>
            /// If true, add WITH ROLLUP to the GROUP BY clause.
            /// </summary>
            /// <example>
            /// <code>
            /// emps.Query.es.WithRollup = true;
            /// </code>
            /// </example>
            public bool WithRollup
            {
                get { return this.dynamicQuery.withRollup; }
                set { this.dynamicQuery.withRollup = value; }
            }

            /// <summary>
            /// Returns a string containing the Sql from the last Query.Load().
            /// Useful for testing and debugging query syntax.
            /// </summary>
            public string LastQuery
            {
                get { return this.dynamicQuery.lastQuery; }
                set { this.dynamicQuery.lastQuery = value; }
            }

            /// <summary>
            /// esConnection Connection.
            /// </summary>
            //public esConnection Connection
            //{
            //    get
            //    {
            //        if (this.dynamicQuery.connection == null)
            //        {
            //            this.dynamicQuery.connection = new esConnection();

            //            if (esConnection.ConnectionService != null)
            //            {
            //                this.dynamicQuery.connection.Name = esConnection.ConnectionService.GetName();
            //            }
            //            else
            //            {
            //                string connName = this.dynamicQuery.GetConnectionName();
            //                if (connName != null)
            //                {
            //                    this.dynamicQuery.connection.Name = connName;
            //                }
            //            }
            //        }

            //        return this.dynamicQuery.connection;
            //    }
            //    set { this.dynamicQuery.connection = value; }
            //}

            private esDynamicQuerySerializable dynamicQuery;
        }
        #endregion

#if (WCF || SILVERLIGHT)
        #region Serializer


        /// <summary>
        /// This class will allow you to serialize via the DataContract manaually, but normally you won't have to use this class
        /// </summary>
        static public class SerializeHelper
        {
            static public List<System.Type> GetKnownTypes(esDynamicQuerySerializable query)
            {
                List<System.Type> types = new List<Type>();
                GetKnownTypes(query, types);
                return types;
            }

            static public DataContractSerializer GetSerializer(esDynamicQuerySerializable query)
            {
                List<System.Type> types = GetKnownTypes(query);
#if !SILVERLIGHT 
                return new DataContractSerializer(query.GetType(), query.GetQueryName(),
                    "http://www.entityspaces.net", types, Int32.MaxValue, false, true, null);
#else
                return new DataContractSerializer(query.GetType(), query.GetQueryName(),
                    "http://www.entityspaces.net", types);
#endif
            }

            static public DataContractSerializer GetSerializer(esDynamicQuerySerializable query, Type type)
            {
                List<System.Type> types = GetKnownTypes(query);
#if !SILVERLIGHT 
                return new DataContractSerializer(type, query.GetQueryName(),
                    "http://www.entityspaces.net", types, Int32.MaxValue, false, true, null);
#else
                return new DataContractSerializer(type, query.GetQueryName(),
                    "http://www.entityspaces.net", types);
#endif
            }

            static public DataContractSerializer GetSerializer(esDynamicQuerySerializable query, List<System.Type> knownTypes)
            {
#if !SILVERLIGHT
                return new DataContractSerializer(query.GetType(), query.GetQueryName(),
                    "http://www.entityspaces.net", knownTypes, Int32.MaxValue, false, true, null);
#else
                return new DataContractSerializer(query.GetType(), query.GetQueryName(),
                    "http://www.entityspaces.net", knownTypes);
#endif
            }

            static public DataContractSerializer GetSerializer(esDynamicQuerySerializable query, Type type, List<System.Type> knownTypes)
            {
#if !SILVERLIGHT
                return new DataContractSerializer(type, query.GetQueryName(),
                    "http://www.entityspaces.net", knownTypes, Int32.MaxValue, false, true, null);
#else
                return new DataContractSerializer(type, query.GetQueryName(),
                    "http://www.entityspaces.net", knownTypes);
#endif
            }

            static public string ToXml(esDynamicQuerySerializable query)
            {
                string xml = "";

                DataContractSerializer dcs = GetSerializer(query);

                using (MemoryStream ms = new MemoryStream())
                {
                    dcs.WriteObject(ms, query);
                    ms.Seek(0, SeekOrigin.Begin);

                    using (StreamReader sr = new StreamReader(ms))
                    {
                        xml = sr.ReadToEnd();
                    }
                }

                return xml;
            }

#if !SILVERLIGHT
            static public esDynamicQuerySerializable FromXml(string xml, Type type)
            {
                esDynamicQuerySerializable query = null;

                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.MaxCharactersFromEntities = long.MaxValue;
                    settings.MaxCharactersInDocument = long.MaxValue;

                    using (var reader = System.Xml.XmlDictionaryReader.Create(memoryStream, settings))
                    {
                        // Deserialize
                        DataContractSerializer serializer = new DataContractSerializer(type, type.Name, "http://www.entityspaces.net");
                        query = serializer.ReadObject(reader) as esDynamicQuerySerializable;
                    }
                }

                return query;
            }

            static public esDynamicQuerySerializable FromXml(string xml, Type type, List<System.Type> knownTypes)
            {
                esDynamicQuerySerializable query = null;

                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.MaxCharactersFromEntities = long.MaxValue;
                    settings.MaxCharactersInDocument = long.MaxValue;

                    using (var reader = System.Xml.XmlDictionaryReader.Create(memoryStream, settings))
                    {
                        // Deserialize
                        DataContractSerializer serializer = new DataContractSerializer(type, type.Name, "http://www.entityspaces.net", 
                            knownTypes, Int32.MaxValue, false, true, null);
                        query = serializer.ReadObject(reader) as esDynamicQuerySerializable;
                    }
                }

                return query;
            }

#endif
            static private void GetKnownTypes(esDynamicQuerySerializable query, List<System.Type> types)
            {
                bool found = false;

                string strType = query.GetType().ToString();

                foreach (Type t in types)
                {
                    if (t.ToString() == strType)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    types.Add(query.GetType());
                }

                foreach (esDynamicQuerySerializable subQuery in query.queries.Values)
                {
                    GetKnownTypes(subQuery, types);
                }
            }
        }

        #endregion
#endif

        #region Internal Workings

        internal void AddSubOperator(esExpression selectItem)
        {
            if (this.selectColumns == null)
            {
                this.selectColumns = new List<esExpression>();
            }

            this.selectColumns.Add(selectItem);
        }

        internal void AddOrderBy(esOrderByItem orderByItem)
        {
            if (this.orderByItems == null)
            {
                this.orderByItems = new List<esOrderByItem>();
            }

            this.orderByItems.Add(orderByItem);
        }

        internal void AddWhere(esComparison where)
        {
            if (this.whereItems == null)
            {
                this.whereItems = new List<esComparison>();
            }

            this.whereItems.Add(where);
        }

        #endregion

        // Holds the name of the metadata maps that will eventually be turned into 
        // the real ones by esDynamicQuery
#if (WCF)
        [DataMember(Name = "Queries", EmitDefaultValue = false)]
#endif
        internal protected Dictionary<string, esDynamicQuerySerializable> queries = new Dictionary<string, esDynamicQuerySerializable>();
        // Filled in by SelectAllExcept()
#if (WCF)
        [DataMember(Name = "SelectAllExcept", EmitDefaultValue = false)]
#endif
        internal protected List<esQueryItem> m_selectAllExcept;

        // Set to true by SelectAll()
#if (WCF)
        [DataMember(Name = "SelectAll", EmitDefaultValue = false)]
#endif
        internal protected bool m_selectAll = false;

#if (WCF)
        [DataMember(Name = "DefaultConjunction", EmitDefaultValue = false)]
#endif
        internal esConjunction defaultConjunction = esConjunction.And;

#if (WCF)
        [DataMember(Name = "QuerySource", EmitDefaultValue = false)]
#endif
        internal string querySource;
#if (WCF)
        [DataMember(Name = "DataID", EmitDefaultValue = false)]
#endif
        internal Guid dataID;

#if (WCF)
        [DataMember(Name = "Catalog", EmitDefaultValue = false)]
#endif
        internal string catalog;
#if (WCF)
        [DataMember(Name = "Schema", EmitDefaultValue = false)]
#endif
        internal string schema;

#if (WCF)
        [DataMember(Name = "IsInSubQuery", EmitDefaultValue = false)]
#endif
        internal bool isInSubQuery;
#if (WCF)
        [DataMember(Name = "IsExplicitParenthesis", EmitDefaultValue = false)]
#endif
        internal bool isExplicitParenthesis;
#if (WCF)
        [DataMember(Name = "SubQueryAlias", EmitDefaultValue = false, IsRequired = false)]
#endif
        internal string subQueryAlias = string.Empty;
#if (WCF)
        [DataMember(Name = "SelectColumns", Order = 99, EmitDefaultValue = false)]
#endif
        internal List<esExpression> selectColumns;

#if (WCF)
        [DataMember(Name = "FromQuery", Order = 100, EmitDefaultValue = false)]
#endif
        internal esDynamicQuerySerializable fromQuery;

#if (WCF)
        [DataMember(Name = "JoinItems", Order = 101, EmitDefaultValue = false)]
#endif  
        internal List<esJoinItem> joinItems;

#if (WCF)
        [DataMember(Name = "WhereItems", Order = 102, EmitDefaultValue = false)]
#endif  
        internal List<esComparison> whereItems;

#if (WCF)
        [DataMember(Name = "HavingItems", Order = 103, EmitDefaultValue = false)]
#endif
        internal List<esComparison> havingItems;

#if (WCF)
        [DataMember(Name = "OrderByItems", Order = 104, EmitDefaultValue = false)]
#endif  
        internal List<esOrderByItem> orderByItems;

#if (WCF)
        [DataMember(Name = "GroupByItems", Order = 105, EmitDefaultValue = false)]
#endif  
        internal List<esGroupByItem> groupByItems;

#if (WCF)
        [DataMember(Name = "SetOperations", Order = 106, EmitDefaultValue = false)]
#endif
        internal List<esSetOperation> setOperations;

        /// <summary>
        /// Used by derived classes
        /// </summary>
#if (WCF) 
        [DataMember(Name = "Distinct", EmitDefaultValue = false)] 
#endif
        internal bool distinct;
        /// <summary>
        /// Used by derived classes
        /// </summary>
#if (WCF)
        [DataMember(Name = "SubquerySearchCondition", EmitDefaultValue = false)]
#endif        
        internal esSubquerySearchCondition subquerySearchCondition; 
        /// <summary>
        /// Used by derived classes
        /// </summary>
#if (WCF)
        [DataMember(Name = "Top", EmitDefaultValue = false)]
#endif        
        internal int? top;
        /// <summary>
        /// Used by derived classes
        /// </summary>
#if (WCF)
        [DataMember(Name = "WithNoLock", EmitDefaultValue = false)]
#endif
        internal bool? withNoLock;
        /// <summary>
        /// Used by derived classes
        /// </summary>
#if (WCF)
        [DataMember(Name = "PageNumber", EmitDefaultValue = false)]
#endif
        internal int? pageNumber;
        /// <summary>
        /// Used by derived classes
        /// </summary>
#if (WCF)
        [DataMember(Name = "PageSize", EmitDefaultValue = false)]
#endif          
        internal int? pageSize;
        /// <summary>
        /// Used by derived classes
        /// </summary>
#if (WCF)
        [DataMember(Name = "CountAll", EmitDefaultValue = false)]
#endif               
        internal bool countAll;
        /// <summary>
        /// Used by derived classes
        /// </summary>
#if (WCF)
        [DataMember(Name = "CountAllAlias", EmitDefaultValue = false)]
#endif          
        internal string countAllAlias;
        /// <summary>
        /// Used by derived classes
        /// </summary>
#if (WCF)
        [DataMember(Name = "WithRollup", EmitDefaultValue = false)]
#endif         
        internal bool withRollup;
        /// <summary>
        /// Must be supplied when using JOIN's
        /// </summary>
#if (WCF)
        [DataMember(Name = "JoinAlias", EmitDefaultValue = false)]
#endif           
        internal string joinAlias = " ";

#if (WCF)
        [DataMember(Name = "SubQueryNames", EmitDefaultValue = false)]
#endif 
        internal string subQueryNames;

#if (WCF)
        [DataMember(Name = "Skip", EmitDefaultValue = false)]
#endif
        internal int? skip;
#if (WCF)
        [DataMember(Name = "Take", EmitDefaultValue = false)]
#endif
        internal int? take;

        internal string lastQuery;
        private object providerMetadata;
        private object columns;
        private esComparison prevComp;

        #region IDynamicQueryTransportInternal Members

        Guid IDynamicQuerySerializableInternal.DataID
        {
            get { return this.dataID; }
            set { this.dataID = value; }
        }

        string IDynamicQuerySerializableInternal.Catalog
        {
            get { return this.catalog; }
            set { this.catalog = value; }
        }

        string IDynamicQuerySerializableInternal.Schema
        {
            get { return this.schema; }
            set { this.schema = value; }
        }

        bool IDynamicQuerySerializableInternal.IsInSubQuery
        {
            get { return this.isInSubQuery; }
            set { this.isInSubQuery = value; }
        }

        bool IDynamicQuerySerializableInternal.HasSetOperation
        {
            get 
            { 
                return setOperations != null && setOperations.Count > 0; 
            }
        }

        string IDynamicQuerySerializableInternal.SubQueryAlias
        {
            get { return this.subQueryAlias; }
        }

        string IDynamicQuerySerializableInternal.JoinAlias
        {
            get { return this.joinAlias; }
            set { this.joinAlias = value; }
        }

        string IDynamicQuerySerializableInternal.LastQuery
        {
            get { return this.lastQuery; }
            set { this.lastQuery = value; }
        }

        string IDynamicQuerySerializableInternal.QuerySource 
        {
            get { return this.querySource; }
            set { this.querySource = value; }
        }

        bool IDynamicQuerySerializableInternal.SelectAll 
        {
            get { return this.m_selectAll; } 
        }

        List<esQueryItem> IDynamicQuerySerializableInternal.SelectAllExcept 
        {
            get { return this.m_selectAllExcept; } 
        }

        esSubquerySearchCondition IDynamicQuerySerializableInternal.SubquerySearchCondition
        {
            get { return this.subquerySearchCondition; }
        }

        List<esExpression> IDynamicQuerySerializableInternal.InternalSelectColumns
        {
            get { return this.selectColumns; }
            set { this.selectColumns = value; }
        }

        esDynamicQuerySerializable IDynamicQuerySerializableInternal.InternalFromQuery
        {
            get { return this.fromQuery; }
        }

        List<esJoinItem> IDynamicQuerySerializableInternal.InternalJoinItems
        {
            get { return this.joinItems; }
        }

        List<esComparison> IDynamicQuerySerializableInternal.InternalWhereItems
        {
            get { return this.whereItems; }
        }

        List<esOrderByItem> IDynamicQuerySerializableInternal.InternalOrderByItems
        {
            get { return this.orderByItems; }
            set { this.orderByItems = value; }
        }

        List<esComparison> IDynamicQuerySerializableInternal.InternalHavingItems
        {
            get { return this.havingItems; }
        }

        List<esGroupByItem> IDynamicQuerySerializableInternal.InternalGroupByItems
        {
            get { return this.groupByItems; }
            set { this.groupByItems = value; }
        }

        List<esSetOperation> IDynamicQuerySerializableInternal.InternalSetOperations
        {
            get { return this.setOperations; }
            set { this.setOperations = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        object IDynamicQuerySerializableInternal.ProviderMetadata 
        {
            get { return this.providerMetadata; }
            set { this.providerMetadata = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        object IDynamicQuerySerializableInternal.Columns 
        {
            get { return this.columns; } 
            set { this.columns = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        void IDynamicQuerySerializableInternal.HookupProviderMetadata(esDynamicQuerySerializable query)
        {
            AddQueryToList(query);
        }

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, esDynamicQuerySerializable> IDynamicQuerySerializableInternal.queries 
        {
            get { return this.queries; }
        }

        /// <summary>
        /// The number of rows to skip in the result set (starting from the beginning)
        /// </summary>
        int? IDynamicQuerySerializableInternal.Skip
        {
            get { return this.skip; }
        }

        /// <summary>
        /// The number of rows to take from the result set (starting from the Skip)
        /// </summary>
        int? IDynamicQuerySerializableInternal.Take
        {
            get { return this.take; }
        }

        #endregion
    }
}
