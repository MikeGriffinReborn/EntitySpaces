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

#if (WCF)
using System.Runtime.Serialization;
#endif

namespace EntitySpaces.DynamicQuery
{
    /// <summary>
    /// The esOrderByItem class is dynamically created by your BusinessEntity's
    /// DynamicQuery mechanism.
    /// This class is mostly used by the EntitySpaces architecture, not the programmer.
    /// </summary>
    /// <example>
    /// You will not call esOrderByItem directly, but will be limited to use as
    /// in the example below, or to the many uses posted here:
    /// <code>
    /// http://www.entityspaces.net/portal/QueryAPISamples/tabid/80/Default.aspx
    /// </code>
    /// This will be the extent of your use of the esOrderByItem class:
    /// <code>
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
    public class esOrderByItem
    {
        /// <summary>
        /// The esOrderByItem class is dynamically created by your
        /// BusinessEntity's DynamicQuery mechanism.
        /// See <see cref="esOrderByDirection"/> Enumeration.
        /// </summary>
        /// <param name="query">The associated DynamicQuery</param>
        public esOrderByItem()
        {

        }

        /// <summary>
        /// This allows the user to pass in a string to the Query.OrderBy() method
        /// directly and we convert it to an instance of an esOrderByItem class for
        /// them
        /// </summary>
        public static implicit operator esOrderByItem(string literal)
        {
            esOrderByItem item = new esOrderByItem();
            item.Expression = new esExpression();
            item.Expression.Column.Name = literal;
            return item;
        }

        /// <summary>
        /// esOrderByDirection Direction.
        /// See <see cref="esOrderByDirection"/> Enumeration.
        /// </summary>
#if (WCF)
        [DataMember(Name = "Direction", EmitDefaultValue = false)]
#endif         
        public esOrderByDirection Direction;

        /// <summary>
        /// The Expression for the OrderBy statement
        /// </summary>  
#if (WCF)
        [DataMember(Name = "Expression", EmitDefaultValue = false)]
#endif
        public esExpression Expression;
    }
}
