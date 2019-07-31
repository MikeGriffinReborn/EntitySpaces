using System;
using System.Xml;
using System.Collections;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	/// <summary>
	/// Summary description for Enumerator.
	/// </summary>
	public class Enumerator : IEnumerator
	{
		public Enumerator(ArrayList array)
		{
			this._array = array;
		}

		#region IEnumerator Members

		public void Reset()
		{
			_index = -1;
		}

		public object Current
		{
			get
			{
				object obj = null;

				if(_index >= 0 && _index < this._array.Count)
				{
					return _array[_index];
				}

				return obj;
			}
		}

		public bool MoveNext()
		{
			if(++_index < this._array.Count)
				return true;
			else
				return false;
		}

		private int _index = -1;

		#endregion

		protected ArrayList _array = null;
	}
}
