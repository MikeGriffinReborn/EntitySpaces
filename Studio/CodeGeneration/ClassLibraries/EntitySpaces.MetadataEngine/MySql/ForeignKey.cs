using System;
using System.Data;

namespace EntitySpaces.MetadataEngine.MySql
{
	public class MySqlForeignKey : ForeignKey
	{
		public MySqlForeignKey()
		{

		}

		override public ITable ForeignTable
		{
			get
			{
				string catalog = this.ForeignKeys.Table.Database.Name;
				string schema  = this.GetString(ForeignKeys.f_FKTableSchema);

				return this.dbRoot.Databases[catalog].Tables[this.GetString(ForeignKeys.f_FKTableName)];
			}
		}
	
		public override string PrimaryKeyName
		{
			get
			{
				if(this.PrimaryTable.Indexes["PRIMARY"] != null)
					return "PRIMARY";
				else
					return base.PrimaryKeyName;
			}
		}
	}
}
