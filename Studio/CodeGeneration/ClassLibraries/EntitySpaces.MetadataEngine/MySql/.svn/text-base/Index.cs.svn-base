using System;
using System.Data;

namespace EntitySpaces.MetadataEngine.MySql
{
	public class MySqlIndex : Index
	{
		public MySqlIndex()
		{

		}

		public override string Type
		{
			get
			{
				return this.GetString(Indexes.f_Type);
			}
		}

		public override Boolean Unique
		{
			get
			{
				// We have to reverse the meaning
				return (base.Unique) ? false : true;
			}
		}

		public override string Collation
		{
			get
			{
				string s = this.GetString(Indexes.f_Collation);

				switch(s)
				{
					case "A":
						return "ASCENDING";
					case "D":
						return "DECENDING";
					default:
						return "UNKNOWN";
				}
			}
		}
	}
}
