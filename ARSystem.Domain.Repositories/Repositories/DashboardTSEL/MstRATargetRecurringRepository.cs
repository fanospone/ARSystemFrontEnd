
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
	public class MstRATargetRecurringRepository : BaseRepository<MstRATargetRecurring>
	{
		private DbContext _context;
		public MstRATargetRecurringRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<MstRATargetRecurring> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<MstRATargetRecurring> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public MstRATargetRecurring GetByPK(long iD)
		{
			return pGetByPK(iD);
		}

		public MstRATargetRecurring Create(MstRATargetRecurring data)
		{
			return pCreate(data);
		}

        public List<MstRATargetRecurring> CreateBulky(List<MstRATargetRecurring> data)
        {
            return pCreateBulky(data);
        }
       
        public List<MstRATargetRecurring> UploadTargetRecurring(List<MstRATargetRecurring> data, string whereClause)
        {
            return pUploadTargetRecurring(data, whereClause);
        }
        public List<MstRATargetRecurring> UploadTargetRecurringNewProduct(List<MstRATargetRecurring> data, string whereClause)
        {
            return pUploadTargetRecurringNewProduct(data, whereClause);
        }


        public MstRATargetRecurring Update(MstRATargetRecurring data)
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

        public List<MstRATargetRecurring> GetTarget(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMstRATargetRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "GetTarget"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }

        public List<MstRATargetRecurring> GetAllTarget(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMstRATargetRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "GetAllTarget"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }

        #region Private

        private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRATargetRecurring";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<MstRATargetRecurring> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRATargetRecurring";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<MstRATargetRecurring> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRATargetRecurring";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private MstRATargetRecurring pGetByPK(long iD)
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
		private MstRATargetRecurring pCreate(MstRATargetRecurring data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<MstRATargetRecurring>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRATargetRecurring";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<MstRATargetRecurring> pCreateBulky(List<MstRATargetRecurring> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<MstRATargetRecurring>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRATargetRecurring";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
        private MstRATargetRecurring pUpdate(MstRATargetRecurring data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<MstRATargetRecurring>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRATargetRecurring";

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
				command.CommandText = "dbo.uspMstRATargetRecurring";

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
				command.CommandText = "dbo.uspMstRATargetRecurring";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}

        private List<MstRATargetRecurring> pUploadTargetRecurring(List<MstRATargetRecurring> data, string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<MstRATargetRecurring>>(data);
                //increase timeout to 4 minutes
                command.CommandTimeout = 240;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMstRATargetRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "UploadTargetRecurring"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<MstRATargetRecurring> pUploadTargetRecurringNewProduct(List<MstRATargetRecurring> data, string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<MstRATargetRecurring>>(data);
                //increase timeout to 4 minutes
                command.CommandTimeout = 240;

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspMstRATargetRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "UploadTargetRecurringNewProduct"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.ReadTransaction(command).ToList();
            }
        }

        #endregion

        
        //public mstEmail UpdateRecurringHistoryInputTarget(mstEmail data)
        //{
        //    return pUpdateRecurringHistoryInputTarget(data);
        //}

        //public bool DeleteUpdateRecurringHistoryInputTargetByPK(int mstEmailID)
        //{
        //    pDeleteUpdateRecurringHistoryInputTargetByPK(mstEmailID);
        //    return true;
        //}
        //private mstEmail pUpdateRecurringHistoryInputTarget(mstEmail data)
        //{
        //    using (var command = _context.CreateCommand())
        //    {
        //        string xml = Helper.XmlSerializer<mstEmail>(data);

        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "dbo.uspmstEmail";

        //        command.Parameters.Add(command.CreateParameter("@vType", "Update"));
        //        command.Parameters.Add(command.CreateParameter("@vXml", xml));
        //        command.Parameters.Add(command.CreateParameter("@mstEmailID", data.mstEmailID));

        //        command.ExecuteNonQuery();

        //        return data;
        //    }
        //}
        //private void pDeleteUpdateRecurringHistoryInputTargetByPK(int mstEmailID)
        //{
        //    using (var command = _context.CreateCommand())
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "dbo.uspmstEmail";

        //        command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
        //        command.Parameters.Add(command.CreateParameter("@mstEmailID", mstEmailID));

        //        command.ExecuteNonQuery();
        //    }
        //}




    }
}