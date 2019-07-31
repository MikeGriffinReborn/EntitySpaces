using System;
using System.Xml;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class ProviderType : Single, IProviderType, INameValueItem
	{
		public ProviderType()
		{

		}

		#region Properties

		virtual public string Type
		{ 
			get
			{
				return this.GetString(ProviderTypes.f_Type);
			}
		}

		virtual public System.Int32 DataType 
		{ 
			get
			{
				return this.GetInt32(ProviderTypes.f_DataType);
			}
		}

		virtual public System.Int32 ColumnSize
		{ 
			get
			{
				return this.GetInt32(ProviderTypes.f_ColumnSize);
			}
		}

		virtual public string LiteralPrefix 
		{ 
			get
			{
				return this.GetString(ProviderTypes.f_LiteralPrefix);
			}
		}

		virtual public string LiteralSuffix
		{ 
			get
			{
				return this.GetString(ProviderTypes.f_LiteralSuffix);
			}
		}

		virtual public string CreateParams
		{ 
			get
			{
				return this.GetString(ProviderTypes.f_CreateParams);
			}
		}

		virtual public System.Boolean IsNullable
		{ 
			get
			{
				return this.GetBool(ProviderTypes.f_IsNullable);
			}
		}

		virtual public System.Boolean IsCaseSensitive
		{ 
			get
			{
				return this.GetBool(ProviderTypes.f_IsCaseSensitive);
			}
		}

		virtual public string Searchable
		{ 
			get
			{
				return this.GetString(ProviderTypes.f_Searchable);
			}
		}

		virtual public System.Boolean IsUnsigned
		{ 
			get
			{
				return this.GetBool(ProviderTypes.f_IsUnsigned);
			}
		}

		virtual public System.Boolean HasFixedPrecScale
		{ 
			get
			{
				return this.GetBool(ProviderTypes.f_HasFixedPrecScale);
			}
		}

		virtual public System.Boolean CanBeAutoIncrement
		{ 
			get
			{
				return this.GetBool(ProviderTypes.f_CanBeAutoIncrement);
			}
		}

		virtual public string LocalType
		{ 
			get
			{
				return this.GetString(ProviderTypes.f_LocalType);
			}
		}

		virtual public System.Int32 MinimumScale
		{ 
			get
			{
				return this.GetInt32(ProviderTypes.f_MinimumScale);
			}
		}

		virtual public System.Int32 MaximumScale
		{ 
			get
			{
				return this.GetInt32(ProviderTypes.f_MaximumScale);
			}
		}

		virtual public Guid TypeGuid
		{ 
			get
			{
				return this.GetGuid(ProviderTypes.f_TypeGuid);
			}
		}

		virtual public string TypeLib
		{ 
			get
			{
				return this.GetString(ProviderTypes.f_TypeLib);
			}
		}

		virtual public string Version
		{ 
			get
			{
				return this.GetString(ProviderTypes.f_Version);
			}
		}

		virtual public System.Boolean IsLong
		{ 
			get
			{
				return this.GetBool(ProviderTypes.f_IsLong);
			}
		}

		virtual public System.Boolean BestMatch
		{ 
			get
			{
				return this.GetBool(ProviderTypes.f_BestMatch);
			}
		}

		virtual public System.Boolean IsFixedLength
		{ 
			get
			{
				return this.GetBool(ProviderTypes.f_IsFixedLength);
			}
		}

		#endregion

		#region INameValueCollection Members

		public string ItemName
		{
			get
			{
				return this.Type;
			}
		}

		public string ItemValue
		{
			get
			{
				return this.Type;
			}
		}

		#endregion

		internal ProviderTypes ProviderTypes = null;
	}
}
