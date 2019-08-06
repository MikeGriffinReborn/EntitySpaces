
/*
===============================================================================
                    EntitySpaces Studio by EntitySpaces, LLC
             Persistence Layer and Business Objects for Microsoft .NET
             EntitySpaces(TM) is a legal trademark of EntitySpaces, LLC
                          http://www.entityspaces.net
===============================================================================
EntitySpaces Version : 2019.1.0805.0
EntitySpaces Driver  : SQL
Date Generated       : 8/6/2019 9:55:49 AM
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
	/// Encapsulates the 'Order Details' table
	/// </summary>

    [DebuggerDisplay("Data = {Debug}")]
	[Serializable]
	[DataContract]
	[KnownType(typeof(OrderDetails))]	
	[XmlType("OrderDetails")]
	public partial class OrderDetails : esOrderDetails
	{	
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden | DebuggerBrowsableState.Never)]
		protected override esEntityDebuggerView[] Debug
		{
			get { return base.Debug; }
		}

		override public esEntity CreateInstance()
		{
			return new OrderDetails();
		}
		
		#region Static Quick Access Methods
		static public void Delete(System.Int32 orderID, System.Int32 productID)
		{
			var obj = new OrderDetails();
			obj.OrderID = orderID;
			obj.ProductID = productID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save();
		}

	    static public void Delete(System.Int32 orderID, System.Int32 productID, esSqlAccessType sqlAccessType)
		{
			var obj = new OrderDetails();
			obj.OrderID = orderID;
			obj.ProductID = productID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save(sqlAccessType);
		}
		#endregion

		
					
		
	
	}



    [DebuggerDisplay("Count = {Count}")]
	[Serializable]
	[CollectionDataContract]
	[XmlType("OrderDetailsCollection")]
	public partial class OrderDetailsCollection : esOrderDetailsCollection, IEnumerable<OrderDetails>
	{
		public OrderDetails FindByPrimaryKey(System.Int32 orderID, System.Int32 productID)
		{
			return this.SingleOrDefault(e => e.OrderID == orderID && e.ProductID == productID);
		}

		
				
	}



    [DebuggerDisplay("Query = {Parse()}")]
	[Serializable]	
	public partial class OrderDetailsQuery : esOrderDetailsQuery
	{
		public OrderDetailsQuery(string joinAlias)
		{
			this.es.JoinAlias = joinAlias;
		}	

		override protected string GetQueryName()
		{
			return "OrderDetailsQuery";
		}
		
					
	
		#region Explicit Casts
		
		public static explicit operator string(OrderDetailsQuery query)
		{
			return OrderDetailsQuery.SerializeHelper.ToXml(query);
		}

		public static explicit operator OrderDetailsQuery(string query)
		{
			return (OrderDetailsQuery)OrderDetailsQuery.SerializeHelper.FromXml(query, typeof(OrderDetailsQuery));
		}
		
		#endregion		
	}

	[DataContract]
	[Serializable]
	abstract public partial class esOrderDetails : esEntity
	{
		public esOrderDetails()
		{

		}
		
		#region LoadByPrimaryKey
		public virtual bool LoadByPrimaryKey(System.Int32 orderID, System.Int32 productID)
		{
			if(this.es.Connection.SqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(orderID, productID);
			else
				return LoadByPrimaryKeyStoredProcedure(orderID, productID);
		}

		public virtual bool LoadByPrimaryKey(esSqlAccessType sqlAccessType, System.Int32 orderID, System.Int32 productID)
		{
			if (sqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(orderID, productID);
			else
				return LoadByPrimaryKeyStoredProcedure(orderID, productID);
		}

		private bool LoadByPrimaryKeyDynamic(System.Int32 orderID, System.Int32 productID)
		{
			OrderDetailsQuery query = new OrderDetailsQuery();
			query.Where(query.OrderID == orderID, query.ProductID == productID);
			return this.Load(query);
		}

		private bool LoadByPrimaryKeyStoredProcedure(System.Int32 orderID, System.Int32 productID)
		{
			esParameters parms = new esParameters();
			parms.Add("OrderID", orderID);			parms.Add("ProductID", productID);
			return this.Load(esQueryType.StoredProcedure, this.es.spLoadByPrimaryKey, parms);
		}
		#endregion
		
		#region Properties
		
		
		
		/// <summary>
		/// Maps to Order Details.OrderID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? OrderID
		{
			get
			{
				return base.GetSystemInt32(OrderDetailsMetadata.ColumnNames.OrderID);
			}
			
			set
			{
				if(base.SetSystemInt32(OrderDetailsMetadata.ColumnNames.OrderID, value))
				{
					this._Orders = null;
					this.OnPropertyChanged("Orders");
					OnPropertyChanged(OrderDetailsMetadata.PropertyNames.OrderID);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Order Details.ProductID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? ProductID
		{
			get
			{
				return base.GetSystemInt32(OrderDetailsMetadata.ColumnNames.ProductID);
			}
			
			set
			{
				if(base.SetSystemInt32(OrderDetailsMetadata.ColumnNames.ProductID, value))
				{
					this._Products = null;
					this.OnPropertyChanged("Products");
					OnPropertyChanged(OrderDetailsMetadata.PropertyNames.ProductID);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Order Details.UnitPrice
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Decimal? UnitPrice
		{
			get
			{
				return base.GetSystemDecimal(OrderDetailsMetadata.ColumnNames.UnitPrice);
			}
			
			set
			{
				if(base.SetSystemDecimal(OrderDetailsMetadata.ColumnNames.UnitPrice, value))
				{
					OnPropertyChanged(OrderDetailsMetadata.PropertyNames.UnitPrice);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Order Details.Quantity
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int16? Quantity
		{
			get
			{
				return base.GetSystemInt16(OrderDetailsMetadata.ColumnNames.Quantity);
			}
			
			set
			{
				if(base.SetSystemInt16(OrderDetailsMetadata.ColumnNames.Quantity, value))
				{
					OnPropertyChanged(OrderDetailsMetadata.PropertyNames.Quantity);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Order Details.Discount
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Single? Discount
		{
			get
			{
				return base.GetSystemSingle(OrderDetailsMetadata.ColumnNames.Discount);
			}
			
			set
			{
				if(base.SetSystemSingle(OrderDetailsMetadata.ColumnNames.Discount, value))
				{
					OnPropertyChanged(OrderDetailsMetadata.PropertyNames.Discount);
				}
			}
		}		
		
		[CLSCompliant(false)]
		internal protected Orders _Orders;
		[CLSCompliant(false)]
		internal protected Products _Products;
		#endregion
		
		#region Housekeeping methods

		override protected IMetadata Meta
		{
			get
			{
				return OrderDetailsMetadata.Meta();
			}
		}

		#endregion		
		
		#region Query Logic

		public OrderDetailsQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new OrderDetailsQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(OrderDetailsQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return this.Query.Load();
		}
		
		protected void InitQuery(OrderDetailsQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntity)this).Connection;
			}			
		}

		#endregion
		
        [IgnoreDataMember]
		private OrderDetailsQuery query;		
	}



	[Serializable]
	abstract public partial class esOrderDetailsCollection : esEntityCollection<OrderDetails>
	{
		#region Housekeeping methods
		override protected IMetadata Meta
		{
			get
			{
				return OrderDetailsMetadata.Meta();
			}
		}

		protected override string GetCollectionName()
		{
			return "OrderDetailsCollection";
		}

		#endregion		
		
		#region Query Logic

	#if (!WindowsCE)
		[BrowsableAttribute(false)]
	#endif
		public OrderDetailsQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new OrderDetailsQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(OrderDetailsQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return Query.Load();
		}

		override protected esDynamicQuery GetDynamicQuery()
		{
			if (this.query == null)
			{
				this.query = new OrderDetailsQuery();
				this.InitQuery(query);
			}
			return this.query;
		}

		protected void InitQuery(OrderDetailsQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntityCollection)this).Connection;
			}			
		}

		protected override void HookupQuery(esDynamicQuery query)
		{
			this.InitQuery((OrderDetailsQuery)query);
		}

		#endregion
		
		private OrderDetailsQuery query;
	}



	[Serializable]
	abstract public partial class esOrderDetailsQuery : esDynamicQuery
	{
		override protected IMetadata Meta
		{
			get
			{
				return OrderDetailsMetadata.Meta();
			}
		}	
		
		#region QueryItemFromName
		
        protected override esQueryItem QueryItemFromName(string name)
        {
            switch (name)
            {
				case "OrderID": return this.OrderID;
				case "ProductID": return this.ProductID;
				case "UnitPrice": return this.UnitPrice;
				case "Quantity": return this.Quantity;
				case "Discount": return this.Discount;

                default: return null;
            }
        }		
		
		#endregion
		
		#region esQueryItems

		public esQueryItem OrderID
		{
			get { return new esQueryItem(this, OrderDetailsMetadata.ColumnNames.OrderID, esSystemType.Int32); }
		} 
		
		public esQueryItem ProductID
		{
			get { return new esQueryItem(this, OrderDetailsMetadata.ColumnNames.ProductID, esSystemType.Int32); }
		} 
		
		public esQueryItem UnitPrice
		{
			get { return new esQueryItem(this, OrderDetailsMetadata.ColumnNames.UnitPrice, esSystemType.Decimal); }
		} 
		
		public esQueryItem Quantity
		{
			get { return new esQueryItem(this, OrderDetailsMetadata.ColumnNames.Quantity, esSystemType.Int16); }
		} 
		
		public esQueryItem Discount
		{
			get { return new esQueryItem(this, OrderDetailsMetadata.ColumnNames.Discount, esSystemType.Single); }
		} 
		
		#endregion
		
	}


	
	public partial class OrderDetails : esOrderDetails
	{

		
		#region Orders - Many To One (FK_Order_Details_Orders)
		
	    [EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeOrders()
		{
		    return this._Orders != null ? true : false;
		}
		

		[DataMember(Name="Orders", EmitDefaultValue = false)]
					
		public Orders Orders
		{
			get
			{
                if (this._Orders == null)
                {
                    this._Orders = new Orders();
                    this._Orders.es.Connection.Name = this.es.Connection.Name;
                    this.SetPreSave("Orders", this._Orders);

					if(this._Orders == null && OrderID != null)
                    {
                        if (!this.es.IsLazyLoadDisabled)
                        {
							this._Orders.Query.Where(this._Orders.Query.OrderID == this.OrderID);
							this._Orders.Query.Load();
                        }
                    }
                }

				return this._Orders;
			}
			
			set
			{
				this.RemovePreSave("Orders");
				

				if(value == null)
				{
					this.OrderID = null;
					this._Orders = null;
				}
				else
				{
					this.OrderID = value.OrderID;
					this._Orders = value;
					this.SetPreSave("Orders", this._Orders);
				}
				
			}
		}
		#endregion
		

		
		#region Products - Many To One (FK_Order_Details_Products)
		
	    [EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeProducts()
		{
		    return this._Products != null ? true : false;
		}
		

		[DataMember(Name="Products", EmitDefaultValue = false)]
					
		public Products Products
		{
			get
			{
                if (this._Products == null)
                {
                    this._Products = new Products();
                    this._Products.es.Connection.Name = this.es.Connection.Name;
                    this.SetPreSave("Products", this._Products);

					if(this._Products == null && ProductID != null)
                    {
                        if (!this.es.IsLazyLoadDisabled)
                        {
							this._Products.Query.Where(this._Products.Query.ProductID == this.ProductID);
							this._Products.Query.Load();
                        }
                    }
                }

				return this._Products;
			}
			
			set
			{
				this.RemovePreSave("Products");
				

				if(value == null)
				{
					this.ProductID = null;
					this._Products = null;
				}
				else
				{
					this.ProductID = value.ProductID;
					this._Products = value;
					this.SetPreSave("Products", this._Products);
				}
				
			}
		}
		#endregion
		

		
		/// <summary>
		/// Used internally for retrieving AutoIncrementing keys
		/// during hierarchical PreSave.
		/// </summary>
		protected override void ApplyPreSaveKeys()
		{
			if(!this.es.IsDeleted && this._Orders != null)
			{
				this.OrderID = this._Orders.OrderID;
			}
			if(!this.es.IsDeleted && this._Products != null)
			{
				this.ProductID = this._Products.ProductID;
			}
		}
		
	}
	



	[Serializable]
	public partial class OrderDetailsMetadata : esMetadata, IMetadata
	{
		#region Protected Constructor
		protected OrderDetailsMetadata()
		{
			m_columns = new esColumnMetadataCollection();
			esColumnMetadata c;

			c = new esColumnMetadata(OrderDetailsMetadata.ColumnNames.OrderID, 0, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = OrderDetailsMetadata.PropertyNames.OrderID;
			c.IsInPrimaryKey = true;
			c.NumericPrecision = 10;
			m_columns.Add(c);
				
			c = new esColumnMetadata(OrderDetailsMetadata.ColumnNames.ProductID, 1, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = OrderDetailsMetadata.PropertyNames.ProductID;
			c.IsInPrimaryKey = true;
			c.NumericPrecision = 10;
			m_columns.Add(c);
				
			c = new esColumnMetadata(OrderDetailsMetadata.ColumnNames.UnitPrice, 2, typeof(System.Decimal), esSystemType.Decimal);
			c.PropertyName = OrderDetailsMetadata.PropertyNames.UnitPrice;
			c.NumericPrecision = 19;
			c.HasDefault = true;
			c.Default = @"((0))";
			m_columns.Add(c);
				
			c = new esColumnMetadata(OrderDetailsMetadata.ColumnNames.Quantity, 3, typeof(System.Int16), esSystemType.Int16);
			c.PropertyName = OrderDetailsMetadata.PropertyNames.Quantity;
			c.NumericPrecision = 5;
			c.HasDefault = true;
			c.Default = @"((1))";
			m_columns.Add(c);
				
			c = new esColumnMetadata(OrderDetailsMetadata.ColumnNames.Discount, 4, typeof(System.Single), esSystemType.Single);
			c.PropertyName = OrderDetailsMetadata.PropertyNames.Discount;
			c.NumericPrecision = 7;
			c.HasDefault = true;
			c.Default = @"((0))";
			m_columns.Add(c);
				
		}
		#endregion	
	
		static public OrderDetailsMetadata Meta()
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
			 public const string OrderID = "OrderID";
			 public const string ProductID = "ProductID";
			 public const string UnitPrice = "UnitPrice";
			 public const string Quantity = "Quantity";
			 public const string Discount = "Discount";
		}
		#endregion	
		
		#region PropertyNames
		public class PropertyNames
		{ 
			 public const string OrderID = "OrderID";
			 public const string ProductID = "ProductID";
			 public const string UnitPrice = "UnitPrice";
			 public const string Quantity = "Quantity";
			 public const string Discount = "Discount";
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
			lock (typeof(OrderDetailsMetadata))
			{
				if(OrderDetailsMetadata.mapDelegates == null)
				{
					OrderDetailsMetadata.mapDelegates = new Dictionary<string,MapToMeta>();
				}
				
				if (OrderDetailsMetadata.meta == null)
				{
					OrderDetailsMetadata.meta = new OrderDetailsMetadata();
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


				meta.AddTypeMap("OrderID", new esTypeMap("int", "System.Int32"));
				meta.AddTypeMap("ProductID", new esTypeMap("int", "System.Int32"));
				meta.AddTypeMap("UnitPrice", new esTypeMap("money", "System.Decimal"));
				meta.AddTypeMap("Quantity", new esTypeMap("smallint", "System.Int16"));
				meta.AddTypeMap("Discount", new esTypeMap("real", "System.Single"));			
				
				
				
				meta.Source = "Order Details";
				meta.Destination = "Order Details";
				
				meta.spInsert = "proc_Order DetailsInsert";				
				meta.spUpdate = "proc_Order DetailsUpdate";		
				meta.spDelete = "proc_Order DetailsDelete";
				meta.spLoadAll = "proc_Order DetailsLoadAll";
				meta.spLoadByPrimaryKey = "proc_Order DetailsLoadByPrimaryKey";
				
				this.m_providerMetadataMaps["esDefault"] = meta;
			}
			
			return this.m_providerMetadataMaps["esDefault"];
		}

		#endregion

		static private OrderDetailsMetadata meta;
		static protected Dictionary<string, MapToMeta> mapDelegates;
		static private int _esDefault = RegisterDelegateesDefault();
	}
}
