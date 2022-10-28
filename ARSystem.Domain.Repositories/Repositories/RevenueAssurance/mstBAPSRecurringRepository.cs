using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.Models.RevenueAssurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Repositories.Repositories.RevenueAssurance
{
    public class mstBAPSRecurringRepository : BaseRepository<mstBAPSRecurring>
    {
        private DbContext _context;
        public mstBAPSRecurringRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(string whereClause = "")
        {
            return pGetCount(whereClause);
        }

        public List<mstBAPSRecurring> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }

        public List<mstBAPSRecurring> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
        }

        public mstBAPSRecurring GetByPK(int iD)
        {
            return pGetByPK(iD);
        }

        public mstBAPSRecurring Create(mstBAPSRecurring data)
        {
            return pCreate(data);
        }

        public List<mstBAPSRecurring> CreateBulky(List<mstBAPSRecurring> data)
        {
            return pCreateBulky(data);
        }

        public mstBAPSRecurring Update(mstBAPSRecurring data)
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

        private int pGetCount(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBAPSRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        private List<mstBAPSRecurring> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBAPSRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<mstBAPSRecurring> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBAPSRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
        private mstBAPSRecurring pGetByPK(int iD)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBAPSRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@ID", iD));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        private mstBAPSRecurring pCreate(mstBAPSRecurring data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<mstBAPSRecurring>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBAPSRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                data.ID = this.WriteTransaction(command);

                return data;
            }
        }
        private List<mstBAPSRecurring> pCreateBulky(List<mstBAPSRecurring> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<mstBAPSRecurring>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBAPSRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private mstBAPSRecurring pUpdate(mstBAPSRecurring data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<mstBAPSRecurring>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBAPSRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "Update"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private void pDeleteByPK(int iD)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBAPSRecurring";

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
                command.CommandText = "dbo.uspmstBAPSRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }
        #endregion

    }
}
