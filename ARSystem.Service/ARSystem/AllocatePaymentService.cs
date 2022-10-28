using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.ViewModels;

namespace ARSystem.Service.ARSystem
{
    public class AllocatePaymentService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;

        private trxAllocatePaymentBankInRepository _trxBankInRepo;
        private vwtrxAllocatePaymentRepository _vwtrxAllocatePayRepo;
        private trxAllocatePaymentBankOutRepository _trxBankOutRepo;

        public AllocatePaymentService() : base()
        {
            _context = this.SetContext();
            _dtNow = Helper.GetDateTimeNow();

            _trxBankInRepo = new trxAllocatePaymentBankInRepository(_context);
            _vwtrxAllocatePayRepo = new vwtrxAllocatePaymentRepository(_context);
            _trxBankOutRepo = new trxAllocatePaymentBankOutRepository(_context);
        }

        #region Data
        public Datatable<vwtrxAllocatePayment> GetDataAllocatePayment(string UserID, vmAllocatePayment filter)
        {
            Datatable<vwtrxAllocatePayment> dataAllocatePayment = new Datatable<vwtrxAllocatePayment>();

            try
            {
                string whereClause = filterData(filter);

                dataAllocatePayment.Count = _vwtrxAllocatePayRepo.GetCount(whereClause);

                if (filter.length > 0)
                    dataAllocatePayment.List = _vwtrxAllocatePayRepo.GetPaged(whereClause, "", filter.start, filter.length);
                else
                    dataAllocatePayment.List = _vwtrxAllocatePayRepo.GetList(whereClause);

                return dataAllocatePayment;
            }
            catch (Exception ex)
            {
                dataAllocatePayment.List.Add(new vwtrxAllocatePayment((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return dataAllocatePayment;
            }
        }

        public Datatable<trxAllocatePaymentBankOut> GetDataBankOut(string UserID, vmAllocatePayment filter)
        {
            Datatable<trxAllocatePaymentBankOut> dataBankOut = new Datatable<trxAllocatePaymentBankOut>();

            try
            {
                dataBankOut.Count = _trxBankOutRepo.GetCount("trxAllocatePaymentBankInID = " + filter.vtrxAllocatePaymentBankInID + " ");

                if (filter.length > 0)
                    dataBankOut.List = _trxBankOutRepo.GetPaged("trxAllocatePaymentBankInID = " + filter.vtrxAllocatePaymentBankInID + " ", "", filter.start, filter.length);
                else
                    dataBankOut.List = _trxBankOutRepo.GetList("trxAllocatePaymentBankInID = " + filter.vtrxAllocatePaymentBankInID + " ");

                return dataBankOut;
            }
            catch (Exception ex)
            {
                dataBankOut.List.Add(new trxAllocatePaymentBankOut((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return dataBankOut;
            }
        }

        public string ValidateAmount(string UserID, vmAllocatePayment filter)
        {
            try
            {
                string result = "";
                decimal bankOut = 0;
                var list = _vwtrxAllocatePayRepo.GetList("trxAllocatePaymentBankInID = " + filter.vtrxAllocatePaymentBankInID + " ").FirstOrDefault();

                bankOut = Convert.ToDecimal((list.AmountBankOut - filter.vAmountBankOutExs) + filter.vAmount);

                if (bankOut > list.Amount)
                {
                    result = "Unable to input! Amount Bank Out " + bankOut.ToString("#,#") +  " is greater than Amount Bank In.";
                }
                else
                {
                    result = "";
                }

                return result;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID);
                return "";
            }
        }

        public decimal getVariance(string UserID, int vtrxAllocatePaymentBankInID)
        {
            try
            {
                var list = _vwtrxAllocatePayRepo.GetList("trxAllocatePaymentBankInID = " + vtrxAllocatePaymentBankInID + " ").FirstOrDefault();

                decimal variance = Convert.ToDecimal(list.Unsettle);

                return variance;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID);
                return 0;
            }
        }
        #endregion

        #region Processing Data
        public trxAllocatePaymentBankIn CreateBankIn(string UserID, trxAllocatePaymentBankIn data)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                trxAllocatePaymentBankIn trxBankIn = pCreateBankIn(UserID, data);

                uow.SaveChanges();
                return trxBankIn;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxAllocatePaymentBankIn((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID));
            }
        }

        private trxAllocatePaymentBankIn pCreateBankIn(string UserID, trxAllocatePaymentBankIn data)
        {
            trxAllocatePaymentBankIn trxBankIn = new trxAllocatePaymentBankIn();
            trxBankIn.PaidDate = data.PaidDate;
            trxBankIn.TypeID = data.TypeID;
            trxBankIn.CompanyID = data.CompanyID;
            trxBankIn.OperatorID = data.OperatorID;
            trxBankIn.Amount = data.Amount;
            trxBankIn.Description = data.Description;
            trxBankIn.CreatedBy = UserID;
            trxBankIn.CreatedDate = _dtNow;
            trxBankIn = _trxBankInRepo.Create(trxBankIn);

            return trxBankIn;
        }

        public trxAllocatePaymentBankIn EditBankIn(string UserID, trxAllocatePaymentBankIn data)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                trxAllocatePaymentBankIn trxBankIn = pGetSingle(Convert.ToInt32(data.trxAllocatePaymentBankInID));
                pEditBankIn(UserID, trxBankIn, data);

                uow.SaveChanges();
                return trxBankIn;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxAllocatePaymentBankIn((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID));
            }
        }

        private trxAllocatePaymentBankIn pGetSingle(int ID)
        {
            trxAllocatePaymentBankIn trxBankIn = _trxBankInRepo.GetByPK(ID);

            return trxBankIn;
        }

        private trxAllocatePaymentBankIn pEditBankIn(string UserID, trxAllocatePaymentBankIn trxBankIn, trxAllocatePaymentBankIn data)
        {
            trxBankIn.PaidDate = data.PaidDate;
            trxBankIn.TypeID = data.TypeID;
            trxBankIn.CompanyID = data.CompanyID;
            trxBankIn.OperatorID = data.OperatorID;
            trxBankIn.Amount = data.Amount;
            trxBankIn.Description = data.Description;
            trxBankIn.UpdatedBy = UserID;
            trxBankIn.UpdatedDate = _dtNow;
            trxBankIn = _trxBankInRepo.Update(trxBankIn);

            return trxBankIn;
        }

        public bool deleteBankIn(string UserID, int trxAllocatePaymentBankInID)
        {
            try
            {
                _trxBankOutRepo.DeleteByFilter("trxAllocatePaymentBankInID = '" + trxAllocatePaymentBankInID + "' ");
                return _trxBankInRepo.DeleteByPK(trxAllocatePaymentBankInID);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID);
                return false;
            }
        }

        public trxAllocatePaymentBankOut AddBankOut(string UserID, trxAllocatePaymentBankOut data)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                trxAllocatePaymentBankOut trxBankOut = pCreateBankOut(UserID, data);

                uow.SaveChanges();
                return trxBankOut;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxAllocatePaymentBankOut((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID));
            }
        }

        private trxAllocatePaymentBankOut pCreateBankOut(string UserID, trxAllocatePaymentBankOut data)
        {
            trxAllocatePaymentBankOut trxBankOut = new trxAllocatePaymentBankOut();
            trxBankOut.trxAllocatePaymentBankInID = data.trxAllocatePaymentBankInID;
            trxBankOut.Amount = data.Amount;
            trxBankOut.Description = data.Description;
            trxBankOut.CreatedBy = UserID;
            trxBankOut.CreatedDate = _dtNow;
            trxBankOut = _trxBankOutRepo.Create(trxBankOut);

            return trxBankOut;
        }

        public trxAllocatePaymentBankOut EditBankOut(string UserID, trxAllocatePaymentBankOut data)
        {
            var uow = _context.CreateUnitOfWork();

            try
            {
                trxAllocatePaymentBankOut trxBankOut = pGetSingleBankOut(Convert.ToInt32(data.trxAllocatePaymentBankOutID));
                pEditBankOut(UserID, trxBankOut, data);

                uow.SaveChanges();
                return trxBankOut;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxAllocatePaymentBankOut((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID));
            }
        }

        private trxAllocatePaymentBankOut pGetSingleBankOut(int ID)
        {
            trxAllocatePaymentBankOut trxBankOut = _trxBankOutRepo.GetByPK(ID);

            return trxBankOut;
        }

        private trxAllocatePaymentBankOut pEditBankOut(string UserID, trxAllocatePaymentBankOut trxBankOut, trxAllocatePaymentBankOut data)
        {
            trxBankOut.Amount = data.Amount;
            trxBankOut.Description = data.Description;
            trxBankOut.UpdatedBy = UserID;
            trxBankOut.UpdatedDate = _dtNow;
            trxBankOut = _trxBankOutRepo.Update(trxBankOut);

            return trxBankOut;
        }

        public bool deleteBankOut(string UserID, int trxAllocatePaymentBankOutID)
        {
            try
            {
                return _trxBankOutRepo.DeleteByPK(trxAllocatePaymentBankOutID);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID);
                return false;
            }
        }
        #endregion

        #region Helper
        private string filterData(vmAllocatePayment data)
        {
            var whereClause = "1=1";

            if (data.vStatus != null || !string.IsNullOrEmpty(data.vStatus))
            {
                whereClause += " AND Status = '" + data.vStatus + "' ";
            }

            if (data.vCompany != null || !string.IsNullOrEmpty(data.vCompany))
            {
                whereClause += " AND CompanyID = '" + data.vCompany + "' ";
            }

            if (data.vOperator != null || !string.IsNullOrEmpty(data.vOperator))
            {
                whereClause += " AND OperatorID = '" + data.vOperator + "' ";
            }

            if (data.vStartPaid != null || !string.IsNullOrWhiteSpace(data.vStartPaid))
            {
                whereClause += " AND PaidDate BETWEEN '" + data.vStartPaid + "' AND '" + data.vEndPaid + "' ";
            }

            return whereClause;
        }
        #endregion
    }
}