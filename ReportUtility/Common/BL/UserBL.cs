using ReportUtility.Common.ModelView;
using ReportUtility.DAL;
using ReportUtility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtility.Common.BL
{
    public class UserBL : BaseBLL
    {
        public IList<EmployeeModel> DepartmentGetAll()
        {
            var users = new List<EmployeeModel>();
            var dal = new EmployeeDAL(GetConnectionString());
            try
            {
                users = dal.UserAll()
                    .Select(p =>
                        new EmployeeModel
                        {
                            UserId  = p.nUserIdn,
                            UserName= p.sUserName,
                            DepartmentId = p.nDepartmentIdn,
                            sUserID = p.sUserID
                        })
                    .ToList();
            }
            catch (Exception)
            {
                dal.RollbackTransaction();
            }
            finally
            {
                dal.Dispose();
            }
            return users;
        }

        public IList<EmployeeModel> UserGetByName(string deptName)
        {
            var users = new List<EmployeeModel>();
            var dal = new EmployeeDAL(GetConnectionString());
            try
            {
                users = dal.UserGetByName(deptName)
                    .Select(p =>
                        new EmployeeModel
                        {
                            UserId = p.nUserIdn,
                            UserName = p.sUserName,
                            DepartmentId = p.nDepartmentIdn,
                            sUserID = p.sUserID
                        })
                    .ToList();
            }
            catch (Exception)
            {
                dal.RollbackTransaction();
            }
            finally
            {
                dal.Dispose();
            }
            return users;
        }
    }
}
