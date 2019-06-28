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

namespace EntitySpaces.Interfaces
{
    /// <summary>
    /// Used Internally by EntitySpaces and its data providers to get at the database specific metadata information.
    /// </summary>
    public interface IProviderSpecificMetadata
    {
        /// <summary>
        /// Accesses the extended data for a given provider
        /// </summary>
        /// <param name="key">The key of the item in the extended data.</param>
        /// <returns>The Value associated with the key</returns>
        string this[System.String key] { get; set; }

        /// <summary>
        /// Used by the generated metadata classes. See <see cref="esTypeMap"/>
        /// </summary>
        /// <param name="columnName">The name of the database column</param>
        /// <param name="typeMap">The raw database column and its appropriate .NET data type</param>
        void      AddTypeMap(string columnName, esTypeMap typeMap);
        /// <summary>
        /// This is used by the EntitySpaces providers to get the appropriate data mappings for a given column
        /// </summary>
        /// <param name="columnName">The name of the database column</param>
        /// <returns>The column mapping</returns>
        esTypeMap GetTypeMap(string columnName);

        /// <summary>
        /// The name of the catalog (or database) can be null.
        /// </summary>
        string Catalog { get; set; }
        /// <summary>
        ///  The name of the schema. This can be null.
        /// </summary>
        string Schema { get; set; }
        /// <summary>
        /// The name of the destination. This is always the Table or View the class was generated against.
        /// </summary>
        string Destination { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// The name of the 'INSERT' stored procedure
        /// </summary>
        string spInsert { get; set; }
        /// <summary>
        /// The name of the 'UPDATE' stored procedure
        /// </summary>
        string spUpdate { get; set; }
        /// <summary>
        /// The name of the 'DELETE' stored procedure
        /// </summary>
        string spDelete { get; set; }
        /// <summary>
        /// The name of the 'LoadAll' stored procedure
        /// </summary>
        string spLoadAll { get; set; }
        /// <summary>
        /// The name of the 'LoadByPrimaryKey' stored procedure
        /// </summary>
        string spLoadByPrimaryKey { get; set; }
    }
}
