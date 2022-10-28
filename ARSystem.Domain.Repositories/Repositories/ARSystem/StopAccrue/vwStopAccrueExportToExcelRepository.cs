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
    public class vwStopAccrueExportToExcelRepository : BaseRepository<vwStopAccrueExportToExcel>
    {
        private DbContext _context;
        public vwStopAccrueExportToExcelRepository(DbContext context) : base(context)
		{
            _context = context;
        }
        public List<vwStopAccrueExportToExcel> GetListExportAllDetailByExcel(string submissionDateFrom, string submissionDateTo, string directorateCode)
        {
            return pGetListExportAllDetailByExcel(submissionDateFrom, submissionDateTo,directorateCode);
        }
        public List<vwStopAccrueExportToExcel> pGetListExportAllDetailByExcel(string submissionDateFrom, string submissionDateTo, string directorateCode)
        {
            if (directorateCode =="" || directorateCode == "null"  )
            {
                directorateCode = "undefined";
            }

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspStopAccrueDashboardToExcel";

                command.Parameters.Add(command.CreateParameter("@submissionDateFrom", submissionDateFrom));
                command.Parameters.Add(command.CreateParameter("@submissionDateTo", submissionDateTo));
                command.Parameters.Add(command.CreateParameter("@directorateCode", directorateCode));

                return this.ReadTransaction(command).ToList();
            }
        }

    }
}
