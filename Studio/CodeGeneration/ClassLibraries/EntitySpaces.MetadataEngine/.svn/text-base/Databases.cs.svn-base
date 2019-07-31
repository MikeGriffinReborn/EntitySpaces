using System;
using System.Xml;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace EntitySpaces.MetadataEngine
{
	public class Databases : Collection, IDatabases, IEnumerable, ICollection
	{
		public Databases()
		{

		}

		#region XML User Data

		override public string UserDataXPath
		{ 
			get
			{
				return dbRoot.UserDataXPath + @"/Databases";
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
				if(this.dbRoot.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Databases";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Databases", null);
			parentNode.AppendChild(myNode);
		}

		#endregion

		internal DataColumn f_Catalog			= null;
		internal DataColumn f_Description		= null;
		internal DataColumn f_SchemaName		= null;
		internal DataColumn f_SchemaOwner		= null;
		internal DataColumn f_DefCharSetCat		= null;
		internal DataColumn f_DefCharSetSchema	= null;
		internal DataColumn f_DefCharSetName	= null;

		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("CATALOG_NAME"))	f_Catalog		 = metaData.Columns["CATALOG_NAME"];
				if(metaData.Columns.Contains("DESCRIPTION"))	f_Description	 = metaData.Columns["DESCRIPTION"];
				if(metaData.Columns.Contains("SCHEMA_NAME"))	f_SchemaName	 = metaData.Columns["SCHEMA_NAME"];
				if(metaData.Columns.Contains("SCHEMA_OWNER"))	f_SchemaOwner	 = metaData.Columns["SCHEMA_OWNER"];
				if(metaData.Columns.Contains("DEFAULT_CHARACTER_SET_CATALOG"))	f_DefCharSetCat		= metaData.Columns["DEFAULT_CHARACTER_SET_CATALOG"];
				if(metaData.Columns.Contains("DEFAULT_CHARACTER_SET_SCHEMA"))	f_DefCharSetSchema	= metaData.Columns["DEFAULT_CHARACTER_SET_SCHEMA"];
				if(metaData.Columns.Contains("DEFAULT_CHARACTER_SET_NAME"))		f_DefCharSetName	= metaData.Columns["DEFAULT_CHARACTER_SET_NAME"];


				if(null == f_SchemaName)
				{
					f_SchemaName = metaData.Columns.Add("SCHEMA_NAME", Type.GetType("System.String"));
				}

				if(null == f_SchemaOwner)
				{
					f_SchemaOwner = metaData.Columns.Add("SCHEMA_OWNER", Type.GetType("System.String"));
				}

				if(null == f_DefCharSetCat)
				{
					f_DefCharSetCat	= metaData.Columns.Add("DEFAULT_CHARACTER_SET_CATALOG", Type.GetType("System.String"));
				}

				if(null == f_DefCharSetSchema)
				{
					f_DefCharSetSchema	= metaData.Columns.Add("DEFAULT_CHARACTER_SET_SCHEMA", Type.GetType("System.String"));
				}

				if(null == f_DefCharSetName)
				{
					f_DefCharSetName	= metaData.Columns.Add("DEFAULT_CHARACTER_SET_NAME", Type.GetType("System.String"));
				}
			}
		}

		internal virtual void LoadAll()
		{

		}

		internal void PopulateArray(DataTable metaData)
		{
			BindToColumns(metaData);

			Database database = null;

			int count = metaData.Rows.Count;
			for(int i = 0; i < count; i++)
			{
				database = (Database)this.dbRoot.ClassFactory.CreateDatabase();
				database.dbRoot = this.dbRoot;
				database.Databases = this;
				database.Row = metaData.Rows[i];
				this._array.Add(database);
			}

			PopulateSchemaData();
		}

		internal virtual void PopulateSchemaData()
		{
			DataTable dt;
			Database  db;

			for(int i = 0; i < this.Count; i++)
			{
				db = this[i] as Database;
				dt = this.LoadData(OleDbSchemaGuid.Schemata, new object[] { db.Name } );

				if(dt.Rows.Count == 1)
				{
					db._row[f_SchemaName]		= dt.Rows[0]["SCHEMA_NAME"];
					db._row[f_SchemaOwner]		= dt.Rows[0]["SCHEMA_OWNER"];
					db._row[f_DefCharSetCat]	= dt.Rows[0]["DEFAULT_CHARACTER_SET_CATALOG"];
					db._row[f_DefCharSetSchema]	= dt.Rows[0]["DEFAULT_CHARACTER_SET_SCHEMA"];
					db._row[f_DefCharSetName]	= dt.Rows[0]["DEFAULT_CHARACTER_SET_NAME"];
				}
			}
		}

		internal void AddDatabase(Database database)
		{
			this._array.Add(database);
		}

		#region indexers
	
		virtual public IDatabase this[object index]
		{
			get
			{
				if(index.GetType() == Type.GetType("System.String"))
				{
					return GetByPhysicalName(index as String) as IDatabase;
				}
				else
				{
					int idx = Convert.ToInt32(index);
					return this._array[idx] as IDatabase;
				}
			}
		}
	
		virtual public Database GetByName(string name)
		{
			Database obj = null;
			Database tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Database;

				if(this.CompareStrings(name,tmp.Name))
				{
					obj = tmp;
					break;
				}
			}

			return obj;
		}
	
		internal Database GetByPhysicalName(string name)
		{
			Database obj = null;
			Database tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Database;

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

		#region IList Members

		object System.Collections.IList.this[int index]
		{
			get	{ return this[index];}
			set	{ }
		}

		#endregion
	}
}
