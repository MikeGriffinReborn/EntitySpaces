
/*
===============================================================================
                    EntitySpaces Studio by EntitySpaces, LLC
             Persistence Layer and Business Objects for Microsoft .NET
             EntitySpaces(TM) is a legal trademark of EntitySpaces, LLC
                          http://www.entityspaces.net
===============================================================================
EntitySpaces Version : 2019.1.0805.0
EntitySpaces Driver  : SQL
Date Generated       : 8/6/2019 9:55:46 AM
===============================================================================
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using EntitySpaces.Core;
using EntitySpaces.Interfaces;
using EntitySpaces.DynamicQuery;



namespace BusinessObjects
{
	/// <summary>
	/// Encapsulates the 'CustomerCustomerDemo' table
	/// </summary>

    [DebuggerDisplay("Data = {Debug}")]
	[Serializable]
	[DataContract]
	[KnownType(typeof(CustomerCustomerDemo))]	
	[XmlType("CustomerCustomerDemo")]
	public partial class CustomerCustomerDemo : esCustomerCustomerDemo
	{	
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden | DebuggerBrowsableState.Never)]
		protected override esEntityDebuggerView[] Debug
		{
			get { return base.Debug; }
		}

		override public esEntity CreateInstance()
		{
			return new CustomerCustomerDemo();
		}
		
		#region Static Quick Access Methods
		static public void Delete(System.String customerID, System.String customerTypeID)
		{
			var obj = new CustomerCustomerDemo();
			obj.CustomerID = customerID;
			obj.CustomerTypeID = customerTypeID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save();
		}

	    static public void Delete(System.String customerID, System.String customerTypeID, esSqlAccessType sqlAccessType)
		{
			var obj = new CustomerCustomerDemo();
			obj.CustomerID = customerID;
			obj.CustomerTypeID = customerTypeID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save(sqlAccessType);
		}
		#endregion

		
					
		
	
	}



    [DebuggerDisplay("Count = {Count}")]
	[Serializable]
	[CollectionDataContract]
	[XmlType("CustomerCustomerDemoCollection")]
	public partial class CustomerCustomerDemoCollection : esCustomerCustomerDemoCollection, IEnumerable<CustomerCustomerDemo>
	{
		public CustomerCustomerDemo FindByPrimaryKey(System.String customerID, System.String customerTypeID)
		{
			return this.SingleOrDefault(e => e.CustomerID == customerID && e.CustomerTypeID == customerTypeID);
		}

		
				
	}



    [DebuggerDisplay("Query = {Parse()}")]
	[Serializable]	
	public partial class CustomerCustomerDemoQuery : esCustomerCustomerDemoQuery
	{
		public CustomerCustomerDemoQuery(string joinAlias)
		{
			this.es.JoinAlias = joinAlias;
		}	

		override protected string GetQueryName()
		{
			return "CustomerCustomerDemoQuery";
		}
		
					
	
		#region Explicit Casts
		
		public static explicit operator string(CustomerCustomerDemoQuery query)
		{
			return CustomerCustomerDemoQuery.SerializeHelper.ToXml(query);
		}

		public static explicit operator CustomerCustomerDemoQuery(string query)
		{
			return (CustomerCustomerDemoQuery)CustomerCustomerDemoQuery.SerializeHelper.FromXml(query, typeof(CustomerCustomerDemoQuery));
		}
		
		#endregion		
	}

	[DataContract]
	[Serializable]
	abstract public partial class esCustomerCustomerDemo : esEntity
	{
		public esCustomerCustomerDemo()
		{

		}
		
		#region LoadByPrimaryKey
		public virtual bool LoadByPrimaryKey(System.String customerID, System.String customerTypeID)
		{
			if(this.es.Connection.SqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(customerID, customerTypeID);
			else
				return LoadByPrimaryKeyStoredProcedure(customerID, customerTypeID);
		}

		public virtual bool LoadByPrimaryKey(esSqlAccessType sqlAccessType, System.String customerID, System.String customerTypeID)
		{
			if (sqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(customerID, customerTypeID);
			else
				return LoadByPrimaryKeyStoredProcedure(customerID, customerTypeID);
		}

		private bool LoadByPrimaryKeyDynamic(System.String customerID, System.String customerTypeID)
		{
			CustomerCustomerDemoQuery query = new CustomerCustomerDemoQuery();
			query.Where(query.CustomerID == customerID, query.CustomerTypeID == customerTypeID);
			return this.Load(query);
		}

		private bool LoadByPrimaryKeyStoredProcedure(System.String customerID, System.String customerTypeID)
		{
			esParameters parms = new esParameters();
			parms.Add("CustomerID", customerID);			parms.Add("CustomerTypeID", customerTypeID);
			return this.Load(esQueryType.StoredProcedure, this.es.spLoadByPrimaryKey, parms);
		}
		#endregion
		
		#region Properties
		
		
		
		/// <summary>
		/// Maps to CustomerCustomerDemo.CustomerID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String CustomerID
		{
			get
			{
				return base.GetSystemString(CustomerCustomerDemoMetadata.ColumnNames.CustomerID);
			}
			
			set
			{
				if(base.SetSystemString(CustomerCustomerDemoMetadata.ColumnNames.CustomerID, value))
				{
					this._Customers = null;
					this.OnPropertyChanged("Customers");
					OnPropertyChanged(CustomerCustomerDemoMetadata.PropertyNames.CustomerID);
				}
			}
		}		
		
		/// <summary>
		/// Maps to CustomerCustomerDemo.CustomerTypeID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String CustomerTypeID
		{
			get
			{
				return base.GetSystemString(CustomerCustomerDemoMetadata.ColumnNames.CustomerTypeID);
			}
			
			set
			{
				if(base.SetSystemString(CustomerCustomerDemoMetadata.ColumnNames.CustomerTypeID, value))
				{
					this._CustomerDemographics = null;
					this.OnPropertyChanged("CustomerDemographics");
					OnPropertyChanged(CustomerCustomerDemoMetadata.PropertyNames.CustomerTypeID);
				}
			}
		}		
		
		[CLSCompliant(false)]
		internal protected CustomerDemographics _CustomerDemographics;
		[CLSCompliant(false)]
		internal protected Customers _Customers;
		#endregion
		
		#region Housekeeping methods

		override protected IMetadata Meta
		{
			get
			{
				return CustomerCustomerDemoMetadata.Meta();
			}
		}

		#endregion		
		
		#region Query Logic

		public CustomerCustomerDemoQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new CustomerCustomerDemoQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(CustomerCustomerDemoQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return this.Query.Load();
		}
		
		protected void InitQuery(CustomerCustomerDemoQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntity)this).Connection;
			}			
		}

		#endregion
		
        [IgnoreDataMember]
		private CustomerCustomerDemoQuery query;		
	}



	[Serializable]
	abstract public partial class esCustomerCustomerDemoCollection : esEntityCollection<CustomerCustomerDemo>
	{
		#region Housekeeping methods
		override protected IMetadata Meta
		{
			get
			{
				return CustomerCustomerDemoMetadata.Meta();
			}
		}

		protected override string GetCollectionName()
		{
			return "CustomerCustomerDemoCollection";
		}

		#endregion		
		
		#region Query Logic

	#if (!WindowsCE)
		[BrowsableAttribute(false)]
	#endif
		public CustomerCustomerDemoQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new CustomerCustomerDemoQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(CustomerCustomerDemoQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return Query.Load();
		}

		override protected esDynamicQuery GetDynamicQuery()
		{
			if (this.query == null)
			{
				this.query = new CustomerCustomerDemoQuery();
				this.InitQuery(query);
			}
			return this.query;
		}

		protected void InitQuery(CustomerCustomerDemoQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntityCollection)this).Connection;
			}			
		}

		protected override void HookupQuery(esDynamicQuery query)
		{
			this.InitQuery((CustomerCustomerDemoQuery)query);
		}

		#endregion
		
		private CustomerCustomerDemoQuery query;
	}



	[Serializable]
	abstract public partial class esCustomerCustomerDemoQuery : esDynamicQuery
	{
		override protected IMetadata Meta
		{
			get
			{
				return CustomerCustomerDemoMetadata.Meta();
			}
		}	
		
		#region QueryItemFromName
		
        protected override esQueryItem QueryItemFromName(string name)
        {
            switch (name)
            {
				case "CustomerID": return this.CustomerID;
				case "CustomerTypeID": return this.CustomerTypeID;

                default: return null;
            }
        }		
		
		#endregion
		
		#region esQueryItems

		public esQueryItem CustomerID
		{
			get { return new esQueryItem(this, CustomerCustomerDemoMetadata.ColumnNames.CustomerID, esSystemType.String); }
		} 
		
		public esQueryItem CustomerTypeID
		{
			get { return new esQueryItem(this, CustomerCustomerDemoMetadata.ColumnNames.CustomerTypeID, esSystemType.String); }
		} 
		
		#endregion
		
	}


	
	public partial class CustomerCustomerDemo : esCustomerCustomerDemo
	{

		
		#region CustomerDemographics - Many To One (FK_CustomerCustomerDemo)
		
	    [EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeCustomerDemographics()
		{
		    return this._CustomerDemographics != null ? true : false;
		}
		

		[DataMember(Name="CustomerDemographics", EmitDefaultValue = false)]
					
		public CustomerDemographics CustomerDemographics
		{
			get
			{
                if (this._CustomerDemographics == null)
                {
                    this._CustomerDemographics = new CustomerDemographics();
                    this._CustomerDemographics.es.Connection.Name = this.es.Connection.Name;
                    this.SetPreSave("CustomerDemographics", this._CustomerDemographics);

					if(this._CustomerDemographics == null && CustomerTypeID != null)
                    {
                        if (!this.es.IsLazyLoadDisabled)
                        {
							this._CustomerDemographics.Query.Where(this._CustomerDemographics.Query.CustomerTypeID == this.CustomerTypeID);
							this._CustomerDemographics.Query.Load();
                        }
                    }
                }

				return this._CustomerDemographics;
			}
			
			set
			{
				this.RemovePreSave("CustomerDemographics");
				

				if(value == null)
				{
					this.CustomerTypeID = null;
					this._CustomerDemographics = null;
				}
				else
				{
					this.CustomerTypeID = value.CustomerTypeID;
					this._CustomerDemographics = value;
					this.SetPreSave("CustomerDemographics", this._CustomerDemographics);
				}
				
			}
		}
		#endregion
		

		
		#region Customers - Many To One (FK_CustomerCustomerDemo_Customers)
		
	    [EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeCustomers()
		{
		    return this._Customers != null ? true : false;
		}
		

		[DataMember(Name="Customers", EmitDefaultValue = false)]
					
		public Customers Customers
		{
			get
			{
                if (this._Customers == null)
                {
                    this._Customers = new Customers();
                    this._Customers.es.Connection.Name = this.es.Connection.Name;
                    this.SetPreSave("Customers", this._Customers);

					if(this._Customers == null && CustomerID != null)
                    {
                        if (!this.es.IsLazyLoadDisabled)
                        {
							this._Customers.Query.Where(this._Customers.Query.CustomerID == this.CustomerID);
							this._Customers.Query.Load();
                        }
                    }
                }

				return this._Customers;
			}
			
			set
			{
				this.RemovePreSave("Customers");
				

				if(value == null)
				{
					this.CustomerID = null;
					this._Customers = null;
				}
				else
				{
					this.CustomerID = value.CustomerID;
					this._Customers = value;
					this.SetPreSave("Customers", this._Customers);
				}
				
			}
		}
		#endregion
		

		
		
	}
	



	[Serializable]
	public partial class CustomerCustomerDemoMetadata : esMetadata, IMetadata
	{
		#region Protected Constructor
		protected CustomerCustomerDemoMetadata()
		{
			m_columns = new esColumnMetadataCollection();
			esColumnMetadata c;

			c = new esColumnMetadata(CustomerCustomerDemoMetadata.ColumnNames.CustomerID, 0, typeof(System.String), esSystemType.String);
			c.PropertyName = CustomerCustomerDemoMetadata.PropertyNames.CustomerID;
			c.IsInPrimaryKey = true;
			c.CharacterMaxLength = 5;
			m_columns.Add(c);
				
			c = new esColumnMetadata(CustomerCustomerDemoMetadata.ColumnNames.CustomerTypeID, 1, typeof(System.String), esSystemType.String);
			c.PropertyName = CustomerCustomerDemoMetadata.PropertyNames.CustomerTypeID;
			c.IsInPrimaryKey = true;
			c.CharacterMaxLength = 10;
			m_columns.Add(c);
				
		}
		#endregion	
	
		static public CustomerCustomerDemoMetadata Meta()
		{
			return meta;
		}	
		
		public Guid DataID
		{
			get { return base.m_dataID; }
		}	
		
		public bool MultiProviderMode
		{
			get { return false; }
		}		

		public esColumnMetadataCollection Columns
		{
			get	{ return base.m_columns; }
		}
		
		#region ColumnNames
		public class ColumnNames
		{ 
			 public const string CustomerID = "CustomerID";
			 public const string CustomerTypeID = "CustomerTypeID";
		}
		#endregion	
		
		#region PropertyNames
		public class PropertyNames
		{ 
			 public const string CustomerID = "CustomerID";
			 public const string CustomerTypeID = "CustomerTypeID";
		}
		#endregion	

		public esProviderSpecificMetadata GetProviderMetadata(string mapName)
		{
			MapToMeta mapMethod = mapDelegates[mapName];

			if (mapMethod != null)
				return mapMethod(mapName);
			else
				return null;
		}
		
		#region MAP esDefault
		
		static private int RegisterDelegateesDefault()
		{
			// This is only executed once per the life of the application
			lock (typeof(CustomerCustomerDemoMetadata))
			{
				if(CustomerCustomerDemoMetadata.mapDelegates == null)
				{
					CustomerCustomerDemoMetadata.mapDelegates = new Dictionary<string,MapToMeta>();
				}
				
				if (CustomerCustomerDemoMetadata.meta == null)
				{
					CustomerCustomerDemoMetadata.meta = new CustomerCustomerDemoMetadata();
				}
				
				MapToMeta mapMethod = new MapToMeta(meta.esDefault);
				mapDelegates.Add("esDefault", mapMethod);
				mapMethod("esDefault");
			}
			return 0;
		}			

		private esProviderSpecificMetadata esDefault(string mapName)
		{
			if(!m_providerMetadataMaps.ContainsKey(mapName))
			{
				esProviderSpecificMetadata meta = new esProviderSpecificMetadata();			


				meta.AddTypeMap("CustomerID", new esTypeMap("nchar", "System.String"));
				meta.AddTypeMap("CustomerTypeID", new esTypeMap("nchar", "System.String"));			
				
				
				
				meta.Source = "CustomerCustomerDemo";
				meta.Destination = "CustomerCustomerDemo";
				
				meta.spInsert = "proc_CustomerCustomerDemoInsert";				
				meta.spUpdate = "proc_CustomerCustomerDemoUpdate";		
				meta.spDelete = "proc_CustomerCustomerDemoDelete";
				meta.spLoadAll = "proc_CustomerCustomerDemoLoadAll";
				meta.spLoadByPrimaryKey = "proc_CustomerCustomerDemoLoadByPrimaryKey";
				
				this.m_providerMetadataMaps["esDefault"] = meta;
			}
			
			return this.m_providerMetadataMaps["esDefault"];
		}

		#endregion

		static private CustomerCustomerDemoMetadata meta;
		static protected Dictionary<string, MapToMeta> mapDelegates;
		static private int _esDefault = RegisterDelegateesDefault();
	}
}
