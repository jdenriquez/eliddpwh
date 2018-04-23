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
    public class DepartmentDAL : BaseDAL
    {
        public DepartmentDAL(string connectionString) : base(connectionString)
        {
        }

        public DepartmentDAL(string connectionName, string connectionString)
            : base(connectionName, connectionString)
        {

        }

        public DepartmentDAL(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {

        }

        private static TB_USER_DEPT ConvertReaderToEntity(dynamic reader)
        {
            if (!reader.Read())
            {
                return null;
            }

            TB_USER_DEPT item = new TB_USER_DEPT();

            item.nDepartmentIdn = int.Parse(reader["nDepartmentIdn"].ToString());
            item.sName = reader["sName"].ToString();
            item.sDepartment = reader["sDepartment"].ToString();


            return item;
        }

        private static IList<TB_USER_DEPT> ConvertReaderToList(dynamic reader)
        {
            List<TB_USER_DEPT> listItem = new List<TB_USER_DEPT>();
            TB_USER_DEPT item = null;
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

        public IList<TB_USER_DEPT> UserDeptAll()
        {
            string script = string.Empty;
            script = "SELECT * FROM TB_USER_DEPT ORDER BY sName ASC ";
            using (SqlDataReader reader = SQLHelper.ExecuteReader(
                ConnectionString,
                CommandType.Text,
                script))
            {
                return ConvertReaderToList(reader);
            }
        }

        public IList<TB_USER_DEPT> UserDeptByDeptName(string deptName)
        {
            string script = string.Empty;
            script = "SELECT * FROM TB_USER_DEPT WHERE sName = '" + deptName + "' ORDER BY sName ASC ";
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
