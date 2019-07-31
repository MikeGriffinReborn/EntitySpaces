using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Oracle
{
	public class OracleColumn : Column
	{
		public OracleColumn()
		{

		}

        public override bool HasDefault
        {
            get
            {
                return (this.Default != null && this.Default.Length > 0);
            }
        }

		override internal Column Clone()
		{
			Column c = base.Clone();

			return c;
		}

		public override string DataTypeName
		{
			get
			{
				OracleColumns cols = Columns as OracleColumns;
				return this.GetString(cols.f_TypeName);
			}
		}


		override public string DataTypeNameComplete
		{
			get
			{
                return GetFullDataTypeName(DataTypeName, CharacterMaxLength, NumericPrecision, NumericScale).Replace("\'", string.Empty);
			}
		}

        internal static string GetFullDataTypeName(string name, int charMaxLen, int precision, int scale)
        {
            string dtnf = null;
            switch (name)
            {
                case "VARCHAR2":
                case "NVARCHAR2":
                case "RAW":
                case "LONGRAW":
                case "BFILE":
                case "BLOB":
                case "CHAR":
                case "NCHAR":
                    dtnf = name + "(" + charMaxLen + ")";
                    break;

                case "FLOAT":
                case "INTEGER":
                case "NUMBER":
                case "UNSIGNEDINTEGER":

                    dtnf = name + "(" + precision + "," + scale + ")";
                    break;

                default:

                    dtnf = name;
                    break;
            }

            return dtnf;
        }
	}
}
