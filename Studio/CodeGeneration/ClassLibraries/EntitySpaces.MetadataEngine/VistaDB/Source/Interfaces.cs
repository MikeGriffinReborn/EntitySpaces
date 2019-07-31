using System;

namespace Provider.VistaDB
{
	/// <summary>
	/// Summary description for IVistaDBDataSet.
	/// </summary>
	internal interface IVistaDBDataSet
	{
		void OpenAfterInit();
	}

	/// <summary>
	/// This interface gives some service functions to user
	/// </summary>
	public interface IVistaDBRemoteService
	{
		/// <summary>
		/// Returns VistaDBMetaDataReader object, whic allows to get database meta data
		/// </summary>
		/// <returns>VistaDBMetaDataReader object</returns>
		VistaDBMetaDataReader EnumTables();

		/// <summary>
		/// Returns active user list (excluding adminstrative user connection)
		/// </summary>
		/// <returns>String array of active users</returns>
		string[] GetActiveUserList();

		/// <summary>
		/// Returns alias list
		/// </summary>
		/// <returns>String array of aliases</returns>
		string[] GetAliasList();

		/// <summary>
		/// Returns content of server log file
		/// </summary>
		/// <returns>Content of server log file</returns>
		string GetLogFile();

		/// <summary>
		/// Returns a value indicating whether current connect is aministrative
		/// </summary>
		/// <returns>True if current connection is administrative</returns>
		bool IsAdminConnection();

		/// <summary>
		/// Force updating of alias list on the server
		/// </summary>
		void UpdateAliasList();

		/// <summary>
		/// Force updating user list on the server
		/// </summary>
		void UpdateUserList();
	}
}
