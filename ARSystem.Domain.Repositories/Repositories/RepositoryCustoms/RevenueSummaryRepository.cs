using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Repositories
{
    public class RevenueSummaryRepository : BaseRepository<vmRevenueSummaryAmount>
    {
        private DbContext _context;
        public RevenueSummaryRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<T> GetRevSummaryOf<T>(vmRevenueSummaryParameters param)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspARRevSummary";

                command.Parameters.Add(command.CreateParameter("@vViewBy", param.vViewBy));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", param.vGroupBy));
                command.Parameters.Add(command.CreateParameter("@vAccount", param.vAccount));
                command.Parameters.Add(command.CreateParameter("@vYear", param.vYear));
                command.Parameters.Add(command.CreateParameter("@vCompany", param.vCompanyId));
                command.Parameters.Add(command.CreateParameter("@vRegional", param.vRegionalName));
                command.Parameters.Add(command.CreateParameter("@vOperator", param.vOperatorId));
                command.Parameters.Add(command.CreateParameter("@vProduct", param.vProduct));

                return this.ReadTransaction<T>(command).ToList();
            }
        }

        public List<vmRevenueSoNumberList> GetRevSummarySoNumberList(vmRevenueSummaryParameters param)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspARRevSummarySoNumber";

                command.Parameters.Add(command.CreateParameter("@vGroupBy", param.vGroupBy));
                command.Parameters.Add(command.CreateParameter("@vGroupValue", param.vGroupValue));
                command.Parameters.Add(command.CreateParameter("@vAccount", param.vAccount));
                command.Parameters.Add(command.CreateParameter("@vYear", param.vYear));
                command.Parameters.Add(command.CreateParameter("@vCompany", param.vCompanyId));
                command.Parameters.Add(command.CreateParameter("@vRegional", param.vRegionalName));
                command.Parameters.Add(command.CreateParameter("@vOperator", param.vOperatorId));
                command.Parameters.Add(command.CreateParameter("@vProduct", param.vProduct));
                command.Parameters.Add(command.CreateParameter("@vMonth", param.vMonth));

                return this.ReadTransaction<vmRevenueSoNumberList>(command).ToList();
            }
        }
    }
}
