using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;

namespace Provider.VistaDB
{
	/// <summary>
	/// New column definition object
	/// </summary>
	/// <remarks>
	/// Useful when creating table structures from source code, this object
	/// lets a column be defined by setting properties. This object can then
	/// be passed to VistaDBTable.AlterColumn and VistaDBTable.CreateColumn
	/// overloaded methods.
	/// </remarks>
	public class VistaDBNewColumn
	{
		private string name = null;
		private VistaDBType type = VistaDBType.Character;
		private int length = 0;
		private short decimals = 0;
		private bool required = false;
		private bool readOnly = false;
		private bool packed = false;
		private bool hidden = false;
		private bool encrypted = false;
		private bool unicode = false;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="Name">Column name</param>
		/// <param name="DataType">Column data type</param>
		/// <param name="Length">Column data size</param>
		/// <param name="Decimals">Column decimals</param>
		/// <param name="Required">If True, then column required</param>
		/// <param name="ReadOnly">If True then column read only</param>
		/// <param name="Packed">If True then column packed</param>
		/// <param name="Hidden">If True then column hidden</param>
		/// <param name="Encrypted">If True then column encrypted</param>
		/// <param name="Unicode">Reserved, must be False</param>
		public VistaDBNewColumn(
			string Name, 
			VistaDBType DataType, 
			int Length,
			short Decimals,
			bool Required,
			bool ReadOnly,
			bool Packed,
			bool Hidden,
			bool Encrypted,
			bool Unicode
			)
		{
			name = Name;
			type = DataType; 
			length = Length;
			decimals = Decimals;
			required = Required;
			readOnly = ReadOnly;
			packed = Packed;
			hidden = Hidden;
			encrypted = Encrypted;
			unicode = Unicode;
		}
		/// <summary>
		/// Gets and sets the new column name.
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		/// <summary>
		/// Gets and sets the new column type. See VistaDBType for more.
		/// </summary>
		public VistaDBType Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}

		/// <summary>
		/// Gets and sets the new column size.
		/// </summary>
		public int Length
		{
			get
			{
				return length;
			}
			set
			{
				length = value;
			}
		}

		/// <summary>
		/// Gets and sets the new column decimals.
		/// </summary>
		public short Decimals
		{
			get
			{
				return decimals;
			}
			set
			{
				decimals = value;
			}
		}

		/// <summary>
		/// Gets and sets if this new column is required. This is a column constraint.
		/// </summary>
		public bool Required
		{
			get
			{
				return required;
			}
			set
			{
				required = value;
			}
		}

		/// <summary>
		/// Gets and sets if new column is readonly. 
		/// </summary>
		/// <remarks>
		/// Useful in preventing write access  to a database, specifically to a column. 
		/// Can be set to False prior to populating the column with data, then alter 
		/// the table and change the property to True.
		/// </remarks>
		public bool ReadOnly
		{
			get
			{
				return readOnly;
			}
			set
			{
				readOnly = value;
			}
		}

		/// <summary>
		/// Gets and sets if the new column should be packed (i.e. compressed)
		/// </summary>
		public bool Packed
		{
			get
			{
				return packed;
			}
			set
			{
				packed = value;
			}
		}

		/// <summary>
		/// Gets and sets if the new column should be hidden. 
		/// </summary>
		/// <remarks>
		/// SELECT * FROM Test will not retrieve the column. Only direct calls to the hidden column name will return results.</remarks>
		public bool Hidden
		{
			get
			{
				return hidden;
			}
			set
			{
				hidden = value;
			}
		}
		/// <summary>
		/// Gets and sets if a column is encrypted.
		/// </summary>
		public bool Encrypted
		{
			get
			{
				return encrypted;
			}
			set
			{
				encrypted = value;
			}
		}

		/// <summary>
		/// Gets and sets if a column is a unicode. 
		/// </summary>
		public bool Unicode
		{
			get
			{
				return unicode;
			}
			set
			{
				unicode = value;
			}
		}
	}

	/// <summary>
	/// VistaDBTable class provides an object-based live data cursor into a VistaDB data table
	/// </summary>
	public class VistaDBTable
	{

		/// <summary>
		/// Collection of active constraints.
		/// </summary>
		public class ConstraintActiveCollection
		{
			VistaDBTable parent;

			internal ConstraintActiveCollection(VistaDBTable parent_)
			{
				parent = parent_;
			}

			/// <summary>
			/// Get or set the active property of the constraint.
			/// </summary>
			public bool this[string constraintName]
			{
				get
				{
					lock(parent.database.SyncRoot)
					{
						parent.GetFocus();
						return VistaDBAPI.ivdb_IsConstraintActive(constraintName);
					}
				}
				set
				{
					lock(parent.database.SyncRoot)
					{
						parent.GetFocus();

						if( value )
						{
							if( ! VistaDBAPI.ivdb_ActivateConstraint(constraintName) )
								throw new VistaDBException( VistaDBErrorCodes.ObjectCannotBeActivated );
						}
						else
						{
							if( ! VistaDBAPI.ivdb_DeactivateConstraint(constraintName) )
								throw new VistaDBException( VistaDBErrorCodes.ObjectCannotBeDeactivated );
						}
					}
				}
			}
		}

		/// <summary>
		/// Collection of columns
		/// </summary>
		public class ColumnCollection: IList, ICollection, IEnumerable
		{
			VistaDBTable parent;

			internal ColumnCollection(VistaDBTable parent_)
			{
				parent = parent_;
			}

			/// <summary>
			/// Get column info by name
			/// </summary>
			public VistaDBColumn this[string columnName]
			{
				get
				{
					if(!parent.Opened)
						throw new VistaDBException( VistaDBErrorCodes.TableNotOpened );

					object idx = parent.columns[columnName.ToUpper()];
					if(idx == null)
						throw new VistaDBException( VistaDBErrorCodes.ColumnNotExist );

					return (VistaDBColumn)(parent.columnsInfo[(int)idx]);
				}
			}

			/// <summary>
			/// Get column info by index
			/// </summary>
			public VistaDBColumn this[int columnIndex]
			{
				get
				{
					if(!parent.Opened)
						throw new VistaDBException( VistaDBErrorCodes.TableNotOpened );

					if(parent.columnsInfo == null || columnIndex < 0 || columnIndex >= parent.columnsInfo.Count)
						throw new VistaDBException( VistaDBErrorCodes.ColumnNotExist );

					return (VistaDBColumn)(parent.columnsInfo[columnIndex]);
				}
			}

			#region IList Members

			/// <summary>
			/// 
			/// </summary>
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			object System.Collections.IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public void RemoveAt(int index)
			{
			}

			/// <summary>
			/// 
			/// </summary>
			public void Insert(int index, object value)
			{
			}

			/// <summary>
			/// 
			/// </summary>
			public void Remove(object value)
			{
			}

			/// <summary>
			/// 
			/// </summary>
			public bool Contains(object value)
			{
				for(int i = 0; i < this.Count; i++)
				{
					if(this[i] == value)
						return true;
				}
				return false;
			}

			/// <summary>
			/// 
			/// </summary>
			public void Clear()
			{
			}

			/// <summary>
			/// 
			/// </summary>
			public int IndexOf(object value)
			{
				for(int i = 0; i < this.Count; i++)
				{
					if(this[i] == value)
						return i;
				}
				return -1;
			}

			/// <summary>
			/// 
			/// </summary>
			public int Add(object value)
			{
				return -1;
			}

			/// <summary>
			/// 
			/// </summary>
			public bool IsFixedSize
			{
				get
				{
					return false;
				}
			}

			#endregion

			#region ICollection Members

			/// <summary>
			/// 
			/// </summary>
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public int Count
			{
				get
				{
					return this.parent.columnsInfo.Count;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public void CopyTo(Array array, int index)
			{
			}

			/// <summary>
			/// 
			/// </summary>
			public object SyncRoot
			{
				get
				{
					return null;
				}
			}

			#endregion

			#region IEnumerable Members

			/// <summary>
			/// 
			/// </summary>
			public IEnumerator GetEnumerator()
			{
				return new Enumerator(this);
			}

			#endregion

			#region Enumerator
			class Enumerator: IEnumerator
			{
				private int position;
				private ColumnCollection parent;

				/// <summary>
				/// 
				/// </summary>
				public Enumerator(ColumnCollection parent)
				{
					this.parent = parent;
					this.Reset();
				}

				/// <summary>
				/// 
				/// </summary>
				public bool MoveNext()
				{
					if(this.position < this.parent.Count - 1)
					{
						this.position++;
						return true;
					}
					return false;
				}

				/// <summary>
				/// 
				/// </summary>
				public void Reset()
				{
					this.position = -1;
				}

				/// <summary>
				/// 
				/// </summary>
				public VistaDBColumn Current
				{
					get
					{
						return this.parent[this.position];
					}
				}

				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

			}
			#endregion Enumerator

		}

		/// <summary>
		/// Collection of active triggers.
		/// </summary>
		public class TriggerActiveCollection
		{
			VistaDBTable parent;

			internal TriggerActiveCollection(VistaDBTable parent_)
			{
				parent = parent_;
			}

			/// <summary>
			/// Get or set the active property of the trigger.
			/// </summary>
			public bool this[string triggerName]
			{
				get
				{
					lock(parent.database.SyncRoot)
					{
						parent.GetFocus();
						return VistaDBAPI.ivdb_IsTriggerActive(triggerName);
					}
				}
				set
				{
					lock(parent.database.SyncRoot)
					{
						parent.GetFocus();
						if( value )
						{
							if( ! VistaDBAPI.ivdb_ActivateTrigger(triggerName) )
								throw new VistaDBException( VistaDBErrorCodes.ObjectCannotBeActivated );
						}
						else
						{
							if( ! VistaDBAPI.ivdb_DeactivateTrigger(triggerName) )
								throw new VistaDBException( VistaDBErrorCodes.ObjectCannotBeDeactivated );
						}
					}
				}
			}
		}


		///////////////////////////////////////////////////////////////
		////////////////////CONSTRUCTOR\DESTRUCTOR/////////////////////
		///////////////////////////////////////////////////////////////
		
		/// <summary>
		/// Constructor
		/// </summary>
		public VistaDBTable()
		{
			InitClass();
		
			triggerActive = new TriggerActiveCollection(this);
			constraintActive = new ConstraintActiveCollection(this);
			columnCollection = new ColumnCollection(this);
		}

		/// <summary>
		/// Constructor. Set Database property
		/// </summary>
		/// <param name="database_">Database object</param>
		public VistaDBTable(VistaDBDatabase database_)
		{
			InitClass();

			triggerActive = new TriggerActiveCollection(this);
			constraintActive = new ConstraintActiveCollection(this);
			columnCollection = new ColumnCollection(this);

			Database = database_;
		}

		/// <summary>
		/// Constructor. Set Database and TableName properties.
		/// </summary>
		/// <param name="database_">Database object</param>
		/// <param name="tablename">Table name</param>
		public VistaDBTable(VistaDBDatabase database_, string tablename)
		{
			InitClass();

			triggerActive = new TriggerActiveCollection(this);
			constraintActive = new ConstraintActiveCollection(this);
			columnCollection = new ColumnCollection(this);

			Database = database_;
			tableName = tablename;
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~VistaDBTable()
		{
			if( database != null )
				database.UnregisterTable(this);
			Close();
		}



		///////////////////////////////////////////////////////////////
		////////////////////////////METHODS////////////////////////////
		///////////////////////////////////////////////////////////////

		/// <summary>
		/// Start table restructuring
		/// </summary>
		/// <param name="sNewTableName">New table name</param>
		/// <returns>True for success</returns>
		public bool AlterTable(string sNewTableName)
		{
			lock(database.SyncRoot)
			{
				ConnectDatabase();
				tableId = -1;
				return VistaDBAPI.ivdb_AlterTable(tableName, sNewTableName);
			}
		}

		/// <summary>
		/// Alter the column
		/// </summary>
		/// <param name="sOldColumnName">Old column name</param>
		/// <param name="sNewColumnName">New column name</param>
		/// <param name="vdbType">Column data type</param>
		/// <param name="iLength">Column data length</param>
		/// <param name="iDecimals">Column data decimals</param>
		/// <param name="bRequired">Column required attribute</param>
		/// <param name="bReadOnly">Column read only attribute</param>
		/// <param name="bPacked">Column packed attribute</param>
		/// <param name="bHidden">Column hidden attribute</param>
		/// <param name="bEncrypted">Column encrypted attribute</param>
		/// <param name="bUnicode">Reserved</param>
		/// <returns>True for success</returns>
		public bool AlterColumn( string sOldColumnName, string sNewColumnName, VistaDBType vdbType, int iLength, short iDecimals, bool bRequired, bool bReadOnly, bool bPacked, bool bHidden, bool bEncrypted, bool bUnicode)
		{
			string sType = VistaDBAPI.NativeDataType(vdbType);

			lock(database.SyncRoot)
			{
				ConnectDatabase();
				return VistaDBAPI.ivdb_AlterColumn(sOldColumnName, sNewColumnName, sType, iLength, iDecimals, bRequired, bReadOnly, bPacked, bHidden, bEncrypted, bUnicode);
			}
		}

		/// <summary>
		/// Alter the column
		/// </summary>
		/// <param name="sOldColumnName">Old column name</param>
		/// <param name="column">New column description</param>
		/// <returns>True for success</returns>
		public bool AlterColumn( string sOldColumnName, VistaDBNewColumn column )
		{
			return AlterColumn(sOldColumnName, column.Name, column.Type, column.Length,
				column.Decimals, column.Required, column.ReadOnly, column.Packed, column.Hidden,
				column.Encrypted, column.Unicode);
		}

		/// <summary>
		/// Finish table restructuring and save changes in the database
		/// </summary>
		/// <param name="bForceAlter">Force table to be restructured</param>
		/// <returns>True for success</returns>
		public bool AlterTableFinalize(bool bForceAlter)
		{
			lock(database.SyncRoot)
			{
				ConnectDatabase();
				tableId = 0;
				return VistaDBAPI.ivdb_AlterTableExec(bForceAlter);
			}
		}

		/// <summary>
		/// Writes a BLOB stored in a column data directly to a new disk file. If a file of the same name exists, it is overwritten without warning.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="sFilename">File name</param>
		/// <returns>Return value is (true) if the file was created and written correctly or (false) if anything goes wrong (e.g., invalid column name, etc)</returns>
		public bool BlobToFile(string sColumnName, string sFilename)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_BlobToFile(sColumnName, sFilename);
			}
		}

		/// <summary>
		/// Tests if a row movement function has placed the row pointer before the first row in the table.
		/// </summary>
		/// <returns>True or false. If Bof returns true, the row buffer will contain the first row in the table.</returns>
		public bool BeginOfSet()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_Bof();
			}
		}

		/// <summary>
		/// Clear filter
		/// </summary>
		public void ClearFilter()
		{
			SetFilter("");
		}

		/// <summary>
		/// Clear scope
		/// </summary>
		public void ClearScope()
		{
			SetScope("", "");
		}

		/// <summary>
		/// Close the current table and release allocated memory. Hot row buffers are flushed to disk before closing.
		/// </summary>
		public virtual void Close()
		{
			if(tableId > 0)
			{
				lock(database.SyncRoot)
				{
					Select();
					VistaDBAPI.ivdb_Close();
				}
			}

			tableId = 0;
		}

		/// <summary>
		/// Extracts the number of columns in the current table.
		/// </summary>
		/// <returns>An integer containing the number of columns in each row.</returns>
		public short ColumnCount()
		{
			GetFocus();
			return (short)columns.Count;
		}
		
		/// <summary>
		/// Extracts the number of decimals defined for a numeric column.
		/// </summary>
		/// <param name="columnName">Column name</param>
		/// <returns>The number of decimals defined for the named column. Only meaningful if the column in question is numeric - otherwise zero.</returns>
		public short ColumnDecimals(string columnName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return (short)VistaDBAPI.ivdb_ColumnDecimals(columnName);
			}
		}

		/// <summary>
		/// Extracts the number of decimals defined for a numeric column.
		/// </summary>
		/// <param name="columnIndex">Column index (0-based)</param>
		/// <returns>The number of decimals defined for the named column. Only meaningful if the column in question is numeric - otherwise zero.</returns>
		public short ColumnDecimals(int columnIndex)
		{
			VistaDBColumn column = Columns[columnIndex];

			if(column == null)
			{
				return -1;
			}

			return ColumnDecimals(column.Name);
		}

		/// <summary>
		/// Gets number of named column in column array (relative to 1). This function is useful to test the validity of a column name.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>The relative position of the column in the column array (relative to 1). If the column does not exist, zero is returned.</returns>
		public short ColumnIndex(string sColumnName)
		{
			GetFocus();

			object idx;

			idx = columns[sColumnName.ToUpper()];

			if(idx == null)
				throw new VistaDBException(VistaDBErrorCodes.ColumnNotExist);

			return (short)((int)idx);
		}	
		
		/// <summary>
		/// Extracts the name of the nth column. The first column is number 1.
		/// </summary>
		/// <param name="iNum">Column order number</param>
		/// <returns>The field name is returned as a string. If uiFieldNum is invalid, a NULL string is returned.</returns>
		public string ColumnName(short iNum)
		{
			GetFocus();

			if(iNum < 1 || iNum > columnsInfo.Count)
				throw new VistaDBException(VistaDBErrorCodes.ColumnNotExist);

			return ((VistaDBColumn)columnsInfo[iNum - 1]).Name;
		}

		/// <summary>
		/// Reports the type of the named field.
		/// </summary>
		/// <param name="columnName">Column name</param>
		/// <returns>The column type</returns>
		public VistaDBType ColumnType(string columnName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				string s = "";
				s += (char)((byte)(VistaDBAPI.ivdb_ColumnTypeManaged(columnName)));
				return VistaDBAPI.NetDataType(s);
			}
		}

		/// <summary>
		/// Reports the type of the named field.
		/// </summary>
		/// <param name="columnIndex">Column index (0-based)</param>
		/// <returns>The column type</returns>
		public VistaDBType ColumnType(int columnIndex)
		{
			VistaDBColumn column = Columns[columnIndex];

			if(column == null)
			{
				return VistaDBType.Character;
			}

			return ColumnType(column.Name);
		}


		/// <summary>
		/// Extracts the width of the named column.
		/// </summary>
		/// <param name="columnName">Column name</param>
		/// <returns>The size in bytes of the named column.</returns>
		public short ColumnWidth(string columnName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return (short) VistaDBAPI.ivdb_ColumnWidth(columnName);
			}
		}

		/// <summary>
		/// Extracts the width of the named column.
		/// </summary>
		/// <param name="columnIndex">Column index (0-based)</param>
		/// <returns>The size in bytes of the named column.</returns>
		public short ColumnWidth(int columnIndex)
		{
			VistaDBColumn column = Columns[columnIndex];

			if(column == null)
			{
				return -1;
			}

			return ColumnWidth(column.Name);
		}


		/// <summary>
		/// Connects to database
		/// </summary>
		/// <returns></returns>
		private bool ConnectDatabase()
		{
			return database.ConnectWithoutSync();
		}
	
		/// <summary>
		/// Set FTS filter
		/// </summary>
		/// <param name="sColumnName">Name of the FTS column</param>
		/// <param name="sPattern">Filter pattern for FTS</param>
		/// <returns>First key position</returns>
		public int Contains(string sColumnName, string sPattern)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_Contains(sColumnName, sPattern);
			}
		}

		/// <summary>
		/// Defines a column to be included in a new table. A new table is created by first making a table ID with CreateNew. Each column is then defined with CreateColumn, and the table is finally physically created by calling CreateFinalize.
		/// </summary>
		/// <param name="sName">Column name</param>
		/// <param name="vdbType">Column data type</param>
		/// <param name="iLength">Length for column</param>
		/// <param name="iDecimals">Decimals for column</param>
		public void CreateColumn(string sName, VistaDBType vdbType, short iLength, short iDecimals)
		{
			if( tableId >= 0 )
			{
				throw new VistaDBException(VistaDBErrorCodes.TableNotInCreateMode);
			}

			string sType = VistaDBAPI.NativeDataType(vdbType);

			lock(database.SyncRoot)
			{
				ConnectDatabase();
				VistaDBAPI.ivdb_CreateColumn(sName, sType, iLength, iDecimals, false, false, false, false, false, false);
			}
		}
	
		/// <summary>
		///  Defines a column with extended options to be included in a new table. A new table is created by first making a table ID with CreateNew. Each column is then defined with CreateColumn, and the table is finally physically created by calling CreateFinalize.
		/// </summary>
		/// <param name="sName">Column name</param>
		/// <param name="vdbType">Column data type</param>
		/// <param name="iLength">Length for column</param>
		/// <param name="iDecimals">Decimals for column</param>
		/// <param name="bRequired">True for required</param>
		/// <param name="bReadOnly">True for read only</param>
		/// <param name="bPacked">True for packed</param>
		/// <param name="bHidden">True for hidden</param>
		/// <param name="bEncrypted">True for encrypted</param>
		/// <param name="bUnicode">This is parameter reserved</param>
		public void CreateColumn(string sName, VistaDBType vdbType, int iLength, short iDecimals,
			bool bRequired, bool bReadOnly, bool bPacked, bool bHidden, bool bEncrypted, bool bUnicode)
		{
			if(tableId >= 0)
			{
				throw new VistaDBException(VistaDBErrorCodes.TableNotInCreateMode);
			}

			string sType = VistaDBAPI.NativeDataType(vdbType);

			lock(database.SyncRoot)
			{
				ConnectDatabase();
				VistaDBAPI.ivdb_CreateColumn(sName, sType, iLength, iDecimals,
					bRequired, bReadOnly, bPacked, bHidden, bEncrypted, bUnicode);
			}
		}

		/// <summary>
		/// Create new column
		/// </summary>
		/// <param name="column">Column description object</param>
		public void CreateColumn(VistaDBNewColumn column)
		{
			CreateColumn(column.Name, column.Type, column.Length, column.Decimals,
				column.Required, column.ReadOnly, column.Packed, column.Hidden,
				column.Encrypted, column.Unicode);
		}

		private void CreateColumnInfo()
		{
			int columnCount;
			VistaDBColumn column;
			StringBuilder name;
			StringBuilder caption;
			string nameS;
			string captionS;
			string dataType;
			VistaDBType netDataType;
			bool reservedWord;
			StringBuilder seed;
			string seedS;
			double step = 0;
			bool packed, hidden, encrypted, unicode;

			columns = new Hashtable();
			columnsInfo = new ArrayList();

			columnCount = VistaDBAPI.ivdb_ColumnCount();

			for(int i = 0; i < columnCount; i++)
			{
				name = new StringBuilder(VistaDBAPI.MAX_STRING_SIZE);
				VistaDBAPI.ivdb_ColumnNameManaged((ushort)(i + 1), name, VistaDBAPI.MAX_STRING_SIZE);
				nameS = VistaDBAPI.CutString(name);

				caption = new StringBuilder(VistaDBAPI.MAX_STRING_SIZE);
				VistaDBAPI.ivdb_GetColumnCaptionManaged(nameS, caption, VistaDBAPI.MAX_STRING_SIZE);
				captionS = VistaDBAPI.CutString(caption);

				dataType = "";
				dataType += (char)((byte)(VistaDBAPI.ivdb_ColumnTypeManaged(nameS)));
				netDataType = VistaDBAPI.NetDataType(dataType);
				reservedWord = VistaDBAPI.ivsql_IsReservedWord(nameS);

				seed = new StringBuilder(VistaDBAPI.MAX_STRING_SIZE);
				VistaDBAPI.ivdb_GetIdentityManaged(nameS, ref step, seed, VistaDBAPI.MAX_STRING_SIZE);
				seedS = VistaDBAPI.CutString(seed);

				packed = VistaDBAPI.ivdb_ColumnCompressed(nameS);
				hidden = VistaDBAPI.ivdb_ColumnHidden(nameS);
				encrypted = VistaDBAPI.ivdb_ColumnEncrypted(nameS);
				unicode = VistaDBAPI.ivdb_ColumnUnicode(nameS);

				column = new VistaDBColumn(nameS, netDataType, VistaDBAPI.ivdb_ColumnWidth(nameS),
					(short)VistaDBAPI.ivdb_ColumnWidth(nameS), (short)VistaDBAPI.ivdb_ColumnDecimals(nameS), 
					!VistaDBAPI.ivdb_ColumnRequired(nameS), VistaDBAPI.ivdb_ColumnReadOnly(nameS), 
					VistaDBAPI.ivdb_ColumnIndexed(nameS, true), 
					VistaDBAPI.ivdb_ColumnIndexed(nameS, true), seedS.Length > 0,
					step, seedS, captionS, VistaDBAPI.ivdb_GetColumnDescription(nameS), reservedWord,
					packed, hidden, encrypted, unicode);

				columns.Add(column.Name.ToUpper(), i);
				columnsInfo.Add(column);
			}
		}

		/// <summary>
		/// Creates a table in the table ID set up via CreateNew and according to the column specifications defined with CreateColumn.
		/// This function is called as the last in a series of functions used to create a new table. CreateFinalize only operates upon the structure set up via a call to CreateNew (which sets up a new table ID) and calls to CreateColumn which define the columns in the table.
		/// </summary>
		/// <returns>True or False depending upon the outcome of the operation. It will be False if the preliminary operations have not been called successfully.</returns>
		public bool CreateFinalize()
		{
			if( tableId >= 0 )
			{
				throw new VistaDBException(VistaDBErrorCodes.TableNotInCreateMode);
			}

			lock(database.SyncRoot)
			{
				ConnectDatabase();
				if ( VistaDBAPI.ivdb_CreateTableExec() )
				{
					tableId = (short) VistaDBAPI.ivdb_TableId("");
					CreateColumnInfo();
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Creates new table and load data in one database, which is copy of the table from another database
		/// </summary>
		/// <param name="table">VistaDBTable object</param>
		/// <param name="loadData">Load data (true) or no (false)</param>
		/// <param name="importIndexes">If True, then indexes will be imported from the source table</param>
		/// <returns>True is succed, otherwise false</returns>
		public bool CreateFromTable(VistaDBTable table, bool loadData, bool importIndexes)
		{
			if(this.database == null || !this.database.Connected)
				throw new VistaDBException(VistaDBErrorCodes.DatabaseNotOpened);
			if(table == null || !table.Opened)
				throw new VistaDBException(VistaDBErrorCodes.TableNotOpened);
			
			if(tableId > 0)
				this.Close();

			short id;
			string name;

			lock(database.SyncRoot)
			{
				VistaDBAPI.ivdb_SelectDb(this.database.DatabaseId);

				name = (this.tableName == null || this.tableName.Length == 0) ? table.TableName : this.tableName;

				id = VistaDBAPI.ivdb_CreateFromTable(table.TableID, name, loadData, importIndexes, null);

				if(id > 0)
				{
					this.tableName = name;
					this.tableId = id;
					this.CreateColumnInfo();

					return true;
				}
				else
					return false;
			}
		}

		/// <summary>
		/// Create new FTS index with default words content for this index.
		/// Such default symbols set: digit symbols, apostrophe symbol and the other symbols this ANSI values greater of 0x40 (the letter 'A').
		/// </summary>
		/// <param name="indexName">FTS index name</param>
		/// <param name="columnName">Column name</param>
		/// <returns>Return index order. If it is 0, then index is not created.</returns>
		public short CreateFTSIndex(string indexName, string columnName)
		{
			return CreateFTSIndex(indexName, columnName, null);
		}

		/// <summary>
		/// Create new FTS index and set words content for this index.
		/// </summary>
		/// <param name="indexName">FTS index name</param>
		/// <param name="columnName">Column name</param>
		/// <param name="wordsContent">The set of character that will be included into FTS index keys. 
		/// The other symbols will be respected as words separators.
		/// If string is null or empty the engine uses default set of these symbols.
		/// Such default symbols set: digit symbols, apostrophe symbol and the other symbols this ANSI values greater of 0x40 (the letter 'A').</param>
		/// <returns>Return index order. If it is 0, then index is not created.</returns>
		public short CreateFTSIndex(string indexName, string columnName, string wordsContent)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_CreateIndex(indexName, columnName, (short)VDBIndexOption.FTS, false, wordsContent, 0);
			}
		}

		/// <summary>
		/// Create new index
		/// </summary>
		/// <param name="indexName">Index name</param>
		/// <param name="expr">Index expression</param>
		/// <param name="option">Option for new index</param>
		/// <param name="descend">True for descend order for index</param>
		/// <param name="locale">Index order locale. Put 0 for system locale</param>
		/// <returns>Return index order. If it is 0, then index is not created.</returns>
		public short CreateIndex(string indexName, string expr, VDBIndexOption option, bool descend, int locale)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_CreateIndex(indexName, expr, (short)option, descend, "", (uint) locale);
			}
		}

		/// <summary>
		/// Initializes a new table ID. This is the first step in creating a new table. The second step is to define each column with calls to CreateColumn. The final step is CreateFinalize, which physically creates the new table
		/// </summary>
		public bool CreateNew()
		{
			if( database == null )
				throw new VistaDBException(VistaDBErrorCodes.DatabaseNotAssigned);

			Close();

			lock(database.SyncRoot)
			{
				if ( !ConnectDatabase() )
					return false;

				if( VistaDBAPI.ivdb_CreateTable(tableName, "") )
				{
					tableId = -1;
					return true;
				}

				tableId = 0;
				return false;
			}
		}

		/// <summary>
		/// Delete all rows from the table
		/// </summary>
		/// <param name="usingFilter">If set to True, then delete rows, which visible in current filter else delete all rows</param>
		public void DeleteAllRows(bool usingFilter)
		{
			DeleteAllRows(usingFilter, true);
		}
			
		/// <summary>
		/// Delete all rows from the table
		/// </summary>
		/// <param name="usingFilter">If set to True, then delete rows, which visible in current filter else delete all rows</param>
		/// <param name="withRecycling">If set to False, then method doesn't use recycling mechanism, this increase speed</param>
		public void DeleteAllRows(bool usingFilter, bool withRecycling)
		{
			bool recycled;

			lock(database.SyncRoot)
			{
				GetFocus();

				recycled = !withRecycling && VistaDBAPI.ivdb_ActivatedRecycling();

				if(recycled)
				{
					VistaDBAPI.ivdb_DeactivateRecycling();
				}

				try
				{
					VistaDBAPI.ivdb_RowFirst();

					if( usingFilter )
					{
						while(!VistaDBAPI.ivdb_Eof())
						{
							VistaDBAPI.ivdb_DeleteRow();
						}
					}
					else
					{
						if(database.Exclusive)
							VistaDBAPI.ivdb_DeleteAllTableRows();
						else
						{
							VistaDBAPI.ivdb_SetScope(null, null);
							VistaDBAPI.ivdb_Query(null);

							while(!VistaDBAPI.ivdb_Eof())
							{
								VistaDBAPI.ivdb_DeleteRow();
							}
						}
					}
				}
				finally
				{
					if (!withRecycling && recycled)
						VistaDBAPI.ivdb_ActivateRecycling();
				}

				if (!withRecycling && recycled)
					VistaDBAPI.ivdb_ActivateRecycling();
			}
		}

		/// <summary>
		/// Delete current row
		/// </summary>
		public void DeleteCurrentRow()
		{
			uint recNo;

			lock(database.SyncRoot)
			{
				GetFocus();
				recNo = VistaDBAPI.ivdb_RowId();
				VistaDBAPI.ivdb_DeleteRow();
			}
		}

		/// <summary>
		/// Drop column under restructuring
		/// </summary>
		/// <param name="columnName">Column name</param>
		/// <returns>True if column dropped, else false</returns>
		public bool DropColumn(string columnName)
		{
			lock(database.SyncRoot)
			{
				ConnectDatabase();
				return VistaDBAPI.ivdb_DropColumn(columnName);
			}
		}

		/// <summary>
		/// Drop constraint
		/// </summary>
		/// <param name="sName">Constraint name</param>
		/// <returns>True for success</returns>
		public bool DropConstraint(string sName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_DropConstraint(sName);
			}
		}

		/// <summary>
		/// Drop foreign key from table
		/// </summary>
		/// <param name="sForeignKeyName">Foreign key name</param>
		/// <returns>True if success </returns>
		public bool DropForeignKey(string sForeignKeyName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_DropForeignKey(sForeignKeyName);
			}
		}

		/// <summary>
		/// Drop identity
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>True for success</returns>
		public bool DropIdentity(string sColumnName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_DropIdentity(sColumnName);
			}
		}

		/// <summary>
		/// Drop index
		/// </summary>
		/// <param name="indexName">Index name</param>
		/// <returns>True for success</returns>
		public bool DropIndex(string indexName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_DropIndex(indexName);
			}
		}

		/// <summary>
		/// Drop trigger
		/// </summary>
		/// <param name="sName">Trigger name</param>
		/// <returns>True for success</returns>
		public bool DropTrigger(string sName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_DropTrigger(sName);
			}
		}

		/// <summary>
		/// Determines whether the table is encrypted.
		/// </summary>
		/// <returns>True if the table is encrypted.</returns>
		public bool Encrypted()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_IsEncrypted(1);
			}
		}

		/// <summary>
		/// Tests if a row movement function has placed the row pointer beyond the last row in the table.
		/// </summary>
		/// <returns>True if end of file has been encountered and false if not. If Eof returns true, the row buffer is empty and the current row number is equal to the number of rows in the table plus one.</returns>
		public bool EndOfSet()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_Eof();
			}
		}

		/// <summary>
		/// Enumerate table constraints 
		/// </summary>
		/// <param name="list">Constraints names list</param>
		public void EnumConstraints(out string[] list)
		{
			uint i;
			string strConstraintName;
			string[] listTemp;
			StringBuilder buf;

			list = null;

			lock(database.SyncRoot)
			{
				GetFocus();

				buf = new StringBuilder(64);

				i = 0;

				do
				{
					i++;

					i = VistaDBAPI.ivdb_EnumConstraints(i, buf, 63);

					if (i != 0)
					{
						strConstraintName = VistaDBAPI.CutString(buf);

						if (list == null)
							list = new string[1];
						else
						{
							listTemp = new string[list.Length + 1];
							list.CopyTo(listTemp, 0);
							list = listTemp;
						}

						list[list.GetUpperBound(0)] = strConstraintName;
					}

				}
				while(i != 0);
			}

		}

		/// <summary>
		/// Enumerate all foreign keys for table
		/// </summary>
		/// <param name="list">List of foreign keys</param>
		public void EnumForeignKeys(out string [] list)
		{
			uint i;
			string strTriggerName;
			string[] listTemp;
			StringBuilder buf;

			list = null;

			lock(database.SyncRoot)
			{
				GetFocus();

				buf = new StringBuilder(64);

				i = 0;

				do
				{
					i++;

					i = VistaDBAPI.ivdb_EnumForeignKeys(i, buf, 63);

					if (i != 0)
					{
						strTriggerName = VistaDBAPI.CutString(buf);

						if (list == null)
							list = new string[1];
						else
						{
							listTemp = new string[list.Length + 1];
							list.CopyTo(listTemp, 0);
							list = listTemp;
						}

						list[list.GetUpperBound(0)] = strTriggerName;
					}

				}
				while(i != 0);
			}

		}
		
		/// <summary>
		/// Enumerate table identities
		/// </summary>
		/// <param name="list">Identities names list</param>
		public void EnumIdentities(out string[] list)
		{
			uint i;
			string strIdentityName;
			string[] listTemp;
			StringBuilder buf;

			list = null;

			lock(database.SyncRoot)
			{
				GetFocus();

				buf = new StringBuilder(64);

				i = 0;
				do
				{
					i++;

					i = VistaDBAPI.ivdb_EnumIdentities(i, buf, 63);

					if (i != 0)
					{
						strIdentityName = VistaDBAPI.CutString(buf);
						if (list == null)
							list = new string[1];
						else
						{
							listTemp = new string[list.Length + 1];
							list.CopyTo(listTemp, 0);
							list = listTemp;
						}

						list[list.GetUpperBound(0)] = strIdentityName;
					}

				}
				while(i != 0);
			}

		}

		/// <summary>
		/// Enumerate table indexes
		/// </summary>
		/// <param name="list">Indexes names list</param>
		public void EnumIndexes(out string[] list)
		{
			uint i;
			string strTagName;
			string[] listTemp;
			StringBuilder buf;

			list = null;

			lock(database.SyncRoot)
			{
				if (!ConnectDatabase())
					return;

				i = 0;
				do
				{
					i++;

					buf = new StringBuilder(64);

					i = VistaDBAPI.ivdb_EnumIndexes(i, tableName, buf, 63);

					if (i != 0)
					{

						strTagName = VistaDBAPI.CutString(buf);

						if (list == null)
							list = new string[1];
						else
						{
							listTemp = new string[list.Length + 1];
							list.CopyTo(listTemp, 0);
							list = listTemp;
						}

						list[list.GetUpperBound(0)] = strTagName;
					}

				}
				while(i != 0);
			}

		}

		/// <summary>
		/// Enumerate table triggers
		/// </summary>
		/// <param name="list">Triggers names list</param>
		public void EnumTriggers(out string [] list)
		{
			uint i;
			string strTriggerName;
			string[] listTemp;
			StringBuilder buf;

			list = null;

			lock(database.SyncRoot)
			{
				GetFocus();

				buf = new StringBuilder(64);

				i = 0;

				do
				{
					i++;

					i = VistaDBAPI.ivdb_EnumTriggers(i, buf, 63);

					if (i != 0)
					{
						strTriggerName = VistaDBAPI.CutString(buf);

						if (list == null)
							list = new string[1];
						else
						{
							listTemp = new string[list.Length + 1];
							list.CopyTo(listTemp, 0);
							list = listTemp;
						}

						list[list.GetUpperBound(0)] = strTriggerName;
					}

				}
				while(i != 0);
			}

		}

		/// <summary>
		/// Export table data to file
		/// </summary>
		/// <param name="sFileName">File name</param>
		/// <param name="vdbFileType">File type. There two file types:
		/// 1) Text File
		/// 2) XML File
		/// </param>
		/// <returns>True for success</returns>
		public bool ExportToFile(string sFileName, VDBFileType vdbFileType)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_ExportTableToText(sFileName, (short)vdbFileType);
			}
		}

		/// <summary>
		/// Find row by key value expression with using index
		/// </summary>
		/// <param name="sKeyValue">Key value expression</param>
		/// <param name="sIndexName">Index name</param>
		/// <returns>Return true if row found</returns>
		public bool Find(string sKeyValue, string sIndexName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_Find(sKeyValue, sIndexName, true, false);
			}
		}

		/// <summary>
		/// Find row by key value expression with using index
		/// </summary>
		/// <param name="keyValue">Key value expression</param>
		/// <param name="indexName">Index name</param>
		/// <param name="exactMatch">True for searching by using exact matching</param>
		/// <param name="noMatchStopsOnNextRecord">No match stops on next record</param>
		/// <returns>Return true if row found</returns>
		public bool Find(string keyValue, string indexName, bool exactMatch, bool noMatchStopsOnNextRecord)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_Find(keyValue, indexName, exactMatch, noMatchStopsOnNextRecord);
			}
		}

		/// <summary>
		/// Find row by key value expression with using active index
		/// </summary>
		/// <param name="keyValue">Key value expression</param>
		/// <returns>Return true if active index is set and row found</returns>
		public bool Find(string keyValue)
		{
			lock(database.SyncRoot)
			{
				GetFocus();

				string s = ActiveIndex;

				if(s != null)
					return VistaDBAPI.ivdb_Find(keyValue, s, true, false);
				else
					return false;
			}
		}

		/// <summary>
		/// Find row by key value expression with using active index
		/// </summary>
		/// <param name="sKeyValue">Key value expression</param>
		/// <param name="bExactMatch">True for searching by using exact matching</param>
		/// <param name="bSoftPosition">True for searching by using soft position</param>
		/// <returns>Return true if active index is set and row found</returns>
		public bool Find(string sKeyValue, bool bExactMatch, bool bSoftPosition)
		{
			lock(database.SyncRoot)
			{
				GetFocus();

				string s = ActiveIndex;

				if(s != null)
					return VistaDBAPI.ivdb_Find(sKeyValue, s, bExactMatch, bSoftPosition);
				else
					return false;
			}
		}

		/// <summary>
		/// Move to the first row in the table
		/// </summary>
		public void First()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_RowFirst();
			}
		}

		/// <summary>
		/// Retrieves a BLOB that was stored in a column. A BLOB is a binary large object.
		/// </summary>
		/// <param name="columnName">Column name</param>
		/// <param name="buffer">Buffer for blob data</param>
		/// <returns>Return bytes count which puts in the buffer</returns>
		public int GetBlob(string columnName, out byte[] buffer)
		{
			uint size;

			buffer = null;
			
			lock(database.SyncRoot)
			{
				GetFocus();

				size = VistaDBAPI.ivdb_GetBlobLength(columnName);
				buffer = new byte[size];

				return (int)VistaDBAPI.ivdb_GetBlob(columnName, buffer);
			}
		}

		/// <summary>
		/// Retrieves a BLOB that was stored in a column. A BLOB is a binary large object.
		/// </summary>
		/// <param name="columnName">Column name</param>
		/// <param name="stream">MemoryStream object for blob data</param>
		/// <returns>Return bytes count which puts in the MemoryStream object</returns>
		public int GetBlob(string columnName, out MemoryStream stream)
		{
			byte[] buffer;
			int res;
			
			res = GetBlob(columnName, out buffer);
			if(res > 0)
				stream = new MemoryStream(buffer);
			else
				stream = null;

			return res;
		}

		/// <summary>
		/// Retrieves a BLOB that was stored in a column. A BLOB is a binary large object.
		/// </summary>
		/// <param name="columnIndex">Column index (0-based)</param>
		/// <param name="buffer">Buffer for blob data</param>
		/// <returns>Return bytes count which puts in the buffer</returns>
		public int GetBlob(int columnIndex, out byte[] buffer)
		{
			VistaDBColumn column = Columns[columnIndex];

			if(column == null)
			{
				buffer = null;
				return -1;
			}

			return GetBlob(column.Name, out buffer);
		}

		/// <summary>
		/// Retrieves a BLOB that was stored in a column. A BLOB is a binary large object.
		/// </summary>
		/// <param name="columnIndex">Column index (0-based)</param>
		/// <param name="stream">MemoryStream object for blob data</param>
		/// <returns>Return bytes count which puts in the MemoryStream object</returns>
		public int GetBlob(int columnIndex, out MemoryStream stream)
		{
			byte[] buffer;
			int res;
			
			res = GetBlob(columnIndex, out buffer);
			if(res > 0)
				stream = new MemoryStream(buffer);
			else
				stream = null;

			return res;
		}

		/// <summary>
		/// Retrieves the length of a BLOB that was stored in a BLOB column. A BLOB is a binary large object.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>The size of the BLOB as integer.</returns>
		public int GetBlobLength(string sColumnName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return (int) VistaDBAPI.ivdb_GetBlobLength(sColumnName);
			}
		}

		/// <summary>
		/// Determines whether a boolean column contains a True or False value.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>True if the column evaluates as True, and False if not.</returns>
		public bool GetBoolean(string sColumnName)
		{
			object idx = columns[sColumnName.ToUpper()];

			if(idx == null)
				idx = -1;

			return GetBoolean((int)idx);
		}

		/// <summary>
		/// Determines whether a boolean column contains a True or False value.
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <returns>True if the column evaluates as True, and False if not.</returns>
		public bool GetBoolean(int iColumnIndex)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_GetBooleanByIndex(iColumnIndex + 1);
			}
		}

		/// <summary>
		/// Get column caption
		/// </summary>
		/// <param name="columnName">Column name</param>
		/// <returns>Return column caption</returns>
		public string GetColumnCaption(string columnName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();

				StringBuilder buf = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);
				if( VistaDBAPI.ivdb_GetColumnCaptionManaged(columnName, buf, VistaDBAPI.MAX_DESCRIPTION_SIZE) )
				{
					return VistaDBAPI.CutString(buf);
				}
				else
					return null;
			}
		}

		/// <summary>
		/// Get column caption
		/// </summary>
		/// <param name="columnIndex">Column index (0-based)</param>
		/// <returns>Return column caption</returns>
		public string GetColumnCaption(int columnIndex)
		{
			VistaDBColumn column = Columns[columnIndex];

			if(column == null)
			{
				return "";
			}

			return GetColumnCaption(column.Name);
		}
	
		/// <summary>
		/// Get constraint
		/// </summary>
		/// <param name="sName">Constraint name</param>
		/// <returns></returns>
		public string GetConstraint(string sName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();

				StringBuilder buf = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);
			
				if( VistaDBAPI.ivdb_GetConstraintManaged(sName, buf, VistaDBAPI.MAX_DESCRIPTION_SIZE))
					return VistaDBAPI.CutString(buf);
				else
					return null;
			}
		}

		/// <summary>
		/// Get currency value for the column name
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>Currency value</returns>
		public decimal GetCurrency(string sColumnName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();

				object idx = columns[sColumnName.ToUpper()];

				if(idx == null)
					idx = -1;

				return GetCurrency((int)idx);
			}
		}

		/// <summary>
		/// Get currency value by column index
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <returns>Currency value</returns>
		public decimal GetCurrency(int iColumnIndex)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return (decimal)(VistaDBAPI.ivdb_GetCurrencyByIndex(iColumnIndex + 1)) / (decimal)10000;
			}
		}

		/// <summary>
		/// Extracts date by column name
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>Date and time</returns>
		public DateTime GetDate(string sColumnName)
		{
			return GetDateTime(sColumnName).Date;
		}

		/// <summary>
		/// Extracts date by column index
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <returns>Date and time</returns>
		public DateTime GetDate(int iColumnIndex)
		{
			return GetDateTime(iColumnIndex).Date;
		}

		/// <summary>
		/// Extracts date and time by column name
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>Date and time</returns>
		public DateTime GetDateTime(string sColumnName)
		{
			object idx = columns[sColumnName.ToUpper()];

			if(idx == null)
				idx = -1;

			return GetDateTime((int)idx);
		}

		/// <summary>
		/// Extracts date and time by column index
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <returns>Date and time</returns>
		public DateTime GetDateTime(int iColumnIndex)
		{
			double d;
			long t, t2;

			lock(database.SyncRoot)
			{
				GetFocus();

				d = VistaDBAPI.ivdb_GetDateTimeByIndex(iColumnIndex + 1);

				t = (long)((d - 1) * 24*60*60*10000000);
				t2 = (long)((d - 1) * 24*60*60*1000);

				if (t2 * 10000 != t)
					t = (t2 + 1) * 10000;

				return new DateTime(t);
			}
		}
		
		/// <summary>
		/// Get default value
		/// </summary>
		/// <param name="columnName">Column name</param>
		/// <param name="useInUpdate">Use this default value in updates. 
		/// Useful when combined with V-Script functions that generate dynamic values, such as the DATE() function.</param>
		/// <returns>Default value for specified column</returns>
		public string GetDefaultValue(string columnName, out bool useInUpdate)
		{
			useInUpdate = false;

			lock(database.SyncRoot)
			{
				GetFocus();

				StringBuilder buf = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);
				if( VistaDBAPI.ivdb_GetDefaultValueManaged(columnName, ref useInUpdate, buf, VistaDBAPI.MAX_DESCRIPTION_SIZE ) )
					return VistaDBAPI.CutString(buf);
				else
					return null;
			}
		}

		/// <summary>
		/// Get default value
		/// </summary>
		/// <param name="columnIndex">Column index (0-based)</param>
		/// <param name="useInUpdate">Use this default value in updates. 
		/// Useful when combined with V-Script functions that generate dynamic values, such as the DATE() function.</param>
		/// <returns>Default value for specified column</returns>
		public string GetDefaultValue(int columnIndex, out bool useInUpdate)
		{
			VistaDBColumn column = Columns[columnIndex];

			if(column == null)
			{
				useInUpdate = false;
				return "";
			}

			return GetDefaultValue(column.Name, out useInUpdate);
		}


		/// <summary>
		/// Extracts the contents of a double value.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>The double value of the column</returns>
		public double GetDouble(string sColumnName)
		{
			object idx = columns[sColumnName.ToUpper()];

			if(idx == null)
				idx = -1;

			return GetDouble((int)idx);
		}

		/// <summary>
		/// Extracts the contents of a double value.
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <returns>The double value of the column</returns>
		public double GetDouble(int iColumnIndex)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_GetDoubleByIndex(iColumnIndex + 1);
			}
		}

		private void GetFocus()
		{
			if(tableId <= 0)
			{
				throw new VistaDBException(VistaDBErrorCodes.TableNotOpened);
			}

			Select();
		}

		/// <summary>
		/// Get foreign key information by foreign key name
		/// </summary>
		/// <param name="sForeignKeyName">Foreign key name</param>
		/// <param name="sForeignKey">Foreign key</param>
		/// <param name="sPrimTable">Primary table</param>
		/// <param name="sPrimKey">Primary key</param>
		/// <returns>Foreign key description</returns>
		public string GetForeignKey( string sForeignKeyName, out string sForeignKey,
			out string sPrimTable, out string sPrimKey)
		{
			uint ulOptions = 0;
			StringBuilder foreignKey = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);
			StringBuilder primTable = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);
			StringBuilder primKey = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);
			StringBuilder descr = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);

			lock(database.SyncRoot)
			{
				GetFocus();

				VistaDBAPI.ivdb_GetForeignKeyManaged( sForeignKeyName, 
					foreignKey, primTable, 
					primKey, VistaDBAPI.MAX_DESCRIPTION_SIZE, ref ulOptions, 
					descr, VistaDBAPI.MAX_DESCRIPTION_SIZE);

				sForeignKey = VistaDBAPI.CutString(foreignKey);
				sPrimTable = VistaDBAPI.CutString(primTable);
				sPrimKey = VistaDBAPI.CutString(primKey);

				return VistaDBAPI.CutString(descr);
			}
		}

		/// <summary>
		/// Get foreign key information (including options) by foreign key name
		/// </summary>
		/// <param name="sForeignKeyName">Foreign key name</param>
		/// <param name="sForeignKey">Foreign key</param>
		/// <param name="sPrimTable">Primary table</param>
		/// <param name="sPrimKey">Primary key</param>
		/// <param name="options">Foreign key options</param>
		/// <returns>Foreign key description</returns>
		public string GetForeignKey( string sForeignKeyName, out string sForeignKey,
			out string sPrimTable, out string sPrimKey, out VDBForeignKeyOptions options)
		{
			uint ulOptions = 0;
			StringBuilder foreignKey = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);
			StringBuilder primTable = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);
			StringBuilder primKey = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);
			StringBuilder descr = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);

			lock(database.SyncRoot)
			{
				GetFocus();

				VistaDBAPI.ivdb_GetForeignKeyManaged( sForeignKeyName, 
					foreignKey, primTable, 
					primKey, VistaDBAPI.MAX_DESCRIPTION_SIZE, ref ulOptions, 
					descr, VistaDBAPI.MAX_DESCRIPTION_SIZE);
			}

			sForeignKey = VistaDBAPI.CutString(foreignKey);
			sPrimTable = VistaDBAPI.CutString(primTable);
			sPrimKey = VistaDBAPI.CutString(primKey);

			switch(ulOptions)
			{
				case 1:
					options = VDBForeignKeyOptions.OnDeleteCascade;
					break;
				case 2:
					options = VDBForeignKeyOptions.OnUpdateCascade;
					break;
				default:
					options = VDBForeignKeyOptions.None;
					break;
			}

			return VistaDBAPI.CutString(descr);
		}

		/// <summary>
		/// Extracts the contents of a Guid column as Guid structure.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>The contents of the column converted to Guid structure</returns>
		public Guid GetGuid(string sColumnName)
		{
			object idx = columns[sColumnName.ToUpper()];

			if(idx == null)
				idx = -1;

			return GetGuid((int)idx);
		}

		/// <summary>
		/// Extracts the contents of a Guid column as Guid structure.
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <returns>The contents of the column converted to Guid structure</returns>
		public Guid GetGuid(int iColumnIndex)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_GetGuidByIndex(iColumnIndex + 1);
			}
		}

		/// <summary>
		/// Get identity
		/// </summary>
		/// <param name="columnName">Column name</param>
		/// <param name="step">Identity step</param>
		/// <returns>Return current identity value</returns>
		public string GetIdentity(string columnName, out double step)
		{
			step = 0;

			lock(database.SyncRoot)
			{
				GetFocus();

				StringBuilder buf = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);
				if( VistaDBAPI.ivdb_GetIdentityManaged(columnName, ref step, buf, VistaDBAPI.MAX_DESCRIPTION_SIZE) )
					return VistaDBAPI.CutString(buf);
				else
					return null;
			}
		}

		/// <summary>
		/// Get identity
		/// </summary>
		/// <param name="columnIndex">Column index (0-based)</param>
		/// <param name="step">Identity step</param>
		/// <returns>Return current identity value</returns>
		public string GetIdentity(int columnIndex, out double step)
		{
			VistaDBColumn column = Columns[columnIndex];

			if(column == null)
			{
				step = 0;
				return "";
			}

			return GetIdentity(column.Name, out step);
		}


		/// <summary>
		/// Get index info
		/// </summary>
		/// <param name="indexName">Index name</param>
		/// <param name="active">If true then index is active else inactive</param>
		/// <param name="orderIndex">Index order</param>
		/// <param name="unique">If true then index is unique</param>
		/// <param name="primary">If true then index is primary key</param>
		/// <param name="desc">If true then index descending</param>
		/// <param name="keyExpr">Index expression</param>
		/// <returns>True for success</returns>
		public bool GetIndex(string indexName, out bool active, out int orderIndex, out bool unique, out bool primary, out bool desc, out string keyExpr)
		{
			bool fts;
			return this.GetIndex(indexName, out active, out orderIndex, out unique, out primary, out desc, out keyExpr, out fts);
		}
			
		/// <summary>
		/// Get index info
		/// </summary>
		/// <param name="indexName">Index name</param>
		/// <param name="active">If true then index is active else inactive</param>
		/// <param name="orderIndex">Index order</param>
		/// <param name="unique">If true then index is unique</param>
		/// <param name="primary">If true then index is primary key</param>
		/// <param name="desc">If true then index descending</param>
		/// <param name="keyExpr">Index expression</param>
		/// <param name="fts">If tru then index is FTS</param>
		/// <returns>True for success</returns>
		public bool GetIndex(string indexName, out bool active, out int orderIndex, out bool unique, out bool primary, out bool desc, out string keyExpr, out bool fts)
		{
			RTagInfo info = new RTagInfo();
			bool res = false;

			active = false;
			orderIndex = 0;
			keyExpr = "";
			unique = false;
			primary = false;
			desc = false;
			fts = false;

			lock(database.SyncRoot)
			{
				GetFocus();

				info.iInfoSize = VistaDBAPI.RTagInfoSize;

				res = VistaDBAPI.ivdb_GetIndexInformationByName(indexName, ref info);
			}

			active = info.bActive;
			orderIndex = info.iOrderIndex;
			keyExpr = info.cpKeyExpr;
			unique = info.bUnique;
			primary = info.bPrimary;
			desc = info.bDesc;
			fts = info.bFts;

			return res;
		}

		/// <summary>
		/// Extracts the contents of a 32-bit integer column
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>The contents of the column</returns>
		public int GetInt32(string sColumnName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();

				object idx = columns[sColumnName.ToUpper()];

				if(idx == null)
					idx = -1;

				return VistaDBAPI.ivdb_GetInt32ByIndex((int)idx + 1);
			}
		}

		/// <summary>
		/// Extracts the contents of a 32-bit integer column
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <returns>The contents of the column</returns>
		public int GetInt32(int iColumnIndex)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_GetInt32ByIndex(iColumnIndex + 1);
			}
		}

		/// <summary>
		/// Extracts the contents of a numeric column as a signed 64-bit integer value.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>The contents of the column converted to a signed long integer value.</returns>
		public long GetInt64(string sColumnName)
		{
			object idx = columns[sColumnName.ToUpper()];

			if(idx == null)
				idx = -1;

			return GetInt64((int)idx);
		}

		/// <summary>
		/// Extracts the contents of a numeric column as a signed 64-bit integer value.
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <returns>The contents of the column converted to a signed long integer value.</returns>
		public long GetInt64(int iColumnIndex)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_GetInt64ByIndex(iColumnIndex + 1);
			}
		}
	
		/// <summary>
		/// Extracts the contents of a memo column as a string and optionally format the memo with hard carriage returns and line feeds for printing.
		/// </summary>
		/// <param name="columnName">Column name</param>
		/// <returns>The a reference to the memo contents is returned as a string. An empty memo is returned as a NULL string.</returns>
		public string GetMemo(string columnName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();

				int len = (int)VistaDBAPI.ivdb_GetBlobLength(columnName) + 1;
				StringBuilder buf = new StringBuilder(len);
			
				if(VistaDBAPI.ivdb_GetMemoManaged(columnName, buf, len))
					return VistaDBAPI.CutString(buf);
				else
					return null;
			}
		}

		/// <summary>
		/// Extracts the contents of a memo column as a string and optionally format the memo with hard carriage returns and line feeds for printing.
		/// </summary>
		/// <param name="columnIndex">Column index (0-based)</param>
		/// <returns>The a reference to the memo contents is returned as a string. An empty memo is returned as a NULL string.</returns>
		public string GetMemo(int columnIndex)
		{
			VistaDBColumn column = Columns[columnIndex];

			if(column == null)
			{
				return "";
			}

			return GetMemo(column.Name);
		}

		/// <summary>
		/// Check if column data is null
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>Return True if column data is NULL</returns>
		public bool GetNull(string sColumnName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_GetNull(sColumnName);
			}
		}

		/// <summary>
		/// Check if column data is null
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <returns>Return True if column data is NULL</returns>
		public bool GetNull(int iColumnIndex)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_GetNullByIndex(iColumnIndex + 1);
			}
		}

		/// <summary>
		/// Extract VistaDB type column value
		/// </summary>
		/// <param name="columnIndex">Column index (0-based)</param>
		/// <returns>Content of column</returns>
		public object GetObject(int columnIndex)
		{
			VistaDBColumn column = Columns[columnIndex];

			if(column == null)
				return null;

			if(GetNull(columnIndex))
			{
				return DBNull.Value;
			}
			else
			{
				switch(column.VistaDBType)
				{
					case VistaDBType.Blob:
					case VistaDBType.Picture:
						byte[] res;
						GetBlob(column.Name, out res);
						return res;
					case VistaDBType.Boolean:
						return GetBoolean(columnIndex);
					case VistaDBType.Character:
					case VistaDBType.Varchar:
						return GetString(columnIndex);
					case VistaDBType.Currency:
						return GetCurrency(columnIndex);
					case VistaDBType.Date:
						return GetDate(columnIndex);
					case VistaDBType.DateTime:
						return GetDateTime(columnIndex);
					case VistaDBType.Double:
						return GetDouble(columnIndex);
					case VistaDBType.Guid:
						return GetGuid(columnIndex);
					case VistaDBType.Int32:
						return GetInt32(columnIndex);
					case VistaDBType.Int64:
						return GetInt64(columnIndex);
					case VistaDBType.Memo:
						return GetMemo(column.Name);
					default:
						return null;
				}
			}
		}

		/// <summary>
		/// Extract VistaDB type column value
		/// </summary>
		/// <param name="columnName">Column name</param>
		/// <returns>Content of column</returns>
		public object GetObject(string columnName)
		{
			object columnIndex = columns[columnName.ToUpper()];

			if(columnIndex == null)
				throw new VistaDBException( VistaDBErrorCodes.ColumnNotExist );

			return GetObject((int)columnIndex);
		}

		/// <summary>
		/// Extracts the contents of any column as a string value.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <returns>The column contents are returned as a string.</returns>
		public string GetString(string sColumnName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();

				object idx = columns[sColumnName.ToUpper()];

				if(idx == null || (int)idx < 0 || (int)idx >= columnsInfo.Count)
					throw new VistaDBException(VistaDBErrorCodes.ColumnNotExist);

				int len = ((VistaDBColumn)columnsInfo[(int)idx]).ColumnWidth + 1;
				StringBuilder buf = new StringBuilder(len);

				if(VistaDBAPI.ivdb_GetStringManagedByIndex((int)idx + 1, buf, (uint)len))
					return VistaDBAPI.CutString(buf);
				else
					return null;
			}
		}

		/// <summary>
		/// Extracts the contents of any column as a string value.
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <returns>The column contents are returned as a string.</returns>
		public string GetString(int iColumnIndex)
		{
			lock(database.SyncRoot)
			{
				GetFocus();

				if(iColumnIndex < 0 || iColumnIndex >= columnsInfo.Count)
					throw new VistaDBException(VistaDBErrorCodes.ColumnNotExist);

				int len = ((VistaDBColumn)columnsInfo[iColumnIndex]).ColumnWidth + 1;
				StringBuilder buf = new StringBuilder(len);

				if(VistaDBAPI.ivdb_GetStringManagedByIndex(iColumnIndex + 1, buf, (uint)len))
					return VistaDBAPI.CutString(buf);
				else
					return null;
			}
		}

		/// <summary>
		/// Get trigger
		/// </summary>
		/// <param name="sName">Trigger name</param>
		/// <param name="EventOption">Event option for trigger</param>
		/// <param name="piPriority">Trigger priority</param>
		/// <returns>Return trigger text</returns>
		public string GetTrigger(string sName, out VDBTriggerEvent EventOption, out int piPriority)
		{
			int pulEventOption;
			string res;

			pulEventOption = 1;
			piPriority = 0;

			lock(database.SyncRoot)
			{
				GetFocus();

				StringBuilder buf = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);

				VistaDBAPI.ivdb_GetTriggerManaged(sName, ref pulEventOption, ref piPriority,
					buf, VistaDBAPI.MAX_DESCRIPTION_SIZE);
				res = VistaDBAPI.CutString(buf);

				EventOption = (VDBTriggerEvent)pulEventOption;

				return res;
			}
		}

		/// <summary>
		/// Go to the next key for the current FTS filter
		/// </summary>
		/// <returns>Key position in column data</returns>
		public int GoNextFtsKey()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_GoNextFtsKey();
			}
		}

		/// <summary>
		/// Import data to table from file
		/// </summary>
		/// <param name="sFileName">File name</param>
		/// <param name="vdbFileType">File type. There two file types:
		/// 1) Text File
		/// 2) XML File
		/// </param>
		/// <param name="sScopeExpr">Filter expression for importing data</param>
		/// <returns>True for success</returns>
		public bool ImportFromFile(string sFileName, VDBFileType vdbFileType, string sScopeExpr)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_ImportTableFrom(sFileName, (short)vdbFileType, sScopeExpr);
			}
		}

		/// <summary>
		/// Flip index order to ascending
		/// </summary>
		/// <returns></returns>
		public bool IndexAscending()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_IndexAscending();
			}
		}

		/// <summary>
		/// Flip index order to descending
		/// </summary>
		/// <returns></returns>
		public bool IndexDescending()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_IndexDescending();
			}
		}

		/// <summary>
		/// Get index information
		/// </summary>
		/// <param name="sIndexName">Index name</param>
		/// <param name="indexInfo">Index info</param>
		/// <returns>Return true if success</returns>
		public bool IndexInformation(string sIndexName, ref RTagInfo indexInfo)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_GetIndexInformationByName(sIndexName, ref indexInfo);
			}
		}

		/// <summary>
		/// Return current index order
		/// </summary>
		/// <returns>Return current index order</returns>
		public short IndexOrder()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_IndexOrder();
			}
		}

		private void InitClass()
		{
			VistaDBErrorMsgs.SetErrorFunc();

			tableId = 0;		
		}

		/// <summary>
		/// Inserts a new blank row to the table.
		/// </summary>
		/// <example>This sample shows how to insert new row
		/// <code>
		/// static void Main()
		/// {
		///		VistaDBDatabase vistaDB = new VistaDBDatabase();
		///		VistaDBTable vistaTbl = new VistaDBTable();
		/// 
		///		vistaDB.DataPath = @"D:\Database\data.vdb";
		///		vistaDB.Open();
		///		
		///		vistaTbl.Database = vistaDB;
		///		vistaTbl.Open();
		///		
		///		//Insert new string with the 
		///		vistaTbl.Insert();
		///		vistaTbl.PutString("FirstName", "Pavel");
		/// 
		///		string alias = vistaTbl.Alias();
		///		System.Console.WriteLine("Alias = " + alias);
		/// 
		///		vistaTbl.Close();
		///		vistaDB.Close();
		///	}
		/// </code>
		/// </example>		
		public void Insert()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_AppendBlank();
			}
		}

		/// <summary>
		/// Returns true if index exists.
		/// </summary>
		/// <param name="name">Index name</param>
		/// <returns>True if index exists, else false</returns>
		public bool IsIndexExists(string name)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return (VistaDBAPI.ivdb_IndexOrderByName(name) > 0);
			}
		}

		/// <summary>
		/// Returns true if trigger exists
		/// </summary>
		/// <param name="triggerName">Trigger name</param>
		/// <returns>Return true if trigger exists, else false</returns>
		public bool IsTriggerExist(string triggerName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_IsTriggerExist(triggerName);
			}
		}
		
		/// <summary>
		/// Move to the last row in the table
		/// </summary>
		public void Last()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_RowLast();
			}
		}

		/// <summary>
		/// Load data from the table
		/// </summary>
		/// <param name="table">VistaDBTable object</param>
		/// <returns>True if succed, otherwise false</returns>
		public bool LoadFromTable(VistaDBTable table)
		{
			if(table == null || !table.Opened || !this.Opened)
				throw new VistaDBException(VistaDBErrorCodes.TableNotOpened);

			lock(this.database.SyncRoot)
			{
				VistaDBAPI.ivdb_Select((ushort)table.TableID);
				return VistaDBAPI.ivdb_ImportToTable(this.tableId, null);
			}
		}

		/// <summary>
		/// Locks row by row ID
		/// </summary>
		/// <param name="rowID">Row ID</param>
		/// <returns>True if row locked successfully and false if no</returns>
		public bool LockRow(long rowID)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_RowLock((uint)rowID);
			}
		}

		/// <summary>
		/// Locks current row
		/// </summary>
		/// <returns>True if row locked successfully and false if no</returns>
		public bool LockRow()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_RowLock((uint)(VistaDBAPI.ivdb_RowId()));
			}
		}

		/// <summary>
		/// Move forward or backward a specified number of rows
		/// </summary>
		/// <param name="bypassRecs">The number of rows to move</param>
		public void MoveBy(int bypassRecs)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_MoveBy(bypassRecs);
			}
		}

		/// <summary>
		/// Move to the specified row
		/// </summary>
		/// <param name="rowID">Row ID</param>
		public void MoveTo(long rowID)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_GoFiltered((uint)rowID);
			}
		}
		
		/// <summary>
		/// Move to the next row
		/// </summary>
		public void Next()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_RowNext();
			}
		}

		/// <summary>
		/// Open table
		/// </summary>
		/// <returns>Return true if success</returns>
		public bool Open()
		{
			tableId = 0;

			if ( database == null )
			{
				throw new VistaDBException(VistaDBErrorCodes.DatabaseNotAssigned);
			}

			lock(database.SyncRoot)
			{
				if (! ConnectDatabase() || database.DatabaseId == 0)
					return false;

				tableId = VistaDBAPI.ivdb_OpenTable(tableName, "", 0, 1);

				if( tableId > 0 )
				{
					CreateColumnInfo();
				}

				return tableId > 0;
			}
		}

		/// <summary>
		/// Writes the contents of the row buffer to the database.
		/// </summary>
		public void Post()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_Commit();
			}
		}

		/// <summary>
		/// Move to the previous row
		/// </summary>
		public void Prior()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_RowPrior();
			}
		}

		/// <summary>
		/// Stores a BLOB in a column. A BLOB is a binary large object.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="vpData">Blob data</param>
		/// <returns>The number of bytes written.</returns>
		public int PutBlob(string sColumnName, byte[] vpData)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				int lSize = vpData.Length;
				return VistaDBAPI.ivdb_PutBlob(sColumnName, vpData, lSize);
			}
		}

		/// <summary>
		/// Stores a BLOB in a column. A BLOB is a binary large object.
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <param name="vpData">Blob data</param>
		/// <returns>The number of bytes written.</returns>
		public int PutBlob(int iColumnIndex, byte[] vpData)
		{
			lock(database.SyncRoot)
			{
				GetFocus();

				int lSize = vpData.Length;

				if(iColumnIndex < 0 || iColumnIndex >= columnsInfo.Count)
					throw new VistaDBException(VistaDBErrorCodes.ColumnNotExist);

				return VistaDBAPI.ivdb_PutBlob(((VistaDBColumn)(columnsInfo[(int)iColumnIndex])).Name, vpData, lSize);
			}
		}

		/// <summary>
		/// Put BLOB info from file to database
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="sFileName">Source file name</param>
		/// <returns>True for success else False</returns>
		public bool PutBlobFromFile(string sColumnName, string sFileName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();		
				return VistaDBAPI.ivdb_PutBlobFromFile(sColumnName, sFileName);
			}
		}

		/// <summary>
		/// Put BLOB info from file to database
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <param name="sFileName">Source file name</param>
		/// <returns>True for success else False</returns>
		public bool PutBlobFromFile(int iColumnIndex, string sFileName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
			
				if(iColumnIndex < 0 || iColumnIndex >= columnsInfo.Count)
					throw new VistaDBException(VistaDBErrorCodes.ColumnNotExist);

				return VistaDBAPI.ivdb_PutBlobFromFile(((VistaDBColumn)(columnsInfo[(int)iColumnIndex])).Name, sFileName);
			}
		}

		/// <summary>
		/// Stores boolean value in a column.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="bVal">Boolean value</param>
		public void PutBoolean(string sColumnName, bool bVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutBoolean(sColumnName, bVal);
			}
		}

		/// <summary>
		/// Stores boolean value in a column.
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <param name="bVal">Boolean value</param>
		public void PutBoolean(int iColumnIndex, bool bVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutBooleanByIndex(iColumnIndex + 1, bVal);
			}
		}

		/// <summary>
		/// Stores currency value in a column.
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <param name="dVal">Currency value</param>
		public void PutCurrency(int iColumnIndex, decimal dVal)
		{
			long longValue;
			longValue = (long)(dVal * 10000);

			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutCurrencyByIndex(iColumnIndex + 1, longValue);
			}
		}

		/// <summary>
		/// Stores currency value in a column.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="dVal">Currency value</param>
		public void PutCurrency(string sColumnName, decimal dVal)
		{
			long longValue;
			longValue = (long)(dVal * 10000);

			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutCurrency(sColumnName, longValue);
			}
		}

		/// <summary>
		/// Stores date value in a column.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="dVal">Date and time value</param>
		public void PutDate(string sColumnName, DateTime dVal)
		{
			PutDateTime(sColumnName, dVal);
		}

		/// <summary>
		/// Stores date value in a column.
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <param name="dVal">Date and time value</param>
		public void PutDate(int iColumnIndex, DateTime dVal)
		{
			PutDateTime(iColumnIndex, dVal);
		}

		/// <summary>
		/// Stores date and time value in a column.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="dVal">Date and time value</param>
		public void PutDateTime(string sColumnName, DateTime dVal)
		{
			long t;
			double d;

			lock(database.SyncRoot)
			{
				GetFocus();

				t = dVal.Ticks;

				d = (double)(t/((double)24*60*60*10000000)) + 1;

				VistaDBAPI.ivdb_PutDateTime(sColumnName, d);
			}
		}

		/// <summary>
		/// Stores date and time value in a column.
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <param name="dVal">Date and time value</param>
		public void PutDateTime(int iColumnIndex, DateTime dVal)
		{
			long t;
			double d;

			lock(database.SyncRoot)
			{
				GetFocus();

				t = dVal.Ticks;

				d = (double)(t/((double)24*60*60*10000000)) + 1;

				VistaDBAPI.ivdb_PutDateTimeByIndex(iColumnIndex + 1, d);
			}
		}

		/// <summary>
		/// Stores double value in a column.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="dVal">Double value</param>
		public void PutDouble(string sColumnName, double dVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutDouble(sColumnName, dVal);
			}
		}

		/// <summary>
		/// Stores double value in a column.
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <param name="dVal">Double value</param>
		public void PutDouble(int iColumnIndex, double dVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutDoubleByIndex(iColumnIndex + 1, dVal);
			}
		}

		/// <summary>
		/// Stores Guid value in a column
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="guidVal">Guid value</param>
		public void PutGuid(string sColumnName, Guid guidVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutGuid(sColumnName, guidVal);
			}
		}

		/// <summary>
		/// Stores Guid value in a column
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <param name="guidVal">Guid value</param>
		public void PutGuid(int iColumnIndex, Guid guidVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutGuidByIndex(iColumnIndex + 1, guidVal);
			}
		}

		/// <summary>
		/// Stores 32-bit integer value in a column.
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="iVal">Integer value</param>
		public void PutInt32(string sColumnName, int iVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutInt32(sColumnName, iVal);
			}
		}

		/// <summary>
		/// Stores 32-bit integer value in a column.
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <param name="iVal">Integer value</param>
		public void PutInt32(int iColumnIndex, int iVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutInt32ByIndex(iColumnIndex + 1, iVal);
			}
		}

		/// <summary>
		/// Stores long value (64-bit integer value) in a column
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="iVal">Long value</param>
		public void PutInt64(string sColumnName, long iVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutInt64(sColumnName, iVal);
			}
		}

		/// <summary>
		/// Stores long value (64-bit integer value) in a column
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <param name="iVal">Long value</param>
		public void PutInt64(int iColumnIndex, long iVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutInt64ByIndex(iColumnIndex + 1, iVal);
			}
		}

		/// <summary>
		/// Stores memo data in a column
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="sVal">Memo data</param>
		public void PutMemo(string sColumnName, string sVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutMemo(sColumnName, sVal);
			}
		}

		/// <summary>
		/// Stores memo data in a column
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <param name="sVal">Memo data</param>
		public void PutMemo(int iColumnIndex, string sVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutMemoByIndex(iColumnIndex + 1, sVal);
			}
		}

		/// <summary>
		/// Stores string in a column
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="sVal">String data</param>
		public void PutString(string sColumnName, string sVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutString(sColumnName, sVal);
			}
		}

		/// <summary>
		/// Stores string in a column
		/// </summary>
		/// <param name="iColumnIndex">Column index (0-based)</param>
		/// <param name="sVal">String data</param>
		public void PutString(int iColumnIndex, string sVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutStringByIndex(iColumnIndex + 1, sVal);
			}
		}

		/// <summary>
		/// Reindex all indexes in the table
		/// </summary>
		public void Reindex()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_ReindexTable();
			}
		}

		/// <summary>
		/// Rename table
		/// </summary>
		/// <param name="newName">New table name</param>
		/// <returns>True if table renamed</returns>
		public bool RenameTable(string newName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				bool res = VistaDBAPI.ivdb_RenameCurrentTable(newName);
				if(res)
					tableName = newName;
				return res;
			}
		}

		/// <summary>
		/// Clear filter for table
		/// </summary>
		/// <returns>Return true if success</returns>
		public bool ResetFilter()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_SqlQuery("");
			}
		}

		/// <summary>
		/// Return row count
		/// </summary>
		/// <returns>Return row count</returns>
		public long RowCount()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return (long)VistaDBAPI.ivdb_QueryRowCount();
			}
		}

		/// <summary>
		/// Return ID for current row
		/// </summary>
		/// <returns>Row ID</returns>
		public long CurrentRowID()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return (long)(VistaDBAPI.ivdb_RowId());
			}
		}

		/// <summary>
		/// Return lock status by row ID
		/// </summary>
		/// <param name="rowID">Row ID</param>
		/// <returns>True if row locked, else false</returns>
		public bool RowLocked(long rowID)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_RowLocked((uint)rowID);
			}
		}

		/// <summary>
		/// Return lock status for current row
		/// </summary>
		/// <returns>True if row is locked, else false</returns>
		public bool RowLocked()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_RowLocked((uint)(VistaDBAPI.ivdb_RowId()));
			}
		}

		/// <summary>
		/// Save table data to another table
		/// </summary>
		/// <param name="table">VistaDBTable object</param>
		/// <returns>True if succed, otherwise false</returns>
		public bool SaveToDatabase(VistaDBTable table)
		{
			if(table == null || !table.Opened || !this.Opened)
				throw new VistaDBException(VistaDBErrorCodes.TableNotOpened);

			table.DeleteAllRows(false);

			lock(this.database.SyncRoot)
			{
				VistaDBAPI.ivdb_Select((ushort)this.tableId);
				return VistaDBAPI.ivdb_ImportToTable(table.TableID, null);
			}
		}

		private void Select()
		{
			VistaDBAPI.ivdb_Select((ushort)tableId);
		}

		/// <summary>
		/// Put VistaDB type column value to database
		/// </summary>
		/// <param name="columnIndex">Column index (0-based)</param>
		/// <param name="value">Column data value</param>
		public void PutObject(int columnIndex, object value)
		{
			VistaDBColumn column = Columns[columnIndex];

			if(column == null)
				return;

			if(value == null || value == DBNull.Value)
			{
				SetNull(columnIndex);
			}
			else
			{
				switch(column.VistaDBType)
				{
					case VistaDBType.Blob:
					case VistaDBType.Picture:
						PutBlob(columnIndex, (byte[])value);
						break;
					case VistaDBType.Boolean:
						PutBoolean(columnIndex, (bool)value);
						break;
					case VistaDBType.Character:
					case VistaDBType.Varchar:
						PutString(columnIndex, (string)value);
						break;
					case VistaDBType.Currency:
						PutCurrency(columnIndex, (decimal)value);
						break;
					case VistaDBType.Date:
						PutDate(columnIndex, (DateTime)value);
						break;
					case VistaDBType.DateTime:
						PutDateTime(columnIndex, (DateTime)value);
						break;
					case VistaDBType.Double:
						PutDouble(columnIndex, (double)value);
						break;
					case VistaDBType.Guid:
						PutGuid(columnIndex, (Guid)value);
						break;
					case VistaDBType.Int32:
						PutInt32(columnIndex, (int)value);
						break;
					case VistaDBType.Int64:
						PutInt64(columnIndex, (long)value);
						break;
					case VistaDBType.Memo:
						PutMemo(columnIndex, (string)value);
						break;
				}
			}
		}

		/// <summary>
		/// Put VistaDB type column value to database
		/// </summary>
		/// <param name="columnName">Column name</param>
		/// <param name="value">Column data value</param>
		public void PutObject(string columnName, object value)
		{
			object columnIndex = columns[columnName.ToUpper()];

			if(columnIndex == null)
				throw new VistaDBException( VistaDBErrorCodes.ColumnNotExist );

			PutObject((int)columnIndex, value);
		}

		/// <summary>
		/// Set column caption
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="sCaption">Caption for column</param>
		/// <returns>Return true if success</returns>
		public bool SetColumnCaption(string sColumnName, string sCaption)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_SetColumnCaption(sColumnName, sCaption);
			}
		}

		/// <summary>
		/// Set constraint
		/// </summary>
		/// <param name="sName">Constraint name</param>
		/// <param name="sConstraint">Constraint text</param>
		/// <param name="sConstraintDescr">Constraint description</param>
		/// <returns>True for success</returns>
		public bool SetConstraint(string sName, string sConstraint, string sConstraintDescr)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_SetConstraint(sName, sConstraint, sConstraintDescr);
			}
		}

		/// <summary>
		/// Set default value for column
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="sSeed">Default value or V-Script function</param>
		/// <param name="bUseInUpdate">Use this default value in updates. 
		/// Useful when combined with V-Script functions that generate dynamic values, such as the DATE() function.</param>
		/// <returns>True for success</returns>
		public bool SetDefaultValue(string sColumnName, string sSeed, bool bUseInUpdate)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_SetDefaultValue(sColumnName, sSeed, bUseInUpdate);
			}
		}

		/// <summary>
		/// Set filter for table
		/// </summary>
		/// <param name="sExpression">V-Script filter expression</param>
		/// <returns>True for success</returns>
		public bool SetFilter(string sExpression)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_SqlQuery(sExpression);
			}
		}

		/// <summary>
		/// Set filter for table with "use/no use" optimization option
		/// </summary>
		/// <param name="sExpression">Filter expression</param>
		/// <param name="bUseOptimization">If true, then use optimization else not use</param>
		/// <returns>If success then return true</returns>
		public bool SetFilter(string sExpression, bool bUseOptimization)
		{
			lock(database.SyncRoot)
			{
				GetFocus();

				if( bUseOptimization )
					return VistaDBAPI.ivdb_SqlQuery(sExpression);
				else
					return VistaDBAPI.ivdb_SetFilter(sExpression);
			}
		}

		/// <summary>
		/// Set foreign key for table
		/// </summary>
		/// <param name="sForeignKeyName">Foreign key name</param>
		/// <param name="sForeignConstraint">Foreign key body</param>
		/// <param name="sDescription">Foreign key description</param>
		/// <returns>True if success</returns>
		public bool SetForeignKey(string sForeignKeyName, string sForeignConstraint, string sDescription)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_SetForeignKey(sForeignKeyName, sForeignConstraint, sDescription, 0);
			}
		}

		/// <summary>
		/// Set foreign key with options 
		/// </summary>
		/// <param name="sForeignKeyName">Foreign key name</param>
		/// <param name="sForeignConstraint">Foreign key body</param>
		/// <param name="sDescription">Foreign key description</param>
		/// <param name="options">Option for foreign key</param>
		/// <returns>True if success</returns>
		public bool SetForeignKey(string sForeignKeyName, string sForeignConstraint, string sDescription, VDBForeignKeyOptions options)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_SetForeignKey(sForeignKeyName, sForeignConstraint, sDescription, (uint)options);
			}
		}

		/// <summary>
		/// Set identity
		/// </summary>
		/// <param name="sColumnName">Column name</param>
		/// <param name="sSeedExpr">Seed expression</param>
		/// <param name="dStep">Identity step</param>
		/// <returns>True for success</returns>
		public bool SetIdentity(string sColumnName, string sSeedExpr, double dStep)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_SetIdentity(sColumnName, sSeedExpr, dStep);
			}
		}

		/// <summary>
		/// Sets lock timeout
		/// </summary>
		/// <param name="value">Lock timeout int seconds</param>
		public void SetLockTimeout(short value)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_SetLockTimeout(value);
			}
		}
		
		/// <summary>
		/// Set null value for column data
		/// </summary>
		/// <param name="columnName">Column name</param>
		public void SetNull(string columnName)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_PutNull(columnName);
			}
		}

		/// <summary>
		/// Set null value for column data
		/// </summary>
		/// <param name="columnIndex">Column index (0-based)</param>
		public void SetNull(int columnIndex)
		{
			VistaDBColumn column = Columns[columnIndex];

			if(column == null)
				return;

			SetNull(column.Name);
		}

		/// <summary>
		/// Set a scope filter on the table. Scope must act on indexed columns.
		/// </summary>
		/// <param name="sLowVal">Low border</param>
		/// <param name="sHighVal">High border</param>
		/// <returns>Return true if success</returns>
		public bool SetScope(string sLowVal, string sHighVal)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_SetScope(sLowVal, sHighVal);
			}
		}

		/// <summary>
		/// Set a scope filter on the table. Scope must act on indexed columns.
		/// </summary>
		/// <param name="val">High and low border</param>
		/// <returns>Return true if success</returns>
		public bool SetScope(string val)
		{
			return SetScope(val, val);
		}

		/// <summary>
		/// Create a new Trigger.
		/// </summary>
		/// <param name="sName">Trigger name</param>
		/// <param name="sTrigger">Trigger V-Script</param>
		/// <param name="sTriggerDescr">Trigger description</param>
		/// <param name="EventOption">Event options</param>
		/// <param name="iPriority">Trigger priority</param>
		/// <returns>True for success</returns>
		public bool SetTrigger(string sName, string sTrigger, string sTriggerDescr, VDBTriggerEvent EventOption, int iPriority)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				return VistaDBAPI.ivdb_SetTrigger(sName, sTrigger, sTriggerDescr, (uint) EventOption, iPriority);
			}
		}

		/// <summary>
		/// Unlock specified row
		/// </summary>
		/// <param name="RowID">Row ID</param>
		public void UnlockRow(int RowID)
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_RowUnlock((uint)RowID);
			}
		}

		/// <summary>
		/// Unlock current row
		/// </summary>
		public void UnlockRow()
		{
			lock(database.SyncRoot)
			{
				GetFocus();
				VistaDBAPI.ivdb_RowUnlock(VistaDBAPI.ivdb_RowId());
			}
		}
		
		/////////////////////////////////////////////////////////////////////
		///////////////////////////PROPERTIES////////////////////////////////
		/////////////////////////////////////////////////////////////////////
		
		/// <summary>
		/// Gets or sets the active index.
		/// </summary>
		public string ActiveIndex
		{
			set
			{
				short indexOrder;

				lock(database.SyncRoot)
				{
					GetFocus();

					indexOrder = VistaDBAPI.ivdb_IndexOrderByName(value);

					if( indexOrder > 0 )
						VistaDBAPI.ivdb_SetOrder(indexOrder);
					else
						VistaDBAPI.ivdb_SetOrder(0);
				}
			}
			get
			{
				if(!this.Opened)
					return null;

				short indexOrder;

				lock(database.SyncRoot)
				{
					GetFocus();
					indexOrder = VistaDBAPI.ivdb_IndexOrder();

					if( indexOrder > 0 )
					{
						StringBuilder buf = new StringBuilder(VistaDBAPI.MAX_DESCRIPTION_SIZE);
						VistaDBAPI.ivdb_IndexNameManaged(indexOrder, buf, VistaDBAPI.MAX_DESCRIPTION_SIZE);
						return VistaDBAPI.CutString(buf);
					}
					else
						return null;
				}
			}
		}

		/// <summary>
		/// Gets the collection of columns
		/// </summary>
//		[Editor(typeof(ArrayEditor), typeof(CollectionEditor))]
		public ColumnCollection Columns
		{
			get
			{
				return columnCollection;
			}
		}
	
		/// <summary>
		/// Gets the collection of active constraints. 
		/// </summary>
		public ConstraintActiveCollection ConstraintActive
		{
			get
			{
				return constraintActive;
			}
		}

		/// <summary>
		/// Gets or sets the database object (VistaDBDatabase).
		/// </summary>
		public VistaDBDatabase Database
		{
			get
			{
				return database;
			}
			set
			{
				if( database == value )
					return;

				if( tableId != 0 )
				{
					throw new VistaDBException( VistaDBErrorCodes.DatabaseCanNotBeChanged );
				}

				if( database != null )
					database.UnregisterTable(this);

				if(value != null)
				{
					database = value;
					database.RegisterTable(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets the table description.
		/// </summary>
		public string Description
		{
			get
			{
				lock(database.SyncRoot)
				{
					GetFocus();
					return VistaDBAPI.ivdb_GetTableDescription();
				}
			}
			set
			{
				lock(database.SyncRoot)
				{
					GetFocus();
					VistaDBAPI.ivdb_SetTableDescription(value);
				}
			}
		}
		
		/// <summary>
		/// Gets the table state. True if the is table opened.
		/// </summary>
		public bool Opened
		{
			get
			{
				return tableId > 0;
			}
		}
		
		/// <summary>
		/// Gets the numeric Table ID associated with the opened table. This property is the inverse of Alias, which retrieves the alias name assigned to a given Table ID.
		/// </summary>
		public short TableID
		{
			get
			{
				return tableId;
			}
		}

		/// <summary>
		/// Gets or sets the table name
		/// </summary>
		public string TableName
		{
			get
			{
				return tableName.Trim();
			}
			set
			{
				tableName = value;
			}
		}

		/// <summary>
		/// Gets the collection of active triggers. 
		/// </summary>
		public TriggerActiveCollection TriggerActive
		{
			get
			{
				return triggerActive;
			}
		}
	
		private VistaDBDatabase database = null;
		private short	tableId = 0;
		private string	tableName = "";
		private Hashtable columns = null;
		private ArrayList columnsInfo = null;
		private ColumnCollection columnCollection;
		private ConstraintActiveCollection constraintActive;
		private TriggerActiveCollection triggerActive;
	}
}