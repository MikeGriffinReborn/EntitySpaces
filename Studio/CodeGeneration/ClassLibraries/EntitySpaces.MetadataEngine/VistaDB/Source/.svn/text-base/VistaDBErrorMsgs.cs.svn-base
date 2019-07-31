using System;
using System.Runtime.InteropServices;
using System.Collections;

namespace Provider.VistaDB
{
	delegate bool VDBUserErrorFunc(  int ulErrorCode, 
		[MarshalAs(UnmanagedType.LPStr)]string cpErrorMsg, 
		[MarshalAs(UnmanagedType.LPStr)]string cpExtraInfo,
		int ulCardinal );

	/// <summary>
	/// Error codes
	/// </summary>
	public enum VistaDBErrorCodes
	{
		/// <summary>
		/// Query cannot be opened
		/// </summary>
		QueryCannotOpen                  = 0,
		/// <summary>
		/// Query is not open. No result set to work with
		/// </summary>
		QueryNotOpened                   = 1,
		/// <summary>
		/// Connection cannot be changed
		/// </summary>
		ConnectionCannotBeChanged        = 2,
		/// <summary>
		/// Connection.DataSource is not set
		/// </summary>
		ConnectionDataSourceIsEmpty      = 3,
		/// <summary>
		/// Connection is invalid.
		/// </summary>
		ConnectionInvalid                = 4,
		/// <summary>
		/// Database could not be found
		/// </summary>
		SQLDatabaseCouldNotBeFound       = 5,
		/// <summary>
		/// Table does not have a unique column
		/// </summary>
		TableDoesNotHaveUniqueColumn     = 6,
		/// <summary>
		/// DatabaseName cannot be changed
		/// </summary>
		DatabaseNameCanNotBeChanged      = 7,
		/// <summary>
		/// ReadOnly cannot be changed
		/// </summary>
		ReadOnlyCanNotBeChanged          = 8,
		/// <summary>
		/// Exclusive cannot be changed
		/// </summary>
		ExclusiveCanNotBeChanged         = 9,
		/// <summary>
		/// Parameters cannot be changed
		/// </summary>
		ParametersCanNotBeChanged        = 10,
		/// <summary>
		/// Database must be closed before creating
		/// </summary>
		DatabaseMustBeClosedBeforeCreate = 11,
		/// <summary>
		/// Database cannot be changed.
		/// </summary>
		DatabaseCanNotBeChanged          = 12,
		/// <summary>
		/// Table is not in Create mode
		/// </summary>
		TableNotInCreateMode             = 13,
		/// <summary>
		/// Database is not assigned
		/// </summary>
		DatabaseNotAssigned              = 14,
		/// <summary>
		/// Table is not opened
		/// </summary>
		TableNotOpened                   = 15,
		/// <summary>
		/// VistaDB object cannot be activated
		/// </summary>
		ObjectCannotBeActivated          = 16,
		/// <summary>
		/// VistaDB object cannot be deactivated
		/// </summary>
		ObjectCannotBeDeactivated        = 17,
		/// <summary>
		/// Table column does not exist
		/// </summary>
		ColumnNotExist                   = 18,
		/// <summary>
		/// Database is not opened
		/// </summary>
		DatabaseNotOpened                = 19,
		/// <summary>
		/// Unusable access mode
		/// </summary>
		UnusableAccessMode               = 20,
		/// <summary>
		/// Property cannot be changed while connection opened
		/// </summary>
		PropertyCannotBeChanged          = 21,
		/// <summary>
		/// Connection is not opened
		/// </summary>
		ConnectionNotOpened              = 22,
		/// <summary>
		/// Server error
		/// </summary>
		ServerError                      = 23,
		/// <summary>
		/// Database must be opened as temporary
		/// </summary>
		DatabaseMustBeTemporary          = 24,

		/// <summary>
		/// Duplicate Alias name
		/// </summary>
		AliasDupe                        = 100,
		/// <summary>
		/// Alias name exceeds maximum length allowed
		/// </summary>
		AliasLen                         = 101,
		/// <summary>
		/// Invalid Name or Alias
		/// </summary>
		InvalidAlias                     = 102,
		/// <summary>
		/// Database creation error
		/// </summary>
		CreateDatabase                   = 103,
		/// <summary>
		/// Database connection error
		/// </summary>
		OpenDatabase                     = 104,
		/// <summary>
		/// Table creation error
		/// </summary>
		CreateTable                      = 105,
		/// <summary>
		/// Table opening error
		/// </summary>
		OpenTable                        = 106,
		/// <summary>
		/// Database format error. Does not appear to be a valid .VDB database.
		/// </summary>
		NotADatabase                     = 107,
		/// <summary>
		/// Database must be opened in Exclusive mode
		/// </summary>
		MustExclusive                    = 108,
		/// <summary>
		/// Index file format error
		/// </summary>
		NotIndex                         = 109,
		/// <summary>
		/// Invalid database engine property
		/// </summary>
		InvalidSysProp                   = 110,
		/// <summary>
		/// Invalid characters in Column name
		/// </summary>
		InvalidColumnName                = 112,
		/// <summary>
		/// Duplicate Column name
		/// </summary>
		DupeColumn                       = 113,
		/// <summary>
		/// Column not found
		/// </summary>
		ColumnNotFound                   = 114,
		/// <summary>
		/// File creation error
		/// </summary>
		FileCreate                       = 115,
		/// <summary>
		/// File locking error
		/// </summary>
		FileLock                         = 116,
		/// <summary>
		/// File opening error
		/// </summary>
		FileOpen                         = 117,
		/// <summary>
		/// File opening mode error
		/// </summary>
		FileOpenMode                     = 118,
		/// <summary>
		/// File read error
		/// </summary>
		FileRead                         = 119,
		/// <summary>
		/// Index opening error
		/// </summary>
		FileOpenIndex                    = 120,
		/// <summary>
		/// File write error
		/// </summary>
		FileWrite                        = 121,
		/// <summary>
		/// Invalid child Relationships
		/// </summary>
		InvalidChild                     = 123,
		/// <summary>
		/// Unsupported data type
		/// </summary>
		InvalidType                      = 124,
		/// <summary>
		/// Invalid Memo block size
		/// </summary>
		InvalidString                    = 129,
		/// <summary>
		/// Invalid Record
		/// </summary>
		InvalidRecord                    = 130,
		/// <summary>
		/// Invalid database structure
		/// </summary>
		InvalidStruc                     = 131,
		/// <summary>
		/// Invalid connection to database
		/// </summary>
		NoDatabase                       = 132,
		/// <summary>
		/// Not enough disk space to sort keys
		/// </summary>
		DiskSpace                        = 137,
		/// <summary>
		/// Unsuitable Table driver specified
		/// </summary>
		FileVDBType                      = 140,
		/// <summary>
		/// Expression length exceeds maximum allowed
		/// </summary>
		ExprLen                          = 142,
		/// <summary>
		/// Expression error: Invalid number of parameters
		/// </summary>
		InvalidParams                    = 143,
		/// <summary>
		/// Expression error: Invalid type
		/// </summary>
		ExprTypChk                       = 144,
		/// <summary>
		/// Expression evaluation error
		/// </summary>
		BadExpr                          = 145,
		/// <summary>
		/// Expression is incomplete
		/// </summary>
		IncompExpr                       = 146,
		/// <summary>
		/// Expression error: Number of delimiters is invalid
		/// </summary>
		InvalidDelim                     = 147,
		/// <summary>
		/// Expression error: Operator is invalid
		/// </summary>
		InvalidOp                        = 148,
		/// <summary>
		/// Expression error: Parentheses are mismatched
		/// </summary>
		ParenMismatch                    = 149,
		/// <summary>
		/// Expression error: String delimiter is missing
		/// </summary>
		MissinDelim                      = 150,
		/// <summary>
		/// Expression error: Type mismatch within function or operator parameters
		/// </summary>
		XType                            = 151,
		/// <summary>
		/// System Error
		/// </summary>
		SystemError                      = 152,
		/// <summary>
		/// Index name is required
		/// </summary>
		IndexRequired                    = 153,
		/// <summary>
		/// Duplicate Index name
		/// </summary>
		IndexDuplicate                   = 154,
		/// <summary>
		/// Memory allocation system error
		/// </summary>
		OutOfMemory                      = 156,
		/// <summary>
		/// Disk space allocation error
		/// </summary>
		FileSpaceAlloc                   = 157,
		/// <summary>
		/// Incompatible user mode
		/// </summary>
		IncompatibleMode                 = 158,
		/// <summary>
		/// Internal error
		/// </summary>
		InternalError                    = 159,
		/// <summary>
		/// Unable to initialize engine
		/// </summary>
		CannotInitLib                    = 160,
		/// <summary>
		/// Unable to initialize thread apartment
		/// </summary>
		CannotInitParent                 = 161,
		/// <summary>
		/// Unable to create record
		/// </summary>
		CreateRec                        = 162,
		/// <summary>
		/// Unable to update record
		/// </summary>
		UpdateRec                        = 163,
		/// <summary>
		/// Unable to delete record
		/// </summary>
		DeleteRec                        = 164,
		/// <summary>
		/// Column contains Null value
		/// </summary>
		ErrorNull                        = 165,
		/// <summary>
		/// Old VistaDB database format. Pack the database to update.
		/// </summary>
		IncompVersion                    = 166,
		/// <summary>
		/// Unable to start transaction
		/// </summary>
		TPFaultToStart                   = 167,
		/// <summary>
		/// Transaction fault
		/// </summary>
		TPFaultCommit                    = 168,
		/// <summary>
		/// SureCommit forced
		/// </summary>
		TPSureCommit                     = 169,
		/// <summary>
		/// Record is out of date
		/// </summary>
		RecordVersion                    = 170,
		/// <summary>
		/// Transaction rolled back
		/// </summary>
		TPRollback                       = 171,
		/// <summary>
		/// Windows Locale that is not currently installed is being used for index collation
		/// </summary>
		NonInstLocale                    = 172,
		/// <summary>
		/// Unsupported Windows Locale used for index collation
		/// </summary>
		NonSuppLocale                    = 173,
		/// <summary>
		/// Incorrect Trigger type
		/// </summary>
		BadTriggerType                   = 174,
		/// <summary>
		/// Invalid Index order for table
		/// </summary>
		InvalidOrder                     = 175,
		/// <summary>
		/// Incorrect decryption key entered
		/// </summary>
		WrongPassword                    = 176,
		/// <summary>
		/// Incorrect symbol
		/// </summary>
		WrongSymbol                      = 177,
		/// <summary>
		/// Incorrect data length in Varchar Column
		/// </summary>
		WrongVarcharLength               = 178,
		/// <summary>
		/// Incorrect property for Column
		/// </summary>
		WrongColumnProperty              = 179,
		/// <summary>
		/// Primary Key index cannot contain Null values. Check Column definition.
		/// </summary>
		PrimaryContainsNull              = 180,
		/// <summary>
		/// Duplicate key in index
		/// </summary>
		DupKey                           = 181,
		/// <summary>
		/// Trigger fault
		/// </summary>
		TriggerFault                     = 182,
		/// <summary>
		/// Constraint fault
		/// </summary>
		ConstraintFault                  = 183,
		/// <summary>
		/// Unable to update. Column is Readonly
		/// </summary>
		ReadOnlyFault                    = 184,
		/// <summary>
		/// Unable to assign default value
		/// </summary>
		DefaultValueFault                = 185,
		/// <summary>
		/// Incorrect syntax. Use\n 'Column1: Column1Value; Column2: Column2Value; ... '
		/// </summary>
		WrongKeySyntax                   = 186,
		/// <summary>
		/// Then() operator expected
		/// </summary>
		WrongIfElse                      = 187,
		/// <summary>
		/// Unexpected Then() operator
		/// </summary>
		NonExpectedThen                  = 188,
		/// <summary>
		/// Unexpected Else() operator
		/// </summary>
		NonExpectedElse                  = 189,
		/// <summary>
		/// Unable to set Trigger
		/// </summary>
		CannotSetTrigger                 = 190,
		/// <summary>
		/// Unable to set Constraint
		/// </summary>
		CannotSetConstraint              = 191,
		/// <summary>
		/// Unable to set Foreign Key Constraint
		/// </summary>
		CannotSetFKConstraint            = 192,
		/// <summary>
		/// Unable to set Identity
		/// </summary>
		CannotSetIdentity                = 193,
		/// <summary>
		/// Unable to set read-only property
		/// </summary>
		CannotSetReadOnly                = 194,
		/// <summary>
		/// Unable to set default value for column
		/// </summary>
		CannotSetFDefaultValue           = 195,
		/// <summary>
		/// Incorrect reference to Primary Key
		/// </summary>
		WrongPKReference                 = 196,
		/// <summary>
		/// Unable to set Column property
		/// </summary>
		BadColumnProp                    = 197,
		/// <summary>
		/// Column has an Identity
		/// </summary>
		IdentitySet                      = 198,
		/// <summary>
		/// Broken relationships. Unable to alter/drop table or primary index
		/// </summary>
		CannotAlterRelation              = 199,
		/// <summary>
		/// Invalid Column name characters. Using a reserved word.
		/// </summary>
		InvalidColumnNameRes		         = 200,
		/// <summary>
		/// Key length exceeds maximum allowed value
		/// </summary>
		MaxKeyLen							      		 = 201,
		/// <summary>
		/// Key expression contains wrong column
		/// </summary>
		IndexKeyColumn						       = 202,
		/// <summary>
		/// Unable to re-assign default value for column
		/// </summary>
		DefaultValueAssigned			       = 203,
		/// <summary>
		/// Newer VistaDB database format detected. Update the VistaDB engine before proceeding.
		/// </summary>
		OutdatedEngine						       = 204,

		//SQL
		/// <summary>
		/// Initial SQL Error. Details to follow.
		/// </summary>
		SQLFirstError                    = 500,
		/// <summary>
		/// SQL Parser syntax error
		/// </summary>
		SQLExprParserError               = 501,
		/// <summary>
		/// INSERT Column does not exist in the table
		/// </summary>
		SQLInsertWrongFieldName          = 502,
		/// <summary>
		/// Columns in GROUP BY must by specified by SELECT columns
		/// </summary>
		SQLGroupByInCongruent            = 503,
		/// <summary>
		/// Use SELECT only with ExecuteQuery method
		/// </summary>
		SQLIsNotValidInExecSQL           = 504,
		/// <summary>
		/// Use UPDATE, DELETE, INSERT or CREATE TABLE with ExecuteNonQuery method
		/// </summary>
		SQLIsNotValidInSelect            = 505,
		/// <summary>
		/// GetRecord Invalid record
		/// </summary>
		SQLGetRecordInvalid              = 506,
		/// <summary>
		/// Parameter types for Extract must be of Float or Integer
		/// </summary>
		SQLWrongParamsInExtract          = 507,
		/// <summary>
		/// Parameter types for date/time function must be of Float or Integer
		/// </summary>
		SQLWrongParamsInDateTime         = 508,
		/// <summary>
		/// SQL statement may not be empty
		/// </summary>
		SQLIsEmpty			                 = 509,
		/// <summary>
		/// Length of trimmed character returned by TRIM() must be at least 1
		/// </summary>
		SQLWrongLengthInTrim             = 510,
		/// <summary>
		/// Incorrect expression in HAVING clause
		/// </summary>
		SQLHavingExprWrong               = 511,
		/// <summary>
		/// Aggregate function not found in SELECT
		/// </summary>
		SQLHavingWrong                   = 512,
		/// <summary>
		/// Circular reference not allowed
		/// </summary>
		SQLCircularReference             = 513,
		/// <summary>
		/// Number of tables in JOIN clause must match the number of tables in the FROM clause
		/// </summary>
		SQLJoinPredicateWrong            = 514,
		/// <summary>
		/// Table is not opened
		/// </summary>
		SQLDataSetNotOpened              = 515,
		/// <summary>
		/// Invalid Table name in expression
		/// </summary>
		SQLWrongDataSetNameInExpr        = 516,
		/// <summary>
		/// Invalid Column name in expression
		/// </summary>
		SQLWrongResultSetFieldName       = 517,
		/// <summary>
		/// JOIN clause is incorrectly defined
		/// </summary>
		SQLJoinExpectedDataSet           = 518,
		/// <summary>
		/// Parameter(s) invalid for function
		/// </summary>
		SQLWrongParameters               = 519,
		/// <summary>
		/// Argument must be alphanumeric
		/// </summary>
		SQLWrongFirstArg                 = 520,
		/// <summary>
		/// Duplicate Table name exists in the table list
		/// </summary>
		SQLDuplicatedDataSet             = 521,
		/// <summary>
		/// Duplicate Column name exists
		/// </summary>
		SQLColumnRepeated                = 522,
		/// <summary>
		/// Record number out of range
		/// </summary>
		SQLRecNoInvalid                  = 523,
		/// <summary>
		/// Each Column listed in GROUP BY must exist in the SELECT clause
		/// </summary>
		SQLGroupBySelectWrong            = 524,
		/// <summary>
		/// Number of Columns in GROUP differs from Columns listed in SELECT
		/// </summary>
		SQLGroupByWrongNum               = 525,
		/// <summary>
		/// Incorrect table name in format
		/// </summary>
		SQLWrongTableName                = 526,
		/// <summary>
		/// Incorrect Column index
		/// </summary>
		SQLWrongIndexField               = 527,
		/// <summary>
		/// Column was not found
		/// </summary>
		SQLFieldNotFound                 = 528,
		/// <summary>
		/// Left and right tables in JOIN must be different
		/// </summary>
		SQLJoinOnMustHaveDiffTables      = 529,
		/// <summary>
		/// Right table in JOIN must match second table in FROM
		/// </summary>
		SQLJoinOnWrongRightTable	       = 530,
		/// <summary>
		/// Left table in JOIN must match first table in FROM
		/// </summary>
		SQLJoinOnWrongLeftTable          = 531,
		/// <summary>
		/// Number of tables in FROM clause differs from the JOIN ON clause
		/// </summary>
		SQLJoinOnWrongTableNum		       = 532,
		/// <summary>
		/// Subquery with more than one Column is not allowed
		/// </summary>
		SQLSubQueryWrongCols             = 533,
		/// <summary>
		/// More than one table in FROM clause found in subquery
		/// </summary>
		SQLSubQueryWrongTables           = 534,
		/// <summary>
		/// No Tables defined in FROM clause
		/// </summary>
		SQLWrongTableNumber              = 535,
		/// <summary>
		/// Column name not found
		/// </summary>
		SQLWrongFieldName                = 536,
		/// <summary>
		/// Table does not exist
		/// </summary>
		SQLWrongDataSetName              = 537,
		/// <summary>
		/// Error specifying a table Column
		/// </summary>
		SQLErrorInDBField                = 538,
		/// <summary>
		/// Column may not have parameters
		/// </summary>
		SQLCannotContainParams           = 539,
		/// <summary>
		/// Expression is empty
		/// </summary>
		SQLExpressionNull                = 540,
		/// <summary>
		/// Expression must be Boolean type
		/// </summary>
		SQLExprNotBoolean                = 541,
		/// <summary>
		/// Unable to find a record that corresponds to the expression
		/// </summary>
		SQLRecordNotFound                = 542,
		/// <summary>
		/// Bookmark not found
		/// </summary>
		SQLBookMarkNotFound              = 543,
		/// <summary>
		/// Index is out of range
		/// </summary>
		SQLIndexOutOfRange               = 544,
		/// <summary>
		/// Error in WHERE clause
		/// </summary>
		SQLErrorInWhere                  = 545,
		/// <summary>
		/// Parameter not found in the list of params
		/// </summary>
		SQLParameterNotFound             = 546,
		/// <summary>
		/// (LoadFromBinaryFile) File does not exist
		/// </summary>
		SQLFileNotExist                  = 547,
		/// <summary>
		/// Duplicate Column name
		/// </summary>
		SQLDuplicateFieldName            = 548,
		/// <summary>
		/// Column name not found
		/// </summary>
		SQLFieldNameNotFound             = 549,
		/// <summary>
		/// Incorrect BLOB type
		/// </summary>
		SQLBLOBFieldWrongType            = 550,
		/// <summary>
		/// Unable to read field as Boolean
		/// </summary>
		SQLReadBooleanField              = 551,
		/// <summary>
		/// Unable to read field as Float
		/// </summary>
		SQLReadFloatField                = 552,
		/// <summary>
		/// Unable to read field as Integer
		/// </summary>
		SQLReadIntegerField              = 553,
		/// <summary>
		/// Unable to read field as String
		/// </summary>
		SQLReadStringField               = 554,
		/// <summary>
		/// Unable to assign field as Boolean
		/// </summary>
		SQLWriteBooleanField             = 555,
		/// <summary>
		/// Unable to assign field as Float
		/// </summary>
		SQLWriteFloatField               = 556,
		/// <summary>
		/// Unable to assign field as Integer
		/// </summary>
		SQLWriteIntegerField             = 557,
		/// <summary>
		/// Unable to assign field as String
		/// </summary>
		SQLWriteStringField              = 558,
		/// <summary>
		/// Invalid Float value
		/// </summary>
		SQLIsInvalidFloatValue           = 559,
		/// <summary>
		/// Invalid Integer value
		/// </summary>
		SQLIsInvalidIntegerValue         = 560,
		/// <summary>
		/// Invalid Boolean value
		/// </summary>
		SQLIsInvalidBoolValue            = 561,
		/// <summary>
		/// Column is not an aggregate type
		/// </summary>
		SQLNotAnAggregate                = 562,
		/// <summary>
		/// Invalid Column number
		/// </summary>
		SQLInvalidFieldNo                = 563,
		/// <summary>
		/// Number of columns in SELECT mismatch in GROUP BY
		/// </summary>
		SQLTransfColumnMismatch          = 564,
		/// <summary>
		/// Number of columns in SELECT mismatch in ORDER BY
		/// </summary>
		SQLTransfOrderByMismatch         = 565,
		/// <summary>
		/// Column order in GROUP BY must match in SELECT
		/// </summary>
		SQLTransWrongColumnGroup         = 566,
		/// <summary>
		/// Column order in order BY must match in SELECT
		/// </summary>
		SQLTransfWrongColumnOrder        = 567,
		/// <summary>
		/// Failed to open or create file
		/// </summary>
		SQLFailOpenFile                  = 568,
		/// <summary>
		/// Failed to create file mapping
		/// </summary>
		SQLFailCreateMapping             = 569,
		/// <summary>
		/// Failed to map view of file
		/// </summary>
		SQLFailMapView                   = 570,
		/// <summary>
		/// Position beyond end of file (EOF)
		/// </summary>
		SQLBeyondEof                     = 571,
		/// <summary>
		/// List index error
		/// </summary>
		SQLListError                     = 572,
		/// <summary>
		/// Syntax Error
		/// </summary>
		SQLSyntaxErrorMsg                = 573,
		/// <summary>
		/// Params were not updated
		/// </summary>
		SQLParamsError                   = 574,
		/// <summary>
		/// Alias already exists
		/// </summary>
		SQLDuplicateAlias                = 575,
		/// <summary>
		/// Nested subqueries are not allowed in SELECT subquery
		/// </summary>
		SQLSubQueryInSelectsError        = 576,
		/// <summary>
		/// Simultaneous use of both quote and double quote is not supported
		/// </summary>
		SQLWrongParameterQuotes          = 577,
		/// <summary>
		/// More than one table being accessed. JOIN is required.
		/// </summary>
		SQLWrongJoin                     = 578,
		/// <summary>
		/// Use ANY or ALL in subquery, but not both
		/// </summary>
		SQLSubQueryKindWrong             = 579,
		/// <summary>
		/// Table listed in the INTO clause does not exist
		/// </summary>
		SQLWrongToTable                  = 580,
		/// <summary>
		/// Column not found
		/// </summary>
		SQLXQFieldNotFound               = 581,
		/// <summary>
		/// CASE statement requires an alias
		/// </summary>
		SQLCaseListMissingAlias          = 582,
		/// <summary>
		/// Expression is not of Boolean type in CASE clause
		/// </summary>
		SQLCaseExprNotBoolean            = 583,
		/// <summary>
		/// Tables referenced in JOIN do not correspond
		/// </summary>
		SQLJoinNotMismatch               = 584,
		/// <summary>
		/// (ParamsAsFields) duplicate name
		/// </summary>
		SQLDupParamsAsFields             = 585,
		/// <summary>
		/// Internal JOIN optimization error
		/// </summary>
		SQLJoinOptimization              = 586,
		/// <summary>
		/// Currency conversion error
		/// </summary>
		SQLCurrencyConv                  = 587,
		/// <summary>
		/// In64 conversion error
		/// </summary>
		SQLInt64Conv                     = 588,
		/// <summary>
		/// Statement did not return a ResultSet
		/// </summary>
		SQLDidNotReturnAres              = 589,
		/// <summary>
		/// Unable to open table
		/// </summary>
		SQLCouldNotOpenTab               = 590,
		/// <summary>
		/// Unknown identifier
		/// </summary>
		SQLUnknownIdentifier             = 591,
		/// <summary>
		/// Error setting filter for WHERE clause
		/// </summary>
		SQLWhereFilter                   = 592,
		/// <summary>
		/// Unable to create database
		/// </summary>
		SQLCouldNotCreateDB              = 593,
		/// <summary>
		/// Unable to create table
		/// </summary>
		SQLCouldNotCreateTab             = 594,
		/// <summary>
		/// Invalid Column type
		/// </summary>
		SQLInvalidFieldType              = 595,
		/// <summary>
		/// Unable to create primary key
		/// </summary>
		SQLCouldNotCreatePK              = 596,
		/// <summary>
		/// Unable to create constraint
		/// </summary>
		SQLCreateConstraint              = 597,
		/// <summary>
		/// Unable to open table in exclusive mode
		/// </summary>
		SQLOpenTableExclusive            = 598,
		/// <summary>
		/// Unable to create index
		/// </summary>
		SQLCreateIndex                   = 599,
		/// <summary>
		/// Unable to drop index
		/// </summary>
		SQLDropIndex                     = 600,
		/// <summary>
		/// Database does not exist
		/// </summary>
		SQLDatabaseFileName              = 601,
		/// <summary>
		/// Unable to alter table
		/// </summary>
		SQLAlterTable                    = 602,
		/// <summary>
		/// Unable to reindex table
		/// </summary>
		SQLReindexTable                  = 603,
		/// <summary>
		/// Unable to drop table
		/// </summary>
		SQLDropTable                     = 604,
		/// <summary>
		/// Unable to delete all rows of table
		/// </summary>
		SQLZapTable                      = 605,
		/// <summary>
		/// Unable to create trigger
		/// </summary>
		SQLCreateTrigger                 = 606,
		/// <summary>
		/// Unable to alter trigger
		/// </summary>
		SQLAlterTrigger                  = 607,
		/// <summary>
		/// Unable to drop trigger
		/// </summary>
		SQLDropTrigger                   = 608,
		/// <summary>
		/// Invalid internal database connection
		/// </summary>
		SQLInvalidInternalDB             = 609,
		/// <summary>
		/// OpenSQL error
		/// </summary>
		SQLOpen                          = 610,
		/// <summary>
		/// ExecSQL error
		/// </summary>
		SQLExec                          = 611,
		/// <summary>
		/// Internal SQL error
		/// </summary>
		SQLInternalError                 = 612,
		/// <summary>
		/// Last SQL error to report.
		/// </summary>
		SQLLastError                     = 613
	}

	/// <summary>
	/// VistaDB error structure
	/// </summary>
	public class VistaDBErrorStruct
	{
		/// <summary>
		/// Error code
		/// </summary>
		public VistaDBErrorCodes errorCode;
		/// <summary>
		/// Error message
		/// </summary>
		public string errorMsg;
		/// <summary>
		/// Error extra info
		/// </summary>
		public string extraInfo;
	}

	internal class VistaDBErrorMsgs
	{
		public  static bool errorTrapped = false;
		public  static Queue queue = new Queue();
		private static VDBUserErrorFunc errorFunc;

		public static string[] Errors =
			{
				"Query cannot be opened",
				"Query not opened",
				"Cannot change connection string while the connection is open",
				"Must specify a DataSource prior to Open",
				"Connection must be valid and open",
				"Database could not be found",
				"Table doesn't have unique column",
				"Property DatabaseName cannot be changed while database is opened",
				"Property ReadOnly cannot be changed while database id opened",
				"Property Exclusive cannot be changed while database id opened",
				"Property Parameters cannot be changed while database id opened",
				"Database must be closed before create",
				"Property Database cannot be changed while table is opened",
				"Table is not in create mode",
				"Database not assigned",
				"Table is not opened",
				"Object cannot be activated",
				"Object cannot be deactivated",
				"Column doesn't exist",
				"Database is not opened",
				"Unusable access mode",
				"Property cannot be changed while connection opened",
				"Connection is not opened",
				"Server error",
				"Database must be opened as temporary"
			};
	
		public static bool UserErrorFunc(  int ulErrorCode, 
			string cpErrorMsg, string cpExtraInfo, int ulCardinal )
		{
			VistaDBErrorStruct error = new VistaDBErrorStruct();

			error.errorCode = (VistaDBErrorCodes)ulErrorCode;
			error.errorMsg = cpErrorMsg;
			error.extraInfo = cpExtraInfo;

			lock(queue.SyncRoot)
			{
				queue.Enqueue(error);
			}

			return true;
		}

		public static void ShowErrors(bool ShowError, object result)
		{
			string msg = "";
			int count = 0;
			VistaDBErrorStruct error;
			ArrayList list = new ArrayList();

			lock(queue.SyncRoot)
			{
				while(queue.Count > 0)
				{
					error = (VistaDBErrorStruct)queue.Dequeue();

					msg += "\r\nError code: " + ((int)error.errorCode).ToString() + 
						" " +
						error.errorMsg + " " + error.extraInfo;

					list.Add(error);

					count++;
				}
			}

			if( count > 0 )
			{
				throw new VistaDBException(msg, list, ShowError, result);
			}
		}

		public static void SetErrorFunc()
		{
			if( errorTrapped )
				return;

			queue = new Queue(10);

			VistaDBAPI.ivdb_ErrorLevel(2);

			errorFunc = new VDBUserErrorFunc(UserErrorFunc);
			VistaDBAPI.ivdb_SetErrorFunc(errorFunc, 0);
			errorTrapped = true;
		}
	}

	/// <summary>
	/// VistaDB exception class
	/// </summary>
	public class VistaDBException: InvalidOperationException
	{
		private ArrayList arrayList;
		private bool critical;
		private object result;

		internal VistaDBException(string message, ArrayList list, bool _critical, object _result): base(message)
		{
			this.arrayList = list;
			this.critical  = _critical;
			this.result    = _result;
		}

		internal VistaDBException(VistaDBErrorCodes error): base(VistaDBErrorMsgs.Errors[(int)error])
		{
			this.arrayList = null;
			this.critical  = true;
			this.result    = null;
		}

		internal VistaDBException(string message, VistaDBErrorCodes error): base(message)
		{
			this.arrayList = new ArrayList();
			this.arrayList.Add(error);
			this.critical  = true;
			this.result    = null;
		}

		/// <summary>
		/// Return True is pointed error code present
		/// </summary>
		/// <param name="errorCode">Error code</param>
		/// <returns>True is pointed error code present</returns>
		public bool IsErrorCodePresent(VistaDBErrorCodes errorCode)
		{
			bool res = false;

			if(arrayList != null)
			{
				for(int i = 0; i < arrayList.Count; i++)
				{
					if( ((VistaDBErrorStruct)arrayList[i]).errorCode == errorCode )
					{
						res = true;
						break;
					}
				}
			}

			return res;
		}

		/// <summary>
		/// Errors count
		/// </summary>
		/// <returns>Errors count</returns>
		public int ErrorCount()
		{
			if(arrayList != null)
				return arrayList.Count;
			else
				return 0;
		}

		/// <summary>
		/// Return error code by zero-based index
		/// </summary>
		/// <param name="index">Zero-based index</param>
		/// <returns>Error code by zero-based index</returns>
		public VistaDBErrorStruct ErrorCode(int index)
		{
			if(arrayList != null)
				return (VistaDBErrorStruct)(arrayList[index]);
			else
				return null;
		}

		/// <summary>
		/// True is error is critical
		/// </summary>
		public bool Critical
		{
			get
			{
				return critical;
			}
		}

		/// <summary>
		/// Result value of function, which raised error
		/// </summary>
		public object Result
		{
			get
			{
				return result;
			}
		}
	}
}
