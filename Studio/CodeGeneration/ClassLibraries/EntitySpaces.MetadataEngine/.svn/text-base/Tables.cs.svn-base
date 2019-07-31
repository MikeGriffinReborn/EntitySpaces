using System;
using System.Xml;
using System.Collections;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class Tables : Collection, ITables, IEnumerable, ICollection
	{
		public Tables()
		{

		}

		internal DataColumn f_Catalog			= null;
		internal DataColumn f_Schema			= null;
		internal DataColumn f_Name				= null;
		internal DataColumn f_Type				= null;
		internal DataColumn f_Guid				= null;
		internal DataColumn f_Description		= null;
		internal DataColumn f_PropID			= null;
		internal DataColumn f_DateCreated		= null;
		internal DataColumn f_DateModified		= null;


		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("TABLE_CATALOG"))		f_Catalog		 = metaData.Columns["TABLE_CATALOG"];
				if(metaData.Columns.Contains("TABLE_SCHEMA"))		f_Schema		 = metaData.Columns["TABLE_SCHEMA"];
				if(metaData.Columns.Contains("TABLE_NAME"))			f_Name			 = metaData.Columns["TABLE_NAME"];
				if(metaData.Columns.Contains("TABLE_TYPE"))			f_Type			 = metaData.Columns["TABLE_TYPE"];
				if(metaData.Columns.Contains("TABLE_GUID"))			f_Guid			 = metaData.Columns["TABLE_GUID"];
				if(metaData.Columns.Contains("DESCRIPTION"))		f_Description	 = metaData.Columns["DESCRIPTION"];
				if(metaData.Columns.Contains("TABLE_PROPID"))		f_PropID		 = metaData.Columns["TABLE_PROPID"];
				if(metaData.Columns.Contains("DATE_CREATED"))		f_DateCreated	 = metaData.Columns["DATE_CREATED"];
				if(metaData.Columns.Contains("DATE_MODIFIED"))		f_DateModified	 = metaData.Columns["DATE_MODIFIED"];
			}
		}

		internal virtual void LoadAll()
		{

		}

		protected void PopulateArray(DataTable metaData)
		{
			BindToColumns(metaData);

			Table table = null;

			if(metaData.DefaultView.Count > 0)
			{
				IEnumerator enumerator = metaData.DefaultView.GetEnumerator();
				while(enumerator.MoveNext())
				{
					DataRowView rowView = enumerator.Current as DataRowView;

					table = (Table)this.dbRoot.ClassFactory.CreateTable();
					table.dbRoot = this.dbRoot;
					table.Tables = this;
					table.Row = rowView.Row;
					this._array.Add(table);

				}
			}
		}

		internal void AddTable(Table table)
		{
			this._array.Add(table);
		}

		#region indexers
	
		public ITable this[object index]
		{
			get
			{
				if(index.GetType() == Type.GetType("System.String"))
				{
					return GetByPhysicalName(index as String);
				}
				else
				{
					int idx = Convert.ToInt32(index);
					return this._array[idx] as Table;
				}
			}
		}

		public Table GetByName(string name)
		{
			Table table = null;
			Table tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Table;

				if(this.CompareStrings(name,tmp.Name))
				{
					table = tmp;
					break;
				}
			}

			return table;
		}

		internal Table GetByPhysicalName(string name)
		{
			Table table = null;
			Table tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Table;

                if (this.CompareStrings(name, tmp.Name))
                {
                    table = tmp;
                    break;
                }
                else if (this.CompareStrings(name, tmp.FullName))
                {
                    table = tmp;
                    break;
                }
			}

			return table;
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
				return Database.UserDataXPath + @"/Tables";
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
				if(this.Database.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Tables";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Tables", null);
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
		
		internal Database Database = null;
	}
}
