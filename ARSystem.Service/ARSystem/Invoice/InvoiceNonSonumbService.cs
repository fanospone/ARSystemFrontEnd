using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.ViewModels.ARSystem;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Service.ARSystem.Invoice
{
    public class InvoiceNonSonumbService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;
        private readonly mstCategoryInvoiceRepository _mstCategoryInvoice;
        private readonly mstParameterRepository _mstTax;
        private readonly vwMstSonumbNonTowerRepository _vwmstSonumbNonTower;

        public InvoiceNonSonumbService()
        {
            _context = this.SetContext();
            _dtNow = Helper.GetDateTimeNow();

            _mstCategoryInvoice = new mstCategoryInvoiceRepository(_context);
            _vwmstSonumbNonTower = new vwMstSonumbNonTowerRepository(_context);
            _mstTax = new mstParameterRepository(_context);
        }

        public mstParameter GetTaxSingle(string userID)
        {
            try
            {
                return _mstTax.GetList(@"parameterType='PPN' AND parameterKey like 'PPN%' 
                        AND StartDate <= CONVERT(date, GETDATE()) 
                        AND CONVERT(DATE,EndDate) >= CONVERT(DATE, GETDATE())").FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new mstParameter((int)Helper.ErrorType.Error, Helper.logError(ex.Message, "InvoiceNonSonumbService", "GetTaxSingle", userID));
            }
        }

        public List<mstCategoryInvoice> GetCategoryInvoiceList(string userID)
        {
            try
            {
                return _mstCategoryInvoice.GetList();
            }
            catch (Exception ex)
            {
                return new List<mstCategoryInvoice>() { new mstCategoryInvoice((int)Helper.ErrorType.Error, Helper.logError(ex.Message, "InvoiceNonSonumbService", "GetCategoryInvoiceList", userID)) };
            }
        }

        public Datatable<vwDataCreateInvoiceNonRevenue> GetDataSiteNonRevenue(string UserID, vmCreateInvoiceNonRevenue filter, List<string> strSONumber)
        {
            Datatable<vwDataCreateInvoiceNonRevenue> siteNonRevenue = new Datatable<vwDataCreateInvoiceNonRevenue>();

            try
            {
                string whereClause = filterSite(filter);

                if (strSONumber.Count > 0 && strSONumber[0] != null)
                {
                    whereClause += " AND SoNumb IN ('-X'";
                    foreach (var item in strSONumber)
                    {
                        whereClause += ",'" + item + "'";
                    }
                    whereClause += ")";
                }

                siteNonRevenue.Count = _vwmstSonumbNonTower.GetCount(whereClause);

                if (filter.length > 0)
                    siteNonRevenue.List = _vwmstSonumbNonTower.GetPaged(whereClause, "", filter.start, filter.length)
                        .Select(x => new vwDataCreateInvoiceNonRevenue()
                        {
                            RowNumber = x.RowNumber.GetValueOrDefault(),
                            SoNumber = x.SoNumb,
                            SiteID = x.SiteID,
                            SiteName = x.SiteName,
                            SiteIDCustomer = x.SiteID,
                            SiteNameCustomer = x.SiteName,
                            CompanyID = x.CompanyCode,
                            OperatorID = x.OperatorID,
                            StartPeriod = string.Empty,
                            EndPeriod = string.Empty,
                            Amount = 0
                        }).ToList();
                else
                    siteNonRevenue.List = _vwmstSonumbNonTower.GetList(whereClause)
                        .Select(x => new vwDataCreateInvoiceNonRevenue()
                        {
                            RowNumber = x.RowNumber.GetValueOrDefault(),
                            SoNumber = x.SoNumb,
                            SiteID = x.SiteID,
                            SiteName = x.SiteName,
                            SiteIDCustomer = x.SiteID,
                            SiteNameCustomer = x.SiteName,
                            CompanyID = x.CompanyCode,
                            OperatorID = x.OperatorID,
                            StartPeriod = string.Empty,
                            EndPeriod = string.Empty,
                            Amount = 0
                        }).ToList();

                return siteNonRevenue;
            }
            catch (Exception ex)
            {
                siteNonRevenue.List.Add(new vwDataCreateInvoiceNonRevenue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return siteNonRevenue;
            }
        }

        public List<vwDataCreateInvoiceNonRevenue> GetSiteCheckList(string UserID, List<int> ListID)
        {
            List<vwDataCreateInvoiceNonRevenue> siteCheckList = new List<vwDataCreateInvoiceNonRevenue>();

            try
            {
                string whereClause = "";
                if (ListID.Count() != 0)
                {
                    string listId = string.Empty;

                    foreach (int RowNumber in ListID)
                    {
                        if (listId == string.Empty)
                            listId = "'" + RowNumber.ToString() + "'";
                        else
                            listId += ",'" + RowNumber.ToString() + "'";
                    }
                    whereClause += "RowNumber IN (" + listId + ") ";

                    siteCheckList = _vwmstSonumbNonTower.GetList(whereClause)
                        .Select(x => new vwDataCreateInvoiceNonRevenue()
                        {
                            RowNumber = x.RowNumber.GetValueOrDefault(),
                            SoNumber = x.SoNumb,
                            SiteID = x.SiteID,
                            SiteName = x.SiteName,
                            SiteIDCustomer = x.SiteID,
                            SiteNameCustomer = x.SiteName,
                            CompanyID = x.CompanyCode,
                            OperatorID = x.OperatorID,
                            StartPeriod = string.Empty,
                            EndPeriod = string.Empty,
                            Amount = 0
                        }).ToList();
                }
                return siteCheckList;
            }
            catch (Exception ex)
            {
                siteCheckList.Add(new vwDataCreateInvoiceNonRevenue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return siteCheckList;
            }
        }

        private string filterSite(vmCreateInvoiceNonRevenue data)
        {
            var whereClause = "1=1";

            if (data.vCompany != null || !string.IsNullOrEmpty(data.vCompany))
            {
                whereClause += " AND CompanyCode = '" + data.vCompany + "' ";
            }

            if (data.vOperator != null || !string.IsNullOrEmpty(data.vOperator))
            {
                whereClause += " AND OperatorID = '" + data.vOperator + "' ";
            }

            if (data.CategoryInvoiceID != 0)
            {
                whereClause += $" AND mstCategoryInvoiceID={data.CategoryInvoiceID}";
            }

            return whereClause;
        }
    }
}
