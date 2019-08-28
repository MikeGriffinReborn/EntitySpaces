
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
	/// Encapsulates the 'Territories' table
	/// </summary>

    [DebuggerDisplay("Data = {Debug}")]
	[Serializable]
	[DataContract]
	[KnownType(typeof(Territories))]	
	[XmlType("Territories")]
	public partial class Territories : esTerritories
	{	
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden | DebuggerBrowsableState.Never)]
		protected override esEntityDebuggerView[] Debug
		{
			get { return base.Debug; }
		}

		override public esEntity CreateInstance()
		{
			return new Territories();
		}
		
		#region Static Quick Access Methods
		static public void Delete(System.String territoryID)
		{
			var obj = new Territories();
			obj.TerritoryID = territoryID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save();
		}

	    static public void Delete(System.String territoryID, esSqlAccessType sqlAccessType)
		{
			var obj = new Territories();
			obj.TerritoryID = territoryID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save(sqlAccessType);
		}
		#endregion

		
					
		
	
	}



    [DebuggerDisplay("Count = {Count}")]
	[Serializable]
	[CollectionDataContract]
	[XmlType("TerritoriesCollection")]
	public partial class TerritoriesCollection : esTerritoriesCollection, IEnumerable<Territories>
	{
		public Territories FindByPrimaryKey(System.String territoryID)
		{
			return this.SingleOrDefault(e => e.TerritoryID == territoryID);
		}

		
				
	}



    [DebuggerDisplay("Query = {Parse()}")]
	[Serializable]	
	public partial class TerritoriesQuery : esTerritoriesQuery
	{
		public TerritoriesQuery(string joinAlias)
		{
			this.es.JoinAlias(joinAlias);
		}	

		override protected string GetQueryName()
		{
			return "TerritoriesQuery";
		}
		
					
	
		#region Explicit Casts
		
		public static explicit operator string(TerritoriesQuery query)
		{
			return TerritoriesQuery.SerializeHelper.ToXml(query);
		}

		public static explicit operator TerritoriesQuery(string query)
		{
			return (TerritoriesQuery)TerritoriesQuery.SerializeHelper.FromXml(query, typeof(TerritoriesQuery));
		}
		
		#endregion		
	}

	[DataContract]
	[Serializable]
	abstract public partial class esTerritories : esEntity
	{
		public esTerritories()
		{

		}
		
		#region LoadByPrimaryKey
		public virtual bool LoadByPrimaryKey(System.String territoryID)
		{
			if(this.es.Connection.SqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(territoryID);
			else
				return LoadByPrimaryKeyStoredProcedure(territoryID);
		}

		public virtual bool LoadByPrimaryKey(esSqlAccessType sqlAccessType, System.String territoryID)
		{
			if (sqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(territoryID);
			else
				return LoadByPrimaryKeyStoredProcedure(territoryID);
		}

		private bool LoadByPrimaryKeyDynamic(System.String territoryID)
		{
			TerritoriesQuery query = new TerritoriesQuery();
			query.Where(query.TerritoryID == territoryID);
			return this.Load(query);
		}

		private bool LoadByPrimaryKeyStoredProcedure(System.String territoryID)
		{
			esParameters parms = new esParameters();
			parms.Add("TerritoryID", territoryID);
			return this.Load(esQueryType.StoredProcedure, this.es.spLoadByPrimaryKey, parms);
		}
		#endregion
		
		#region Properties
		
		
		
		/// <summary>
		/// Maps to Territories.TerritoryID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String TerritoryID
		{
			get
			{
				return base.GetSystemString(TerritoriesMetadata.ColumnNames.TerritoryID);
			}
			
			set
			{
				if(base.SetSystemString(TerritoriesMetadata.ColumnNames.TerritoryID, value))
				{
					OnPropertyChanged(TerritoriesMetadata.PropertyNames.TerritoryID);
				}
			}
		}
		
		/// <summary>
		/// Maps to Territories.TerritoryDescription
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String TerritoryDescription
		{
			get
			{
				return base.GetSystemString(TerritoriesMetadata.ColumnNames.TerritoryDescription);
			}
			
			set
			{
				if(base.SetSystemString(TerritoriesMetadata.ColumnNames.TerritoryDescription, value))
				{
					OnPropertyChanged(TerritoriesMetadata.PropertyNames.TerritoryDescription);
				}
			}
		}
		
		/// <summary>
		/// Maps to Territories.RegionID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? RegionID
		{
			get
			{
				return base.GetSystemInt32(TerritoriesMetadata.ColumnNames.RegionID);
			}
			
			set
			{
				if(base.SetSystemInt32(TerritoriesMetadata.ColumnNames.RegionID, value))
				{
					this._Region = null;
					this.OnPropertyChanged("Region");
					OnPropertyChanged(TerritoriesMetadata.PropertyNames.RegionID);
				}
			}
		}
		
		internal protected Region _Region;
		#endregion
		
		#region Housekeeping methods

		override protected IMetadata Meta
		{
			get
			{
				return TerritoriesMetadata.Meta();
			}
		}

		#endregion		
		
		#region Query Logic

		public TerritoriesQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new TerritoriesQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(TerritoriesQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return this.Query.Load();
		}
		
		protected void InitQuery(TerritoriesQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntity)this).Connection;
			}			
		}

		#endregion
		
        [IgnoreDataMember]
		private TerritoriesQuery query;		
	}



	[Serializable]
	abstract public partial class esTerritoriesCollection : esEntityCollection<Territories>
	{
		#region Housekeeping methods
		override protected IMetadata Meta
		{
			get
			{
				return TerritoriesMetadata.Meta();
			}
		}

		protected override string GetCollectionName()
		{
			return "TerritoriesCollection";
		}

		#endregion		
		
		#region Query Logic

	#if (!WindowsCE)
		[BrowsableAttribute(false)]
	#endif
		public TerritoriesQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new TerritoriesQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(TerritoriesQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return Query.Load();
		}

		override protected esDynamicQuery GetDynamicQuery()
		{
			if (this.query == null)
			{
				this.query = new TerritoriesQuery();
				this.InitQuery(query);
			}
			return this.query;
		}

		protected void InitQuery(TerritoriesQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntityCollection)this).Connection;
			}			
		}

		protected override void HookupQuery(esDynamicQuery query)
		{
			this.InitQuery((TerritoriesQuery)query);
		}

		#endregion
		
		private TerritoriesQuery query;
	}



	[Serializable]
	abstract public partial class esTerritoriesQuery : esDynamicQuery
	{
		override protected IMetadata Meta
		{
			get
			{
				return TerritoriesMetadata.Meta();
			}
		}	
		
		#region QueryItemFromName
		
        protected override esQueryItem QueryItemFromName(string name)
        {
            switch (name)
            {
				case "TerritoryID": return this.TerritoryID;
				case "TerritoryDescription": return this.TerritoryDescription;
				case "RegionID": return this.RegionID;

                default: return null;
            }
        }		
		
		#endregion
		
		#region esQueryItems

		public esQueryItem TerritoryID
		{
			get { return new esQueryItem(this, TerritoriesMetadata.ColumnNames.TerritoryID, esSystemType.String); }
		} 
		
		public esQueryItem TerritoryDescription
		{
			get { return new esQueryItem(this, TerritoriesMetadata.ColumnNames.TerritoryDescription, esSystemType.String); }
		} 
		
		public esQueryItem RegionID
		{
			get { return new esQueryItem(this, TerritoriesMetadata.ColumnNames.RegionID, esSystemType.Int32); }
		} 
		
		#endregion
		
	}


	
	public partial class Territories : esTerritories
	{

			
		#region EmployeesCollection - Many To Many (FK_EmployeeTerritories_Territories)
		
	    [EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeEmployeesCollection()
		{
		    if(this._EmployeesCollection != null && this._EmployeesCollection.Count > 0)
				return true;
            else
				return false;
		}
		

		[DataMember(Name="EmployeesCollection", EmitDefaultValue = false)]
		public EmployeesCollection EmployeesCollection
		{
			get
			{
				if(this._EmployeesCollection == null)
				{
					this._EmployeesCollection = new EmployeesCollection();
					this._EmployeesCollection.es.Connection.Name = this.es.Connection.Name;
					this.SetPostSave("EmployeesCollection", this._EmployeesCollection);
					if (!this.es.IsLazyLoadDisabled && this.TerritoryID != null)
					{
						EmployeesQuery m = new EmployeesQuery("m");
						EmployeeTerritoriesQuery j = new EmployeeTerritoriesQuery("j");
						m.Select(m);
						m.InnerJoin(j).On(m.EmployeeID == j.EmployeeID);
                        m.Where(j.TerritoryID == this.TerritoryID);

						this._EmployeesCollection.Load(m);
					}
				}

				return this._EmployeesCollection;
			}
			
			set 
			{ 
				if (value != null) throw new Exception("'value' Must be null"); 
			 
				if (this._EmployeesCollection != null) 
				{ 
					this.RemovePostSave("EmployeesCollection"); 
					this._EmployeesCollection = null;
					
				} 
			}  			
		}

		/// <summary>
		/// Many to Many Associate
		/// Foreign Key Name - FK_EmployeeTerritories_Territories
		/// </summary>
		public void ASsociateEmployeeTerritoriesCollection(Employees entity)
		{
			if (this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection == null)
			{
				this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection = new EmployeeTerritoriesCollection();
				this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection.es.Connection.Name = this.es.Connection.Name;
				this.SetPostSave("ManyEntitySpacesMetadataEngineSqlSqlTableCollection", this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection);
			}

			EmployeeTerritories obj = this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection.AddNew();
			obj.TerritoryID = this.TerritoryID;
			obj.EmployeeID = entity.EmployeeID;
		}

		/// <summary>
		/// Many to Many Dissociate
		/// Foreign Key Name - FK_EmployeeTerritories_Territories
		/// </summary>
		public void DiSsociateEmployeeTerritoriesCollection(Employees entity)
		{
			if (this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection == null)
			{
				this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection = new EmployeeTerritoriesCollection();
				this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection.es.Connection.Name = this.es.Connection.Name;
				this.SetPostSave("ManyEntitySpacesMetadataEngineSqlSqlTableCollection", this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection);
			}

			EmployeeTerritories obj = this._ManyEntitySpacesMetadataEngineSqlSqlTableCollection.AddNew();
			obj.TerritoryID = this.TerritoryID;
            obj.EmployeeID = entity.EmployeeID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
		}

		private EmployeesCollection _EmployeesCollection;
		private EmployeeTerritoriesCollection _ManyEntitySpacesMetadataEngineSqlSqlTableCollection;
		#endregion

		#region EmployeeTerritoriesCollection - Zero To Many (FK_EmployeeTerritories_Territories)
		
		static public esPrefetchMap Prefetch_EmployeeTerritoriesCollection
		{
			get
			{
				esPrefetchMap map = new esPrefetchMap
				{
					PrefetchDelegate = BusinessObjects.Territories.EmployeeTerritoriesCollection_Delegate,
					PropertyName = "EmployeeTerritoriesCollection",
					MyColumnName = "TerritoryID",
					ParentColumnName = "TerritoryID",
					IsMultiPartKey = false
				};
				return map;
			}
		}		
		
		static private void EmployeeTerritoriesCollection_Delegate(esPrefetchParameters data)
		{
			TerritoriesQuery parent = new TerritoriesQuery(data.NextAlias());

			EmployeeTerritoriesQuery me = data.You != null ? data.You as EmployeeTerritoriesQuery : new EmployeeTerritoriesQuery(data.NextAlias());

			if (data.Root == null)
			{
				data.Root = me;
			}
			
			data.Root.InnerJoin(parent).On(parent.TerritoryID == me.TerritoryID);

			data.You = parent;
		}	
	
		/// <summary>
		/// Zero to Many
		/// Foreign Key Name - FK_EmployeeTerritories_Territories
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeEmployeeTerritoriesCollection()
		{
		    if(this._EmployeeTerritoriesCollection != null && this._EmployeeTerritoriesCollection.Count > 0)
				return true;
            else
				return false;
		}	
		

		[DataMember(Name="EmployeeTerritoriesCollection", EmitDefaultValue = false)]
		public EmployeeTerritoriesCollection EmployeeTerritoriesCollection
		{
			get
			{
				if(this._EmployeeTerritoriesCollection == null)
				{
					this._EmployeeTerritoriesCollection = new EmployeeTerritoriesCollection();
					this._EmployeeTerritoriesCollection.es.Connection.Name = this.es.Connection.Name;
					this.SetPostSave("EmployeeTerritoriesCollection", this._EmployeeTerritoriesCollection);
				
					if (this.TerritoryID != null)
					{
						if (!this.es.IsLazyLoadDisabled)
						{
							this._EmployeeTerritoriesCollection.Query.Where(this._EmployeeTerritoriesCollection.Query.TerritoryID == this.TerritoryID);
							this._EmployeeTerritoriesCollection.Query.Load();
						}

						// Auto-hookup Foreign Keys
						this._EmployeeTerritoriesCollection.fks.Add(EmployeeTerritoriesMetadata.ColumnNames.TerritoryID, this.TerritoryID);
					}
				}

				return this._EmployeeTerritoriesCollection;
			}
			
			set 
			{ 
				if (value != null) throw new Exception("'value' Must be null"); 
			 
				if (this._EmployeeTerritoriesCollection != null) 
				{ 
					this.RemovePostSave("EmployeeTerritoriesCollection"); 
					this._EmployeeTerritoriesCollection = null;
					
				} 
			} 			
		}
		

		
			
		
		private EmployeeTerritoriesCollection _EmployeeTerritoriesCollection;
		#endregion

		
		#region Region - Many To One (FK_Territories_Region)
		
	    [EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeRegion()
		{
		    return this._Region != null ? true : false;
		}
		

		[DataMember(Name="Region", EmitDefaultValue = false)]
					
		public Region Region
		{
			get
			{
                if (this._Region == null)
                {
                    this._Region = new Region();
                    this._Region.es.Connection.Name = this.es.Connection.Name;
                    this.SetPreSave("Region", this._Region);

					if(this._Region == null && RegionID != null)
                    {
                        if (!this.es.IsLazyLoadDisabled)
                        {
							this._Region.Query.Where(this._Region.Query.RegionID == this.RegionID);
							this._Region.Query.Load();
                        }
                    }
                }

				return this._Region;
			}
			
			set
			{
				this.RemovePreSave("Region");
				

				if(value == null)
				{
					this.RegionID = null;
					this._Region = null;
				}
				else
				{
					this.RegionID = value.RegionID;
					this._Region = value;
					this.SetPreSave("Region", this._Region);
				}
				
			}
		}
		#endregion
		

		
		protected override esEntityCollectionBase CreateCollectionForPrefetch(string name)
		{
			esEntityCollectionBase coll = null;

			switch (name)
			{
				case "EmployeeTerritoriesCollection":
					coll = this.EmployeeTerritoriesCollection;
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
			
			props.Add(new esPropertyDescriptor(this, "EmployeeTerritoriesCollection", typeof(EmployeeTerritoriesCollection), new EmployeeTerritories()));
		
			return props;
		}
		
	}
	



	[Serializable]
	public partial class TerritoriesMetadata : esMetadata, IMetadata
	{
		#region Protected Constructor
		protected TerritoriesMetadata()
		{
			m_columns = new esColumnMetadataCollection();
			esColumnMetadata c;

			c = new esColumnMetadata(TerritoriesMetadata.ColumnNames.TerritoryID, 0, typeof(System.String), esSystemType.String);
			c.PropertyName = TerritoriesMetadata.PropertyNames.TerritoryID;
			c.IsInPrimaryKey = true;
			c.CharacterMaxLength = 20;
			m_columns.Add(c);
				
			c = new esColumnMetadata(TerritoriesMetadata.ColumnNames.TerritoryDescription, 1, typeof(System.String), esSystemType.String);
			c.PropertyName = TerritoriesMetadata.PropertyNames.TerritoryDescription;
			c.CharacterMaxLength = 50;
			m_columns.Add(c);
				
			c = new esColumnMetadata(TerritoriesMetadata.ColumnNames.RegionID, 2, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = TerritoriesMetadata.PropertyNames.RegionID;
			c.NumericPrecision = 10;
			m_columns.Add(c);
				
		}
		#endregion	
	
		static public TerritoriesMetadata Meta()
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
			 public const string TerritoryID = "TerritoryID";
			 public const string TerritoryDescription = "TerritoryDescription";
			 public const string RegionID = "RegionID";
		}
		#endregion	
		
		#region PropertyNames
		public class PropertyNames
		{ 
			 public const string TerritoryID = "TerritoryID";
			 public const string TerritoryDescription = "TerritoryDescription";
			 public const string RegionID = "RegionID";
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
			lock (typeof(TerritoriesMetadata))
			{
				if(TerritoriesMetadata.mapDelegates == null)
				{
					TerritoriesMetadata.mapDelegates = new Dictionary<string,MapToMeta>();
				}
				
				if (TerritoriesMetadata.meta == null)
				{
					TerritoriesMetadata.meta = new TerritoriesMetadata();
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


				meta.AddTypeMap("TerritoryID", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("TerritoryDescription", new esTypeMap("nchar", "System.String"));
				meta.AddTypeMap("RegionID", new esTypeMap("int", "System.Int32"));			
				
				
				
				meta.Source = "Territories";
				meta.Destination = "Territories";
				
				meta.spInsert = "proc_TerritoriesInsert";				
				meta.spUpdate = "proc_TerritoriesUpdate";		
				meta.spDelete = "proc_TerritoriesDelete";
				meta.spLoadAll = "proc_TerritoriesLoadAll";
				meta.spLoadByPrimaryKey = "proc_TerritoriesLoadByPrimaryKey";
				
				this.m_providerMetadataMaps["esDefault"] = meta;
			}
			
			return this.m_providerMetadataMaps["esDefault"];
		}

		#endregion

		static private TerritoriesMetadata meta;
		static protected Dictionary<string, MapToMeta> mapDelegates;
		static private int _esDefault = RegisterDelegateesDefault();
	}
}
