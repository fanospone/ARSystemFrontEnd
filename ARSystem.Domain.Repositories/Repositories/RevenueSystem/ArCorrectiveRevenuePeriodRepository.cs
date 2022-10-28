
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ARSystem.Domain.Repositories
{
  public  class ArCorrectiveRevenuePeriodRepository : BaseRepository<ArCorrectiveRevenuePeriod>
    {
        private DbContext _context;
        public ArCorrectiveRevenuePeriodRepository(DbContext context) : base(context)
        {
            _context = context;
        }


        public ArCorrectiveRevenuePeriod GetPeriod()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspArGetCorrectiveRevuePeriod";

                return this.ReadTransaction(command).FirstOrDefault();
            }
        }

    }
}
