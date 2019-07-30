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
using System.Data;
using System.Collections.Generic;

#if (LINQ)
using System.Data.Linq;
using System.Linq;
#endif


using EntitySpaces.DynamicQuery;

namespace EntitySpaces.Interfaces
{
    /// <summary>
    /// Sent to the EntitySpaces DataProviders to carry out commands.
    /// </summary>
    [Serializable] 
    public class esDataRequest
    {
        #region Error Handling

        /// <summary>
        /// Delegate used by our OnError event handler
        /// </summary>
        /// <param name="packet">The esEntityPacket in error</param>
        /// <param name="ex">The exception that was thrown</param>
        public delegate void OnErrorHandler(esEntitySavePacket packet, string error);

        /// <summary>
        /// The OnError event handler, used interally by EntitySpaces
        /// </summary>
        public event OnErrorHandler OnError;

        /// <summary>
        /// Called by the DataProviders to invoke the OnError event handler
        /// </summary>
        /// <param name="packet">The esEntityPacket in error</param>
        /// <param name="ex">The exception that was thrown</param>
        public void FireOnError(esEntitySavePacket packet, string error)
        {
            OnErrorHandler handler = OnError;

            if (handler != null)
            {
                handler(packet, error);
            }
        }

        #endregion

        /// <summary>
        /// This is used during the Saving of a collection, if true, exceptions do not prevent successful records from being saved.
        /// </summary>
        public bool ContinueUpdateOnError;

        /// <summary>
        /// The connection string to connect to the actual DBMS system
        /// </summary>
        public string ConnectionString;

        /// <summary>
        /// The name of the catalog, for instance, in SQL Server this 
        /// could be "Northwind".
        /// </summary>
        public string Catalog;

        /// <summary>
        /// The name of the Schema, for instance, in SQL Server this 
        /// is typically "dbo".
        /// </summary>
        public string Schema;

        /// <summary>
        /// This is optional and typically for SQL Server it is either
        /// 2000 or 2005. 
        /// </summary>
        public string DatabaseVersion;

        /// <summary>
        /// Currently not used.
        /// </summary>
        public Guid DataID;

        /// <summary>
        /// Whether to use Stored Procedures or Dynamic SQL
        /// </summary>
        public esSqlAccessType SqlAccessType;

        /// <summary>
        /// The DataTable containing only the changed rows to commit
        /// </summary>
        public DataTable Table;

        /// <summary>
        /// Used when a single entity is being saved
        /// </summary>
        public esEntitySavePacket EntitySavePacket;

        /// <summary>
        /// Used when saving a collection
        /// </summary>
        public List<esEntitySavePacket> CollectionSavePacket;

        /// <summary>
        /// Set to true by esEntityCollection.SaveAndDiscard() so it doesn't bother returning
        /// indentity column values, timestamps, or other computed columns.
        /// </summary>
        public bool IgnoreComputedColumns;

        // Query Data
        /// <summary>
        /// The form of the query itself, see <see cref="esQueryType"/>
        /// </summary>
        public esQueryType QueryType;

        /// <summary>
        /// Depending on what the value of <see cref="esQueryType"/> is, this could
        /// be the actual name of a stored procedure, DBMS function, raw text, or 
        /// whatever else is possible.
        /// </summary>
        public string QueryText;

        /// <summary>
        /// The parameters if any required to carry out the command. Remember, do not
        /// tack on decorators such as @ or ? on the front of your parameters, the EntitySpaces
        /// DataProviders do that for you so you can write provider independent queries.
        /// </summary>
        public esParameters Parameters;

        /// <summary>
        /// Automatically populated whenver you use the EntitySpaces dynamic query mechanism.
        /// </summary>
        public esDynamicQuerySerializable DynamicQuery;

#if (LINQ)
        /// <summary>
        /// The DataContext for the LINQ query
        /// </summary>
        public DataContext LinqContext;

        /// <summary>
        /// The LINQ to SQL query
        /// </summary>
        public IQueryable LinqQuery;
#endif

        /// <summary>
        /// Optional, the command timeout as defined in the web.config or app.config file
        /// </summary>
        public int? CommandTimeout;

        /// <summary>
        /// These are the columns from the generated Metadata class. These columns contain
        /// enough metadata to generate the dynamic sql.
        /// </summary>
        public esColumnMetadataCollection Columns;

        /// <summary>
        /// A List of Lists, each nested list contains the modified columns for each
        /// DataRow contained in the DataTable
        /// </summary>
        /// <seealso cref="Table"/>
        public List<List<string>> ModifiedColumns = new List<List<string>>();

        /// <summary>
        /// This contains metadata that may be specific to an EntitySpaces DataProvider. The
        /// data in this property is driven by the "providerMetadataKey" in the config file.
        /// This allows stored procedure names, for instance, to have different names on
        /// different databases and yet the esEntity and esEntityCollection objects are
        /// oblivious to that fact.
        /// </summary>
        public esProviderSpecificMetadata ProviderMetadata;

        /// <summary>
        /// Transient data using during the SQL insert/update/delete process
        /// </summary>
        public PropertyCollection Properties;

        public Dictionary<string, int> SelectedColumns;

        public object Caller;

        public bool BulkSave;

        public string[] BulkSaveOptions;
    }
}
