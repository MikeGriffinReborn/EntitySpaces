using System;
using System.Data;

namespace Provider.VistaDB
{
	/// <summary>
	/// Represents a VistaDB transaction to be made in a VistaDB database. This class cannot be inherited.
	/// </summary>
	public sealed class VistaDBTransaction: IDbTransaction
	{

		internal VistaDBConnection vistaDBConnection;

		/// <summary>
		/// Gets the transaction isolation level. VistaDB always uses snapshot isolation.
		/// </summary>
		public IsolationLevel IsolationLevel
		{
			get
			{
				return IsolationLevel.Unspecified;
			}
		}

		IDbConnection IDbTransaction.Connection
		{
			get
			{
				return vistaDBConnection;
			}
		}
	

		void IDisposable.Dispose()
		{
			vistaDBConnection = null;
		}

		/// <summary>
		/// Commit the current transaction level.
		/// </summary>
		public void Commit()//IDbTransaction.Commit
		{
			if(vistaDBConnection.State != ConnectionState.Open)
				throw new VistaDBException(VistaDBErrorCodes.ConnectionInvalid);
			vistaDBConnection.VistaDBSQL.CommitTransaction();
		}

		/// <summary>
		/// Rollback all changes and discard the transaction.
		/// </summary>
		public void Rollback()//IDbTransaction.Rollback
		{
			if(vistaDBConnection.State != ConnectionState.Open)
				throw new VistaDBException(VistaDBErrorCodes.ConnectionInvalid);
			vistaDBConnection.VistaDBSQL.RollbackTransaction();
		}

		/// <summary>
		/// Gets the current connection object.
		/// </summary>
		public VistaDBConnection Connection
		{
			get
			{
				return vistaDBConnection;
			}
		}

		internal VistaDBTransaction(VistaDBConnection connection)
		{
			vistaDBConnection = connection;
			vistaDBConnection.VistaDBSQL.BeginTransaction();
		}

	}
}
