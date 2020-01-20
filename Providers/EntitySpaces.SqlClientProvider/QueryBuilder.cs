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
using System.Data;
using System.Data.SqlClient;

namespace EntitySpaces.SqlClientProvider
{
    class QueryBuilder
    {
        public static SqlCommand PrepareCommand(esDataRequest request)
        {
            StandardProviderParameters std = new StandardProviderParameters();
            std.cmd = new SqlCommand();
            std.pindex = NextParamIndex(std.cmd); 
            std.request = request;

            string sql = BuildQuery(std, request.DynamicQuery);

            std.cmd.CommandText = sql;
            return (SqlCommand)std.cmd;
        }

        protected static string BuildQuery(StandardProviderParameters std, esDynamicQuery query)
        {
            bool paging = false;

            if (query.pageNumber.HasValue && query.pageSize.HasValue)
                paging = true;

            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

            string select = GetSelectStatement(std, query);
            string from = GetFromStatement(std, query);
            string join = GetJoinStatement(std, query);
            string apply = GetApplyStatement(std, query);
            string where = GetComparisonStatement(std, query, iQuery.InternalWhereItems, " WHERE ");
            string groupBy = GetGroupByStatement(std, query);
            string having = GetComparisonStatement(std, query, iQuery.InternalHavingItems, " HAVING ");
            string orderBy = GetOrderByStatement(std, query);
            string setOperation = GetSetOperationStatement(std, query);

            string sql = String.Empty;

            if (paging)
            {
                int begRow = ((query.pageNumber.Value - 1) * query.pageSize.Value) + 1;
                int endRow = begRow + (query.pageSize.Value - 1);

                // The WITH statement
                sql += "WITH [withStatement] AS (";
                sql += "SELECT " + select + ", ROW_NUMBER() OVER(" + orderBy + ") AS ESRN ";
                sql += "FROM " + from + join + apply + where + groupBy + ") ";

                sql += "SELECT * FROM [withStatement] ";

                sql += "WHERE ESRN BETWEEN " + begRow + " AND " + endRow;
                sql += " ORDER BY ESRN ASC";
            }
            else if (iQuery.PartitionByTop != null && iQuery.PartitionByTop.Value > 0)
            {
                //------------------------------------------------
                // Partition
                //------------------------------------------------

                /*
                    //---------------------------------------------------------
                    // C#
                    //---------------------------------------------------------
                    eQuery = new EmployeesQuery("e");
                    OrdersQuery o = new OrdersQuery("o");
                    OrderDetailsQuery od = new OrderDetailsQuery("od");

                    eQuery.Select(eQuery.EmployeeID)
                    .InnerJoin(o).On(eQuery.EmployeeID == o.EmployeeID)
                    .InnerJoin(od).On(o.OrderID == od.OrderID)
                    .Where(eQuery.EmployeeID < 6);
               
                    eQuery.es.PartitionBy(eQuery.EmployeeID).OrderBy().DistinctBy(eQuery.EmployeeID).Top = 1;                  
                  
                    //---------------------------------------------------------
                    // SQL
                    //---------------------------------------------------------
                    SELECT e.[EmployeeID], e.LastName, e.FirstName, e.ReportsTo 
                    FROM [Employees] e 
                    INNER JOIN
                    (
                        SELECT DISTINCT   [EmployeeID] 
                        FROM
                        (
                            SELECT DISTINCT
                                e.[EmployeeID],
                                ROW_NUMBER() OVER(PARTITION BY e.[EmployeeID] 
                            ORDER BY e.[LastName], o.[Freight], od.[Quantity] ) AS ESRN 
                            FROM [Employees] e  
		                    INNER JOIN [Orders] o ON e.[EmployeeID] = o.[EmployeeID] 
                            INNER JOIN [Order Details] od  ON o.[OrderID] = od.[OrderID] 
                            WHERE e.[EmployeeID] < @EmployeeID1
                        )  r 
                        WHERE ESRN <= 1
                    ) ij on ij.[EmployeeID] = e.[EmployeeID] 
                    ORDER BY e.LastName ASC
                 */

                sql = "SELECT " + select + " FROM " + Shared.CreateFullName(std.request, query) + " " +
                     iQuery.JoinAlias + " INNER JOIN ( SELECT DISTINCT " + GetPartitionColumnNames(iQuery.PartitionByDistinctColumns, "r") + " FROM ( ";
                sql += "SELECT DISTINCT " + GetPartitionColumnNames(iQuery.PartitionByDistinctColumns) + ", ROW_NUMBER() OVER(PARTITION BY " +
                    GetPartitionColumnNames(iQuery.PartitionByColumns);
                sql += " ORDER BY " + GetPartitionOderByColumnNames(iQuery.PartitionByOrderByItems) + " ) AS ESRN FROM ";

                // Normal query embedded here
                sql += from + join + apply + where + ") r WHERE ESRN <=" + iQuery.PartitionByTop + ") ij on ";

                string and = " ";
                foreach (esQueryItem item in iQuery.PartitionByDistinctColumns)
                {
                    sql += and + GetPartitionColumnName(item, "ij") + " = " + GetPartitionColumnName(item, iQuery.JoinAlias);
                    and = " and ";
                }

                if (orderBy != null && orderBy.Length > 0)
                {
                    sql += " " + orderBy;
                }
            }
            else
            {
                sql += "SELECT " + select + " FROM " + from + join +  apply + where + setOperation + groupBy + having + orderBy;
            }

            if (iQuery.Skip.HasValue || iQuery.Take.HasValue)
            {
                if (iQuery.Skip.HasValue)
                {
                    sql += " OFFSET " + iQuery.Skip.ToString() + " ROWS ";
                }

                if (iQuery.Take.HasValue)
                {
                    sql += " FETCH NEXT " + iQuery.Take.ToString() + " ROWS ONLY ";
                }
            }

            return sql;
        }

        protected static string GetFromStatement(StandardProviderParameters std, esDynamicQuery query)
        {
            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

            string sql = String.Empty;

            if (iQuery.InternalFromQuery == null)
            {
                sql = Shared.CreateFullName(std.request, query);

                if (iQuery.JoinAlias != " ")
                {
                    sql += " " + iQuery.JoinAlias;
                }

                if (query.withNoLock == true)
                {
                    sql += " WITH (NOLOCK)";
                }
            }
            else
            {
                IDynamicQueryInternal iSubQuery = iQuery.InternalFromQuery as IDynamicQueryInternal;

                iSubQuery.IsInSubQuery = true;

                sql += "(";
                sql += BuildQuery(std, iQuery.InternalFromQuery);
                sql += ")";

                if (iSubQuery.SubQueryAlias != " ")
                {
                    sql += " AS " + iSubQuery.SubQueryAlias;
                }

                iSubQuery.IsInSubQuery = false;
            }

            return sql;
        }

        protected static string GetSelectStatement(StandardProviderParameters std, esDynamicQuery query)
        {
            string sql = String.Empty;
            string comma = String.Empty;
            bool selectAll = true;

            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

            if (query.distinct) sql += " DISTINCT ";
            if (query.top >= 0) sql += " TOP " + query.top.ToString() + " ";

            if (iQuery.InternalSelectColumns != null)
            {
                selectAll = false;

                foreach (esExpression expressionItem in iQuery.InternalSelectColumns)
                {
                    if (expressionItem.Query != null)
                    {
                        IDynamicQueryInternal iSubQuery = expressionItem.Query as IDynamicQueryInternal;

                        sql += comma;

                        if (iSubQuery.SubQueryAlias == string.Empty)
                        {
                            sql += iSubQuery.JoinAlias + ".*";
                        }
                        else
                        {
                            iSubQuery.IsInSubQuery = true;
                            sql += " (" + BuildQuery(std, expressionItem.Query as esDynamicQuery) + ") AS " + iSubQuery.SubQueryAlias;
                            iSubQuery.IsInSubQuery = false;
                        }

                        comma = ",";
                    }
                    else
                    {
                        sql += comma;

                        string columnName = expressionItem.Column.Name;

                        if (columnName != null && columnName[0] == '<')
                            sql += columnName.Substring(1, columnName.Length - 2);
                        else
                            sql += GetExpressionColumn(std, query, expressionItem, false, true);

                        comma = ",";
                    }
                }
                sql += " ";
            }

            if (query.countAll)
            {
                selectAll = false;

                sql += comma;
                sql += "COUNT(*)";

                if (query.countAllAlias != null)
                {
                    // Need DBMS string delimiter here
                    sql += " AS " + Delimiters.StringOpen + query.countAllAlias + Delimiters.StringClose;
                }
            }

            if (selectAll)
            {
                sql += "*";
            }

            return sql;
        }

        protected static string GetJoinStatement(StandardProviderParameters std, esDynamicQuery query)
        {
            string sql = String.Empty;

            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

            if (iQuery.InternalJoinItems != null)
            {
                foreach (esJoinItem joinItem in iQuery.InternalJoinItems)
                {
                    esJoinItem.esJoinItemData joinData = (esJoinItem.esJoinItemData)joinItem;

                    switch (joinData.JoinType)
                    {
                        case esJoinType.InnerJoin:
                            sql += " INNER JOIN ";
                            break;
                        case esJoinType.LeftJoin:
                            sql += " LEFT JOIN ";
                            break;
                        case esJoinType.RightJoin:
                            sql += " RIGHT JOIN ";
                            break;
                        case esJoinType.FullJoin:
                            sql += " FULL JOIN ";
                            break;
                    }

                    IDynamicQueryInternal iSubQuery = joinData.Query as IDynamicQueryInternal;

                    sql += Shared.CreateFullName(std.request, joinData.Query);

                    sql += " " + iSubQuery.JoinAlias;

                    if (query.withNoLock == true)
                    {
                        sql += " WITH (NOLOCK)";
                    }

                    sql += " ON ";

                    sql += GetComparisonStatement(std, query, joinData.WhereItems, String.Empty);
                }
            }
            return sql;
        }

        protected static string GetApplyStatement(StandardProviderParameters std, esDynamicQuery query)
        {
            string sql = String.Empty;

            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

            if (iQuery.InternalApplyItems != null)
            {
                foreach (esApplyItem applyItem in iQuery.InternalApplyItems)
                {
                    esApplyItem.esApplyItemData applyData = (esApplyItem.esApplyItemData)applyItem;

                    switch (applyData.ApplyType)
                    {
                        case esApplyType.CrossApply:
                            sql += " CROSS APPLY ";
                            break;

                        case esApplyType.OuterApply:
                            sql += " OUTER APPLY ";
                            break;
                    }

                    IDynamicQueryInternal iSubQuery = applyData.Query as IDynamicQueryInternal;

                    iSubQuery.IsInSubQuery = true;

                    sql += "(";
                    sql += BuildQuery(std, applyData.Query);
                    sql += ")";

                    if (iSubQuery.SubQueryAlias != " ")
                    {
                        sql += " AS " + applyData.Query.joinAlias;
                    }

                    iSubQuery.IsInSubQuery = false;
                }
            }

            return sql;
        }

        protected static string GetComparisonStatement(StandardProviderParameters std, esDynamicQuery query, List<esComparison> items, string prefix)
        {
            string sql = String.Empty;
            string comma = String.Empty;

            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

            //=======================================
            // WHERE
            //=======================================
            if (items != null)
            {
                sql += prefix;

                DbType paramType = DbType.String;

                string compareTo = String.Empty;
                foreach (esComparison comparisonItem in items)
                {
                    paramType = DbType.String;

                    esComparison.esComparisonData comparisonData = (esComparison.esComparisonData)comparisonItem;
                    esDynamicQuery subQuery = null;

                    bool requiresParam = true;
                    bool needsStringParameter = false;
                    std.needsIntegerParameter = false;

                    if (comparisonData.IsParenthesis)
                    {
                        if (comparisonData.Parenthesis == esParenthesis.Open)
                            sql += "(";
                        else
                            sql += ")";

                        continue;
                    }

                    if (comparisonData.IsConjunction)
                    {
                        switch (comparisonData.Conjunction)
                        {
                            case esConjunction.And: sql += " AND "; break;
                            case esConjunction.Or: sql += " OR "; break;
                            case esConjunction.AndNot: sql += " AND NOT "; break;
                            case esConjunction.OrNot: sql += " OR NOT "; break;
                        }
                        continue;
                    }

                    Dictionary<string, SqlParameter> types = null;
                    if (comparisonData.Column.Query != null)
                    {
                        IDynamicQueryInternal iLocalQuery = comparisonData.Column.Query as IDynamicQueryInternal;
                        types = Cache.GetParameters(iLocalQuery.DataID, (esProviderSpecificMetadata)iLocalQuery.ProviderMetadata, (esColumnMetadataCollection)iLocalQuery.Columns);
                    }

                    if (comparisonData.IsLiteral)
                    {
                        if (comparisonData.Column.Name[0] == '<')
                        {
                            sql += comparisonData.Column.Name.Substring(1, comparisonData.Column.Name.Length - 2);
                        }
                        else
                        {
                            sql += comparisonData.Column.Name;
                        }
                        continue;
                    }

                    if (comparisonData.ComparisonColumn.Name == null)
                    {
                        subQuery = comparisonData.Value as esDynamicQuery;

                        if (subQuery == null)
                        {
                            if (comparisonData.Column.Name != null)
                            {
                                IDynamicQueryInternal iColQuery = comparisonData.Column.Query as IDynamicQueryInternal;
                                esColumnMetadataCollection columns = (esColumnMetadataCollection)iColQuery.Columns;

                                esColumnMetadata metaData = columns.FindByColumnName(comparisonData.Column.Name);

                                if (metaData != null)
                                {
                                    compareTo = Delimiters.Param + columns[comparisonData.Column.Name].PropertyName + (++std.pindex).ToString();
                                }
                                else
                                {
                                    compareTo = Delimiters.Param + "Expr" + (++std.pindex).ToString();
                                }
                            }
                            else
                            {
                                compareTo = Delimiters.Param + "Expr" + (++std.pindex).ToString();
                            }
                        }
                        else
                        {
                            // It's a sub query
                            compareTo = GetSubquerySearchCondition(subQuery) + " (" + BuildQuery(std, subQuery) + ") ";
                            requiresParam = false;
                        }
                    }
                    else
                    {
                        compareTo = GetColumnName(comparisonData.ComparisonColumn);
                        requiresParam = false;
                    }

                    switch (comparisonData.Operand)
                    {
                        case esComparisonOperand.Exists:
                            sql += " EXISTS" + compareTo;
                            break;
                        case esComparisonOperand.NotExists:
                            sql += " NOT EXISTS" + compareTo;
                            break;

                        //-----------------------------------------------------------
                        // Comparison operators, left side vs right side
                        //-----------------------------------------------------------
                        case esComparisonOperand.Equal:
                            if(comparisonData.ItemFirst)
                                sql += ApplyWhereSubOperations(std, query, comparisonData) + " = " + compareTo;
                            else
                                sql += compareTo + " = " + ApplyWhereSubOperations(std, query, comparisonData);
                            break;
                        case esComparisonOperand.NotEqual:
                            if (comparisonData.ItemFirst)
                                sql += ApplyWhereSubOperations(std, query, comparisonData) + " <> " + compareTo;
                            else
                                sql += compareTo + " <> " + ApplyWhereSubOperations(std, query, comparisonData);
                            break;
                        case esComparisonOperand.GreaterThan:
                            if (comparisonData.ItemFirst)
                                sql += ApplyWhereSubOperations(std, query, comparisonData) + " > " + compareTo;
                            else
                                sql += compareTo + " > " + ApplyWhereSubOperations(std, query, comparisonData);
                            break;
                        case esComparisonOperand.LessThan:
                            if (comparisonData.ItemFirst)
                                sql += ApplyWhereSubOperations(std, query, comparisonData) + " < " + compareTo;
                            else
                                sql += compareTo + " < " + ApplyWhereSubOperations(std, query, comparisonData);
                            break;
                        case esComparisonOperand.LessThanOrEqual:
                            if (comparisonData.ItemFirst)
                                sql += ApplyWhereSubOperations(std, query, comparisonData) + " <= " + compareTo;
                            else
                                sql += compareTo + " <= " + ApplyWhereSubOperations(std, query, comparisonData);
                            break;
                        case esComparisonOperand.GreaterThanOrEqual:
                            if (comparisonData.ItemFirst)
                                sql += ApplyWhereSubOperations(std, query, comparisonData) + " >= " + compareTo;
                            else
                                sql += compareTo + " >= " + ApplyWhereSubOperations(std, query, comparisonData);
                            break;

                        case esComparisonOperand.Like:
                            string esc = comparisonData.LikeEscape.ToString();
                            if(String.IsNullOrEmpty(esc) || esc == "\0")
                            {
                                sql += ApplyWhereSubOperations(std, query, comparisonData) + " LIKE " + compareTo;
                                needsStringParameter = true;
                            }
                            else
                            {
                                sql += ApplyWhereSubOperations(std, query, comparisonData) + " LIKE " + compareTo;
                                sql += " ESCAPE '" + esc + "'";
                                needsStringParameter = true;
                            }
                            break;
                        case esComparisonOperand.NotLike:
                            esc = comparisonData.LikeEscape.ToString();
                            if (String.IsNullOrEmpty(esc) || esc == "\0")
                            {
                                sql += ApplyWhereSubOperations(std, query, comparisonData) + " NOT LIKE " + compareTo;
                                needsStringParameter = true;
                            }
                            else
                            {
                                sql += ApplyWhereSubOperations(std, query, comparisonData) + " NOT LIKE " + compareTo;
                                sql += " ESCAPE '" + esc + "'";
                                needsStringParameter = true;
                            }
                            break;
                        case esComparisonOperand.Contains:
                            sql += " CONTAINS(" + GetColumnName(comparisonData.Column) + ", " + compareTo + ")";
                            paramType = DbType.AnsiStringFixedLength;
                            needsStringParameter = true;
                            break;
                        case esComparisonOperand.IsNull:
                            sql += ApplyWhereSubOperations(std, query, comparisonData) + " IS NULL";
                            requiresParam = false;
                            break;
                        case esComparisonOperand.IsNotNull:
                            sql += ApplyWhereSubOperations(std, query, comparisonData) + " IS NOT NULL";
                            requiresParam = false;
                            break;
                        case esComparisonOperand.In:
                        case esComparisonOperand.NotIn:
                            {
                                if (subQuery != null)
                                {
                                    // They used a subquery for In or Not 
                                    sql += ApplyWhereSubOperations(std, query, comparisonData);
                                    sql += (comparisonData.Operand == esComparisonOperand.In) ? " IN" : " NOT IN";
                                    sql += compareTo;
                                }
                                else
                                {
                                    comma = String.Empty;
                                    if (comparisonData.Operand == esComparisonOperand.In)
                                    {
                                        sql += ApplyWhereSubOperations(std, query, comparisonData) + " IN (";
                                    }
                                    else
                                    {
                                        sql += ApplyWhereSubOperations(std, query, comparisonData) + " NOT IN (";
                                    }

                                    foreach(object oin in comparisonData.Values)
                                    {
                                        string str = oin as string;
                                        if (str != null)
                                        {
                                            // STRING
                                            sql += comma + Delimiters.StringOpen + str + Delimiters.StringClose;
                                            comma = ",";
                                        }
                                        else if (null != oin as System.Collections.IEnumerable)
                                        {
                                            // LIST OR COLLECTION OF SOME SORT
                                            System.Collections.IEnumerable enumer = oin as System.Collections.IEnumerable;
                                            if (enumer != null)
                                            {
                                                System.Collections.IEnumerator iter = enumer.GetEnumerator();

                                                while (iter.MoveNext())
                                                {
                                                    object o = iter.Current;

                                                    string soin = o as string;

                                                    if (soin != null)
                                                        sql += comma + Delimiters.StringOpen + soin + Delimiters.StringClose;
                                                    else
                                                        sql += comma + Convert.ToString(o);

                                                    comma = ",";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // NON STRING OR LIST
                                            sql += comma + Convert.ToString(oin);
                                            comma = ",";
                                        }
                                    }
                                    sql += ")";
                                    requiresParam = false;
                                }
                            }
                            break;

                        case esComparisonOperand.Between:

                            SqlCommand sqlCommand = std.cmd as SqlCommand;

                            sql += ApplyWhereSubOperations(std, query, comparisonData) + " BETWEEN ";
                            sql += compareTo;
                            if (comparisonData.ComparisonColumn.Name == null)
                            {
                                sqlCommand.Parameters.AddWithValue(compareTo, comparisonData.BetweenBegin);
                            }

                            if (comparisonData.ComparisonColumn2.Name == null)
                            {
                                IDynamicQueryInternal iColQuery = comparisonData.Column.Query as IDynamicQueryInternal;
                                esColumnMetadataCollection columns = (esColumnMetadataCollection)iColQuery.Columns;
                                compareTo = Delimiters.Param + columns[comparisonData.Column.Name].PropertyName + (++std.pindex).ToString();

                                sql += " AND " + compareTo;
                                sqlCommand.Parameters.AddWithValue(compareTo, comparisonData.BetweenEnd);
                            }
                            else
                            {
                                sql += " AND " + Delimiters.ColumnOpen + comparisonData.ComparisonColumn2 + Delimiters.ColumnClose;
                            }

                            requiresParam = false;
                            break;
                    }

                    if (requiresParam)
                    {
                        SqlParameter p;

                        if (comparisonData.Column.Name != null)
                        {
                            if (types.ContainsKey(comparisonData.Column.Name))
                            {
                                p = types[comparisonData.Column.Name];

                                p = Cache.CloneParameter(p);
                                p.ParameterName = compareTo;
                                p.Value = comparisonData.Value;
                            }
                            else
                            {
                                p = new SqlParameter(compareTo, comparisonData.Value);
                            }

                            if (needsStringParameter)
                            {
                                p.DbType = paramType;
                            }
                            else if (std.needsIntegerParameter)
                            {
                                p.DbType = DbType.Int32;
                            }
                        }
                        else
                        {
                            p = new SqlParameter(compareTo, comparisonData.Value);
                        }

                        std.cmd.Parameters.Add(p);
                    }
                }
            }

            return sql;
        }

        protected static string GetOrderByStatement(StandardProviderParameters std, esDynamicQuery query)
        {
            string sql = String.Empty;
            string comma = String.Empty;

            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

            if (iQuery.InternalOrderByItems != null)
            {
                sql += " ORDER BY ";
                sql += GetOrderByColumns(std, query, iQuery.InternalOrderByItems);
            }

            return sql;
        }

        protected static string GetGroupByStatement(StandardProviderParameters std, esDynamicQuery query)
        {
            string sql = String.Empty;
            string comma = String.Empty;

            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

            if (iQuery.InternalGroupByItems != null)
            {
                sql += " GROUP BY ";

                foreach (esGroupByItem groupBy in iQuery.InternalGroupByItems)
                {
                    sql += comma;

                    string columnName = groupBy.Expression.Column.Name;

                    if (columnName != null && columnName[0] == '<')
                        sql += columnName.Substring(1, columnName.Length - 2);
                    else
                        sql += GetExpressionColumn(std, query, groupBy.Expression, false, false);

                    comma = ",";
                }

                if (query.withRollup)
                {
                    sql += " WITH ROLLUP";
                }
            }

            return sql;
        }

        protected static string GetSetOperationStatement(StandardProviderParameters std, esDynamicQuery query)
        {
            string sql = String.Empty;

            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

            if (iQuery.InternalSetOperations != null)
            {
                foreach (esSetOperation setOperation in iQuery.InternalSetOperations)
                {
                    switch (setOperation.SetOperationType)
                    {
                        case esSetOperationType.Union: sql += " UNION "; break;
                        case esSetOperationType.UnionAll: sql += " UNION ALL "; break;
                        case esSetOperationType.Intersect: sql += " INTERSECT "; break;
                        case esSetOperationType.Except: sql += " EXCEPT "; break;
                    }

                    sql += BuildQuery(std, setOperation.Query);
                }
            }

            return sql;
        }

        protected static string GetExpressionColumn(StandardProviderParameters std, esDynamicQuery query, esExpression expression, bool inExpression, bool useAlias)
        {
            string sql = String.Empty;

            if (expression.OverClause != null)
            {
                return GetOverClause(std, query, expression.OverClause);
            }

            if (expression.CaseWhen != null)
            {
                return GetCaseWhenThenEnd(std, query, expression.CaseWhen);
            }

            if (expression.HasMathmaticalExpression)
            {
                sql += GetMathmaticalExpressionColumn(std, query, expression.MathmaticalExpression);
            }
            else
            {
                sql += GetColumnName(expression.Column);
            }

            if (expression.SubOperators != null)
            {
                if (expression.Column.Distinct)
                {
                    sql = BuildSubOperationsSql(std, "DISTINCT " + sql, expression.SubOperators);
                }
                else
                {
                    sql = BuildSubOperationsSql(std, sql, expression.SubOperators);
                }
            }

            if (!inExpression && useAlias)
            {
                if (expression.SubOperators != null || expression.Column.HasAlias)
                {
                    sql += " AS " + Delimiters.StringOpen + expression.Column.Alias + Delimiters.StringClose;
                }
            }

            return sql;
        }

        protected static string GetCaseWhenThenEnd(StandardProviderParameters std, esDynamicQuery query, esCase caseWhenThen)
        {
            string sql = string.Empty;

            EntitySpaces.DynamicQuery.esCase.esSimpleCaseData caseStatement = caseWhenThen;

            esColumnItem column = caseStatement.QueryItem;

            sql += Delimiters.ColumnOpen + column.Alias + Delimiters.ColumnClose + " = "; 
            sql += "CASE ";

            List<esComparison> list = new List<esComparison>();

            foreach (EntitySpaces.DynamicQuery.esCase.esSimpleCaseData.esCaseClause caseClause in caseStatement.Cases)
            {
                sql += " WHEN ";
                if (!caseClause.When.IsExpression)
                {
                    sql += GetComparisonStatement(std, query, caseClause.When.Comparisons, string.Empty);
                }
                else
                {
                    if (!caseClause.When.Expression.IsLiteralValue)
                    {
                        sql += GetExpressionColumn(std, query, caseClause.When.Expression, false, true);
                    }
                    else
                    {
                        if (caseClause.When.Expression.LiteralValue is string)
                        {
                            sql += Delimiters.StringOpen + caseClause.When.Expression.LiteralValue + Delimiters.StringClose;
                        }
                        else
                        {
                            sql += Convert.ToString(caseClause.When.Expression.LiteralValue);
                        }
                    }
                }

                sql += " THEN ";

                if (!caseClause.Then.IsLiteralValue)
                {
                    sql += GetExpressionColumn(std, query, caseClause.Then, false, true);
                }
                else
                {
                    if (caseClause.Then.LiteralValue is string)
                    {
                        sql += Delimiters.StringOpen + caseClause.Then.LiteralValue + Delimiters.StringClose;
                    }
                    else
                    {
                        sql += Convert.ToString(caseClause.Then.LiteralValue);
                    }
                }
            }

            if (caseStatement.Else != null)
            {
                sql += " ELSE ";

                if (!caseStatement.Else.IsLiteralValue)
                {
                    sql += GetExpressionColumn(std, query, caseStatement.Else, false, true);
                }
                else
                {
                    if (caseStatement.Else.LiteralValue is string)
                    {
                        sql += Delimiters.StringOpen + caseStatement.Else.LiteralValue + Delimiters.StringClose;
                    }
                    else
                    {
                        sql += Convert.ToString(caseStatement.Else.LiteralValue);
                    }
                }
            }

            sql += " END ";

            return sql;
        }

        protected static string GetMathmaticalExpressionColumn(StandardProviderParameters std, esDynamicQuery query, esMathmaticalExpression mathmaticalExpression)
        {
            string sql = "(";

            if (mathmaticalExpression.ItemFirst)
            {
                sql += GetExpressionColumn(std, query, mathmaticalExpression.SelectItem1, true, true);
                sql += esArithmeticOperatorToString(mathmaticalExpression.Operator);

                if (mathmaticalExpression.SelectItem2 != null)
                {
                    sql += GetExpressionColumn(std, query, mathmaticalExpression.SelectItem2, true, true);
                }
                else
                {
                    sql += GetMathmaticalExpressionLiteralType(std, mathmaticalExpression);
                }
            }
            else
            {
                if (mathmaticalExpression.SelectItem2 != null)
                {
                    sql += GetExpressionColumn(std, query, mathmaticalExpression.SelectItem2, true, true);
                }
                else
                {
                    sql += GetMathmaticalExpressionLiteralType(std, mathmaticalExpression);
                }

                sql += esArithmeticOperatorToString(mathmaticalExpression.Operator);
                sql += GetExpressionColumn(std, query, mathmaticalExpression.SelectItem1, true, true);
            }

            sql += ")";

            return sql;
        }

        protected static string esArithmeticOperatorToString(esArithmeticOperator arithmeticOperator)
        {
            switch (arithmeticOperator)
            {
                case esArithmeticOperator.Add: return " + ";
                case esArithmeticOperator.Subtract: return " - ";
                case esArithmeticOperator.Multiply: return " * ";
                case esArithmeticOperator.Divide: return " / ";
                case esArithmeticOperator.Modulo: return " % ";
                default: return "";
            }
        }

        protected static string GetMathmaticalExpressionLiteralType(StandardProviderParameters std, esMathmaticalExpression mathmaticalExpression)
        {
            switch (mathmaticalExpression.LiteralType)
            {
                case esSystemType.String:
                    return Delimiters.StringOpen + (string)mathmaticalExpression.Literal + Delimiters.StringClose;

                case esSystemType.DateTime:
                    return Delimiters.StringOpen + ((DateTime)(mathmaticalExpression.Literal)).ToShortDateString() + Delimiters.StringClose;

                default:
                    return Convert.ToString(mathmaticalExpression.Literal);
             }
        }

        protected static string GetOrderByColumns(StandardProviderParameters std, esDynamicQuery query, List<esOrderByItem> orderByItems)
        {
            string sql = String.Empty;
            string comma = String.Empty;

            if(orderByItems.Count > 0)
            { 
                foreach (esOrderByItem orderByItem in orderByItems)
                {
                    bool literal = false;

                    sql += comma;

                    string columnName = orderByItem.Expression.Column.Name;

                    if (columnName != null && columnName[0] == '<')
                    {
                        sql += columnName.Substring(1, columnName.Length - 2);

                        if (orderByItem.Direction == esOrderByDirection.Unassigned)
                        {
                            literal = true; // They must provide the DESC/ASC in the literal string
                        }
                    }
                    else
                    {
                        sql += GetExpressionColumn(std, query, orderByItem.Expression, false, false);
                    }

                    if (!literal)
                    {
                        if (orderByItem.Direction == esOrderByDirection.Ascending)
                            sql += " ASC";
                        else
                            sql += " DESC";
                    }

                    comma = ",";
                }
            }

            return sql;
        }

        protected static string GetOverClause(StandardProviderParameters std, esDynamicQuery query, IOverClause clause)
        {
            string columnExpression = null;
            string partitionBy = null;
            string orderby = null;
            string alias = null;

            if(clause.ColumnExpression is object)
            {
                columnExpression = GetExpressionColumn(std, query, clause.ColumnExpression, false, false);
            }

            if (clause.PartionByColumns != null)
            {
                partitionBy = "";

                string comma = "";
                foreach (esQueryItem partionColumn in clause.PartionByColumns)
                {
                    partitionBy += comma + GetExpressionColumn(std, query, partionColumn, false, false);
                    comma = ", ";
                }
            }

            if (clause.OrderByColumns != null)
            {
                orderby = GetOrderByColumns(std, query, clause.OrderByColumns);
            }

            if(!String.IsNullOrWhiteSpace(clause.Alias))
            {
                alias = clause.Alias;
            }

            string sql = clause.CreateOverStatement(columnExpression, partitionBy, orderby, alias);

            return sql;
        }

        protected static string ApplyWhereSubOperations(StandardProviderParameters std, esDynamicQuery query, esComparison.esComparisonData comparisonData)
        {
            string sql = string.Empty;

            if (comparisonData.HasExpression)
            {
                sql += GetMathmaticalExpressionColumn(std, query, comparisonData.Expression);

                if (comparisonData.SubOperators != null && comparisonData.SubOperators.Count > 0)
                {
                    sql = BuildSubOperationsSql(std, sql, comparisonData.SubOperators);
                }

                return sql;
            }

            string delimitedColumnName = GetColumnName(comparisonData.Column);

            if (comparisonData.SubOperators != null)
            {
                sql = BuildSubOperationsSql(std, delimitedColumnName, comparisonData.SubOperators);
            }
            else
            {
                sql = delimitedColumnName;
            }

            return sql;
        }

        protected static string BuildSubOperationsSql(StandardProviderParameters std, string columnName, List<esQuerySubOperator> subOperators)
        {
            string sql = string.Empty;

            subOperators.Reverse();

            Stack<object> stack = new Stack<object>();

            if (subOperators != null)
            {
                foreach (esQuerySubOperator op in subOperators)
                {
                    switch (op.SubOperator)
                    {
                        case esQuerySubOperatorType.ToLower:
                            sql += "LOWER(";
                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.ToUpper:
                            sql += "UPPER(";
                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.LTrim:
                            sql += "LTRIM(";
                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.RTrim:
                            sql += "RTRIM(";
                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.Trim:
                            sql += "LTRIM(RTRIM(";
                            stack.Push("))");
                            break;

                        case esQuerySubOperatorType.SubString:

                            sql += "SUBSTRING(";

                            stack.Push(")");
                            stack.Push(op.Parameters["length"]);
                            stack.Push(",");

                            if (op.Parameters.ContainsKey("start"))
                            {
                                stack.Push(op.Parameters["start"]);
                                stack.Push(",");
                            }
                            else
                            {
                                // They didn't pass in start so we start
                                // at the beginning
                                stack.Push(1);
                                stack.Push(",");
                            }
                            break;

                        case esQuerySubOperatorType.Coalesce:
                            sql += "COALESCE(";

                            stack.Push(")");
                            stack.Push(op.Parameters["expressions"]);
                            stack.Push(",");
                            break;

                        case esQuerySubOperatorType.Date:
                            sql += "DATEADD(dd, 0, DATEDIFF(dd, 0,";
                            stack.Push("))");
                            break;
                        
                        case esQuerySubOperatorType.Length:
                            sql += "LEN(";
                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.Round:
                            sql += "ROUND(";

                            stack.Push(")");
                            stack.Push(op.Parameters["SignificantDigits"]);
                            stack.Push(",");
                            break;

                        case esQuerySubOperatorType.DatePart:
                            std.needsIntegerParameter = true;
                            sql += "DATEPART(";
                            sql += op.Parameters["DatePart"];
                            sql += ",";

                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.Avg:
                            sql += "AVG(";

                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.Count:
                            sql += "COUNT(";

                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.Max:
                            sql += "MAX(";

                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.Min:
                            sql += "MIN(";

                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.StdDev:
                            sql += "STDEV(";
                            
                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.Sum:
                            sql += "SUM(";

                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.Var:
                            sql += "VAR(";

                            stack.Push(")");
                            break;

                        case esQuerySubOperatorType.Cast:
                            sql += "CAST(";
                            stack.Push(")");

                            if(op.Parameters.Count > 1)
                            {
                                stack.Push(")");

                                if (op.Parameters.Count == 2)
                                {
                                    stack.Push(op.Parameters["length"].ToString());
                                }
                                else
                                {
                                    stack.Push(op.Parameters["scale"].ToString());
                                    stack.Push(",");
                                    stack.Push(op.Parameters["precision"].ToString());
                                }

                                stack.Push("(");
                            }


                            stack.Push(GetCastSql((esCastType)op.Parameters["esCastType"]));
                            stack.Push(" AS ");
                            break;
                    }
                }

                sql += columnName;

                while (stack.Count > 0)
                {
                    sql += stack.Pop().ToString();
                }
            }
            return sql;
        }

        protected static string GetCastSql(esCastType castType)
        {
            switch (castType)
            {
                case esCastType.Boolean:   return "bit";
                case esCastType.Byte:      return "tinyint";
                case esCastType.Char:      return "char";
                case esCastType.DateTime:  return "datetime";
                case esCastType.Double:    return "float";
                case esCastType.Decimal:   return "decimal";
                case esCastType.Guid:      return "uniqueidentifier";
                case esCastType.Int16:     return "smallint";
                case esCastType.Int32:     return "int";
                case esCastType.Int64:     return "bigint";
                case esCastType.Single:    return "real";
                case esCastType.String:    return "nvarchar";

                default: return "error";
            }
        }

        protected static string GetColumnName(esColumnItem column)
        {
            if (String.IsNullOrWhiteSpace(column.Name))
            {
                if (!String.IsNullOrWhiteSpace(column.Alias))
                {
                    return Delimiters.StringOpen + column.Alias + Delimiters.StringClose;
                }

                return String.Empty;

            }

            if (column.Query == null || column.Query.joinAlias == " ")
            {
                return Delimiters.ColumnOpen + column.Name + Delimiters.ColumnClose;
            }
            else
            {
                IDynamicQueryInternal iQuery = column.Query as IDynamicQueryInternal;

                if (iQuery.IsInSubQuery)
                {
                    return column.Query.joinAlias + "." + Delimiters.ColumnOpen + column.Name + Delimiters.ColumnClose;
                }
                else
                {
                    string alias = iQuery.SubQueryAlias == string.Empty ? iQuery.JoinAlias : iQuery.SubQueryAlias;
                    return alias + "." + Delimiters.ColumnOpen + column.Name + Delimiters.ColumnClose;
                }
            }
        }

        private static int NextParamIndex(IDbCommand cmd)
        {
            return cmd.Parameters.Count;
        }

        private static string GetSubquerySearchCondition(esDynamicQuery query)
        {
            string searchCondition = String.Empty;

            IDynamicQueryInternal iQuery = query as IDynamicQueryInternal;

            switch (iQuery.SubquerySearchCondition)
            {
                case esSubquerySearchCondition.All:  searchCondition = "ALL";  break;
                case esSubquerySearchCondition.Any:  searchCondition = "ANY";  break;
                case esSubquerySearchCondition.Some: searchCondition = "SOME"; break;
            }

            return searchCondition;
        }

        private static string GetPartitionColumnNames(List<esQueryItem> items, string alias = "")
        {
            if (items == null) return "";

            string result = "";
            string comma = "";

            if (alias != null)
            {
                alias = alias.Length > 0 ? (alias + ".") : "";
            }

            foreach (esQueryItem item in items)
            {
                if (alias.Length == 0)
                    result += comma + item.Column.Query.joinAlias + ".[" + item.Column.Name + "]";
                else
                    result += comma + "[" + item.Column.Name + "]";

                comma = ", ";
            }

            return result;
        }

        private static string GetPartitionColumnName(esQueryItem item, string alias = "")
        {
            if (alias.Length == 0)
                return item.Column.Query.joinAlias + ".[" + item.Column.Name + "]";
            else
                return alias + ".[" + item.Column.Name + "]";
        }

        private static string GetPartitionOderByColumnNames(List<esOrderByItem> items)
        {
            if (items == null) return "";

            string comma = " ";
            string result = "";

            foreach (esOrderByItem item in items)
            {
                result += comma + item.Expression.Query.joinAlias + ".[" + item.Expression.Column.Name + "]";
                comma = ", ";
            }

            return result;
        }

        private static string GetPartitionOderByColumnName(esOrderByItem item)
        {
            return item.Expression.Query.joinAlias + ".[" + item.Expression.Column.Name + "]";
        }
    }
}
