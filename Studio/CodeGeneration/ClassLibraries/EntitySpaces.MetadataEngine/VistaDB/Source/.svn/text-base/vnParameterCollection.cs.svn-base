using System;
using System.Data;
using System.Collections;
using System.Globalization;

namespace Provider.VistaDB
{
	/// <summary>
	/// Represents a collection of parameters relevant to a VistaDBCommand as well as their 
	/// respective mappings to columns in a DataSet. This class cannot be inherited.
	/// </summary>
	public class VistaDBParameterCollection: MarshalByRefObject, IDataParameterCollection
	{

		private ArrayList items = new ArrayList();

		object IDataParameterCollection.this[string index]
		{
			get
			{
				int num1;
				num1 = IndexOf(index);

				if(num1 >= 0)
					return (VistaDBParameter)items[num1];
	
				return null;
			}
			set
			{
				int num1;
				num1 = IndexOf(index);
				if(num1 >= 0)
					items[num1] = value;
	
			}
		}

		/// <summary>
		/// Overloaded. Gets a value indicating whether a VistaDBParameter exists in the collection.
		/// </summary>
		/// <param name="parameterName">Parameter name</param>
		/// <returns></returns>
		public bool Contains(string parameterName)//IDataParameterCollection.Contains
		{
			return (-1 != IndexOf(parameterName));
		}

		/// <summary>
		/// Overloaded. Gets the location of a VistaDBParameter in the collection.
		/// </summary>
		/// <param name="parameterName">Parameter name</param>
		/// <returns></returns>
		public int IndexOf(string parameterName)//IDataParameterCollection.IndexOf
		{
			int index = 0;

			foreach(VistaDBParameter item in items)
			{
				if(_cultureAwareCompare(item.ParameterName, parameterName) == 0)
					return index;
	
				index++;
			}
	
			return -1;
		}

		/// <summary>
		/// Overloaded. Removes the specified VistaDBParameter from the collection.
		/// </summary>
		/// <param name="parameterName">Parameter name</param>
		public void RemoveAt(string parameterName)//IDataParameterCollection.RemoveAt
		{
			RemoveAt(IndexOf(parameterName));
		}

		/// <summary>
		/// Overloaded. Adds a VistaDBParameter to the VistaDBParameterCollection.
		/// </summary>
		/// <param name="value">The VistaDBParameter to add to the collection.</param>
		/// <returns>The index of the new VistaDBParameter object.</returns>
		public int Add(object value)
		{
			return items.Add(value);
		}
		/// <summary>
		/// Overloaded. Adds a VistaDBParameter to the VistaDBParameterCollection.
		/// </summary>
		/// <param name="value">The VistaDBParameter to add to the collection. </param>
		/// <returns>VistaDBParameter</returns>
		public VistaDBParameter Add(VistaDBParameter value)
		{
			if(value.ParameterName != null)
			{
				Add((object)value);
				return (VistaDBParameter)items[items.Count - 1];
			}
			else
				throw new ArgumentException("Parameter must be named");
		}

		/// <summary>
		/// Adds a VistaDBParameter to the VistaDBParameterCollection given the specified parameter name and type.
		/// </summary>
		/// <param name="parameterName">The name of the VistaDBParameter parameter.</param>
		/// <param name="type"></param>
		/// <returns></returns>
		public VistaDBParameter Add(string parameterName, VistaDBType type)
		{
			return Add(new VistaDBParameter(parameterName, type));
		}

		/// <summary>
		/// Adds a VistaDBParameter to the VistaDBParameterCollection given the specified parameter name and value.
		/// </summary>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The value of the VistaDBParameter to add to the collection. </param>
		/// <returns>The index of the new VistaDBParameter object.</returns>
		public VistaDBParameter Add(string parameterName, object value)
		{
			return Add(new VistaDBParameter(parameterName, value));
		}

		/// <summary>
		/// Adds a VistaDBParameter to the VistaDBParameterCollection with the parameter name, the data type, and the source column name.
		/// </summary>
		/// <param name="parameterName">The name of the parameter. </param>
		/// <param name="dbType">One of the VistaDBType values. </param>
		/// <param name="sourceColumn">The name of the source column. </param>
		/// <returns>The index of the new VistaDBParameter object.</returns>
		public VistaDBParameter Add(string parameterName, VistaDBType dbType, string sourceColumn)
		{
			return Add(new VistaDBParameter(parameterName, dbType, sourceColumn));
		}

		private int _cultureAwareCompare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase);
		}

		/// <summary>
		/// 
		/// </summary>
		public object SyncRoot//ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Not supported
		/// </summary>
		public void CopyTo(Array array, int index)//ICollection.CopyTo
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsSynchronized//ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets enumerator for VistaDBParameterCollection object
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new VistaDBParameterEnumerator(items);
		}

		object IList.this[int index]
		{
			get
			{
				return (VistaDBParameter)items[index];
			}
			set
			{
				items[index] = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether a VistaDBParameter exists in the collection.
		/// </summary>
		/// <param name="value">The value of the VistaDBParameter object to find. </param>
		/// <returns>True if the collection contains the VistaDBParameter object; otherwise, False.</returns>
		public bool Contains(object value)//IList.Contains
		{
			return items.Contains(value);
		}

		/// <summary>
		/// Gets the location of a VistaDBParameter in the collection.
		/// </summary>
		/// <param name="value">The VistaDBParameter object to locate.</param>
		/// <returns>The zero-based location of the VistaDBParameter in the collection.</returns>
		public int IndexOf(object value)//IList.IndexOf
		{
			return items.IndexOf(value);
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsFixedSize//IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsReadOnly//IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Removes all items from the collection.
		/// </summary>
		public void Clear()//IList.Clear()
		{
			items.Clear();
		}

		/// <summary>
		/// Inserts a VistaDBParameter into the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index where the parameter is to be inserted within the collection.</param>
		/// <param name="value">The VistaDBParameter to add to the collection.</param>
		public void Insert(int index, object value)//IList.Insert
		{
			items.Insert(index, value);
		}

		/// <summary>
		/// Removes the specified VistaDBParameter from the collection.
		/// </summary>
		/// <param name="value">A VistaDBParameter object to remove from the collection.</param>
		public void Remove(object value)//IList.Remove
		{
			items.Remove(value);
		}

		/// <summary>
		/// Removes the specified VistaDBParameter from the collection using a specific index.
		/// </summary>
		/// <param name="index">The zero-based index of the parameter.</param>
		public void RemoveAt(int index)//IList.RemoveAt
		{
			items.RemoveAt(index);
		}

		/// <summary>
		/// Gets the number of VistaDBParameter objects in the collection.
		/// </summary>
		public int Count//ICollection.Count
		{
			get
			{
				return items.Count;
			}
		}

		/// <summary>
		/// Gets the VistaDBParameter with the specified name.
		/// </summary>
		public VistaDBParameter this[string index]
		{
			get
			{
				int num1;

				num1 = IndexOf(index);

				if(num1 >= 0)
					return (VistaDBParameter)items[num1];

				return null;
			}
			set
			{
				int num1;

				num1 = IndexOf(index);

				if(num1 >= 0)
					items[num1] = value;
			}
		}

		/// <summary>
		/// Gets the VistaDBParameter at the specified index.
		/// </summary>
		public VistaDBParameter this[int index]
		{
			get
			{
				return (VistaDBParameter)items[index];
			}
			set
			{
				items[index] = value;
			}
		}
	}
}
