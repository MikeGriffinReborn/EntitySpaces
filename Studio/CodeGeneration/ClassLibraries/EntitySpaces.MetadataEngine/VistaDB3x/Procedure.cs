using System;
using System.Data;

namespace MyMeta.VistaDB3x
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedure))]
#endif 
	public class VistaDBProcedure : Procedure
	{
		public VistaDBProcedure()
		{

		}
	}
}
