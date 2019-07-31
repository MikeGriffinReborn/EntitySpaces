using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Sql
{
	public class SqlProcedure : Procedure
	{
		public SqlProcedure()
		{

		}

		override public string Alias
		{
			get
			{
				string[] name = base.Name.Split(';');

				return name[0];
			}
		}

		override public string Name
		{
			get
			{
				string[] name = base.Name.Split(';');

				return name[0];
			}
		}

		override public string ProcedureText 
		{
			get 
			{
				string tmp = base.ProcedureText;
				if (tmp.Length == 0) 
				{
					tmp = LoadProcedureSource();
				}
				return tmp;
			}
		}

		
		private string LoadProcedureSource()
		{
			string text = string.Empty;
			OleDbConnection cn = null;
			OleDbDataReader reader = null;
			try
			{
				string select = string.Format(@"SELECT CASE WHEN encrypted = 1 THEN NULL ELSE com.text END as Source FROM sysobjects o, syscomments com 
WHERE o.id = object_id(N'[{0}].[{1}]')
and com.id=o.id 
and com.status=2 
order by colid;", this.Schema, this.Name);
				cn = new OleDbConnection(dbRoot.ConnectionString);
				cn.Open();
				cn.ChangeDatabase(this.Database.Name);

				OleDbCommand cmd = cn.CreateCommand();
				cmd.CommandText = select;
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        text += reader[0].ToString();
                    }

                    reader.Close();
                }
                catch
                {
                    if (reader != null)
                        reader.Close();
                }

                if (text == string.Empty)
                {
					select = string.Format(@"SELECT CASE WHEN encrypted = 1 THEN NULL ELSE com.text END as Source FROM sysobjects o, syscomments com 
WHERE o.id = object_id(N'[{0}].[{1}]')
and com.id=o.id 
order by colid;", this.Schema, this.Name);

                    cmd = cn.CreateCommand();
                    cmd.CommandText = select;
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        text += reader[0].ToString();
                    }

                    reader.Close();
                }

                cn.Close();
				text = text.TrimStart(' ', '\r', '\n', '\t');
			}
			catch 
			{
				if (reader != null) 
					reader.Close();
				if ((cn != null) && (cn.State != ConnectionState.Closed) && (cn.State != ConnectionState.Broken) )
					cn.Close();
			}

			return text;
		}

		public override object DatabaseSpecificMetaData(string key)
		{
			return SqlDatabase.DBSpecific(key, this);
		}
	}
}
