using System;
using System.ComponentModel;
using System.Xml;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class View : Single, IView, INameValueItem
	{
		public View()
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
					_columns.View = this;
					_columns.dbRoot = this.dbRoot;
					_columns.LoadForView();
				}
				return _columns;
			}
		}

        [Browsable(false)]
		virtual public IViews SubViews 
		{ 
			get
			{
				if(null == _views)
				{
					_views = (Views)this.dbRoot.ClassFactory.CreateViews();
					_views.dbRoot = this._dbRoot;
					_views.Database = this.Views.Database;
				}
				return _views;				
			}
		}

        [Browsable(false)]
		virtual public ITables SubTables
		{ 
			get
			{
				if(null == _tables)
				{
					_tables = (Tables)this.dbRoot.ClassFactory.CreateTables();
					_tables.dbRoot = this._dbRoot;
					_tables.Database = this.Views.Database;
				}
				return _tables;
			}
		}

		#endregion

		#region Objects

        [Browsable(false)]
		public IDatabase Database
		{
			get
			{
				return this.Views.Database;
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
				return this.GetString(Views.f_Name);
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
				return this.GetString(Views.f_Schema);
			}
		}

        [Browsable(false)]
		public virtual string ViewText
		{
			get
			{
				return this.GetString(Views.f_ViewDefinition);
			}
		}

        [Browsable(false)]
		public System.Boolean CheckOption
		{
			get
			{
				return this.GetBool(Views.f_CheckOption);
			}
		}

		public System.Boolean IsUpdateable
		{
			get
			{
				return this.GetBool(Views.f_IsUpdateable);
			}
		}

		public string Type
		{
			get
			{
				return this.GetString(Views.f_Type);
			}
		}

        [Browsable(false)]
		public Guid Guid
		{
			get
			{
				return this.GetGuid(Views.f_Guid);
			}
		}

		public string Description
		{
			get
			{
				return this.GetString(Views.f_Description);
			}
		}

        [Browsable(false)]
		public System.Int32 PropID
		{
			get
			{
				return this.GetInt32(Views.f_PropID);
			}
		}

		public DateTime DateCreated
		{
			get
			{
				return this.GetDateTime(Views.f_DateCreated);
			}
		}

		public DateTime DateModified
		{
			get
			{
				return this.GetDateTime(Views.f_DateModified);
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
				return Views.UserDataXPath + @"/View[@Name='" + this.Name + "']";
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
				if(this.Views.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./View[@Name='" + this.Name + "']";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "View", null);
			parentNode.AppendChild(myNode);

			XmlAttribute attr;

			attr = parentNode.OwnerDocument.CreateAttribute("Name");
			attr.Value = this.Name;
			myNode.Attributes.Append(attr);
		}

		#endregion

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

		internal Views Views = null;
		private Columns _columns = null;
		protected bool _subViewInfoLoaded = false;
		protected Views _views = null;
		protected Tables _tables = null;
	}
}
