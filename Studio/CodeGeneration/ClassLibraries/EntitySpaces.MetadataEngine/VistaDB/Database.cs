using System;
using System.Data;
using System.Reflection;

namespace esMetadataEngine.VistaDB
{
	public class VistaDBDatabase : Database
	{
		public VistaDBDatabase()
		{

		}

		override public string Alias
		{
			get
			{
				return _name;
			}
		}

		override public string Name
		{
			get
			{
				return _name;
			}
		}

		override public string Description
		{
			get
			{
				return _desc;
			}
		}

		internal string _name = "";
		internal string _desc = "";
		internal object _metaHelper = null;
		internal MetaHelper _mh = new MetaHelper();

		internal bool _FKsInLoad = false;
	}
}
