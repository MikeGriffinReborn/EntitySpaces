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
using System.Collections.Generic;

#if (WCF)
using System.Runtime.Serialization;
#endif

namespace EntitySpaces.DynamicQuery
{
    /// <summary>
    /// The esComparison class is dynamically created by your BusinessEntity's
    /// DynamicQuery mechanism.
    /// This class is mostly used by the EntitySpaces architecture, not the programmer.
    /// </summary>
    /// <example>
    /// You will not call esComparison directly, but will be limited to use as
    /// in the example below, or to the many uses posted here:
    /// <code>
    /// http://www.entityspaces.net/portal/QueryAPISamples/tabid/80/Default.aspx
    /// </code>
    /// This will be the extent of your use of the esComparison class:
    /// <code>
    /// .Where
    /// (
    ///		emps.Query.LastName.Like("D%"),
    ///		emps.Query.Age == 30
    /// );
    /// </code>
    /// </example>
#if !SILVERLIGHT    
    [Serializable]
#endif
#if (WCF)
    [DataContract(Namespace = "es", IsReference = true)]
#endif
    public class esComparison
    {
        /// <summary>
        /// The esComparison class is dynamically created by your
        /// BusinessEntity's DynamicQuery mechanism.
        /// </summary>
        public esComparison(esDynamicQuerySerializable query) 
        {
            this.data.Query = query;
        }

        /// <summary>
        /// The esComparison class is dynamically created by your
        /// BusinessEntity's DynamicQuery mechanism.
        /// See <see cref="esParenthesis"/> Enumeration.
        /// </summary>
        /// <param name="paren">The esParenthesis passed in via DynamicQuery</param>
        public esComparison(esParenthesis paren) 
        {
            this.data.Parenthesis = paren;
        }

        /// <summary>
        /// The esComparison class is dynamically created by your
        /// BusinessEntity's DynamicQuery mechanism.
        /// See <see cref="esConjunction"/> Enumeration.
        /// </summary>
        /// <param name="conj">The esConjunction passed in via DynamicQuery</param>
        public esComparison(esConjunction conj)
        {
            this.data.Conjunction = conj;
        }

        /// <summary>
        /// Or | (to use in Where clauses).
        /// </summary>
        /// <example>
        /// The operators provide an alternative, natural syntax for DynamicQueries.
        /// <code>
        ///	emps.Query.Where
        /// (
        ///		emps.Query.LastName == "Doe" |
        ///		emps.Query.LastName == "Smith"
        /// );
        /// </code>
        /// </example>
        /// <param name="c1">First esComparison passed in via DynamicQuery</param>
        /// <param name="c2">Second esComparison passed in via DynamicQuery</param>
        /// <returns>The esComparison returned to DynamicQuery</returns>
        public static esComparison operator |(esComparison c1, esComparison c2)
        {
            return HandleOperator(c1, c2, esConjunction.Or);
        }

        /// <summary>
        /// And &amp; (to use in Where clauses).
        /// </summary>
        /// <example>
        /// The operators provide an alternative, natural syntax for DynamicQueries.
        /// <code>
        ///	emps.Query.Where
        /// (
        ///		emps.Query.LastName == "Doe" &amp;
        ///		emps.Query.FirstName == "Jane"
        /// );
        /// </code>
        /// </example>
        /// <param name="c1">First esComparison passed in via DynamicQuery</param>
        /// <param name="c2">Second esComparison passed in via DynamicQuery</param>
        /// <returns>The esComparison returned to DynamicQuery</returns>
        public static esComparison operator &(esComparison c1, esComparison c2)
        {
            return HandleOperator(c1, c2, esConjunction.And);
        }

        /// <summary>
        /// NOT ! (to use in Where clauses).
        /// </summary>
        /// <example>
        /// The operators provide an alternative, natural syntax for DynamicQueries.
        /// <code>
        ///	emps.Query.Where
        /// (
        ///		emps.Query.LastName == "Doe" &amp;
        ///		emps.Query.FirstName == "Jane"
        /// );
        /// </code>
        /// </example>
        /// <param name="c1">The esComparison to negate</param>
        /// <returns>The esComparison returned to DynamicQuery</returns>
        public static esComparison operator !(esComparison comparison)
        {
            comparison.not = true;
            return comparison;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        private static esComparison HandleOperator(esComparison c1, esComparison c2, esConjunction op)
        {
            List<esComparison> exp = null;

            if (c1.data.WhereExpression == null)
            {
                c1.data.WhereExpression = new List<esComparison>();
                exp = c1.data.WhereExpression;

                exp.Add(new esComparison(esParenthesis.Open));
                exp.Add(c1);
            }
            else
            {
                exp = c1.data.WhereExpression;
                exp.Insert(0, new esComparison(esParenthesis.Open));
            }

            esConjunction conj = op;

            if (c2.not)
            {
                switch (op)
                {
                    case esConjunction.And:
                        conj = esConjunction.AndNot;
                        break;

                    case esConjunction.Or:
                        conj = esConjunction.OrNot;
                        break;
                }
            }

            exp.Add(new esComparison(conj));

            if (c2.data.WhereExpression == null)
            {
                exp.Add(c2);
            }
            else
            {
                exp.AddRange(c2.data.WhereExpression);
            }

            exp.Add(new esComparison(esParenthesis.Close));

            return c1;
        }

        /// <summary>
        /// Force true (to use in Where clauses).
        /// </summary>
        public static bool operator true(esComparison c1)
        {
            return false;
        }

        ///// <summary>
        ///// Force false (to use in Where clauses).
        ///// </summary>
        public static bool operator false(esComparison c1)
        {
            return false;
        }


        /// <summary>
        /// string ColumnName.
        /// </summary>
        internal string ColumnName
        {
            get { return data.Column.Name; }
            set { data.Column.Name = value; }
        }

        /// <summary>
        /// bool IsLiteral.
        /// </summary>
        internal bool IsLiteral
        {
            get { return data.IsLiteral; }
            set { data.IsLiteral = value; }
        }

        /// <summary>
        /// object value.
        /// </summary>
        internal object Value
        {
            get { return data.Value; }
            set { data.Value = value; }
        }

        /// <summary>
        /// Used for In() and NotIn()
        /// </summary>
        internal List<object> Values
        {
            get { return data.Values; }
            set { data.Values = value; }
        }

        /// <summary>
        /// Used whenever a value is needed on the right hand side of an operator in a Where clause.
        /// </summary>
        internal string ComparisonColumn
        {
            get { return data.ComparisonColumn.Name; }
            set { data.ComparisonColumn.Name = value; }
        }


        /// <summary>
        /// Used only when <see cref="esComparisonOperand.Between"/> and the 2nd date is another column in the database.
        /// </summary>
        internal string ComparisonColumn2
        {
            get { return data.ComparisonColumn2.Name; }
            set { data.ComparisonColumn2.Name = value; }
        }

        /// <summary>
        /// esComparisonOperand Operand.
        /// See <see cref="esComparisonOperand"/> Enumeration.
        /// </summary>
        internal esComparisonOperand Operand
        {
            get { return data.Operand; }
            set { data.Operand = value; }
        }

        /// <summary>
        /// esQuerySubOperator
        /// See <see cref="esQuerySubOperator"/> Enumeration.
        /// </summary>
        internal List<esQuerySubOperator> SubOperators
        {
            get { return data.SubOperators; }
            set { data.SubOperators = value; }
        }

        /// <summary>
        /// esConjunction Conjunction.
        /// See <see cref="esConjunction"/> Enumeration.
        /// </summary>
        internal esConjunction Conjunction
        {
            get { return data.Conjunction; }
            set { data.Conjunction = value; }
        }

        /// <summary>
        /// esParenthesis Parenthesis.
        /// See <see cref="esParenthesis"/> Enumeration.
        /// </summary>
        internal esParenthesis Parenthesis
        {
            get { return data.Parenthesis; }
            set { data.Parenthesis = value; }
        }

        /// <summary>
        /// The first date when <see cref="esComparisonOperand.Between"/> is used and the value is 
        /// not another column in the table but a literal value being passed in.
        /// </summary>
        internal object BetweenBegin
        {
            get { return data.BetweenBegin; }
            set { data.BetweenBegin = value; }
        }

        /// <summary>
        /// The second date when <see cref="esComparisonOperand.Between"/> is used and the value is 
        /// not another column in the table but a literal value being passed in.
        /// </summary>
        internal object BetweenEnd
        {
            get { return data.BetweenEnd; }
            set { data.BetweenEnd = value; }
        }

        /// <summary>
        /// char LikeEscape.
        /// </summary>
        internal char LikeEscape
        {
            get { return data.LikeEscape; }
            set { data.LikeEscape = value; }
        }

        /// <summary>
        /// Whether the esComparison goes first in the expression
        /// </summary>
        internal bool ItemFirst
        {
            get { return data.ItemFirst; }
            set { data.ItemFirst = value; }
        }

        /// <summary>
        /// Used internally by EntitySpaces to make the <see cref="esComparison"/> classes data available to the
        /// EntitySpaces data providers.
        /// </summary>
#if !SILVERLIGHT    
        [Serializable]
#endif
#if (WCF)
        [DataContract(Namespace = "es", IsReference = true)]
#endif
        public class esComparisonData
        {
            /// <summary>
            /// bool IsConjunction.
            /// </summary>
            public bool IsConjunction
            {
                get
                {
                    return (this.Conjunction == esConjunction.Unassigned) ? false : true;
                }
            }

            /// <summary>
            /// bool IsParenthesis.
            /// </summary>
            public bool IsParenthesis
            {
                get
                {
                    return (this.Parenthesis == esParenthesis.Unassigned) ? false : true;
                }
            }

            /// <summary>
            /// bool HasComparisonColumn.
            /// </summary>
            public bool HasComparisonColumn
            {
                get
                {
                    return (this.ComparisonColumn.Name != null || this.ComparisonColumn2.Name != null) ? true : false;
                }
            }

            /// <summary>
            /// bool HasExpression.
            /// </summary>
            public bool HasExpression
            {
                get
                {
                    return (this.Expression != null) ? true : false;
                }
            }

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "ParentQuery", Order = 99, EmitDefaultValue = false)]
#endif            
            public esDynamicQuerySerializable Query;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "Column", EmitDefaultValue = false)]
#endif             
            public esColumnItem Column;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "IsLiteral", EmitDefaultValue = false)]
#endif
            public bool IsLiteral;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "ComparisonColumn", EmitDefaultValue = false)]
#endif              
            public esColumnItem ComparisonColumn;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "ComparisonColumn2", EmitDefaultValue = false)]
#endif            
            public esColumnItem ComparisonColumn2;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "Expression", EmitDefaultValue = false)]
#endif             
            public esMathmaticalExpression Expression;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "Value", EmitDefaultValue = false)]
#endif             
            public object Value;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "Values", EmitDefaultValue = false)]
#endif
            public List<object> Values;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "Operand", EmitDefaultValue = false)]
#endif             
            public esComparisonOperand Operand;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "Conjunction", EmitDefaultValue = false)]
#endif             
            public esConjunction Conjunction;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "Parenthesis", EmitDefaultValue = false)]
#endif             
            public esParenthesis Parenthesis;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "BetweenBegin", EmitDefaultValue = false)]
#endif            
            public object BetweenBegin;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "BetweenEnd", EmitDefaultValue = false)]
#endif            
            public object BetweenEnd;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "LikeEscape", EmitDefaultValue = false)]
#endif             
            public char LikeEscape;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "SubOperators", EmitDefaultValue = false)]
#endif             
            public List<esQuerySubOperator> SubOperators;

            /// <summary>
            /// Internal data used by <see cref="esComparison"/> and accessed by the EntitySpaces data providers.
            /// </summary>
#if (WCF)
            [DataMember(Name = "WhereExpression", EmitDefaultValue = false)]
#endif            
            public List<esComparison> WhereExpression;

            /// <summary>
            /// Whether the esQueryItem goes first in the expression
            /// </summary>
#if (WCF)
            [DataMember(Name = "ItemFirst", EmitDefaultValue = false)]
#endif
            public bool ItemFirst = true;
        }

        /// <summary>
        /// The data is hidden from intellisense, however, the providers, can typecast
        /// the esComparison and get to the real data without properties having to 
        /// be exposed thereby cluttering up the intellisense
        /// </summary>
        /// <param name="where">The esComparison to cast</param>
        /// <returns>The esComparisonData interface</returns>
        public static explicit operator esComparisonData(esComparison where)
        {
            return where.data;
        }

#if (WCF)
        [DataMember(Name = "Data", EmitDefaultValue = false)]
#endif  
        internal esComparisonData data = new esComparisonData();

#if !SILVERLIGHT
        [NonSerialized]
#endif
        // this lives for just a fraction during the queries build process
        private bool not;
    }
}
