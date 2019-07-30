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

using EntitySpaces.DynamicQuery;

namespace EntitySpaces.Interfaces
{
    /// <summary>
    /// This class is contained in an <see cref="esColumnMetadataCollection"/> and used to store
    /// the onboard metadata in the generated Metadata class. For example, for a table named Employee
    /// a class named EmployeeMetadata will be generated. The Metadata class contains an 
    /// esColumnMetadataCollection containing an esColumnMetadata for each column in the table.<br></br>
    /// The EntitySpaces data provider use this information to build dynamic sql statements. This
    /// data can be very useful and you have full access to the this onboard metadata as a protected
    /// method as shown below:
    /// </summary>
    /// <remarks>
    /// This sample code shows how to iterate over the on-board meta data.
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
    public class esColumnMetadata
    {
        /// <summary>
        /// The Constructor
        /// </summary>
        public esColumnMetadata() { }

        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="name">Physical Column Name</param>
        /// <param name="ordinal">The Orderinal position in the Table or View</param>
        /// <param name="type">The .NET Data Type of the Column</param>
        public esColumnMetadata(string name, int ordinal, Type type)
        {
            this.name = name;
            this.Ordinal = ordinal;
            this.Type = type;
        }

        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="name">Physical Column Name</param>
        /// <param name="ordinal">The Orderinal position in the Table or View</param>
        /// <param name="type">The .NET Data Type of the Column</param>
        /// <param name="esType">The EntitySpaces data type</param>
        public esColumnMetadata(string name, int ordinal, Type type, esSystemType esType)
        {
            this.name = name;
            this.Ordinal = ordinal;
            this.Type = type;
            this.esType = esType;
        }

        /// <summary>
        /// The physical name of the column in the table or view.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Taken from MyGeneration as the column alias (if provided).
        /// </summary>
        public string Alias
        {
            get { return (this.alias == null) ? this.name : this.alias; }
            set { this.alias = value; }
        }

        /// <summary>
        /// The property name in the esEntity that exposes this column.
        /// </summary>
        public string PropertyName;

        /// <summary>
        /// The ordinal postition of this column in the table or view
        /// </summary>
        public int Ordinal;

        /// <summary>
        /// The numerical precision of this column, valid only if this column
        /// is a decimal or some similiar type.
        /// </summary>
        public int NumericPrecision;

        /// <summary>
        /// The numerical scale (number of decimal points) in this column, 
        /// valid only if this column is a decimal or some similiar type.
        /// </summary>
        public int NumericScale;

        /// <summary>
        /// For all string types this contains the maximum length of the
        /// column in the table or view.
        /// </summary>
        public long CharacterMaxLength;

        /// <summary>
        /// The "raw" default value as stored in the DBMS system.
        /// </summary>
        /// <seealso cref="HasDefault"/>
        public string Default;

        /// <summary>
        /// The description of the column in the table or view provided
        /// by MyGeneration.
        /// </summary>
        public string Description;

        /// <summary>
        /// The System type of this column
        /// </summary>
        public Type Type;

        /// <summary>
        /// The esSystemType enum, can be useful if you need to perform a switch
        /// statement on the Type.
        /// </summary>
        public esSystemType esType;

        /// <summary>
        /// True if <see cref="Default"/> has a value.
        /// </summary>
        /// <seealso cref="Default"/>
        public bool HasDefault;

        /// <summary>
        /// True if this column is all or part of the primary key.
        /// </summary>
        public bool IsInPrimaryKey;

        /// <summary>
        /// True if this column is an auto identity column. This is true
        /// for SQL. MySQL, and Access auto-incrementing columns. This is also
        /// true for Oracle Sequences if mapped properly in MyGeneration.
        /// </summary>
        public bool IsAutoIncrement;

        /// <summary>
        /// Contains the Custom autokey text such as the name of the sequencer
        /// </summary>
        public string AutoKeyText;

        /// <summary>
        /// True if this column can be NULL in the database.
        /// </summary>
        public bool IsNullable;

        /// <summary>
        /// True if this column is a derived or computed column in the database.
        /// </summary>
        public bool IsComputed;

        /// <summary>
        /// True if this column is read only, not all databases support read-only fields
        /// </summary>
        public bool IsReadOnly;

        /// <summary>
        /// True if this column is used for concurrency checking, such as Microsoft SQL Server's
        /// timestamp column. EntitySpaces can also support this for Oracle.
        /// </summary>
        public bool IsConcurrency;

        /// <summary>
        /// True if this column is used for EntitySpaces Multiprovider concurrency checking model
        /// This column must be an integer type for this to work
        /// </summary>
        public bool IsEntitySpacesConcurrency;

        /// <summary>
        /// True if this is a transient column
        /// </summary>
        public bool IsTransient;

        // Private Variables
        private string name;
        private string alias;
    }
}
