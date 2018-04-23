using ReportUtility.DataModel;
using ReportUtility.Helper;
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
    public class EmployeeDAL : BaseDAL
    {
        public EmployeeDAL(string connectionString) : base(connectionString)
        {
        }

        public EmployeeDAL(string connectionName, string connectionString)
            : base(connectionName, connectionString)
        {

        }

        public EmployeeDAL(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {

        }

        private static TB_USER ConvertReaderToEntity(dynamic reader)
        {
            if (!reader.Read())
            {
                return null;
            }

            TB_USER item = new TB_USER();

            item.nUserIdn = int.Parse(reader["nUserIdn"].ToString());
            item.sUserName = reader["sUserName"].ToString();
            item.sUserID = reader["sUserID"].ToString();
            item.nDepartmentIdn = int.Parse(reader["nDepartmentIdn"].ToString());

            return item;
        }

        private static IList<TB_USER> ConvertReaderToList(dynamic reader)
        {
            List<TB_USER> listItem = new List<TB_USER>();
            TB_USER item = null;
            while (true)
            {
                item = ConvertReaderToEntity(reader);
                if (item == null)
                {
                    break;
                }
                listItem.Add(item);
            }

            return listItem;
        }

        public IList<TB_USER> UserAll()
        {
            string script = string.Empty;
            script = "SELECT * FROM TB_USER ORDER BY sUserName ASC ";
            using (SqlDataReader reader = SQLHelper.ExecuteReader(
                ConnectionString,
                CommandType.Text,
                script))
            {
                return ConvertReaderToList(reader);
            }
        }

        public IList<TB_USER> UserGetByName(string name)
        {
            string script = string.Empty;
            script = "SELECT * FROM TB_USER WHERE sUserName = '" + name + "' ORDER BY sUserName ASC ";
            using (SqlDataReader reader = SQLHelper.ExecuteReader(
                ConnectionString,
                CommandType.Text,
                script))
            {
                return ConvertReaderToList(reader);
            }
        }
    }
}
