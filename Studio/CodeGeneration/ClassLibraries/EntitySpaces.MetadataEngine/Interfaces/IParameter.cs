using System;

namespace EntitySpaces.MetadataEngine
{	
	/// <summary>
	/// ParamDirection is a property on <see cref="IParameter"/>
	/// </summary>
	public enum ParamDirection
	{
		/// <summary>
		/// The direction of the parameter is unknown.
		/// </summary>
		Unknown,
		/// <summary>
		/// The parameter is an input parameter
		/// </summary>
		Input,
		/// <summary>
		/// The parameter is an input/output parameter
		/// </summary>
		InputOutput, 
		/// <summary>
		/// The parameter is an output parameter
		/// </summary>
		Output, 
		/// <summary>
		/// The parameter is a procedure return value. For example, in the following ODBC SQL statement to call a procedure, the question mark marks a procedure return value:
		///	{? = call GetNextOrderID}
		/// </summary>
		ReturnValue
	}

	/// <summary>
	/// IParameter represents a parameter of an IProcedure in your DBMS.
	/// </summary>
	/// <remarks>
	///	IParameter Collections:
	/// <list type="table">
	///		<item><term>Properties</term><description>A collection that can hold key/value pairs of your choosing</description></item>
	///	</list>
	/// </remarks>
	/// <example>
	/// VBScript
	/// <code>
	/// Dim objParameter
	/// For Each objParameter in objProcedure.Parameters
	///     output.writeLn objParameter.Name
	///	    output.writeLn objParameter.Alias
	/// Next
	/// </code>
	/// JScript
	/// <code>
	/// var objParameter;
	/// for (var j = 0; j &lt; objProcedure.Parameters; j++) 
	/// {
	///	    objParameter = objProcedure.Parameters.Item(j);
	///	    
	///	    output.writeln(objParameter.Name);
	///	    output.writeln(objParameter.Alias);
	/// }
	/// </code>
	/// </example>
	public interface IParameter
	{
		/// <summary>
		/// The Properties for this parameter. These are user defined and are typically stored in 'UserMetaData.xml' unless changed in the Default Settings dialog.
		/// Properties consist of key/value pairs.  You can populate this collection during your script or via the Dockable window.
		/// To save any data added to this collection call esMetadataEngine.SaveUserMetaData(). See <see cref="IProperty"/>
		/// </summary>
		IPropertyCollection Properties { get; }

		// User Meta Data
		string UserDataXPath { get; }

		/// <summary>
		/// You can override the physical name of the Table. If you do not provide an Alias the value of 'Table.Name' is returned.
		/// If your table in your DBMS is 'Q99AAB' you might want to give it an Alias of 'Employees' so that your business object names will make sense.
		/// You can provide an Alias the User Meta Data window. You can also set this during a script and then call esMetadataEngine.SaveUserMetaData().
		/// See <see cref="Name"/>
		/// </summary>
		string Alias { get; set; }
	
		/// <summary>
		/// This is the physical table name as stored in your DBMS system. See <see cref="Alias"/>
		/// </summary>
		string Name { get; }

		/// <summary>
		/// If the parameter is an input, input/output, or output parameter, this is the one-based ordinal position of the parameter in the procedure call. 
		/// If the parameter is the return value, this is zero.
		/// </summary>
		System.Int32 Ordinal { get; }

		/// <summary>
		/// N/A
		/// </summary>
		System.Int32 ParameterType { get; }

		/// <summary>
		/// True if the parameter has a default value, False if not.
		/// </summary>
		System.Boolean HasDefault { get; }

		/// <summary>
		/// The unparsed DBMS default value.  If the default is a null then "&lt;null&gt;" is the value.
		/// </summary>
		string Default { get; }

		/// <summary>
		/// True if the parameter is nullable, False if not.
		/// </summary>
		System.Boolean IsNullable { get; }

		/// <summary>
		/// 
		/// </summary>
		System.Int32 DataType { get; }

		/// <summary>
		/// This is the full data type name, whereas the DataType property might be 'nvarchar' the DataTypeName property would be 'nvarchar(200)', this varies from DBMS to DBMS.
		/// See <see cref="TypeName"/>
		/// </summary>
		string DataTypeNameComplete { get; }

		/// <summary>
		/// The Language Mappings window is where these are entered and they are stored in 'Languages.xml'. 
		/// If your DMBS system is Microsoft SQL and your language is C# then nvarchar will be mapped to a C# 'string'. 
		/// Anytime that you need to expose this Parameters value to your programming language use this value.
		/// See <see cref="DbTargetType"/>
		/// </summary>
		string LanguageType { get; }

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
		/// Human-readable description of the parameter. 
		/// For example, the description of a parameter named Name in a procedure that adds a new employee might be "Employee name."
		/// </summary>
		string Description { get; }

		/// <summary>
		/// The native data type as stored in your DBMS system, for instance a SQL 'nvarchar', or Access 'Memo'.
		/// </summary>
		string TypeName { get; }

		/// <summary>
		/// Localized version of 'TypeName'. Blank is returned if a localized name is not supported by the data provider.
		/// </summary>
		string LocalTypeName { get; }

		/// <summary>
		/// See the ParamDirection Enumeration.
		/// </summary>
        ParamDirection Direction { get; }

        /// <summary>
        /// Fetch any database specific meta data through this generic interface by key. The keys will have to be defined by the specific database provider
        /// </summary>
        /// <param name="key">A key identifying the type of meta data desired.</param>
        /// <returns>A meta-data object or collection.</returns>
        object DatabaseSpecificMetaData(string key);
	}
}

