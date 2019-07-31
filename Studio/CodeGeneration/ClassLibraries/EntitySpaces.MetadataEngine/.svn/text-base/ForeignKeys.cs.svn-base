using System;
using System.Xml;
using System.Collections;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class ForeignKeys : Collection, IForeignKeys, IEnumerable, ICollection
	{
		public ForeignKeys()
		{

		}

		internal DataColumn f_PKTableCatalog	= null;
		internal DataColumn f_PKTableSchema		= null;
		internal DataColumn f_PKTableName		= null;
		internal DataColumn f_FKTableCatalog	= null;
		internal DataColumn f_FKTableSchema		= null;
		internal DataColumn f_FKTableName		= null;
		internal DataColumn f_Ordinal			= null;
		internal DataColumn f_UpdateRule		= null;
		internal DataColumn f_DeleteRule		= null;
		internal DataColumn f_PKName			= null;
		internal DataColumn f_FKName			= null;
		internal DataColumn f_Deferrability 	= null;

		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("PK_TABLE_CATALOG"))    f_PKTableCatalog = metaData.Columns["PK_TABLE_CATALOG"];
				if(metaData.Columns.Contains("PK_TABLE_SCHEMA"))	 f_PKTableSchema = metaData.Columns["PK_TABLE_SCHEMA"];
				if(metaData.Columns.Contains("PK_TABLE_NAME"))		 f_PKTableName = metaData.Columns["PK_TABLE_NAME"];
				if(metaData.Columns.Contains("FK_TABLE_CATALOG"))	 f_FKTableCatalog = metaData.Columns["FK_TABLE_CATALOG"];
				if(metaData.Columns.Contains("FK_TABLE_SCHEMA"))	 f_FKTableSchema = metaData.Columns["FK_TABLE_SCHEMA"];
				if(metaData.Columns.Contains("FK_TABLE_NAME"))		 f_FKTableName = metaData.Columns["FK_TABLE_NAME"];
				if(metaData.Columns.Contains("ORDINAL"))			 f_Ordinal = metaData.Columns["ORDINAL"];
				if(metaData.Columns.Contains("UPDATE_RULE"))		 f_UpdateRule = metaData.Columns["UPDATE_RULE"];
				if(metaData.Columns.Contains("DELETE_RULE"))		 f_DeleteRule = metaData.Columns["DELETE_RULE"];
				if(metaData.Columns.Contains("PK_NAME"))			 f_PKName = metaData.Columns["PK_NAME"];
				if(metaData.Columns.Contains("FK_NAME"))			 f_FKName= metaData.Columns["FK_NAME"];
				if(metaData.Columns.Contains("DEFERRABILITY"))		 f_Deferrability = metaData.Columns["DEFERRABILITY"];

				_fieldsBound = true;
			}
		}

		virtual internal void LoadAll()
		{

		}

		virtual internal void LoadAllIndirect()
		{

		}

		internal void PopulateArray(DataTable metaData)
		{
			BindToColumns(metaData);

			ForeignKey key  = null;
			string keyName = "";

			foreach(DataRowView rowView in metaData.DefaultView)
			{
				try
				{
					DataRow row = rowView.Row;

					keyName = row["FK_NAME"] as string;

					key = this.GetByName(keyName);

					if(null == key)
					{
						key = (ForeignKey)this.dbRoot.ClassFactory.CreateForeignKey();
						key.dbRoot = this.dbRoot;
						key.ForeignKeys = this;
						key.Row = row;
						this._array.Add(key);
					}

					string catalog = (DBNull.Value == row["PK_TABLE_CATALOG"]) ? string.Empty : (row["PK_TABLE_CATALOG"] as string);
					string schema  = (DBNull.Value == row["PK_TABLE_SCHEMA"])  ? string.Empty : (row["PK_TABLE_SCHEMA"] as string);
					key.AddForeignColumn(catalog, schema, (string)row["PK_TABLE_NAME"], (string)row["PK_COLUMN_NAME"], true);

					catalog = (DBNull.Value == row["FK_TABLE_CATALOG"]) ? string.Empty : (row["FK_TABLE_CATALOG"] as string);
					schema  = (DBNull.Value == row["FK_TABLE_SCHEMA"])  ? string.Empty : (row["FK_TABLE_SCHEMA"] as string);
					key.AddForeignColumn(catalog, schema, (string)row["FK_TABLE_NAME"], (string)row["FK_COLUMN_NAME"], false);
				}
				catch {}
			}
		}

		internal void PopulateArrayNoHookup(DataTable metaData)
		{
			BindToColumns(metaData);

			ForeignKey key  = null;
			string keyName = "";

			foreach(DataRowView rowView in metaData.DefaultView)
			{
				DataRow row = rowView.Row;

				keyName = row["FK_NAME"] as string;

				key = this.GetByName(keyName);

				if(null == key)
				{
					key = (ForeignKey)this.dbRoot.ClassFactory.CreateForeignKey();
					key.dbRoot = this.dbRoot;
					key.ForeignKeys = this;
					key.Row = row;
					this._array.Add(key);
				}
			}
		}

		internal void AddForeignKey(ForeignKey fk)
		{
			IForeignKey exists = this[fk.Name];

			if(null == exists)
			{
				this._array.Add(fk);
			}
		}

		#region indexers

		virtual public IForeignKey this[object index]
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
					return this._array[idx] as ForeignKey;
				}
			}
		}

		public ForeignKey GetByName(string name)
		{
			ForeignKey obj = null;
			ForeignKey tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as ForeignKey;

				if(this.CompareStrings(name,tmp.Name))
				{
					obj = tmp;
					break;
				}
			}

			return obj;
		}

		internal ForeignKey GetByPhysicalName(string name)
		{
			ForeignKey obj = null;
			ForeignKey tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as ForeignKey;

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
				return Table.UserDataXPath + @"/ForeignKeys";
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
				if(this.Table.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./ForeignKeys";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "ForeignKeys", null);
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
	}
}
