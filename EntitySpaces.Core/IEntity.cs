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


using System.Collections.Generic;
using System.Data;

using EntitySpaces.Interfaces;

namespace EntitySpaces.Core
{
    /// <summary>
    /// The IEntity interface really serves to semi-hide certain properties
    /// and avoid collisions that might occur during code generation. This
    /// interface is obtained as follows using the "es" property.
    /// </summary>
    /// <remarks>
    /// This code overrides the default connection in the config file and
    /// uses an alternate entry in the config file named "Oracle" as the
    /// connection information. 
    /// <code>
    ///	Employees emp = new Employees();
    ///	emp.es.Connection.Name = "Oracle";
    ///	emp.LoadByPrimaryKey(1);
    /// </code>
    /// </remarks>
    /// <seealso cref="IEntityCollection"/>
    public interface IEntity
    {
        /// <summary>
        /// Get or Set the current esEntity's connection information. See <see cref="esConnection"/>
        /// </summary>
        /// <seealso cref="esConnection"/>
        esConnection Connection { get; set; }

        /// <summary>
        /// The esEntity class keeps a list of modified columns in order to update only those 
        /// columns which have truly changed during DynamicSQL updates. The modified columns
        /// list is not used when saving via stored procedures, in that case all columns are 
        /// always updated whether they are modified or not. The ModifiedColumns list is cleared
        /// after a successful call to Save.
        /// </summary>
        List<string> ModifiedColumns { get; }

        /// <summary>
        /// The name of the DBMS catalog (or database). This catalog can be driven by the connection
        /// string or it can be pulled from the meta data class which is generated during code
        /// generation.
        /// </summary>
        /// <seealso cref="Schema"/>
        string Catalog { get; }

        /// <summary>
        /// The name of the DBMS schema. This schema be driven by the connection
        /// string or will be pulled from the meta data class which is generated during code
        /// generation.
        /// </summary>
        /// <seealso cref="Catalog"/>
        string Schema { get; }

        /// <summary>
        /// By default this is the name of the table or view which was used to generate
        /// the esEntity during code generation. 
        /// </summary>
        /// <seealso cref="Source"/>
        string Destination { get; }

        /// <summary>
        /// By default this is the name of the table or view which was used to generate
        /// the esEntity during code generation. 
        /// </summary>
        /// <seealso cref="Destination"/>
        string Source { get; }

        /// <summary>
        /// Created during the code generation process. This is the name of the INSERT stored
        /// procedure used when saving via stored procedures. This is not used when the current
        /// connection for the esEntity has "sqlAccessType" set to "DynamicSQL".
        /// </summary>
        string spInsert { get; }

        /// <summary>
        /// Created during the code generation process. This is the name of the UPDATE stored
        /// procedure used when saving via stored procedures. This is not used when the current
        /// connection for the esEntity has "sqlAccessType" set to "DynamicSQL".
        /// </summary>
        string spUpdate { get; }

        /// <summary>
        /// Created during the code generation process. This is the name of the DELETE stored
        /// procedure used when saving via stored procedures. This is not used when the current
        /// connection for the esEntity has "sqlAccessType" set to "DynamicSQL".
        /// </summary>
        string spDelete { get; }

        /// <summary>
        /// Created during the code generation process. This is the name of the stored
        /// procedure used to load all of the records when calling LoadAll(). LoadAll() is
        /// not generally called however on an esEntity because loading more than one 
        /// record into an esEntity results in an exception.
        /// </summary>
        string spLoadAll { get; }

        /// <summary>
        /// Created during the code generation process. This is the name of the stored
        /// procedure used to load a specific record by its primary key when calling LoadByPrimaryKey().
        /// This is not used when the current connection for the esEntity has "sqlAccessType" set to "DynamicSQL".
        /// </summary>
        string spLoadByPrimaryKey { get; }

        /// <summary>
        /// This method returns true if the esEntity has been loaded with data and the esEntity is not
        /// marked as Deleted. Remember, an esEntity can only represent a single row. However the full Query
        /// syntax is allowed to load an esEntity. See <see cref="esDynamicQuery"/>
        /// </summary>
        bool HasData { get; }

        /// <summary>
        /// This is set to true if AddNew(), MarkAsDeleted(), or if any of the esEntities table based properties 
        /// have been changed. After a successful call to <see cref="Save"/> IsDirty will report false.
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// This is set to true if any entity or collection in the hierarchical model (which has already been loaded)
        /// is dirty. This is also available on the Collection class as well.
        /// </summary>
        bool IsGraphDirty { get; }

        /// <summary>
        /// Returns true of the RowState is esDataRowState.Modified
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        /// Returns true of the RowState is esDataRowState.Added
        /// </summary>
        bool IsAdded { get; }

        /// <summary>
        /// Returns true of the RowState is esDataRowState.Deleted
        /// </summary>
        bool IsDeleted { get; }

        /// <summary>
        /// See the ADO.NET esDataRowState enum for more information. 
        /// </summary>
        esDataRowState RowState { get; set; }

        /// <summary>
        /// See the ADO.NET DataRow.RowError for more information.
        /// </summary>
        string RowError { get; }

        /// <summary>
        /// Provides access to the on-board metadata
        /// </summary>
        IMetadata Meta { get; }

        /// <summary>
        /// Indicates whether or not the Lazy Loading of the Hierarchical Data Model is turned off
        /// </summary>
        bool IsLazyLoadDisabled { get; set; }

        /// <summary>
        /// Called by the DynamicQuery Prefetch logic
        /// </summary>
        /// <param name="collection">The name of the collection, example "OrdersCollectionByEmployeeID"</param>
        /// <returns>The proper collection type, example, "OrdersCollection"</returns>
        esEntityCollectionBase CreateCollection(string collection);
    }
}
