using System;
using System.Data;
using System.Collections;
using System.Globalization;

namespace Provider.VistaDB
{
	/// <summary>
	/// Used Internally. 
	/// </summary>
	public class VistaDBParameterEnumerator: MarshalByRefObject, IEnumerator
	{

		private ArrayList parameters;
		private int currentIdx;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="prms">Parameter list</param>
		public VistaDBParameterEnumerator(ArrayList prms)
		{
			parameters = prms;
			currentIdx = -1;
		}

		object IEnumerator.Current
		{
			get
			{
				return (VistaDBParameter)(parameters[currentIdx]);
			}
		}

		/// <summary>
		/// Gets the current element in the collection.
		/// </summary>
		public VistaDBParameter Current
		{
			get
			{
				return (VistaDBParameter)parameters[currentIdx];
			}
		}

		/// <summary>
		/// Advances the enumerator to the next element of the collection.
		/// </summary>
		/// <returns>True if the enumerator was successfully advanced to the next element; False if the enumerator has passed the end of the collection</returns>
		public bool MoveNext()//IEnumerator.MoveNext
		{
			currentIdx++;
			return (currentIdx < parameters.Count);
		}

		/// <summary>
		/// Sets the enumerator to its initial position, which is before the first element in the collection.
		/// </summary>
		public void Reset()//IEnumerator.Reset
		{
			currentIdx = -1;
		}
	}
}
