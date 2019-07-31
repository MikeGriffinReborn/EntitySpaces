using System;
using System.Data;
using System.Globalization;
using System.Collections;

namespace Provider.VistaDB
{
	/// <summary>
	/// Provides a means of reading a forward-only stream of rows from a VistaDB database. This class cannot be inherited.
	/// </summary>
	public sealed class VistaDBDataReader: MarshalByRefObject, IDataReader, IDataRecord, IEnumerable
	{
		private bool                    opened = true;
		private int                     rowsAffected;
		private int                     rowCount;
		private VistaDBConnection       vistaDBConnection;
		private VistaDBSQLQuery         query;
		private VistaDBColumnCollection columns;


		internal VistaDBDataReader(VistaDBSQLQuery query, bool fillData, VistaDBConnection connection)
		{
			this.query             = query;
			this.vistaDBConnection = connection;
			this.rowsAffected      = this.query.RowsAffected;
			this.rowCount          = this.query.RecordCount;

			this.columns           = new VistaDBColumnCollection(this);
		}

		internal VistaDBDataReader(int rowsAffected_)
		{
			this.rowsAffected      = rowsAffected_;

			this.opened            = false;
			this.rowCount          = 0;
			this.vistaDBConnection = null;
		}

		/// <summary>
		/// VistaDBDataReader destructor
		/// </summary>
		~VistaDBDataReader()
		{
			Dispose();
		}

		/// <summary>
		/// Gets a value indicating the depth of nesting for the current row. Always returns 0.
		/// </summary>
		public int Depth//Implements IDataReader.Depth
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the data reader is closed.
		/// </summary>
		public bool IsClosed//IDataReader.IsClosed
		{
			get
			{
				return !this.opened;
			}
		}

		/// <summary>
		/// Gets the number of rows changed, inserted, or deleted by execution of the V-SQL statement.
		/// </summary>
		public int RecordsAffected//IDataReader.RecordsAffected
		{
			get
			{
				return this.rowsAffected;
			}
		}

		/// <summary>
		/// Closes the VistaDBDataReader object.
		/// </summary>
		public void Close()//IDataReader.Close
		{
			if(this.opened)
			{
				this.query.Close();
				this.query.DropQuery();
			}

			this.opened = false;

			if( this.vistaDBConnection != null )
				this.vistaDBConnection.Close();
		}

		/// <summary>
		/// Move to the first record in the data set.
		/// </summary>
		public void First()
		{
			this.query.First();
		}

		/// <summary>
		/// Move to the next record in the data set.
		/// </summary>
		public bool NextResult()//Implements IDataReader.NextResult
		{
			return false;
		}

		/// <summary>
		/// Advances the VistaDBDataReader to the next record.
		/// </summary>
		/// <returns></returns>
		public bool Read()//IDataReader.Read
		{
			this.query.Next();
			return !this.query.Eof;
		}

		/// <summary>
		/// Returns a DataTable that describes the column metadata.
		/// </summary>
		/// <returns>A DataTable that describes the column metadata.</returns>
		public DataTable GetSchemaTable() //IDataReader.GetSchemaTable
		{
			object[] array;
			DataTable schemaTable = new DataTable("SchemaTable");

			schemaTable.Columns.Add("ColumnName", typeof(string));
			schemaTable.Columns.Add("ColumnOrdinal", typeof(int));
			schemaTable.Columns.Add("ColumnSize", typeof(int));
			schemaTable.Columns.Add("NumericPrecision", typeof(short));
			schemaTable.Columns.Add("NumericScale", typeof(short));
			schemaTable.Columns.Add("DataType", typeof(Type));
			schemaTable.Columns.Add("ProviderType", typeof(string));
			schemaTable.Columns.Add("IsLong", typeof(bool));
			schemaTable.Columns.Add("AllowDBNull", typeof(bool));
			schemaTable.Columns.Add("IsReadOnly", typeof(bool));
			schemaTable.Columns.Add("IsRowVersion", typeof(bool));
			schemaTable.Columns.Add("IsUnique", typeof(bool));
			schemaTable.Columns.Add("IsKey", typeof(bool));
			schemaTable.Columns.Add("IsAutoIncrement", typeof(bool));
			schemaTable.Columns.Add("BaseSchemaName", typeof(string));
			schemaTable.Columns.Add("BaseCatalogName", typeof(string));
			schemaTable.Columns.Add("BaseTableName", typeof(string));
			schemaTable.Columns.Add("BaseColumnName", typeof(string));
			schemaTable.Columns.Add("IsAliased", typeof(bool));
			schemaTable.Columns.Add("IsKeyColumn", typeof(bool));

			array = new object[20];
			
			for(int i = 0; i < this.columns.Count; i++)
			{
				array[0] = this.columns[i].Name;//ColumnName
				array[1] = i + 1;//ColumnOrdinal
				array[2] = this.columns[i].DataSize;//ColumnSize
				array[3] = 0;//NumericPrecision
				array[4] = 0;//NumericScale
				array[5] = this.columns[i].Type;//DataType
				array[6] = this.columns[i].VistaDBType;//DataType
				array[7] = false;
				array[8] = this.columns[i].AllowNull;//AllowDBNull
				array[9] = this.columns[i].ReadOnly;//IsReadOnly
				array[10] = false;//IsRowVersion
				array[11] = this.columns[i].PrimaryKey;//IsUnique;
				array[12] = this.columns[i].PrimaryKey;//IsKey
				array[13] = this.columns[i].Identity;//IsAutoIncrement
				array[14] = null;//BaseSchemaName
				array[15] = null;//BaseCatalogName
				array[16] = null;//BaseTableName
				array[17] = this.columns[i].Name;//BaseColumnName
				array[18] = false;//IsAliased
				array[19] = this.columns[i].PrimaryKey;//IsKeyColumn

				schemaTable.Rows.Add(array);
			}

			return schemaTable;
		}

		/// <summary>
		/// Gets the column located at the specified index.
		/// </summary>
		public object this[int i]//IDataRecord.Item
		{
			get
			{
				return this.query.GetValue(i);
			}
		}

		/// <summary>
		/// Gets the column with the specified name.
		/// </summary>
		public object this[string name]//IDataRecord.Item
		{
			get
			{
				int i = GetOrdinal(name);

				return this.query.GetValue(i);
			}
		}

		/// <summary>
		/// Gets the number of columns in the current row.
		/// </summary>
		public int FieldCount//IDataRecord.FieldCount
		{
			//Return a count of the number of columns, which in
			//this case is the size of the column metadata array.
			get
			{
				return this.columns.Count;
			}
		}

		/// <summary>
		/// Gets the name for the field to find.
		/// </summary>
		/// <param name="i">The index of the field to find. </param>
		/// <returns></returns>
		public string GetName(int i)//IDataRecord.GetName
		{
			return this.columns[i].Name;
		}

		/// <summary>
		/// Gets the column type of the specified column number.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns></returns>
		public string GetDataTypeName(int i)//IDataRecord.GetDataTypeName
		{
			return this.columns[i].Type.Name;
		}

		/// <summary>
		/// Releases the resources used by the component.
		/// </summary>
		public void Dispose()
		{
			this.columns           = null;
			this.vistaDBConnection = null;
			Close();
		}

		
		/// <summary>
		/// Gets the Type information corresponding to the type of Object that would be returned from GetValue.
		/// </summary>
		/// <param name="i">The index of the field to find. </param>
		/// <returns></returns>
		public Type GetFieldType(int i)//IDataRecord.GetFieldType
		{
			//Return the actual Type class for the data type.
			return this.columns[i].Type;
		}

		/// <summary>
		/// Return the value of the specified field.
		/// </summary>
		/// <param name="i">The index of the field to find.</param>
		/// <returns></returns>
		public object GetValue(int i)
		{
			return this.query.GetValue(i);
		}

		/// <summary>
		/// Gets all the attribute fields in the collection for the current record.
		/// </summary>
		/// <param name="values">An array of Object to copy the attribute fields into. </param>
		/// <returns>The number of instances of Object in the array.</returns>
		public int GetValues(object[] values)
		{
			int i = 0;

			while(i < values.Length && i < this.columns.Count)
			{
				values[i] = this.query.GetValue(i);
				i++;
			}
			return i;
		}

		/// <summary>
		/// Return the index of the named field.
		/// </summary>
		/// <param name="name">The name of the field to find. </param>
		/// <returns>The index of the named field.</returns>
		public int GetOrdinal(string name)
		{
			// Look for the ordinal of the column with the same name and return it.
			int i;

			for(i = 0; i < this.columns.Count; i++)
				if(0 == _cultureAwareCompare(name, this.columns[i].Name))
					return i;

			// Throw an exception if the ordinal cannot be found.
			throw new IndexOutOfRangeException("Could not find the specified column in the results");
		}

		/// <summary>
		/// Gets the value of the specified column as a Boolean.
		/// </summary>
		/// <param name="i">The zero-based column ordinal.</param>
		/// <returns>The value of the column.</returns>
		public bool GetBoolean(int i)//IDataRecord.GetBoolean
		{
			return (bool)(this.query.GetValue(i));
		}

		/// <summary>
		/// Gets the byte value for a Character or Varchar column type by passing the column number.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns>Byte value</returns>
		public byte GetByte(int i)//IDataRecord.GetByte
		{
			return (byte)(this.query.GetValue(i));
		}

		/// <summary>
		/// Gets the bytes for a Character or Varchar type column by passing the column number.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <param name="fieldOffset">String offset</param>
		/// <param name="buffer">Byte array to hold the result</param>
		/// <param name="bufferOffSet"></param>
		/// <param name="length">Number of bytes to return</param>
		/// <returns></returns>
		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffSet, int length)//IDataRecord.GetBytes
		{
			byte[] sourceArray = (byte[])this[i];

			if(buffer != null)
			{
				long copySize = Math.Min(length, sourceArray.LongLength - fieldOffset);
				Array.Copy(sourceArray, fieldOffset, buffer, bufferOffSet, copySize);
				return copySize;
			}
			else
				return sourceArray.LongLength;
		}

		/// <summary>
		/// Gets the character value for a Character or Varchar type column by passing the column number.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns></returns>
		public char GetChar(int i)//IDataRecord.GetChar
		{
			return (char)(this.query.GetValue(i));
		}


		/// <summary>
		/// Gets the characters for a Character or Varchar type column by passing the column number.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <param name="fieldOffSet">String offset</param>
		/// <param name="buffer">Byte array to hold the result</param>
		/// <param name="bufferOffSet"></param>
		/// <param name="length">Number of characters to return</param>
		/// <returns>Characters</returns>
		public long GetChars(int i, long fieldOffSet, char[] buffer, int bufferOffSet, int length)//IDataRecord.GetChars
		{
			throw new NotSupportedException("GetChars not supported.");
		}

		/// <summary>
		/// Gets the GUID value for a column
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns></returns>
		public Guid GetGuid(int i)//IDataRecord.GetGuid
		{
			return (Guid)(this.query.GetValue(i));
		}
		/// <summary>
		/// Gets the Int16 value for a column.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns></returns>
		public Int16 GetInt16(int i)//IDataRecord.GetInt16
		{
			return (Int16)(this.query.GetValue(i));
		}

		/// <summary>
		/// Gets the Int32 value for a column.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns></returns>
		public Int32 GetInt32(int i)//IDataRecord.GetInt32
		{
			return (Int32)(this.query.GetValue(i));
		}

		/// <summary>
		/// Gets the Int64 value for a column.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns></returns>
		public Int64 GetInt64(int i)//IDataRecord.GetInt64
		{
			return (Int64)(this.query.GetValue(i));
		}

		/// <summary>
		/// Gets the float value for a column.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns></returns>
		public float GetFloat(int i)//IDataRecord.GetFloat
		{
			return (float)(this.query.GetValue(i));
		}

		/// <summary>
		/// Gets the double value for a column.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns></returns>
		public double GetDouble(int i)//IDataRecord.GetDouble
		{
			return (double)(this.query.GetValue(i));
		}

		/// <summary>
		/// Gets the string value for a column.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns></returns>
		public string GetString(int i)//IDataRecord.GetString
		{
			return (string)(this.query.GetValue(i));
		}

		/// <summary>
		/// Gets the decimal value for a column.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns></returns>
		public decimal GetDecimal(int i)//IDataRecord.GetDecimal
		{
			return (decimal)(this.query.GetValue(i));
		}

		/// <summary>
		/// Gets the DateTime value for a column.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns></returns>
		public DateTime GetDateTime(int i)//IDataRecord.GetDateTime
		{
			return (DateTime)(this.query.GetValue(i));
		}

		/// <summary>
		/// Not supported
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public IDataReader GetData(int i)//IDataRecord.GetData
		{
			throw new NotSupportedException("GetData not supported.");
		}

		/// <summary>
		/// Return true if the column value is set to NULL.
		/// </summary>
		/// <param name="i">Column number</param>
		/// <returns>Return true if the column value is set to NULL.</returns>
		public bool IsDBNull(int i)//IDataRecord.IsDBNull
		{
			return this.query.IsNull(i);
		}

		private int _cultureAwareCompare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase);
		}

		/// <summary>
		/// Return IEnumerator
		/// </summary>
		/// <returns>Return IEnumerator</returns>
		public IEnumerator GetEnumerator()//IEnumerable.GetEnumerator
		{
			return new System.Data.Common.DbEnumerator(this);
		}

		/// <summary>
		/// VistaDBColumnCollection of columns
		/// </summary>
		public VistaDBColumnCollection Columns
		{
			get
			{
				return this.columns;
			}
		}

		/// <summary>
		/// Constructor for manaing a collection of VistaDBColumn objects.
		/// </summary>
		public class VistaDBColumnCollection
		{
			private VistaDBDataReader parent;

			internal VistaDBColumnCollection(VistaDBDataReader parent)
			{
				this.parent = parent;
			}

			/// <summary>
			/// Gets the column object at a given position.
			/// </summary>
			public VistaDBColumn this[int i]
			{
				get
				{
					if( i < 0 || i > Count)
						return  null;
					else
						return this.parent.query.Columns[i];
				}
			}
			
			/// <summary>
			/// Gets the number of column objects in the collection.
			/// </summary>
			public int Count
			{
				get
				{
					return this.parent.query.ColumnCount;
				}
			}
		}
	}
}