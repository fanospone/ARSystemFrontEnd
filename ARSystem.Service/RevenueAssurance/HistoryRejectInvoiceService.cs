using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.ViewModels;

namespace ARSystem.Service.RevenueAssurance
{
    public class HistoryRejectInvoiceService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;

        private vwHistoryRejectInvoiceRepository _vwHistoryRepo;

        public HistoryRejectInvoiceService() : base()
        {
            _context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            _dtNow = Helper.GetDateTimeNow();

            _vwHistoryRepo = new vwHistoryRejectInvoiceRepository(_context);
        }

        public Datatable<vwHistoryRejectInvoice> GetDataHistoryRejectInvoice(string UserID, vmHistoryRejectInvoice filter, List<string> vSONumber)
        {
            Datatable<vwHistoryRejectInvoice> dataHistory = new Datatable<vwHistoryRejectInvoice>();

            try
            {
                string whereClause = filterData(filter);

                if (vSONumber.Count > 0 && vSONumber[0] != null && vSONumber[0] != "")
                {
                    whereClause += " AND SONumber IN ('-X'";
                    foreach (var item in vSONumber)
                    {
                        whereClause += ",'" + item + "'";
                    }
                    whereClause += ")";
                }

                dataHistory.Count = _vwHistoryRepo.GetCount(whereClause);

                if (filter.length > 0)
                    dataHistory.List = _vwHistoryRepo.GetPaged(whereClause, "", filter.start, filter.length);
                else
                    dataHistory.List = _vwHistoryRepo.GetList(whereClause);

                return dataHistory;
            }
            catch (Exception ex)
            {
                dataHistory.List.Add(new vwHistoryRejectInvoice((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return dataHistory;
            }
        }

        #region Helper
        private string filterData(vmHistoryRejectInvoice data)
        {
            var whereClause = "1=1";

            if (data.vReconcileType != null || !string.IsNullOrEmpty(data.vReconcileType))
            {
                whereClause += " AND ReconcileType = '" + data.vReconcileType + "' ";
            }

            if (data.vPowerType != null || !string.IsNullOrEmpty(data.vPowerType))
            {
                whereClause += " AND PowerType = '" + data.vPowerType + "' ";
            }

            if (data.vDepartmentCode != null || !string.IsNullOrEmpty(data.vDepartmentCode))
            {
                whereClause += " AND DepartmentCode = '" + data.vDepartmentCode + "' ";
            }

            if (data.vYear > 0 || data.vYear != 0)
            {
                whereClause += " AND RejectYear = '" + data.vYear + "' ";
            }

            if (data.vMonth > 0 || data.vMonth != 0)
            {
                whereClause += " AND RejectMonth = '" + data.vMonth + "' ";
            }

            return whereClause;
        }
        #endregion
    }
}
