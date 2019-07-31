
/*
===============================================================================
                    EntitySpaces Studio by EntitySpaces, LLC
             Persistence Layer and Business Objects for Microsoft .NET
             EntitySpaces(TM) is a legal trademark of EntitySpaces, LLC
                          http://www.entityspaces.net
===============================================================================
EntitySpaces Version : 2019.1.0725.0
EntitySpaces Driver  : SQL
Date Generated       : 7/31/2019 10:51:48 AM
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
	/// Encapsulates the 'Categories' table
	/// </summary>

    [DebuggerDisplay("Data = {Debug}")]
	[Serializable]
	[DataContract]
	[KnownType(typeof(Categories))]	
	[XmlType("Categories")]
	public partial class Categories : esCategories
	{	
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden | DebuggerBrowsableState.Never)]
		protected override esEntityDebuggerView[] Debug
		{
			get { return base.Debug; }
		}

		override public esEntity CreateInstance()
		{
			return new Categories();
		}
		
		#region Static Quick Access Methods
		static public void Delete(System.Int32 categoryID)
		{
			var obj = new Categories();
			obj.CategoryID = categoryID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save();
		}

	    static public void Delete(System.Int32 categoryID, esSqlAccessType sqlAccessType)
		{
			var obj = new Categories();
			obj.CategoryID = categoryID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save(sqlAccessType);
		}
		#endregion

		
					
		
	
	}



    [DebuggerDisplay("Count = {Count}")]
	[Serializable]
	[CollectionDataContract]
	[XmlType("CategoriesCollection")]
	public partial class CategoriesCollection : esCategoriesCollection, IEnumerable<Categories>
	{
		public Categories FindByPrimaryKey(System.Int32 categoryID)
		{
			return this.SingleOrDefault(e => e.CategoryID == categoryID);
		}

		
				
	}



    [DebuggerDisplay("Query = {Parse()}")]
	[Serializable]	
	public partial class CategoriesQuery : esCategoriesQuery
	{
		public CategoriesQuery(string joinAlias)
		{
			this.es.JoinAlias = joinAlias;
		}	

		override protected string GetQueryName()
		{
			return "CategoriesQuery";
		}
		
					
	
		#region Explicit Casts
		
		public static explicit operator string(CategoriesQuery query)
		{
			return CategoriesQuery.SerializeHelper.ToXml(query);
		}

		public static explicit operator CategoriesQuery(string query)
		{
			return (CategoriesQuery)CategoriesQuery.SerializeHelper.FromXml(query, typeof(CategoriesQuery));
		}
		
		#endregion		
	}

	[DataContract]
	[Serializable]
	abstract public partial class esCategories : esEntity
	{
		public esCategories()
		{

		}
		
		#region LoadByPrimaryKey
		public virtual bool LoadByPrimaryKey(System.Int32 categoryID)
		{
			if(this.es.Connection.SqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(categoryID);
			else
				return LoadByPrimaryKeyStoredProcedure(categoryID);
		}

		public virtual bool LoadByPrimaryKey(esSqlAccessType sqlAccessType, System.Int32 categoryID)
		{
			if (sqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(categoryID);
			else
				return LoadByPrimaryKeyStoredProcedure(categoryID);
		}

		private bool LoadByPrimaryKeyDynamic(System.Int32 categoryID)
		{
			CategoriesQuery query = new CategoriesQuery();
			query.Where(query.CategoryID == categoryID);
			return this.Load(query);
		}

		private bool LoadByPrimaryKeyStoredProcedure(System.Int32 categoryID)
		{
			esParameters parms = new esParameters();
			parms.Add("CategoryID", categoryID);
			return this.Load(esQueryType.StoredProcedure, this.es.spLoadByPrimaryKey, parms);
		}
		#endregion
		
		#region Properties
		
		
		
		/// <summary>
		/// Maps to Categories.CategoryID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? CategoryID
		{
			get
			{
				return base.GetSystemInt32(CategoriesMetadata.ColumnNames.CategoryID);
			}
			
			set
			{
				if(base.SetSystemInt32(CategoriesMetadata.ColumnNames.CategoryID, value))
				{
					OnPropertyChanged(CategoriesMetadata.PropertyNames.CategoryID);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Categories.CategoryName
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String CategoryName
		{
			get
			{
				return base.GetSystemString(CategoriesMetadata.ColumnNames.CategoryName);
			}
			
			set
			{
				if(base.SetSystemString(CategoriesMetadata.ColumnNames.CategoryName, value))
				{
					OnPropertyChanged(CategoriesMetadata.PropertyNames.CategoryName);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Categories.Description
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Description
		{
			get
			{
				return base.GetSystemString(CategoriesMetadata.ColumnNames.Description);
			}
			
			set
			{
				if(base.SetSystemString(CategoriesMetadata.ColumnNames.Description, value))
				{
					OnPropertyChanged(CategoriesMetadata.PropertyNames.Description);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Categories.Picture
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Byte[] Picture
		{
			get
			{
				return base.GetSystemByteArray(CategoriesMetadata.ColumnNames.Picture);
			}
			
			set
			{
				if(base.SetSystemByteArray(CategoriesMetadata.ColumnNames.Picture, value))
				{
					OnPropertyChanged(CategoriesMetadata.PropertyNames.Picture);
				}
			}
		}		
		
		#endregion
		
		#region Housekeeping methods

		override protected IMetadata Meta
		{
			get
			{
				return CategoriesMetadata.Meta();
			}
		}

		#endregion		
		
		#region Query Logic

		public CategoriesQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new CategoriesQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(CategoriesQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return this.Query.Load();
		}
		
		protected void InitQuery(CategoriesQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntity)this).Connection;
			}			
		}

		#endregion
		
        [IgnoreDataMember]
		private CategoriesQuery query;		
	}



	[Serializable]
	abstract public partial class esCategoriesCollection : esEntityCollection<Categories>
	{
		#region Housekeeping methods
		override protected IMetadata Meta
		{
			get
			{
				return CategoriesMetadata.Meta();
			}
		}

		protected override string GetCollectionName()
		{
			return "CategoriesCollection";
		}

		#endregion		
		
		#region Query Logic

	#if (!WindowsCE)
		[BrowsableAttribute(false)]
	#endif
		public CategoriesQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new CategoriesQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(CategoriesQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return Query.Load();
		}

		override protected esDynamicQuery GetDynamicQuery()
		{
			if (this.query == null)
			{
				this.query = new CategoriesQuery();
				this.InitQuery(query);
			}
			return this.query;
		}

		protected void InitQuery(CategoriesQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntityCollection)this).Connection;
			}			
		}

		protected override void HookupQuery(esDynamicQuery query)
		{
			this.InitQuery((CategoriesQuery)query);
		}

		#endregion
		
		private CategoriesQuery query;
	}



	[Serializable]
	abstract public partial class esCategoriesQuery : esDynamicQuery
	{
		override protected IMetadata Meta
		{
			get
			{
				return CategoriesMetadata.Meta();
			}
		}	
		
		#region QueryItemFromName
		
        protected override esQueryItem QueryItemFromName(string name)
        {
            switch (name)
            {
				case "CategoryID": return this.CategoryID;
				case "CategoryName": return this.CategoryName;
				case "Description": return this.Description;
				case "Picture": return this.Picture;

                default: return null;
            }
        }		
		
		#endregion
		
		#region esQueryItems

		public esQueryItem CategoryID
		{
			get { return new esQueryItem(this, CategoriesMetadata.ColumnNames.CategoryID, esSystemType.Int32); }
		} 
		
		public esQueryItem CategoryName
		{
			get { return new esQueryItem(this, CategoriesMetadata.ColumnNames.CategoryName, esSystemType.String); }
		} 
		
		public esQueryItem Description
		{
			get { return new esQueryItem(this, CategoriesMetadata.ColumnNames.Description, esSystemType.String); }
		} 
		
		public esQueryItem Picture
		{
			get { return new esQueryItem(this, CategoriesMetadata.ColumnNames.Picture, esSystemType.ByteArray); }
		} 
		
		#endregion
		
	}


	
	public partial class Categories : esCategories
	{

		#region ProductsCollection - Zero To Many
		
		static public esPrefetchMap Prefetch_ProductsCollection
		{
			get
			{
				esPrefetchMap map = new esPrefetchMap();
				map.PrefetchDelegate = BusinessObjects.Categories.ProductsCollection_Delegate;
				map.PropertyName = "ProductsCollection";
				map.MyColumnName = "CategoryID";
				map.ParentColumnName = "CategoryID";
				map.IsMultiPartKey = false;
				return map;
			}
		}		
		
		static private void ProductsCollection_Delegate(esPrefetchParameters data)
		{
			CategoriesQuery parent = new CategoriesQuery(data.NextAlias());

			ProductsQuery me = data.You != null ? data.You as ProductsQuery : new ProductsQuery(data.NextAlias());

			if (data.Root == null)
			{
				data.Root = me;
			}
			
			data.Root.InnerJoin(parent).On(parent.CategoryID == me.CategoryID);

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
		/// Foreign Key Name - FK_Products_Categories
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
				
					if (this.CategoryID != null)
					{
						if (!this.es.IsLazyLoadDisabled)
						{
							this._ProductsCollection.Query.Where(this._ProductsCollection.Query.CategoryID == this.CategoryID);
							this._ProductsCollection.Query.Load();
						}

						// Auto-hookup Foreign Keys
						this._ProductsCollection.fks.Add(ProductsMetadata.ColumnNames.CategoryID, this.CategoryID);
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
				Apply(this._ProductsCollection, "CategoryID", this.CategoryID);
			}
		}
		
	}
	



	[Serializable]
	public partial class CategoriesMetadata : esMetadata, IMetadata
	{
		#region Protected Constructor
		protected CategoriesMetadata()
		{
			m_columns = new esColumnMetadataCollection();
			esColumnMetadata c;

			c = new esColumnMetadata(CategoriesMetadata.ColumnNames.CategoryID, 0, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = CategoriesMetadata.PropertyNames.CategoryID;
			c.IsInPrimaryKey = true;
			c.IsAutoIncrement = true;
			c.NumericPrecision = 10;
			m_columns.Add(c);
				
			c = new esColumnMetadata(CategoriesMetadata.ColumnNames.CategoryName, 1, typeof(System.String), esSystemType.String);
			c.PropertyName = CategoriesMetadata.PropertyNames.CategoryName;
			c.CharacterMaxLength = 15;
			m_columns.Add(c);
				
			c = new esColumnMetadata(CategoriesMetadata.ColumnNames.Description, 2, typeof(System.String), esSystemType.String);
			c.PropertyName = CategoriesMetadata.PropertyNames.Description;
			c.CharacterMaxLength = 1073741823;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(CategoriesMetadata.ColumnNames.Picture, 3, typeof(System.Byte[]), esSystemType.ByteArray);
			c.PropertyName = CategoriesMetadata.PropertyNames.Picture;
			c.CharacterMaxLength = 2147483647;
			c.IsNullable = true;
			m_columns.Add(c);
				
		}
		#endregion	
	
		static public CategoriesMetadata Meta()
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
			 public const string CategoryID = "CategoryID";
			 public const string CategoryName = "CategoryName";
			 public const string Description = "Description";
			 public const string Picture = "Picture";
		}
		#endregion	
		
		#region PropertyNames
		public class PropertyNames
		{ 
			 public const string CategoryID = "CategoryID";
			 public const string CategoryName = "CategoryName";
			 public const string Description = "Description";
			 public const string Picture = "Picture";
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
			lock (typeof(CategoriesMetadata))
			{
				if(CategoriesMetadata.mapDelegates == null)
				{
					CategoriesMetadata.mapDelegates = new Dictionary<string,MapToMeta>();
				}
				
				if (CategoriesMetadata.meta == null)
				{
					CategoriesMetadata.meta = new CategoriesMetadata();
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


				meta.AddTypeMap("CategoryID", new esTypeMap("int", "System.Int32"));
				meta.AddTypeMap("CategoryName", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("Description", new esTypeMap("ntext", "System.String"));
				meta.AddTypeMap("Picture", new esTypeMap("image", "System.Byte[]"));			
				
				
				
				meta.Source = "Categories";
				meta.Destination = "Categories";
				
				meta.spInsert = "proc_CategoriesInsert";				
				meta.spUpdate = "proc_CategoriesUpdate";		
				meta.spDelete = "proc_CategoriesDelete";
				meta.spLoadAll = "proc_CategoriesLoadAll";
				meta.spLoadByPrimaryKey = "proc_CategoriesLoadByPrimaryKey";
				
				this.m_providerMetadataMaps["esDefault"] = meta;
			}
			
			return this.m_providerMetadataMaps["esDefault"];
		}

		#endregion

		static private CategoriesMetadata meta;
		static protected Dictionary<string, MapToMeta> mapDelegates;
		static private int _esDefault = RegisterDelegateesDefault();
	}
}
