
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
	public class ExistingRepository
	{
		private DbContext _context;
        public ExistingRepository(DbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// untuk Update Data BAPS di Server TBGSys
        /// </summary>
        /// <param name="RejectBaps"></param>
        /// <returns></returns>
        public vmRejectBAPSExisting RejectBaps(vmRejectBAPSExisting RejectBaps)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<vmRejectBAPSExisting>(RejectBaps);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspProcessBapsReject";
                
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return RejectBaps;
            }
        }

        public string ConfirmBAPS(string vXML, string userID)
        {
            try
            {
                using (var command = _context.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "tbg.uspInsertJobConfirmBaps";

                    command.Parameters.Add(command.CreateParameter("@vXml", vXML));
                    command.Parameters.Add(command.CreateParameter("@UserId", userID));

                    command.ExecuteNonQuery();

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public List<vmGetInvoicePostedList> PostingToAXTower(List<vmGetInvoicePostedList> data)
        {
            return pPostingToAXTower(data);
        }
        private List<vmGetInvoicePostedList> pPostingToAXTower(List<vmGetInvoicePostedList> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<vmGetInvoicePostedList>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGenerateToAXLocalDBTower";

                command.Parameters.Add(command.CreateParameter("@vType", "InputTableStaging"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }

        public List<vmGetInvoicePostedList> PostingToAXBuilding(List<vmGetInvoicePostedList> data)
        {
            return pPostingToAXBuilding(data);
        }
        private List<vmGetInvoicePostedList> pPostingToAXBuilding(List<vmGetInvoicePostedList> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<vmGetInvoicePostedList>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGenerateToAXLocalDBBuilding";

                command.Parameters.Add(command.CreateParameter("@vType", "InputTableStaging"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
    }
}