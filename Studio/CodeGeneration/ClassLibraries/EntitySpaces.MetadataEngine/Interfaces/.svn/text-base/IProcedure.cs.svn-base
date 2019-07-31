using System;

namespace EntitySpaces.MetadataEngine
{
	/// <summary>
	/// IProcedure represents a view or function in your DBMS.
	/// </summary>
	/// <remarks>
	///	IProcedure Collections:
	/// <list type="table">
	///		<item><term>Parameters</term><description>Contains all of the parameters for the procedure</description></item>
	///		<item><term>ResultColumns</term><description>Contains an IResultColumn for each column return by the procedure</description></item>
	///		<item><term>Properties</term><description>A collection that can hold key/value pairs of your choosing</description></item>
	///	</list>	
	/// </remarks>
	/// <example>
	/// VBScript
	/// <code>
	/// output.writeLn "This Procedure takes   " + objProcedure.Parameters.Count    + " Parameters"
	/// output.writeLn "This Procedure returns " + objProcedure.ResultColumns.Count + " ResultColumns"
	/// </code>
	/// JScript
	/// <code>
	/// output.writeln("This Procedure takes   " + objProcedure.Parameters.Count    + " Parameters");
	/// output.writeln("This Procedure returns " + objProcedure.ResultColumns.Count + " ResultColumns");
	/// </code>
	/// </example>
	public interface IProcedure
	{
		// Collections
		/// <summary>
		/// Contains all of the parameters for the procedure.
		/// </summary>
		IParameters Parameters { get; }

		/// <summary>
		/// Contains an IResultColumn for each column return by the procedure.
		/// </summary>
		IResultColumns ResultColumns { get; }

		/// <summary>
		/// The Properties for this procedure. These are user defined and are typically stored in 'UserMetaData.xml' unless changed in the Default Settings dialog.
		/// Properties consist of key/value pairs.  You can populate this collection during your script or via the Dockable window.
		/// To save any data added to this collection call esMetadataEngine.SaveUserMetaData(). See <see cref="IProperty"/>
		/// </summary>
		IPropertyCollection Properties { get; }

		// Objects
		/// <summary>
		/// The parent database of the procedure.
		/// </summary>
		IDatabase Database { get; }

		// User Meta Data
		string UserDataXPath { get; }

		/// <summary>
		/// You can override the physical name of the Procedure. If you do not provide an Alias the value of 'Procedure.Name' is returned.
		/// If your Procedure in your DBMS is 'Q99AAB' you might want to give it an Alias of 'Employees' so that your business object names will make sense.
		/// You can provide an Alias the User Meta Data window. You can also set this during a script and then call esMetadataEngine.SaveUserMetaData().
		/// See <see cref="Name"/>
		/// </summary>
		string Alias { get; set; }
	
		/// <summary>
		/// This is the physical table name as stored in your DBMS system. See <see cref="Alias"/>
		/// </summary>
		string Name { get; }

		/// <summary>
		/// This is the schema of the Procedure.
		/// </summary>
		string Schema { get; }

		/// <summary>
		/// N/A
		/// </summary>
		System.Int16 Type { get; }

		/// <summary>
		/// Procedure definition or blank if not provided.
		/// </summary>
		string ProcedureText { get; }

		/// <summary>
		/// Human-readable description of the procedure.
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Date when the procedure was created or '1/1/0001' if the provider does not have this information. 
		/// </summary>
		DateTime DateCreated { get; }

		/// <summary>
		/// Date when the procedure definition was last modified or '1/1/0001' if the provider does not have this information. 
		/// </summary>
        DateTime DateModified { get; }

        /// <summary>
        /// Fetch any database specific meta data through this generic interface by key. The keys will have to be defined by the specific database provider
        /// </summary>
        /// <param name="key">A key identifying the type of meta data desired.</param>
        /// <returns>A meta-data object or collection.</returns>
        object DatabaseSpecificMetaData(string key);
	}
}

