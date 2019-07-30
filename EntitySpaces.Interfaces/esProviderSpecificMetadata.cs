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

namespace EntitySpaces.Interfaces
{
    /// <summary>
    /// This class houses the specific meta data for a given database type. When doing multi-database
    /// development with EntitySpaces a given class will have a different esProviderSpecificMetadata
    /// for each database. 
    /// </summary>
    [Serializable] 
    public class esProviderSpecificMetadata : IProviderSpecificMetadata
    {
        /// <summary>
        /// The Constructor
        /// </summary>
        public esProviderSpecificMetadata()
        {

        }

        public bool ContainsKey(string key)
        {
            return this.extData.ContainsKey(key);
        }

        #region IProviderSpecificMetadata Members

        public string this[string key]
        {
            get { return extData[key];  }
            set { extData[key] = value; }
        }

        public void AddTypeMap(string columnName, esTypeMap typeMap)
        {
            types[columnName] = typeMap;
        }

        public esTypeMap GetTypeMap(string columnName)
        {
            if (types.ContainsKey(columnName))
                return types[columnName];
            else
                return null;
        }

        public string Catalog
        {
            get { return catalog; }
            set { catalog = value; }
        }

        public string Schema
        {
            get { return schema; }
            set { schema = value; }
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        public string spInsert
        {
            get { return sp_Insert; }
            set { sp_Insert = value; }
        }

        public string spUpdate
        {
            get { return sp_Update; }
            set { sp_Update = value; }
        }

        public string spDelete
        {
            get { return sp_Delete; }
            set { sp_Delete = value; }
        }

        public string spLoadAll
        {
            get { return sp_LoadAll; }
            set { sp_LoadAll = value; }
        }

        public string spLoadByPrimaryKey
        {
            get { return sp_LoadByPrimaryKey; }
            set { sp_LoadByPrimaryKey = value; }
        }

        #endregion

        private string catalog;
        private string schema;
        private string destination;
        private string source;
        private string sp_Insert;
        private string sp_Update;
        private string sp_Delete;
        private string sp_LoadAll;
        private string sp_LoadByPrimaryKey;

        private Dictionary<string, esTypeMap> types = new Dictionary<string, esTypeMap>();
        private Dictionary<string, string> extData = new Dictionary<string, string>();
    }
}
