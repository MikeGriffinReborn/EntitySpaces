using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Sql
{
	public class SqlResultColumn : ResultColumn
	{
		public SqlResultColumn()
		{

		}

		#region Properties
        //ColumnName ColumnOrdinal ColumnSize NumericPrecision NumericScale IsUnique IsKey BaseServerName BaseCatalogName BaseColumnName BaseSchemaName BaseTableName DataType AllowDBNull ProviderType IsAliased IsExpression IsIdentity IsAutoIncrement IsRowVersion IsHidden IsLong IsReadOnly ProviderSpecificDataType DataTypeName XmlSchemaCollectionDatabase XmlSchemaCollectionOwningSchema XmlSchemaCollectionName UdtAssemblyQualifiedName NonVersionedProviderType 
		override public string Name
		{
			get
			{
                if (_row != null)
                {
                    return _row["ColumnName"].ToString();
                }
                else
                {
                    return this._column.ColumnName;
                }
			}
        }

        override public string DataTypeName
        {
            get
            {
                if (_row != null)
                {
                    return _row["DataTypeName"].ToString();
                }
                else
                {
                    return _column.DataType.ToString();
                }
            }
        }

        override public string DataTypeNameComplete
		{
			get
			{
                return SqlColumn.GetFullDataTypeName(DataTypeName, CharacterMaxLength, NumericPrecision, NumericScale);
			}
		}

		override public System.Int32 Ordinal
		{
			get
            {
                if (_row != null)
                {
                    return Convert.ToInt32(_row["ColumnOrdinal"]);
                }
                else
                {
                    return this._column.Ordinal;
                }
			}
        }

        public System.Int32 CharacterMaxLength
        {
            get
            {
                try
                {
                    return Convert.ToInt32(_row["ColumnSize"]);
                }
                catch { }
                return 0;
            }
        }

        public System.Int32 CharacterOctetLength
        {
            get
            {
                if (DataTypeName.StartsWith("n", StringComparison.CurrentCultureIgnoreCase))
                {
                    return CharacterMaxLength * 2;
                }
                else
                {
                    return CharacterMaxLength;
                }
            }
        }

        public System.Int32 NumericPrecision
        {
            get
            {
                try
                {
                    int i = Convert.ToInt32(_row["NumericPrecision"]);
                    if (i < 255) return i;
                }
                catch { }
                return 0;
            }
        }

        public System.Int32 NumericScale
        {
            get
            {
                try
                {
                    int i = Convert.ToInt32(_row["NumericScale"]);
                    if (i < 255) return i;
                }
                catch { }
                return 0;
            }
        }

		#endregion

        internal DataRow _row = null;
        internal DataColumn _column = null;
	}
}
