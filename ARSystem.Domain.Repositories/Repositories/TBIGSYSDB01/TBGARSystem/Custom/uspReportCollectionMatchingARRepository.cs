using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.ViewModels;

namespace ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem
{
    public class uspReportCollectionMatchingARRepository : BaseRepository<vwtrxInvoiceMatchingAR>
    {
        private DbContext _context;
        public uspReportCollectionMatchingARRepository(DbContext context) : base(context)
		{
            _context = context;
        }

        public List<vwtrxInvoiceMatchingAR> GetList(vmInvoiceMatchingAR param)
        {
            return pGetList(param);
        }

        private List<vwtrxInvoiceMatchingAR> pGetList(vmInvoiceMatchingAR param)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspReportCollectionMatchingAR";

                command.Parameters.Add(command.CreateParameter("@vStartPeriod", param.vStartPaid));
                command.Parameters.Add(command.CreateParameter("@vEndPeriod", param.vEndPaid));
                command.Parameters.Add(command.CreateParameter("@vCompanyID", param.vCompanyID));
                command.Parameters.Add(command.CreateParameter("@vInvoiceNo", param.vInvoiceNo));
                command.Parameters.Add(command.CreateParameter("@vDocumentPayment", param.vDocumentPayment));
                command.Parameters.Add(command.CreateParameter("@vStatus", param.vStatus));

                return this.ReadTransaction(command).ToList();
            }
        }
    }
}
