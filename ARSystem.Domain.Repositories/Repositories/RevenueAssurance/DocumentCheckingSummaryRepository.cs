using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;

namespace ARSystem.Domain.Repositories
{
  public  class DocumentCheckingSummaryRepository : BaseRepository<DocumentCheckingSummary>
    {
        private DbContext _context;
        public DocumentCheckingSummaryRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<DocumentCheckingSummary> GetList(string customerId, string siteId, string soNumber, string companyId, int productId)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDocumentCheckingSummary";

                command.Parameters.Add(command.CreateParameter("@Type", "Header"));
                command.Parameters.Add(command.CreateParameter("@CustomerId", customerId));
                command.Parameters.Add(command.CreateParameter("@SiteIdCompany", siteId));
                command.Parameters.Add(command.CreateParameter("@SoNumber", soNumber));
                command.Parameters.Add(command.CreateParameter("@Company", companyId));
                command.Parameters.Add(command.CreateParameter("@ProductId", productId));

                return this.ReadTransaction(command).ToList();
            }
        }

    }
}
