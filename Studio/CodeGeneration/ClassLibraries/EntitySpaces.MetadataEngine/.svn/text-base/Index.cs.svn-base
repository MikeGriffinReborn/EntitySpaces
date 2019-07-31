using System;
using System.ComponentModel;
using System.Xml;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class Index : Single, IIndex
	{
		public Index()
		{
			
		}

		internal void AddColumn(string physicalColumnName)
		{
			if(null == _columns)
			{
				_columns = (Columns)this.dbRoot.ClassFactory.CreateColumns();
				_columns.dbRoot = this.dbRoot;
				_columns.Index = this;
			}

			Column column  = this.Indexes.Table.Columns[physicalColumnName] as Column;
			_columns.AddColumn(column);
		}

		#region Objects

        [Browsable(false)]
		public ITable Table
		{
			get
			{
				return this.Indexes.Table;
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
				return this.GetString(Indexes.f_IndexName);
			}
		}

		virtual public string Schema
		{
			get
			{
				return this.GetString(Indexes.f_IndexSchema);
			}
		}

		virtual public System.Boolean Unique
		{
			get
			{
				return this.GetBool(Indexes.f_Unique);
			}
		}

		virtual public System.Boolean Clustered
		{
			get
			{
				return this.GetBool(Indexes.f_Clustered);
			}
		}

		virtual public string Type
		{
			get
			{
				System.Int32 i = this.GetInt32(Indexes.f_Type);

				switch(i)
				{
					case 1:
						return "BTREE";
					case 2:
						return "HASH";
					case 3:
						return "CONTENT";
					case 4:
						return "OTHER";
					default:
						return "OTHER";
				}
			}
		}

		virtual public System.Int32 FillFactor
		{
			get
			{
				return this.GetInt32(Indexes.f_FillFactor);
			}
		}

		virtual public System.Int32 InitialSize
		{
			get
			{
				return this.GetInt32(Indexes.f_InitializeSize);
			}
		}

		virtual public System.Boolean SortBookmarks
		{
			get
			{
				return this.GetBool(Indexes.f_SortBookmarks);
			}
		}

		virtual public System.Boolean AutoUpdate
		{
			get
			{
				return this.GetBool(Indexes.f_AutoUpdate);
			}
		}

		virtual public string NullCollation
		{
			get
			{
				System.Int32 i = this.GetInt32(Indexes.f_NullCollation);

				switch(i)
				{
					case 1:
						return "END";
					case 2:
						return "HIGH";
					case 4:
						return "LOW";
					case 8:
						return "START";
					default:
						return "UNKNOWN";
				}
			}
		}

		virtual public string Collation
		{
			get
			{
				System.Int32 i = this.GetInt16(Indexes.f_Collation);

				switch(i)
				{
					case 1:
						return "ASCENDING";
					case 2:
						return "DECENDING";
					default:
						return "UNKNOWN";
				}
			}
		}

		virtual public Decimal Cardinality
		{
			get
			{
				return this.GetDecimal(Indexes.f_Cardinality);
			}
		}

		virtual public System.Int32 Pages
		{
			get
			{
				return this.GetInt32(Indexes.f_Pages);
			}
		}

		virtual public string FilterCondition
		{
			get
			{
				return this.GetString(Indexes.f_FilterCondition);
			}
		}

		virtual public System.Boolean Integrated
		{
			get
			{
				return this.GetBool(Indexes.f_Integrated);
			}
		}
		#endregion

		#region XML User Data

        [Browsable(false)]
		override public string UserDataXPath
		{ 
			get
			{
				return Indexes.UserDataXPath + @"/Index[@Name='" + this.Name + "']";
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
				if(this.Indexes.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Index[@Name='" + this.Name + "']";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Index", null);
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

        [Browsable(false)]
		virtual public IColumns Columns
		{
			get
			{
				return _columns;
			}
		}

		private Columns _columns = null;
		internal Indexes Indexes = null;
	}
}
