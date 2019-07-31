using System;
using System.Data;
//using System.Windows.Forms;
using System.Text;
using System.ComponentModel;

namespace Provider.VistaDB
{

	/// <summary>
	/// Table column definition.
	/// </summary>
	public class VistaDBColumn
	{
		private string name;
		private VistaDBType vistaDBType;
		private Type type;
		private int dataSize;
		private short columnWidth;
		private short columnDecimals;//
		private bool allowNull;
		private bool readOnly;
		private bool primaryKey;
		private bool unique;
		private bool identity;
		private double identityStep;//
		private string identityValue;//
		private string columnCaption;
		private string columnDescription;//

		private bool packed;
		private bool hidden;
		private bool encrypted;
		private bool unicode;

		private bool reservedWord;
		
		internal VistaDBColumn(string _name, 
			VistaDBType _vistaDBType,
			int _dataSize,
			short _columnWidth,
			short _columnDecimals,
			bool _allowNull, 
			bool _readOnly, 
			bool _primaryKey,
			bool _unique,
			bool _identity,
			double _identityStep,
			string _identityValue,
			string _columnCaption,
			string _columnDescription,
			bool _reservedWord,
			bool _packed,
			bool _hidden,
			bool _encrypted,
			bool _unicode)
		{
			name = _name;
			vistaDBType = _vistaDBType;
			type = VistaDBAPI.GetFieldType(vistaDBType);
			dataSize = _dataSize;
			columnWidth = _columnWidth;
			columnDecimals = _columnDecimals;
			allowNull = _allowNull;
			readOnly = _readOnly;
			primaryKey = _primaryKey;
			unique = _unique;
			identity = _identity;
			identityStep = _identityStep;
			identityValue = _identityValue;
			columnCaption = _columnCaption;
			columnDescription = _columnDescription;
			reservedWord = _reservedWord;
			packed = _packed;
			hidden = _hidden;
			encrypted = _encrypted;
			unicode = _unicode;
		}

		/// <summary>
		/// Column name.
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
			}
		}

		/// <summary>
		/// VistaDBType for the column.
		/// </summary>
		public VistaDBType VistaDBType
		{
			get
			{
				return vistaDBType;
			}
			set
			{
			}
		}

		/// <summary>
		/// Column type.
		/// </summary>
		public Type Type
		{
			get
			{
				return type;
			}
			set
			{
			}
		}

		/// <summary>
		/// Physical size of column. 
		/// </summary>
		public int DataSize
		{
			get
			{
				return dataSize;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns the column width. Useful for Varchar and Character column types.
		/// </summary>
		public short ColumnWidth
		{
			get
			{
				return columnWidth;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns the column decimals.
		/// </summary>
		public short ColumnDecimals
		{
			get
			{
				return columnDecimals;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns true if column can store NULL values.
		/// </summary>
		public bool AllowNull
		{
			get
			{
				return allowNull;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns true if the column is readonly.
		/// </summary>
		public bool ReadOnly
		{
			get
			{
				return readOnly;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns true if the column participates in the Primary Key index.
		/// </summary>
		public bool PrimaryKey
		{
			get
			{
				return primaryKey;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns true if the column participates in the Unique index
		/// </summary>
		public bool Unique
		{
			get
			{
				return unique;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns true if the column is an Identity field.
		/// </summary>
		public bool Identity
		{
			get
			{
				return identity;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns increment step for identity
		/// </summary>
		public double IdentityStep
		{
			get
			{
				return identityStep;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns current value for identity
		/// </summary>
		public string IdentityValue
		{
			get
			{
				return identityValue;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns the Caption for the column.
		/// </summary>
		public string ColumnCaption
		{
			get
			{
				return columnCaption;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns the column description
		/// </summary>
		public string ColumnDescription
		{
			get
			{
				return columnDescription;
			}
			set
			{
			}
		}

		/// <summary>
		/// Returns true if column name is reserved word
		/// </summary>
		public bool ReservedWord
		{
			get
			{
				return reservedWord;
			}
			set
			{
			}
		}

		/// <summary>
		/// Return true if column is packed
		/// </summary>
		public bool Packed
		{
			get
			{
				return this.packed;
			}
			set
			{
			}
		}

		/// <summary>
		/// Return true if column marked as hidden
		/// </summary>
		public bool Hidden
		{
			get
			{
				return this.hidden;
			}
			set
			{
			}
		}

		/// <summary>
		/// Return encrypted if column encrypted
		/// </summary>
		public bool Encrypted
		{
			get
			{
				return this.encrypted;
			}
			set
			{
			}
		}

		/// <summary>
		/// Not supported property. Always return false.
		/// </summary>
		public bool Unicode
		{
			get
			{
				return this.unicode;
			}
			set
			{
			}
		}
	}

	/// <summary>
	/// V-SQL query class
	/// </summary>
	internal abstract class VistaDBSQLQuery
	{
		protected int rowsAffected;
		protected int recordCount;
		protected string commandText;
		protected bool opened;

		protected int columnCount;
		protected string password;

		protected int errorNumber;

		protected VistaDBColumn[] columns;
		protected VistaDBSQLConnection parent;

		protected static object syncRoot = new object();

		public VistaDBSQLQuery(VistaDBSQLConnection p)
		{
			rowsAffected = 0;
			recordCount = 0;
			commandText = "";
			columnCount = 0;
			password = "";
			errorNumber = 0;
			parent = p;
			opened = false;

			Columns = new VistaDBColumnCollection(this);
		}

		public abstract void CreateQuery();

		public abstract void FreeQuery();

		/// <summary>
		/// Used internally.
		/// </summary>
		public void DropQuery()
		{
			this.parent.DropQuery(this);
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~VistaDBSQLQuery()
		{
			try
			{
				Close();
			}
			finally
			{
				this.columns = null;
				FreeQuery();
			}
		}


		///////////////////////////////////////////////////////////
		////////////////Open and prepare SQL functions/////////////
		///////////////////////////////////////////////////////////

		/// <summary>
		/// Open a V-SQL query. Open is used with SELECT statements only.
		/// </summary>
		public abstract void Open();

		/// <summary>
		/// Close a V-SQL query.
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// Execute a V-SQL query that does not return a result set. These include INSERT, DELETE and UPDATE.
		/// </summary>
		public abstract void ExecSQL();


		////////////////////////////////////////////////////////////
		////////////////Data and parameter functions////////////////
		////////////////////////////////////////////////////////////

		/// <summary>
		/// Set a V-SQL parameter.
		/// </summary>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="dataType">Parameter data type.</param>
		/// <param name="value">Data value</param>
		public abstract void SetParameter(string paramName, VistaDBType dataType, object value);
		
		/// <summary>
		/// Returns True if the V-SQL parameter value is NULL.
		/// </summary>
		public abstract bool ParamIsNull(string pName);

		/// <summary>
		/// Set a V-SQL parameter value to NULL.
		/// </summary>
		public abstract void SetParamNull(string pName, VistaDBType type);

		/// <summary>
		/// Returns if the V-SQL query has been opened. Applies to SELECT statements only.
		/// </summary>
		public bool Opened
		{
			get
			{
				return this.opened;
			}
		}

		//////////////////////////////////////////////////////////
		///////////////Navigation functions///////////////////////
		//////////////////////////////////////////////////////////

		/// <summary>
		/// Go to the first row in the dataset.
		/// </summary>
		/// <returns>False if current position doesn't change</returns>
		public abstract bool First();

		/// <summary>
		/// Go to the next row in dataset
		/// </summary>
		/// <returns>False if current position doesn't change</returns>
		public abstract bool Next();

		/// <summary>
		/// End of file. Tests if a row movement function has placed the row pointer beyond the last row in the dataset.
		/// </summary>
		public abstract bool Eof
		{
			get;
		}


		//////////////////////////////////////////////////////////
		///////////////////Meta data functions////////////////////
		//////////////////////////////////////////////////////////
		
		/// <summary>
		/// Return the number of rows affected.
		/// </summary>
		public int RowsAffected
		{
			get
			{
				return this.rowsAffected;
			}
		}

		/// <summary>
		/// Return the column count for the V-SQL query.
		/// </summary>
		public int ColumnCount
		{
			get
			{
				return this.columnCount;
			}
		}

		/// <summary>
		/// The collection of column objects in the V-SQL query.
		/// </summary>
		public class VistaDBColumnCollection
		{
			private VistaDBSQLQuery parent;

			public VistaDBColumnCollection(VistaDBSQLQuery p)
			{
				this.parent = p;
			}

			/// <summary>
			/// Gets the column object at the specified offset.
			/// </summary>
			public VistaDBColumn this[int i]
			{
				get
				{
					if( i < 0 || i > parent.columns.GetUpperBound(0) )
						return  null;
					else
						return this.parent.columns[i];
				}
			}

			/// <summary>
			/// Gets the number of columns in the collection.
			/// </summary>
			public int Count
			{
				get
				{
					return this.parent.columns.Length;
				}
			}

		}
		
		/// <summary>
		/// Column objects in the collection.
		/// </summary>
		public readonly VistaDBColumnCollection Columns;

		/// <summary>
		/// Returns the value of the column at the given position in the table schema. The first column is 1.
		/// </summary>
		public abstract object GetValue(int fieldNo);

		/// <summary>
		/// Returns the number of records retrieved by the last V-SQL query statement. Applies to SELECT.
		/// </summary>
		public int RecordCount
		{
			get
			{
				return this.recordCount;
			}

		}

		/// <summary>
		/// Gets or sets the V-SQL query statement to be executed.
		/// </summary>
		public string SQL
		{
			get
			{
				return this.commandText;
			}
			set
			{
				this.commandText = value;
			}
		}
	
		public abstract bool IsNull(int columnNumber);
	}
}