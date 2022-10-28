using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.ViewModels;

namespace ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem
{
    public class uspMonitoringAgingExecutiveRepository : BaseRepository<vmMonitoringAgingExecutiveSummary>
    {
        private DbContext _context;
        public uspMonitoringAgingExecutiveRepository(DbContext context) : base(context)
		{
            _context = context;
        }

        public List<vmMonitoringAgingExecutiveSummary> GetList(vmMonitoringAgingExecutive param)
        {
            return pGetList(param);
        }

        public List<vmMonitoringAgingExecutiveSummary> GetList_30D(vmMonitoringAgingExecutive param)
        {
            return pGetList_30D(param);
        }

        public List<vmMonitoringAgingExecutiveSummary> GetList_60D(vmMonitoringAgingExecutive param)
        {
            return pGetList_60D(param);
        }

        public List<vmMonitoringAgingExecutiveSummary> GetDetail(vmMonitoringAgingExecutive param)
        {
            return pGetDetail(param);
        }

        private List<vmMonitoringAgingExecutiveSummary> pGetList(vmMonitoringAgingExecutive param)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMonitoringAgingExecutive";

                command.Parameters.Add(command.CreateParameter("@vCompanyID", param.vCompanyID));
                command.Parameters.Add(command.CreateParameter("@vOperatorID", param.vOperatorID));
                command.Parameters.Add(command.CreateParameter("@vAmountType", param.vAmountType));
                command.Parameters.Add(command.CreateParameter("@vPeriod", param.vPeriod.ToString()));
                command.Parameters.Add(command.CreateParameter("@vInvoiceType", param.vInvoiceType));
                command.Parameters.Add(command.CreateParameter("@vPKP", param.vPKP));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<vmMonitoringAgingExecutiveSummary> pGetList_30D(vmMonitoringAgingExecutive param)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMonitoringAgingExecutive_30D";

                command.Parameters.Add(command.CreateParameter("@vCompanyID", param.vCompanyID));
                command.Parameters.Add(command.CreateParameter("@vOperatorID", param.vOperatorID));
                command.Parameters.Add(command.CreateParameter("@vAmountType", param.vAmountType));
                command.Parameters.Add(command.CreateParameter("@vPeriod", param.vPeriod));
                command.Parameters.Add(command.CreateParameter("@vInvoiceType", param.vInvoiceType));
                command.Parameters.Add(command.CreateParameter("@vPKP", param.vPKP));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<vmMonitoringAgingExecutiveSummary> pGetList_60D(vmMonitoringAgingExecutive param)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMonitoringAgingExecutive_60D";

                command.Parameters.Add(command.CreateParameter("@vCompanyID", param.vCompanyID));
                command.Parameters.Add(command.CreateParameter("@vOperatorID", param.vOperatorID));
                command.Parameters.Add(command.CreateParameter("@vAmountType", param.vAmountType));
                command.Parameters.Add(command.CreateParameter("@vPeriod", param.vPeriod));
                command.Parameters.Add(command.CreateParameter("@vInvoiceType", param.vInvoiceType));
                command.Parameters.Add(command.CreateParameter("@vPKP", param.vPKP));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<vmMonitoringAgingExecutiveSummary> pGetDetail(vmMonitoringAgingExecutive param)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMonitoringAgingExecutiveDetail";

                command.Parameters.Add(command.CreateParameter("@vCompanyID", param.vCompanyID));
                command.Parameters.Add(command.CreateParameter("@vOperatorID", param.vOperatorID));
                command.Parameters.Add(command.CreateParameter("@vPeriod", param.vPeriod));
                command.Parameters.Add(command.CreateParameter("@vInvoiceType", param.vInvoiceType));
                command.Parameters.Add(command.CreateParameter("@vPKP", param.vPKP));

                return this.ReadTransaction(command).ToList();
            }
        }
    }
}
