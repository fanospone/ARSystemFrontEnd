using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.ViewModels;

namespace ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem
{
    public class uspInvoiceProductionSummaryRepository : BaseRepository<vmInvoiceProduction>
    {
        private DbContext _context;
        public uspInvoiceProductionSummaryRepository(DbContext context) : base(context)
		{
            _context = context;
        }

        public List<vmInvoiceProduction> GetSummaryOutStanding(vmInvoiceProductionPost filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspDashboardInvoiceOutStanding";

                command.Parameters.Add(command.CreateParameter("@vOperator", filter.vOperator));
                command.Parameters.Add(command.CreateParameter("@vCompany", filter.vCompany));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveStart", filter.vBAPSReceiveStart));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveEnd", filter.vBAPSReceiveEnd));
                command.Parameters.Add(command.CreateParameter("@vAgingCategory", filter.vAgingCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceCategory", filter.vInvoiceCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateStart", filter.vInvoiceDateStart));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateEnd", filter.vInvoiceDateEnd));
                command.Parameters.Add(command.CreateParameter("@vPKP", filter.vPKP));

                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vmInvoiceProduction> GetSummaryProduction(vmInvoiceProductionPost filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspDashboardInvoiceProduction";

                command.Parameters.Add(command.CreateParameter("@vOperator", filter.vOperator));
                command.Parameters.Add(command.CreateParameter("@vCompany", filter.vCompany));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveStart", filter.vBAPSReceiveStart));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveEnd", filter.vBAPSReceiveEnd));
                command.Parameters.Add(command.CreateParameter("@vAgingCategory", filter.vAgingCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceCategory", filter.vInvoiceCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateStart", filter.vInvoiceDateStart));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateEnd", filter.vInvoiceDateEnd));
                command.Parameters.Add(command.CreateParameter("@vPKP", filter.vPKP));

                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vmInvoiceProduction> GetHeaderOutStanding(string vType, vmInvoiceProductionPost filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspDashboardInvoiceOutStandingHeader";

                command.Parameters.Add(command.CreateParameter("@vType", vType));
                command.Parameters.Add(command.CreateParameter("@vOperator", filter.vOperator));
                command.Parameters.Add(command.CreateParameter("@vCompany", filter.vCompany));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveStart", filter.vBAPSReceiveStart));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveEnd", filter.vBAPSReceiveEnd));
                command.Parameters.Add(command.CreateParameter("@vAgingCategory", filter.vAgingCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceCategory", filter.vInvoiceCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateStart", filter.vInvoiceDateStart));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateEnd", filter.vInvoiceDateEnd));
                command.Parameters.Add(command.CreateParameter("@vPKP", filter.vPKP));

                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vmInvoiceProduction> GetDetailOutStanding(string vType, vmInvoiceProductionPost filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspDashboardInvoiceOutStandingDetail";

                command.Parameters.Add(command.CreateParameter("@vType", vType));
                command.Parameters.Add(command.CreateParameter("@vOperator", filter.vOperator));
                command.Parameters.Add(command.CreateParameter("@vCompany", filter.vCompany));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveStart", filter.vBAPSReceiveStart));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveEnd", filter.vBAPSReceiveEnd));
                command.Parameters.Add(command.CreateParameter("@vAgingCategory", filter.vAgingCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceCategory", filter.vInvoiceCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateStart", filter.vInvoiceDateStart));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateEnd", filter.vInvoiceDateEnd));
                command.Parameters.Add(command.CreateParameter("@vPKP", filter.vPKP));

                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vmInvoiceProduction> GetHeaderProduction(string vType, vmInvoiceProductionPost filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspDashboardInvoiceProductionHeader";

                command.Parameters.Add(command.CreateParameter("@vType", vType));
                command.Parameters.Add(command.CreateParameter("@vOperator", filter.vOperator));
                command.Parameters.Add(command.CreateParameter("@vCompany", filter.vCompany));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveStart", filter.vBAPSReceiveStart));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveEnd", filter.vBAPSReceiveEnd));
                command.Parameters.Add(command.CreateParameter("@vAgingCategory", filter.vAgingCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceCategory", filter.vInvoiceCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateStart", filter.vInvoiceDateStart));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateEnd", filter.vInvoiceDateEnd));
                command.Parameters.Add(command.CreateParameter("@vPKP", filter.vPKP));

                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vmInvoiceProduction> GetDetailProduction(string vType, vmInvoiceProductionPost filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspDashboardInvoiceProductionDetail";

                command.Parameters.Add(command.CreateParameter("@vType", vType));
                command.Parameters.Add(command.CreateParameter("@vOperator", filter.vOperator));
                command.Parameters.Add(command.CreateParameter("@vCompany", filter.vCompany));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveStart", filter.vBAPSReceiveStart));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveEnd", filter.vBAPSReceiveEnd));
                command.Parameters.Add(command.CreateParameter("@vAgingCategory", filter.vAgingCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceCategory", filter.vInvoiceCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateStart", filter.vInvoiceDateStart));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateEnd", filter.vInvoiceDateEnd));
                command.Parameters.Add(command.CreateParameter("@vPKP", filter.vPKP));

                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vmInvoiceProduction> GetAllOutStanding(vmInvoiceProductionPost filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspDashboardInvoiceReport";

                command.Parameters.Add(command.CreateParameter("@vType", "Outstanding"));
                command.Parameters.Add(command.CreateParameter("@vOperator", filter.vOperator));
                command.Parameters.Add(command.CreateParameter("@vCompany", filter.vCompany));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveStart", filter.vBAPSReceiveStart));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveEnd", filter.vBAPSReceiveEnd));
                command.Parameters.Add(command.CreateParameter("@vAgingCategory", filter.vAgingCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceCategory", filter.vInvoiceCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateStart", filter.vInvoiceDateStart));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateEnd", filter.vInvoiceDateEnd));
                command.Parameters.Add(command.CreateParameter("@vPKP", filter.vPKP));

                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vmInvoiceProduction> GetAllProduction(vmInvoiceProductionPost filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspDashboardInvoiceReport";

                command.Parameters.Add(command.CreateParameter("@vType", "Production"));
                command.Parameters.Add(command.CreateParameter("@vOperator", filter.vOperator));
                command.Parameters.Add(command.CreateParameter("@vCompany", filter.vCompany));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveStart", filter.vBAPSReceiveStart));
                command.Parameters.Add(command.CreateParameter("@vBAPSReceiveEnd", filter.vBAPSReceiveEnd));
                command.Parameters.Add(command.CreateParameter("@vAgingCategory", filter.vAgingCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceCategory", filter.vInvoiceCategory));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateStart", filter.vInvoiceDateStart));
                command.Parameters.Add(command.CreateParameter("@vInvoiceDateEnd", filter.vInvoiceDateEnd));
                command.Parameters.Add(command.CreateParameter("@vPKP", filter.vPKP));

                return this.ReadTransaction(command).ToList();
            }
        }
    }
}
