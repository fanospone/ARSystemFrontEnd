using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.Models;
using static ARSystem.Service.Constants;

namespace ARSystem.Service
{
    public class MstDepartmentService
    {
        public vwMstDepartment GetDepartment(vwMstDepartment param)
        {
            var context = new DbContext(Helper.GetConnection("HumanCapital"));
            var repo = new vwMstDepartmentRepository(context);
            var vwMstDepartment = new vwMstDepartment();
            try
            {
                vwMstDepartment = repo.GetList(WhereClause(param)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                new vwMstDepartment { ErrorType = 1, ErrorMessage = ex.Message };
            }
            finally
            {
                context.Dispose();
            }
            return vwMstDepartment;
        }

        public List<vwMstDepartment> GetDepartmentList(vwMstDepartment param)
        {
            var context = new DbContext(Helper.GetConnection("HumanCapital"));
            var repo = new vwMstDepartmentRepository(context);
            var vwMstDepartment = new List<vwMstDepartment>();
            try
            {
                vwMstDepartment = repo.GetList(WhereClause(param)).ToList();
            }
            catch (Exception ex)
            {
                new vwMstDepartment { ErrorType = 1, ErrorMessage = ex.Message };
            }
            finally
            {
                context.Dispose();
            }
            return vwMstDepartment;
        }
        public List<vwMstDepartment> GetDepartmentListInputTarget()
        {
            var context = new DbContext(Helper.GetConnection("HumanCapital"));
            var repo = new vwMstDepartmentRepository(context);
            var vwMstDepartment = new List<vwMstDepartment>();
            try
            {
                var whereClause = "DepartmentCode LIKE '%" + RADepartmentCodeTabEnum.NewBaps + "%' OR " +
                    "DepartmentCode LIKE '%" + RADepartmentCodeTabEnum.NewProduct + "%' OR " +
                    "DepartmentCode LIKE '%" + RADepartmentCodeTabEnum.NonTSEL + "%' OR " +
                    "DepartmentCode LIKE '%" + RADepartmentCodeTabEnum.TSEL + "%' ";
                vwMstDepartment = repo.GetList(whereClause).ToList();
            }
            catch (Exception ex)
            {
                new vwMstDepartment { ErrorType = 1, ErrorMessage = ex.Message };
            }
            finally
            {
                context.Dispose();
            }
            return vwMstDepartment;
        }

        public List<vwMstDepartment> GetDepartmentList(vwMstDepartment param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("HumanCapital"));
            var repo = new vwMstDepartmentRepository(context);
            var vwMstDepartment = new List<vwMstDepartment>();
            try
            {
                vwMstDepartment = repo.GetPaged(WhereClause(param),"",rowSkip,pageSize).ToList();
            }
            catch (Exception ex)
            {
                new vwMstDepartment { ErrorType = 1, ErrorMessage = ex.Message };
            }
            finally
            {
                context.Dispose();
            }
            return vwMstDepartment;
        }

        public int GetDepartmentCount(vwMstDepartment param)
        {
            var context = new DbContext(Helper.GetConnection("HumanCapital"));
            var repo = new vwMstDepartmentRepository(context);
            var vwMstDepartment = new List<vwMstDepartment>();
            try
            {
                return repo.GetCount(WhereClause(param));
            }
            catch (Exception ex)
            {
                new vwMstDepartment { ErrorType = 1, ErrorMessage = ex.Message };
                return 0;
            }
            finally
            {
                context.Dispose();
            }
           
        }


        private string WhereClause(vwMstDepartment param)
        {
            string whereClause = "";

            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.DepartmentName))
                    whereClause += "DepartmentName like '%" + param.DepartmentName + "%' AND ";

                if (!string.IsNullOrEmpty(param.DepartmentInitial))
                {
                    if (param.DepartmentInitial.ToLower().TrimStart().TrimEnd() == "null")
                    {
                        whereClause += "DepartmentInitial IS NULL AND ";
                    }
                    else
                    {
                        whereClause += "DepartmentInitial = '" + param.DepartmentInitial + "' AND  ";
                    }

                }


                if (!string.IsNullOrEmpty(param.DepartmentCode))
                    whereClause += "DepartmentCode = '" + param.DepartmentCode + "' AND ";

                if (!string.IsNullOrEmpty(param.DivisionCode))
                    whereClause += "DivisionCode = '" + param.DivisionCode + "' AND ";

                if (!string.IsNullOrEmpty(param.DirectorateCode))
                    whereClause += "DirectorateCode = '" + param.DirectorateCode + "' AND ";

                whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
            }

            return whereClause;
        }
    }
}
