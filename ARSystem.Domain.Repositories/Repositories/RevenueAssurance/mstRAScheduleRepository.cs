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
    public class mstRAScheduleRepository : BaseRepository<mstRASchedule>
    {
        private DbContext _context;
        public mstRAScheduleRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(string whereClause = "")
        {
            return pGetCount(whereClause);
        }

        public List<mstRASchedule> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }

        public List<mstRASchedule> GetMappingList(string whereClause = "", string orderBy = "")
        {
            return pGetMappingList(whereClause, orderBy);
        }

        public List<mstRASchedule> GetMappingRASChedule(string whereClause = "", string orderBy = "")
        {
            return pGetMappingRASChedule(whereClause, orderBy);
        }


        public List<mstRASchedule> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
        }

        public mstRASchedule GetByPK(long iD)
        {
            return pGetByPK(iD);
        }

        public mstRASchedule Create(mstRASchedule data)
        {
            return pCreate(data);
        }

        public int CreateBulky(int NextActivity, string ListID, string CreatedBy)
        {
            return pCreateBulky(NextActivity, ListID, CreatedBy);
        }

        public mstRASchedule Update(mstRASchedule data)
        {
            return pUpdate(data);
        }

        public bool DeleteByPK(long iD)
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
                command.CommandText = "dbo.uspmstRASchedule";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        private List<mstRASchedule> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstRASchedule";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<mstRASchedule> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstRASchedule";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<mstRASchedule> pGetMappingList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstRASchedule";

                command.Parameters.Add(command.CreateParameter("@vType", "GetMappingBAPS"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<mstRASchedule> pGetMappingRASChedule(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstRASchedule";

                command.Parameters.Add(command.CreateParameter("@vType", "GetMappingRASChedule"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }

        private mstRASchedule pGetByPK(long iD)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstRASchedule";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@ID", iD));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        private mstRASchedule pCreate(mstRASchedule data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<mstRASchedule>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstRASchedule";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                data.ID = this.WriteTransaction(command);

                return data;
            }
        }
        private int pCreateBulky(int NextActivity, string ListID, string CreatedBy)
        {
            using (var command = _context.CreateCommand())
            {
                //string xml = Helper.XmlSerializer<List<mstRASchedule>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstRASchedule";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@ListID", ListID));
                command.Parameters.Add(command.CreateParameter("@NextActivity", NextActivity));
                command.Parameters.Add(command.CreateParameter("@CreatedBy", CreatedBy));

                //command.ExecuteNonQuery();

                return command.ExecuteNonQuery();
            }
        }
        private mstRASchedule pUpdate(mstRASchedule data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<mstRASchedule>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstRASchedule";

                command.Parameters.Add(command.CreateParameter("@vType", "Update"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private void pDeleteByPK(long iD)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstRASchedule";

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
                command.CommandText = "dbo.uspmstRASchedule";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }
        #endregion

    }
}
