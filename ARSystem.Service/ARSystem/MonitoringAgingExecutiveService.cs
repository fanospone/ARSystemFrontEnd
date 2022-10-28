using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Models.ViewModels;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;

namespace ARSystem.Service.ARSystem
{
    public class MonitoringAgingExecutiveService : BaseService
    {
        private DbContext _contextDWH;
        private DateTime _dtNow;

        private uspMonitoringAgingExecutiveRepository _uspAgingExecutiveRepo;

        public MonitoringAgingExecutiveService() : base()
        {
            _contextDWH = new DbContext(Helper.GetConnection("HTBGDWH01_TBGARSystemWH"));
            _dtNow = Helper.GetDateTimeNow();

            _uspAgingExecutiveRepo = new uspMonitoringAgingExecutiveRepository(_contextDWH);
        }

        public List<vmMonitoringAgingExecutiveSummary> GetSummary(string UserID, vmMonitoringAgingExecutive filter)
        {
            List<vmMonitoringAgingExecutiveSummary> summary = new List<vmMonitoringAgingExecutiveSummary>();

            try
            {
                if (filter.vCategory == 1)
                {
                    summary = _uspAgingExecutiveRepo.GetList(filter);
                } else if (filter.vCategory == 2)
                {
                    summary = _uspAgingExecutiveRepo.GetList_30D(filter);
                } else
                {
                    summary = _uspAgingExecutiveRepo.GetList_60D(filter);
                }

                return summary;
            }
            catch (Exception ex)
            {
                summary.Add(new vmMonitoringAgingExecutiveSummary((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return summary;
            }
        }

        public List<vmMonitoringAgingExecutiveSummary> GetDetail(string UserID, vmMonitoringAgingExecutive param)
        {
            List<vmMonitoringAgingExecutiveSummary> detail = new List<vmMonitoringAgingExecutiveSummary>();

            try
            {
                detail = _uspAgingExecutiveRepo.GetDetail(param);

                return detail;
            }
            catch (Exception ex)
            {
                detail.Add(new vmMonitoringAgingExecutiveSummary((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return detail;
            }
        }
    }
}
