using System;
using System.Xml;
using System.Collections;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class Indexes : Collection, IIndexes, IEnumerable, ICollection
	{
		public Indexes()
		{

		}

		internal DataColumn f_Catalog			= null;
		internal DataColumn f_Schema			= null;
		internal DataColumn f_TableName			= null;
		internal DataColumn f_IndexCatalog		= null;
		internal DataColumn f_IndexSchema		= null;
		internal DataColumn f_IndexName			= null;
		internal DataColumn f_Unique			= null;
		internal DataColumn f_Clustered			= null;
		internal DataColumn f_Type				= null;
		internal DataColumn f_FillFactor		= null;
		internal DataColumn f_InitializeSize	= null;
		internal DataColumn f_Nulls				= null;
		internal DataColumn f_SortBookmarks		= null;
		internal DataColumn f_AutoUpdate		= null;
		internal DataColumn f_NullCollation		= null;
		internal DataColumn f_Collation			= null;
		internal DataColumn f_Cardinality		= null;
		internal DataColumn f_Pages				= null;
		internal DataColumn f_FilterCondition	= null;
		internal DataColumn f_Integrated		= null;

		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("TABLE_CATALOG"))			f_Catalog			 = metaData.Columns["TABLE_CATALOG"];
				if(metaData.Columns.Contains("TABLE_SCHEMA"))			f_Schema			 = metaData.Columns["TABLE_SCHEMA"];
				if(metaData.Columns.Contains("TABLE_NAME"))				f_TableName			 = metaData.Columns["TABLE_NAME"];
				if(metaData.Columns.Contains("INDEX_CATALOG"))			f_IndexCatalog		 = metaData.Columns["INDEX_CATALOG"];
				if(metaData.Columns.Contains("INDEX_SCHEMA"))			f_IndexSchema		 = metaData.Columns["INDEX_SCHEMA"];
				if(metaData.Columns.Contains("INDEX_NAME"))				f_IndexName			 = metaData.Columns["INDEX_NAME"];
				if(metaData.Columns.Contains("UNIQUE"))					f_Unique			 = metaData.Columns["UNIQUE"];
				if(metaData.Columns.Contains("CLUSTERED"))				f_Clustered			 = metaData.Columns["CLUSTERED"];
				if(metaData.Columns.Contains("TYPE"))					f_Type				 = metaData.Columns["TYPE"];
				if(metaData.Columns.Contains("FILL_FACTOR"))			f_FillFactor		 = metaData.Columns["FILL_FACTOR"];
				if(metaData.Columns.Contains("INITIAL_SIZE"))			f_InitializeSize	 = metaData.Columns["INITIAL_SIZE"];
				if(metaData.Columns.Contains("NULLS"))					f_Nulls				 = metaData.Columns["NULLS"];
				if(metaData.Columns.Contains("SORT_BOOKMARKS"))			f_SortBookmarks		 = metaData.Columns["SORT_BOOKMARKS"];
				if(metaData.Columns.Contains("AUTO_UPDATE"))			f_AutoUpdate		 = metaData.Columns["AUTO_UPDATE"];
				if(metaData.Columns.Contains("NULL_COLLATION"))			f_NullCollation		 = metaData.Columns["NULL_COLLATION"];
				if(metaData.Columns.Contains("COLLATION"))				f_Collation			 = metaData.Columns["COLLATION"];
				if(metaData.Columns.Contains("CARDINALITY"))			f_Cardinality		 = metaData.Columns["CARDINALITY"];
				if(metaData.Columns.Contains("PAGES"))					f_Pages				 = metaData.Columns["PAGES"];
				if(metaData.Columns.Contains("FILTER_CONDITION"))		f_FilterCondition	 = metaData.Columns["FILTER_CONDITION"];
				if(metaData.Columns.Contains("INTEGRATED"))				f_Integrated		 = metaData.Columns["INTEGRATED"];
			}
		}

		virtual internal void LoadAll()
		{

		}

		internal void PopulateArray(DataTable metaData)
		{
			BindToColumns(metaData);

			Index index = null;
			string indexName = "";
				
			DataRowCollection rows = metaData.Rows; 
			int count = rows.Count;
			for(int i = 0; i < count; i++)
			{
				indexName = rows[i]["INDEX_NAME"] as string;

				index = this.GetByName(indexName);

				if(null == index)
				{
					index = (Index)this.dbRoot.ClassFactory.CreateIndex();
					index.dbRoot = this.dbRoot;
					index.Indexes = this;
					index.Row = metaData.Rows[i];
					this._array.Add(index);
				}

				index.AddColumn(rows[i]["COLUMN_NAME"] as string);
			}
		}

		internal void PopulateArrayNoHookup(DataTable metaData)
		{
			BindToColumns(metaData);

			Index index = null;
			string indexName = "";
				
			DataRowCollection rows = metaData.Rows; 
			int count = rows.Count;
			for(int i = 0; i < count; i++)
			{
				indexName = rows[i]["INDEX_NAME"] as string;

				index = this.GetByName(indexName);

				if(null == index)
				{
					index = (Index)this.dbRoot.ClassFactory.CreateIndex();
					index.dbRoot = this.dbRoot;
					index.Indexes = this;
					index.Row = metaData.Rows[i];
					this._array.Add(index);
				}
			}
		}

		internal void AddIndex(Index index)
		{
			this._array.Add(index);
		}

		#region indexers

		virtual public IIndex this[object index]
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
					return this._array[idx] as Index;
				}
			}
		}

		public Index GetByName(string name)
		{
			Index obj = null;
			Index tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Index;

				if(this.CompareStrings(name,tmp.Name))
				{
					obj = tmp;
					break;
				}
			}

			return obj;
		}

		public Index GetByPhysicalName(string name)
		{
			Index obj = null;
			Index tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Index;

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
				return Table.UserDataXPath + @"/Indexes";
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
					string xPath = @"./Indexes";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Indexes", null);
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
