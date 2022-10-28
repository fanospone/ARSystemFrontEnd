
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
	public class dwhARRevenueSysSummaryRepository : BaseRepository<dwhARRevenueSysSummary>
	{
        private DbContext _context;
        public dwhARRevenueSysSummaryRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(string whereClause = "")
        {
            return pGetCount(whereClause);
        }

        public List<dwhARRevenueSysSummary> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }

        public List<dwhARRevenueSysSummary> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
        }

        public dwhARRevenueSysSummary GetByPK(string sONumber, int dataYear)
        {
            return pGetByPK(sONumber, dataYear);
        }

        public dwhARRevenueSysSummary Create(dwhARRevenueSysSummary data)
        {
            return pCreate(data);
        }

        public List<dwhARRevenueSysSummary> CreateBulky(List<dwhARRevenueSysSummary> data)
        {
            return pCreateBulky(data);
        }

        public dwhARRevenueSysSummary Update(dwhARRevenueSysSummary data)
        {
            return pUpdate(data);
        }

        public bool DeleteByPK(string sONumber, int dataYear)
        {
            pDeleteByPK(sONumber, dataYear);
            return true;
        }

        public bool DeleteByFilter(string whereClause)
        {
            pDeleteByFilter(whereClause);
            return true;
        }

        #region Private

        private int pGetCount(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspdwhARRevenueSysSummary";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        private List<dwhARRevenueSysSummary> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspdwhARRevenueSysSummary";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<dwhARRevenueSysSummary> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspdwhARRevenueSysSummary";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
        private dwhARRevenueSysSummary pGetByPK(string sONumber, int dataYear)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspdwhARRevenueSysSummary";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@SONumber", sONumber));
                command.Parameters.Add(command.CreateParameter("@DataYear", dataYear));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        private dwhARRevenueSysSummary pCreate(dwhARRevenueSysSummary data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<dwhARRevenueSysSummary>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspdwhARRevenueSysSummary";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                this.WriteTransaction(command);

                return data;
            }
        }
        private List<dwhARRevenueSysSummary> pCreateBulky(List<dwhARRevenueSysSummary> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<dwhARRevenueSysSummary>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspdwhARRevenueSysSummary";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private dwhARRevenueSysSummary pUpdate(dwhARRevenueSysSummary data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<dwhARRevenueSysSummary>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspdwhARRevenueSysSummary";

                command.Parameters.Add(command.CreateParameter("@vType", "Update"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@SONumber", data.SONumber));
                command.Parameters.Add(command.CreateParameter("@DataYear", data.DataYear));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private void pDeleteByPK(string sONumber, int dataYear)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspdwhARRevenueSysSummary";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
                command.Parameters.Add(command.CreateParameter("@SONumber", sONumber));
                command.Parameters.Add(command.CreateParameter("@DataYear", dataYear));

                command.ExecuteNonQuery();
            }
        }
        private void pDeleteByFilter(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspdwhARRevenueSysSummary";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }
        #endregion

    }
}