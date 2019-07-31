using System;
using System.ComponentModel;
using System.Xml;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class ForeignKey : Single, IForeignKey
	{
		public ForeignKey()
		{

		}

		#region Objects

        [Browsable(false)]
		virtual public ITable PrimaryTable
		{
			get
			{
				string cat_schema = "";

				try
				{		
					string catalog = (DBNull.Value == this._row["PK_TABLE_CATALOG"]) ? string.Empty : (this._row["PK_TABLE_CATALOG"] as string);
					string schema  = (DBNull.Value == this._row["PK_TABLE_SCHEMA"])  ? string.Empty : (this._row["PK_TABLE_SCHEMA"] as string);

					if( (catalog != null && catalog.Length > 0) || 
						(schema  != null && schema.Length  > 0) )
					{
						cat_schema = catalog != string.Empty ? catalog : schema;
					}
					else
					{
						cat_schema = this.ForeignKeys.Table.Database.Name;
					}
				}
				catch
				{
					cat_schema = this.ForeignKeys.Table.Database.Name;
				}

				return this.dbRoot.Databases[cat_schema].Tables[this.GetString(ForeignKeys.f_PKTableName)];
			}
		}

        [Category("Tables")]
        public string PrimaryTableName
        {
            get
            {
                return PrimaryTable == null ? string.Empty : PrimaryTable.Name;
            }
        }

        [Browsable(false)]
		virtual public ITable ForeignTable
		{
			get
			{
				string cat_schema = "";

				try
				{

					string catalog = (DBNull.Value == this._row["FK_TABLE_CATALOG"]) ? string.Empty : (this._row["FK_TABLE_CATALOG"] as string);
					string schema  = (DBNull.Value == this._row["FK_TABLE_SCHEMA"])  ? string.Empty : (this._row["FK_TABLE_SCHEMA"] as string);

					if( (catalog != null && catalog.Length > 0) || 
						(schema  != null && schema.Length  > 0) )
					{
						cat_schema = catalog != string.Empty ? catalog : schema;
					}
					else
					{
						cat_schema = this.ForeignKeys.Table.Database.Name;
					}
				}
				catch
				{
					cat_schema = this.ForeignKeys.Table.Database.Name;
				}

				return this.dbRoot.Databases[cat_schema].Tables[this.GetString(ForeignKeys.f_FKTableName)];
			}
		}

        [Category("Tables")]
        public string ForeignTableName
        {
            get
            {
                return ForeignTable == null ? string.Empty : ForeignTable.Name;
            }
        }

		#endregion

		#region Properties
		override public string Alias
		{
			get
			{
				XmlNode node = null;
				if(this.GetXmlNode(out node, false))
				{
					string niceName = null;

					if(this.GetUserData(node, "Alias", out niceName))
					{
						if(string.Empty != niceName)
							return niceName;
					}
				}

				// There was no nice name
				return this.Name;
			}

			set
			{
				XmlNode node = null;
				if(this.GetXmlNode(out node, true))
				{
					this.SetUserData(node, "Alias", value);
				}
			}
		}

		override public string Name
		{
			get
			{
				return this.GetString(ForeignKeys.f_FKName);
			}
		}

		virtual public System.Int32 Ordinal
		{
			get
			{
				return this.GetInt32(ForeignKeys.f_Ordinal);
			}
		}

        [Category("Rules")]
		virtual public string UpdateRule
		{
			get
			{
				return this.GetString(ForeignKeys.f_UpdateRule);
			}
		}

        [Category("Rules")]
		virtual public string DeleteRule
		{
			get
			{
				return this.GetString(ForeignKeys.f_DeleteRule);
			}
		}

		virtual public string PrimaryKeyName
		{
			get
			{
				return this.GetString(ForeignKeys.f_PKName);
			}
		}

        [Category("Rules")]
		virtual public string Deferrability            
		{
			get
			{
				System.Int16 i = this.GetInt16(ForeignKeys.f_Deferrability);

				switch(i)
				{
					case 1:
						return "INITIALLY_DEFERRED";
					case 2:
						return "INITIALLY_IMMEDIATE";
					case 3:
						return "NOT_DEFERRABLE";
					default:
						return "UNKNOWN";
				}
			}
		}

		#endregion

		#region XML User Data

        [Browsable(false)]
		override public string UserDataXPath
		{ 
			get
			{
				return ForeignKeys.UserDataXPath + @"/ForeignKey[@Name='" + this.Name + "']";
			} 
		}

		override internal bool GetXmlNode(out XmlNode node, bool forceCreate)
		{
			node = null;
			bool success = false;

			if(null == _xmlNode)
			{
				// Get the parent node
				XmlNode parentNode = null;
				if(this.ForeignKeys.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./ForeignKey[@Name='" + this.Name + "']";
					if(!GetUserData(xPath, parentNode, out _xmlNode) && forceCreate)
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "ForeignKey", null);
			parentNode.AppendChild(myNode);

			XmlAttribute attr;

			attr = parentNode.OwnerDocument.CreateAttribute("Name");
			attr.Value = this.Name;
			myNode.Attributes.Append(attr);
		}

		#endregion

        [Browsable(false)]
		virtual public IColumns PrimaryColumns
		{
			get
			{
				return _primaryColumns;
			}
		}

        [Browsable(false)]
		virtual public IColumns ForeignColumns
		{
			get
			{
				return _foreignColumns;
			}
		}

		internal virtual void AddForeignColumn(string catalog, string schema,
			string physicalTableName, string physicalColumnName, bool primary)
		{
			Tables tables = null;

			if( (catalog != null && catalog.Length > 0) || 
				(schema  != null && schema.Length  > 0) )
			{
				string cat_schema = catalog != string.Empty ? catalog : schema;
				tables  = this.dbRoot.Databases[cat_schema].Tables as Tables;
			}
			else
			{
				// This DBMS is a one horse database
				tables = (Tables)this.ForeignKeys.Table.Database.Tables;
			}

			Column column = tables[physicalTableName].Columns[physicalColumnName] as Column;
			Column c = column.Clone();

			if(primary)
			{
				if(null == _primaryColumns)
				{
					_primaryColumns = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryColumns.ForeignKey = this;
				}

				_primaryColumns.AddColumn(c);
			}
			else
			{
				if(null == _foreignColumns)
				{
					_foreignColumns = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_foreignColumns.ForeignKey = this;
				}

				_foreignColumns.AddColumn(c);
			}

			column.AddForeignKey(this);
		}

		#region INameValueCollection Members

        [Browsable(false)]
		public string ItemName
		{
			get
			{
				return this.Name;
			}
		}

        [Browsable(false)]
		public string ItemValue
		{
			get
			{
				return this.Name;
			}
		}

		#endregion

		protected Columns _primaryColumns = null;
		protected Columns _foreignColumns = null;
		internal ForeignKeys ForeignKeys = null;
	}
}
