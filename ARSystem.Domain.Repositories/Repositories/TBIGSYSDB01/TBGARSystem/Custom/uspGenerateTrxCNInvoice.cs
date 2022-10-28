using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.ViewModels;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem
{
    public class uspGenerateTrxCNInvoiceRepository : BaseRepository<trxCNInvoiceTowerDetail>
    {
        private DbContext _context;
        public uspGenerateTrxCNInvoiceRepository(DbContext context) : base(context)
		{
            _context = context;
        }

        public int CreateCNInvoiceTowerDetail(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGenerateTrxCNInvoice";

                command.Parameters.Add(command.CreateParameter("@vType", "TOWER"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                return this.WriteTransaction(command);
            }
        }

        public int CreateCNTax(int trxInvoiceHeaderID, int invoiceCategoryID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGenerateTrxCNInvoice";

                command.Parameters.Add(command.CreateParameter("@vType", "TAX"));
                command.Parameters.Add(command.CreateParameter("@vtrxInvoiceHeaderID", trxInvoiceHeaderID));
                command.Parameters.Add(command.CreateParameter("@vInvoiceCategoryID", invoiceCategoryID));
                return this.WriteTransaction(command);
            }
        }

        public int CreateCNInvoiceDocDetail(int trxInvoiceHeaderID, int invoiceCategoryID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGenerateTrxCNInvoice";

                command.Parameters.Add(command.CreateParameter("@vType", "INVOICE_DOC"));
                command.Parameters.Add(command.CreateParameter("@vtrxInvoiceHeaderID", trxInvoiceHeaderID));
                command.Parameters.Add(command.CreateParameter("@vInvoiceCategoryID", invoiceCategoryID));
                return this.WriteTransaction(command);
            }
        }

    }
}
