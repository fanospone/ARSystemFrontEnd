
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
	public class trxARCashInForecastVsForecastRepository : BaseRepository<uspARCashInForecastVsForecast>
	{
		private DbContext _context;
		public trxARCashInForecastVsForecastRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause, int year, int quarter)
		{
			return pGetCount(whereClause, year, quarter);
		}

		public List<uspARCashInForecastVsForecast> GetList(string whereClause, int year, int quarter, string orderBy = "")
		{
			return pGetList(whereClause, year, quarter, orderBy);
		}

		public List<uspARCashInForecastVsForecast> GetPaged(string whereClause, int year, int quarter, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, year, quarter, orderBy, rowSkip, pageSize);
		}

		public uspARCashInForecastVsForecast GetByPK(int iD)
		{
			return pGetByPK(iD);
		}
        public uspARCashInForecastVsForecast GetByOperatorInPeriod(string operatorID, int year, int quarter)
        {
            return pGetByOperatorInPeriod(operatorID, year, quarter);
        }
        public uspARCashInForecastVsForecast Create(uspARCashInForecastVsForecast data)
		{
			return pCreate(data);
		}

		public List<uspARCashInForecastVsForecast> CreateBulky(List<uspARCashInForecastVsForecast> data)
		{
			return pCreateBulky(data);
		}

		public List<uspARCashInForecastVsForecast> Update(List<uspARCashInForecastVsForecast> data)
		{
			return pUpdate(data);
		}
        public List<uspARCashInForecastVsForecast> CheckPiCaValidation(int year, int quarter,  string orderBy)
        {
            return pCheckPiCaValidation(year, quarter, orderBy);
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
				command.CommandText = "dbo.usptrxARCashInForecastVsForecast";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<uspARCashInForecastVsForecast> pGetList(string whereClause, int year, int quarter, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsForecast";
                command.CommandTimeout = 320;

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<uspARCashInForecastVsForecast> pGetPaged(string whereClause, int year, int quarter, string orderBy, int rowSkip, int pageSize)
		{
            int currentQuarter = Convert.ToInt32(Math.Floor((decimal)((DateTime.Today.AddMonths(-1).Month) + 2) / 3));
            int numberOfMonthWithinQuarter = 0;
            int startMonth = currentQuarter * 3 - 2;
            if (currentQuarter == quarter)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (DateTime.Today.AddMonths(-1).Month == (startMonth + i))
                    {
                        numberOfMonthWithinQuarter = i + 1;
                    }
                }
            }
            else
            {
                //magic number, set 99, set to take actual amount
                numberOfMonthWithinQuarter = 99;
            }


            using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsForecast";
                command.CommandTimeout = 240;
				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vQuarter", quarter));
                command.Parameters.Add(command.CreateParameter("@vMonthWithinQuarter", numberOfMonthWithinQuarter));
                
                return this.ReadTransaction(command).ToList();
			}
		}
		private uspARCashInForecastVsForecast pGetByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsForecast";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
        private uspARCashInForecastVsForecast pGetByOperatorInPeriod(string operatorID, int year, int quarter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxARCashInForecastVsForecast";
                command.CommandTimeout = 320;
                command.Parameters.Add(command.CreateParameter("@vType", "GetByOperatorInPeriod"));
                command.Parameters.Add(command.CreateParameter("@vOperatorID", operatorID));
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vQuarter", quarter));
                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        private uspARCashInForecastVsForecast pCreate(uspARCashInForecastVsForecast data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<uspARCashInForecastVsForecast>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsForecast";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<uspARCashInForecastVsForecast> pCreateBulky(List<uspARCashInForecastVsForecast> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<uspARCashInForecastVsForecast>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsForecast";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private List<uspARCashInForecastVsForecast> pUpdate(List<uspARCashInForecastVsForecast> data)
		{
			using (var command = _context.CreateCommand())
			{
                string xml = Helper.XmlSerializer<List<uspARCashInForecastVsForecast>>(data);

                command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastVsForecast";

				command.Parameters.Add(command.CreateParameter("@vType", "SubmitDataHeader"));
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
				command.CommandText = "dbo.usptrxARCashInForecastVsForecast";

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
				command.CommandText = "dbo.usptrxARCashInForecastVsForecast";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
        private List<uspARCashInForecastVsForecast> pCheckPiCaValidation(int year, int quarter, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxARCashInForecastVsForecast";
                command.CommandTimeout = 320;
                int currentQuarter = Convert.ToInt32(Math.Floor((decimal)((DateTime.Today.AddMonths(-1).Month) + 2) / 3));
                int numberOfMonthWithinQuarter = 0;
                int startMonth = currentQuarter * 3 - 2;
                if (currentQuarter == quarter)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (DateTime.Today.AddMonths(-1).Month == (startMonth + i))
                        {
                            numberOfMonthWithinQuarter = i + 1;
                        }
                    }
                }
                else
                {
                    //magic number, set 99, set to take actual amount
                    numberOfMonthWithinQuarter = 99;
                }
                command.Parameters.Add(command.CreateParameter("@vType", "CheckPiCaValidation"));
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vQuarter", quarter));
                command.Parameters.Add(command.CreateParameter("@vMonthWithinQuarter", numberOfMonthWithinQuarter));

                return this.ReadTransaction(command).ToList();
            }
        }
        #endregion

    }
}