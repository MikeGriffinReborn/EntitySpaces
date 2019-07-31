using System;
using System.Data;
using System.Data.OleDb;

using ADODB;

namespace EntitySpaces.MetadataEngine.PostgreSQL
{
	public class PostgreSQLDatabase : Database
	{
		public PostgreSQLDatabase()
		{

		}

		override public ADODB.Recordset ExecuteSql(string sql)
		{
            IDbConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Name);

			return this.ExecuteIntoRecordset(sql, cn);
		}
	}
}
