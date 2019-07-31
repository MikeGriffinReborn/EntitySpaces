using System;
using System.Collections;

namespace EntitySpaces.MetadataEngine
{
	public class KeyValuePair : IProperty
	{
		private string key = string.Empty;
		private string val = string.Empty;

		public KeyValuePair(string key, string val) 
		{
			this.key = key;
			this.val = val;
		}

		public string Key
		{
			get { return key; }
		}

		public string Value
		{
			get { return val; }
			set { val = value; }
		}

		public bool IsGlobal
		{
			get { return true; }
		}
	}


	/// <summary>
	/// Summary description for KeyValueCollection.
	/// </summary>
	public class KeyValueCollection : IPropertyCollection, IEnumerable, ICollection, IEnumerator
	{
		IEnumerator enumerator;
		Hashtable hash = new Hashtable();

		public KeyValueCollection() {}

		public IProperty this[string key]
		{
			get
			{
				return hash[key] as IProperty;
			}
		}

		public IProperty AddKeyValue(string key, string value)
		{
			KeyValuePair pair = new KeyValuePair(key, value);
			hash[key] = pair;
			return pair;
		}

		public void RemoveKey(string key)
		{
			hash.Remove(key);
		}

		public bool ContainsKey(string key)
		{
			return hash.ContainsKey(key);
		}

		public void Clear()
		{
			hash.Clear();
		}

		
		public void Reset()
		{
			enumerator = null;
		}

		public object Current
		{
			get
			{
				if (enumerator == null) enumerator = this.GetEnumerator();
				IProperty entry =  (IProperty)enumerator.Current;
				return entry; 
			}
		}

		public bool MoveNext()
		{
			if (enumerator == null) enumerator = this.GetEnumerator();
			return enumerator.MoveNext();
		}

		public bool IsSynchronized
		{
			get { return hash.Values.IsSynchronized; }
		}

		public int Count
		{
			get { return hash.Count; }
		}

		public void CopyTo(Array array, int index)
		{
			hash.Values.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get { return hash.Values.SyncRoot; }
		}

		public System.Collections.IEnumerator GetEnumerator()
		{
			return hash.Values.GetEnumerator();
		}
	}
}
