
/*
===============================================================================
                    EntitySpaces Studio by EntitySpaces, LLC
             Persistence Layer and Business Objects for Microsoft .NET
             EntitySpaces(TM) is a legal trademark of EntitySpaces, LLC
                          http://www.entityspaces.net
===============================================================================
EntitySpaces Version : 2019.1.0731.0
EntitySpaces Driver  : SQL
Date Generated       : 8/1/2019 10:19:40 AM
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
	/// Encapsulates the 'EmployeeTerritories' table
	/// </summary>

    [DebuggerDisplay("Data = {Debug}")]
	[Serializable]
	[DataContract]
	[KnownType(typeof(EmployeeTerritories))]	
	[XmlType("EmployeeTerritories")]
	public partial class EmployeeTerritories : esEmployeeTerritories
	{	
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden | DebuggerBrowsableState.Never)]
		protected override esEntityDebuggerView[] Debug
		{
			get { return base.Debug; }
		}

		override public esEntity CreateInstance()
		{
			return new EmployeeTerritories();
		}
		
		#region Static Quick Access Methods
		static public void Delete(System.Int32 employeeID, System.String territoryID)
		{
			var obj = new EmployeeTerritories();
			obj.EmployeeID = employeeID;
			obj.TerritoryID = territoryID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save();
		}

	    static public void Delete(System.Int32 employeeID, System.String territoryID, esSqlAccessType sqlAccessType)
		{
			var obj = new EmployeeTerritories();
			obj.EmployeeID = employeeID;
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
	[XmlType("EmployeeTerritoriesCollection")]
	public partial class EmployeeTerritoriesCollection : esEmployeeTerritoriesCollection, IEnumerable<EmployeeTerritories>
	{
		public EmployeeTerritories FindByPrimaryKey(System.Int32 employeeID, System.String territoryID)
		{
			return this.SingleOrDefault(e => e.EmployeeID == employeeID && e.TerritoryID == territoryID);
		}

		
				
	}



    [DebuggerDisplay("Query = {Parse()}")]
	[Serializable]	
	public partial class EmployeeTerritoriesQuery : esEmployeeTerritoriesQuery
	{
		public EmployeeTerritoriesQuery(string joinAlias)
		{
			this.es.JoinAlias = joinAlias;
		}	

		override protected string GetQueryName()
		{
			return "EmployeeTerritoriesQuery";
		}
		
					
	
		#region Explicit Casts
		
		public static explicit operator string(EmployeeTerritoriesQuery query)
		{
			return EmployeeTerritoriesQuery.SerializeHelper.ToXml(query);
		}

		public static explicit operator EmployeeTerritoriesQuery(string query)
		{
			return (EmployeeTerritoriesQuery)EmployeeTerritoriesQuery.SerializeHelper.FromXml(query, typeof(EmployeeTerritoriesQuery));
		}
		
		#endregion		
	}

	[DataContract]
	[Serializable]
	abstract public partial class esEmployeeTerritories : esEntity
	{
		public esEmployeeTerritories()
		{

		}
		
		#region LoadByPrimaryKey
		public virtual bool LoadByPrimaryKey(System.Int32 employeeID, System.String territoryID)
		{
			if(this.es.Connection.SqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(employeeID, territoryID);
			else
				return LoadByPrimaryKeyStoredProcedure(employeeID, territoryID);
		}

		public virtual bool LoadByPrimaryKey(esSqlAccessType sqlAccessType, System.Int32 employeeID, System.String territoryID)
		{
			if (sqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(employeeID, territoryID);
			else
				return LoadByPrimaryKeyStoredProcedure(employeeID, territoryID);
		}

		private bool LoadByPrimaryKeyDynamic(System.Int32 employeeID, System.String territoryID)
		{
			EmployeeTerritoriesQuery query = new EmployeeTerritoriesQuery();
			query.Where(query.EmployeeID == employeeID, query.TerritoryID == territoryID);
			return this.Load(query);
		}

		private bool LoadByPrimaryKeyStoredProcedure(System.Int32 employeeID, System.String territoryID)
		{
			esParameters parms = new esParameters();
			parms.Add("EmployeeID", employeeID);			parms.Add("TerritoryID", territoryID);
			return this.Load(esQueryType.StoredProcedure, this.es.spLoadByPrimaryKey, parms);
		}
		#endregion
		
		#region Properties
		
		
		
		/// <summary>
		/// Maps to EmployeeTerritories.EmployeeID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? EmployeeID
		{
			get
			{
				return base.GetSystemInt32(EmployeeTerritoriesMetadata.ColumnNames.EmployeeID);
			}
			
			set
			{
				if(base.SetSystemInt32(EmployeeTerritoriesMetadata.ColumnNames.EmployeeID, value))
				{
					this._UpToEmployees = null;
					this.OnPropertyChanged("UpToEmployees");
					OnPropertyChanged(EmployeeTerritoriesMetadata.PropertyNames.EmployeeID);
				}
			}
		}		
		
		/// <summary>
		/// Maps to EmployeeTerritories.TerritoryID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String TerritoryID
		{
			get
			{
				return base.GetSystemString(EmployeeTerritoriesMetadata.ColumnNames.TerritoryID);
			}
			
			set
			{
				if(base.SetSystemString(EmployeeTerritoriesMetadata.ColumnNames.TerritoryID, value))
				{
					this._UpToTerritories = null;
					this.OnPropertyChanged("UpToTerritories");
					OnPropertyChanged(EmployeeTerritoriesMetadata.PropertyNames.TerritoryID);
				}
			}
		}		
		
		[CLSCompliant(false)]
		internal protected Employees _UpToEmployees;
		[CLSCompliant(false)]
		internal protected Territories _UpToTerritories;
		#endregion
		
		#region Housekeeping methods

		override protected IMetadata Meta
		{
			get
			{
				return EmployeeTerritoriesMetadata.Meta();
			}
		}

		#endregion		
		
		#region Query Logic

		public EmployeeTerritoriesQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new EmployeeTerritoriesQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(EmployeeTerritoriesQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return this.Query.Load();
		}
		
		protected void InitQuery(EmployeeTerritoriesQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntity)this).Connection;
			}			
		}

		#endregion
		
        [IgnoreDataMember]
		private EmployeeTerritoriesQuery query;		
	}



	[Serializable]
	abstract public partial class esEmployeeTerritoriesCollection : esEntityCollection<EmployeeTerritories>
	{
		#region Housekeeping methods
		override protected IMetadata Meta
		{
			get
			{
				return EmployeeTerritoriesMetadata.Meta();
			}
		}

		protected override string GetCollectionName()
		{
			return "EmployeeTerritoriesCollection";
		}

		#endregion		
		
		#region Query Logic

	#if (!WindowsCE)
		[BrowsableAttribute(false)]
	#endif
		public EmployeeTerritoriesQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new EmployeeTerritoriesQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(EmployeeTerritoriesQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return Query.Load();
		}

		override protected esDynamicQuery GetDynamicQuery()
		{
			if (this.query == null)
			{
				this.query = new EmployeeTerritoriesQuery();
				this.InitQuery(query);
			}
			return this.query;
		}

		protected void InitQuery(EmployeeTerritoriesQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntityCollection)this).Connection;
			}			
		}

		protected override void HookupQuery(esDynamicQuery query)
		{
			this.InitQuery((EmployeeTerritoriesQuery)query);
		}

		#endregion
		
		private EmployeeTerritoriesQuery query;
	}



	[Serializable]
	abstract public partial class esEmployeeTerritoriesQuery : esDynamicQuery
	{
		override protected IMetadata Meta
		{
			get
			{
				return EmployeeTerritoriesMetadata.Meta();
			}
		}	
		
		#region QueryItemFromName
		
        protected override esQueryItem QueryItemFromName(string name)
        {
            switch (name)
            {
				case "EmployeeID": return this.EmployeeID;
				case "TerritoryID": return this.TerritoryID;

                default: return null;
            }
        }		
		
		#endregion
		
		#region esQueryItems

		public esQueryItem EmployeeID
		{
			get { return new esQueryItem(this, EmployeeTerritoriesMetadata.ColumnNames.EmployeeID, esSystemType.Int32); }
		} 
		
		public esQueryItem TerritoryID
		{
			get { return new esQueryItem(this, EmployeeTerritoriesMetadata.ColumnNames.TerritoryID, esSystemType.String); }
		} 
		
		#endregion
		
	}


	
	public partial class EmployeeTerritories : esEmployeeTerritories
	{

				
				
		#region UpToEmployees - Many To One
		/// <summary>
		/// Many to One
		/// Foreign Key Name - FK_EmployeeTerritories_Employees
		/// </summary>
	    [EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeUpToEmployees()
		{
		    if(this._UpToEmployees != null)
				return true;
            else
				return false;
		}
		

		[DataMember(Name="UpToEmployees", EmitDefaultValue = false)]
					
		public Employees UpToEmployees
		{
			get
			{
				if (this.es.IsLazyLoadDisabled) return null;
				
				if(this._UpToEmployees == null && EmployeeID != null)
				{
					this._UpToEmployees = new Employees();
					this._UpToEmployees.es.Connection.Name = this.es.Connection.Name;
					this.SetPreSave("UpToEmployees", this._UpToEmployees);
					this._UpToEmployees.Query.Where(this._UpToEmployees.Query.EmployeeID == this.EmployeeID);
					this._UpToEmployees.Query.Load();
				}	
				return this._UpToEmployees;
			}
			
			set
			{
				this.RemovePreSave("UpToEmployees");
				

				if(value == null)
				{
					this.EmployeeID = null;
					this._UpToEmployees = null;
				}
				else
				{
					this.EmployeeID = value.EmployeeID;
					this._UpToEmployees = value;
					this.SetPreSave("UpToEmployees", this._UpToEmployees);
				}
				
			}
		}
		#endregion
		

				
				
		#region UpToTerritories - Many To One
		/// <summary>
		/// Many to One
		/// Foreign Key Name - FK_EmployeeTerritories_Territories
		/// </summary>
	    [EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeUpToTerritories()
		{
		    if(this._UpToTerritories != null)
				return true;
            else
				return false;
		}
		

		[DataMember(Name="UpToTerritories", EmitDefaultValue = false)]
					
		public Territories UpToTerritories
		{
			get
			{
				if (this.es.IsLazyLoadDisabled) return null;
				
				if(this._UpToTerritories == null && TerritoryID != null)
				{
					this._UpToTerritories = new Territories();
					this._UpToTerritories.es.Connection.Name = this.es.Connection.Name;
					this.SetPreSave("UpToTerritories", this._UpToTerritories);
					this._UpToTerritories.Query.Where(this._UpToTerritories.Query.TerritoryID == this.TerritoryID);
					this._UpToTerritories.Query.Load();
				}	
				return this._UpToTerritories;
			}
			
			set
			{
				this.RemovePreSave("UpToTerritories");
				

				if(value == null)
				{
					this.TerritoryID = null;
					this._UpToTerritories = null;
				}
				else
				{
					this.TerritoryID = value.TerritoryID;
					this._UpToTerritories = value;
					this.SetPreSave("UpToTerritories", this._UpToTerritories);
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
			if(!this.es.IsDeleted && this._UpToEmployees != null)
			{
				this.EmployeeID = this._UpToEmployees.EmployeeID;
			}
		}
		
	}
	



	[Serializable]
	public partial class EmployeeTerritoriesMetadata : esMetadata, IMetadata
	{
		#region Protected Constructor
		protected EmployeeTerritoriesMetadata()
		{
			m_columns = new esColumnMetadataCollection();
			esColumnMetadata c;

			c = new esColumnMetadata(EmployeeTerritoriesMetadata.ColumnNames.EmployeeID, 0, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = EmployeeTerritoriesMetadata.PropertyNames.EmployeeID;
			c.IsInPrimaryKey = true;
			c.NumericPrecision = 10;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeeTerritoriesMetadata.ColumnNames.TerritoryID, 1, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeeTerritoriesMetadata.PropertyNames.TerritoryID;
			c.IsInPrimaryKey = true;
			c.CharacterMaxLength = 20;
			m_columns.Add(c);
				
		}
		#endregion	
	
		static public EmployeeTerritoriesMetadata Meta()
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
			 public const string EmployeeID = "EmployeeID";
			 public const string TerritoryID = "TerritoryID";
		}
		#endregion	
		
		#region PropertyNames
		public class PropertyNames
		{ 
			 public const string EmployeeID = "EmployeeID";
			 public const string TerritoryID = "TerritoryID";
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
			lock (typeof(EmployeeTerritoriesMetadata))
			{
				if(EmployeeTerritoriesMetadata.mapDelegates == null)
				{
					EmployeeTerritoriesMetadata.mapDelegates = new Dictionary<string,MapToMeta>();
				}
				
				if (EmployeeTerritoriesMetadata.meta == null)
				{
					EmployeeTerritoriesMetadata.meta = new EmployeeTerritoriesMetadata();
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


				meta.AddTypeMap("EmployeeID", new esTypeMap("int", "System.Int32"));
				meta.AddTypeMap("TerritoryID", new esTypeMap("nvarchar", "System.String"));			
				
				
				
				meta.Source = "EmployeeTerritories";
				meta.Destination = "EmployeeTerritories";
				
				meta.spInsert = "proc_EmployeeTerritoriesInsert";				
				meta.spUpdate = "proc_EmployeeTerritoriesUpdate";		
				meta.spDelete = "proc_EmployeeTerritoriesDelete";
				meta.spLoadAll = "proc_EmployeeTerritoriesLoadAll";
				meta.spLoadByPrimaryKey = "proc_EmployeeTerritoriesLoadByPrimaryKey";
				
				this.m_providerMetadataMaps["esDefault"] = meta;
			}
			
			return this.m_providerMetadataMaps["esDefault"];
		}

		#endregion

		static private EmployeeTerritoriesMetadata meta;
		static protected Dictionary<string, MapToMeta> mapDelegates;
		static private int _esDefault = RegisterDelegateesDefault();
	}
}
