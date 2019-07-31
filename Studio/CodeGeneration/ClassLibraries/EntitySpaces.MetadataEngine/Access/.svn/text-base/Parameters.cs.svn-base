using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Access
{
	public class AccessParameters : Parameters
	{
		public AccessParameters()
		{

		}

		private DataTable CreateDataTable()
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("PROCEDURE_CATALOG", Type.GetType("System.String"));
			dt.Columns.Add("PROCEDURE_SCHEMA", Type.GetType("System.String"));
			dt.Columns.Add("PROCEDURE_NAME", Type.GetType("System.String"));
			dt.Columns.Add("PARAMETER_NAME", Type.GetType("System.String"));
			dt.Columns.Add("ORDINAL_POSITION", Type.GetType("System.Int32"));
			dt.Columns.Add("PARAMETER_TYPE", Type.GetType("System.Int32"));
			dt.Columns.Add("PARAMETER_HASDEFAULT", Type.GetType("System.Boolean"));
			dt.Columns.Add("PARAMETER_DEFAULT", Type.GetType("System.String"));
			dt.Columns.Add("IS_NULLABLE", Type.GetType("System.Boolean"));
			dt.Columns.Add("DATA_TYPE", Type.GetType("System.Int32"));
			dt.Columns.Add("CHARACTER_MAXIMUM_LENGTH", Type.GetType("System.Int32"));
			dt.Columns.Add("CHARACTER_OCTET_LENGTH", Type.GetType("System.Int32"));
			dt.Columns.Add("NUMERIC_PRECISION", Type.GetType("System.Int32"));
			dt.Columns.Add("NUMERIC_SCALE", Type.GetType("System.Int16"));
			dt.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
			dt.Columns.Add("TYPE_NAME", Type.GetType("System.String"));
			dt.Columns.Add("LOCAL_TYPE_NAME", Type.GetType("System.String"));
			return dt;
		}

		override internal void LoadAll()
		{
			DataTable metaData = CreateDataTable();

			ADODB.Connection cnn = new ADODB.Connection();
			ADOX.Catalog cat = new ADOX.Catalog();
    
			// Open the Connection
			cnn.Open(dbRoot.ConnectionString, null, null, 0);
			cat.ActiveConnection = cnn;

			ADOX.Procedure proc = cat.Procedures[this.Procedure.Name];

			ADODB.Command cmd = proc.Command as ADODB.Command;
       
			// Retrieve Parameter information
			cmd.Parameters.Refresh();

			if(cmd.Parameters.Count > 0)
			{
				int ordinal = 0;

				foreach(ADODB.Parameter param in cmd.Parameters)
				{
					DataRow row = metaData.NewRow();

					string hyperlink = "False";

					try
					{
						hyperlink = param.Properties["Jet OLEDB:Hyperlink"].Value.ToString();
					} 
					catch {}

					row["TYPE_NAME"]                = hyperlink == "False" ? param.Type.ToString() : "Hyperlink";

					row["PROCEDURE_CATALOG"]		= this.Procedure.Database;
					row["PROCEDURE_SCHEMA"]			= null;
					row["PROCEDURE_NAME"]			= this.Procedure.Name;
					row["PARAMETER_NAME"]			= param.Name;
					row["ORDINAL_POSITION"]			= ordinal++;
					row["PARAMETER_TYPE"]			= param.Type;//.ToString();
					row["PARAMETER_HASDEFAULT"] 	= false;
					row["PARAMETER_DEFAULT"]    	= null;
					row["IS_NULLABLE"]				= false;
					row["DATA_TYPE"]				= param.Type;//.ToString();
					row["CHARACTER_MAXIMUM_LENGTH"]	= 0;
					row["CHARACTER_OCTET_LENGTH"]	= 0;
					row["NUMERIC_PRECISION"]		= param.Precision;
					row["NUMERIC_SCALE"]			= param.NumericScale;
					row["DESCRIPTION"]				= "";
				//	row["TYPE_NAME"]				= "";
					row["LOCAL_TYPE_NAME"]			= "";

					metaData.Rows.Add(row);
				}
			}

			cnn.Close();

			base.PopulateArray(metaData);
 		}
	}
}
