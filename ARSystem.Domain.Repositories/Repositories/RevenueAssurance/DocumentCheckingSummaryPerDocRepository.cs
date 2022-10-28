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
  public  class DocumentCheckingSummaryPerDocRepository : BaseRepository<DocumentCheckingSummaryPerDoc>
    {
        private DbContext _context;
        public DocumentCheckingSummaryPerDocRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<DocumentCheckingSummaryPerDoc> GetList(string customerId, string siteId, string soNumber, string companyId, int productId)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDocumentCheckingSummary";

                command.Parameters.Add(command.CreateParameter("@Type", "Detail"));
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
