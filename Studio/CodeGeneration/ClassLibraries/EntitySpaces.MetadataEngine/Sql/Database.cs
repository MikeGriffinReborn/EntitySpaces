using System;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine.Sql
{
	public class SqlDatabase : Database
	{
		public SqlDatabase() {}

		internal static object DBSpecific(string key, Single single)
		{
			object retVal = null;
			DatabaseSpecific dext = new DatabaseSpecific();

			if (key == DatabaseSpecific.EXTENDED_PROPERTIES) 
			{
				if (single is IColumn) 
				{
					retVal = dext.ExtendedProperties(single as IColumn);
				}
				else if (single is ITable) 
				{
					retVal = dext.ExtendedProperties(single as ITable);
				}
				else if (single is IProcedure) 
				{
					retVal = dext.ExtendedProperties(single as IProcedure);
				}
				else if (single is IView) 
				{
					retVal = dext.ExtendedProperties(single as IView);
				}
			}
			
			return retVal;
		}
	}
}
