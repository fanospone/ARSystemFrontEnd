using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace ARSystem.Domain.Repositories
{
    public class ReportInvoiceTowerRepository
    {
        private DbContext _context;

        public ReportInvoiceTowerRepository(DbContext context)
        {
            _context = context;
        }
        

        #region AR Report Invoice Parameters

        public List<vmReportInvoiceYear> GetReportInvoiceYear()
        {
            List<vmReportInvoiceYear> data = new List<vmReportInvoiceYear>();
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspReportInvoiceTowerLoadData";

                command.Parameters.Add(command.CreateParameter("@p_Type", "DDLYear"));

                SqlDataReader dr = (SqlDataReader)command.ExecuteReader();
                vmReportInvoiceYear temp;
                while (dr.Read())
                {
                    temp = new vmReportInvoiceYear();
                    temp.ValueId = dr["ValID"].ToString();
                    temp.ValueDesc = dr["ValDesc"].ToString();

                    data.Add(temp);
                }
                dr.Close();
            }
            return data;
        }

        public List<vmReportInvoiceMonth> GetReportInvoiceMonth()
        {
            List<vmReportInvoiceMonth> data = new List<vmReportInvoiceMonth>();
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspReportInvoiceTowerLoadData";

                command.Parameters.Add(command.CreateParameter("@p_Type", "DDLMonth"));

                SqlDataReader dr = (SqlDataReader)command.ExecuteReader();
                vmReportInvoiceMonth temp;
                while (dr.Read())
                {
                    temp = new vmReportInvoiceMonth();
                    temp.ValueId = dr["ValID"].ToString();
                    temp.ValueDesc = dr["ValDesc"].ToString();

                    data.Add(temp);
                }
                dr.Close();
            }
            return data;
        }

        public List<vmReportInvoiceWeek> GetReportInvoiceWeek(int strYearPosting,int strMonthPosting)
        {
            List<vmReportInvoiceWeek> data = new List<vmReportInvoiceWeek>();
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspReportInvoiceTowerLoadData";

                command.Parameters.Add(command.CreateParameter("@p_Type", "DDLWeek"));
                command.Parameters.Add(command.CreateParameter("@p_PostingYear", strYearPosting));
                command.Parameters.Add(command.CreateParameter("@p_PostingMonth", strMonthPosting));

                SqlDataReader dr = (SqlDataReader)command.ExecuteReader();
                vmReportInvoiceWeek temp;
                while (dr.Read())
                {
                    temp = new vmReportInvoiceWeek();
                    temp.ValueId = dr["ValID"].ToString();
                    temp.ValueDesc = dr["ValDesc"].ToString();

                    data.Add(temp);
                }
                dr.Close();
            }
            return data;
        }

        public List<vmReportInvoiceMaxMinLogDate> GetReportInvoiceMaxMinLogDate(int strYearPosting, int strMonthPosting,int strWeekPosting)
        {
            List<vmReportInvoiceMaxMinLogDate> data = new List<vmReportInvoiceMaxMinLogDate>();
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspReportInvoiceTowerLoadData";

                command.Parameters.Add(command.CreateParameter("@p_Type", "GetMaxMinLogDate"));
                command.Parameters.Add(command.CreateParameter("@p_PostingYear", strYearPosting));
                command.Parameters.Add(command.CreateParameter("@p_PostingMonth", strMonthPosting));
                command.Parameters.Add(command.CreateParameter("@p_PostingWeek", strWeekPosting));

                SqlDataReader dr = (SqlDataReader)command.ExecuteReader();
                vmReportInvoiceMaxMinLogDate temp;
                while (dr.Read())
                {
                    temp = new vmReportInvoiceMaxMinLogDate();
                    temp.MaxLogDate = DateTime.Parse(dr["MaxLogDate"].ToString());
                    temp.MinLogDate = DateTime.Parse(dr["MinLogDate"].ToString());

                    data.Add(temp);
                }
                dr.Close();
            }
            return data;
        }

        #endregion
    }
}
