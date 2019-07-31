using System;
using System.ComponentModel;
using System.Xml;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class Table : Single, ITable, INameValueItem
	{
		public Table()
		{

		}

		#region Collections

		public IColumns Columns
		{
			get
			{
				if(null == _columns)
				{
					_columns = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_columns.Table = this;
					_columns.dbRoot = this.dbRoot;
					_columns.LoadForTable();
				}
				return _columns;
			}
		}

        [Browsable(false)]
		public IForeignKeys ForeignKeys
		{
			get
			{
				if(null == _foreignKeys)
				{
					_foreignKeys = (ForeignKeys)this.dbRoot.ClassFactory.CreateForeignKeys();
					_foreignKeys.Table = this;
					_foreignKeys.dbRoot = this.dbRoot;
					_foreignKeys.LoadAll();
				}
				return _foreignKeys;
			}
		}

        [Browsable(false)]
		public IForeignKeys IndirectForeignKeys
		{
			get
			{
				if(null == _indirectForeignKeys)
				{
					_indirectForeignKeys = (ForeignKeys)this.dbRoot.ClassFactory.CreateForeignKeys();
					_indirectForeignKeys.Table = this;
					_indirectForeignKeys.dbRoot = this.dbRoot;
					_indirectForeignKeys.LoadAllIndirect();
				}
				return _indirectForeignKeys;
			}
		}

        [Browsable(false)]
		public IIndexes Indexes
		{
			get
			{
				if(null == _indexes)
				{
					_indexes = (Indexes)this.dbRoot.ClassFactory.CreateIndexes();
					_indexes.Table = this;
					_indexes.dbRoot = this.dbRoot;
					_indexes.LoadAll();
				}
				return _indexes;
			}
		}

        [Browsable(false)]
		public virtual IColumns PrimaryKeys
		{
			get
			{
				return null;
			}
		}

		#endregion

		#region Objects

        [Browsable(false)]
		public IDatabase Database
		{
			get
			{
				return this.Tables.Database;
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
				return this.GetString(Tables.f_Name);
			}
		}

        public string FullName
        {
            get
            {
                if (this.Schema != null && this.Schema != string.Empty)
                {
                    return Schema + "." + Name;
                }
                else
                {
                    return Name;
                }
            }
        }

		public string Schema
		{
			get
			{
				return this.GetString(Tables.f_Schema);
			}
		}

		public string Type
		{
			get
			{
				return this.GetString(Tables.f_Type);
			}
		}

        [Browsable(false)]
		public Guid Guid
		{
			get
			{
				return this.GetGuid(Tables.f_Guid);
			}
		}

		public string Description
		{
			get
			{
				return this.GetString(Tables.f_Description);
			}
		}

        [Browsable(false)]
		public System.Int32 PropID
		{
			get
			{
				return this.GetInt32(Tables.f_PropID);
			}
		}

		public DateTime DateCreated
		{
			get
			{
				return this.GetDateTime(Tables.f_DateCreated);
			}
		}

		public DateTime DateModified
		{
			get
			{
				return this.GetDateTime(Tables.f_DateModified);
			}
		}

		#endregion

        #region Naming Properties for Code Generation

        [Category("Code Generation")]
        public string Entity
        {
            get
            {
                return _dbRoot.esPlugIn.Entity(this);
            }
        }

        [Category("Code Generation")]
        public string esEntity
        {
            get
            {
                return _dbRoot.esPlugIn.esEntity(this);
            }
        }

        [Category("Code Generation")]
        public string Collection
        {
            get
            {
                return _dbRoot.esPlugIn.Collection(this);
            }
        }

        [Category("Code Generation")]
        public string esCollection
        {
            get
            {
                return _dbRoot.esPlugIn.esCollection(this);
            }
        }

        [Category("Code Generation")]
        public string Query
        {
            get
            {
                return _dbRoot.esPlugIn.Query(this);
            }
        }

        [Category("Code Generation")]
        public string esQuery
        {
            get
            {
                return _dbRoot.esPlugIn.esQuery(this);
            }
        }

        [Category("Code Generation")]
        public string Metadata
        {
            get
            {
                return _dbRoot.esPlugIn.Metadata(this);
            }
        }

        [Category("Code Generation")]
        public string ProxyStub
        {
            get
            {
                return _dbRoot.esPlugIn.ProxyStub(this);
            }
        }

        [Category("Code Generation")]
        public string ProxyStubCollection
        {
            get
            {
                return _dbRoot.esPlugIn.ProxyStubCollection(this);
            }
        }

        [Category("Code Generation")]
        public string ProxyStubQuery
        {
            get
            {
                return _dbRoot.esPlugIn.ProxyStubQuery(this);
            }
        }

        #endregion

		#region XML User Data

        [Browsable(false)]
		override public string UserDataXPath
		{ 
			get
			{
				return Tables.UserDataXPath + @"/Table[@Name='" + this.Name + "']";
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
				if(this.Tables.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Table[@Name='" + this.Name + "']";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Table", null);
			parentNode.AppendChild(myNode);

			XmlAttribute attr;

			attr = parentNode.OwnerDocument.CreateAttribute("Name");
			attr.Value = this.Name;
			myNode.Attributes.Append(attr);
		}

		#endregion

		internal Tables Tables = null;
		protected Columns _columns = null;
		protected Columns _primaryKeys = null;
		protected ForeignKeys _foreignKeys = null;
		protected ForeignKeys _indirectForeignKeys = null;
		protected Indexes _indexes = null;
		
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
	}
}
