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
    /// Used when arithmetic expressions are used in the DynamicQuery syntax.
    /// See <see cref="esArithmeticOperator"/>
    /// </summary>
#if !SILVERLIGHT     
    [Serializable]
#endif
#if (WCF)
    [DataContract(Namespace = "es", IsReference = true)]
#endif
    public class esMathmaticalExpression
    {
        /// <summary>
        /// The item on the left side of the operation
        /// </summary>
#if (WCF)
        [DataMember(Name = "SelectItem1", EmitDefaultValue = false)]
#endif            
        public esExpression SelectItem1;
        /// <summary>
        /// The item on the right side of the operation
        /// </summary>
#if (WCF)
        [DataMember(Name = "SelectItem2", EmitDefaultValue = false)]
#endif         
        public esExpression SelectItem2;
        /// <summary>
        /// The esArithmeticOperator applied to SelectItem1 and SelectItem2
        /// </summary>
#if (WCF)
        [DataMember(Name = "Operator", EmitDefaultValue = false)]
#endif        
        public esArithmeticOperator Operator;
        /// <summary>
        /// When the right hand side is a literal value this holds its value.
        /// </summary>
#if (WCF)
        [DataMember(Name = "Literal", EmitDefaultValue = false)]
#endif         
        public object Literal;
        /// <summary>
        /// When the right hand side is a literal value this describes its data type.
        /// </summary>
#if (WCF)
        [DataMember(Name = "LiteralType", EmitDefaultValue = false)]
#endif        
        public esSystemType LiteralType;

        /// <summary>
        /// Whether the esQueryItem goes first in the expression
        /// </summary>
#if (WCF)
        [DataMember(Name = "ItemFirst", EmitDefaultValue = false)]
#endif         
        public bool ItemFirst = true;
    }
}
