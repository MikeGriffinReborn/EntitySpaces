
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
	/// Encapsulates the 'Products' table
	/// </summary>

    [DebuggerDisplay("Data = {Debug}")]
	[Serializable]
	[DataContract]
	[KnownType(typeof(Products))]	
	[XmlType("Products")]
	public partial class Products : esProducts
	{	
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden | DebuggerBrowsableState.Never)]
		protected override esEntityDebuggerView[] Debug
		{
			get { return base.Debug; }
		}

		override public esEntity CreateInstance()
		{
			return new Products();
		}
		
		#region Static Quick Access Methods
		static public void Delete(System.Int32 productID)
		{
			var obj = new Products();
			obj.ProductID = productID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save();
		}

	    static public void Delete(System.Int32 productID, esSqlAccessType sqlAccessType)
		{
			var obj = new Products();
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
	[XmlType("ProductsCollection")]
	public partial class ProductsCollection : esProductsCollection, IEnumerable<Products>
	{
		public Products FindByPrimaryKey(System.Int32 productID)
		{
			return this.SingleOrDefault(e => e.ProductID == productID);
		}

		
				
	}



    [DebuggerDisplay("Query = {Parse()}")]
	[Serializable]	
	public partial class ProductsQuery : esProductsQuery
	{
		public ProductsQuery(string joinAlias)
		{
			this.es.JoinAlias = joinAlias;
		}	

		override protected string GetQueryName()
		{
			return "ProductsQuery";
		}
		
					
	
		#region Explicit Casts
		
		public static explicit operator string(ProductsQuery query)
		{
			return ProductsQuery.SerializeHelper.ToXml(query);
		}

		public static explicit operator ProductsQuery(string query)
		{
			return (ProductsQuery)ProductsQuery.SerializeHelper.FromXml(query, typeof(ProductsQuery));
		}
		
		#endregion		
	}

	[DataContract]
	[Serializable]
	abstract public partial class esProducts : esEntity
	{
		public esProducts()
		{

		}
		
		#region LoadByPrimaryKey
		public virtual bool LoadByPrimaryKey(System.Int32 productID)
		{
			if(this.es.Connection.SqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(productID);
			else
				return LoadByPrimaryKeyStoredProcedure(productID);
		}

		public virtual bool LoadByPrimaryKey(esSqlAccessType sqlAccessType, System.Int32 productID)
		{
			if (sqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(productID);
			else
				return LoadByPrimaryKeyStoredProcedure(productID);
		}

		private bool LoadByPrimaryKeyDynamic(System.Int32 productID)
		{
			ProductsQuery query = new ProductsQuery();
			query.Where(query.ProductID == productID);
			return this.Load(query);
		}

		private bool LoadByPrimaryKeyStoredProcedure(System.Int32 productID)
		{
			esParameters parms = new esParameters();
			parms.Add("ProductID", productID);
			return this.Load(esQueryType.StoredProcedure, this.es.spLoadByPrimaryKey, parms);
		}
		#endregion
		
		#region Properties
		
		
		
		/// <summary>
		/// Maps to Products.ProductID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? ProductID
		{
			get
			{
				return base.GetSystemInt32(ProductsMetadata.ColumnNames.ProductID);
			}
			
			set
			{
				if(base.SetSystemInt32(ProductsMetadata.ColumnNames.ProductID, value))
				{
					OnPropertyChanged(ProductsMetadata.PropertyNames.ProductID);
				}
			}
		}
		
		/// <summary>
		/// Maps to Products.ProductName
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String ProductName
		{
			get
			{
				return base.GetSystemString(ProductsMetadata.ColumnNames.ProductName);
			}
			
			set
			{
				if(base.SetSystemString(ProductsMetadata.ColumnNames.ProductName, value))
				{
					OnPropertyChanged(ProductsMetadata.PropertyNames.ProductName);
				}
			}
		}
		
		/// <summary>
		/// Maps to Products.SupplierID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? SupplierID
		{
			get
			{
				return base.GetSystemInt32(ProductsMetadata.ColumnNames.SupplierID);
			}
			
			set
			{
				if(base.SetSystemInt32(ProductsMetadata.ColumnNames.SupplierID, value))
				{
					this._Suppliers = null;
					this.OnPropertyChanged("Suppliers");
					OnPropertyChanged(ProductsMetadata.PropertyNames.SupplierID);
				}
			}
		}
		
		/// <summary>
		/// Maps to Products.CategoryID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? CategoryID
		{
			get
			{
				return base.GetSystemInt32(ProductsMetadata.ColumnNames.CategoryID);
			}
			
			set
			{
				if(base.SetSystemInt32(ProductsMetadata.ColumnNames.CategoryID, value))
				{
					this._Categories = null;
					this.OnPropertyChanged("Categories");
					OnPropertyChanged(ProductsMetadata.PropertyNames.CategoryID);
				}
			}
		}
		
		/// <summary>
		/// Maps to Products.QuantityPerUnit
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String QuantityPerUnit
		{
			get
			{
				return base.GetSystemString(ProductsMetadata.ColumnNames.QuantityPerUnit);
			}
			
			set
			{
				if(base.SetSystemString(ProductsMetadata.ColumnNames.QuantityPerUnit, value))
				{
					OnPropertyChanged(ProductsMetadata.PropertyNames.QuantityPerUnit);
				}
			}
		}
		
		/// <summary>
		/// Maps to Products.UnitPrice
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Decimal? UnitPrice
		{
			get
			{
				return base.GetSystemDecimal(ProductsMetadata.ColumnNames.UnitPrice);
			}
			
			set
			{
				if(base.SetSystemDecimal(ProductsMetadata.ColumnNames.UnitPrice, value))
				{
					OnPropertyChanged(ProductsMetadata.PropertyNames.UnitPrice);
				}
			}
		}
		
		/// <summary>
		/// Maps to Products.UnitsInStock
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int16? UnitsInStock
		{
			get
			{
				return base.GetSystemInt16(ProductsMetadata.ColumnNames.UnitsInStock);
			}
			
			set
			{
				if(base.SetSystemInt16(ProductsMetadata.ColumnNames.UnitsInStock, value))
				{
					OnPropertyChanged(ProductsMetadata.PropertyNames.UnitsInStock);
				}
			}
		}
		
		/// <summary>
		/// Maps to Products.UnitsOnOrder
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int16? UnitsOnOrder
		{
			get
			{
				return base.GetSystemInt16(ProductsMetadata.ColumnNames.UnitsOnOrder);
			}
			
			set
			{
				if(base.SetSystemInt16(ProductsMetadata.ColumnNames.UnitsOnOrder, value))
				{
					OnPropertyChanged(ProductsMetadata.PropertyNames.UnitsOnOrder);
				}
			}
		}
		
		/// <summary>
		/// Maps to Products.ReorderLevel
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int16? ReorderLevel
		{
			get
			{
				return base.GetSystemInt16(ProductsMetadata.ColumnNames.ReorderLevel);
			}
			
			set
			{
				if(base.SetSystemInt16(ProductsMetadata.ColumnNames.ReorderLevel, value))
				{
					OnPropertyChanged(ProductsMetadata.PropertyNames.ReorderLevel);
				}
			}
		}
		
		/// <summary>
		/// Maps to Products.Discontinued
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Boolean? Discontinued
		{
			get
			{
				return base.GetSystemBoolean(ProductsMetadata.ColumnNames.Discontinued);
			}
			
			set
			{
				if(base.SetSystemBoolean(ProductsMetadata.ColumnNames.Discontinued, value))
				{
					OnPropertyChanged(ProductsMetadata.PropertyNames.Discontinued);
				}
			}
		}
		
		internal protected Categories _Categories;
		internal protected Suppliers _Suppliers;
		#endregion
		
		#region Housekeeping methods

		override protected IMetadata Meta
		{
			get
			{
				return ProductsMetadata.Meta();
			}
		}

		#endregion		
		
		#region Query Logic

		public ProductsQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new ProductsQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(ProductsQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return this.Query.Load();
		}
		
		protected void InitQuery(ProductsQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntity)this).Connection;
			}			
		}

		#endregion
		
        [IgnoreDataMember]
		private ProductsQuery query;		
	}



	[Serializable]
	abstract public partial class esProductsCollection : esEntityCollection<Products>
	{
		#region Housekeeping methods
		override protected IMetadata Meta
		{
			get
			{
				return ProductsMetadata.Meta();
			}
		}

		protected override string GetCollectionName()
		{
			return "ProductsCollection";
		}

		#endregion		
		
		#region Query Logic

	#if (!WindowsCE)
		[BrowsableAttribute(false)]
	#endif
		public ProductsQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new ProductsQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(ProductsQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return Query.Load();
		}

		override protected esDynamicQuery GetDynamicQuery()
		{
			if (this.query == null)
			{
				this.query = new ProductsQuery();
				this.InitQuery(query);
			}
			return this.query;
		}

		protected void InitQuery(ProductsQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntityCollection)this).Connection;
			}			
		}

		protected override void HookupQuery(esDynamicQuery query)
		{
			this.InitQuery((ProductsQuery)query);
		}

		#endregion
		
		private ProductsQuery query;
	}



	[Serializable]
	abstract public partial class esProductsQuery : esDynamicQuery
	{
		override protected IMetadata Meta
		{
			get
			{
				return ProductsMetadata.Meta();
			}
		}	
		
		#region QueryItemFromName
		
        protected override esQueryItem QueryItemFromName(string name)
        {
            switch (name)
            {
				case "ProductID": return this.ProductID;
				case "ProductName": return this.ProductName;
				case "SupplierID": return this.SupplierID;
				case "CategoryID": return this.CategoryID;
				case "QuantityPerUnit": return this.QuantityPerUnit;
				case "UnitPrice": return this.UnitPrice;
				case "UnitsInStock": return this.UnitsInStock;
				case "UnitsOnOrder": return this.UnitsOnOrder;
				case "ReorderLevel": return this.ReorderLevel;
				case "Discontinued": return this.Discontinued;

                default: return null;
            }
        }		
		
		#endregion
		
		#region esQueryItems

		public esQueryItem ProductID
		{
			get { return new esQueryItem(this, ProductsMetadata.ColumnNames.ProductID, esSystemType.Int32); }
		} 
		
		public esQueryItem ProductName
		{
			get { return new esQueryItem(this, ProductsMetadata.ColumnNames.ProductName, esSystemType.String); }
		} 
		
		public esQueryItem SupplierID
		{
			get { return new esQueryItem(this, ProductsMetadata.ColumnNames.SupplierID, esSystemType.Int32); }
		} 
		
		public esQueryItem CategoryID
		{
			get { return new esQueryItem(this, ProductsMetadata.ColumnNames.CategoryID, esSystemType.Int32); }
		} 
		
		public esQueryItem QuantityPerUnit
		{
			get { return new esQueryItem(this, ProductsMetadata.ColumnNames.QuantityPerUnit, esSystemType.String); }
		} 
		
		public esQueryItem UnitPrice
		{
			get { return new esQueryItem(this, ProductsMetadata.ColumnNames.UnitPrice, esSystemType.Decimal); }
		} 
		
		public esQueryItem UnitsInStock
		{
			get { return new esQueryItem(this, ProductsMetadata.ColumnNames.UnitsInStock, esSystemType.Int16); }
		} 
		
		public esQueryItem UnitsOnOrder
		{
			get { return new esQueryItem(this, ProductsMetadata.ColumnNames.UnitsOnOrder, esSystemType.Int16); }
		} 
		
		public esQueryItem ReorderLevel
		{
			get { return new esQueryItem(this, ProductsMetadata.ColumnNames.ReorderLevel, esSystemType.Int16); }
		} 
		
		public esQueryItem Discontinued
		{
			get { return new esQueryItem(this, ProductsMetadata.ColumnNames.Discontinued, esSystemType.Boolean); }
		} 
		
		#endregion
		
	}


	
	public partial class Products : esProducts
	{

			
		#region OrdersCollection - Many To Many (FK_Order_Details_Products)
		
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
					if (!this.es.IsLazyLoadDisabled && this.ProductID != null)
					{
						OrdersQuery m = new OrdersQuery("m");
						OrderDetailsQuery j = new OrderDetailsQuery("j");
						m.Select(m);
						m.InnerJoin(j).On(m.OrderID == j.OrderID);
                        m.Where(j.ProductID == this.ProductID);

						this._OrdersCollection.Load(m);
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

		/// <summary>
		/// Many to Many Associate
		/// Foreign Key Name - FK_Order_Details_Products
		/// </summary>
		public void ASsociateOrderDetailsCollection(Orders entity)
		{
			if (this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection == null)
			{
				this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection = new OrderDetailsCollection();
				this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection.es.Connection.Name = this.es.Connection.Name;
				this.SetPostSave("ManyEntitySpacesMetadataEngineSqlSqlTableCollection", this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection);
			}

			OrderDetails obj = this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection.AddNew();
			obj.ProductID = this.ProductID;
			obj.OrderID = entity.OrderID;
		}

		/// <summary>
		/// Many to Many Dissociate
		/// Foreign Key Name - FK_Order_Details_Products
		/// </summary>
		public void DiSsociateOrderDetailsCollection(Orders entity)
		{
			if (this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection == null)
			{
				this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection = new OrderDetailsCollection();
				this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection.es.Connection.Name = this.es.Connection.Name;
				this.SetPostSave("ManyEntitySpacesMetadataEngineSqlSqlTableCollection", this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection);
			}

			OrderDetails obj = this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection.AddNew();
			obj.ProductID = this.ProductID;
            obj.OrderID = entity.OrderID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
		}

		private OrdersCollection _OrdersCollection;
		private OrderDetailsCollection _ManyEntitySpacesMetadataEngineSqlSqlTableCollection;
		#endregion

		#region OrderDetailsCollection - Zero To Many (FK_Order_Details_Products)
		
		static public esPrefetchMap Prefetch_OrderDetailsCollection
		{
			get
			{
				esPrefetchMap map = new esPrefetchMap
				{
					PrefetchDelegate = BusinessObjects.Products.OrderDetailsCollection_Delegate,
					PropertyName = "OrderDetailsCollection",
					MyColumnName = "ProductID",
					ParentColumnName = "ProductID",
					IsMultiPartKey = false
				};
				return map;
			}
		}		
		
		static private void OrderDetailsCollection_Delegate(esPrefetchParameters data)
		{
			ProductsQuery parent = new ProductsQuery(data.NextAlias());

			OrderDetailsQuery me = data.You != null ? data.You as OrderDetailsQuery : new OrderDetailsQuery(data.NextAlias());

			if (data.Root == null)
			{
				data.Root = me;
			}
			
			data.Root.InnerJoin(parent).On(parent.ProductID == me.ProductID);

			data.You = parent;
		}	
	
		/// <summary>
		/// Zero to Many
		/// Foreign Key Name - FK_Order_Details_Products
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeOrderDetailsCollection()
		{
		    if(this._OrderDetailsCollection != null && this._OrderDetailsCollection.Count > 0)
				return true;
            else
				return false;
		}	
		

		[DataMember(Name="OrderDetailsCollection", EmitDefaultValue = false)]
		public OrderDetailsCollection OrderDetailsCollection
		{
			get
			{
				if(this._OrderDetailsCollection == null)
				{
					this._OrderDetailsCollection = new OrderDetailsCollection();
					this._OrderDetailsCollection.es.Connection.Name = this.es.Connection.Name;
					this.SetPostSave("OrderDetailsCollection", this._OrderDetailsCollection);
				
					if (this.ProductID != null)
					{
						if (!this.es.IsLazyLoadDisabled)
						{
							this._OrderDetailsCollection.Query.Where(this._OrderDetailsCollection.Query.ProductID == this.ProductID);
							this._OrderDetailsCollection.Query.Load();
						}

						// Auto-hookup Foreign Keys
						this._OrderDetailsCollection.fks.Add(OrderDetailsMetadata.ColumnNames.ProductID, this.ProductID);
					}
				}

				return this._OrderDetailsCollection;
			}
			
			set 
			{ 
				if (value != null) throw new Exception("'value' Must be null"); 
			 
				if (this._OrderDetailsCollection != null) 
				{ 
					this.RemovePostSave("OrderDetailsCollection"); 
					this._OrderDetailsCollection = null;
					
				} 
			} 			
		}
		

		
			
		
		private OrderDetailsCollection _OrderDetailsCollection;
		#endregion

		
		#region Categories - Many To One (FK_Products_Categories)
		
	    [EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeCategories()
		{
		    return this._Categories != null ? true : false;
		}
		

		[DataMember(Name="Categories", EmitDefaultValue = false)]
					
		public Categories Categories
		{
			get
			{
                if (this._Categories == null)
                {
                    this._Categories = new Categories();
                    this._Categories.es.Connection.Name = this.es.Connection.Name;
                    this.SetPreSave("Categories", this._Categories);

					if(this._Categories == null && CategoryID != null)
                    {
                        if (!this.es.IsLazyLoadDisabled)
                        {
							this._Categories.Query.Where(this._Categories.Query.CategoryID == this.CategoryID);
							this._Categories.Query.Load();
                        }
                    }
                }

				return this._Categories;
			}
			
			set
			{
				this.RemovePreSave("Categories");
				

				if(value == null)
				{
					this.CategoryID = null;
					this._Categories = null;
				}
				else
				{
					this.CategoryID = value.CategoryID;
					this._Categories = value;
					this.SetPreSave("Categories", this._Categories);
				}
				
			}
		}
		#endregion
		

		
		#region Suppliers - Many To One (FK_Products_Suppliers)
		
	    [EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeSuppliers()
		{
		    return this._Suppliers != null ? true : false;
		}
		

		[DataMember(Name="Suppliers", EmitDefaultValue = false)]
					
		public Suppliers Suppliers
		{
			get
			{
                if (this._Suppliers == null)
                {
                    this._Suppliers = new Suppliers();
                    this._Suppliers.es.Connection.Name = this.es.Connection.Name;
                    this.SetPreSave("Suppliers", this._Suppliers);

					if(this._Suppliers == null && SupplierID != null)
                    {
                        if (!this.es.IsLazyLoadDisabled)
                        {
							this._Suppliers.Query.Where(this._Suppliers.Query.SupplierID == this.SupplierID);
							this._Suppliers.Query.Load();
                        }
                    }
                }

				return this._Suppliers;
			}
			
			set
			{
				this.RemovePreSave("Suppliers");
				

				if(value == null)
				{
					this.SupplierID = null;
					this._Suppliers = null;
				}
				else
				{
					this.SupplierID = value.SupplierID;
					this._Suppliers = value;
					this.SetPreSave("Suppliers", this._Suppliers);
				}
				
			}
		}
		#endregion
		

		
		protected override esEntityCollectionBase CreateCollectionForPrefetch(string name)
		{
			esEntityCollectionBase coll = null;

			switch (name)
			{
				case "OrderDetailsCollection":
					coll = this.OrderDetailsCollection;
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
			
			props.Add(new esPropertyDescriptor(this, "OrderDetailsCollection", typeof(OrderDetailsCollection), new OrderDetails()));
		
			return props;
		}
		/// <summary>
		/// Used internally for retrieving AutoIncrementing keys
		/// during hierarchical PreSave.
		/// </summary>
		protected override void ApplyPreSaveKeys()
		{
			if(!this.es.IsDeleted && this._Categories != null)
			{
				this.CategoryID = this._Categories.CategoryID;
			}
			if(!this.es.IsDeleted && this._Suppliers != null)
			{
				this.SupplierID = this._Suppliers.SupplierID;
			}
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
			if(this._OrderDetailsCollection != null)
			{
				Apply(this._OrderDetailsCollection, "ProductID", this.ProductID);
			}
			if(this._OrderDetailsCollection != null)
			{
				Apply(this._OrderDetailsCollection, "ProductID", this.ProductID);
			}
		}
		
	}
	



	[Serializable]
	public partial class ProductsMetadata : esMetadata, IMetadata
	{
		#region Protected Constructor
		protected ProductsMetadata()
		{
			m_columns = new esColumnMetadataCollection();
			esColumnMetadata c;

			c = new esColumnMetadata(ProductsMetadata.ColumnNames.ProductID, 0, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = ProductsMetadata.PropertyNames.ProductID;
			c.IsInPrimaryKey = true;
			c.IsAutoIncrement = true;
			c.NumericPrecision = 10;
			m_columns.Add(c);
				
			c = new esColumnMetadata(ProductsMetadata.ColumnNames.ProductName, 1, typeof(System.String), esSystemType.String);
			c.PropertyName = ProductsMetadata.PropertyNames.ProductName;
			c.CharacterMaxLength = 40;
			m_columns.Add(c);
				
			c = new esColumnMetadata(ProductsMetadata.ColumnNames.SupplierID, 2, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = ProductsMetadata.PropertyNames.SupplierID;
			c.NumericPrecision = 10;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(ProductsMetadata.ColumnNames.CategoryID, 3, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = ProductsMetadata.PropertyNames.CategoryID;
			c.NumericPrecision = 10;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(ProductsMetadata.ColumnNames.QuantityPerUnit, 4, typeof(System.String), esSystemType.String);
			c.PropertyName = ProductsMetadata.PropertyNames.QuantityPerUnit;
			c.CharacterMaxLength = 20;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(ProductsMetadata.ColumnNames.UnitPrice, 5, typeof(System.Decimal), esSystemType.Decimal);
			c.PropertyName = ProductsMetadata.PropertyNames.UnitPrice;
			c.NumericPrecision = 19;
			c.HasDefault = true;
			c.Default = @"(0)";
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(ProductsMetadata.ColumnNames.UnitsInStock, 6, typeof(System.Int16), esSystemType.Int16);
			c.PropertyName = ProductsMetadata.PropertyNames.UnitsInStock;
			c.NumericPrecision = 5;
			c.HasDefault = true;
			c.Default = @"(0)";
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(ProductsMetadata.ColumnNames.UnitsOnOrder, 7, typeof(System.Int16), esSystemType.Int16);
			c.PropertyName = ProductsMetadata.PropertyNames.UnitsOnOrder;
			c.NumericPrecision = 5;
			c.HasDefault = true;
			c.Default = @"(0)";
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(ProductsMetadata.ColumnNames.ReorderLevel, 8, typeof(System.Int16), esSystemType.Int16);
			c.PropertyName = ProductsMetadata.PropertyNames.ReorderLevel;
			c.NumericPrecision = 5;
			c.HasDefault = true;
			c.Default = @"(0)";
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(ProductsMetadata.ColumnNames.Discontinued, 9, typeof(System.Boolean), esSystemType.Boolean);
			c.PropertyName = ProductsMetadata.PropertyNames.Discontinued;
			c.HasDefault = true;
			c.Default = @"(0)";
			m_columns.Add(c);
				
		}
		#endregion	
	
		static public ProductsMetadata Meta()
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
			 public const string ProductID = "ProductID";
			 public const string ProductName = "ProductName";
			 public const string SupplierID = "SupplierID";
			 public const string CategoryID = "CategoryID";
			 public const string QuantityPerUnit = "QuantityPerUnit";
			 public const string UnitPrice = "UnitPrice";
			 public const string UnitsInStock = "UnitsInStock";
			 public const string UnitsOnOrder = "UnitsOnOrder";
			 public const string ReorderLevel = "ReorderLevel";
			 public const string Discontinued = "Discontinued";
		}
		#endregion	
		
		#region PropertyNames
		public class PropertyNames
		{ 
			 public const string ProductID = "ProductID";
			 public const string ProductName = "ProductName";
			 public const string SupplierID = "SupplierID";
			 public const string CategoryID = "CategoryID";
			 public const string QuantityPerUnit = "QuantityPerUnit";
			 public const string UnitPrice = "UnitPrice";
			 public const string UnitsInStock = "UnitsInStock";
			 public const string UnitsOnOrder = "UnitsOnOrder";
			 public const string ReorderLevel = "ReorderLevel";
			 public const string Discontinued = "Discontinued";
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
			lock (typeof(ProductsMetadata))
			{
				if(ProductsMetadata.mapDelegates == null)
				{
					ProductsMetadata.mapDelegates = new Dictionary<string,MapToMeta>();
				}
				
				if (ProductsMetadata.meta == null)
				{
					ProductsMetadata.meta = new ProductsMetadata();
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


				meta.AddTypeMap("ProductID", new esTypeMap("int", "System.Int32"));
				meta.AddTypeMap("ProductName", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("SupplierID", new esTypeMap("int", "System.Int32"));
				meta.AddTypeMap("CategoryID", new esTypeMap("int", "System.Int32"));
				meta.AddTypeMap("QuantityPerUnit", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("UnitPrice", new esTypeMap("money", "System.Decimal"));
				meta.AddTypeMap("UnitsInStock", new esTypeMap("smallint", "System.Int16"));
				meta.AddTypeMap("UnitsOnOrder", new esTypeMap("smallint", "System.Int16"));
				meta.AddTypeMap("ReorderLevel", new esTypeMap("smallint", "System.Int16"));
				meta.AddTypeMap("Discontinued", new esTypeMap("bit", "System.Boolean"));			
				
				
				
				meta.Source = "Products";
				meta.Destination = "Products";
				
				meta.spInsert = "proc_ProductsInsert";				
				meta.spUpdate = "proc_ProductsUpdate";		
				meta.spDelete = "proc_ProductsDelete";
				meta.spLoadAll = "proc_ProductsLoadAll";
				meta.spLoadByPrimaryKey = "proc_ProductsLoadByPrimaryKey";
				
				this.m_providerMetadataMaps["esDefault"] = meta;
			}
			
			return this.m_providerMetadataMaps["esDefault"];
		}

		#endregion

		static private ProductsMetadata meta;
		static protected Dictionary<string, MapToMeta> mapDelegates;
		static private int _esDefault = RegisterDelegateesDefault();
	}
}
