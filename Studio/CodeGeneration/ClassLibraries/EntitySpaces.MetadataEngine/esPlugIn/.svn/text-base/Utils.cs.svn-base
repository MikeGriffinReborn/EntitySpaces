/*
 * Created by David Parsons.
 * Date: 11/4/2005
 * Dnp.Utils.dll is a MyGeneration 1.1 plugin which
 * provides easy access to commonly used template functions.
 *
 * Revised - 11/5/2005
 * Mike Griffin contributed a template cache.
 * 
 * Revised - 11/12/2005
 * Added ReadUserMap()
 * 
 * Revised - 11/20/2005
 * Angelo Hulshout contributed the GentleRelation class
 * from his Gentle.NET template (renamed to TableRelation)
 * which was derived from Justin Greenwood's Hierarchical template.
 * 
 * Revised - 12/1/2005
 * jcfigueiredo suggested some additional properties which
 * were added to the TableRelation class.
 * 
 * Revised - 12/17/2005
 * Added the ProgressDialog class.
 * 
 * Revised - 12/18/2005
 * Added overloads to TrimSpaces, SetCamelCase, and SetPascalCase
 * to trim addtional characters.
 * 
 * Revised - 1/5/2006
 * Greatly expanded the XML comments for the compiled help.
 * 
 * Revised - 1/6/2006
 * Added RelType Enum and RelationType Property.
 * 
 * Revised - 1/8/2006
 * Added VB.NET example code to the compiled help.
 * 
 * Revised - 5/8/2006
 * Added GetOrderedTableList.
 * 
 * Revised - 6/18/2006
 * Added Experimental method CreateTableRelation
 * to support templates in VBScript and JScript.
 * Works for JScript, but not for VBScript.
 */

using System;
using System.IO;
using System.Xml;
using System.Collections;
using EntitySpaces.MetadataEngine;

namespace EntitySpaces.MetadataEngine
{
    /// <summary>
    /// MyGeneration helper functions. Starting with MyGeneration 1.1.5
    /// the plugin is included with the download and installed for you.
    /// </summary>
    /// <remarks>
    /// The latest version of MyGeneration can be found at
    /// <code>http://www.mygenerationsoftware.com.</code>
    /// The latest version of the plugin is in this Template Archive:
    /// <code>http://www.mygenerationsoftware.com/TemplateLibrary/Archive/?guid=4a285a9a-4dd2-4655-b7ca-996b5516a5a1</code>
    /// The archive also contains additional explanation of the functions included,
    /// source code, and many example templates demonstrating how to use the plugin.
    /// </remarks>
    /// <example>
    /// <para>The following instructions only apply to MyGeneration 1.1.4:</para>
    /// Copy Dnp.Utils.dll to your MyGeneration program folder.
    /// <para>
    /// Add this line to ZeusConfig.xml:
    /// </para>
    /// <code>
    /// &lt;IntrinsicObject assembly="%ZEUSHOME%\Dnp.Utils.dll" classpath="Dnp.Utils.Utils" varname="DnpUtils" /&gt;
    /// </code>
    /// </example>
    public class Utils
    {
        /// <summary>
        /// MyGeneration helper functions.
        /// </summary>
        public Utils()
        {

        }

        public Utils(esSettings settings)
        {
            this.settings = settings;
        }
        private esSettings settings;

        #region Naming Functions
        /// <summary>
        /// Trim spaces from a string.
        /// </summary>
        public string TrimSpaces(string name)
        {
            return name.Replace(" ", "");
        }

        /// <summary>
        /// Trim spaces from a string.
        /// Pass in a char[] containing a list of additional characters
        /// to remove. Spaces are always trimmed.
        /// </summary>
        /// <example>
        /// <code>
        /// char[] c = new char[5] {'\\', '@', ',', '-', ':'};
        ///	string str = DnpUtils.TrimSpaces(Table.Alias, c);
        /// </code>
        /// </example>
        /// <param name="name">The string that needs to be adjusted.</param>
        /// <param name="trimList">char[] of characters to be trimmed.</param>
        /// <returns>A string with all listed characters trimmed. </returns>
        public string RemoveIllegalCharacters(string name)
        {
            string convertedName = String.Empty;

            for (int i = 0; i < name.Length; i++)
            {
                if (Char.IsLetterOrDigit(name[i]))
                {
                    convertedName += name[i];
                }
                else if (name[i] == '_')
                {
                    convertedName += name[i];
                }
            }

            if (Char.IsNumber(convertedName[0]))
            {
                convertedName = convertedName.Insert(0, "_");
            }

            return convertedName;
        }

        /// <summary>
        /// SetPascalCase sets the first character to upper case,
        /// trims spaces, underscores, and periods, and sets next to upper.
        /// (PascalCase is sometimes referred to as UpperCamelCase.)
        /// </summary>
        /// <example>
        /// <code>
        /// string tableName = SetPascalCase("MY_TABLE_NAME");
        /// </code>
        /// The result is "MyTableName"
        /// </example>
        public string SetPascalCase(string name, esSettings settings)
        {
            string convertedName = String.Empty;
            bool next2upper = true;
            bool allUpper = true;

            // checks for names in all CAPS
            foreach (char c in name)
            {
                if (Char.IsLower(c))
                {
                    allUpper = false;
                    break;
                }
            }

            foreach (char c in name)
            {
                if (Char.IsLetterOrDigit(c))
                {
                    if (!settings.UseRawNames)
                    {
                        if (next2upper)
                        {
                            convertedName += c.ToString().ToUpper();
                            next2upper = false;
                        }
                        else if (allUpper)
                        {
                            convertedName += c.ToString().ToLower();
                        }
                        else
                        {
                            convertedName += c;
                        }
                    }
                    else
                    {
                        convertedName += c;
                    }
                }
                else if (c == '_' && (settings.PreserveUnderscores || settings.UseRawNames))
                {
                    convertedName += c;
                    next2upper = true;
                }
                else
                {
                    next2upper = true;
                }
            }

            if (Char.IsDigit(convertedName[0]))
            {
                convertedName = convertedName.Insert(0, "_");
            }

            return convertedName;
        }

        /// <summary>
        /// SetCamelCase sets the first character to lower case,
        /// trims spaces, underscores, and periods, and sets next to upper.
        /// (CamelCase in this context refers to the lowerCamelCase convention.)
        /// </summary>
        /// <example>
        /// <code>
        /// string tableName = SetCamelCase("MY_TABLE_NAME");
        /// </code>
        /// The result is "myTableName"
        /// </example>
        public string SetCamelCase(string name, esSettings settings)
        {
            string convertedName = SetPascalCase(name, settings);
            string camelName = String.Empty;
            bool firstToLowered = false;

            for (int i = 0; i < convertedName.Length; i++)
            {
                if (Char.IsLetterOrDigit(convertedName[i]))
                {
                    if (!firstToLowered)
                    {
                        camelName += Char.ToLower(convertedName[i]);
                        firstToLowered = true;
                    }
                    else
                    {
                        camelName += convertedName[i];
                    }
                }
                else
                {
                    // This could really only be a underscore
                    camelName += convertedName[i];
                }
            }

            return camelName;
        }

        /// <summary>
        /// Used to construct an output file name.
        /// </summary>
        /// <example>
        /// <code>
        /// string outputPath = "C:\\Output";
        /// string fileName = "Order Details";
        /// bool prefix = true;
        /// bool trim = true;
        /// string fullFileName = 
        ///     DnpUtils.GetOutputFile(outputPath, fileName, ".cs", prefix, trim);
        /// </code>
        /// The result is "C:\Output\_OrderDetails.cs"
        /// </example>
        /// <param name="path">The path (with or without the trailing '\')</param>
        /// <param name="fileName">The name of the file (without the extension)</param>
        /// <param name="suffix">the file extension (with the dot), e.g. '.cs'</param>
        /// <param name="prefix">Set to true if you want a preceding underscore</param>
        /// <param name="trimName">Set to true to TrimSpaces() from the fileName</param>
        /// <returns>A string containing the concatenated path, prefix, filename, and suffix</returns>
        public string GetOutputFile(string path, string fileName, string suffix, bool prefix, bool trimName)
        {
            string buff = path;

            if (!buff.EndsWith("\\"))
            {
                buff += "\\";
            }

            if (prefix)
            {
                if (trimName)
                {
                    buff += "_" + TrimSpaces(fileName);
                }
                else
                {
                    buff += "_" + fileName;
                }
            }
            else
            {
                if (trimName)
                {
                    buff += TrimSpaces(fileName);
                }
                else
                {
                    buff += fileName;
                }
            }

            buff += suffix;

            return buff;
        }
        #endregion

        #region Miscellaneous

        /// <summary>
        /// EXPERIMENTAL!
        /// This creates an instance of the TableRelation Class
        /// so that it can be accessed in templates using
        /// Microsoft script (VBScript or JScript.)
        /// </summary>
        /// <remarks>
        /// THIS IS EXPERIMENTAL!
        /// As yet, it does not work for VBScript.
        /// </remarks>
        /// <example>
        /// In JScript:
        /// <code>
        /// var tableRel = DnpUtils.CreateTableRelation(tableMeta, foreignKey);
        /// </code>
        /// In VBScript:
        /// <code>
        /// Dim tableRel
        /// Set tableRel = DnpUtils.CreateTableRelation(tableMeta, foreignKey)
        /// </code>
        /// </example>
        public TableRelation CreateTableRelation(ITable tableMeta, IForeignKey foreignKey)
        {
            TableRelation tr = new TableRelation(tableMeta, foreignKey);
            return tr;
        }

        /// <summary>
        /// Helper function that retrieves an ordered list of tables in a database.
        /// If reverse is false, the list is an insert order list, so that
        /// the parent tables are listed before the child tables.
        /// If reverse is true, the list is a delete order list, so that
        /// the child tables are listed before the parent tables.
        /// Most of this is converted from Justin Greenwood's sql_library.js.
        /// </summary>
        /// <example>
        /// <code>
        /// ArrayList orderedList = DnpUtils.GetOrderedTableList(MyMeta.Databases[DatabaseName], false);
        /// </code>
        /// </example>
        public ArrayList GetOrderedTableList(IDatabase database, bool reverse)
        {
            ArrayList orderedList = new ArrayList();
            int numberAdded = -1;

            while (numberAdded != 0)
            {
                numberAdded = 0;

                for (int i = 0; i < database.Tables.Count; i++)
                {
                    string tableName = database.Tables[i].Name;
                    IForeignKeys fKeys = database.Tables[i].ForeignKeys;

                    // If there are no foreign keys add it to the list.
                    if (fKeys.Count == 0 && !orderedList.Contains(tableName))
                    {
                        orderedList.Add(tableName);
                        numberAdded++;
                    }
                    else
                    {
                        // If there are foreign keys, loop through them and see if
                        // all of the referencing tables are already in the collection.
                        // If they are all in there, add the table.
                        bool allExist = true;
                        for (int x = 0; x < fKeys.Count; x++)
                        {
                            IForeignKey fKey = fKeys[x];

                            // only look at indirect foreign keys.
                            if (fKey.ForeignTable.Name == tableName)
                            {
                                // Check to see if it is in the list or is self-referencing.
                                if (!orderedList.Contains(fKey.PrimaryTable.Name) && (fKey.PrimaryTable.Name != tableName))
                                {
                                    allExist = false;
                                    break;
                                }
                            }
                        }

                        if (allExist)
                        {
                            if (!orderedList.Contains(tableName))
                            {
                                orderedList.Add(tableName);
                                numberAdded++;
                            }
                        }
                    }
                } // End table loop
            } // End while

            // Any tables left over get added to the end of the list
            for (int i = 0; i < database.Tables.Count; i++)
            {
                string tableName = database.Tables[i].Name;
                if (!orderedList.Contains(tableName))
                {
                    orderedList.Add(tableName);
                }
            }

            if (reverse)
            {
                orderedList.Reverse();
            }

            return orderedList;
        } // End GetOrderedTableList

        #endregion

    }

    #region TableRelation
    /// <summary>
    /// This class takes a great deal of information that is
    /// contained in MyMeta ForeignKeys and exposes it in
    /// a very intuitive way.
    /// Thanks to Angelo Hulshout and Justin Greenwood for this contribution.
    /// See <see cref="ReferencedName" /> for VB.NET example code.
    /// </summary>
    /// <example>
    /// See the Table Relation Example templates in the
    /// DNP.PluginExamples namespace for more details,
    /// including C# and VB.NET examples.
    /// <code>
    /// // Loop through the tables selected in the template UI.
    /// foreach (string tableName in Tables)
    /// {
    /// 	// Get MyMeta information for a table.
    /// 	tableMeta = MyMeta.Databases[DatabaseName].Tables[tableName];
    /// 
    /// 	// Loop through the ForeignKeys in the table
    /// 	foreach( IForeignKey fk in tableMeta.ForeignKeys )
    /// 	{
    /// 		// Get the TableRelation properties for the specific table and ForeignKey.
    /// 		Dnp.Utils.TableRelation tr = new Dnp.Utils.TableRelation(tableMeta, fk);
    /// 
    /// 		// Use one of the many properties in your template.
    /// 		if(tr.IsZeroToMany)
    /// 		{
    /// 			output.autoTabLn("Zero-To-Many");
    /// 		}
    /// 	}
    /// }
    /// </code>
    /// What follows in an explanation of the terminology used by the
    /// TableRelation class. This diagram displays the relevant portions
    /// of the Northwind database. The EmployeeID field in the Orders
    /// table was changed to EmpID to make the examples clearer.
    /// <code>
    /// 
    ///      |-----------------|        |-------------------|
    ///      |    Employees    |        |EmployeeTerritories|      |----------------|
    ///      |-----------------|        |-------------------|      |  Territories   |
    ///   --&gt;|PK  | EmployeeID |&lt;-------|PK,FK1 |EmployeeID |      |----------------|
    ///   |  |-----------------|   |    |PK,FK2 |TerritoryID|-----&gt;|PK |TerritoryID |
    ///   |  |I1  | LastName   |   |    |-------------------|      |----------------|
    ///   |  |    | FirstName  |   |    |       |           |      |   |Description |
    ///   ---|FK1 | ReportsTo  |   |    |-------------------|      |----------------|
    ///      |-----------------|   |
    ///                            |
    ///                            |    |------------------|
    ///                            |    |     Orders       |
    ///                            |    |------------------|
    ///                            |    |PK  | OrderID     |
    ///                            |    |------------------|
    ///                            |    |    | CustomerID  |
    ///                            -----|FK1 | EmpID       |
    ///                                 |    | OrderDate   |
    ///                                 |------------------|
    /// 
    /// </code>
    /// <para>(Refer to the excerpt from the TableRelation Report template below.)</para>
    /// From a database design perspective, a ForeignKey constraint
    /// is placed on a field (or fields) in one table. In MyMeta, that
    /// ForeignKey will show up in both tables involved in the relationship.
    /// FK_Orders_Employees is only placed on the EmpID field in
    /// the Orders table in the database. But, you will see it listed
    /// for both the Employees table and the Orders table in the report.
    /// Consequently, TableRelation will adjust your perspective for you
    /// depending on which table was passed to the TableRelation constructor.
    /// <para>
    /// For all ForeignKeys, the 'Name' property is the Name of the
    /// ForeignKey (e.g., FK_Orders_Employees). The 'PrimaryTable' property
    /// is a MyMeta ITable. It is always the table that was passed in
    /// when you constructed the TableRelation. All the MyMeta ITable properties
    /// can be accessed with it (e.g., tr.PrimaryTable.Name, tr.PrimaryTable.Alias).
    /// The 'ForeignTable' property is the other table referenced by
    /// the ForeignKey.
    /// </para>
    /// <para>
    /// 'PrimaryColumns' is a MyMeta IColumns collection containing
    /// the PrimaryTable columns in the ForeignKey. 'ForeignColumns'
    /// contains the ForeignTable columns. Compare FK_Orders_Employees
    /// in both Employees and Orders below. Notice how PrimaryTable, ForeignTable,
    /// PrimaryColumns, and ForeignColumns are all adjusted for you.
    /// </para>
    /// <code>
    ///              PrimaryTable.Name : Employees
    ///                           Name : FK_Orders_Employees
    ///              ForeignTable.Name : Orders
    ///                     ObjectType : Orders
    ///                    ColumnCount : 1
    ///                 PrimaryColumns : EmployeeID
    ///                 ForeignColumns : EmpID
    ///  ForeignTableHasRequiredFields : True
    ///                       IsDirect : False
    ///                IsSelfReference : False
    ///                     IsOneToOne : False
    ///                   IsZeroToMany : True
    ///                    IsManyToOne : False
    ///                   IsManyToMany : False
    ///                 ReferencedName : ReferencedOrdersAsEmpID
    ///                  ReferringName : ReferringOrdersAsEmpID
    ///                       IsLookup : False
    ///                     LookupName : LookupOrdersAsEmpID
    ///               IsCrossReference : False
    /// ------------------------------ : ----------------------------------------
    ///              PrimaryTable.Name : Orders
    ///                           Name : FK_Orders_Employees
    ///              ForeignTable.Name : Employees
    ///                     ObjectType : Employees
    ///                    ColumnCount : 1
    ///                 PrimaryColumns : EmpID
    ///                 ForeignColumns : EmployeeID
    ///  ForeignTableHasRequiredFields : True
    ///                       IsDirect : True
    ///                IsSelfReference : False
    ///                     IsOneToOne : False
    ///                   IsZeroToMany : False
    ///                    IsManyToOne : True
    ///                   IsManyToMany : False
    ///                 ReferencedName : ReferencedEmployeesAsEmpID
    ///                  ReferringName : ReferringEmployeesAsEmpID
    ///                       IsLookup : True
    ///                     LookupName : LookupEmployeesAsEmpID
    ///               IsCrossReference : False
    /// ------------------------------ : ----------------------------------------
    /// </code>
    /// <code>
    /// IsLookup:
    /// IsManyToOne:
    /// IsZeroToMany:
    /// </code>
    /// ManyToOne is a common ForeignKey constraint often referred to
    /// as a Lookup. It will always have a corresponding ZeroToMany
    /// in the other table. In the report above, an Order is placed
    /// by one Employee (IsManyToOne is true in Orders, IsLookup is true.)
    /// An Employee can have many orders, but is not required
    /// to have any (IsZeroToMany is true in Employees.)
    /// While IsLookup and IsManyToMany are essentially the same test,
    /// the distinction is useful when using the naming properties (described below.)
    /// <code>IsOneToOne:</code>
    /// Northwind does not contain an example of a One-To-One
    /// relationship. A normalized database would not usually
    /// have tables related One-To-One. You would just combine the
    /// tables into one table containing all the fields.
    /// De-normalizing the database and adding a new table with a
    /// One-To-One relationship to an existing table is one way
    /// to extend a third-party database in anticipation of
    /// future releases from the vendor. One-To-One may also be necessary
    /// to overcome database limitations on the number of columns in a table.
    /// <code>
    /// IsSelfReference:
    /// </code>
    /// This is a special circumstance where the ManyToOne constraint
    /// refers to the same table. Employees ReportsTo (below) is an
    /// example. If IsSelfReference is true, then IsManyToOne, IsZeroToMany,
    /// and IsLookup will all be true for the same ForeignKey.
    /// An Employee reports to one Employee (IsManyToOne.) An Employee can have many
    /// Employees reporting to him, but may not have any (IsZeroToMany.)
    /// <code>
    ///              PrimaryTable.Name : Employees
    ///                           Name : FK_Employees_Employees
    ///              ForeignTable.Name : Employees
    ///                     ObjectType : Employees
    ///                    ColumnCount : 1
    ///                 PrimaryColumns : ReportsTo
    ///                 ForeignColumns : EmployeeID
    ///  ForeignTableHasRequiredFields : True
    ///                       IsDirect : True
    ///                IsSelfReference : True
    ///                     IsOneToOne : False
    ///                   IsZeroToMany : True
    ///                    IsManyToOne : True
    ///                   IsManyToMany : False
    ///                 ReferencedName : ReferencedEmployeesAsReportsTo
    ///                  ReferringName : ReferringEmployeesAsReportsTo
    ///                       IsLookup : True
    ///                     LookupName : LookupEmployeesAsReportsTo
    ///               IsCrossReference : False
    /// </code>
    /// <code>
    /// IsManyToMany:
    /// IsCrossReference:
    /// </code>
    /// Most databases do not handle ManyToMany relationships directly.
    /// An intermediate linking table is used with two ManyToOne
    /// relationships. EmployeeTerritories (below) has two ManyToOne
    /// ForeignKeys (to Employees and to Territories.) As you
    /// would expect, Employees and Territories both report ZeroToMany
    /// relationships with EmployeeTerritories. But, TableRelation
    /// recognizes this special case as a ManyToMany. For both
    /// Employees and Territories, IsManyToMany is also true. When IsManyToMany
    /// is true, then a number of additional properties are available.
    /// The ForeignTable acts as a link to the CrossReferenceTable.
    /// <para>IsCrossReference is a special case of ManyToMany. It
    /// will report true if the ForeignTable fields
    /// are all in the PrimaryKey, are calculated, or have defaults.
    /// Let's say we were to add an optional Notes field in
    /// EmployeeTerritories and re-run the report. IsManyToMany
    /// would still be true for Employees and Territories,
    /// but IsCrossReference would be false. It is alerting you
    /// that the ForeignTable (EmployeeTerritories) has fields that
    /// may need to be filled in by the user in the UI. It is not
    /// a simple mechanical linking table.
    /// </para>
    /// Be sure to test for IsManyToMany
    /// before trying to use any of the following properties:
    /// <code>
    /// CrossReferenceTable
    /// CrossReferenceColumnCount
    /// CrossReferenceColumns
    /// LinkingColumns
    /// Linking Name
    /// </code>
    /// This is how things look from the Employees table.
    /// <list type="table">
    /// <listheader>
    /// <term>PrimaryTable</term>
    /// <term>&lt;--ForeignTable--&gt;</term>
    /// <term>CrossReferenceTable</term>
    /// </listheader>
    /// <item>
    /// <description>Employees</description>
    /// <description>&lt;--EmployeeTerritories--&gt;</description>
    /// <description>Territories</description>
    /// </item>
    /// <item>
    /// <description>PrimaryColumns</description>
    /// <description>&lt;--ForeignColumns:LinkingColumns--&gt;</description>
    /// <description>CrossReferenceColumns</description>
    /// </item>
    /// <item>
    /// <description>EmployeeID</description>
    /// <description>&lt;--EmployeeID:TerritoryID--&gt;</description>
    /// <description>TerritoryID</description>
    /// </item>
    /// </list>
    /// Note the additional properties beneath 'IsCrossReference' in
    /// both the Employees table and Territories table for
    /// the FK_EmployeeTerritories_Employees ForeignKey in the report excerpt below.
    /// They are available to any Many-To-Many ForeignKey
    /// (regardless of whether IsCrossReference is true or not.)
    /// <code>
    ///              PrimaryTable.Name : Employees
    ///                           Name : FK_EmployeeTerritories_Employees
    ///              ForeignTable.Name : EmployeeTerritories
    ///                     ObjectType : EmployeeTerritories
    ///                    ColumnCount : 1
    ///                 PrimaryColumns : EmployeeID
    ///                 ForeignColumns : EmployeeID
    ///  ForeignTableHasRequiredFields : False
    ///                       IsDirect : False
    ///                IsSelfReference : False
    ///                     IsOneToOne : False
    ///                   IsZeroToMany : True
    ///                    IsManyToOne : False
    ///                   IsManyToMany : True
    ///                 ReferencedName : ReferencedTerritoriesUsingEmployeeTerritoriesAsEmployeeID
    ///                  ReferringName : ReferringEmployeeTerritoriesAsEmployeeID
    ///                       IsLookup : False
    ///                     LookupName : LookupEmployeeTerritoriesAsEmployeeID
    ///               IsCrossReference : True
    ///       CrossReferenceTable.Name : Territories
    ///      CrossReferenceColumnCount : 1
    ///          CrossReferenceColumns : TerritoryID
    ///                 LinkingColumns : TerritoryID
    ///                   Linking Name : LinkingEmployeeTerritoriesAsEmployeeID
    /// ------------------------------ : ----------------------------------------
    ///              PrimaryTable.Name : EmployeeTerritories
    ///                           Name : FK_EmployeeTerritories_Employees
    ///              ForeignTable.Name : Employees
    ///                     ObjectType : Employees
    ///                    ColumnCount : 1
    ///                 PrimaryColumns : EmployeeID
    ///                 ForeignColumns : EmployeeID
    ///  ForeignTableHasRequiredFields : False
    ///                       IsDirect : True
    ///                IsSelfReference : False
    ///                     IsOneToOne : False
    ///                   IsZeroToMany : False
    ///                    IsManyToOne : True
    ///                   IsManyToMany : False
    ///                 ReferencedName : ReferencedEmployeesAsEmployeeID
    ///                  ReferringName : ReferringEmployeesAsEmployeeID
    ///                       IsLookup : True
    ///                     LookupName : LookupEmployeesAsEmployeeID
    ///               IsCrossReference : False
    /// ------------------------------ : ----------------------------------------
    ///              PrimaryTable.Name : EmployeeTerritories
    ///                           Name : FK_EmployeeTerritories_Territories
    ///              ForeignTable.Name : Territories
    ///                     ObjectType : Territories
    ///                    ColumnCount : 1
    ///                 PrimaryColumns : TerritoryID
    ///                 ForeignColumns : TerritoryID
    ///  ForeignTableHasRequiredFields : False
    ///                       IsDirect : True
    ///                IsSelfReference : False
    ///                     IsOneToOne : False
    ///                   IsZeroToMany : False
    ///                    IsManyToOne : True
    ///                   IsManyToMany : False
    ///                 ReferencedName : ReferencedTerritoriesAsTerritoryID
    ///                  ReferringName : ReferringTerritoriesAsTerritoryID
    ///                       IsLookup : True
    ///                     LookupName : LookupTerritoriesAsTerritoryID
    ///               IsCrossReference : False
    /// ------------------------------ : ----------------------------------------
    ///              PrimaryTable.Name : Territories
    ///                           Name : FK_EmployeeTerritories_Territories
    ///              ForeignTable.Name : EmployeeTerritories
    ///                     ObjectType : EmployeeTerritories
    ///                    ColumnCount : 1
    ///                 PrimaryColumns : TerritoryID
    ///                 ForeignColumns : TerritoryID
    ///  ForeignTableHasRequiredFields : False
    ///                       IsDirect : False
    ///                IsSelfReference : False
    ///                     IsOneToOne : False
    ///                   IsZeroToMany : True
    ///                    IsManyToOne : False
    ///                   IsManyToMany : True
    ///                 ReferencedName : ReferencedEmployeesUsingEmployeeTerritoriesAsTerritoryID
    ///                  ReferringName : ReferringEmployeeTerritoriesAsTerritoryID
    ///                       IsLookup : False
    ///                     LookupName : LookupEmployeeTerritoriesAsTerritoryID
    ///               IsCrossReference : True
    ///       CrossReferenceTable.Name : Employees
    ///      CrossReferenceColumnCount : 1
    ///          CrossReferenceColumns : EmployeeID
    ///                 LinkingColumns : EmployeeID
    ///                   Linking Name : LinkingEmployeeTerritoriesAsTerritoryID
    /// </code>
    /// <code>
    /// Naming Properties:
    /// </code>
    /// The intention is to create unique names for each relationship
    /// that can then be used in the template to provide function
    /// names.
    /// <list type="table">
    /// <listheader>
    /// <term>When this is true</term>
    /// <term>Use this name</term>
    /// <term>For this function</term>
    /// </listheader>
    /// <item>
    /// <description>IsManyToOne</description>
    /// <description>ReferencedName</description>
    /// <description>Retrieve the 1 related row.</description>
    /// </item>
    /// <item>
    /// <description>IsZeroToMany</description>
    /// <description>ReferringName</description>
    /// <description>Retrieve the many rows related to the current row.</description>
    /// </item>
    /// <item>
    /// <description>IsLookup</description>
    /// <description>LookupName</description>
    /// <description>Retrieve PrimaryKey column and display column for all rows for a ComboBox.</description>
    /// </item>
    /// <item>
    /// <description>***************</description>
    /// <description>***************</description>
    /// <description>************************************</description>
    /// </item>
    /// </list>
    /// See <see cref="ReferringName"/> for an example when IsSelfReference is true.
    /// See <see cref="ReferencedName" /> for an example when IsManyToMany is true.
    /// </example>
    public class TableRelation
    {
        private IForeignKey _ForeignKey;
        private ITable _CurrentTable;
        private bool _AllFksArePks;
        private bool _ForeignTableHasRequiredFields;
        private bool _IsDirect;
        private Utils theUtils = new Utils();

        /// <summary>
        /// The type of Relationship in the ForeignKey
        /// </summary>
        public enum RelType
        {
            /// <summary>
            /// OneToOne
            /// </summary>
            OneToOne = 1,
            /// <summary>
            /// SelfReference - IsZeroToMany, IsLookup, and IsManyToOne
            ///                 would also be true
            /// </summary>
            SelfReference,
            /// <summary>
            /// ManyToMany - IsZeroToMany would also be true.
            ///              IsCrossReference may also be true.
            /// </summary>
            ManyToMany,
            /// <summary>
            /// ZeroToManyOnly
            /// </summary>
            ZeroToManyOnly,
            /// <summary>
            /// ManyToOneOnly - IsLookup would also be true.
            /// </summary>
            ManyToOneOnly
        };

        /// <summary>
        /// This class takes a great deal of information that is
        /// contained in MyMeta Foreign keys and exposes it in
        /// a very intuitive way.
        /// Thanks to Angelo Hulshout and Justin Greenwood for this contribution.
        /// See <see cref="TableRelation" /> for a detailed explantion.
        /// See <see cref="ReferencedName" /> for a VB.NET code example.
        /// </summary>
        /// <example>
        /// See the Table Relation Example templates in the
        /// DNP.PluginExamples namespace for more details.
        /// <code>
        /// // Loop through the tables selected in the template UI.
        /// foreach (string tableName in Tables)
        /// {
        /// 	// Grab MyMeta info for a table.
        /// 	tableMeta = MyMeta.Databases[DatabaseName].Tables[tableName];
        /// 
        /// 	// Loop through each ForeignKey in the table.
        /// 	foreach( IForeignKey fk in tableMeta.ForeignKeys )
        /// 	{
        /// 		// Get the TableRelation properties for the specific table and ForeignKey.
        /// 		Dnp.Utils.TableRelation tr = new Dnp.Utils.TableRelation(tableMeta, fk);
        /// 
        /// 		// Use one of the many properties in your template.
        /// 		if(tr.IsZeroToMany)
        /// 		{
        /// 			output.autoTabLn("Zero-To-Many");
        /// 		}
        /// 	}
        /// }
        /// </code>
        /// </example>
        /// <param name="table">The database table of interest</param>
        /// <param name="foreignKey">The ForeignKey for which you need info.</param>
        public TableRelation(ITable table, IForeignKey foreignKey)
        {
            // The main sources of evil
            _ForeignKey = foreignKey;
            _CurrentTable = table;

            // A helper member to optimize speed of analysis
            _AllFksArePks = true;
            for (int j = 0; j < _ForeignKey.ForeignColumns.Count; j++)
            {
                IColumn fkColumn = _ForeignKey.ForeignColumns[j];
                IColumn pkColumn = _ForeignKey.PrimaryColumns[j];

                if (!fkColumn.IsInPrimaryKey || !pkColumn.IsInPrimaryKey)
                {
                    _AllFksArePks = false;
                }
            }

            // Another helper to optimize speed
            _ForeignTableHasRequiredFields = false;
            for (int j = 0; j < _ForeignKey.ForeignTable.Columns.Count; j++)
            {
                IColumn column = _ForeignKey.ForeignTable.Columns[j];

                if (!column.IsInPrimaryKey && !column.HasDefault && !column.IsComputed)
                {
                    _ForeignTableHasRequiredFields = true;
                    break;
                }
            }

            _IsDirect = _CurrentTable.Name == _ForeignKey.ForeignTable.Name;
        }

        /// <summary>
        /// The type of Relationship in the ForeignKey.
        /// One of the RelType Enums.
        /// See <see cref="RelType" />.
        /// </summary>
        /// <example>
        /// <code>
        /// switch(tr.RelationType.ToString())
        /// {
        /// 	case "OneToOne":
        /// 		// Handle OneToOne
        /// 		break;
        /// 	case "SelfReference":
        /// 		// Handle SelfReference
        /// 		break;
        /// 	case "ManyToMany":
        /// 		// Handle ManyToMany
        /// 		break;
        /// 	case "ZeroToManyOnly":
        /// 		// Handle ZeroToManyOnly
        /// 		break;
        /// 	case "ManyToOneOnly":
        /// 		// Handle ManyToOneOnly
        /// 		break;
        /// 	default:
        /// 	{
        /// 		output.writeln("What is this thing you speak of - RelationType?");
        /// 		break;
        /// 	}
        /// }
        /// </code>
        /// </example>
        public RelType RelationType
        {
            get
            {
                RelType returnValue = RelType.ManyToOneOnly;

                if (IsOneToOne)
                {
                    returnValue = RelType.OneToOne;
                }
                else if (IsSelfReference)
                {
                    returnValue = RelType.SelfReference;
                }
                else if (IsManyToMany)
                {
                    returnValue = RelType.ManyToMany;
                }
                else if (IsZeroToMany)
                {
                    returnValue = RelType.ZeroToManyOnly;
                }

                return returnValue;
            }
        }

        /// <summary>
        /// The ForeignKey name (e.g., FK_Orders_Employees)
        /// </summary>
        /// <example>
        /// See the Table Relation Example templates in the
        /// DNP.PluginExamples namespace for more details.
        /// <code>
        /// // Loop through the tables selected in the template UI.
        /// foreach (string tableName in Tables)
        /// {
        /// 	// Get MyMeta information for a table.
        /// 	tableMeta = MyMeta.Databases[DatabaseName].Tables[tableName];
        /// 
        /// 	// Loop through the ForeignKeys in the table
        /// 	foreach( IForeignKey fk in tableMeta.ForeignKeys )
        /// 	{
        /// 		// Get the TableRelation properties for the specific table and ForeignKey.
        /// 		Dnp.Utils.TableRelation tr = new Dnp.Utils.TableRelation(tableMeta, fk);
        /// 
        /// 		// Use one of the many properties in your template.
        ///			output.autoTabLn(tr.Name);
        /// 	}
        /// }
        /// </code>
        /// Typical Output:
        /// <code>
        /// FK_Orders_Employees
        /// FK_Orders_Shippers
        /// FK_Orders_Customers
        /// </code>
        /// </example>
        public string Name
        {
            get
            {
                return _ForeignKey.Name;
            }
        }

        /// <summary>
        /// The ForeignKey alias
        /// </summary>
        public string Alias
        {
            get
            {
                return _ForeignKey.Alias;
            }
        }

        #region Relationship tests
        /// <summary>
        /// The ForeignKey describes a one-to-one link
        /// </summary>
        /// <example>
        /// Northwind does not contain an example of a One-To-One
        /// relationship. A normalized database would not usually
        /// have tables related One-To-One. You would just combine the
        /// tables into one table containing all the fields.
        /// De-normalizing the database and adding a new table with a
        /// One-To-One relationship to an existing table is one way
        /// to extend a third-party database in anticipation of
        /// future releases from the vendor. One-To-One may also be necessary
        /// to overcome database limitations on the number of columns in a table.
        /// </example>
        public bool IsOneToOne
        {
            get
            {
                bool result = false;
                // TODO: document the rationale of the if statement below
                if (_AllFksArePks && _CurrentTable.PrimaryKeys.Count == this.ForeignTable.PrimaryKeys.Count)
                {
                    result = true;
                }
                if (IsSelfReference)
                {
                    result = false;
                }
                return result;
            }
        }

        /// <summary>
        /// The ForeignKey describes a zero-to-many link
        /// </summary>
        /// <example>
        /// ManyToOne is a common ForeignKey constraint often referred to
        /// as a Lookup. It will always have a corresponding ZeroToMany
        /// in the other table. An Order is placed
        /// by one Employee (IsManyToOne is true in Orders, IsLookup is true.)
        /// An Employee can have many orders, but is not required
        /// to have any (IsZeroToMany is true in Employees.)
        /// <code>
        ///              PrimaryTable.Name : Employees
        ///                           Name : FK_Orders_Employees
        ///              ForeignTable.Name : Orders
        ///                     ObjectType : Orders
        ///                    ColumnCount : 1
        ///                 PrimaryColumns : EmployeeID
        ///                 ForeignColumns : EmpID
        ///  ForeignTableHasRequiredFields : True
        ///                       IsDirect : False
        ///                IsSelfReference : False
        ///                     IsOneToOne : False
        ///                   IsZeroToMany : True
        ///                    IsManyToOne : False
        ///                   IsManyToMany : False
        ///                 ReferencedName : ReferencedOrdersAsEmpID
        ///                  ReferringName : ReferringOrdersAsEmpID
        ///                       IsLookup : False
        ///                     LookupName : LookupOrdersAsEmpID
        ///               IsCrossReference : False
        /// ------------------------------ : ----------------------------------------
        ///              PrimaryTable.Name : Orders
        ///                           Name : FK_Orders_Employees
        ///              ForeignTable.Name : Employees
        ///                     ObjectType : Employees
        ///                    ColumnCount : 1
        ///                 PrimaryColumns : EmpID
        ///                 ForeignColumns : EmployeeID
        ///  ForeignTableHasRequiredFields : True
        ///                       IsDirect : True
        ///                IsSelfReference : False
        ///                     IsOneToOne : False
        ///                   IsZeroToMany : False
        ///                    IsManyToOne : True
        ///                   IsManyToMany : False
        ///                 ReferencedName : ReferencedEmployeesAsEmpID
        ///                  ReferringName : ReferringEmployeesAsEmpID
        ///                       IsLookup : True
        ///                     LookupName : LookupEmployeesAsEmpID
        ///               IsCrossReference : False
        /// ------------------------------ : ----------------------------------------
        /// </code>
        /// </example>
        public bool IsZeroToMany
        {
            get
            {
                bool result = (_ForeignKey.PrimaryTable.Name == _CurrentTable.Name) && (_ForeignKey.ForeignTable.Name != _CurrentTable.Name);
                // TODO: document the rationale of the if statements below
                if (_AllFksArePks && _CurrentTable.PrimaryKeys.Count == this.ForeignTable.PrimaryKeys.Count)
                {
                    result = false;
                }
                if (IsSelfReference)
                {
                    result = true;
                }
                return result;
            }
        }

        /// <summary>
        /// The ForeignKey describes a many-to-one link
        /// </summary>
        /// <example>
        /// ManyToOne is a common ForeignKey constraint often referred to
        /// as a Lookup. It will always have a corresponding ZeroToMany
        /// in the other table. An Order is placed
        /// by one Employee (IsManyToOne is true in Orders, IsLookup is true.)
        /// An Employee can have many orders, but is not required
        /// to have any (IsZeroToMany is true in Employees.)
        /// While IsLookup and IsManyToOne are essentially the same test,
        /// the distinction is useful when using the naming properties.
        /// <code>
        ///              PrimaryTable.Name : Employees
        ///                           Name : FK_Orders_Employees
        ///              ForeignTable.Name : Orders
        ///                     ObjectType : Orders
        ///                    ColumnCount : 1
        ///                 PrimaryColumns : EmployeeID
        ///                 ForeignColumns : EmpID
        ///  ForeignTableHasRequiredFields : True
        ///                       IsDirect : False
        ///                IsSelfReference : False
        ///                     IsOneToOne : False
        ///                   IsZeroToMany : True
        ///                    IsManyToOne : False
        ///                   IsManyToMany : False
        ///                 ReferencedName : ReferencedOrdersAsEmpID
        ///                  ReferringName : ReferringOrdersAsEmpID
        ///                       IsLookup : False
        ///                     LookupName : LookupOrdersAsEmpID
        ///               IsCrossReference : False
        /// ------------------------------ : ----------------------------------------
        ///              PrimaryTable.Name : Orders
        ///                           Name : FK_Orders_Employees
        ///              ForeignTable.Name : Employees
        ///                     ObjectType : Employees
        ///                    ColumnCount : 1
        ///                 PrimaryColumns : EmpID
        ///                 ForeignColumns : EmployeeID
        ///  ForeignTableHasRequiredFields : True
        ///                       IsDirect : True
        ///                IsSelfReference : False
        ///                     IsOneToOne : False
        ///                   IsZeroToMany : False
        ///                    IsManyToOne : True
        ///                   IsManyToMany : False
        ///                 ReferencedName : ReferencedEmployeesAsEmpID
        ///                  ReferringName : ReferringEmployeesAsEmpID
        ///                       IsLookup : True
        ///                     LookupName : LookupEmployeesAsEmpID
        ///               IsCrossReference : False
        /// ------------------------------ : ----------------------------------------
        /// </code>
        /// </example>
        public bool IsManyToOne
        {
            get
            {
                bool result = (_ForeignKey.ForeignTable.Name == _CurrentTable.Name) && (_ForeignKey.PrimaryTable.Name != _CurrentTable.Name);
                // TODO: document the rationale of the if statements below
                if (_AllFksArePks && _CurrentTable.PrimaryKeys.Count == this.ForeignTable.PrimaryKeys.Count)
                {
                    result = false;
                }

                if (IsSelfReference)
                {
                    result = true;
                }
                return result;
            }
        }

        /// <summary>
        /// The ForeignKey describes a many-to-many link
        /// </summary>
        /// <example>
        /// Most databases do not handle ManyToMany relationships directly.
        /// An intermediate linking table is used with two ManyToOne
        /// relationships. EmployeeTerritories (below) has two ManyToOne
        /// ForeignKeys (to Employees and to Territories.) As you
        /// would expect, Employees and Territories both report ZeroToMany
        /// relationships with EmployeeTerritories. But, TableRelation
        /// recognizes this special case as a ManyToMany. For both
        /// Employees and Territories, IsManyToMany is also true. When IsManyToMany
        /// is true, then a number of additional properties are available.
        /// The ForeignTable acts as a link to the CrossReferenceTable.
        /// <para>IsCrossReference is a special case of ManyToMany. It
        /// will report true if the ForeignTable fields
        /// are all in the PrimaryKey, are calculated, or have defaults.
        /// Let's say we were to add an optional Notes field in
        /// EmployeeTerritories and re-run the report. IsManyToMany
        /// would still be true for Employees and Territories,
        /// but IsCrossReference would be false. It is alerting you
        /// that the ForeignTable (EmployeeTerritories) has fields that
        /// may need to be filled in by the user in the UI. It is not
        /// a simple mechanical linking table.
        /// </para>
        /// Be sure to test for IsManyToMany
        /// before trying to use any of the following properties:
        /// <code>
        /// CrossReferenceTable
        /// CrossReferenceColumnCount
        /// CrossReferenceColumns
        /// LinkingColumns
        /// Linking Name
        /// </code>
        /// This is how things look from the Employees table.
        /// <list type="table">
        /// <listheader>
        /// <term>PrimaryTable</term>
        /// <term>&lt;--ForeignTable--&gt;</term>
        /// <term>CrossReferenceTable</term>
        /// </listheader>
        /// <item>
        /// <description>Employees</description>
        /// <description>&lt;--EmployeeTerritories--&gt;</description>
        /// <description>Territories</description>
        /// </item>
        /// <item>
        /// <description>PrimaryColumns</description>
        /// <description>&lt;--ForeignColumns:LinkingColumns--&gt;</description>
        /// <description>CrossReferenceColumns</description>
        /// </item>
        /// <item>
        /// <description>EmployeeID</description>
        /// <description>&lt;--EmployeeID:TerritoryID--&gt;</description>
        /// <description>TerritoryID</description>
        /// </item>
        /// </list>
        /// Note the additional properties beneath 'IsCrossReference' in the report excerpt below.
        /// They are available to any Many-To-Many ForeignKey
        /// (regardless of whether IsCrossReference is true or not.)
        /// <code>
        ///              PrimaryTable.Name : Employees
        ///                           Name : FK_EmployeeTerritories_Employees
        ///              ForeignTable.Name : EmployeeTerritories
        ///                     ObjectType : EmployeeTerritories
        ///                    ColumnCount : 1
        ///                 PrimaryColumns : EmployeeID
        ///                 ForeignColumns : EmployeeID
        ///  ForeignTableHasRequiredFields : False
        ///                       IsDirect : False
        ///                IsSelfReference : False
        ///                     IsOneToOne : False
        ///                   IsZeroToMany : True
        ///                    IsManyToOne : False
        ///                   IsManyToMany : True
        ///                 ReferencedName : ReferencedTerritoriesUsingEmployeeTerritoriesAsEmployeeID
        ///                  ReferringName : ReferringEmployeeTerritoriesAsEmployeeID
        ///                       IsLookup : False
        ///                     LookupName : LookupEmployeeTerritoriesAsEmployeeID
        ///               IsCrossReference : True
        ///       CrossReferenceTable.Name : Territories
        ///      CrossReferenceColumnCount : 1
        ///          CrossReferenceColumns : TerritoryID
        ///                 LinkingColumns : TerritoryID
        ///                   Linking Name : LinkingEmployeeTerritoriesAsEmployeeID
        /// ------------------------------ : ----------------------------------------
        ///              PrimaryTable.Name : EmployeeTerritories
        ///                           Name : FK_EmployeeTerritories_Employees
        ///              ForeignTable.Name : Employees
        ///                     ObjectType : Employees
        ///                    ColumnCount : 1
        ///                 PrimaryColumns : EmployeeID
        ///                 ForeignColumns : EmployeeID
        ///  ForeignTableHasRequiredFields : False
        ///                       IsDirect : True
        ///                IsSelfReference : False
        ///                     IsOneToOne : False
        ///                   IsZeroToMany : False
        ///                    IsManyToOne : True
        ///                   IsManyToMany : False
        ///                 ReferencedName : ReferencedEmployeesAsEmployeeID
        ///                  ReferringName : ReferringEmployeesAsEmployeeID
        ///                       IsLookup : True
        ///                     LookupName : LookupEmployeesAsEmployeeID
        ///               IsCrossReference : False
        /// ------------------------------ : ----------------------------------------
        ///              PrimaryTable.Name : EmployeeTerritories
        ///                           Name : FK_EmployeeTerritories_Territories
        ///              ForeignTable.Name : Territories
        ///                     ObjectType : Territories
        ///                    ColumnCount : 1
        ///                 PrimaryColumns : TerritoryID
        ///                 ForeignColumns : TerritoryID
        ///  ForeignTableHasRequiredFields : False
        ///                       IsDirect : True
        ///                IsSelfReference : False
        ///                     IsOneToOne : False
        ///                   IsZeroToMany : False
        ///                    IsManyToOne : True
        ///                   IsManyToMany : False
        ///                 ReferencedName : ReferencedTerritoriesAsTerritoryID
        ///                  ReferringName : ReferringTerritoriesAsTerritoryID
        ///                       IsLookup : True
        ///                     LookupName : LookupTerritoriesAsTerritoryID
        ///               IsCrossReference : False
        /// ------------------------------ : ----------------------------------------
        ///              PrimaryTable.Name : Territories
        ///                           Name : FK_EmployeeTerritories_Territories
        ///              ForeignTable.Name : EmployeeTerritories
        ///                     ObjectType : EmployeeTerritories
        ///                    ColumnCount : 1
        ///                 PrimaryColumns : TerritoryID
        ///                 ForeignColumns : TerritoryID
        ///  ForeignTableHasRequiredFields : False
        ///                       IsDirect : False
        ///                IsSelfReference : False
        ///                     IsOneToOne : False
        ///                   IsZeroToMany : True
        ///                    IsManyToOne : False
        ///                   IsManyToMany : True
        ///                 ReferencedName : ReferencedEmployeesUsingEmployeeTerritoriesAsTerritoryID
        ///                  ReferringName : ReferringEmployeeTerritoriesAsTerritoryID
        ///                       IsLookup : False
        ///                     LookupName : LookupEmployeeTerritoriesAsTerritoryID
        ///               IsCrossReference : True
        ///       CrossReferenceTable.Name : Employees
        ///      CrossReferenceColumnCount : 1
        ///          CrossReferenceColumns : EmployeeID
        ///                 LinkingColumns : EmployeeID
        ///                   Linking Name : LinkingEmployeeTerritoriesAsTerritoryID
        /// </code>
        /// </example>
        public bool IsManyToMany
        {
            get
            {
                // TODO: document the rationale of the if statement below
                if (_AllFksArePks && _CurrentTable.PrimaryKeys.Count < this.ForeignTable.PrimaryKeys.Count)
                {
                    for (int j = 0; j < this.ForeignTable.ForeignKeys.Count; j++)
                    {
                        IForeignKey fk = this.ForeignTable.ForeignKeys[j];

                        if ((fk.PrimaryTable.Name != _CurrentTable.Name) &&
                            (fk.PrimaryTable.Name != this.ForeignTable.Name))
                        {
                            if (fk.ForeignColumns[0].IsInPrimaryKey)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// A direct, or local, foreign key. This is used internally
        /// by TableRelation and helps determine which side of the key
        /// the current table is looking from.
        /// </summary>
        public bool IsDirect
        {
            get
            {
                return _IsDirect;
            }
        }

        /// <summary>
        /// A self-referencing ForeignKey like Northwind
        /// Employees ReportsTo
        /// </summary>
        /// <example>
        /// This is a special circumstance where a ManyToOne constraint
        /// refers to the same table. Employees ReportsTo (below) is an
        /// example. If IsSelfReference is true, then IsManyToOne, IsZeroToMany,
        /// and IsLookup will all be true for the same ForeignKey.
        /// An Employee reports to one Employee (IsManyToOne.) An Employee can have many
        /// Employees reporting to him, but may not have any (IsZeroToMany.)
        /// <code>
        ///              PrimaryTable.Name : Employees
        ///                           Name : FK_Employees_Employees
        ///              ForeignTable.Name : Employees
        ///                     ObjectType : Employees
        ///                    ColumnCount : 1
        ///                 PrimaryColumns : ReportsTo
        ///                 ForeignColumns : EmployeeID
        ///  ForeignTableHasRequiredFields : True
        ///                       IsDirect : True
        ///                IsSelfReference : True
        ///                     IsOneToOne : False
        ///                   IsZeroToMany : True
        ///                    IsManyToOne : True
        ///                   IsManyToMany : False
        ///                 ReferencedName : ReferencedEmployeesAsReportsTo
        ///                  ReferringName : ReferringEmployeesAsReportsTo
        ///                       IsLookup : True
        ///                     LookupName : LookupEmployeesAsReportsTo
        ///               IsCrossReference : False
        /// </code>
        /// </example>
        public bool IsSelfReference
        {
            get
            {
                return _ForeignKey.PrimaryTable.Name == _ForeignKey.ForeignTable.Name;
            }
        }

        /// <summary>
        /// The ForeignKey describes a Lookup
        /// </summary>
        /// <example>
        /// ManyToOne is a common ForeignKey constraint often referred to
        /// as a Lookup. It will always have a corresponding ZeroToMany
        /// in the other table. An Order is placed
        /// by one Employee (IsManyToOne is true in Orders, IsLookup is true.)
        /// An Employee can have many orders, but is not required
        /// to have any (IsZeroToMany is true in Employees.)
        /// While IsLookup and IsManyToOne are essentially the same test,
        /// the distinction is useful when using the naming properties.
        /// <code>
        ///              PrimaryTable.Name : Employees
        ///                           Name : FK_Orders_Employees
        ///              ForeignTable.Name : Orders
        ///                     ObjectType : Orders
        ///                    ColumnCount : 1
        ///                 PrimaryColumns : EmployeeID
        ///                 ForeignColumns : EmpID
        ///  ForeignTableHasRequiredFields : True
        ///                       IsDirect : False
        ///                IsSelfReference : False
        ///                     IsOneToOne : False
        ///                   IsZeroToMany : True
        ///                    IsManyToOne : False
        ///                   IsManyToMany : False
        ///                 ReferencedName : ReferencedOrdersAsEmpID
        ///                  ReferringName : ReferringOrdersAsEmpID
        ///                       IsLookup : False
        ///                     LookupName : LookupOrdersAsEmpID
        ///               IsCrossReference : False
        /// ------------------------------ : ----------------------------------------
        ///              PrimaryTable.Name : Orders
        ///                           Name : FK_Orders_Employees
        ///              ForeignTable.Name : Employees
        ///                     ObjectType : Employees
        ///                    ColumnCount : 1
        ///                 PrimaryColumns : EmpID
        ///                 ForeignColumns : EmployeeID
        ///  ForeignTableHasRequiredFields : True
        ///                       IsDirect : True
        ///                IsSelfReference : False
        ///                     IsOneToOne : False
        ///                   IsZeroToMany : False
        ///                    IsManyToOne : True
        ///                   IsManyToMany : False
        ///                 ReferencedName : ReferencedEmployeesAsEmpID
        ///                  ReferringName : ReferringEmployeesAsEmpID
        ///                       IsLookup : True
        ///                     LookupName : LookupEmployeesAsEmpID
        ///               IsCrossReference : False
        /// ------------------------------ : ----------------------------------------
        /// </code>
        /// </example>
        public bool IsLookup
        {
            get
            {
                bool result = (_ForeignKey.ForeignTable.Name == _CurrentTable.Name) && (_ForeignKey.PrimaryTable.Name != _CurrentTable.Name);
                if (_AllFksArePks && _CurrentTable.PrimaryKeys.Count == this.ForeignTable.PrimaryKeys.Count)
                {
                    result = false;
                }

                if (IsSelfReference)
                {
                    result = true;
                }
                return result;
            }
        }

        /// <summary>
        /// The Foreign Table has non-primary key fields
        /// without a default or computed value.
        /// This is used internally by TableRelation as part
        /// of the IsCrossReference test.
        /// </summary>
        public bool ForeignTableHasRequiredFields
        {
            get
            {
                return _ForeignTableHasRequiredFields;
            }
        }

        /// <summary>
        /// The ForeignTable is a simple Cross-Reference object.
        /// All fields in the foreign table are either
        /// in the primary key, are calculated, or
        /// have defaults.
        /// </summary>
        /// <example>
        /// IsCrossReference is a special case of ManyToMany. It
        /// will report true if the ForeignTable fields
        /// are all in the PrimaryKey, are calculated, or have defaults.
        /// Let's say we were to add an optional Notes field in
        /// EmployeeTerritories and re-run the report below. IsManyToMany
        /// would still be true for Employees,
        /// but IsCrossReference would be false. It is alerting you
        /// that the ForeignTable (EmployeeTerritories) has fields that
        /// may need to be filled in by the user in the UI. It is not
        /// a simple mechanical linking table.
        /// <code>
        ///              PrimaryTable.Name : Employees
        ///                           Name : FK_EmployeeTerritories_Employees
        ///              ForeignTable.Name : EmployeeTerritories
        ///                     ObjectType : EmployeeTerritories
        ///                    ColumnCount : 1
        ///                 PrimaryColumns : EmployeeID
        ///                 ForeignColumns : EmployeeID
        ///  ForeignTableHasRequiredFields : False
        ///                       IsDirect : False
        ///                IsSelfReference : False
        ///                     IsOneToOne : False
        ///                   IsZeroToMany : True
        ///                    IsManyToOne : False
        ///                   IsManyToMany : True
        ///                 ReferencedName : ReferencedTerritoriesUsingEmployeeTerritoriesAsEmployeeID
        ///                  ReferringName : ReferringEmployeeTerritoriesAsEmployeeID
        ///                       IsLookup : False
        ///                     LookupName : LookupEmployeeTerritoriesAsEmployeeID
        ///               IsCrossReference : True
        ///       CrossReferenceTable.Name : Territories
        ///      CrossReferenceColumnCount : 1
        ///          CrossReferenceColumns : TerritoryID
        ///                 LinkingColumns : TerritoryID
        ///                   Linking Name : LinkingEmployeeTerritoriesAsEmployeeID
        /// </code>
        /// </example>
        public bool IsCrossReference
        {
            get
            {
                return (!_ForeignTableHasRequiredFields && IsManyToMany);
            }
        }
        #endregion

        #region Tables, Columns, and ObjectTypes
        /// <summary>
        /// The ForeignTable alias which can be used as an
        /// ObjectType. It is set to PascalCase for you.
        /// </summary>
        /// <example>
        /// The following code in the template:
        /// <code>
        /// public &lt;%=tr.ObjectType &gt; &lt;%=tr.ReferencedName &gt;()
        /// </code>
        /// Would output this:
        /// <code>
        /// public Employees ReferencedEmployeesAsReportsTo()
        /// </code>
        /// </example>
        public string ObjectType
        {
            get
            {
                return theUtils.SetPascalCase(this.ForeignTable.Alias, this.ForeignTable.Database.Root.Settings);
            }
        }

        /// <summary>
        /// This is always the table that was passed in
        /// when the TableRelation was constructed.
        /// Also see <see cref="CrossReferenceTable" /> and
        /// <see cref="ForeignTable" />.
        /// </summary>
        public ITable PrimaryTable
        {
            get
            {
                return _CurrentTable;
            }
        }

        /// <summary>
        /// Columns involved from the Primary Table.
        /// See <see cref="LinkingColumns" />.
        /// </summary>
        public IColumns PrimaryColumns
        {
            get
            {
                if (_IsDirect)
                {
                    return _ForeignKey.ForeignColumns;
                }
                else
                {
                    return _ForeignKey.PrimaryColumns;
                }
            }
        }

        /// <summary>
        /// This is the table other than the table that
        /// was passed in when the TableRelation was constructed.
        /// Also see <see cref="CrossReferenceTable" /> and
        /// <see cref="PrimaryTable" />.
        /// </summary>
        public ITable ForeignTable
        {
            get
            {
                if (_IsDirect)
                {
                    return _ForeignKey.PrimaryTable;
                }
                else
                {
                    return _ForeignKey.ForeignTable;
                }
            }
        }

        /// <summary>
        /// Columns involved from the Foreign Table.
        /// See <see cref="LinkingColumns" />.
        /// </summary>
        public IColumns ForeignColumns
        {
            get
            {
                if (_IsDirect)
                {
                    return _ForeignKey.PrimaryColumns;
                }
                else
                {
                    return _ForeignKey.ForeignColumns;
                }
            }
        }

        /// <summary>
        /// The number of Columns in the ForeignKey
        /// </summary>
        public int ColumnCount
        {
            get
            {
                if (_IsDirect)
                {
                    return _ForeignKey.ForeignColumns.Count;
                }
                else
                {
                    return _ForeignKey.PrimaryColumns.Count;
                }
            }
        }

        /// <summary>
        /// The cross-reference table from a many-to-many link.
        /// This is only valid for many-to-many relationships.
        /// </summary>
        /// <example>
        /// <code>
        /// PrimaryTable   ForeignTable          CrossReferenceTable
        /// Employees      EmployeeTerritories   Territories
        /// </code>
        /// Be sure to check IsManyToMany in your template
        /// before you retrieve this.
        /// <code>
        /// if(tr.IsManyToMany)
        /// {
        /// 	output.autoTabLn("Cross Reference Table: " + tr.CrossReferenceTable.Alias);
        /// }
        /// </code>
        /// </example>
        public ITable CrossReferenceTable
        {
            get
            {
                if (this.IsManyToMany)
                {
                    for (int j = 0; j < this.ForeignTable.ForeignKeys.Count; j++)
                    {
                        IForeignKey fk = this.ForeignTable.ForeignKeys[j];

                        if ((fk.PrimaryTable.Name != _CurrentTable.Name) &&
                            (fk.PrimaryTable.Name != this.ForeignTable.Name))
                        {
                            if (fk.ForeignColumns[0].IsInPrimaryKey)
                            {
                                return fk.PrimaryTable;
                            }
                        }
                    }
                    return null;
                }
                return null;
            }
        }

        /// <summary>
        /// Columns involved from the CrossReference Table.
        /// See <see cref="LinkingColumns" />.
        /// Be sure to check IsManyToMany in your template
        /// before you retrieve this.
        /// </summary>
        public IColumns CrossReferenceColumns
        {
            get
            {
                if (this.IsManyToMany)
                {
                    for (int j = 0; j < this.ForeignTable.ForeignKeys.Count; j++)
                    {
                        IForeignKey fk = this.ForeignTable.ForeignKeys[j];

                        if ((fk.PrimaryTable.Name != _CurrentTable.Name) &&
                            (fk.PrimaryTable.Name != this.ForeignTable.Name))
                        {
                            if (fk.ForeignColumns[0].IsInPrimaryKey)
                            {
                                return fk.PrimaryColumns;
                            }
                        }
                    }
                    return null;
                }
                return null;
            }
        }

        /// <summary>
        /// Columns in the Foreign Table that link to the CrossReference Table.
        /// </summary>
        /// <example>
        /// <code>
        /// PrimaryTable             ForeignTable             CrossReferenceTable
        ///  Employees            EmployeeTerritories            Territories
        /// PrimaryColumns   ForeignColumns:LinkingColumns    CrossReferenceColumns
        ///  EmployeeID          EmployeeID:TerritoryID          TerritoryID
        /// </code>
        /// Be sure to check IsManyToMany in your template
        /// before you retrieve this.
        /// </example>
        public IColumns LinkingColumns
        {
            get
            {
                if (this.IsManyToMany)
                {
                    for (int j = 0; j < this.ForeignTable.ForeignKeys.Count; j++)
                    {
                        IForeignKey fk = this.ForeignTable.ForeignKeys[j];

                        if ((fk.PrimaryTable.Name != _CurrentTable.Name) &&
                            (fk.PrimaryTable.Name != this.ForeignTable.Name))
                        {
                            if (fk.ForeignColumns[0].IsInPrimaryKey)
                            {
                                return fk.ForeignColumns;
                            }
                        }
                    }
                    return null;
                }
                return null;
            }
        }

        /// <summary>
        /// The number of Columns involved from the
        /// CrossReference Table.
        /// See <see cref="LinkingColumns" />.
        /// Be sure to check IsManyToMany in your template
        /// before you retrieve this.
        /// </summary>
        public int CrossReferenceColumnCount
        {
            get
            {
                if (this.IsManyToMany)
                {
                    for (int j = 0; j < this.ForeignTable.ForeignKeys.Count; j++)
                    {
                        IForeignKey fk = this.ForeignTable.ForeignKeys[j];

                        if ((fk.PrimaryTable.Name != _CurrentTable.Name) &&
                            (fk.PrimaryTable.Name != this.ForeignTable.Name))
                        {
                            if (fk.ForeignColumns[0].IsInPrimaryKey)
                            {
                                return fk.PrimaryColumns.Count;
                            }
                        }
                    }
                    return 0;
                }
                return 0;
            }
        }

        #endregion

        #region Unique Names
        /// <summary>
        /// Returns a unique name for the relationship.
        /// It is a combination of foreign table name and
        /// column name(set to Pascal Case.)
        /// See <see cref="ReferencedName"/>
        /// </summary>
        /// <example>
        /// For example, self-referencing foreign keys contain
        /// 2 relationships which can be used in 3 ways:
        /// <code>
        /// LookupName - ManyToOne
        ///   Could be used in a lookup ComboBox for
        ///   Northwind Employees ReportsTo
        /// ReferencedName - ManyToOne
        ///   Could be used to get additional info
        ///   for the specific Employee retrieved
        ///   in the ComboBox above
        /// ReferringName - ZeroToMany
        ///   Could be used to populate a DataGrid
        ///   of Employees Reporting to the current Employee
        /// </code>
        /// By chosing:
        /// <code>
        /// LookupName - LookupEmployeesAsReportsTo
        /// ReferencedName - ReferencedEmployeesAsReportsTo
        /// ReferringName - ReferringEmployeesAsReportsTo
        /// </code>
        /// you can have a unique name for all methods.
        /// </example>
        public string ReferringName
        {
            get
            {
                if (_ForeignKey.Alias != _ForeignKey.Name)
                {
                    return theUtils.SetPascalCase("Referring" + _ForeignKey.Alias, 
                        _ForeignKey.PrimaryTable.Database.Root.Settings);
                }
                else
                {
                    return theUtils.SetPascalCase("Referring" + this.ForeignTable.Alias + "As" + _ForeignKey.ForeignColumns[0].Alias,
                        _ForeignKey.PrimaryTable.Database.Root.Settings);
                }
            }
        }

        /// <summary>
        /// Returns a unique name for the relationship.
        /// It is a combination of foreign table name and
        /// column name (set to Pascal Case.)
        /// See <see cref="ReferringName"/>
        /// </summary>
        /// <example>
        /// For example, ManyToMany relationships are generally
        /// constructed using 3 tables:
        /// <code>
        /// PrimaryTable - Employees
        /// ForeignTable - EmployeeTerritories
        /// CrossReferenceTable - Territories
        /// </code>
        /// To get a unique name for each relationship:
        /// <code>
        /// ReferencedName - ManyToMany
        ///   Links thru to the CrossReferenced table
        ///   Northwind Employees to Territories
        /// ReferringName - ZeroToMany
        ///   To the intermediate linking table
        ///   Northwind Employees to EmployeeTerritories
        /// LinkingName - ManyToOne
        ///   The link from the intermediate linking table
        ///   to the CrossReferenceTable
        ///   Northwind EmployeeTerritories to Territories
        /// </code>
        /// By chosing:
        /// <code>
        /// ReferencedName - ReferencedTerritoriesUsingEmployeeTerritoriesAsEmployeeID
        /// ReferringName - ReferringEmployeeTerritoriesAsEmployeeID
        /// LinkingName - LinkingEmployeeTerritoriesAsEmployeeID
        /// </code>
        /// you can have a unique name for all methods.
        /// <para>The following code loops through the ForeignKeys
        /// in selected tables and generates a function for
        /// every ManyToOne it finds.</para>
        /// C#
        /// <code>
        /// // Loop through the tables selected in the template UI.
        /// foreach (string tableName in Tables)
        /// {
        /// 	// Get MyMeta information for a table.
        /// 	tableMeta = MyMeta.Databases[DatabaseName].Tables[tableName];
        /// 
        /// 	// Loop through the ForeignKeys in the table
        /// 	foreach( IForeignKey foreignKey in tableMeta.ForeignKeys )
        /// 	{
        /// 		// Get the TableRelation properties for the specific table and ForeignKey.
        /// 		Dnp.Utils.TableRelation tr = new Dnp.Utils.TableRelation(tableMeta, foreignKey);
        /// 
        /// 		// Use some of the TableRelation properties in your template.
        /// 		if(tr.IsManyToOne)
        /// 		{
        /// 			string textTypes = "";
        /// 			string methodTypes = "";
        /// 			string callTypes = "";
        /// 			string delimiter = "";
        /// 
        /// 			// Loop through the PrimaryColumns
        /// 			foreach(IColumn col in tr.PrimaryColumns)
        /// 			{
        /// 				textTypes += delimiter + DnpUtils.SetPascalCase(col.Alias);
        /// 				methodTypes += delimiter + col.LanguageType + " " + DnpUtils.SetCamelCase(col.Alias);
        /// 				callTypes += delimiter + DnpUtils.SetCamelCase(col.Alias);
        /// 				delimiter = ", ";
        /// 			} // Next PrimaryColumn
        /// 
        /// 			output.autoTabLn("");
        /// 			output.autoTabLn("        // Get a dOOdad for the row referenced by " + tr.PrimaryTable.Alias + " - " + textTypes);
        /// %&gt;		public &lt;%=tr.ObjectType %&gt; &lt;%=tr.ReferencedName %&gt;(&lt;%=methodTypes %&gt;)
        /// {
        /// 	&lt;%=DnpUtils.SetPascalCase(tr.ObjectType) %&gt; &lt;%=DnpUtils.SetCamelCase(tr.ObjectType) %&gt;Entity = new &lt;%=DnpUtils.SetPascalCase(tr.ObjectType) %&gt;();
        /// 	&lt;%=DnpUtils.SetCamelCase(tr.ObjectType) %&gt;Entity.LoadByPrimaryKey(&lt;%=callTypes%&gt;);
        /// 	return &lt;%=DnpUtils.SetCamelCase(tr.ObjectType) %&gt;Entity;
        /// }
        /// &lt;%
        /// 		} // End if
        /// 	} // Next ForeignKey
        /// } // Next Table
        /// </code>
        /// Running the code above against Northwind Orders would
        /// yield the following output:
        /// <code>
        /// // Get a dOOdad for the row referenced by Orders - CustomerID
        /// public Customers ReferencedCustomersAsCustomerID(string customerID)
        /// {
        /// 	Customers customersEntity = new Customers();
        /// 	customersEntity.LoadByPrimaryKey(customerID);
        /// 	return customersEntity;
        /// }
        /// 
        /// // Get a dOOdad for the row referenced by Orders - EmployeeID
        /// public Employees ReferencedEmployeesAsEmployeeID(int employeeID)
        /// {
        /// 	Employees employeesEntity = new Employees();
        /// 	employeesEntity.LoadByPrimaryKey(employeeID);
        /// 	return employeesEntity;
        /// }
        /// 
        /// // Get a dOOdad for the row referenced by Orders - ShipVia
        /// public Shippers ReferencedShippersAsShipVia(int shipVia)
        /// {
        /// 	Shippers shippersEntity = new Shippers();
        /// 	shippersEntity.LoadByPrimaryKey(shipVia);
        /// 	return shippersEntity;
        /// }
        /// </code>
        /// VB.NET
        /// <code>
        /// Dim tableMeta As ITable
        /// Dim tableName As String = ""
        /// Dim foreignKey As IForeignKey
        /// Dim col As IColumn
        /// 
        /// ' Loop through the tables selected in the template UI.
        /// For Each tableName in Tables
        /// 
        /// 	' Get MyMeta information for a table.
        /// 	tableMeta = MyMeta.Databases(DatabaseName).Tables(tableName)
        /// 
        /// 	' Loop through the ForeignKeys in the table
        /// 	For Each foreignKey In tableMeta.ForeignKeys
        /// 	
        /// 		' Get the TableRelation properties for the specific table and ForeignKey.
        /// 		Dim tr As Dnp.Utils.TableRelation = New Dnp.Utils.TableRelation(tableMeta, foreignKey)
        /// 		
        /// 		' Use some of the TableRelation properties in your template.
        /// 		If tr.IsManyToOne
        /// 			Dim textTypes As String = ""
        /// 			Dim methodTypes As String = ""
        /// 			Dim callTypes As String = ""
        /// 			Dim delimiter As String = ""
        /// 
        /// 			' Loop through the PrimaryColumns
        /// 			For Each col In tr.PrimaryColumns
        /// 				textTypes = textTypes + delimiter + DnpUtils.SetPascalCase(col.Alias)
        /// 				methodTypes = methodTypes + delimiter + "ByVal " + DnpUtils.SetCamelCase(col.Alias) + " As "  + col.LanguageType
        /// 				callTypes = callTypes + delimiter + DnpUtils.SetCamelCase(col.Alias)
        /// 				delimiter = ", "
        /// 			Next col
        /// 
        /// 			output.autoTabLn("")
        /// 			output.autoTabLn("        ' Get a dOOdad for the row referenced by " + tr.PrimaryTable.Alias + " - " + textTypes)
        /// %&gt;		Public Function &lt;%=tr.ReferencedName %&gt;(&lt;%=methodTypes %&gt;) As  &lt;%=tr.ObjectType %&gt;
        /// 	Dim &lt;%=DnpUtils.SetCamelCase(tr.ObjectType) %&gt;Entity As New &lt;%=DnpUtils.SetPascalCase(tr.ObjectType) %&gt;
        /// 	&lt;%=DnpUtils.SetCamelCase(tr.ObjectType) %&gt;Entity.LoadByPrimaryKey(&lt;%=callTypes%&gt;)
        /// 	Return &lt;%=DnpUtils.SetCamelCase(tr.ObjectType) %&gt;Entity
        /// End Function
        /// &lt;%
        /// 		End If
        /// 	Next foreignKey
        /// Next tableName
        /// </code>
        /// Running the code above against Northwind Orders would
        /// yield the following output:
        /// <code>
        /// ' Get a dOOdad for the row referenced by Orders - CustomerID
        /// Public Function ReferencedCustomersAsCustomerID(ByVal customerID As String) As  Customers
        /// 	Dim customersEntity As New Customers
        /// 	customersEntity.LoadByPrimaryKey(customerID)
        /// 	Return customersEntity
        /// End Function
        /// 
        /// ' Get a dOOdad for the row referenced by Orders - EmployeeID
        /// Public Function ReferencedEmployeesAsEmployeeID(ByVal employeeID As Integer) As  Employees
        /// 	Dim employeesEntity As New Employees
        /// 	employeesEntity.LoadByPrimaryKey(employeeID)
        /// 	Return employeesEntity
        /// End Function
        /// 
        /// ' Get a dOOdad for the row referenced by Orders - ShipVia
        /// Public Function ReferencedShippersAsShipVia(ByVal shipVia As Integer) As  Shippers
        /// 	Dim shippersEntity As New Shippers
        /// 	shippersEntity.LoadByPrimaryKey(shipVia)
        /// 	Return shippersEntity
        /// End Function
        /// </code>
        /// </example>
        public string ReferencedName
        {
            get
            {
                if (_ForeignKey.Alias != _ForeignKey.Name)
                {
                    return theUtils.SetPascalCase("Referenced" + _ForeignKey.Alias,
                        _ForeignKey.PrimaryTable.Database.Root.Settings);
                }
                else
                {
                    if (this.IsManyToMany)
                    {
                        return theUtils.SetPascalCase("Referenced" + this.CrossReferenceTable.Alias + "Using" + this.ForeignTable.Alias + "As" + _ForeignKey.ForeignColumns[0].Alias,
                            _ForeignKey.PrimaryTable.Database.Root.Settings);
                    }
                    else
                    {
                        return theUtils.SetPascalCase("Referenced" + this.ForeignTable.Alias + "As" + _ForeignKey.ForeignColumns[0].Alias,
                            _ForeignKey.PrimaryTable.Database.Root.Settings);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a unique name for the relationship.
        /// It is a combination of foreign table name and
        /// column name (set to Pascal Case.)
        /// </summary>
        /// <example>
        /// The intention is to create unique names for each relationship
        /// that can then be used in the template to provide function
        /// names.
        /// <list type="table">
        /// <listheader>
        /// <term>When this is true</term>
        /// <term>Use this name</term>
        /// <term>For this function</term>
        /// </listheader>
        /// <item>
        /// <description>IsManyToOne</description>
        /// <description>ReferencedName</description>
        /// <description>Retrieve the 1 related row.</description>
        /// </item>
        /// <item>
        /// <description>IsZeroToMany</description>
        /// <description>ReferringName</description>
        /// <description>Retrieve the many rows related to the current row.</description>
        /// </item>
        /// <item>
        /// <description>IsLookup</description>
        /// <description>LookupName</description>
        /// <description>Retrieve PrimaryKey column and display column for all rows for a ComboBox.</description>
        /// </item>
        /// <item>
        /// <description>***************</description>
        /// <description>***************</description>
        /// <description>************************************</description>
        /// </item>
        /// </list>
        /// See <see cref="ReferringName"/> for an example when IsSelfReference is true.
        /// See <see cref="ReferencedName" /> for an example when IsManyToMany is true.
        /// </example>
        public string LookupName
        {
            get
            {
                if (_ForeignKey.Alias != _ForeignKey.Name)
                {
                    return theUtils.SetPascalCase("Lookup" + _ForeignKey.Alias,
                        _ForeignKey.PrimaryTable.Database.Root.Settings);
                }
                else
                {
                    return theUtils.SetPascalCase("Lookup" + this.ForeignTable.Alias + "As" + _ForeignKey.ForeignColumns[0].Alias,
                        _ForeignKey.PrimaryTable.Database.Root.Settings);
                }
            }
        }

        /// <summary>
        /// Returns a unique name for the relationship.
        /// It is a combination of foreign table name and
        /// column name (set to Pascal Case.)
        /// See <see cref="ReferencedName"/>
        /// </summary>
        /// <example>
        /// The intention is to create unique names for each relationship
        /// that can then be used in the template to provide function
        /// names.
        /// <list type="table">
        /// <listheader>
        /// <term>When this is true</term>
        /// <term>Use this name</term>
        /// <term>For this function</term>
        /// </listheader>
        /// <item>
        /// <description>IsManyToOne</description>
        /// <description>ReferencedName</description>
        /// <description>Retrieve the 1 related row.</description>
        /// </item>
        /// <item>
        /// <description>IsZeroToMany</description>
        /// <description>ReferringName</description>
        /// <description>Retrieve the many rows related to the current row.</description>
        /// </item>
        /// <item>
        /// <description>IsLookup</description>
        /// <description>LookupName</description>
        /// <description>Retrieve PrimaryKey column and display column for all rows for a ComboBox.</description>
        /// </item>
        /// <item>
        /// <description>***************</description>
        /// <description>***************</description>
        /// <description>************************************</description>
        /// </item>
        /// </list>
        /// See <see cref="ReferringName"/> for an example when IsSelfReference is true.
        /// See <see cref="ReferencedName" /> for an example when IsManyToMany is true.
        /// </example>
        public string LinkingName
        {
            get
            {
                if (_ForeignKey.Alias != _ForeignKey.Name)
                {
                    return theUtils.SetPascalCase("Linking" + _ForeignKey.Alias, _ForeignKey.PrimaryTable.Database.Root.Settings);
                }
                else
                {
                    return theUtils.SetPascalCase("Linking" + this.ForeignTable.Alias + "As" + _ForeignKey.ForeignColumns[0].Alias,
                        _ForeignKey.PrimaryTable.Database.Root.Settings);
                }
            }
        }
        #endregion

    }
    #endregion
}
