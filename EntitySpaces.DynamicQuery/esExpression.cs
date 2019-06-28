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
using System.Runtime.Serialization;

namespace EntitySpaces.DynamicQuery
{
    /// <summary>
    /// Used to house the parameters to Query.Select()
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "es", IsReference = true)]
    public class esExpression
    {
        public esExpression() { }

        /// <summary>
        /// Back Pointer to the Parent Query
        /// </summary>
        [DataMember(Name = "ParentQuery", Order = 99, EmitDefaultValue = false)]
        public esDynamicQuerySerializable Query;

        /// <summary>
        /// Contains the necessary information to describe this column
        /// </summary>
        [DataMember(Name = "Column", EmitDefaultValue = false)]
        public esColumnItem Column;
       
        /// <summary>
        /// A collection of SubOperators such as ToLower to apply to the select column
        /// </summary>
        [DataMember(Name = "SubOperators", EmitDefaultValue = false)]
        public List<esQuerySubOperator> SubOperators;

        /// <summary>
        /// Case / When / Then / End
        /// </summary>
        [DataMember(Name = "CaseWhen", EmitDefaultValue = false)]
        public esCase CaseWhen;

        /// <summary>
        /// The data behind the expression. This ends up looking like a tree in the end as 
        /// more arithmetic expressions are applied
        /// </summary>
        [DataMember(Name = "Expression", EmitDefaultValue = false)]
        public esMathmaticalExpression MathmaticalExpression;

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "LiteralValue", EmitDefaultValue = false)]
        public object LiteralValue;

        /// <summary>
        /// True if this esExpression has an expression formed by the arithmetic operators +,-,*,/,%
        /// </summary>
        public bool HasMathmaticalExpression
        {
            get { return this.MathmaticalExpression != null; }
        }

        /// <summary>
        /// True if this esExpression merely represents a literal value
        /// </summary>
        public bool IsLiteralValue
        {
            get { return this.LiteralValue != null; }
        }
    }
}
