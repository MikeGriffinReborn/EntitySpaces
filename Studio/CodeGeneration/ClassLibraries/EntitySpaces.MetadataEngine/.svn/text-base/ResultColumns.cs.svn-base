using System;
using System.Xml;
using System.Collections;

namespace EntitySpaces.MetadataEngine
{
	public class ResultColumns : Collection, IResultColumns, IEnumerable, ICollection
	{
		public ResultColumns()
		{

		}

		#region XML User Data

		override public string UserDataXPath
		{ 
			get
			{
				return Procedure.UserDataXPath + @"/ResultColumns";
			} 
		}

		override internal bool GetXmlNode(out XmlNode node, bool forceCreate)
		{
			node = null;
			bool success = false;

			if(null == _xmlNode)
			{
				// Get the parent node
				XmlNode parentNode = null;
				if(this.Procedure.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./ResultColumns";
					if(!GetUserData(xPath, parentNode, out _xmlNode) && forceCreate)
					{
						// Create it, and try again
						this.CreateUserMetaData(parentNode);
						GetUserData(xPath, parentNode, out _xmlNode);
					}
				}
			}

			if(null != _xmlNode)
			{
				node = _xmlNode;
				success = true;
			}

			return success;
		}

		override public void CreateUserMetaData(XmlNode parentNode)
		{
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "ResultColumns", null);
			parentNode.AppendChild(myNode);
		}

		#endregion

		virtual internal void LoadAll()
		{

		}

		public IResultColumn this[object index] 
		{ 
			get
            {
                if (index is String)
                {
                    return GetByPhysicalName(index as String) as IResultColumn;
                }
                else
                {
                    int idx = Convert.ToInt32(index);
                    return this._array[idx] as IResultColumn;
                }
			}
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return new Enumerator(this._array);
		}

		#endregion

		#region IList Members

		object System.Collections.IList.this[int index]
		{
			get	{ return this[index];}
			set	{ }
		}

		#endregion

        internal ResultColumn GetByPhysicalName(string name)
        {
            ResultColumn obj = null;
            ResultColumn tmp = null;

            int count = this._array.Count;
            for (int i = 0; i < count; i++)
            {
                tmp = this._array[i] as ResultColumn;

                if (this.CompareStrings(name, tmp.Name))
                {
                    obj = tmp;
                    break;
                }
            }

            return obj;
        }

		internal Procedure Procedure = null;
	}
}
