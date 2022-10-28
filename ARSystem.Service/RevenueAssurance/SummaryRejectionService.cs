using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.ViewModels;


namespace ARSystem.Service.RevenueAssurance
{
    public class SummaryRejectionService : BaseService
    {
        private DbContext _context;

        private uspSummaryRejectionHeaderRepository _summaryRejRepo;
        public SummaryRejectionService() : base()
        {
            _context = new DbContext(Helper.GetConnection("ARSystemDWH"));

            _summaryRejRepo = new uspSummaryRejectionHeaderRepository(_context);
        }

        public List<vmSummaryRejection> GetSummaryRejection(string UserID, vmSummaryRejectionPost filter)
        {
            List<vmSummaryRejection> summary = new List<vmSummaryRejection>();

            try
            {
                summary = _summaryRejRepo.GetSummaryRejection(filter);
                return summary;
            }
            catch (Exception ex)
            {
                summary.Add(new vmSummaryRejection((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return summary;
            }
        }

        public List<vmSummaryRejection> GetSummaryRejectionDetail(string UserID, vmSummaryRejectionPost filter)
        {
            List<vmSummaryRejection> summary = new List<vmSummaryRejection>();

            try
            {
                summary = _summaryRejRepo.GetSummaryRejectionDetail(filter);
                return summary;
            }
            catch (Exception ex)
            {
                summary.Add(new vmSummaryRejection((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return summary;
            }
        }
    }
}
