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
            return new esDenseRankOver();
        }

        /// <summary>
        /// Numbers the output of a result set. 
        /// More specifically, returns the sequential number of a row within a partition of a result set, starting at 1 for the first row in each partition.
        /// </summary>
        public esRowNumberOver RowNumber()
        {
            return new esRowNumberOver();
        }

        /// <summary>
        /// Calculates the relative rank of a row within a group of rows. 
        /// Use PERCENT_RANK to evaluate the relative standing of a value within a query result set or partition. 
        /// PERCENT_RANK is similar to the CUME_DIST function.
        /// </summary>
        public esPercentRankOver PercentRank()
        {
            return new esPercentRankOver();
        }

        /// <summary>
        /// Returns the rank of each row within the partition of a result set. 
        /// The rank of a row is one plus the number of ranks that come before the row in question.
        /// </summary>
        public esRankOver Rank()
        {
            return new esRankOver();
        }

        /// <summary>
        /// Distributes the rows in an ordered partition into a specified number of groups. 
        /// The groups are numbered, starting at one. For each row, NTILE returns the number of the group to which the row belongs.
        /// </summary>
        /// <param name="nTile">Is a positive integer expression that specifies the number of groups into which each partition must be divided. 
        /// nTile can be of type int, or long.</param>
        public esNtileOver Ntile(int nTile)
        {
            return new esNtileOver(nTile);
        }

        /// <summary>
        /// Distributes the rows in an ordered partition into a specified number of groups. 
        /// The groups are numbered, starting at one. For each row, NTILE returns the number of the group to which the row belongs.
        /// </summary>
        /// <param name="nTile">Is a positive integer expression that specifies the number of groups into which each partition must be divided. 
        /// nTile can be of type int, or long.</param>
        public esNtileOver Ntile(long nTile)
        {
            return new esNtileOver(nTile);
        }

        /// <summary>
        /// This function returns the average of the values in a group. It ignores null values.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esAvgOver Avg(esQueryItem columnExpression)
        {
            return new esAvgOver(columnExpression);
        }

        /// <summary>
        /// This function returns the number of items found in a group. COUNT operates like the COUNT_BIG function. 
        /// These functions differ only in the data types of their return values. 
        /// COUNT always returns an int data type value. COUNT_BIG always returns a bigint data type value.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esCountOver Count(esQueryItem columnExpression)
        {
            return new esCountOver(columnExpression);
        }

        /// <summary>
        /// This function returns the number of items found in a group. COUNT operates like the COUNT_BIG function. 
        /// These functions differ only in the data types of their return values. 
        /// COUNT always returns an int data type value. COUNT_BIG always returns a bigint data type value.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esCountBigOver CountBig(esQueryItem columnExpression)
        {
            return new esCountBigOver(columnExpression);
        }

        /// <summary>
        /// Returns the minimum value in the column or expression. 
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esMaxOver Max(esQueryItem columnExpression)
        {
            return new esMaxOver(columnExpression);
        }

        /// <summary>
        /// Returns the minimum value in the column or expression. 
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esMinOver Min(esQueryItem columnExpression)
        {
            return new esMinOver(columnExpression);
        }

        /// <summary>
        /// Returns the statistical standard deviation of all values in the specified column or expression.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esStdDevOver StdDev(esQueryItem columnExpression)
        {
            return new esStdDevOver(columnExpression);
        }

        /// <summary>
        /// Returns the statistical standard deviation for the population for all values in the specified column or expression.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esStdDevpOver StdDevP(esQueryItem columnExpression)
        {
            return new esStdDevpOver(columnExpression);
        }

        /// <summary>
        /// Returns the sum of all the values in the expression or column. 
        /// SUM can be used with numeric columns only. Null values are ignored.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esSumOver Sum(esQueryItem columnExpression)
        {
            return new esSumOver(columnExpression);
        }

        /// <summary>
        /// Returns the statistical variance of all values in the specified expression or column. 
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esVarOver Var(esQueryItem columnExpression)
        {
            return new esVarOver(columnExpression);
        }

        /// <summary>
        /// Returns the statistical variance for the population for all values in the specified expression or column.
        /// </summary>
        /// <param name="columnExpression">Column or Expression</param>
        public esVarpOver VarP(esQueryItem columnExpression)
        {
            return new esVarpOver(columnExpression);
        }
    }
}