
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
	public class trxRAPurchaseOrderDetailRepository : BaseRepository<trxRAPurchaseOrderDetail>
	{
		private DbContext _context;
		public trxRAPurchaseOrderDetailRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxRAPurchaseOrderDetail> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxRAPurchaseOrderDetail> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxRAPurchaseOrderDetail GetByPK(int purchaseOrderID, long trxReconcileID)
		{
			return pGetByPK(purchaseOrderID, trxReconcileID);
		}

		public trxRAPurchaseOrderDetail Create(trxRAPurchaseOrderDetail data)
		{
			return pCreate(data);
		}

		public List<trxRAPurchaseOrderDetail> CreateBulky(List<trxRAPurchaseOrderDetail> data)
		{
			return pCreateBulky(data);
		}

		public trxRAPurchaseOrderDetail Update(trxRAPurchaseOrderDetail data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int purchaseOrderID, long trxReconcileID)
		{
			pDeleteByPK(purchaseOrderID, trxReconcileID);
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
				command.CommandText = "dbo.usptrxRAPurchaseOrderDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxRAPurchaseOrderDetail> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrderDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxRAPurchaseOrderDetail> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrderDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxRAPurchaseOrderDetail pGetByPK(int purchaseOrderID, long trxReconcileID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrderDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@PurchaseOrderID", purchaseOrderID));
				command.Parameters.Add(command.CreateParameter("@trxReconcileID", trxReconcileID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxRAPurchaseOrderDetail pCreate(trxRAPurchaseOrderDetail data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxRAPurchaseOrderDetail>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrderDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxRAPurchaseOrderDetail> pCreateBulky(List<trxRAPurchaseOrderDetail> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxRAPurchaseOrderDetail>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrderDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxRAPurchaseOrderDetail pUpdate(trxRAPurchaseOrderDetail data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxRAPurchaseOrderDetail>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrderDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@PurchaseOrderID", data.PurchaseOrderID));
				command.Parameters.Add(command.CreateParameter("@trxReconcileID", data.trxReconcileID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int purchaseOrderID, long trxReconcileID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrderDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@PurchaseOrderID", purchaseOrderID));
				command.Parameters.Add(command.CreateParameter("@trxReconcileID", trxReconcileID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrderDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}