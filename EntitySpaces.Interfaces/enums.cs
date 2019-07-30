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
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EntitySpaces.Interfaces
{
    /// <summary>
    /// Provides additional options for creating a transaction scope
    /// </summary>
    public enum esTransactionScopeOption
    {
        /// <summary>
        /// This setting of 'None' is returned by esTransactionScope.GetCurrentTransactionScopeOption()
        /// if there is no ongoing esTransactionScope. The value of 'None' can never be passed to the
        /// constructor of the esTransactionScope class.
        /// </summary>
        None = 0,
        /// <summary>
        /// A transaction is required by the scope. It uses an ambient transaction if
        /// one already exists. Otherwise, it creates a new transaction before entering
        /// the scope. This is the default value.
        /// </summary>
        Required = 1, 
        /// <summary>
        /// A new transaction is always created for the scope.
        /// </summary>
        RequiresNew = 2,
        /// <summary>
        /// The ambient transaction context is suppressed when creating the scope. All
        /// operations within the scope are done without an ambient transaction context.
        /// </summary>
        Suppress = 3,
    }
 
    /// <summary>
    /// Sql access type, dynamic or stored procedures
    /// </summary>
    public enum esSqlAccessType
    {
        /// <summary>
        /// Unassigned
        /// </summary>
        Unassigned = 0,
        /// <summary>
        /// Use StoredProcedures for CRUD operations
        /// </summary>
        StoredProcedure,
        /// <summary>
        /// Create CRUD operations dynamically
        /// </summary>
        DynamicSQL
    }

    [Flags]
    [XmlType(IncludeInSchema = false, Namespace="")]
    [DataContract(Namespace= "www.entityspaces.net")]
    public enum esDataRowState
    {
        [EnumMember]
        Invalid = 0,

        [EnumMember]
        Unchanged = 2,

        [EnumMember]
        Added = 4,

        [EnumMember]
        Deleted = 8,

        [EnumMember]
        Modified = 16,
    }

    public enum DateType
    {
        Unassigned = 0,
        ClientSide = 1,
        ServerSide = 2
    };

    public enum ClientType
    {
        Unassigned = 0,
        Now = 1,
        UtcNow = 2
    };
}
