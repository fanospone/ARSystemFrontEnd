
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystemFrontEnd.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

namespace ARSystemFrontEnd.Domain.Repositories
{
	public class trxARCashInForecastVsActualRepository : BaseRepository<uspARCashInForecastVsActual>
	{
		private DbContext _context;
		public trxARCashInForecastVsActualRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause, int year, int quarter)
		{
			return pGetCount(whereClause, year, quarter);
		}

		public List<uspARCashInForecastVsActual> GetList(string whereClause, int year, int quarter, string orderBy = "")
		{
			return pGetList(whereClause, year, quarter, orderBy);
		}

		public List<uspARCashInForecastVsActual> GetPaged(string whereClause, int year, int quarter, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, year, quarter, orderBy, rowSkip, pageSize);
		}
        public List<uspARCashInForecastVsActual> CheckPiCaValidation(int year, int quarter, int month, string orderBy)
        {
            return pCheckPiCaValidation( year, quarter, month,orderBy);
        }

        
        public uspARCashInForecastVsActual GetByPK(int iD)
		{
			return pGetByPK(iD);
		}
        public uspARCashInForecastVsActual GetByOperatorInPeriod(string operatorID, int year, int quarter)
        {
            return pGetByOperatorInPeriod(operatorID,year, quarter);
        }
        public uspARCashInForecastVsActual Create(uspARCashInForecastVsActual data)
		{
			return pCreate(data);
		}

		public List<uspARCashInForecastVsActual> CreateBulky(List<uspARCashInForecastVsActual> data)
		{
			return pCreateBulky(data);
		}

		public List<uspARCashInForecastVsActual> Update(List<uspARCashInForecastVsActual> data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int iD)
		{
			pDeleteByPK(iD);
			return true;
		}

		public bool DeleteByFilter(string whereClause)
		{
			pDeleteByFilter(whereClause);
			return true;
		}

		#region Private

		private int pGetCount(string whereClause, int year, int quarter)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsActual";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<uspARCashInForecastVsActual> pGetList(string whereClause, int year, int quarter, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsActual";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<uspARCashInForecastVsActual> pGetPaged(string whereClause, int year, int quarter, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsActual";
                command.CommandTimeout = 320;

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));
				command.Parameters.Add(command.CreateParameter("@vYear", year));
				command.Parameters.Add(command.CreateParameter("@vQuarter", quarter));

				return this.ReadTransaction(command).ToList();
			}
		}
        private List<uspARCashInForecastVsActual> pCheckPiCaValidation(int year, int quarter, int month, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxARCashInForecastVsActual";
                command.CommandTimeout = 320;

                command.Parameters.Add(command.CreateParameter("@vType", "CheckPiCaValidation"));
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vInputPicaMonth", month));
                command.Parameters.Add(command.CreateParameter("@vQuarter", quarter));

                return this.ReadTransaction(command).ToList();
            }
        }

        private uspARCashInForecastVsActual pGetByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsActual";
                command.CommandTimeout = 320;

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
        private uspARCashInForecastVsActual pGetByOperatorInPeriod(string operatorID, int year, int quarter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxARCashInForecastVsActual";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByOperatorInPeriod"));
                command.Parameters.Add(command.CreateParameter("@vOperatorID", operatorID));
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vQuarter", quarter));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }

        private uspARCashInForecastVsActual pCreate(uspARCashInForecastVsActual data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<uspARCashInForecastVsActual>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsActual";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<uspARCashInForecastVsActual> pCreateBulky(List<uspARCashInForecastVsActual> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<uspARCashInForecastVsActual>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsActual";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private List<uspARCashInForecastVsActual> pUpdate(List<uspARCashInForecastVsActual> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<uspARCashInForecastVsActual>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsActual";

				command.Parameters.Add(command.CreateParameter("@vType", "SubmitData"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsActual";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsActual";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}