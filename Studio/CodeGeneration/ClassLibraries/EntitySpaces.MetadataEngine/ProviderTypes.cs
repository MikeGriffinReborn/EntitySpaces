using System;
using System.Xml;
using System.Collections;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class ProviderTypes : Collection, IProviderTypes, IEnumerable, ICollection
	{
		public ProviderTypes()
		{

		}

		internal DataColumn f_Type					= null;
		internal DataColumn f_DataType				= null;
		internal DataColumn f_ColumnSize			= null;
		internal DataColumn f_LiteralPrefix			= null;
		internal DataColumn f_LiteralSuffix			= null;
		internal DataColumn f_CreateParams			= null;
		internal DataColumn f_IsNullable			= null;
		internal DataColumn f_IsCaseSensitive		= null;
		internal DataColumn f_Searchable			= null;
		internal DataColumn f_IsUnsigned			= null;
		internal DataColumn f_HasFixedPrecScale		= null;
		internal DataColumn f_CanBeAutoIncrement	= null;
		internal DataColumn f_LocalType				= null;
		internal DataColumn f_MinimumScale			= null;
		internal DataColumn f_MaximumScale			= null;
		internal DataColumn f_TypeGuid				= null;
		internal DataColumn f_TypeLib				= null;
		internal DataColumn f_Version				= null;
		internal DataColumn f_IsLong				= null;
		internal DataColumn f_BestMatch				= null;
		internal DataColumn f_IsFixedLength			= null;

		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("TYPE_NAME"))			f_Type					= metaData.Columns["TYPE_NAME"];
				if(metaData.Columns.Contains("DATA_TYPE"))			f_DataType				= metaData.Columns["DATA_TYPE"];
				if(metaData.Columns.Contains("COLUMN_SIZE "))		f_ColumnSize			= metaData.Columns["COLUMN_SIZE"];
				if(metaData.Columns.Contains("LITERAL_PREFIX"))		f_LiteralPrefix			= metaData.Columns["LITERAL_PREFIX"];
				if(metaData.Columns.Contains("LITERAL_SUFFIX"))		f_LiteralSuffix			= metaData.Columns["LITERAL_SUFFIX"];
				if(metaData.Columns.Contains("CREATE_PARAMS "))		f_CreateParams			= metaData.Columns["CREATE_PARAMS"];
				if(metaData.Columns.Contains("IS_NULLABLE"))		f_IsNullable			= metaData.Columns["IS_NULLABLE"];
				if(metaData.Columns.Contains("CASE_SENSITIVE "))	f_IsCaseSensitive		= metaData.Columns["CASE_SENSITIVE"];
				if(metaData.Columns.Contains("SEARCHABLE"))			f_Searchable			= metaData.Columns["SEARCHABLE"];
				if(metaData.Columns.Contains("UNSIGNED_ATTRIBUTE"))	f_IsUnsigned			= metaData.Columns["UNSIGNED_ATTRIBUTE"];
				if(metaData.Columns.Contains("FIXED_PREC_SCALE"))	f_HasFixedPrecScale		= metaData.Columns["FIXED_PREC_SCALE"];
				if(metaData.Columns.Contains("AUTO_UNIQUE_VALUE"))	f_CanBeAutoIncrement	= metaData.Columns["AUTO_UNIQUE_VALUE"];
				if(metaData.Columns.Contains("LOCAL_TYPE_NAME"))	f_LocalType				= metaData.Columns["LOCAL_TYPE_NAME"];
				if(metaData.Columns.Contains("MINIMUM_SCALE"))		f_MinimumScale			= metaData.Columns["MINIMUM_SCALE"];
				if(metaData.Columns.Contains("MAXIMUM_SCALE"))		f_MaximumScale			= metaData.Columns["MAXIMUM_SCALE"];
				if(metaData.Columns.Contains("GUID"))				f_TypeGuid				= metaData.Columns["GUID"];
				if(metaData.Columns.Contains("TYPELIB"))			f_TypeLib				= metaData.Columns["TYPELIB"];
				if(metaData.Columns.Contains("VERSION"))			f_Version				= metaData.Columns["VERSION"];
				if(metaData.Columns.Contains("VARIANT_FALSE"))		f_IsLong				= metaData.Columns["VARIANT_FALSE"];
				if(metaData.Columns.Contains("BEST_MATCH "))		f_BestMatch				= metaData.Columns["BEST_MATCH"];
				if(metaData.Columns.Contains("IS_FIXEDLENGTH"))		f_IsFixedLength			= metaData.Columns["IS_FIXEDLENGTH"];
			}
		}

		internal virtual void LoadAll()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Provider_Types, null);

			PopulateArray(metaData);
		}

		protected void PopulateArray(DataTable metaData)
		{
			BindToColumns(metaData);

			ProviderType type = null;

			int count = metaData.Rows.Count;
			for(int i = 0; i < count; i++)
			{
				type = (ProviderType)this.dbRoot.ClassFactory.CreateProviderType();
				type.dbRoot = this.dbRoot;
				type.ProviderTypes = this;
				type.Row = metaData.Rows[i];
				this._array.Add(type);
			}
		}

		#region indexers

		public IProviderType this[object index]
		{
			get
			{
				if(index.GetType() == Type.GetType("System.String"))
				{
					return GetByType(index as String);
				}
				else
				{
					int idx = Convert.ToInt32(index);
					return this._array[idx] as ProviderType;
				}
			}
		}

		public ProviderType GetByType(string type)
		{
			ProviderType obj = null;
			ProviderType tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as ProviderType;

				if(this.CompareStrings(type,tmp.Type))
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
	}
}
