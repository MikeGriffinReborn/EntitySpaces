using System;
using System.Data;

namespace EntitySpaces.MetadataEngine.PostgreSQL
{
	public class PostgreSQLDomain : Domain
	{
		public PostgreSQLDomain()
		{

		}

		public override string DataTypeNameComplete
		{
			get
			{
				PostgreSQLDomains domains = this.Domains as PostgreSQLDomains;
				return this.GetString(domains.f_TypeNameComplete);
			}
		}

	}
}
