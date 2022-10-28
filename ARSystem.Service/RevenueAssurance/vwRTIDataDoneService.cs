using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Service.RevenueAssurance
{
    public class vwRTIDataDoneService
    {
        public List<vwRTIDataDone> GetDataDone(string strToken, string strCompanyInv, string strOperatorInv, List<string> strBAPSNumber, List<string> strPONumber, List<string> strSONumber, string strYear, string strQuartal, string strBapsType, string strPowerType, string strOrderBy, int intRowSkip = 0, int intPageSize = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var RTIDataRepo = new vwRTIDataDoneRepository(context);
            string SP = "";

            List<vwRTIDataDone> listRTIData = new List<vwRTIDataDone>();
            List<vwRTIDataDone> filters = new List<vwRTIDataDone>();

            //var userCredential = CheckUserToken(strToken);
            //if (userCredential.ErrorType > 0)
            //{
            //    listRTIData.Add(new vwRTIDataDone(userCredential.ErrorType, userCredential.ErrorMessage));
            //    return listRTIData;
            //}

            try
            {
                string strWhereClause = "";

                if (!string.IsNullOrEmpty(strCompanyInv))
                    strWhereClause += "CompanyInvoice = '" + strCompanyInv + "' AND ";
                if (!string.IsNullOrEmpty(strOperatorInv))
                    strWhereClause += "CustomerID = '" + strOperatorInv + "'  AND ";
                if (!string.IsNullOrEmpty(strYear))
                    strWhereClause += "left(StartInvoiceDate,4) = '" + strYear + "' AND ";
                if (!string.IsNullOrEmpty(strQuartal))
                    strWhereClause += "Quartal = '" + strQuartal + "'  AND ";
                if (!string.IsNullOrEmpty(strBapsType))
                    strWhereClause += "BapsType = '" + strBapsType + "'  AND ";
                if (!string.IsNullOrEmpty(strPowerType))
                    strWhereClause += "PowerType = '" + strPowerType + "'  AND ";
                if (startDate.HasValue && endDate.HasValue)
                    strWhereClause += $"CAST(StartInvoiceDate AS Date) BETWEEN CAST('{startDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}' AS Date) AND CAST('{endDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}'AS Date)" + "  AND ";


                string SONumber = "";
                if (strSONumber[0] != null)
                {
                    if (strSONumber[0] != "0")
                    {
                        foreach (string Sonumb in strSONumber)
                        {
                            if (SONumber == "")
                                SONumber = "'" + Sonumb + "'";
                            else
                                SONumber += ",'" + Sonumb + "'";
                        }

                        if (SONumber != "")
                        {
                            strWhereClause += "SONumber IN (" + SONumber + ")  AND ";
                        }
                    }
                }

                string strBAPSNumberFull = "";
                if (strBAPSNumber[0] != null)
                {
                    if (strBAPSNumber[0] != "0")
                    {
                        foreach (string BAPS in strBAPSNumber)
                        {
                            if (strBAPSNumberFull == "")
                                strBAPSNumberFull = "'" + BAPS + "'";
                            else
                                strBAPSNumberFull += ",'" + BAPS + "'";
                        }

                        if (strBAPSNumberFull != "")
                        {
                            strWhereClause += "BAPSNumber IN (" + strBAPSNumberFull + ") AND ";
                        }
                    }
                }

                string strPONumberFull = "";
                if (strPONumber[0] != null)
                {
                    if (strPONumber[0] != "0")
                    {
                        foreach (string PO in strPONumber)
                        {
                            if (strPONumberFull == "")
                                strPONumberFull = "'" + PO + "'";
                            else
                                strPONumberFull += ",'" + PO + "'";
                        }

                        if (strPONumberFull != "")
                        {
                            strWhereClause += "PONumber IN (" + strPONumberFull + ") AND ";
                        }
                    }
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (intPageSize > 0)
                    listRTIData = RTIDataRepo.GetPaged(strWhereClause, "", intRowSkip, intPageSize);
                else
                    listRTIData = RTIDataRepo.GetList(strWhereClause, "");

                return listRTIData;
            }
            catch (Exception ex)
            {
                listRTIData.Add(new vwRTIDataDone((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIService", "GetDataDone", "")));
                return listRTIData;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetCountDone(string strToken, string strCompanyInv, string strOperatorInv, List<string> strBAPSNumber, List<string> strPONumber, List<string> strSONumber, string strYear, string strQuartal, string strBapsType, string strPowerType, DateTime? startDate = null, DateTime? endDate = null)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var RTIRepo = new vwRTIDataDoneRepository(context);
            List<vwRTIDataDone> listBAPS = new List<vwRTIDataDone>();

            //ARSystem.Domain.Models.vmUserCredential userCredential = CheckUserToken(strToken);
            //if (userCredential.ErrorType > 0)
            //    return 0;

            try
            {

                string strWhereClause = " ";

                if (!string.IsNullOrEmpty(strCompanyInv))
                    strWhereClause += "CompanyInvoice = '" + strCompanyInv + "' AND ";
                if (!string.IsNullOrEmpty(strOperatorInv))
                    strWhereClause += "CustomerID = '" + strOperatorInv + "' AND ";
                if (!string.IsNullOrEmpty(strYear))
                    strWhereClause += "left(StartInvoiceDate,4) = '" + strYear + "' AND ";
                if (!string.IsNullOrEmpty(strQuartal))
                    strWhereClause += "Quartal = '" + strQuartal + "'  AND ";
                if (!string.IsNullOrEmpty(strBapsType))
                    strWhereClause += "BapsType = '" + strBapsType + "'  AND ";
                if (!string.IsNullOrEmpty(strPowerType))
                    strWhereClause += "PowerType = '" + strPowerType + "'  AND ";
                if (startDate.HasValue && endDate.HasValue)
                    strWhereClause += $"CAST(StartInvoiceDate AS Date) BETWEEN CAST('{startDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}' AS Date) AND CAST('{endDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}'AS Date)" + "  AND ";


                string SONumber = "";
                if (strSONumber[0] != null)
                {
                    if (strSONumber[0] != "0")
                    {
                        foreach (string Sonumb in strSONumber)
                        {
                            if (SONumber == "")
                                SONumber = "'" + Sonumb + "'";
                            else
                                SONumber += ",'" + Sonumb + "'";
                        }

                        if (SONumber != "")
                        {
                            strWhereClause += "SONumber IN (" + SONumber + ") AND ";
                        }
                    }
                }

                string strBAPSNumberFull = "";
                if (strBAPSNumber[0] != null)
                {
                    foreach (string PO in strBAPSNumber)
                    {
                        if (strBAPSNumberFull == "")
                            strBAPSNumberFull = "'" + PO + "'";
                        else
                            strBAPSNumberFull += ",'" + PO + "'";
                    }

                    if (strBAPSNumberFull != "")
                    {
                        strWhereClause += "BAPSNumber IN (" + strBAPSNumberFull + ") AND ";
                    }
                }

                string strPONumberFull = "";
                if (strPONumber[0] != null)
                {
                    foreach (string PO in strPONumber)
                    {
                        if (strPONumberFull == "")
                            strPONumberFull = "'" + PO + "'";
                        else
                            strPONumberFull += ",'" + PO + "'";
                    }

                    if (strPONumberFull != "")
                    {
                        strWhereClause += "PONumber IN (" + strPONumberFull + ") AND ";
                    }
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return RTIRepo.GetCount(strWhereClause);

            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "RTIService", "GetCountDone", "");
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

    }
}
