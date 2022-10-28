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
    public class mstProvinceRepository : BaseRepository<mstProvince>
    {
        private DbContext _context;
        private DbContext _tbgSysContext;
        public mstProvinceRepository(DbContext context) : base(context)
		{
            _context = context;
        }
        public List<mstProvince> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }
        private List<mstProvince> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstProvince";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
    }

    public class mstTbgSysProvinceRepository : BaseRepository<mstProvince>
    {
        private DbContext _context;
        public mstTbgSysProvinceRepository(DbContext context) : base(context)
        {
            _context = context;
        }
        public List<mstProvince> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }
        private List<mstProvince> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstProvince";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }

    }

}
