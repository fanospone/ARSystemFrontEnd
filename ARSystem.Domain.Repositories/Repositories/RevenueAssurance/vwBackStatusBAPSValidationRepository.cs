
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
	public class vwBackStatusBAPSValidationRepository : BaseRepository<vwBackStatusBAPSValidation>
	{
		private DbContext _context;
		public vwBackStatusBAPSValidationRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<vwBackStatusBAPSValidation> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<vwBackStatusBAPSValidation> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public List<BackStatusBAPSValidationData> ProcessBackStatus(string remark, List<BackStatusBAPSValidationData> dataList, string UserID)
		{
			return pProcessBackStatus(remark, dataList, UserID);
		}

		public List<int> GetListStipSiro()
		{
			return pGetListStipSiro();
		}

        #region Private

        private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwBackStatusBAPSValidation";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<vwBackStatusBAPSValidation> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwBackStatusBAPSValidation";
                    
				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<vwBackStatusBAPSValidation> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwBackStatusBAPSValidation";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}

		private List<BackStatusBAPSValidationData> pProcessBackStatus(string remark, List<BackStatusBAPSValidationData> dataList, string UserID)
		{

			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<BackStatusBAPSValidationData>>(dataList);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspBackStatusBAPSValidation";

				command.Parameters.Add(command.CreateParameter("@vType", "Submit"));
				command.Parameters.Add(command.CreateParameter("@vAuthor", UserID));
				command.Parameters.Add(command.CreateParameter("@vRemark", remark));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return dataList;
			}
		}

		private List<int> pGetListStipSiro()
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspBackStatusBAPSValidation";

				command.Parameters.Add(command.CreateParameter("@vType", "GetListStipSiro"));

				using (var reader = command.ExecuteReader())
				{
					List<int> siroList = new List<int>();

					while (reader.Read())
						siroList.Add(reader.GetInt32(0));

					return siroList;
				}
			}
		}
		#endregion

	}
}