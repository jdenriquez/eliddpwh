using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtility.Helper
{
    public sealed class SqlHelperParameter
    {
        private SqlHelperParameter() { }

        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        private static SqlParameter[] SearchSpParameterList(SqlConnection conn, string spName, bool includeReturnValueParam)
        {

            if (conn == null)
            {
                throw new ArgumentNullException("Database Connection is not available.");
            }

            if (spName == null || spName.Length == 0)
            {
                throw new ArgumentNullException("Stored Procedure name is null or empty.");
            }

            SqlCommand cmd = new SqlCommand(spName, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            conn.Close();

            if (!includeReturnValueParam)
            {
                cmd.Parameters.RemoveAt(0);
            }

            SqlParameter[] parameterList = new SqlParameter[cmd.Parameters.Count];

            cmd.Parameters.CopyTo(parameterList, 0);

            foreach (SqlParameter parameter in parameterList)
            {
                parameter.Value = DBNull.Value;
            }
            return parameterList;
        }

        private static SqlParameter[] CloneParameterList(SqlParameter[] origParameters)
        {
            SqlParameter[] cloneParameters = new SqlParameter[origParameters.Length];

            for (int i = 0; i < origParameters.Length; i++)
            {
                cloneParameters[i] = (SqlParameter)((ICloneable)origParameters[i]).Clone();
            }

            return cloneParameters;
        }

        public static void CacheParameterList(string connString, string commandText, params SqlParameter[] commandParameters)
        {
            if (connString == null || connString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("CommandText is null or empty");

            string hashKey = connString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }

        public static SqlParameter[] GetParameterList(string connString, string commandText)
        {
            if (connString == null || connString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("CommandText is null or empty");

            string hashKey = connString + ":" + commandText;

            SqlParameter[] cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameterList(cachedParameters);
            }
        }

        private static SqlParameter[] GetSpParameterListInternal(SqlConnection conn, string spName, bool includeReturnValueParameter)
        {
            if (conn == null) throw new ArgumentNullException("Database Connection is not available.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            string hashKey = conn.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            SqlParameter[] cachedParameters;

            cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                SqlParameter[] spParameters = SearchSpParameterList(conn, spName, includeReturnValueParameter);
                paramCache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }

            return CloneParameterList(cachedParameters);
        }

        public static SqlParameter[] GetSpParameterList(string connString, string spName)
        {
            return GetSpParameterList(connString, spName, false);
        }

        public static SqlParameter[] GetSpParameterList(string connString, string spName, bool includeReturnValueParameter)
        {
            if (connString == null || connString.Length == 0) throw new ArgumentNullException("ConnectionString is null or empty.");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("Stored Procedure name is null or empty.");

            using (SqlConnection connection = new SqlConnection(connString))
            {
                return GetSpParameterListInternal(connection, spName, includeReturnValueParameter);
            }
        }

        internal static SqlParameter[] GetSpParameterList(SqlConnection conn, string spName)
        {
            return GetSpParameterList(conn, spName, false);
        }

        internal static SqlParameter[] GetSpParameterList(SqlConnection conn, string spName, bool includeReturnValueParameter)
        {
            if (conn == null) throw new ArgumentNullException("Database Connection is not available.");
            using (SqlConnection clonedConnection = (SqlConnection)((ICloneable)conn).Clone())
            {
                return GetSpParameterListInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }
    }
}
