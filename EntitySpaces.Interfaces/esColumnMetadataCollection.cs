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
using System.Collections;
using System.Collections.Generic;

using System.Diagnostics;

namespace EntitySpaces.Interfaces
{
    /// <summary>
    /// This is the collection used to house the onboard metadata created during 
    /// code generation.
    /// </summary>
    /// <remarks>
    /// This collection contains one <see cref="esColumnMetadata"/> for each column in 
    /// the table or view. This data is used by EntitySpaces data providers to create dynamic
    /// sql and by EntitySpaces during databinding.
    /// <code>
    /// public partial class EmployeesCollection : esEmployeesCollection
    ///	{
    ///		public void CheckoutTheMetadata()
    ///		{
    ///			foreach(esColumnMetadata esCol in this.Meta.Columns)
    ///			{
    ///				Console.WriteLine(esCol.IsInPrimaryKey.ToString());
    ///			}
    ///		}
    ///	}
    /// </code>
    /// </remarks>
    [Serializable] 
    public partial class esColumnMetadataCollection : IEnumerable 
    {
        public esColumnMetadataCollection() 
        {
     
        }

        public bool IsSpecialColumn(esColumnMetadata col)
        {
            if (DateAdded != null && DateAdded.IsEnabled && DateAdded.ColumnName == col.Name) return true;
            if (DateModified != null && DateModified.IsEnabled && DateModified.ColumnName == col.Name) return true;
            if (AddedBy != null && AddedBy.IsEnabled && AddedBy.ColumnName == col.Name) return true;
            if (ModifiedBy != null && ModifiedBy.IsEnabled && ModifiedBy.ColumnName == col.Name) return true;

            return false;
        }

        /// <summary>
        /// The number of <see cref="esColumnMetadata"/> objects in the collection.
        /// </summary>
        public System.Int32 Count
        {
            get { return list.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ThereAreDefaults
        {
            get { return this.thereAreDefaults; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<esColumnMetadata> PrimaryKeys
        {
            get
            {
                return this.primaryKeys;
            }
        }

        /// <summary>
        /// Provides direct access into the collection by column name. 
        /// </summary>
        /// <param name="columnName">The name of the desired column. This parameter is expected
        /// to be the physical name of the column as in the table or view, for example, 
        /// Employees.ColumnNames.LastName</param>
        /// <returns></returns>
        public esColumnMetadata this[System.String columnName]
        {
            get
            {
                esColumnMetadata col = null;

                if (this.hashByColumnName.ContainsKey(columnName))
                {
                    col = this.hashByColumnName[columnName];
                }

                return col;
            }
        }

        /// <summary>
        /// Provides sequential access into the collection.
        /// </summary>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public esColumnMetadata this[System.Int32 ordinal]
        {
            get { return this.list[ordinal]; }
        }

        /// <summary>
        /// Used internally by EntitySpaces. This should never be called by user code
        /// </summary>
        /// <param name="column">The new esColumnMetadata to add to the array</param>
        public void Add(esColumnMetadata column)
        {
            list.Add(column);
            hashByColumnName[column.Name] = column;
            hashByPropertyName[column.PropertyName] = column;

            if (column.HasDefault)
            {
                this.thereAreDefaults = true;
            }

            if (column.IsInPrimaryKey)
            {
                this.primaryKeys.Add(column);
            }
        }

        /// <summary>
        /// Searches for an esColumnMetadata by the propery name. This method is the same as
        /// using the indexer.
        /// </summary>
        /// <param name="columnName">The high level PropertyName, for example, Employees.ColumnNames.LastName</param>
        /// <returns>The esColumnMetadata or null if not found.</returns>
        public esColumnMetadata FindByColumnName(string columnName)
        {
            esColumnMetadata col = null;

            if (this.hashByColumnName.ContainsKey(columnName))
            {
                col = this.hashByColumnName[columnName];
            }

            return col;
        }

        /// <summary>
        /// Searches for an esColumnMetadata by the propery name.
        /// </summary>
        /// <param name="propertyName">The high level PropertyName, for example, Employees.PropertyNames.LastName</param>
        /// <returns>The esColumnMetadata or null if not found.</returns>
        public esColumnMetadata FindByPropertyName(string propertyName)
        {
            esColumnMetadata col = null;

            if (this.hashByPropertyName.ContainsKey(propertyName))
            {
                col = this.hashByPropertyName[propertyName];
            }

            return col;
        }

        #region IEnumerable Members

        /// <summary>
        /// Supports the foreach() syntax over the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        #endregion

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<esColumnMetadata> list = new List<esColumnMetadata>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<esColumnMetadata> primaryKeys = new List<esColumnMetadata>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<System.String, esColumnMetadata> hashByColumnName = new Dictionary<string, esColumnMetadata>(StringComparer.OrdinalIgnoreCase);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<System.String, esColumnMetadata> hashByPropertyName = new Dictionary<string, esColumnMetadata>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool thereAreDefaults;

        public SpecialDate DateAdded;
        public SpecialDate DateModified;
        public AuditingInfo AddedBy;
        public AuditingInfo ModifiedBy;

        #region Nested Classes
        [Serializable]
        public class SpecialDate
        {
            public bool IsEnabled;
            public string ColumnName;
            public DateType Type;
            public ClientType ClientType;

            public bool IsServerSide
            {
                get
                {
                    return IsEnabled && Type == DateType.ServerSide;
                }
            }
        }

        [Serializable]
        public class AuditingInfo
        {
            public bool IsEnabled;
            public bool UseEventHandler;
            public string ColumnName;

            public bool IsServerSide
            {
                get
                {
                    return IsEnabled && !UseEventHandler;
                }
            }
        }
        #endregion
    }
}
