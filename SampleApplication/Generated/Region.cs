
/*
===============================================================================
                    EntitySpaces Studio by EntitySpaces, LLC
             Persistence Layer and Business Objects for Microsoft .NET
             EntitySpaces(TM) is a legal trademark of EntitySpaces, LLC
                          http://www.entityspaces.net
===============================================================================
EntitySpaces Version : 2019.1.0702.0
EntitySpaces Driver  : SQL
Date Generated       : 7/8/2019 9:34:12 AM
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
	/// Encapsulates the 'Region' table
	/// </summary>

    [DebuggerDisplay("Data = {Debug}")]
	[Serializable]
	[DataContract]
	[KnownType(typeof(Region))]	
	[XmlType("Region")]
	public partial class Region : esRegion
	{	
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden | DebuggerBrowsableState.Never)]
		protected override esEntityDebuggerView[] Debug
		{
			get { return base.Debug; }
		}

		override public esEntity CreateInstance()
		{
			return new Region();
		}
		
		#region Static Quick Access Methods
		static public void Delete(System.Int32 regionID)
		{
			var obj = new Region();
			obj.RegionID = regionID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save();
		}

	    static public void Delete(System.Int32 regionID, esSqlAccessType sqlAccessType)
		{
			var obj = new Region();
			obj.RegionID = regionID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save(sqlAccessType);
		}
		#endregion

		
					
		
	
	}



    [DebuggerDisplay("Count = {Count}")]
	[Serializable]
	[CollectionDataContract]
	[XmlType("RegionCollection")]
	public partial class RegionCollection : esRegionCollection, IEnumerable<Region>
	{
		public Region FindByPrimaryKey(System.Int32 regionID)
		{
			return this.SingleOrDefault(e => e.RegionID == regionID);
		}

		
		
		#region WCF Service Class
		
		[DataContract]
		[KnownType(typeof(Region))]
		public class RegionCollectionWCFPacket : esCollectionWCFPacket<RegionCollection>
		{
			public static implicit operator RegionCollection(RegionCollectionWCFPacket packet)
			{
				return packet.Collection;
			}

			public static implicit operator RegionCollectionWCFPacket(RegionCollection collection)
			{
				return new RegionCollectionWCFPacket() { Collection = collection };
			}
		}
		
		#endregion
		
				
	}



    [DebuggerDisplay("Query = {Parse()}")]
	[Serializable]	
	public partial class RegionQuery : esRegionQuery
	{
		public RegionQuery(string joinAlias)
		{
			this.es.JoinAlias = joinAlias;
		}	

		override protected string GetQueryName()
		{
			return "RegionQuery";
		}
		
					
	
		#region Explicit Casts
		
		public static explicit operator string(RegionQuery query)
		{
			return RegionQuery.SerializeHelper.ToXml(query);
		}

		public static explicit operator RegionQuery(string query)
		{
			return (RegionQuery)RegionQuery.SerializeHelper.FromXml(query, typeof(RegionQuery));
		}
		
		#endregion		
	}

	[DataContract]
	[Serializable]
	abstract public partial class esRegion : esEntity
	{
		public esRegion()
		{

		}
		
		#region LoadByPrimaryKey
		public virtual bool LoadByPrimaryKey(System.Int32 regionID)
		{
			if(this.es.Connection.SqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(regionID);
			else
				return LoadByPrimaryKeyStoredProcedure(regionID);
		}

		public virtual bool LoadByPrimaryKey(esSqlAccessType sqlAccessType, System.Int32 regionID)
		{
			if (sqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(regionID);
			else
				return LoadByPrimaryKeyStoredProcedure(regionID);
		}

		private bool LoadByPrimaryKeyDynamic(System.Int32 regionID)
		{
			RegionQuery query = new RegionQuery();
			query.Where(query.RegionID == regionID);
			return this.Load(query);
		}

		private bool LoadByPrimaryKeyStoredProcedure(System.Int32 regionID)
		{
			esParameters parms = new esParameters();
			parms.Add("RegionID", regionID);
			return this.Load(esQueryType.StoredProcedure, this.es.spLoadByPrimaryKey, parms);
		}
		#endregion
		
		#region Properties
		
		
		
		/// <summary>
		/// Maps to Region.RegionID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? RegionID
		{
			get
			{
				return base.GetSystemInt32(RegionMetadata.ColumnNames.RegionID);
			}
			
			set
			{
				if(base.SetSystemInt32(RegionMetadata.ColumnNames.RegionID, value))
				{
					OnPropertyChanged(RegionMetadata.PropertyNames.RegionID);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Region.RegionDescription
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String RegionDescription
		{
			get
			{
				return base.GetSystemString(RegionMetadata.ColumnNames.RegionDescription);
			}
			
			set
			{
				if(base.SetSystemString(RegionMetadata.ColumnNames.RegionDescription, value))
				{
					OnPropertyChanged(RegionMetadata.PropertyNames.RegionDescription);
				}
			}
		}		
		
		#endregion	

		#region .str() Properties
		
		public override void SetProperties(IDictionary values)
		{
			foreach (string propertyName in values.Keys)
			{
				this.SetProperty(propertyName, values[propertyName]);
			}
		}
		
		public override void SetProperty(string name, object value)
		{
			esColumnMetadata col = this.Meta.Columns.FindByPropertyName(name);
			if (col != null)
			{
				if(value == null || value is System.String)
				{				
					// Use the strongly typed property
					switch (name)
					{							
						case "RegionID": this.str().RegionID = (string)value; break;							
						case "RegionDescription": this.str().RegionDescription = (string)value; break;
					}
				}
				else
				{
					switch (name)
					{	
						case "RegionID":
						
							if (value == null || value is System.Int32)
								this.RegionID = (System.Int32?)value;
								OnPropertyChanged(RegionMetadata.PropertyNames.RegionID);
							break;
					

						default:
							break;
					}
				}
			}
            else if (this.ContainsColumn(name))
            {
                this.SetColumn(name, value);
            }
			else
			{
				throw new Exception("SetProperty Error: '" + name + "' not found");
			}
		}		

		public esStrings str()
		{
			if (esstrings == null)
			{
				esstrings = new esStrings(this);
			}
			return esstrings;
		}

		sealed public class esStrings
		{
			public esStrings(esRegion entity)
			{
				this.entity = entity;
			}
			
	
			public System.String RegionID
			{
				get
				{
					System.Int32? data = entity.RegionID;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.RegionID = null;
					else entity.RegionID = Convert.ToInt32(value);
				}
			}
				
			public System.String RegionDescription
			{
				get
				{
					System.String data = entity.RegionDescription;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.RegionDescription = null;
					else entity.RegionDescription = Convert.ToString(value);
				}
			}
			

			private esRegion entity;
		}
		
		[NonSerialized]
		private esStrings esstrings;		
		
		#endregion
		
		#region Housekeeping methods

		override protected IMetadata Meta
		{
			get
			{
				return RegionMetadata.Meta();
			}
		}

		#endregion		
		
		#region Query Logic

		public RegionQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new RegionQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(RegionQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return this.Query.Load();
		}
		
		protected void InitQuery(RegionQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntity)this).Connection;
			}			
		}

		#endregion
		
        [IgnoreDataMember]
		private RegionQuery query;		
	}



	[Serializable]
	abstract public partial class esRegionCollection : esEntityCollection<Region>
	{
		#region Housekeeping methods
		override protected IMetadata Meta
		{
			get
			{
				return RegionMetadata.Meta();
			}
		}

		protected override string GetCollectionName()
		{
			return "RegionCollection";
		}

		#endregion		
		
		#region Query Logic

	#if (!WindowsCE)
		[BrowsableAttribute(false)]
	#endif
		public RegionQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new RegionQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(RegionQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return Query.Load();
		}

		override protected esDynamicQuery GetDynamicQuery()
		{
			if (this.query == null)
			{
				this.query = new RegionQuery();
				this.InitQuery(query);
			}
			return this.query;
		}

		protected void InitQuery(RegionQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntityCollection)this).Connection;
			}			
		}

		protected override void HookupQuery(esDynamicQuery query)
		{
			this.InitQuery((RegionQuery)query);
		}

		#endregion
		
		private RegionQuery query;
	}



	[Serializable]
	abstract public partial class esRegionQuery : esDynamicQuery
	{
		override protected IMetadata Meta
		{
			get
			{
				return RegionMetadata.Meta();
			}
		}	
		
		#region QueryItemFromName
		
        protected override esQueryItem QueryItemFromName(string name)
        {
            switch (name)
            {
				case "RegionID": return this.RegionID;
				case "RegionDescription": return this.RegionDescription;

                default: return null;
            }
        }		
		
		#endregion
		
		#region esQueryItems

		public esQueryItem RegionID
		{
			get { return new esQueryItem(this, RegionMetadata.ColumnNames.RegionID, esSystemType.Int32); }
		} 
		
		public esQueryItem RegionDescription
		{
			get { return new esQueryItem(this, RegionMetadata.ColumnNames.RegionDescription, esSystemType.String); }
		} 
		
		#endregion
		
	}


	
	public partial class Region : esRegion
	{

		#region TerritoriesCollectionByRegionID - Zero To Many
		
		static public esPrefetchMap Prefetch_TerritoriesCollectionByRegionID
		{
			get
			{
				esPrefetchMap map = new esPrefetchMap();
				map.PrefetchDelegate = BusinessObjects.Region.TerritoriesCollectionByRegionID_Delegate;
				map.PropertyName = "TerritoriesCollectionByRegionID";
				map.MyColumnName = "RegionID";
				map.ParentColumnName = "RegionID";
				map.IsMultiPartKey = false;
				return map;
			}
		}		
		
		static private void TerritoriesCollectionByRegionID_Delegate(esPrefetchParameters data)
		{
			RegionQuery parent = new RegionQuery(data.NextAlias());

			TerritoriesQuery me = data.You != null ? data.You as TerritoriesQuery : new TerritoriesQuery(data.NextAlias());

			if (data.Root == null)
			{
				data.Root = me;
			}
			
			data.Root.InnerJoin(parent).On(parent.RegionID == me.RegionID);

			data.You = parent;
		}	
		
		public bool ShouldSerializeTerritoriesCollectionByRegionID()
		{
            if(this._TerritoriesCollectionByRegionID != null && this._TerritoriesCollectionByRegionID.Count > 0)
				return true;
            else
				return false;
		}	
		
		/// <summary>
		/// Zero to Many
		/// Foreign Key Name - FK_Territories_Region
		/// </summary>
		

		[XmlIgnore]
		[DataMember]
		public TerritoriesCollection TerritoriesCollectionByRegionID
		{
			get
			{
				if(this._TerritoriesCollectionByRegionID == null)
				{
					this._TerritoriesCollectionByRegionID = new TerritoriesCollection();
					this._TerritoriesCollectionByRegionID.es.Connection.Name = this.es.Connection.Name;
					this.SetPostSave("TerritoriesCollectionByRegionID", this._TerritoriesCollectionByRegionID);
				
					if (this.RegionID != null)
					{
						if (!this.es.IsLazyLoadDisabled)
						{
							this._TerritoriesCollectionByRegionID.Query.Where(this._TerritoriesCollectionByRegionID.Query.RegionID == this.RegionID);
							this._TerritoriesCollectionByRegionID.Query.Load();
						}

						// Auto-hookup Foreign Keys
						this._TerritoriesCollectionByRegionID.fks.Add(TerritoriesMetadata.ColumnNames.RegionID, this.RegionID);
					}
				}

				return this._TerritoriesCollectionByRegionID;
			}
			
			set 
			{ 
				if (value != null) throw new Exception("'value' Must be null"); 
			 
				if (this._TerritoriesCollectionByRegionID != null) 
				{ 
					this.RemovePostSave("TerritoriesCollectionByRegionID"); 
					this._TerritoriesCollectionByRegionID = null;
					
				} 
			} 			
		}
		

		
			
		
		private TerritoriesCollection _TerritoriesCollectionByRegionID;
		#endregion

		
		protected override esEntityCollectionBase CreateCollectionForPrefetch(string name)
		{
			esEntityCollectionBase coll = null;

			switch (name)
			{
				case "TerritoriesCollectionByRegionID":
					coll = this.TerritoriesCollectionByRegionID;
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
			
			props.Add(new esPropertyDescriptor(this, "TerritoriesCollectionByRegionID", typeof(TerritoriesCollection), new Territories()));
		
			return props;
		}
		
	}
	



	[Serializable]
	public partial class RegionMetadata : esMetadata, IMetadata
	{
		#region Protected Constructor
		protected RegionMetadata()
		{
			m_columns = new esColumnMetadataCollection();
			esColumnMetadata c;

			c = new esColumnMetadata(RegionMetadata.ColumnNames.RegionID, 0, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = RegionMetadata.PropertyNames.RegionID;
			c.IsInPrimaryKey = true;
			c.NumericPrecision = 10;
			m_columns.Add(c);
				
			c = new esColumnMetadata(RegionMetadata.ColumnNames.RegionDescription, 1, typeof(System.String), esSystemType.String);
			c.PropertyName = RegionMetadata.PropertyNames.RegionDescription;
			c.CharacterMaxLength = 50;
			m_columns.Add(c);
				
		}
		#endregion	
	
		static public RegionMetadata Meta()
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
			 public const string RegionID = "RegionID";
			 public const string RegionDescription = "RegionDescription";
		}
		#endregion	
		
		#region PropertyNames
		public class PropertyNames
		{ 
			 public const string RegionID = "RegionID";
			 public const string RegionDescription = "RegionDescription";
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
			lock (typeof(RegionMetadata))
			{
				if(RegionMetadata.mapDelegates == null)
				{
					RegionMetadata.mapDelegates = new Dictionary<string,MapToMeta>();
				}
				
				if (RegionMetadata.meta == null)
				{
					RegionMetadata.meta = new RegionMetadata();
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


				meta.AddTypeMap("RegionID", new esTypeMap("int", "System.Int32"));
				meta.AddTypeMap("RegionDescription", new esTypeMap("nchar", "System.String"));			
				
				
				
				meta.Source = "Region";
				meta.Destination = "Region";
				
				meta.spInsert = "proc_RegionInsert";				
				meta.spUpdate = "proc_RegionUpdate";		
				meta.spDelete = "proc_RegionDelete";
				meta.spLoadAll = "proc_RegionLoadAll";
				meta.spLoadByPrimaryKey = "proc_RegionLoadByPrimaryKey";
				
				this.m_providerMetadataMaps["esDefault"] = meta;
			}
			
			return this.m_providerMetadataMaps["esDefault"];
		}

		#endregion

		static private RegionMetadata meta;
		static protected Dictionary<string, MapToMeta> mapDelegates;
		static private int _esDefault = RegisterDelegateesDefault();
	}
}
