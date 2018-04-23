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
    public class DepartmentBL : BaseBLL
    {
        public IList<DepartmentModel> DepartmentGetAll()
        {
            var departments = new List<DepartmentModel>();
            
            var dal = new DepartmentDAL(GetConnectionString());
            try
            {

                departments = dal.UserDeptAll()
                    .Select(p => 
                        new DepartmentModel
                            {
                                DepartmentId = p.nDepartmentIdn,
                                Name = p.sName,
                                Department = p.sDepartment
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

            return departments;
        }

        public IList<DepartmentModel> DepartmentGetByName(string deptName)
        {
            var departments = new List<DepartmentModel>();


            var dal = new DepartmentDAL(GetConnectionString());
            try
            {

                departments = dal.UserDeptByDeptName(deptName)
                    .Select(p =>
                        new DepartmentModel
                        {
                            DepartmentId = p.nDepartmentIdn,
                            Name = p.sName,
                            Department = p.sDepartment
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

            return departments;

        }
    }
}
