
/*
===============================================================================
                    EntitySpaces Studio by EntitySpaces, LLC
             Persistence Layer and Business Objects for Microsoft .NET
             EntitySpaces(TM) is a legal trademark of EntitySpaces, LLC
                          http://www.entityspaces.net
===============================================================================
EntitySpaces Version : 2019.1.0807.0
EntitySpaces Driver  : SQL
Date Generated       : 8/8/2019 8:05:39 AM
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
	/// Encapsulates the 'Shippers' table
	/// </summary>

    [DebuggerDisplay("Data = {Debug}")]
	[Serializable]
	[DataContract]
	[KnownType(typeof(Shippers))]	
	[XmlType("Shippers")]
	public partial class Shippers : esShippers
	{	
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden | DebuggerBrowsableState.Never)]
		protected override esEntityDebuggerView[] Debug
		{
			get { return base.Debug; }
		}

		override public esEntity CreateInstance()
		{
			return new Shippers();
		}
		
		#region Static Quick Access Methods
		static public void Delete(System.Int32 shipperID)
		{
			var obj = new Shippers();
			obj.ShipperID = shipperID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save();
		}

	    static public void Delete(System.Int32 shipperID, esSqlAccessType sqlAccessType)
		{
			var obj = new Shippers();
			obj.ShipperID = shipperID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save(sqlAccessType);
		}
		#endregion

		
					
		
	
	}



    [DebuggerDisplay("Count = {Count}")]
	[Serializable]
	[CollectionDataContract]
	[XmlType("ShippersCollection")]
	public partial class ShippersCollection : esShippersCollection, IEnumerable<Shippers>
	{
		public Shippers FindByPrimaryKey(System.Int32 shipperID)
		{
			return this.SingleOrDefault(e => e.ShipperID == shipperID);
		}

		
				
	}



    [DebuggerDisplay("Query = {Parse()}")]
	[Serializable]	
	public partial class ShippersQuery : esShippersQuery
	{
		public ShippersQuery(string joinAlias)
		{
			this.es.JoinAlias = joinAlias;
		}	

		override protected string GetQueryName()
		{
			return "ShippersQuery";
		}
		
					
	
		#region Explicit Casts
		
		public static explicit operator string(ShippersQuery query)
		{
			return ShippersQuery.SerializeHelper.ToXml(query);
		}

		public static explicit operator ShippersQuery(string query)
		{
			return (ShippersQuery)ShippersQuery.SerializeHelper.FromXml(query, typeof(ShippersQuery));
		}
		
		#endregion		
	}

	[DataContract]
	[Serializable]
	abstract public partial class esShippers : esEntity
	{
		public esShippers()
		{

		}
		
		#region LoadByPrimaryKey
		public virtual bool LoadByPrimaryKey(System.Int32 shipperID)
		{
			if(this.es.Connection.SqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(shipperID);
			else
				return LoadByPrimaryKeyStoredProcedure(shipperID);
		}

		public virtual bool LoadByPrimaryKey(esSqlAccessType sqlAccessType, System.Int32 shipperID)
		{
			if (sqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(shipperID);
			else
				return LoadByPrimaryKeyStoredProcedure(shipperID);
		}

		private bool LoadByPrimaryKeyDynamic(System.Int32 shipperID)
		{
			ShippersQuery query = new ShippersQuery();
			query.Where(query.ShipperID == shipperID);
			return this.Load(query);
		}

		private bool LoadByPrimaryKeyStoredProcedure(System.Int32 shipperID)
		{
			esParameters parms = new esParameters();
			parms.Add("ShipperID", shipperID);
			return this.Load(esQueryType.StoredProcedure, this.es.spLoadByPrimaryKey, parms);
		}
		#endregion
		
		#region Properties
		
		
		
		/// <summary>
		/// Maps to Shippers.ShipperID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? ShipperID
		{
			get
			{
				return base.GetSystemInt32(ShippersMetadata.ColumnNames.ShipperID);
			}
			
			set
			{
				if(base.SetSystemInt32(ShippersMetadata.ColumnNames.ShipperID, value))
				{
					OnPropertyChanged(ShippersMetadata.PropertyNames.ShipperID);
				}
			}
		}
		
		/// <summary>
		/// Maps to Shippers.CompanyName
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String CompanyName
		{
			get
			{
				return base.GetSystemString(ShippersMetadata.ColumnNames.CompanyName);
			}
			
			set
			{
				if(base.SetSystemString(ShippersMetadata.ColumnNames.CompanyName, value))
				{
					OnPropertyChanged(ShippersMetadata.PropertyNames.CompanyName);
				}
			}
		}
		
		/// <summary>
		/// Maps to Shippers.Phone
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Phone
		{
			get
			{
				return base.GetSystemString(ShippersMetadata.ColumnNames.Phone);
			}
			
			set
			{
				if(base.SetSystemString(ShippersMetadata.ColumnNames.Phone, value))
				{
					OnPropertyChanged(ShippersMetadata.PropertyNames.Phone);
				}
			}
		}
		
		#endregion
		
		#region Housekeeping methods

		override protected IMetadata Meta
		{
			get
			{
				return ShippersMetadata.Meta();
			}
		}

		#endregion		
		
		#region Query Logic

		public ShippersQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new ShippersQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(ShippersQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return this.Query.Load();
		}
		
		protected void InitQuery(ShippersQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntity)this).Connection;
			}			
		}

		#endregion
		
        [IgnoreDataMember]
		private ShippersQuery query;		
	}



	[Serializable]
	abstract public partial class esShippersCollection : esEntityCollection<Shippers>
	{
		#region Housekeeping methods
		override protected IMetadata Meta
		{
			get
			{
				return ShippersMetadata.Meta();
			}
		}

		protected override string GetCollectionName()
		{
			return "ShippersCollection";
		}

		#endregion		
		
		#region Query Logic

	#if (!WindowsCE)
		[BrowsableAttribute(false)]
	#endif
		public ShippersQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new ShippersQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(ShippersQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return Query.Load();
		}

		override protected esDynamicQuery GetDynamicQuery()
		{
			if (this.query == null)
			{
				this.query = new ShippersQuery();
				this.InitQuery(query);
			}
			return this.query;
		}

		protected void InitQuery(ShippersQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntityCollection)this).Connection;
			}			
		}

		protected override void HookupQuery(esDynamicQuery query)
		{
			this.InitQuery((ShippersQuery)query);
		}

		#endregion
		
		private ShippersQuery query;
	}



	[Serializable]
	abstract public partial class esShippersQuery : esDynamicQuery
	{
		override protected IMetadata Meta
		{
			get
			{
				return ShippersMetadata.Meta();
			}
		}	
		
		#region QueryItemFromName
		
        protected override esQueryItem QueryItemFromName(string name)
        {
            switch (name)
            {
				case "ShipperID": return this.ShipperID;
				case "CompanyName": return this.CompanyName;
				case "Phone": return this.Phone;

                default: return null;
            }
        }		
		
		#endregion
		
		#region esQueryItems

		public esQueryItem ShipperID
		{
			get { return new esQueryItem(this, ShippersMetadata.ColumnNames.ShipperID, esSystemType.Int32); }
		} 
		
		public esQueryItem CompanyName
		{
			get { return new esQueryItem(this, ShippersMetadata.ColumnNames.CompanyName, esSystemType.String); }
		} 
		
		public esQueryItem Phone
		{
			get { return new esQueryItem(this, ShippersMetadata.ColumnNames.Phone, esSystemType.String); }
		} 
		
		#endregion
		
	}


	
	public partial class Shippers : esShippers
	{

		#region OrdersCollection - Zero To Many (FK_Orders_Shippers)
		
		static public esPrefetchMap Prefetch_OrdersCollection
		{
			get
			{
				esPrefetchMap map = new esPrefetchMap
				{
					PrefetchDelegate = BusinessObjects.Shippers.OrdersCollection_Delegate,
					PropertyName = "OrdersCollection",
					MyColumnName = "ShipVia",
					ParentColumnName = "ShipperID",
					IsMultiPartKey = false
				};
				return map;
			}
		}		
		
		static private void OrdersCollection_Delegate(esPrefetchParameters data)
		{
			ShippersQuery parent = new ShippersQuery(data.NextAlias());

			OrdersQuery me = data.You != null ? data.You as OrdersQuery : new OrdersQuery(data.NextAlias());

			if (data.Root == null)
			{
				data.Root = me;
			}
			
			data.Root.InnerJoin(parent).On(parent.ShipperID == me.ShipVia);

			data.You = parent;
		}	
	
		/// <summary>
		/// Zero to Many
		/// Foreign Key Name - FK_Orders_Shippers
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeOrdersCollection()
		{
		    if(this._OrdersCollection != null && this._OrdersCollection.Count > 0)
				return true;
            else
				return false;
		}	
		

		[DataMember(Name="OrdersCollection", EmitDefaultValue = false)]
		public OrdersCollection OrdersCollection
		{
			get
			{
				if(this._OrdersCollection == null)
				{
					this._OrdersCollection = new OrdersCollection();
					this._OrdersCollection.es.Connection.Name = this.es.Connection.Name;
					this.SetPostSave("OrdersCollection", this._OrdersCollection);
				
					if (this.ShipperID != null)
					{
						if (!this.es.IsLazyLoadDisabled)
						{
							this._OrdersCollection.Query.Where(this._OrdersCollection.Query.ShipVia == this.ShipperID);
							this._OrdersCollection.Query.Load();
						}

						// Auto-hookup Foreign Keys
						this._OrdersCollection.fks.Add(OrdersMetadata.ColumnNames.ShipVia, this.ShipperID);
					}
				}

				return this._OrdersCollection;
			}
			
			set 
			{ 
				if (value != null) throw new Exception("'value' Must be null"); 
			 
				if (this._OrdersCollection != null) 
				{ 
					this.RemovePostSave("OrdersCollection"); 
					this._OrdersCollection = null;
					
				} 
			} 			
		}
		

		
			
		
		private OrdersCollection _OrdersCollection;
		#endregion

		
		protected override esEntityCollectionBase CreateCollectionForPrefetch(string name)
		{
			esEntityCollectionBase coll = null;

			switch (name)
			{
				case "OrdersCollection":
					coll = this.OrdersCollection;
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
			
			props.Add(new esPropertyDescriptor(this, "OrdersCollection", typeof(OrdersCollection), new Orders()));
		
			return props;
		}
		
		/// <summary>
		/// Called by ApplyPostSaveKeys 
		/// </summary>
		/// <param name="coll">The collection to enumerate over</param>
		/// <param name="key">"The column name</param>
		/// <param name="value">The column value</param>
		private void Apply(esEntityCollectionBase coll, string key, object value)
		{
			foreach (esEntity obj in coll)
			{
				if (obj.es.IsAdded)
				{
					obj.SetProperty(key, value);
				}
			}
		}
		
		/// <summary>
		/// Used internally for retrieving AutoIncrementing keys
		/// during hierarchical PostSave.
		/// </summary>
		protected override void ApplyPostSaveKeys()
		{
			if(this._OrdersCollection != null)
			{
				Apply(this._OrdersCollection, "ShipVia", this.ShipperID);
			}
		}
		
	}
	



	[Serializable]
	public partial class ShippersMetadata : esMetadata, IMetadata
	{
		#region Protected Constructor
		protected ShippersMetadata()
		{
			m_columns = new esColumnMetadataCollection();
			esColumnMetadata c;

			c = new esColumnMetadata(ShippersMetadata.ColumnNames.ShipperID, 0, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = ShippersMetadata.PropertyNames.ShipperID;
			c.IsInPrimaryKey = true;
			c.IsAutoIncrement = true;
			c.NumericPrecision = 10;
			m_columns.Add(c);
				
			c = new esColumnMetadata(ShippersMetadata.ColumnNames.CompanyName, 1, typeof(System.String), esSystemType.String);
			c.PropertyName = ShippersMetadata.PropertyNames.CompanyName;
			c.CharacterMaxLength = 40;
			m_columns.Add(c);
				
			c = new esColumnMetadata(ShippersMetadata.ColumnNames.Phone, 2, typeof(System.String), esSystemType.String);
			c.PropertyName = ShippersMetadata.PropertyNames.Phone;
			c.CharacterMaxLength = 24;
			c.IsNullable = true;
			m_columns.Add(c);
				
		}
		#endregion	
	
		static public ShippersMetadata Meta()
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
			 public const string ShipperID = "ShipperID";
			 public const string CompanyName = "CompanyName";
			 public const string Phone = "Phone";
		}
		#endregion	
		
		#region PropertyNames
		public class PropertyNames
		{ 
			 public const string ShipperID = "ShipperID";
			 public const string CompanyName = "CompanyName";
			 public const string Phone = "Phone";
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
			lock (typeof(ShippersMetadata))
			{
				if(ShippersMetadata.mapDelegates == null)
				{
					ShippersMetadata.mapDelegates = new Dictionary<string,MapToMeta>();
				}
				
				if (ShippersMetadata.meta == null)
				{
					ShippersMetadata.meta = new ShippersMetadata();
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


				meta.AddTypeMap("ShipperID", new esTypeMap("int", "System.Int32"));
				meta.AddTypeMap("CompanyName", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("Phone", new esTypeMap("nvarchar", "System.String"));			
				
				
				
				meta.Source = "Shippers";
				meta.Destination = "Shippers";
				
				meta.spInsert = "proc_ShippersInsert";				
				meta.spUpdate = "proc_ShippersUpdate";		
				meta.spDelete = "proc_ShippersDelete";
				meta.spLoadAll = "proc_ShippersLoadAll";
				meta.spLoadByPrimaryKey = "proc_ShippersLoadByPrimaryKey";
				
				this.m_providerMetadataMaps["esDefault"] = meta;
			}
			
			return this.m_providerMetadataMaps["esDefault"];
		}

		#endregion

		static private ShippersMetadata meta;
		static protected Dictionary<string, MapToMeta> mapDelegates;
		static private int _esDefault = RegisterDelegateesDefault();
	}
}
