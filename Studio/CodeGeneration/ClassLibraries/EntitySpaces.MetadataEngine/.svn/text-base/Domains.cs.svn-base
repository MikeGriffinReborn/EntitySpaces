using System;
using System.Xml;
using System.Collections;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class Domains : Collection, IDomains, IEnumerable, ICollection
	{
		public Domains()
		{

		}

		#region DataColumn Binding Stuff


		internal DataColumn f_HasDefault        = null;
		internal DataColumn f_Default			= null;
		internal DataColumn f_IsNullable		= null;
		internal DataColumn f_DataType			= null;	
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

		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("DOMAIN_CATALOG"))						f_DomainCatalog		= metaData.Columns["DOMAIN_CATALOG"];
				if(metaData.Columns.Contains("DOMAIN_SCHEMA"))						f_DomainSchema		= metaData.Columns["DOMAIN_SCHEMA"];
				if(metaData.Columns.Contains("DOMAIN_NAME"))						f_DomainName		= metaData.Columns["DOMAIN_NAME"];
				if(metaData.Columns.Contains("DATA_TYPE"))							f_DataType			= metaData.Columns["DATA_TYPE"];
				if(metaData.Columns.Contains("CHARACTER_MAXIMUM_LENGTH"))			f_MaxLength			= metaData.Columns["CHARACTER_MAXIMUM_LENGTH"];
				if(metaData.Columns.Contains("CHARACTER_OCTET_LENGTH"))				f_OctetLength		= metaData.Columns["CHARACTER_OCTET_LENGTH"];
				if(metaData.Columns.Contains("COLLATION_CATALOG"))					f_CollationCatalog	= metaData.Columns["COLLATION_CATALOG"];
				if(metaData.Columns.Contains("COLLATION_SCHEMA"))					f_CollationSchema	= metaData.Columns["COLLATION_SCHEMA"];
				if(metaData.Columns.Contains("COLLATION_NAME"))						f_CollationName		= metaData.Columns["COLLATION_NAME"];
				if(metaData.Columns.Contains("CHARACTER_SET_CATALOG"))				f_CharSetCatalog    = metaData.Columns["CHARACTER_SET_CATALOG"];
				if(metaData.Columns.Contains("CHARACTER_SET_SCHEMA"))				f_CharSetSchema     = metaData.Columns["CHARACTER_SET_SCHEMA"];
				if(metaData.Columns.Contains("CHARACTER_SET_NAME"))					f_CharSetName       = metaData.Columns["CHARACTER_SET_NAME"];
				if(metaData.Columns.Contains("NUMERIC_PRECISION"))					f_NumericPrecision	= metaData.Columns["NUMERIC_PRECISION"];
				if(metaData.Columns.Contains("NUMERIC_SCALE"))						f_NumericScale		= metaData.Columns["NUMERIC_SCALE"];
				if(metaData.Columns.Contains("DATETIME_PRECISION"))					f_DatetimePrecision = metaData.Columns["DATETIME_PRECISION"];
				if(metaData.Columns.Contains("DOMAIN_DEFAULT"))						f_Default			= metaData.Columns["DOMAIN_DEFAULT"];
				if(metaData.Columns.Contains("IS_NULLABLE"))						f_IsNullable		= metaData.Columns["IS_NULLABLE"];
			}																		
		}
		#endregion

		virtual internal void LoadAll()
		{

		}

		internal void PopulateArray(DataTable metaData)
		{
			BindToColumns(metaData);

			Domain domain = null;

			if(metaData.DefaultView.Count > 0)
			{
				IEnumerator enumerator = metaData.DefaultView.GetEnumerator();
				while(enumerator.MoveNext())
				{
					DataRowView rowView = enumerator.Current as DataRowView;

					domain = (Domain)this.dbRoot.ClassFactory.CreateDomain();
					domain.dbRoot = this.dbRoot;
					domain.Domains = this;
					domain.Row = rowView.Row;
					this._array.Add(domain);
				}
			}
		}

		#region indexers

		public IDomain this[object index]
		{
			get
			{
				if(index.GetType() == Type.GetType("System.String"))
				{
					return GetByPhysicalName(index as String) as IDomain;
				}
				else
				{
					int idx = Convert.ToInt32(index);
					return this._array[idx] as IDomain;
				}
			}
		}
		
		public Domain GetByName(string name)
		{
			Domain obj = null;
			Domain tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Domain;

				if(this.CompareStrings(name,tmp.Name))
				{
					obj = tmp;
					break;
				}
			}

			return obj;
		}
		
		internal Domain GetByPhysicalName(string name)
		{
			Domain obj = null;
			Domain tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Domain;

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
				return Database.UserDataXPath + @"/Domains";
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
					string xPath = @"./Domains";
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

		internal Database Database = null;
	}
}
