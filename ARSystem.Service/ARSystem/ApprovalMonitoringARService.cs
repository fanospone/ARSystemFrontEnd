using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.ViewModels;
using System.Reflection;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGSAPIntegration;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGSAPIntegration;
using System.Globalization;

namespace ARSystem.Service.ARSystem
{
    public class ApprovalMonitoringARService : BaseService
    {
        private DbContext _context;
        private DbContext _contextSAP;
        private DateTime _dtNow;

        private uspReportCollectionMatchingARRepository _collMatchingARRepo;
        private stgTRStatusMatchingARRepository _stgTRMatchingARRepo;
        private trxInvoiceMatchingARRepository _trxMatchRepo;
        private vwstgTRStatusPenerimaanPembayaranRepository _vwstgStatusPenerimaanRepo;
        private vwReportInvoiceMatchingARRepository _vwReportInv;
        private uspMonitoringMatchingARFilterRepository _filterRepo;
        private stgTRStatusMatchingARLogRepository _stgTRMatchingARLogRepo;
        private vwMatchingARLogDocumentPaymentRepository _vwDocPaymentLogRepo;
        private stgTRStatusMatchingARLogErrorRepository _stgTRMatchingARLogErrorRepo;

        public ApprovalMonitoringARService() : base()
        {
            _context = this.SetContext();
            _contextSAP = new DbContext(Helper.GetConnection("TBIGSYSDB01_TBGSAPIntegration"));
            _dtNow = Helper.GetDateTimeNow();

            _collMatchingARRepo = new uspReportCollectionMatchingARRepository(_context);
            _stgTRMatchingARRepo = new stgTRStatusMatchingARRepository(_contextSAP);
            _trxMatchRepo = new trxInvoiceMatchingARRepository(_context);
            _vwstgStatusPenerimaanRepo = new vwstgTRStatusPenerimaanPembayaranRepository(_context);
            _vwReportInv = new vwReportInvoiceMatchingARRepository(_context);
            _filterRepo = new uspMonitoringMatchingARFilterRepository(_context);
            _stgTRMatchingARLogRepo = new stgTRStatusMatchingARLogRepository(_context);
            _vwDocPaymentLogRepo = new vwMatchingARLogDocumentPaymentRepository(_context);
            _stgTRMatchingARLogErrorRepo = new stgTRStatusMatchingARLogErrorRepository(_contextSAP);
        }

        #region Data
        public Datatable<vwReportInvoiceMatchingAR> GetDataInvoiceMatchingAR(string UserID, vmInvoiceMatchingAR filter)
        {
            Datatable<vwReportInvoiceMatchingAR> dataInvMatchingAR = new Datatable<vwReportInvoiceMatchingAR>();

            try
            {
                string whereClause = filterData(filter);

                dataInvMatchingAR.Count = _vwReportInv.GetCount(whereClause);

                if (filter.length > 0)
                    dataInvMatchingAR.List = _vwReportInv.GetPaged(whereClause, "CreatedDate DESC", filter.start, filter.length);
                else
                    dataInvMatchingAR.List = _vwReportInv.GetList(whereClause, "CreatedDate DESC");

                return dataInvMatchingAR;
            }
            catch (Exception ex)
            {
                dataInvMatchingAR.List.Add(new vwReportInvoiceMatchingAR((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return dataInvMatchingAR;
            }
        }

        public Datatable<vwtrxInvoiceMatchingAR> GetDataCollectionMatchingAR(string UserID, vmInvoiceMatchingAR filter)
        {
            Datatable<vwtrxInvoiceMatchingAR> dataInvMatchingAR = new Datatable<vwtrxInvoiceMatchingAR>();

            try
            {
                dataInvMatchingAR.List = _collMatchingARRepo.GetList(filter);

                return dataInvMatchingAR;
            }
            catch (Exception ex)
            {
                dataInvMatchingAR.List.Add(new vwtrxInvoiceMatchingAR((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return dataInvMatchingAR;
            }
        }

        public List<vwMatchingARLogDocumentPayment> GetDocumentPaymentLog(string UserID)
        {
            List<vwMatchingARLogDocumentPayment> docPayment = new List<vwMatchingARLogDocumentPayment>();

            try
            {
                docPayment = _vwDocPaymentLogRepo.GetList();

                return docPayment;
            }
            catch (Exception ex)
            {
                docPayment.Add(new vwMatchingARLogDocumentPayment((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return docPayment;
            }
        }

        public List<stgTRStatusPenerimaanPembayaran> GetDocumentPaymentList(string UserID)
        {
            List<stgTRStatusPenerimaanPembayaran> docPayment = new List<stgTRStatusPenerimaanPembayaran>();

            try
            {

                docPayment = _filterRepo.GetList();

                return docPayment;
            }
            catch (Exception ex)
            {
                docPayment.Add(new stgTRStatusPenerimaanPembayaran((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return docPayment;
            }
        }
        #endregion

        #region Processing Data
        public stgTRStatusMatchingAR GenerateToSAP(string UserID, List<stgTRStatusMatchingAR> dataList)
        {
            var uow = _context.CreateUnitOfWork();
            var result = new stgTRStatusMatchingAR();
            try
            {
                List<stgTRStatusMatchingAR> stgTRMatching = pCreateToStgSAPIntegration(UserID, dataList);

                uow.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new stgTRStatusMatchingAR((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID));
            }
        }

        private List<stgTRStatusMatchingAR> pCreateToStgSAPIntegration(string UserID, List<stgTRStatusMatchingAR> dataList)
        {
            var list = new List<stgTRStatusMatchingAR>();
            var listlog = new List<stgTRStatusMatchingARLog>();

            foreach (var item in dataList)
            {
                string FlagOthers = "";
                if (item.FlagOthers == "1")
                    FlagOthers = "X";

                list.Add(new stgTRStatusMatchingAR
                {
                    SourceSys = "ARSYS",
                    DocheaderText = item.DocheaderText,
                    Entrydate = item.Entrydate,
                    Entrytime = item.Entrytime,
                    CompanycodeInvoice = item.CompanycodeInvoice,
                    CustomerNumber = item.CustomerNumber,
                    CompanycodePayment = item.CompanycodePayment,
                    DocumentPayment = item.DocumentPayment,
                    Tanggaluangmasuk = item.Tanggaluangmasuk,
                    Currency = item.Currency,
                    TotalPayment = item.TotalPayment,
                    NilaiInvoice = item.NilaiInvoice,
                    PaidAmount = item.PaidAmount,
                    PphAmount = item.PphAmount,
                    Rounding = item.Rounding,
                    Wapu = item.Wapu,
                    Rtgs = item.Rtgs,
                    Penalty = item.Penalty,
                    PpnExpired = item.PpnExpired,
                    Status = item.Status,
                    CreatedDate = _dtNow,
                    FlagOthers = FlagOthers
                });

                listlog.Add(new stgTRStatusMatchingARLog
                {
                    SourceSys = "ARSYS",
                    DocheaderText = item.DocheaderText,
                    Entrydate = item.Entrydate,
                    Entrytime = item.Entrytime,
                    CompanycodeInvoice = item.CompanycodeInvoice,
                    CustomerNumber = item.CustomerNumber,
                    CompanycodePayment = item.CompanycodePayment,
                    DocumentPayment = item.DocumentPayment,
                    Tanggaluangmasuk = item.Tanggaluangmasuk,
                    Currency = item.Currency,
                    TotalPayment = item.TotalPayment,
                    NilaiInvoice = item.NilaiInvoice,
                    PaidAmount = item.PaidAmount,
                    PphAmount = item.PphAmount,
                    Rounding = item.Rounding,
                    Wapu = item.Wapu,
                    Rtgs = item.Rtgs,
                    Penalty = item.Penalty,
                    PpnExpired = item.PpnExpired,
                    Status = item.Status,
                    CreatedDate = _dtNow,
                    FlagOthers = FlagOthers
                });

                if (item.Status != "ERROR")
                {
                    var matchingAR = new trxInvoiceMatchingAR();

                    if (item.trxMatchingARID != null || item.trxMatchingARID > 0)
                    {
                        matchingAR = _trxMatchRepo.GetByPK(Convert.ToInt64(item.trxMatchingARID));
                        matchingAR.Status = (int)StatusHelper.StatusCollectionToSAP.Posted;
                        _trxMatchRepo.Update(matchingAR);
                    }
                    else
                    {
                        string whereClause = trxWhereClause(item.RekeningKoranid, item.DocumentPayment, item.CompanycodePayment, item.Tanggaluangmasuk);
                        matchingAR = _trxMatchRepo.GetList(whereClause).FirstOrDefault();
                        if (matchingAR != null)
                        {
                            matchingAR.Status = (int)StatusHelper.StatusCollectionToSAP.Posted;
                            _trxMatchRepo.Update(matchingAR);
                        }
                        else
                        {
                            var trxMatchingAR = new trxInvoiceMatchingAR();
                            trxMatchingAR.RekeningKoranID = item.RekeningKoranid;
                            trxMatchingAR.DocumentPayment = item.DocumentPayment;
                            trxMatchingAR.CompanyCode = item.CompanycodePayment;
                            trxMatchingAR.TanggalUangMasuk = item.Tanggaluangmasuk;
                            trxMatchingAR.InsertDate = Convert.ToDateTime(_dtNow.ToString("dd-MM-yyyy"));
                            trxMatchingAR.InsertTime = _dtNow.ToString("hh:mm:ss");
                            trxMatchingAR.Status = (int)StatusHelper.StatusCollectionToSAP.Posted;
                            trxMatchingAR.CreatedDate = _dtNow;
                            trxMatchingAR.CreatedBy = UserID;
                            _trxMatchRepo.Create(trxMatchingAR);
                        }
                    }
                }
                else
                {
                    string whereClause = WhereClauseLogError(item.DocheaderText, item.Entrydate, item.Entrytime, item.CompanycodePayment,
                        item.DocumentPayment, item.Tanggaluangmasuk, item.PaidAmount);
                    _stgTRMatchingARLogErrorRepo.DeleteByFilter(whereClause);
                }                
            }

            _stgTRMatchingARLogRepo.CreateBulky(listlog);
            list = _stgTRMatchingARRepo.CreateBulky(list);

            return list;
        }

        public trxInvoiceMatchingAR UpdateDocumentPaymentOther(string UserID, int ID, vwMatchingARLogDocumentPayment data)
        {
            var uow = _context.CreateUnitOfWork();
            var result = new trxInvoiceMatchingAR();
            try
            {
                trxInvoiceMatchingAR trxMatch = _trxMatchRepo.GetByPK(ID);
                trxMatch.RekeningKoranID = data.Rekeningkoranid;
                trxMatch.DocumentPayment = data.DocumentPayment;
                trxMatch.CompanyCode = data.Companycode;
                trxMatch.TanggalUangMasuk = data.Tanggaluangmasuk;
                trxMatch.IsOtherRevision = 1;
                trxMatch.PaymentDate = DateTime.ParseExact(data.Tanggaluangmasuk, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                trxMatch = _trxMatchRepo.Update(trxMatch);

                uow.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxInvoiceMatchingAR((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID));
            }
        }
        #endregion

        #region Helper
        private string filterData(vmInvoiceMatchingAR data)
        {
            var whereClause = "1=1";

            if (data.vStartPaid != null || !string.IsNullOrWhiteSpace(data.vStartPaid) || data.vEndPaid != null || !string.IsNullOrWhiteSpace(data.vEndPaid))
            {
                whereClause += " AND InvPaidDate BETWEEN '" + data.vStartPaid + "' AND '" + data.vEndPaid + "' ";
            }

            if (data.vCompanyID != null || !string.IsNullOrEmpty(data.vCompanyID))
            {
                whereClause += " AND CompanyCodeInvoice = '" + data.vCompanyID + "' ";
            }

            if (data.vInvoiceNo != null || !string.IsNullOrEmpty(data.vInvoiceNo))
            {
                whereClause += " AND InvoiceNumber = '" + data.vInvoiceNo + "' ";
            }

            if (data.vDocumentPayment != null || !string.IsNullOrEmpty(data.vDocumentPayment))
            {
                whereClause += " AND DocumentPayment = '" + data.vDocumentPayment + "' ";
            }

            return whereClause;
        }

        private string trxWhereClause(string RekeningKoranid, string DocumentPayment, string CompanyCode, string Tanggaluangmasuk)
        {
            var whereClause = "1=1";

            whereClause += " AND Status = 0";

            if (RekeningKoranid != null || !string.IsNullOrEmpty(RekeningKoranid))
            {
                whereClause += " AND RekeningKoranid = '" + RekeningKoranid + "' ";
            }

            if (DocumentPayment != null || !string.IsNullOrEmpty(DocumentPayment))
            {
                whereClause += " AND DocumentPayment = '" + DocumentPayment + "' ";
            }

            if (CompanyCode != null || !string.IsNullOrEmpty(CompanyCode))
            {
                whereClause += " AND CompanyCode = '" + CompanyCode + "' ";
            }

            if (Tanggaluangmasuk != null || !string.IsNullOrEmpty(Tanggaluangmasuk))
            {
                whereClause += " AND TanggalUangMasuk = '" + Tanggaluangmasuk + "' ";
            }

            return whereClause;
        }

        private string WhereClauseLogError(string DocheaderText, string Entrydate, string Entrytime, string CompanycodePayment,
            string DocumentPayment, string Tanggaluangmasuk, decimal? PaidAmount)
        {
            var whereClause = "1=1";

            if (DocheaderText != null || !string.IsNullOrEmpty(DocheaderText))
            {
                whereClause += " AND DocheaderText = '" + DocheaderText + "' ";
            }

            if (Entrydate != null || !string.IsNullOrEmpty(Entrydate))
            {
                whereClause += " AND Entrydate = '" + Entrydate + "' ";
            }

            if (Entrytime != null || !string.IsNullOrEmpty(Entrytime))
            {
                whereClause += " AND Entrytime = '" + Entrytime + "' ";
            }

            if (CompanycodePayment != null || !string.IsNullOrEmpty(CompanycodePayment))
            {
                whereClause += " AND CompanycodePayment = '" + CompanycodePayment + "' ";
            }

            if (DocumentPayment != null || !string.IsNullOrEmpty(DocumentPayment))
            {
                whereClause += " AND DocumentPayment = '" + DocumentPayment + "' ";
            }

            if (Tanggaluangmasuk != null || !string.IsNullOrEmpty(Tanggaluangmasuk))
            {
                whereClause += " AND Tanggaluangmasuk = '" + Tanggaluangmasuk + "' ";
            }

            if (PaidAmount != null || PaidAmount > 0)
            {
                whereClause += " AND PaidAmount = '" + PaidAmount + "' ";
            }

            return whereClause;
        }
        #endregion
    }
}
