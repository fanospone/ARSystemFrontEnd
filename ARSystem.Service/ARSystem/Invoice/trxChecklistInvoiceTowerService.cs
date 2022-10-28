using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;

namespace ARSystem.Service.ARSystem.Invoice
{
    public class trxChecklistInvoiceTowerService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;

        private vwChecklistInvoiceTowerRepository _vwChecklistRepo;

        public trxChecklistInvoiceTowerService() : base()
        {
            _context = this.SetContext();
            _dtNow = Helper.GetDateTimeNow();

            _vwChecklistRepo = new vwChecklistInvoiceTowerRepository(_context);
        }

        #region Data
        public List<vwChecklistInvoiceTower> GetDataCheckList(string userID, vwChecklistInvoiceTower filter, DateTime? postingDateFrom, DateTime? postingDateTo)
        {
            List<vwChecklistInvoiceTower> checkList = new List<vwChecklistInvoiceTower>();

            try
            {
                string whereClause = filterCheckList(userID, filter, postingDateFrom, postingDateTo);

                checkList = _vwChecklistRepo.GetList(whereClause, "UpdatedDate DESC");

                return checkList;
            }
            catch (Exception ex)
            {
                checkList.Add(new vwChecklistInvoiceTower((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), userID)));
                return checkList;
            }
        }
        #endregion

        #region Helper
        private string filterCheckList(string userID, vwChecklistInvoiceTower data, DateTime? postingDateFrom, DateTime? postingDateTo)
        {
            var whereClause = "1=1";

            if (!string.IsNullOrWhiteSpace(data.InvCompanyId))
            {
                whereClause += " AND InvCompanyId = '" + data.InvCompanyId + "' ";
            }

            if (!string.IsNullOrWhiteSpace(data.InvOperatorID))
            {
                whereClause += " AND InvOperatorId = '" + data.InvOperatorID + "' ";
            }

            if (!string.IsNullOrWhiteSpace(data.InvoiceTypeId))
            {
                if (data.InvoiceTypeId != "0")
                    whereClause += " AND InvoiceTypeId = '" + data.InvoiceTypeId + "' ";
            }

            if (!string.IsNullOrWhiteSpace(data.Status) && data.Status != "null")
            {
                whereClause += " AND VerificationStatus = '" + data.Status + "' ";
            }

            if (postingDateFrom != null && postingDateFrom.Value != DateTime.MinValue)
            {
                whereClause += " AND PostedDate >= '" + postingDateFrom.Value.ToString("yyyy-MM-dd") + "' ";
            }

            if (postingDateTo != null)
            {
                whereClause += " AND PostedDate <= '" + postingDateTo.Value.ToString("yyyy-MM-dd hh:mm:ss") + "' ";
            }
            if (!string.IsNullOrWhiteSpace(data.InvNo))
            {
                whereClause += " AND InvNo LIKE '%" + data.InvNo + "%' ";
            }
            //CHECK IF USER IS AR DEPT HEAD OR NOT
            if (UserHelper.GetUserARPosition(userID) == "DEPT HEAD")
            {
                whereClause += " AND mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.StateWaitingCNInvoiceChecklist + " ";//SHOW WAITING FOR APPROVAL
            }
            else
            {
                whereClause += " AND mstInvoiceStatusId <> " + (int)StatusHelper.InvoiceStatus.StateWaitingCNInvoiceChecklist + " ";
            }

            return whereClause;
        }
        #endregion
    }
}
