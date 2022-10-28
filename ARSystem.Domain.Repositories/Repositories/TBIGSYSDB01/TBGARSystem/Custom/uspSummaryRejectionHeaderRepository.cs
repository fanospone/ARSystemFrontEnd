using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem
{
    public class uspSummaryRejectionHeaderRepository : BaseRepository<vmSummaryRejection>
    {
        private DbContext _context;
        public uspSummaryRejectionHeaderRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<vmSummaryRejection> GetSummaryRejection(vmSummaryRejectionPost filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspSummaryRejectionHeader";

                command.Parameters.Add(command.CreateParameter("@vCompany", filter.vCompanyID));
                command.Parameters.Add(command.CreateParameter("@vOperator", filter.vOperatorID));
                command.Parameters.Add(command.CreateParameter("@vRTIPeriodStart", filter.vRTIPeriodStart));
                command.Parameters.Add(command.CreateParameter("@vRTIPeriodEnd", filter.vRTIPeriodEnd));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", filter.vGroupBy));

                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vmSummaryRejection> GetSummaryRejectionDetail(vmSummaryRejectionPost filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspSummaryRejectionDetail";

                command.Parameters.Add(command.CreateParameter("@vCompany", filter.vCompanyID));
                command.Parameters.Add(command.CreateParameter("@vOperator", filter.vOperatorID));
                command.Parameters.Add(command.CreateParameter("@vRTIPeriodStart", filter.vRTIPeriodStart));
                command.Parameters.Add(command.CreateParameter("@vRTIPeriodEnd", filter.vRTIPeriodEnd));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", filter.vGroupBy));
                command.Parameters.Add(command.CreateParameter("@vCol", filter.vCol));
                command.Parameters.Add(command.CreateParameter("@vDepartmentCode", filter.vDepartmentCode));

                return this.ReadTransaction(command).ToList();
            }
        }
    }
}
