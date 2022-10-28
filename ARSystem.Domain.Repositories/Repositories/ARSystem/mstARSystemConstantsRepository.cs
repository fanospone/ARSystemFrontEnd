using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Repositories
{
    public class mstARSystemConstantsRepository : BaseRepository<mstARSystemConstants>
    {
        private DbContext _context;
        public mstARSystemConstantsRepository(DbContext context) : base(context)
		{
            _context = context;
        }
        public string GetByName(string name)
        {
            return pGetByName(name);
        }
        private string pGetByName(string name)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.fnGetConstantsValue";

                command.Parameters.Add(command.CreateParameter("@vName", name));

                var retValParam = new SqlParameter("RetVal", SqlDbType.VarChar)
                {
                    //Set this property as return value
                    Direction = ParameterDirection.ReturnValue
                };

                command.Parameters.Add(retValParam);
                command.ExecuteScalar();

               return retValParam.Value.ToString();

                //return "";//return this.ReadTransaction(command).SingleOrDefault().ToString();
            }
        }
    }
}
