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
    public class MstRATargetNewBapsRepository : BaseRepository<MstRATargetNewBaps>
    {
        private DbContext _context;
        public MstRATargetNewBapsRepository(DbContext context) : base(context)
		{
            _context = context;
        }
        public List<MstRATargetNewBaps> UploadTargetNewBaps(List<MstRATargetNewBaps> data, string whereClause)
        {
            return pUploadTargetNewBaps(data, whereClause);
        }
        public List<MstRATargetNewBaps> CreateBulkyNewBaps(List<MstRATargetNewBaps> data)
        {
            return pCreateBulkyNewBaps(data);
        }

        private List<MstRATargetNewBaps> pUploadTargetNewBaps(List<MstRATargetNewBaps> data, string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<MstRATargetNewBaps>>(data);
                //increase timeout to 4 minutes
                command.CommandTimeout = 240;

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMstRATargetRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "UploadTargetNewBaps"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<MstRATargetNewBaps> pCreateBulkyNewBaps(List<MstRATargetNewBaps> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<MstRATargetNewBaps>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMstRATargetRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulkyNewBaps"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }

        public MstRATargetNewBaps UpdateTargetNewBaps(MstRATargetNewBaps data)
        {
            return pUpdateTargetNewBaps(data);
        }

        public bool DeleteTargetNewBapsByPK(long targetID)
        {
            pDeleteTargetNewBapsByPK(targetID);
            return true;
        }
        public MstRATargetNewBaps GetByPK(long iD)
        {
            return pGetByPK(iD);
        }
        private MstRATargetNewBaps pUpdateTargetNewBaps(MstRATargetNewBaps data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<MstRATargetNewBaps>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMstRATargetRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateTargetNewBaps"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private MstRATargetNewBaps   pGetByPK(long iD)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMstRATargetRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@ID", iD));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        private void pDeleteTargetNewBapsByPK(long targetID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMstRATargetRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteTargetNewBapsByPK"));
                command.Parameters.Add(command.CreateParameter("@ID", targetID));

                command.ExecuteNonQuery();
            }
        }
    }
}
