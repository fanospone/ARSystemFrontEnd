using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGSAPIntegration;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGSAPIntegration;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.Models.ViewModels.Datatable;
using System.Reflection;

namespace ARSystem.Service.ARSystem.Invoice
{
    public class trxARPaymentInvoiceTowerService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;

        private vwARPaymentInvoiceTowerRepository _vwARPaymentRepo;
        private vwInvoiceTowerDetailRepository _vwInvTowerDetailRepo;
        private vwARPaymentInvoiceTowerHistoryRepository _vwARPaymentHistoryRepo;
        private trxInvoiceHeaderRepository _trxInvHeaderRepo;
        private trxInvoiceHeaderRemainingAmountRepository _trxInvHeaderRemainRepo;
        private trxInvoicePaymentRepository _trxInvPaymentRepo;
        private logArActivityRepository _logARActRepo;
        private trxInvoiceNonRevenueRepository _trxInvNonRevenueRepo;
        private vwstgTRStatusPenerimaanPembayaranRepository _stgStatusPenerimaanRepo;
        private trxInvoiceMatchingARRepository _trxInvMatchingRepo;

        public trxARPaymentInvoiceTowerService() : base()
        {
            _context = this.SetContext();
            _dtNow = Helper.GetDateTimeNow();

            _vwARPaymentRepo = new vwARPaymentInvoiceTowerRepository(_context);
            _vwInvTowerDetailRepo = new vwInvoiceTowerDetailRepository(_context);
            _vwARPaymentHistoryRepo = new vwARPaymentInvoiceTowerHistoryRepository(_context);
            _trxInvHeaderRepo = new trxInvoiceHeaderRepository(_context);
            _trxInvHeaderRemainRepo = new trxInvoiceHeaderRemainingAmountRepository(_context);
            _trxInvPaymentRepo = new trxInvoicePaymentRepository(_context);
            _logARActRepo = new logArActivityRepository(_context);
            _trxInvNonRevenueRepo = new trxInvoiceNonRevenueRepository(_context);
            _stgStatusPenerimaanRepo = new vwstgTRStatusPenerimaanPembayaranRepository(_context);
            _trxInvMatchingRepo = new trxInvoiceMatchingARRepository(_context);
        }

        public int GetARPaymentInvoiceTowerCount(string userID, string term, string invOperatorId, string invCompanyId, string invNo)
        {
            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(term))
                {
                    strWhereClause += "InvoiceTypeId = '" + term + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(invOperatorId))
                {
                    strWhereClause += "invOperatorId = '" + invOperatorId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(invCompanyId))
                {
                    strWhereClause += "invCompanyId = '" + invCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(invNo))
                {
                    strWhereClause += "InvNo LIKE '%" + invNo + "%' AND ";
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return _vwARPaymentRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxARPaymentInvoiceTowerService", "GetARPaymentInvoiceTowerCount", userID);
                return 0;
            }
        }

        public List<vmARPaymentInvoiceTower> GetARPaymentInvoiceTowerToList(string userID, string term, string invOperatorId, string invCompanyId, string invNo, string orderBy, int rowSkip = 0, int pageSize = 10)
        {
            List<vwARPaymentInvoiceTower> listView = new List<vwARPaymentInvoiceTower>();
            List<vmARPaymentInvoiceTower> list = new List<vmARPaymentInvoiceTower>();

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(term))
                {
                    strWhereClause += "InvoiceTypeId = '" + term + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(invOperatorId))
                {
                    strWhereClause += "invOperatorId = '" + invOperatorId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(invCompanyId))
                {
                    strWhereClause += "invCompanyId = '" + invCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(invNo))
                {
                    strWhereClause += "InvNo LIKE '%" + invNo + "%' AND ";
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = "UpdatedDate DESC";
                }

                if (pageSize > 0)
                    listView = _vwARPaymentRepo.GetPaged(strWhereClause, orderBy, rowSkip, pageSize);
                else
                    listView = _vwARPaymentRepo.GetList(strWhereClause, orderBy);

                vmARPaymentInvoiceTower temp;
                string where = string.Empty;
                foreach (vwARPaymentInvoiceTower vw in listView)
                {
                    where = "trxInvoiceHeaderID = " + vw.trxInvoiceHeaderID + " AND mstInvoiceCategoryId = " + vw.mstInvoiceCategoryId;
                    temp = new vmARPaymentInvoiceTower();
                    temp.trxInvoiceHeaderID = vw.trxInvoiceHeaderID;
                    temp.InvNo = vw.InvNo;
                    temp.InvTemp = vw.InvTemp;
                    temp.InvPrintDate = vw.InvPrintDate;
                    temp.Term = vw.Term;
                    temp.InvSumADPP = vw.InvSumADPP;
                    temp.InvTotalAmount = vw.InvTotalAmount;
                    temp.Discount = vw.Discount;
                    temp.InvTotalAPPN = vw.InvTotalAPPN;
                    temp.InvTotalAPPH = vw.InvTotalAPPH;
                    temp.InvTotalPenalty = vw.InvTotalPenalty;
                    temp.InvReceiptDate = vw.InvReceiptDate;
                    temp.Currency = vw.Currency;
                    temp.AgingDays = vw.AgingDays;
                    temp.InvInternalPIC = vw.InvInternalPIC;
                    temp.PaidStatus = vw.PaidStatus;
                    temp.InvReceiptFile = vw.InvReceiptFile;
                    temp.ChecklistDate = vw.ChecklistDate;
                    temp.mstInvoiceStatusId = vw.mstInvoiceStatusId;
                    temp.mstInvoiceCategoryId = vw.mstInvoiceCategoryId;
                    temp.InvOperatorID = vw.InvOperatorID;
                    temp.InvCompanyId = vw.InvCompanyId;
                    temp.Company = vw.Company;
                    temp.InvoiceTypeId = vw.InvoiceTypeId;
                    temp.PartialPaid = vw.PartialPaid;
                    temp.FilePath = vw.FilePath;
                    temp.ContentType = vw.ContentType;
                    temp.PPHIndex = vw.PPHIndex;
                    temp.PPEIndex = vw.PPEIndex;
                    temp.isPPHFinal = vw.IsPPHFinal;
                    temp.PPHType = vw.PPHType;
                    temp.PPFIndex = vw.PPFIndex;
                    temp.InvAmountLossPPN = vw.SUMALossPPN;
                    temp.CompanyCode = vw.CompanyCode;
                    temp.Detail = _vwInvTowerDetailRepo.GetList(where);
                    temp.History = _vwARPaymentHistoryRepo.GetList(where, "BatchCode");
                    list.Add(temp);

                }
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vmARPaymentInvoiceTower((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxARPaymentInvoiceTowerService", "GetARPaymentInvoiceTowerToList", userID)));
                return list;
            }
        }

        public trxInvoiceHeader SavePayment(string userID, vmARPaymentInvoiceTower mdl)
        {
            var uow = _context.CreateUnitOfWork();

            logArActivity logAr;
            trxInvoicePayment InvoicePayment;
            string PaidStatus = Constants.PaidStatus[mdl.InvPaidStatus].ToString();

            decimal[] PaymentCodeValue = new decimal[] {
                mdl.DBT.Value,
                mdl.PAM.Value,
                mdl.PNT.Value,
                mdl.PPE.Value,
                mdl.PPH.Value,
                mdl.RTGS.Value,
                mdl.RND.Value,
                mdl.PAT.Value,
                mdl.PPF.Value
            };

            try
            {
                int batchNumber = 1;
                if (mdl.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                {
                    //UPDATE Invoice Header
                    trxInvoiceHeader InvoiceHeader = new trxInvoiceHeader();
                    InvoiceHeader = _trxInvHeaderRepo.GetByPK(mdl.trxInvoiceHeaderID);
                    InvoiceHeader.InvPaidDate = mdl.InvPaidDate;
                    InvoiceHeader.InvIDPaymentBank = mdl.mstPaymentId;
                    InvoiceHeader.InvPaidStatus = PaidStatus;
                    InvoiceHeader.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateInvoicePaid; //validate wheter invoice has been paid off or not if paid off set status
                    InvoiceHeader.InvAPaid = (InvoiceHeader.InvAPaid.HasValue) ? (InvoiceHeader.InvAPaid.Value + mdl.ARTotalPaid) : mdl.ARTotalPaid; //will shown as Partial Paid Field 
                    InvoiceHeader.UpdatedBy = userID;
                    InvoiceHeader.UpdatedDate = _dtNow;
                    InvoiceHeader.IsPPHFinal = mdl.isPPHFinal;
                    InvoiceHeader = _trxInvHeaderRepo.Update(InvoiceHeader);
                    List<string> listPaymentCode = new List<string>();
                    listPaymentCode = Helper.GenerateVoucherNumber(InvoiceHeader.InvCompanyInvoice, InvoiceHeader.InvOperatorID, userID);

                    trxInvoicePayment tempPayment = _trxInvPaymentRepo.GetList("trxInvoiceHeaderID = " + InvoiceHeader.trxInvoiceHeaderID, "BatchNumber DESC").FirstOrDefault();
                    if (tempPayment != null)
                    {
                        batchNumber = tempPayment.BatchNumber.Value + 1;
                    }
                    for (int i = 0; i < listPaymentCode.Count(); i++)
                    {
                        //INSERT trxInvoicePayment
                        InvoicePayment = new trxInvoicePayment();
                        InvoicePayment.mstPaymentCodeId = i + 1;
                        InvoicePayment.VoucherNumber = listPaymentCode[i];
                        InvoicePayment.trxInvoiceHeaderId = InvoiceHeader.trxInvoiceHeaderID;
                        InvoicePayment.Amount = PaymentCodeValue[i];
                        InvoicePayment.CreatedBy = userID;
                        InvoicePayment.CreatedDate = _dtNow;
                        InvoicePayment.BatchNumber = batchNumber;
                        InvoicePayment.mstPaymentBankID = mdl.mstPaymentId;
                        InvoicePayment.PaymentDate = mdl.InvPaidDate;
                        InvoicePayment = _trxInvPaymentRepo.Create(InvoicePayment);
                    }
                    //INSERT LogArActivity
                    logAr = new logArActivity();
                    logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.PayInvoice;
                    logAr.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                    logAr.trxInvoiceHeaderID = InvoiceHeader.trxInvoiceHeaderID;
                    logAr.CreatedBy = userID;
                    logAr.CreatedDate = _dtNow;
                    _logARActRepo.Create(logAr);

                    //INSERT trxInvoiceMatchingAR
                    trxInvoiceMatchingAR matchingAR = new trxInvoiceMatchingAR();
                    matchingAR.RekeningKoranID = mdl.InvoiceMatchingAR.RekeningKoranID;
                    matchingAR.DocumentPayment = mdl.InvoiceMatchingAR.DocumentPayment;
                    matchingAR.CompanyCode = mdl.InvoiceMatchingAR.CompanyCode;
                    matchingAR.TanggalUangMasuk = mdl.InvoiceMatchingAR.TanggalUangMasuk;
                    matchingAR.InsertDate = _dtNow;
                    matchingAR.InsertTime = _dtNow.ToString("hh:mm:ss");
                    matchingAR.trxInvoiceHeaderID = InvoiceHeader.trxInvoiceHeaderID;
                    matchingAR.BatchNumber = batchNumber;
                    matchingAR.PaymentDate = mdl.InvPaidDate;
                    matchingAR.Status = (int)StatusHelper.StatusCollectionToSAP.NotYet;
                    matchingAR.CreatedDate = _dtNow;
                    matchingAR.CreatedBy = userID;
                    _trxInvMatchingRepo.Create(matchingAR);

                }
                else if (mdl.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower10 || mdl.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower15)
                {
                    //UPDATE Invoice Header
                    trxInvoiceHeaderRemainingAmount InvoiceHeaderRemaining = new trxInvoiceHeaderRemainingAmount();
                    InvoiceHeaderRemaining = _trxInvHeaderRemainRepo.GetByPK(mdl.trxInvoiceHeaderID);
                    InvoiceHeaderRemaining.InvPaidDate = mdl.InvPaidDate;
                    InvoiceHeaderRemaining.InvIDPaymentBank = mdl.mstPaymentId;
                    InvoiceHeaderRemaining.InvPaidStatus = PaidStatus;
                    InvoiceHeaderRemaining.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateInvoicePaid; //validate wheter invoice has been paid off or not if paid off set status
                    InvoiceHeaderRemaining.InvAPaid = (InvoiceHeaderRemaining.InvAPaid.HasValue) ? (InvoiceHeaderRemaining.InvAPaid.Value + mdl.ARTotalPaid) : mdl.ARTotalPaid; //will shown as Partial Paid Field 
                    InvoiceHeaderRemaining.UpdatedBy = userID;
                    InvoiceHeaderRemaining.UpdatedDate = Helper.GetDateTimeNow();
                    InvoiceHeaderRemaining.IsPPHFinal = mdl.isPPHFinal;
                    InvoiceHeaderRemaining = _trxInvHeaderRemainRepo.Update(InvoiceHeaderRemaining);
                    List<string> listPaymentCode = new List<string>();
                    listPaymentCode = Helper.GenerateVoucherNumber(InvoiceHeaderRemaining.InvCompanyInvoice, InvoiceHeaderRemaining.InvOperatorID, userID);

                    trxInvoicePayment tempPayment = _trxInvPaymentRepo.GetList("trxInvoiceHeaderRemainingAmountID = " + InvoiceHeaderRemaining.trxInvoiceHeaderRemainingAmountID, "BatchNumber DESC").FirstOrDefault();
                    if (tempPayment != null)
                    {
                        batchNumber = tempPayment.BatchNumber.Value + 1;
                    }
                    for (int i = 0; i < listPaymentCode.Count(); i++)
                    {
                        //INSERT trxInvoicePayment
                        InvoicePayment = new trxInvoicePayment();
                        InvoicePayment.mstPaymentCodeId = i + 1;
                        InvoicePayment.VoucherNumber = listPaymentCode[i];
                        InvoicePayment.trxInvoiceHeaderRemainingAmountId = InvoiceHeaderRemaining.trxInvoiceHeaderRemainingAmountID;
                        InvoicePayment.Amount = PaymentCodeValue[i];
                        InvoicePayment.CreatedBy = userID;
                        InvoicePayment.CreatedDate = Helper.GetDateTimeNow();
                        InvoicePayment.BatchNumber = batchNumber;
                        InvoicePayment.mstPaymentBankID = mdl.mstPaymentId;
                        InvoicePayment.PaymentDate = mdl.InvPaidDate;
                        InvoicePayment = _trxInvPaymentRepo.Create(InvoicePayment);
                    }
                    //INSERT LogArActivity
                    logAr = new logArActivity();
                    logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.PayInvoice;
                    logAr.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                    logAr.trxInvoiceHeaderRemainingAmountID = InvoiceHeaderRemaining.trxInvoiceHeaderRemainingAmountID;
                    logAr.CreatedBy = userID;
                    logAr.CreatedDate = Helper.GetDateTimeNow();
                    _logARActRepo.Create(logAr);

                    //INSERT trxInvoiceMatchingAR
                    //trxInvoiceMatchingAR matchingAR = new trxInvoiceMatchingAR();
                    //matchingAR.RekeningKoranID = mdl.InvoiceMatchingAR.RekeningKoranID;
                    //matchingAR.DocumentPayment = mdl.InvoiceMatchingAR.DocumentPayment;
                    //matchingAR.CompanyCode = mdl.InvoiceMatchingAR.CompanyCode;
                    //matchingAR.TanggalUangMasuk = mdl.InvoiceMatchingAR.TanggalUangMasuk;
                    //matchingAR.InsertDate = _dtNow;
                    //matchingAR.InsertTime = _dtNow.ToString("hh:mm:ss");
                    //matchingAR.trxInvoiceHeaderRemainingAmountID = InvoiceHeaderRemaining.trxInvoiceHeaderRemainingAmountID;
                    //matchingAR.BatchNumber = batchNumber;
                    //matchingAR.PaymentDate = mdl.InvPaidDate;
                    //matchingAR.Status = (int)StatusHelper.StatusCollectionToSAP.NotYet;
                    //matchingAR.CreatedDate = _dtNow;
                    //matchingAR.CreatedBy = userID;
                    //_trxInvMatchingRepo.Create(matchingAR);
                }
                else
                {
                    //UPDATE Invoice Non Revenue
                    trxInvoiceNonRevenue InvoiceNonRevenue = new trxInvoiceNonRevenue();
                    InvoiceNonRevenue = _trxInvNonRevenueRepo.GetByPK(mdl.trxInvoiceHeaderID);
                    InvoiceNonRevenue.InvPaidDate = mdl.InvPaidDate;
                    InvoiceNonRevenue.InvIDPaymentBank = mdl.mstPaymentId;
                    InvoiceNonRevenue.InvPaidStatus = PaidStatus;
                    InvoiceNonRevenue.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateInvoicePaid; //validate wheter invoice has been paid off or not if paid off set status
                    InvoiceNonRevenue.InvAPaid = (InvoiceNonRevenue.InvAPaid.HasValue) ? (InvoiceNonRevenue.InvAPaid.Value + mdl.ARTotalPaid) : mdl.ARTotalPaid; //will shown as Partial Paid Field 
                    InvoiceNonRevenue.UpdatedBy = userID;
                    InvoiceNonRevenue.UpdatedDate = Helper.GetDateTimeNow();
                    InvoiceNonRevenue = _trxInvNonRevenueRepo.Update(InvoiceNonRevenue);
                    List<string> listPaymentCode = new List<string>();
                    listPaymentCode = Helper.GenerateVoucherNumber(InvoiceNonRevenue.CompanyID, InvoiceNonRevenue.OperatorID, userID);

                    trxInvoicePayment tempPayment = _trxInvPaymentRepo.GetList("trxInvoiceNonRevenueID = " + InvoiceNonRevenue.trxInvoiceNonRevenueID, "BatchNumber DESC").FirstOrDefault();
                    if (tempPayment != null)
                    {
                        batchNumber = tempPayment.BatchNumber.Value + 1;
                    }
                    for (int i = 0; i < listPaymentCode.Count(); i++)
                    {
                        //INSERT trxInvoicePayment
                        InvoicePayment = new trxInvoicePayment();
                        InvoicePayment.mstPaymentCodeId = i + 1;
                        InvoicePayment.VoucherNumber = listPaymentCode[i];
                        InvoicePayment.trxInvoiceNonRevenueID = InvoiceNonRevenue.trxInvoiceNonRevenueID;
                        InvoicePayment.Amount = PaymentCodeValue[i];
                        InvoicePayment.CreatedBy = userID;
                        InvoicePayment.CreatedDate = Helper.GetDateTimeNow();
                        InvoicePayment.BatchNumber = batchNumber;
                        InvoicePayment.mstPaymentBankID = mdl.mstPaymentId;
                        InvoicePayment.PaymentDate = mdl.InvPaidDate;
                        InvoicePayment = _trxInvPaymentRepo.Create(InvoicePayment);
                    }
                    //INSERT LogArActivity
                    logAr = new logArActivity();
                    logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.PayInvoice;
                    logAr.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                    logAr.trxInvoiceNonRevenueID = InvoiceNonRevenue.trxInvoiceNonRevenueID;
                    logAr.CreatedBy = userID;
                    logAr.CreatedDate = Helper.GetDateTimeNow();
                    _logARActRepo.Create(logAr);

                    //INSERT trxInvoiceMatchingAR
                    trxInvoiceMatchingAR matchingAR = new trxInvoiceMatchingAR();
                    matchingAR.RekeningKoranID = mdl.InvoiceMatchingAR.RekeningKoranID;
                    matchingAR.DocumentPayment = mdl.InvoiceMatchingAR.DocumentPayment;
                    matchingAR.CompanyCode = mdl.InvoiceMatchingAR.CompanyCode;
                    matchingAR.TanggalUangMasuk = mdl.InvoiceMatchingAR.TanggalUangMasuk;
                    matchingAR.InsertDate = _dtNow;
                    matchingAR.InsertTime = _dtNow.ToString("hh:mm:ss");
                    matchingAR.trxInvoiceNonRevenueID = InvoiceNonRevenue.trxInvoiceNonRevenueID;
                    matchingAR.BatchNumber = batchNumber;
                    matchingAR.PaymentDate = mdl.InvPaidDate;
                    matchingAR.Status = (int)StatusHelper.StatusCollectionToSAP.NotYet;
                    matchingAR.CreatedDate = _dtNow;
                    matchingAR.CreatedBy = userID;
                    _trxInvMatchingRepo.Create(matchingAR);
                }

                uow.SaveChanges();

                return new trxInvoiceHeader();

            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxInvoiceHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxARPaymentInvoiceTowerService", "SavePayment", userID));
            }
        }

        public vmStringResult ValidateIncludedAmount(string userID, int HeaderId, int CategoryId)
        {
            List<trxInvoicePayment> listInvoicePaymentPPH = new List<trxInvoicePayment>();
            List<trxInvoicePayment> listInvoicePaymentPPE = new List<trxInvoicePayment>();
            List<trxInvoicePayment> listInvoicePaymentPAT = new List<trxInvoicePayment>();
            List<trxInvoicePayment> listInvoicePaymentPPF = new List<trxInvoicePayment>();

            try
            {
                vmStringResult StringModel = new vmStringResult();
                StringModel.Result = "";
                string strWhereClausePPH = "";
                string strWhereClausePPE = "";
                string strWhereClausePAT = "";
                string strWhereClausePPF = "";
                if (CategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                {
                    strWhereClausePPH = "trxInvoiceHeaderID = " + HeaderId + " AND mstPaymentCodeId = " + (int)Constants.PaymentCode.PPH;//PPH
                    strWhereClausePPE = "trxInvoiceHeaderID = " + HeaderId + " AND mstPaymentCodeId = " + (int)Constants.PaymentCode.PPE;//PPE
                    strWhereClausePAT = "trxInvoiceHeaderID = " + HeaderId + " AND mstPaymentCodeId = " + (int)Constants.PaymentCode.PAT;//PAT
                    strWhereClausePPF = "trxInvoiceHeaderID = " + HeaderId + " AND mstPaymentCodeId = " + (int)Constants.PaymentCode.PPF;//PPF
                }
                else if (CategoryId == (int)StatusHelper.InvoiceCategory.Tower10 || CategoryId == (int)StatusHelper.InvoiceCategory.Tower15)
                {
                    strWhereClausePPH = "trxInvoiceHeaderRemainingAmountId = " + HeaderId + " AND mstPaymentCodeId = " + (int)Constants.PaymentCode.PPH;//PPH
                    strWhereClausePPE = "trxInvoiceHeaderRemainingAmountId = " + HeaderId + " AND mstPaymentCodeId = " + (int)Constants.PaymentCode.PPE;//PPE
                    strWhereClausePAT = "trxInvoiceHeaderRemainingAmountId = " + HeaderId + " AND mstPaymentCodeId = " + (int)Constants.PaymentCode.PAT;//PAT
                    strWhereClausePPF = "trxInvoiceHeaderRemainingAmountId = " + HeaderId + " AND mstPaymentCodeId = " + (int)Constants.PaymentCode.PPF;//PPF
                }
                else
                {
                    strWhereClausePPH = "trxInvoiceNonRevenueID = " + HeaderId + " AND mstPaymentCodeId = " + (int)Constants.PaymentCode.PPH;//PPH
                    strWhereClausePPE = "trxInvoiceNonRevenueID = " + HeaderId + " AND mstPaymentCodeId = " + (int)Constants.PaymentCode.PPE;//PPE
                    strWhereClausePAT = "trxInvoiceNonRevenueID = " + HeaderId + " AND mstPaymentCodeId = " + (int)Constants.PaymentCode.PAT;//PAT
                    strWhereClausePPF = "trxInvoiceNonRevenueID = " + HeaderId + " AND mstPaymentCodeId = " + (int)Constants.PaymentCode.PPF;//PPF
                }
                listInvoicePaymentPPH = _trxInvPaymentRepo.GetList(strWhereClausePPH);
                listInvoicePaymentPPE = _trxInvPaymentRepo.GetList(strWhereClausePPE);
                listInvoicePaymentPAT = _trxInvPaymentRepo.GetList(strWhereClausePAT);
                listInvoicePaymentPPF = _trxInvPaymentRepo.GetList(strWhereClausePPF);

                //Check if PPH has been Submit before or not
                if (listInvoicePaymentPPH.Count > 0)
                {
                    decimal PPHAmount = 0;
                    foreach (trxInvoicePayment list in listInvoicePaymentPPH)
                    {
                        PPHAmount += list.Amount.Value;
                    }
                    if (PPHAmount == 0)
                        StringModel.isShowPPH = true;
                    else
                        StringModel.isShowPPH = false;
                }
                else
                {
                    StringModel.isShowPPH = true;
                }

                //Check if PPE has been Submit before or not
                if (listInvoicePaymentPPE.Count > 0)
                {
                    decimal PPEAmount = 0;
                    foreach (trxInvoicePayment list in listInvoicePaymentPPE)
                    {
                        PPEAmount += list.Amount.Value;
                    }
                    if (PPEAmount == 0)
                        StringModel.isShowPPE = true;
                    else
                        StringModel.isShowPPE = false;
                }
                else
                {
                    StringModel.isShowPPE = true;
                }

                // Check if Penalty has been Submitted before or not
                if (listInvoicePaymentPAT.Count > 0)
                {
                    decimal PATAmount = 0;
                    foreach (trxInvoicePayment payment in listInvoicePaymentPAT)
                    {
                        PATAmount += payment.Amount.Value;
                    }
                    StringModel.isShowPAT = PATAmount == 0;
                }
                else
                {
                    StringModel.isShowPAT = true;
                }

                // Check if PPF has been Submitted before or not
                if (listInvoicePaymentPPF.Count > 0)
                {
                    decimal PPFAmount = 0;
                    foreach (trxInvoicePayment payment in listInvoicePaymentPPF)
                    {
                        PPFAmount += payment.Amount.Value;
                    }
                    StringModel.isShowPPF = PPFAmount == 0;
                }
                else
                {
                    StringModel.isShowPPF = true;
                }

                return StringModel;
            }
            catch (Exception ex)
            {
                return new vmStringResult((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxARPaymentInvoiceTowerService", "ValidateIncludedAmount", userID));
            }
        }

        public trxInvoiceHeader BackToARProcess(string userID, int HeaderId, int CategoryId, string strRemarks)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                #region 'Update InvoiceHeader Status'
                //ALL PAYMENT PROCESS DELETED, UPDATE STATUS TO BE SHOWN IN AR PROCESS, DELETE TrxInvoicePayment
                if (CategoryId == (int)StatusHelper.InvoiceCategory.Tower) //IF INVOICE IS TOWER
                {
                    trxInvoiceHeader InvoiceHeader = _trxInvHeaderRepo.GetByPK(HeaderId);
                    InvoiceHeader.InvPaidDate = null;
                    InvoiceHeader.InvIDPaymentBank = null;
                    InvoiceHeader.InvPaidStatus = null;
                    InvoiceHeader.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateReceivedByARCollection; //SET TO SHOWN IN AR PROCESS
                    InvoiceHeader.InvAPaid = null;
                    InvoiceHeader.InvReceiptDate = null;
                    InvoiceHeader.InvReceiptFile = null;
                    InvoiceHeader.ContentType = null;
                    InvoiceHeader.FilePath = null;
                    InvoiceHeader.ARProcessRemark = null;
                    InvoiceHeader.InvInternalPIC = null;
                    InvoiceHeader.UpdatedBy = userID;
                    InvoiceHeader.UpdatedDate = Helper.GetDateTimeNow();
                    InvoiceHeader = _trxInvHeaderRepo.Update(InvoiceHeader);

                    _trxInvPaymentRepo.DeleteByFilter("trxInvoiceHeaderID =" + HeaderId);
                }
                if (CategoryId == (int)StatusHelper.InvoiceCategory.Tower15 || CategoryId == (int)StatusHelper.InvoiceCategory.Tower10)
                {
                    trxInvoiceHeaderRemainingAmount InvoiceHeaderRemaining = _trxInvHeaderRemainRepo.GetByPK(HeaderId);
                    InvoiceHeaderRemaining.InvPaidDate = null;
                    InvoiceHeaderRemaining.InvIDPaymentBank = null;
                    InvoiceHeaderRemaining.InvPaidStatus = null;
                    InvoiceHeaderRemaining.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateReceivedByARCollection; //SET TO SHOWN IN AR PROCESS
                    InvoiceHeaderRemaining.InvAPaid = null;
                    InvoiceHeaderRemaining.InvReceiptDate = null;
                    InvoiceHeaderRemaining.InvReceiptFile = null;
                    InvoiceHeaderRemaining.ContentType = null;
                    InvoiceHeaderRemaining.FilePath = null;
                    InvoiceHeaderRemaining.ARProcessRemark = null;
                    InvoiceHeaderRemaining.InvInternalPIC = null;
                    InvoiceHeaderRemaining.UpdatedBy = userID;
                    InvoiceHeaderRemaining.UpdatedDate = Helper.GetDateTimeNow();
                    InvoiceHeaderRemaining = _trxInvHeaderRemainRepo.Update(InvoiceHeaderRemaining);

                    _trxInvPaymentRepo.DeleteByFilter("trxInvoiceHeaderRemainingAmountID =" + HeaderId);

                }
                if (CategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue) //IF INVOICE IS NON REVENUE
                {
                    trxInvoiceNonRevenue InvoiceNonRevenue = _trxInvNonRevenueRepo.GetByPK(HeaderId);
                    InvoiceNonRevenue.InvPaidDate = null;
                    InvoiceNonRevenue.InvIDPaymentBank = null;
                    InvoiceNonRevenue.InvPaidStatus = null;
                    InvoiceNonRevenue.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateReceivedByARCollection; //SET TO SHOWN IN AR PROCESS
                    InvoiceNonRevenue.InvAPaid = null;
                    InvoiceNonRevenue.InvReceiptDate = null;
                    InvoiceNonRevenue.InvReceiptFile = null;
                    InvoiceNonRevenue.ContentType = null;
                    InvoiceNonRevenue.FilePath = null;
                    InvoiceNonRevenue.ARProcessRemark = null;
                    InvoiceNonRevenue.InvInternalPIC = null;
                    InvoiceNonRevenue.UpdatedBy = userID;
                    InvoiceNonRevenue.UpdatedDate = Helper.GetDateTimeNow();
                    InvoiceNonRevenue = _trxInvNonRevenueRepo.Update(InvoiceNonRevenue);

                    _trxInvPaymentRepo.DeleteByFilter("trxInvoiceNonRevenueID =" + HeaderId);
                }
                #endregion


                #region 'INSERT LOGARACTIVITY'
                logArActivity logAr = new logArActivity();
                logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.BackToARProcess;
                logAr.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                if (CategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                {
                    logAr.trxInvoiceHeaderID = HeaderId;
                }
                if (CategoryId == (int)StatusHelper.InvoiceCategory.Tower15 || CategoryId == (int)StatusHelper.InvoiceCategory.Tower10)
                {
                    logAr.trxInvoiceHeaderRemainingAmountID = HeaderId;
                }
                if (CategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                {
                    logAr.trxInvoiceNonRevenueID = HeaderId;
                }
                logAr.ReprintRemarks = strRemarks;
                logAr.CreatedBy = userID;
                logAr.CreatedDate = Helper.GetDateTimeNow();
                logAr = _logARActRepo.Create(logAr);
                #endregion


                uow.SaveChanges();

                return new trxInvoiceHeader();

            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxInvoiceHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxARPaymentInvoiceTowerService", "BackToARProcess", userID));
            }
        }

        public List<vwstgTRStatusPenerimaanPembayaran> GetDocumentPaymentSAP(string UserID, string vDocumentPayment, string vCompanyCode, string vTglUangMasuk, string InvPaidDate)
        {
            List<vwstgTRStatusPenerimaanPembayaran> docPayment = new List<vwstgTRStatusPenerimaanPembayaran>();

            try
            {
                string strWhereClause = "1=1";
                if (!string.IsNullOrWhiteSpace(vDocumentPayment))
                {
                    strWhereClause += " AND Documentpayment = '" + vDocumentPayment + "' ";
                }
                if (!string.IsNullOrWhiteSpace(vCompanyCode))
                {
                    strWhereClause += " AND Companycode = '" + vCompanyCode + "' ";
                }
                if (!string.IsNullOrWhiteSpace(vTglUangMasuk) || vTglUangMasuk != null)
                { 
                    strWhereClause += " AND Tanggaluangmasuk = '" + vTglUangMasuk + "' ";
                }
                if (!string.IsNullOrWhiteSpace(InvPaidDate) || InvPaidDate != null)
                {
                    strWhereClause += " AND Tanggaluangmasuk = '" + Convert.ToDateTime(InvPaidDate).ToString("dd.MM.yyyy") + "' ";
                }

                strWhereClause += " AND TotalPayment > 0 ";
                strWhereClause += " OR Documentpayment = 'OTHERS' ";
                docPayment = _stgStatusPenerimaanRepo.GetList(strWhereClause);

                return docPayment;
            }
            catch (Exception ex)
            {
                docPayment.Add(new vwstgTRStatusPenerimaanPembayaran((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return docPayment;
            }
        }
    }
}
