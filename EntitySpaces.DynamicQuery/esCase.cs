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

using EntitySpaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EntitySpaces.DynamicQuery
{
    [Serializable]
    [DataContract(Namespace = "es", IsReference = true)]
    public class esCase
    {
        [NonSerialized]
        private esExpressionOrComparison WhenItem;

        /// <summary>
        /// The Constructor
        /// </summary>
        public esCase(esDynamicQuery query, esQueryItem queryItem)
        {
            this.data.Query = query;
            this.data.QueryItem = queryItem;
        }

        /// <summary>
        /// The Constructor
        /// </summary>
        public esCase(esDynamicQuery query)
        {
            this.data.Query = query;
        }

        public esCase When(object value)
        {
            this.WhenItem = new esExpressionOrComparison();
            this.WhenItem.Expression = new esExpression();
            this.WhenItem.Expression.LiteralValue = value;

            return this;
        }

        public esCase When(esQueryItem ex)
        {
            return this;
        }

        public esCase When(esExpression ex)
        {
            return this;
        }

        public esCase When(esComparison comparison)
        {
            this.WhenItem = new esExpressionOrComparison();
            this.WhenItem.Comparisons = new List<esComparison>();

            if (comparison != null)
            {
                if (comparison.data.WhereExpression != null)
                {
                    foreach (esComparison exp in comparison.data.WhereExpression)
                    {
                        esDynamicQuery q = exp.Value as esDynamicQuery;

                        if (q != null)
                        {
                            IDynamicQueryInternal iQ = q as IDynamicQueryInternal;
                            iQ.HookupProviderMetadata(q);
                        }
                    }

                    this.WhenItem.Comparisons.AddRange(comparison.data.WhereExpression);
                }
                else
                {
                    this.WhenItem.Comparisons.Add(comparison);
                }

                esDynamicQuery query = comparison.Value as esDynamicQuery;

                if (query != null)
                {
                    IDynamicQueryInternal iQ = query as IDynamicQueryInternal;
                    iQ.HookupProviderMetadata(query);
                }
            }

            return this;
        }

        #region Then

        public esCase Then(object value)
        {

            esSimpleCaseData.esCaseClause clause = new esSimpleCaseData.esCaseClause();
            clause.When = this.WhenItem;
            clause.Then = new esExpression();
            clause.Then.LiteralValue = value;

            if (data.Cases == null)
            {
                data.Cases = new List<esSimpleCaseData.esCaseClause>();
            }

            this.data.Cases.Add(clause);

            return this;
        }

        public esCase Then(esQueryItem queryItem)
        {
            esExpression expression = queryItem;

            esSimpleCaseData.esCaseClause clause = new esSimpleCaseData.esCaseClause();
            clause.When = this.WhenItem;
            clause.Then = expression;

            if (data.Cases == null)
            {
                data.Cases = new List<esSimpleCaseData.esCaseClause>();
            }

            this.data.Cases.Add(clause);

            return this;
        }

        public esCase Then(esExpression expression)
        {
            esSimpleCaseData.esCaseClause clause = new esSimpleCaseData.esCaseClause();
            clause.When = this.WhenItem;
            clause.Then = expression;

            if (data.Cases == null)
            {
                data.Cases = new List<esSimpleCaseData.esCaseClause>();
            }

            this.data.Cases.Add(clause);

            return this;
        }

        #endregion

        #region Else

        public esCase Else(object value)
        {
            this.data.Else = new esExpression();
            this.data.Else.LiteralValue = value;

            return this;
        }

        public esCase Else(esQueryItem queryItem)
        {
            esExpression expression = queryItem;
            this.data.Else = expression;

            return this;
        }

        public esCase Else(esExpression expression)
        {
            this.data.Else = expression;
            return this;
        }

        #endregion

        #region End

        public esQueryItem End()
        {
            return this.data.QueryItem;
        }

        #endregion

        /// <summary>
        /// Used internally by EntitySpaces to make the <see cref="esJoinItem"/> classes data available to the
        /// EntitySpaces data providers.
        /// </summary>
        [Serializable]
        [DataContract(Namespace = "es")]
        public struct esSimpleCaseData
        {
            public struct esCaseClause
            {
                public esExpressionOrComparison When;
                public esExpression Then;
            }

            public esDynamicQuery Query;
            public esQueryItem QueryItem;

            public List<esCaseClause> Cases;
            public esExpression Else;
        }

        [DataMember(Name = "Data", EmitDefaultValue = false)]
        internal esSimpleCaseData data;

        public static implicit operator esSimpleCaseData(esCase caseWhen)
        {
            return caseWhen.data;
        }
    }
}
