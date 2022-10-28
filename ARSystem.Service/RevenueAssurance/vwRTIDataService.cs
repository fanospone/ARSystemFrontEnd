using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.Repositories.Repositories.RevenueAssurance;
using ARSystem.Domain.Models.Models.RevenueAssurance;
using System.Data;

namespace ARSystem.Service.RevenueAssurance
{
    public class vwRTIDataService
    {
        public List<vwRTIData> GetData(string strToken, string strCompanyInv, string strOperatorInv, List<string> strBAPSNumber, List<string> strPONumber, List<string> strSONumber, string strYear, string strQuartal, string strBapsType, string strPowerType, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var RTIDataRepo = new vwRTIDataRepository(context);
            string SP = "";

            List<vwRTIData> listRTIData = new List<vwRTIData>();
            List<vwRTIData> filters = new List<vwRTIData>();

            //ARSystem.Domain.Models.vmUserCredential userCredential = CheckUserToken(strToken);
            //if (userCredential.ErrorType > 0)
            //{
            //    listRTIData.Add(new vwRTIData(userCredential.ErrorType, userCredential.ErrorMessage));
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
                listRTIData.Add(new vwRTIData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIService", "GetData", "")));
                return listRTIData;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetCount(string strToken, string strCompanyInv, string strOperatorInv, List<string> strBAPSNumber, List<string> strPONumber, List<string> strSONumber, string strYear, string strQuartal, string strBapsType, string strPowerType)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var RTIRepo = new vwRTIDataRepository(context);
            List<vwRTIData> listBAPS = new List<vwRTIData>();

            //ARSystem.Domain.Models.vmUserCredential userCredential = CheckUserToken(strToken);
            //if (userCredential.ErrorType > 0)
            //    return 0;

            try
            {

                string strWhereClause = "";

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
                Helper.logError(ex.Message.ToString(), "RTIService", "GetCount", "");
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int UploadRTI(string strToken, trxRARTI data, trxRAUploadDocument upload, List<trxReconcile> detail, string BapsType, string PoNumber)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));

            var trxRARTIRepo = new trxRARTIRepository(context);
            var trxRAUploadDocumentRepo = new trxRAUploadDocumentRepository(context);
            var trxReconcileRepo = new trxReconcileRepository(context);
            var RTIRepo = new vwRTIDataRepository(context);
            var trxRAPurchaseOrderRepo = new trxRAPurchaseOrderRepository(context);
            var trxRAPurchaseOrderDetailRepo = new trxRAPurchaseOrderDetailRepository(context);
            var logs = new trxRALogActivityRepository(context);

            var trxReconcile = new List<trxReconcile>();
            var trxRAPurchaseOrder = new trxRAPurchaseOrder();
            var trxRAPurchaseOrderDetail = new List<trxRAPurchaseOrderDetail>();
            var logact = new List<trxRALogActivity>();
            var RTI = new List<vwRTIData>();

            int Result = 0;

            //ARSystem.Domain.Models.vmUserCredential userCredential = CheckUserToken(strToken);
            //if (userCredential.ErrorType > 0)
            //{
            //    return 0;
            //}

            try
            {
                data.CreatedBy =   "";
                upload.CreatedBy = "";
                upload.CreatedDate = DateTime.Now;
                var hasil = trxRARTIRepo.Create(data);

                if (hasil.ID != 0)
                {
                    if (data.Type == "TOWER")
                    {
                        upload.TransactionID = Convert.ToInt32(hasil.ID);
                        Result = trxRAUploadDocumentRepo.Create(upload).ID;
                    }
                    else
                    {
                        Result += 1;
                    }

                    if (Result > 0)
                    {
                        foreach (var item in detail)
                        {
                            logact.Add(new trxRALogActivity
                            {
                                Label = "RTI Created",
                                LogDate = DateTime.Now,
                                LogState = true,
                                mstRAActivityID = 7,
                                Remarks = "RTI Created : " + data.PONumber,
                                TransactionID = item.ID,
                                UserID = ""
                            });
                        }

                        logs.CreateBulky(logact);
                    }
                }

                string IDstring;
                string where;
                List<int> IDs = detail.Select(l => Convert.ToInt32(l.ID)).Distinct().ToList();
                IDstring = string.Join(",", IDs);

                string POString;

                List<int> PoID;

                if (PoNumber == "")
                {
                    List<vwRTIData> ListRTI = new List<vwRTIData>();
                    where = "ID IN (" + IDstring + ")";
                    ListRTI = RTIRepo.GetList(where);
                    PoID = ListRTI.Select(l => Convert.ToInt32(l.POId)).Distinct().ToList();
                }
                else
                {
                    List<trxRAPurchaseOrder> ListPOHeader = new List<trxRAPurchaseOrder>();
                    where = "PONumber IN (" + PoNumber + ")";
                    ListPOHeader = trxRAPurchaseOrderRepo.GetList(where);
                    PoID = ListPOHeader.Select(l => Convert.ToInt32(l.ID)).Distinct().ToList();
                }

                POString = string.Join(",", PoID);

                UpdatePODetailBulky(strToken, IDstring, BapsType, POString);

                var hasildetail = trxReconcileRepo.UpdateBulkyRTI(detail, hasil.ID);

                if (PoID.Count != 0)
                {
                    foreach (int trxRAPurchaseOrderID in PoID)
                    {
                        where = "PurchaseOrderID = " + trxRAPurchaseOrderID + " AND ISNULL(mstRAActivityID," + (int)Constants.RAActivity.PO_DONE + ") <> " + (int)Constants.RAActivity.RTI + " ";
                        trxRAPurchaseOrderDetail = trxRAPurchaseOrderDetailRepo.GetList(where);

                        where = "trxRAPurchaseOrderID = " + trxRAPurchaseOrderID + " AND mstRAActivityID <> " + (int)Constants.RAActivity.RTI + " ";
                        trxReconcile = trxReconcileRepo.GetList(where);

                        if (trxReconcile.Count == 0 && trxRAPurchaseOrderDetail.Count == 0)
                        {
                            trxRAPurchaseOrder = trxRAPurchaseOrderRepo.GetByPK(Convert.ToInt64(trxRAPurchaseOrderID));
                            trxRAPurchaseOrder.mstRAActivityID = (int)Constants.RAActivity.RTI;
                            trxRAPurchaseOrder = trxRAPurchaseOrderRepo.Update(trxRAPurchaseOrder);
                        }
                    }
                }

                return Result;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxRARTIService", "UploadRTI", "");
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public string UpdatePODetailBulky(string strToken, string trxReconcileID, string BapsType, string POId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));

            try
            {
                var command = context.CreateCommand();
                command.CommandType = CommandType.Text;

                if (BapsType == "INF")
                {
                    command.CommandText = "UPDATE TBGARSystem.dbo.trxRAPurchaseOrderDetail SET mstRAActivityID = " + (int)Constants.RAActivity.RTI + " WHERE trxReconcileId IN (" + trxReconcileID + ") AND PurchaseOrderID IN (" + POId + ") AND Type = 'Inflation'";
                }
                else
                {
                    command.CommandText = "UPDATE TBGARSystem.dbo.trxRAPurchaseOrderDetail SET mstRAActivityID = " + (int)Constants.RAActivity.RTI + " WHERE trxReconcileId IN (" + trxReconcileID + ") AND PurchaseOrderID IN (" + POId + ") AND Type <> 'Inflation'";
                }
                command.ExecuteNonQuery();

                return "";
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "RTIService", "UpdatePODetailBulky", "");
                return ex.Message;
            }
            finally
            {
                context.Dispose();
            }
        }


    }
}
