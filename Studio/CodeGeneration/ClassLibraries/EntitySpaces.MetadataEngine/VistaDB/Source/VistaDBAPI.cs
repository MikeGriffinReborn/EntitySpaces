//============================================================================
//
// Copyright (c)2004 Vista Software
//
//============================================================================

using System;
using System.Runtime.InteropServices;
using System.Text;
//using System.Windows.Forms;
using System.Security;

namespace Provider.VistaDB
{
	/// <summary>
	/// Connection access mode
	/// </summary>
	public enum AccessMode
	{
		/// <summary>
		/// Local connection
		/// </summary>
		Local		= 0,
		/// <summary>
		/// Remote connection
		/// </summary>
		Remote	= 1
	}

	/// <summary>
	/// Used internally.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct RTagInfo
	{
		/// <summary>
		/// Size of structure in bytes
		/// </summary>
		public int iInfoSize;
		/// <summary>
		/// True if index active
		/// </summary>
		public bool bActive;
		/// <summary>
		/// Index order
		/// </summary>
		public int iOrderIndex;
		/// <summary>
		/// Obsolete
		/// </summary>
		public bool bHasParentIndex;
		/// <summary>
		/// True if index is unique
		/// </summary>
		public bool bUnique;
		/// <summary>
		/// True if index is primary key
		/// </summary>
		public bool bPrimary;
		/// <summary>
		/// True if index keys sorts in descending order
		/// </summary>
		public bool bDesc;
		/// <summary>
		/// Obsolete
		/// </summary>
		public bool bRYO;
		/// <summary>
		/// True if index is FTS index
		/// </summary>
		public bool bFts;
		/// <summary>
		/// True if index is standard
		/// </summary>
		public bool bStandardIndex;
		/// <summary>
		/// Reserved, not used for now
		/// </summary>
		public bool bCaseInsensitive;
		
		/// <summary>
		/// Index name
		/// </summary>
		[MarshalAs(UnmanagedType.LPStr)] public string cpTagName;

		/// <summary>
		/// Index key expression
		/// </summary>
		[MarshalAs(UnmanagedType.LPStr)] public string cpKeyExpr;

		/// <summary>
		/// Index condition expression
		/// </summary>
		[MarshalAs(UnmanagedType.LPStr)] public string cpForExpr;

		/// <summary>
		/// Index locale ID
		/// </summary>
		public int ulLocaleId;
	}

	/// <summary>
	/// Used internally.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct RTagBackInfo
	{
		/// <summary>
		/// Count of indexes
		/// </summary>
		public int iCount;
		/// <summary>
		/// Array of index information
		/// </summary>
		public RTagInfo[] pTagsInfo;
	}

	/// <summary>
	/// Types of encryption.
	/// </summary>
	public enum CypherType
	{
		/// <summary>
		/// Encryption not used
		/// </summary>
		None		= -1,
		/// <summary>
		/// Encrypt using Blowfish algorithm 
		/// </summary>
		Blowfish	= 0,
		/// <summary>
		/// Encrypt using DES algorithm
		/// </summary>
		DES			= 1
	}

	/// <summary>
	/// Data types supported (Column types).
	/// </summary>
	public enum VistaDBType
	{
		/// <summary>
		/// Character data type
		/// </summary>
		Character			= 0,
		/// <summary>
		/// Date data type
		/// </summary>
		Date				= 1,
		/// <summary>
		/// Date and time data type
		/// </summary>
		DateTime			= 2,
		/// <summary>
		/// Boolean data type
		/// </summary>
		Boolean				= 3,
		/// <summary>
		/// Memo data type
		/// </summary>
		Memo				= 4,
		/// <summary>
		/// Picture data type
		/// </summary>
		Picture				= 5,
		/// <summary>
		/// Blob data type
		/// </summary>
		Blob				= 6,
		/// <summary>
		/// Currency data type
		/// </summary>
		Currency			= 7,
		/// <summary>
		/// Int32 data type
		/// </summary>
		Int32				= 8,
		/// <summary>
		/// Int64 data type
		/// </summary>
		Int64				= 9,
		/// <summary>
		/// Double data type
		/// </summary>
		Double				= 10,
		/// <summary>
		/// Varchar data type
		/// </summary>
		Varchar				= 11,
		/// <summary>
		/// Guid data type
		/// </summary>
		Guid				= 12
	}

	/// <summary>
	/// Indexing options.
	/// </summary>
	public enum VDBIndexOption
	{
		/// <summary>
		/// V-Index
		/// </summary>
		VIndex				= 0,
		/// <summary>
		/// Standard unique index
		/// </summary>
		Unique				= 1 | 64,
		/// <summary>
		/// Unique V-Index
		/// </summary>
		UniqueVIndex		= 1,
		/// <summary>
		/// Standard index is not created immediately. 
		/// The creation is postponed until the option 
		/// is omited in a next CreateIndex
		/// </summary>
		Wait				= 04 | 64,
		/// <summary>
		/// V-Index is not created immediately. 
		/// The creation is postponed until the option 
		/// is omited in a next CreateIndex
		/// </summary>
		WaitVIndex			= 04,
		/// <summary>
		/// In-memory standard index
		/// </summary>
		Heap				= 08 | 64,
		/// <summary>
		/// Temporary V-Index
		/// </summary>
		HeapVIndex			= 08,
		/// <summary>
		/// Primary Key standard index
		/// </summary>
		Primary				= 16 | 64,
		/// <summary>
		/// FTS index
		/// </summary>
		FTS					= 32,
		/// <summary>
		/// Index is standard
		/// </summary>
		Standard			= 64,
		/// <summary>
		/// Case sensitive standard index
		/// </summary>
		Sensitive			= 1281 | 64,
		/// <summary>
		/// V-Index is case sensitive
		/// </summary>
		SensitiveVIndex		= 1281
	}

	/// <summary>
	/// Trigger events.
	/// </summary>
	public enum VDBTriggerEvent
	{
		/// <summary>
		/// Before append event
		/// </summary>
		BeforeAppend = 0x01,
		/// <summary>
		/// After append event
		/// </summary>
		AfterAppend  = 0x02,
		/// <summary>
		/// Before update event
		/// </summary>
		BeforeUpdate = 0x04,
		/// <summary>
		/// After update event
		/// </summary>
		AfterUpdate  = 0x08,
		/// <summary>
		/// Before delete event
		/// </summary>
		BeforeDelete = 0x10,
		/// <summary>
		/// After delete event
		/// </summary>
		AfterDelete  = 0x20
	}

	/// <summary>
	/// Database parameters.
	/// </summary>
	public enum VDBDatabaseParam
	{
		/// <summary>
		/// None
		/// </summary>
		None		= 0,
		/// <summary>
		/// Auto close, when all tables in database closed
		/// </summary>
		AutoClose	= 1,
		/// <summary>
		/// Creates in-memory database and destroys it after closing
		/// </summary>
		InMemory		= 2
	}

	/// <summary>
	/// Type of import/export files.
	/// </summary>
	public enum VDBFileType
	{
		/// <summary>
		/// Comma delimited text format
		/// </summary>
		CommaDelimitedText	= 21,
		/// <summary>
		/// Text format
		/// </summary>
		XML					= 26
	}

	/// <summary>
	/// Foreign key options
	/// </summary>
	public enum VDBForeignKeyOptions
	{
		/// <summary>
		/// None
		/// </summary>
		None = 0,
		/// <summary>
		/// Cascade update
		/// </summary>
		OnUpdateCascade = 1,
		/// <summary>
		/// Cascade delete
		/// </summary>
		OnDeleteCascade = 2
	}

	/// <summary>
	/// Internal class used to communicate with the core data engine.
	/// </summary>
	[SuppressUnmanagedCodeSecurity()]
	internal class VistaDBAPI
	{
		private const string LibName = @"VistaDb20.dll";
		private static string[] vdbTypes = {"C", "D", "T", "L", "M", "P", "B", "Y", "I", "U", "E", "V", "G"};
		public const int MAX_STRING_SIZE = 65;
		public const int MAX_DESCRIPTION_SIZE = 1024;

		public static Type GetFieldType(VistaDBType colType) 
		{
			switch (colType) 
			{
				case VistaDBType.Character:		return typeof(System.String);
				case VistaDBType.Varchar:		return typeof(System.String);
				case VistaDBType.Memo:		    return typeof(System.String);

				case VistaDBType.Date:
				case VistaDBType.DateTime: 		return typeof(System.DateTime);

				case VistaDBType.Double:		return typeof(System.Double);
				case VistaDBType.Int32:			return typeof(System.Int32);
				case VistaDBType.Int64:			return typeof(System.Int64);

				case VistaDBType.Currency:		return typeof(System.Decimal);

				case VistaDBType.Boolean:	    return typeof(System.Boolean);

				case VistaDBType.Picture:
				case VistaDBType.Blob:		    return typeof(System.Array);

				case VistaDBType.Guid:			return typeof(System.Guid);

				default:
					return typeof(System.String);
			}
		}

		public static string NativeDataType(VistaDBType t)
		{
			return vdbTypes[(int)t];
		}

		public static VistaDBType NetDataType(string t)
		{
			return (VistaDBType)Array.IndexOf(vdbTypes, t);
		}

		public const int  RTagInfoSize = 60;
		public const long MSecsPerDay = 24 * 60 * 60 * 1000;

		// -----------------------------------
		// Engine API
		// -----------------------------------
		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_ActivateRecycling();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ActivatedRecycling();
		
		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_AlterColumn( 
			string cpOldColumnName, 
			string cpNewColumnName,
			string cpType, 
			int iLength, 
			short iDecimals, 
			bool bRequired, 
			bool bReadOnly, 
			bool bPacked, 
			bool bHidden,
			bool bEncrypted, 
			bool bUnicode
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_AlterTable( 
			string cpOldTableName, 
			string cpNewTableName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_AlterTableExec(
			bool bForceAlter
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_Append();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_AppendBlank();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_AppendRecord(
			string cpRecord
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_BlobToFile(
			string cpColumnName,
			string cpFileName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_Bof();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_ClearMemoryCache();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_Close();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_CloseAll();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_CloseDatabase();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern ushort vdb_ColumnCount();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ColumnCompressed(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern ushort vdb_ColumnDecimals(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ColumnEncrypted( 
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ColumnHidden(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ColumnIndexed(
			string cpColumnName, 
			bool bPrimary
			);

		/*[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_ColumnName(
			ushort uiColumnNum
			);*/

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ColumnNameManaged(
			ushort uiColumnNum,
			StringBuilder cpBuffer,
			int ulBufferLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern ushort vdb_ColumnNum(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern ushort vdb_ColumnOffset(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ColumnReadOnly(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ColumnRequired(
			string cpColumnName
			);

		/*[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_ColumnType(
			string cpColumnName
			);*/

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_ColumnTypeManaged(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern ushort vdb_ColumnWidth(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ColumnUnicode(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_Commit();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_Contains(
			string cpColumnName, 
			string cpPattern
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_CopyFile(
			string cpToFileName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_CopyStructure(
			string cpFileName,
			string cpAlias
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_CopyStructureExtended(
			string cpFileName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_CreateColumn(
			string cpName, 
			string cpType,
			int iLength,
			short iDecimals,
			bool bRequired, 
			bool bReadOnly, 
			bool bPacked, 
			bool bHidden,
			bool bEncrypted, 
			bool bUnicode
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_CreateDatabase(
			string cpDbPath, 
			string cpDbAlias,
			bool bReserved1, 
			bool bReserved2,
			uint ulCultureId,
			uint ulParams,
			string cpPassword,
			uint ulCypher,
			bool CaseSensitive
			);
		
		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_CreateFrom(
			string cpFileName,
			string cpAlias,
			string cpStructFile,
			short iDriverType
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern short vdb_CreateFromTable(
			short iSrcTableId,
			string cpTableName,
			bool bImportData,
			bool bImportIndexes,
			string cpConstraint
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern short vdb_CreateIndex(
			string cpIndexName, 
			string cpKeyExpr,
			short iOption,
			bool bDescend,
			string cpFilterExpr,
			uint ulLocaleID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_CreateTable(
			string cpTableName, 
			string cpTableAlias
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_CreateTableExec();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern short vdb_CreateTemporaryIndex(
			string cpFileName, 
			string cpKeyExpr,
			short iOption,
			bool bDescend,
			string cpFilterExpr,
			uint ulLocaleID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_DeactivateRecycling();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DeleteAllTableRows();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_DeleteRow();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DecryptTable();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_Descend(
			string cpCharBuff,
			uint ulLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DropIndex(
			string cpIndexName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DropTable(
			string cpTableName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DropTableInstantly(
			string cpTableName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_EncryptTable();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_EnumTables(
			uint ulPrevTableId,
			StringBuilder cpNameBuffer,
			uint ulBuffLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_EnumIndexes(
			uint ulPrevTagId,
			string cpTableName, 
			StringBuilder cpNameBuffer,
			uint ulBuffLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_EnumTriggers(
			uint ulPrevId,
			StringBuilder cpNameBuffer,
			uint ulBuffLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_EnumConstraints(
			uint ulPrevId,
			StringBuilder cpNameBuffer,
			uint ulBuffLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_EnumIdentities(
			uint ulPrevId,
			StringBuilder cpNameBuffer,
			uint ulBuffLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_EnumForeignKeys(
			uint ulPrevId,
			StringBuilder cpNameBuffer,
			uint ulBuffLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_Eof();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern short vdb_ErrorLevel(short iErrorLevel);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_EvalBoolean( 
			string cpExpression
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern double vdb_EvalDouble(
			string cpExpression
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_EvalInt64(
			string cpExpression
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_EvalString( 
			string cpExpression
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern short vdb_EvalTest( 
			short cpExpression
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ExportTableToText(
			string cpTextFileName,
			short iFileType
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_FileSpace(
			ref uint ulAllocatedSpace
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_Find(
			string cpKeyValue, 
			string cpIndexName,
			bool bExactMatch,
			bool bSoftPosition
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_FlushFileBuffers();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_TableLock();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_Found();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_GetBlob(
			string cpColumnName,
			byte[] vpVar
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_GetBlobLength(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetBoolean(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern long vdb_GetCurrency(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_GetDatabaseIdByPath(
			string cpDbPath
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_GetDatabaseIdByAlias(
			string cpDbAlias
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetDatabaseAliasById(
			int iDbId
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_GetDatabaseCultureId();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetDateString(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern double vdb_GetDateTime(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern double vdb_GetDouble(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern long vdb_GetInt64(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_GetInt32(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetMemoManaged( 
			string cpColumnName, 
			StringBuilder cpBuffer,
			int ulBufferLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetNull(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_GetRecord(
			string cpRecord
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetScope(
			short iWhichScope
			);

		/*[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetString(
			string cpColumnName
			);*/

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetStringManaged(
			string cpColumnName, 
			StringBuilder cpBuffer,
			int ulBufferLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetTrimString(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetIndexesInformation(
			ref RTagBackInfo pTagInfo
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetIndexInformationByName(
			[MarshalAs(UnmanagedType.LPStr)] string cpTagName,
			ref RTagInfo pInfo
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetCharacterOrder();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_GetDateFormat();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetSystemLocale();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_Go(
			int lRowId
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_GoFiltered(
			uint lRecNum
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_GoNextFtsKey();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ImportTableFrom(
			string cpFileOrTableName,
			short iSourceType,
			string cpScopeExpr
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ImportToTable(
			short iDstTableId,
			string cpConstraint
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_IndexAscending();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_IndexClose();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_IndexCount();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_IndexDescending();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_IndexFileName(
			short iIndexOrder
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_IndexKey();

		/*[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_IndexName(
			short iIndexOrder
			);*/

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_IndexNameManaged(
			short iIndexOrder,
			StringBuilder cpBuffer,
			int ulBufferLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern short vdb_IndexOpen( 
			string cpFileName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern short vdb_IndexOrder();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern short vdb_IndexOrderByName(
			string cpTagName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_IsEncrypted(
			short iFileOrRec
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern ushort vdb_LockCount();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_Locate(
			string cpExpression,
			bool bBackDirection,
			bool bContinue
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_OpenDatabase(
			string cpDbPath, 
			string cpDbAlias,
			bool bExclusive,
			bool bReadOnly,
			uint ulParams,
			string cpPassword,
			uint ulCypher,
			bool bCaseSensitive
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern short vdb_OpenTable(
			string cpTableName,
			string cpTableAlias,
			short iOpenMode,
			uint ulHint
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_PackDatabase(
			string cpDbName,
			string cpOldPasswd,
			uint ulOldCypher,
			string cpNewPasswd,
			uint ulNewCypher,
			bool bCaseSensitivity,
			int ulNewLocale,
			bool bMarkFullEncryption,
			bool bSaveFilePermission
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_PutBlob(
			string cpColumnName,
			byte[] vpVar,
			int lSize
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_PutBlobFromFile(
			string cpColumnName,
			string cpFileName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutBoolean(
			string cpColumnName,
			bool bVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutCurrency(
			string cpColumnName,
			long iCurr
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutDateString(
			string cpColumnName,
			string pVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutDateTime(
			string cpColumnName,
			double dDateTime
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutDouble(
			string cpColumnName,
			double dVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutInt32(
			string cpColumnName,
			int lVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutInt64(
			string cpColumnName,
			long i64Val
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutMemo(
			string cpColumnName,
			string pMemoText
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutNull(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutRecord(
			string cpRecord
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutString(
			string cpColumnName,
			string pVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_Query(
			string cpExpression
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_QueryRowCount();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_QuerySetExact(
			bool bSetExactQuery
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ReindexTable();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_RenameCurrentTable(
			string cpNewName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_RowCount();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_RowFirst();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_RowId();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_RowLast();
		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_RowLock(
			uint lRecNum
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_RowLocked(
			uint lRecNum
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_RowNext();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_RowPrior();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_RowSize();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_RowUnlock(
			uint ulRecNum
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SaveToDatabase(
			string cpDatabaseName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_MoveBy(
			int iBypassRecs
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_Seek(
			string cpKeyValue
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SeekBin(
			string cpKeyValue,
			ushort uiLength
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern ushort vdb_Select(
			ushort uiTableId
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SelectDb(
			int iDbId
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_SetCentury(
			bool bUseCentury
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_SetDateFormat(
			ushort uiDateType
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetDetailedRelation(
			ushort uiChildTableId,
			string cpKeyExpr
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern ushort vdb_SetEpoch(
			ushort uiBaseYear
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_SetErrorFunc(
			VDBUserErrorFunc func, 
			int pUserErrorInfo
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_SetExact(
			bool bOn
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetFilter(
			string cpExpression
			);

		/*		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
				private static extern int vdb_SetGaugeFunc(pUserGaugeFunc: Pointer);*/

		//procedure vdb_SetGaugeHook(hwndGauge: HWND);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_SetLockTimeout(
			short iSeconds
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern short vdb_SetOrder(
			short iIndex
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetRelation(
			ushort uiChildTableId,
			string cpKeyExpr
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetScope(
			string cpLowVal,
			string cpHighVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_SetSoftSeek(
			bool bSet
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetSureCommit(
			bool bSureCommit
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SqlQuery(
			string cpQueryExpr
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_SysProp(
			ushort uiSysItem,
			int vpValue
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_TableAlias(
			ushort uiTableId
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_TableFilter();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern ushort vdb_TableId(
			string cpAlias
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_TableName(
			ushort uiTableId
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_Version();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern uint vdb_GetTransactionLevel();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_BeginTransaction();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_CommitTransaction();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_RollbackTransaction(
			bool bAllLevels
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetTrigger(
			string cpName, 
			string cpTrigger,
			string cpTriggerDescr,
			uint ulEventOption,
			int iPriority
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DropTrigger(
			string cpName
			);

		/*[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetTrigger(
			string cpName,
			ref int piEventOption, 
			ref int piPriority
			);*/

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetTriggerManaged(
			string cpName,
			ref int piEventOption, 
			ref int piPriority,
			StringBuilder cpBuffer,
			int ulBufferLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ActivateTrigger(
			string cpName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DeactivateTrigger(
			string cpName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_IsTriggerActive(
			string cpName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetTriggerDescription(
			string cpName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetConstraint(
			string cpName,
			string cpConstraint,
			string cpConstrDescr
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DropConstraint(
			string cpName
			);

		/*[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetConstraint(
			string cpName
			);*/

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetConstraintManaged(
			string cpName,
			StringBuilder cpBuffer,
			int ulBufferLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ActivateConstraint(
			string cpName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DeactivateConstraint(
			string cpName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_IsConstraintActive(
			string cpName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetConstraintDescription(
			string cpName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetForeignKey(
			string cpName, 
			string cpForeignConstraint, 
			string cpDescription,
			uint ulOptions
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DropForeignKey(
			string cpName
			);

		/*[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetForeignKey( 
			string cpName,
			StringBuilder cpForeignKey,
			StringBuilder cpPrimTable,
			StringBuilder cpPrimKey,
			uint ulBuffLen,
			ref uint ulOptions
			);*/

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetForeignKeyManaged( 
			string cpName,
			StringBuilder cpForeignKey,
			StringBuilder cpPrimTable,
			StringBuilder cpPrimKey,
			uint ulBuffLen,
			ref uint ulOptions,
			StringBuilder cpDescr,
			int ulDescrBuffLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetIdentity(
			string cpColumnName, 
			string cpSeedExpr,
			double dStep
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DropIdentity(
			string cpColumnName
			);

		/*[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetIdentity(
			string cpColumnName,
			ref double pdStep
			);*/

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetIdentityManaged(
			string cpColumnName,
			ref double pdStep,
			StringBuilder cpBuffer,
			int ulBuffLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ActivateIdentity(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DeactivateIdentity(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetDefaultValue(
			string cpColumnName,
			string cpSeed,
			bool bUseInUpdate
			);

		/*[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetDefaultValue(
			string cpColumnName,
			ref bool bUseInUpdate
			);*/

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetDefaultValueManaged(
			string cpColumnName,
			ref bool bUseInUpdate,
			StringBuilder cpBuffer,
			int ulBuffLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetColumnDescription(
			string cpColumnName,
			string cpDescr
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetColumnDescription(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetColumnCaption(
			string cpColumnName, 
			string cpCaption
			);

		/*[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetColumnCaption(
			string cpColumnName
			);*/

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetColumnCaptionManaged(
			string cpColumnName,
			StringBuilder cpBuffer,
			int ulBuffLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetCaseSensitive();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_IsTableExist(string cpTableName);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_IsTriggerExist(string cpTriggerName);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetTableDescription(string cpDescription);
		
		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetTableDescription();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_AddToExportList(
			string cpTableName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ClearExportList();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ExportData(
			string cpXmlFileName,
			bool bSchemaOnly
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ImportData(
			string cpXmlFileName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ImportSchema(
			string cpXmlFileName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_ImportSchemaAndData(
			string cpXmlFileName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutGuid(
			string cpColumnName, 
			Guid guidVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern Guid vdb_GetGuid(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetNullByIndex(
			int iColumnIndex
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern long vdb_GetCurrencyByIndex(
			int iColumnIndex
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern double vdb_GetDoubleByIndex(
			int iColumnIndex
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern double vdb_GetDateTimeByIndex(
			int iColumnIndex
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern long vdb_GetInt64ByIndex(
			int iColumnIndex
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_GetInt32ByIndex(
			int iColumnIndex
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern Guid vdb_GetGuidByIndex(
			int iColumnIndex
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetBooleanByIndex(
			int iColumnIndex
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_GetStringManagedByIndex(
			int iColumnIndex,
			StringBuilder cpBuffer, 
			uint ulBufferLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetStringByIndex(
			int iColumnIndex
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_SetDatabaseDescription(
			string cpDescription
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vdb_GetDatabaseDescription();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vdb_DropColumn(
			string cpColumnName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutBooleanByIndex(
			int iColumn, 
			bool bVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutCurrencyByIndex(
			int iColumn, 
			long iCurr
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutDateStringByIndex(
			int iColumn,
			string cpVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutDateTimeByIndex(
			int iColumn,
			double dDateTime
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutDoubleByIndex(
			int iColumn,
			double dVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutGuidByIndex(
			int iColumn,
			Guid guidVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutInt32ByIndex(
			int iColumn,
			int lVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutInt64ByIndex(int iColumn,
			long i64Val
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutMemoByIndex(
			int iColumn,
			string cpMemoText
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutNullByIndex(
			int iColumnIndex
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_PutStringByIndex(
			int iColumn,
			string cpVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vdb_SetClusterLength(
			int iLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vdb_GetClusterLength();

		//----------------------------------------------------------------------------------
		// SQL support 
		//----------------------------------------------------------------------------------

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_CreateDatabaseConnection();

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_FreeDatabaseConnection(
			int connectionID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_OpenDatabaseConnection(
			int connectionID, 
			[MarshalAs(UnmanagedType.LPStr)] string cpDataBaseName, 
			bool Exclusive, 
			bool ReadOnly, 
			CypherType defaultCypher, 
			[MarshalAs(UnmanagedType.LPStr)] string defaultPassword,
			bool CaseSensitive
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_CloseDatabaseConnection(
			int connectionID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_BeginTransaction(
			int connectionID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_CommitTransaction(
			int connectionID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_RollbackTransaction(
			int connectionID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_GetTransactionLevel(
			int connectionID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_CreateQuery(
			int connectionID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_FreeQuery(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vsql_GetSQL(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetSQL(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string cpCmdText
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_Prepare(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_UnPrepare(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_Open(
			int QueryID, 
			ref int RowsAffected
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_ExecSQL(
			int QueryID, 
			ref int RowsAffected
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_Close(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetDatabaseName(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern string vsql_GetDatabaseName(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_Eof(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_Bof(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_Last(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_First(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_MoveBy(
			int QueryID, 
			int iVal
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_Next(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_Prior(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_GetString(
			int QueryID, 
			int FieldNo, 
			[MarshalAs(UnmanagedType.LPStr)] StringBuilder strPtr, 
			int iStrLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_GetVarchar(
			int QueryID, 
			int FieldNo, 
			[MarshalAs(UnmanagedType.LPStr)] StringBuilder strPtr, 
			int iStrLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_GetInt32(
			int QueryID, 
			int FieldNo
			);
		
		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern long vsql_GetInt64(
			int QueryID, 
			int FieldNo
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern double vsql_GetDouble(
			int QueryID, 
			int FieldNo
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_GetBoolean(
			int QueryID, 
			int FieldNo
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern long vsql_GetCurrency(
			int QueryID, 
			int FieldNo
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern long vsql_GetDate(
			int QueryID, 
			int FieldNo
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern long vsql_GetDateTime(
			int QueryID, 
			int FieldNo
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_GetMemo(
			int QueryID, 
			int FieldNo, 
			[MarshalAs(UnmanagedType.LPStr)] StringBuilder memPtr, 
			int iMemoLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_GetBlob(
			int QueryID, 
			int FieldNo,  
			byte[] memPtr, 
			int iBuffLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_GetBlobLength(
			int QueryID, 
			int FieldNo
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_GetRecNo(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_RecCount(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_RecSize(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_ColumnCount(
			int QueryID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_IsNull(
			int QueryID, 
			int FieldNo
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_ParamIsNull(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamNull(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName, 
			short shType
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamString(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName, 
			[MarshalAs(UnmanagedType.LPStr)] string Val
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamDate(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName, 
			long Val
			);
	
		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamDateTime(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName, 
			long Val
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamBoolean(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName,
			bool Val
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamInt32(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName, 
			int Val
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamInt64(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName, 
			long Val
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamDouble(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName, 
			double Val
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamCurrency(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName,
			long Val);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamVarchar(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName, 
			[MarshalAs(UnmanagedType.LPStr)] string Val
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamMemo(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName, 
			[MarshalAs(UnmanagedType.LPStr)] string Val
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamBlob(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName, 
			byte[] Val,
			int iBlobSize
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamPicture(
			int QueryID, 
			[MarshalAs(UnmanagedType.LPStr)] string pName, 
			byte[] Val,
			int iBlobSize
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_ColumnName(
			int QueryID, 
			int No, 
			[MarshalAs(UnmanagedType.LPStr)] StringBuilder strPtr,
			int iStrLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern char vsql_ColumnType(
			int QueryID, 
			int No
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_ColumnWidth(
			int QueryID, 
			int No
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_ColumnRequired(
			int QueryID, 
			int No
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_ColumnReadOnly(
			int QueryID, 
			int No
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_ColumnIsIdentity(
			int QueryID, 
			int No
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_ColumnCaption(
			int QueryID, 
			int No, 
			[MarshalAs(UnmanagedType.LPStr)] StringBuilder strPtr, 
			int iStrLen
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_ColumnIsPrimaryKey(
			int QueryID, 
			int No
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern Guid vsql_GetGuid(
			int SQLID, 
			int FieldNo
			);
		
		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern void vsql_SetParamGuid(
			int QueryID,
			string pName,
			Guid Val
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern int vsql_GetCurrentDatabaseID(
			int iConnectionID
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_ColumnIsUnique(
			int QueryID, 
			int FieldNo
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_IsReservedWord(
			string pName
			);

		[DllImport(LibName, CharSet=CharSet.Ansi, ExactSpelling=true)]
		private static extern bool vsql_AssignDatabaseConnection(
			int iConnectionID,
			int iDatabaseID,
			string cpDatabaseName,
			bool bExclusive,
			bool bReadOnly,
			int defaultCypher,
			string defaultPassword,
			bool CaseSensitive
			);


		//'-----------------Utility function--------------------
		public static string CutString(StringBuilder s)
		{
			/*int k;
			string tmp, res;

			tmp = s.ToString();
			res = "";

			for(k = 0;k < tmp.Length;k++)
			{
				if(tmp[k] == (char)0)
					break;
				else
					res = res + tmp[k];
			}

			return res;*/
			return s.ToString();
		}


		//Mapping functions

		public static void ivdb_ActivateRecycling()
		{
			vdb_ActivateRecycling();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_ActivatedRecycling()
		{
			bool res = vdb_ActivatedRecycling();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_AlterColumn( 
			string cpOldColumnName, 
			string cpNewColumnName,
			string cpType, 
			int iLength, 
			short iDecimals, 
			bool bRequired, 
			bool bReadOnly, 
			bool bPacked, 
			bool bHidden,
			bool bEncrypted, 
			bool bUnicode
			)
		{
			bool res = vdb_AlterColumn( cpOldColumnName, cpNewColumnName, cpType, iLength, iDecimals, bRequired, bReadOnly, bPacked, bHidden, bEncrypted, bUnicode);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_AlterTable( 
			string cpOldTableName, 
			string cpNewTableName
			)
		{
			bool res = vdb_AlterTable(cpOldTableName, cpNewTableName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_AlterTableExec(
			bool bForceAlter
			)
		{
			bool res = vdb_AlterTableExec(bForceAlter);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static void ivdb_Append()
		{
			vdb_Append();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_AppendBlank()
		{
			vdb_AppendBlank();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_AppendRecord(
			string cpRecord
			)
		{
			bool res = vdb_AppendRecord(cpRecord);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_BlobToFile(
			string cpColumnName,
			string cpFileName
			)
		{
			bool res = vdb_BlobToFile(cpColumnName, cpFileName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_Bof()
		{
			bool res = vdb_Bof();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_ClearMemoryCache()
		{
			vdb_ClearMemoryCache();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_Close()
		{
			vdb_Close();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_CloseAll()
		{
			vdb_CloseAll();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_CloseDatabase()
		{
			bool res = vdb_CloseDatabase();
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static ushort ivdb_ColumnCount()
		{
			ushort res = vdb_ColumnCount();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_ColumnCompressed(
			string cpColumnName
			)
		{
			bool res = vdb_ColumnCompressed(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static ushort ivdb_ColumnDecimals(
			string cpColumnName
			)
		{
			ushort res = vdb_ColumnDecimals(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_ColumnEncrypted( 
			string cpColumnName
			)
		{
			bool res = vdb_ColumnEncrypted(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_ColumnHidden(
			string cpColumnName
			)
		{
			bool res = vdb_ColumnHidden(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_ColumnIndexed(
			string cpColumnName, 
			bool bPrimary
			)
		{
			bool res = vdb_ColumnIndexed(cpColumnName, bPrimary);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_ColumnNameManaged(
			ushort uiColumnNum,
			StringBuilder cpBuffer,
			int ulBufferLen
			)
		{
			bool res = vdb_ColumnNameManaged(uiColumnNum, cpBuffer, ulBufferLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static ushort ivdb_ColumnNum(
			string cpColumnName
			)
		{
			ushort res = vdb_ColumnNum(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static ushort ivdb_ColumnOffset(
			string cpColumnName
			)
		{
			ushort res = vdb_ColumnOffset(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_ColumnReadOnly(
			string cpColumnName
			)
		{
			bool res = vdb_ColumnReadOnly(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_ColumnRequired(
			string cpColumnName
			)
		{
			bool res = vdb_ColumnRequired(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static int ivdb_ColumnTypeManaged(
			string cpColumnName
			)
		{
			int res = vdb_ColumnTypeManaged(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static ushort ivdb_ColumnWidth(
			string cpColumnName
			)
		{
			ushort res = vdb_ColumnWidth(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_ColumnUnicode(
			string cpColumnName
			)
		{
			bool res = vdb_ColumnUnicode(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static void ivdb_Commit()
		{
			vdb_Commit();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static int ivdb_Contains(
			string cpColumnName, 
			string cpPattern
			)
		{
			int res = vdb_Contains(cpColumnName, cpPattern);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_CopyFile(
			string cpToFileName
			)
		{
			bool res = vdb_CopyFile(cpToFileName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_CopyStructure(
			string cpFileName,
			string cpAlias
			)
		{
			bool res = vdb_CopyStructure(cpFileName, cpAlias);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_CopyStructureExtended(
			string cpFileName
			)
		{
			bool res = vdb_CopyStructureExtended(cpFileName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static void ivdb_CreateColumn(
			string cpName, 
			string cpType,
			int iLength,
			short iDecimals,
			bool bRequired, 
			bool bReadOnly, 
			bool bPacked, 
			bool bHidden,
			bool bEncrypted, 
			bool bUnicode
			)
		{
			vdb_CreateColumn(cpName, cpType, iLength, iDecimals, bRequired, bReadOnly, bPacked, bHidden, bEncrypted, bUnicode);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static int ivdb_CreateDatabase(
			string cpDbPath, 
			string cpDbAlias,
			bool bReserved1, 
			bool bReserved2,
			uint ulCultureId,
			uint ulParams,
			string cpPassword,
			uint ulCypher,
			bool CaseSensitive
			)
		{
			int res = vdb_CreateDatabase(cpDbPath, cpDbAlias, bReserved1, bReserved2, ulCultureId, ulParams, cpPassword, ulCypher, CaseSensitive);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}
		
		public static int ivdb_CreateFrom(
			string cpFileName,
			string cpAlias,
			string cpStructFile,
			short iDriverType
			)
		{
			int res = vdb_CreateFrom(cpFileName, cpAlias, cpStructFile, iDriverType);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static short ivdb_CreateFromTable(
			short iSrcTableId,
			string cpTableName,
			bool bImportData,
			bool bImportIndexes,
			string cpConstraint
			)
		{
			short res = vdb_CreateFromTable(iSrcTableId, cpTableName, bImportData, bImportIndexes, cpConstraint);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static short ivdb_CreateIndex(
			string cpIndexName, 
			string cpKeyExpr,
			short iOption,
			bool bDescend,
			string cpFilterExpr,
			uint ulLocaleID
			)
		{
			short res = vdb_CreateIndex(cpIndexName, cpKeyExpr, iOption, bDescend, cpFilterExpr, ulLocaleID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_CreateTable(
			string cpTableName, 
			string cpTableAlias
			)
		{
			bool res = vdb_CreateTable(cpTableName, cpTableAlias);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_CreateTableExec()
		{
			bool res = vdb_CreateTableExec();
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static short ivdb_CreateTemporaryIndex(
			string cpFileName, 
			string cpKeyExpr,
			short iOption,
			bool bDescend,
			string cpFilterExpr,
			uint ulLocaleID
			)
		{
			short res = vdb_CreateTemporaryIndex(cpFileName, cpKeyExpr, iOption, bDescend, cpFilterExpr, ulLocaleID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_DeactivateRecycling()
		{
			vdb_DeactivateRecycling();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_DeleteAllTableRows()
		{
			bool res = vdb_DeleteAllTableRows();
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static void ivdb_DeleteRow()
		{
			vdb_DeleteRow();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_DecryptTable()
		{
			bool res = vdb_DecryptTable();
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static string ivdb_Descend(
			string cpCharBuff,
			uint ulLen
			)
		{
			string res = vdb_Descend(cpCharBuff, ulLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_DropIndex(
			string cpIndexName
			)
		{
			bool res = vdb_DropIndex(cpIndexName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_DropTable(
			string cpTableName
			)
		{
			bool res = vdb_DropTable(cpTableName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_DropTableInstantly(
			string cpTableName
			)
		{
			bool res = vdb_DropTableInstantly(cpTableName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_EncryptTable()
		{
			bool res = vdb_EncryptTable();
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static uint ivdb_EnumTables(
			uint ulPrevTableId,
			StringBuilder cpNameBuffer,
			uint ulBuffLen
			)
		{
			uint res = vdb_EnumTables(ulPrevTableId, cpNameBuffer, ulBuffLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static uint ivdb_EnumIndexes(
			uint ulPrevTagId,
			string cpTableName, 
			StringBuilder cpNameBuffer,
			uint ulBuffLen
			)
		{
			uint res = vdb_EnumIndexes(ulPrevTagId, cpTableName, cpNameBuffer, ulBuffLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static uint ivdb_EnumTriggers(
			uint ulPrevId,
			StringBuilder cpNameBuffer,
			uint ulBuffLen
			)
		{
			uint res = vdb_EnumTriggers(ulPrevId, cpNameBuffer, ulBuffLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static uint ivdb_EnumConstraints(
			uint ulPrevId,
			StringBuilder cpNameBuffer,
			uint ulBuffLen
			)
		{
			uint res = vdb_EnumConstraints(ulPrevId, cpNameBuffer, ulBuffLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static uint ivdb_EnumIdentities(
			uint ulPrevId,
			StringBuilder cpNameBuffer,
			uint ulBuffLen
			)
		{
			uint res = vdb_EnumIdentities(ulPrevId, cpNameBuffer, ulBuffLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static uint ivdb_EnumForeignKeys(
			uint ulPrevId,
			StringBuilder cpNameBuffer,
			uint ulBuffLen
			)
		{
			uint res = vdb_EnumForeignKeys(ulPrevId,cpNameBuffer,ulBuffLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_Eof()
		{
			bool res = vdb_Eof();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static short ivdb_ErrorLevel(short iErrorLevel)
		{
			short res = vdb_ErrorLevel(iErrorLevel);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_EvalBoolean( 
			string cpExpression
			)
		{
			bool res = vdb_EvalBoolean(cpExpression);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static double ivdb_EvalDouble(
			string cpExpression
			)
		{
			double res = vdb_EvalDouble(cpExpression);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_EvalInt64(
			string cpExpression
			)
		{
			bool res = vdb_EvalInt64(cpExpression);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_EvalString( 
			string cpExpression
			)
		{
			string res = vdb_EvalString(cpExpression);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static short ivdb_EvalTest( 
			short cpExpression
			)
		{
			short res = vdb_EvalTest(cpExpression);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_ExportTableToText(
			string cpTextFileName,
			short iFileType
			)
		{
			bool res = vdb_ExportTableToText(cpTextFileName, iFileType);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static uint ivdb_FileSpace(
			ref uint ulAllocatedSpace
			)
		{
			uint res = vdb_FileSpace(ref ulAllocatedSpace);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_Find(
			string cpKeyValue, 
			string cpIndexName,
			bool bExactMatch,
			bool bSoftPosition
			)
		{
			bool res = vdb_Find(cpKeyValue, cpIndexName, bExactMatch, bSoftPosition);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_FlushFileBuffers()
		{
			vdb_FlushFileBuffers();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_TableLock()
		{
			bool res = vdb_TableLock();
			VistaDBErrorMsgs.ShowErrors(res, res);

			return res;
		}

		public static bool ivdb_Found()
		{
			bool res = vdb_Found();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static uint ivdb_GetBlob(
			string cpColumnName,
			byte[] vpVar
			)
		{
			uint res = vdb_GetBlob(cpColumnName, vpVar);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static uint ivdb_GetBlobLength(
			string cpColumnName
			)
		{
			uint res = vdb_GetBlobLength(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_GetBoolean(
			string cpColumnName
			)
		{
			bool res = vdb_GetBoolean(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static long ivdb_GetCurrency(
			string cpColumnName
			)
		{
			long res = vdb_GetCurrency(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivdb_GetDatabaseIdByPath(
			string cpDbPath
			)
		{
			int res = vdb_GetDatabaseIdByPath(cpDbPath);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivdb_GetDatabaseIdByAlias(
			string cpDbAlias
			)
		{
			int res = vdb_GetDatabaseIdByAlias(cpDbAlias);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_GetDatabaseAliasById(
			int iDbId
			)
		{
			string res = vdb_GetDatabaseAliasById(iDbId);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static uint ivdb_GetDatabaseCultureId()
		{
			uint res = vdb_GetDatabaseCultureId();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_GetDateString(
			string cpColumnName
			)
		{
			string res = vdb_GetDateString(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static double ivdb_GetDateTime(
			string cpColumnName
			)
		{
			double res = vdb_GetDateTime(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static double ivdb_GetDouble(
			string cpColumnName
			)
		{
			double res = vdb_GetDouble(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static long ivdb_GetInt64(
			string cpColumnName
			)
		{
			long res = vdb_GetInt64(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivdb_GetInt32(
			string cpColumnName
			)
		{
			int res = vdb_GetInt32(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_GetMemoManaged( 
			string cpColumnName, 
			StringBuilder cpBuffer,
			int ulBufferLen
			)
		{
			bool res = vdb_GetMemoManaged(cpColumnName, cpBuffer, ulBufferLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_GetNull(
			string cpColumnName
			)
		{
			bool res = vdb_GetNull(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_GetRecord(
			string cpRecord
			)
		{
			vdb_GetRecord(cpRecord);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static string ivdb_GetScope(
			short iWhichScope
			)
		{
			string res = vdb_GetScope(iWhichScope);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_GetStringManaged(
			string cpColumnName, 
			StringBuilder cpBuffer,
			int ulBufferLen
			)
		{
			bool res = vdb_GetStringManaged(cpColumnName, cpBuffer, ulBufferLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_GetTrimString(
			string cpColumnName
			)
		{
			string res = vdb_GetTrimString(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_GetIndexesInformation(
			ref RTagBackInfo pTagInfo
			)
		{
			bool res = vdb_GetIndexesInformation(ref pTagInfo);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_GetIndexInformationByName(
			string cpTagName,
			ref RTagInfo pInfo
			)
		{
			bool res = vdb_GetIndexInformationByName(cpTagName, ref pInfo);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static string ivdb_GetCharacterOrder()
		{
			string res = vdb_GetCharacterOrder();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivdb_GetDateFormat()
		{
			int res = vdb_GetDateFormat();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_GetSystemLocale()
		{
			string res = vdb_GetSystemLocale();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_Go(
			int lRowId
			)
		{
			vdb_Go(lRowId);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static uint ivdb_GoFiltered(
			uint lRecNum
			)
		{
			uint res = vdb_GoFiltered(lRecNum);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivdb_GoNextFtsKey()
		{
			int res = vdb_GoNextFtsKey();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_ImportTableFrom(
			string cpFileOrTableName,
			short iSourceType,
			string cpScopeExpr
			)
		{
			bool res = vdb_ImportTableFrom(cpFileOrTableName, iSourceType, cpScopeExpr);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_ImportToTable(
			short iDstTableId,
			string cpConstraint
			)
		{
			bool res = vdb_ImportToTable(iDstTableId, cpConstraint);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_IndexAscending()
		{
			bool res = vdb_IndexAscending();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_IndexClose()
		{
			vdb_IndexClose();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static int ivdb_IndexCount()
		{
			int res = vdb_IndexCount();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_IndexDescending()
		{
			bool res = vdb_IndexDescending();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_IndexFileName(
			short iIndexOrder
			)
		{
			string res = vdb_IndexFileName(iIndexOrder);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_IndexKey()
		{
			string res = vdb_IndexKey();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_IndexNameManaged(
			short iIndexOrder,
			StringBuilder cpBuffer,
			int ulBufferLen
			)
		{
			bool res = vdb_IndexNameManaged(iIndexOrder, cpBuffer, ulBufferLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static short ivdb_IndexOpen( 
			string cpFileName
			)
		{
			short res = vdb_IndexOpen(cpFileName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static short ivdb_IndexOrder()
		{
			short res = vdb_IndexOrder();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static short ivdb_IndexOrderByName(
			string cpTagName
			)
		{
			short res = vdb_IndexOrderByName(cpTagName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_IsEncrypted(
			short iFileOrRec
			)
		{
			bool res = vdb_IsEncrypted(iFileOrRec);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static ushort ivdb_LockCount()
		{
			ushort res = vdb_LockCount();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivdb_Locate(
			string cpExpression,
			bool bBackDirection,
			bool bContinue
			)
		{
			int res = vdb_Locate(cpExpression, bBackDirection, bContinue);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}


		public static int ivdb_OpenDatabase(
			string cpDbPath, 
			string cpDbAlias,
			bool bExclusive,
			bool bReadOnly,
			uint ulParams,
			string cpPassword,
			uint ulCypher,
			bool bCaseSensitive
			)
		{
			int res = vdb_OpenDatabase(cpDbPath, cpDbAlias, bExclusive, bReadOnly, ulParams, cpPassword, ulCypher, bCaseSensitive);
			VistaDBErrorMsgs.ShowErrors(res == 0, res);

			return res;
		}

		public static short ivdb_OpenTable(
			string cpTableName,
			string cpTableAlias,
			short iOpenMode,
			uint ulHint
			)
		{
			short res = vdb_OpenTable(cpTableName, cpTableAlias, iOpenMode, ulHint);
			VistaDBErrorMsgs.ShowErrors(res == 0, res);

			return res;
		}

		public static bool ivdb_PackDatabase(
			string cpDbName,
			string cpOldPasswd,
			uint ulOldCypher,
			string cpNewPasswd,
			uint ulNewCypher,
			bool bCaseSensitivity,
			int ulNewLocale,
			bool bMarkFullEncryption,
			bool bSaveFilePermission
			)
		{
			bool res = vdb_PackDatabase(cpDbName, cpOldPasswd, ulOldCypher, cpNewPasswd, ulNewCypher, bCaseSensitivity, ulNewLocale, bMarkFullEncryption, bSaveFilePermission);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static int ivdb_PutBlob(
			string cpColumnName,
			byte[] vpVar,
			int lSize
			)
		{
			int res = vdb_PutBlob(cpColumnName, vpVar, lSize);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_PutBlobFromFile(
			string cpColumnName,
			string cpFileName
			)
		{
			bool res = vdb_PutBlobFromFile(cpColumnName, cpFileName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_PutBoolean(
			string cpColumnName,
			bool bVal
			)
		{
			vdb_PutBoolean(cpColumnName, bVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutCurrency(
			string cpColumnName,
			long iCurr
			)
		{
			vdb_PutCurrency(cpColumnName, iCurr);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutDateString(
			string cpColumnName,
			string pVal
			)
		{
			vdb_PutDateString(cpColumnName, pVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutDateTime(
			string cpColumnName,
			double dDateTime
			)
		{
			vdb_PutDateTime(cpColumnName, dDateTime);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutDouble(
			string cpColumnName,
			double dVal
			)
		{
			vdb_PutDouble(cpColumnName, dVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutInt32(
			string cpColumnName,
			int lVal
			)
		{
			vdb_PutInt32(cpColumnName, lVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutInt64(
			string cpColumnName,
			long i64Val
			)
		{
			vdb_PutInt64(cpColumnName, i64Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutMemo(
			string cpColumnName,
			string pMemoText
			)
		{
			vdb_PutMemo(cpColumnName, pMemoText);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutNull(
			string cpColumnName
			)
		{
			vdb_PutNull(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutRecord(
			string cpRecord
			)
		{
			vdb_PutRecord(cpRecord);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutString(
			string cpColumnName,
			string pVal
			)
		{
			vdb_PutString(cpColumnName, pVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static int ivdb_Query(
			string cpExpression
			)
		{
			int res = vdb_Query(cpExpression);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static uint ivdb_QueryRowCount()
		{
			uint res = vdb_QueryRowCount();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_QuerySetExact(
			bool bSetExactQuery
			)
		{
			bool res = vdb_QuerySetExact(bSetExactQuery);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_ReindexTable()
		{
			bool res = vdb_ReindexTable();
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_RenameCurrentTable(
			string cpNewName
			)
		{
			bool res = vdb_RenameCurrentTable(cpNewName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static uint ivdb_RowCount()
		{
			uint res = vdb_RowCount();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_RowFirst()
		{
			vdb_RowFirst();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static uint ivdb_RowId()
		{
			uint res = vdb_RowId();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_RowLast()
		{
			vdb_RowLast();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_RowLock(
			uint lRecNum
			)
		{
			bool res = vdb_RowLock(lRecNum);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_RowLocked(
			uint lRecNum
			)
		{
			bool res = vdb_RowLocked(lRecNum);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_RowNext()
		{
			vdb_RowNext();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_RowPrior()
		{
			vdb_RowPrior();
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static uint ivdb_RowSize()
		{
			uint res = vdb_RowSize();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_RowUnlock(
			uint ulRecNum
			)
		{
			vdb_RowUnlock(ulRecNum);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_SaveToDatabase(
			string cpDatabaseName
			)
		{
			bool res = vdb_SaveToDatabase(cpDatabaseName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_MoveBy(
			int iBypassRecs
			)
		{
			vdb_MoveBy(iBypassRecs);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_Seek(
			string cpKeyValue
			)
		{
			bool res = vdb_Seek(cpKeyValue);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_SeekBin(
			string cpKeyValue,
			ushort uiLength
			)
		{
			bool res = vdb_SeekBin(cpKeyValue, uiLength);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static ushort ivdb_Select(
			ushort uiTableId
			)
		{
			ushort res = vdb_Select(uiTableId);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_SelectDb(
			int iDbId
			)
		{
			bool res = vdb_SelectDb(iDbId);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_SetCentury(
			bool bUseCentury
			)
		{
			vdb_SetCentury(bUseCentury);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_SetDateFormat(
			ushort uiDateType
			)
		{
			vdb_SetDateFormat(uiDateType);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_SetDetailedRelation(
			ushort uiChildTableId,
			string cpKeyExpr
			)
		{
			bool res = vdb_SetDetailedRelation(uiChildTableId, cpKeyExpr);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static ushort ivdb_SetEpoch(
			ushort uiBaseYear
			)
		{
			ushort res = vdb_SetEpoch(uiBaseYear);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivdb_SetErrorFunc(
			VDBUserErrorFunc func, 
			int pUserErrorInfo
			)
		{
			return vdb_SetErrorFunc(func, pUserErrorInfo);
		}

		public static void ivdb_SetExact(
			bool bOn
			)
		{
			vdb_SetExact(bOn);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_SetFilter(
			string cpExpression
			)
		{
			bool res = vdb_SetFilter(cpExpression);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_SetLockTimeout(
			short iSeconds
			)
		{
			vdb_SetLockTimeout(iSeconds);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static short ivdb_SetOrder(
			short iIndex
			)
		{
			short res = vdb_SetOrder(iIndex);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_SetRelation(
			ushort uiChildTableId,
			string cpKeyExpr
			)
		{
			bool res = vdb_SetRelation(uiChildTableId, cpKeyExpr);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_SetScope(
			string cpLowVal,
			string cpHighVal
			)
		{
			bool res = vdb_SetScope(cpLowVal, cpHighVal);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivdb_SetSoftSeek(
			bool bSet
			)
		{
			vdb_SetSoftSeek(bSet);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_SetSureCommit(
			bool bSureCommit
			)
		{
			bool res = vdb_SetSureCommit(bSureCommit);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_SqlQuery(
			string cpQueryExpr
			)
		{
			bool res = vdb_SqlQuery(cpQueryExpr);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static uint ivdb_SysProp(
			ushort uiSysItem,
			int vpValue
			)
		{
			uint res = vdb_SysProp(uiSysItem, vpValue);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_TableAlias(
			ushort uiTableId
			)
		{
			string res = vdb_TableAlias(uiTableId);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_TableFilter()
		{
			string res = vdb_TableFilter();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static ushort ivdb_TableId(
			string cpAlias
			)
		{
			ushort res = vdb_TableId(cpAlias);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_TableName(
			ushort uiTableId
			)
		{
			string res = vdb_TableName(uiTableId);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_Version()
		{
			string res = vdb_Version();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static uint ivdb_GetTransactionLevel()
		{
			uint res = vdb_GetTransactionLevel();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_BeginTransaction()
		{
			bool res = vdb_BeginTransaction();
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_CommitTransaction()
		{
			bool res = vdb_CommitTransaction();
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static void ivdb_RollbackTransaction(
			bool bAllLevels
			)
		{
			vdb_RollbackTransaction(bAllLevels);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivdb_SetTrigger(
			string cpName, 
			string cpTrigger,
			string cpTriggerDescr,
			uint ulEventOption,
			int iPriority
			)
		{
			bool res = vdb_SetTrigger(cpName, cpTrigger, cpTriggerDescr, ulEventOption, iPriority);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_DropTrigger(
			string cpName
			)
		{
			bool res = vdb_DropTrigger(cpName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_GetTriggerManaged(
			string cpName,
			ref int piEventOption, 
			ref int piPriority,
			StringBuilder cpBuffer,
			int ulBufferLen
			)
		{
			bool res = vdb_GetTriggerManaged(cpName, ref piEventOption, ref piPriority, cpBuffer, ulBufferLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_ActivateTrigger(
			string cpName
			)
		{
			bool res = vdb_ActivateTrigger(cpName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_DeactivateTrigger(
			string cpName
			)
		{
			bool res = vdb_DeactivateTrigger(cpName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_IsTriggerActive(
			string cpName
			)
		{
			bool res = vdb_IsTriggerActive(cpName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_GetTriggerDescription(
			string cpName
			)
		{
			string res = vdb_GetTriggerDescription(cpName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_SetConstraint(
			string cpName,
			string cpConstraint,
			string cpConstrDescr
			)
		{
			bool res = vdb_SetConstraint(cpName, cpConstraint, cpConstrDescr);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_DropConstraint(
			string cpName
			)
		{
			bool res = vdb_DropConstraint(cpName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_GetConstraintManaged(
			string cpName,
			StringBuilder cpBuffer,
			int ulBufferLen
			)
		{
			bool res = vdb_GetConstraintManaged(cpName, cpBuffer, ulBufferLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_ActivateConstraint(
			string cpName
			)
		{
			bool res = vdb_ActivateConstraint(cpName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_DeactivateConstraint(
			string cpName
			)
		{
			bool res = vdb_DeactivateConstraint(cpName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_IsConstraintActive(
			string cpName
			)
		{
			bool res = vdb_IsConstraintActive(cpName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_GetConstraintDescription(
			string cpName
			)
		{
			string res = vdb_GetConstraintDescription(cpName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_SetForeignKey(
			string cpName, 
			string cpForeignConstraint, 
			string cpDescription,
			uint ulOptions
			)
		{
			bool res = vdb_SetForeignKey(cpName, cpForeignConstraint, cpDescription, ulOptions);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_DropForeignKey(
			string cpName
			)
		{
			bool res = vdb_DropForeignKey(cpName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_GetForeignKeyManaged( 
			string cpName,
			StringBuilder cpForeignKey,
			StringBuilder cpPrimTable,
			StringBuilder cpPrimKey,
			uint ulBuffLen,
			ref uint ulOptions,
			StringBuilder cpDescr,
			int ulDescrBuffLen
			)
		{
			bool res = vdb_GetForeignKeyManaged(cpName, cpForeignKey, cpPrimTable, cpPrimKey, ulBuffLen, ref ulOptions, cpDescr, ulDescrBuffLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_SetIdentity(
			string cpColumnName, 
			string cpSeedExpr,
			double dStep
			)
		{
			bool res = vdb_SetIdentity(cpColumnName, cpSeedExpr, dStep);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_DropIdentity(
			string cpColumnName
			)
		{
			bool res = vdb_DropIdentity(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_GetIdentityManaged(
			string cpColumnName,
			ref double pdStep,
			StringBuilder cpBuffer,
			int ulBuffLen
			)
		{
			bool res = vdb_GetIdentityManaged(cpColumnName, ref pdStep, cpBuffer, ulBuffLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_ActivateIdentity(
			string cpColumnName
			)
		{
			bool res = vdb_ActivateIdentity(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_DeactivateIdentity(
			string cpColumnName
			)
		{
			bool res = vdb_DeactivateIdentity(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_SetDefaultValue(
			string cpColumnName,
			string cpSeed,
			bool bUseInUpdate
			)
		{
			bool res = vdb_SetDefaultValue(cpColumnName, cpSeed, bUseInUpdate);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_GetDefaultValueManaged(
			string cpColumnName,
			ref bool bUseInUpdate,
			StringBuilder cpBuffer,
			int ulBuffLen
			)
		{
			bool res = vdb_GetDefaultValueManaged(cpColumnName, ref bUseInUpdate, cpBuffer, ulBuffLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_SetColumnDescription(
			string cpColumnName,
			string cpDescr
			)
		{
			bool res = vdb_SetColumnDescription(cpColumnName, cpDescr);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static string ivdb_GetColumnDescription(
			string cpColumnName
			)
		{
			string res = vdb_GetColumnDescription(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_SetColumnCaption(
			string cpColumnName, 
			string cpCaption
			)
		{
			bool res = vdb_SetColumnCaption(cpColumnName, cpCaption);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_GetColumnCaptionManaged(
			string cpColumnName,
			StringBuilder cpBuffer,
			int ulBuffLen
			)
		{
			bool res = vdb_GetColumnCaptionManaged(cpColumnName, cpBuffer, ulBuffLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_GetCaseSensitive()
		{
			bool res = vdb_GetCaseSensitive();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_IsTableExist(string cpTableName)
		{
			bool res = vdb_IsTableExist(cpTableName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_IsTriggerExist(string cpTriggerName)
		{
			bool res = vdb_IsTriggerExist(cpTriggerName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_SetTableDescription(string cpDescription)
		{
			bool res = vdb_SetTableDescription(cpDescription);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}
		
		public static string ivdb_GetTableDescription()
		{
			string res = vdb_GetTableDescription();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_AddToExportList(
			string cpTableName
			)
		{
			bool res = vdb_AddToExportList(cpTableName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_ClearExportList()
		{
			bool res = vdb_ClearExportList();
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_ExportData(
			string cpXmlFileName,
			bool bSchemaOnly
			)
		{
			bool res = vdb_ExportData(cpXmlFileName, bSchemaOnly);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_ImportData(
			string cpXmlFileName
			)
		{
			bool res = vdb_ImportData(cpXmlFileName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_ImportSchema(
			string cpXmlFileName
			)
		{
			bool res = vdb_ImportSchema(cpXmlFileName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static bool ivdb_ImportSchemaAndData(
			string cpXmlFileName
			)
		{
			bool res = vdb_ImportSchemaAndData(cpXmlFileName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static void ivdb_PutGuid(
			string cpColumnName, 
			Guid guidVal
			)
		{
			vdb_PutGuid(cpColumnName, guidVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static Guid ivdb_GetGuid(
			string cpColumnName
			)
		{
			Guid res = vdb_GetGuid(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_GetNullByIndex(
			int iColumnIndex
			)
		{
			bool res = vdb_GetNullByIndex(iColumnIndex);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static long ivdb_GetCurrencyByIndex(
			int iColumnIndex
			)
		{
			long res = vdb_GetCurrencyByIndex(iColumnIndex);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static double ivdb_GetDoubleByIndex(
			int iColumnIndex
			)
		{
			double res = vdb_GetDoubleByIndex(iColumnIndex);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static double ivdb_GetDateTimeByIndex(
			int iColumnIndex
			)
		{
			double res = vdb_GetDateTimeByIndex(iColumnIndex);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static long ivdb_GetInt64ByIndex(
			int iColumnIndex
			)
		{
			long res = vdb_GetInt64ByIndex(iColumnIndex);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivdb_GetInt32ByIndex(
			int iColumnIndex
			)
		{
			int res = vdb_GetInt32ByIndex(iColumnIndex);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static Guid ivdb_GetGuidByIndex(
			int iColumnIndex
			)
		{
			Guid res = vdb_GetGuidByIndex(iColumnIndex);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_GetBooleanByIndex(
			int iColumnIndex
			)
		{
			bool res = vdb_GetBooleanByIndex(iColumnIndex);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_GetStringManagedByIndex(
			int iColumnIndex,
			StringBuilder cpBuffer, 
			uint ulBufferLen
			)
		{
			bool res = vdb_GetStringManagedByIndex(iColumnIndex, cpBuffer, ulBufferLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static string ivdb_GetStringByIndex(
			int iColumnIndex
			)
		{
			string res = vdb_GetStringByIndex(iColumnIndex);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_SetDatabaseDescription(
			string cpDescription
			)
		{
			bool res = vdb_SetDatabaseDescription(cpDescription);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static string ivdb_GetDatabaseDescription()
		{
			string res = vdb_GetDatabaseDescription();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivdb_DropColumn(string cpColumnName)
		{
			bool res = vdb_DropColumn(cpColumnName);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static void ivdb_PutBooleanByIndex(
			int iColumn, 
			bool bVal
			)
		{
			vdb_PutBooleanByIndex(iColumn, bVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutCurrencyByIndex(
			int iColumn, 
			long iCurr
			)
		{
			vdb_PutCurrencyByIndex(iColumn, iCurr);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutDateStringByIndex(
			int iColumn,
			string cpVal
			)
		{
			vdb_PutDateStringByIndex(iColumn, cpVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutDateTimeByIndex(
			int iColumn,
			double dDateTime
			)
		{
			vdb_PutDateTimeByIndex(iColumn, dDateTime);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutDoubleByIndex(
			int iColumn,
			double dVal
			)
		{
			vdb_PutDoubleByIndex(iColumn, dVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutGuidByIndex(
			int iColumn,
			Guid guidVal
			)
		{
			vdb_PutGuidByIndex(iColumn, guidVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutInt32ByIndex(
			int iColumn,
			int lVal
			)
		{
			vdb_PutInt32ByIndex(iColumn, lVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutInt64ByIndex(int iColumn,
			long i64Val
			)
		{
			vdb_PutInt64ByIndex(iColumn, i64Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutMemoByIndex(
			int iColumn,
			string cpMemoText
			)
		{
			vdb_PutMemoByIndex(iColumn, cpMemoText);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutNullByIndex(
			int iColumnIndex
			)
		{
			vdb_PutNullByIndex(iColumnIndex);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivdb_PutStringByIndex(
			int iColumn,
			string cpVal
			)
		{
			vdb_PutStringByIndex(iColumn, cpVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}
		
		public static void ivdb_SetClusterLength(
			int iLen
			)
		{
			vdb_SetClusterLength(iLen);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static int ivdb_GetClusterLength()
		{
			int res = vdb_GetClusterLength();
			VistaDBErrorMsgs.ShowErrors(true, null);

			return res;
		}

		//----------------------------------------------------------------------------------
		// SQL support 
		//----------------------------------------------------------------------------------

		public static int ivsql_CreateDatabaseConnection()
		{
			int res = vsql_CreateDatabaseConnection();
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivsql_FreeDatabaseConnection(
			int connectionID
			)
		{
			vsql_FreeDatabaseConnection(connectionID);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivsql_OpenDatabaseConnection(
			int connectionID, 
			string cpDataBaseName, 
			bool Exclusive, 
			bool ReadOnly, 
			CypherType defaultCypher, 
			string defaultPassword,
			bool CaseSensitive
			)
		{
			bool res = vsql_OpenDatabaseConnection(connectionID, cpDataBaseName, Exclusive, ReadOnly, defaultCypher, defaultPassword, CaseSensitive);
			VistaDBErrorMsgs.ShowErrors(!res, res);

			return res;
		}

		public static void ivsql_CloseDatabaseConnection(
			int connectionID
			)
		{
			vsql_CloseDatabaseConnection(connectionID);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static bool ivsql_BeginTransaction(
			int connectionID
			)
		{
			bool res = vsql_BeginTransaction(connectionID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivsql_CommitTransaction(
			int connectionID
			)
		{
			bool res = vsql_CommitTransaction(connectionID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivsql_RollbackTransaction(
			int connectionID
			)
		{
			vsql_RollbackTransaction(connectionID);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static int ivsql_GetTransactionLevel(
			int connectionID
			)
		{
			int res = vsql_GetTransactionLevel(connectionID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_CreateQuery(
			int connectionID
			)
		{
			int res = vsql_CreateQuery(connectionID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivsql_FreeQuery(
			int QueryID
			)
		{
			vsql_FreeQuery(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static string ivsql_GetSQL(
			int QueryID
			)
		{
			string res = vsql_GetSQL(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivsql_SetSQL(
			int QueryID, 
			string cpCmdText
			)
		{
			vsql_SetSQL(QueryID, cpCmdText);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static int ivsql_Prepare(
			int QueryID
			)
		{
			int res = vsql_Prepare(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivsql_UnPrepare(
			int QueryID
			)
		{
			bool res = vsql_UnPrepare(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_Open(
			int QueryID, 
			ref int RowsAffected
			)
		{
			int res = vsql_Open(QueryID, ref RowsAffected);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_ExecSQL(
			int QueryID, 
			ref int RowsAffected
			)
		{
			int res = vsql_ExecSQL(QueryID, ref RowsAffected);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivsql_Close(
			int QueryID
			)
		{
			vsql_Close(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_SetDatabaseName(
			int QueryID, 
			string pVal
			)
		{
			vsql_SetDatabaseName(QueryID, pVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static string ivsql_GetDatabaseName(
			int QueryID
			)
		{
			string res = vsql_GetDatabaseName(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_Eof(
			int QueryID
			)
		{
			int res = vsql_Eof(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_Bof(
			int QueryID
			)
		{
			int res = vsql_Bof(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivsql_Last(
			int QueryID
			)
		{
			vsql_Last(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_First(
			int QueryID
			)
		{
			vsql_First(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_MoveBy(
			int QueryID, 
			int iVal
			)
		{
			vsql_MoveBy(QueryID, iVal);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_Next(
			int QueryID
			)
		{
			vsql_Next(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_Prior(
			int QueryID
			)
		{
			vsql_Prior(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static int ivsql_GetString(
			int QueryID, 
			int FieldNo, 
			StringBuilder strPtr, 
			int iStrLen
			)
		{
			int res = vsql_GetString(QueryID, FieldNo, strPtr, iStrLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_GetVarchar(
			int QueryID, 
			int FieldNo, 
			StringBuilder strPtr, 
			int iStrLen
			)
		{
			int res = vsql_GetVarchar(QueryID, FieldNo, strPtr, iStrLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_GetInt32(
			int QueryID, 
			int FieldNo
			)
		{
			int res = vsql_GetInt32(QueryID, FieldNo);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}
		
		public static long ivsql_GetInt64(
			int QueryID, 
			int FieldNo
			)
		{
			long res = vsql_GetInt64(QueryID, FieldNo);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static double ivsql_GetDouble(
			int QueryID, 
			int FieldNo
			)
		{
			double res = vsql_GetDouble(QueryID, FieldNo);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivsql_GetBoolean(
			int QueryID, 
			int FieldNo
			)
		{
			bool res = vsql_GetBoolean(QueryID, FieldNo);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static long ivsql_GetCurrency(
			int QueryID, 
			int FieldNo
			)
		{
			long res = vsql_GetCurrency(QueryID, FieldNo);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static long ivsql_GetDate(
			int QueryID, 
			int FieldNo
			)
		{
			long res = vsql_GetDate(QueryID, FieldNo);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static long ivsql_GetDateTime(
			int QueryID, 
			int FieldNo
			)
		{
			long res = vsql_GetDateTime(QueryID, FieldNo);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_GetMemo(
			int QueryID, 
			int FieldNo, 
			StringBuilder memPtr,
			int iMemoLen
			)
		{
			int res = vsql_GetMemo(QueryID, FieldNo, memPtr, iMemoLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_GetBlob(
			int QueryID, 
			int FieldNo,  
			byte[] memPtr, 
			int iBuffLen
			)
		{
			int res = vsql_GetBlob(QueryID, FieldNo, memPtr, iBuffLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_GetBlobLength(
			int QueryID, 
			int FieldNo
			)
		{
			int res = vsql_GetBlobLength(QueryID, FieldNo);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_GetRecNo(
			int QueryID
			)
		{
			int res = vsql_GetRecNo(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_RecCount(
			int QueryID
			)
		{
			int res = vsql_RecCount(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_RecSize(
			int QueryID
			)
		{
			int res = vsql_RecSize(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_ColumnCount(
			int QueryID
			)
		{
			int res = vsql_ColumnCount(QueryID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivsql_IsNull(
			int QueryID, 
			int FieldNo
			)
		{
			bool res = vsql_IsNull(QueryID, FieldNo);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivsql_ParamIsNull(
			int QueryID, 
			string pName
			)
		{
			bool res = vsql_ParamIsNull(QueryID, pName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static void ivsql_SetParamNull(
			int QueryID, 
			string pName, 
			short shType
			)
		{
			vsql_SetParamNull(QueryID, pName, shType);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_SetParamString(
			int QueryID, 
			string pName, 
			string Val
			)
		{
			vsql_SetParamString(QueryID, pName, Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_SetParamDate(
			int QueryID, 
			string pName, 
			long Val
			)
		{
			vsql_SetParamDate(QueryID, pName, Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}
	
		public static void ivsql_SetParamDateTime(
			int QueryID, 
			string pName, 
			long Val
			)
		{
			vsql_SetParamDateTime(QueryID, pName, Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_SetParamBoolean(
			int QueryID, 
			string pName,
			bool Val
			)
		{
			vsql_SetParamBoolean(QueryID, pName, Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_SetParamInt32(
			int QueryID, 
			string pName, 
			int Val
			)
		{
			vsql_SetParamInt32(QueryID, pName, Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_SetParamInt64(
			int QueryID, 
			string pName, 
			long Val
			)
		{
			vsql_SetParamInt64(QueryID, pName, Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_SetParamDouble(
			int QueryID, 
			string pName, 
			double Val
			)
		{
			vsql_SetParamDouble(QueryID, pName, Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_SetParamCurrency(
			int QueryID, 
			string pName,
			long Val)
		{
			vsql_SetParamCurrency(QueryID, pName, Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_SetParamVarchar(
			int QueryID, 
			string pName, 
			string Val
			)
		{
			vsql_SetParamVarchar(QueryID, pName, Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_SetParamMemo(
			int QueryID, 
			string pName, 
			string Val
			)
		{
			vsql_SetParamMemo(QueryID, pName, Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_SetParamBlob(
			int QueryID, 
			string pName, 
			byte[] Val,
			int iBlobSize
			)
		{
			vsql_SetParamBlob(QueryID, pName, Val, iBlobSize);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static void ivsql_SetParamPicture(
			int QueryID, 
			string pName, 
			byte[] Val,
			int iBlobSize
			)
		{
			vsql_SetParamPicture(QueryID, pName, Val, iBlobSize);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static int ivsql_ColumnName(
			int QueryID, 
			int No, 
			StringBuilder strPtr,
			int iStrLen
			)
		{
			int res = vsql_ColumnName(QueryID, No, strPtr, iStrLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static char ivsql_ColumnType(
			int QueryID, 
			int No
			)
		{
			char res = vsql_ColumnType(QueryID, No);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_ColumnWidth(
			int QueryID, 
			int No
			)
		{
			int res = vsql_ColumnWidth(QueryID, No);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivsql_ColumnRequired(
			int QueryID, 
			int No
			)
		{
			bool res = vsql_ColumnRequired(QueryID, No);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivsql_ColumnReadOnly(
			int QueryID, 
			int No
			)
		{
			bool res = vsql_ColumnReadOnly(QueryID, No);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivsql_ColumnIsIdentity(
			int QueryID, 
			int No
			)
		{
			bool res = vsql_ColumnIsIdentity(QueryID, No);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static int ivsql_ColumnCaption(
			int QueryID, 
			int No, 
			StringBuilder strPtr, 
			int iStrLen
			)
		{
			int res = vsql_ColumnCaption(QueryID, No, strPtr, iStrLen);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivsql_ColumnIsPrimaryKey(
			int QueryID, 
			int No
			)
		{
			bool res = vsql_ColumnIsPrimaryKey(QueryID, No);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static Guid ivsql_GetGuid(
			int SQLID, 
			int FieldNo
			)
		{
			Guid res = vsql_GetGuid(SQLID, FieldNo);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}
		
		public static void ivsql_SetParamGuid(
			int QueryID,
			string pName,
			Guid Val
			)
		{
			vsql_SetParamGuid(QueryID, pName, Val);
			VistaDBErrorMsgs.ShowErrors(true, null);
		}

		public static int ivsql_GetCurrentDatabaseID(
			int iConnectionID
			)
		{
			int res = vsql_GetCurrentDatabaseID(iConnectionID);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivsql_ColumnIsUnique(
			int QueryID, 
			int FieldNo
			)
		{
			bool res = vsql_ColumnIsUnique(QueryID, FieldNo);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivsql_IsReservedWord(
			string pName
			)
		{
			bool res = vsql_IsReservedWord(pName);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

		public static bool ivsql_AssignDatabaseConnection(
			int iConnectionID,
			int iDatabaseID,
			string cpDatabaseName,
			bool bExclusive,
			bool bReadOnly,
			int defaultCypher,
			string defaultPassword,
			bool CaseSensitive
			)
		{
			bool res = vsql_AssignDatabaseConnection(iConnectionID, iDatabaseID, cpDatabaseName,
				bExclusive, bReadOnly, defaultCypher, defaultPassword, CaseSensitive);
			VistaDBErrorMsgs.ShowErrors(true, res);

			return res;
		}

	}
}