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
using System.ComponentModel;

#if (WCF)
using System.Runtime.Serialization;
#endif

namespace EntitySpaces.DynamicQuery
{
    /// <summary>
    /// The esQueryItem class is dynamically created by your BusinessEntity's
    /// DynamicQuery mechanism.
    /// This class is mostly used by the EntitySpaces architecture, not the programmer.
    /// </summary>
    /// <example>
    /// You will not call esQueryItem directly, but will be limited to use as
    /// in the example below, or to the many uses posted here:
    /// <code>
    /// http://www.entityspaces.net/portal/QueryAPISamples/tabid/80/Default.aspx
    /// </code>
    /// This will be the extent of your use of the esQueryItem class:
    /// <code>
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
    ///		emps.Query.LastName
    /// )
    /// .OrderBy
    /// (
    ///		emps.Query.LastName.Descending,
    ///		emps.Query.FirstName.Ascending
    /// );
    /// </code>
    /// </example>
#if !SILVERLIGHT
    [Serializable]
#endif
#if (WCF)
    [DataContract(Namespace = "es", IsReference = true)]
#endif  
    public class esQueryItem
    {
        private esQueryItem()
        {
            this.Expression = new esMathmaticalExpression();
        }

        /// <summary>
        /// The esQueryItem class is dynamically created by your
        /// BusinessEntity's DynamicQuery mechanism.
        /// </summary>
        /// <param name="query">The esDynamicQueryTransport passed in via DynamicQuery</param>
        /// <param name="columnName">The columnName passed in via DynamicQuery</param>
        /// <param name="datatype">The esSystemType</param>
        public esQueryItem(esDynamicQuerySerializable query, string columnName, esSystemType datatype)
        {
            this.query = query;
            this.Column.Query = query;
            this.Column.Name = columnName;
            this.Column.Query.es.JoinAlias = query.es.JoinAlias;
            this.Column.Datatype = datatype;
        }

        #region operators applied to other QueryItems (LiteralExpression)

        #region > operator literal overloads

        /// <summary>
        /// Greater Than &gt; (to use in Where clauses).
        /// </summary>
        /// <example>
        /// The operators provide an alternative, natural syntax for DynamicQueries.
        /// <code>
        ///	emps.Query.Where(emps.Query.BirthDate &gt; "2000-01-01");
        /// </code>
        /// </example>
        /// <param name="item">Passed in via DynamicQuery</param>
        /// <param name="value">Passed in via DynamicQuery</param>
        /// <returns>The esComparison returned to DynamicQuery</returns>
        public static esComparison operator >(esQueryItem item, esQueryItem value)
        {
            esComparison wi = new esComparison(item.query);
            wi.Operand = esComparisonOperand.GreaterThan;

            wi.data.Column = item.Column;
            wi.data.ComparisonColumn = value.Column;

            wi.SubOperators = item.SubOperators;
            return wi;
        }

        private static esComparison GreaterThan(esQueryItem queryItem, object literal, esSystemType literalType, bool itemFirst)
        {
            esComparison wi = new esComparison(queryItem.query);
            wi.Operand = esComparisonOperand.GreaterThan;

            wi.data.Column = queryItem.Column;
            wi.data.Value = literal;
            wi.data.Expression = queryItem.Expression;
            wi.data.ItemFirst = itemFirst;

            wi.SubOperators = queryItem.SubOperators;
            return wi;
        }

        // esSystemType.Boolean
        public static esComparison operator >(esQueryItem item1, bool literal)
        {
            return GreaterThan(item1, literal, esSystemType.Boolean, true);
        }

        public static esComparison operator >(bool literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esComparison operator >(esQueryItem item1, byte literal)
        {
            return GreaterThan(item1, literal, esSystemType.Byte, true);
        }

        public static esComparison operator >(byte literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esComparison operator >(esQueryItem item1, char literal)
        {
            return GreaterThan(item1, literal, esSystemType.Char, true);
        }

        public static esComparison operator >(char literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esComparison operator >(esQueryItem item1, DateTime literal)
        {
            return GreaterThan(item1, literal, esSystemType.DateTime, true);
        }

        public static esComparison operator >(DateTime literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esComparison operator >(esQueryItem item1, double literal)
        {
            return GreaterThan(item1, literal, esSystemType.Double, true);
        }

        public static esComparison operator >(double literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esComparison operator >(esQueryItem item1, decimal literal)
        {
            return GreaterThan(item1, literal, esSystemType.Decimal, true);
        }

        public static esComparison operator >(decimal literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esComparison operator >(esQueryItem item1, Guid literal)
        {
            return GreaterThan(item1, literal, esSystemType.Guid, true);
        }

        public static esComparison operator >(Guid literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esComparison operator >(esQueryItem item1, short literal)
        {
            return GreaterThan(item1, literal, esSystemType.Int16, true);
        }

        public static esComparison operator >(short literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esComparison operator >(esQueryItem item1, int literal)
        {
            return GreaterThan(item1, literal, esSystemType.Int32, true);
        }

        public static esComparison operator >(int literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esComparison operator >(esQueryItem item1, long literal)
        {
            return GreaterThan(item1, literal, esSystemType.Int64, true);
        }

        public static esComparison operator >(long literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.Object
        public static esComparison operator >(esQueryItem item1, object literal)
        {
            return GreaterThan(item1, literal, esSystemType.Object, true);
        }

        public static esComparison operator >(object literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Object, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esComparison operator >(esQueryItem item1, sbyte literal)
        {
            return GreaterThan(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >(sbyte literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esComparison operator >(esQueryItem item1, float literal)
        {
            return GreaterThan(item1, literal, esSystemType.Single, true);
        }

        public static esComparison operator >(float literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.String
        public static esComparison operator >(esQueryItem item1, string literal)
        {
            return GreaterThan(item1, literal, esSystemType.String, true);
        }

        public static esComparison operator >(string literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.String, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esComparison operator >(esQueryItem item1, ushort literal)
        {
            return GreaterThan(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >(ushort literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esComparison operator >(esQueryItem item1, uint literal)
        {
            return GreaterThan(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >(uint literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esComparison operator >(esQueryItem item1, ulong literal)
        {
            return GreaterThan(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >(ulong literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region < operator literal overloads

        /// <summary>
        /// Less Than &lt; (to use in Where clauses).
        /// </summary>
        /// <example>
        /// The operators provide an alternative, natural syntax for DynamicQueries.
        /// <code>
        ///	emps.Query.Where(emps.Query.BirthDate &lt; "2000-01-01");
        /// </code>
        /// </example>
        /// <param name="item">Passed in via DynamicQuery</param>
        /// <param name="value">Passed in via DynamicQuery</param>
        /// <returns>The esComparison returned to DynamicQuery</returns>
        public static esComparison operator <(esQueryItem item, esQueryItem value)
        {
            esComparison wi = new esComparison(item.query);
            wi.Operand = esComparisonOperand.LessThan;

            wi.data.Column = item.Column;
            wi.data.ComparisonColumn = value.Column;

            wi.SubOperators = item.SubOperators;
            return wi;
        }

        private static esComparison LessThan(esQueryItem queryItem, object literal, esSystemType literalType, bool itemFirst)
        {
            esComparison wi = new esComparison(queryItem.query);
            wi.Operand = esComparisonOperand.LessThan;

            wi.data.Column = queryItem.Column;
            wi.data.Value = literal;
            wi.data.Expression = queryItem.Expression;
            wi.data.ItemFirst = itemFirst;

            wi.SubOperators = queryItem.SubOperators;
            return wi;
        }

        // esSystemType.Boolean
        public static esComparison operator <(esQueryItem item1, bool literal)
        {
            return LessThan(item1, literal, esSystemType.Boolean, true);
        }

        public static esComparison operator <(bool literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esComparison operator <(esQueryItem item1, byte literal)
        {
            return LessThan(item1, literal, esSystemType.Byte, true);
        }

        public static esComparison operator <(byte literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esComparison operator <(esQueryItem item1, char literal)
        {
            return LessThan(item1, literal, esSystemType.Char, true);
        }

        public static esComparison operator <(char literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esComparison operator <(esQueryItem item1, DateTime literal)
        {
            return LessThan(item1, literal, esSystemType.DateTime, true);
        }

        public static esComparison operator <(DateTime literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esComparison operator <(esQueryItem item1, double literal)
        {
            return LessThan(item1, literal, esSystemType.Double, true);
        }

        public static esComparison operator <(double literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esComparison operator <(esQueryItem item1, decimal literal)
        {
            return LessThan(item1, literal, esSystemType.Decimal, true);
        }

        public static esComparison operator <(decimal literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esComparison operator <(esQueryItem item1, Guid literal)
        {
            return LessThan(item1, literal, esSystemType.Guid, true);
        }

        public static esComparison operator <(Guid literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esComparison operator <(esQueryItem item1, short literal)
        {
            return LessThan(item1, literal, esSystemType.Int16, true);
        }

        public static esComparison operator <(short literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esComparison operator <(esQueryItem item1, int literal)
        {
            return LessThan(item1, literal, esSystemType.Int32, true);
        }

        public static esComparison operator <(int literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esComparison operator <(esQueryItem item1, long literal)
        {
            return LessThan(item1, literal, esSystemType.Int64, true);
        }

        public static esComparison operator <(long literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.Object
        public static esComparison operator <(esQueryItem item1, object literal)
        {
            return LessThan(item1, literal, esSystemType.Object, true);
        }

        public static esComparison operator <(object literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Object, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esComparison operator <(esQueryItem item1, sbyte literal)
        {
            return LessThan(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <(sbyte literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esComparison operator <(esQueryItem item1, float literal)
        {
            return LessThan(item1, literal, esSystemType.Single, true);
        }

        public static esComparison operator <(float literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.String
        public static esComparison operator <(esQueryItem item1, string literal)
        {
            return LessThan(item1, literal, esSystemType.String, true);
        }

        public static esComparison operator <(string literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.String, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esComparison operator <(esQueryItem item1, ushort literal)
        {
            return LessThan(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <(ushort literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esComparison operator <(esQueryItem item1, uint literal)
        {
            return LessThan(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <(uint literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esComparison operator <(esQueryItem item1, ulong literal)
        {
            return LessThan(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <(ulong literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region <= operator literal overloads

        /// <summary>
        /// Less Than or Equal &lt;= (to use in Where clauses).
        /// </summary>
        /// <example>
        /// The operators provide an alternative, natural syntax for DynamicQueries.
        /// <code>
        ///	emps.Query.Where(emps.Query.BirthDate &lt;= "2000-01-01");
        /// </code>
        /// </example>
        /// <param name="item">Passed in via DynamicQuery</param>
        /// <param name="value">Passed in via DynamicQuery</param>
        /// <returns>The esComparison returned to DynamicQuery</returns>
        public static esComparison operator <=(esQueryItem item, esQueryItem value)
        {
            esComparison wi = new esComparison(item.query);
            wi.Operand = esComparisonOperand.LessThanOrEqual;

            wi.data.Column = item.Column;
            wi.data.ComparisonColumn = value.Column;

            wi.SubOperators = item.SubOperators;
            return wi;
        }

        private static esComparison LessThanOrEqual(esQueryItem queryItem, object literal, esSystemType literalType, bool itemFirst)
        {
            esComparison wi = new esComparison(queryItem.query);
            wi.Operand = esComparisonOperand.LessThanOrEqual;

            wi.data.Column = queryItem.Column;
            wi.data.Value = literal;
            wi.data.Expression = queryItem.Expression;
            wi.data.ItemFirst = itemFirst;

            wi.SubOperators = queryItem.SubOperators;
            return wi;
        }

        // esSystemType.Boolean
        public static esComparison operator <=(esQueryItem item1, bool literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Boolean, true);
        }

        public static esComparison operator <=(bool literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esComparison operator <=(esQueryItem item1, byte literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Byte, true);
        }

        public static esComparison operator <=(byte literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esComparison operator <=(esQueryItem item1, char literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Char, true);
        }

        public static esComparison operator <=(char literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esComparison operator <=(esQueryItem item1, DateTime literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.DateTime, true);
        }

        public static esComparison operator <=(DateTime literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esComparison operator <=(esQueryItem item1, double literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Double, true);
        }

        public static esComparison operator <=(double literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esComparison operator <=(esQueryItem item1, decimal literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Decimal, true);
        }

        public static esComparison operator <=(decimal literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esComparison operator <=(esQueryItem item1, Guid literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Guid, true);
        }

        public static esComparison operator <=(Guid literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esComparison operator <=(esQueryItem item1, short literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Int16, true);
        }

        public static esComparison operator <=(short literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esComparison operator <=(esQueryItem item1, int literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Int32, true);
        }

        public static esComparison operator <=(int literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esComparison operator <=(esQueryItem item1, long literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Int64, true);
        }

        public static esComparison operator <=(long literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.Object
        public static esComparison operator <=(esQueryItem item1, object literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Object, true);
        }

        public static esComparison operator <=(object literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Object, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esComparison operator <=(esQueryItem item1, sbyte literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <=(sbyte literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esComparison operator <=(esQueryItem item1, float literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Single, true);
        }

        public static esComparison operator <=(float literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.String
        public static esComparison operator <=(esQueryItem item1, string literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.String, true);
        }

        public static esComparison operator <=(string literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.String, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esComparison operator <=(esQueryItem item1, ushort literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <=(ushort literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esComparison operator <=(esQueryItem item1, uint literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <=(uint literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esComparison operator <=(esQueryItem item1, ulong literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <=(ulong literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region >= operator literal overloads

        /// <summary>
        /// Greater Than or Equal &gt;= (to use in Where clauses).
        /// </summary>
        /// <example>
        /// The operators provide an alternative, natural syntax for DynamicQueries.
        /// <code>
        ///	emps.Query.Where(emps.Query.BirthDate &gt;= "2000-01-01");
        /// </code>
        /// </example>
        /// <param name="item">Passed in via DynamicQuery</param>
        /// <param name="value">Passed in via DynamicQuery</param>
        /// <returns>The esComparison returned to DynamicQuery</returns>
        public static esComparison operator >=(esQueryItem item, esQueryItem value)
        {
            esComparison wi = new esComparison(item.query);
            wi.Operand = esComparisonOperand.GreaterThanOrEqual;

            wi.data.Column = item.Column;
            wi.data.ComparisonColumn = value.Column;

            wi.SubOperators = item.SubOperators;
            return wi;
        }

        private static esComparison GreaterThanOrEqual(esQueryItem queryItem, object literal, esSystemType literalType, bool itemFirst)
        {
            esComparison wi = new esComparison(queryItem.query);
            wi.Operand = esComparisonOperand.GreaterThanOrEqual;

            wi.data.Column = queryItem.Column;
            wi.data.Value = literal;
            wi.data.Expression = queryItem.Expression;
            wi.data.ItemFirst = itemFirst;

            wi.SubOperators = queryItem.SubOperators;
            return wi;
        }

        // esSystemType.Boolean
        public static esComparison operator >=(esQueryItem item1, bool literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Boolean, true);
        }

        public static esComparison operator >=(bool literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esComparison operator >=(esQueryItem item1, byte literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Byte, true);
        }

        public static esComparison operator >=(byte literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esComparison operator >=(esQueryItem item1, char literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Char, true);
        }

        public static esComparison operator >=(char literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esComparison operator >=(esQueryItem item1, DateTime literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.DateTime, true);
        }

        public static esComparison operator >=(DateTime literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esComparison operator >=(esQueryItem item1, double literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Double, true);
        }

        public static esComparison operator >=(double literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esComparison operator >=(esQueryItem item1, decimal literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Decimal, true);
        }

        public static esComparison operator >=(decimal literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esComparison operator >=(esQueryItem item1, Guid literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Guid, true);
        }

        public static esComparison operator >=(Guid literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esComparison operator >=(esQueryItem item1, short literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Int16, true);
        }

        public static esComparison operator >=(short literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esComparison operator >=(esQueryItem item1, int literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Int32, true);
        }

        public static esComparison operator >=(int literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esComparison operator >=(esQueryItem item1, long literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Int64, true);
        }

        public static esComparison operator >=(long literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.Object
        public static esComparison operator >=(esQueryItem item1, object literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Object, true);
        }

        public static esComparison operator >=(object literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Object, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esComparison operator >=(esQueryItem item1, sbyte literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >=(sbyte literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esComparison operator >=(esQueryItem item1, float literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Single, true);
        }

        public static esComparison operator >=(float literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.String
        public static esComparison operator >=(esQueryItem item1, string literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.String, true);
        }

        public static esComparison operator >=(string literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.String, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esComparison operator >=(esQueryItem item1, ushort literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >=(ushort literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esComparison operator >=(esQueryItem item1, uint literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >=(uint literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esComparison operator >=(esQueryItem item1, ulong literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >=(ulong literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region == operator literal overloads

        public static esComparison operator ==(esQueryItem item, esQueryItem value)
        {
            esComparison wi = new esComparison(item.query);
            wi.Operand = esComparisonOperand.Equal;

            wi.data.Column = item.Column;
            wi.data.ComparisonColumn = value.Column;

            wi.SubOperators = item.SubOperators;
            return wi;
        }

        private static esComparison EqualOperator(esQueryItem queryItem, object literal, esSystemType literalType, bool itemFirst)
        {
            esComparison wi = new esComparison(queryItem.query);
            wi.Operand = esComparisonOperand.Equal;

            wi.data.Column = queryItem.Column;
            wi.data.Value = literal;
            wi.data.Expression = queryItem.Expression;
            wi.data.ItemFirst = itemFirst;

            wi.SubOperators = queryItem.SubOperators;
            return wi;
        }

        // esSystemType.Boolean
        public static esComparison operator ==(esQueryItem item1, bool literal)
        {
            return EqualOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esComparison operator ==(bool literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esComparison operator ==(esQueryItem item1, byte literal)
        {
            return EqualOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esComparison operator ==(byte literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esComparison operator ==(esQueryItem item1, char literal)
        {
            return EqualOperator(item1, literal, esSystemType.Char, true);
        }

        public static esComparison operator ==(char literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esComparison operator ==(esQueryItem item1, DateTime literal)
        {
            return EqualOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esComparison operator ==(DateTime literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esComparison operator ==(esQueryItem item1, double literal)
        {
            return EqualOperator(item1, literal, esSystemType.Double, true);
        }

        public static esComparison operator ==(double literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esComparison operator ==(esQueryItem item1, decimal literal)
        {
            return EqualOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esComparison operator ==(decimal literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esComparison operator ==(esQueryItem item1, Guid literal)
        {
            return EqualOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esComparison operator ==(Guid literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esComparison operator ==(esQueryItem item1, short literal)
        {
            return EqualOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esComparison operator ==(short literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esComparison operator ==(esQueryItem item1, int literal)
        {
            return EqualOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esComparison operator ==(int literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esComparison operator ==(esQueryItem item1, long literal)
        {
            return EqualOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esComparison operator ==(long literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.Object
        public static esComparison operator ==(esQueryItem item1, object literal)
        {
            return EqualOperator(item1, literal, esSystemType.Object, true);
        }

        public static esComparison operator ==(object literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Object, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esComparison operator ==(esQueryItem item1, sbyte literal)
        {
            return EqualOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator ==(sbyte literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esComparison operator ==(esQueryItem item1, float literal)
        {
            return EqualOperator(item1, literal, esSystemType.Single, true);
        }

        public static esComparison operator ==(float literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.String
        public static esComparison operator ==(esQueryItem item1, string literal)
        {
            return EqualOperator(item1, literal, esSystemType.String, true);
        }

        public static esComparison operator ==(string literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.String, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esComparison operator ==(esQueryItem item1, ushort literal)
        {
            return EqualOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator ==(ushort literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esComparison operator ==(esQueryItem item1, uint literal)
        {
            return EqualOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator ==(uint literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esComparison operator ==(esQueryItem item1, ulong literal)
        {
            return EqualOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator ==(ulong literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region != operator literal overloads

        public static esComparison operator !=(esQueryItem item, esQueryItem value)
        {
            esComparison wi = new esComparison(item.query);
            wi.Operand = esComparisonOperand.NotEqual;

            wi.data.Column = item.Column;
            wi.data.ComparisonColumn = value.Column;

            wi.SubOperators = item.SubOperators;

            return wi;
        }

        private static esComparison NotEqualOperator(esQueryItem queryItem, object literal, esSystemType literalType, bool itemFirst)
        {
            esComparison wi = new esComparison(queryItem.query);
            wi.Operand = esComparisonOperand.NotEqual;

            wi.data.Column = queryItem.Column;
            wi.data.Value = literal;
            wi.data.Expression = queryItem.Expression;
            wi.data.ItemFirst = itemFirst;

            wi.SubOperators = queryItem.SubOperators;
            return wi;
        }

        // esSystemType.Boolean
        public static esComparison operator !=(esQueryItem item1, bool literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esComparison operator !=(bool literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esComparison operator !=(esQueryItem item1, byte literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esComparison operator !=(byte literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esComparison operator !=(esQueryItem item1, char literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Char, true);
        }

        public static esComparison operator !=(char literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esComparison operator !=(esQueryItem item1, DateTime literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esComparison operator !=(DateTime literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esComparison operator !=(esQueryItem item1, double literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Double, true);
        }

        public static esComparison operator !=(double literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esComparison operator !=(esQueryItem item1, decimal literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esComparison operator !=(decimal literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esComparison operator !=(esQueryItem item1, Guid literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esComparison operator !=(Guid literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esComparison operator !=(esQueryItem item1, short literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esComparison operator !=(short literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esComparison operator !=(esQueryItem item1, int literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esComparison operator !=(int literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esComparison operator !=(esQueryItem item1, long literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esComparison operator !=(long literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.Object
        public static esComparison operator !=(esQueryItem item1, object literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Object, true);
        }

        public static esComparison operator !=(object literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Object, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esComparison operator !=(esQueryItem item1, sbyte literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator !=(sbyte literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esComparison operator !=(esQueryItem item1, float literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Single, true);
        }

        public static esComparison operator !=(float literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.String
        public static esComparison operator !=(esQueryItem item1, string literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.String, true);
        }

        public static esComparison operator !=(string literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.String, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esComparison operator !=(esQueryItem item1, ushort literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.UInt16, true);
        }
        
        [CLSCompliant(false)]
        public static esComparison operator !=(ushort literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esComparison operator !=(esQueryItem item1, uint literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator !=(uint literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esComparison operator !=(esQueryItem item1, ulong literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator !=(ulong literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region Arithmetic Expressions

        public static esQueryItem operator +(esQueryItem item1, esQueryItem item2)
        {
            esQueryItem qi = new esQueryItem();
            qi.Expression.SelectItem1 = item1;
            qi.Expression.Operator = esArithmeticOperator.Add;
            qi.Expression.SelectItem2 = item2;
            return qi;
        }

        #region + operator literal overloads

        private static esQueryItem AddOperator(esQueryItem queryItem, object literal, esSystemType literalType, bool itemFirst)
        {
            esQueryItem qi = new esQueryItem();
            qi.Expression.SelectItem1 = queryItem;
            qi.Expression.Operator = esArithmeticOperator.Add;
            qi.Expression.Literal = literal;
            qi.Expression.LiteralType = literalType;
            qi.Expression.ItemFirst = itemFirst;
            return qi;
        }

        // esSystemType.Boolean
        public static esQueryItem operator +(esQueryItem item1, bool literal)
        {
            return AddOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esQueryItem operator +(bool literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esQueryItem operator +(esQueryItem item1, byte literal)
        {
            return AddOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esQueryItem operator +(byte literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esQueryItem operator +(esQueryItem item1, char literal)
        {
            return AddOperator(item1, literal, esSystemType.Char, true);
        }

        public static esQueryItem operator +(char literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esQueryItem operator +(esQueryItem item1, DateTime literal)
        {
            return AddOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esQueryItem operator +(DateTime literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esQueryItem operator +(esQueryItem item1, double literal)
        {
            return AddOperator(item1, literal, esSystemType.Double, true);
        }

        public static esQueryItem operator +(double literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esQueryItem operator +(esQueryItem item1, decimal literal)
        {
            return AddOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esQueryItem operator +(decimal literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esQueryItem operator +(esQueryItem item1, Guid literal)
        {
            return AddOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esQueryItem operator +(Guid literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esQueryItem operator +(esQueryItem item1, short literal)
        {
            return AddOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esQueryItem operator +(short literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esQueryItem operator +(esQueryItem item1, int literal)
        {
            return AddOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esQueryItem operator +(int literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esQueryItem operator +(esQueryItem item1, long literal)
        {
            return AddOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esQueryItem operator +(long literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.Object
        public static esQueryItem operator +(esQueryItem item1, object literal)
        {
            return AddOperator(item1, literal, esSystemType.Object, true);
        }

        public static esQueryItem operator +(object literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Object, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esQueryItem operator +(esQueryItem item1, sbyte literal)
        {
            return AddOperator(item1, literal, esSystemType.SByte, true);
        }
        
        [CLSCompliant(false)]
        public static esQueryItem operator +(sbyte literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esQueryItem operator +(esQueryItem item1, float literal)
        {
            return AddOperator(item1, literal, esSystemType.Single, true);
        }

        public static esQueryItem operator +(float literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.String
        public static esQueryItem operator +(esQueryItem item1, string literal)
        {
            return AddOperator(item1, literal, esSystemType.String, true);
        }

        public static esQueryItem operator +(string literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.String, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esQueryItem operator +(esQueryItem item1, ushort literal)
        {
            return AddOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator +(ushort literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esQueryItem operator +(esQueryItem item1, uint literal)
        {
            return AddOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator +(uint literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esQueryItem operator +(esQueryItem item1, ulong literal)
        {
            return AddOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator +(ulong literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        public static esQueryItem operator -(esQueryItem item1, esQueryItem item2)
        {
            esQueryItem qi = new esQueryItem();
            qi.Expression.SelectItem1 = item1;
            qi.Expression.Operator = esArithmeticOperator.Subtract;
            qi.Expression.SelectItem2 = item2;
            return qi;
        }

        #region - operator literal overloads

        private static esQueryItem SubtractOperator(esQueryItem queryItem, object literal, esSystemType literalType, bool itemFirst)
        {
            esQueryItem qi = new esQueryItem();
            qi.Expression.SelectItem1 = queryItem;
            qi.Expression.Operator = esArithmeticOperator.Subtract;
            qi.Expression.Literal = literal;
            qi.Expression.LiteralType = literalType;
            qi.Expression.ItemFirst = itemFirst;
            return qi;
        }

        // esSystemType.Boolean
        public static esQueryItem operator -(esQueryItem item1, bool literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esQueryItem operator -(bool literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esQueryItem operator -(esQueryItem item1, byte literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esQueryItem operator -(byte literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esQueryItem operator -(esQueryItem item1, char literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Char, true);
        }

        public static esQueryItem operator -(char literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esQueryItem operator -(esQueryItem item1, DateTime literal)
        {
            return SubtractOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esQueryItem operator -(DateTime literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esQueryItem operator -(esQueryItem item1, double literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Double, true);
        }

        public static esQueryItem operator -(double literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esQueryItem operator -(esQueryItem item1, decimal literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esQueryItem operator -(decimal literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esQueryItem operator -(esQueryItem item1, Guid literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esQueryItem operator -(Guid literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esQueryItem operator -(esQueryItem item1, short literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esQueryItem operator -(short literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esQueryItem operator -(esQueryItem item1, int literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esQueryItem operator -(int literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esQueryItem operator -(esQueryItem item1, long literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esQueryItem operator -(long literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.Object
        public static esQueryItem operator -(esQueryItem item1, object literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Object, true);
        }

        public static esQueryItem operator -(object literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Object, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esQueryItem operator -(esQueryItem item1, sbyte literal)
        {
            return SubtractOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator -(sbyte literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esQueryItem operator -(esQueryItem item1, float literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Single, true);
        }

        public static esQueryItem operator -(float literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.String
        public static esQueryItem operator -(esQueryItem item1, string literal)
        {
            return SubtractOperator(item1, literal, esSystemType.String, true);
        }

        public static esQueryItem operator -(string literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.String, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esQueryItem operator -(esQueryItem item1, ushort literal)
        {
            return SubtractOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator -(ushort literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esQueryItem operator -(esQueryItem item1, uint literal)
        {
            return SubtractOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator -(uint literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esQueryItem operator -(esQueryItem item1, ulong literal)
        {
            return SubtractOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator -(ulong literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        public static esQueryItem operator *(esQueryItem item1, esQueryItem item2)
        {
            esQueryItem qi = new esQueryItem();
            qi.Expression.SelectItem1 = item1;
            qi.Expression.Operator = esArithmeticOperator.Multiply;
            qi.Expression.SelectItem2 = item2;
            return qi;
        }

        #region * operator literal overloads

        private static esQueryItem MultiplyOperator(esQueryItem queryItem, object literal, esSystemType literalType, bool itemFirst)
        {
            esQueryItem qi = new esQueryItem();
            qi.Expression.SelectItem1 = queryItem;
            qi.Expression.Operator = esArithmeticOperator.Multiply;
            qi.Expression.Literal = literal;
            qi.Expression.LiteralType = literalType;
            qi.Expression.ItemFirst = itemFirst;
            return qi;
        }

        // esSystemType.Boolean
        public static esQueryItem operator *(esQueryItem item1, bool literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esQueryItem operator *(bool literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esQueryItem operator *(esQueryItem item1, byte literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esQueryItem operator *(byte literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esQueryItem operator *(esQueryItem item1, char literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Char, true);
        }

        public static esQueryItem operator *(char literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esQueryItem operator *(esQueryItem item1, DateTime literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esQueryItem operator *(DateTime literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esQueryItem operator *(esQueryItem item1, double literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Double, true);
        }

        public static esQueryItem operator *(double literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esQueryItem operator *(esQueryItem item1, decimal literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esQueryItem operator *(decimal literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esQueryItem operator *(esQueryItem item1, Guid literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esQueryItem operator *(Guid literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esQueryItem operator *(esQueryItem item1, short literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esQueryItem operator *(short literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esQueryItem operator *(esQueryItem item1, int literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esQueryItem operator *(int literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esQueryItem operator *(esQueryItem item1, long literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esQueryItem operator *(long literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.Object
        public static esQueryItem operator *(esQueryItem item1, object literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Object, true);
        }

        public static esQueryItem operator *(object literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Object, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esQueryItem operator *(esQueryItem item1, sbyte literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator *(sbyte literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esQueryItem operator *(esQueryItem item1, float literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Single, true);
        }

        public static esQueryItem operator *(float literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.String
        public static esQueryItem operator *(esQueryItem item1, string literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.String, true);
        }

        public static esQueryItem operator *(string literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.String, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esQueryItem operator *(esQueryItem item1, ushort literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator *(ushort literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esQueryItem operator *(esQueryItem item1, uint literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator *(uint literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esQueryItem operator *(esQueryItem item1, ulong literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator *(ulong literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        public static esQueryItem operator /(esQueryItem item1, esQueryItem item2)
        {
            esQueryItem qi = new esQueryItem();
            qi.Expression.SelectItem1 = item1;
            qi.Expression.Operator = esArithmeticOperator.Divide;
            qi.Expression.SelectItem2 = item2;
            return qi;
        }

        #region / operator literal overloads

        private static esQueryItem DivideOperator(esQueryItem queryItem, object literal, esSystemType literalType, bool itemFirst)
        {
            esQueryItem qi = new esQueryItem();
            qi.Expression.SelectItem1 = queryItem;
            qi.Expression.Operator = esArithmeticOperator.Divide;
            qi.Expression.Literal = literal;
            qi.Expression.LiteralType = literalType;
            qi.Expression.ItemFirst = itemFirst;
            return qi;
        }

        // esSystemType.Boolean
        public static esQueryItem operator /(esQueryItem item1, bool literal)
        {
            return DivideOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esQueryItem operator /(bool literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esQueryItem operator /(esQueryItem item1, byte literal)
        {
            return DivideOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esQueryItem operator /(byte literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esQueryItem operator /(esQueryItem item1, char literal)
        {
            return DivideOperator(item1, literal, esSystemType.Char, true);
        }

        public static esQueryItem operator /(char literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esQueryItem operator /(esQueryItem item1, DateTime literal)
        {
            return DivideOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esQueryItem operator /(DateTime literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esQueryItem operator /(esQueryItem item1, double literal)
        {
            return DivideOperator(item1, literal, esSystemType.Double, true);
        }

        public static esQueryItem operator /(double literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esQueryItem operator /(esQueryItem item1, decimal literal)
        {
            return DivideOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esQueryItem operator /(decimal literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esQueryItem operator /(esQueryItem item1, Guid literal)
        {
            return DivideOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esQueryItem operator /(Guid literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esQueryItem operator /(esQueryItem item1, short literal)
        {
            return DivideOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esQueryItem operator /(short literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esQueryItem operator /(esQueryItem item1, int literal)
        {
            return DivideOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esQueryItem operator /(int literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esQueryItem operator /(esQueryItem item1, long literal)
        {
            return DivideOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esQueryItem operator /(long literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.Object
        public static esQueryItem operator /(esQueryItem item1, object literal)
        {
            return DivideOperator(item1, literal, esSystemType.Object, true);
        }

        public static esQueryItem operator /(object literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Object, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esQueryItem operator /(esQueryItem item1, sbyte literal)
        {
            return DivideOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator /(sbyte literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esQueryItem operator /(esQueryItem item1, float literal)
        {
            return DivideOperator(item1, literal, esSystemType.Single, true);
        }

        public static esQueryItem operator /(float literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.String
        public static esQueryItem operator /(esQueryItem item1, string literal)
        {
            return DivideOperator(item1, literal, esSystemType.String, true);
        }

        public static esQueryItem operator /(string literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.String, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esQueryItem operator /(esQueryItem item1, ushort literal)
        {
            return DivideOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator /(ushort literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esQueryItem operator /(esQueryItem item1, uint literal)
        {
            return DivideOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator /(uint literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esQueryItem operator /(esQueryItem item1, ulong literal)
        {
            return DivideOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator /(ulong literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        public static esQueryItem operator %(esQueryItem item1, esQueryItem item2)
        {
            esQueryItem qi = new esQueryItem();
            qi.Expression.SelectItem1 = item1;
            qi.Expression.Operator = esArithmeticOperator.Modulo;
            qi.Expression.SelectItem2 = item2;
            return qi;
        }

        #region % operator literal overloads

        private static esQueryItem ModuloOperator(esQueryItem queryItem, object literal, esSystemType literalType, bool itemFirst)
        {
            esQueryItem qi = new esQueryItem();
            qi.Expression.SelectItem1 = queryItem;
            qi.Expression.Operator = esArithmeticOperator.Modulo;
            qi.Expression.Literal = literal;
            qi.Expression.LiteralType = literalType;
            qi.Expression.ItemFirst = itemFirst;
            return qi;
        }

        // esSystemType.Boolean
        public static esQueryItem operator %(esQueryItem item1, bool literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esQueryItem operator %(bool literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esQueryItem operator %(esQueryItem item1, byte literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esQueryItem operator %(byte literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esQueryItem operator %(esQueryItem item1, char literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Char, true);
        }

        public static esQueryItem operator %(char literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esQueryItem operator %(esQueryItem item1, DateTime literal)
        {
            return ModuloOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esQueryItem operator %(DateTime literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esQueryItem operator %(esQueryItem item1, double literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Double, true);
        }

        public static esQueryItem operator %(double literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esQueryItem operator %(esQueryItem item1, decimal literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esQueryItem operator %(decimal literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esQueryItem operator %(esQueryItem item1, Guid literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esQueryItem operator %(Guid literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esQueryItem operator %(esQueryItem item1, short literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esQueryItem operator %(short literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esQueryItem operator %(esQueryItem item1, int literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esQueryItem operator %(int literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esQueryItem operator %(esQueryItem item1, long literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esQueryItem operator %(long literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.Object
        public static esQueryItem operator %(esQueryItem item1, object literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Object, true);
        }

        public static esQueryItem operator %(object literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Object, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esQueryItem operator %(esQueryItem item1, sbyte literal)
        {
            return ModuloOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator %(sbyte literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esQueryItem operator %(esQueryItem item1, float literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Single, true);
        }

        public static esQueryItem operator %(float literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.String
        public static esQueryItem operator %(esQueryItem item1, string literal)
        {
            return ModuloOperator(item1, literal, esSystemType.String, true);
        }

        public static esQueryItem operator %(string literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.String, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esQueryItem operator %(esQueryItem item1, ushort literal)
        {
            return ModuloOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator %(ushort literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esQueryItem operator %(esQueryItem item1, uint literal)
        {
            return ModuloOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator %(uint literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esQueryItem operator %(esQueryItem item1, ulong literal)
        {
            return ModuloOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator %(ulong literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #endregion

        #endregion

        #region operators applied to other QueryItems (LiteralExpression) with Nullable overloads

        #region > operator literal overloads

        // esSystemType.Boolean
        public static esComparison operator >(esQueryItem item1, bool? literal)
        {
            return GreaterThan(item1, literal, esSystemType.Boolean, true);
        }

        public static esComparison operator >(bool? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esComparison operator >(esQueryItem item1, byte? literal)
        {
            return GreaterThan(item1, literal, esSystemType.Byte, true);
        }

        public static esComparison operator >(byte? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esComparison operator >(esQueryItem item1, char? literal)
        {
            return GreaterThan(item1, literal, esSystemType.Char, true);
        }

        public static esComparison operator >(char? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esComparison operator >(esQueryItem item1, DateTime? literal)
        {
            return GreaterThan(item1, literal, esSystemType.DateTime, true);
        }

        public static esComparison operator >(DateTime? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esComparison operator >(esQueryItem item1, double? literal)
        {
            return GreaterThan(item1, literal, esSystemType.Double, true);
        }

        public static esComparison operator >(double? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esComparison operator >(esQueryItem item1, decimal? literal)
        {
            return GreaterThan(item1, literal, esSystemType.Decimal, true);
        }

        public static esComparison operator >(decimal? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esComparison operator >(esQueryItem item1, Guid? literal)
        {
            return GreaterThan(item1, literal, esSystemType.Guid, true);
        }

        public static esComparison operator >(Guid? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esComparison operator >(esQueryItem item1, short? literal)
        {
            return GreaterThan(item1, literal, esSystemType.Int16, true);
        }

        public static esComparison operator >(short? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esComparison operator >(esQueryItem item1, int? literal)
        {
            return GreaterThan(item1, literal, esSystemType.Int32, true);
        }

        public static esComparison operator >(int? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esComparison operator >(esQueryItem item1, long? literal)
        {
            return GreaterThan(item1, literal, esSystemType.Int64, true);
        }

        public static esComparison operator >(long? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esComparison operator >(esQueryItem item1, sbyte? literal)
        {
            return GreaterThan(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >(sbyte? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esComparison operator >(esQueryItem item1, float? literal)
        {
            return GreaterThan(item1, literal, esSystemType.Single, true);
        }

        public static esComparison operator >(float? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esComparison operator >(esQueryItem item1, ushort? literal)
        {
            return GreaterThan(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >(ushort? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esComparison operator >(esQueryItem item1, uint? literal)
        {
            return GreaterThan(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >(uint? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esComparison operator >(esQueryItem item1, ulong? literal)
        {
            return GreaterThan(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >(ulong? literal, esQueryItem item1)
        {
            return GreaterThan(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region < operator literal overloads

        // esSystemType.Boolean
        public static esComparison operator <(esQueryItem item1, bool? literal)
        {
            return LessThan(item1, literal, esSystemType.Boolean, true);
        }

        public static esComparison operator <(bool? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esComparison operator <(esQueryItem item1, byte? literal)
        {
            return LessThan(item1, literal, esSystemType.Byte, true);
        }

        public static esComparison operator <(byte? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esComparison operator <(esQueryItem item1, char? literal)
        {
            return LessThan(item1, literal, esSystemType.Char, true);
        }

        public static esComparison operator <(char? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esComparison operator <(esQueryItem item1, DateTime? literal)
        {
            return LessThan(item1, literal, esSystemType.DateTime, true);
        }

        public static esComparison operator <(DateTime? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esComparison operator <(esQueryItem item1, double? literal)
        {
            return LessThan(item1, literal, esSystemType.Double, true);
        }

        public static esComparison operator <(double? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esComparison operator <(esQueryItem item1, decimal? literal)
        {
            return LessThan(item1, literal, esSystemType.Decimal, true);
        }

        public static esComparison operator <(decimal? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esComparison operator <(esQueryItem item1, Guid? literal)
        {
            return LessThan(item1, literal, esSystemType.Guid, true);
        }

        public static esComparison operator <(Guid? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esComparison operator <(esQueryItem item1, short? literal)
        {
            return LessThan(item1, literal, esSystemType.Int16, true);
        }

        public static esComparison operator <(short? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esComparison operator <(esQueryItem item1, int? literal)
        {
            return LessThan(item1, literal, esSystemType.Int32, true);
        }

        public static esComparison operator <(int? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esComparison operator <(esQueryItem item1, long? literal)
        {
            return LessThan(item1, literal, esSystemType.Int64, true);
        }

        public static esComparison operator <(long? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esComparison operator <(esQueryItem item1, sbyte? literal)
        {
            return LessThan(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <(sbyte? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esComparison operator <(esQueryItem item1, float? literal)
        {
            return LessThan(item1, literal, esSystemType.Single, true);
        }

        public static esComparison operator <(float? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esComparison operator <(esQueryItem item1, ushort? literal)
        {
            return LessThan(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <(ushort? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esComparison operator <(esQueryItem item1, uint? literal)
        {
            return LessThan(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <(uint? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esComparison operator <(esQueryItem item1, ulong? literal)
        {
            return LessThan(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <(ulong? literal, esQueryItem item1)
        {
            return LessThan(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region <= operator literal overloads

        // esSystemType.Boolean
        public static esComparison operator <=(esQueryItem item1, bool? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Boolean, true);
        }

        public static esComparison operator <=(bool? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esComparison operator <=(esQueryItem item1, byte? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Byte, true);
        }

        public static esComparison operator <=(byte? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esComparison operator <=(esQueryItem item1, char? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Char, true);
        }

        public static esComparison operator <=(char? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esComparison operator <=(esQueryItem item1, DateTime? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.DateTime, true);
        }

        public static esComparison operator <=(DateTime? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esComparison operator <=(esQueryItem item1, double? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Double, true);
        }

        public static esComparison operator <=(double? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esComparison operator <=(esQueryItem item1, decimal? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Decimal, true);
        }

        public static esComparison operator <=(decimal? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esComparison operator <=(esQueryItem item1, Guid? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Guid, true);
        }

        public static esComparison operator <=(Guid? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esComparison operator <=(esQueryItem item1, short? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Int16, true);
        }

        public static esComparison operator <=(short? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esComparison operator <=(esQueryItem item1, int? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Int32, true);
        }

        public static esComparison operator <=(int? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esComparison operator <=(esQueryItem item1, long? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Int64, true);
        }

        public static esComparison operator <=(long? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esComparison operator <=(esQueryItem item1, sbyte? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <=(sbyte? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esComparison operator <=(esQueryItem item1, float? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Single, true);
        }

        public static esComparison operator <=(float? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esComparison operator <=(esQueryItem item1, ushort? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <=(ushort? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esComparison operator <=(esQueryItem item1, uint? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <=(uint? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esComparison operator <=(esQueryItem item1, ulong? literal)
        {
            return LessThanOrEqual(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator <=(ulong? literal, esQueryItem item1)
        {
            return LessThanOrEqual(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region >= operator literal overloads

        // esSystemType.Boolean
        public static esComparison operator >=(esQueryItem item1, bool? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Boolean, true);
        }

        public static esComparison operator >=(bool? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esComparison operator >=(esQueryItem item1, byte? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Byte, true);
        }

        public static esComparison operator >=(byte? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esComparison operator >=(esQueryItem item1, char? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Char, true);
        }

        public static esComparison operator >=(char? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esComparison operator >=(esQueryItem item1, DateTime? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.DateTime, true);
        }

        public static esComparison operator >=(DateTime? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esComparison operator >=(esQueryItem item1, double? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Double, true);
        }

        public static esComparison operator >=(double? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esComparison operator >=(esQueryItem item1, decimal? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Decimal, true);
        }

        public static esComparison operator >=(decimal? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esComparison operator >=(esQueryItem item1, Guid? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Guid, true);
        }

        public static esComparison operator >=(Guid? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esComparison operator >=(esQueryItem item1, short? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Int16, true);
        }

        public static esComparison operator >=(short? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esComparison operator >=(esQueryItem item1, int? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Int32, true);
        }

        public static esComparison operator >=(int? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esComparison operator >=(esQueryItem item1, long? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Int64, true);
        }

        public static esComparison operator >=(long? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esComparison operator >=(esQueryItem item1, sbyte? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >=(sbyte? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esComparison operator >=(esQueryItem item1, float? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Single, true);
        }

        public static esComparison operator >=(float? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esComparison operator >=(esQueryItem item1, ushort? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >=(ushort? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esComparison operator >=(esQueryItem item1, uint? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >=(uint? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esComparison operator >=(esQueryItem item1, ulong? literal)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator >=(ulong? literal, esQueryItem item1)
        {
            return GreaterThanOrEqual(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region == operator literal overloads

        // esSystemType.Boolean
        public static esComparison operator ==(esQueryItem item1, bool? literal)
        {
            return EqualOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esComparison operator ==(bool? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esComparison operator ==(esQueryItem item1, byte? literal)
        {
            return EqualOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esComparison operator ==(byte? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esComparison operator ==(esQueryItem item1, char? literal)
        {
            return EqualOperator(item1, literal, esSystemType.Char, true);
        }

        public static esComparison operator ==(char? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esComparison operator ==(esQueryItem item1, DateTime? literal)
        {
            return EqualOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esComparison operator ==(DateTime? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esComparison operator ==(esQueryItem item1, double? literal)
        {
            return EqualOperator(item1, literal, esSystemType.Double, true);
        }

        public static esComparison operator ==(double? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esComparison operator ==(esQueryItem item1, decimal? literal)
        {
            return EqualOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esComparison operator ==(decimal? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esComparison operator ==(esQueryItem item1, Guid? literal)
        {
            return EqualOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esComparison operator ==(Guid? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esComparison operator ==(esQueryItem item1, short? literal)
        {
            return EqualOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esComparison operator ==(short? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esComparison operator ==(esQueryItem item1, int? literal)
        {
            return EqualOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esComparison operator ==(int? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esComparison operator ==(esQueryItem item1, long? literal)
        {
            return EqualOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esComparison operator ==(long? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esComparison operator ==(esQueryItem item1, sbyte? literal)
        {
            return EqualOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator ==(sbyte? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esComparison operator ==(esQueryItem item1, float? literal)
        {
            return EqualOperator(item1, literal, esSystemType.Single, true);
        }

        public static esComparison operator ==(float? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esComparison operator ==(esQueryItem item1, ushort? literal)
        {
            return EqualOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator ==(ushort? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esComparison operator ==(esQueryItem item1, uint? literal)
        {
            return EqualOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator ==(uint? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esComparison operator ==(esQueryItem item1, ulong? literal)
        {
            return EqualOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator ==(ulong? literal, esQueryItem item1)
        {
            return EqualOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region != operator literal overloads

        // esSystemType.Boolean
        public static esComparison operator !=(esQueryItem item1, bool? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esComparison operator !=(bool? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esComparison operator !=(esQueryItem item1, byte? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esComparison operator !=(byte? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esComparison operator !=(esQueryItem item1, char? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Char, true);
        }

        public static esComparison operator !=(char? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esComparison operator !=(esQueryItem item1, DateTime? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esComparison operator !=(DateTime? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esComparison operator !=(esQueryItem item1, double? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Double, true);
        }

        public static esComparison operator !=(double? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esComparison operator !=(esQueryItem item1, decimal? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esComparison operator !=(decimal? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esComparison operator !=(esQueryItem item1, Guid? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esComparison operator !=(Guid? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esComparison operator !=(esQueryItem item1, short? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esComparison operator !=(short? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esComparison operator !=(esQueryItem item1, int? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esComparison operator !=(int? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esComparison operator !=(esQueryItem item1, long? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esComparison operator !=(long? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esComparison operator !=(esQueryItem item1, sbyte? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator !=(sbyte? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esComparison operator !=(esQueryItem item1, float? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.Single, true);
        }

        public static esComparison operator !=(float? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esComparison operator !=(esQueryItem item1, ushort? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator !=(ushort? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esComparison operator !=(esQueryItem item1, uint? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator !=(uint? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esComparison operator !=(esQueryItem item1, ulong? literal)
        {
            return NotEqualOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esComparison operator !=(ulong? literal, esQueryItem item1)
        {
            return NotEqualOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region Arithmetic Expressions

        #region + operator literal overloads

        // esSystemType.Boolean
        public static esQueryItem operator +(esQueryItem item1, bool? literal)
        {
            return AddOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esQueryItem operator +(bool? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esQueryItem operator +(esQueryItem item1, byte? literal)
        {
            return AddOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esQueryItem operator +(byte? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esQueryItem operator +(esQueryItem item1, char? literal)
        {
            return AddOperator(item1, literal, esSystemType.Char, true);
        }

        public static esQueryItem operator +(char? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esQueryItem operator +(esQueryItem item1, DateTime? literal)
        {
            return AddOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esQueryItem operator +(DateTime? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esQueryItem operator +(esQueryItem item1, double? literal)
        {
            return AddOperator(item1, literal, esSystemType.Double, true);
        }

        public static esQueryItem operator +(double? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esQueryItem operator +(esQueryItem item1, decimal? literal)
        {
            return AddOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esQueryItem operator +(decimal? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esQueryItem operator +(esQueryItem item1, Guid? literal)
        {
            return AddOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esQueryItem operator +(Guid? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esQueryItem operator +(esQueryItem item1, short? literal)
        {
            return AddOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esQueryItem operator +(short? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esQueryItem operator +(esQueryItem item1, int? literal)
        {
            return AddOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esQueryItem operator +(int? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esQueryItem operator +(esQueryItem item1, long? literal)
        {
            return AddOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esQueryItem operator +(long? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esQueryItem operator +(esQueryItem item1, sbyte? literal)
        {
            return AddOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator +(sbyte? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esQueryItem operator +(esQueryItem item1, float? literal)
        {
            return AddOperator(item1, literal, esSystemType.Single, true);
        }

        public static esQueryItem operator +(float? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esQueryItem operator +(esQueryItem item1, ushort? literal)
        {
            return AddOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator +(ushort? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esQueryItem operator +(esQueryItem item1, uint? literal)
        {
            return AddOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator +(uint? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esQueryItem operator +(esQueryItem item1, ulong? literal)
        {
            return AddOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator +(ulong? literal, esQueryItem item1)
        {
            return AddOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region - operator literal overloads

        // esSystemType.Boolean
        public static esQueryItem operator -(esQueryItem item1, bool? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esQueryItem operator -(bool? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esQueryItem operator -(esQueryItem item1, byte? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esQueryItem operator -(byte? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esQueryItem operator -(esQueryItem item1, char? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Char, true);
        }

        public static esQueryItem operator -(char? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esQueryItem operator -(esQueryItem item1, DateTime? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esQueryItem operator -(DateTime? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esQueryItem operator -(esQueryItem item1, double? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Double, true);
        }

        public static esQueryItem operator -(double? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esQueryItem operator -(esQueryItem item1, decimal? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esQueryItem operator -(decimal? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esQueryItem operator -(esQueryItem item1, Guid? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esQueryItem operator -(Guid? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esQueryItem operator -(esQueryItem item1, short? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esQueryItem operator -(short? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esQueryItem operator -(esQueryItem item1, int? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esQueryItem operator -(int? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esQueryItem operator -(esQueryItem item1, long? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esQueryItem operator -(long? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esQueryItem operator -(esQueryItem item1, sbyte? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator -(sbyte? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esQueryItem operator -(esQueryItem item1, float? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.Single, true);
        }

        public static esQueryItem operator -(float? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esQueryItem operator -(esQueryItem item1, ushort? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator -(ushort? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esQueryItem operator -(esQueryItem item1, uint? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator -(uint? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esQueryItem operator -(esQueryItem item1, ulong? literal)
        {
            return SubtractOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator -(ulong? literal, esQueryItem item1)
        {
            return SubtractOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region * operator literal overloads

        // esSystemType.Boolean
        public static esQueryItem operator *(esQueryItem item1, bool? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esQueryItem operator *(bool? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esQueryItem operator *(esQueryItem item1, byte? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esQueryItem operator *(byte? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esQueryItem operator *(esQueryItem item1, char? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Char, true);
        }

        public static esQueryItem operator *(char? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esQueryItem operator *(esQueryItem item1, DateTime? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esQueryItem operator *(DateTime? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esQueryItem operator *(esQueryItem item1, double? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Double, true);
        }

        public static esQueryItem operator *(double? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esQueryItem operator *(esQueryItem item1, decimal? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esQueryItem operator *(decimal? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esQueryItem operator *(esQueryItem item1, Guid? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esQueryItem operator *(Guid? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esQueryItem operator *(esQueryItem item1, short? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esQueryItem operator *(short? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esQueryItem operator *(esQueryItem item1, int? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esQueryItem operator *(int? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esQueryItem operator *(esQueryItem item1, long? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esQueryItem operator *(long? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esQueryItem operator *(esQueryItem item1, sbyte? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator *(sbyte? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esQueryItem operator *(esQueryItem item1, float? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.Single, true);
        }

        public static esQueryItem operator *(float? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esQueryItem operator *(esQueryItem item1, ushort? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator *(ushort? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esQueryItem operator *(esQueryItem item1, uint? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator *(uint? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esQueryItem operator *(esQueryItem item1, ulong? literal)
        {
            return MultiplyOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator *(ulong? literal, esQueryItem item1)
        {
            return MultiplyOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region / operator literal overloads

        // esSystemType.Boolean
        public static esQueryItem operator /(esQueryItem item1, bool? literal)
        {
            return DivideOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esQueryItem operator /(bool? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esQueryItem operator /(esQueryItem item1, byte? literal)
        {
            return DivideOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esQueryItem operator /(byte? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esQueryItem operator /(esQueryItem item1, char? literal)
        {
            return DivideOperator(item1, literal, esSystemType.Char, true);
        }

        public static esQueryItem operator /(char? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esQueryItem operator /(esQueryItem item1, DateTime? literal)
        {
            return DivideOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esQueryItem operator /(DateTime? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esQueryItem operator /(esQueryItem item1, double? literal)
        {
            return DivideOperator(item1, literal, esSystemType.Double, true);
        }

        public static esQueryItem operator /(double? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esQueryItem operator /(esQueryItem item1, decimal? literal)
        {
            return DivideOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esQueryItem operator /(decimal? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esQueryItem operator /(esQueryItem item1, Guid? literal)
        {
            return DivideOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esQueryItem operator /(Guid? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esQueryItem operator /(esQueryItem item1, short? literal)
        {
            return DivideOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esQueryItem operator /(short? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esQueryItem operator /(esQueryItem item1, int? literal)
        {
            return DivideOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esQueryItem operator /(int? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esQueryItem operator /(esQueryItem item1, long? literal)
        {
            return DivideOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esQueryItem operator /(long? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esQueryItem operator /(esQueryItem item1, sbyte? literal)
        {
            return DivideOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator /(sbyte? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esQueryItem operator /(esQueryItem item1, float? literal)
        {
            return DivideOperator(item1, literal, esSystemType.Single, true);
        }

        public static esQueryItem operator /(float? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esQueryItem operator /(esQueryItem item1, ushort? literal)
        {
            return DivideOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator /(ushort? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esQueryItem operator /(esQueryItem item1, uint? literal)
        {
            return DivideOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator /(uint? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esQueryItem operator /(esQueryItem item1, ulong? literal)
        {
            return DivideOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator /(ulong? literal, esQueryItem item1)
        {
            return DivideOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #region % operator literal overloads

        // esSystemType.Boolean
        public static esQueryItem operator %(esQueryItem item1, bool? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Boolean, true);
        }

        public static esQueryItem operator %(bool? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Boolean, false);
        }

        // esSystemType.Byte
        public static esQueryItem operator %(esQueryItem item1, byte? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Byte, true);
        }

        public static esQueryItem operator %(byte? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Byte, false);
        }

        // esSystemType.Char
        public static esQueryItem operator %(esQueryItem item1, char? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Char, true);
        }

        public static esQueryItem operator %(char? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Char, false);
        }

        // esSystemType.DateTime
        public static esQueryItem operator %(esQueryItem item1, DateTime? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.DateTime, true);
        }

        public static esQueryItem operator %(DateTime? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.DateTime, false);
        }

        // esSystemType.Double
        public static esQueryItem operator %(esQueryItem item1, double? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Double, true);
        }

        public static esQueryItem operator %(double? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Double, false);
        }

        // esSystemType.Decimal
        public static esQueryItem operator %(esQueryItem item1, decimal? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Decimal, true);
        }

        public static esQueryItem operator %(decimal? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Decimal, false);
        }

        // esSystemType.Guid
        public static esQueryItem operator %(esQueryItem item1, Guid? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Guid, true);
        }

        public static esQueryItem operator %(Guid? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Guid, false);
        }

        // esSystemType.Int16
        public static esQueryItem operator %(esQueryItem item1, short? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Int16, true);
        }

        public static esQueryItem operator %(short? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Int16, false);
        }

        // esSystemType.Int32
        public static esQueryItem operator %(esQueryItem item1, int? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Int32, true);
        }

        public static esQueryItem operator %(int? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Int32, false);
        }

        // esSystemType.Int64
        public static esQueryItem operator %(esQueryItem item1, long? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Int64, true);
        }

        public static esQueryItem operator %(long? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Int64, false);
        }

        // esSystemType.SByte
        [CLSCompliant(false)]
        public static esQueryItem operator %(esQueryItem item1, sbyte? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.SByte, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator %(sbyte? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.SByte, false);
        }

        // esSystemType.Single
        public static esQueryItem operator %(esQueryItem item1, float? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.Single, true);
        }

        public static esQueryItem operator %(float? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.Single, false);
        }

        // esSystemType.UInt16
        [CLSCompliant(false)]
        public static esQueryItem operator %(esQueryItem item1, ushort? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.UInt16, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator %(ushort? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.UInt16, false);
        }

        // esSystemType.UInt32
        [CLSCompliant(false)]
        public static esQueryItem operator %(esQueryItem item1, uint? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.UInt32, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator %(uint? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.UInt32, false);
        }

        // esSystemType.UInt64
        [CLSCompliant(false)]
        public static esQueryItem operator %(esQueryItem item1, ulong? literal)
        {
            return ModuloOperator(item1, literal, esSystemType.UInt64, true);
        }

        [CLSCompliant(false)]
        public static esQueryItem operator %(ulong? literal, esQueryItem item1)
        {
            return ModuloOperator(item1, literal, esSystemType.UInt64, false);
        }
        #endregion

        #endregion

        #endregion

        #region Where Clause

        private esComparison CreateComparisonParameter(esComparisonOperand operand, object value)
        {
            esComparison comparison = null;

            esQueryItem qi = value as esQueryItem;
            if (Object.Equals(qi, null))
            {
                comparison = new esComparison(this.query);
                comparison.Operand = operand;

                if (this.HasExpression)
                {
                    comparison.data.Expression = this.Expression;
                }

                comparison.data.Column = this.Column;
                comparison.data.Value = value;
                comparison.SubOperators = this.SubOperators;
            }
            else
            {
                comparison = new esComparison(this.query);
                comparison.Operand = operand;
                comparison.data.Column = this.Column;
                comparison.data.ComparisonColumn = qi.Column;
                comparison.SubOperators = qi.SubOperators;
            }

            return comparison;
        }

        private esComparison CreateComparisonParameter(esComparisonOperand operand)
        {
            esComparison comparison = new esComparison(this.query);

            comparison.Operand = operand;

            if (this.HasExpression)
            {
                comparison.data.Expression = this.Expression;
            }

            comparison.data.Column = this.Column;
            comparison.SubOperators = this.SubOperators;

            return comparison;
        }

        /// <summary>
        /// Where parameter operand creation is called by DynamicQuery.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <param name="op">E.g., esComparisonOperand.IsNotNull</param>
        /// <returns>The esComparison returned to DynamicQuery</returns>
        public esComparison OP(esComparisonOperand op)
        {
            switch (op)
            {
                case esComparisonOperand.IsNotNull:
                case esComparisonOperand.IsNull:
                    return CreateComparisonParameter(op);

                default:
                    throw new InvalidOperationException("Operand requires at least one value");
            }
        }

        /// <summary>
        /// Where parameter operand creation is called by DynamicQuery.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <param name="op">E.g., esComparisonOperand.IsNotNull</param>
        /// <param name="value">The value for this comparison</param>
        /// <returns>The esComparison returned to DynamicQuery</returns>
        public esComparison OP(esComparisonOperand op, object value)
        {
            switch (op)
            {
                case esComparisonOperand.IsNull:
                case esComparisonOperand.IsNotNull:
                    return CreateComparisonParameter(op);

                case esComparisonOperand.Equal:
                case esComparisonOperand.NotEqual:
                case esComparisonOperand.GreaterThan:
                case esComparisonOperand.GreaterThanOrEqual:
                case esComparisonOperand.LessThan:
                case esComparisonOperand.LessThanOrEqual:
                case esComparisonOperand.Like:
                case esComparisonOperand.In:
                case esComparisonOperand.NotIn:
                case esComparisonOperand.NotLike:
                case esComparisonOperand.Contains:
                    return CreateComparisonParameter(op, value);

                case esComparisonOperand.Between:
                    throw new InvalidOperationException("Between requires two parameters");

                default:
                    throw new InvalidOperationException("Invalid Operand");
            }
        }

        /// <summary>
        /// Where parameter operand creation is called by DynamicQuery.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <param name="op">E.g., esComparisonOperand.Between</param>
        /// <param name="value1">The first value for this comparison</param>
        /// <param name="value2">The second value for this comparison</param>
        /// <returns>The esComparison returned to DynamicQuery</returns>
        public esComparison OP(esComparisonOperand op, object value1, object value2)
        {
            switch (op)
            {
                case esComparisonOperand.IsNull:
                case esComparisonOperand.IsNotNull:
                    return CreateComparisonParameter(op);

                case esComparisonOperand.Equal:
                case esComparisonOperand.NotEqual:
                case esComparisonOperand.GreaterThan:
                case esComparisonOperand.GreaterThanOrEqual:
                case esComparisonOperand.LessThan:
                case esComparisonOperand.LessThanOrEqual:
                case esComparisonOperand.In:
                case esComparisonOperand.NotIn:
                case esComparisonOperand.Contains:
                    return CreateComparisonParameter(op, value1);

                case esComparisonOperand.Like:
                    return this.Like(value1, Convert.ToChar(value2));

                case esComparisonOperand.NotLike:
                    return this.NotLike(value1, Convert.ToChar(value2));

                case esComparisonOperand.Between:
                    return this.Between(value1, value2);

                default:
                    throw new InvalidOperationException("Invalid Operand");
            }
        }

        /// <summary>
        /// Comparison ensuring that the value passed in is EQUAL to this column.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(Age.Equal(24))
        /// </code>
        /// or try the natural syntax
        /// <code>
        /// Where(Age == 24)
        /// </code>
        /// </example>
        /// <param name="value">The value for comparison.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        [Obsolete("For more readable code use '==' in C# or '=' in VB.NET rather than this method")]
        public esComparison Equal(object value)
        {
            return CreateComparisonParameter(esComparisonOperand.Equal, value);
        }

        /// <summary>
        /// Comparison ensuring that the value passed in is NOT EQUAL to this column.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(Age.NotEqual(24))
        /// </code>
        /// or try the natural syntax
        /// <code>
        /// Where(Age != 24)
        /// </code>
        /// </example>
        /// <param name="value">The value for comparison.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        [Obsolete("For more readable code use '!=' in C# or '<>' in VB.NET rather than this method")] 
        public esComparison NotEqual(object value)
        {
            return CreateComparisonParameter(esComparisonOperand.NotEqual, value);
        }

        /// <summary>
        /// Comparison ensuring that this column is GREATER THAN the value passed in.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(Age.GreaterThan(24))
        /// </code>
        /// or try the natural syntax
        /// <code>
        /// Where(Age &gt; 24)
        /// </code>
        /// </example>
        /// <param name="value">The value for comparison.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        [Obsolete("For more readable code use '>' rather than this method")]
        public esComparison GreaterThan(object value)
        {
            return CreateComparisonParameter(esComparisonOperand.GreaterThan, value);
        }

        /// <summary>
        /// Comparison ensuring that this column is GREATER THAN OR EQUAL
        /// to the value passed in.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(Age.GreaterThanOrEqual(24))
        /// </code>
        /// or try the natural syntax
        /// <code>
        /// Where(Age &gt;= 24)
        /// </code>
        /// </example>
        /// <param name="value">The value for comparison.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        [Obsolete("For more readable code use '>=' rather than this method")]
        public esComparison GreaterThanOrEqual(object value)
        {
            return CreateComparisonParameter(esComparisonOperand.GreaterThanOrEqual, value);
        }

        /// <summary>
        /// Comparison ensuring that this column is LESS THAN the value passed in.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(Age.LessThan(24))
        /// </code>
        /// or try the natural syntax
        /// <code>
        /// Where(Age &lt; 24)
        /// </code>
        /// </example>
        /// <param name="value">The value for comparison.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        [Obsolete("For more readable code use '<' rather than this method")]
        public esComparison LessThan(object value)
        {
            return CreateComparisonParameter(esComparisonOperand.LessThan, value);
        }

        /// <summary>
        /// Comparison ensuring that this column is LESS THAN OR EQUAL
        /// to the value passed in.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(Age.LessThanOrEqual(24))
        /// </code>
        /// or try the natural syntax
        /// <code>
        /// Where(Age &lt;= 24)
        /// </code>
        /// </example>
        /// <param name="value">The value for comparison.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        [Obsolete("For more readable code use '<=' rather than this method")]
        public esComparison LessThanOrEqual(object value)
        {
            return CreateComparisonParameter(esComparisonOperand.LessThanOrEqual, value);
        }

        /// <summary>
        /// Comparison ensuring that the value passed in is LIKE this column.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(LastName.Like("D%"))
        /// </code>
        /// </example>
        /// <param name="value">The value for comparison.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>        
        public esComparison Like(object value)
        {
            return CreateComparisonParameter(esComparisonOperand.Like, value);
        }

        /// <summary>
        /// Comparison ensuring that the value passed in is LIKE this column.
        /// This overload takes a single escape character.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// The optional escape character is used if the search phrase
        /// contains one of the LIKE wildcards. For example,
        /// the following will match "30%" anywhere in the column.
        /// <code>
        /// Where(Discount.Like("%30!//", '!'))
        /// </code>
        /// </example>
        /// <param name="value">The value for comparison.</param>
        /// <param name="escapeCharacter">The single escape character.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        public esComparison Like(object value, char escapeCharacter)
        {
            esComparison comparison = new esComparison(this.query);
            comparison.data.Column = this.Column;
            comparison.data.LikeEscape = escapeCharacter;
            comparison.Operand = esComparisonOperand.Like;
            comparison.SubOperators = this.SubOperators;

            esQueryItem qi = value as esQueryItem;
            if (Object.Equals(qi, null))
            {
                comparison.data.Value = value;
            }
            else
            {
                comparison.data.ComparisonColumn = qi.Column;
            }

            return comparison;
        }

        /// <summary>
        /// Comparison ensuring that the value passed in is NOT LIKE this column.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(LastName.NotLike("D%"))
        /// </code>
        /// </example>
        /// <param name="value">The value for comparison.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        public esComparison NotLike(object value)
        {
            return CreateComparisonParameter(esComparisonOperand.NotLike, value);
        }

        /// <summary>
        /// Comparison ensuring that the value passed in is NOT LIKE this column.
        /// This overload takes a single escape character.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// The optional escape character is used if the search phrase
        /// contains one of the LIKE wildcards. For example,
        /// the following will exclude columns with "30%" anywhere in them.
        /// <code>
        /// Where(Discount.NotLike("%30!//", '!'))
        /// </code>
        /// </example>
        /// <param name="value">The value for comparison.</param>
        /// <param name="escapeCharacter">The single escape character.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        public esComparison NotLike(object value, char escapeCharacter)
        {
            esComparison comparison = new esComparison(this.query);
            comparison.data.Column = this.Column;
            comparison.data.LikeEscape = escapeCharacter;
            comparison.Operand = esComparisonOperand.NotLike;
            comparison.SubOperators = this.SubOperators;

            esQueryItem qi = value as esQueryItem;
            if (Object.Equals(qi, null))
            {
                comparison.data.Value = value;
            }
            else
            {
                comparison.data.ComparisonColumn = qi.Column;
            }

            return comparison;
        }

        /// <summary>
        /// Contains is used to search columns containing
        /// character-based data types for precise or
        /// fuzzy (less precise) matches to single words and phrases.
        /// The column must have a Full Text index.
        /// </summary>
        /// <remarks>
        /// See CONTAINS in the SQL server MSDN docs
        /// for a full explanation of supported search terms
        /// and how to enable Full Text searching at the
        /// database, table, and column levels.
        /// </remarks>
        /// <example>
        /// The company name contains both "Acme" and
        /// "Company" in close approximation, AND
        /// the address contains "Road", but no form of
        /// "St", "Street", "Ave", or "Avenue".
        /// <code>
        /// string nameTerm =
        ///     "Acme NEAR Company";
        /// string addressTerm =
        ///     "Road AND NOT (\"St*\" OR \"Ave*\")";
        /// 
        /// cust.Query.Where(
        ///     cust.Query.CompanyName.Contains(nameTerm));
        /// cust.Query.Where(
        ///     cust.Query.Address.Contains(addressTerm));
        /// cust.Query.Load();
        /// </code>
        /// </example>
        /// <param name="value">The value for comparison.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        public esComparison Contains(object value)
        {
            return CreateComparisonParameter(esComparisonOperand.Contains, value);
        }

        /// <summary>
        /// Comparison ensuring that this column is NULL.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(LastName.IsNull())
        /// </code>
        /// </example>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        public esComparison IsNull()
        {
            return CreateComparisonParameter(esComparisonOperand.IsNull);
        }

        /// <summary>
        /// Comparison ensuring that this column is NOT NULL.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(LastName.IsNotNull())
        /// </code>
        /// </example>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        public esComparison IsNotNull()
        {
            return CreateComparisonParameter(esComparisonOperand.IsNotNull);
        }

        /// <summary>
        /// Comparison ensuring that this column is BETWEEN two values.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(BirthDate.Between("2000-01-01", "2000-12-31"))
        /// </code>
        /// </example>
        /// <param name="start">The starting value for comparison.</param>
        /// <param name="end">The ending value for comparison.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        public esComparison Between(object start, object end)
        {
            esComparison comparison = new esComparison(this.query);
            comparison.Operand = esComparisonOperand.Between;
            comparison.SubOperators = this.SubOperators;

            comparison.data.Column = this.Column;

            esQueryItem qi = start as esQueryItem;
            if (Object.Equals(qi, null))
            {
                comparison.BetweenBegin = start;
            }
            else
            {
                comparison.data.ComparisonColumn = qi.Column;
            }

            qi = end as esQueryItem;
            if (Object.Equals(qi, null))
            {
                comparison.BetweenEnd = end;
            }
            else
            {
                comparison.data.ComparisonColumn2 = qi.Column;
            }

            comparison.SubOperators = this.SubOperators;

            return comparison;
        }

        /// <summary>
        /// Comparison ensuring that this column is IN a list of values.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(LastName.In("Doe", "Smith", "Johnson"))
        /// </code>
        /// </example>
        /// <param name="value">The list of values for comparison.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        public esComparison In(params object[] value)
        {
            List<object> values = new List<object>();

            #region Convert object[] into a List<object>

            object[] oValues = (object[])value;

            foreach (object o in oValues)
            {
                string str = o as string;
                if (str != null)
                {
                    // String supports IEnumerable and we don't want to break
                    // up each individual character
                    values.Add(o);
                }
                else
                {
                    IEnumerable enumer = o as IEnumerable;
                    if (enumer != null)
                    {
                        foreach (object oo in enumer)
                        {
                            values.Add(oo);
                        }
                    }
                    else
                    {
                        values.Add(o);
                    }
                }
            }
            #endregion

            esComparison comparison = new esComparison(this.query);
            comparison.Operand = esComparisonOperand.In;
            comparison.data.Column = this.Column;
            comparison.SubOperators = this.SubOperators;
            comparison.Values = values;
            return comparison;
        }


        /// <summary>
        /// Comparison ensuring that this column is IN a list of values.
        /// </summary>
        /// <param name="value">The Query to provide the matching values for the IN comparison</param>
        /// <returns></returns>
        public esComparison In(esDynamicQuerySerializable subQuery)
        {
            esComparison comparison = new esComparison(this.query);
            comparison.Operand = esComparisonOperand.In;
            comparison.data.Column = this.Column;
            comparison.SubOperators = this.SubOperators;
            comparison.Value = subQuery;

            this.query.AddQueryToList(subQuery);

            return comparison;
        }

        /// <summary>
        /// Comparison ensuring that this column is NOT IN a list of values.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// Where(LastName.NotIn("Doe", "Smith", "Johnson"))
        /// </code>
        /// </example>
        /// <param name="value">The list of values for comparison.</param>
        /// <returns>The esComparison returned to DynamicQuery.</returns>
        public esComparison NotIn(params object[] value)
        {
            List<object> values = new List<object>();

            #region Convert object[] into a List<object>

            object[] oValues = (object[])value;

            foreach (object o in oValues)
            {
                string str = o as string;
                if (str != null)
                {
                    // String supports IEnumerable and we don't want to break
                    // up each individual character
                    values.Add(o);
                }
                else
                {
                    IEnumerable enumer = o as IEnumerable;
                    if (enumer != null)
                    {
                        foreach (object oo in enumer)
                        {
                            values.Add(oo);
                        }
                    }
                    else
                    {
                        values.Add(o);
                    }
                }
            }
            #endregion

            esComparison comparison = new esComparison(this.query);
            comparison.Operand = esComparisonOperand.NotIn;
            comparison.data.Column = this.Column;
            comparison.SubOperators = this.SubOperators;
            comparison.Values = values;
            return comparison;
        }

        /// <summary>
        /// Comparison ensuring that this column is NOT IN a list of values.
        /// </summary>
        /// <param name="value">The Query to provide the matching values for the NOT IN comparison</param>
        /// <returns></returns>
        public esComparison NotIn(esDynamicQuerySerializable subQuery)
        {
            esComparison comparison = new esComparison(this.query);
            comparison.Operand = esComparisonOperand.NotIn;
            comparison.data.Column = this.Column;
            comparison.SubOperators = this.SubOperators;
            comparison.Value = subQuery;

            this.query.AddQueryToList(subQuery);

            return comparison;
        }
        #endregion

        //public esCase Case(esQueryItem column)
        //{
        //    this.CaseWhen = new esCase(this.query, this, column);
        //    return this.CaseWhen;
        //}

        public esCase Case()
        {
            this.CaseWhen = new esCase(this.query, this);
            return this.CaseWhen;
        }

        #region OrderBy

        /// <summary>
        /// Sort in descending order.
        /// See <see cref="esOrderByDirection"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// emps.Query.OrderBy(emps.Query.LastName.Descending);
        /// </code>
        /// </example>
        /// <returns>The esOrderByItem returned to DynamicQuery.</returns>
        public esOrderByItem Descending
        {
            get 
            {
                esOrderByItem item = new esOrderByItem();
                item.Direction = esOrderByDirection.Descending;
                item.Expression = this;
                item.Expression.Query = this.query;
                return item;
            }
        }

        /// <summary>
        /// Sort in ascending order.
        /// See <see cref="esOrderByDirection"/> Enumeration.
        /// </summary>
        /// <example>
        /// <code>
        /// emps.Query.OrderBy(emps.Query.LastName.Ascending);
        /// </code>
        /// </example>
        /// <returns>The esOrderByItem returned to DynamicQuery.</returns>
        public esOrderByItem Ascending
        {
            get
            {
                esOrderByItem item = new esOrderByItem();
                item.Direction = esOrderByDirection.Ascending;
                item.Expression = this;
                item.Expression.Query = this.query;
                return item;
            }
        }

        #endregion

        #region Cast

        /// <summary>
        /// Cast informs the DataProviders that a SQL CAST operation is needed.
        /// </summary>
        /// <remarks>
        /// In C# you can cast with the overloaded cast operators, like this: (esString)query.Age
        /// </remarks>
        /// <param name="castType">The type of cast needed</param>
        /// <returns>The very same esQueryItem now with Cast instructions</returns>
        public esQueryItem Cast(esCastType castType)
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Cast;
            subOp.Parameters["esCastType"] = castType;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Cast informs the DataProviders that a SQL CAST operation is needed. This overloaded version
        /// of Cast is useful for Casting variable length character columns
        /// </summary>
        /// <remarks>
        /// In C# you can cast with the overloaded cast operators, like this: (esString)query.Age
        /// </remarks>
        /// <param name="castType">The type of cast needed</param>
        /// <returns>The very same esQueryItem now with Cast instructions</returns>
        public esQueryItem Cast(esCastType castType, int length)
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Cast;
            subOp.Parameters["esCastType"] = castType;
            subOp.Parameters["length"] = length;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Cast informs the DataProviders that a SQL CAST operation is needed. This overloaded version
        /// of Cast is useful for Casting decimal types
        /// </summary>
        /// <remarks>
        /// In C# you can cast with the overloaded cast operators, like this: (esString)query.Age
        /// </remarks>
        /// <param name="castType">The type of cast needed</param>
        /// <returns>The very same esQueryItem now with Cast instructions</returns>
        public esQueryItem Cast(esCastType castType, int precision, int scale)
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Cast;
            subOp.Parameters["esCastType"] = castType;
            subOp.Parameters["precision"] = precision;
            subOp.Parameters["scale"] = scale;
            this.AddSubOperator(subOp);

            return this;
        }

        #endregion

        /// <summary>
        /// Privides the Ability to Alias a column name
        /// </summary>
        /// <param name="alias">The Alias Name</param>
        /// <returns>esQueryItem</returns>
        public esQueryItem As(string alias)
        {
            this.Column.Alias = alias;
            return this;
        }

        #region Sub Operators

        /// <summary>
        /// Returns the column in UPPER CASE
        /// </summary>
         public esQueryItem ToUpper()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.ToUpper;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Returns the column in LOWER CASE
        /// </summary>
        public esQueryItem ToLower()
        {    
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.ToLower;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Performs a Left Trim (remove blanks) on the column
        /// </summary>
        public esQueryItem LTrim()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.LTrim;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Performs a Right Trim (remove blanks) on the column
        /// </summary>
        public  esQueryItem RTrim()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.RTrim;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Removes blanks from the beginning and end of the column
        /// </summary>
        public esQueryItem Trim()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Trim;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Returns a portion of the string column
        /// </summary>
        /// <param name="start">The starting character</param>
        /// <param name="length">How many characters to return</param>
        public esQueryItem Substring(System.Int64 start, System.Int64 length)
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.SubString;
            subOp.Parameters["start"] = start;
            subOp.Parameters["length"] = length;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Returns a portion of the string column
        /// </summary>
        /// <param name="length">How many characters to return</param>
        public esQueryItem Substring(System.Int64 length)
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.SubString;
            subOp.Parameters["length"] = length;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Returns a portion of the string column
        /// </summary>
        /// <param name="start">The starting character</param>
        /// <param name="length">How many characters to return</param>
        public esQueryItem Substring(int start, int length)
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.SubString;
            subOp.Parameters["start"] = start;
            subOp.Parameters["length"] = length;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Returns a portion of the string column
        /// </summary>
        /// <param name="length">How many characters to return</param>
        public esQueryItem Substring(int length)
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.SubString;
            subOp.Parameters["length"] = length;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// This can be used to return the first non null parameter.
        /// </summary>
        /// <remarks>
        /// The code below will return "Smith" is the LastName column in the database is null.
        /// <code>
        /// MyCollection coll = new MyCollection();
        /// coll.Query.Select(coll.Query.LastName.Coalesce("'Smith'"));
        /// coll.Query.Load();
        /// </code>
        /// </remarks>
        /// <param name="expresssions">The value to return if null</param>
        public esQueryItem Coalesce(string expresssions)
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Coalesce;
            subOp.Parameters["expressions"] = expresssions;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Returns the Date only portion of a date columm
        /// </summary>
        /// <returns></returns>
        public esQueryItem Date()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Date;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Returns the length of a character based column
        /// </summary>
        public esQueryItem Length()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Length;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Performs a round on the column
        /// </summary>
        /// <param name="significantDigits">Round to the number of significant digits</param>
        public esQueryItem Round(int significantDigits)
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Round;
            subOp.Parameters["SignificantDigits"] = significantDigits;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Returns a particular date part of a date column
        /// </summary>
        /// <param name="datePart"></param>
        public esQueryItem DatePart(string datePart)
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.DatePart;
            subOp.Parameters["DatePart"] = datePart;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Aggregate Sum.
        /// See <see cref="esQuerySubOperatorType"/> Enumeration.
        /// </summary>
        /// <example>
        /// Aggregate Sum with the column name as the default Alias.
        /// <code>
        /// emps.Query.Select(emps.Query.Age.Sum());
        /// </code>
        /// </example>
        /// <returns>The esAggregateItem returned to DynamicQuery.</returns>
        public esQueryItem Sum()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Sum;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Aggregate Avg.
        /// See <see cref="esQuerySubOperatorType"/> Enumeration.
        /// </summary>
        /// <example>
        /// Aggregate Avg with the column name as the default Alias.
        /// <code>
        /// emps.Query.Select(emps.Query.Age.Avg());
        /// </code>
        /// </example>
        /// <returns>The esAggregateItem returned to DynamicQuery.</returns>
        public esQueryItem Avg()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Avg;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Aggregate Max.
        /// See <see cref="esQuerySubOperatorType"/> Enumeration.
        /// </summary>
        /// <example>
        /// Aggregate Max with the column name as the default Alias.
        /// <code>
        /// emps.Query.Select(emps.Query.Age.Max());
        /// </code>
        /// </example>
        /// <returns>The esAggregateItem returned to DynamicQuery.</returns>
        public esQueryItem Max()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Max;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Aggregate Min.
        /// See <see cref="esQuerySubOperatorType"/> Enumeration.
        /// </summary>
        /// <example>
        /// Aggregate Min with the column name as the default Alias.
        /// <code>
        /// emps.Query.Select(emps.Query.Age.Min());
        /// </code>
        /// </example>
        /// <returns>The esAggregateItem returned to DynamicQuery.</returns>
        public esQueryItem Min()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Min;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Aggregate StdDev.
        /// See <see cref="esQuerySubOperatorType"/> Enumeration.
        /// </summary>
        /// <example>
        /// Aggregate StdDev with the column name as the default Alias.
        /// <code>
        /// emps.Query.Select(emps.Query.Age.StdDev());
        /// </code>
        /// </example>
        /// <returns>The esAggregateItem returned to DynamicQuery.</returns>
        public esQueryItem StdDev()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.StdDev;
            this.AddSubOperator(subOp);

            return this;
        }

        /// <summary>
        /// Aggregate Var.
        /// See <see cref="esQuerySubOperatorType"/> Enumeration.
        /// </summary>
        /// <example>
        /// Aggregate Var with the column name as the default Alias.
        /// <code>
        /// emps.Query.Select(emps.Query.Age.Var());
        /// </code>
        /// </example>
        /// <returns>The esAggregateItem returned to DynamicQuery.</returns>
        public esQueryItem Var()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Var;
            this.AddSubOperator(subOp);
           
            return this;
        }

        /// <summary>
        /// Aggregate Count.
        /// See <see cref="esQuerySubOperatorType"/> Enumeration.
        /// </summary>
        /// <example>
        /// Aggregate Count with the column name as the default Alias.
        /// <code>
        /// emps.Query.Select(emps.Query.Age.Count());
        /// </code>
        /// </example>
        /// <returns>The esAggregateItem returned to DynamicQuery.</returns>
        public esQueryItem Count()
        {
            esQuerySubOperator subOp = new esQuerySubOperator();
            subOp.SubOperator = esQuerySubOperatorType.Count;
            this.AddSubOperator(subOp);

            return this;
        }

        #endregion

        /// <summary>
        /// Set to true for (DISTINCT columnName).
        /// </summary>
        public esQueryItem Distinct()
        {
            this.Column.Distinct = true;
            return this;
        }

        /// <summary>
        /// Required due to operator overloading. Use 'Equal' not 'Equals'
        /// for DynamicQueries.
        /// </summary>
#if !SILVERLIGHT    
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public override bool Equals(object o)
        {
            throw new NotImplementedException("Use 'Equal' not 'Equals'");
        }

        /// <summary>
        /// Required due to operator overloading.
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// ToString() (to use in GroupBy/OrderBy and such ....)
        /// </summary>
        public override string ToString()
        {
            return this.Column.Name;
        }

        #region Cast Operators

        public static implicit operator esBoolean(esQueryItem item)
        {
            return new esBoolean(item);
        }

        public static implicit operator esByte(esQueryItem item)
        {
            return new esByte(item);
        }

        public static implicit operator esChar(esQueryItem item)
        {
            return new esChar(item);
        }

        public static implicit operator esDateTime(esQueryItem item)
        {
            return new esDateTime(item);
        }

        public static implicit operator esDouble(esQueryItem item)
        {
            return new esDouble(item);
        }

        public static implicit operator esDecimal(esQueryItem item)
        {
            return new esDecimal(item);
        }

        public static implicit operator esGuid(esQueryItem item)
        {
            return new esGuid(item);
        }

        public static implicit operator esInt16(esQueryItem item)
        {
            return new esInt16(item);
        }

        public static implicit operator esInt32(esQueryItem item)
        {
            return new esInt32(item);
        }

        public static implicit operator esInt64(esQueryItem item)
        {
            return new esInt64(item);
        }

        public static implicit operator esSingle(esQueryItem item)
        {
            return new esSingle(item);
        }

        public static implicit operator esString(esQueryItem item)
        {
            return new esString(item);
        }

        #endregion

        /// <summary>
        /// ToString() (to use in GroupBy/OrderBy and such ....)
        /// </summary>
        public static implicit operator string(esQueryItem item)
        {
            return item.Column.Name;
        }

        /// <summary>
        /// ToString() (to use in GroupBy/OrderBy and such ....)
        /// </summary>
        public static implicit operator esColumnItem(esQueryItem item)
        {
            return item.Column;
        }

        /// <summary>
        /// ToString() (to use in GroupBy/OrderBy and such ....)
        /// </summary>
        public static implicit operator esExpression(esQueryItem item)
        {
            esExpression sItem = new esExpression();
            sItem.Column = item.Column;
            sItem.CaseWhen = item.CaseWhen;
            sItem.SubOperators = item.SubOperators;
            sItem.MathmaticalExpression = item.Expression;
            return sItem;
        }

        private void AddSubOperator(esQuerySubOperator subOperator)
        {
            if (this.SubOperators == null)
            {
                this.SubOperators = new List<esQuerySubOperator>();
            }

            this.SubOperators.Add(subOperator);
        }

        //-----------------------------
        // Data
        //-----------------------------
        internal esColumnItem Column;
        internal List<esQuerySubOperator> SubOperators;
        internal esCase CaseWhen;
        private esDynamicQuerySerializable query;

        /// <summary>
        /// Used Internally by EntitySpaces
        /// </summary>
        public bool HasExpression
        {
            get
            {
                return this.Expression != null;
            }
        }

        internal esMathmaticalExpression Expression;
    }
}
