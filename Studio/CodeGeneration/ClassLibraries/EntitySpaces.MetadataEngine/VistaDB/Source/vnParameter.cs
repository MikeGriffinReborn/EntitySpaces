using System;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Globalization;

namespace Provider.VistaDB
{
	/// <summary>
	/// Represents a parameter to a VistaDBCommand
	/// </summary>
	[TypeConverter(typeof(VistaDBParameter.VDBParameterConverter)),Serializable()]
	public class VistaDBParameter: IDataParameter, IDbDataParameter, ICloneable
	{
		private VistaDBType dbType = VistaDBType.Character;
		private bool typeDefined = false;
		private ParameterDirection m_direction = ParameterDirection.Input;
		private bool nullable = false;
		private string paramName;
		private string sourceColumn;

		private DataRowVersion m_sourceVersion = DataRowVersion.Current;
		
		private object val;
		private byte precision;
		private byte scale;
		private int size;

		private static DbType[] vdbTypes = 
		{
			DbType.StringFixedLength,//Character
			DbType.Date,//Date
			DbType.DateTime,//DateTime
			DbType.Boolean,//Boolean
			DbType.Binary,//Memo
			DbType.Binary,//Picture
			DbType.Binary,//Blob
			DbType.Currency,//Currency
			DbType.Int32,//Integer
			DbType.Int64,//Int64
			DbType.Double,//Double
			DbType.String//Varchar
		};

		/// <summary>
		/// Constructor
		/// </summary>
		public VistaDBParameter()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parameterName">Parameter name</param>
		/// <param name="type">Parameter data type</param>
		public VistaDBParameter(string parameterName, VistaDBType type)
		{
			this.paramName = parameterName;
			this.dbType = type;
			this.typeDefined = true;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parameterName">Parameter name</param>
		/// <param name="value">Parameter value</param>
		public VistaDBParameter(string parameterName, object value)
		{
			this.paramName = parameterName;
			this.Value = value;
			//' Setting the value also infers the type.
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parameterName">Parameter name</param>
		/// <param name="type">Parameter data type</param>
		/// <param name="sourceColumn">The name of the source column.</param>
		public VistaDBParameter(string parameterName, VistaDBType type, string sourceColumn)
		{
			this.paramName = parameterName;
			this.dbType = type;
			this.sourceColumn = sourceColumn;
			this.typeDefined = true;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parameterName">Parameter name.</param>
		/// <param name="dbType">VistaDBType type of the parameter.</param>
		/// <param name="size">maximum size, in bytes, of the data within the column.</param>
		/// <param name="direction">ParameterDirection.</param>
		/// <param name="isNullable">Indicates whether the parameter accepts null values.</param>
		/// <param name="precision">Maximum number of digits used to represent the Value property.</param>
		/// <param name="scale">Number of decimal places to which Value is resolved.</param>
		/// <param name="sourceColumn">Name of the source column that is mapped to the DataSet and used for loading or returning the Value.</param>
		/// <param name="sourceVersion">DataRowVersion to use when loading Value.</param>
		/// <param name="value">Value of the parameter.</param>
		public VistaDBParameter(string parameterName, VistaDBType dbType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			this.dbType = dbType;
			this.m_direction = direction;
			this.nullable = isNullable;
			this.paramName = parameterName;
			this.sourceColumn = sourceColumn;

			this.m_sourceVersion = sourceVersion;
			
			this.val = value;
			this.size = size;
			this.scale = scale;
			this.precision = precision;

			this.typeDefined = true;
		}

		/// <summary>
		/// Gets or sets the parameter type.
		/// </summary>
		/// <remarks>Provided for compatibility with Sql's DbType. Use VistaDBType</remarks>
		public DbType DbType
		{
			get
			{
				return VistaDBTypeToDbType(this.dbType);
			}
			set
			{
				this.dbType = DbTypeToVistaDBType(value);
				this.typeDefined = true;
			}
		}

		/// <summary>
		/// Gets or sets the VistaDBType parameter.
		/// </summary>
		public VistaDBType VistaDBType
		{
			get
			{
				return this.dbType;
			}
			set
			{
				this.dbType = value;
				this.typeDefined = true;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the parameter is input-only, output-only or bidirectional return value parameter.
		/// </summary>
		public ParameterDirection Direction//IDataParameter.Direction
		{
			get
			{
				return m_direction;
			}
			set
			{
				m_direction = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the parameter accepts null values.
		/// </summary>
		public bool IsNullable
		{
			get
			{
				return true;	//m_fNullable;
			}
		}

		/// <summary>
		/// Gets the parameter name.
		/// </summary>
		[Browsable(false)]
		public string Name
		{
			get
			{
				return this.ParameterName;
			}
		}

		/// <summary>
		/// Gets or sets the parameter name.
		/// </summary>
		public string ParameterName
		{
			get
			{
				return paramName;
			}
			set
			{
				paramName = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the source column that is mapped to the DataSet and used for loading or returning the Value.
		/// </summary>
		public string SourceColumn
		{
			get
			{
				return sourceColumn;
			}
			set
			{
				sourceColumn = value;
			}
		}

		/// <summary>
		/// Gets or sets the DataRowVersion to use when loading Value.
		/// </summary>
		public DataRowVersion SourceVersion
		{
			get
			{
				return m_sourceVersion;
			}
			set
			{
				m_sourceVersion = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of the parameter.
		/// </summary>
		public object Value
		{
			get
			{
				return this.val;
			}
			set
			{
				if( value != null && !this.typeDefined )
				{
					this.dbType = FitSystemType(value);
					this.typeDefined = true;
				}

				if( value == null || value == DBNull.Value )
					this.val = null;
				else
				{
					switch(this.dbType)
					{
						case VistaDBType.Blob:
						case VistaDBType.Picture:
							this.val = value;
							break;

						case VistaDBType.Boolean:
							this.val = (bool)value;
							break;

						case VistaDBType.Character:
						case VistaDBType.Memo:
						case VistaDBType.Varchar:
							this.val = (string)value.ToString();
							break;

						case VistaDBType.Currency:
							this.val = (decimal)value;
							break;

						case VistaDBType.Double:
							this.val = (double)value;
							break;

						case VistaDBType.Date:
						case VistaDBType.DateTime:
							this.val = (DateTime)value;
							break;

						case VistaDBType.Int32:
							this.val = (int)value;
							break;

						case VistaDBType.Int64:
							this.val = (long)value;
							break;

						case VistaDBType.Guid:
							this.val = (Guid)value;
							break;
					}
				}
			}
		}

		private VistaDBType FitSystemType( object value )
		{
			TypeCode typeCode = Type.GetTypeCode(value.GetType());

			switch( typeCode )
			{
				case TypeCode.Empty:
				case TypeCode.DBNull:
				case TypeCode.Byte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.Single:
				case TypeCode.Char:
					//' Throw a SystemException for unsupported data types.
					return this.dbType;

				case TypeCode.Boolean:
					return VistaDBType.Boolean;
				case TypeCode.Int32:
					return VistaDBType.Int32;
				case TypeCode.Int64:
					return VistaDBType.Int64;
				case TypeCode.Double:
					return VistaDBType.Double;
				case TypeCode.DateTime:
					return VistaDBType.DateTime;
				case TypeCode.String:
					return VistaDBType.Character;
				case TypeCode.Decimal:
					return VistaDBType.Currency;
				case TypeCode.Object:
					if(value.GetType() == typeof(Guid))
						return VistaDBType.Guid;
					else
					if ( value.GetType() == typeof(byte[]) ) 
						return VistaDBType.Blob;
					else
						throw new SystemException("Value is of unknown data type");
				default:
					throw new SystemException("Value is of unknown data type");
			}
		}

		/// <summary>
		/// Creates a new VistaDBParameter object.
		/// </summary>
		/// <returns>New VistaDBParameter object</returns>
		public object Clone()
		{
			return new VistaDBParameter();
		}

		/// <summary>
		/// Gets or sets the maximum number of digits used to represent the Value property.
		/// </summary>
		public byte Precision
		{
			get
			{
				return precision;
			}
			set
			{
				precision = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of decimal places to which Value is resolved.
		/// </summary>
		public byte Scale
		{
			get
			{
				return scale;
			}
			set
			{
				scale = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum size, in bytes, of the data within the column.
		/// </summary>
		public int Size
		{
			get
			{
				return size;
			}
			set
			{
				size = value;
			}
		}

		/// <summary>
		/// Provides a unified way of converting types of values to other types, as well as for accessing standard values and subproperties.
		/// </summary>
		/// <remarks>
		/// VistaDB type converter is used to convert values between data types, and to assist property configuration 
		/// at design time by providing text-to-value conversion or a drop-down list of values to select from. 
		/// </remarks>
		public class VDBParameterConverter: TypeConverter
		{
			/// <summary>
			/// Overloaded. Returns whether this converter can convert the object to the specified type.
			/// </summary>
			/// <param name="context">An ITypeDescriptorContext that provides a format context. </param>
			/// <param name="destinationType">The Type to convert the value parameter to.</param>
			/// <returns></returns>
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if(destinationType == typeof(InstanceDescriptor))
				{
					return true;
				}

				return base.CanConvertTo(context, destinationType);
			}

			/// <summary>
			/// Overloaded. Converts the given value object to the specified type.
			/// </summary>
			/// <param name="context">An ITypeDescriptorContext that provides a format context. </param>
			/// <param name="culture">A CultureInfo object. If a null reference (Nothing in Visual Basic) is passed, the current culture is assumed. </param>
			/// <param name="value">The Object to convert.</param>
			/// <param name="destinationType">The Type to convert the value parameter to.</param>
			/// <returns></returns>
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				ConstructorInfo info1;
				VistaDBParameter parameter1;
				Type[] array1;
				object[] array2;

				if(destinationType == typeof(InstanceDescriptor))
				{
					array1 = new Type[10];
					array1[0] = typeof(string);
					array1[1] = typeof(VistaDBType);
					array1[2] = typeof(int);
					array1[3] = typeof(ParameterDirection);
					array1[4] = typeof(bool);
					array1[5] = typeof(byte);
					array1[6] = typeof(byte);
					array1[7] = typeof(string);
					array1[8] = typeof(DataRowVersion);
					array1[9] = typeof(object);

					info1 = typeof(VistaDBParameter).GetConstructor(array1);
					parameter1 = (VistaDBParameter)value;
					array2 = new object[10];
					array2[0] = parameter1.ParameterName;
					array2[1] = parameter1.VistaDBType;
					array2[2] = parameter1.Size;
					array2[3] = parameter1.Direction;
					array2[4] = parameter1.IsNullable;
					array2[5] = parameter1.Precision;
					array2[6] = parameter1.Scale;
					array2[7] = parameter1.SourceColumn;
					array2[8] = parameter1.SourceVersion;
					array2[9] = parameter1.Value;
					return new InstanceDescriptor(info1, array2);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		private DbType VistaDBTypeToDbType(VistaDBType type)
		{
			return vdbTypes[(int)type];
		}

		private VistaDBType DbTypeToVistaDBType(DbType type)
		{
			return (VistaDBType) Array.IndexOf(vdbTypes, type);
		}

	}
}
