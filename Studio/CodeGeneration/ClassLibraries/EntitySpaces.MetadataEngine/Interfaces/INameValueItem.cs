using System;
using System.Collections;

namespace EntitySpaces.MetadataEngine
{
	/// <summary>
	/// This interface allows all the collections here to be bound to 
	/// Name/Value collection type objects. with ease
	/// </summary>
	public interface INameValueItem
	{
		string ItemName{ get; }
		string ItemValue{ get; }
	}
}

