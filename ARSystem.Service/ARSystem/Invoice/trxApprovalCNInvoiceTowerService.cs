using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;

namespace ARSystem.Service.ARSystem
{
    public class trxApprovalCNInvoiceTowerService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;

        private trxInvoiceHeaderRepository _trxInvoiceHeaderRepo;
        private trxInvoiceTowerDetailRepository _trxInvoiceDetailRepo;
        private trxInvoiceHeaderRemainingAmountRepository _trxInvoiceHeaderRemainRepo;
        private trxInvoiceTowerDetailRemaningAmountRepository _trxInvDetailRemainRepo;
        private trxPICAARRepository _trxPICAARRepo;
        private trxCancelNoteFinanceRepository _trxCancelNoteRepo;
        private logArActivityRepository _logARActRepo;
        private trxInvoiceNonRevenueRepository _trxInvNonRevenueRepo;
        private trxInvoiceNonRevenueSiteRepository _trxInvNonRevenueSiteRepo;
        private vwCNInvoiceTowerRepository _vwCNInvoiceRepo;
        private trxCNInvoiceHeaderRejectRepository _trxCNInvRejectRepo;
        private trxCNInvoiceJobTBGConsoleRepository _trxCNInvJob;
        private logInvoiceActivityRepository _logInvActRepo;
        private mstTaxInvoiceRepository _mstTaxRepo;
        private trxDocInvoiceDetailRepository _trxDocRepo;
        private trxBapsDataRepository _trxBapsRepo;
        private trxInvoiceManualRepository _trxInvManualeRepo;
        private logARProcessRepository _logARProcessRepo;
        private logCNARProcessRepository _logCNARProcessRepo;
        private trxCNDocInvoiceDetailRepository _trxCNDocRepo;
        private trxCNPICAARRepository _trxCNPICARepo;
        private mstCNTaxInvoiceRepository _mstCNTaxRepo;
        private logCNArActivityRepository _logCNARActRepo;
        private trxCNInvoiceHeaderRepository _trxCNInvHeaderRepo;
        private trxCNInvoiceTowerDetailRepository _trxCNInvDetailRepo;
        private trxArDetailRepository _trxARDetailRepo;
        private trxArHeaderRepository _trxARHeaderRepo;
        private trxCNInvoiceNonRevenueRepository _trxCNInvNonRevenueRepo;
        private trxCNInvoiceNonRevenueSiteRepository _trxCNInvNonRevenueSiteRepo;
        private trxCNInvoiceHeaderRemainingAmountRepository _trxCNInvHeaderRemainRepo;
        private trxCNInvoiceTowerDetailRemaningAmountRepository _trxCNInvDetailRemainRepo;
        private trxArDetailRemainingAmountRepository _trxARDetailRemainRepo;
        private uspGenerateTrxCNInvoiceRepository _uspGenTrxCNInvoiceRepo;

        public trxApprovalCNInvoiceTowerService() : base()
        {
            _context = this.SetContext();
            _dtNow = Helper.GetDateTimeNow();

            _trxInvoiceHeaderRepo = new trxInvoiceHeaderRepository(_context);
            _trxInvoiceDetailRepo = new trxInvoiceTowerDetailRepository(_context);
            _trxInvoiceHeaderRemainRepo = new trxInvoiceHeaderRemainingAmountRepository(_context);
            _trxInvDetailRemainRepo = new trxInvoiceTowerDetailRemaningAmountRepository(_context);
            _trxPICAARRepo = new trxPICAARRepository(_context);
            _trxCancelNoteRepo = new trxCancelNoteFinanceRepository(_context);
            _logARActRepo = new logArActivityRepository(_context);
            _trxInvNonRevenueRepo = new trxInvoiceNonRevenueRepository(_context);
            _vwCNInvoiceRepo = new vwCNInvoiceTowerRepository(_context);
            _trxCNInvRejectRepo = new trxCNInvoiceHeaderRejectRepository(_context);
            _trxCNInvJob = new trxCNInvoiceJobTBGConsoleRepository(_context);
            _logInvActRepo = new logInvoiceActivityRepository(_context);
            _mstTaxRepo = new mstTaxInvoiceRepository(_context);
            _trxDocRepo = new trxDocInvoiceDetailRepository(_context);
            _trxBapsRepo = new trxBapsDataRepository(_context);
            _trxInvManualeRepo = new trxInvoiceManualRepository(_context);
            _logARProcessRepo = new logARProcessRepository(_context);
            _logCNARProcessRepo = new logCNARProcessRepository(_context);
            _trxCNDocRepo = new trxCNDocInvoiceDetailRepository(_context);
            _trxCNPICARepo = new trxCNPICAARRepository(_context);
            _mstCNTaxRepo = new mstCNTaxInvoiceRepository(_context);
            _logCNARActRepo = new logCNArActivityRepository(_context);
            _trxCNInvHeaderRepo = new trxCNInvoiceHeaderRepository(_context);
            _trxCNInvDetailRepo = new trxCNInvoiceTowerDetailRepository(_context);
            _trxARDetailRepo = new trxArDetailRepository(_context);
            _trxARHeaderRepo = new trxArHeaderRepository(_context);
            _trxInvNonRevenueSiteRepo = new trxInvoiceNonRevenueSiteRepository(_context);
            _trxCNInvNonRevenueRepo = new trxCNInvoiceNonRevenueRepository(_context);
            _trxCNInvNonRevenueSiteRepo = new trxCNInvoiceNonRevenueSiteRepository(_context);
            _trxCNInvHeaderRemainRepo = new trxCNInvoiceHeaderRemainingAmountRepository(_context);
            _trxCNInvDetailRemainRepo = new trxCNInvoiceTowerDetailRemaningAmountRepository(_context);
            _trxARDetailRemainRepo = new trxArDetailRemainingAmountRepository(_context);
            _uspGenTrxCNInvoiceRepo = new uspGenerateTrxCNInvoiceRepository(_context);

        }

        public int GetApprovalCNInvoiceTowerCount(string userID, string invoiceTypeId, string companyId, string operatorId, string invNo)
        {
            try
            {
                string strWhereClause = "";
                //CHECK IF USER IS AR DEPT HEAD OR NOT
                if (UserHelper.GetUserARPosition(userID) == "DEPT HEAD")
                {
                    strWhereClause += "mstInvoiceStatusId IN (" + (int)StatusHelper.InvoiceStatus.StateWaitingDeptHeadApprovalCNInvoice +
                         ") AND ";//APPROVED BY SPV
                }
                else
                {
                    strWhereClause += "mstInvoiceStatusId NOT IN (" + (int)StatusHelper.InvoiceStatus.StateWaitingDeptHeadApprovalCNInvoice
                        + ") AND ";//SHOW NOT APPROVED BY DEPT
                }
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    strWhereClause += "InvCompanyId = '" + companyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(invoiceTypeId) && invoiceTypeId != "0")
                {
                    strWhereClause += "InvoiceTypeId = '" + invoiceTypeId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(operatorId))
                {
                    strWhereClause += "InvOperatorId = '" + operatorId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(invNo))
                {
                    strWhereClause += "InvNo LIKE '%" + invNo + "%' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return _vwCNInvoiceRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxApprovalCNInvoiceTowerService", "GetApprovalCNInvoiceTowerCount", userID);
                return 0;
            }
        }

        public List<vwCNInvoiceTower> GetApprovalCNInvoiceTowerToList(string userID, string invoiceTypeId, string companyId, string operatorId, string invNo, string strOrderBy, int intRowStart = 0, int intPageSize = 0)
        {
            List<vwCNInvoiceTower> listView = new List<vwCNInvoiceTower>();

            try
            {
                string strWhereClause = "";
                //CHECK IF USER IS AR DEPT HEAD OR NOT
                if (UserHelper.GetUserARPosition(userID) == "DEPT HEAD")
                {
                    strWhereClause += "mstInvoiceStatusId IN (" + (int)StatusHelper.InvoiceStatus.StateWaitingDeptHeadApprovalCNInvoice +
                         ") AND ";//APPROVED BY SPV
                }
                else
                {
                    strWhereClause += "mstInvoiceStatusId NOT IN (" + (int)StatusHelper.InvoiceStatus.StateWaitingDeptHeadApprovalCNInvoice
                        + ") AND ";//SHOW NOT APPROVED BY DEPT
                }
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    strWhereClause += "InvCompanyId = '" + companyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(invoiceTypeId) && invoiceTypeId != "0")
                {
                    strWhereClause += "InvoiceTypeId = '" + invoiceTypeId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(operatorId))
                {
                    strWhereClause += "InvOperatorId = '" + operatorId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(invNo))
                {
                    strWhereClause += "InvNo LIKE '%" + invNo + "%' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (string.IsNullOrWhiteSpace(strOrderBy))
                {
                    /*edit by mtr*/
                    //strOrderBy = "UpdatedDate DESC";
                    strOrderBy = "ApprovalStatus DESC";
                    /*edit by mtr*/
                }

                if (intPageSize > 0)
                    listView = _vwCNInvoiceRepo.GetPaged(strWhereClause, strOrderBy, intRowStart, intPageSize);
                else
                    listView = _vwCNInvoiceRepo.GetList(strWhereClause, strOrderBy);
                return listView;
            }
            catch (Exception ex)
            {
                listView.Add(new vwCNInvoiceTower((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxApprovalCNInvoiceTowerService", "GetApprovalCNInvoiceTowerToList", userID)));
                return listView;
            }
        }

        public vwCNInvoiceTower ApprovalCNSPVInvoiceTower(string userID, int trxInvoiceHeaderID, int mstInvoiceCategoryId, int mstPICATypeIDSection, int mstPICADetailIDSection)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                #region Update trxCNInvoiceHeader and Insert PICA Type from Section

                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                {
                    trxInvoiceHeader header = new trxInvoiceHeader();
                    header = _trxInvoiceHeaderRepo.GetByPK(trxInvoiceHeaderID);
                    header.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateWaitingDeptHeadApprovalCNInvoice;
                    header.UpdatedBy = userID;
                    header.UpdatedDate = Helper.GetDateTimeNow();
                    header = _trxInvoiceHeaderRepo.Update(header);

                    trxPICAAR pica = new trxPICAAR();
                    pica = _trxPICAARRepo.GetList("trxInvoiceHeaderID = " + trxInvoiceHeaderID).FirstOrDefault();
                    pica.mstPICATypeIDSection = mstPICATypeIDSection;
                    pica.mstPICADetailIDSection = mstPICADetailIDSection;
                    pica.UpdatedBy = userID;
                    pica.UpdatedDate = Helper.GetDateTimeNow();
                    pica = _trxPICAARRepo.Update(pica);
                }
                else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                {
                    trxInvoiceNonRevenue header = new trxInvoiceNonRevenue();
                    header = _trxInvNonRevenueRepo.GetByPK(trxInvoiceHeaderID);
                    header.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateWaitingDeptHeadApprovalCNInvoice;
                    header.UpdatedBy = userID;
                    header.UpdatedDate = Helper.GetDateTimeNow();
                    header = _trxInvNonRevenueRepo.Update(header);

                    trxPICAAR pica = new trxPICAAR();
                    pica = _trxPICAARRepo.GetList("trxInvoiceNonRevenueID = " + trxInvoiceHeaderID).FirstOrDefault();
                    pica.mstPICATypeIDSection = mstPICATypeIDSection;
                    pica.mstPICADetailIDSection = mstPICADetailIDSection;
                    pica.UpdatedBy = userID;
                    pica.UpdatedDate = Helper.GetDateTimeNow();
                    pica = _trxPICAARRepo.Update(pica);
                }
                else
                {
                    trxInvoiceHeaderRemainingAmount header = new trxInvoiceHeaderRemainingAmount();
                    header = _trxInvoiceHeaderRemainRepo.GetByPK(trxInvoiceHeaderID);
                    header.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateWaitingDeptHeadApprovalCNInvoice;
                    header.UpdatedBy = userID;
                    header.UpdatedDate = Helper.GetDateTimeNow();
                    header = _trxInvoiceHeaderRemainRepo.Update(header);

                    trxPICAAR pica = new trxPICAAR();
                    pica = _trxPICAARRepo.GetList("trxInvoiceHeaderRemainingAmountID = " + trxInvoiceHeaderID).FirstOrDefault();
                    pica.mstPICATypeIDSection = mstPICATypeIDSection;
                    pica.mstPICADetailIDSection = mstPICADetailIDSection;
                    pica.UpdatedBy = userID;
                    pica.UpdatedDate = Helper.GetDateTimeNow();
                    pica = _trxPICAARRepo.Update(pica);
                }

                #endregion

                #region Insert to logArActivity

                logArActivity logAr = new logArActivity();
                logAr.CreatedBy = userID;
                logAr.CreatedDate = Helper.GetDateTimeNow();
                logAr.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.SPVApproveCNInvoice;
                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                    logAr.trxInvoiceHeaderID = trxInvoiceHeaderID;
                else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                    logAr.trxInvoiceNonRevenueID = trxInvoiceHeaderID;
                else
                    logAr.trxInvoiceHeaderRemainingAmountID = trxInvoiceHeaderID;
                logAr = _logARActRepo.Create(logAr);

                #endregion

                uow.SaveChanges();

                return new vwCNInvoiceTower();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new vwCNInvoiceTower((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxApprovalCNInvoiceTowerService", "ApprovalCNSPVInvoiceTower", userID));
            }
        }

        public vwCNInvoiceTower ApprovalCNDeptHeadInvoiceTower(string userID, int trxInvoiceHeaderID, int mstInvoiceCategoryId, string vSource)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                int trxCNInvoiceHeaderParentID = ApprovalCNInvoiceTower(trxInvoiceHeaderID, mstInvoiceCategoryId, userID, null, vSource);

                if (trxCNInvoiceHeaderParentID != 0)
                {
                    List<trxInvoiceHeader> children = _trxInvoiceHeaderRepo.GetList("trxInvoiceHeaderParentID = " + trxInvoiceHeaderID);
                    foreach (trxInvoiceHeader child in children)
                    {
                        ApprovalCNInvoiceTower(child.trxInvoiceHeaderID, child.mstInvoiceCategoryId.Value, userID, trxCNInvoiceHeaderParentID, vSource);
                    }
                }
                else
                {
                    return new vwCNInvoiceTower((int)Helper.ErrorType.Error, "Error on System. Please Call IT Help Desk.");
                }

                uow.SaveChanges();

                return new vwCNInvoiceTower();
            }
            catch (Exception ex)
            {
                return (new vwCNInvoiceTower((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxApprovalCNInvoiceTowerService", "ApprovalCNDeptHeadInvoiceTower", userID)));
            }
        }

        private int ApprovalCNInvoiceTower(int trxInvoiceHeaderID, int mstInvoiceCategoryId, string userID, int? trxCNInvoiceHeaderParentID, string vSource)
        {
            logInvoiceActivity log = new logInvoiceActivity();
            List<mstTaxInvoice> taxes = new List<mstTaxInvoice>();
            logArActivity logAr = new logArActivity();
            List<trxDocInvoiceDetail> docs = new List<trxDocInvoiceDetail>();

            List<logARProcess> logARProcesses = new List<logARProcess>();
            List<logArActivity> listLogBaps = new List<logArActivity>();
            logCNARProcess logCNARPRocess = new logCNARProcess();
            trxPICAAR picaAR = new trxPICAAR();
            trxCNPICAAR cnPICAAR = new trxCNPICAAR();
            trxCNDocInvoiceDetail cnDoc = new trxCNDocInvoiceDetail();
            logCNArActivity cnLog = new logCNArActivity();
            mstCNTaxInvoice cnTaxInvoice = new mstCNTaxInvoice();
            trxBapsData bapsData = new trxBapsData();
            var trxBapsDataManual = new trxInvoiceManual();
            string filter = string.Empty;

            try
            {
                int trxCNInvoiceHeaderID = 0;
                string whereID = string.Empty;
                string operatorID = string.Empty;
                string companyID = string.Empty;

                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                {
                    #region Invoice Tower
                    trxInvoiceHeader header = new trxInvoiceHeader();
                    List<trxInvoiceTowerDetail> details = new List<trxInvoiceTowerDetail>();
                    trxCNInvoiceHeader cnHeader = new trxCNInvoiceHeader();
                    trxCNInvoiceTowerDetail cnDetail = new trxCNInvoiceTowerDetail();                   
                    trxArDetail arDetail = new trxArDetail();

                    whereID = "trxInvoiceHeaderID = " + trxInvoiceHeaderID;

                    #region Update trxInvoiceHeader

                    header = _trxInvoiceHeaderRepo.GetByPK(trxInvoiceHeaderID);
                    header.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateApprovedCNInvoice;
                    header.UpdatedDate = Helper.GetDateTimeNow();
                    header.UpdatedBy = userID;
                    header = _trxInvoiceHeaderRepo.Update(header);

                    #endregion

                    #region Insert to trxCNInvoiceHeader

                    cnHeader.AppHeaderId = header.AppHeaderId;
                    cnHeader.ApprovalDate = header.ApprovalDate;
                    cnHeader.ApprovalRemark = header.ApprovalRemark;
                    cnHeader.ApprovalStatus = header.ApprovalStatus;
                    cnHeader.ApprovedBy = header.ApprovedBy;
                    cnHeader.ARProcessRemark = header.ARProcessRemark;
                    cnHeader.ContentType = header.ContentType;
                    cnHeader.CreatedBy = header.CreatedBy;
                    cnHeader.CreatedDate = header.CreatedDate;
                    cnHeader.Currency = header.Currency;
                    cnHeader.FilePath = header.FilePath;
                    cnHeader.InvAPaid = header.InvAPaid;
                    cnHeader.InvCollectionRemarks = header.InvCollectionRemarks;
                    cnHeader.InvCompanyId = header.InvCompanyId;
                    cnHeader.InvCompanyInvoice = header.InvCompanyInvoice;
                    cnHeader.InvExternalPIC = header.InvExternalPIC;
                    cnHeader.InvFARSignature = header.InvFARSignature;
                    cnHeader.InvFirstPrintDate = header.InvFirstPrintDate;
                    cnHeader.InvIDPaymentBank = header.InvIDPaymentBank;
                    cnHeader.InvInternalPIC = header.InvInternalPIC;
                    cnHeader.InvIsAx = false;
                    cnHeader.InvIsParent = header.InvIsParent;
                    cnHeader.InvNo = header.InvNo;
                    cnHeader.InvoiceTypeId = header.InvoiceTypeId;
                    cnHeader.InvOperatorAsset = header.InvOperatorAsset;
                    cnHeader.InvOperatorID = header.InvOperatorID;
                    cnHeader.InvOprRegionID = header.InvOprRegionID;
                    cnHeader.InvPaidDate = header.InvPaidDate;
                    cnHeader.InvPaidStatus = header.InvPaidStatus;
                    cnHeader.InvPaidUser = header.InvPaidUser;
                    cnHeader.InvParentNo = header.InvParentNo;
                    cnHeader.InvPrintDate = header.InvPrintDate;
                    cnHeader.InvReceiptDateByOperator = header.InvReceiptDateByOperator;
                    cnHeader.InvReceiptFile = header.InvReceiptFile;
                    cnHeader.InvRemarksPrint = header.InvRemarksPrint;
                    cnHeader.InvRemarksPosting = header.InvRemarksPosting;
                    cnHeader.InvSubject = header.InvSubject;
                    cnHeader.InvSumADPP = header.InvSumADPP;
                    cnHeader.InvTaxNumber = header.InvTaxNumber;
                    cnHeader.InvTemp = header.InvTemp;
                    cnHeader.InvTotalAmount = header.InvTotalAmount;
                    cnHeader.InvTotalAPPH = header.InvTotalAPPH;
                    cnHeader.InvTotalAPPN = header.InvTotalAPPN;
                    cnHeader.InvTotalPenalty = header.InvTotalPenalty;
                    cnHeader.InvTotalDiscount = header.InvTotalDiscount;
                    cnHeader.IsPPH = header.IsPPH;
                    cnHeader.IsPPN = header.IsPPN;
                    cnHeader.IsPPHFinal = header.IsPPHFinal;
                    cnHeader.mstInvoiceCategoryId = header.mstInvoiceCategoryId;
                    cnHeader.mstInvoiceStatusId = header.mstInvoiceStatusId;
                    cnHeader.PICARemark = header.PICARemark;
                    cnHeader.ReturnToInvStatus = header.ReturnToInvStatus;
                    cnHeader.ARProcessPenalty = header.ARProcessPenalty;
                    cnHeader.UpdatedBy = header.UpdatedBy;
                    cnHeader.UpdatedDate = header.UpdatedDate;
                    cnHeader.VerificationDate = header.VerificationDate;
                    cnHeader.VerificationRemark = header.VerificationRemark;
                    cnHeader.VerificationStatus = header.VerificationStatus;
                    cnHeader.VerifiedBy = header.VerifiedBy;
                    cnHeader.InvoiceManual = header.InvoiceManual;

                    if (trxCNInvoiceHeaderParentID != null)
                        cnHeader.trxCNInvoiceHeaderParentId = trxCNInvoiceHeaderParentID;

                    cnHeader = _trxCNInvHeaderRepo.Create(cnHeader);

                    #endregion

                    #region Insert to trxCNInvoiceTowerDetail

                    _uspGenTrxCNInvoiceRepo.CreateCNInvoiceTowerDetail(whereID);

                    //details = _trxInvoiceDetailRepo.GetList("trxInvoiceHeaderID = " + trxInvoiceHeaderID);
                    //foreach (trxInvoiceTowerDetail detail in details)
                    //{
                    //    cnDetail = new trxCNInvoiceTowerDetail();
                    //    cnDetail.AmountInvoicePeriod = detail.AmountInvoicePeriod;
                    //    cnDetail.AmountLossPPN = detail.AmountLossPPN;
                    //    cnDetail.AmountOverblast = detail.AmountOverblast;
                    //    cnDetail.AmountOverdaya = detail.AmountOverdaya;
                    //    cnDetail.AmountPenaltyPeriod = detail.AmountPenaltyPeriod;
                    //    cnDetail.AmountPPN = detail.AmountPPN;
                    //    cnDetail.AmountRental = detail.AmountRental;
                    //    cnDetail.AmountService = detail.AmountService;
                    //    cnDetail.BapsNo = detail.BapsNo;
                    //    cnDetail.BapsPeriod = detail.BapsPeriod;
                    //    cnDetail.BapsType = detail.BapsType;
                    //    cnDetail.CreatedBy = detail.CreatedBy;
                    //    cnDetail.CreatedDate = detail.CreatedDate;
                    //    cnDetail.EndDatePeriod = detail.EndDatePeriod;
                    //    cnDetail.IsLossPPN = detail.IsLossPPN;
                    //    cnDetail.IsPartial = detail.IsPartial;
                    //    cnDetail.PeriodInvoice = detail.PeriodInvoice;
                    //    cnDetail.PoNumber = detail.PoNumber;
                    //    cnDetail.PowerType = detail.PowerType;
                    //    cnDetail.PowerTypeCode = detail.PowerTypeCode;
                    //    cnDetail.RfiDate = detail.RfiDate;
                    //    cnDetail.SiteIdOld = detail.SiteIdOld;
                    //    cnDetail.SiteName = detail.SiteName;
                    //    cnDetail.SONumber = detail.SONumber;
                    //    cnDetail.SpkDate = detail.SpkDate;
                    //    cnDetail.StartDatePeriod = detail.StartDatePeriod;
                    //    cnDetail.StipSiro = detail.StipSiro;
                    //    cnDetail.StipSiroId = detail.StipSiroId;
                    //    cnDetail.trxCNInvoiceHeaderID = cnHeader.trxCNInvoiceHeaderID;
                    //    cnDetail.Type = detail.Type;
                    //    cnDetail.UpdatedBy = detail.UpdatedBy;
                    //    cnDetail.UpdatedDate = detail.UpdatedDate;
                    //  _trxCNInvDetailRepo.Create(cnDetail);
                    //}

                    #endregion

                    #region Update trxArDetail
                    picaAR = _trxPICAARRepo.GetList(whereID).FirstOrDefault();
                    foreach (trxInvoiceTowerDetail detail in details)
                    {
                        filter = "[SONumber]='" + detail.SONumber + "' AND [BapsNo]='" + detail.BapsNo + "'" +
                            "AND [BapsType] = '" + detail.BapsType + "' AND [StipSiro]='" + detail.StipSiro + "'" +
                            "AND [BapsPeriod]='" + detail.BapsPeriod + "' AND [Currency]='" + header.Currency + "'";
                        arDetail = _trxARDetailRepo.GetList(filter).FirstOrDefault();
                        //bapsData = trxBapsDataRepo.GetList(filter).FirstOrDefault();
                        if (arDetail != null)
                        {
                            if (picaAR != null)
                            {
                                if (picaAR.mstPICATypeIDSection.Value == (int)Constants.PICATypeIDARData.CNInternal)
                                {
                                    //if PICA Type is Internal, Data back to Create Menu
                                    arDetail.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.NotProcessed;
                                    arDetail.UpdatedBy = userID;
                                    arDetail.UpdatedDate = Helper.GetDateTimeNow();
                                    _trxARDetailRepo.Update(arDetail);
                                }
                                else if (picaAR.mstPICATypeIDSection.Value == (int)Constants.PICATypeIDARData.CNExternal)
                                {
                                    //if PICA Type is External,Delete AR Detail Log, trxARHeader and trxARDetail, Data back to BapsConfirm
                                    _logARActRepo.DeleteByFilter("trxArDetailId = " + arDetail.trxArDetailId);
                                    _trxARHeaderRepo.DeleteByPK(arDetail.TrxArHeaderId);
                                    _trxARDetailRepo.DeleteByPK(arDetail.trxArDetailId);

                                    if (arDetail.InvoiceManual == false || arDetail.InvoiceManual == null)
                                    {
                                        bapsData = _trxBapsRepo.GetList(filter).FirstOrDefault();
                                        bapsData.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateBAPSReceive;
                                        bapsData.UpdatedBy = userID;
                                        bapsData.UpdatedDate = Helper.GetDateTimeNow();
                                        _trxBapsRepo.Update(bapsData);
                                        _logARActRepo.DeleteByFilter("trxBapsDataId = " + bapsData.trxBapsDataId + " AND mstInvoiceStatusId <> " + (int)StatusHelper.InvoiceStatus.BAPSReceive);

                                        if (bapsData != null)
                                        {
                                            listLogBaps = _logARActRepo.GetList("trxBapsDataId = " + bapsData.trxBapsDataId);
                                            foreach (logArActivity l in listLogBaps)
                                            {
                                                cnLog = new logCNArActivity();
                                                cnLog.CreatedBy = l.CreatedBy;
                                                cnLog.CreatedDate = l.CreatedDate;
                                                cnLog.LogWeek = l.LogWeek;
                                                cnLog.mstInvoiceStatusId = l.mstInvoiceStatusId;
                                                cnLog.PICAReprintID = l.PICAReprintID;
                                                cnLog.ReprintRemarks = l.ReprintRemarks;
                                                cnLog.trxBapsDataId = l.trxBapsDataId;
                                                cnLog.trxCNArDetailId = l.trxArDetailId;
                                                cnLog.trxCNInvoiceHeaderID = l.trxInvoiceHeaderID;
                                                _logCNARActRepo.Create(cnLog);
                                            }
                                            _logARActRepo.DeleteByFilter("trxBapsDataId = " + bapsData.trxBapsDataId);
                                        }
                                    }
                                    else
                                    {
                                        //CHANGE TRXBAPSDATA STATUS TO RECEIVED
                                        //filter = "[SONumber]='" + arDetail.SONumber + "' AND [BapsNO]='" + arDetail.BapsNo + "'" +
                                        //    " AND [BapsType] = '" + arDetail.BapsType + "' AND [StipSiro]='" + arDetail.StipSiroId + "'" +
                                        //    " AND [BapsPeriod]='" + arDetail.BapsPeriod + "' AND [PriceCurrency]='" + arDetail.Currency + "'" +
                                        //    " AND [InitialPONumber]='" + arDetail.PoNumber + "'";
                                        filter = "[SONumber]='" + arDetail.SONumber + "' AND [BapsNO]='" + arDetail.BapsNo + "'" +
                                                " AND [BapsType] = '" + arDetail.BapsType + "' AND [InvoiceEndDate]='" + arDetail.EndDateInvoice + "'" +
                                                " AND [InvoiceStartDate] ='" + arDetail.StartDateInvoice + "' AND [PriceCurrency]='" + arDetail.Currency + "'";

                                        trxBapsDataManual = _trxInvManualeRepo.GetList(filter, "")[0];
                                        trxBapsDataManual.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateBAPSReceive;//STATE BAPS RECEIVED IN TRXBAPS DATA
                                        trxBapsDataManual.UpdatedBy = userID;
                                        trxBapsDataManual.UpdatedDate = Helper.GetDateTimeNow();
                                        trxBapsDataManual = _trxInvManualeRepo.Update(trxBapsDataManual);

                                        _logARActRepo.DeleteByFilter("trxInvoiceManualID = " + trxBapsDataManual.trxInvoiceManualID + " AND mstInvoiceStatusId <> " + (int)StatusHelper.InvoiceStatus.BAPSReceive);

                                        if (trxBapsDataManual != null)
                                        {
                                            listLogBaps = _logARActRepo.GetList("trxInvoiceManualID = " + trxBapsDataManual.trxInvoiceManualID);
                                            foreach (logArActivity l in listLogBaps)
                                            {
                                                cnLog = new logCNArActivity();
                                                cnLog.CreatedBy = l.CreatedBy;
                                                cnLog.CreatedDate = l.CreatedDate;
                                                cnLog.LogWeek = l.LogWeek;
                                                cnLog.mstInvoiceStatusId = l.mstInvoiceStatusId;
                                                cnLog.PICAReprintID = l.PICAReprintID;
                                                cnLog.ReprintRemarks = l.ReprintRemarks;
                                                cnLog.trxInvoiceManualID = l.trxInvoiceManualID;
                                                cnLog.trxCNArDetailId = l.trxArDetailId;
                                                cnLog.trxCNInvoiceHeaderID = l.trxInvoiceHeaderID;
                                                _logCNARActRepo.Create(cnLog);
                                            }
                                            _logARActRepo.DeleteByFilter("trxInvoiceManualID = " + trxBapsDataManual.trxInvoiceManualID);
                                        }
                                    }

                                }
                            }

                        }


                    }

                    #endregion

                    trxCNInvoiceHeaderID = cnHeader.trxCNInvoiceHeaderID;
                    operatorID = cnHeader.InvOperatorID;
                    companyID = cnHeader.InvCompanyId;

                    #region Insert to Job TBG Console
                    trxCNInvoiceJobTBGConsole cnJob = new trxCNInvoiceJobTBGConsole();
                    cnJob.trxCNInvoiceHeaderID = trxCNInvoiceHeaderID;
                    cnJob.Status = 0;
                    cnJob.CreatedDate = Helper.GetDateTimeNow();
                    cnJob.CreatedBy = userID;
                    cnJob = _trxCNInvJob.Create(cnJob);
                    #endregion

                    #endregion
                }
                else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                {
                    #region Invoice NonRevenue
                    trxInvoiceNonRevenue header = new trxInvoiceNonRevenue();
                    List<trxInvoiceNonRevenueSite> sites = new List<trxInvoiceNonRevenueSite>();
                    trxCNInvoiceNonRevenue cnNonRevenue = new trxCNInvoiceNonRevenue();
                    trxCNInvoiceNonRevenueSite cnSite = new trxCNInvoiceNonRevenueSite();

                    whereID = "trxInvoiceNonRevenueID = " + trxInvoiceHeaderID;

                    #region Update trxInvoiceNonRevenue

                    header = _trxInvNonRevenueRepo.GetByPK(trxInvoiceHeaderID);
                    header.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateApprovedCNInvoice;
                    header.UpdatedDate = Helper.GetDateTimeNow();
                    header.UpdatedBy = userID;
                    header = _trxInvNonRevenueRepo.Update(header);

                    #endregion

                    #region Insert to trxCNInvoiceNonRevenue

                    cnNonRevenue.InvNo = header.InvNo; ;
                    cnNonRevenue.InvTaxNumber = header.InvTaxNumber;
                    cnNonRevenue.CompanyID = header.CompanyID;
                    cnNonRevenue.OperatorID = header.OperatorID;
                    cnNonRevenue.Amount = header.Amount;
                    cnNonRevenue.Discount = header.Discount;
                    cnNonRevenue.DPP = header.DPP;
                    cnNonRevenue.TotalPPN = header.TotalPPN;
                    cnNonRevenue.TotalPPH = header.TotalPPH;
                    cnNonRevenue.Penalty = header.Penalty;
                    cnNonRevenue.InvoiceTotal = header.InvoiceTotal;
                    cnNonRevenue.IsPPN = header.IsPPN;
                    cnNonRevenue.IsPPH = header.IsPPH;
                    cnNonRevenue.mstInvoiceCategoryId = header.mstInvoiceCategoryId;
                    cnNonRevenue.mstInvoiceStatusId = header.mstInvoiceStatusId;
                    cnNonRevenue.InvoiceTypeId = header.InvoiceTypeId;
                    cnNonRevenue.InvPrintDate = header.InvPrintDate;
                    cnNonRevenue.InvSubject = header.InvSubject;
                    cnNonRevenue.InvOprRegionID = header.InvOprRegionID;
                    cnNonRevenue.InvFARSignature = header.InvFARSignature;
                    cnNonRevenue.InvAdditionalNote = header.InvAdditionalNote;
                    cnNonRevenue.VerificationStatus = header.VerificationStatus;
                    cnNonRevenue.VerificationDate = header.VerificationDate;
                    cnNonRevenue.InvPaidStatus = header.InvPaidStatus;
                    cnNonRevenue.Currency = header.Currency;
                    cnNonRevenue.ReturnToInvStatus = header.ReturnToInvStatus;
                    cnNonRevenue.InvRemarksPosting = header.InvRemarksPosting;
                    cnNonRevenue.InvFirstPrintDate = header.InvFirstPrintDate;
                    cnNonRevenue.InvRemarksPrint = header.InvRemarksPrint;
                    cnNonRevenue.InvReceiptDate = header.InvReceiptDate;
                    cnNonRevenue.InvReceiptFile = header.InvReceiptFile;
                    cnNonRevenue.FilePath = header.FilePath;
                    cnNonRevenue.ContentType = header.ContentType;
                    cnNonRevenue.ARProcessRemark = header.ARProcessRemark;
                    cnNonRevenue.InvInternalPIC = header.InvInternalPIC;
                    cnNonRevenue.ARProcessPenalty = header.ARProcessPenalty;
                    cnNonRevenue.InvPaidDate = header.InvPaidDate;
                    cnNonRevenue.InvIDPaymentBank = header.InvIDPaymentBank;
                    cnNonRevenue.InvAPaid = header.InvAPaid;
                    cnNonRevenue.CreatedDate = header.CreatedDate;
                    cnNonRevenue.CreatedBy = header.CreatedBy;
                    cnNonRevenue.UpdatedBy = header.UpdatedBy;
                    cnNonRevenue.UpdatedDate = header.UpdatedDate;

                    cnNonRevenue = _trxCNInvNonRevenueRepo.Create(cnNonRevenue);

                    #endregion

                    #region Insert to trxCNInvoiceNonRevenueSite

                    sites = _trxInvNonRevenueSiteRepo.GetList("trxInvoiceNonRevenueID = " + trxInvoiceHeaderID);
                    foreach (trxInvoiceNonRevenueSite detail in sites)
                    {
                        cnSite = new trxCNInvoiceNonRevenueSite();
                        cnSite.SONumber = detail.SONumber;
                        cnSite.SiteID = detail.SiteID;
                        cnSite.SiteName = detail.SiteName;
                        cnSite.SiteIDCustomer = detail.SiteIDCustomer;
                        cnSite.SiteNameCustomer = detail.SiteNameCustomer;
                        cnSite.CompanyID = detail.CompanyID;
                        cnSite.OperatorID = detail.OperatorID;
                        cnSite.StartPeriod = detail.StartPeriod;
                        cnSite.EndPeriod = detail.EndPeriod;
                        cnSite.Amount = detail.Amount;
                        cnSite.CreatedDate = detail.CreatedDate;
                        cnSite.CreatedBy = detail.CreatedBy;
                        cnSite.UpdatedDate = detail.UpdatedDate;
                        cnSite.UpdatedBy = detail.UpdatedBy;
                        _trxCNInvNonRevenueSiteRepo.Create(cnSite);
                    }

                    #endregion

                    trxCNInvoiceHeaderID = cnNonRevenue.trxCNInvoiceNonRevenueID;
                    operatorID = cnNonRevenue.OperatorID;
                    companyID = cnNonRevenue.CompanyID;
                    #endregion
                }
                else
                {
                    #region Invoice Tower Remaining
                    trxInvoiceHeaderRemainingAmount header = new trxInvoiceHeaderRemainingAmount();
                    List<trxInvoiceTowerDetailRemaningAmount> details = new List<trxInvoiceTowerDetailRemaningAmount>();
                    trxCNInvoiceHeaderRemainingAmount cnHeader = new trxCNInvoiceHeaderRemainingAmount();
                    trxCNInvoiceTowerDetailRemaningAmount cnDetail = new trxCNInvoiceTowerDetailRemaningAmount();
                    trxArDetailRemainingAmount arDetail = new trxArDetailRemainingAmount();

                    whereID = "trxInvoiceHeaderRemainingAmountID = " + trxInvoiceHeaderID;

                    #region Update trxInvoiceHeader

                    header = _trxInvoiceHeaderRemainRepo.GetByPK(trxInvoiceHeaderID);
                    header.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateWaitingSPVApprovalCNInvoice;
                    header.UpdatedDate = Helper.GetDateTimeNow();
                    header.UpdatedBy = userID;
                    header = _trxInvoiceHeaderRemainRepo.Update(header);

                    #endregion

                    #region Insert to trxCNInvoiceHeaderRemainingAmount

                    cnHeader.AppHeaderId = header.AppHeaderId;
                    cnHeader.ApprovalDate = header.ApprovalDate;
                    cnHeader.ApprovalRemark = header.ApprovalRemark;
                    cnHeader.ApprovalStatus = header.ApprovalStatus;
                    cnHeader.ApprovedBy = header.ApprovedBy;
                    cnHeader.ARProcessRemark = header.ARProcessRemark;
                    cnHeader.ContentType = header.ContentType;
                    cnHeader.CreatedBy = header.CreatedBy;
                    cnHeader.CreatedDate = header.CreatedDate;
                    cnHeader.Currency = header.Currency;
                    cnHeader.FilePath = header.FilePath;
                    cnHeader.InvAPaid = header.InvAPaid;
                    cnHeader.InvCollectionRemarks = header.InvCollectionRemarks;
                    cnHeader.InvCompanyId = header.InvCompanyId;
                    cnHeader.InvCompanyInvoice = header.InvCompanyInvoice;
                    cnHeader.InvExternalPIC = header.InvExternalPIC;
                    cnHeader.InvFARSignature = header.InvFARSignature;
                    cnHeader.InvFirstPrintDate = header.InvFirstPrintDate;
                    cnHeader.InvIDPaymentBank = header.InvIDPaymentBank;
                    cnHeader.InvInternalPIC = header.InvInternalPIC;
                    cnHeader.InvIsAx = false;
                    cnHeader.InvIsParent = header.InvIsParent;
                    cnHeader.InvNo = header.InvNo;
                    cnHeader.InvoiceTypeId = header.InvoiceTypeId;
                    cnHeader.InvOperatorAsset = header.InvOperatorAsset;
                    cnHeader.InvOperatorID = header.InvOperatorID;
                    cnHeader.InvOprRegionID = header.InvOprRegionID;
                    cnHeader.InvPaidDate = header.InvPaidDate;
                    cnHeader.InvPaidStatus = header.InvPaidStatus;
                    cnHeader.InvPaidUser = header.InvPaidUser;
                    cnHeader.InvParentNo = header.InvParentNo;
                    cnHeader.InvPrintDate = header.InvPrintDate;
                    cnHeader.InvReceiptDateByOperator = header.InvReceiptDateByOperator;
                    cnHeader.InvReceiptFile = header.InvReceiptFile;
                    cnHeader.InvRemarksPrint = header.InvRemarksPrint;
                    cnHeader.InvRemarksPosting = header.InvRemarksPosting;
                    cnHeader.InvSubject = header.InvSubject;
                    cnHeader.InvSumADPP = header.InvSumADPP;
                    cnHeader.InvTaxNumber = header.InvTaxNumber;
                    cnHeader.InvTemp = header.InvTemp;
                    cnHeader.InvTotalAmount = header.InvTotalAmount;
                    cnHeader.InvTotalAPPH = header.InvTotalAPPH;
                    cnHeader.InvTotalAPPN = header.InvTotalAPPN;
                    cnHeader.InvTotalPenalty = header.InvTotalPenalty;
                    cnHeader.IsPPH = header.IsPPH;
                    cnHeader.IsPPN = header.IsPPN;
                    cnHeader.IsPPHFinal = header.IsPPHFinal;
                    cnHeader.ARProcessPenalty = header.ARProcessPenalty;
                    cnHeader.mstInvoiceCategoryId = header.mstInvoiceCategoryId;
                    cnHeader.mstInvoiceStatusId = header.mstInvoiceStatusId;
                    cnHeader.PICARemark = header.PICARemark;
                    cnHeader.ReturnToInvStatus = header.ReturnToInvStatus;
                    cnHeader.UpdatedBy = header.UpdatedBy;
                    cnHeader.UpdatedDate = header.UpdatedDate;
                    cnHeader.VerificationDate = header.VerificationDate;
                    cnHeader.VerificationRemark = header.VerificationRemark;
                    cnHeader.VerificationStatus = header.VerificationStatus;
                    cnHeader.VerifiedBy = header.VerifiedBy;

                    if (trxCNInvoiceHeaderParentID != null)
                        cnHeader.trxCNInvoiceHeaderRemainingAmountParentId = trxCNInvoiceHeaderParentID;

                    cnHeader = _trxCNInvHeaderRemainRepo.Create(cnHeader);

                    #endregion

                    #region Insert to trxCNInvoiceTowerDetailRemaningAmount

                    details = _trxInvDetailRemainRepo.GetList("trxInvoiceHeaderRemainingID = " + header.trxInvoiceHeaderRemainingAmountID);
                    foreach (trxInvoiceTowerDetailRemaningAmount detail in details)
                    {
                        cnDetail = new trxCNInvoiceTowerDetailRemaningAmount();
                        cnDetail.AmountInvoicePeriod = detail.AmountInvoicePeriod;
                        cnDetail.AmountLossPPN = detail.AmountLossPPN;
                        cnDetail.AmountOverblast = detail.AmountOverblast;
                        cnDetail.AmountOverdaya = detail.AmountOverdaya;
                        cnDetail.AmountPenaltyPeriod = detail.AmountPenaltyPeriod;
                        cnDetail.AmountPPN = detail.AmountPPN;
                        cnDetail.AmountRental = detail.AmountRental;
                        cnDetail.AmountService = detail.AmountService;
                        cnDetail.BapsNo = detail.BapsNo;
                        cnDetail.BapsPeriod = detail.BapsPeriod;
                        cnDetail.BapsType = detail.BapsType;
                        cnDetail.CreatedBy = detail.CreatedBy;
                        cnDetail.CreatedDate = detail.CreatedDate;
                        cnDetail.EndDatePeriod = detail.EndDatePeriod;
                        cnDetail.IsLossPPN = detail.IsLossPPN;
                        cnDetail.IsPartial = detail.IsPartial;
                        cnDetail.PeriodInvoice = detail.PeriodInvoice;
                        cnDetail.PoNumber = detail.PoNumber;
                        cnDetail.PowerType = detail.PowerType;
                        cnDetail.PowerTypeCode = detail.PowerTypeCode;
                        cnDetail.RfiDate = detail.RfiDate;
                        cnDetail.SiteIdOld = detail.SiteIdOld;
                        cnDetail.SiteName = detail.SiteName;
                        cnDetail.SONumber = detail.SONumber;
                        cnDetail.SpkDate = detail.SpkDate;
                        cnDetail.StartDatePeriod = detail.StartDatePeriod;
                        cnDetail.StipSiro = detail.StipSiro;
                        cnDetail.StipSiroId = detail.StipSiroId;
                        cnDetail.trxCNInvoiceHeaderRemainingID = cnHeader.trxCNInvoiceHeaderRemainingAmountID;
                        cnDetail.Type = detail.Type;
                        cnDetail.UpdatedBy = detail.UpdatedBy;
                        cnDetail.UpdatedDate = detail.UpdatedDate;
                        _trxCNInvDetailRemainRepo.Create(cnDetail);
                    }

                    #endregion

                    #region Update trxArDetailRemainingAmount

                    picaAR = _trxPICAARRepo.GetList(whereID).FirstOrDefault();
                    foreach (trxInvoiceTowerDetailRemaningAmount detail in details)
                    {
                        filter = "[SONumber]='" + detail.SONumber + "' AND [BapsNo]='" + detail.BapsNo + "'" +
                            "AND [BapsType] = '" + detail.BapsType + "' AND [StipSiro]='" + detail.StipSiro + "'" +
                            "AND [BapsPeriod]='" + detail.BapsPeriod + "' AND [Currency]='" + header.Currency + "'";
                        arDetail = _trxARDetailRemainRepo.GetList(filter).FirstOrDefault();

                        bapsData = _trxBapsRepo.GetList(filter).FirstOrDefault();
                        if (arDetail != null)
                        {
                            if (picaAR != null)
                            {
                                if (picaAR.mstPICATypeIDSection.Value == (int)StatusHelper.PicaTypeID.ARCollectionInternal)
                                {
                                    //if PICA Type is Internal, Data back to Create Menu
                                    arDetail.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.NotProcessed;
                                    arDetail.UpdatedBy = userID;
                                    arDetail.UpdatedDate = Helper.GetDateTimeNow();
                                    _trxARDetailRemainRepo.Update(arDetail);
                                }
                                else if (picaAR.mstPICATypeIDSection.Value == (int)StatusHelper.PicaTypeID.ARCollectionExternal)
                                {
                                    //if PICA Type is External,Delete AR Detail Log and trxARDetail
                                    _logARActRepo.DeleteByFilter("trxArDetailRemainingAmountId = " + arDetail.trxArDetailRemainingAmountId);
                                    _trxARDetailRemainRepo.DeleteByPK(arDetail.trxArDetailRemainingAmountId);
                                }

                            }
                        }

                        if (bapsData != null)
                        {
                            listLogBaps = _logARActRepo.GetList("trxBapsDataId = " + bapsData.trxBapsDataId);
                            foreach (logArActivity l in listLogBaps)
                            {
                                cnLog = new logCNArActivity();
                                cnLog.CreatedBy = l.CreatedBy;
                                cnLog.CreatedDate = l.CreatedDate;
                                cnLog.LogWeek = l.LogWeek;
                                cnLog.mstInvoiceStatusId = l.mstInvoiceStatusId;
                                cnLog.PICAReprintID = l.PICAReprintID;
                                cnLog.ReprintRemarks = l.ReprintRemarks;
                                if (arDetail.InvoiceManual == false || arDetail.InvoiceManual == null)
                                    cnLog.trxBapsDataId = l.trxBapsDataId;
                                else
                                    cnLog.trxInvoiceManualID = l.trxInvoiceManualID;
                                cnLog.trxCNArDetailRemainingAmountId = l.trxArDetailRemainingAmountId;
                                cnLog.trxCNInvoiceHeaderRemainingAmountID = l.trxInvoiceHeaderRemainingAmountID;
                                _logCNARActRepo.Create(cnLog);
                            }
                            if (arDetail.InvoiceManual == false || arDetail.InvoiceManual == null)
                                _logInvActRepo.DeleteByFilter("trxBapsDataId = " + bapsData.trxBapsDataId);
                            else
                                _logInvActRepo.DeleteByFilter("trxInvoiceManualID = " + trxBapsDataManual.trxInvoiceManualID);
                        }
                    }

                    #endregion

                    trxCNInvoiceHeaderID = cnHeader.trxCNInvoiceHeaderRemainingAmountID;
                    operatorID = cnHeader.InvOperatorID;
                    companyID = cnHeader.InvCompanyId;
                    #endregion
                }

                #region Insert to logArActivity

                logAr.CreatedBy = userID;
                logAr.CreatedDate = Helper.GetDateTimeNow();
                logAr.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.DeptHeadApproveCNInvoice;
                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                    logAr.trxInvoiceHeaderID = trxInvoiceHeaderID;
                else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                    logAr.trxInvoiceNonRevenueID = trxInvoiceHeaderID;
                else
                    logAr.trxInvoiceHeaderRemainingAmountID = trxInvoiceHeaderID;
                logAr = _logARActRepo.Create(logAr);

                #endregion

                #region Insert into logCNARProcess

                logARProcesses = _logARProcessRepo.GetList(whereID);
                foreach (logARProcess logARProcess in logARProcesses)
                {
                    logCNARPRocess = new logCNARProcess();
                    logCNARPRocess.ARProcessPenalty = logARProcess.ARProcessPenalty;
                    logCNARPRocess.ContentType = logARProcess.ContentType;
                    logCNARPRocess.CreatedBy = logARProcess.CreatedBy;
                    logCNARPRocess.CreatedDate = logARProcess.CreatedDate;
                    logCNARPRocess.FilePath = logARProcess.FilePath;
                    logCNARPRocess.InvInternalPIC = logARProcess.InvInternalPIC;
                    logCNARPRocess.InvReceiptFile = logARProcess.InvReceiptFile;
                    logCNARPRocess.ReceiptDate = logARProcess.ReceiptDate;
                    logCNARPRocess.Remark = logARProcess.Remark;
                    if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                        logCNARPRocess.trxCNInvoiceHeaderID = trxCNInvoiceHeaderID;
                    else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                        logCNARPRocess.trxCNInvoiceNonRevenueID = trxCNInvoiceHeaderID;
                    else
                        logCNARPRocess.trxCNInvoiceHeaderRemainingAmountID = trxCNInvoiceHeaderID;
                    _logCNARProcessRepo.Create(logCNARPRocess);
                }

                #endregion

                #region Insert to trxCNPICAAR

                picaAR = _trxPICAARRepo.GetList(whereID).FirstOrDefault();
                if (picaAR != null)
                {
                    cnPICAAR = new trxCNPICAAR();
                    cnPICAAR.CreatedBy = picaAR.CreatedBy;
                    cnPICAAR.CreatedDate = picaAR.CreatedDate;
                    cnPICAAR.UpdatedBy = userID;
                    cnPICAAR.UpdatedDate = Helper.GetDateTimeNow();
                    cnPICAAR.mstPICADetailID = picaAR.mstPICADetailID;
                    cnPICAAR.mstPICAMajorID = picaAR.mstPICAMajorID;
                    cnPICAAR.mstPICATypeID = picaAR.mstPICATypeID;
                    cnPICAAR.mstPICATypeIDSection = picaAR.mstPICATypeIDSection;
                    cnPICAAR.mstPICADetailIDSection = picaAR.mstPICADetailIDSection;
                    cnPICAAR.NeedCN = picaAR.NeedCN;
                    cnPICAAR.Remark = picaAR.Remark;
                    if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                        cnPICAAR.trxCNInvoiceHeaderID = trxCNInvoiceHeaderID;
                    else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                        cnPICAAR.trxCNInvoiceNonRevenueID = trxCNInvoiceHeaderID;
                    else
                        cnPICAAR.trxCNInvoiceHeaderRemainingAmountID = trxCNInvoiceHeaderID;
                    cnPICAAR = _trxCNPICARepo.Create(cnPICAAR);
                }

                #endregion

                #region Insert to trxCNDocInvoiceDetail

                _uspGenTrxCNInvoiceRepo.CreateCNInvoiceDocDetail(trxCNInvoiceHeaderID, mstInvoiceCategoryId);

                //docs = _trxDocRepo.GetList(whereID);
                //foreach (trxDocInvoiceDetail doc in docs)
                //{
                //    cnDoc = new trxCNDocInvoiceDetail();
                //    cnDoc.CreatedBy = doc.CreatedBy;
                //    cnDoc.CreatedDate = doc.CreatedDate;
                //    cnDoc.DocInvoiceID = doc.DocInvoiceID;
                //    cnDoc.IsChecked = doc.IsChecked;
                //    cnDoc.IsReceived = doc.IsReceived;
                //    cnDoc.Remark = doc.Remark;
                //    if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                //        cnDoc.trxCNInvoiceHeaderID = trxCNInvoiceHeaderID;
                //    else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                //        cnDoc.trxCNInvoiceNonRevenueID = trxCNInvoiceHeaderID;
                //    else
                //        cnDoc.trxCNInvoiceHeaderRemainingAmountId = trxCNInvoiceHeaderID;
                //    cnDoc.UpdatedBy = doc.UpdatedBy;
                //    cnDoc.UpdatedDate = doc.UpdatedDate;
                //    _trxCNDocRepo.Create(cnDoc);
                //}

                #endregion

                #region Insert to logCNArActivity

                List<logArActivity> currentLogs = _logARActRepo.GetList(whereID);
                foreach (logArActivity currentLog in currentLogs)
                {
                    cnLog = new logCNArActivity();
                    cnLog.CreatedBy = currentLog.CreatedBy;
                    cnLog.CreatedDate = currentLog.CreatedDate;
                    cnLog.LogWeek = currentLog.LogWeek;
                    cnLog.mstInvoiceStatusId = currentLog.mstInvoiceStatusId;
                    cnLog.PICAReprintID = currentLog.PICAReprintID;
                    cnLog.ReprintRemarks = currentLog.ReprintRemarks;
                    cnLog.trxCNArDetailId = currentLog.trxArDetailId;
                    if (currentLog.trxBapsDataId == null || currentLog.trxBapsDataId == 0)
                        cnLog.trxBapsDataId = currentLog.trxBapsDataId;
                    else
                        cnLog.trxInvoiceManualID = currentLog.trxInvoiceManualID;
                    if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                        cnLog.trxCNInvoiceHeaderID = trxCNInvoiceHeaderID;
                    else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                        cnLog.trxCNInvoiceNonRevenueID = trxCNInvoiceHeaderID;
                    else
                        cnLog.trxCNInvoiceHeaderRemainingAmountID = trxCNInvoiceHeaderID;
                    cnLog = _logCNARActRepo.Create(cnLog);
                }

                #endregion

                #region Insert into mstCNTaxInvoice

                _uspGenTrxCNInvoiceRepo.CreateCNTax(trxCNInvoiceHeaderID, mstInvoiceCategoryId);
                //taxes = _mstTaxRepo.GetList(whereID);
                //foreach (mstTaxInvoice tax in taxes)
                //{
                //    cnTaxInvoice = new mstCNTaxInvoice();
                //    cnTaxInvoice.CreatedBy = tax.CreatedBy;
                //    cnTaxInvoice.CreatedDate = tax.CreatedDate;
                //    cnTaxInvoice.InvNo = tax.InvNo;
                //    cnTaxInvoice.TaxInvoiceNo = tax.TaxInvoiceNo;
                //    if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                //        cnTaxInvoice.TrxCNInvoiceHeaderID = trxCNInvoiceHeaderID;
                //    else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                //        cnTaxInvoice.trxCNInvoiceNonRevenueID = trxCNInvoiceHeaderID;
                //    else
                //        cnTaxInvoice.trxCNInvoiceHeaderRemainingAmountId = trxCNInvoiceHeaderID;
                //    cnTaxInvoice.UpdatedBy = tax.UpdatedBy;
                //    cnTaxInvoice.UpdatedDate = tax.UpdatedDate;
                //    _mstCNTaxRepo.Create(cnTaxInvoice);
                //}

                #endregion

                #region Update trxCancelNoteFinance

                trxCancelNoteFinance note = _trxCancelNoteRepo.GetList(whereID).FirstOrDefault();
                if (note != null)
                {
                    if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                        note.trxCNInvoiceHeaderID = trxCNInvoiceHeaderID;
                    else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                        note.trxCNInvoiceNonRevenueID = trxCNInvoiceHeaderID;
                    else
                        note.trxCNInvoiceHeaderRemainingAmountID = trxCNInvoiceHeaderID;
                    note.UpdatedBy = userID;
                    note.UpdatedDate = Helper.GetDateTimeNow();
                    note = _trxCancelNoteRepo.Update(note);
                }

                #endregion

                #region Delete from DocInvoiceDetail

                _trxDocRepo.DeleteByFilter(whereID);

                #endregion

                #region Delete logArActivity

                _logARActRepo.DeleteByFilter(whereID);

                #endregion

                #region Delete from mstTaxInvoice

                _mstTaxRepo.DeleteByFilter(whereID);

                #endregion

                #region Delete from trxPICAAR

                _trxPICAARRepo.DeleteByFilter(whereID);

                #endregion

                #region Delete from logArProcess

                _logARProcessRepo.DeleteByFilter(whereID);

                #endregion

                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                {
                    #region Delete Invoice
                    #region Delete from trxInvoiceTowerDetail

                    _trxInvoiceDetailRepo.DeleteByFilter(whereID);

                    #endregion

                    #region Delete from trxInvoiceHeader

                    _trxInvoiceHeaderRepo.DeleteByPK(trxInvoiceHeaderID);

                    #endregion
                    #endregion
                }
                else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                {
                    #region Delete Invoice Non Revenue
                    #region Delete from trxInvoiceTowerDetail

                    _trxInvNonRevenueSiteRepo.DeleteByFilter(whereID);

                    #endregion

                    #region Delete from trxInvoiceHeader

                    _trxInvNonRevenueRepo.DeleteByPK(trxInvoiceHeaderID);

                    #endregion
                    #endregion
                }
                else
                {
                    #region Delete Invoice Remaining
                    #region Delete from trxInvoiceTowerDetailRemaningAmount

                    _trxInvDetailRemainRepo.DeleteByFilter("trxInvoiceHeaderRemainingId = " + trxInvoiceHeaderID);

                    #endregion

                    #region Delete from trxInvoiceHeaderRemainingAmount

                    _trxInvoiceHeaderRemainRepo.DeleteByPK(trxInvoiceHeaderID);

                    #endregion
                    #endregion
                }

                return trxCNInvoiceHeaderID;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxApprovalCNInvoiceTowerService", "ApprovalCNInvoiceTower", userID);
                return 0;
            }
        }

        public vwCNInvoiceTower RejectCNInvoiceTower(string userID, int trxInvoiceHeaderID, int mstInvoiceCategoryId, string RejectRole)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                #region Update trxInvoiceHeader

                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                {
                    trxInvoiceHeader InvHeader = new trxInvoiceHeader();
                    InvHeader = _trxInvoiceHeaderRepo.GetByPK(trxInvoiceHeaderID);
                    InvHeader.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateReceiptUploaded;
                    InvHeader.UpdatedBy = userID;
                    InvHeader.UpdatedDate = Helper.GetDateTimeNow();
                    InvHeader = _trxInvoiceHeaderRepo.Update(InvHeader);
                }
                else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                {
                    trxInvoiceNonRevenue InvNonRevenue = new trxInvoiceNonRevenue();
                    InvNonRevenue = _trxInvNonRevenueRepo.GetByPK(trxInvoiceHeaderID);
                    InvNonRevenue.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateReceiptUploaded;
                    InvNonRevenue.UpdatedBy = userID;
                    InvNonRevenue.UpdatedDate = Helper.GetDateTimeNow();
                    InvNonRevenue = _trxInvNonRevenueRepo.Update(InvNonRevenue);
                }
                else
                {
                    trxInvoiceHeaderRemainingAmount InvRemain = new trxInvoiceHeaderRemainingAmount();
                    InvRemain = _trxInvoiceHeaderRemainRepo.GetByPK(trxInvoiceHeaderID);
                    InvRemain.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateReceiptUploaded;
                    InvRemain.UpdatedBy = userID;
                    InvRemain.UpdatedDate = Helper.GetDateTimeNow();
                    InvRemain = _trxInvoiceHeaderRemainRepo.Update(InvRemain);
                }

                #endregion

                trxInvoiceHeader header = new trxInvoiceHeader();
                header = _trxInvoiceHeaderRepo.GetByPK(trxInvoiceHeaderID);

                #region Delete trxPICAAR and trxCancelNoteFinance
                string deleteFilter = "";
                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                {
                    deleteFilter = "trxInvoiceHeaderID = ";
                }
                else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                {
                    deleteFilter = "trxInvoiceNonRevenueID = ";
                }
                else
                {
                    deleteFilter = "trxInvoiceHeaderRemainingAmountID = ";
                }

                _trxPICAARRepo.DeleteByFilter(deleteFilter + trxInvoiceHeaderID);
                _trxCancelNoteRepo.DeleteByFilter(deleteFilter + trxInvoiceHeaderID);
                #endregion

                #region Insert to logArActivity

                logArActivity logAr = new logArActivity();
                logAr.CreatedBy = userID;
                logAr.CreatedDate = Helper.GetDateTimeNow();
                logAr.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                if (RejectRole.ToLower() == "dept")
                    logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.DeptHeadRejectCNInvoice;
                else
                    logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.SPVRejectCNInvoice;
                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                    logAr.trxInvoiceHeaderID = trxInvoiceHeaderID;
                else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                    logAr.trxInvoiceNonRevenueID = trxInvoiceHeaderID;
                else
                    logAr.trxInvoiceHeaderRemainingAmountID = trxInvoiceHeaderID;
                logAr = _logARActRepo.Create(logAr);

                #endregion

                uow.SaveChanges();

                return new vwCNInvoiceTower();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new vwCNInvoiceTower((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxApprovalCNInvoiceTowerService", "RejectCNInvoiceTower", userID));
            }
        }
    }
}
