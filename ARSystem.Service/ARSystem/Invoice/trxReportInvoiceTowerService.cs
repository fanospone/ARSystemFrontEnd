using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Service
{
    public class trxReportInvoiceTowerService
    {
        public int GetTrxReportInvoiceTowerCount(string strToken, string strStartPeriod, string strEndPeriod, int intYearPosting = 0, int intMonthPosting = 0
            , int intWeekPosting = 0, string invNo = "", string strUserCompanyCode = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxDataReportInvoiceRepo = new vwReportInvoiceTowerByInvoiceRepository(context);
            List<vwReportInvoiceTowerByInvoice> listDataReportInvoice = new List<vwReportInvoiceTowerByInvoice>();

            var Customrepo = new ReportInvoiceTowerRepository(context);
            List<vmReportInvoiceMaxMinLogDate> listMaxMinLogDate = new List<vmReportInvoiceMaxMinLogDate>();
            if (intYearPosting != 0 && intMonthPosting != 0 && intWeekPosting != 0)
                listMaxMinLogDate = Customrepo.GetReportInvoiceMaxMinLogDate(intYearPosting, intMonthPosting, intWeekPosting);

            // Modified by Aisyar Arfah 8 October 2020 PKP Change : UserCredentialService hold up to prod 
            //vmUserCredential userCredential = UserCredentialService.CheckUserToken(strToken);

            //if (userCredential.ErrorType > 0)
            //    return 0;

            try
            {
                string strWhereClause = "";

                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "InvPrintDate >= '" + DateTime.Parse(strStartPeriod).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "InvPrintDate <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strUserCompanyCode))
                {
                    strWhereClause += "InvCompanyId = '" + strUserCompanyCode + "' AND CompanyIdAx='" + strUserCompanyCode + "' AND ";
                }
                //if (listMaxMinLogDate.Count() > 0)
                //{
                //    strWhereClause += "ApprovalDate >= '" + listMaxMinLogDate[0].MinLogDate.ToString("yyyy-MM-dd") + "' AND ";
                //    strWhereClause += "ApprovalDate <= '" + listMaxMinLogDate[0].MaxLogDate.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                //}

                if (!string.IsNullOrEmpty(invNo))
                    strWhereClause += "InvNo LIKE '%" + invNo + "%' AND ";
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return TrxDataReportInvoiceRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxReportInvoiceTowerService", "GetTrxReportInvoiceTowerCount", "PKP Changes");
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwReportInvoiceTowerByInvoice> GetTrxReportInvoiceTowerToList(string strToken, string strStartPeriod, string strEndPeriod, string strOrderBy
            , int intYearPosting = 0, int intMonthPosting = 0, int intWeekPosting = 0, string invNo = "", int intRowSkip = 0, int intPageSize = 0, string strUserCompanyCode = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxReportInvoiceRepo = new vwReportInvoiceTowerByInvoiceRepository(context);
            var Customrepo = new ReportInvoiceTowerRepository(context);

            List<vwReportInvoiceTowerByInvoice> listDataReportInvoice = new List<vwReportInvoiceTowerByInvoice>();
            List<vmReportInvoiceMaxMinLogDate> listMaxMinLogDate = new List<vmReportInvoiceMaxMinLogDate>();

            if (intYearPosting != 0 && intMonthPosting != 0 && intWeekPosting != 0)
                listMaxMinLogDate = Customrepo.GetReportInvoiceMaxMinLogDate(intYearPosting, intMonthPosting, intWeekPosting);

            // Modified by Aisyar Arfah 8 October 2020 PKP Change : UserCredentialService hold up to prod 
            //vmUserCredential userCredential = UserCredentialService.CheckUserToken(strToken);
            //if (userCredential.ErrorType > 0)
            //{
            //    listDataReportInvoice.Add(new vwReportInvoiceTowerByInvoice(userCredential.ErrorType, userCredential.ErrorMessage));
            //    return listDataReportInvoice;
            //}

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "InvPrintDate >= '" + DateTime.Parse(strStartPeriod).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "InvPrintDate <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strUserCompanyCode))
                {
                    strWhereClause += "InvCompanyId = '" + strUserCompanyCode + "' AND CompanyIdAx='" + strUserCompanyCode + "' AND ";
                }
                //if (listMaxMinLogDate.Count() > 0)
                //{
                //    strWhereClause += "ApprovalDate >= '" + listMaxMinLogDate[0].MinLogDate.ToString("yyyy-MM-dd") + "' AND ";
                //    strWhereClause += "ApprovalDate <= '" + listMaxMinLogDate[0].MaxLogDate.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                //}

                if (!string.IsNullOrEmpty(invNo))
                    strWhereClause += "InvNo LIKE '%" + invNo + "%' AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (intPageSize > 0)
                    listDataReportInvoice = TrxReportInvoiceRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    listDataReportInvoice = TrxReportInvoiceRepo.GetList(strWhereClause, strOrderBy);

                return listDataReportInvoice;
            }
            catch (Exception ex)
            {
                listDataReportInvoice.Add(new vwReportInvoiceTowerByInvoice((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxReportInvoiceTowerService", "GetTrxReportInvoiceTowerToList", "PKP Change")));
                return listDataReportInvoice;
            }
            finally
            {
                context.Dispose();
            }
        }


        public int GetTrxReportInvoiceTowerBySONumberCount(string strToken, string strStartPeriod, string strEndPeriod, string invNo, string strUserCompanyCode="")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxDataReportInvoiceRepo = new vwReportInvoiceTowerBySoNumberRepository(context);
            List<vwReportInvoiceTowerBySoNumber> listDataReportInvoice = new List<vwReportInvoiceTowerBySoNumber>();

            // Modified by Aisyar Arfah 8 October 2020 PKP Change : UserCredentialService hold up to prod 
            //vmUserCredential userCredential = UserCredentialService.CheckUserToken(strToken);
            //if (userCredential.ErrorType > 0)
            //    return 0;

            try
            {
                string strWhereClause = "";

                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "InvPrintDate >= '" + DateTime.Parse(strStartPeriod).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "InvPrintDate <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strUserCompanyCode))
                {
                    strWhereClause += "InvCompanyId = '" + strUserCompanyCode + "' AND CompanyIdAx='" + strUserCompanyCode + "' AND ";
                }
                if (!string.IsNullOrEmpty(invNo))
                    strWhereClause += "InvNo LIKE '%" + invNo + "%' AND ";
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return TrxDataReportInvoiceRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxReportInvoiceTowerService", "GetTrxReportInvoiceTowerBySONumberCount", "PKP Change");
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwReportInvoiceTowerBySoNumber> GetTrxReportInvoiceTowerBySONumberToList(string strToken, string strStartPeriod, string strEndPeriod, string invNo, string strOrderBy
            , int intRowSkip = 0, int intPageSize = 0, string strUserCompanyCode = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxReportInvoiceRepo = new vwReportInvoiceTowerBySoNumberRepository(context);
            List<vwReportInvoiceTowerBySoNumber> listDataReportInvoice = new List<vwReportInvoiceTowerBySoNumber>();

            // Modified by Aisyar Arfah 8 October 2020 PKP Change : UserCredentialService hold up to prod 
            //vmUserCredential userCredential = UserCredentialService.CheckUserToken(strToken);
            //if (userCredential.ErrorType > 0)
            //{
            //    listDataReportInvoice.Add(new vwReportInvoiceTowerBySoNumber(userCredential.ErrorType, userCredential.ErrorMessage));
            //    return listDataReportInvoice;
            //}

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "InvPrintDate >= '" + DateTime.Parse(strStartPeriod).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "InvPrintDate <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strUserCompanyCode))
                {
                    strWhereClause += "InvCompanyId = '" + strUserCompanyCode + "' AND CompanyIdAx='"+ strUserCompanyCode +"' AND ";
                }
                if (!string.IsNullOrEmpty(invNo))
                    strWhereClause += "InvNo LIKE '%" + invNo + "%' AND ";
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (intPageSize > 0)
                    listDataReportInvoice = TrxReportInvoiceRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    listDataReportInvoice = TrxReportInvoiceRepo.GetList(strWhereClause, strOrderBy);

                return listDataReportInvoice;
            }
            catch (Exception ex)
            {
                listDataReportInvoice.Add(new vwReportInvoiceTowerBySoNumber((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxReportInvoiceTowerService", "GetTrxReportInvoiceTowerBySONumberToList", "PKP Change")));
                return listDataReportInvoice;
            }
            finally
            {
                context.Dispose();
            }
        }





    }
}
