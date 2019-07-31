using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Sql
{
	public class SqlColumn : Column
	{
		public SqlColumn()
		{

		}

		override internal Column Clone()
		{
			Column c = base.Clone();

			return c;
		}

		override public System.Boolean IsComputed
		{
			get
			{
				if(this.DataTypeName == "timestamp") return true;

				return base.IsComputed;
			}
		}

        public override bool IsConcurrency
        {
            get
            {
			    if(this.DataTypeName.ToLower() == "timestamp")
			    {
    				return true;
			    }

                return false;
            }
        }

        public override string LanguageType
        {
            get
            {
               return base.LanguageType;
            }
        }


		override public string DataTypeName
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.DataTypeName;
						}
					}
				}

				SqlColumns cols = Columns as SqlColumns;
				return this.GetString(cols.f_TypeName);
			}
		}

		override public string DataTypeNameComplete
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.DataTypeNameComplete;
						}
					}
				}

                return GetFullDataTypeName(DataTypeName, CharacterMaxLength, NumericPrecision, NumericScale).Replace("\'", string.Empty);
			}
		}

		public override object DatabaseSpecificMetaData(string key)
		{
			return SqlDatabase.DBSpecific(key, this);
		}

        internal static string GetFullDataTypeName(string name, int charMaxLen, int precision, int scale)
        {
            string dtnf = null;
            switch (name)
            {
                case "varchar":
                case "nvarchar":
                case "varbinary":
                    if (charMaxLen > 1000000)
                        dtnf = name + "(MAX)";
                    else
                        dtnf = name + "(" + charMaxLen + ")";
                    break;
                case "binary":
                case "char":
                case "nchar":

                    dtnf = name + "(" + charMaxLen + ")";
                    break;

                case "decimal":
                case "numeric":

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
