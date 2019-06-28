/*  New BSD License
-------------------------------------------------------------------------------
Copyright (c) 2006-2012, EntitySpaces, LLC
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the EntitySpaces, LLC nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL EntitySpaces, LLC BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
-------------------------------------------------------------------------------
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace EntitySpaces.Interfaces
{
    /// <summary>
    /// This is the EntitySpaces ADO.NET connection based transaction class that mimics the System.Transactions.TransactionScope class.
    /// </summary>
    /// <remarks>
    /// EntitySpaces supports two transactions models, connection based via this class (esTransactionScope) and the 
    /// new System.Transactions.TransactionScope. Some databases such as Microsoft Access don't support the new System.Transactions.TransactionScope
    /// class. And still there are other cases where the System.Transactions.TransactionScope class cannot be used as is the case with a lot
    /// of hosting companies. Thus EntitySpaces provides a very nice ADO.NET connection based transaction handler.
    /// 
    /// The Syntax should be as follows:
    /// <code>
    /// using (esTransactionScope scope = new esTransactionScope())
    /// {
    ///	    // Logic here ...
    /// 
    ///     scope.Complete(); // last line of using statement
    /// }
    /// </code>
    /// Note that if an exception is thrown scope.Complete will not be called, and the transaction upon leaving the using statement
    /// will be rolled back. You indicate whether you want the provider to use the esTransactionScope or the System.Transactions.TransactionScope
    /// class in your .config file. Notice in the config file setting below that providerClass="DataProvider". This indicates that you want to use
    /// the esTransactionScope class, use providerClass="DataProviderEnterprise" to use System.Transactions.TransactionScope.
    /// <code>
    ///    &lt;add name="SQL" 
    ///       providerMetadataKey="esDefault" 
    ///       sqlAccessType="DynamicSQL" 
    ///       provider="EntitySpaces.SqlClientProvider" 
    ///       providerClass="DataProvider" 
    ///       connectionString="User ID=sa;Password=griffinski;Initial Catalog=Northwind;Data Source=localhost" 
    ///       databaseVersion="2005"/&gt;
    /// </code>
    /// 
    /// 
    /// </remarks>
    public class esTransactionScope : IDisposable
    {
        /// <summary>
        /// The default constructor, this transactions <see cref="esTransactionScopeOption"/> will be
        /// set to Required. The IsolationLevel is set to Unspecified.
        /// <code>
        /// using (esTransactionScope scope = new esTransactionScope())
        /// {
        ///		// Do your work here
        ///		scope.Complete();
        /// }
        /// </code>
        /// </summary>
         public esTransactionScope()
        {
            this.option = esTransactionScopeOption.Required;
            this.level  = esTransactionScope.IsolationLevel;

            CommonInit(this);

            this.root.count++;
        }

        /// <summary>
        /// Use this constructor to control the esTransactionScopeOption as it applies
        /// to this transaction.
        /// <code>
        /// using (esTransactionScope scope = new esTransactionScope(esTransactionScopeOption.RequiresNew))
        /// {
        ///		// Do your work here
        ///		scope.Complete();
        /// }
        /// </code>
        /// </summary>
        /// <param name="option">See <see cref="esTransactionScopeOption"/></param>
        public esTransactionScope(esTransactionScopeOption option)
        {
            if (option == esTransactionScopeOption.None) throw new ArgumentException("'None' cannot be passed");

            this.option = option;
            this.level  = esTransactionScope.IsolationLevel;

            CommonInit(this);

            this.root.count++;
        }

        /// <summary>
        /// Use this constructor to control the esTransactionScopeOption as it applies
        /// to this transaction.
        /// <code>
        /// using (esTransactionScope scope = new 
        ///   esTransactionScope(esTransactionScopeOption.RequiresNew, IsolationLevel.ReadCommitted))
        /// {
        ///		// Do your work here
        ///		scope.Complete();
        /// }
        /// </code>
        /// </summary>
        /// <param name="option">See <see cref="esTransactionScopeOption"/></param>
        /// <param name="level">See IsolationLevel in the System.Data namespace</param>
        public esTransactionScope(esTransactionScopeOption option, IsolationLevel level)
        {
            this.option = option;
            this.level  = level;

            CommonInit(this);

            this.root.count++;
        }

        /// <summary>
        /// You must call Complete to commit the transaction. Calls and transactions can be nested, only the final outer call to
        /// Complete will commit the transaction.
        /// </summary>
        public void Complete()
        {
            this.root.count--;

            if (this.root == this && this.root.count == 0 && this.option != esTransactionScopeOption.Suppress)
            {
                foreach (Transaction tx in this.root.transactions.Values)
                {
                    IDbConnection cn = tx.sqlTx.Connection;

                    tx.sqlTx.Commit();

                    tx.sqlTx.Dispose();
                    tx.sqlTx = null;

                    if (cn != null && cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }

                    tx.sqlCn = null;
                }

                this.root.transactions.Clear();

                if (this.commitList != null)
                {
                    foreach (ICommittable commit in this.commitList)
                    {
                        commit.Commit();
                    }

                    commitList.Clear();
                }
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Internal. Called when the "using" statement is exited.
        /// </summary>
        void IDisposable.Dispose()
        {
            try
            {
                if (this.root == this && this.count > 0)
                {
                    // Somebody didn't call Complete, we must roll back ...
                    if (this.root.transactions.Count > 0)
                    {
                        Rollback();
                        return;
                    }
                }
            }
            finally
            {
                if (this.commitList != null)
                {
                    commitList.Clear();
                    this.commitList = null;
                }

                Stack<esTransactionScope> stack = (Stack<esTransactionScope>)Thread.GetData(txSlot);
                stack.Pop();
            }
        }

        #endregion


        /// <summary>
        /// Internal method, called if the "using" statement is left without calling scope.Complete()
        /// </summary>
        private void Rollback()
        {
            if (false == this.root.hasRolledBack && this.root.count > 0)
            {
                this.root.hasRolledBack = true;

                foreach (Transaction tx in this.root.transactions.Values)
                {
                    IDbConnection cn = tx.sqlTx.Connection;

                    try
                    {
                        // It may look as though we are eating an exception here
                        // but this method is private and only called when we 
                        // have already received an error, we don't want our cleanup
                        // code to cloud the issue.
                        cn = tx.sqlTx.Connection;

                        tx.sqlTx.Rollback();
                        tx.sqlTx.Dispose();
                    }
                    catch { }

                    tx.sqlTx = null;
                    tx.sqlCn = null;

                    if (cn != null && cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }

                this.root.transactions.Clear();
                this.root.count = 0;
            }
        }

        // We might have multple transactions going at the same time.
        // There's one per connnection string
        private class Transaction
        {
            public IDbTransaction sqlTx = null;
            public IDbConnection sqlCn = null;
        }


        private Dictionary<string, Transaction> transactions;
        private List<ICommittable> commitList;
        private bool hasRolledBack;
        private int count;
        private esTransactionScope root;
        private esTransactionScopeOption option;
        private IsolationLevel level;

        #region "static"

        /// <summary>
        /// EntitySpaces providers register this callback so that the esTransactionScope class can ask it to create the proper
        /// type of connection, ie, SqlConnection, OracleConnection, OleDbConnection and so on ...
        /// </summary>
        /// <returns></returns>
        public delegate IDbConnection CreateIDbConnectionDelegate();

        /// <summary>
        /// This can be used to get the esTransactionScopeOption from the current esTransactionScope (remember transactions can be nested).
        /// If there is no on-going transaction then esTransactionScopeOption.None is returned.
        /// </summary>
        /// <returns></returns>
        static public esTransactionScopeOption GetCurrentTransactionScopeOption()
        {
            esTransactionScope currentTx = GetCurrentTx();

            if (currentTx == null) 
                return esTransactionScopeOption.None;
            else
                return currentTx.option;
        }
        
        /// <summary>
        /// You should never call this directly, the providers call this method.
        /// </summary>
        /// <param name="cmd">The command to enlist into a transaction</param>
        /// <param name="connectionString">The connection string passed to the CreateIDbConnectionDelegate delegate</param>
        /// <param name="creator">The delegate previously registered by the provider</param>
        static public void Enlist(IDbCommand cmd, string connectionString, CreateIDbConnectionDelegate creator)
        {
            esTransactionScope currentTx = GetCurrentTx();

            if (currentTx == null || currentTx.option == esTransactionScopeOption.Suppress)
            {
                cmd.Connection = creator();
                cmd.Connection.ConnectionString = connectionString;
                cmd.Connection.Open();
            }
            else
            {
                Transaction tx = null;

                if (currentTx.root.transactions.ContainsKey(connectionString))
                {
                    tx = currentTx.root.transactions[connectionString] as Transaction;
                }
                else
                {
                    tx = new Transaction();

                    IDbConnection cn = creator();
                    cn.ConnectionString = connectionString;
                    cn.Open();

                    // The .NET framework has a bug in that the IDbTransaction only maintains
                    // a weak reference to the Connection, thus, we put a strong reference 
                    // on it.
                    tx.sqlCn = cn;

                    if (_isolationLevel != IsolationLevel.Unspecified)
                    {
                        tx.sqlTx = cn.BeginTransaction(_isolationLevel);
                    }
                    else
                    {
                        tx.sqlTx = cn.BeginTransaction();
                    }

                    currentTx.root.transactions[connectionString] = tx;
                }
      
                cmd.Connection  = tx.sqlTx.Connection;
                cmd.Transaction = tx.sqlTx;
            }
        }

        /// <summary>
        /// You should never call this directly, the providers call this method.
        /// </summary>
        /// <param name="cmd">The command to enlist into a transaction</param>
        static public void DeEnlist(IDbCommand cmd)
        {
            esTransactionScope current = GetCurrentTx();
            if (current == null || current.option == esTransactionScopeOption.Suppress)
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// You should never call this directly, EntitySpaces calls this internally.
        /// </summary>
        /// <param name="commit">Any class that implements ICommittable</param>
        /// <returns>True if successful</returns>
        static public bool AddForCommit(ICommittable commit)
        {
            esTransactionScope current = GetCurrentTx();
            if (current != null)
            {
                if (current.commitList == null)
                {
                    current.commitList = new List<ICommittable>();
                }

                current.commitList.Add(commit);

                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// This is the common constructor logic, tx is "this" from the constructor
        /// </summary>
        /// <param name="tx"></param>
        static protected void CommonInit(esTransactionScope tx)
        {
            Stack<esTransactionScope> stack;

            // See if our stack is already created (there is only one per thread)
            object obj = Thread.GetData(txSlot);
            if (obj == null)
            {
                stack = new Stack<esTransactionScope>();
                Thread.SetData(txSlot, stack);
            }
            else
            {
                stack = (Stack<esTransactionScope>)obj;
            }

            // If this transaction is required we need to set it's root
            if (tx.option == esTransactionScopeOption.Required)
            {
                foreach (esTransactionScope esTrans in stack)
                {
                    // The root can be either a Requires or RequiresNew, and a root always points to
                    // itself, therefore, as long as it's not a Suppress and it's pointing to itself
                    // then we know this the next root up on the stack
                    if (esTrans.option != esTransactionScopeOption.Suppress && esTrans == esTrans.root)
                    {
                        tx.root = esTrans;
                        break;
                    }
                }
            }

            // If we didn't find a root, then we are by definition the root
            if (tx.root == null)
            {
                tx.root = tx;
                tx.transactions = new Dictionary<string, Transaction>();
            }

            stack.Push(tx);
        }

        /// <summary>
        /// Internal method.
        /// </summary>
        /// <returns></returns>
        static private esTransactionScope GetCurrentTx()
        {
            esTransactionScope tx = null;

            object o = Thread.GetData(txSlot);
            if(o != null)
            {
                Stack<esTransactionScope> stack = o as Stack<esTransactionScope>;
                if (stack.Count > 0)
                {
                    tx = stack.Peek();
                }
            }

            return tx;
        }


        /// <summary>
        /// This is the Transaction's strength. The default is "IsolationLevel.Unspecified, the strongest is "IsolationLevel.Serializable" which is what
        /// is recommended for serious enterprize level projects.
        /// </summary>
        public static IsolationLevel IsolationLevel
        {
            get
            {
                return _isolationLevel;
            }

            set
            {
                _isolationLevel = value;
            }
        }

        private static IsolationLevel _isolationLevel = IsolationLevel.Unspecified;
        private static LocalDataStoreSlot txSlot = Thread.AllocateDataSlot();
        #endregion
    }
}
