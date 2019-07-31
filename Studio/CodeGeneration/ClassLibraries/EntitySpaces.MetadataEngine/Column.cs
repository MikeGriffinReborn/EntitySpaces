using System;
using System.ComponentModel;
using System.Xml;
using System.Data;
using System.Data.OleDb;

namespace EntitySpaces.MetadataEngine
{
	public class Column : Single, IColumn, INameValueItem
	{
		public Column()
		{

		}

		virtual internal Column Clone()
		{
			Column c = (Column)this.dbRoot.ClassFactory.CreateColumn();

			c.dbRoot	= this.dbRoot;
			c.Columns	= this.Columns;
			c._row		= this._row;

			c._foreignKeys	= Column._emptyForeignKeys;

			return c;
        }

        #region Code Generation Properties
        [Category("Code Generation")]
        virtual public string PropertyName
        {
            get
            {
                return this.dbRoot.esPlugIn.PropertyName(this);
            }
        }

        [Category("Code Generation")]
        virtual public string CSharpToSystemType
        {
            get
            {
                return this.dbRoot.esPlugIn.CSharpToSystemType(this);
            }
        }

        [Category("Code Generation")]
        virtual public string VBToSystemType
        {
            get
            {
                return this.dbRoot.esPlugIn.VBToSystemType(this);
            }
        }

        [Category("Code Generation")]
        virtual public string esSystemType
        {
            get
            {
                return this.dbRoot.esPlugIn.esSystemType(this);
            }
        }

        [Category("Code Generation")]
        virtual public string ParameterName
        {
            get
            {
                return this.dbRoot.esPlugIn.ParameterName(this);
            }
        }

        [Category("Code Generation")]
        virtual public System.Boolean IsArrayType
        {
            get
            {
                return this.dbRoot.esPlugIn.IsArrayType(this);
            }
        }
        
        [Category("Code Generation")]
        virtual public System.Boolean IsObjectType
        {
            get
            {
                return this.dbRoot.esPlugIn.IsObjectType(this);
            }
        }

        [Category("Code Generation")]
        virtual public System.Boolean IsNullableType
        {
            get
            {
                return this.dbRoot.esPlugIn.IsNullableType(this);
            }
        }
        
        [Category("Code Generation")]
        virtual public string NullableType
        {
            get
            {
                return this.dbRoot.esPlugIn.NullableType(this);
            }
        }

        [Category("Code Generation")]
        virtual public string NullableTypeVB
        {
            get
            {
                return this.dbRoot.esPlugIn.NullableTypeVB(this);
            }
        }
        
        [Category("Code Generation")]
        virtual public string SetRowAccessor
        {
            get
            {
                return this.dbRoot.esPlugIn.SetRowAccessor(this);
            }
        }
        
        [Category("Code Generation")]
        virtual public string GetRowAccessor
        {
            get
            {
                return this.dbRoot.esPlugIn.GetRowAccessor(this);
            }
        }

        #endregion

        #region Objects

        [Browsable(false)]
		public ITable Table
		{
			get
			{
				ITable theTable = null;

				if(null != Columns.Table)
				{
					theTable = Columns.Table;
				}
				else if(null != Columns.Index)
				{
					theTable =  Columns.Index.Indexes.Table;
				}
				else if(null != Columns.ForeignKey)
				{
					theTable =  Columns.ForeignKey.ForeignKeys.Table;
				}

				return theTable;
			}
		}

        [Browsable(false)]
		public IView View
		{
			get
			{
				IView theView = null;

				if(null != Columns.View)
				{
					theView = Columns.View;
				}

				return theView;
			}
		}

        [Category("Domain")]
		public IDomain Domain
		{
			get
			{
				IDomain theDomain = null;

				if(this.HasDomain)
				{
					theDomain = this.Columns.GetDatabase().Domains[this.DomainName];
				}

				return theDomain;
			}
		}

		#endregion

		#region Properties

        [Category("Name")]
		override public string Name
		{
			get
			{
				return this.GetString(Columns.f_Name);
			}
		}

        [Browsable(false)]
		virtual public Guid Guid
		{
			get
			{
				return this.GetGuid(Columns.f_Guid);
			}
		}

        [Browsable(false)]
		virtual public System.Int32 PropID
		{
			get
			{
				return this.GetInt32(Columns.f_PropID);
			}
		}

        [Browsable(false)]
		virtual public System.Int32 Ordinal
		{
			get
			{
				return this.GetInt32(Columns.f_Ordinal);
			}
		}

        [Category("Default Value")]
		virtual public System.Boolean HasDefault
		{
			get
			{
				return this.GetBool(Columns.f_HasDefault);
			}
		}

        [Category("Default Value")]
		virtual public string Default
		{
			get
			{
				return this.GetString(Columns.f_Default);
			}
		}

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		virtual public System.Int32 Flags
		{
			get
			{
				return this.GetInt32(Columns.f_Flags);
			}
		}

        [Category("Flags")]
		virtual public System.Boolean IsNullable
		{
			get
			{
				return this.GetBool(Columns.f_IsNullable);
			}
		}

        [Browsable(false)]
		virtual public System.Int32 DataType
		{
			get
			{
				return this.GetInt32(Columns.f_DataType);
			}
		}

        [Category("Data Type")]
        virtual public System.Boolean IsNonSystemType
        {
            get
            {
                if (dbRoot.LanguageNode != null)
                {
                    // First Let's try the 'DataTypeNameComplete' or char(1)
                    string xPath = @"./Type[@From='" + this.DataTypeNameComplete + "']";
                    XmlNode node = dbRoot.LanguageNode.SelectSingleNode(xPath, null);

                    if (node != null)
                    {
                        string flag = "";
                        if (this.GetUserData(node, "NonSystemType", out flag))
                        {
                            return flag == "true" ? true : false;
                        }
                    }

                    // No match, so lets just try the 'DataTypeName' or char
                    xPath = @"./Type[@From='" + this.DataTypeName + "']";
                    node = dbRoot.LanguageNode.SelectSingleNode(xPath, null);

                    if (node != null)
                    {
                        string flag = "";
                        if (this.GetUserData(node, "NonSystemType", out flag))
                        {
                            return flag == "true" ? true : false;
                        }
                    }
                }

                return false;
            }
        }

        [Browsable(false)]
		virtual public Guid TypeGuid
		{
			get
			{
				return this.GetGuid(Columns.f_TypeGuid);
			}
		}

        [Category("Character")]
		virtual public System.Int32 CharacterMaxLength
		{
			get
			{
				return this.GetInt32(Columns.f_MaxLength);
			}
		}

        [Browsable(false)]
		virtual public System.Int32 CharacterOctetLength
		{
			get
			{
				return this.GetInt32(Columns.f_OctetLength);
			}
		}

        [Category("Numeric")]
		virtual public System.Int32 NumericPrecision
		{
			get
			{
				return this.GetInt32(Columns.f_NumericPrecision);
			}
		}

        [Category("Numeric")]
		virtual public System.Int32 NumericScale
		{
			get
			{
				return this.GetInt32(Columns.f_NumericScale);
			}
		}

        [Browsable(false)]
		virtual public System.Int32 DateTimePrecision
		{
			get
			{
				return this.GetInt32(Columns.f_DatetimePrecision);
			}
		}

        [Browsable(false)]
		virtual public string CharacterSetCatalog
		{
			get
			{
				return this.GetString(Columns.f_CharSetCatalog);
			}
		}

        [Browsable(false)]
		virtual public string CharacterSetSchema
		{
			get
			{
				return this.GetString(Columns.f_CharSetSchema);
			}
		}

        [Browsable(false)]
		virtual public string CharacterSetName
		{
			get
			{
				return this.GetString(Columns.f_CharSetName);
			}
		}

        [Category("Domain")]
		virtual public string DomainCatalog
		{
			get
			{
				return this.GetString(Columns.f_DomainCatalog);
			}
		}

        [Category("Domain")]
		virtual public string DomainSchema
		{
			get
			{
				return this.GetString(Columns.f_DomainSchema);
			}
		}

        [Category("Domain")]
		virtual public string DomainName
		{
			get
			{
				return this.GetString(Columns.f_DomainName);
			}
		}

		virtual public string Description
		{
			get
			{
				return this.GetString(Columns.f_Description);
			}
		}

        [Browsable(false)]
		virtual public System.Int32 LCID
		{
			get
			{
				return this.GetInt32(Columns.f_LCID);
			}
		}

        [Browsable(false)]
		virtual public System.Int32 CompFlags
		{
			get
			{
				return this.GetInt32(Columns.f_CompFlags);
			}
		}

        [Browsable(false)]
		virtual public System.Int32 SortID
		{
			get
			{
				return this.GetInt32(Columns.f_SortID);
			}
		}

        [Browsable(false)]
		virtual public System.Byte[] TDSCollation
		{
			get
			{
				return this.GetByteArray(Columns.f_TDSCollation);
			}
		}

        [Category("Flags")]
		virtual public System.Boolean IsComputed
		{
			get
			{
                XmlNode node = null;
                if (this.GetXmlNode(out node, false))
                {
                    string theOut = "";
                    if (this.GetUserData(node, "IsComputed", out theOut))
                    {
                        return Convert.ToBoolean(theOut);
                    }
                }

				return this.GetBool(Columns.f_IsComputed);
			}

            //set
            //{
            //    XmlNode node = null;
            //    if (this.GetXmlNode(out node, true))
            //    {
            //        this.SetUserData(node, "IsComputed", value.ToString());
            //    }
            //}
		}

        [Category("Flags")]
		virtual public System.Boolean IsInPrimaryKey
		{
			get
			{
                XmlNode node = null;
                if (this.GetXmlNode(out node, false))
                {
                    string theOut = "";
                    if (this.GetUserData(node, "IsInPrimaryKey", out theOut))
                    {
                        return Convert.ToBoolean(theOut);
                    }
                }

				System.Boolean isPrimaryKey = false;

				if(null != Columns.Table)
				{
					IColumn c = Columns.Table.PrimaryKeys[this.Name];

					if(null != c)
					{
						isPrimaryKey = true;
					}
				}

				return isPrimaryKey;
			}

            //set
            //{
            //    XmlNode node = null;
            //    if (this.GetXmlNode(out node, true))
            //    {
            //        this.SetUserData(node, "IsInPrimaryKey", value.ToString());
            //    }
            //}
		}

        [Category("Auto Key")]
		virtual public System.Boolean IsAutoKey
		{
			get
			{
                XmlNode node = null;
                if (this.GetXmlNode(out node, false))
                {
                    string theOut = "";
                    if (this.GetUserData(node, "IsAutoKey", out theOut))
                    {
                        return Convert.ToBoolean(theOut);
                    }
                }

				return this.GetBool(Columns.f_IsAutoKey);
			}

            set
            {
                XmlNode node = null;
                if (this.GetXmlNode(out node, true))
                {
                    this.SetUserData(node, "IsAutoKey", value.ToString());
                }
            }
		}

        [Category("Data Type")]
		virtual public string DataTypeName
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.DataTypeName;
						}
					}
				}

				return this.GetString(null);
			}
		}

        [Category("Data Type")]
		virtual public string LanguageType
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.LanguageType;
						}
					}
				}

                if (dbRoot.LanguageNode != null)
                {
                    // First Let's try the 'DataTypeNameComplete' or char(1)
                    string xPath = @"./Type[@From='" + this.DataTypeNameComplete + "']";

                    XmlNode node = null;

                    try
                    {
                        node = dbRoot.LanguageNode.SelectSingleNode(xPath, null);
                    }
                    catch { }

                    if (node != null)
                    {
                        string languageType = "";
                        if (this.GetUserData(node, "To", out languageType))
                        {
                            return languageType;
                        }
                    }

                    // No match, so lets just try the 'DataTypeName' or char
					xPath = @"./Type[@From='" + this.DataTypeName + "']";
					node = dbRoot.LanguageNode.SelectSingleNode(xPath, null);

					if(node != null)
					{
						string languageType = "";
						if(this.GetUserData(node, "To", out languageType))
						{
							return languageType;
						}
					}
				}

				return "Unknown";
			}
		}

        [Category("Data Type")]
		virtual public string DataTypeNameComplete
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
                            return this.Domain.DataTypeNameComplete.Replace("\'", string.Empty);
						}
					}
				}

				return "Unknown";
			}
		}

        [Category("Flags")]
		virtual public System.Boolean IsInForeignKey
		{
			get
			{
				if(this.ForeignKeys == Column._emptyForeignKeys)
					return true;
				else
					return this.ForeignKeys.Count > 0 ? true : false;
			}
		}

        [Category("Auto Key")]
		virtual public System.Int32 AutoKeySeed
		{
			get
			{
				return this.GetInt32(Columns.f_AutoKeySeed);
			}
		}

        [Category("Auto Key")]
        [Description("Typically the name of a sequence")]
        virtual public string AutoKeyText
        {
            get
            {
                string customAutoKeyText = "";

                XmlNode node = null;
                if (this.GetXmlNode(out node, false))
                {
                    string theOut = "";
                    if (this.GetUserData(node, "AutoKeyText", out theOut))
                    {
                        customAutoKeyText = theOut;
                    }
                }

                // There was no nice name
                return customAutoKeyText;
            }

            set
            {
                XmlNode node = null;
                if (this.GetXmlNode(out node, true))
                {
                    this.SetUserData(node, "AutoKeyText", value.ToString());
                }
            }
        }

        [Category("Auto Key")]
		virtual public System.Int32 AutoKeyIncrement
		{
			get
			{
				return this.GetInt32(Columns.f_AutoKeyIncrement);
			}
		}

        [Category("Domain")]
		virtual public System.Boolean HasDomain
		{
			get
			{
				if(this._row.Table.Columns.Contains("DOMAIN_NAME"))
				{
					object o = this._row["DOMAIN_NAME"];

					if(o != null && o != DBNull.Value)
					{
						return true;
					}
				}
				return false;
			}
        }

        #region DateAdded Properties

        [Category("DateAdded")]
        virtual public System.Boolean IsDateAddedColumn
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.DateAdded.IsEnabled && driverInfo.DateAdded.ColumnName == this.Name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [Category("DateAdded")]
        virtual public esSettingsDriverInfo.DateType DateAddedType
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.DateAdded.IsEnabled && driverInfo.DateAdded.ColumnName == this.Name)
                {
                    return driverInfo.DateAdded.Type;
                }
                else
                {
                    return esSettingsDriverInfo.DateType.Unassigned;
                }
            }
        }

        [Category("DateAdded")]
        virtual public string DateAddedServerSideText
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.DateAdded.IsEnabled && driverInfo.DateAdded.ColumnName == this.Name && 
                    driverInfo.DateAdded.Type == esSettingsDriverInfo.DateType.ServerSide)
                {
                    return driverInfo.DateAdded.ServerSideText;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        [Category("DateAdded")]
        virtual public esSettingsDriverInfo.ClientType DateAddedClientType
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.DateAdded.IsEnabled && driverInfo.DateAdded.ColumnName == this.Name &&
                    driverInfo.DateAdded.Type == esSettingsDriverInfo.DateType.ClientSide)
                {
                    return driverInfo.DateAdded.ClientType;
                }
                else
                {
                    return esSettingsDriverInfo.ClientType.Unassigned;
                }
            }
        }

        #endregion

        #region DateModified Properties

        [Category("DateModified")]
        virtual public System.Boolean IsDateModifiedColumn
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.DateModified.IsEnabled && driverInfo.DateModified.ColumnName == this.Name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [Category("DateModified")]
        virtual public esSettingsDriverInfo.DateType DateModifiedType
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.DateModified.IsEnabled && driverInfo.DateModified.ColumnName == this.Name)
                {
                    return driverInfo.DateModified.Type;
                }
                else
                {
                    return esSettingsDriverInfo.DateType.Unassigned;
                }
            }
        }

        [Category("DateModified")]
        virtual public string DateModifiedServerSideText
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.DateModified.IsEnabled && driverInfo.DateModified.ColumnName == this.Name &&
                    driverInfo.DateModified.Type == esSettingsDriverInfo.DateType.ServerSide)
                {
                    return driverInfo.DateModified.ServerSideText;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        [Category("DateModified")]
        virtual public esSettingsDriverInfo.ClientType DateModifiedClientType
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.DateModified.IsEnabled && driverInfo.DateModified.ColumnName == this.Name &&
                    driverInfo.DateModified.Type == esSettingsDriverInfo.DateType.ClientSide)
                {
                    return driverInfo.DateModified.ClientType;
                }
                else
                {
                    return esSettingsDriverInfo.ClientType.Unassigned;
                }
            }
        }

        #endregion

        #region AddedBy Properties

        [Category("AddedBy")]
        virtual public System.Boolean IsAddedByColumn
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.AddedBy.IsEnabled && driverInfo.AddedBy.ColumnName == this.Name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [Category("AddedBy")]
        virtual public System.Boolean UseAddedByEventHandler
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.AddedBy.IsEnabled && driverInfo.AddedBy.UseEventHandler == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [Category("AddedBy")]
        virtual public string AddedByServerSideText
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.AddedBy.IsEnabled && driverInfo.AddedBy.ColumnName == this.Name)
                {
                    return driverInfo.AddedBy.ServerSideText;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #endregion

        #region ModifiedBy Properties

        [Category("ModifiedBy")]
        virtual public System.Boolean IsModifiedByColumn
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.ModifiedBy.IsEnabled && driverInfo.ModifiedBy.ColumnName == this.Name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [Category("ModifiedBy")]
        virtual public System.Boolean UseModifiedByEventHandler
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.ModifiedBy.IsEnabled && driverInfo.ModifiedBy.UseEventHandler == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [Category("ModifiedBy")]
        virtual public string ModifiedByServerSideText
        {
            get
            {
                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;
                if (driverInfo.ModifiedBy.IsEnabled && driverInfo.ModifiedBy.ColumnName == this.Name)
                {
                    return driverInfo.ModifiedBy.ServerSideText;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #endregion

        #endregion

        #region EntitySpaces Extended Properties

        [Category("Name")]
        [Description("Provide your Column with an Alias")]
        override public string Alias
        {
            get
            {
                XmlNode node = null;
                if (this.GetXmlNode(out node, false))
                {
                    string niceName = null;

                    if (this.GetUserData(node, "Alias", out niceName))
                    {
                        if (string.Empty != niceName)
                            return niceName;
                    }
                }

                // There was no nice name
                return this.Name;
            }

            set
            {
                XmlNode node = null;
                if (this.GetXmlNode(out node, true))
                {
                    this.SetUserData(node, "Alias", value);
                }
            }
        }

        [Category("Flags")]
        [Description("Exlude this column from code generation")]
        public bool Exclude
        {
            get
            {
                string exclude = "False";

                XmlNode node = null;
                if (this.GetXmlNode(out node, false))
                {
                    string theOut = "";
                    if(this.GetUserData(node, "Exclude", out theOut))
                    {
                        exclude = theOut;
                    }
                }

                // There was no nice name
                return Convert.ToBoolean(exclude);
            }

            //set
            //{
            //    XmlNode node = null;
            //    if (this.GetXmlNode(out node, true))
            //    {
            //        this.SetUserData(node, "Exclude", value.ToString());
            //    }
            //}
        }

        [Category("Concurrency")]
        [Description("Use this integer field for Concurrency management")]
        virtual public bool IsEntitySpacesConcurrency
        {
            get
            {
                string isEntitySpacesConcurrency = "False";

                esSettingsDriverInfo driverInfo = this.dbRoot.SettingsDriverInfo;

                if (driverInfo.ConcurrencyColumnEnabled && driverInfo.ConcurrencyColumn != null && driverInfo.ConcurrencyColumn == this.Name)
                {
                    isEntitySpacesConcurrency = "True";
                }

                XmlNode node = null;
                if (this.GetXmlNode(out node, false))
                {
                    string theOut = "";
                    if (this.GetUserData(node, "IsEntitySpacesConcurrency", out theOut))
                    {
                        isEntitySpacesConcurrency = theOut;
                    }
                }

                // There was no nice name
                return Convert.ToBoolean(isEntitySpacesConcurrency);
            }

            set
            {
                XmlNode node = null;
                if (this.GetXmlNode(out node, true))
                {
                    this.SetUserData(node, "IsEntitySpacesConcurrency", value.ToString());
                }
            }
        }

        [Category("Concurrency")]
        [Description("True if this is the Native type for a Database's concurrency handling")]
        virtual public bool IsConcurrency
        {
            get
            {
                return this.GetBool(Columns.f_IsConcurrency);
            }
        }

        #endregion

        #region Collections

        [Browsable(false)]
        public IForeignKeys ForeignKeys
		{
			get
			{
				if(null == _foreignKeys)
				{
					_foreignKeys = (ForeignKeys)this.dbRoot.ClassFactory.CreateForeignKeys();
					_foreignKeys.dbRoot = this.dbRoot;

					if(this.Columns.Table != null)
					{
						IForeignKeys fk = this.Columns.Table.ForeignKeys;
					}
				}
				return _foreignKeys;
			}
		}

		protected internal virtual void AddForeignKey(ForeignKey fk)
		{
			if(null == _foreignKeys)
			{
				_foreignKeys = (ForeignKeys)this.dbRoot.ClassFactory.CreateForeignKeys();
				_foreignKeys.dbRoot = this.dbRoot;
			}

			this._foreignKeys.AddForeignKey(fk);
		}

		internal PropertyCollectionAll _allProperties = null;

		#endregion

		#region XML User Data

        [Browsable(false)]
		override public string UserDataXPath
		{ 
			get
			{
				return Columns.UserDataXPath + @"/Column[@Name='" + this.Name + "']";
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
				if(this.Columns.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Column[@Name='" + this.Name + "']";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Column", null);
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

		internal Columns Columns = null;
		protected ForeignKeys _foreignKeys = null;
		static private ForeignKeys _emptyForeignKeys = new ForeignKeys();
	}
}
