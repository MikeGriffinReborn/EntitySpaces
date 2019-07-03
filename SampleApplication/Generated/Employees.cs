
/*
===============================================================================
                    EntitySpaces Studio by EntitySpaces, LLC
             Persistence Layer and Business Objects for Microsoft .NET
             EntitySpaces(TM) is a legal trademark of EntitySpaces, LLC
                          http://www.entityspaces.net
===============================================================================
EntitySpaces Version : 2019.1.0702.0
EntitySpaces Driver  : SQL
Date Generated       : 7/3/2019 2:00:42 PM
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
	/// Encapsulates the 'Employees' table
	/// </summary>

    [DebuggerDisplay("Data = {Debug}")]
	[Serializable]
	[DataContract]
	[KnownType(typeof(Employees))]	
	[XmlType("Employees")]
	public partial class Employees : esEmployees
	{	
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden | DebuggerBrowsableState.Never)]
		protected override esEntityDebuggerView[] Debug
		{
			get { return base.Debug; }
		}

		override public esEntity CreateInstance()
		{
			return new Employees();
		}
		
		#region Static Quick Access Methods
		static public void Delete(System.Int32 employeeID)
		{
			var obj = new Employees();
			obj.EmployeeID = employeeID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save();
		}

	    static public void Delete(System.Int32 employeeID, esSqlAccessType sqlAccessType)
		{
			var obj = new Employees();
			obj.EmployeeID = employeeID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
			obj.Save(sqlAccessType);
		}
		#endregion

		
					
		
	
	}



    [DebuggerDisplay("Count = {Count}")]
	[Serializable]
	[CollectionDataContract]
	[XmlType("EmployeesCollection")]
	public partial class EmployeesCollection : esEmployeesCollection, IEnumerable<Employees>
	{
		public Employees FindByPrimaryKey(System.Int32 employeeID)
		{
			return this.SingleOrDefault(e => e.EmployeeID == employeeID);
		}

		
		
		#region WCF Service Class
		
		[DataContract]
		[KnownType(typeof(Employees))]
		public class EmployeesCollectionWCFPacket : esCollectionWCFPacket<EmployeesCollection>
		{
			public static implicit operator EmployeesCollection(EmployeesCollectionWCFPacket packet)
			{
				return packet.Collection;
			}

			public static implicit operator EmployeesCollectionWCFPacket(EmployeesCollection collection)
			{
				return new EmployeesCollectionWCFPacket() { Collection = collection };
			}
		}
		
		#endregion
		
				
	}



    [DebuggerDisplay("Query = {Parse()}")]
	[Serializable]	
	public partial class EmployeesQuery : esEmployeesQuery
	{
		public EmployeesQuery(string joinAlias)
		{
			this.es.JoinAlias = joinAlias;
		}	

		override protected string GetQueryName()
		{
			return "EmployeesQuery";
		}
		
					
	
		#region Explicit Casts
		
		public static explicit operator string(EmployeesQuery query)
		{
			return EmployeesQuery.SerializeHelper.ToXml(query);
		}

		public static explicit operator EmployeesQuery(string query)
		{
			return (EmployeesQuery)EmployeesQuery.SerializeHelper.FromXml(query, typeof(EmployeesQuery));
		}
		
		#endregion		
	}

	[DataContract]
	[Serializable]
	abstract public partial class esEmployees : esEntity
	{
		public esEmployees()
		{

		}
		
		#region LoadByPrimaryKey
		public virtual bool LoadByPrimaryKey(System.Int32 employeeID)
		{
			if(this.es.Connection.SqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(employeeID);
			else
				return LoadByPrimaryKeyStoredProcedure(employeeID);
		}

		public virtual bool LoadByPrimaryKey(esSqlAccessType sqlAccessType, System.Int32 employeeID)
		{
			if (sqlAccessType == esSqlAccessType.DynamicSQL)
				return LoadByPrimaryKeyDynamic(employeeID);
			else
				return LoadByPrimaryKeyStoredProcedure(employeeID);
		}

		private bool LoadByPrimaryKeyDynamic(System.Int32 employeeID)
		{
			EmployeesQuery query = new EmployeesQuery();
			query.Where(query.EmployeeID == employeeID);
			return this.Load(query);
		}

		private bool LoadByPrimaryKeyStoredProcedure(System.Int32 employeeID)
		{
			esParameters parms = new esParameters();
			parms.Add("EmployeeID", employeeID);
			return this.Load(esQueryType.StoredProcedure, this.es.spLoadByPrimaryKey, parms);
		}
		#endregion
		
		#region Properties
		
		
		
		/// <summary>
		/// Maps to Employees.EmployeeID
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? EmployeeID
		{
			get
			{
				return base.GetSystemInt32(EmployeesMetadata.ColumnNames.EmployeeID);
			}
			
			set
			{
				if(base.SetSystemInt32(EmployeesMetadata.ColumnNames.EmployeeID, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.EmployeeID);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.LastName
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String LastName
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.LastName);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.LastName, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.LastName);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.FirstName
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String FirstName
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.FirstName);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.FirstName, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.FirstName);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.Title
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Title
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.Title);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.Title, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.Title);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.TitleOfCourtesy
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String TitleOfCourtesy
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.TitleOfCourtesy);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.TitleOfCourtesy, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.TitleOfCourtesy);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.BirthDate
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.DateTime? BirthDate
		{
			get
			{
				return base.GetSystemDateTime(EmployeesMetadata.ColumnNames.BirthDate);
			}
			
			set
			{
				if(base.SetSystemDateTime(EmployeesMetadata.ColumnNames.BirthDate, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.BirthDate);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.HireDate
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.DateTime? HireDate
		{
			get
			{
				return base.GetSystemDateTime(EmployeesMetadata.ColumnNames.HireDate);
			}
			
			set
			{
				if(base.SetSystemDateTime(EmployeesMetadata.ColumnNames.HireDate, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.HireDate);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.Address
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Address
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.Address);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.Address, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.Address);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.City
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String City
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.City);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.City, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.City);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.Region
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Region
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.Region);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.Region, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.Region);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.PostalCode
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String PostalCode
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.PostalCode);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.PostalCode, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.PostalCode);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.Country
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Country
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.Country);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.Country, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.Country);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.HomePhone
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String HomePhone
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.HomePhone);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.HomePhone, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.HomePhone);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.Extension
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Extension
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.Extension);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.Extension, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.Extension);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.Photo
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Byte[] Photo
		{
			get
			{
				return base.GetSystemByteArray(EmployeesMetadata.ColumnNames.Photo);
			}
			
			set
			{
				if(base.SetSystemByteArray(EmployeesMetadata.ColumnNames.Photo, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.Photo);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.Notes
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String Notes
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.Notes);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.Notes, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.Notes);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.ReportsTo
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.Int32? ReportsTo
		{
			get
			{
				return base.GetSystemInt32(EmployeesMetadata.ColumnNames.ReportsTo);
			}
			
			set
			{
				if(base.SetSystemInt32(EmployeesMetadata.ColumnNames.ReportsTo, value))
				{
					this._UpToEmployeesByReportsTo = null;
					this.OnPropertyChanged("UpToEmployeesByReportsTo");
					OnPropertyChanged(EmployeesMetadata.PropertyNames.ReportsTo);
				}
			}
		}		
		
		/// <summary>
		/// Maps to Employees.PhotoPath
		/// </summary>
		[DataMember(EmitDefaultValue=false)]
		virtual public System.String PhotoPath
		{
			get
			{
				return base.GetSystemString(EmployeesMetadata.ColumnNames.PhotoPath);
			}
			
			set
			{
				if(base.SetSystemString(EmployeesMetadata.ColumnNames.PhotoPath, value))
				{
					OnPropertyChanged(EmployeesMetadata.PropertyNames.PhotoPath);
				}
			}
		}		
		
		[CLSCompliant(false)]
		internal protected Employees _UpToEmployeesByReportsTo;
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
						case "EmployeeID": this.str().EmployeeID = (string)value; break;							
						case "LastName": this.str().LastName = (string)value; break;							
						case "FirstName": this.str().FirstName = (string)value; break;							
						case "Title": this.str().Title = (string)value; break;							
						case "TitleOfCourtesy": this.str().TitleOfCourtesy = (string)value; break;							
						case "BirthDate": this.str().BirthDate = (string)value; break;							
						case "HireDate": this.str().HireDate = (string)value; break;							
						case "Address": this.str().Address = (string)value; break;							
						case "City": this.str().City = (string)value; break;							
						case "Region": this.str().Region = (string)value; break;							
						case "PostalCode": this.str().PostalCode = (string)value; break;							
						case "Country": this.str().Country = (string)value; break;							
						case "HomePhone": this.str().HomePhone = (string)value; break;							
						case "Extension": this.str().Extension = (string)value; break;							
						case "Notes": this.str().Notes = (string)value; break;							
						case "ReportsTo": this.str().ReportsTo = (string)value; break;							
						case "PhotoPath": this.str().PhotoPath = (string)value; break;
					}
				}
				else
				{
					switch (name)
					{	
						case "EmployeeID":
						
							if (value == null || value is System.Int32)
								this.EmployeeID = (System.Int32?)value;
								OnPropertyChanged(EmployeesMetadata.PropertyNames.EmployeeID);
							break;
						
						case "BirthDate":
						
							if (value == null || value is System.DateTime)
								this.BirthDate = (System.DateTime?)value;
								OnPropertyChanged(EmployeesMetadata.PropertyNames.BirthDate);
							break;
						
						case "HireDate":
						
							if (value == null || value is System.DateTime)
								this.HireDate = (System.DateTime?)value;
								OnPropertyChanged(EmployeesMetadata.PropertyNames.HireDate);
							break;
						
						case "Photo":
						
							if (value == null || value is System.Byte[])
								this.Photo = (System.Byte[])value;
								OnPropertyChanged(EmployeesMetadata.PropertyNames.Photo);
							break;
						
						case "ReportsTo":
						
							if (value == null || value is System.Int32)
								this.ReportsTo = (System.Int32?)value;
								OnPropertyChanged(EmployeesMetadata.PropertyNames.ReportsTo);
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
			public esStrings(esEmployees entity)
			{
				this.entity = entity;
			}
			
	
			public System.String EmployeeID
			{
				get
				{
					System.Int32? data = entity.EmployeeID;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.EmployeeID = null;
					else entity.EmployeeID = Convert.ToInt32(value);
				}
			}
				
			public System.String LastName
			{
				get
				{
					System.String data = entity.LastName;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.LastName = null;
					else entity.LastName = Convert.ToString(value);
				}
			}
				
			public System.String FirstName
			{
				get
				{
					System.String data = entity.FirstName;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.FirstName = null;
					else entity.FirstName = Convert.ToString(value);
				}
			}
				
			public System.String Title
			{
				get
				{
					System.String data = entity.Title;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.Title = null;
					else entity.Title = Convert.ToString(value);
				}
			}
				
			public System.String TitleOfCourtesy
			{
				get
				{
					System.String data = entity.TitleOfCourtesy;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.TitleOfCourtesy = null;
					else entity.TitleOfCourtesy = Convert.ToString(value);
				}
			}
				
			public System.String BirthDate
			{
				get
				{
					System.DateTime? data = entity.BirthDate;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.BirthDate = null;
					else entity.BirthDate = Convert.ToDateTime(value);
				}
			}
				
			public System.String HireDate
			{
				get
				{
					System.DateTime? data = entity.HireDate;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.HireDate = null;
					else entity.HireDate = Convert.ToDateTime(value);
				}
			}
				
			public System.String Address
			{
				get
				{
					System.String data = entity.Address;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.Address = null;
					else entity.Address = Convert.ToString(value);
				}
			}
				
			public System.String City
			{
				get
				{
					System.String data = entity.City;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.City = null;
					else entity.City = Convert.ToString(value);
				}
			}
				
			public System.String Region
			{
				get
				{
					System.String data = entity.Region;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.Region = null;
					else entity.Region = Convert.ToString(value);
				}
			}
				
			public System.String PostalCode
			{
				get
				{
					System.String data = entity.PostalCode;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.PostalCode = null;
					else entity.PostalCode = Convert.ToString(value);
				}
			}
				
			public System.String Country
			{
				get
				{
					System.String data = entity.Country;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.Country = null;
					else entity.Country = Convert.ToString(value);
				}
			}
				
			public System.String HomePhone
			{
				get
				{
					System.String data = entity.HomePhone;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.HomePhone = null;
					else entity.HomePhone = Convert.ToString(value);
				}
			}
				
			public System.String Extension
			{
				get
				{
					System.String data = entity.Extension;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.Extension = null;
					else entity.Extension = Convert.ToString(value);
				}
			}
				
			public System.String Notes
			{
				get
				{
					System.String data = entity.Notes;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.Notes = null;
					else entity.Notes = Convert.ToString(value);
				}
			}
				
			public System.String ReportsTo
			{
				get
				{
					System.Int32? data = entity.ReportsTo;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.ReportsTo = null;
					else entity.ReportsTo = Convert.ToInt32(value);
				}
			}
				
			public System.String PhotoPath
			{
				get
				{
					System.String data = entity.PhotoPath;
					return (data == null) ? String.Empty : Convert.ToString(data);
				}

				set
				{
					if (value == null || value.Length == 0) entity.PhotoPath = null;
					else entity.PhotoPath = Convert.ToString(value);
				}
			}
			

			private esEmployees entity;
		}
		
		[NonSerialized]
		private esStrings esstrings;		
		
		#endregion
		
		#region Housekeeping methods

		override protected IMetadata Meta
		{
			get
			{
				return EmployeesMetadata.Meta();
			}
		}

		#endregion		
		
		#region Query Logic

		public EmployeesQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new EmployeesQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(EmployeesQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return this.Query.Load();
		}
		
		protected void InitQuery(EmployeesQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntity)this).Connection;
			}			
		}

		#endregion
		
        [IgnoreDataMember]
		private EmployeesQuery query;		
	}



	[Serializable]
	abstract public partial class esEmployeesCollection : esEntityCollection<Employees>
	{
		#region Housekeeping methods
		override protected IMetadata Meta
		{
			get
			{
				return EmployeesMetadata.Meta();
			}
		}

		protected override string GetCollectionName()
		{
			return "EmployeesCollection";
		}

		#endregion		
		
		#region Query Logic

	#if (!WindowsCE)
		[BrowsableAttribute(false)]
	#endif
		public EmployeesQuery Query
		{
			get
			{
				if (this.query == null)
				{
					this.query = new EmployeesQuery();
					InitQuery(this.query);
				}

				return this.query;
			}
		}

		public bool Load(EmployeesQuery query)
		{
			this.query = query;
			InitQuery(this.query);
			return Query.Load();
		}

		override protected esDynamicQuery GetDynamicQuery()
		{
			if (this.query == null)
			{
				this.query = new EmployeesQuery();
				this.InitQuery(query);
			}
			return this.query;
		}

		protected void InitQuery(EmployeesQuery query)
		{
			query.OnLoadDelegate = this.OnQueryLoaded;
			
			if (!query.es2.HasConnection)
			{
				query.es2.Connection = ((IEntityCollection)this).Connection;
			}			
		}

		protected override void HookupQuery(esDynamicQuery query)
		{
			this.InitQuery((EmployeesQuery)query);
		}

		#endregion
		
		private EmployeesQuery query;
	}



	[Serializable]
	abstract public partial class esEmployeesQuery : esDynamicQuery
	{
		override protected IMetadata Meta
		{
			get
			{
				return EmployeesMetadata.Meta();
			}
		}	
		
		#region QueryItemFromName
		
        protected override esQueryItem QueryItemFromName(string name)
        {
            switch (name)
            {
				case "EmployeeID": return this.EmployeeID;
				case "LastName": return this.LastName;
				case "FirstName": return this.FirstName;
				case "Title": return this.Title;
				case "TitleOfCourtesy": return this.TitleOfCourtesy;
				case "BirthDate": return this.BirthDate;
				case "HireDate": return this.HireDate;
				case "Address": return this.Address;
				case "City": return this.City;
				case "Region": return this.Region;
				case "PostalCode": return this.PostalCode;
				case "Country": return this.Country;
				case "HomePhone": return this.HomePhone;
				case "Extension": return this.Extension;
				case "Photo": return this.Photo;
				case "Notes": return this.Notes;
				case "ReportsTo": return this.ReportsTo;
				case "PhotoPath": return this.PhotoPath;

                default: return null;
            }
        }		
		
		#endregion
		
		#region esQueryItems

		public esQueryItem EmployeeID
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.EmployeeID, esSystemType.Int32); }
		} 
		
		public esQueryItem LastName
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.LastName, esSystemType.String); }
		} 
		
		public esQueryItem FirstName
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.FirstName, esSystemType.String); }
		} 
		
		public esQueryItem Title
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.Title, esSystemType.String); }
		} 
		
		public esQueryItem TitleOfCourtesy
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.TitleOfCourtesy, esSystemType.String); }
		} 
		
		public esQueryItem BirthDate
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.BirthDate, esSystemType.DateTime); }
		} 
		
		public esQueryItem HireDate
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.HireDate, esSystemType.DateTime); }
		} 
		
		public esQueryItem Address
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.Address, esSystemType.String); }
		} 
		
		public esQueryItem City
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.City, esSystemType.String); }
		} 
		
		public esQueryItem Region
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.Region, esSystemType.String); }
		} 
		
		public esQueryItem PostalCode
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.PostalCode, esSystemType.String); }
		} 
		
		public esQueryItem Country
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.Country, esSystemType.String); }
		} 
		
		public esQueryItem HomePhone
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.HomePhone, esSystemType.String); }
		} 
		
		public esQueryItem Extension
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.Extension, esSystemType.String); }
		} 
		
		public esQueryItem Photo
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.Photo, esSystemType.ByteArray); }
		} 
		
		public esQueryItem Notes
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.Notes, esSystemType.String); }
		} 
		
		public esQueryItem ReportsTo
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.ReportsTo, esSystemType.Int32); }
		} 
		
		public esQueryItem PhotoPath
		{
			get { return new esQueryItem(this, EmployeesMetadata.ColumnNames.PhotoPath, esSystemType.String); }
		} 
		
		#endregion
		
	}


	
	public partial class Employees : esEmployees
	{

		#region EmployeesCollectionByReportsTo - Zero To Many
		
		static public esPrefetchMap Prefetch_EmployeesCollectionByReportsTo
		{
			get
			{
				esPrefetchMap map = new esPrefetchMap();
				map.PrefetchDelegate = BusinessObjects.Employees.EmployeesCollectionByReportsTo_Delegate;
				map.PropertyName = "EmployeesCollectionByReportsTo";
				map.MyColumnName = "EmployeeID";
				map.ParentColumnName = "ReportsTo";
				map.IsMultiPartKey = false;
				return map;
			}
		}		
		
		static private void EmployeesCollectionByReportsTo_Delegate(esPrefetchParameters data)
		{
			EmployeesQuery parent = new EmployeesQuery(data.NextAlias());

			EmployeesQuery me = data.You != null ? data.You as EmployeesQuery : new EmployeesQuery(data.NextAlias());

			if (data.Root == null)
			{
				data.Root = me;
			}
			
			data.Root.InnerJoin(parent).On(parent.ReportsTo == me.EmployeeID);

			data.You = parent;
		}			
		
		/// <summary>
		/// Zero to Many
		/// Foreign Key Name - FK_Employees_Employees
		/// </summary>

		[XmlIgnore]
		public EmployeesCollection EmployeesCollectionByReportsTo
		{
			get
			{
				if(this._EmployeesCollectionByReportsTo == null)
				{
					this._EmployeesCollectionByReportsTo = new EmployeesCollection();
					this._EmployeesCollectionByReportsTo.es.Connection.Name = this.es.Connection.Name;
					this.SetPostSave("EmployeesCollectionByReportsTo", this._EmployeesCollectionByReportsTo);
				
					if (this.EmployeeID != null)
					{
						if (!this.es.IsLazyLoadDisabled)
						{
							this._EmployeesCollectionByReportsTo.Query.Where(this._EmployeesCollectionByReportsTo.Query.ReportsTo == this.EmployeeID);
							this._EmployeesCollectionByReportsTo.Query.Load();
						}

						// Auto-hookup Foreign Keys
						this._EmployeesCollectionByReportsTo.fks.Add(EmployeesMetadata.ColumnNames.ReportsTo, this.EmployeeID);
					}
				}

				return this._EmployeesCollectionByReportsTo;
			}
			
			set 
			{ 
				if (value != null) throw new Exception("'value' Must be null"); 
			 
				if (this._EmployeesCollectionByReportsTo != null) 
				{ 
					this.RemovePostSave("EmployeesCollectionByReportsTo"); 
					this._EmployeesCollectionByReportsTo = null;
					
				} 
			} 			
		}
			
		
		private EmployeesCollection _EmployeesCollectionByReportsTo;
		#endregion

				
		#region UpToEmployeesByReportsTo - Many To One
		/// <summary>
		/// Many to One
		/// Foreign Key Name - FK_Employees_Employees
		/// </summary>

		[XmlIgnore]
					
		public Employees UpToEmployeesByReportsTo
		{
			get
			{
				if (this.es.IsLazyLoadDisabled) return null;
				
				if(this._UpToEmployeesByReportsTo == null && ReportsTo != null)
				{
					this._UpToEmployeesByReportsTo = new Employees();
					this._UpToEmployeesByReportsTo.es.Connection.Name = this.es.Connection.Name;
					this.SetPreSave("UpToEmployeesByReportsTo", this._UpToEmployeesByReportsTo);
					this._UpToEmployeesByReportsTo.Query.Where(this._UpToEmployeesByReportsTo.Query.EmployeeID == this.ReportsTo);
					this._UpToEmployeesByReportsTo.Query.Load();
				}	
				return this._UpToEmployeesByReportsTo;
			}
			
			set
			{
				this.RemovePreSave("UpToEmployeesByReportsTo");
				

				if(value == null)
				{
					this.ReportsTo = null;
					this._UpToEmployeesByReportsTo = null;
				}
				else
				{
					this.ReportsTo = value.EmployeeID;
					this._UpToEmployeesByReportsTo = value;
					this.SetPreSave("UpToEmployeesByReportsTo", this._UpToEmployeesByReportsTo);
				}
				
			}
		}
		#endregion
		

		#region UpToTerritoriesCollection - Many To Many
		/// <summary>
		/// Many to Many
		/// Foreign Key Name - FK_EmployeeTerritories_Employees
		/// </summary>

		[XmlIgnore]
		public TerritoriesCollection UpToTerritoriesCollection
		{
			get
			{
				if(this._UpToTerritoriesCollection == null)
				{
					this._UpToTerritoriesCollection = new TerritoriesCollection();
					this._UpToTerritoriesCollection.es.Connection.Name = this.es.Connection.Name;
					this.SetPostSave("UpToTerritoriesCollection", this._UpToTerritoriesCollection);
					if (!this.es.IsLazyLoadDisabled && this.EmployeeID != null)
					{
						TerritoriesQuery m = new TerritoriesQuery("m");
						EmployeeTerritoriesQuery j = new EmployeeTerritoriesQuery("j");
						m.Select(m);
						m.InnerJoin(j).On(m.TerritoryID == j.TerritoryID);
                        m.Where(j.EmployeeID == this.EmployeeID);

						this._UpToTerritoriesCollection.Load(m);
					}
				}

				return this._UpToTerritoriesCollection;
			}
			
			set 
			{ 
				if (value != null) throw new Exception("'value' Must be null"); 
			 
				if (this._UpToTerritoriesCollection != null) 
				{ 
					this.RemovePostSave("UpToTerritoriesCollection"); 
					this._UpToTerritoriesCollection = null;
					
				} 
			}  			
		}

		/// <summary>
		/// Many to Many Associate
		/// Foreign Key Name - FK_EmployeeTerritories_Employees
		/// </summary>
		public void AssociateTerritoriesCollection(Territories entity)
		{
			if (this._EmployeeTerritoriesCollection == null)
			{
				this._EmployeeTerritoriesCollection = new EmployeeTerritoriesCollection();
				this._EmployeeTerritoriesCollection.es.Connection.Name = this.es.Connection.Name;
				this.SetPostSave("EmployeeTerritoriesCollection", this._EmployeeTerritoriesCollection);
			}

			EmployeeTerritories obj = this._EmployeeTerritoriesCollection.AddNew();
			obj.EmployeeID = this.EmployeeID;
			obj.TerritoryID = entity.TerritoryID;
		}

		/// <summary>
		/// Many to Many Dissociate
		/// Foreign Key Name - FK_EmployeeTerritories_Employees
		/// </summary>
		public void DissociateTerritoriesCollection(Territories entity)
		{
			if (this._EmployeeTerritoriesCollection == null)
			{
				this._EmployeeTerritoriesCollection = new EmployeeTerritoriesCollection();
				this._EmployeeTerritoriesCollection.es.Connection.Name = this.es.Connection.Name;
				this.SetPostSave("EmployeeTerritoriesCollection", this._EmployeeTerritoriesCollection);
			}

			EmployeeTerritories obj = this._EmployeeTerritoriesCollection.AddNew();
			obj.EmployeeID = this.EmployeeID;
            obj.TerritoryID = entity.TerritoryID;
			obj.AcceptChanges();
			obj.MarkAsDeleted();
		}

		private TerritoriesCollection _UpToTerritoriesCollection;
		private EmployeeTerritoriesCollection _EmployeeTerritoriesCollection;
		#endregion

		#region EmployeeTerritoriesCollectionByEmployeeID - Zero To Many
		
		static public esPrefetchMap Prefetch_EmployeeTerritoriesCollectionByEmployeeID
		{
			get
			{
				esPrefetchMap map = new esPrefetchMap();
				map.PrefetchDelegate = BusinessObjects.Employees.EmployeeTerritoriesCollectionByEmployeeID_Delegate;
				map.PropertyName = "EmployeeTerritoriesCollectionByEmployeeID";
				map.MyColumnName = "EmployeeID";
				map.ParentColumnName = "EmployeeID";
				map.IsMultiPartKey = false;
				return map;
			}
		}		
		
		static private void EmployeeTerritoriesCollectionByEmployeeID_Delegate(esPrefetchParameters data)
		{
			EmployeesQuery parent = new EmployeesQuery(data.NextAlias());

			EmployeeTerritoriesQuery me = data.You != null ? data.You as EmployeeTerritoriesQuery : new EmployeeTerritoriesQuery(data.NextAlias());

			if (data.Root == null)
			{
				data.Root = me;
			}
			
			data.Root.InnerJoin(parent).On(parent.EmployeeID == me.EmployeeID);

			data.You = parent;
		}			
		
		/// <summary>
		/// Zero to Many
		/// Foreign Key Name - FK_EmployeeTerritories_Employees
		/// </summary>

		[XmlIgnore]
		public EmployeeTerritoriesCollection EmployeeTerritoriesCollectionByEmployeeID
		{
			get
			{
				if(this._EmployeeTerritoriesCollectionByEmployeeID == null)
				{
					this._EmployeeTerritoriesCollectionByEmployeeID = new EmployeeTerritoriesCollection();
					this._EmployeeTerritoriesCollectionByEmployeeID.es.Connection.Name = this.es.Connection.Name;
					this.SetPostSave("EmployeeTerritoriesCollectionByEmployeeID", this._EmployeeTerritoriesCollectionByEmployeeID);
				
					if (this.EmployeeID != null)
					{
						if (!this.es.IsLazyLoadDisabled)
						{
							this._EmployeeTerritoriesCollectionByEmployeeID.Query.Where(this._EmployeeTerritoriesCollectionByEmployeeID.Query.EmployeeID == this.EmployeeID);
							this._EmployeeTerritoriesCollectionByEmployeeID.Query.Load();
						}

						// Auto-hookup Foreign Keys
						this._EmployeeTerritoriesCollectionByEmployeeID.fks.Add(EmployeeTerritoriesMetadata.ColumnNames.EmployeeID, this.EmployeeID);
					}
				}

				return this._EmployeeTerritoriesCollectionByEmployeeID;
			}
			
			set 
			{ 
				if (value != null) throw new Exception("'value' Must be null"); 
			 
				if (this._EmployeeTerritoriesCollectionByEmployeeID != null) 
				{ 
					this.RemovePostSave("EmployeeTerritoriesCollectionByEmployeeID"); 
					this._EmployeeTerritoriesCollectionByEmployeeID = null;
					
				} 
			} 			
		}
			
		
		private EmployeeTerritoriesCollection _EmployeeTerritoriesCollectionByEmployeeID;
		#endregion

		#region OrdersCollectionByEmployeeID - Zero To Many
		
		static public esPrefetchMap Prefetch_OrdersCollectionByEmployeeID
		{
			get
			{
				esPrefetchMap map = new esPrefetchMap();
				map.PrefetchDelegate = BusinessObjects.Employees.OrdersCollectionByEmployeeID_Delegate;
				map.PropertyName = "OrdersCollectionByEmployeeID";
				map.MyColumnName = "EmployeeID";
				map.ParentColumnName = "EmployeeID";
				map.IsMultiPartKey = false;
				return map;
			}
		}		
		
		static private void OrdersCollectionByEmployeeID_Delegate(esPrefetchParameters data)
		{
			EmployeesQuery parent = new EmployeesQuery(data.NextAlias());

			OrdersQuery me = data.You != null ? data.You as OrdersQuery : new OrdersQuery(data.NextAlias());

			if (data.Root == null)
			{
				data.Root = me;
			}
			
			data.Root.InnerJoin(parent).On(parent.EmployeeID == me.EmployeeID);

			data.You = parent;
		}			
		
		/// <summary>
		/// Zero to Many
		/// Foreign Key Name - FK_Orders_Employees
		/// </summary>

		[XmlIgnore]
		public OrdersCollection OrdersCollectionByEmployeeID
		{
			get
			{
				if(this._OrdersCollectionByEmployeeID == null)
				{
					this._OrdersCollectionByEmployeeID = new OrdersCollection();
					this._OrdersCollectionByEmployeeID.es.Connection.Name = this.es.Connection.Name;
					this.SetPostSave("OrdersCollectionByEmployeeID", this._OrdersCollectionByEmployeeID);
				
					if (this.EmployeeID != null)
					{
						if (!this.es.IsLazyLoadDisabled)
						{
							this._OrdersCollectionByEmployeeID.Query.Where(this._OrdersCollectionByEmployeeID.Query.EmployeeID == this.EmployeeID);
							this._OrdersCollectionByEmployeeID.Query.Load();
						}

						// Auto-hookup Foreign Keys
						this._OrdersCollectionByEmployeeID.fks.Add(OrdersMetadata.ColumnNames.EmployeeID, this.EmployeeID);
					}
				}

				return this._OrdersCollectionByEmployeeID;
			}
			
			set 
			{ 
				if (value != null) throw new Exception("'value' Must be null"); 
			 
				if (this._OrdersCollectionByEmployeeID != null) 
				{ 
					this.RemovePostSave("OrdersCollectionByEmployeeID"); 
					this._OrdersCollectionByEmployeeID = null;
					
				} 
			} 			
		}
			
		
		private OrdersCollection _OrdersCollectionByEmployeeID;
		#endregion

		
		protected override esEntityCollectionBase CreateCollectionForPrefetch(string name)
		{
			esEntityCollectionBase coll = null;

			switch (name)
			{
				case "EmployeesCollectionByReportsTo":
					coll = this.EmployeesCollectionByReportsTo;
					break;
				case "EmployeeTerritoriesCollectionByEmployeeID":
					coll = this.EmployeeTerritoriesCollectionByEmployeeID;
					break;
				case "OrdersCollectionByEmployeeID":
					coll = this.OrdersCollectionByEmployeeID;
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
			
			props.Add(new esPropertyDescriptor(this, "EmployeesCollectionByReportsTo", typeof(EmployeesCollection), new Employees()));
			props.Add(new esPropertyDescriptor(this, "EmployeeTerritoriesCollectionByEmployeeID", typeof(EmployeeTerritoriesCollection), new EmployeeTerritories()));
			props.Add(new esPropertyDescriptor(this, "OrdersCollectionByEmployeeID", typeof(OrdersCollection), new Orders()));
		
			return props;
		}
		/// <summary>
		/// Used internally for retrieving AutoIncrementing keys
		/// during hierarchical PreSave.
		/// </summary>
		protected override void ApplyPreSaveKeys()
		{
			if(!this.es.IsDeleted && this._UpToEmployeesByReportsTo != null)
			{
				this.ReportsTo = this._UpToEmployeesByReportsTo.EmployeeID;
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
			if(this._EmployeesCollectionByReportsTo != null)
			{
				Apply(this._EmployeesCollectionByReportsTo, "ReportsTo", this.EmployeeID);
			}
			if(this._EmployeeTerritoriesCollection != null)
			{
				Apply(this._EmployeeTerritoriesCollection, "EmployeeID", this.EmployeeID);
			}
			if(this._EmployeeTerritoriesCollectionByEmployeeID != null)
			{
				Apply(this._EmployeeTerritoriesCollectionByEmployeeID, "EmployeeID", this.EmployeeID);
			}
			if(this._OrdersCollectionByEmployeeID != null)
			{
				Apply(this._OrdersCollectionByEmployeeID, "EmployeeID", this.EmployeeID);
			}
		}
		
	}
	



	[Serializable]
	public partial class EmployeesMetadata : esMetadata, IMetadata
	{
		#region Protected Constructor
		protected EmployeesMetadata()
		{
			m_columns = new esColumnMetadataCollection();
			esColumnMetadata c;

			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.EmployeeID, 0, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = EmployeesMetadata.PropertyNames.EmployeeID;
			c.IsInPrimaryKey = true;
			c.IsAutoIncrement = true;
			c.NumericPrecision = 10;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.LastName, 1, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.LastName;
			c.CharacterMaxLength = 20;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.FirstName, 2, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.FirstName;
			c.CharacterMaxLength = 10;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.Title, 3, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.Title;
			c.CharacterMaxLength = 30;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.TitleOfCourtesy, 4, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.TitleOfCourtesy;
			c.CharacterMaxLength = 25;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.BirthDate, 5, typeof(System.DateTime), esSystemType.DateTime);
			c.PropertyName = EmployeesMetadata.PropertyNames.BirthDate;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.HireDate, 6, typeof(System.DateTime), esSystemType.DateTime);
			c.PropertyName = EmployeesMetadata.PropertyNames.HireDate;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.Address, 7, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.Address;
			c.CharacterMaxLength = 60;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.City, 8, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.City;
			c.CharacterMaxLength = 15;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.Region, 9, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.Region;
			c.CharacterMaxLength = 15;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.PostalCode, 10, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.PostalCode;
			c.CharacterMaxLength = 10;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.Country, 11, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.Country;
			c.CharacterMaxLength = 15;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.HomePhone, 12, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.HomePhone;
			c.CharacterMaxLength = 24;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.Extension, 13, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.Extension;
			c.CharacterMaxLength = 4;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.Photo, 14, typeof(System.Byte[]), esSystemType.ByteArray);
			c.PropertyName = EmployeesMetadata.PropertyNames.Photo;
			c.CharacterMaxLength = 2147483647;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.Notes, 15, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.Notes;
			c.CharacterMaxLength = 1073741823;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.ReportsTo, 16, typeof(System.Int32), esSystemType.Int32);
			c.PropertyName = EmployeesMetadata.PropertyNames.ReportsTo;
			c.NumericPrecision = 10;
			c.IsNullable = true;
			m_columns.Add(c);
				
			c = new esColumnMetadata(EmployeesMetadata.ColumnNames.PhotoPath, 17, typeof(System.String), esSystemType.String);
			c.PropertyName = EmployeesMetadata.PropertyNames.PhotoPath;
			c.CharacterMaxLength = 255;
			c.IsNullable = true;
			m_columns.Add(c);
				
		}
		#endregion	
	
		static public EmployeesMetadata Meta()
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
			 public const string LastName = "LastName";
			 public const string FirstName = "FirstName";
			 public const string Title = "Title";
			 public const string TitleOfCourtesy = "TitleOfCourtesy";
			 public const string BirthDate = "BirthDate";
			 public const string HireDate = "HireDate";
			 public const string Address = "Address";
			 public const string City = "City";
			 public const string Region = "Region";
			 public const string PostalCode = "PostalCode";
			 public const string Country = "Country";
			 public const string HomePhone = "HomePhone";
			 public const string Extension = "Extension";
			 public const string Photo = "Photo";
			 public const string Notes = "Notes";
			 public const string ReportsTo = "ReportsTo";
			 public const string PhotoPath = "PhotoPath";
		}
		#endregion	
		
		#region PropertyNames
		public class PropertyNames
		{ 
			 public const string EmployeeID = "EmployeeID";
			 public const string LastName = "LastName";
			 public const string FirstName = "FirstName";
			 public const string Title = "Title";
			 public const string TitleOfCourtesy = "TitleOfCourtesy";
			 public const string BirthDate = "BirthDate";
			 public const string HireDate = "HireDate";
			 public const string Address = "Address";
			 public const string City = "City";
			 public const string Region = "Region";
			 public const string PostalCode = "PostalCode";
			 public const string Country = "Country";
			 public const string HomePhone = "HomePhone";
			 public const string Extension = "Extension";
			 public const string Photo = "Photo";
			 public const string Notes = "Notes";
			 public const string ReportsTo = "ReportsTo";
			 public const string PhotoPath = "PhotoPath";
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
			lock (typeof(EmployeesMetadata))
			{
				if(EmployeesMetadata.mapDelegates == null)
				{
					EmployeesMetadata.mapDelegates = new Dictionary<string,MapToMeta>();
				}
				
				if (EmployeesMetadata.meta == null)
				{
					EmployeesMetadata.meta = new EmployeesMetadata();
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
				meta.AddTypeMap("LastName", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("FirstName", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("Title", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("TitleOfCourtesy", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("BirthDate", new esTypeMap("datetime", "System.DateTime"));
				meta.AddTypeMap("HireDate", new esTypeMap("datetime", "System.DateTime"));
				meta.AddTypeMap("Address", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("City", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("Region", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("PostalCode", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("Country", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("HomePhone", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("Extension", new esTypeMap("nvarchar", "System.String"));
				meta.AddTypeMap("Photo", new esTypeMap("image", "System.Byte[]"));
				meta.AddTypeMap("Notes", new esTypeMap("ntext", "System.String"));
				meta.AddTypeMap("ReportsTo", new esTypeMap("int", "System.Int32"));
				meta.AddTypeMap("PhotoPath", new esTypeMap("nvarchar", "System.String"));			
				
				
				
				meta.Source = "Employees";
				meta.Destination = "Employees";
				
				meta.spInsert = "proc_EmployeesInsert";				
				meta.spUpdate = "proc_EmployeesUpdate";		
				meta.spDelete = "proc_EmployeesDelete";
				meta.spLoadAll = "proc_EmployeesLoadAll";
				meta.spLoadByPrimaryKey = "proc_EmployeesLoadByPrimaryKey";
				
				this.m_providerMetadataMaps["esDefault"] = meta;
			}
			
			return this.m_providerMetadataMaps["esDefault"];
		}

		#endregion

		static private EmployeesMetadata meta;
		static protected Dictionary<string, MapToMeta> mapDelegates;
		static private int _esDefault = RegisterDelegateesDefault();
	}
}
