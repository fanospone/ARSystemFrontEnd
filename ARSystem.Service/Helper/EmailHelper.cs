using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;

namespace ARSystem.Service
{
    public static class EmailHelper
    {
       

        public static void SendEmail(string strTo, string strCC, string strBCC, string strSubject, string strBody, string strReplyTo, string strUserID)
        {
            var context = new DbContext(Helper.GetConnection("TBIGSys"));

            try
            {
                //Sending email
                using (var command = context.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "msdb.dbo.sp_send_dbmail";

                    command.Parameters.Add(command.CreateParameter("@profile_name", ConfigurationManager.AppSettings["EmailProfile"].ToString()));
                    command.Parameters.Add(command.CreateParameter("@recipients", strTo));
                    command.Parameters.Add(command.CreateParameter("@copy_recipients", strCC));
                    command.Parameters.Add(command.CreateParameter("@blind_copy_recipients", strBCC));
                    command.Parameters.Add(command.CreateParameter("@subject", strSubject));
                    command.Parameters.Add(command.CreateParameter("@body", strBody));
                    command.Parameters.Add(command.CreateParameter("@body_format", "HTML"));
                    command.Parameters.Add(command.CreateParameter("@reply_to", strReplyTo));

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "EmailHelper", "SendEmail", strUserID);
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}