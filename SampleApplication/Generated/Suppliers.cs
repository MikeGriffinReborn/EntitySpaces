
/*
===============================================================================
                    EntitySpaces Studio by EntitySpaces, LLC
             Persistence Layer and Business Objects for Microsoft .NET
             EntitySpaces(TM) is a legal trademark of EntitySpaces, LLC
                          http://www.entityspaces.net
===============================================================================
EntitySpaces Version : 2019.1.0725.0
EntitySpaces Driver  : SQL
Date Generated       : 7/31/2019 10:51:55 AM
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
	/// Encapsulates the 'Suppliers' table
	/// </summary>

    [DebuggerDisplay("Data = {Debug}")]
	[Serializable]
	[DataContract]
	[KnownType(typeof(Suppliers))]	
	[XmlType("Suppliers")]
	public partial class Suppliers : esSuppliers
	{	
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden | DebuggerBrowsableState.Never)]
		protected override esEntityDebuggerView[] Debug
		{
			get { return base.Debug; }
		}

		override public esEntity CreateInstance()
		{
			return new Suppliers();
		}
		
		#region Static Quick Access Methods
		static public void Delete(System.Int32 supplierID)
		{
			var obj = new Suppliers();
			obj.SupplierID = supplierID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save();
		}

	    static public void Delete(System.Int32 supplierID, esSqlAccessType sqlAccessType)
		{
			var obj = new Suppliers();
			obj.SupplierID = supplierID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save(sqlAccessType);
		}
		#endregion

		
					
		
	
	}



    [DebuggerDisplay("Count = {Count}")]
	[Serializable]
	[CollectionDataContract]
	[XmlType("SuppliersCollection")]
	public partial class SuppliersCollection : esSuppliersCollection, IEnumerable<Suppliers>
	{
		public Suppliers FindByPrimaryKey(System.Int32 supplierID)
		{
			return this.SingleOrDefault(e => e.SupplierID == supplierID);
		}

		
				
	}



    [DebuggerDisplay("Query = {Parse()}")]
	[Serializable]	
	public partial class SuppliersQuery : esSuppliersQuery
	{
		public SuppliersQuery(string joinAlias)
		{
			this.es.JoinAlias = joinAlias;
		}	

		override protected string GetQueryName()
		{
			return "SuppliersQuery";
		}
		
					
	
		#region Explicit Casts
		
		public static explicit operator string(SuppliersQuery query)
		{
			return SuppliersQuery.SerializeHelper.ToXml(query);
		}

		public static explicit operator SuppliersQuery(string query)
		{
			return (SuppliersQuery)SuppliersQuery.SerializeHelper.FromXml(query, typeof(SuppliersQuery));
		}
		
		#endregion		
	}

	[DataContract]
	[Serializable]
	abstract public partial class esSuppliers : esEntity
	{
		public esSuppliers()
		{

		}
		
		#region LoadByPrimaryKey
		public virtual bool LoadByPrimaryKey(System.Int32 supplierID)
		{
			if(this.es.Connection.SqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(supplierID);
			else
				return LoadByPrimaryKeyStoredProcedure(supplierID);
		}

		public virtual bool LoadByPrimaryKey(esSqlAccessType sqlAccessType, System.Int32 supplierID)
		{
			if (sqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(supplierID);
			else
				return LoadByPrimaryKeyStoredProcedure(supplierID);
		}

		private bool LoadByPrimaryKeyDynamic(System.Int32 supplierID)
		{
			SuppliersQuery query = new SuppliersQuery();
			query.Where(query.SupplierID == supplierID);
			return this.Load(query);
		}

		private bool LoadByPrimaryKeyStoredProcedure(System.Int32 supplierID)
		{
			esParameters parms = new esParameters();
			parms.Add("SupplierID", supplierID);
			return this.Load(esQueryType.StoredProcedure, this.es.spLoadByPrimaryKey, parms);
		}
		#endregion
		
		#region Properties
		
		
		
		/// <summary>
		/// Maps to Suppliers.SupplierID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? SupplierID
		{
			get
			{
				return base.GetSystemInt32(SuppliersMetadata.ColumnNames.SupplierID);
			}
			
			set
			{
				if(base.SetSystemInt32(SuppliersMetadata.ColumnNames.SupplierID, value))
				{
					OnPropertyChanged(SuppliersMetadata.PropertyNames.SupplierID);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Suppliers.CompanyName
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String CompanyName
		{
			get
			{
				return base.GetSystemString(SuppliersMetadata.ColumnNames.CompanyName);
			}
			
			set
			{
				if(base.SetSystemString(SuppliersMetadata.ColumnNames.CompanyName, value))
				{
					OnPropertyChanged(SuppliersMetadata.PropertyNames.CompanyName);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Suppliers.ContactName
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String ContactName
		{
			get
			{
				return base.GetSystemString(SuppliersMetadata.ColumnNames.ContactName);
			}
			
			set
			{
				if(base.SetSystemString(SuppliersMetadata.ColumnNames.ContactName, value))
				{
					OnPropertyChanged(SuppliersMetadata.PropertyNames.ContactName);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Suppliers.ContactTitle
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String ContactTitle
		{
			get
			{
				return base.GetSystemString(SuppliersMetadata.ColumnNames.ContactTitle);
			}
			
			set
			{
				if(base.SetSystemString(SuppliersMetadata.ColumnNames.ContactTitle, value))
				{
					OnPropertyChanged(SuppliersMetadata.PropertyNames.ContactTitle);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Suppliers.Address
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Address
		{
			get
			{
				return base.GetSystemString(SuppliersMetadata.ColumnNames.Address);
			}
			
			set
			{
				if(base.SetSystemString(SuppliersMetadata.ColumnNames.Address, value))
				{
					OnPropertyChanged(SuppliersMetadata.PropertyNames.Address);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Suppliers.City
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String City
		{
			get
			{
				return base.GetSystemString(SuppliersMetadata.ColumnNames.City);
			}
			
			set
			{
				if(base.SetSystemString(SuppliersMetadata.ColumnNames.City, value))
				{
					OnPropertyChanged(SuppliersMetadata.PropertyNames.City);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Suppliers.Region
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Region
		{
			get
			{
				return base.GetSystemString(SuppliersMetadata.ColumnNames.Region);
			}
			
			set
			{
				if(base.SetSystemString(SuppliersMetadata.ColumnNames.Region, value))
				{
					OnPropertyChanged(SuppliersMetadata.PropertyNames.Region);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Suppliers.PostalCode
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String PostalCode
		{
			get
			{
				return base.GetSystemString(SuppliersMetadata.ColumnNames.PostalCode);
			}
			
			set
			{
				if(base.SetSystemString(SuppliersMetadata.ColumnNames.PostalCode, value))
				{
					OnPropertyChanged(SuppliersMetadata.PropertyNames.PostalCode);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Suppliers.Country
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Country
		{
			get
			{
				return base.GetSystemString(SuppliersMetadata.ColumnNames.Country);
			}
			
			set
			{
				if(base.SetSystemString(SuppliersMetadata.ColumnNames.Country, value))
				{
					OnPropertyChanged(SuppliersMetadata.PropertyNames.Country);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Suppliers.Phone
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Phone
		{
			get
			{
				return base.GetSystemString(SuppliersMetadata.ColumnNames.Phone);
			}
			
			set
			{
				if(base.SetSystemString(SuppliersMetadata.ColumnNames.Phone, value))
				{
					OnPropertyChanged(SuppliersMetadata.PropertyNames.Phone);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Suppliers.Fax
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Fax
		{
			get
			{
				return base.GetSystemString(SuppliersMetadata.ColumnNames.Fax);
			}
			
			set
			{
				if(base.SetSystemString(SuppliersMetadata.ColumnNames.Fax, value))
				{
					OnPropertyChanged(SuppliersMetadata.PropertyNames.Fax);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Suppliers.HomePage
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String HomePage
		{
			get
			{
				return base.GetSystemString(SuppliersMetadata.ColumnNames.HomePage);
			}
			
			set
			{
				if(base.SetSystemString(SuppliersMetadata.ColumnNames.HomePage, value))
				{
					OnPropertyChanged(SuppliersMetadata.PropertyNames.HomePage);
				}
			}
		}		
		
		#endregion
		
		#region Housekeeping methods

		override protected IMetadata Meta
		{
			get
			{
				return SuppliersMetadata.Meta();
			}
		}

		#endregion		
		
		#region Query Logic

		public SuppliersQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new SuppliersQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(SuppliersQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return this.Query.Load();
		}
		
		protected void InitQuery(SuppliersQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntity)this).Connection;
			}			
		}

		#endregion
		
        [IgnoreDataMember]
		private SuppliersQuery query;		
	}



	[Serializable]
	abstract public partial class esSuppliersCollection : esEntityCollection<Suppliers>
	{
		#region Housekeeping methods
		override protected IMetadata Meta
		{
			get
			{
				return SuppliersMetadata.Meta();
			}
		}

		protected override string GetCollectionName()
		{
			return "SuppliersCollection";
		}

		#endregion		
		
		#region Query Logic

	#if (!WindowsCE)
		[BrowsableAttribute(false)]
	#endif
		public SuppliersQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new SuppliersQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(SuppliersQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return Query.Load();
		}

		override protected esDynamicQuery GetDynamicQuery()
		{
			if (this.query == null)
			{
				this.query = new SuppliersQuery();
				this.InitQuery(query);
			}
			return this.query;
		}

		protected void InitQuery(SuppliersQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntityCollection)this).Connection;
			}			
		}

		protected override void HookupQuery(esDynamicQuery query)
		{
			this.InitQuery((SuppliersQuery)query);
		}

		#endregion
		
		private SuppliersQuery query;
	}



	[Serializable]
	abstract public partial class esSuppliersQuery : esDynamicQuery
	{
		override protected IMetadata Meta
		{
			get
			{
				return SuppliersMetadata.Meta();
			}
		}	
		
		#region QueryItemFromName
		
        protected override esQueryItem QueryItemFromName(string name)
        {
            switch (name)
            {
				case "SupplierID": return this.SupplierID;
				case "CompanyName": return this.CompanyName;
				case "ContactName": return this.ContactName;
				case "ContactTitle": return this.ContactTitle;
				case "Address": return this.Address;
				case "City": return this.City;
				case "Region": return this.Region;
				case "PostalCode": return this.PostalCode;
				case "Country": return this.Country;
				case "Phone": return this.Phone;
				case "Fax": return this.Fax;
				case "HomePage": return this.HomePage;

                default: return null;
            }
        }		
		
		#endregion
		
		#region esQueryItems

		public esQueryItem SupplierID
		{
			get { return new esQueryItem(this, SuppliersMetadata.ColumnNames.SupplierID, esSystemType.Int32); }
		} 
		
		public esQueryItem CompanyName
		{
			get { return new esQueryItem(this, SuppliersMetadata.ColumnNames.CompanyName, esSystemType.String); }
		} 
		
		public esQueryItem ContactName
		{
			get { return new esQueryItem(this, SuppliersMetadata.ColumnNames.ContactName, esSystemType.String); }
		} 
		
		public esQueryItem ContactTitle
		{
			get { return new esQueryItem(this, SuppliersMetadata.ColumnNames.ContactTitle, esSystemType.String); }
		} 
		
		public esQueryItem Address
		{
			get { return new esQueryItem(this, SuppliersMetadata.ColumnNames.Address, esSystemType.String); }
		} 
		
		public esQueryItem City
		{
			get { return new esQueryItem(this, SuppliersMetadata.ColumnNames.City, esSystemType.String); }
		} 
		
		public esQueryItem Region
		{
			get { return new esQueryItem(this, SuppliersMetadata.ColumnNames.Region, esSystemType.String); }
		} 
		
		public esQueryItem PostalCode
		{
			get { return new esQueryItem(this, SuppliersMetadata.ColumnNames.PostalCode, esSystemType.String); }
		} 
		
		public esQueryItem Country
		{
			get { return new esQueryItem(this, SuppliersMetadata.ColumnNames.Country, esSystemType.String); }
		} 
		
		public esQueryItem Phone
		{
			get { return new esQueryItem(this, SuppliersMetadata.ColumnNames.Phone, esSystemType.String); }
		} 
		
		public esQueryItem Fax
		{
			get { return new esQueryItem(this, SuppliersMetadata.ColumnNames.Fax, esSystemType.String); }
		} 
		
		public esQueryItem HomePage
		{
			get { return new esQueryItem(this, SuppliersMetadata.ColumnNames.HomePage, esSystemType.String); }
		} 
		
		#endregion
		
	}


	
	public partial class Suppliers : esSuppliers
	{

		#region ProductsCollection - Zero To Many
		
		static public esPrefetchMap Prefetch_ProductsCollection
		{
			get
			{
				esPrefetchMap map = new esPrefetchMap();
				map.PrefetchDelegate = BusinessObjects.Suppliers.ProductsCollection_Delegate;
				map.PropertyName = "ProductsCollection";
				map.MyColumnName = "SupplierID";
				map.ParentColumnName = "SupplierID";
				map.IsMultiPartKey = false;
				return map;
			}
		}		
		
		static private void ProductsCollection_Delegate(esPrefetchParameters data)
		{
			SuppliersQuery parent = new SuppliersQuery(data.NextAlias());

			ProductsQuery me = data.You != null ? data.You as ProductsQuery : new ProductsQuery(data.NextAlias());

			if (data.Root == null)
			{
				data.Root = me;
			}
			
			data.Root.InnerJoin(parent).On(parent.SupplierID == me.SupplierID);

			data.You = parent;
		}	
		
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeProductsCollection()
		{
		    if(this._ProductsCollection != null && this._ProductsCollection.Count > 0)
				return true;
            else
				return false;
		}	
		
		/// <summary>
		/// Zero to Many
		/// Foreign Key Name - FK_Products_Suppliers
		/// </summary>
		

		[DataMember(Name="ProductsCollection", EmitDefaultValue = false)]
		public ProductsCollection ProductsCollection
		{
			get
			{
				if(this._ProductsCollection == null)
				{
					this._ProductsCollection = new ProductsCollection();
					this._ProductsCollection.es.Connection.Name = this.es.Connection.Name;
					this.SetPostSave("ProductsCollection", this._ProductsCollection);
				
					if (this.SupplierID != null)
					{
						if (!this.es.IsLazyLoadDisabled)
						{
							this._ProductsCollection.Query.Where(this._ProductsCollection.Query.SupplierID == this.SupplierID);
							this._ProductsCollection.Query.Load();
						}

						// Auto-hookup Foreign Keys
						this._ProductsCollection.fks.Add(ProductsMetadata.ColumnNames.SupplierID, this.SupplierID);
					}
				}

				return this._ProductsCollection;
			}
			
			set 
			{ 
				if (value != null) throw new Exception("'value' Must be null"); 
			 
				if (this._ProductsCollection != null) 
				{ 
					this.RemovePostSave("ProductsCollection"); 
					this._ProductsCollection = null;
					
				} 
			} 			
		}
		

		
			
		
		private ProductsCollection _ProductsCollection;
		#endregion

		
		protected override esEntityCollectionBase CreateCollectionForPrefetch(string name)
		{
			esEntityCollectionBase coll = null;

			switch (name)
			{
				case "ProductsCollection":
					coll = this.ProductsCollection;
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
			
			props.Add(new esPropertyDescriptor(this, "ProductsCollection", typeof(ProductsCollection), new Products()));
		
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
			if(this._ProductsCollection != null)
			{
				Apply(this._ProductsCollection, "SupplierID", this.SupplierID);
			}
		}
		
	}
	



	[Serializable]
	public partial class SuppliersMetadata : esMetadata, IMetadata
	{
		#region Protected Constructor
		protected SuppliersMetadata()
		{
			m_columns = new esColumnMetadataCollection();
			esColumnMetadata c;

			c = new esColumnMetadata(SuppliersMetadata.ColumnNames.SupplierID, 0, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = SuppliersMetadata.PropertyNames.SupplierID;
			c.IsInPrimaryKey = true;
			c.IsAutoIncrement = true;
			c.NumericPrecision = 10;
			m_columns.Add(c);
				
			c = new esColumnMetadata(SuppliersMetadata.ColumnNames.CompanyName, 1, typeof(System.String), esSystemType.String);
			c.PropertyName = SuppliersMetadata.PropertyNames.CompanyName;
			c.CharacterMaxLength = 40;
			m_columns.Add(c);
				
			c = new esColumnMetadata(SuppliersMetadata.ColumnNames.ContactName, 2, typeof(System.String), esSystemType.String);
			c.PropertyName = SuppliersMetadata.PropertyNames.ContactName;
			c.CharacterMaxLength = 30;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(SuppliersMetadata.ColumnNames.ContactTitle, 3, typeof(System.String), esSystemType.String);
			c.PropertyName = SuppliersMetadata.PropertyNames.ContactTitle;
			c.CharacterMaxLength = 30;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(SuppliersMetadata.ColumnNames.Address, 4, typeof(System.String), esSystemType.String);
			c.PropertyName = SuppliersMetadata.PropertyNames.Address;
			c.CharacterMaxLength = 60;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(SuppliersMetadata.ColumnNames.City, 5, typeof(System.String), esSystemType.String);
			c.PropertyName = SuppliersMetadata.PropertyNames.City;
			c.CharacterMaxLength = 15;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(SuppliersMetadata.ColumnNames.Region, 6, typeof(System.String), esSystemType.String);
			c.PropertyName = SuppliersMetadata.PropertyNames.Region;
			c.CharacterMaxLength = 15;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(SuppliersMetadata.ColumnNames.PostalCode, 7, typeof(System.String), esSystemType.String);
			c.PropertyName = SuppliersMetadata.PropertyNames.PostalCode;
			c.CharacterMaxLength = 10;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(SuppliersMetadata.ColumnNames.Country, 8, typeof(System.String), esSystemType.String);
			c.PropertyName = SuppliersMetadata.PropertyNames.Country;
			c.CharacterMaxLength = 15;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(SuppliersMetadata.ColumnNames.Phone, 9, typeof(System.String), esSystemType.String);
			c.PropertyName = SuppliersMetadata.PropertyNames.Phone;
			c.CharacterMaxLength = 24;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(SuppliersMetadata.ColumnNames.Fax, 10, typeof(System.String), esSystemType.String);
			c.PropertyName = SuppliersMetadata.PropertyNames.Fax;
			c.CharacterMaxLength = 24;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(SuppliersMetadata.ColumnNames.HomePage, 11, typeof(System.String), esSystemType.String);
			c.PropertyName = SuppliersMetadata.PropertyNames.HomePage;
			c.CharacterMaxLength = 1073741823;
			c.IsNullable = true;
			m_columns.Add(c);
				
		}
		#endregion	
	
		static public SuppliersMetadata Meta()
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
			 public const string SupplierID = "SupplierID";
			 public const string CompanyName = "CompanyName";
			 public const string ContactName = "ContactName";
			 public const string ContactTitle = "ContactTitle";
			 public const string Address = "Address";
			 public const string City = "City";
			 public const string Region = "Region";
			 public const string PostalCode = "PostalCode";
			 public const string Country = "Country";
			 public const string Phone = "Phone";
			 public const string Fax = "Fax";
			 public const string HomePage = "HomePage";
		}
		#endregion	
		
		#region PropertyNames
		public class PropertyNames
		{ 
			 public const string SupplierID = "SupplierID";
			 public const string CompanyName = "CompanyName";
			 public const string ContactName = "ContactName";
			 public const string ContactTitle = "ContactTitle";
			 public const string Address = "Address";
			 public const string City = "City";
			 public const string Region = "Region";
			 public const string PostalCode = "PostalCode";
			 public const string Country = "Country";
			 public const string Phone = "Phone";
			 public const string Fax = "Fax";
			 public const string HomePage = "HomePage";
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
			lock (typeof(SuppliersMetadata))
			{
				if(SuppliersMetadata.mapDelegates == null)
				{
					SuppliersMetadata.mapDelegates = new Dictionary<string,MapToMeta>();
				}
				
				if (SuppliersMetadata.meta == null)
				{
					SuppliersMetadata.meta = new SuppliersMetadata();
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


				meta.AddTypeMap("SupplierID", new esTypeMap("int", "System.Int32"));
				meta.AddTypeMap("CompanyName", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("ContactName", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("ContactTitle", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("Address", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("City", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("Region", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("PostalCode", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("Country", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("Phone", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("Fax", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("HomePage", new esTypeMap("ntext", "System.String"));			
				
				
				
				meta.Source = "Suppliers";
				meta.Destination = "Suppliers";
				
				meta.spInsert = "proc_SuppliersInsert";				
				meta.spUpdate = "proc_SuppliersUpdate";		
				meta.spDelete = "proc_SuppliersDelete";
				meta.spLoadAll = "proc_SuppliersLoadAll";
				meta.spLoadByPrimaryKey = "proc_SuppliersLoadByPrimaryKey";
				
				this.m_providerMetadataMaps["esDefault"] = meta;
			}
			
			return this.m_providerMetadataMaps["esDefault"];
		}

		#endregion

		static private SuppliersMetadata meta;
		static protected Dictionary<string, MapToMeta> mapDelegates;
		static private int _esDefault = RegisterDelegateesDefault();
	}
}
