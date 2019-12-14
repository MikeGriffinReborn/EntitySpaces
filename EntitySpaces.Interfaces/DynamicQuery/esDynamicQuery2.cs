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

using EntitySpaces.DynamicQuery;
using EntitySpaces.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

#if (LINQ)
using System.Data.Linq;
using System.Linq;
#endif



//SharpArgumentInfo.Create'	EntitySpaces.ORM.SqlServer C:\Users\MikeGriffin\source\repos\EntitySpaces_DotNetStandard\EntitySpaces.DynamicQuery\esDynamicQuery.cs	349	N/A


namespace EntitySpaces.Interfaces
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
  //  [Serializable]
    [DataContract(Namespace = "es", IsReference = true)]
    public partial class esDynamicQuery : DynamicObject, IDynamicQueryInternal
    {

        #region DynamicObject Stuff

        public dynamic Subquery
        {
            get
            {
                return this as dynamic;
            }
        }

        private Dictionary<string, object> dyno = new Dictionary<string, object>();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return dyno.Keys;
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            DynamicMetaObject meta =  base.GetMetaObject(parameter);
            return meta;
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            return base.TryBinaryOperation(binder, arg, out result);
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            return base.TryConvert(binder, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result)
        {
            return base.TryCreateInstance(binder, args, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
        {
            return base.TryDeleteIndex(binder, indexes);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            return base.TryDeleteMember(binder);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            string property = (string)indexes[0];

            if(!dyno.ContainsKey(property))
            {
                throw new Exception("Property '" + property + "' not found or not yet declared on '" + this.joinAlias + "'");
            }

            result = dyno[property];
            return true;
         // return base.TryGetIndex(binder, indexes, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!dyno.ContainsKey(binder.Name))
            {
                throw new Exception("Property '" + binder.Name + "' not found or not yet declared on '" + this.joinAlias + "'");
            }

            result = dyno[binder.Name];
            return true;
         // return dyno.TryGetValue(binder.Name, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return base.TryInvoke(binder, args, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            return base.TryInvokeMember(binder, args, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            string property = (string)indexes[0];

            if (dyno.ContainsKey(property))
            {
                throw new Exception("Property '" + property + "' is already declared on '" + this.joinAlias + "'");
            }

            dyno[property] = value;
            return true;
        //  return base.TrySetIndex(binder, indexes, value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dyno[binder.Name] = value;
            return true;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            return base.TryUnaryOperation(binder, out result);
        }

        #endregion

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
        internal void AddQueryToList(esDynamicQuery query)
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
        public esDynamicQuery Select()
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
        public esDynamicQuery Select(object obj)
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
                if (queryItem is object)
                {
                    sItem = queryItem;
                }
                else
                {
                    sItem = new esExpression();
                    esDynamicQuery query = obj as esDynamicQuery;
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
        public esDynamicQuery Select(params object[] columns)
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
                if (queryItem is object)
                {
                    sItem = queryItem;
                }
                else
                {
                    sItem = new esExpression();
                    esDynamicQuery query = obj as esDynamicQuery;
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
        //virtual public esDynamicQuery SelectAllExcept(params esQueryItem[] columns)
        //{
        //    if (m_selectAllExcept == null)
        //    {
        //        m_selectAllExcept = new List<esQueryItem>();
        //    }

        //    foreach (esQueryItem item in columns)
        //    {
        //        m_selectAllExcept.Add(item);
        //    }
        //    return this;
        //}

        /// <summary>
        /// This method will select all of the columns explicity by name that were present when you generated your
        /// classes as opposed to doing a SELECT *
        /// </summary>
        /// <returns></returns>
        //virtual public esDynamicQuery SelectAll()
        //{
        //    m_selectAll = true;
        //    return this;
        //}

        /// <summary>
        /// Use this method in conjuction with Take()
        /// </summary>
        /// <param name="count">The number of rows to skip</param>
        /// <returns></returns>
        public esDynamicQuery Skip(int count)
        {
            this.skip = count;
            return this;
        }

        /// <summary>
        /// Use this method in conjuction with Skip()
        /// </summary>
        /// <param name="count">The number of rows to return</param>
        /// <returns></returns>
        public esDynamicQuery Take(int count)
        {
            this.take = count;
            return this;
        }

        /// <summary>
        /// Creates an INNER JOIN 
        /// </summary>
        /// <param name="joinQuery">This query represents the table you are joining to</param>
        /// <returns>An esJoinItem, which you then call the On() method.</returns>
        public esJoinItem InnerJoin(esDynamicQuery joinQuery)
        {
            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[joinQuery.joinAlias] = joinQuery;
            return JoinCommon(joinQuery, esJoinType.InnerJoin);
        }

        /// <summary>
        /// This method allows you to streamline join syntax
        /// </summary>
        /// <typeparam name="T">The DynamicQuery Class</typeparam>
        /// <param name="alias">The desired alias</param>
        /// <param name="query">The name of your output parameter</param>
        /// <returns></returns>
        public esJoinItem InnerJoin<T>(string alias, out T query) where T : esDynamicQuery, new()
        {
            query = new T();
            query.joinAlias = alias;
            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[alias] = query;
            return JoinCommon(query, esJoinType.InnerJoin);
        }

        /// <summary>
        /// Creates a LEFT JOIN 
        /// </summary>
        /// <param name="joinQuery">This query represents the table you are joining to</param>
        /// <returns>An esJoinItem, which you then call the On() method.</returns>
        public esJoinItem LeftJoin(esDynamicQuery joinQuery)
        {
            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[joinQuery.joinAlias] = joinQuery;
            return JoinCommon(joinQuery, esJoinType.LeftJoin);
        }

        /// <summary>
        /// This method allows you to streamline join syntax
        /// </summary>
        /// <typeparam name="T">The DynamicQuery Class</typeparam>
        /// <param name="alias">The desired alias</param>
        /// <param name="query">The name of your output parameter</param>
        /// <returns></returns>
        public esJoinItem LeftJoin<T>(string alias, out T query) where T : esDynamicQuery, new()
        {
            query = new T();
            query.joinAlias = alias;
            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[alias] = query;
            return JoinCommon(query, esJoinType.LeftJoin);
        }


        /// <summary>
        /// Creates a RIGHT JOIN 
        /// </summary>
        /// <param name="joinQuery">This query represents the table you are joining to</param>
        /// <returns>An esJoinItem, which you then call the On() method.</returns>
        public esJoinItem RightJoin(esDynamicQuery joinQuery)
        {
            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[joinQuery.joinAlias] = joinQuery;
            return JoinCommon(joinQuery, esJoinType.RightJoin);
        }

        /// <summary>
        /// This method allows you to streamline join syntax
        /// </summary>
        /// <typeparam name="T">The DynamicQuery Class</typeparam>
        /// <param name="alias">The desired alias</param>
        /// <param name="query">The name of your output parameter</param>
        /// <returns></returns>
        public esJoinItem RightJoin<T>(string alias, out T query) where T : esDynamicQuery, new()
        {
            query = new T();
            query.joinAlias = alias;
            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[alias] = query;
            return JoinCommon(query, esJoinType.RightJoin);
        }

        /// <summary>
        /// Creates a FULL JOIN 
        /// </summary>
        /// <param name="joinQuery">This query represents the table you are joining to</param>
        /// <returns>An esJoinItem, which you then call the On() method.</returns>
        public esJoinItem FullJoin(esDynamicQuery joinQuery)
        {
            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[joinQuery.joinAlias] = joinQuery;
            return JoinCommon(joinQuery, esJoinType.FullJoin);
        }

        /// <summary>
        /// This method allows you to streamline join syntax
        /// </summary>
        /// <typeparam name="T">The DynamicQuery Class</typeparam>
        /// <param name="alias">The desired alias</param>
        /// <param name="query">The name of your output parameter</param>
        /// <returns></returns>
        public esJoinItem FullJoin<T>(string alias, out T query) where T : esDynamicQuery, new()
        {
            query = new T();
            query.joinAlias = alias;
            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[alias] = query;
            return JoinCommon(query, esJoinType.FullJoin);
        }

        /// <summary>
        /// Creates an INNER JOIN 
        /// </summary>
        /// <param name="joinQuery">This query represents the table you are joining to</param>
        /// <returns>An esJoinItem, which you then call the On() method.</returns>
        public esJoinItem CrossJoin(esDynamicQuery joinQuery)
        {
            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[joinQuery.joinAlias] = joinQuery;
            return JoinCommon(joinQuery, esJoinType.CrossJoin);
        }

        /// <summary>
        /// This method allows you to streamline join syntax
        /// </summary>
        /// <typeparam name="T">The DynamicQuery Class</typeparam>
        /// <param name="alias">The desired alias</param>
        /// <param name="query">The name of your output parameter</param>
        /// <returns></returns>
        public esJoinItem CrossJoin<T>(string alias, out T query) where T : esDynamicQuery, new()
        {
            query = new T();
            query.joinAlias = alias;
            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[alias] = query;
            return JoinCommon(query, esJoinType.CrossJoin);
        }

        /// <summary>
        /// Used by all of the join functions as all of the logic is the same except for 
        /// the type of join
        /// </summary>
        /// <param name="joinQuery">This query represents the table you are joining to</param>
        /// <param name="joinType">The type of join, ie, Inner, Left, Right, Full</param>
        /// <returns></returns>
        private esJoinItem JoinCommon(esDynamicQuery joinQuery, esJoinType joinType)
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
        /// Creates a CROSS APPLY 
        /// </summary>
        /// <param name="applyQuery">This query represents the query you want to CROSS APPLY</param>
        /// <returns>An esJoinItem, which you then call the On() method.</returns>
        public esDynamicQuery CrossApply(esDynamicQuery applyQuery)
        {
            if (applyQuery.joinAlias == " ")
            {
                throw new Exception("Your DynamicQuery must have an Alias, use the alternate constructor");
            }

            if (this.applyItems == null)
            {
                this.applyItems = new List<esApplyItem>();
            }

            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[applyQuery.joinAlias] = applyQuery;

            esApplyItem applyItem = new esApplyItem(this);
            applyItem.data.Query = applyQuery;
            applyItem.data.ApplyType = esApplyType.CrossApply;

            this.applyItems.Add(applyItem);

            AddQueryToList(applyQuery);

            return this;
        }

        public esDynamicQuery CrossApply(Func<esDynamicQuery> func)
        {
            esDynamicQuery query = func();
            return CrossApply(query);
        }

        public esDynamicQuery CrossApply<T>(out T query, Func<esDynamicQuery> func) where T : esDynamicQuery, new()
        {
            esDynamicQuery theQuery = func();
            query = (T)theQuery;
            return CrossApply(theQuery);
        }

        /// <summary>
        /// This method allows you to streamline the cross apply syntax
        /// </summary>
        /// <typeparam name="T">The DynamicQuery Class</typeparam>
        /// <param name="alias">The desired alias</param>
        /// <param name="query">The name of your output parameter</param>
        /// <returns></returns>
        public esDynamicQuery CrossApply<T>(string alias, out T query) where T : esDynamicQuery, new()
        {
            query = new T();
            query.joinAlias = alias;
            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[alias] = query;
            return CrossApply(query);
        }

        /// <summary>
        /// Creates a OUTER APPLY 
        /// </summary>
        /// <param name="applyQuery">This query represents the query you want to OUTER APPLY</param>
        /// <returns>An esJoinItem, which you then call the On() method.</returns>
        public esDynamicQuery OuterApply(esDynamicQuery applyQuery)
        {
            if (applyQuery.joinAlias == " ")
            {
                throw new Exception("Your DynamicQuery must have an Alias, use the alternate constructor");
            }

            if (this.applyItems == null)
            {
                this.applyItems = new List<esApplyItem>();
            }

            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[applyQuery.joinAlias] = applyQuery;

            esApplyItem applyItem = new esApplyItem(this);
            applyItem.data.Query = applyQuery;
            applyItem.data.ApplyType = esApplyType.OuterApply;

            this.applyItems.Add(applyItem);

            AddQueryToList(applyQuery);

            return this;
        }

        public esDynamicQuery OuterApply(Func<esDynamicQuery> func)
        {
            esDynamicQuery query = func();
            return OuterApply(query);
        }

        public esDynamicQuery OuterApply<T>(out T query, Func<esDynamicQuery> func) where T : esDynamicQuery, new()
        {
            esDynamicQuery theQuery = func();
            query = (T)theQuery;
            return OuterApply(theQuery);
        }

        /// <summary>
        /// This method allows you to streamline the cross apply syntax
        /// </summary>
        /// <typeparam name="T">The DynamicQuery Class</typeparam>
        /// <param name="alias">The desired alias</param>
        /// <param name="query">The name of your output parameter</param>
        /// <returns></returns>
        public esDynamicQuery OuterApply<T>(string alias, out T query) where T : esDynamicQuery, new()
        {
            query = new T();
            query.joinAlias = alias;
            dynamic thisAsDynamic = (dynamic)this;
            thisAsDynamic[alias] = query;
            return OuterApply(query);
        }

        /// <summary>
        /// Allows you to pass in a SubQuery for your FROM statement
        /// </summary>
        /// <param name="fromQuery">The subquery to use as your FROM statement</param>
        /// <returns></returns>
        public esDynamicQuery From(esDynamicQuery fromQuery)
        {
            this.fromQuery = fromQuery;
            AddQueryToList(fromQuery);
            return this;
        }

        /// <summary>
        /// Allows you to pass in a nested SubQuery for your FROM statement
        /// </summary>
        /// <param name="fromQuery">The subquery to use as your FROM statement</param>
        /// <returns></returns>
        public esDynamicQuery From(Func<esDynamicQuery> func)
        {
            esDynamicQuery query = func();
            return From(query);
        }

        public esDynamicQuery From<T>(out T query, Func<esDynamicQuery> func) where T : esDynamicQuery, new()
        {
            esDynamicQuery theQuery = func();
            query = (T)theQuery;
            return From(theQuery);
        }

        /// <summary>
        /// Performs a UNION between two statements
        /// </summary>
        /// <returns></returns>
        public esDynamicQuery Union(esDynamicQuery query)
        {
            esSetOperation setOperation = new esSetOperation(query);
            setOperation.SetOperationType = esSetOperationType.Union;

            if (setOperations == null)
            {
                setOperations = new List<esSetOperation>();
            }

            setOperations.Add(setOperation);

            return this;
        }

        /// <summary>
        /// Performs a UNION between two statements
        /// </summary>
        /// <returns></returns>
        public esDynamicQuery Union(Func<esDynamicQuery> func)
        {
            esDynamicQuery query = func();
            return Union(query);
        }

        public esDynamicQuery Union<T>(out T query, Func<esDynamicQuery> func) where T : esDynamicQuery, new()
        {
            esDynamicQuery theQuery = func();
            query = (T)theQuery;
            return Union(theQuery);
        }

        /// <summary>
        /// Performs a UNION ALL between two statements
        /// </summary>
        /// <returns></returns>
        public esDynamicQuery UnionAll(esDynamicQuery query)
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
        /// Performs a UNION ALL between two statements
        /// </summary>
        /// <returns></returns>
        public esDynamicQuery UnionAll(Func<esDynamicQuery> func)
        {
            esDynamicQuery query = func();
            return UnionAll(query);
        }

        public esDynamicQuery UnionAll<T>(out T query, Func<esDynamicQuery> func) where T : esDynamicQuery, new()
        {
            esDynamicQuery theQuery = func();
            query = (T)theQuery;
            return UnionAll(theQuery);
        }

        /// <summary>
        /// Peforms an INTERSECT between two statements
        /// </summary>
        /// <returns></returns>
        public esDynamicQuery Intersect(esDynamicQuery query)
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
        /// Peforms an INTERSECT between two statements
        /// </summary>
        /// <returns></returns>
        public esDynamicQuery Intersect(Func<esDynamicQuery> func)
        {
            esDynamicQuery query = func();
            return Intersect(query);
        }

        public esDynamicQuery Intersect<T>(out T query, Func<esDynamicQuery> func) where T : esDynamicQuery, new()
        {
            esDynamicQuery theQuery = func();
            query = (T)theQuery;
            return Intersect(theQuery);
        }

        /// <summary>
        /// Performs an EXCEPT between two statements
        /// </summary>
        /// <returns></returns>
        public esDynamicQuery Except(esDynamicQuery query)
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

        public esDynamicQuery Except(Func<esDynamicQuery> func)
        {
            esDynamicQuery query = func();
            return Except(query);
        }

        public esDynamicQuery Except<T>(out T query, Func<esDynamicQuery> func) where T : esDynamicQuery, new()
        {
            esDynamicQuery theQuery = func();
            query = (T)theQuery;
            return Except(theQuery);
        }

        /// <summary>
        /// Provide an Alias for your query or subquery
        /// </summary>
        /// <param name="subQueryAlias"></param>
        /// <returns></returns>
        public esDynamicQuery As(string subQueryAlias)
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
        public esDynamicQuery Where(params object[] items)
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
                                esDynamicQuery q = expItem.data.Value as esDynamicQuery;
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

                    esDynamicQuery query = wi.Value as esDynamicQuery;

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
        public esDynamicQuery Having(params object[] items)
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
                                esDynamicQuery q = expItem.data.Value as esDynamicQuery;
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

                    esDynamicQuery query = wi.Value as esDynamicQuery;

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
        public esComparison Exists(esDynamicQuery query)
        {
            AddQueryToList(query);

            esComparison where = new esComparison(query);
            where.Operand = esComparisonOperand.Exists;
            where.Value = query;
            return where;
        }

        /// <summary>
        /// Embed a subquery to use in an EXISTS statement
        /// </summary>
        /// <param name="func">And embedded query</param>
        /// <returns></returns>
        public esComparison Exists(Func<esDynamicQuery> func)
        {
            esDynamicQuery query = func();
            return Exists(query);
        }

        public esComparison Exists<T>(out T query, Func<esDynamicQuery> func) where T : esDynamicQuery, new()
        {
            esDynamicQuery theQuery = func();
            query = (T)theQuery;
            return Exists(theQuery);
        }

        /// <summary>
        /// Pass in a subquery to use in an NOT EXISTS statement
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public esComparison NotExists(esDynamicQuery query)
        {
            AddQueryToList(query);

            esComparison where = new esComparison(query);
            where.Operand = esComparisonOperand.NotExists;
            where.Value = query;
            return where;
        }

        /// <summary>
        /// Embed a subquery to use in an EXISTS statement
        /// </summary>
        /// <param name="func">And embedded query</param>
        /// <returns></returns>
        public esComparison NotExists(Func<esDynamicQuery> func)
        {
            esDynamicQuery query = func();
            return NotExists(query);
        }

        public esComparison NotExists<T>(out T query, Func<esDynamicQuery> func) where T : esDynamicQuery, new()
        {
            esDynamicQuery theQuery = func();
            query = (T)theQuery;
            return NotExists(theQuery);
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
        public esDynamicQuery OrderBy(params esOrderByItem[] orderByItems)
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
        public esDynamicQuery OrderBy(string columnName, esOrderByDirection direction)
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
        public esDynamicQuery GroupBy(params esQueryItem[] columns)
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
        public esDynamicQuery GroupBy(params string[] columns)
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
        public T GetQuery<T>() where T : esDynamicQuery
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

        /// <summary>
        /// Read-only metadata for the entity.
        /// </summary>
        /// <remarks>
        /// The sample below loops through the <see cref="esColumnMetadataCollection"/> in provided
        /// by the <see cref="IMetadata"/> interface. There is a lot of useful information here, in fact,
        /// there is enough information for EntitySpaces to build all of the dynamic sql required during
        /// operations that use dynamic sql.
        /// <code>
        /// public partial class Employees : esEmployees
        /// {
        /// 	public void CustomMethod()
        /// 	{
        /// 		foreach(esColumnMetadata col in this.Meta.Columns)
        /// 		{
        /// 			if(col.IsInPrimaryKey)
        /// 			{
        /// 				// do something ...
        /// 			}
        /// 		}
        /// 	}
        /// }
        /// </code>
        /// </remarks>
        /// <seealso cref="esColumnMetadata"/>
        /// <seealso cref="esColumnMetadataCollection"/>
        /// <seealso cref="esProviderSpecificMetadata"/>
        virtual protected IMetadata Meta { get { return null; } }

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

        protected void HookupWithNoLock(esDynamicQuery query)
        {
            if (query.withNoLock.HasValue && query.withNoLock == true)
            {
                if (query.queries != null)
                {
                    foreach (esDynamicQuery nestedQuery in query.queries.Values)
                    {
                        nestedQuery.withNoLock = true;

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

        [NonSerialized]
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
            public DynamicQueryProps(esDynamicQuery query)
            {
                this.dynamicQuery = query;
            }

            /// <summary>
            /// string QuerySource.
            /// </summary>
            /// <returns>string QuerySource.</returns>
            public esDynamicQuery QuerySource(string querySource)
            {
                this.dynamicQuery.querySource = querySource;
                return this.dynamicQuery;
            }

            /// <summary>
            /// string JoinAlias.
            /// </summary>
            /// <returns>string JoinAlias.</returns>
            public esDynamicQuery JoinAlias(string alias)
            {
                this.dynamicQuery.joinAlias = alias;
                return this.dynamicQuery;
            }

            /// <summary>
            /// esConjunction DefaultConjunction.
            /// </summary>
            /// <returns>esConjunction DefaultConjunction.</returns>
            public esDynamicQuery DefaultConjunction(esConjunction conj)
            {
                this.dynamicQuery.defaultConjunction = conj;
                return this.dynamicQuery;
            }

            /// <summary>
            /// This will limit the number of rows returned, after sorting.
            /// Setting Top to 10 will return the top ten rows after sorting.
            /// </summary>
            public esDynamicQuery Top(int top)
            {
                this.dynamicQuery.top = top;
                return this.dynamicQuery;
            }

            #region Partion Code

            public esPartionOrderBy PartitionBy(params esQueryItem[] partitionByColumns)
            {
                this.dynamicQuery.es.PartitionByColumns(partitionByColumns);

                return new esPartionOrderBy(this.dynamicQuery);
            }


            /// <summary>
            ///  Allows you use Top within a Group without using GroupBy
            /// </summary>
            /// <param name="partitionByTop"></param>
            internal int? PartitionByTop
            {
                get { return this.dynamicQuery.partitionByTop; }
                set { this.dynamicQuery.partitionByTop = value; }
            }

            /// <summary>
            ///  Allows you use Top within a Group without using GroupBy
            /// </summary>
            /// <param name="partitionByColumns"></param>
            internal void PartitionByColumns(params esQueryItem[] partitionByColumns)
            {
                if(this.dynamicQuery.partitionByColumns == null)
                {
                    this.dynamicQuery.partitionByColumns = new List<esQueryItem>();
                }

                foreach(esQueryItem item in partitionByColumns) 
                {
                    this.dynamicQuery.partitionByColumns.Add(item);
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="partitionDistinctColumns"></param>
            internal void PartitionDistinctColumns(params esQueryItem[] partitionDistinctColumns)
            {
                if (this.dynamicQuery.partitionByDistinctColumns == null)
                {
                    this.dynamicQuery.partitionByDistinctColumns = new List<esQueryItem>();
                }

                foreach (esQueryItem item in partitionDistinctColumns)
                {
                    this.dynamicQuery.partitionByDistinctColumns.Add(item);
                }
            }

            /// <summary>
            ///  Allows you use Top within a Group without using GroupBy
            /// </summary>
            /// <param name="partitionByorderByItems"></param>
            internal void PartitionByOrderBy(params esOrderByItem[] partitionByorderByItems)
            {
                if (this.dynamicQuery.partitionByOrderByItems == null)
                {
                    this.dynamicQuery.partitionByOrderByItems = new List<esOrderByItem>();
                }

                foreach (esOrderByItem item in partitionByorderByItems)
                {
                    this.dynamicQuery.partitionByOrderByItems.Add(item);
                }
            }

            #endregion

            /// <summary>
            /// This will use the WITH (NOLOCK) syntax on all tables joined in the query. Currently
            /// only implemented for Microsoft SQL Server.
            /// </summary>
            public esDynamicQuery WithNoLock()
            {
                this.dynamicQuery.withNoLock = true;
                return this.dynamicQuery;
            }

            /// <summary>
            /// Used in SubQueries. The Any qualifier works just like the In except it allows for >, >=, 
            /// <, <= as well as the = (In) and != (Not In) operators
            /// </summary>
            public esDynamicQuery Any()
            {
                this.dynamicQuery.subquerySearchCondition = esSubquerySearchCondition.Any;
                return this.dynamicQuery;
            }

            /// <summary>
            /// Used in SubQueries. Used like 'Any' except for one key exception - any operator applied must be true for
            /// ALL the values returned in our subquery.
            /// </summary>
            public esDynamicQuery All()
            {
                this.dynamicQuery.subquerySearchCondition = esSubquerySearchCondition.All;
                return this.dynamicQuery;
            }

            /// <summary>
            /// Used in SubQueries. The word SOME is an alias for ANY, and may be used anywhere that ANY is used. 
            /// The SQL standard defines these two words with the same meaning to overcome a limitation in the English language, 
            /// particularly for inequality comparisons.
            /// </summary>
            public esDynamicQuery Some()
            {
                this.dynamicQuery.subquerySearchCondition = esSubquerySearchCondition.Some;
                return this.dynamicQuery;
            }

            /// <summary>
            /// This will retrieve a specific row number from a select.
            /// This is useful when paging large sets of data
            /// </summary>
            public esDynamicQuery PageNumber(int pageNumber)
            {
                this.dynamicQuery.pageNumber = pageNumber;
                return this.dynamicQuery;
            }

            /// <summary>
            /// This will retrieve a specific row number from a select.
            /// This is useful when paging large sets of data
            /// </summary>
            public esDynamicQuery PageSize(int pageSize)
            {
                this.dynamicQuery.pageSize = pageSize;
                return this.dynamicQuery;
            }

            /// <summary>
            /// Setting Distinct = True will elimate duplicate rows from the data.
            /// </summary>
            public esDynamicQuery Distinct()
            {
                this.dynamicQuery.distinct = true;
                return this.dynamicQuery;
            }

            /// <summary>
            /// Add a COUNT(*) Aggregate to the selected columns list.
            /// </summary>
            public esDynamicQuery CountAll()
            {
                this.dynamicQuery.countAll = true;
                if (String.IsNullOrEmpty(this.dynamicQuery.countAllAlias))
                {
                    this.dynamicQuery.countAllAlias = "Count";
                }

                return this.dynamicQuery;
            }

            /// <summary>
            /// If CountAll is set to true, use this to add a user-friendly column name.
            /// </summary>
            public esDynamicQuery CountAllAlias(string alias)
            {
                this.dynamicQuery.countAllAlias = alias;
                return this.dynamicQuery;
            }

            /// <summary>
            /// If true, add WITH ROLLUP to the GROUP BY clause.
            /// </summary>
            /// <example>
            /// <code>
            /// emps.Query.es.WithRollup = true;
            /// </code>
            /// </example>
            public esDynamicQuery WithRollup()
            {
                this.dynamicQuery.withRollup = true;
                return this.dynamicQuery;
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

            internal esDynamicQuery dynamicQuery;
        }
        #endregion

        #region Serializer

        /// <summary>
        /// This class will allow you to serialize via the DataContract manaually, but normally you won't have to use this class
        /// </summary>
        static public class SerializeHelper
        {
            static public List<System.Type> GetKnownTypes(esDynamicQuery query)
            {
                List<System.Type> types = new List<Type>();
                GetKnownTypes(query, types);
                return types;
            }

            static public DataContractSerializer GetSerializer(esDynamicQuery query)
            {
                List<System.Type> types = GetKnownTypes(query);
                return new DataContractSerializer(query.GetType(), query.GetQueryName(),
                    "http://www.entityspaces.net", types);
            }

            static public DataContractSerializer GetSerializer(esDynamicQuery query, Type type)
            {
                List<System.Type> types = GetKnownTypes(query);
                return new DataContractSerializer(type, query.GetQueryName(),
                    "http://www.entityspaces.net", types);
            }

            static public DataContractSerializer GetSerializer(esDynamicQuery query, List<System.Type> knownTypes)
            {
                return new DataContractSerializer(query.GetType(), query.GetQueryName(),
                    "http://www.entityspaces.net", knownTypes);
            }

            static public DataContractSerializer GetSerializer(esDynamicQuery query, Type type, List<System.Type> knownTypes)
            {
                return new DataContractSerializer(type, query.GetQueryName(),
                    "http://www.entityspaces.net", knownTypes);
            }

            static public string ToXml(esDynamicQuery query)
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

            static public esDynamicQuery FromXml(string xml, Type type)
            {
                esDynamicQuery query = null;

                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.MaxCharactersFromEntities = long.MaxValue;
                    settings.MaxCharactersInDocument = long.MaxValue;

                    using (var reader = System.Xml.XmlDictionaryReader.Create(memoryStream, settings))
                    {
                        // Deserialize
                        DataContractSerializer serializer = new DataContractSerializer(type, type.Name, "http://www.entityspaces.net");
                        query = serializer.ReadObject(reader) as esDynamicQuery;
                    }
                }

                return query;
            }

            static public esDynamicQuery FromXml(string xml, Type type, List<System.Type> knownTypes)
            {
                esDynamicQuery query = null;

                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.MaxCharactersFromEntities = long.MaxValue;
                    settings.MaxCharactersInDocument = long.MaxValue;

                    using (var reader = System.Xml.XmlDictionaryReader.Create(memoryStream, settings))
                    {
                        // Deserialize
                        DataContractSerializer serializer = new DataContractSerializer(type, type.Name, "http://www.entityspaces.net",
                            knownTypes);
                        query = serializer.ReadObject(reader) as esDynamicQuery;
                    }
                }

                return query;
            }

            static private void GetKnownTypes(esDynamicQuery query, List<System.Type> types)
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

                foreach (esDynamicQuery subQuery in query.queries.Values)
                {
                    GetKnownTypes(subQuery, types);
                }
            }
        }

        #endregion

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
        [DataMember(Name = "Queries", EmitDefaultValue = false)]
        internal protected Dictionary<string, esDynamicQuery> queries = new Dictionary<string, esDynamicQuery>();
        // Filled in by SelectAllExcept()
        [DataMember(Name = "SelectAllExcept", EmitDefaultValue = false)]
        internal protected List<esQueryItem> m_selectAllExcept;

        // Set to true by SelectAll()
        [DataMember(Name = "SelectAll", EmitDefaultValue = false)]
        internal protected bool m_selectAll = false;

        [DataMember(Name = "DefaultConjunction", EmitDefaultValue = false)]
        internal esConjunction defaultConjunction = esConjunction.And;

        [DataMember(Name = "QuerySource", EmitDefaultValue = false)]
        internal string querySource;

        [DataMember(Name = "DataID", EmitDefaultValue = false)]
        internal Guid dataID;

        [DataMember(Name = "Catalog", EmitDefaultValue = false)]
        internal string catalog;

        [DataMember(Name = "Schema", EmitDefaultValue = false)]
        internal string schema;

        [DataMember(Name = "IsInSubQuery", EmitDefaultValue = false)]
        internal bool isInSubQuery;

        [DataMember(Name = "IsExplicitParenthesis", EmitDefaultValue = false)]
        internal bool isExplicitParenthesis;

        [DataMember(Name = "SubQueryAlias", EmitDefaultValue = false, IsRequired = false)]
        internal string subQueryAlias = string.Empty;

        [DataMember(Name = "SelectColumns", Order = 99, EmitDefaultValue = false)]
        internal List<esExpression> selectColumns;

        [DataMember(Name = "FromQuery", Order = 100, EmitDefaultValue = false)]
        internal esDynamicQuery fromQuery;

        [DataMember(Name = "JoinItems", Order = 101, EmitDefaultValue = false)]
        internal List<esJoinItem> joinItems;

        [DataMember(Name = "WhereItems", Order = 102, EmitDefaultValue = false)]
        internal List<esComparison> whereItems;

        [DataMember(Name = "HavingItems", Order = 103, EmitDefaultValue = false)]
        internal List<esComparison> havingItems;

        [DataMember(Name = "OrderByItems", Order = 104, EmitDefaultValue = false)]
        internal List<esOrderByItem> orderByItems;

        [DataMember(Name = "GroupByItems", Order = 105, EmitDefaultValue = false)]
        internal List<esGroupByItem> groupByItems;

        [DataMember(Name = "SetOperations", Order = 106, EmitDefaultValue = false)]
        internal List<esSetOperation> setOperations;

        [DataMember(Name = "ApplyItems", Order = 101, EmitDefaultValue = false)]
        internal List<esApplyItem> applyItems;

        /// <summary>
        /// Used by derived classes
        /// </summary>
        [DataMember(Name = "Distinct", EmitDefaultValue = false)]
        internal bool distinct;
        /// <summary>
        /// Used by derived classes
        /// </summary>
        [DataMember(Name = "SubquerySearchCondition", EmitDefaultValue = false)]
        internal esSubquerySearchCondition subquerySearchCondition;
        /// <summary>
        /// Used by derived classes
        /// </summary>
        [DataMember(Name = "Top", EmitDefaultValue = false)]
        internal int? top;

        [DataMember(Name = "PartitionByTop", EmitDefaultValue = false)]
        internal int? partitionByTop;

        [DataMember(Name = "PartitionByColumns", EmitDefaultValue = false)]
        internal List<esQueryItem> partitionByColumns;

        [DataMember(Name = "PartitionDistinctColumns", EmitDefaultValue = false)]
        internal List<esQueryItem> partitionByDistinctColumns;

        [DataMember(Name = "PartitionByColumns", EmitDefaultValue = false)]
        internal List<esOrderByItem> partitionByOrderByItems;

        /// <summary>
        /// Used by derived classes
        /// </summary>
        [DataMember(Name = "WithNoLock", EmitDefaultValue = false)]
        internal bool? withNoLock;
        /// <summary>
        /// Used by derived classes
        /// </summary>
        [DataMember(Name = "PageNumber", EmitDefaultValue = false)]
        internal int? pageNumber;
        /// <summary>
        /// Used by derived classes
        /// </summary>
        [DataMember(Name = "PageSize", EmitDefaultValue = false)]
        internal int? pageSize;
        /// <summary>
        /// Used by derived classes
        /// </summary>
        [DataMember(Name = "CountAll", EmitDefaultValue = false)]
        internal bool countAll;
        /// <summary>
        /// Used by derived classes
        /// </summary>
        [DataMember(Name = "CountAllAlias", EmitDefaultValue = false)]
        internal string countAllAlias;
        /// <summary>
        /// Used by derived classes
        /// </summary>
        [DataMember(Name = "WithRollup", EmitDefaultValue = false)]
        internal bool withRollup;
        /// <summary>
        /// Must be supplied when using JOIN's
        /// </summary>
        [DataMember(Name = "JoinAlias", EmitDefaultValue = false)]
        internal string joinAlias = " ";

        [DataMember(Name = "SubQueryNames", EmitDefaultValue = false)]
        internal string subQueryNames;

        [DataMember(Name = "Skip", EmitDefaultValue = false)]
        internal int? skip;
        [DataMember(Name = "Take", EmitDefaultValue = false)]
        internal int? take;

        internal string lastQuery;
        private object providerMetadata;
        private object columns;
        private esComparison prevComp;

        #region IDynamicQueryTransportInternal Members

        Guid IDynamicQueryInternal.DataID
        {
            get { return this.dataID; }
            set { this.dataID = value; }
        }

        string IDynamicQueryInternal.Catalog
        {
            get { return this.catalog; }
            set { this.catalog = value; }
        }

        string IDynamicQueryInternal.Schema
        {
            get { return this.schema; }
            set { this.schema = value; }
        }

        bool IDynamicQueryInternal.IsInSubQuery
        {
            get { return this.isInSubQuery; }
            set { this.isInSubQuery = value; }
        }

        bool IDynamicQueryInternal.HasSetOperation
        {
            get
            {
                return setOperations != null && setOperations.Count > 0;
            }
        }

        string IDynamicQueryInternal.SubQueryAlias
        {
            get { return this.subQueryAlias; }
        }

        string IDynamicQueryInternal.JoinAlias
        {
            get { return this.joinAlias; }
            set { this.joinAlias = value; }
        }

        string IDynamicQueryInternal.LastQuery
        {
            get { return this.lastQuery; }
            set { this.lastQuery = value; }
        }

        string IDynamicQueryInternal.QuerySource
        {
            get { return this.querySource; }
            set { this.querySource = value; }
        }

        bool IDynamicQueryInternal.SelectAll
        {
            get { return this.m_selectAll; }
        }

        List<esQueryItem> IDynamicQueryInternal.SelectAllExcept
        {
            get { return this.m_selectAllExcept; }
        }

        esSubquerySearchCondition IDynamicQueryInternal.SubquerySearchCondition
        {
            get { return this.subquerySearchCondition; }
        }

        List<esExpression> IDynamicQueryInternal.InternalSelectColumns
        {
            get { return this.selectColumns; }
            set { this.selectColumns = value; }
        }

        esDynamicQuery IDynamicQueryInternal.InternalFromQuery
        {
            get { return this.fromQuery; }
        }

        List<esJoinItem> IDynamicQueryInternal.InternalJoinItems
        {
            get { return this.joinItems; }
        }

        List<esApplyItem> IDynamicQueryInternal.InternalApplyItems
        {
            get { return this.applyItems; }
        }

        List<esComparison> IDynamicQueryInternal.InternalWhereItems
        {
            get { return this.whereItems; }
        }

        List<esOrderByItem> IDynamicQueryInternal.InternalOrderByItems
        {
            get { return this.orderByItems; }
            set { this.orderByItems = value; }
        }

        List<esComparison> IDynamicQueryInternal.InternalHavingItems
        {
            get { return this.havingItems; }
        }

        List<esGroupByItem> IDynamicQueryInternal.InternalGroupByItems
        {
            get { return this.groupByItems; }
            set { this.groupByItems = value; }
        }

        List<esSetOperation> IDynamicQueryInternal.InternalSetOperations
        {
            get { return this.setOperations; }
            set { this.setOperations = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        object IDynamicQueryInternal.ProviderMetadata
        {
            get { return this.providerMetadata; }
            set { this.providerMetadata = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        object IDynamicQueryInternal.Columns
        {
            get { return this.columns; }
            set { this.columns = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        void IDynamicQueryInternal.HookupProviderMetadata(esDynamicQuery query)
        {
            AddQueryToList(query);
        }

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, esDynamicQuery> IDynamicQueryInternal.queries => this.queries;

        /// <summary>
        /// The number of rows to skip in the result set (starting from the beginning)
        /// </summary>
        int? IDynamicQueryInternal.Skip => this.skip;

        /// <summary>
        /// The number of rows to take from the result set (starting from the Skip)
        /// </summary>
        int? IDynamicQueryInternal.Take => this.take;

        int? IDynamicQueryInternal.PartitionByTop => this.es.dynamicQuery.partitionByTop;

        List<esQueryItem> IDynamicQueryInternal.PartitionByColumns => this.es.dynamicQuery.partitionByColumns;

        List<esQueryItem> IDynamicQueryInternal.PartitionByDistinctColumns => this.es.dynamicQuery.partitionByDistinctColumns;

        List<esOrderByItem> IDynamicQueryInternal.PartitionByOrderByItems => this.es.dynamicQuery.partitionByOrderByItems;

        /// <summary>
        /// Allow you to gain access to the on-board metadata
        /// </summary>
        IMetadata IDynamicQueryInternal.Meta
        {
            get { return this.Meta; }
        }
        #endregion
    }
}
