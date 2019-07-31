using System;
using System.Collections;

namespace EntitySpaces.MetadataEngine
{
	/// <summary>
	/// This is a MyMeta Collection. The only two methods meant for public consumption are Count and Item.
	/// </summary>
	public interface IDomains : IList, IEnumerable	
	{
		// User Meta Data
		string UserDataXPath { get; }

		/// <summary>
		/// You access items in the collect using this method. The return is the object in the collection.
		/// </summary>
		/// <param name="index">Either an integer or a string.
		/// </param>
		IDomain this[object index] { get; }

		// ICollection
		/// <summary>
		/// ICollection support. Not implemented.
		/// </summary>
		new bool IsSynchronized { get; }

		/// <summary>
		/// The number of items in the collection
		/// </summary>
		new int Count { get; }

		/// <summary>
		/// ICollection support. Not implemented.
		/// </summary>
		new void CopyTo(System.Array array, int index);

		/// <summary>
		/// ICollection support. Not implemented.
		/// </summary>
		new object SyncRoot { get; }

		// IEnumerable
		/// <summary>
		/// Used to support 'foreach' sytax. Do not call this directly.
		/// </summary>
		new IEnumerator GetEnumerator();
	}
}

