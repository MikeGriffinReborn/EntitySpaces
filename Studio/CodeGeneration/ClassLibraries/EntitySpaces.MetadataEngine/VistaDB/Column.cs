using System;
using System.Data;

namespace esMetadataEngine.VistaDB
{
	public class VistaDBColumn : Column
	{
		public VistaDBColumn()
		{

		}

		override internal Column Clone()
		{
			Column c = base.Clone();

			return c;
		}

		override public string DataTypeName
		{
			get
			{
				VistaDBColumns cols = Columns as VistaDBColumns;
				return this.GetString(cols.f_TypeName);
			}
		}

		public override Boolean IsInPrimaryKey
		{
			get
			{
				VistaDBColumns cols = Columns as VistaDBColumns;
				return this.GetBool(cols.f_InPrimaryKey);
			}
		}


		override public string DataTypeNameComplete
		{
			get
			{
				return "Unknown";
//				VistaDBColumns cols = Columns as VistaDBColumns;
//				return this.GetString(cols.f_TypeName);
			}
		}
	}
}
