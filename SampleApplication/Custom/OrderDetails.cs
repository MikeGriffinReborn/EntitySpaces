/*
===============================================================================
                    EntitySpaces Studio by EntitySpaces, LLC
             Persistence Layer and Business Objects for Microsoft .NET
             EntitySpaces(TM) is a legal trademark of EntitySpaces, LLC
                          http://www.entityspaces.net
===============================================================================
EntitySpaces Version : 2019.1.0702.0
EntitySpaces Driver  : SQL
Date Generated       : 7/3/2019 1:59:27 PM
===============================================================================
*/

using System;

using EntitySpaces.Core;
using EntitySpaces.Interfaces;
using EntitySpaces.DynamicQuery;

namespace BusinessObjects
{
	public partial class OrderDetails : esOrderDetails
	{
		public OrderDetails()
		{
		
		}

        public override void SetProperty(string name, object value)
        {
            base.SetProperty(name, value);
            int i = 0;
        }


    }
}
