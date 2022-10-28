using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System.Data;
using static ARSystem.Service.Constants;

namespace ARSystem.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TrxBAPSData" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TrxBAPSData.svc or TrxBAPSData.svc.cs at the Solution Explorer and start debugging.
    public class DashboardTSELService
    {

        public List<vwRABapsSite> GetTrxDashbordTSELDataToList(vwRABapsSite param, int rowSkip, int pageSize, string strOrderBy = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwRABapsSite> data = new List<vwRABapsSite>();
            try
            {
                var repo = new vwRABapsSiteRepository(context);
                string strWhereClause = " t.ID IS NULL AND TBGHCMstDepartmentCode = '" + RADepartmentCodeTabEnum.TSEL + "' AND ";

                if (!string.IsNullOrWhiteSpace(param.ListIdString))
                {
                    strWhereClause += "MstBapsId IN (" + param.ListIdString + ") AND ";
                }
                // Modification Or Added By Ibnu Setiawan 29. January 2020 Change filter Grid using Like 
                if (!string.IsNullOrWhiteSpace(param.CompanyInvoiceId))
                {
                    strWhereClause += "CompanyInvoiceId = '" + param.CompanyInvoiceId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerID))
                {
                    strWhereClause += "CustomerID = '" + param.CustomerID + "' AND ";
                }
                if (param.ProductID > 0)
                {
                    strWhereClause += "ProductID = '" + param.ProductID + "' AND ";
                }
                if (param.SectionProductId > 0)
                {
                    strWhereClause += "SectionProductId = '" + param.SectionProductId + "' AND ";
                }
                if (param.SowProductId > 0)
                {
                    strWhereClause += "SowProductId = '" + param.SowProductId + "' AND ";
                }
                if (param.YearBill > 0)
                {
                    strWhereClause += "b.YearBill = '" + param.YearBill + "' AND ";
                }
                if (param.RegionID > 0)
                {
                    strWhereClause += "b.RegionID = '" + param.RegionID + "' AND ";
                }
                if (param.ProvinceID > 0)
                {
                    strWhereClause += "b.ProvinceID = '" + param.ProvinceID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    strWhereClause += "SONumber LIKE '%" + param.SONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteID))
                {
                    strWhereClause += "b.SiteID LIKE '%" + param.SiteID + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteName))
                {
                    strWhereClause += "b.SiteName LIKE '%" + param.SiteName + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerSiteID))
                {
                    strWhereClause += "CustomerSiteID LIKE '%" + param.CustomerSiteID + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerSiteName))
                {
                    strWhereClause += "CustomerSiteName LIKE '%" + param.CustomerSiteName + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.RegionName))
                {
                    strWhereClause += "RegionName LIKE '%" + param.RegionName + "%' AND ";
                }
                if (param.StartInvoiceDate != null)
                {
                    strWhereClause += "sStartInvoiceDate >= '" + param.StartInvoiceDate + "' AND sEndInvoiceDate >= '" + param.StartInvoiceDate + "' AND ";
                }
                if (param.EndInvoiceDate != null)
                {
                    strWhereClause += "sEndInvoiceDate <= '" + param.EndInvoiceDate + "' AND sStartInvoiceDate <= '" + param.EndInvoiceDate + "' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (pageSize < 0)
                    data = repo.GetList(strWhereClause, strOrderBy, param.TargetBaps, param.TargetPower);
                //data = GetPage(data, rowSkip, pageSize);
                else
                    data = repo.GetPaged(strWhereClause, strOrderBy, rowSkip, pageSize, param.TargetBaps, param.TargetPower);

            }

            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwRABapsSite { ErrorMessage = ex.Message });
            }
            return data;

        }

        //public List<vwRABapsSite> GetPage(List<vwRABapsSite> list, int page, int pageSize)
        //{
        //    return list.Skip(page * pageSize).Take(pageSize).ToList();
        //}

        public List<string> GetListId(vwRABapsSite param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var CustomRepo = new GetListIdRepository(context);
            List<string> ListId = new List<string>();

            List<vwRABapsSite> data = new List<vwRABapsSite>();

            try
            {
                string strWhereClause = " t.ID IS NULL AND TBGHCMstDepartmentCode = '" + RADepartmentCodeTabEnum.TSEL + "' AND ";
                if (!string.IsNullOrWhiteSpace(param.CompanyInvoiceId))
                {
                    strWhereClause += "CompanyInvoiceId = '" + param.CompanyInvoiceId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerID))
                {
                    strWhereClause += "CustomerID = '" + param.CustomerID + "' AND ";
                }
                if (param.SectionProductId > 0)
                {
                    strWhereClause += "SectionProductId = '" + param.SectionProductId + "' AND ";
                }
                if (param.SowProductId > 0)
                {
                    strWhereClause += "SowProductId = '" + param.SowProductId + "' AND ";
                }
                if (param.ProductID > 0)
                {
                    strWhereClause += "ProductID = '" + param.ProductID + "' AND ";
                }
                if (param.YearBill > 0)
                {
                    strWhereClause += "b.YearBill = '" + param.YearBill + "' AND ";
                }
                if (param.RegionID > 0)
                {
                    strWhereClause += "b.RegionID = '" + param.RegionID + "' AND ";
                }
                if (param.ProvinceID > 0)
                {
                    strWhereClause += "b.ProvinceID = '" + param.ProvinceID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    strWhereClause += "SONumber = '" + param.SONumber + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteID))
                {
                    strWhereClause += "b.SiteID = '" + param.SiteID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteName))
                {
                    strWhereClause += "b.SiteName = '" + param.SiteName + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerSiteID))
                {
                    strWhereClause += "CustomerSiteID = '" + param.CustomerSiteID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerSiteName))
                {
                    strWhereClause += "CustomerSiteName = '" + param.CustomerSiteName + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerID))
                {
                    strWhereClause += "CustomerID = '" + param.CustomerID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.RegionName))
                {
                    strWhereClause += "RegionName = '" + param.RegionName + "' AND ";
                }
                if (param.StartInvoiceDate != null)
                {
                    strWhereClause += "sStartInvoiceDate >= '" + param.StartInvoiceDate + "' AND sEndInvoiceDate >= '" + param.StartInvoiceDate + "' AND ";
                }
                if (param.EndInvoiceDate != null)
                {
                    strWhereClause += "sEndInvoiceDate <= '" + param.EndInvoiceDate + "' AND sStartInvoiceDate <= '" + param.EndInvoiceDate + "' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                ListId = CustomRepo.GetListTargetRecurringId(param.TargetBaps, param.TargetPower, strWhereClause);
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwRABapsSite { ErrorMessage = ex.Message });
            }

            return ListId;

        }

        public int GetTrxDashbordTSELDataCount(vwRABapsSite param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxDashbordTSELDataRepo = new vwRABapsSiteRepository(context);
            List<vwRABapsSite> listDashboardTselData = new List<vwRABapsSite>();
            try
            {
                string strWhereClause = " t.ID IS NULL AND TBGHCMstDepartmentCode = '" + RADepartmentCodeTabEnum.TSEL + "' AND SowProductId IS NOT NULL AND ";

                // Modification Or Added By Ibnu Setiawan 29. January 2020 Add Filter MstBapsId and Change filter Grid using Like 
                if (!string.IsNullOrWhiteSpace(param.ListIdString))
                {
                    strWhereClause += "MstBapsId IN (" + param.ListIdString + ") AND ";
                }

                if (!string.IsNullOrWhiteSpace(param.CompanyInvoiceId))
                {
                    strWhereClause += "CompanyInvoiceId = '" + param.CompanyInvoiceId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerID))
                {
                    strWhereClause += "CustomerID = '" + param.CustomerID + "' AND ";
                }
                if (param.SectionProductId > 0)
                {
                    strWhereClause += "SectionProductId = '" + param.SectionProductId + "' AND ";
                }
                if (param.SowProductId > 0)
                {
                    strWhereClause += "SowProductId = '" + param.SowProductId + "' AND ";
                }
                if (param.ProductID > 0)
                {
                    strWhereClause += "ProductID = '" + param.ProductID + "' AND ";
                }
                if (param.YearBill > 0)
                {
                    strWhereClause += "b.YearBill = '" + param.YearBill + "' AND ";
                }
                if (param.RegionID > 0)
                {
                    strWhereClause += "b.RegionID = '" + param.RegionID + "' AND ";
                }
                if (param.ProvinceID > 0)
                {
                    strWhereClause += "b.ProvinceID = '" + param.ProvinceID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    strWhereClause += "SONumber LIKE '%" + param.SONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteID))
                {
                    strWhereClause += "b.SiteID LIKE '%" + param.SiteID + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteName))
                {
                    strWhereClause += "b.SiteName LIKE '%" + param.SiteName + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerSiteID))
                {
                    strWhereClause += "CustomerSiteID LIKE '%" + param.CustomerSiteID + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerSiteName))
                {
                    strWhereClause += "CustomerSiteName LIKE '%" + param.CustomerSiteName + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.RegionName))
                {
                    strWhereClause += "RegionName LIKE '%" + param.RegionName + "%' AND ";
                }

                if (param.StartInvoiceDate != null)
                {
                    strWhereClause += "sStartInvoiceDate >= '" + param.StartInvoiceDate + "' AND sEndInvoiceDate >= '" + param.StartInvoiceDate + "' AND ";
                }
                if (param.EndInvoiceDate != null)
                {
                    strWhereClause += "sEndInvoiceDate <= '" + param.EndInvoiceDate + "' AND sStartInvoiceDate <= '" + param.EndInvoiceDate + "' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                return TrxDashbordTSELDataRepo.GetCount(strWhereClause, param.TargetBaps, param.TargetPower);
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }
        }

        public List<MstRASectionProduct> GetSectionToList(string UserID, string strOrderBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var SectionRepo = new MstRASectionProductRepository(context);
            List<MstRASectionProduct> listSection = new List<MstRASectionProduct>();
            try
            {
                string strWhereClause = "";
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                listSection = SectionRepo.GetList(strWhereClause, strOrderBy);
                return listSection;
            }
            catch (Exception ex)
            {
                listSection.Add(new MstRASectionProduct((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetSectionToList", UserID)));
                return listSection;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<MstRASowProduct> GetSOWToList(string UserID, string strOrderBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var SOWRepo = new MstRASowProductRepository(context);
            List<MstRASowProduct> listSOW = new List<MstRASowProduct>();
            try
            {
                string strWhereClause = "";
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                listSOW = SOWRepo.GetList(strWhereClause, strOrderBy);
                return listSOW;
            }
            catch (Exception ex)
            {
                listSOW.Add(new MstRASowProduct((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetSOWToList", UserID)));
                return listSOW;
            }
            finally
            {
                context.Dispose();
            }
        }


        public List<MstRATargetRecurring> AddDataDashboardTSEL(List<MstRATargetRecurring> post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var resultRepo = new MstRATargetRecurringRepository(context);
            var vwRABapsSiteRepo = new vwRABapsSiteRepository(context);

            List<MstRATargetRecurring> result = new List<MstRATargetRecurring>();

            try
            {
                var repo = new MstRATargetRecurringRepository(context);
                List<MstRATargetRecurring> MstRATargetRecurring = new List<MstRATargetRecurring>();

                result = repo.CreateBulky(post);
                return result;
            }
            catch (Exception ex)
            {
                context.Dispose();
                result.Add(new MstRATargetRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "AddDataDashboardTSEL", "")));
                return result;
            }
        }

        public List<MstRATargetNewBaps> AddDataTargetNewBaps(List<MstRATargetNewBaps> post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var resultRepo = new MstRATargetNewBapsRepository(context);
            var vwRABapsSiteRepo = new vwRABapsSiteRepository(context);

            List<MstRATargetNewBaps> result = new List<MstRATargetNewBaps>();

            try
            {
                result = resultRepo.CreateBulkyNewBaps(post);
                return result;
            }
            catch (Exception ex)
            {
                context.Dispose();
                result.Add(new MstRATargetNewBaps((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "AddDataTargetNewBaps", "")));
                return result;
            }
        }


        //  return result;

        public int GetHistoryRecurringListCount(vwRABapsSite param, List<string> strSONumberMultiple)
        {
            return pGetHistoryRecurringListCount(param, strSONumberMultiple);
        }
        public List<vwRABapsSite> GetHistoryRecurringToList(vwRABapsSite param, List<string> strSONumberMultiple, int rowSkip, int pageSize)
        {
            return pGetHistoryRecurringToList(param, strSONumberMultiple, rowSkip, pageSize);
        }
        private int pGetHistoryRecurringListCount(vwRABapsSite param, List<string> strSONumberMultiple)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {
                var repo = new vwRABapsSiteRepository(context);
                string strWhereClause = " ID IS NOT NULL AND ";
                if (strSONumberMultiple != null && strSONumberMultiple.Any())
                {
                    strWhereClause += "(";
                    for (int i = 0; i < strSONumberMultiple.Count(); i++)
                    {

                        strWhereClause += "SoNumber like '%" + strSONumberMultiple[i] + "%' ";
                        strWhereClause += (i == (strSONumberMultiple.Count() - 1)) ? "" : "OR ";
                    }
                    strWhereClause += ") AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.PowerType))
                {
                    strWhereClause += "PowerType = '" + param.PowerType + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.ReconcileType))
                {
                    strWhereClause += "BapsType = '" + param.ReconcileType + "' AND ";
                }
                if (param.TargetYear > 0)
                {
                    strWhereClause += "TargetYear = '" + param.TargetYear + "' AND ";
                }
                if (param.TargetMonth > 0)
                {
                    strWhereClause += "TargetMonth= '" + param.TargetMonth + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.DepartmentType))
                {
                    strWhereClause += "DepartmentCode= '" + param.DepartmentType + "' AND ";

                }

                if (!string.IsNullOrWhiteSpace(param.CustomerID))
                {
                    strWhereClause += "CustomerID= '" + param.CustomerID + "' AND ";

                }
                if (!string.IsNullOrWhiteSpace(param.CompanyInvoiceId))
                {
                    strWhereClause += "CompanyInvoiceId= '" + param.CompanyInvoiceId + "' AND ";

                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                return repo.pGetHistoryCount(strWhereClause, param.TargetBaps, param.TargetPower);
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }

        }

        private List<vwRABapsSite> pGetHistoryRecurringToList(vwRABapsSite param, List<string> strSONumberMultiple, int rowSkip, int pageSize)

        {

            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwRABapsSite> data = new List<vwRABapsSite>();
            try
            {
                var repo = new vwRABapsSiteRepository(context);
                List<vwRABapsSite> dataHistory = new List<vwRABapsSite>();
                string strWhereClause = " ID IS NOT NULL AND ";
                if (strSONumberMultiple != null && strSONumberMultiple.Any())
                {
                    strWhereClause += "(";
                    for (int i = 0; i < strSONumberMultiple.Count(); i++)
                    {

                        strWhereClause += "SoNumber like '%" + strSONumberMultiple[i] + "%' ";
                        strWhereClause += (i == (strSONumberMultiple.Count() - 1)) ? "" : "OR ";
                    }
                    strWhereClause += ") AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.PowerType))
                {
                    strWhereClause += "PowerType = '" + param.PowerType + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.ReconcileType))
                {
                    strWhereClause += "BapsType = '" + param.ReconcileType + "' AND ";
                }
                if (param.TargetYear > 0)
                {
                    strWhereClause += "TargetYear = '" + param.TargetYear + "' AND ";
                }
                if (param.TargetMonth > 0)
                {
                    strWhereClause += "TargetMonth= '" + param.TargetMonth + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.DepartmentType))
                {
                    strWhereClause += "DepartmentCode= '" + param.DepartmentType + "' AND ";

                }

                if (!string.IsNullOrWhiteSpace(param.CustomerID))
                {
                    strWhereClause += "CustomerID= '" + param.CustomerID + "' AND ";

                }
                if (!string.IsNullOrWhiteSpace(param.CompanyInvoiceId))
                {
                    strWhereClause += "CompanyInvoiceId= '" + param.CompanyInvoiceId + "' AND ";

                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                data = repo.pGetHistoryPaged(strWhereClause, "", rowSkip, pageSize, param.TargetBaps, param.TargetPower);
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwRABapsSite { ErrorMessage = ex.Message });
            }
            return data;

        }

        #region Achievement
        public List<vwGetInvoiceRecurring> GetDataRecurring(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwGetInvoiceRecurringRepository(context);
            List<vwGetInvoiceRecurring> list = new List<vwGetInvoiceRecurring>();
            try
            {
                string whereClause = strWhereClause;
                list = repo.GetRecurring(whereClause, "MonthBill");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwGetInvoiceRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetDataRecurring", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwGetInvoiceRecurring> GetTowerDataRecurring(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwGetInvoiceRecurringRepository(context);
            List<vwGetInvoiceRecurring> list = new List<vwGetInvoiceRecurring>();
            try
            {
                string whereClause = strWhereClause;
                list = repo.GetTowerRecurring(whereClause, "MonthBill");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwGetInvoiceRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetDataRecurring", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwGetInvoiceRecurring> GetPowerRecurring(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwGetInvoiceRecurringRepository(context);
            List<vwGetInvoiceRecurring> list = new List<vwGetInvoiceRecurring>();
            try
            {
                string whereClause = strWhereClause;
                list = repo.GetPowerRecurring(whereClause, "MonthBill");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwGetInvoiceRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetPowerRecurring", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion

        #region Target
        public List<vwGetInvoiceRecurring> GetAchievementTarget(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwGetInvoiceRecurringRepository(context);
            List<vwGetInvoiceRecurring> list = new List<vwGetInvoiceRecurring>();
            try
            {
                string whereClause = strWhereClause;
                list = repo.GetAchievementTarget(whereClause, "MonthBill");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwGetInvoiceRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetAchievementTarget", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<MstRATargetRecurring> GetTarget(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new MstRATargetRecurringRepository(context);
            List<MstRATargetRecurring> list = new List<MstRATargetRecurring>();
            try
            {
                string whereClause = strWhereClause;
                list = repo.GetTarget(whereClause, "");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new MstRATargetRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetTarget", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<MstRATargetRecurring> GetAllTarget(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new MstRATargetRecurringRepository(context);
            List<MstRATargetRecurring> list = new List<MstRATargetRecurring>();
            try
            {
                string whereClause = strWhereClause;
                list = repo.GetAllTarget(whereClause, "");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new MstRATargetRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetAllTarget", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion

        #region Detail Site
        public List<vwRADetailSiteRecurring> GetDetailSite(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwRADetailSiteRecurringRepository(context);
            List<vwRADetailSiteRecurring> list = new List<vwRADetailSiteRecurring>();
            try
            {
                string whereClause = strWhereClause;
                list = repo.GetList(whereClause, "");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwRADetailSiteRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetDetailSite", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwRADetailSiteRecurring> GetDetailTargetSite(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwRADetailSiteRecurringRepository(context);
            List<vwRADetailSiteRecurring> list = new List<vwRADetailSiteRecurring>();
            try
            {
                string whereClause = strWhereClause;
                list = repo.GetTargetList(whereClause, "");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwRADetailSiteRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetDetailTargetSite", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetCountDetailSite(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwRADetailSiteRecurringRepository(context);
            List<vwRADetailSiteRecurring> list = new List<vwRADetailSiteRecurring>();
            try
            {
                var dt = repo.GetCount(whereClause);
                return dt;
            }
            catch (Exception ex)
            {
                list.Add(new vwRADetailSiteRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetCountDetailSite", "")));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetCountDetailTargetSite(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwRADetailSiteRecurringRepository(context);
            List<vwRADetailSiteRecurring> list = new List<vwRADetailSiteRecurring>();
            try
            {
                var dt = repo.GetTargetCount(whereClause);
                return dt;
            }
            catch (Exception ex)
            {
                list.Add(new vwRADetailSiteRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetCountDetailTargetSite", "")));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwRADetailSiteRecurring> GetPageDetailSite(string whereClause, int RowSkip, int PageSize, string strOrderBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwRADetailSiteRecurringRepository(context);
            List<vwRADetailSiteRecurring> list = new List<vwRADetailSiteRecurring>();
            try
            {
                list = repo.GetPaged(whereClause, strOrderBy, RowSkip, PageSize);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwRADetailSiteRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetPageDetailSite", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwRADetailSiteRecurring> GetPageDetailTargetSite(string whereClause, int RowSkip, int PageSize, string strOrderBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwRADetailSiteRecurringRepository(context);
            List<vwRADetailSiteRecurring> list = new List<vwRADetailSiteRecurring>();
            try
            {
                list = repo.GetTargetPaged(whereClause, strOrderBy, RowSkip, PageSize);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwRADetailSiteRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "GetPageDetailTargetSite", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion

        public List<mstBapsType> GetBapsTypeToListTSEL(string UserID, string strOrderBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var BapsTypeRepo = new mstBapsTypeRepository(context);
            List<mstBapsType> listBapsType = new List<mstBapsType>();

            try
            {
                string strWhereClause = "mstBapsTypeId IN (3,5) and ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                listBapsType = BapsTypeRepo.GetList(strWhereClause, strOrderBy);


                return listBapsType;
            }
            catch (Exception ex)
            {
                listBapsType.Add(new mstBapsType((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstDataSourceService", "GetBapsTypeToListTSEL", UserID)));
                return listBapsType;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<mstBapsType> GetBapsTypeToList(string UserID, string strOrderBy, string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var BapsTypeRepo = new mstBapsTypeRepository(context);
            List<mstBapsType> listBapsType = new List<mstBapsType>();

            try
            {
                whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";

                listBapsType = BapsTypeRepo.GetList(whereClause, strOrderBy);


                return listBapsType;
            }
            catch (Exception ex)
            {
                listBapsType.Add(new mstBapsType((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstDataSourceService", "GetBapsTypeToList", UserID)));
                return listBapsType;
            }
            finally
            {
                context.Dispose();
            }
        }

    }
}