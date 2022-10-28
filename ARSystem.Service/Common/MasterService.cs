using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.Models;
using System.Data;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystem.Service
{
    public class MasterService
    {
        private DbContext _aRSystemContext;
        private DbContext _tbgSysContext;
        private DbContext _humanCapitalContext;

        public MasterService()
        {
            _aRSystemContext =  new DbContext(Helper.GetConnection("ARSystem"));
            _tbgSysContext = new DbContext(Helper.GetConnection("TBIGSys"));
            _humanCapitalContext = new DbContext(Helper.GetConnection("HumanCapital"));
        }

        #region Product
        public List<mstProduct> GetProductList(string userID)
        {
            var context = new DbContext(Helper.GetConnection("TBIGSys"));
            var productRepo = new mstProductRepository(context);
            List<mstProduct> productList = new List<mstProduct>();

            try
            {
                return productRepo.GetList("IsActive = 1", "Product");
            }
            catch (Exception ex)
            {
                productList.Add(new mstProduct((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "MasterService", "GetProductList", userID)));
                return productList;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<mstDropdown> GetTenantTypeByOperator(string operatorID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<mstDropdown> result = new List<mstDropdown>();
            try
            {
                var command = context.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetProductByOperator";
                if(operatorID != null)
                {
                    command.Parameters.Add(command.CreateParameter("@vOperatorID", operatorID));
                }else
                {
                    command.Parameters.Add(command.CreateParameter("@vOperatorID", DBNull.Value));
                }
                using (var rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        result.Add(new mstDropdown
                        {
                            Text = rdr["Product"].ToString(),
                            Value = rdr["ProductID"].ToString()
                        });
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Add(new mstDropdown((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "MasterService", "GetTenantTypeByOperator", operatorID)));
                return result;
            }
            finally
            {
                context.Dispose();
            }
        }

        #endregion

        #region Company
        public List<mstCompany> GetCompanyList(string userID)
        {
            var context = new DbContext(Helper.GetConnection("TBIGSys"));
            var productRepo = new mstCompanyRepository(context);
            List<mstCompany> productList = new List<mstCompany>();

            try
            {
                return productRepo.GetList("IsActive = 1", "CompanyID");
            }
            catch (Exception ex)
            {
                productList.Add(new mstCompany((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "MasterService", "GetCompanyList", userID)));
                return productList;
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion

        #region Customer
        public List<mstCustomer> GetCustomerList(string userID)
        {
            var context = new DbContext(Helper.GetConnection("TBIGSys"));
            var productRepo = new mstCustomerRepository(context);
            List<mstCustomer> productList = new List<mstCustomer>();

            try
            {
                return productRepo.GetList("IsActive = 1 AND IsTelco = 1", "CustomerID");
            }
            catch (Exception ex)
            {
                productList.Add(new mstCustomer((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "MasterService", "GetCustomerList", userID)));
                return productList;
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion

        #region STIP
        public List<mstSTIP> GetSTIPList(string userID)
        {
            var context = new DbContext(Helper.GetConnection("TBIGSys"));
            var productRepo = new mstSTIPRepository(context);
            List<mstSTIP> productList = new List<mstSTIP>();

            try
            {
                return productRepo.GetList("IsActive = 1", "STIPID");
            }
            catch (Exception ex)
            {
                productList.Add(new mstSTIP((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "MasterService", "GetSTIPList", userID)));
                return productList;
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion

        #region Province
        public List<mstProvince> GetProvince(string userID)
        {
            var provinceRepo = new mstProvinceRepository(_aRSystemContext);
            List<mstProvince> provinceList = new List<mstProvince>();

            try
            {
                return provinceRepo.GetList("","");
            }
            catch (Exception ex)
            {
                provinceList.Add(new mstProvince((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "MasterService", "GetProvince", userID)));
                return provinceList;
            }
            finally
            {
                _aRSystemContext.Dispose();
            }
        }
        public List<mstProvince> GetTbgSysProvince(string userID)
        {
            var provinceRepo = new mstTbgSysProvinceRepository(_tbgSysContext);
            List<mstProvince> provinceList = new List<mstProvince>();

            try
            {
                return provinceRepo.GetList("", "");
            }
            catch (Exception ex)
            {
                provinceList.Add(new mstProvince((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "MasterService", "GetProvince", userID)));
                return provinceList;
            }
            finally
            {
                _aRSystemContext.Dispose();
            }
        }

        #endregion

        #region HumanCapital
        public List<mstDepartment> GetmstDepartmentList(string userID)
        {
            var mstDepartmentRepo = new mstDepartmentRepository(_humanCapitalContext);
            List<mstDepartment> deptList = new List<mstDepartment>();

            try
            {
                return mstDepartmentRepo.GetList("", "");
            }
            catch (Exception ex)
            {
                deptList.Add(new mstDepartment((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "MasterService", "GetmstDepartmentList", userID)));
                return deptList;
            }
            finally
            {
                _humanCapitalContext.Dispose();
            }
        }
        #endregion

        #region PPN
        public vmStringResult GetPPNPercentage(string userID)
        {
            vmStringResult StringModel = new vmStringResult();
            var parameterRepo = new mstParameterRepository(_aRSystemContext);
            double PPNValue = 1;

            try
            {
                string ParameterWhereClause = "ParameterKey LIKE '" + Constants.Parameter.PPN + "%' AND CONVERT(DATE, StartDate) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, EndDate) >= CONVERT(DATE, GETDATE())";
                PPNValue = double.Parse(parameterRepo.GetList(ParameterWhereClause, "").FirstOrDefault().ParameterValue);
                string PPNText = parameterRepo.GetList(ParameterWhereClause, "").FirstOrDefault().ParameterKey;
                StringModel.PPNValue = PPNValue;
                StringModel.PPNText = PPNText;
                return StringModel;
            }
            catch (Exception ex)
            {
                return new vmStringResult((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstDataSourceService", "GetPPNPercentage", userID));
            }
            finally
            {
                _aRSystemContext.Dispose();
            }
        }
        #endregion
    }
}
