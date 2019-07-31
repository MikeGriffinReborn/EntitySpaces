using System;


namespace EntitySpaces.MetadataEngine
{
	/// <summary>
	/// Summary description for IDomain.
	/// </summary>
	public interface IDomain
	{
		/// <summary>
		/// The Properties (Local) for this domain. These are user defined and are typically stored in 'UserMetaData.xml' unless changed in the Default Settings dialog.
		/// Properties consist of key/value pairs.  You can populate this collection during your script or via the Dockable window.
		/// To save any data added to this collection call esMetadataEngine.SaveUserMetaData(). See <see cref="IProperty"/>
		/// </summary>
		IPropertyCollection Properties { get; }

		// User Meta Data
		string UserDataXPath { get; }

		/// <summary>
		/// You can override the physical name of the Domain. If you do not provide an Alias the value of 'Domain.Name' is returned.
		/// If your domain in your DBMS is 'TXT_FIRST_NAME' you might want to give it an Alias of 'FirstName' so that your business object property will be a nice name.
		/// You can provide an Alias the User Meta Data window. You can also set this during a script and then call esMetadataEngine.SaveUserMetaData().
		/// See <see cref="Name"/>
		/// </summary>
		string Alias { get; set; }
	
		/// <summary>
		/// This is the physical domain name as stored in your DBMS system. See <see cref="Alias"/>
		/// </summary>
		string Name { get; }	
	

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
		/// Anytime that you need to expose this domains value to your programming language use this value.
		/// See <see cref="DbTargetType"/>
		/// </summary>
		string LanguageType { get; }

		/// <summary>
		/// The Domain allows null in your DBMS system.
		/// </summary>
		System.Boolean IsNullable { get; }

		/// <summary>
		/// If 'True' the domain has a default value. If 'False' the domain does not have a default value, or it is unknown whether the domain has a default value.
		/// </summary>
		System.Boolean HasDefault { get; }

		/// <summary>
		/// The actual unparsed default from your DBMS system, for instance in SQL server it might be 'getdate()' for a DateTime field or N'USA' for Country.
		/// </summary>
		string Default { get; }

		/// <summary>
		/// The maximum possible length of a value in the domain. For character, binary, or bit domains, this is one of the following: 
		/// <list type="bullet">
		/// <item>
		/// The maximum length of the domain in characters, bytes, or bits, respectively, if one is defined. For example, a CHAR(5) domain in an SQL table has a maximum length of 5. 
		/// </item>
		/// <item>
		/// The maximum length of the data type in characters, bytes, or bits, respectively, if the domain does not have a defined length. 
		/// </item>
		/// <item>
		/// Zero (0) if neither the domain nor the data type has a defined maximum length.
		/// </item>
		/// </list>
		/// </summary>
		System.Int32 CharacterMaxLength { get; }

		/// <summary>
		/// Maximum length in octets (bytes) of the domain, if the type of the domain is character or binary. Zero for all other types of domains.
		/// </summary>
		System.Int32 CharacterOctetLength { get; }

		/// <summary>
		/// If the domain's data type is of a numeric data type other than VARNUMERIC, this is the maximum precision of the domain. 
		/// The precision of domains with a data type of DBTYPE_DECIMAL or DBTYPE_NUMERIC depends on the definition of the domain. 
		/// For the precision of all other numeric data types, see (http://msdn.microsoft.com/library/en-us/oledb/htm/olprappendixa_2.asp) "Precision of Numeric Data Types" in Appendix A.
		/// If the domain's data type is not numeric or is VARNUMERIC, this is zero.
		/// </summary>
		System.Int32 NumericPrecision { get; }

		/// <summary>
		/// If the domain's type indicator is DBTYPE_DECIMAL, DBTYPE_NUMERIC, or DBTYPE_VARNUMERIC, 
		/// this is the number of digits to the right of the decimal point. Otherwise, this is zero.
		/// </summary>
		System.Int32 NumericScale { get; }

		/// <summary>
		/// Datetime precision (number of digits in the fractional seconds portion) of the domain if the domain is a datetime or interval type. If the domain's data type is not datetime, this is zero.
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
        /// Fetch any database specific meta data through this generic interface by key. The keys will have to be defined by the specific database provider
        /// </summary>
        /// <param name="key">A key identifying the type of meta data desired.</param>
        /// <returns>A meta-data object or collection.</returns>
        object DatabaseSpecificMetaData(string key);
	}
}

