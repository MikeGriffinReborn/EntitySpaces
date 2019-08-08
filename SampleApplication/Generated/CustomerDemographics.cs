
/*
===============================================================================
                    EntitySpaces Studio by EntitySpaces, LLC
             Persistence Layer and Business Objects for Microsoft .NET
             EntitySpaces(TM) is a legal trademark of EntitySpaces, LLC
                          http://www.entityspaces.net
===============================================================================
EntitySpaces Version : 2019.1.0807.0
EntitySpaces Driver  : SQL
Date Generated       : 8/8/2019 8:05:38 AM
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
	/// Encapsulates the 'CustomerDemographics' table
	/// </summary>

    [DebuggerDisplay("Data = {Debug}")]
	[Serializable]
	[DataContract]
	[KnownType(typeof(CustomerDemographics))]	
	[XmlType("CustomerDemographics")]
	public partial class CustomerDemographics : esCustomerDemographics
	{	
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden | DebuggerBrowsableState.Never)]
		protected override esEntityDebuggerView[] Debug
		{
			get { return base.Debug; }
		}

		override public esEntity CreateInstance()
		{
			return new CustomerDemographics();
		}
		
		#region Static Quick Access Methods
		static public void Delete(System.String customerTypeID)
		{
			var obj = new CustomerDemographics();
			obj.CustomerTypeID = customerTypeID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save();
		}

	    static public void Delete(System.String customerTypeID, esSqlAccessType sqlAccessType)
		{
			var obj = new CustomerDemographics();
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
	[XmlType("CustomerDemographicsCollection")]
	public partial class CustomerDemographicsCollection : esCustomerDemographicsCollection, IEnumerable<CustomerDemographics>
	{
		public CustomerDemographics FindByPrimaryKey(System.String customerTypeID)
		{
			return this.SingleOrDefault(e => e.CustomerTypeID == customerTypeID);
		}

		
				
	}



    [DebuggerDisplay("Query = {Parse()}")]
	[Serializable]	
	public partial class CustomerDemographicsQuery : esCustomerDemographicsQuery
	{
		public CustomerDemographicsQuery(string joinAlias)
		{
			this.es.JoinAlias = joinAlias;
		}	

		override protected string GetQueryName()
		{
			return "CustomerDemographicsQuery";
		}
		
					
	
		#region Explicit Casts
		
		public static explicit operator string(CustomerDemographicsQuery query)
		{
			return CustomerDemographicsQuery.SerializeHelper.ToXml(query);
		}

		public static explicit operator CustomerDemographicsQuery(string query)
		{
			return (CustomerDemographicsQuery)CustomerDemographicsQuery.SerializeHelper.FromXml(query, typeof(CustomerDemographicsQuery));
		}
		
		#endregion		
	}

	[DataContract]
	[Serializable]
	abstract public partial class esCustomerDemographics : esEntity
	{
		public esCustomerDemographics()
		{

		}
		
		#region LoadByPrimaryKey
		public virtual bool LoadByPrimaryKey(System.String customerTypeID)
		{
			if(this.es.Connection.SqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(customerTypeID);
			else
				return LoadByPrimaryKeyStoredProcedure(customerTypeID);
		}

		public virtual bool LoadByPrimaryKey(esSqlAccessType sqlAccessType, System.String customerTypeID)
		{
			if (sqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(customerTypeID);
			else
				return LoadByPrimaryKeyStoredProcedure(customerTypeID);
		}

		private bool LoadByPrimaryKeyDynamic(System.String customerTypeID)
		{
			CustomerDemographicsQuery query = new CustomerDemographicsQuery();
			query.Where(query.CustomerTypeID == customerTypeID);
			return this.Load(query);
		}

		private bool LoadByPrimaryKeyStoredProcedure(System.String customerTypeID)
		{
			esParameters parms = new esParameters();
			parms.Add("CustomerTypeID", customerTypeID);
			return this.Load(esQueryType.StoredProcedure, this.es.spLoadByPrimaryKey, parms);
		}
		#endregion
		
		#region Properties
		
		
		
		/// <summary>
		/// Maps to CustomerDemographics.CustomerTypeID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String CustomerTypeID
		{
			get
			{
				return base.GetSystemString(CustomerDemographicsMetadata.ColumnNames.CustomerTypeID);
			}
			
			set
			{
				if(base.SetSystemString(CustomerDemographicsMetadata.ColumnNames.CustomerTypeID, value))
				{
					OnPropertyChanged(CustomerDemographicsMetadata.PropertyNames.CustomerTypeID);
				}
			}
		}
		
		/// <summary>
		/// Maps to CustomerDemographics.CustomerDesc
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String CustomerDesc
		{
			get
			{
				return base.GetSystemString(CustomerDemographicsMetadata.ColumnNames.CustomerDesc);
			}
			
			set
			{
				if(base.SetSystemString(CustomerDemographicsMetadata.ColumnNames.CustomerDesc, value))
				{
					OnPropertyChanged(CustomerDemographicsMetadata.PropertyNames.CustomerDesc);
				}
			}
		}
		
		#endregion
		
		#region Housekeeping methods

		override protected IMetadata Meta
		{
			get
			{
				return CustomerDemographicsMetadata.Meta();
			}
		}

		#endregion		
		
		#region Query Logic

		public CustomerDemographicsQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new CustomerDemographicsQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(CustomerDemographicsQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return this.Query.Load();
		}
		
		protected void InitQuery(CustomerDemographicsQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntity)this).Connection;
			}			
		}

		#endregion
		
        [IgnoreDataMember]
		private CustomerDemographicsQuery query;		
	}



	[Serializable]
	abstract public partial class esCustomerDemographicsCollection : esEntityCollection<CustomerDemographics>
	{
		#region Housekeeping methods
		override protected IMetadata Meta
		{
			get
			{
				return CustomerDemographicsMetadata.Meta();
			}
		}

		protected override string GetCollectionName()
		{
			return "CustomerDemographicsCollection";
		}

		#endregion		
		
		#region Query Logic

	#if (!WindowsCE)
		[BrowsableAttribute(false)]
	#endif
		public CustomerDemographicsQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new CustomerDemographicsQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(CustomerDemographicsQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return Query.Load();
		}

		override protected esDynamicQuery GetDynamicQuery()
		{
			if (this.query == null)
			{
				this.query = new CustomerDemographicsQuery();
				this.InitQuery(query);
			}
			return this.query;
		}

		protected void InitQuery(CustomerDemographicsQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntityCollection)this).Connection;
			}			
		}

		protected override void HookupQuery(esDynamicQuery query)
		{
			this.InitQuery((CustomerDemographicsQuery)query);
		}

		#endregion
		
		private CustomerDemographicsQuery query;
	}



	[Serializable]
	abstract public partial class esCustomerDemographicsQuery : esDynamicQuery
	{
		override protected IMetadata Meta
		{
			get
			{
				return CustomerDemographicsMetadata.Meta();
			}
		}	
		
		#region QueryItemFromName
		
        protected override esQueryItem QueryItemFromName(string name)
        {
            switch (name)
            {
				case "CustomerTypeID": return this.CustomerTypeID;
				case "CustomerDesc": return this.CustomerDesc;

                default: return null;
            }
        }		
		
		#endregion
		
		#region esQueryItems

		public esQueryItem CustomerTypeID
		{
			get { return new esQueryItem(this, CustomerDemographicsMetadata.ColumnNames.CustomerTypeID, esSystemType.String); }
		} 
		
		public esQueryItem CustomerDesc
		{
			get { return new esQueryItem(this, CustomerDemographicsMetadata.ColumnNames.CustomerDesc, esSystemType.String); }
		} 
		
		#endregion
		
	}


	
	public partial class CustomerDemographics : esCustomerDemographics
	{

			
		#region CustomersCollection - Many To Many (FK_CustomerCustomerDemo)
		
	    [EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeCustomersCollection()
		{
		    if(this._CustomersCollection != null && this._CustomersCollection.Count > 0)
				return true;
            else
				return false;
		}
		

		[DataMember(Name="CustomersCollection", EmitDefaultValue = false)]
		public CustomersCollection CustomersCollection
		{
			get
			{
				if(this._CustomersCollection == null)
				{
					this._CustomersCollection = new CustomersCollection();
					this._CustomersCollection.es.Connection.Name = this.es.Connection.Name;
					this.SetPostSave("CustomersCollection", this._CustomersCollection);
					if (!this.es.IsLazyLoadDisabled && this.CustomerTypeID != null)
					{
						CustomersQuery m = new CustomersQuery("m");
						CustomerCustomerDemoQuery j = new CustomerCustomerDemoQuery("j");
						m.Select(m);
						m.InnerJoin(j).On(m.CustomerID == j.CustomerID);
                        m.Where(j.CustomerTypeID == this.CustomerTypeID);

						this._CustomersCollection.Load(m);
					}
				}

				return this._CustomersCollection;
			}
			
			set 
			{ 
				if (value != null) throw new Exception("'value' Must be null"); 
			 
				if (this._CustomersCollection != null) 
				{ 
					this.RemovePostSave("CustomersCollection"); 
					this._CustomersCollection = null;
					
				} 
			}  			
		}

		/// <summary>
		/// Many to Many Associate
		/// Foreign Key Name - FK_CustomerCustomerDemo
		/// </summary>
		public void ASsociateCustomerCustomerDemoCollection(Customers entity)
		{
			if (this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection == null)
			{
				this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection = new CustomerCustomerDemoCollection();
				this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection.es.Connection.Name = this.es.Connection.Name;
				this.SetPostSave("ManyEntitySpacesMetadataEngineSqlSqlTableCollection", this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection);
			}

			CustomerCustomerDemo obj = this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection.AddNew();
			obj.CustomerTypeID = this.CustomerTypeID;
			obj.CustomerID = entity.CustomerID;
		}

		/// <summary>
		/// Many to Many Dissociate
		/// Foreign Key Name - FK_CustomerCustomerDemo
		/// </summary>
		public void DiSsociateCustomerCustomerDemoCollection(Customers entity)
		{
			if (this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection == null)
			{
				this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection = new CustomerCustomerDemoCollection();
				this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection.es.Connection.Name = this.es.Connection.Name;
				this.SetPostSave("ManyEntitySpacesMetadataEngineSqlSqlTableCollection", this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection);
			}

			CustomerCustomerDemo obj = this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection.AddNew();
			obj.CustomerTypeID = this.CustomerTypeID;
            obj.CustomerID = entity.CustomerID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
		}

		private CustomersCollection _CustomersCollection;
		private CustomerCustomerDemoCollection _ManyEntitySpacesMetadataEngineSqlSqlTableCollection;
		#endregion

		#region CustomerCustomerDemoCollection - Zero To Many (FK_CustomerCustomerDemo)
		
		static public esPrefetchMap Prefetch_CustomerCustomerDemoCollection
		{
			get
			{
				esPrefetchMap map = new esPrefetchMap
				{
					PrefetchDelegate = BusinessObjects.CustomerDemographics.CustomerCustomerDemoCollection_Delegate,
					PropertyName = "CustomerCustomerDemoCollection",
					MyColumnName = "CustomerTypeID",
					ParentColumnName = "CustomerTypeID",
					IsMultiPartKey = false
				};
				return map;
			}
		}		
		
		static private void CustomerCustomerDemoCollection_Delegate(esPrefetchParameters data)
		{
			CustomerDemographicsQuery parent = new CustomerDemographicsQuery(data.NextAlias());

			CustomerCustomerDemoQuery me = data.You != null ? data.You as CustomerCustomerDemoQuery : new CustomerCustomerDemoQuery(data.NextAlias());

			if (data.Root == null)
			{
				data.Root = me;
			}
			
			data.Root.InnerJoin(parent).On(parent.CustomerTypeID == me.CustomerTypeID);

			data.You = parent;
		}	
	
		/// <summary>
		/// Zero to Many
		/// Foreign Key Name - FK_CustomerCustomerDemo
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeCustomerCustomerDemoCollection()
		{
		    if(this._CustomerCustomerDemoCollection != null && this._CustomerCustomerDemoCollection.Count > 0)
				return true;
            else
				return false;
		}	
		

		[DataMember(Name="CustomerCustomerDemoCollection", EmitDefaultValue = false)]
		public CustomerCustomerDemoCollection CustomerCustomerDemoCollection
		{
			get
			{
				if(this._CustomerCustomerDemoCollection == null)
				{
					this._CustomerCustomerDemoCollection = new CustomerCustomerDemoCollection();
					this._CustomerCustomerDemoCollection.es.Connection.Name = this.es.Connection.Name;
					this.SetPostSave("CustomerCustomerDemoCollection", this._CustomerCustomerDemoCollection);
				
					if (this.CustomerTypeID != null)
					{
						if (!this.es.IsLazyLoadDisabled)
						{
							this._CustomerCustomerDemoCollection.Query.Where(this._CustomerCustomerDemoCollection.Query.CustomerTypeID == this.CustomerTypeID);
							this._CustomerCustomerDemoCollection.Query.Load();
						}

						// Auto-hookup Foreign Keys
						this._CustomerCustomerDemoCollection.fks.Add(CustomerCustomerDemoMetadata.ColumnNames.CustomerTypeID, this.CustomerTypeID);
					}
				}

				return this._CustomerCustomerDemoCollection;
			}
			
			set 
			{ 
				if (value != null) throw new Exception("'value' Must be null"); 
			 
				if (this._CustomerCustomerDemoCollection != null) 
				{ 
					this.RemovePostSave("CustomerCustomerDemoCollection"); 
					this._CustomerCustomerDemoCollection = null;
					
				} 
			} 			
		}
		

		
			
		
		private CustomerCustomerDemoCollection _CustomerCustomerDemoCollection;
		#endregion

		
		protected override esEntityCollectionBase CreateCollectionForPrefetch(string name)
		{
			esEntityCollectionBase coll = null;

			switch (name)
			{
				case "CustomerCustomerDemoCollection":
					coll = this.CustomerCustomerDemoCollection;
					break;	
			}

			return coll;
		}		
		/// <summary>
		/// Used internally by the entity's hierarchical properties.
		/// </summary>
		protected override List<esPropertyDescriptor> GetHierarchicalProperties()
		{
			List<esPropertyDescriptor> props = new List<esPropertyDescriptor>();
			
			props.Add(new esPropertyDescriptor(this, "CustomerCustomerDemoCollection", typeof(CustomerCustomerDemoCollection), new CustomerCustomerDemo()));
		
			return props;
		}
		
	}
	



	[Serializable]
	public partial class CustomerDemographicsMetadata : esMetadata, IMetadata
	{
		#region Protected Constructor
		protected CustomerDemographicsMetadata()
		{
			m_columns = new esColumnMetadataCollection();
			esColumnMetadata c;

			c = new esColumnMetadata(CustomerDemographicsMetadata.ColumnNames.CustomerTypeID, 0, typeof(System.String), esSystemType.String);
			c.PropertyName = CustomerDemographicsMetadata.PropertyNames.CustomerTypeID;
			c.IsInPrimaryKey = true;
			c.CharacterMaxLength = 10;
			m_columns.Add(c);
				
			c = new esColumnMetadata(CustomerDemographicsMetadata.ColumnNames.CustomerDesc, 1, typeof(System.String), esSystemType.String);
			c.PropertyName = CustomerDemographicsMetadata.PropertyNames.CustomerDesc;
			c.CharacterMaxLength = 1073741823;
			c.IsNullable = true;
			m_columns.Add(c);
				
		}
		#endregion	
	
		static public CustomerDemographicsMetadata Meta()
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
			 public const string CustomerTypeID = "CustomerTypeID";
			 public const string CustomerDesc = "CustomerDesc";
		}
		#endregion	
		
		#region PropertyNames
		public class PropertyNames
		{ 
			 public const string CustomerTypeID = "CustomerTypeID";
			 public const string CustomerDesc = "CustomerDesc";
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
			lock (typeof(CustomerDemographicsMetadata))
			{
				if(CustomerDemographicsMetadata.mapDelegates == null)
				{
					CustomerDemographicsMetadata.mapDelegates = new Dictionary<string,MapToMeta>();
				}
				
				if (CustomerDemographicsMetadata.meta == null)
				{
					CustomerDemographicsMetadata.meta = new CustomerDemographicsMetadata();
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


				meta.AddTypeMap("CustomerTypeID", new esTypeMap("nchar", "System.String"));
				meta.AddTypeMap("CustomerDesc", new esTypeMap("ntext", "System.String"));			
				
				
				
				meta.Source = "CustomerDemographics";
				meta.Destination = "CustomerDemographics";
				
				meta.spInsert = "proc_CustomerDemographicsInsert";				
				meta.spUpdate = "proc_CustomerDemographicsUpdate";		
				meta.spDelete = "proc_CustomerDemographicsDelete";
				meta.spLoadAll = "proc_CustomerDemographicsLoadAll";
				meta.spLoadByPrimaryKey = "proc_CustomerDemographicsLoadByPrimaryKey";
				
				this.m_providerMetadataMaps["esDefault"] = meta;
			}
			
			return this.m_providerMetadataMaps["esDefault"];
		}

		#endregion

		static private CustomerDemographicsMetadata meta;
		static protected Dictionary<string, MapToMeta> mapDelegates;
		static private int _esDefault = RegisterDelegateesDefault();
	}
}
