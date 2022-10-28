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
    public class trxARProcessInvoiceTowerService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;

        private trxInvoiceHeaderRepository _trxInvoiceHeaderRepo;
        private trxInvoiceTowerDetailRepository _trxInvoiceDetailRepo;
        private trxInvoiceHeaderRemainingAmountRepository _trxInvoiceHeaderRemainRepo;
        private trxInvoiceTowerDetailRemaningAmountRepository _trxInvoiceDetailRemainRepo;
        private vwDataPostedInvoiceTowerRepository _vwDataPostedTowerRepo;
        private trxPICAARRepository _trxPICAARRepo;
        private trxCancelNoteFinanceRepository _trxCancelNoteRepo;
        private logArActivityRepository _logARActRepo;
        private trxInvoiceNonRevenueRepository _trxInvNonRevenueRepo;
        private trxCNInvoiceJobTBGConsoleRepository _trxCNInvJob;

        public trxARProcessInvoiceTowerService() : base()
        {
            _context = this.SetContext();
            _dtNow = Helper.GetDateTimeNow();

            _trxInvoiceHeaderRepo = new trxInvoiceHeaderRepository(_context);
            _trxInvoiceDetailRepo = new trxInvoiceTowerDetailRepository(_context);
            _trxInvoiceHeaderRemainRepo = new trxInvoiceHeaderRemainingAmountRepository(_context);
            _trxInvoiceDetailRemainRepo = new trxInvoiceTowerDetailRemaningAmountRepository(_context);
            _vwDataPostedTowerRepo = new vwDataPostedInvoiceTowerRepository(_context);
            _trxPICAARRepo = new trxPICAARRepository(_context);
            _trxCancelNoteRepo = new trxCancelNoteFinanceRepository(_context);
            _logARActRepo = new logArActivityRepository(_context);
            _trxInvNonRevenueRepo = new trxInvoiceNonRevenueRepository(_context);
            _trxCNInvJob = new trxCNInvoiceJobTBGConsoleRepository(_context);
        }

        public trxPICAAR SavePICAARProcessInvoiceTower(string userID, trxPICAAR data, int mstInvoiceCategoryId)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                #region Validate Invoice 15% and 10%

                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                {
                    List<trxInvoiceTowerDetail> details = _trxInvoiceDetailRepo.GetList("trxInvoiceHeaderID = " + data.trxInvoiceHeaderID);
                    vwDataPostedInvoiceTower dataPostedInvoiceTower;
                    trxInvoiceHeaderRemainingAmount headerRemaining;
                    trxInvoiceTowerDetailRemaningAmount detailRemaining;
                    string invoiceType = string.Empty;
                    string whereRemaining = string.Empty;
                    foreach (trxInvoiceTowerDetail detail in details)
                    {
                        detailRemaining = _trxInvoiceDetailRemainRepo.GetList("SONumber = '" + detail.SONumber + "' AND BapsNo = '" + detail.BapsNo +
                            "' AND BapsPeriod = '" + detail.BapsPeriod + "' AND BapsType = '" + detail.BapsType + "'").FirstOrDefault();
                        if (detailRemaining != null)
                        {
                            whereRemaining = "trxInvoiceHeaderID = " + detailRemaining.trxInvoiceHeaderRemainingID + " AND mstInvoiceCategoryId IN(" + (int)StatusHelper.InvoiceCategory.Tower15 + ", " + (int)StatusHelper.InvoiceCategory.Tower10 + ")";
                            dataPostedInvoiceTower = _vwDataPostedTowerRepo.GetList(whereRemaining).FirstOrDefault();
                            if (dataPostedInvoiceTower != null)
                            {
                                headerRemaining = _trxInvoiceHeaderRemainRepo.GetByPK(dataPostedInvoiceTower.trxInvoiceHeaderID);
                                if (headerRemaining != null)
                                {
                                    if (string.IsNullOrEmpty(headerRemaining.InvParentNo))
                                    {
                                        if (headerRemaining.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower10)
                                            invoiceType = "10% ";
                                        else if (headerRemaining.mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower15)
                                            invoiceType = "15% ";
                                        return new trxPICAAR((int)Helper.ErrorType.Validation, "Please Cancel Invoice " + invoiceType + "with No: " + headerRemaining.InvNo + " first.");
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Create PICA AR COllection

                trxPICAAR picaAR = new trxPICAAR();
                picaAR.mstPICADetailID = data.mstPICADetailID;
                picaAR.mstPICAMajorID = data.mstPICAMajorID;
                picaAR.mstPICATypeID = data.mstPICATypeID;
                picaAR.NeedCN = data.NeedCN;
                picaAR.Remark = data.Remark;
                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                    picaAR.trxInvoiceHeaderID = data.trxInvoiceHeaderID;
                else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                    picaAR.trxInvoiceNonRevenueID = data.trxInvoiceHeaderID;
                else
                    picaAR.trxInvoiceHeaderRemainingAmountID = data.trxInvoiceHeaderID;
                picaAR.CreatedBy = userID;
                picaAR.CreatedDate = Helper.GetDateTimeNow();
                _trxPICAARRepo.Create(picaAR);

                #endregion

                #region Create Cancellation Note

                trxCancelNoteFinance note = new trxCancelNoteFinance();
                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                    note.trxInvoiceHeaderID = data.trxInvoiceHeaderID;
                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                    note.trxCNInvoiceNonRevenueID = data.trxInvoiceHeaderID;
                else
                    note.trxInvoiceHeaderRemainingAmountID = data.trxInvoiceHeaderID;
                note.PrintDate = Helper.GetDateTimeNow();
                note.CreatedBy = userID;
                note.CreatedDate = Helper.GetDateTimeNow();
                note = _trxCancelNoteRepo.Create(note);

                string companyId = string.Empty;
                string operatorId = string.Empty;
                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                {
                    trxInvoiceHeader header = _trxInvoiceHeaderRepo.GetByPK(data.trxInvoiceHeaderID.Value);
                    header.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateWaitingSPVApprovalCNInvoice;
                    header.UpdatedBy = userID;
                    header.UpdatedDate = Helper.GetDateTimeNow();
                    header = _trxInvoiceHeaderRepo.Update(header);
                    companyId = header.InvCompanyId;
                    operatorId = header.InvOperatorID;
                }
                else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                {
                    trxInvoiceNonRevenue nonRevenue = _trxInvNonRevenueRepo.GetByPK(data.trxInvoiceHeaderID.Value);
                    nonRevenue.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateWaitingSPVApprovalCNInvoice;
                    nonRevenue.UpdatedBy = userID;
                    nonRevenue.UpdatedDate = Helper.GetDateTimeNow();
                    nonRevenue = _trxInvNonRevenueRepo.Update(nonRevenue);
                    companyId = nonRevenue.CompanyID;
                    operatorId = nonRevenue.OperatorID;
                }
                else
                {
                    trxInvoiceHeaderRemainingAmount header = _trxInvoiceHeaderRemainRepo.GetByPK(data.trxInvoiceHeaderID.Value);
                    header.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateWaitingSPVApprovalCNInvoice;
                    header.UpdatedBy = userID;
                    header.UpdatedDate = Helper.GetDateTimeNow();
                    header = _trxInvoiceHeaderRemainRepo.Update(header);
                    companyId = header.InvCompanyId;
                    operatorId = header.InvOperatorID;
                }
                note.CancelNoteNo = Helper.GenerateCNNoteNumber(note.trxCancelNoteFinanceID, companyId, operatorId);
                note.MemoNo = Helper.GenerateCNMemoNumber(note.trxCancelNoteFinanceID, companyId);
                note.UpdatedBy = userID;
                note.UpdatedDate = Helper.GetDateTimeNow();
                note = _trxCancelNoteRepo.Update(note);

                #endregion

                #region Write Log

                logArActivity log = new logArActivity();
                log.CreatedBy = userID;
                log.CreatedDate = Helper.GetDateTimeNow();
                log.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                log.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.RequestCNInvoice;
                log.ReprintRemarks = data.Remark;
                if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower)
                    log.trxInvoiceHeaderID = data.trxInvoiceHeaderID;
                else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue)
                    log.trxInvoiceNonRevenueID = data.trxInvoiceHeaderID;
                else
                    log.trxInvoiceHeaderRemainingAmountID = data.trxInvoiceHeaderID;
                _logARActRepo.Create(log);

                #endregion

                uow.SaveChanges();

                return picaAR;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxPICAAR((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxARProcessInvoiceTowerService", "SavePICAARProcessInvoiceTower", userID));
            }
        }
    }
}
