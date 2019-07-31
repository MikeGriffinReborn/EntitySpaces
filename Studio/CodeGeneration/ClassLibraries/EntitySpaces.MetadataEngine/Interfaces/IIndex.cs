using System;

namespace EntitySpaces.MetadataEngine
{
	/// <summary>
	/// IIndex represents an index on a table in your DBMS.
	/// </summary>
	/// <remarks>
	///	IIndex Collections:
	/// <list type="table">
	///		<item><term>Columns</term><description>A collection of columns that make up this Index</description></item>
	///		<item><term>Properties</term><description>A collection that can hold key/value pairs of your choosing</description></item>
	///	</list>
	/// </remarks>
	/// <example>
	/// VBScript
	/// <code>
	/// Dim objIndex
	/// For Each objIndex in objTable.Indexes
	///     output.writeLn objIndex.Name
	///	    output.writeLn objIndex.Alias
	/// Next
	/// </code>
	/// JScript
	/// <code>
	/// var objIndex;
	/// for (var j = 0; j &lt; objTable.Indexes; j++) 
	/// {
	///	    objIndex = objTable.Indexes.Item(j);
	///	    
	///	    output.writeln(objIndex.Name);
	///	    output.writeln(objIndex.Alias);
	/// }
	/// </code>
	/// </example>
	public interface IIndex
	{
		// Objects
		/// <summary>
		/// The parent table of the index.
		/// </summary>
		ITable Table { get; }

		// Collections
		/// <summary>
		/// This is a columns collection that contains the columns that make up the index.
		/// </summary>
		IColumns Columns{ get; }

		/// <summary>
		/// The Properties for this index. These are user defined and are typically stored in 'UserMetaData.xml' unless changed in the Default Settings dialog.
		/// Properties consist of key/value pairs.  You can populate this collection during your script or via the Dockable window.
		/// To save any data added to this collection call esMetadataEngine.SaveUserMetaData(). See <see cref="IProperty"/>
		/// </summary>
		IPropertyCollection Properties { get; }

		// User Meta Data
		string UserDataXPath { get; }

		// Properties
		/// <summary>
		/// You can override the physical name of the index. If you do not provide an Alias the value of 'Index.Name' is returned.
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
		/// This is the schema of the Index.
		/// </summary>
		string Schema { get; }


		/// <summary>
		/// True if the index keys must be unique, False if duplicate keys are allowed.
		/// </summary>
		System.Boolean Unique{ get; }

		/// <summary>
		/// <list type="bullet">
		/// <item>True - if the leaf nodes of the index contain full rows, not bookmarks. This is a way to represent a table clustered by key value.</item>
		/// <item>False - if the leaf nodes of the index contain bookmarks of the base table rows whose key value matches the key value of the index entry.</item>
		/// </list>
		/// </summary>
		System.Boolean Clustered{ get; }

		/// <summary>
		/// The type of the index. One of the following:
		/// <list type="table">
		///		<item><term>BTREE</term><description>The index is a B+-tree</description></item>
		///		<item><term>HASH</term><description>The index is a hash file using, for example, linear or extensible hashing</description></item>		
		///		<item><term>CONTENT</term><description>The index is a content index</description></item>
		///		<item><term>OTHER</term><description>The index is some other type of index</description></item>	
		///	</list>
		/// </summary>
		string Type{ get; }

		/// <summary>
		/// For a B+-tree index, this property represents the storage utilization factor of page nodes during the creation of the index. 
		/// The value is an integer from 0 to 100 representing the percentage of use of an index node. 
		/// For a linear hash index, this property represents the storage utilization of the entire hash structure 
		/// (the ratio of used area to total allocated area) before a file structure expansion occurs.
		/// </summary>
		System.Int32 FillFactor{ get; }

		/// <summary>
		/// The total amount of bytes allocated to this structure at creation time.
		/// </summary>
		System.Int32 InitialSize{ get; }

		/// <summary>
		/// True if the index sorts repeated keys by bookmark, False if the index does not sort repeated keys by bookmark.
		/// </summary>
		System.Boolean SortBookmarks{ get; }

		/// <summary>
		/// Whether the index is maintained automatically when changes are made to the corresponding base table. One of the following: 
		/// <list type="bullet">
		/// <item>True - The index is automatically maintained.</item>
		/// <item>False - The index must be maintained by the consumer through explicit calls.</item>
		/// </list> 
		/// </summary>
		System.Boolean AutoUpdate{ get; }

		/// <summary>
		/// How NULLs are collated in the index. One of the following 
		/// <list type="table">
		///		<item><term>END</term><description>NULLs are collated at the end of the list, regardless of the collation order</description></item>
		///		<item><term>START</term><description>NULLs are collated at the start of the list, regardless of the collation order</description></item>		
		///		<item><term>HIGH</term><description>NULLs are collated at the high end of the list</description></item>
		///		<item><term>LOW</term><description>NULLs are collated at the low end of the list</description></item>	
		///	</list>
		/// </summary>
		string NullCollation{ get; }

		/// <summary>
		/// One of the following: 
		/// <list type="table">
		///		<item><term>ASCENDING</term><description>The sort sequence for the column is ascending</description></item>
		///		<item><term>DESCENDING</term><description>The sort sequence for the column is descending</description></item>		
		///	</list>
		/// </summary>
		string Collation{ get; }

		/// <summary>
		/// Number of unique values in the index.
		/// </summary>
		Decimal Cardinality{ get; }

		/// <summary>
		/// Number of pages used to store the index.
		/// </summary>
		System.Int32 Pages{ get; }

		/// <summary>
		/// The WHERE clause identifying the filtering restriction.
		/// </summary>
		string FilterCondition{ get; }

		/// <summary>
		/// Whether the index is integrated. That is, all base table columns are available from the index. One of the following:
		/// <list type="bullet">
		/// <item>True - The index is integrated. For clustered indexes this value will always be True.</item>
		/// <item>False - The index is not integrated.</item>
		/// </list> ///  
		/// </summary>
        System.Boolean Integrated { get; }

        /// <summary>
        /// Fetch any database specific meta data through this generic interface by key. The keys will have to be defined by the specific database provider
        /// </summary>
        /// <param name="key">A key identifying the type of meta data desired.</param>
        /// <returns>A meta-data object or collection.</returns>
        object DatabaseSpecificMetaData(string key);
	}
}

