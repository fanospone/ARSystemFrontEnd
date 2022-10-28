using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Models.ViewModels.ARSystem;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;

namespace ARSystem.Service.ARSystem.Invoice
{
    public class NonRevenueService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;

        private vwDataCreateInvoiceNonRevenueRepository _vwNonRevenueSiteRepo;
        private trxInvoiceNonRevenueRepository _trxInvoiceRepo;
        private trxInvoiceNonRevenueSiteRepository _trxInvoiceSiteRepo;
        private mstTaxInvoiceRepository _mstTaxRepo;
        private logInvoiceActivityRepository _logInvoiceActRepo;
        private logArActivityRepository _logArActRepo;
        private vwDataInvoiceNonRevenueRepository _vwDataNonRevenueRepo;

        public NonRevenueService() : base()
        {
            _context = this.SetContext();
            _dtNow = Helper.GetDateTimeNow();

            _vwNonRevenueSiteRepo = new vwDataCreateInvoiceNonRevenueRepository(_context);
            _trxInvoiceRepo = new trxInvoiceNonRevenueRepository(_context);
            _trxInvoiceSiteRepo = new trxInvoiceNonRevenueSiteRepository(_context);
            _mstTaxRepo = new mstTaxInvoiceRepository(_context);
            _logInvoiceActRepo = new logInvoiceActivityRepository(_context);
            _logArActRepo = new logArActivityRepository(_context);
            _vwDataNonRevenueRepo = new vwDataInvoiceNonRevenueRepository(_context);
        }

        #region Data
        public Datatable<vwDataInvoiceNonRevenue> GetDataNonRevenue(string UserID, vmCreateInvoiceNonRevenue filter)
        {
            Datatable<vwDataInvoiceNonRevenue> dataNonRevenue = new Datatable<vwDataInvoiceNonRevenue>();

            try
            {
                string whereClause = filterSite(filter);
                whereClause += " AND mstInvoiceStatusId IN (" + (int)StatusHelper.InvoiceStatus.StateCreated
                        + "," + (int)StatusHelper.InvoiceStatus.StatePosted
                        + "," + (int)StatusHelper.InvoiceStatus.StatePrinted
                        + "," + (int)StatusHelper.InvoiceStatus.StateRejectedByARCollection + ") ";

                dataNonRevenue.Count = _vwDataNonRevenueRepo.GetCount(whereClause);

                if (filter.length > 0)
                    dataNonRevenue.List = _vwDataNonRevenueRepo.GetPaged(whereClause, "", filter.start, filter.length);
                else
                    dataNonRevenue.List = _vwDataNonRevenueRepo.GetList(whereClause);

                return dataNonRevenue;
            }
            catch (Exception ex)
            {
                dataNonRevenue.List.Add(new vwDataInvoiceNonRevenue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return dataNonRevenue;
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
                    whereClause += " AND SONumber IN ('-X'";
                    foreach (var item in strSONumber)
                    {
                        whereClause += ",'" + item + "'";
                    }
                    whereClause += ")";
                }

                siteNonRevenue.Count = _vwNonRevenueSiteRepo.GetCount(whereClause);

                if (filter.length > 0)
                    siteNonRevenue.List = _vwNonRevenueSiteRepo.GetPaged(whereClause, "", filter.start, filter.length);
                else
                    siteNonRevenue.List = _vwNonRevenueSiteRepo.GetList(whereClause);

                return siteNonRevenue;
            }
            catch (Exception ex)
            {
                siteNonRevenue.List.Add(new vwDataCreateInvoiceNonRevenue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return siteNonRevenue;
            }
        }

        public Datatable<vwDataInvoiceNonRevenue> GetDataHistory(string UserID, vmCreateInvoiceNonRevenue filter)
        {
            Datatable<vwDataInvoiceNonRevenue> historyNonRevenue = new Datatable<vwDataInvoiceNonRevenue>();

            try
            {
                string whereClause = filterSite(filter);

                historyNonRevenue.Count = _vwDataNonRevenueRepo.GetCount(whereClause);

                if (filter.length > 0)
                    historyNonRevenue.List = _vwDataNonRevenueRepo.GetPaged(whereClause, "", filter.start, filter.length);
                else
                    historyNonRevenue.List = _vwDataNonRevenueRepo.GetList(whereClause);

                return historyNonRevenue;
            }
            catch (Exception ex)
            {
                historyNonRevenue.List.Add(new vwDataInvoiceNonRevenue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return historyNonRevenue;
            }
        }

        public List<trxInvoiceNonRevenueSite> GetSiteNonRevenueByID(string UserID, vmCreateInvoiceNonRevenue filter)
        {
            List<trxInvoiceNonRevenueSite> site = new List<trxInvoiceNonRevenueSite>();

            try
            {
                string whereClause = filterSite(filter);

                site = _trxInvoiceSiteRepo.GetList(whereClause);

                return site;
            }
            catch (Exception ex)
            {
                site.Add(new trxInvoiceNonRevenueSite((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return site;
            }
        }

        public List<int> GetListID(string UserID, vmCreateInvoiceNonRevenue filter)
        {
            List<int> ListID = new List<int>();

            try
            {
                string whereClause = filterSite(filter);

                ListID = _vwNonRevenueSiteRepo.GetListID(whereClause);
                return ListID;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID);
                return ListID;
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

                    siteCheckList = _vwNonRevenueSiteRepo.GetList(whereClause);
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
        #endregion

        #region Processing Data
        public trxInvoiceNonRevenue CreateInvoiceNonRevenue(string UserID, trxInvoiceNonRevenue data, List<trxInvoiceNonRevenueSite> dataList)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                string InvoiceNumber = Helper.GenerateInvoiceNumber(data.CompanyID, data.OperatorID, UserID, (int)StatusHelper.InvoiceCategory.NonRevenue);

                trxInvoiceNonRevenue trxInvoice = pCreateInvoice(UserID, data, InvoiceNumber);

                if (!data.IsPPN)
                {
                    mstTaxInvoice tax = new mstTaxInvoice();
                    tax.trxInvoiceNonRevenueID = trxInvoice.trxInvoiceNonRevenueID;
                    tax.TaxInvoiceNo = "TANPA PPN";
                    tax.InvNo = InvoiceNumber;
                    tax.TaxInvoiceDate = _dtNow.Date;
                    tax.CreatedBy = UserID;
                    tax.CreatedDate = _dtNow;
                    _mstTaxRepo.Create(tax);
                }

                List<trxInvoiceNonRevenueSite> trxInvoiceSite = pCreateInvoiceSite(UserID, dataList, trxInvoice.trxInvoiceNonRevenueID);

                #region 'INSERT InvoiceActivityLog, logArActivity'
                var logInvoiceActivity = new logInvoiceActivity();
                logInvoiceActivity.InvNo = InvoiceNumber;
                logInvoiceActivity.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.Create;//CREATE
                logInvoiceActivity.LogDate = _dtNow;
                logInvoiceActivity = _logInvoiceActRepo.Create(logInvoiceActivity);

                logArActivity logAr = new logArActivity();
                logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.Create;
                logAr.LogWeek = LogHelper.GetLogWeek(_dtNow);
                logAr.trxInvoiceNonRevenueID = trxInvoice.trxInvoiceNonRevenueID;
                logAr.CreatedBy = UserID;
                logAr.CreatedDate = Helper.GetDateTimeNow();
                _logArActRepo.Create(logAr);
                #endregion

                uow.SaveChanges();
                return trxInvoice;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxInvoiceNonRevenue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID));
            }
        }

        private trxInvoiceNonRevenue pCreateInvoice(string UserID, trxInvoiceNonRevenue data, string InvoiceNumber)
        {
            trxInvoiceNonRevenue trxInvoice = new trxInvoiceNonRevenue();
            trxInvoice.InvNo = InvoiceNumber;
            trxInvoice.CompanyID = data.CompanyID;
            trxInvoice.OperatorID = data.OperatorID;
            trxInvoice.Amount = data.Amount;
            trxInvoice.Discount = data.Discount;
            trxInvoice.DPP = data.DPP;
            trxInvoice.TotalPPN = data.TotalPPN;
            trxInvoice.TotalPPH = data.TotalPPH;
            trxInvoice.Penalty = data.Penalty;
            trxInvoice.InvoiceTotal = data.InvoiceTotal;
            trxInvoice.IsPPN = data.IsPPN;
            trxInvoice.IsPPH = data.IsPPH;
            trxInvoice.mstInvoiceCategoryId = (int)StatusHelper.InvoiceCategory.NonRevenue; //NON REVENUE
            trxInvoice.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateCreated; //STATE CREATED
            trxInvoice.InvoiceTypeId = "0003"; //YEARLY
            trxInvoice.Currency = "IDR";
            trxInvoice.CreatedDate = _dtNow;
            trxInvoice.CreatedBy = UserID;
            trxInvoice.IsPPHFinal = data.IsPPHFinal;
            trxInvoice.mstCategoryInvoiceID = data.mstCategoryInvoiceID;
            trxInvoice = _trxInvoiceRepo.Create(trxInvoice);

            return trxInvoice;
        }

        private List<trxInvoiceNonRevenueSite> pCreateInvoiceSite(string UserID, List<trxInvoiceNonRevenueSite> dataList, int trxInvoiceNonRevenueID)
        {
            var listInvoiceSite = new List<trxInvoiceNonRevenueSite>();
            foreach (var item in dataList)
            {
                listInvoiceSite.Add(new trxInvoiceNonRevenueSite
                {
                    trxInvoiceNonRevenueID = trxInvoiceNonRevenueID,
                    RowNumber = item.RowNumber,
                    SONumber = item.SONumber,
                    SiteID = item.SiteID,
                    SiteName = item.SiteName,
                    SiteIDCustomer = item.SiteIDCustomer,
                    SiteNameCustomer = item.SiteNameCustomer,
                    CompanyID = item.CompanyID,
                    OperatorID = item.OperatorID,
                    StartPeriod = item.StartPeriod,
                    EndPeriod = item.EndPeriod,
                    Amount = item.Amount,
                    CreatedDate = _dtNow,
                    CreatedBy = UserID
                });
            }

            listInvoiceSite = _trxInvoiceSiteRepo.CreateBulky(listInvoiceSite);

            return listInvoiceSite;
        }

        public trxInvoiceNonRevenue UpdateInvoiceNonRevenue(string UserID, trxInvoiceNonRevenue data, List<trxInvoiceNonRevenueSite> dataList, int trxInvoiceNonRevenueID)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {

                trxInvoiceNonRevenue trxInvoice = _trxInvoiceRepo.GetByPK(trxInvoiceNonRevenueID);
                trxInvoice.CompanyID = data.CompanyID;
                trxInvoice.OperatorID = data.OperatorID;
                trxInvoice.Amount = data.Amount;
                trxInvoice.Discount = data.Discount;
                trxInvoice.DPP = data.DPP;
                trxInvoice.TotalPPN = data.TotalPPN;
                trxInvoice.TotalPPH = data.TotalPPH;
                trxInvoice.Penalty = data.Penalty;
                trxInvoice.InvoiceTotal = data.InvoiceTotal;
                trxInvoice.IsPPN = data.IsPPN;
                trxInvoice.IsPPH = data.IsPPH;
                trxInvoice.InvSubject = data.InvSubject;
                trxInvoice.UpdatedDate = _dtNow;
                trxInvoice.UpdatedBy = UserID;
                trxInvoice.mstCategoryInvoiceID = data.mstCategoryInvoiceID;
                trxInvoice = _trxInvoiceRepo.Update(trxInvoice);

                _trxInvoiceSiteRepo.DeleteByFilter("trxInvoiceNonRevenueID = " + trxInvoiceNonRevenueID);

                List<trxInvoiceNonRevenueSite> trxInvoiceSite = pCreateInvoiceSite(UserID, dataList, trxInvoiceNonRevenueID);

                uow.SaveChanges();
                return trxInvoice;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxInvoiceNonRevenue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID));
            }
        }
        #endregion

        #region Helper
        private string filterSite(vmCreateInvoiceNonRevenue data)
        {
            var whereClause = "1=1";

            if (data.vCompany != null || !string.IsNullOrEmpty(data.vCompany))
            {
                whereClause += " AND CompanyID = '" + data.vCompany + "' ";
            }

            if (data.vOperator != null || !string.IsNullOrEmpty(data.vOperator))
            {
                whereClause += " AND OperatorID = '" + data.vOperator + "' ";
            }

            if (data.trxInvoiceNonRevenueID > 0)
            {
                whereClause += " AND trxInvoiceNonRevenueID = '" + data.trxInvoiceNonRevenueID + "' ";
            }

            if (data.vInvNo != null || !string.IsNullOrWhiteSpace(data.vInvNo))
            {
                whereClause += " AND InvNo LIKE '%" + data.vInvNo + "%' ";
            }

            return whereClause;
        }
        #endregion
    }
}
