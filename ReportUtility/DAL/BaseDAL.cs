using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtility.DAL
{
    public class BaseDAL : IDisposable
    {
        private const string DEFAULT_CONNECTION_NAME = "DbConnection";
        private readonly ConnectionStringSettings _connectionStringSetting;
        private SqlConnection _sqlConnection;
        private SqlTransaction _sqlTransaction;
        private bool _withTransaction = false;
        private bool _isCommited = false;
        private bool _isRolledback = false;

        private bool _disposed = false;
        private IntPtr handle;
        protected string ConnectionString
        {
            get
            {
                string connectionString = _connectionStringSetting.ConnectionString;
                return connectionString;
            }
        }
        protected ConnectionStringSettings ConnectionStringSettings
        {
            get
            {
                ConnectionStringSettings connectionStringSetting = this._connectionStringSetting;
                return connectionStringSetting;
            }
        }
        public bool WithTransaction
        {
            get
            {
                return this._withTransaction;
            }
        }
        public dynamic Transaction
        {
            get
            {
                return this._sqlTransaction;
            }
        }

        public bool IsCommited
        {
            get
            {
                return this._isCommited;
            }
        }

        public bool IsRolledback
        {
            get
            {
                return this._isRolledback;
            }
        }

        public BaseDAL(string connectionString)
        {
            this._withTransaction = false;
            this._isCommited = false;
            this._isRolledback = false;
            this._connectionStringSetting = new ConnectionStringSettings("DbConnection", connectionString);
        }

        public BaseDAL(string connectionName, string connectionString)
        {
            this._withTransaction = false;
            this._isCommited = false;
            this._isRolledback = false;
            this._connectionStringSetting = new ConnectionStringSettings(connectionName, connectionString);
        }

        public BaseDAL(ConnectionStringSettings connectionStringSetting)
        {
            this._withTransaction = false;
            this._isCommited = false;
            this._isRolledback = false;
            this._connectionStringSetting = connectionStringSetting;
        }

        protected void InitializeTransaction()
        {
            bool connStateClosed = false;

            bool flag = this._sqlTransaction != null;
            if (!flag)
            {
                if (this._sqlConnection == null)
                {
                    connStateClosed = true;
                }
                else if (this._sqlConnection.State == ConnectionState.Closed)
                {
                    connStateClosed = true;
                }

                if (connStateClosed)
                {
                    this._sqlConnection = new SqlConnection(this.ConnectionString);
                    this._sqlConnection.Open();
                }

                this._sqlTransaction = this._sqlConnection.BeginTransaction();
                this._withTransaction = true;
            }
        }

        public void BeginTransaction()
        {
            InitializeTransaction();
        }

        public void BeginTransaction(string connectionString)
        {
            _connectionStringSetting.ConnectionString = connectionString;
            InitializeTransaction();
        }

        public virtual void CommitTransaction()
        {
            bool flagCommit;
            if (this._withTransaction == false || this._sqlTransaction == null || this._isCommited == true)
            {
                flagCommit = true;
            }
            else
            {
                flagCommit = this.IsRolledback;
            }

            if (flagCommit == false)
            {
                this._sqlTransaction.Commit();
                this._isCommited = true;
                this._isRolledback = false;
            }
        }

        public virtual void RollbackTransaction()
        {
            bool flagRollback;
            if (!this._withTransaction || this._sqlTransaction == null || this._isRolledback)
            {
                flagRollback = true;
            }
            else
            {
                flagRollback = this._isCommited;
            }

            if (!flagRollback)
            {
                this._sqlTransaction.Commit();
                this._isCommited = false;
                this._isRolledback = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

            if (!this._disposed)
            {

                if (disposing)
                {
                    // Dispose managed resources.
                    if (_sqlTransaction != null)
                    {
                        _sqlTransaction.Dispose();
                    }

                    if (_sqlConnection != null)
                    {
                        _sqlConnection.Dispose();
                    }
                }

                handle = IntPtr.Zero;

            }
            _disposed = true;
        }
        ~BaseDAL()
        {
            Dispose(false);
        }
    }
}