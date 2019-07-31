using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Access
{
	public class AccessParameter : Parameter
	{
		public AccessParameter()
		{

		}

		override public string TypeName
		{
			get
			{
				AccessParameters param = this.Parameters as AccessParameters;
				string type = this.GetString(param.f_TypeName);

				switch(type)
				{
					case "adVarWChar":
						return "Text";
					case "adLongVarWChar":
						return "Memo";
					case "adUnsignedTinyInt":
						return "Byte";
					case "adCurrency":
						return "Currency";
					case "adDate":
						return "DateTime";
					case "adBoolean":
						return @"Yes/No";
					case "adLongVarBinary":
						return "OLE Object";
					case "adInteger":
						return "Long";
					case "adDouble":
						return "Double";
					case "adGUID":
						return "Replication ID";
					case "adSingle":
						return "Single";
					case "adNumeric":
						return "Decimal";
					case "adSmallInt":
						return "Integer";
					case "adVarBinary":
						return "Binary";
					case "Hyperlink":
						return "Hyperlink";
					default:
						return type;
				}
			}
		}

		override public string DataTypeNameComplete
		{
			get
			{
				AccessParameters param = this.Parameters as AccessParameters;
				string type = this.GetString(param.f_TypeName);

				switch(type)
				{
					case "adVarWChar":
						return "Text";
					case "adLongVarWChar":
						return "Memo";
					case "adUnsignedTinyInt":
						return "Byte";
					case "adCurrency":
						return "Currency";
					case "adDate":
						return "DateTime";
					case "adBoolean":
						//return @"Yes/No";
						return "Bit";
					case "adLongVarBinary":
						//return "OLE Object";
						return "LongBinary";
					case "adInteger":
						return "Long";
					case "adDouble":
						return "IEEEDouble";
					case "adGUID":
						//return "Replication ID";
						return "Guid";
					case "adSingle":
						return "IEEESingle";
					case "adNumeric":
						return "Decimal";
					case "adSmallInt":
						return "Integer";
					case "adVarBinary":
						return "Binary";
					case "Hyperlink":
						return "Text (255)";
					default:
						return type;
				}
			}
		}
	}
}
