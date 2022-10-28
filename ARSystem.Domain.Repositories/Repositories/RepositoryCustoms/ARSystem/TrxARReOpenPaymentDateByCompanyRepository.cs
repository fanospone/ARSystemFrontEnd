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
    public class TrxARReOpenPaymentDateByCompanyRepository : BaseRepository<TrxARReOpenPaymentDate>
    {
        private DbContext _context;
        public TrxARReOpenPaymentDateByCompanyRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(string whereClause = "")
        {
            return pGetCount(whereClause);
        }

        public List<TrxARReOpenPaymentDate> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }

        public List<TrxARReOpenPaymentDate> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
        }

        public TrxARReOpenPaymentDate GetByPK(int iD)
        {
            return pGetByPK(iD);
        }

        public TrxARReOpenPaymentDate Create(TrxARReOpenPaymentDate data)
        {
            return pCreate(data);
        }

        public List<TrxARReOpenPaymentDate> CreateBulky(List<TrxARReOpenPaymentDate> data)
        {
            return pCreateBulky(data);
        }

        public TrxARReOpenPaymentDate Update(TrxARReOpenPaymentDate data)
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
                command.CommandText = "dbo.uspTrxARReOpenPaymentDateByCompany";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        private List<TrxARReOpenPaymentDate> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARReOpenPaymentDateByCompany";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<TrxARReOpenPaymentDate> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARReOpenPaymentDateByCompany";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
        private TrxARReOpenPaymentDate pGetByPK(int iD)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARReOpenPaymentDateByCompany";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@ID", iD));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        private TrxARReOpenPaymentDate pCreate(TrxARReOpenPaymentDate data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<TrxARReOpenPaymentDate>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARReOpenPaymentDateByCompany";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                data.ID = this.WriteTransaction(command);

                return data;
            }
        }
        private List<TrxARReOpenPaymentDate> pCreateBulky(List<TrxARReOpenPaymentDate> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<TrxARReOpenPaymentDate>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARReOpenPaymentDateByCompany";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private TrxARReOpenPaymentDate pUpdate(TrxARReOpenPaymentDate data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<TrxARReOpenPaymentDate>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARReOpenPaymentDateByCompany";

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
                command.CommandText = "dbo.uspTrxARReOpenPaymentDateByCompany";

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
                command.CommandText = "dbo.uspTrxARReOpenPaymentDateByCompany";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }
        #endregion
    }
}
