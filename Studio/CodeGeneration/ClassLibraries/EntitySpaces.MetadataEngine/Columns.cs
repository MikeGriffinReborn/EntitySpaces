using System;
using System.Xml;
using System.Collections;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class Columns : Collection, IColumns, IEnumerable, ICollection
	{
		public Columns()
		{
		}

		#region DataColumn Binding Stuff

		internal DataColumn f_TableCatalog		= null;
		internal DataColumn f_TableSchema		= null;
		internal DataColumn f_TableName			= null;
		internal DataColumn f_Name				= null;
		internal DataColumn f_Guid				= null;
		internal DataColumn f_PropID            = null;
		internal DataColumn f_Ordinal           = null;
		internal DataColumn f_HasDefault        = null;
		internal DataColumn f_Default			= null;
		internal DataColumn f_Flags				= null;
		internal DataColumn f_IsNullable		= null;
		internal DataColumn f_DataType			= null;
        internal DataColumn f_IsNonSystemType   = null;	
		internal DataColumn f_TypeGuid			= null;
		internal DataColumn f_MaxLength			= null;
		internal DataColumn f_OctetLength		= null;
		internal DataColumn f_NumericPrecision	= null;
		internal DataColumn f_NumericScale		= null;	
		internal DataColumn f_DatetimePrecision = null;
		internal DataColumn f_CharSetCatalog    = null;
		internal DataColumn f_CharSetSchema     = null;
		internal DataColumn f_CharSetName       = null;
		internal DataColumn f_CollationCatalog	= null;
		internal DataColumn f_CollationSchema	= null;
		internal DataColumn f_CollationName		= null;
		internal DataColumn f_DomainCatalog		= null;
		internal DataColumn f_DomainSchema		= null;
		internal DataColumn f_DomainName		= null;
		internal DataColumn f_Description		= null;
		internal DataColumn f_LCID				= null;
		internal DataColumn f_CompFlags			= null;
		internal DataColumn f_SortID			= null;
		internal DataColumn f_TDSCollation		= null;
		internal DataColumn f_IsComputed		= null;
		internal DataColumn f_IsAutoKey         = null;
		internal DataColumn f_AutoKeySeed		= null;
		internal DataColumn f_AutoKeyIncrement	= null;
        internal DataColumn f_IsConcurrency     = null;


		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("TABLE_CATALOG"))						f_TableCatalog		= metaData.Columns["TABLE_CATALOG"];
				if(metaData.Columns.Contains("TABLE_SCHEMA"))						f_TableSchema		= metaData.Columns["TABLE_SCHEMA"];
				if(metaData.Columns.Contains("TABLE_NAME"))							f_TableName			= metaData.Columns["TABLE_NAME"];
				if(metaData.Columns.Contains("COLUMN_NAME"))						f_Name				= metaData.Columns["COLUMN_NAME"];
				if(metaData.Columns.Contains("COLUMN_GUID"))						f_Guid				= metaData.Columns["COLUMN_GUID"];
				if(metaData.Columns.Contains("COLUMN_PROPID"))						f_PropID            = metaData.Columns["COLUMN_PROPID"];
				if(metaData.Columns.Contains("ORDINAL_POSITION"))					f_Ordinal           = metaData.Columns["ORDINAL_POSITION"];
				if(metaData.Columns.Contains("COLUMN_HASDEFAULT"))					f_HasDefault        = metaData.Columns["COLUMN_HASDEFAULT"];
				if(metaData.Columns.Contains("COLUMN_DEFAULT"))						f_Default			= metaData.Columns["COLUMN_DEFAULT"];
				if(metaData.Columns.Contains("COLUMN_FLAGS"))						f_Flags				= metaData.Columns["COLUMN_FLAGS"];
				if(metaData.Columns.Contains("IS_NULLABLE"))						f_IsNullable		= metaData.Columns["IS_NULLABLE"];
				if(metaData.Columns.Contains("DATA_TYPE"))							f_DataType			= metaData.Columns["DATA_TYPE"];
				if(metaData.Columns.Contains("TYPE_GUID"))							f_TypeGuid			= metaData.Columns["TYPE_GUID"];
				if(metaData.Columns.Contains("CHARACTER_MAXIMUM_LENGTH"))			f_MaxLength			= metaData.Columns["CHARACTER_MAXIMUM_LENGTH"];
				if(metaData.Columns.Contains("CHARACTER_OCTET_LENGTH"))				f_OctetLength		= metaData.Columns["CHARACTER_OCTET_LENGTH"];
				if(metaData.Columns.Contains("NUMERIC_PRECISION"))					f_NumericPrecision	= metaData.Columns["NUMERIC_PRECISION"];
				if(metaData.Columns.Contains("NUMERIC_SCALE"))						f_NumericScale		= metaData.Columns["NUMERIC_SCALE"];
				if(metaData.Columns.Contains("DATETIME_PRECISION"))					f_DatetimePrecision = metaData.Columns["DATETIME_PRECISION"];
				if(metaData.Columns.Contains("CHARACTER_SET_CATALOG"))				f_CharSetCatalog    = metaData.Columns["CHARACTER_SET_CATALOG"];
				if(metaData.Columns.Contains("CHARACTER_SET_SCHEMA"))				f_CharSetSchema     = metaData.Columns["CHARACTER_SET_SCHEMA"];
				if(metaData.Columns.Contains("CHARACTER_SET_NAME"))					f_CharSetName       = metaData.Columns["CHARACTER_SET_NAME"];
				if(metaData.Columns.Contains("COLLATION_CATALOG"))					f_CollationCatalog	= metaData.Columns["COLLATION_CATALOG"];
				if(metaData.Columns.Contains("COLLATION_SCHEMA"))					f_CollationSchema	= metaData.Columns["COLLATION_SCHEMA"];
				if(metaData.Columns.Contains("COLLATION_NAME"))						f_CollationName		= metaData.Columns["COLLATION_NAME"];
				if(metaData.Columns.Contains("DOMAIN_CATALOG"))						f_DomainCatalog		= metaData.Columns["DOMAIN_CATALOG"];
				if(metaData.Columns.Contains("DOMAIN_SCHEMA"))						f_DomainSchema		= metaData.Columns["DOMAIN_SCHEMA"];
				if(metaData.Columns.Contains("DOMAIN_NAME"))						f_DomainName		= metaData.Columns["DOMAIN_NAME"];
				if(metaData.Columns.Contains("DESCRIPTION"))						f_Description		= metaData.Columns["DESCRIPTION"];
				if(metaData.Columns.Contains("COLUMN_LCID"))						f_LCID				= metaData.Columns["COLUMN_LCID"];
				if(metaData.Columns.Contains("COLUMN_COMPFLAGS"))					f_CompFlags			= metaData.Columns["COLUMN_COMPFLAGS"];
				if(metaData.Columns.Contains("COLUMN_SORTID"))						f_SortID			= metaData.Columns["COLUMN_SORTID"];
				if(metaData.Columns.Contains("COLUMN_TDSCOLLATION"))				f_TDSCollation		= metaData.Columns["COLUMN_TDSCOLLATION"];
				if(metaData.Columns.Contains("IS_COMPUTED"))						f_IsComputed		= metaData.Columns["IS_COMPUTED"];
				if(metaData.Columns.Contains("IS_AUTO_KEY"))						f_IsAutoKey			= metaData.Columns["IS_AUTO_KEY"];
				if(metaData.Columns.Contains("AUTO_KEY_SEED"))						f_AutoKeySeed		= metaData.Columns["AUTO_KEY_SEED"];
				if(metaData.Columns.Contains("AUTO_KEY_INCREMENT"))					f_AutoKeyIncrement	= metaData.Columns["AUTO_KEY_INCREMENT"];
                if(metaData.Columns.Contains("IS_CONCURRENCY"))                     f_IsConcurrency     = metaData.Columns["IS_CONCURRENCY"];
			}																		
		}
		#endregion

		virtual internal void LoadForTable()
		{

		}

		virtual internal void LoadForView()
		{

		}

		internal void PopulateArray(DataTable metaData)
		{
			BindToColumns(metaData);

			Column column = null;

			if(metaData.DefaultView.Count > 0)
			{
				IEnumerator enumerator = metaData.DefaultView.GetEnumerator();
				while(enumerator.MoveNext())
				{
					DataRowView rowView = enumerator.Current as DataRowView;

					column = (Column)this.dbRoot.ClassFactory.CreateColumn();
					column.dbRoot = this.dbRoot;
					column.Columns = this;
					column.Row = rowView.Row;
					this._array.Add(column);
				}
			}
		}

		internal void AddColumn(Column column)
		{
			Column c;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				c = this._array[i] as Column;

				if(c.Name == column.Name)
				{
					return;
				}
			}

			this._array.Add(column);
		}

		#region indexers
	
		public IColumn this[object index]
		{
			get
			{
				if(index.GetType() == Type.GetType("System.String"))
				{
					return GetByPhysicalName(index as String) as IColumn;
				}
				else
				{
					int idx = Convert.ToInt32(index);
					return this._array[idx] as IColumn;
				}
			}
		}

		public Column GetByName(string name)
		{
			Column obj = null;
			Column tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Column;

				if(this.CompareStrings(name,tmp.Name))
				{
					obj = tmp;
					break;
				}
			}

			return obj;
		}

		internal Column GetByPhysicalName(string name)
		{
			Column obj = null;
			Column tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Column;

				if(this.CompareStrings(name,tmp.Name))
				{
					obj = tmp;
					break;
				}
			}

			return obj;
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return new Enumerator(this._array);
		}

		#endregion

		#region XML User Data
	
		override public string UserDataXPath
		{ 
			get
			{
				string xPath = "";

				if(null != Table)
				{
					xPath = Table.UserDataXPath;
				}
				else if(null != View)
				{
					xPath = View.UserDataXPath;
				}
				else if(null != Index)
				{
					xPath = Index.UserDataXPath;
				}
				else if(null != ForeignKey)
				{
					xPath = ForeignKey.UserDataXPath;
				}

				return  xPath + @"/Columns";
			} 
		}

		private MetaObject GetParent()
		{
			MetaObject parent = null;

			if(null != Table)
			{
				parent = Table;
			}
			else if(null != View)
			{
				parent = View;
			}
			else if(null != Index)
			{
				parent =  Index.Indexes.Table;
			}
			else if(null != ForeignKey)
			{
				return ForeignKey.ForeignKeys.Table;
			}

			return parent;
		}

		internal IDatabase GetDatabase()
		{
			IDatabase database = null;

			if(null != Table)
			{
				database = Table.Database;
			}
			else if(null != View)
			{
				database = View.Database;
			}
			else if(null != Index)
			{
				database =  Index.Indexes.Table.Database;
			}
			else if(null != ForeignKey)
			{
				database = ForeignKey.ForeignKeys.Table.Database;
			}

			return database;
		}
	
		override internal bool GetXmlNode(out XmlNode node, bool forceCreate)
		{
			node = null;
			bool success = false;

			if(null == _xmlNode)
			{
				// Get the parent node
				XmlNode parentNode = null;
				if(this.GetParent().GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Columns";
					if(!GetUserData(xPath, parentNode, out _xmlNode)  && forceCreate)
					{
						// Create it, and try again
						this.CreateUserMetaData(parentNode);
						GetUserData(xPath, parentNode, out _xmlNode);
					}
				}
			}

			if(null != _xmlNode)
			{
				node = _xmlNode;
				success = true;
			}

			return success;
		}
	
		override public void CreateUserMetaData(XmlNode parentNode)
		{
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Columns", null);
			parentNode.AppendChild(myNode);
		}

		#endregion

		#region IList Members

		object System.Collections.IList.this[int index]
		{
			get	{ return this[index];}
			set	{ }
		}

		#endregion

		internal Table Table = null;
		internal View View = null;
		internal Index Index = null;
		internal ForeignKey ForeignKey = null;

		internal bool FKsLoaded = false;

	}
}
