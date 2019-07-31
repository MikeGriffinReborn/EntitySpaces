using System;
using System.Collections;

namespace EntitySpaces.MetadataEngine
{
	/// <summary>
	/// IPropertyCollection is a collection of user defined key/value pairs.
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
	public interface IPropertyCollection
	{
		/// <summary>
		/// This method returns an IProperty from the collection using the Key to index into the collection. An error is thrown if the key doesn't exist, see <see cref="ContainsKey"/>.
		/// </summary>
		IProperty this[string key] { get; }

		/// <summary>
		/// This method will either add or update a key value pair.  If the key already exists in the collection the value will be updated.
		/// If this key doesn't exist the key/value pair will be added.  If only want to update, and not add new items, use <see cref="ContainsKey"/> to determine if the key already exists.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IProperty AddKeyValue(string key, string value);

		/// <summary>
		/// Removes a key/value pair from the collection, no error is thrown if the key doesn't exist.
		/// </summary>
		/// <param name="key">The key of the desired key/value pair</param>
		void RemoveKey(string key);

		/// <summary>
		/// Use ContainsKey to determine if a key exists in the collection.
		/// </summary>
		/// <param name="key">The key of the desired key/value pair</param>
		/// <returns>True if the key exists, False if not</returns>
		bool ContainsKey(string key);

		/// <summary>
		/// Removes all key/value pairs from the collection.
		/// </summary>
		void Clear();

		// IEnumerator
		/// <summary>
		/// Used to support 'foreach' sytax. Do not call this directly.
		/// </summary>
		void Reset();

		/// <summary>
		/// Used to support 'foreach' sytax. Do not call this directly.
		/// </summary>
		object Current { get; }

		/// <summary>
		/// Used to support 'foreach' sytax. Do not call this directly.
		/// </summary>
		bool MoveNext();

		// ICollection
		/// <summary>
		/// ICollection support. Not implemented.
		/// </summary>
		bool IsSynchronized { get; }

		/// <summary>
		/// The number of items in the collection
		/// </summary>
		int Count { get; }

		/// <summary>
		/// ICollection support. Not implemented.
		/// </summary>
		void CopyTo(System.Array array, int index);

		/// <summary>
		/// ICollection support. Not implemented.
		/// </summary>
		object SyncRoot { get; }

		// IEnumerable
		/// <summary>
		/// Used to support 'foreach' sytax. Do not call this directly.
		/// </summary>
		IEnumerator GetEnumerator();
	}
}

