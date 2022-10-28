using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGSAPIntegration;

namespace ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem
{
    public class uspMonitoringMatchingARFilterRepository : BaseRepository<stgTRStatusPenerimaanPembayaran>
    {
        private DbContext _context;
        public uspMonitoringMatchingARFilterRepository(DbContext context) : base(context)
		{
            _context = context;
        }

        public List<stgTRStatusPenerimaanPembayaran> GetList()
        {
            return pGetList();
        }

        private List<stgTRStatusPenerimaanPembayaran> pGetList()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMonitoringMatchingARFilter";

                command.Parameters.Add(command.CreateParameter("@vType", "DocumentType"));

                return this.ReadTransaction(command).ToList();
            }
        }
    }
}
