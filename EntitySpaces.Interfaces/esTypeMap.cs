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

namespace EntitySpaces.Interfaces
{
    /// <summary>
    /// Used by the generated metadata classes and used to map raw database types to .NET types. 
    /// </summary>
    /// <example>
    /// This code was taken from EmployeeMetadata class generated from Microsoft SQL's Northwind database.
    /// Notice the use of the esTypeMap class.
    /// <code>
    /// private esProviderSpecificMetadata esDefault(string mapName)
    /// {
    /// 	if(!_providerMetadataMaps.ContainsKey(mapName))
    /// 	{
    /// 		esProviderSpecificMetadata meta = new esProviderSpecificMetadata();
    /// 		
    /// 		meta.AddTypeMap("EmployeeID", new esTypeMap("int", "System.Int32"));
    /// 		meta.AddTypeMap("LastName", new esTypeMap("varchar", "System.String"));
    /// 		meta.AddTypeMap("FirstName", new esTypeMap("varchar", "System.String"));
    /// 		meta.AddTypeMap("Supervisor", new esTypeMap("int", "System.Int32"));
    /// 		meta.AddTypeMap("Age", new esTypeMap("int", "System.Int32"));			
    /// 		
    /// 		this._providerMetadataMaps["esDefault"] = meta;
    /// 	}
    /// 	
    /// 	return this._providerMetadataMaps["esDefault"];
    /// }
    /// </code>
    /// </example>
    [Serializable]
    public class esTypeMap
    {
        /// <summary>
        /// Null constructor, never used
        /// </summary>
        public esTypeMap() { }

        /// <summary>
        /// This constructor is used by the generated metadata map classes
        /// </summary>
        /// <param name="nativeType"></param>
        /// <param name="systemType"></param>
        public esTypeMap(string nativeType, string systemType)
        {
            this.nativeType = nativeType;
            this.systemType = systemType;
        }

        /// <summary>
        /// The low level database type for this column.
        /// </summary>
        public string NativeType
        {
            get { return nativeType; }
        }

        /// <summary>
        /// The .NET System type to use for this column.
        /// </summary>
        public string SystemType
        {
            get { return systemType; }
        }

        private string nativeType;
        private string systemType;
    }
}
