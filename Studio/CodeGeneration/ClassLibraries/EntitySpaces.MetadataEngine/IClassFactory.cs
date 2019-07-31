using System;
using System.EnterpriseServices;

namespace dbMeta
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(false)]
#endif
	public interface IClassFactory 
	{
		Database		CreateDatabase();
		Databases		CreateDatabases();
		Tables			CreateTables();
		Table			CreateTable();
		Column			CreateColumn();
		Columns			CreateColumns();
		Procedure		CreateProcedure();
		Procedures		CreateProcedures();
		View			CreateView();
		Views			CreateViews();
		Parameter   	CreateParameter();
		Parameters  	CreateParameters();
		ForeignKey  	CreateForeignKey();
		ForeignKeys 	CreateForeignKeys();
		Index       	CreateIndex();
		Indexes     	CreateIndexes();
		ResultColumn	CreateResultColumn();
		ResultColumns	CreateResultColumns();
	}
}
