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
    /// Describes a SubOperator such as ToLower, ToUpper that can be applied
    /// to an <seealso cref="esComparison"/> or <seealso cref="esExpression"/>. This
    /// class is used internally by EntitySpaces.
    /// </summary>
    /// <remarks>
    /// SubOperator Feature Support by DBMS:
    /// <code>
    ///                 MS    MS    My    SQ    Vista Fire  Post
    /// Feature         SQL   Acces SQL   Lite  DB    bird  gre   Oracle  Ads
    /// --------------- ----- ----- ----- ----- ----- ----- ----- ------ -----
    /// Coalesce         Y     N     Y     -     Y     -     -     Y       Y
    /// Date             Y     N     Y     -     Y     -     -     Y       -
    /// LTrim            Y     Y     Y     -     Y     -     -     Y       Y
    /// RTrim            Y     Y     Y     -     Y     -     -     Y       Y
    /// SubString        Y     Y     Y     -     Y     -     -     Y       Y
    /// ToLower          Y     Y     Y     -     Y     -     -     Y       Y
    /// ToUpper          Y     Y     Y     -     Y     -     -     Y       -
    /// Trim             Y     Y     Y     -     Y     -     -     Y       Y
    /// 
    /// Notes:
    ///   (1) - xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    ///         xxxxxxxxxxxxxxx.
    ///   (2) - xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    ///         xxxxxxxxxxxx.
    ///   (3) - xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    ///         xxxxxxxxxxxxxxxxxxxxx.  
    ///   (4) - xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    ///   
    /// </code>
    /// </remarks>
    /// <example>
    /// You will not call esQuerySubOperator directly, but will be limited to use as
    /// in the example below, or to the many uses posted here:
    /// <code>
    /// http://www.entityspaces.net/portal/QueryAPISamples/tabid/80/Default.aspx
    /// </code>
    /// This will be the extent of your use of the esQuerySubOperator class:
    /// <code>
    /// .Select
    /// (
    ///		emps.Query.ReportsTo.ToUpper(),
    ///		emps.Query.Age.Avg("Average")
    /// .Where
    /// (
    ///		emps.Query.ReportsTo.ToUpper() == "TED",
    ///     emps.Query.BirthDate.ToShortDate() == "05/06/1948"
    /// );
    /// </code>
    /// </example>
#if !SILVERLIGHT    
    [Serializable]
#endif
#if (WCF)
    [DataContract(Namespace = "es", IsReference = true)]
#endif
    public class esQuerySubOperator
    {
        /// <summary>
        /// The type of the suboperator
        /// </summary>
#if (WCF)
        [DataMember(Name = "SubOperator", Order=1, EmitDefaultValue = false)]
#endif              
        public esQuerySubOperatorType SubOperator;

        /// <summary>
        /// Any parameters that this SubOperator may need. For instance Substring requires
        /// parameters
        /// </summary>
#if (WCF)
        [DataMember(Name = "Parameters", Order=2, EmitDefaultValue = false)]
#endif          
        public Dictionary<string, object> Parameters
        {
            get
            {
                if (this.parameters == null)
                {
                    parameters = new Dictionary<string, object>();
                }

                return this.parameters;
            }
        }

        private Dictionary<string, object> parameters;
    }
}
