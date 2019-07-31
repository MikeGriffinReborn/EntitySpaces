using System;
using System.Collections;

namespace MyMeta
{
	/// <summary>
	/// Summary description for CollectionEnumerator.
	/// </summary>
	/// 
#if ENTERPRISE
	using System.Runtime.InteropServices;
	using System.EnterpriseServices;
	[GuidAttribute("3c35ad55-7c6a-4f5f-af6f-340cf4e8b4ea"),ClassInterface(ClassInterfaceType.AutoDual)]
	public class CollectionEnumerator : ServicedComponent, IEnumerator
#else
	public class CollectionEnumerator : IEnumerator
#endif 
	{
		public CollectionEnumerator()
		{

		}

		public void Reset()
		{
			_index = 0;
		}

		// Get's the next object 
		public object Current
		{
			get
			{
				return _array[_index];
			}
		}

		public bool MoveNext()
		{
			if(++_index < _count)
				return true;
			else
				return false;
		}

		public void SetArrayList(ArrayList arrayList)
		{
			_array = new ArrayList();

			foreach(object o in arrayList)
			{
				_array.Add(o);
			}
			_count = _array.Count;
		}

		private ArrayList _array = null;
		private int _index = -1;
		private int _count = 0;
	}
}

