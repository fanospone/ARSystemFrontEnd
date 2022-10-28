
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
    public class TBGSysRepository : BaseRepository<tbgsysdata>
    {
        private DbContext _context;
        public TBGSysRepository(DbContext context) : base(context)
        {
            _context = context;
        }


        public string getStringData(string query)
        {

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = query;
                return command.ExecuteScalar() == null ? "" : command.ExecuteScalar().ToString();
            }



        }

        public int getNumericData(string query)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = query;
                return int.Parse(command.ExecuteScalar() == null ? "0" : command.ExecuteScalar().ToString());
            }

        }
    }
}
