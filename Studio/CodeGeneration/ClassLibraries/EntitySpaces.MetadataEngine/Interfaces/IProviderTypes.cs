using System;
using System.Collections;

namespace EntitySpaces.MetadataEngine
{
	public interface IProviderTypes
	{
		IProviderType this[object index] { get; }

		// ICollection
		bool IsSynchronized { get; }
		int Count { get; }
		void CopyTo(System.Array array, int index);
		object SyncRoot { get; }

		// IEnumerable
		IEnumerator GetEnumerator();
	}
}


