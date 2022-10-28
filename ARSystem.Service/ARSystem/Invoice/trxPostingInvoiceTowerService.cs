using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;

namespace ARSystem.Service.ARSystem.Invoice
{
    public class trxPostingInvoiceTowerService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;

        private vwDataCreatedInvoiceTowerRepository _vwCreatedInvoiceTowerRepo;
        private trxInvoiceHeaderRepository _trxInvoiceHeaderRepo;
        private trxInvoiceHeaderRemainingAmountRepository _trxInvoiceHeaderRemRepo;
        private logInvoiceActivityRepository _logInvoiceActRepo;
        private trxDocInvoiceDetailRepository _trxDocInvoiceRepo;
        private mstDocInvoiceRepository _mstDocInvoiceRepo;
        private logArActivityRepository _logARActRepo;
        private trxInvoiceNonRevenueRepository _trxInvoiceNonRevenueRepo;
        private trxInvoiceTowerDetailRepository _trxInvoiceTwrDetailRepo;
        private trxArDetailRepository _trxARDetailRepo;
        private trxArActivityLogRepository _trxARActLogRepo;
        private mstTaxInvoiceRepository _mstTaxInvoiceRepo;
        private trxBapsDataRepository _trxBapsRepo;
        private trxInvoiceManualRepository _trxInvManualRepo;
        private trxPICAARDataRepository _trxPICAARRepo;
        private trxArHeaderRepository _trxARHeaderRepo;
        private logInvoiceHeaderRepository _logInvHeaderRepo;
        private logInvoiceTowerDetailRepository _logInvTwrDetailRepo;
        private trxArDetailRemainingAmountRepository _trxARDetailRemainAmountRepo;
        private trxInvoiceTowerDetailRemaningAmountRepository _trxInvTwrDetailRemainAmountRepo;
        private logInvoiceHeaderRemainingAmountRepository _logInvHeaderRemainAmountRepo;
        private logInvoiceTowerDetailRemainingAmountRepository _logInvTwrDetailRemainAmountRepo;
        private logInvoiceNonRevenueRepository _logInvNonRevenueRepo;

        private trxPrintInvoiceTowerService _trxPrintInvoiceTowerService;

        public trxPostingInvoiceTowerService() : base()
        {
            _context = this.SetContext();
            _dtNow = Helper.GetDateTimeNow();

            _vwCreatedInvoiceTowerRepo = new vwDataCreatedInvoiceTowerRepository(_context);
            _trxInvoiceHeaderRepo = new trxInvoiceHeaderRepository(_context);
            _trxInvoiceHeaderRemRepo = new trxInvoiceHeaderRemainingAmountRepository(_context);
            _logInvoiceActRepo = new logInvoiceActivityRepository(_context);
            _trxDocInvoiceRepo = new trxDocInvoiceDetailRepository(_context);
            _mstDocInvoiceRepo = new mstDocInvoiceRepository(_context);
            _logARActRepo = new logArActivityRepository(_context);
            _trxInvoiceNonRevenueRepo = new trxInvoiceNonRevenueRepository(_context);
            _trxInvoiceTwrDetailRepo = new trxInvoiceTowerDetailRepository(_context);
            _trxARDetailRepo = new trxArDetailRepository(_context);
            _trxARActLogRepo = new trxArActivityLogRepository(_context);
            _mstTaxInvoiceRepo = new mstTaxInvoiceRepository(_context);
            _trxBapsRepo = new trxBapsDataRepository(_context);
            _trxInvManualRepo = new trxInvoiceManualRepository(_context);
            _trxPICAARRepo = new trxPICAARDataRepository(_context);
            _trxARHeaderRepo = new trxArHeaderRepository(_context);
            _logInvHeaderRepo = new logInvoiceHeaderRepository(_context);
            _logInvTwrDetailRepo = new logInvoiceTowerDetailRepository(_context);
            _trxARDetailRemainAmountRepo = new trxArDetailRemainingAmountRepository(_context);
            _trxInvTwrDetailRemainAmountRepo = new trxInvoiceTowerDetailRemaningAmountRepository(_context);
            _logInvHeaderRemainAmountRepo = new logInvoiceHeaderRemainingAmountRepository(_context);
            _logInvTwrDetailRemainAmountRepo = new logInvoiceTowerDetailRemainingAmountRepository(_context);
            _logInvNonRevenueRepo = new logInvoiceNonRevenueRepository(_context);

            _trxPrintInvoiceTowerService = new trxPrintInvoiceTowerService();
        }

        public int GetTrxPostingInvoiceTowerCount(string userID, string strCompanyId, string strOperator, string strInvoiceType, int mstInvoiceStatusId, string invNo, int InvoiceManual)
        {
            List<vwDataCreatedInvoiceTower> listDataCreatedInvoiceTower = new List<vwDataCreatedInvoiceTower>();

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(strCompanyId))
                {
                    strWhereClause += "Company = '" + strCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += "Operator = '" + strOperator + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strInvoiceType))
                {
                    strWhereClause += "InvoiceTypeId = '" + strInvoiceType + "' AND ";
                }
                if (mstInvoiceStatusId != -1)
                {
                    strWhereClause += "mstInvoiceStatusId = '" + mstInvoiceStatusId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(invNo))
                {
                    strWhereClause += "InvNo LIKE '%" + invNo + "%' AND ";
                }
                //CHECK IF USER IS AR DEPT HEAD OR NOT
                if (UserHelper.GetUserARPosition(userID) == "DEPT HEAD")
                {
                    strWhereClause += "mstInvoiceStatusId IN (" + (int)StatusHelper.InvoiceStatus.StateCancelApproved +
                        "," + (int)StatusHelper.InvoiceStatus.StateWaitingCancel + ") AND ";//SHOW WAITING FOR APPROVAL AND APPROVED INVOICE
                }
                else
                {
                    strWhereClause += "mstInvoiceStatusId IN (" + (int)StatusHelper.InvoiceStatus.StateCreated
                        + "," + (int)StatusHelper.InvoiceStatus.StateCreated15
                        + "," + (int)StatusHelper.InvoiceStatus.StateWaitingCancel
                        + "," + (int)StatusHelper.InvoiceStatus.StateCNInvoicePrintApproved + ") AND ";//SHOW WAITING FOR APPROVAL , CREATED INVOICE, AND CN FROM PRINT
                }
                //Tambahan Field Inv.Manual
                if (InvoiceManual != -1)
                {
                    strWhereClause += "InvoiceManual = " + InvoiceManual + " AND ";
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return _vwCreatedInvoiceTowerRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxPostingInvoiceTowerService", "GetTrxPostingInvoiceTowerCount", userID);
                return 0;
            }
        }

        public List<vwDataCreatedInvoiceTower> GetTrxPostingInvoiceTowerToList(string userID, string strCompanyId, string strOperator, string strInvoiceType, int mstInvoiceStatusId, string invNo, int InvoiceManual, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            List<vwDataCreatedInvoiceTower> listDataCreatedInvoiceTower = new List<vwDataCreatedInvoiceTower>();

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(strCompanyId))
                {
                    strWhereClause += "Company = '" + strCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += "Operator = '" + strOperator + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strInvoiceType))
                {
                    strWhereClause += "InvoiceTypeId = '" + strInvoiceType + "' AND ";
                }
                if (mstInvoiceStatusId != -1)
                {
                    strWhereClause += "mstInvoiceStatusId = '" + mstInvoiceStatusId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(invNo))
                {
                    strWhereClause += "InvNo LIKE '%" + invNo + "%' AND ";
                }
                //CHECK IF USER IS AR DEPT HEAD OR NOT
                if (UserHelper.GetUserARPosition(userID) == "DEPT HEAD")
                {
                    strWhereClause += "mstInvoiceStatusId IN (" + (int)StatusHelper.InvoiceStatus.StateCancelApproved +
                        "," + (int)StatusHelper.InvoiceStatus.StateWaitingCancel + ") AND ";//SHOW WAITING FOR APPROVAL AND APPROVED INVOICE
                }
                else
                {
                    strWhereClause += "mstInvoiceStatusId IN (" + (int)StatusHelper.InvoiceStatus.StateCreated
                        + "," + (int)StatusHelper.InvoiceStatus.StateCreated15
                        + "," + (int)StatusHelper.InvoiceStatus.StateWaitingCancel
                        + "," + (int)StatusHelper.InvoiceStatus.StateCNInvoicePrintApproved + ") AND ";//SHOW WAITING FOR APPROVAL , CREATED INVOICE, AND CN FROM PRINT
                }
                //Tambahan Field Inv.Manual
                if (InvoiceManual != -1)
                {
                    strWhereClause += "InvoiceManual = " + InvoiceManual + " AND ";
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (intPageSize > 0)
                    listDataCreatedInvoiceTower = _vwCreatedInvoiceTowerRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    listDataCreatedInvoiceTower = _vwCreatedInvoiceTowerRepo.GetList(strWhereClause, strOrderBy);

                return listDataCreatedInvoiceTower;
            }
            catch (Exception ex)
            {
                listDataCreatedInvoiceTower.Add(new vwDataCreatedInvoiceTower((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxPostingInvoiceTowerService", "GetTrxPostingInvoiceTowerToList", userID)));
                return listDataCreatedInvoiceTower;
            }
        }

        public trxInvoiceHeader PostingInvoiceTower(string userID, string InvoiceDate, string Subject, string OperatorRegionId, string Signature, vwDataCreatedInvoiceTower DataCreatedInvoiceTower, string AdditionalNote)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                #region 'Update InvoiceHeader Status'
                string operatorId = string.Empty;
                int invoiceHeaderId = 0;
                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower) //IF INVOICE IS TOWER
                {
                    trxInvoiceHeader InvoiceHeader = _trxInvoiceHeaderRepo.GetByPK(DataCreatedInvoiceTower.trxInvoiceHeaderID.Value);
                    InvoiceHeader.InvPrintDate = DateTime.Parse(InvoiceDate);
                    InvoiceHeader.InvSubject = Subject;
                    InvoiceHeader.InvOprRegionID = OperatorRegionId;
                    InvoiceHeader.InvFARSignature = Signature;
                    InvoiceHeader.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StatePosted; //STATE POSTED, NOT SHOWN IN POSTING MENU
                    InvoiceHeader.InvAdditionalNote = AdditionalNote;
                    InvoiceHeader.UpdatedBy = userID;
                    InvoiceHeader.UpdatedDate = _dtNow;
                    InvoiceHeader = _trxInvoiceHeaderRepo.Update(InvoiceHeader);
                    operatorId = InvoiceHeader.InvOperatorID;
                    invoiceHeaderId = InvoiceHeader.trxInvoiceHeaderID;
                }
                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower15 || DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower10)
                {
                    trxInvoiceHeaderRemainingAmount InvoiceHeaderRemaining = _trxInvoiceHeaderRemRepo.GetByPK(DataCreatedInvoiceTower.trxInvoiceHeaderID.Value);
                    InvoiceHeaderRemaining.InvPrintDate = DateTime.Parse(InvoiceDate);
                    InvoiceHeaderRemaining.InvSubject = Subject;
                    InvoiceHeaderRemaining.InvOprRegionID = OperatorRegionId;
                    InvoiceHeaderRemaining.InvFARSignature = Signature;
                    InvoiceHeaderRemaining.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StatePosted; //STATE POSTED, NOT SHOWN IN POSTING MENU
                    InvoiceHeaderRemaining.InvAdditionalNote = AdditionalNote;
                    InvoiceHeaderRemaining.UpdatedBy = userID;
                    InvoiceHeaderRemaining.UpdatedDate = _dtNow;
                    InvoiceHeaderRemaining = _trxInvoiceHeaderRemRepo.Update(InvoiceHeaderRemaining);
                    operatorId = InvoiceHeaderRemaining.InvOperatorID;
                    invoiceHeaderId = InvoiceHeaderRemaining.trxInvoiceHeaderRemainingAmountID;
                }
                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue) //IF INVOICE IS NON REVENUE
                {
                    trxInvoiceNonRevenue InvoiceNonRevenue = _trxInvoiceNonRevenueRepo.GetByPK(DataCreatedInvoiceTower.trxInvoiceHeaderID.Value);
                    InvoiceNonRevenue.InvPrintDate = DateTime.Parse(InvoiceDate);
                    InvoiceNonRevenue.InvSubject = Subject;
                    InvoiceNonRevenue.InvOprRegionID = OperatorRegionId;
                    InvoiceNonRevenue.InvFARSignature = Signature;
                    InvoiceNonRevenue.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StatePosted; //STATE POSTED, NOT SHOWN IN POSTING MENU
                    InvoiceNonRevenue.InvAdditionalNote = AdditionalNote;
                    InvoiceNonRevenue.UpdatedBy = userID;
                    InvoiceNonRevenue.UpdatedDate = _dtNow;
                    InvoiceNonRevenue = _trxInvoiceNonRevenueRepo.Update(InvoiceNonRevenue);
                    operatorId = InvoiceNonRevenue.OperatorID;
                    invoiceHeaderId = InvoiceNonRevenue.trxInvoiceNonRevenueID;
                }
                #endregion

                #region 'INSERT InvoiceActivityLog'
                var logInvoiceActivity = new logInvoiceActivity();
                logInvoiceActivity.InvNo = DataCreatedInvoiceTower.InvNo;
                logInvoiceActivity.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.Posting;//POSTING
                logInvoiceActivity.LogDate = _dtNow;
                logInvoiceActivity = _logInvoiceActRepo.Create(logInvoiceActivity);
                #endregion

                #region 'INSERT LOGARACTIVITY'
                logArActivity logAr = new logArActivity();
                logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.Posting;
                logAr.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                {
                    logAr.trxInvoiceHeaderID = DataCreatedInvoiceTower.trxInvoiceHeaderID;
                }
                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower15 || DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower10)
                {
                    logAr.trxInvoiceHeaderRemainingAmountID = DataCreatedInvoiceTower.trxInvoiceHeaderID;
                }
                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                {
                    logAr.trxInvoiceNonRevenueID = DataCreatedInvoiceTower.trxInvoiceHeaderID;
                }
                logAr.CreatedBy = userID;
                logAr.CreatedDate = _dtNow;
                logAr = _logARActRepo.Create(logAr);
                #endregion

                #region Iinsert into trxDocInvoiceDetail

                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower15 || DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower10)
                    _trxDocInvoiceRepo.DeleteByFilter("trxInvoiceHeaderRemainingAmountId = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                else if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                    _trxDocInvoiceRepo.DeleteByFilter("trxInvoiceHeaderID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                else
                    _trxDocInvoiceRepo.DeleteByFilter("trxInvoiceNonRevenueID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);

                List<mstDocInvoice> documents = _mstDocInvoiceRepo.GetList("OperatorID = '" + operatorId + "'");
                trxDocInvoiceDetail trxDoc;
                foreach (mstDocInvoice document in documents)
                {
                    trxDoc = new trxDocInvoiceDetail();
                    if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower15 || DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower10)
                    {
                        trxDoc.trxInvoiceHeaderRemainingAmountId = invoiceHeaderId;
                    }
                    else if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                    {
                        trxDoc.trxInvoiceHeaderID = invoiceHeaderId;
                    }
                    else
                    {
                        trxDoc.trxInvoiceNonRevenueID = invoiceHeaderId;
                    }
                    trxDoc.DocInvoiceID = document.DocInvoiceID;
                    trxDoc.CreatedBy = userID;
                    trxDoc.CreatedDate = _dtNow;
                    _trxDocInvoiceRepo.Create(trxDoc);
                }

                #endregion

                uow.SaveChanges();
                //For Returning InvNo to View
                trxInvoiceHeader ReturnInvNo = new trxInvoiceHeader();
                ReturnInvNo.InvNo = DataCreatedInvoiceTower.InvNo;
                return ReturnInvNo;

            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxInvoiceHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxPostingInvoiceTowerService", "PostingInvoiceTower", userID));
            }
        }

        public trxInvoiceHeader CancelPosting(string userID, vwDataCreatedInvoiceTower DataCreatedInvoiceTower, string RemarksCancel, int mstPICATypeID, int mstPICADetailID)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                #region 'Insert Into trxPICAARData'
                string InsertResult = "";
                vmGetInvoicePostedList vm = new vmGetInvoicePostedList();
                List<int> ListHeaderID = new List<int>();
                List<int> ListCategoryID = new List<int>();
                ListHeaderID.Add(DataCreatedInvoiceTower.trxInvoiceHeaderID.Value);
                ListCategoryID.Add(DataCreatedInvoiceTower.mstInvoiceCategoryId.Value);
                vm.HeaderId = ListHeaderID;
                vm.CategoryId = ListCategoryID;
                InsertResult = _trxPrintInvoiceTowerService.InsertTrxPICAARData(vm, RemarksCancel, mstPICATypeID, mstPICADetailID, userID, (int)StatusHelper.InvoiceStatus.Posting);
                if (!string.IsNullOrEmpty(InsertResult))
                    return new trxInvoiceHeader((int)Helper.ErrorType.Error, Helper.logError(InsertResult, "trxPostingInvoiceTowerService", "InsertTrxPICAARData", userID));
                #endregion

                #region 'Update InvoiceHeader Status'
                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)//IF INVOICE IS TOWER
                {
                    trxInvoiceHeader InvoiceHeader = new trxInvoiceHeader();
                    InvoiceHeader = _trxInvoiceHeaderRepo.GetByPK(DataCreatedInvoiceTower.trxInvoiceHeaderID.Value);
                    InvoiceHeader.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateWaitingCancel; // WAITING FOR DEPT HEAD APPROVAL
                    InvoiceHeader.ReturnToInvStatus = (int)StatusHelper.InvoiceStatus.StateCreated;
                    InvoiceHeader.InvRemarksPosting = RemarksCancel;
                    InvoiceHeader.UpdatedBy = userID;
                    InvoiceHeader.UpdatedDate = _dtNow;
                    InvoiceHeader = _trxInvoiceHeaderRepo.Update(InvoiceHeader);
                }
                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower15 || DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower10)
                {
                    trxInvoiceHeaderRemainingAmount InvoiceHeaderRemaining = new trxInvoiceHeaderRemainingAmount();
                    InvoiceHeaderRemaining = _trxInvoiceHeaderRemRepo.GetByPK(DataCreatedInvoiceTower.trxInvoiceHeaderID.Value);
                    InvoiceHeaderRemaining.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateWaitingCancel; // WAITING FOR DEPT HEAD APPROVAL
                    InvoiceHeaderRemaining.ReturnToInvStatus = (int)StatusHelper.InvoiceStatus.StateCreated;
                    InvoiceHeaderRemaining.InvRemarksPosting = RemarksCancel;
                    InvoiceHeaderRemaining.UpdatedBy = userID;
                    InvoiceHeaderRemaining.UpdatedDate = _dtNow;
                    InvoiceHeaderRemaining = _trxInvoiceHeaderRemRepo.Update(InvoiceHeaderRemaining);
                }
                #endregion

                #region 'INSERT InvoiceActivityLog'
                var logInvoiceActivity = new logInvoiceActivity();
                logInvoiceActivity.InvNo = DataCreatedInvoiceTower.InvNo;
                logInvoiceActivity.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.CancelInvoice;//CANCEL INVOICE
                logInvoiceActivity.LogDate = _dtNow;
                logInvoiceActivity = _logInvoiceActRepo.Create(logInvoiceActivity);
                #endregion

                uow.SaveChanges();

                return new trxInvoiceHeader();

            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxInvoiceHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxPostingInvoiceTowerService", "CancelInvoiceTower", userID));
            }
        }

        public trxInvoiceHeader ApproveCancelPosting(string userID, vwDataCreatedInvoiceTower DataCreatedInvoiceTower)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                //UPDATE trxArDetail filter by SONumber,BapsPeriod,Currency,BapsNo
                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                {
                    #region 'Update ArDetail Status'
                    List<trxInvoiceTowerDetail> ListInvoiceTowerDetail = new List<trxInvoiceTowerDetail>();
                    trxPICAARData PicaARData = new trxPICAARData();
                    trxBapsData bapsData = new trxBapsData();
                    var trxBapsDataManual = new trxInvoiceManual();
                    ListInvoiceTowerDetail = _trxInvoiceTwrDetailRepo.GetList("trxInvoiceHeaderID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                    //Get PicaType to Check if it is Pica External or Internal
                    PicaARData = _trxPICAARRepo.GetList("trxInvoiceHeaderID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID + " AND mstInvoiceState = " + (int)StatusHelper.InvoiceStatus.Posting).FirstOrDefault();
                    foreach (trxInvoiceTowerDetail TowerDetail in ListInvoiceTowerDetail)
                    {
                        trxArDetail ArDetail = new trxArDetail();
                        string filter = "";
                        filter = "[SONumber]='" + TowerDetail.SONumber + "' AND [BapsNo]='" + TowerDetail.BapsNo + "'" +
                            "AND [BapsType] = '" + TowerDetail.BapsType + "' AND [StipSiro]='" + TowerDetail.StipSiro + "'" +
                            "AND [BapsPeriod]='" + TowerDetail.BapsPeriod + "' AND [Currency]='" + DataCreatedInvoiceTower.Currency + "'";
                        ArDetail = _trxARDetailRepo.GetList(filter, "").FirstOrDefault();

                        //if PICA Type is Internal, Data back to BapsConfirm
                        if (PicaARData.mstPICATypeID == (int)Constants.PICATypeIDARData.ReturnProcessInternal)
                        {
                            ArDetail.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.NotProcessed; //SET BACK TO WHEN BAPS CONFIRMED 
                            ArDetail.UpdatedBy = userID;
                            ArDetail.UpdatedDate = _dtNow;
                            ArDetail = _trxARDetailRepo.Update(ArDetail);

                            #region 'INSERT ARActivityLog'
                            trxArActivityLog ARActivityLog = new trxArActivityLog();
                            ARActivityLog.TrxArHeaderId = ArDetail.TrxArHeaderId;
                            ARActivityLog.Action = "CANCEL INVOICE";
                            ARActivityLog.CreatedBy = userID;
                            ARActivityLog.CreatedDate = _dtNow;
                            ARActivityLog = _trxARActLogRepo.Create(ARActivityLog);
                            #endregion
                        }
                        //if PICA Type is External,Delete AR Detail Log, trxARHeader and trxARDetail, Data back to BapsConfirm Or trxInvoiceManual (Baps Confirm for inv Manual)
                        else if (PicaARData.mstPICATypeID == (int)Constants.PICATypeIDARData.ReturnProcessExternal)
                        {
                            //Delete ArDetail Log,ArDetail and ArHeader
                            _logARActRepo.DeleteByFilter("trxArDetailId = " + ArDetail.trxArDetailId);
                            _trxARHeaderRepo.DeleteByPK(ArDetail.TrxArHeaderId);
                            _trxARDetailRepo.DeleteByPK(ArDetail.trxArDetailId);

                            //Delete BAPS Data Log (Except Receive State) and update BAPS to Received State
                            if (ArDetail.InvoiceManual == false || ArDetail.InvoiceManual == null)
                            {
                                filter = "[SONumber]='" + TowerDetail.SONumber + "' AND [BapsNo]='" + TowerDetail.BapsNo + "'" +
                                "AND [BapsType] = '" + TowerDetail.BapsType + "' AND [StipSiro]='" + TowerDetail.StipSiro + "'" +
                                "AND [BapsPeriod]='" + TowerDetail.BapsPeriod + "' AND [Currency]='" + DataCreatedInvoiceTower.Currency + "'";
                                bapsData = _trxBapsRepo.GetList(filter).FirstOrDefault();

                                bapsData.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateBAPSReceive;
                                bapsData.UpdatedBy = userID;
                                bapsData.UpdatedDate = Helper.GetDateTimeNow();
                                _trxBapsRepo.Update(bapsData);
                                _logARActRepo.DeleteByFilter("trxBapsDataId = " + bapsData.trxBapsDataId + " AND mstInvoiceStatusId <> " + (int)StatusHelper.InvoiceStatus.BAPSReceive);
                            }
                            else
                            {
                                //filter = "[SONumber]='" + ArDetail.SONumber + "' AND [BapsNO]='" + ArDetail.BapsNo + "'" +
                                //" AND [BapsType] = '" + ArDetail.BapsType + "' AND [StipSiro]='" + ArDetail.StipSiroId + "'" +
                                //" AND [BapsPeriod]='" + ArDetail.BapsPeriod + "' AND [PriceCurrency]='" + ArDetail.Currency + "'" +
                                //" AND [InitialPONumber]='" + ArDetail.PoNumber + "'";
                                filter = "[SONumber]='" + ArDetail.SONumber + "' AND [BapsNO]='" + ArDetail.BapsNo + "'" +
                            " AND [BapsType] = '" + ArDetail.BapsType + "' AND [InvoiceEndDate]='" + ArDetail.EndDateInvoice + "'" +
                            " AND [InvoiceStartDate] ='" + ArDetail.StartDateInvoice + "' AND [PriceCurrency]='" + ArDetail.Currency + "'";

                                trxBapsDataManual = _trxInvManualRepo.GetList(filter, "")[0];

                                trxBapsDataManual.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateBAPSReceive;//STATE BAPS RECEIVED IN TRXBAPS DATA
                                trxBapsDataManual.UpdatedBy = userID;
                                trxBapsDataManual.UpdatedDate = _dtNow;
                                trxBapsDataManual = _trxInvManualRepo.Update(trxBapsDataManual);
                                _logARActRepo.DeleteByFilter("trxInvoiceManualID = " + trxBapsDataManual.trxInvoiceManualID + " AND mstInvoiceStatusId <> " + (int)StatusHelper.InvoiceStatus.BAPSReceive);
                            }

                        }

                    }

                    #endregion

                    #region 'Insert Invoice to Log and Delete from Invoice'
                    trxInvoiceHeader InvoiceHeader = new trxInvoiceHeader();
                    InvoiceHeader = _trxInvoiceHeaderRepo.GetList("trxInvoiceHeaderID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID)[0];

                    logInvoiceHeader logInvoiceHeader = new logInvoiceHeader();
                    logInvoiceHeader.trxInvoiceHeaderID = InvoiceHeader.trxInvoiceHeaderID;
                    logInvoiceHeader.InvNo = InvoiceHeader.InvNo;
                    logInvoiceHeader.InvTemp = InvoiceHeader.InvTemp;
                    logInvoiceHeader.InvOperatorID = InvoiceHeader.InvOperatorID;
                    logInvoiceHeader.InvOperatorAsset = InvoiceHeader.InvOperatorAsset;
                    logInvoiceHeader.InvCompanyInvoice = InvoiceHeader.InvCompanyInvoice;
                    logInvoiceHeader.InvCompanyId = InvoiceHeader.InvCompanyId;
                    logInvoiceHeader.InvSumADPP = InvoiceHeader.InvSumADPP;
                    logInvoiceHeader.InvTotalAmount = InvoiceHeader.InvTotalAmount;
                    logInvoiceHeader.InvoiceTypeId = InvoiceHeader.InvoiceTypeId;
                    logInvoiceHeader.InvTotalAPPH = InvoiceHeader.InvTotalAPPH;
                    logInvoiceHeader.InvTotalAPPN = InvoiceHeader.InvTotalAPPN;
                    logInvoiceHeader.InvIDPaymentBank = InvoiceHeader.InvIDPaymentBank;
                    logInvoiceHeader.InvTotalPenalty = InvoiceHeader.InvTotalPenalty;
                    logInvoiceHeader.InvIsAx = InvoiceHeader.InvIsAx;
                    logInvoiceHeader.Currency = InvoiceHeader.Currency;
                    logInvoiceHeader.InvIsParent = InvoiceHeader.InvIsParent;
                    logInvoiceHeader.mstInvoiceCategoryId = InvoiceHeader.mstInvoiceCategoryId.Value;
                    logInvoiceHeader.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateCancelApproved;//STATE REJECT APPROVED SHOW IN DEPT PAGE
                    logInvoiceHeader.InvRemarksPosting = InvoiceHeader.InvRemarksPosting;
                    logInvoiceHeader.InvAdditionalNote = InvoiceHeader.InvAdditionalNote;
                    logInvoiceHeader.InvoiceManual = InvoiceHeader.InvoiceManual;
                    logInvoiceHeader.CreatedBy = userID;
                    logInvoiceHeader.CreatedDate = _dtNow;
                    _logInvHeaderRepo.Create(logInvoiceHeader);

                    List<logInvoiceTowerDetail> listLogInvoiceTowerDetail = new List<logInvoiceTowerDetail>();
                    foreach (trxInvoiceTowerDetail InvoiceTowerDetail in ListInvoiceTowerDetail)
                    {
                        logInvoiceTowerDetail logInvoiceTowerDetail = new logInvoiceTowerDetail();
                        logInvoiceTowerDetail.trxInvoiceTowerDetailId = InvoiceTowerDetail.trxInvoiceTowerDetailId;
                        logInvoiceTowerDetail.trxInvoiceHeaderID = InvoiceTowerDetail.trxInvoiceHeaderID;
                        logInvoiceTowerDetail.SONumber = InvoiceTowerDetail.SONumber;
                        logInvoiceTowerDetail.SiteIdOld = InvoiceTowerDetail.SiteIdOld;
                        logInvoiceTowerDetail.SiteName = InvoiceTowerDetail.SiteName;
                        logInvoiceTowerDetail.SpkDate = InvoiceTowerDetail.SpkDate;
                        logInvoiceTowerDetail.RfiDate = InvoiceTowerDetail.RfiDate;
                        logInvoiceTowerDetail.BapsPeriod = InvoiceTowerDetail.BapsPeriod;
                        logInvoiceTowerDetail.PoNumber = InvoiceTowerDetail.PoNumber;
                        logInvoiceTowerDetail.Type = InvoiceTowerDetail.Type;
                        logInvoiceTowerDetail.BapsNo = InvoiceTowerDetail.BapsNo;
                        logInvoiceTowerDetail.PeriodInvoice = InvoiceTowerDetail.PeriodInvoice;
                        logInvoiceTowerDetail.BapsType = InvoiceTowerDetail.BapsType;
                        logInvoiceTowerDetail.StipSiro = InvoiceTowerDetail.StipSiro;
                        logInvoiceTowerDetail.StipSiroId = InvoiceTowerDetail.StipSiroId;
                        logInvoiceTowerDetail.PowerType = InvoiceTowerDetail.PowerType;
                        logInvoiceTowerDetail.PowerTypeCode = InvoiceTowerDetail.PowerTypeCode;
                        logInvoiceTowerDetail.StartDatePeriod = InvoiceTowerDetail.StartDatePeriod;
                        logInvoiceTowerDetail.EndDatePeriod = InvoiceTowerDetail.EndDatePeriod;
                        logInvoiceTowerDetail.StartDateRent = InvoiceTowerDetail.StartDateRent;
                        logInvoiceTowerDetail.EndDateRent = InvoiceTowerDetail.EndDateRent;
                        logInvoiceTowerDetail.AmountRental = InvoiceTowerDetail.AmountRental;
                        logInvoiceTowerDetail.AmountService = InvoiceTowerDetail.AmountService;
                        logInvoiceTowerDetail.AmountInvoicePeriod = InvoiceTowerDetail.AmountInvoicePeriod;
                        logInvoiceTowerDetail.AmountPenaltyPeriod = InvoiceTowerDetail.AmountPenaltyPeriod;
                        logInvoiceTowerDetail.AmountOverdaya = InvoiceTowerDetail.AmountOverdaya;
                        logInvoiceTowerDetail.AmountOverblast = InvoiceTowerDetail.AmountOverblast;
                        logInvoiceTowerDetail.AmountPPN = InvoiceTowerDetail.AmountPPN;
                        logInvoiceTowerDetail.AmountLossPPN = InvoiceTowerDetail.AmountLossPPN;
                        logInvoiceTowerDetail.SiteIdOpr = InvoiceTowerDetail.SiteIdOpr;
                        logInvoiceTowerDetail.SiteNameOpr = InvoiceTowerDetail.SiteNameOpr;
                        logInvoiceTowerDetail.ContractNumber = InvoiceTowerDetail.ContractNumber;
                        logInvoiceTowerDetail.ReslipNumber = InvoiceTowerDetail.ReslipNumber;
                        logInvoiceTowerDetail.CreatedBy = userID;
                        logInvoiceTowerDetail.CreatedDate = _dtNow;
                        listLogInvoiceTowerDetail.Add(logInvoiceTowerDetail);
                    }
                    _logInvTwrDetailRepo.CreateBulky(listLogInvoiceTowerDetail);

                    _trxInvoiceTwrDetailRepo.DeleteByFilter("trxInvoiceHeaderID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                    _trxInvoiceHeaderRepo.DeleteByFilter("trxInvoiceHeaderID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                    #endregion

                    #region Delete from mstTaxInvoice
                    _mstTaxInvoiceRepo.DeleteByFilter("TrxInvoiceHeaderID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                    #endregion

                    #region Delete From trxDocInvoiceDetail
                    _trxDocInvoiceRepo.DeleteByFilter("trxInvoiceHeaderID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                    #endregion
                }
                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower15 || DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower10)
                {
                    #region 'Update ArDetail Remaining Status'
                    List<trxInvoiceTowerDetailRemaningAmount> ListInvoiceTowerDetailRemaining = new List<trxInvoiceTowerDetailRemaningAmount>();
                    trxPICAARData PicaARData = new trxPICAARData();
                    ListInvoiceTowerDetailRemaining = _trxInvTwrDetailRemainAmountRepo.GetList("trxInvoiceHeaderRemainingID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                    //Get PicaType to Check if it is Pica External or Internal
                    PicaARData = _trxPICAARRepo.GetList("trxInvoiceHeaderRemainingAmountID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID + " AND mstInvoiceState = " + (int)StatusHelper.InvoiceStatus.Posting).FirstOrDefault();
                    foreach (trxInvoiceTowerDetailRemaningAmount TowerDetail in ListInvoiceTowerDetailRemaining)
                    {
                        trxArDetailRemainingAmount ArDetail = new trxArDetailRemainingAmount();
                        string filter = "";
                        filter = "[SONumber]='" + TowerDetail.SONumber + "' AND [BapsNo]='" + TowerDetail.BapsNo + "'" +
                            "AND [BapsType] = '" + TowerDetail.BapsType + "' AND [StipSiro]='" + TowerDetail.StipSiro + "'" +
                            "AND [BapsPeriod]='" + TowerDetail.BapsPeriod + "' AND [Currency]='" + DataCreatedInvoiceTower.Currency + "'";
                        ArDetail = _trxARDetailRemainAmountRepo.GetList(filter, "").FirstOrDefault();
                        //if PICA Type is Internal, Data back to BapsConfirm
                        if (PicaARData.mstPICATypeID == (int)Constants.PICATypeIDARData.ReturnProcessInternal)
                        {
                            ArDetail.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.NotProcessed;
                            ArDetail.UpdatedBy = userID;
                            ArDetail.UpdatedDate = _dtNow;
                            ArDetail = _trxARDetailRemainAmountRepo.Update(ArDetail);
                        }
                        //if PICA Type is External,Delete AR Detail Log, trxARHeader and trxARDetail, Data back to BapsConfirm
                        else if (PicaARData.mstPICATypeID == (int)Constants.PICATypeIDARData.ReturnProcessExternal)
                        {
                            //Delete ArDetail Log,ArDetail 
                            _logARActRepo.DeleteByFilter("trxArDetailRemainingAmountId = " + ArDetail.trxArDetailId);
                            _trxARDetailRemainAmountRepo.DeleteByPK(ArDetail.trxArDetailId);

                        }
                    }

                    #endregion

                    #region 'Insert Invoice Remaining to Log and Delete from Invoice'
                    trxInvoiceHeaderRemainingAmount InvoiceHeaderRemaining = new trxInvoiceHeaderRemainingAmount();
                    InvoiceHeaderRemaining = _trxInvoiceHeaderRemRepo.GetList("trxInvoiceHeaderRemainingAmountID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID)[0];

                    logInvoiceHeaderRemainingAmount logInvoiceHeader = new logInvoiceHeaderRemainingAmount();
                    logInvoiceHeader.trxInvoiceHeaderRemainingAmountID = InvoiceHeaderRemaining.trxInvoiceHeaderRemainingAmountID;
                    logInvoiceHeader.InvNo = InvoiceHeaderRemaining.InvNo;
                    logInvoiceHeader.InvTemp = InvoiceHeaderRemaining.InvTemp;
                    logInvoiceHeader.InvOperatorID = InvoiceHeaderRemaining.InvOperatorID;
                    logInvoiceHeader.InvOperatorAsset = InvoiceHeaderRemaining.InvOperatorAsset;
                    logInvoiceHeader.InvCompanyInvoice = InvoiceHeaderRemaining.InvCompanyInvoice;
                    logInvoiceHeader.InvCompanyId = InvoiceHeaderRemaining.InvCompanyId;
                    logInvoiceHeader.InvSumADPP = InvoiceHeaderRemaining.InvSumADPP;
                    logInvoiceHeader.InvTotalAmount = InvoiceHeaderRemaining.InvTotalAmount;
                    logInvoiceHeader.InvoiceTypeId = InvoiceHeaderRemaining.InvoiceTypeId;
                    logInvoiceHeader.InvTotalAPPH = InvoiceHeaderRemaining.InvTotalAPPH;
                    logInvoiceHeader.InvTotalAPPN = InvoiceHeaderRemaining.InvTotalAPPN;
                    logInvoiceHeader.InvIDPaymentBank = InvoiceHeaderRemaining.InvIDPaymentBank;
                    logInvoiceHeader.InvTotalPenalty = InvoiceHeaderRemaining.InvTotalPenalty;
                    logInvoiceHeader.InvIsAx = InvoiceHeaderRemaining.InvIsAx;
                    logInvoiceHeader.Currency = InvoiceHeaderRemaining.Currency;
                    logInvoiceHeader.InvIsParent = InvoiceHeaderRemaining.InvIsParent;
                    logInvoiceHeader.mstInvoiceCategoryId = InvoiceHeaderRemaining.mstInvoiceCategoryId.Value;
                    logInvoiceHeader.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateCancelApproved;//STATE REJECT APPROVED SHOW IN DEPT PAGE
                    logInvoiceHeader.InvRemarksPosting = InvoiceHeaderRemaining.InvRemarksPosting;
                    logInvoiceHeader.InvAdditionalNote = InvoiceHeaderRemaining.InvAdditionalNote;
                    logInvoiceHeader.CreatedBy = userID;
                    logInvoiceHeader.CreatedDate = _dtNow;
                    _logInvHeaderRemainAmountRepo.Create(logInvoiceHeader);

                    List<logInvoiceTowerDetailRemainingAmount> listLogInvoiceTowerDetail = new List<logInvoiceTowerDetailRemainingAmount>();
                    foreach (trxInvoiceTowerDetailRemaningAmount InvoiceTowerDetail in ListInvoiceTowerDetailRemaining)
                    {
                        logInvoiceTowerDetailRemainingAmount logInvoiceTowerDetail = new logInvoiceTowerDetailRemainingAmount();
                        logInvoiceTowerDetail.trxInvoiceHeaderRemainingID = InvoiceTowerDetail.trxInvoiceHeaderRemainingID;
                        logInvoiceTowerDetail.SONumber = InvoiceTowerDetail.SONumber;
                        logInvoiceTowerDetail.SiteIdOld = InvoiceTowerDetail.SiteIdOld;
                        logInvoiceTowerDetail.SiteName = InvoiceTowerDetail.SiteName;
                        logInvoiceTowerDetail.SpkDate = InvoiceTowerDetail.SpkDate;
                        logInvoiceTowerDetail.RfiDate = InvoiceTowerDetail.RfiDate;
                        logInvoiceTowerDetail.BapsPeriod = InvoiceTowerDetail.BapsPeriod;
                        logInvoiceTowerDetail.PoNumber = InvoiceTowerDetail.PoNumber;
                        logInvoiceTowerDetail.Type = InvoiceTowerDetail.Type;
                        logInvoiceTowerDetail.BapsNo = InvoiceTowerDetail.BapsNo;
                        logInvoiceTowerDetail.PeriodInvoice = InvoiceTowerDetail.PeriodInvoice;
                        logInvoiceTowerDetail.BapsType = InvoiceTowerDetail.BapsType;
                        logInvoiceTowerDetail.StipSiro = InvoiceTowerDetail.StipSiro;
                        logInvoiceTowerDetail.StipSiroId = InvoiceTowerDetail.StipSiroId;
                        logInvoiceTowerDetail.PowerType = InvoiceTowerDetail.PowerType;
                        logInvoiceTowerDetail.PowerTypeCode = InvoiceTowerDetail.PowerTypeCode;
                        logInvoiceTowerDetail.StartDatePeriod = InvoiceTowerDetail.StartDatePeriod;
                        logInvoiceTowerDetail.EndDatePeriod = InvoiceTowerDetail.EndDatePeriod;
                        logInvoiceTowerDetail.StartDateRent = InvoiceTowerDetail.StartDateRent;
                        logInvoiceTowerDetail.EndDateRent = InvoiceTowerDetail.EndDateRent;
                        logInvoiceTowerDetail.AmountRental = InvoiceTowerDetail.AmountRental;
                        logInvoiceTowerDetail.AmountService = InvoiceTowerDetail.AmountService;
                        logInvoiceTowerDetail.AmountInvoicePeriod = InvoiceTowerDetail.AmountInvoicePeriod;
                        logInvoiceTowerDetail.AmountPenaltyPeriod = InvoiceTowerDetail.AmountPenaltyPeriod;
                        logInvoiceTowerDetail.AmountOverdaya = InvoiceTowerDetail.AmountOverdaya;
                        logInvoiceTowerDetail.AmountOverblast = InvoiceTowerDetail.AmountOverblast;
                        logInvoiceTowerDetail.AmountPPN = InvoiceTowerDetail.AmountPPN;
                        logInvoiceTowerDetail.AmountLossPPN = InvoiceTowerDetail.AmountLossPPN;
                        logInvoiceTowerDetail.SiteIdOpr = InvoiceTowerDetail.SiteIdOpr;
                        logInvoiceTowerDetail.SiteNameOpr = InvoiceTowerDetail.SiteNameOpr;
                        logInvoiceTowerDetail.ContractNumber = InvoiceTowerDetail.ContractNumber;
                        logInvoiceTowerDetail.ReslipNumber = InvoiceTowerDetail.ReslipNumber;
                        logInvoiceTowerDetail.CreatedBy = userID;
                        logInvoiceTowerDetail.CreatedDate = _dtNow;
                        listLogInvoiceTowerDetail.Add(logInvoiceTowerDetail);
                    }
                    _logInvTwrDetailRemainAmountRepo.CreateBulky(listLogInvoiceTowerDetail);

                    _trxInvTwrDetailRemainAmountRepo.DeleteByFilter("trxInvoiceHeaderRemainingID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                    _trxInvoiceHeaderRemRepo.DeleteByFilter("trxInvoiceHeaderRemainingAmountID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                    #endregion

                    #region Delete from mstTaxInvoice
                    _mstTaxInvoiceRepo.DeleteByFilter("trxInvoiceHeaderRemainingAmountId = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                    #endregion

                    #region Delete From trxDocInvoiceDetail
                    _trxDocInvoiceRepo.DeleteByFilter("trxInvoiceHeaderRemainingAmountId = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                    #endregion
                }

                #region 'INSERT InvoiceActivityLog'
                var logInvoiceActivity = new logInvoiceActivity();
                logInvoiceActivity.InvNo = DataCreatedInvoiceTower.InvNo;
                logInvoiceActivity.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.ApproveCancelInvoice;//APPROVE CANCEL INVOICE
                logInvoiceActivity.LogDate = _dtNow;
                logInvoiceActivity = _logInvoiceActRepo.Create(logInvoiceActivity);
                #endregion

                #region 'INSERT LOGARACTIVITY'
                //Supposed to be deleted because the Invoice it self has been deleted
                //var LogArRepo = new logArActivityRepository(context);
                //logArActivity logAr = new logArActivity();
                //logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.ApproveCancelInvoice;
                //logAr.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                //if (DataCreatedInvoiceTower.mstInvoiceCategoryId == 1)
                //    logAr.trxInvoiceHeaderID = DataCreatedInvoiceTower.trxInvoiceHeaderID;
                //if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower15 || DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower10)
                //    logAr.trxInvoiceHeaderRemainingAmountID = DataCreatedInvoiceTower.trxInvoiceHeaderID;

                //logAr.CreatedBy = userCredential.UserID;
                //logAr.CreatedDate = Helper.GetDateTimeNow();
                //logAr = LogArRepo.Create(logAr);

                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                    _logARActRepo.DeleteByFilter("trxInvoiceHeaderID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                if (DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower15 || DataCreatedInvoiceTower.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower10)
                    _logARActRepo.DeleteByFilter("trxInvoiceHeaderRemainingAmountID = " + DataCreatedInvoiceTower.trxInvoiceHeaderID);
                #endregion
                uow.SaveChanges();

                return new trxInvoiceHeader();

            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxInvoiceHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxPostingInvoiceTowerService", "PostingInvoiceTower", userID));
            }
        }
    }
}
