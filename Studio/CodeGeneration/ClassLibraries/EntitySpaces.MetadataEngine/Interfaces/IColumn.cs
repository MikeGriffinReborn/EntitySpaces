using System;

namespace EntitySpaces.MetadataEngine
{
	/// <summary>
	/// IColumn represents a column or field in your DBMS. IColumn is used to represent columns for both Tables and Views.
	/// </summary>
	/// <remarks>
	///	IColumn Collections:
	/// <list type="table">
	///		<item><term>ForeignKeys</term><description>Contains all of the foreign keys this column plays a role in</description></item>
	///		<item><term>Properties</term><description>A collection that can hold key/value pairs of your choosing</description></item>
	///	</list>
	///	</remarks>
	///	<example>
	/// VBScript
	///	<code>
	/// Dim objColumn
	/// Set objColumn = objTable.Columns.Item(5)
	/// Set objColumn = objTable.Columns.Item("FirstName")
	/// 
	/// ' Loop through the collection
	/// For Each objColumn in objTable.Columns
	///	    output.writeLn objColumn.Name
	///	    output.writeLn objColumn.Alias
	///     output.writeLn objColumn.DataTypeNam
	/// Next
	///	</code>
	/// JScript
	///	<code>
	/// var objColumn;
	///	objColumn = objTable.Columns.Item(5);
	///	objColumn = objTable.Columns.Item("FirstName");
	///	
	/// for (var j = 0; j &lt; objTable.Columns.Count; j++) 
	/// {
	///	    objColumn = objTable.Columns.Item(j);
	///	    
	///	    output.writeln(objColumn.Name);
	///	    output.writeln(objColumn.Alias);
	///	    output.writeln(objColumn.DataTypeName);				
	/// }
	///	</code>
	/// </example>
	public interface IColumn 
	{
		// Objects
		/// <summary>
		/// The parent ITable of this Column, null if the parent is a View.
		/// </summary>
		ITable Table { get; }
		/// <summary>
		/// The parent IView of this Column, null if the parent is a Table. 
		/// </summary>
		IView View { get; }
		/// <summary>
		/// The domain of this Column, null if this columns type isn't derived from a domain. 
		/// </summary>
		IDomain Domain { get; }


		// Collections
		/// <summary>
		/// The ForeignKeys that this Column participates in. See <see cref="ForeignKeys"/>
		/// </summary>
		IForeignKeys ForeignKeys { get; }

		/// <summary>
		/// The Properties (Local) for this column. These are user defined and are typically stored in 'UserMetaData.xml' unless changed in the Default Settings dialog.
		/// Properties consist of key/value pairs.  You can populate this collection during your script or via the Dockable window.
		/// To save any data added to this collection call esMetadataEngine.SaveUserMetaData(). See <see cref="IProperty"/>
		/// </summary>
		IPropertyCollection Properties { get; }

		// User Meta Data
		string UserDataXPath { get; }

		/// <summary>
		/// You can override the physical name of the Column. If you do not provide an Alias the value of 'Column.Name' is returned.
		/// If your column in your DBMS is 'TXT_FIRST_NAME' you might want to give it an Alias of 'FirstName' so that your business object property will be a nice name.
		/// You can provide an Alias the User Meta Data window. You can also set this during a script and then call esMetadataEngine.SaveUserMetaData().
		/// See <see cref="Name"/>
		/// </summary>
		string Alias { get; set; }
	
		/// <summary>
		/// This is the physical column name as stored in your DBMS system. See <see cref="Alias"/>
		/// </summary>
		string Name { get; }			

		/// <summary>
		/// The OLEDB data type value see (http://msdn.microsoft.com/library/default.asp?url=/library/en-us/oledb/htm/olprappendixa_1.asp)
		/// </summary>
		System.Int32 DataType { get; }

        /// <summary>
        /// If this is true this is a database extension type such as Microsoft SQL 'SqlGeometry' class
        /// </summary>
        System.Boolean IsNonSystemType { get; }

		/// <summary>
		/// The native data type as stored in your DBMS system, for instance a SQL 'nvarchar', or Access 'Memo'. See <see cref="DataTypeNameComplete"/>
		///	</summary>
		string DataTypeName { get; }

		/// <summary>
		/// This is the full data type name, whereas the DataType property might be 'nvarchar' the DataTypeName property would be 'nvarchar(200)', this varies from DBMS to DBMS.
		/// See <see cref="DataTypeName"/>
		/// </summary>
		string DataTypeNameComplete { get; }

		/// <summary>
		/// The Language Mappings window is where these are entered and they are stored in 'Languages.xml'. 
		/// If your DMBS system is Microsoft SQL and your language is C# then nvarchar will be mapped to a C# 'string'. 
		/// Anytime that you need to expose this columns value to your programming language use this value.
		/// See <see cref="DbTargetType"/>
		/// </summary>
		string LanguageType { get; }

		/// <summary>
		/// Column GUID. For Providers that do not use GUIDs to identify columns 'Guid.Empty' is returned.
		/// </summary>
		Guid Guid { get; }

		/// <summary>
		/// Column property ID. For Providers that do not associate PROPIDs with columns 0 is returned.
		/// </summary>
		System.Int32 PropID { get; }

		/// <summary>
		/// The ordinal of the column. Columns are numbered starting from one.
		/// </summary>
		System.Int32 Ordinal { get; }

		/// <summary>
		/// If 'True' the column has a default value. If 'False' the column does not have a default value, or it is unknown whether the column has a default value.
		/// </summary>
		System.Boolean HasDefault { get; }

		/// <summary>
		/// The actual unparsed default from your DBMS system, for instance in SQL server it might be 'getdate()' for a DateTime field or N'USA' for Country.
		/// </summary>
		string Default { get; }

		/// <summary>
		/// A bitmask that describes column characteristics. The DBCOLUMNFLAGS enumerated type specifies the bits in the bitmask. For information about DBCOLUMNFLAGS, see IColumnsInfo::GetColumnInfo in the reference section. 
		/// If COLUMN_NAME refers to a column in a table or view that is updatable, one of either DBCOLUMNFLAGS_WRITE or DBCOLUMNFLAGS_WRITEUNKNOWN should be set. 
		/// For more information about these flags, see "DBCOLUMNFLAGS Enumerated Type.
		/// See (http://msdn.microsoft.com/library/default.asp?url=/library/en-us/oledb/htm/olprirowsetchange_2.asp)
		/// </summary>
		System.Int32 Flags { get; }

		/// <summary>
		/// The Column allows null in your DBMS system.
		/// </summary>
		System.Boolean IsNullable { get; }

		/// <summary>
		/// The GUID of the column's data type. For Providers that do not use GUIDs to identify data types 'Guid.Empty' is returned.
		/// </summary>
		Guid TypeGuid { get; }

		/// <summary>
		/// The maximum possible length of a value in the column. For character, binary, or bit columns, this is one of the following: 
		/// <list type="bullet">
		/// <item>
		/// The maximum length of the column in characters, bytes, or bits, respectively, if one is defined. For example, a CHAR(5) column in an SQL table has a maximum length of 5. 
		/// </item>
		/// <item>
		/// The maximum length of the data type in characters, bytes, or bits, respectively, if the column does not have a defined length. 
		/// </item>
		/// <item>
		/// Zero (0) if neither the column nor the data type has a defined maximum length.
		/// </item>
		/// </list>
		/// </summary>
		System.Int32 CharacterMaxLength { get; }

		/// <summary>
		/// Maximum length in octets (bytes) of the column, if the type of the column is character or binary. Zero for all other types of columns.
		/// </summary>
		System.Int32 CharacterOctetLength { get; }

		/// <summary>
		/// If the column's data type is of a numeric data type other than VARNUMERIC, this is the maximum precision of the column. 
		/// The precision of columns with a data type of DBTYPE_DECIMAL or DBTYPE_NUMERIC depends on the definition of the column. 
		/// For the precision of all other numeric data types, see (http://msdn.microsoft.com/library/en-us/oledb/htm/olprappendixa_2.asp) "Precision of Numeric Data Types" in Appendix A.
		/// If the column's data type is not numeric or is VARNUMERIC, this is zero.
		/// </summary>
		System.Int32 NumericPrecision { get; }

		/// <summary>
		/// If the column's type indicator is DBTYPE_DECIMAL, DBTYPE_NUMERIC, or DBTYPE_VARNUMERIC, 
		/// this is the number of digits to the right of the decimal point. Otherwise, this is zero.
		/// </summary>
		System.Int32 NumericScale { get; }

		/// <summary>
		/// Datetime precision (number of digits in the fractional seconds portion) of the column if the column is a datetime or interval type. If the column's data type is not datetime, this is zero.
		/// </summary>
		System.Int32 DateTimePrecision { get; }

		/// <summary>
		/// Catalog name in which the character set is defined. Blank if the provider does not support catalogs or different character sets.
		/// </summary>
		string CharacterSetCatalog { get; }

		/// <summary>
		/// Unqualified schema name in which the character set is defined. Blank if the provider does not support schemas or different character sets.
		/// </summary>
		string CharacterSetSchema { get; }

		/// <summary>
		/// Character set name. Blank if the provider does not support different character sets.
		/// </summary>
		string CharacterSetName { get; }

		/// <summary>
		/// Catalog name in which the domain is defined. Blank if the provider does not support catalogs or domains.
		/// </summary>
		string DomainCatalog { get; }

		/// <summary>
		/// Unqualified schema name in which the domain is defined. Blank if the provider does not support schemas or domains.
		/// </summary>
		string DomainSchema { get; }

		/// <summary>
		/// Domain name. Blank if the provider does not support domains.
		/// </summary>
		string DomainName { get; }

		/// <summary>
		/// Human-readable description of the column. For example, the description for a column named Name in the Employee table might be "Employee name." 
		/// Blank if this column is not supported by the provider, or if there is no description associated with the column.
		/// </summary>
		string Description { get; }

		/// <summary>
		/// 
		/// </summary>
		System.Int32 LCID { get; }

		/// <summary>
		/// 
		/// </summary>
		System.Int32 CompFlags { get; }

		/// <summary>
		/// 
		/// </summary>
		System.Int32 SortID { get; }

		/// <summary>
		/// 
		/// </summary>
		System.Byte[] TDSCollation { get; }

		/// <summary>
		/// 'True' if this Column is a computed column and/or derived from a formula. Typically, this means don't try and update.  'False' it is either not computed or unknowable.
		/// </summary>
        System.Boolean IsComputed { get; }

		/// <summary>
		/// 'True' if this column is part of all of the tables primary key, 'False' if not.
		/// </summary>
        System.Boolean IsInPrimaryKey { get; }

		/// <summary>
		/// 'True' if this column is an auto-incremented value.  
		/// </summary>
        System.Boolean IsAutoKey { get; set;  }

		/// <summary>
		/// 'True' if this Column is involved in a least one ForeignKey.  
		/// </summary>
		System.Boolean IsInForeignKey { get; }

		/// <summary>
		/// The starting value of the AutoKey (Identity, Sequence, or Generator) associated with this Column.
		/// </summary>
        System.Int32 AutoKeySeed { get; }

		/// <summary>
		/// Returns the incrementing value associated with the AutoKey. 
		/// </summary>
		System.Int32 AutoKeyIncrement { get; }

        /// <summary>
        /// Contains the CustomAutoKey text, for example, the sequence name
        /// </summary>
        System.String AutoKeyText { get; set; }

		/// <summary>
		/// True if this Column has a domain
		/// </summary>
        System.Boolean HasDomain { get; }

        /// <summary>
        /// Exlude this column from the code generation process
        /// </summary>
        System.Boolean Exclude { get; }

        /// <summary>
        /// True if this column is the Native type for a Databases Concurrency mechanism
        /// </summary>
        System.Boolean IsConcurrency { get;}

        /// <summary>
        /// If true then use this column for EntitySpaces Multiprovider concurrency checking, column must be an integer type
        /// </summary>
        System.Boolean IsEntitySpacesConcurrency { get; set; }

        // DateAdded
        System.Boolean IsDateAddedColumn { get; }

        esSettingsDriverInfo.DateType DateAddedType { get; }

        System.String DateAddedServerSideText { get; }

        esSettingsDriverInfo.ClientType DateAddedClientType { get; }

        // DateModified
        System.Boolean IsDateModifiedColumn { get; }

        esSettingsDriverInfo.DateType DateModifiedType { get; }

        System.String DateModifiedServerSideText { get; }

        esSettingsDriverInfo.ClientType DateModifiedClientType { get; }

        // AddedBy
        System.Boolean IsAddedByColumn { get; }

        string AddedByServerSideText { get; }

        System.Boolean UseAddedByEventHandler { get; }

        // ModifiedBy
        System.Boolean IsModifiedByColumn { get; }

        string ModifiedByServerSideText { get; }

        System.Boolean UseModifiedByEventHandler { get; }

        /// <summary>
        /// Fetch any database specific meta data through this generic interface by key. The keys will have to be defined by the specific database provider
        /// </summary>
        /// <param name="key">A key identifying the type of meta data desired.</param>
        /// <returns>A meta-data object or collection.</returns>
        object DatabaseSpecificMetaData(string key);

        #region Code Generation Properties

        string PropertyName { get; }

        string CSharpToSystemType { get; }

        string VBToSystemType { get; }

        string esSystemType { get; }

        string ParameterName { get; }

        System.Boolean IsArrayType { get; }

        System.Boolean IsObjectType { get; }

        System.Boolean IsNullableType { get; }

        string NullableType { get; }

        string NullableTypeVB { get; }

        string SetRowAccessor { get; }

        string GetRowAccessor { get; }

        #endregion 
    }
}

