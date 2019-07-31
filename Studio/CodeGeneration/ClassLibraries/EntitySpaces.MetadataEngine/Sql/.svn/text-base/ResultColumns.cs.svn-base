using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace EntitySpaces.MetadataEngine.Sql
{
	public class SqlResultColumns : ResultColumns
	{
		public SqlResultColumns()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string schema = "";

				if(-1 == this.Procedure.Schema.IndexOf("."))
				{
					schema = this.Procedure.Schema + ".";
				}

                //SET FMTONLY ON 
				string select = "EXEC [" + this.Procedure.Database.Name + "]." + schema + "[" +
					this.Procedure.Name + "] ";

				int paramCount = this.Procedure.Parameters.Count;

				if(paramCount > 0)
				{
					IParameters parameters = this.Procedure.Parameters;
					IParameter param = null;

					int c = parameters.Count;

					for(int i = 0; i < c; i++)
					{
						param = parameters[i];

						if(param.Direction == ParamDirection.ReturnValue)
						{
							paramCount--;
						}
					}
				}

				for(int i = 0; i < paramCount; i++)
				{
					if(i > 0) 
					{
						select += ",";
					}

					select += "null";
				}

				DataTable metaData = new DataTable();

				try
				{
                    //Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Initial Catalog=Northwind;Data Source=localhost
                    //Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;
                    //Provider=SQLNCLI;Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;
                    //Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;

					string[] pairs = dbRoot.ConnectionString.Split(';');
                    Hashtable conn = new Hashtable();
                    int idx;
                    string name, val;
                    foreach (string pairstr in pairs)
                    {
                        idx = pairstr.IndexOf('=');
                        if (idx > 0)
                        {
                            name = pairstr.Substring(0, idx);
                            val = pairstr.Substring(idx + 1);
                            conn[name.Trim()] = val.Trim();
                        }
                    }

					string cn = "", tmp;
					foreach(string key in conn.Keys)
                    {
                        tmp = conn[key] as string;
						switch(key.ToLower())
						{
							case "provider":
								break;
							case "extended properties":
                                break;
                            case "server":
                            case "data source":
                                cn += "Data Source=" + tmp + ";";
                                break;
                            case "user id":
                            case "uid":
                                cn += "User ID=" + tmp + ";";
                                break;
                            case "password":
                            case "pwd":
                                cn += "Password=" + tmp + ";"; 
                                break;
                            case "initial catalog":
                            case "database":
                                cn += "Initial Catalog=" + tmp + ";";
                                break;
                            case "marsconn":
                                if (tmp.ToLower() == "yes")
                                {
                                    cn += "MultipleActiveResultSets=" + ((tmp.ToLower() == "yes") ? "true" : "false") + ";";
                                }
                                break;
							default:
                                cn += key + "=" + tmp + ";"; 
								break;
						}
					}
                    SqlConnection sqlconn = new SqlConnection(cn);
                    sqlconn.Open();
                    SqlCommand sqlcmd = sqlconn.CreateCommand(); 
                    sqlcmd.CommandText = select;
                    sqlcmd.CommandType = CommandType.Text;
                    SqlDataReader reader = sqlcmd.ExecuteReader(CommandBehavior.SchemaOnly);

                    metaData = reader.GetSchemaTable();
                    SqlResultColumn resultColumn;
                    foreach (DataRow row in metaData.Rows)
                    {
                        resultColumn = this.dbRoot.ClassFactory.CreateResultColumn() as Sql.SqlResultColumn;
                        resultColumn.dbRoot = this.dbRoot;
                        resultColumn.ResultColumns = this;
                        resultColumn._row = row;
                        this._array.Add(resultColumn);
                    }
				}
				catch
                {
                   //
				}

			}
			catch {}
		}
	}
}
