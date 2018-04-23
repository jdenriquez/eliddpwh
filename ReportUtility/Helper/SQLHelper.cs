using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReportUtility.Helper
{
    public sealed class SQLHelper
    {
        #region private utility methods & constructors

        private SQLHelper() { }

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("SqlCommand is not available.");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
            {
                return;
            }

            int i = 0;

            foreach (SqlParameter commandParameter in commandParameters)
            {
                if (commandParameter.ParameterName == null ||
                    commandParameter.ParameterName.Length <= 1)
                    throw new Exception(
                        string.Format(
                            "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                            i, commandParameter.ParameterName));
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i++;
            }
        }

        private static void AssignParameterValues(SqlParameter[] commandParameters, dynamic[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                return;
            }

            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        private static void PrepareCommand(SqlCommand command, SqlConnection conn, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("SqlCommand is not available.");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("CommandText is null or empty. ");

            if (conn.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                conn.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            command.Connection = conn;

            command.CommandText = commandText;

            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            command.CommandType = commandType;

            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        #endregion private utility methods & constructors

        #region ExecuteNonQuery

        public static void ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        public static void ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                ExecuteNonQuery(conn, commandType, commandText, commandParameters);
            }
        }

        public static void ExecuteNonQuery(string connectionString, string spName, params dynamic[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(connectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        public static void ExecuteNonQuery(SqlConnection conn, CommandType commandType, string commandText)
        {
            ExecuteNonQuery(conn, commandType, commandText, (SqlParameter[])null);
        }

        public static void ExecuteNonQuery(SqlConnection conn, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");

            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, conn, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            int retval = cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();
            if (mustCloseConnection)
                conn.Close();
        }

        public static void ExecuteNonQuery(SqlConnection conn, string spName, params dynamic[] parameterValues)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, parameterValues);

                ExecuteNonQuery(conn, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                ExecuteNonQuery(conn, CommandType.StoredProcedure, spName);
            }
        }

        public static void ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
        }

        public static void ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            int retval = cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();
        }

        public static void ExecuteNonQuery(SqlTransaction transaction, string spName, params dynamic[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteNonQuery

        #region ExecuteDataset

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                return ExecuteDataset(conn, commandType, commandText, commandParameters);
            }
        }

        public static DataSet ExecuteDataset(string connectionString, string spName, params dynamic[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(connectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        public static DataSet ExecuteDataset(SqlConnection conn, CommandType commandType, string commandText)
        {
            return ExecuteDataset(conn, commandType, commandText, (SqlParameter[])null);
        }

        public static DataSet ExecuteDataset(SqlConnection conn, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");

            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, conn, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                da.Fill(ds);

                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    conn.Close();

                return ds;
            }
        }

        public static DataSet ExecuteDataset(SqlConnection conn, string spName, params dynamic[] parameterValues)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteDataset(conn, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(conn, CommandType.StoredProcedure, spName);
            }
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataset(transaction, commandType, commandText, (SqlParameter[])null);
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                da.Fill(ds);

                cmd.Parameters.Clear();

                return ds;
            }
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params dynamic[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteDataset

        #region ExecuteReader

        private enum SqlConnectionOwnership
        {
            Internal,
            External
        }

        private static SqlDataReader ExecuteReader(SqlConnection conn, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");

            bool mustCloseConnection = false;
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

                SqlDataReader dataReader;

                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

                bool canClear = true;
                foreach (SqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear)
                {
                    cmd.Parameters.Clear();
                }

                return dataReader;
            }
            catch
            {
                if (mustCloseConnection)
                    conn.Close();
                throw;
            }
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteReader(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                return ExecuteReader(conn, null, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
            }
            catch
            {
                if (conn != null) conn.Close();
                throw;
            }

        }

        public static SqlDataReader ExecuteReader(string connectionString, string spName, params dynamic[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(connectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        public static SqlDataReader ExecuteReader(SqlConnection conn, CommandType commandType, string commandText)
        {
            return ExecuteReader(conn, commandType, commandText, (SqlParameter[])null);
        }

        public static SqlDataReader ExecuteReader(SqlConnection conn, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(conn, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        public static SqlDataReader ExecuteReader(SqlConnection conn, string spName, params dynamic[] parameterValues)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(conn, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(conn, CommandType.StoredProcedure, spName);
            }
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteReader(transaction, commandType, commandText, (SqlParameter[])null);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params dynamic[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteReader

        #region ExecuteScalar

        public static dynamic ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        public static dynamic ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                return ExecuteScalar(conn, commandType, commandText, commandParameters);
            }
        }

        public static dynamic ExecuteScalar(string connectionString, string spName, params dynamic[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(connectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        public static dynamic ExecuteScalar(SqlConnection conn, CommandType commandType, string commandText)
        {
            return ExecuteScalar(conn, commandType, commandText, (SqlParameter[])null);
        }

        public static dynamic ExecuteScalar(SqlConnection conn, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");

            SqlCommand cmd = new SqlCommand();

            bool mustCloseConnection = false;
            PrepareCommand(cmd, conn, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            dynamic retval = cmd.ExecuteScalar();

            cmd.Parameters.Clear();

            if (mustCloseConnection)
                conn.Close();

            return retval;
        }

        public static dynamic ExecuteScalar(SqlConnection conn, string spName, params dynamic[] parameterValues)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteScalar(conn, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteScalar(conn, CommandType.StoredProcedure, spName);
            }
        }

        public static dynamic ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteScalar(transaction, commandType, commandText, (SqlParameter[])null);
        }

        public static dynamic ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            dynamic retval = cmd.ExecuteScalar();

            cmd.Parameters.Clear();
            return retval;
        }

        public static dynamic ExecuteScalar(SqlTransaction transaction, string spName, params dynamic[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteScalar

        #region ExecuteXmlReader

        public static XmlReader ExecuteXmlReader(SqlConnection conn, CommandType commandType, string commandText)
        {
            return ExecuteXmlReader(conn, commandType, commandText, (SqlParameter[])null);
        }

        public static XmlReader ExecuteXmlReader(SqlConnection conn, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");

            bool mustCloseConnection = false;
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

                XmlReader retval = cmd.ExecuteXmlReader();

                cmd.Parameters.Clear();

                return retval;
            }
            catch
            {
                if (mustCloseConnection)
                    conn.Close();
                throw;
            }
        }

        public static XmlReader ExecuteXmlReader(SqlConnection conn, string spName, params dynamic[] parameterValues)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteXmlReader(conn, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteXmlReader(conn, CommandType.StoredProcedure, spName);
            }
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteXmlReader(transaction, commandType, commandText, (SqlParameter[])null);
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            XmlReader retval = cmd.ExecuteXmlReader();

            cmd.Parameters.Clear();
            return retval;
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params dynamic[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteXmlReader

        #region FillDataset
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (dataSet == null) throw new ArgumentNullException("Dataset is not available.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                FillDataset(conn, commandType, commandText, dataSet, tableNames);
            }
        }

        public static void FillDataset(string connectionString, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (dataSet == null) throw new ArgumentNullException("Dataset is not available.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                FillDataset(conn, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }

        public static void FillDataset(string connectionString, string spName,
            DataSet dataSet, string[] tableNames,
            params dynamic[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (dataSet == null) throw new ArgumentNullException("Dataset is not available.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                FillDataset(conn, spName, dataSet, tableNames, parameterValues);
            }
        }

        public static void FillDataset(SqlConnection conn, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(conn, commandType, commandText, dataSet, tableNames, null);
        }

        public static void FillDataset(SqlConnection conn, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(conn, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        public static void FillDataset(SqlConnection conn, string spName,
            DataSet dataSet, string[] tableNames,
            params dynamic[] parameterValues)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (dataSet == null) throw new ArgumentNullException("Dataset is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, parameterValues);

                FillDataset(conn, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else
            {
                FillDataset(conn, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        public static void FillDataset(SqlTransaction transaction, CommandType commandType,
            string commandText,
            DataSet dataSet, string[] tableNames)
        {
            FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
        }

        public static void FillDataset(SqlTransaction transaction, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        public static void FillDataset(SqlTransaction transaction, string spName,
            DataSet dataSet, string[] tableNames,
            params dynamic[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (dataSet == null) throw new ArgumentNullException("Dataset is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else
            {
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        private static void FillDataset(SqlConnection conn, SqlTransaction transaction, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (dataSet == null) throw new ArgumentNullException("Dataset is not available.");

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, conn, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {

                if (tableNames != null && tableNames.Length > 0)
                {
                    string tableName = "Table";
                    for (int index = 0; index < tableNames.Length; index++)
                    {
                        if (tableNames[index] == null || tableNames[index].Length == 0) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName += (index + 1).ToString();
                    }
                }

                dataAdapter.Fill(dataSet);

                command.Parameters.Clear();
            }

            if (mustCloseConnection)
                conn.Close();
        }
        #endregion

        #region UpdateDataset

        public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
        {
            if (insertCommand == null) throw new ArgumentNullException("insertCommand");
            if (deleteCommand == null) throw new ArgumentNullException("deleteCommand");
            if (updateCommand == null) throw new ArgumentNullException("updateCommand");
            if (tableName == null || tableName.Length == 0) throw new ArgumentNullException("Table name is null or empty.");

            using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
            {
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;

                dataAdapter.Update(dataSet, tableName);

                dataSet.AcceptChanges();
            }
        }
        #endregion

        #region CreateCommand

        public static SqlCommand CreateCommand(SqlConnection conn, string spName, params string[] sourceColumns)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            SqlCommand cmd = new SqlCommand(spName, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            if ((sourceColumns != null) && (sourceColumns.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                for (int index = 0; index < sourceColumns.Length; index++)
                    commandParameters[index].SourceColumn = sourceColumns[index];

                AttachParameters(cmd, commandParameters);
            }

            return cmd;
        }
        #endregion

        #region ExecuteNonQueryTypedParams

        public static void ExecuteNonQueryTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(connectionString, spName);

                AssignParameterValues(commandParameters, dataRow);

                SQLHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                SQLHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        public static void ExecuteNonQueryTypedParams(SqlConnection conn, String spName, DataRow dataRow)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, dataRow);

                SQLHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                SQLHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, spName);
            }
        }

        public static void ExecuteNonQueryTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, dataRow);

                SQLHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                SQLHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region ExecuteDatasetTypedParams

        public static DataSet ExecuteDatasetTypedParams(string connectionString, String spName, DataRow dataRow)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(connectionString, spName);

                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        public static DataSet ExecuteDatasetTypedParams(SqlConnection conn, String spName, DataRow dataRow)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteDataset(conn, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteDataset(conn, CommandType.StoredProcedure, spName);
            }
        }

        public static DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion

        #region ExecuteReaderTypedParams

        public static SqlDataReader ExecuteReaderTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(connectionString, spName);

                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        public static SqlDataReader ExecuteReaderTypedParams(SqlConnection conn, String spName, DataRow dataRow)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteReader(conn, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteReader(conn, CommandType.StoredProcedure, spName);
            }
        }

        public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region ExecuteScalarTypedParams

        public static dynamic ExecuteScalarTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(connectionString, spName);

                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        public static dynamic ExecuteScalarTypedParams(SqlConnection conn, String spName, DataRow dataRow)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteScalar(conn, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteScalar(conn, CommandType.StoredProcedure, spName);
            }
        }

        public static dynamic ExecuteScalarTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region ExecuteXmlReaderTypedParams

        public static XmlReader ExecuteXmlReaderTypedParams(SqlConnection conn, String spName, DataRow dataRow)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {

                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);


                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteXmlReader(conn, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteXmlReader(conn, CommandType.StoredProcedure, spName);
            }
        }


        public static XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {

                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);


                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region ExecuteDatatable

        public static DataTable ExecuteDatatable(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDatatable(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        public static DataTable ExecuteDatatable(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                return ExecuteDatatable(conn, commandType, commandText, commandParameters);
            }
        }

        public static DataTable ExecuteDatatable(string connectionString, string spName, params dynamic[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(connectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteDatatable(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDatatable(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        public static DataTable ExecuteDatatable(SqlConnection conn, CommandType commandType, string commandText)
        {
            return ExecuteDatatable(conn, commandType, commandText, (SqlParameter[])null);
        }

        public static DataTable ExecuteDatatable(SqlConnection conn, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");

            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, conn, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();

                da.Fill(dt);

                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    conn.Close();

                return dt;
            }
        }

        public static DataTable ExecuteDatatable(SqlConnection conn, string spName, params dynamic[] parameterValues)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteDatatable(conn, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDatatable(conn, CommandType.StoredProcedure, spName);
            }
        }

        public static DataTable ExecuteDatatable(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDatatable(transaction, commandType, commandText, (SqlParameter[])null);
        }

        public static DataTable ExecuteDatatable(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();

                da.Fill(dt);

                cmd.Parameters.Clear();

                return dt;
            }
        }

        public static DataTable ExecuteDatatable(SqlTransaction transaction, string spName, params dynamic[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteDatatable(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDatatable(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ExecuteDatatable

        #region FillDataTable
        public static void DataTable(string connectionString, CommandType commandType, string commandText, DataTable dataTable, string[] tableNames)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (dataTable == null) throw new ArgumentNullException("Dataset is not available.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                DataTable(conn, commandType, commandText, dataTable, tableNames);
            }
        }

        public static void DataTable(string connectionString, CommandType commandType,
            string commandText, DataTable dataTable, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (dataTable == null) throw new ArgumentNullException("Dataset is not available.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                DataTable(conn, commandType, commandText, dataTable, tableNames, commandParameters);
            }
        }

        public static void DataTable(string connectionString, string spName,
            DataTable dataTable, string[] tableNames,
            params dynamic[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (dataTable == null) throw new ArgumentNullException("Dataset is not available.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                DataTable(conn, spName, dataTable, tableNames, parameterValues);
            }
        }

        public static void DataTable(SqlConnection conn, CommandType commandType,
            string commandText, DataTable dataTable, string[] tableNames)
        {
            DataTable(conn, commandType, commandText, dataTable, tableNames, null);
        }

        public static void DataTable(SqlConnection conn, CommandType commandType,
            string commandText, DataTable dataTable, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            DataTable(conn, null, commandType, commandText, dataTable, tableNames, commandParameters);
        }

        public static void DataTable(SqlConnection conn, string spName,
            DataTable dataTable, string[] tableNames,
            params dynamic[] parameterValues)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (dataTable == null) throw new ArgumentNullException("Dataset is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, parameterValues);

                DataTable(conn, CommandType.StoredProcedure, spName, dataTable, tableNames, commandParameters);
            }
            else
            {
                DataTable(conn, CommandType.StoredProcedure, spName, dataTable, tableNames);
            }
        }

        public static void DataTable(SqlTransaction transaction, CommandType commandType,
            string commandText,
            DataTable dataTable, string[] tableNames)
        {
            DataTable(transaction, commandType, commandText, dataTable, tableNames, null);
        }

        public static void DataTable(SqlTransaction transaction, CommandType commandType,
            string commandText, DataTable dataTable, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            DataTable(transaction.Connection, transaction, commandType, commandText, dataTable, tableNames, commandParameters);
        }

        public static void DataTable(SqlTransaction transaction, string spName,
            DataTable dataTable, string[] tableNames,
            params dynamic[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (dataTable == null) throw new ArgumentNullException("Dataset is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                DataTable(transaction, CommandType.StoredProcedure, spName, dataTable, tableNames, commandParameters);
            }
            else
            {
                DataTable(transaction, CommandType.StoredProcedure, spName, dataTable, tableNames);
            }
        }

        private static void DataTable(SqlConnection conn, SqlTransaction transaction, CommandType commandType,
            string commandText, DataTable dataTable, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (dataTable == null) throw new ArgumentNullException("Dataset is not available.");

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, conn, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {

                if (tableNames != null && tableNames.Length > 0)
                {
                    string tableName = "Table";
                    for (int index = 0; index < tableNames.Length; index++)
                    {
                        if (tableNames[index] == null || tableNames[index].Length == 0) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName += (index + 1).ToString();
                    }
                }

                dataAdapter.Fill(dataTable);

                command.Parameters.Clear();
            }

            if (mustCloseConnection)
                conn.Close();
        }
        #endregion

        #region ExecuteDatatableTypedParams

        public static DataTable ExecuteDatatableTypedParams(string connectionString, String spName, DataRow dataRow)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(connectionString, spName);

                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteDatatable(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteDatatable(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        public static DataTable ExecuteDatatableTypedParams(SqlConnection conn, String spName, DataRow dataRow)
        {
            if (conn == null) throw new ArgumentNullException("MsSql Server Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(conn, spName);

                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteDatatable(conn, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteDatatable(conn, CommandType.StoredProcedure, spName);
            }
        }

        public static DataTable ExecuteDatatableTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if (transaction == null) throw new ArgumentNullException("The transaction was rollbacked or commited, please provide an open transaction.");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] commandParameters = SqlHelperParameter.GetSpParameterList(transaction.Connection, spName);

                AssignParameterValues(commandParameters, dataRow);

                return SQLHelper.ExecuteDatatable(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SQLHelper.ExecuteDatatable(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion

    }
}
