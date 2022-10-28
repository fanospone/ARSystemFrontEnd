using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models;

namespace ARSystem.Service.ARSystem.Invoice
{
    public class trxPrintInvoiceTowerService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;

        private trxPICAARDataRepository _trxPICAARDataRepo;
        private trxInvoiceHeaderRepository _trxInvoiceHeaderRepo;
        private trxInvoiceHeaderRemainingAmountRepository _trxInvoiceHeaderRemRepo;
        private trxInvoiceNonRevenueRepository _trxInvoiceNonRevenueRepo;

        public trxPrintInvoiceTowerService() : base()
        {
            _context = this.SetContext();
            _dtNow = Helper.GetDateTimeNow();

            _trxPICAARDataRepo = new trxPICAARDataRepository(_context);
            _trxInvoiceHeaderRepo = new trxInvoiceHeaderRepository(_context);
            _trxInvoiceHeaderRemRepo = new trxInvoiceHeaderRemainingAmountRepository(_context);
            _trxInvoiceNonRevenueRepo = new trxInvoiceNonRevenueRepository(_context);
        }

        public string InsertTrxPICAARData(vmGetInvoicePostedList ModelInvoicePosted, string RemarksPrint, int mstPICATypeID, int mstPICADetailID, string UserID, int StatusState)
        {
            var uow = _context.CreateUnitOfWork();

            trxPICAARData PICAARData = new trxPICAARData();
            trxPICAARData PICAARDataExisting = new trxPICAARData();
            trxInvoiceHeader trxHeader = new trxInvoiceHeader();
            trxInvoiceHeaderRemainingAmount trxHeaderRemaining = new trxInvoiceHeaderRemainingAmount();
            List<trxPICAARData> listPICA = new List<trxPICAARData>();

            int ExistingStatus = 0;

            try
            {
                if (ModelInvoicePosted.HeaderId.Count() != 0)
                {
                    for (int i = 0; i < ModelInvoicePosted.HeaderId.Count; i++)
                    {
                        PICAARDataExisting = new trxPICAARData();

                        PICAARData = new trxPICAARData();
                        PICAARData.mstPICATypeID = mstPICATypeID;
                        PICAARData.mstPICADetailID = mstPICADetailID;
                        if (ModelInvoicePosted.CategoryId[i] == (int)StatusHelper.InvoiceCategory.Tower || ModelInvoicePosted.CategoryId[i] == (int)StatusHelper.InvoiceCategory.Building)
                        {
                            PICAARData.trxInvoiceHeaderID = ModelInvoicePosted.HeaderId[i];
                            trxHeader = _trxInvoiceHeaderRepo.GetByPK(ModelInvoicePosted.HeaderId[i]);
                            ExistingStatus = trxHeader.mstInvoiceStatusId.Value;
                            PICAARDataExisting = _trxPICAARDataRepo.GetList("trxInvoiceHeaderID = " + ModelInvoicePosted.HeaderId[i] + " AND mstInvoiceState = " + StatusState).FirstOrDefault();
                        }
                        else
                        {
                            PICAARData.trxInvoiceHeaderRemainingAmountID = ModelInvoicePosted.HeaderId[i];
                            trxHeaderRemaining = _trxInvoiceHeaderRemRepo.GetByPK(ModelInvoicePosted.HeaderId[i]);
                            ExistingStatus = trxHeaderRemaining.mstInvoiceStatusId.Value;
                            PICAARDataExisting = _trxPICAARDataRepo.GetList("trxInvoiceHeaderRemainingAmountID = " + ModelInvoicePosted.HeaderId[i] + " AND mstInvoiceState = " + StatusState).FirstOrDefault();
                        }
                        PICAARData.Remark = RemarksPrint;
                        PICAARData.mstInvoiceCategoryId = ModelInvoicePosted.CategoryId[i];
                        PICAARData.mstInvoiceStatusId = ExistingStatus;
                        PICAARData.mstInvoiceState = StatusState;
                        PICAARData.CreatedBy = UserID;
                        PICAARData.CreatedDate = Helper.GetDateTimeNow();
                        listPICA.Add(PICAARData);
                        if (PICAARDataExisting != null)
                        {
                            _trxPICAARDataRepo.DeleteByPK(PICAARDataExisting.trxPICAARDataID);
                        }
                    }

                }
                if (listPICA.Count > 0)
                    _trxPICAARDataRepo.CreateBulky(listPICA);
                uow.SaveChanges();
                return "";
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return ex.Message;
            }
        }

        public trxInvoiceHeader RejectCNARDataInvoiceTower(string userID, vmGetInvoicePostedList ModelInvoicePosted, string mstInvoiceState)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();
            trxPICAARData PICAARData = new trxPICAARData();
            trxInvoiceHeader trxHeader = new trxInvoiceHeader();
            trxInvoiceHeaderRemainingAmount trxHeaderRemaining = new trxInvoiceHeaderRemainingAmount();

            string WhereClause = "";
            int mstInvoiceStateId = 0;
            if (mstInvoiceState.ToLower() == "print")
                mstInvoiceStateId = (int)StatusHelper.InvoiceStatus.Print;
            else if (mstInvoiceState.ToLower() == "post")
                mstInvoiceStateId = (int)StatusHelper.InvoiceStatus.Posting;
            else if (mstInvoiceState.ToLower() == "checklistardata")
                mstInvoiceStateId = (int)StatusHelper.InvoiceStatus.SubmitChecklist;

            try
            {
                if (ModelInvoicePosted.HeaderId.Count() != 0)
                {
                    for (int i = 0; i < ModelInvoicePosted.HeaderId.Count; i++)
                    {

                        PICAARData = new trxPICAARData();
                        if (ModelInvoicePosted.CategoryId[i] == (int)StatusHelper.InvoiceCategory.Tower || ModelInvoicePosted.CategoryId[i] == (int)StatusHelper.InvoiceCategory.Building)
                        {
                            WhereClause += "trxInvoiceHeaderID = " + ModelInvoicePosted.HeaderId[i] + " AND mstInvoiceState=" + mstInvoiceStateId;
                            PICAARData = _trxPICAARDataRepo.GetList(WhereClause).FirstOrDefault();
                            trxHeader = _trxInvoiceHeaderRepo.GetByPK(ModelInvoicePosted.HeaderId[i]);
                            trxHeader.mstInvoiceStatusId = PICAARData.mstInvoiceStatusId;
                            if (mstInvoiceState.ToLower() == "checklistardata")
                                trxHeader.VerificationStatus = Constants.VerificationStatus["NEW"];
                            trxHeader.UpdatedBy = userID;
                            trxHeader.UpdatedDate = _dtNow;
                            _trxInvoiceHeaderRepo.Update(trxHeader);
                        }
                        else
                        {
                            WhereClause += "trxInvoiceHeaderRemainingAmountID = " + ModelInvoicePosted.HeaderId[i] + " AND mstInvoiceState=" + mstInvoiceStateId;
                            PICAARData = _trxPICAARDataRepo.GetList(WhereClause).FirstOrDefault();
                            trxHeaderRemaining = _trxInvoiceHeaderRemRepo.GetByPK(ModelInvoicePosted.HeaderId[i]);
                            trxHeaderRemaining.mstInvoiceStatusId = PICAARData.mstInvoiceStatusId;
                            if (mstInvoiceState.ToLower() == "checklistardata")
                                trxHeaderRemaining.VerificationStatus = Constants.VerificationStatus["NEW"];
                            trxHeaderRemaining.UpdatedBy = userID;
                            trxHeaderRemaining.UpdatedDate = _dtNow;
                            _trxInvoiceHeaderRemRepo.Update(trxHeaderRemaining);
                        }
                        _trxPICAARDataRepo.DeleteByFilter(WhereClause);
                    }
                }
                uow.SaveChanges();
                return new trxInvoiceHeader();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxInvoiceHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxPrintInvoiceTowerService", "RejectCNPrintInvoiceTower", userID));
            }
        }
    }
}
