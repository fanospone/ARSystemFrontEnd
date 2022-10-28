using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System.Data;
using static ARSystem.Service.Constants;

namespace ARSystem.Service.RASystem.DashboardRA.DashboardTSEL
{
    public class InputTargetService : IDisposable
    {
        private DbContext _context;
        private MstRATargetRecurringRepository _mstRATargetRecurringRepository;
        private MstRATargetNewBapsRepository _mstRATargetNewBapsRepository;
        private mstARSystemConstantsRepository _mstARSystemConstantsRepository;
        
        private const string _departmentCodeTSEL = "DE000434";
        private const string _departmentCodeNonTSEL = "DE000435";
        private const string _departmentCodeNewBAPS = "DE290";
        private const string _departmentCodeNewProduct = "DE000421";
        public InputTargetService()
        {
            _context = new DbContext(Helper.GetConnection("ARSystem"));
            _mstRATargetRecurringRepository = new MstRATargetRecurringRepository(_context);
            _mstRATargetNewBapsRepository = new MstRATargetNewBapsRepository(_context);
            _mstARSystemConstantsRepository = new mstARSystemConstantsRepository(_context);
        }
        public string GetConstants(string name)
        {
            return _mstARSystemConstantsRepository.GetByName(name);
        }
        public int GetTrxTargetNonTSELDataCount(vwRABapsSite param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxDashbordTSELDataRepo = new vwRABapsSiteRepository(context);
            List<vwRABapsSite> listDashboardTselData = new List<vwRABapsSite>();
            try
            {
                string strWhereClause = " t.ID IS NULL AND TBGHCMstDepartmentCode = '" + RADepartmentCodeTabEnum.NonTSEL + "' AND ";

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
                else
                {
                    if (param.DepartmentType == RADepartmentTypeEnum.NonTSEL)
                    {
                        strWhereClause += "CustomerID NOT LIKE 'TSEL%' AND ";
                    }
                }

                if (param.SectionProductId > 0)
                {
                    strWhereClause += "SectionProductId = '" + param.SectionProductId + "' AND ";
                }
                if (param.SowProductId > 0)
                {
                    strWhereClause += "SowProductId = '" + param.SowProductId + "' AND ";
                }
                if (param.RegionID > 0)
                {
                    strWhereClause += "b.RegionID = '" + param.RegionID + "' AND ";
                }
                if (param.ProvinceID > 0)
                {
                    strWhereClause += "b.ProvinceID = '" + param.ProvinceID + "' AND ";
                }
                if (param.ProductID > 0)
                {
                    strWhereClause += "ProductID = '" + param.ProductID + "' AND ";
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
        public List<vwRABapsSite> GetTrxDashbordNonTSELDataToList(vwRABapsSite param, int rowSkip, int pageSize, string strOrderBy = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwRABapsSite> data = new List<vwRABapsSite>();
            try
            {
                var repo = new vwRABapsSiteRepository(context);
                string strWhereClause = " t.ID IS NULL AND TBGHCMstDepartmentCode = '" + RADepartmentCodeTabEnum.NonTSEL + "' AND ";

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
                else
                {
                    if (param.DepartmentType == RADepartmentTypeEnum.NonTSEL)
                    {
                        strWhereClause += "CustomerID NOT LIKE 'TSEL%' AND ";
                    }
                }
                if (param.SectionProductId > 0)
                {
                    strWhereClause += "SectionProductId = '" + param.SectionProductId + "' AND ";
                }
                if (param.SowProductId > 0)
                {
                    strWhereClause += "SowProductId = '" + param.SowProductId + "' AND ";
                }
                //if (param.YearBill > 0)
                //{
                //    //strWhereClause += "b.YearBill = '" + param.YearBill + "' AND ";
                //    strWhereClause += "(b.sStartInvoiceDate IS NOT NULL AND YEAR(b.sStartInvoiceDate) >= " + param.YearBill + " AND " +
                //        "((b.sEndInvoiceDate IS NOT NULL AND YEAR(b.sEndInvoiceDate) <= " + param.YearBill + ") OR b.sEndInvoiceDate IS NULL)) AND ";

                //}
                //if (param.MonthBill > 0)
                //{
                //    strWhereClause += "(b.sStartInvoiceDate IS NOT NULL AND MONTH(b.sStartInvoiceDate) >= " + param.MonthBill + " AND " +
                //        "((b.sEndInvoiceDate IS NOT NULL AND MONTH(b.sEndInvoiceDate) <= " + param.MonthBill + ") OR b.sEndInvoiceDate IS NULL)) AND ";
                //}
                if (param.RegionID > 0)
                {
                    strWhereClause += "b.RegionID = '" + param.RegionID + "' AND ";
                }
                if (param.ProvinceID > 0)
                {
                    strWhereClause += "b.ProvinceID = '" + param.ProvinceID + "' AND ";
                }
                if (param.ProductID > 0)
                {
                    strWhereClause += "ProductID = '" + param.ProductID + "' AND ";
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
        public List<string> GetListNonTSELId(vwRABapsSite param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var CustomRepo = new GetListIdRepository(context);
            List<string> ListId = new List<string>();

            List<vwRABapsSite> data = new List<vwRABapsSite>();

            try
            {
                string strWhereClause = " t.ID IS NULL AND TBGHCMstDepartmentCode = '" + RADepartmentCodeTabEnum.NonTSEL + "' AND ";

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
                else
                {
                    if (param.DepartmentType == RADepartmentTypeEnum.NonTSEL)
                    {
                        strWhereClause += "CustomerID NOT LIKE 'TSEL%' AND ";
                    }
                }

                if (param.SectionProductId > 0)
                {
                    strWhereClause += "SectionProductId = '" + param.SectionProductId + "' AND ";
                }
                if (param.SowProductId > 0)
                {
                    strWhereClause += "SowProductId = '" + param.SowProductId + "' AND ";
                }
                if (param.RegionID > 0)
                {
                    strWhereClause += "b.RegionID = '" + param.RegionID + "' AND ";
                }
                if (param.ProvinceID > 0)
                {
                    strWhereClause += "b.ProvinceID = '" + param.ProvinceID + "' AND ";
                }
                if (param.ProductID > 0)
                {
                    strWhereClause += "ProductID = '" + param.ProductID + "' AND ";
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
                ListId = CustomRepo.GetListTargetRecurringId(param.TargetBaps, param.TargetPower, strWhereClause);
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwRABapsSite { ErrorMessage = ex.Message });
            }

            return ListId;

        }




        public int GetTrxTargetNewBapsDataCount(vwRABapsSite param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxDashbordTSELDataRepo = new vwRABapsSiteRepository(context);
            List<vwRABapsSite> listDashboardTselData = new List<vwRABapsSite>();
            try
            {
                string strWhereClause = " t.ID IS NULL AND ";

                // Modification Or Added By Ibnu Setiawan 29. January 2020 Add Filter MstBapsId and Change filter Grid using Like 
                if (!string.IsNullOrWhiteSpace(param.ListIdString))
                {
                    strWhereClause += "MstBapsId IN (" + param.ListIdString + ") AND ";
                }

                if (!string.IsNullOrWhiteSpace(param.CompanyInvoiceId))
                {
                    strWhereClause += "newBaps.CompanyInvoiceId = '" + param.CompanyInvoiceId + "' AND ";
                }

                if (!string.IsNullOrWhiteSpace(param.CustomerID))
                {
                    strWhereClause += "newBaps.CustomerID = '" + param.CustomerID + "' AND ";
                }
                else
                {
                    if (param.DepartmentType == RADepartmentTypeEnum.NonTSEL)
                    {
                        strWhereClause += "newBaps.CustomerID NOT LIKE 'TSEL%' AND ";
                    }
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
                    strWhereClause += "YEAR(BaukDone) = '" + param.YearBill + "' AND ";

                }
                if (param.MonthBill > 0)
                {
                    strWhereClause += "MONTH(BaukDone) = '" + param.MonthBill + "' AND ";
                }
                if (param.RegionID > 0)
                {
                    strWhereClause += "RegionID = '" + param.RegionID + "' AND ";
                }
                if (param.ProvinceID > 0)
                {
                    strWhereClause += "ProvinceID = '" + param.ProvinceID + "' AND ";
                }
                if (param.ProductID > 0)
                {
                    strWhereClause += "ProductID = '" + param.ProductID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    strWhereClause += "newBaps.SONumber LIKE '%" + param.SONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteID))
                {
                    strWhereClause += "SiteID LIKE '%" + param.SiteID + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteName))
                {
                    strWhereClause += "SiteName LIKE '%" + param.SiteName + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerSiteID))
                {
                    strWhereClause += "newBaps.CustomerSiteID LIKE '%" + param.CustomerSiteID + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerSiteName))
                {
                    strWhereClause += "newBaps.CustomerSiteName LIKE '%" + param.CustomerSiteName + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.RegionName))
                {
                    strWhereClause += "RegionName LIKE '%" + param.RegionName + "%' AND ";
                }
                if (param.StartInvoiceDate != null)
                {
                    strWhereClause += "BaukDone >= '" + param.StartInvoiceDate + "' AND ";
                }
                if (param.EndInvoiceDate != null)
                {
                    strWhereClause += "BaukDone <= '" + param.EndInvoiceDate + "' AND ";
                }
                if (param.StipSiro != null)
                {
                    if (param.StipSiro == 0)
                    {
                        strWhereClause += "(newBaps.StipSiro = '" + param.StipSiro + "' OR newBaps.StipSiro IS NULL) AND ";
                    }
                    else
                    {
                        strWhereClause += "newBaps.StipSiro = '" + param.StipSiro + "' AND ";
                    }
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                return TrxDashbordTSELDataRepo.pGetNewBapsCount(strWhereClause, param.TargetBaps, param.TargetPower);
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }
        }

        public List<vwRABapsSite> GetTrxTargetNewBapsToList(vwRABapsSite param, int rowSkip, int pageSize, string strOrderBy = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwRABapsSite> data = new List<vwRABapsSite>();
            try
            {
                var repo = new vwRABapsSiteRepository(context);
                string strWhereClause = " t.ID IS NULL AND ";

                // Modification Or Added By Ibnu Setiawan 29. January 2020 Add Filter MstBapsId and Change filter Grid using Like 
                if (!string.IsNullOrWhiteSpace(param.ListIdString))
                {
                    strWhereClause += "MstBapsID IN (" + param.ListIdString + ") AND ";
                }

                if (!string.IsNullOrWhiteSpace(param.CompanyInvoiceId))
                {
                    strWhereClause += "newBaps.CompanyInvoiceId = '" + param.CompanyInvoiceId + "' AND ";
                }

                if (!string.IsNullOrWhiteSpace(param.CustomerID))
                {
                    strWhereClause += "newBaps.CustomerID = '" + param.CustomerID + "' AND ";
                }
                else
                {
                    if (param.DepartmentType == RADepartmentTypeEnum.NonTSEL)
                    {
                        strWhereClause += "newBaps.CustomerID NOT LIKE 'TSEL%' AND ";
                    }
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
                    strWhereClause += "YEAR(BaukDone) = '" + param.YearBill + "' AND ";

                }
                if (param.MonthBill > 0)
                {
                    strWhereClause += "MONTH(BaukDone) = '" + param.MonthBill + "' AND ";
                }
                if (param.RegionID > 0)
                {
                    strWhereClause += "RegionID = '" + param.RegionID + "' AND ";
                }
                if (param.ProvinceID > 0)
                {
                    strWhereClause += "ProvinceID = '" + param.ProvinceID + "' AND ";
                }
                if (param.ProductID > 0)
                {
                    strWhereClause += "ProductID = '" + param.ProductID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    strWhereClause += "newBaps.SONumber LIKE '%" + param.SONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteID))
                {
                    strWhereClause += "SiteID LIKE '%" + param.SiteID + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteName))
                {
                    strWhereClause += "SiteName LIKE '%" + param.SiteName + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerSiteID))
                {
                    strWhereClause += "newBaps.CustomerSiteID LIKE '%" + param.CustomerSiteID + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerSiteName))
                {
                    strWhereClause += "newBaps.CustomerSiteName LIKE '%" + param.CustomerSiteName + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.RegionName))
                {
                    strWhereClause += "RegionName LIKE '%" + param.RegionName + "%' AND ";
                }
                if (param.StartInvoiceDate != null)
                {
                    strWhereClause += "BaukDone >= '" + param.StartInvoiceDate + "' AND ";
                }
                if (param.EndInvoiceDate != null)
                {
                    strWhereClause += "BaukDone <= '" + param.EndInvoiceDate + "' AND ";
                }
                if (strOrderBy == "SoNumber")
                {
                    strOrderBy = "newBaps.SoNumber";
                }
                if (param.StipSiro != null)
                {
                    if (param.StipSiro == 0)
                    {
                        strWhereClause += "(newBaps.StipSiro = '" + param.StipSiro + "' OR newBaps.StipSiro IS NULL) AND ";
                    }
                    else
                    {
                        strWhereClause += "newBaps.StipSiro = '" + param.StipSiro + "' AND ";
                    }

                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (pageSize < 0)
                    data = repo.pGetNewBapsList(strWhereClause, strOrderBy, param.TargetBaps, param.TargetPower);
                else
                    data = repo.pGetNewBapsPaged(strWhereClause, strOrderBy, rowSkip, pageSize, param.TargetBaps, param.TargetPower);
            }

            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwRABapsSite { ErrorMessage = ex.Message });
            }
            return data;


        }
        public List<string> GetListNewBapsId(vwRABapsSite param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRABapsSiteRepository(context);
            List<string> ListId = new List<string>();

            List<vwRABapsSite> data = new List<vwRABapsSite>();

            try
            {
                string strWhereClause = " ";//" t.ID IS NULL AND ";

                // Modification Or Added By Ibnu Setiawan 29. January 2020 Add Filter MstBapsId and Change filter Grid using Like 
                if (!string.IsNullOrWhiteSpace(param.ListIdString))
                {
                    strWhereClause += "MstBapsID IN (" + param.ListIdString + ") AND ";
                }

                if (!string.IsNullOrWhiteSpace(param.CompanyInvoiceId))
                {
                    strWhereClause += "newBaps.CompanyInvoiceId = '" + param.CompanyInvoiceId + "' AND ";
                }

                if (!string.IsNullOrWhiteSpace(param.CustomerID))
                {
                    strWhereClause += "newBaps.CustomerID = '" + param.CustomerID + "' AND ";
                }
                else
                {
                    if (param.DepartmentType == RADepartmentTypeEnum.NonTSEL)
                    {
                        strWhereClause += "newBaps.CustomerID NOT LIKE 'TSEL%' AND ";
                    }
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
                    strWhereClause += "YEAR(BaukDone) = '" + param.YearBill + "' AND ";

                }
                if (param.MonthBill > 0)
                {
                    strWhereClause += "MONTH(BaukDone) = '" + param.MonthBill + "' AND ";
                }
                if (param.RegionID > 0)
                {
                    strWhereClause += "RegionID = '" + param.RegionID + "' AND ";
                }
                if (param.ProvinceID > 0)
                {
                    strWhereClause += "ProvinceID = '" + param.ProvinceID + "' AND ";
                }
                if (param.ProductID > 0)
                {
                    strWhereClause += "ProductID = '" + param.ProductID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    strWhereClause += "newBaps.SONumber LIKE '%" + param.SONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteID))
                {
                    strWhereClause += "SiteID LIKE '%" + param.SiteID + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteName))
                {
                    strWhereClause += "SiteName LIKE '%" + param.SiteName + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerSiteID))
                {
                    strWhereClause += "newBaps.CustomerSiteID LIKE '%" + param.CustomerSiteID + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerSiteName))
                {
                    strWhereClause += "newBaps.CustomerSiteName LIKE '%" + param.CustomerSiteName + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(param.RegionName))
                {
                    strWhereClause += "RegionName LIKE '%" + param.RegionName + "%' AND ";
                }
                if (param.StartInvoiceDate != null)
                {
                    strWhereClause += "BaukDone >= '" + param.StartInvoiceDate + "' AND ";
                }
                if (param.EndInvoiceDate != null)
                {
                    strWhereClause += "BaukDone <= '" + param.EndInvoiceDate + "' AND ";
                }
                if (param.StipSiro != null)
                {
                    if (param.StipSiro == 0)
                    {
                        strWhereClause += "(newBaps.StipSiro = '" + param.StipSiro + "' OR newBaps.StipSiro IS NULL) AND ";
                    }
                    else
                    {
                        strWhereClause += "newBaps.StipSiro = '" + param.StipSiro + "' AND ";
                    }
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                ListId = repo.GetListNewBapsId(param.TargetBaps, param.TargetPower, strWhereClause);
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwRABapsSite { ErrorMessage = ex.Message });
            }

            return ListId;

        }


        public int GetTrxTargetNewProductDataCount(vwRABapsSite param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxDashbordTSELDataRepo = new vwRABapsSiteRepository(context);
            List<vwRABapsSite> listDashboardTselData = new List<vwRABapsSite>();
            try
            {
                string strWhereClause = " t.ID IS NULL AND TBGHCMstDepartmentCode = '" + RADepartmentCodeTabEnum.NewProduct + "' AND ";

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
                else
                {
                    if (param.DepartmentType == RADepartmentTypeEnum.NonTSEL)
                    {
                        strWhereClause += "CustomerID NOT LIKE 'TSEL%' AND ";
                    }
                }

                if (param.SectionProductId > 0)
                {
                    strWhereClause += "SectionProductId = '" + param.SectionProductId + "' AND ";
                }
                if (param.SowProductId > 0)
                {
                    strWhereClause += "SowProductId = '" + param.SowProductId + "' AND ";
                }

                if (param.RegionID > 0)
                {
                    strWhereClause += "b.RegionID = '" + param.RegionID + "' AND ";
                }
                if (param.ProvinceID > 0)
                {
                    strWhereClause += "b.ProvinceID = '" + param.ProvinceID + "' AND ";
                }
                if (param.ProductID > 0)
                {
                    strWhereClause += "b.ProductID = '" + param.ProductID + "' AND ";
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
                return TrxDashbordTSELDataRepo.GetNewProductCount(strWhereClause, param.TargetBaps, param.TargetPower);
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }
        }
        public List<vwRABapsSite> GetTrxDashbordNewProductDataToList(vwRABapsSite param, int rowSkip, int pageSize, string strOrderBy = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwRABapsSite> data = new List<vwRABapsSite>();
            try
            {
                var repo = new vwRABapsSiteRepository(context);
                string strWhereClause = " t.ID IS NULL AND TBGHCMstDepartmentCode = '" + RADepartmentCodeTabEnum.NewProduct + "' AND ";

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
                else
                {
                    if (param.DepartmentType == RADepartmentTypeEnum.NonTSEL)
                    {
                        strWhereClause += "CustomerID NOT LIKE 'TSEL%' AND ";
                    }
                }
                if (param.SectionProductId > 0)
                {
                    strWhereClause += "SectionProductId = '" + param.SectionProductId + "' AND ";
                }
                if (param.SowProductId > 0)
                {
                    strWhereClause += "SowProductId = '" + param.SowProductId + "' AND ";
                }
                if (param.RegionID > 0)
                {
                    strWhereClause += "b.RegionID = '" + param.RegionID + "' AND ";
                }
                if (param.ProvinceID > 0)
                {
                    strWhereClause += "b.ProvinceID = '" + param.ProvinceID + "' AND ";
                }
                if (param.ProductID > 0)
                {
                    strWhereClause += "b.ProductID = '" + param.ProductID + "' AND ";
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
                    data = repo.GetNewProductList(strWhereClause, strOrderBy, param.TargetBaps, param.TargetPower);
                //data = GetPage(data, rowSkip, pageSize);
                else
                    data = repo.GetNewProductPaged(strWhereClause, strOrderBy, rowSkip, pageSize, param.TargetBaps, param.TargetPower);

            }

            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwRABapsSite { ErrorMessage = ex.Message });
            }
            return data;


        }
        public List<string> GetListNewProductId(vwRABapsSite param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRABapsSiteRepository(context);
            List<string> ListId = new List<string>();

            List<vwRABapsSite> data = new List<vwRABapsSite>();

            try
            {
                string strWhereClause = " t.ID IS NULL AND TBGHCMstDepartmentCode = '" + RADepartmentCodeTabEnum.NewProduct + "' AND ";

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
                else
                {
                    if (param.DepartmentType == RADepartmentTypeEnum.NonTSEL)
                    {
                        strWhereClause += "CustomerID NOT LIKE 'TSEL%' AND ";
                    }
                }
                if (param.SectionProductId > 0)
                {
                    strWhereClause += "SectionProductId = '" + param.SectionProductId + "' AND ";
                }
                if (param.SowProductId > 0)
                {
                    strWhereClause += "SowProductId = '" + param.SowProductId + "' AND ";
                }
                if (param.RegionID > 0)
                {
                    strWhereClause += "b.RegionID = '" + param.RegionID + "' AND ";
                }
                if (param.ProvinceID > 0)
                {
                    strWhereClause += "b.ProvinceID = '" + param.ProvinceID + "' AND ";
                }
                if (param.ProductID > 0)
                {
                    strWhereClause += "ProductID = '" + param.ProductID + "' AND ";
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
                ListId = repo.GetListNewProductId(param.TargetBaps, param.TargetPower, strWhereClause);
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwRABapsSite { ErrorMessage = ex.Message });
            }

            return ListId;

        }


        public List<mstDropdown> GetTenantTypeByOperator(string operatorID)
        {
            List<mstDropdown> result = new List<mstDropdown>();
            var command = _context.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.uspGetProductByOperator";
            command.Parameters.Add(command.CreateParameter("@vOperatorID", operatorID));
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

        #region upload excel

        public List<MstRATargetRecurring> UploadTargetRecurring(List<MstRATargetRecurring> bapsTarget, string departmentCode)
        {
            List<MstRATargetRecurring> invalidData = new List<MstRATargetRecurring>();
            try
            {
                string strWhereClause = " ";
                if (departmentCode == _departmentCodeTSEL) //TSEL
                {
                    strWhereClause += "CustomerID LIKE 'TSEL%'  ";
                    invalidData = _mstRATargetRecurringRepository.UploadTargetRecurring(bapsTarget, strWhereClause);

                }
                else if (departmentCode == _departmentCodeNonTSEL) //NONTSEL
                {
                    strWhereClause += "CustomerID NOT LIKE 'TSEL%'  ";
                    invalidData = _mstRATargetRecurringRepository.UploadTargetRecurring(bapsTarget, strWhereClause);

                }
                else if (departmentCode == _departmentCodeNewProduct) //NEW Product
                {
                    invalidData = _mstRATargetRecurringRepository.UploadTargetRecurringNewProduct(bapsTarget, strWhereClause);

                }
                return invalidData;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                invalidData.Add(new MstRATargetRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "UploadTargetRecurring", "")));
                return invalidData;
            }
        }
        public List<MstRATargetNewBaps> UploadTargetNewBaps(List<MstRATargetNewBaps> bapsTarget)
        {
            List<MstRATargetNewBaps> result = new List<MstRATargetNewBaps>();
            try
            {
                string whereClause = String.Empty;
                result = _mstRATargetNewBapsRepository.UploadTargetNewBaps(bapsTarget, whereClause);
                return result;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                result.Add(new MstRATargetNewBaps((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELService", "UploadTargetNewBaps", "")));
                return result;
            }
        }

        #endregion

        public vwRABapsSite GetHistoryRecurringInputTargetByID(long targetID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRABapsSiteRepository(context);
            var target = repo.GetReccuringHistoryInputTargetByPK(targetID);
            return target;

        }
        public vwRABapsSite GetHistoryNewBapsInputTargetByID(long targetID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRABapsSiteRepository(context);
            var target = repo.GetNewBapsHistoryInputTargetByPK(targetID);
            return target;

        }


        public MstRATargetRecurring UpdateHistoryRecurringInputTarget(vwRABapsSite inputTarget)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRABapsSiteRepository(context);
            MstRATargetRecurring result = new MstRATargetRecurring();
            try
            {
                DateTime dtNow = Helper.GetDateTimeNow();

                var target = repo.GetReccuringHistoryInputTargetByPK(inputTarget.TargetID.GetValueOrDefault());
                if (target != null)
                {
                    var _target = new MstRATargetRecurring();
                    _target.Month = inputTarget.TargetMonth;
                    _target.Year = inputTarget.TargetYear;
                    _target.ID = inputTarget.TargetID.GetValueOrDefault();
                    _target.StartInvoiceDate = inputTarget.StartInvoiceDate;
                    _target.EndInvoiceDate = inputTarget.EndInvoiceDate;
                    _target.AmountIDR = inputTarget.AmountIDR;
                    _target.AmountUSD = inputTarget.AmountUSD;

                    result = _mstRATargetRecurringRepository.Update(_target);
                }
                return result;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                result = (new MstRATargetRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InputTargetService", "UploadTargetNewBaps", "")));
                return result;
            }

        }

        public MstRATargetRecurring DeleteHistoryRecurringInputTarget(long targetID)
        {
            MstRATargetRecurring result = new MstRATargetRecurring();
            try
            {
                DateTime dtNow = Helper.GetDateTimeNow();

                MstRATargetRecurring target = _mstRATargetRecurringRepository.GetByPK(targetID);
                var res = _mstRATargetRecurringRepository.DeleteByPK(targetID);
                if (res)
                {
                    return target;
                }
                return result;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                result = (new MstRATargetRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InputTargetService", "UploadTargetNewBaps", "")));
                return result;
            }

        }

        public MstRATargetNewBaps UpdateHistoryNewBapsInputTarget(vwRABapsSite inputTarget)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRABapsSiteRepository(context);

            MstRATargetNewBaps result = new MstRATargetNewBaps();
            try
            {
                DateTime dtNow = Helper.GetDateTimeNow();

                var target = repo.GetNewBapsHistoryInputTargetByPK(inputTarget.TargetID.GetValueOrDefault());
                if (target != null)
                {
                    var _target = new MstRATargetNewBaps();
                    _target.Month = inputTarget.TargetMonth;
                    _target.Year = inputTarget.TargetYear;
                    _target.ID = inputTarget.TargetID.GetValueOrDefault();
                    _target.StartInvoiceDate = inputTarget.StartInvoiceDate;
                    _target.EndInvoiceDate = inputTarget.EndInvoiceDate;
                    _target.AmountIDR = inputTarget.AmountIDR;
                    _target.AmountUSD = inputTarget.AmountUSD;
                    result = _mstRATargetNewBapsRepository.UpdateTargetNewBaps(_target);
                }
                return result;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                result = (new MstRATargetNewBaps((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InputTargetService", "UploadTargetNewBaps", "")));
                return result;
            }

        }

        public MstRATargetNewBaps DeleteHistoryNewBapsInputTarget(long targetID)
        {
            MstRATargetNewBaps result = new MstRATargetNewBaps();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRABapsSiteRepository(context);
            try
            {
                DateTime dtNow = Helper.GetDateTimeNow();

                var target = repo.GetNewBapsHistoryInputTargetByPK(targetID);
                if (target != null)
                {
                    result.ID = target.TargetID.GetValueOrDefault();
                    var res = _mstRATargetNewBapsRepository.DeleteTargetNewBapsByPK(targetID);
                    if (res)
                    {
                        return result;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                result = (new MstRATargetNewBaps((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InputTargetService", "UploadTargetNewBaps", "")));
                return result;
            }

        }


        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
