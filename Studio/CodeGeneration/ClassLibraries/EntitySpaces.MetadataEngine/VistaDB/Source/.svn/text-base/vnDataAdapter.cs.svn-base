#if !IGNORE_VISTA

using System;
using System.Data;
using System.Data.Common;
using System.ComponentModel;


namespace Provider.VistaDB
{
	/// <summary>
	/// Represents a set of data commands and a database connection that are used to fill the DataSet 
	/// and update a VistaDB database. This class cannot be inherited. 
	/// </summary>
	[Designer("VistaDB.Designer.VistaDBDataAdapterDesigner, VistaDB.Designer.VS2003")]
	public sealed class VistaDBDataAdapter: DbDataAdapter, IDbDataAdapter
	{

		private VistaDBCommand selectCommand;
		private VistaDBCommand insertCommand;
		private VistaDBCommand updateCommand;
		private VistaDBCommand deleteCommand;

		/// <summary>
		/// Occurs during Update before a command is executed against the data source. 
		/// The attempt to update is made, so the event fires.
		/// </summary>
		public event VistaDBRowUpdatingEventHandler RowUpdating;

		/// <summary>
		/// Occurs during Update after a command is executed against the data source. 
		/// The attempt to update is made, so the event fires.
		/// </summary>
		public event VistaDBRowUpdatedEventHandler RowUpdated;

		/// <summary>
		/// Overloaded constructor.
		/// </summary>
		public VistaDBDataAdapter()
		{
			RowUpdating = null;
			RowUpdated = null;
		}
		/// <summary>
		/// Overloaded constructor.
		/// </summary>
		/// <param name="comm">VistaDBCommand select command object</param>
		public VistaDBDataAdapter( VistaDBCommand comm )
		{
			selectCommand = comm;
		}
		/// <summary>
		/// Overloaded constructor.
		/// </summary>
		/// <param name="commText">V-SQL query text</param>
		/// <param name="conn">VistaDBConnection object</param>
		public VistaDBDataAdapter( string commText, VistaDBConnection conn)
		{
			selectCommand = new VistaDBCommand(commText, conn);
		}
		/// <summary>
		/// Overloaded constructor.
		/// </summary>
		/// <param name="commText">V-SQL query text</param>
		/// <param name="connString">Connectionstring to a VistaDB database</param>
		public VistaDBDataAdapter( string commText, string connString )
		{
			VistaDBConnection conn = new VistaDBConnection(connString);
			selectCommand = new VistaDBCommand(commText, conn);
		}

		/// <summary>
		/// VistaDBAdapter destructor
		/// </summary>
		~VistaDBDataAdapter()
		{
			Dispose(false);
		}

		/// <summary>
		/// Gets or sets the select command object.
		/// </summary>
		IDbCommand IDbDataAdapter.SelectCommand
		{
			get
			{
				return selectCommand;
			}
			set
			{
				selectCommand = (VistaDBCommand)value;
			}
		}
		/// <summary>
		/// Gets or sets the insert command object.
		/// </summary>
		IDbCommand IDbDataAdapter.InsertCommand
		{
			get
			{
				return insertCommand;
			}
			set
			{
				insertCommand = (VistaDBCommand)value;
			}
		}
		/// <summary>
		/// Gets or sets the update command object.
		/// </summary>
		IDbCommand IDbDataAdapter.UpdateCommand
		{
			get
			{
				return updateCommand;
			}
			set
			{
				updateCommand = (VistaDBCommand)value;
			}
		}
		/// <summary>
		/// Gets or sets the delete command object.
		/// </summary>
		IDbCommand IDbDataAdapter.DeleteCommand
		{
			get
			{
				return deleteCommand;
			}
			set
			{
				deleteCommand = (VistaDBCommand)value;
			}
		}
		/// <summary>
		/// Gets or sets a V-SQL statement to select records from the data set.
		/// </summary>
		[TypeConverter(typeof(ComponentConverter))]
		public VistaDBCommand SelectCommand
		{
			get
			{
				return selectCommand;
			}
			set
			{
				selectCommand = (VistaDBCommand)value;
			}
		}

		/// <summary>
		/// Gets or sets a V-SQL statement to insert records into the data set.
		/// </summary>
		[TypeConverter(typeof(ComponentConverter))]
		public VistaDBCommand InsertCommand
		{
			get
			{
				return insertCommand;
			}
			set
			{
				insertCommand = (VistaDBCommand)value;
			}
		}

		/// <summary>
		/// Gets or sets a V-SQL statement to update records in the data set.
		/// </summary>
		[TypeConverter(typeof(ComponentConverter))]
		public VistaDBCommand UpdateCommand
		{
			get
			{
				return updateCommand;
			}
			set
			{
				updateCommand = (VistaDBCommand)value;
			}
		}

		/// <summary>
		/// Gets or sets a V-SQL statement to delete records from the data set.
		/// </summary>
		[TypeConverter(typeof(ComponentConverter))]
		public VistaDBCommand DeleteCommand
		{
			get
			{
				return deleteCommand;
			}
			set
			{
				deleteCommand = (VistaDBCommand)value;
			}
		}


		/// <summary>
		/// Initializes a new instance of the RowUpdatedEventArgs class.
		/// </summary>
		/// <param name="dataRow">The DataRow used to update the data source. </param>
		/// <param name="command">The IDbCommand executed during the Update. </param>
		/// <param name="statementType">Whether the command is an UPDATE, INSERT, DELETE, or SELECT statement. </param>
		/// <param name="tableMapping">A DataTableMapping object. </param>
		/// <returns>A new instance of the RowUpdatingEventArgs class.</returns>
		protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new VistaDBRowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
		}
		/// <summary>
		/// Initializes a new instance of the RowUpdatingEventArgs class.
		/// </summary>
		/// <param name="dataRow">The DataRow used to update the data source. </param>
		/// <param name="command">The IDbCommand executed during the Update. </param>
		/// <param name="statementType">Whether the command is an UPDATE, INSERT, DELETE, or SELECT statement. </param>
		/// <param name="tableMapping">A DataTableMapping object. </param>
		/// <returns>A new instance of the RowUpdatingEventArgs class.</returns>
		protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new VistaDBRowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
		}
		/// <summary>
		/// Raises the RowUpdating event.
		/// </summary>
		/// <param name="value">RowUpdatingEventArgs</param>
		protected override void OnRowUpdating(RowUpdatingEventArgs value)
		{
			if(RowUpdating != null)
				RowUpdating(this, (VistaDBRowUpdatingEventArgs)value);
		}
		/// <summary>
		/// Raises the RowUpdated event.
		/// </summary>
		/// <param name="value">RowUpdatedEventArgs</param>
		protected override void OnRowUpdated(RowUpdatedEventArgs value)
		{
			if(RowUpdated != null)
				RowUpdated(this, (VistaDBRowUpdatedEventArgs)value);
		}
		/// <summary>
		/// Creates a new DataTableMappingCollection.
		/// </summary>
		/// <returns></returns>
		protected override DataTableMappingCollection CreateTableMappings()
		{
			DataTableMappingCollection collection1;
		
			collection1 = base.CreateTableMappings();

			return collection1; 
		}

		/// <summary>
		/// Overloaded. Releases the resources used by the component.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			selectCommand = null;
			insertCommand = null;
			updateCommand = null;
			deleteCommand = null;
		}

		/// <summary>
		/// 
		/// </summary>
		protected override int Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable, IDbCommand command, CommandBehavior behavior)
		{
			DataTable dataTable = dataSet.Tables[srcTable];
			if(dataTable != null)
				dataSet.Tables[srcTable].Clear();
			return base.Fill(dataSet, startRecord, maxRecords, srcTable, command, behavior);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override int Fill(DataSet dataSet, string srcTable, IDataReader dataReader, int startRecord, int maxRecords)
		{
			DataTable dataTable = dataSet.Tables[srcTable];
			if(dataTable != null)
				dataSet.Tables[srcTable].Clear();
			return base.Fill(dataSet, srcTable, dataReader, startRecord, maxRecords);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override int Fill(DataTable dataTable, IDataReader dataReader)
		{
			if(dataTable != null)
				dataTable.Clear();
			return base.Fill(dataTable, dataReader);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override int Fill(DataTable dataTable, IDbCommand command, CommandBehavior behavior)
		{
			if(dataTable != null)
				dataTable.Clear();
			return base.Fill(dataTable, command, behavior);
		}

		/// <summary>
		/// Initializes a new instance of the DataTableMappingCollection class. This new instance is empty
		/// and does not yet contain any DataTableMapping objects.
		/// </summary>
		[Browsable(true), Editor("VistaDB.Designer.VistaDBTableMappingsEditor, VistaDB.Designer.VS2003", "System.Drawing.Design.UITypeEditor")]
		public new DataTableMappingCollection TableMappings
		{
			get
			{
				return base.TableMappings;
			}
		}

	}

	/// <summary>
	/// Represents the method that will handle the RowUpdating event of a VistaDBDataAdapter.
	/// </summary>
	public delegate void VistaDBRowUpdatingEventHandler(object sender, VistaDBRowUpdatingEventArgs e);

	/// <summary>
	/// Represents the method that will handle the RowUpdated event of a VistaDBDataAdapter.
	/// </summary>
	public delegate void VistaDBRowUpdatedEventHandler(object sender, VistaDBRowUpdatedEventArgs e);

	/// <summary>
	/// Manages RowUpdating events of the VistaDB ADO.NET Data Provider.
	/// </summary>
	public class VistaDBRowUpdatingEventArgs: RowUpdatingEventArgs
	{

		/// <summary>
		/// Provides the data for the RowUpdating event of the VistaDB ADO.NET Data Provider.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="command"></param>
		/// <param name="statementType"></param>
		/// <param name="tableMapping"></param>
		public VistaDBRowUpdatingEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping):
			base(row, command, statementType, tableMapping)
		{			
		}
			
		/// <summary>
		/// Gets or sets a new instance of the VistaDBCommand class.
		/// </summary>
		public new VistaDBCommand Command
		{
			get
			{
				return (VistaDBCommand)(base.Command);
			}
			set
			{
				base.Command = value;
			}
		}
	}
	/// <summary>
	/// Provides data for the RowUpdated event of the VistaDB ADO.NET Data Provider.
	/// </summary>
	public class VistaDBRowUpdatedEventArgs: RowUpdatedEventArgs
	{
		/// <summary>
		/// Provides the data for the RowUpdating event of the VistaDB ADO.NET Data Provider.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="command"></param>
		/// <param name="statementType"></param>
		/// <param name="tableMapping"></param>
		public VistaDBRowUpdatedEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping):
			base(row, command, statementType, tableMapping)
		{			
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public new VistaDBCommand Command
		{
			get
			{
				return (VistaDBCommand)(base.Command);
			}
		}
	}
}
#endif
