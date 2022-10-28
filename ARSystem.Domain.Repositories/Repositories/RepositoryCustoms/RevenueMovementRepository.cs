using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;


namespace ARSystem.Domain.Repositories
{
    public class RevenueMovementRepository : BaseRepository<vmRevenueMovementAmount>
    {
        private DbContext _context;
        public RevenueMovementRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<T> GetRevSummaryOf<T>(vmRevenueSummaryParameters param)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspARRevSysReportMovement";

                command.Parameters.Add(command.CreateParameter("@vViewBy", param.vViewBy));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", param.vGroupBy));
                command.Parameters.Add(command.CreateParameter("@vYear", param.vYear));
                command.Parameters.Add(command.CreateParameter("@vMonth", param.vMonth));
                command.Parameters.Add(command.CreateParameter("@vCompany", param.vCompanyId));
                command.Parameters.Add(command.CreateParameter("@vRegional", param.vRegionalName));
                command.Parameters.Add(command.CreateParameter("@vOperator", param.vOperatorId));
                command.Parameters.Add(command.CreateParameter("@vProduct", param.vProduct));

                return this.ReadTransaction<T>(command).ToList();
            }
        }

        public int GetCountRevSummaryOf(vmRevenueSummaryParameters param)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspARRevSysReportMovement";

                command.Parameters.Add(command.CreateParameter("@vViewBy", param.vViewBy));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", param.vGroupBy));
                //command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vYear", param.vYear));
                command.Parameters.Add(command.CreateParameter("@vMonth", param.vMonth));
                command.Parameters.Add(command.CreateParameter("@vCompany", param.vCompanyId));
                command.Parameters.Add(command.CreateParameter("@vRegional", param.vRegionalName));
                command.Parameters.Add(command.CreateParameter("@vOperator", param.vOperatorId));
                command.Parameters.Add(command.CreateParameter("@vProduct", param.vProduct));

                return this.CountTransaction(command);
            }
        }

        public List<vmRevenueDetailMovement> GetRevenueDetailMovementByAmount(vmRevenueSummaryParameters param)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspARRevSysMovementDetailByAmount";

                command.Parameters.Add(command.CreateParameter("@vViewBy", param.vViewBy));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", param.vGroupBy));
                command.Parameters.Add(command.CreateParameter("@vDesc", param.vDesc));
                command.Parameters.Add(command.CreateParameter("@vYear", param.vYear));
                command.Parameters.Add(command.CreateParameter("@vMonth", param.vMonth));
                command.Parameters.Add(command.CreateParameter("@vCompany", param.vCompanyId));
                command.Parameters.Add(command.CreateParameter("@vRegional", param.vRegionalName));
                command.Parameters.Add(command.CreateParameter("@vOperator", param.vOperatorId));
                command.Parameters.Add(command.CreateParameter("@vProduct", param.vProduct));
                command.Parameters.Add(command.CreateParameter("@vSoNumber", param.vSoNumber));
                command.Parameters.Add(command.CreateParameter("@vSiteID", param.vSiteID));
                command.Parameters.Add(command.CreateParameter("@vSiteName", param.vSiteName));

                return this.ReadTransaction<vmRevenueDetailMovement>(command).ToList();
            }
        }

        public List<vmRevenueDetailMovementUnit> GetRevenueDetailMovementByUnit(vmRevenueSummaryParameters param)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspARRevSysMovementDetailByUnit";

                command.Parameters.Add(command.CreateParameter("@vViewBy", param.vViewBy));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", param.vGroupBy));
                command.Parameters.Add(command.CreateParameter("@vYear", param.vYear));
                command.Parameters.Add(command.CreateParameter("@vMonth", param.vMonth));
                command.Parameters.Add(command.CreateParameter("@vCompany", param.vCompanyId));
                command.Parameters.Add(command.CreateParameter("@vRegional", param.vRegionalName));
                command.Parameters.Add(command.CreateParameter("@vOperator", param.vOperatorId));
                command.Parameters.Add(command.CreateParameter("@vProduct", param.vProduct));
                command.Parameters.Add(command.CreateParameter("@vSoNumber", param.vSoNumber));
                command.Parameters.Add(command.CreateParameter("@vSiteID", param.vSiteID));
                command.Parameters.Add(command.CreateParameter("@vSiteName", param.vSiteName));

                return this.ReadTransaction<vmRevenueDetailMovementUnit>(command).ToList();
            }
        }
    }
}
