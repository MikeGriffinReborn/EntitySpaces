using System;
using System.Text;

namespace Provider.VistaDB
{
	#region Meta data structures
	/// <summary>
	/// Column info structure
	/// </summary>
	public struct VDBColumnInfo
	{
		/// <summary>
		/// Column name
		/// </summary>
		public string Name;
		/// <summary>
		/// Column caption
		/// </summary>
		public string Caption;
		/// <summary>
		/// Column data type
		/// </summary>
		public VistaDBType DataType;
		/// <summary>
		/// Column width
		/// </summary>
		public int Width;
		/// <summary>
		/// Column decimals
		/// </summary>
		public int Decimals;
		/// <summary>
		/// Returns True if column has identity
		/// </summary>
		public bool Identity;
		/// <summary>
		/// Column identity increment step
		/// </summary>
		public double IncStep;
		/// <summary>
		/// Column default value or current identity value, if column has identity
		/// </summary>
		public string DefValue;
		/// <summary>
		/// Returns True if default value applyes for update operations
		/// </summary>
		public bool UseDefValInUpdate;
		/// <summary>
		/// Return True if column allow null
		/// </summary>
		public bool AllowNull;
		/// <summary>
		/// Column description
		/// </summary>
		public string Description;
		/// <summary>
		/// Returns True if column is read only
		/// </summary>
		public bool ReadOnly;
		/// <summary>
		/// Returns True if column is compressed
		/// </summary>
		public bool Compressed;
		/// <summary>
		/// Returns True if column has FTS index
		/// </summary>
		public bool FTS;
		/// <summary>
		/// Returns True if column encrypted
		/// </summary>
		public bool Encrypted;
		/// <summary>
		/// Returns true if column is hidden
		/// </summary>
		public bool Hidden;
		/// <summary>
		/// Returns True if column is the part of primary key
		/// </summary>
		public bool PrimaryKey;
		/// <summary>
		/// Returns True if column indexed
		/// </summary>
		public bool Indexed;
		/// <summary>
		/// Not supported now. Always returns False
		/// </summary>
		public bool Unicode;
	}

	/// <summary>
	/// Index info structure
	/// </summary>
	public struct VDBIndexInfo
	{
		/// <summary>
		/// Index name
		/// </summary>
		public string Name;
		/// <summary>
		/// Returns True if index is standard (none V-Index)
		/// </summary>
		public bool Standard;
		/// <summary>
		/// Fields list, separated by semicolon
		/// </summary>
		public string Fields;
		/// <summary>
		/// Return True if index is unique
		/// </summary>
		public bool Unique;
		/// <summary>
		/// Return true if index is primary key
		/// </summary>
		public bool PrimaryKey;
		/// <summary>
		/// Return true if index has descend order
		/// </summary>
		public bool Desc;
		/// <summary>
		/// Return true is index is case sensitive
		/// </summary>
		public bool CaseSens;
		/// <summary>
		/// Condition, used only for V-Indexes
		/// </summary>
		public string Condition;
		/// <summary>
		/// Returns true if index is FTS
		/// </summary>
		public bool FTS;
		/// <summary>
		/// Locale used for index
		/// </summary>
		public long Locale;
	}

	/// <summary>
	/// Trigger info structure
	/// </summary>
	public struct VDBTriggerInfo
	{
		/// <summary>
		/// Trigger name
		/// </summary>
		public string Name;
		/// <summary>
		/// Trigger event
		/// </summary>
		public VDBTriggerEvent Event;
		/// <summary>
		/// Trigger priority
		/// </summary>
		public int Priority;
		/// <summary>
		/// Returns True if trigger is active
		/// </summary>
		public bool Active;
		/// <summary>
		/// Trigger description
		/// </summary>
		public string Description;
		/// <summary>
		/// Trigger body
		/// </summary>
		public string Body;
	}

	/// <summary>
	/// Constraint info structure
	/// </summary>
	public struct VDBConstraintInfo
	{
		/// <summary>
		/// Constraint name
		/// </summary>
		public string Name;
		/// <summary>
		/// Returns True if constraint is active
		/// </summary>
		public bool Active;
		/// <summary>
		/// Constraint description
		/// </summary>
		public string Description;
		/// <summary>
		/// Constraint body
		/// </summary>
		public string Body;
	}

	/// <summary>
	/// Relationship info structure
	/// </summary>
	public struct VDBRelationshipInfo
	{
		/// <summary>
		/// Relationship name
		/// </summary>
		public string Name;
		/// <summary>
		/// Primry table name
		/// </summary>
		public string PrimTable;
		/// <summary>
		/// Relationship decsription
		/// </summary>
		public string Description;
		/// <summary>
		/// Relationship primary key columns
		/// </summary>
		public string[] PrimKeyColumns;
		/// <summary>
		/// Relationship foreign key columns
		/// </summary>
		public string[] ForKeyColumns;
		/// <summary>
		/// Relationship body
		/// </summary>
		public string Body;
		/// <summary>
		/// Returns True if relationship support cascade update
		/// </summary>
		public bool CascadeUpdate;
		/// <summary>
		/// Returns True if relationship support cascade delete
		/// </summary>
		public bool CascadeDelete;
	}
	#endregion Meta data structures

	/// <summary>
	/// Meta data reader class
	/// </summary>
	public class VistaDBMetaDataReader
	{
		#region Fields
		private const int SIZE_INT    = 4;
		private const int SIZE_DOUBLE = 8;

		private byte[] buffer;
		private int size;
		private int position;
		#endregion Fields

		#region Private methods
		private void SetBufferSize(int size)
		{
			this.buffer    = new byte[size];
			this.size = size;
		}

		private int GetInteger(int offset)
		{
			if(offset == -1)
			{
				offset = this.position;
				this.position += SIZE_INT;
			}

			return BitConverter.ToInt32(this.buffer, offset);
		}

		private int GetInteger()
		{
			return GetInteger(-1);
		}

		private string GetString(int offset)
		{
			int len;
			Encoding encoding = Encoding.Default;

			if(offset == -1)
			{
				offset = this.position;
				len = GetInteger(offset);
				this.position += SIZE_INT + len;
			}
			else
				len = GetInteger(offset);

			offset += SIZE_INT;

			return encoding.GetString(this.buffer, offset, len);
		}

		private string GetString()
		{
			return GetString(-1);
		}

		private char GetChar(int offset)
		{
			if(offset == -1)
			{
				offset = this.position;
				this.position++;
			}

			return (char)buffer[offset];
		}

		private char GetChar()
		{
			return GetChar(-1);
		}

		private double GetDouble(int offset)
		{
			if(offset == -1)
			{
				offset = this.position;
				this.position += SIZE_DOUBLE;
			}

			return BitConverter.ToDouble(this.buffer, offset);
		}

		private double GetDouble()
		{
			return GetDouble(-1);
		}

		private bool GetBool(int offset)
		{
			if(offset == -1)
			{
				offset = this.position;
				this.position++;
			}

			return this.buffer[offset] != 0;
		}

		private bool GetBool()
		{
			return GetBool(-1);
		}

		private void FindFirstTable()
		{
			this.position = 4;
		}

		private void FindNextTable()
		{
			this.position += GetInteger(this.position);
		}

		private bool FindTable(string tableName)
		{
			int tablesCount;
			int position;
			bool res;

			tableName = tableName.ToUpper();

			if(GetCurTableName().ToUpper() == tableName)
				return true;

			position  = this.position;
			res = false;

			try
			{
				FindFirstTable();

				tablesCount = GetTablesCount();

				for(int i = 0; i < tablesCount; i++)
				{
					if(GetCurTableName().ToUpper() == tableName)
					{
						res = true;
						break;
					}

					FindNextTable();
				}
			}
			finally
			{
				if(!res)
					this.position = position;
			}

			return res;
		}

		private string GetCurTableName()
		{
			return GetString(this.position + SIZE_INT);
		}

		private int GetTablesCount()
		{
			return GetInteger(0);
		}

		private void FindColumnSection()
		{
			GetInteger();
			GetString();
		}

		private void FindIndexSection()
		{
			FindColumnSection();
			int size = GetInteger();
			this.position += size;
		}

		private void FindTriggerSection()
		{
			FindIndexSection();
			int size = GetInteger();
			this.position += size;
		}

		private void FindConstraintSection()
		{
			FindTriggerSection();
			int size = GetInteger();
			this.position += size;
		}

		private void FindRelationshipSection()
		{
			FindConstraintSection();
			int size = GetInteger();
			this.position += size;
		}

		private string[] ExtractColumnNamesFromExpr(string expr)
		{
			int start, end, columnsCount;
			string s;
			string[] columns;
			int index;

			start = 0;
			end   = 0;

			//Calc columns count
			columnsCount = 0;
			while(end > 0)
			{
				end = expr.IndexOf(";", start);
				if(end > 0)
				{
					start = end + 1;
					columnsCount++;
				}
			}

			if(start < expr.Length - 1)
				columnsCount++;

			//Read columns
			columns = new string[columnsCount];
			index   = 0;
			start   = 0;
			end     = 0;

			while(end >= 0)
			{
				end = expr.IndexOf(";", start);
				if(end > 0)
				{
					s          = expr.Substring(start, end - start).Trim();
					columns[index] = s;
					start      = end + 1;
					index++;
				}
			}

			if(index < columnsCount)
			{
				s          = expr.Substring(start, expr.Length - start).Trim();
				columns[index] = s;
			}

			return columns;
		}
		#endregion Private methods

		#region Public methods
		/// <summary>
		/// Enumerate columns info
		/// </summary>
		/// <param name="tableName">Table name</param>
		/// <returns>Columns info list</returns>
		public VDBColumnInfo[] EnumColumns(string tableName)
		{
			int position;
			int columnsCount;
			VDBColumnInfo[] columns;

			if(!FindTable(tableName))
				return null;

			position = this.position;

			try
			{
				FindColumnSection();
				GetInteger();
				columnsCount = GetInteger();
				columns      = new VDBColumnInfo[columnsCount];
    
				for(int i = 0; i< columnsCount; i++)
				{
					columns[i].Name               = GetString();
					columns[i].Caption            = GetString();
					columns[i].DataType           = VistaDBAPI.NetDataType(GetChar().ToString());
					columns[i].Width              = GetInteger();
					columns[i].Decimals           = GetInteger();

					columns[i].DefValue           = GetString();
					columns[i].Identity           = GetBool();
					columns[i].IncStep            = GetDouble();
					columns[i].UseDefValInUpdate  = GetBool();

					columns[i].AllowNull          = GetBool();
					columns[i].Description        = GetString();
					columns[i].ReadOnly           = GetBool();
					columns[i].Compressed         = GetBool();
					columns[i].Encrypted          = GetBool();
					columns[i].Hidden             = GetBool();
					columns[i].PrimaryKey         = GetBool();
					columns[i].Indexed            = GetBool();
					columns[i].Unicode            = GetBool();

					columns[i].FTS                = GetBool();
				}
			}
			finally
			{
				this.position = position;
			}

			return columns;
		}

		/// <summary>
		/// Enumerate constraints info
		/// </summary>
		/// <param name="tableName">Table name</param>
		/// <returns>Constraints info list</returns>
		public VDBConstraintInfo[] EnumConstraints(string tableName)
		{
			int constraintsCount, position;
			VDBConstraintInfo[] constraints;

			if(!FindTable(tableName))
				return null;

			position = this.position;

			try
			{
				FindConstraintSection();
				GetInteger();
				constraintsCount = GetInteger();
				constraints = new VDBConstraintInfo[constraintsCount];

				for(int i = 0; i < constraintsCount; i++)
				{
					constraints[i].Name        = GetString();
					constraints[i].Active      = GetBool();
					constraints[i].Description = GetString();
					constraints[i].Body        = GetString();
				}
			}
			finally
			{
				this.position = position;
			}

			return constraints;
		}

		/// <summary>
		/// Enumerate indexes info
		/// </summary>
		/// <param name="tableName">Table name</param>
		/// <returns>Indexes info list</returns>
		public VDBIndexInfo[] EnumIndexes(string tableName)
		{
			int indexesCount, position;
			VDBIndexInfo[] indexes;

			if(!FindTable(tableName))
				return null;

			position = this.position;

			try
			{
				FindIndexSection();
				GetInteger();
				indexesCount = GetInteger();
				indexes      = new VDBIndexInfo[indexesCount];

				for(int i = 0; i < indexesCount; i++)
				{
					indexes[i].Name       = GetString();
					indexes[i].Standard   = GetBool();
					indexes[i].Fields     = GetString();
					indexes[i].Unique     = GetBool();
					indexes[i].PrimaryKey = GetBool();
					indexes[i].Desc       = GetBool();
					indexes[i].CaseSens   = GetBool();
					indexes[i].Condition  = GetString();
					indexes[i].FTS        = GetBool();
					indexes[i].Locale     = GetInteger();
				}
			}
			finally
			{
				this.position = position;
			}

			return indexes;
		}

		/// <summary>
		/// Enumerate relationships info
		/// </summary>
		/// <param name="tableName">Table name</param>
		/// <returns>Relationships info list</returns>
		public VDBRelationshipInfo[] EnumRelationships(string tableName)
		{
			int relationshipsCount, position;
			VDBRelationshipInfo[] relationships;

			if(!FindTable(tableName))
				return null;

			position = this.position;

			try
			{
				FindRelationshipSection();
				GetInteger();
				relationshipsCount = GetInteger();
				relationships      = new VDBRelationshipInfo[relationshipsCount];

				for(int i = 0; i < relationshipsCount; i++)
				{
					relationships[i].Name           = GetString();

					relationships[i].Description    = GetString();

					relationships[i].PrimTable      = GetString();
					relationships[i].PrimKeyColumns = ExtractColumnNamesFromExpr(GetString());
					relationships[i].ForKeyColumns  = ExtractColumnNamesFromExpr(GetString());

					relationships[i].CascadeUpdate  = GetBool();
					relationships[i].CascadeDelete  = GetBool();

					relationships[i].Body           = GetString();
				}
			}
			finally
			{
				this.position = position;
			}

			return relationships;
		}

		/// <summary>
		/// Enumerate table names
		/// </summary>
		/// <returns>Table names list</returns>
		public string[] EnumTables()
		{
			int tablesCount;
			int position;			
			string[] tables;

			position = this.position;

			try
			{
				tablesCount = GetTablesCount();
				tables      = new string[tablesCount];

				FindFirstTable();

				for(int i = 0; i < tablesCount; i++)
				{
					tables[i] = GetCurTableName();
					FindNextTable();
				}
			}
			finally
			{
				this.position = position;
			}

			return tables;
		}

		/// <summary>
		/// Enumerate triggers info
		/// </summary>
		/// <param name="tableName">Table name</param>
		/// <returns>Triggers info list</returns>
		public VDBTriggerInfo[] EnumTriggers(string tableName)
		{
			int triggersCount, position;
			VDBTriggerInfo[] triggers;

			if(!FindTable(tableName))
				return null;

			position = this.position;

			try
			{
				FindTriggerSection();
				GetInteger();
				triggersCount = GetInteger();
				triggers      = new VDBTriggerInfo[triggersCount];

				for(int i = 0; i < triggersCount; i++)
				{
					triggers[i].Name        = GetString();
					triggers[i].Body        = GetString();
					triggers[i].Event       = (VDBTriggerEvent)GetInteger();
					triggers[i].Priority    = GetInteger();
					triggers[i].Active      = GetBool();
					triggers[i].Description = GetString();
				}
			}
			finally
			{
				this.position = position;
			}

			return triggers;
		}

		#endregion Public methods

		#region Constructor
		internal VistaDBMetaDataReader(byte[] buffer)
		{
			this.buffer       = buffer;
			this.position     = 4;
		}
		#endregion Constructor
	}
}
