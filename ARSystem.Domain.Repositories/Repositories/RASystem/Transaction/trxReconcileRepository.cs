
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
	public class trxReconcileRepository : BaseRepository<trxReconcile>
	{
		private DbContext _context;
		public trxReconcileRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

        

        public List<trxReconcile> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxReconcile> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxReconcile GetByPK(long iD)
		{
			return pGetByPK(iD);
		}

		public trxReconcile Create(trxReconcile data)
		{
			return pCreate(data);
		}

		public List<trxReconcile> CreateBulky(List<trxReconcile> data)
		{
			return pCreateBulky(data);
		}

        public List<trxReconcile> UpdateBulkyPO(List<trxReconcile> data, long POId, string Type)
        {
            return pUpdateBulkyPO(data, POId, Type);
        }

        public List<trxReconcile> UpdateBulkyRTI(List<trxReconcile> data, int RTIID)
        {
            return pUpdateBulkyRTI(data, RTIID);
        }


        public trxReconcile Update(trxReconcile data)
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

        public trxReconcile UpdateReconcile(trxReconcile data)
        {
            return pUpdateReconcile(data);
        }

        public trxReconcile UpdateBOQ(trxReconcile data)
        {
            return pUpdateBOQ(data);
        }
        

        public List<trxReconcile> UpdateBAPSBulky(List<trxReconcile> data, int mstBAPSRecurringID, int mstRAActivityID)
        {
            return pUpdateBAPSBulky(data, mstBAPSRecurringID, mstRAActivityID);
        }

        public List<trxReconcile> UpdateBOQBulky(List<trxReconcile> data, int mstRABoqID, int mstRAActivityID)
        {
            return pUpdateBOQBulky(data, mstRABoqID, mstRAActivityID);
        }

        public trxReconcile UpdateBAPS(trxReconcile data)
        {
            return pUpdateBAPS(data);
        }

        public List<trxReconcileID> UpdateBulkyDocument(List<trxReconcileID> data, long DocumentID)
        {
            return pUpdateBulkyDocument(data, DocumentID);
        }

        public trxReconcile UpdateReconcileRejectNonTSEL(trxReconcile data, string CustomerID, string CompanyID, string POType, int PoDtlID)
        {
            return pUpdateReconcileRejectNonTSEL(data,CustomerID,CompanyID,POType,PoDtlID);
        }

        public int UpdateBulkyPOTSEL(long POId, string BOQID)
        {
            return pUpdateBulkyPOTSEL(POId, BOQID);
        }

        #region Private
        private trxReconcile pUpdateReconcile(trxReconcile data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReconcile>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateReconcile"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReconcile";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxReconcile> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReconcile";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxReconcile> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReconcile";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxReconcile pGetByPK(long iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReconcile";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxReconcile pCreate(trxReconcile data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxReconcile>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReconcile";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxReconcile> pCreateBulky(List<trxReconcile> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxReconcile>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReconcile";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxReconcile pUpdate(trxReconcile data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxReconcile>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReconcile";

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
				command.CommandText = "dbo.usptrxReconcile";

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
				command.CommandText = "dbo.usptrxReconcile";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
        private int pUpdateBulkyPOTSEL(long POId,string BOQID)
        {
            using (var command = _context.CreateCommand())
            {

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateBulkyPOTSEL"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", BOQID));
                command.Parameters.Add(command.CreateParameter("@ID", POId));

                return command.ExecuteNonQuery();
            }
        }

        private List<trxReconcile> pUpdateBulkyPO(List<trxReconcile> data, long POId,string Type)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReconcile>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateBulkyPO"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", POId));
                command.Parameters.Add(command.CreateParameter("@Type", Type));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private List<trxReconcile> pUpdateBulkyRTI(List<trxReconcile> data, long RTIID)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReconcile>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateBulkyRTI"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@_mstRAActivityID", RTIID));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private List<trxReconcileID> pUpdateBulkyDocument(List<trxReconcileID> data, long DocumentID)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReconcileID>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateBulkyDocument"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@_mstRAActivityID", DocumentID));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private trxReconcile pUpdateBOQ(trxReconcile data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReconcile>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateBOQ"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private List<trxReconcile> pUpdateBOQBulky(List<trxReconcile> data, int mstRABoqID, int mstRAActivityID)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReconcile>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateBOQBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", mstRABoqID));
                command.Parameters.Add(command.CreateParameter("@_mstRAActivityID", mstRAActivityID));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private trxReconcile pUpdateBAPS(trxReconcile data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReconcile>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateBAPS"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private List<trxReconcile> pUpdateBAPSBulky(List<trxReconcile> data, int mstBAPSRecurringID, int mstRAActivityID)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReconcile>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateBAPSBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", mstBAPSRecurringID));
                command.Parameters.Add(command.CreateParameter("@_mstRAActivityID", mstRAActivityID));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private trxReconcile pUpdateReconcileRejectNonTSEL(trxReconcile data, string CustomerID, string CompanyID, string POType, int PoDtlID)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReconcile>(data);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";
                command.Parameters.Add(command.CreateParameter("@vType", "UpdatedReconcileRejectedNonTSEL"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));
                command.Parameters.Add(command.CreateParameter("@POType", POType));
                command.Parameters.Add(command.CreateParameter("@PODtlID", PoDtlID));
                command.Parameters.Add(command.CreateParameter("@CustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@CompanyID", CompanyID));
                command.ExecuteNonQuery();
                return data;
            }
        }

        public int UpdateInflation(string ListID,decimal Percentage,string UserID,int Year)
        {
            using (var command = _context.CreateCommand())
            {

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "uspUpdateInflationYearly";//"dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@ModifiedBy", UserID));
                command.Parameters.Add(command.CreateParameter("@ListID", ListID));
                command.Parameters.Add(command.CreateParameter("@Percentage", Percentage));
                command.Parameters.Add(command.CreateParameter("@Year", Year));

                //command.ExecuteNonQuery();

                return command.ExecuteNonQuery();
            }
        }
        
        #endregion

    }
}