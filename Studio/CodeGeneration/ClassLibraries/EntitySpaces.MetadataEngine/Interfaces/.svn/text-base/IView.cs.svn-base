using System;


namespace EntitySpaces.MetadataEngine
{
	/// <summary>
	/// IView represents a view in your DBMS.
	/// </summary>
	/// <remarks>
	///	IView Collections:
	/// <list type="table">
	///		<item><term>Columns</term><description>Contains all of the columns for the view</description></item>
	///		<item><term>SubTables</term><description>An ITables collection that contains all the tables used directly by this view</description></item>
	///		<item><term>SubViews</term><description>An IViews collection that contains all of the views used directly by this view</description></item>
	///		<item><term>Properties</term><description>A collection that can hold key/value pairs of your choosing</description></item>
	///	</list>	
	/// </remarks>
	/// <example>
	///	VBScript
	/// <code>
	/// Dim objView
	/// For Each objView in objDatabase.Views
	///     output.writeLn objView.Name
	///	    output.writeLn objView.Alias
	/// Next
	/// </code>
	/// JScript
	/// <code>
	/// var objView;
	/// for (var j = 0; j &lt; objDatabase.Views.Count; j++) 
	/// {
	///	    objView = objDatabase.Views.Item(j);
	///	    
	///	    output.writeln(objView.Name);
	///	    output.writeln(objView.Alias);
	/// }
	/// </code>
	/// </example>
	public interface IView : ITableView
	{
		/// <summary>
		/// An ITables collection that contains all the tables used directly by this view. This can be useful for determining dependencies.
		/// </summary>
		ITables SubTables { get; }

		/// <summary>
		/// An IViews collection that contains all of the views used directly by this view. This can be useful for determining dependencies.
		/// </summary>
		IViews SubViews { get; }

		// Properties
		/// <summary>
		/// This is a query expression, Blank if not provided.
		/// </summary>
		string ViewText { get; }

		/// <summary>
		/// 'True' if  Local update checking only, 'False' if cascaded update checking (same as no CHECK OPTION specified on view definition).
		/// </summary>
		System.Boolean CheckOption { get; }

		/// <summary>
		/// ''True' if the view is updateable, 'False' if not.
		/// </summary>
		System.Boolean IsUpdateable { get; }

        #region Common ITable/IView Stuff

        // Collections

        /// <summary>
        /// The Columns collection for this table in ordinal order. See <see cref="IColumn"/>
        /// </summary>
        IColumns Columns { get; }

        /// <summary>
        /// The Properties for this table. These are user defined and are typically stored in 'UserMetaData.xml' unless changed in the Default Settings dialog.
        /// Properties consist of key/value pairs.  You can populate this collection during your script or via the Dockable window. 
        /// To save any data added to this collection call esMetadataEngine.SaveUserMetaData(). See <see cref="IProperty"/>
        /// </summary>
        IPropertyCollection Properties { get; }

        // Objects

        /// <summary>
        /// Parent Database of this Table
        /// </summary>
        IDatabase Database { get; }

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
        /// Schema.TableName
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// This is the schema of the Table.
        /// </summary>
        string Schema { get; }

        /// <summary>
        /// The table type, 'TABLE' if not provided.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        ///		<item>VIEW</item>
        ///		<item>SYSTEM VIEW</item>
        /// </list>
        ///</remarks>
        string Type { get; }

        /// <summary>
        /// Tab;e GUID. For Providers that do not use GUIDs to identify tables 'Guid.Empty' is returned.
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// Human-readable description of the table. Blank if there is no description associated with the table.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Column property ID. For Providers that do not associate PROPIDs with columns 0 is returned.
        /// </summary>
        System.Int32 PropID { get; }

        /// <summary>
        /// Date when the table was created or '1/1/0001' if the provider does not have this information. 
        /// </summary>
        DateTime DateCreated { get; }

        /// <summary>
        /// Date when the table definition was last modified or '1/1/0001' if the provider does not have this information. 
        /// </summary>
        DateTime DateModified { get; }

        /// <summary>
        /// Fetch any database specific meta data through this generic interface by key. The keys will have to be defined by the specific database provider
        /// </summary>
        /// <param name="key">A key identifying the type of meta data desired.</param>
        /// <returns>A meta-data object or collection.</returns>
        object DatabaseSpecificMetaData(string key);

        #endregion

        #region Naming Properties for Code Generation

        string Entity { get; }
        string esEntity { get; }
        string Collection { get; }
        string esCollection { get; }
        string Query { get; }
        string esQuery { get; }
        string Metadata { get; }
        string ProxyStub { get; }
        string ProxyStubCollection { get; }
        string ProxyStubQuery { get; }

        #endregion
	}
}

