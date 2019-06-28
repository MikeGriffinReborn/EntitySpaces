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

using System.Collections;
using System.Collections.Generic;
using System.Data;
using EntitySpaces.Interfaces;

namespace EntitySpaces.Core
{
    /// <summary>
    /// The IEntityCollection interface really serves to semi-hide certain properties
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
    /// <seealso cref="IEntity"/>
    public interface IEntityCollection
    {
        /// <summary>
        /// Get or Set the current esEntity's connection information. See <see cref="esConnection"/>
        /// </summary>
        /// <seealso cref="esConnection"/>
        esConnection Connection { get; set; }

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
        /// Used by esDataSource to gain access to the Query so that it can
        /// implement AutoPaging and AutoSorting
        /// </summary>
        esDynamicQuery Query { get; }

        /// <summary>
        /// Called by esDataSource to hookup the query to the collection during the esSelect event.
        /// </summary>
        /// <returns></returns>
        void HookupQuery(esDynamicQuery query);

        /// <summary>
        /// The List of Entities marked for deletion
        /// </summary>
        IEnumerable DeletedEntities { get; }

        /// <summary>
        /// Provides access to the on-board metadata
        /// </summary>
        IMetadata Meta { get; }

        /// <summary>
        /// Indicates whether or not the Lazy Loading of the Hierarchical Data Model is turned off
        /// </summary>
        bool IsLazyLoadDisabled { get; set; }

        /// <summary>
        /// Called by EntitySpaces when prefetching data
        /// </summary>
        /// <param name="row">The DataRow for the new entity (to be created)</param>
        /// <param name="ordinals">The columm ordinals, null upon the first one</param>
        /// <returns>The column ordrinals, passed right back in as a parameter</returns>
        Dictionary<string, int> PopulateCollection(DataRow row, Dictionary<string, int> ordinals);
    }
}
