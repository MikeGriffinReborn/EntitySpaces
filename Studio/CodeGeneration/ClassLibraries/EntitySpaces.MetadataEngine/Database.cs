using System;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Collections;

using ADODB;

namespace EntitySpaces.MetadataEngine
{
	public class Database : Single, IDatabase, INameValueItem
	{
		public Database()
		{

		}

		virtual public ADODB.Recordset ExecuteSql(string sql)
		{
			Recordset oRS = new Recordset();
			OleDbConnection cn = null;
			OleDbDataReader reader = null;

			try 
			{
				cn = new OleDbConnection(dbRoot.ConnectionString);
				cn.Open();
                try
                {
                    cn.ChangeDatabase(this.Name);
                }
                catch { } // some databases don't have the concept of catalogs. Catch this and throw it out
                
				OleDbCommand command = new OleDbCommand(sql, cn);
				command.CommandType = CommandType.Text;

				reader = command.ExecuteReader();

				DataTable schema;
				string dataType, fieldname;
				int length;
				bool firstTime = true;

				while (reader.Read()) 
				{
					if (firstTime) 
					{
						schema = reader.GetSchemaTable();

						foreach (DataRow row in schema.Rows) 
						{
							fieldname = row["ColumnName"].ToString();
							dataType = row["DataType"].ToString();
							length = Convert.ToInt32(row["ColumnSize"]);

							oRS.Fields.Append(fieldname, GetADOType(dataType), length, 
								FieldAttributeEnum.adFldIsNullable, System.Reflection.Missing.Value);
						}

						oRS.Open(System.Reflection.Missing.Value, System.Reflection.Missing.Value, 
							CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic, 1);

						firstTime = false;
					}
					oRS.AddNew(System.Reflection.Missing.Value,	System.Reflection.Missing.Value);

					for(int i = 0; i < reader.FieldCount; i++)
					{

						if (reader[i] is System.Guid)
						{
							oRS.Fields[i].Value = "{" + reader[i].ToString() + "}";
						}
						else
						{
							oRS.Fields[i].Value = reader[i];
						}
					}
				}

				cn.Close();
				//Move to the first record
				if (!firstTime) 
				{
					oRS.MoveFirst();
				}
				else 
				{
					oRS = null;
				}
			}
			catch (Exception ex) 
			{
				if ((reader != null) && (!reader.IsClosed)) 
				{
					reader.Close();
					reader = null;
				}
				if ((cn != null) && (cn.State == ConnectionState.Open)) 
				{
					cn.Close();
					cn = null;
				}
				throw ex;
			}

			return oRS;
		}

		protected ADODB.Recordset ExecuteIntoRecordset(string sql, IDbConnection cn)
		{
			Recordset oRS = new Recordset();
			IDataReader reader = null;

			try 
			{
				IDbCommand command = cn.CreateCommand();
				command.CommandText = sql;
				command.CommandType = CommandType.Text;

				reader = command.ExecuteReader();

				DataTable schema;
				string dataType, fieldname;
				int length;
				bool firstTime = true;

				// Skip columns contains the index of any columns that we cannot handle, array types and such ...
				Hashtable skipColumns = null;

				while (reader.Read()) 
				{
					if (firstTime) 
					{
						skipColumns = new Hashtable();
						schema = reader.GetSchemaTable();

						int colID = 0;
						foreach (DataRow row in schema.Rows) 
						{
							fieldname = row["ColumnName"].ToString();
							dataType  = row["DataType"].ToString();
							length = Convert.ToInt32(row["ColumnSize"]);

							try
							{
								oRS.Fields.Append(fieldname, GetADOType(dataType), length, 
									FieldAttributeEnum.adFldIsNullable, System.Reflection.Missing.Value);
							} 
							catch
							{
								// We can't handle this column type, ie, Firebird array types
								skipColumns[colID] = colID;
							}

							colID++;
						}

						oRS.Open(System.Reflection.Missing.Value, System.Reflection.Missing.Value, 
							CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic, 1);

						firstTime = false;
					}

					oRS.AddNew(System.Reflection.Missing.Value,	System.Reflection.Missing.Value);

					for(int i = 0, j = 0; i < reader.FieldCount; i++)
					{
						// Skip columns that we cannot handle
						if(!skipColumns.ContainsKey(i))
						{
							if (reader[j] is System.Guid)
							{
								oRS.Fields[j].Value = "{" + reader[j].ToString() + "}";
							}
							else
							{
								try
								{
									oRS.Fields[j].Value = reader[j];
								}
								catch
								{ 
									// For some reason it wouldn't accept this value?
									oRS.Fields[j].Value = DBNull.Value;
								}
							}

							j++;
						}
					}
				}

				cn.Close();

				//Move to the first record
				if (!firstTime) 
				{
					oRS.MoveFirst();
				}
				else 
				{
					oRS = null;
				}
				
			}
			catch (Exception ex) 
			{
				if ((reader != null) && (!reader.IsClosed)) 
				{
					reader.Close();
					reader = null;
				}
				if ((cn != null) && (cn.State == ConnectionState.Open)) 
				{
					cn.Close();
					cn = null;
				}
				throw ex;
			}

			return oRS;
		}

		protected DataTypeEnum GetADOType(string sType)
		{
			switch(sType)
			{
				case null:
					return DataTypeEnum.adEmpty;

				case "System.Byte":
					return DataTypeEnum.adUnsignedTinyInt;

				case "System.SByte":
					return DataTypeEnum.adTinyInt;

				case "System.Boolean":
					return DataTypeEnum.adBoolean;

				case "System.Int16":
					return DataTypeEnum.adSmallInt;

				case "System.Int32":
					return DataTypeEnum.adInteger;

				case "System.Int64":
					return DataTypeEnum.adBigInt;

				case "System.Single":
					return DataTypeEnum.adSingle;

				case "System.Double":
					return DataTypeEnum.adDouble;

				case "System.Decimal":
					return DataTypeEnum.adDecimal;

				case "System.DateTime":
					return DataTypeEnum.adDate;

				case "System.Guid":
					return DataTypeEnum.adGUID;

				case "System.String":
					return DataTypeEnum.adBSTR; //.adChar;

				case "System.Byte[]":
					return DataTypeEnum.adBinary;

				case "System.Array":
					return DataTypeEnum.adArray;

				case "System.Object":
					return DataTypeEnum.adVariant;

				default:
					return 0;
			}
		}


		virtual public ITables Tables
		{
			get
			{
				if(null == _tables)
				{
					_tables = (Tables)this.dbRoot.ClassFactory.CreateTables();
					_tables.dbRoot = this._dbRoot;
					_tables.Database = this;
					_tables.LoadAll();
				}

				return _tables;
			}
		}

		virtual public IViews Views
		{
			get
			{
				if(null == _views)
				{
					_views = (Views)this.dbRoot.ClassFactory.CreateViews();
					_views.dbRoot = this._dbRoot;
					_views.Database = this;
					_views.LoadAll();
				}

				return _views;
			}
		}

        [Browsable(false)]
		virtual public IProcedures Procedures
		{
			get
			{
				if(null == _procedures)
				{
					_procedures = (Procedures)this.dbRoot.ClassFactory.CreateProcedures();
					_procedures.dbRoot = this._dbRoot;
					_procedures.Database = this;
					_procedures.LoadAll();
				}

				return _procedures;
			}
		}

        [Browsable(false)]
		virtual public IDomains Domains
		{
			get
			{
				if(null == _domains)
				{
					_domains = (Domains)this.dbRoot.ClassFactory.CreateDomains();
					_domains.dbRoot = this._dbRoot;
					_domains.Database = this;
					_domains.LoadAll();
				}

				return _domains;
			}
		}

		override public string Alias
		{
			get
			{
				XmlNode node = null;
				if(this.GetXmlNode(out node, false))
				{
					string niceName = null;

					if(this.GetUserData(node, "Alias", out niceName))
					{
						if(string.Empty != niceName)
							return niceName;
					}
				}

				// There was no nice name
				return this.Name;
			}

			set
			{
				XmlNode node = null;
				if(this.GetXmlNode(out node, true))
				{
					this.SetUserData(node, "Alias", value);
				}
			}
		}

		override public string Name
		{
			get
			{
				return this.GetString(Databases.f_Catalog);
			}
		}

		virtual public string Description
		{
			get
			{
				return this.GetString(Databases.f_Description);
			}
		}

		virtual public string SchemaName
		{
			get
			{
				return this.GetString(Databases.f_SchemaName);
			}
		}

		virtual public string SchemaOwner
		{
			get
			{
				return this.GetString(Databases.f_SchemaOwner);
			}
		}

        [Browsable(false)]
		virtual public string DefaultCharSetCatalog
		{
			get
			{
				return this.GetString(Databases.f_DefCharSetCat);
			}
		}

        [Browsable(false)]
		virtual public string DefaultCharSetSchema
		{
			get
			{
				return this.GetString(Databases.f_DefCharSetSchema);
			}
		}

        [Browsable(false)]
		virtual public string DefaultCharSetName
		{
			get
			{
				return this.GetString(Databases.f_DefCharSetName);
			}
		}

        [Browsable(false)]
		virtual public Root Root
		{
			get
			{
				return this.dbRoot;
			}
		}

		#region XML User Data

        [Browsable(false)]
		override public string UserDataXPath
		{ 
			get
			{
				return Databases.UserDataXPath + @"/Database[@Name='" + this.Name + "']";
			} 
		}

		override internal bool GetXmlNode(out XmlNode node, bool forceCreate)
		{
			node = null;
			bool success = false;

			if(null == _xmlNode)
			{
				// Get the parent node
				XmlNode parentNode = null;
				if(this.Databases.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Database[@Name='" + this.Name + "']";
					if(!GetUserData(xPath, parentNode, out _xmlNode) && forceCreate)
					{
						// Create it, and try again
						this.CreateUserMetaData(parentNode);
						GetUserData(xPath, parentNode, out _xmlNode);
					}
				}
			}

			if(null != _xmlNode)
			{
				node = _xmlNode;
				success = true;
			}

			return success;
		}

		override public void CreateUserMetaData(XmlNode parentNode)
		{
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Database", null);
			parentNode.AppendChild(myNode);

			XmlAttribute attr;

			attr = parentNode.OwnerDocument.CreateAttribute("Name");
			attr.Value = this.Name;
			myNode.Attributes.Append(attr);
		}

		#endregion

		#region INameValueCollection Members

        [Browsable(false)]
		public string ItemName
		{
			get
			{
				return this.Name;
			}
		}

        [Browsable(false)]
		public string ItemValue
		{
			get
			{
				return this.Name;
			}
		}

		#endregion

		internal  Databases Databases = null;
		protected Tables _tables = null;
		protected Views _views = null;
		protected Procedures _procedures = null;
		protected Domains _domains = null;

		// Global properties are per Database
		internal PropertyCollection  _columnProperties = null;
		internal PropertyCollection  _databaseProperties = null;
		internal PropertyCollection  _foreignkeyProperties = null;
		internal PropertyCollection  _indexProperties = null;
		internal PropertyCollection  _parameterProperties = null;
		internal PropertyCollection  _procedureProperties = null;
		internal PropertyCollection  _resultColumnProperties = null;
		internal PropertyCollection  _tableProperties = null;
		internal PropertyCollection  _viewProperties = null;
		internal PropertyCollection  _domainProperties = null;
	}
}
