using System;

namespace EntitySpaces.MetadataEngine
{
	/// <summary>
	/// IForeignKey represents an foreign key on a table in your DBMS.
	/// </summary>
	/// <remarks>
	///	IForeignKey Collections:
	/// <list type="table">
	///		<item><term>ForeignColumns</term><description>A collection of columns that are in the foreign table that make up the key</description></item>
	///		<item><term>PrimaryColumns</term><description>A collection of columns that are in the parent table that make up the key</description></item>
	///		<item><term>Properties</term><description>A collection that can hold key/value pairs of your choosing</description></item>	
	///	</list>
	/// </remarks>
	/// <example>
	/// VBScript
	/// <code>
	/// Dim objForeignKey
	/// For Each objForeignKey in objTable.ForeignKeys
	///     output.writeLn objForeignKey.Name
	///	    output.writeLn objForeignKey.Alias
	/// Next
	/// </code>
	/// JScript
	/// <code>
	/// var objForeignKey;
	/// for (var j = 0; j &lt; objTable.ForeignKeys; j++) 
	/// {
	///	    objColumn = objTable.ForeignKeys.Item(j);
	///	    
	///	    output.writeln(objForeignKey.Name);
	///	    output.writeln(objForeignKey.Alias);
	/// }
	/// </code>
	/// </example>
	public interface IForeignKey
	{
		/// <summary>
		/// You can override the physical name of the foreign key. If you do not provide an Alias the value of 'ForeignKey.Name' is returned.
		/// If your foreign key in your DBMS is 'TXT_FIRST_NAME' you might want to give it an Alias of 'FirstName' so that your business object property will be a nice name.
		/// You can provide an Alias the User Meta Data window. You can also set this during a script and then call esMetadataEngine.SaveUserMetaData().
		/// See <see cref="Name"/>
		/// </summary>
		string Alias { get; set; }
	
		/// <summary>
		/// This is the physical name of the primary key name as stored in your DBMS system. See <see cref="Alias"/>
		/// </summary>
		string Name { get; }

		// Objects
		/// <summary>
		/// The parent key table of this foreign key
		/// </summary>
		ITable PrimaryTable { get; }

        // Objects
        /// <summary>
        /// The parent key table's 'Name' of this foreign key
        /// </summary>
        string PrimaryTableName { get; }

        // Objects
        /// <summary>
        /// The parent key table's 'Alias' of this foreign key
        /// </summary>
        string PrimaryTableNameAlias { get; }

        /// <summary>
        /// The foreign key table of this foreign key
        /// </summary>
        ITable ForeignTable { get; }

        /// <summary>
        /// The foreign key table's 'Name' of this foreign key
        /// </summary>
        string ForeignTableName { get; }

        /// <summary>
        /// The foreign key table's 'Alias' of this foreign key
        /// </summary>
        string ForeignTableNameAlias { get; }

        // Collections
        /// <summary>
        /// A collection of columns that are in the foreign table (not in this table) that make up the key
        /// </summary>
        IColumns ForeignColumns { get; }

		/// <summary>
		/// A collection of columns that are in the parent table that make up the key
		/// </summary>
		IColumns PrimaryColumns { get; }

		/// <summary>
		/// The Properties for this ForeignKey. These are user defined and are typically stored in 'UserMetaData.xml' unless changed in the Default Settings dialog.
		/// Properties consist of key/value pairs.  You can populate this collection during your script or via the Dockable window. 
		/// To save any data added to this collection call esMetadataEngine.SaveUserMetaData(). See <see cref="IProperty"/>
		/// </summary>
		IPropertyCollection Properties { get; }

		// User Meta Data
		string UserDataXPath { get; }

		// Properties
		/// <summary>
		/// The type of the foreign key. One of the following (or blank):
		/// <list type="table">
		///		<item><term>CASCADE</term><description>A referential action of CASCADE was specified</description></item>
		///		<item><term>SET NULL</term><description>A referential action of SET NULL was specified</description></item>		
		///		<item><term>SET DEFAULT</term><description>A referential action of SET DEFAULT was specified</description></item>
		///		<item><term>NO ACTION</term><description>A referential action of NO ACTION was specified</description></item>	
		///	</list>
		/// </summary>
		string UpdateRule { get; }

		/// <summary>
		/// The type of the foreign key. One of the following (or blank):
		/// <list type="table">
		///		<item><term>CASCADE</term><description>A referential action of CASCADE was specified</description></item>
		///		<item><term>SET NULL</term><description>A referential action of SET NULL was specified</description></item>		
		///		<item><term>SET DEFAULT</term><description>A referential action of SET DEFAULT was specified</description></item>
		///		<item><term>NO ACTION</term><description>A referential action of NO ACTION was specified</description></item>	
		///	</list>
		/// </summary>
		string DeleteRule { get; }

		/// <summary>
		/// The Primary Key name
		/// </summary>
		string PrimaryKeyName { get; }

		/// <summary>
		/// The type of the foreign key. One of the following (or blank):
		/// <list type="table">
		///		<item><term>INITIALLY_DEFERRED</term><description></description></item>
		///		<item><term>INITIALLY_IMMEDIATE	</term><description></description></item>		
		///		<item><term>NOT_DEFERRABLE</term><description></description></item>
		///		<item><term>UNKNOWN</term><description></description></item>
		///	</list>
		/// </summary>
        string Deferrability { get; }

        /// <summary>
        /// Fetch any database specific meta data through this generic interface by key. The keys will have to be defined by the specific database provider
        /// </summary>
        /// <param name="key">A key identifying the type of meta data desired.</param>
        /// <returns>A meta-data object or collection.</returns>
        object DatabaseSpecificMetaData(string key);
	}
}

