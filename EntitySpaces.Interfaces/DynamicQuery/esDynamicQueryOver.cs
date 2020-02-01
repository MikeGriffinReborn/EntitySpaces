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
using System;
using System.Collections.Generic;
using System.Dynamic;

#if (LINQ)
using System.Data.Linq;
using System.Linq;
#endif


namespace EntitySpaces.Interfaces
{
    public partial class esDynamicQuery : DynamicObject, IDynamicQueryInternal
    {
        /// <summary>
        /// Determines the partitioning and ordering of a rowset before the associated window function is applied. 
        /// That is, the OVER clause defines a window or user-specified set of rows within a query result set. 
        /// A window function then computes a value for each row in the window.
        /// </summary>
        public esOverClause Over
        {
            get
            {
                return new esOverClause(this);
            }
        }

        private esQueryItem Alias(string alias)
        {
            esQueryItem item = null;

            foreach (esExpression exp in this.selectColumns)
            {
                if (exp.Column.Alias == alias)
                {
                    item = new esQueryItem(this, alias, exp.Column.Datatype);
                    item.Column = new esColumnItem();
                    item.Column.Query = this;
                    item.Column.Alias = alias;
                    item.Column.Name = alias;
                }

                if (exp.OverClause != null)
                {
                    if (exp.OverClause.Alias == alias)
                    {
                        item = new esQueryItem(this, alias, esSystemType.Int32);
                        item.Column = new esColumnItem();
                        item.Column.Datatype = exp.Column.Datatype;
                        item.Column.Query = this;
                        item.Column.Alias = alias;
                        item.Column.Name = alias;
                    }
                }

            }

            return item;
        }
    }

    public class esOverClause
    {
        internal esDynamicQuery _query;

        public esOverClause(esDynamicQuery query) => _query = query;

        /// <summary>
        /// This function returns the rank of each row within a result set partition, with no gaps in the ranking values. 
        /// The rank of a specific row is one plus the number of distinct rank values that come before that specific row.
        /// </summary>
        public esDenseRankOver DenseRank()
        {
            var obj = new esDenseRankOver();
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Numbers the output of a result set. 
        /// More specifically, returns the sequential number of a row within a partition of a result set, starting at 1 for the first row in each partition.
        /// </summary>
        public esRowNumberOver RowNumber()
        {
            var obj = new esRowNumberOver();
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Calculates the relative rank of a row within a group of rows. 
        /// Use PERCENT_RANK to evaluate the relative standing of a value within a query result set or partition. 
        /// PERCENT_RANK is similar to the CUME_DIST function.
        /// </summary>
        public esPercentRankOver PercentRank()
        {
            var obj = new esPercentRankOver();
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Returns the rank of each row within the partition of a result set. 
        /// The rank of a row is one plus the number of ranks that come before the row in question.
        /// </summary>
        public esRankOver Rank()
        {
            var obj = new esRankOver();
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Distributes the rows in an ordered partition into a specified number of groups. 
        /// The groups are numbered, starting at one. For each row, NTILE returns the number of the group to which the row belongs.
        /// </summary>
        /// <param name="nTile">Is a positive integer expression that specifies the number of groups into which each partition must be divided. 
        /// nTile can be of type int, or long.</param>
        public esNtileOver Ntile(int nTile)
        {
            var obj = new esNtileOver(nTile);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Distributes the rows in an ordered partition into a specified number of groups. 
        /// The groups are numbered, starting at one. For each row, NTILE returns the number of the group to which the row belongs.
        /// </summary>
        /// <param name="nTile">Is a positive integer expression that specifies the number of groups into which each partition must be divided. 
        /// nTile can be of type int, or long.</param>
        public esNtileOver Ntile(long nTile)
        {
            var obj = new esNtileOver(nTile);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// This function returns the average of the values in a group. It ignores null values.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esAvgOver Avg(esQueryItem columnExpression)
        {
            var obj = new esAvgOver(columnExpression);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// This function returns the number of items found in a group. COUNT operates like the COUNT_BIG function. 
        /// These functions differ only in the data types of their return values. 
        /// COUNT always returns an int data type value. COUNT_BIG always returns a bigint data type value.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esCountOver Count(esQueryItem columnExpression)
        {
            var obj = new esCountOver(columnExpression);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// This function returns the number of items found in a group. COUNT operates like the COUNT_BIG function. 
        /// These functions differ only in the data types of their return values. 
        /// COUNT always returns an int data type value. COUNT_BIG always returns a bigint data type value.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esCountBigOver CountBig(esQueryItem columnExpression)
        {
            var obj = new esCountBigOver(columnExpression);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Returns the minimum value in the column or expression. 
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esMaxOver Max(esQueryItem columnExpression)
        {
            var obj = new esMaxOver(columnExpression);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Returns the minimum value in the column or expression. 
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esMinOver Min(esQueryItem columnExpression)
        {
            var obj = new esMinOver(columnExpression);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Returns the statistical standard deviation of all values in the specified column or expression.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esStdDevOver StdDev(esQueryItem columnExpression)
        {
            var obj = new esStdDevOver(columnExpression);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Returns the statistical standard deviation for the population for all values in the specified column or expression.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esStdDevpOver StdDevP(esQueryItem columnExpression)
        {
            var obj = new esStdDevpOver(columnExpression);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Returns the sum of all the values in the expression or column. 
        /// SUM can be used with numeric columns only. Null values are ignored.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esSumOver Sum(esQueryItem columnExpression)
        {
            var obj = new esSumOver(columnExpression);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Returns the statistical variance of all values in the specified expression or column. 
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esVarOver Var(esQueryItem columnExpression)
        {
            var obj = new esVarOver(columnExpression);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Returns the statistical variance for the population for all values in the specified expression or column.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esVarpOver VarP(esQueryItem columnExpression)
        {
            var obj = new esVarpOver(columnExpression);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Accesses data from a previous row in the same result set without the use of a self-join. 
        /// LAG provides access to a row at a given physical offset that comes before the current row. 
        /// Use this analytic function in a SELECT statement to compare values in the current row with values in a previous row.
        /// </summary>
        /// <param name="expression">>Column or Expression</param>
        /// <param name="offset">The number of rows back from the current row from which to obtain a value. 
        /// If not specified, the default is 1. 
        /// offset can be a column, subquery, or other expression that evaluates to a positive integer or can be implicitly converted to bigint. 
        /// offset cannot be a negative value or an analytic function.</param>
        /// <param name="theDefault">The value to return when offset is beyond the scope of the partition. 
        /// If a default value is not specified, NULL is returned. default can be a column, subquery, or other expression, but it cannot be an analytic function. 
        /// default must be type-compatible with scalar_expression.</param>
        public esLagOver Lag(esQueryItem expression, int offset = 1, esQueryItem theDefault = null)
        {
            var obj = new esLagOver(expression, offset, theDefault);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Accesses data from a previous row in the same result set without the use of a self-join. 
        /// LAG provides access to a row at a given physical offset that comes before the current row. 
        /// Use this analytic function in a SELECT statement to compare values in the current row with values in a previous row.
        /// </summary>
        /// <param name="expression">>Column or Expression</param>
        /// <param name="offset">The number of rows back from the current row from which to obtain a value. 
        /// If not specified, the default is 1. 
        /// offset can be a column, subquery, or other expression that evaluates to a positive integer or can be implicitly converted to bigint. 
        /// offset cannot be a negative value or an analytic function.</param>
        /// <param name="theDefault">The value to return when offset is beyond the scope of the partition. 
        /// If a default value is not specified, NULL is returned. default can be a column, subquery, or other expression, but it cannot be an analytic function. 
        /// default must be type-compatible with scalar_expression.</param>
        public esLagOver Lag(esQueryItem expression, decimal offset = 1.0M, esQueryItem theDefault = null)
        {
            var obj = new esLagOver(expression, offset, theDefault);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Accesses data from a subsequent row in the same result set without the use of a self-join. 
        /// LEAD provides access to a row at a given physical offset that follows the current row. 
        /// Use this analytic function in a SELECT statement to compare values in the current row with values in a following row.
        /// </summary>
        /// <param name="expression">The value to be returned based on the specified offset. 
        /// It is an expression of any type that returns a single (scalar) value. 
        /// The expression cannot be an analytic function.</param>
        /// <param name="offset">he number of rows forward from the current row from which to obtain a value. 
        /// If not specified, the default is 1. 
        /// offset can be a column, subquery, or other expression that evaluates to a positive integer or can be implicitly converted to bigint. 
        /// offset cannot be a negative value or an analytic function.</param>
        /// <param name="theDefault"></param>
        public esLeadOver Lead(esQueryItem expression, int offset = 1, esQueryItem theDefault = null)
        {
            var obj = new esLeadOver(expression, offset, theDefault);
            obj.query = _query;
            return obj;

        }

        /// <summary>
        /// Accesses data from a subsequent row in the same result set without the use of a self-join. 
        /// LEAD provides access to a row at a given physical offset that follows the current row. 
        /// Use this analytic function in a SELECT statement to compare values in the current row with values in a following row.
        /// </summary>
        /// <param name="expression">The value to be returned based on the specified offset. 
        /// It is an expression of any type that returns a single (scalar) value. 
        /// The expression cannot be an analytic function.</param>
        /// <param name="offset">he number of rows forward from the current row from which to obtain a value. 
        /// If not specified, the default is 1. 
        /// offset can be a column, subquery, or other expression that evaluates to a positive integer or can be implicitly converted to bigint. 
        /// offset cannot be a negative value or an analytic function.</param>
        /// <param name="theDefault"></param>
        public esLeadOver Lead(esQueryItem expression, decimal offset = 1.0M, esQueryItem theDefault = null)
        {
            var obj = new esLeadOver(expression, offset, theDefault);
            obj.query = _query;
            return obj;

        }

        /// <summary>
        /// This function calculates the cumulative distribution of a value within a group of values. 
        /// In other words, CUME_DIST calculates the relative position of a specified value in a group of values. 
        /// Assuming ascending ordering, the CUME_DIST of a value in row r is defined as the number of rows with values less than or equal to that value in row r, 
        /// divided by the number of rows evaluated in the partition or query result set. 
        /// CUME_DIST is similar to the PERCENT_RANK function.
        /// </summary>
        public esCumeDistOver CumeDist()
        {
            var obj = new esCumeDistOver();
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Returns the first value in an ordered set of values
        /// </summary>
        /// <param name="expression">Is the value to be returned. 
        /// expression can be a column, subquery, or other arbitrary expression that results in a single value. 
        /// Other analytic functions are not permitted.</param>
        public esFirstValueOver FirstValue(esQueryItem expression)
        {
            var obj = new esFirstValueOver(expression);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Returns the last value in an ordered set of values
        /// </summary>
        /// <param name="expression">Is the value to be returned. 
        /// expression can be a column, subquery, or other expression that results in a single value. 
        /// Other analytic functions are not permitted.</param>
        public esLastValueOver LastValue(esQueryItem expression)
        {
            var obj = new esLastValueOver(expression);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Calculates a percentile based on a continuous distribution of the column value. 
        /// The result is interpolated and might not be equal to any of the specific values in the column.
        /// </summary>
        /// <param name="literal">The percentile to compute. The value must range between 0.0 and 1.0.</param>
        public esPercentileContOver PercentileCont(string literal)
        {
            var obj = new esPercentileContOver(literal);
            obj.query = _query;
            return obj;
        }

        /// <summary>
        /// Computes a specific percentile for sorted values in an entire rowset or within a rowset's distinct partitions. 
        /// For a given percentile value P, PERCENTILE_DISC sorts the expression values in the ORDER BY clause. 
        /// It then returns the value with the smallest CUME_DIST value given (with respect to the same sort specification) that is greater than or equal to P. 
        /// For example, PERCENTILE_DISC (0.5) will compute the 50th percentile (that is, the median) of an expression. 
        /// PERCENTILE_DISC calculates the percentile based on a discrete distribution of the column values. The result is equal to a specific column value.
        /// </summary>
        /// <param name="literal">The percentile to compute. The value must range between 0.0 and 1.0.</param>
        public esPercentileDiscOver PercentileDisc(string literal)
        {
            var obj = new esPercentileDiscOver(literal);
            obj.query = _query;
            return obj;
        }
    }
}