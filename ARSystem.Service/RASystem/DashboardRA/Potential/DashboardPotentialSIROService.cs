using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;

using ARSystem.Domain.Repositories;
using System.Data;
using ARSystem.Domain.Models;

namespace ARSystem.Service
{
    public class DashboardPotentialSIROService
    {
        public DataTable GetDashboardSummaryList(
            string Type,
            string Key,
            string ProductID,
            string STIPID,
            string Customer,
            string CompanyID,
            string Year,
            string Month,
            string Desc
            )
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new DashboardPotentialSIRORepository(context);
            DataTable dt = new DataTable();
            try
            {
                return Repo.GetDashboardAllOperatorOutstandingSummaryList(
                                                                            Type,
                                                                            Key,
                                                                            ProductID,
                                                                            STIPID,
                                                                            Customer,
                                                                            CompanyID,
                                                                            Year,
                                                                            Month,
                                                                            Desc
                                                                            );
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "DashboardPotentialRecurringOutstandingService", "GetDashboardSummaryList", "");
                return dt;
            }
            finally
            {
                context.Dispose();
            }
        }


        public int GetDetailCount(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new DashboardPotentialSIRORepository(context);
            List<dwhDashboardRAPotentialSIRODetailModel> list = new List<dwhDashboardRAPotentialSIRODetailModel>();
            try
            {
                var dt = repo.GetCount(whereClause);
                return dt;
            }
            catch (Exception ex)
            {
                list.Add(new dwhDashboardRAPotentialSIRODetailModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardAllOperatorPotentialRecurringOutstandingService", "GetDetailCount", "")));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<dwhDashboardRAPotentialSIRODetailModel> GetDetailList(string whereClause, int RowSkip, int PageSize, string strOrderBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new DashboardPotentialSIRORepository(context);
            List<dwhDashboardRAPotentialSIRODetailModel> list = new List<dwhDashboardRAPotentialSIRODetailModel>();
            try
            {
                if (PageSize > 0)
                    list = repo.GetPaged(whereClause, strOrderBy, RowSkip, PageSize);
                else
                    list = repo.GetList(whereClause);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new dwhDashboardRAPotentialSIRODetailModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardAllOperatorPotentialRecurringOutstandingService", "GetDetailList", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<MstInitialDepartment> GetSection()
        {
            List<MstInitialDepartment> data = new List<MstInitialDepartment>
            {
                new MstInitialDepartment { Description = "Recurring BAPS Non TSEL Department", DepartmentCode = "DE000435" },
                new MstInitialDepartment { Description = "Recurring BAPS TSEL Department", DepartmentCode = "DE000434" },
                new MstInitialDepartment { Description = "Recurring New Product & Others Revenue Department", DepartmentCode = "DE000421" },
                //new MstInitialDepartment { Description = "Not Mapping", DepartmentCode = "Not Mapping" }
            };

            try
            {

            }
            catch (Exception ex)
            {

                data.Add(new MstInitialDepartment { ErrorMessage = ex.Message });
            }
            return data;
        }

        public List<MstInitialDepartment> GetCustomer()
        {
            List<MstInitialDepartment> data = new List<MstInitialDepartment>
            {
                new MstInitialDepartment { Description = "BERCA", DepartmentCode = "BERCA" },
                new MstInitialDepartment { Description = "FLEXI", DepartmentCode = "FLEXI" },
                new MstInitialDepartment { Description = "HCPT", DepartmentCode = "HCPT" },
                new MstInitialDepartment { Description = "M8", DepartmentCode = "M8" },
                new MstInitialDepartment { Description = "SMART", DepartmentCode = "SMART" },
                new MstInitialDepartment { Description = "SMART8", DepartmentCode = "SMART8" },
                new MstInitialDepartment { Description = "TSEL", DepartmentCode = "TSEL" },
                 new MstInitialDepartment { Description = "XL", DepartmentCode = "XL" },
                new MstInitialDepartment { Description = "ISAT", DepartmentCode = "ISAT" }
            };

            try
            {

            }
            catch (Exception ex)
            {

                data.Add(new MstInitialDepartment { ErrorMessage = ex.Message });
            }
            return data;
        }

        public List<MstInitialDepartment> GetSOW()
        {
            List<MstInitialDepartment> data = new List<MstInitialDepartment>
            {
                 new MstInitialDepartment { Description = "2", DepartmentCode = "2" },
                 new MstInitialDepartment { Description = "ADDITIONAL", DepartmentCode = "ADDITIONAL" },
                 new MstInitialDepartment { Description = "COLO", DepartmentCode = "COLO" },
                 new MstInitialDepartment { Description = "COLO 3G", DepartmentCode = "COLO 3G" },
                 new MstInitialDepartment { Description = "COLO HC", DepartmentCode = "COLO HC" },
                 new MstInitialDepartment { Description = "COLO IBS", DepartmentCode = "COLO IBS" },
                 new MstInitialDepartment { Description = "COLO MCP FO", DepartmentCode = "COLO MCP FO" },
                 new MstInitialDepartment { Description = "COLO MCP NON FO", DepartmentCode = "COLO MCP NON FO" },
                 new MstInitialDepartment { Description = "COLO-RF", DepartmentCode = "COLO-RF" },
                 new MstInitialDepartment { Description = "IBS/DAS", DepartmentCode = "IBS/DAS" },
                 new MstInitialDepartment { Description = "MCP NON FO", DepartmentCode = "MCP NON FO" },
                 new MstInitialDepartment { Description = "NEW MCP FO", DepartmentCode = "NEW MCP FO" },
                 new MstInitialDepartment { Description = "NEW MCP NON FO", DepartmentCode = "NEW MCP NON FO" },
                 new MstInitialDepartment { Description = "SHELTER ONLY", DepartmentCode = "SHELTER ONLY" },
                 new MstInitialDepartment { Description = "SITE ACCESS", DepartmentCode = "SITE ACCESS" },
                 new MstInitialDepartment { Description = "TOWER", DepartmentCode = "TOWER" }


            };

            try
            {

            }
            catch (Exception ex)
            {

                data.Add(new MstInitialDepartment { ErrorMessage = ex.Message });
            }
            return data;
        }
    }
}
