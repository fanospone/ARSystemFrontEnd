
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
    public class vmAccrueLogUserConfirmRepository : BaseRepository<vmAccrueLogUserConfirm>
    {
        private DbContext _context;
        public vmAccrueLogUserConfirmRepository(DbContext context) : base(context)
		{
            _context = context;
        }
        public List<vmAccrueLogUserConfirm> GetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspAccrueLogUserConfirm";
                
                return this.ReadTransaction(command).ToList();
            }
        }

    }
}
