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
    /// Created when Query.InnerJoin (LeftJoin, RightJoin, FullJoin) is called.
    /// </summary>
#if !SILVERLIGHT    
    [Serializable]
#endif
#if (WCF)
    [DataContract(Namespace = "es", IsReference = true)]
#endif
    public class esJoinItem
    {
#if !SILVERLIGHT
        [NonSerialized]
#endif
        private esDynamicQuerySerializable parentQuery;

        /// <summary>
        /// The Constructor
        /// </summary>
        public esJoinItem()
        {

        }

        /// <summary>
        /// The Constructor
        /// </summary>
        public esJoinItem(esDynamicQuerySerializable parentQuery)
        {
            this.parentQuery = parentQuery;
        }

        /// <summary>
        /// Used to describe the "where" conditions of the join itself
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public esDynamicQuerySerializable On(params object[] items)
        {
            if (this.data.WhereItems == null)
            {
                this.data.WhereItems = new List<esComparison>();
            }

            foreach (object item in items)
            {
                esComparison wi = item as esComparison;

                if (wi != null)
                {
                    if (wi.data.WhereExpression != null)
                    {
                        foreach (esComparison exp in wi.data.WhereExpression)
                        {
                            esDynamicQuerySerializable q = exp.Value as esDynamicQuerySerializable;

                            if (q != null)
                            {
                                IDynamicQuerySerializableInternal iQ = q as IDynamicQuerySerializableInternal;
                                iQ.HookupProviderMetadata(q);
                            }                            
                        }

                        this.data.WhereItems.AddRange(wi.data.WhereExpression);
                    }
                    else
                    {
                        this.data.WhereItems.Add(wi);
                    }

                    esDynamicQuerySerializable query = wi.Value as esDynamicQuerySerializable;

                    if (query != null)
                    {
                        IDynamicQuerySerializableInternal iQ = query as IDynamicQuerySerializableInternal;
                        iQ.HookupProviderMetadata(query);
                    }
                }
                else
                {
                    throw new Exception("Unsupported Join Syntax");
                }
            }

            return this.parentQuery;
        }

        #region ProcessWhereItems
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
                        throw new Exception("Unsupported Type");
                    }
                }
            }

            items.Add(new esComparison(esParenthesis.Close));

            return items;
        }
        #endregion

        /// <summary>
        /// Used internally by EntitySpaces to make the <see cref="esJoinItem"/> classes data available to the
        /// EntitySpaces data providers.
        /// </summary>
#if !SILVERLIGHT    
    [Serializable]
#endif
#if (WCF)
        [DataContract(Namespace = "es")]
#endif
        public struct esJoinItemData
        {
            /// <summary>
            /// The Query that makes up the join
            /// </summary>
#if (WCF)
            [DataMember(Name = "Query", Order = 99, EmitDefaultValue = false)]
#endif              
            public esDynamicQuerySerializable Query;
            /// <summary>
            /// The join type, InnerJoin, LeftJoin, ...
            /// </summary>
#if (WCF)
            [DataMember(Name = "JoinType", EmitDefaultValue = false)]
#endif              
            public esJoinType JoinType;
            /// <summary>
            /// The where conditions for the subquery
            /// </summary>
#if (WCF)
            [DataMember(Name = "WhereItems", EmitDefaultValue = false)]
#endif              
            public List<esComparison> WhereItems;
        }

        /// <summary>
        /// The data is hidden from intellisense, however, the providers, can typecast
        /// the esJoinItem and get to the real data without properties having to 
        /// be exposed thereby cluttering up the intellisense.
        /// </summary>
        /// <param name="join"></param>
        /// <returns></returns>
        public static explicit operator esJoinItemData(esJoinItem join)
        {
            return join.data;
        }

#if (WCF)
        [DataMember(Name = "Data", EmitDefaultValue = false)]
#endif   
        internal esJoinItemData data;
    }
}
