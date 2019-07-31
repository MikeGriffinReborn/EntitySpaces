using System;
using System.Xml;
using System.Xml.XPath;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class Single : MetaObject
	{
		public Single()
		{

		}

		#region Properties
		virtual public string Alias
		{
			get
			{
				return string.Empty;
			}

			set
			{

			}
		}

		virtual public string Name
		{
			get
			{
				return string.Empty;
			}
		}
		#endregion

		#region Property Helpers

		protected string GetString(DataColumn col)
		{
			if(null != col)
			{
				object o = _row[col];

				if(DBNull.Value != o)
				{
					string s = (string)o;
					if(dbRoot.StripTrailingNulls)
					{
						if(s.EndsWith(dbRoot.TrailingNull))
						{
							s = s.Remove(s.Length - 1, 1);
						}
					}
					return s;
				}
			}

			return string.Empty;
		}

		protected Guid GetGuid(DataColumn col)
		{
			if(null != col)
			{
				object o = _row[col];

				if(DBNull.Value != o)
					return (Guid)o;
			}

			return Guid.Empty;
		}

		protected DateTime GetDateTime(DataColumn col)
		{
			if(null != col)
			{
				object o = _row[col];

				if(DBNull.Value != o)
					return (DateTime)o;
			}

			return new DateTime(1,1,1,1,1,1,1);
		}

		protected System.Boolean GetBool(DataColumn col)
		{
			if(null != col)
			{
				object o = _row[col];

				if(DBNull.Value != o)
				{
					if(o is System.Boolean)
						return (Boolean)o;
					else
					{
						int i = Convert.ToInt32(o);
						return i == 0 ? false : true;
					}
				}
			}

			return false;
		}

		protected System.Int16 GetInt16(DataColumn col)
		{
			if(null != col)
			{
				object o = _row[col];

				if(DBNull.Value != o)
					return Convert.ToInt16(o);
			}

			return 0;
		}

		protected System.Int32 GetInt32(DataColumn col)
		{
			if(null != col)
			{
				object o = _row[col];

				if(DBNull.Value != o)
					return Convert.ToInt32(o);
			}

			return 0;
		}

		protected Decimal GetDecimal(DataColumn col)
		{
			if(null != col)
			{
				object o = _row[col];

				if(DBNull.Value != o)
					return Convert.ToDecimal(o);
			}

			return 0;
		}

		protected System.Byte[] GetByteArray(DataColumn col)
		{
			if(null != col)
			{
				object o = _row[col];

				if(DBNull.Value == o)
					return null;
				else
					return null;
			}

			return null;
		}

		#endregion

		internal DataRow Row
		{
			set
			{
				this._row = value;
			}
		}


		virtual public IPropertyCollection Properties 
		{ 
			get
			{
				if(null == _properties)
				{
					_properties = new PropertyCollection();
					_properties.Parent = this;
					_properties.LoadAll();
				}

				return _properties;
			}
		}

		virtual public object DatabaseSpecificMetaData(string key) 
		{
			return null;
		}

		protected PropertyCollection  _properties = null;
		internal  DataRow _row = null;
	}
}
