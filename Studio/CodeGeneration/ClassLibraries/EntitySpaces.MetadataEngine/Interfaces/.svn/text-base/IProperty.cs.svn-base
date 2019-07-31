using System;
using System.Collections;

namespace EntitySpaces.MetadataEngine
{
	/// <summary>
	/// IProperty represents a key/value pair within an PropertyCollection.
	/// </summary>
	/// <remarks>
	/// There is a property collection on every entity in your database, you can add key/value
	/// pairs to the User Meta Data either through the user interface of MyGeneration or 
	/// programmatically in your scripts.  User meta data is stored in XML and never writes to your database.
	///
	/// This can be very useful, you might need more meta data than MyMeta supplies, in fact,
	/// MyMeta will eventually offer extended meta data using this feature as well. The current plan
	/// is that any extended data added via MyGeneration will have a key that beings with "esMetadataEngine.Something"
	/// where 'Something' equals the description.
	/// </remarks>
	/// <example>
	///	This VBScript loops though a table of your choosing and adds 6 key/value pairs to the User Meta Data on every column in table.
	/// <code>
	/// '===============================================================================
	/// ' Loop through each column in the selected table and assign it some meta data
	/// '===============================================================================
	/// For Each objColumn in objTable.Columns
	///     objColumn.Properties.AddKeyValue objColumn.Name, "The Name of this Column is " + objColumn.Name
	///     For i = 1 to 5
	///         objColumn.Properties.AddKeyValue i, "This is property " + i
	///     Next
	/// Next
	///
	/// '===============================================================================
	/// ' Loop through each column and print the meta data
	/// '===============================================================================
	/// For Each objColumn in objTable.Columns
	///     Set prop = objColumn.Properties.Item(objColumn.Name)
	///     output.writeLn prop.Key + " = " + prop.Value
	/// Next
	/// </code>
	///	This JScript loops though a table of your choosing and adds 6 key/value pairs to the User Meta Data on every column in table.
	///	<code>
	/// //===============================================================================
	/// // Loop through each column in the selected table and assign it some meta data
	/// //===============================================================================
	/// for (var i = 0; i &lt; objTable.Columns.Count; i++)
	/// {
	///     objColumn = objTable.Columns.Item(i);
	///     objColumn.Properties.AddKeyValue(objColumn.Name, "The Name of this Column is " + objColumn.Name);
	///
	///     for (var j = 0; j &lt; 5; j++)
	///     {
	///         objColumn.Properties.AddKeyValue(j, "This is property " + j);
	///     }
	/// }
	///
	/// //===============================================================================
	/// // Loop through each column and print the meta data
	/// //===============================================================================
	/// for (var i = 0; i &lt; objTable.Columns.Count; i++)
	/// {
	///	    objColumn = objTable.Columns.Item(i);
	///
	///	    var prop = objColumn.Properties.Item(objColumn.Name);
	///     output.writeLn(prop.Key + " = " + prop.Value);
	/// }
	/// </code>
	/// </example>
	public interface IProperty
	{
		/// <summary>
		/// The Key, unique within the property collection. This can be any value of your choosing.
		/// </summary>
		string Key { get; }

		/// <summary>
		/// The Value, this is the information you store in the <see cref="IPropertyCollection"/>, use the Key as a tag.
		/// </summary>
        string Value { get; set; }

		/// <summary>
		/// True if this property comes from the Global User Meta Data, false if not.
		/// </summary>
        bool IsGlobal { get; }
	}
}

