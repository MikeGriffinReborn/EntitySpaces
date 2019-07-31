using System;
using System.Xml;
using System.Collections;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class Parameters : Collection, IParameters, IEnumerable, ICollection
	{
		public Parameters()
		{

		}

		internal DataColumn f_Catalog			= null;
		internal DataColumn f_Schema			= null;
		internal DataColumn f_ProcedureName		= null;
		internal DataColumn f_ParameterName		= null;
		internal DataColumn f_Ordinal			= null;
		internal DataColumn f_Type				= null;
		internal DataColumn f_HasDefault		= null;
		internal DataColumn f_Default			= null;
		internal DataColumn f_IsNullable		= null;
		internal DataColumn f_DataType			= null;
		internal DataColumn f_CharMaxLength		= null;
		internal DataColumn f_CharOctetLength	= null;
		internal DataColumn f_NumericPrecision  = null;
		internal DataColumn f_NumericScale		= null;
		internal DataColumn f_Description		= null;
		internal DataColumn f_TypeName			= null;
		internal DataColumn f_FullTypeName		= null;
		internal DataColumn f_LocalTypeName		= null;


		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("PROCEDURE_CATALOG"))			f_Catalog			 = metaData.Columns["PROCEDURE_CATALOG"];
				if(metaData.Columns.Contains("PROCEDURE_SCHEMA"))			f_Schema			 = metaData.Columns["PROCEDURE_SCHEMA"];
				if(metaData.Columns.Contains("PROCEDURE_NAME"))				f_ProcedureName		 = metaData.Columns["PROCEDURE_NAME"];
				if(metaData.Columns.Contains("PARAMETER_NAME"))				f_ParameterName		 = metaData.Columns["PARAMETER_NAME"];
				if(metaData.Columns.Contains("ORDINAL_POSITION"))			f_Ordinal			 = metaData.Columns["ORDINAL_POSITION"];
				if(metaData.Columns.Contains("PARAMETER_TYPE"))				f_Type				 = metaData.Columns["PARAMETER_TYPE"];
				if(metaData.Columns.Contains("PARAMETER_HASDEFAULT"))		f_HasDefault		 = metaData.Columns["PARAMETER_HASDEFAULT"];
				if(metaData.Columns.Contains("PARAMETER_DEFAULT"))			f_Default			 = metaData.Columns["PARAMETER_DEFAULT"];
				if(metaData.Columns.Contains("IS_NULLABLE"))				f_IsNullable		 = metaData.Columns["IS_NULLABLE"];
				if(metaData.Columns.Contains("DATA_TYPE"))					f_DataType			 = metaData.Columns["DATA_TYPE"];
				if(metaData.Columns.Contains("CHARACTER_MAXIMUM_LENGTH"))	f_CharMaxLength		 = metaData.Columns["CHARACTER_MAXIMUM_LENGTH"];
				if(metaData.Columns.Contains("CHARACTER_OCTET_LENGTH"))		f_CharOctetLength	 = metaData.Columns["CHARACTER_OCTET_LENGTH"];
				if(metaData.Columns.Contains("NUMERIC_PRECISION"))			f_NumericPrecision   = metaData.Columns["NUMERIC_PRECISION"];
				if(metaData.Columns.Contains("NUMERIC_SCALE"))				f_NumericScale		 = metaData.Columns["NUMERIC_SCALE"];
				if(metaData.Columns.Contains("DESCRIPTION"))				f_Description		 = metaData.Columns["DESCRIPTION"];
				if(metaData.Columns.Contains("FULL_TYPE_NAME"))				f_FullTypeName		 = metaData.Columns["FULL_TYPE_NAME"];
				if(metaData.Columns.Contains("TYPE_NAME"))					f_TypeName			 = metaData.Columns["TYPE_NAME"];
				if(metaData.Columns.Contains("LOCAL_TYPE_NAME"))			f_LocalTypeName		 = metaData.Columns["LOCAL_TYPE_NAME"];

			}
		}

		virtual internal void LoadAll()
		{

		}

		internal void PopulateArray(DataTable metaData)
		{
			BindToColumns(metaData);

			Parameter param = null;

			int count = metaData.Rows.Count;
			for(int i = 0; i < count; i++)
			{
				param = (Parameter)this.dbRoot.ClassFactory.CreateParameter();
                param.dbRoot = this.dbRoot;
				param.Parameters = this;
				param.Row = metaData.Rows[i];
				this._array.Add(param);
			}
		}

		internal void AddTable(Parameter param)
		{
			this._array.Add(param);
		}

		#region indexers

		virtual public IParameter this[object index]
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
					return this._array[idx] as Parameter;
				}
			}
		}

		public Parameter GetByName(string name)
		{
			Parameter obj = null;
			Parameter tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Parameter;

				if(this.CompareStrings(name,tmp.Name))
				{
					obj = tmp;
					break;
				}
			}

			return obj;
		}

		public Parameter GetByPhysicalName(string name)
		{
			Parameter obj = null;
			Parameter tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Parameter;

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
				return Procedure.UserDataXPath + @"/Parameters";
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
				if(this.Procedure.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Parameters";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Parameters", null);
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

		internal Procedure Procedure = null;
	}
}
