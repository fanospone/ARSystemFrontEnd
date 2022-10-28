using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System.Data;

namespace ARSystem.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TrxBAPSData" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TrxBAPSData.svc or TrxBAPSData.svc.cs at the Solution Explorer and start debugging.
    public partial class trxBAPSDataService
    {
        /// <summary>
        /// Show Semua data dari trxBAPSData dengan filter by Sonum,PONumber,BAPSNumber
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="strCompanyName"></param>
        /// <param name="Operator"></param>
        /// <param name="strStatusBAPS"></param>
        /// <param name="strPeriodInvoice"></param>
        /// <param name="strInvoiceType"></param>
        /// <param name="strCurrency"></param>
        /// <param name="strPONumber"></param>
        /// <param name="strBAPSNumber"></param>
        /// <param name="strSONumber"></param>
        /// <param name="strOrderBy"></param>
        /// <returns></returns>
        public List<vwBAPSData> GetTrxBAPSDataToList(string UserID, string strCompanyId, string strOperator, string strStatusBAPS, string strPeriodInvoice, string strInvoiceType, string strCurrency, string strPONumber, string strBAPSNumber, string strSONumber, string strBapsType, string strSiteIdOld, string strStartPeriod, string strEndPeriod, int isReceive, string strCreatedBy,string strStatusDismantle, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxBAPSDataRepo = new vwBAPSDataRepository(context);
            List<vwBAPSData> listBAPSData = new List<vwBAPSData>();
            var mstInvoiceStatusRepo = new mstInvoiceStatusRepository(context);
            try
            {
                string strWhereClause = "";
                if (isReceive == 1)
                    strWhereClause = "mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.NotProcessed + " AND "; //TAB BAPS RECEIVE 
                else if (isReceive == 18)
                    strWhereClause = "mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.StateBAPSReceive + " AND "; //TAB BAPS CONFIRM 
                else if (isReceive == 0)
                    strWhereClause = "(mstInvoiceStatusIdPOConfirm = " + mstInvoiceStatusRepo.GetList("Description = 'PO CONFIRM'").FirstOrDefault().mstInvoiceStatusId + " OR " + "mstInvoiceStatusIdPOConfirm = " + mstInvoiceStatusRepo.GetList("Description = 'REJECT PO CONFIRM'").FirstOrDefault().mstInvoiceStatusId + ") AND "; //TAB PO Confirm
                if (!string.IsNullOrWhiteSpace(strCompanyId))
                {
                    strWhereClause += "CompanyId = '" + strCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += "Operator = '" + strOperator + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStatusBAPS))
                {
                    strWhereClause += "Status = '" + strStatusBAPS + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPeriodInvoice))
                {
                    strWhereClause += "PeriodInvoice = '" + strPeriodInvoice + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strInvoiceType))
                {
                    strWhereClause += "InvoiceTypeId = '" + strInvoiceType + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strCurrency))
                {
                    strWhereClause += "Currency = '" + strCurrency + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPONumber))
                {
                    strWhereClause += "PoNumber = '" + strPONumber + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBAPSNumber))
                {
                    strWhereClause += "BapsNo = '" + strBAPSNumber + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSONumber))
                {
                    strWhereClause += "SONumber = '" + strSONumber + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBapsType))
                {
                    strWhereClause += "BapsType = '" + strBapsType + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteIdOld))
                {
                    strWhereClause += "SiteIdOld = '" + strSiteIdOld + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "CONVERT(VARCHAR, StartDateInvoice, 106) LIKE '%" + strStartPeriod.Replace('-', ' ') + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "CONVERT(VARCHAR, EndDateInvoice, 106) LIKE '%" + strEndPeriod.Replace('-', ' ') + "%' AND ";
                    //strWhereClause += "EndDateInvoice <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strCreatedBy))
                {
                    strWhereClause += "CreatedBy LIKE '%" + strCreatedBy + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStatusDismantle))
                {
                    if (strStatusDismantle == "NOTDISMANTLE")
                    {
                        strWhereClause += "StatusTrx NOT IN ('DISMANTLE') AND ";
                    }
                    else
                    {
                        strWhereClause += "StatusTrx = 'DISMANTLE' AND ";
                    }
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                //set order by
                if (string.IsNullOrWhiteSpace(strOrderBy))
                {
                    if (isReceive == 1)
                        strOrderBy = "CreatedDate DESC";
                    else
                        strOrderBy = "UpdatedDate DESC";
                }

                if (intPageSize > 0)
                    listBAPSData = TrxBAPSDataRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    listBAPSData = TrxBAPSDataRepo.GetList(strWhereClause, strOrderBy);

                return listBAPSData;
            }
            catch (Exception ex)
            {
                listBAPSData.Add(new vwBAPSData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataToList", UserID)));
                return listBAPSData;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetTrxBapsDataCount(string UserID, string strCompanyId, string strOperator, string strStatusBAPS, string strPeriodInvoice, string strInvoiceType, string strCurrency, string strPONumber, string strBAPSNumber, string strSONumber, string strBapsType, string strSiteIdOld, string strStartPeriod, string strEndPeriod, int isReceive, string strCreatedBy, string strStatusDismantle)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxBAPSDataRepo = new vwBAPSDataRepository(context);
            List<vwBAPSData> listBAPSData = new List<vwBAPSData>();
            var mstInvoiceStatusRepo = new mstInvoiceStatusRepository(context);
            try
            {
                string strWhereClause = "";
                if (isReceive == 1)
                    strWhereClause = "mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.NotProcessed + " AND "; //TAB BAPS RECEIVE 
                else if (isReceive == 18)
                    strWhereClause = "mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.StateBAPSReceive + " AND "; //TAB BAPS CONFIRM
                else if (isReceive == 0)
                    strWhereClause = "(mstInvoiceStatusIdPOConfirm = " + mstInvoiceStatusRepo.GetList("Description = 'PO CONFIRM'").FirstOrDefault().mstInvoiceStatusId + " OR " + "mstInvoiceStatusIdPOConfirm = " + mstInvoiceStatusRepo.GetList("Description = 'REJECT PO CONFIRM'").FirstOrDefault().mstInvoiceStatusId + ") AND "; //TAB PO Confirm
                if (!string.IsNullOrWhiteSpace(strCompanyId))
                {
                    strWhereClause += "CompanyId = '" + strCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += "Operator = '" + strOperator + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStatusBAPS))
                {
                    strWhereClause += "Status = '" + strStatusBAPS + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPeriodInvoice))
                {
                    strWhereClause += "PeriodInvoice = '" + strPeriodInvoice + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strInvoiceType))
                {
                    strWhereClause += "InvoiceTypeId = '" + strInvoiceType + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strCurrency))
                {
                    strWhereClause += "Currency = '" + strCurrency + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPONumber))
                {
                    strWhereClause += "PoNumber = '" + strPONumber + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBAPSNumber))
                {
                    strWhereClause += "BapsNo = '" + strBAPSNumber + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSONumber))
                {
                    strWhereClause += "SONumber = '" + strSONumber + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBapsType))
                {
                    strWhereClause += "BapsType = '" + strBapsType + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteIdOld))
                {
                    strWhereClause += "SiteIdOld = '" + strSiteIdOld + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "CONVERT(VARCHAR, StartDateInvoice, 106) LIKE '%" + strStartPeriod.Replace('-', ' ') + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "CONVERT(VARCHAR, EndDateInvoice, 106) LIKE '%" + strEndPeriod.Replace('-', ' ') + "%' AND ";
                    //strWhereClause += "EndDateInvoice <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strCreatedBy))
                {
                    strWhereClause += "CreatedBy LIKE '%" + strCreatedBy + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStatusDismantle))
                {
                    if (strStatusDismantle == "NOTDISMANTLE")
                    {
                        strWhereClause += "StatusTrx NOT IN ('DISMANTLE') AND ";
                    }
                    else
                    {
                        strWhereClause += "StatusTrx = 'DISMANTLE' AND ";
                    }
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return TrxBAPSDataRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }


        /// <summary>
        /// Ketika user melakukan action Confirm, Data dari TrxBAPSData disimpan di ARHeader,ARDetail dan ARActivityLog
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ListTrxBapsData"></param>
        /// <returns></returns>

        public trxBapsData ConfirmBAPS(string UserID, List<int> id, string isPOConfirm)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();

            var TrxArHeaderRepo = new trxArHeaderRepository(context);
            var TrxArDetailRepo = new trxArDetailRepository(context);
            var TrxArActivityLogRepo = new trxArActivityLogRepository(context);
            var mstBapsPowerTypeRepo = new mstBapsPowerTypeRepository(context);
            var TrxBapsDataRepo = new trxBapsDataRepository(context);
            var vwBapsDataRepo = new vwBAPSDataRepository(context);
            var ARHeader = new trxArHeader();
            var ARDetail = new trxArDetail();
            var ARActivityLog = new trxArActivityLog();
            var vwBapsData = new vwBAPSData();

            try
            {
                List<trxBapsData> ListTrxBapsData = new List<trxBapsData>();
                ListTrxBapsData = GetTrxBapsListDataToList(UserID, id);
                //Validation
                bool checkSameValue = ListTrxBapsData.Select(i => new { Operator = i.Operator.Trim() }).Distinct().Count() > 1;
                if (checkSameValue)
                    return new trxBapsData((int)Helper.ErrorType.Validation, "Please Select Same Operator");

                //Transaction
                #region 'UNTUK BatchNo'
                var ARBapsPowerType = new List<mstBapsPowerType>();
                ARBapsPowerType = mstBapsPowerTypeRepo.GetList("IsActive = 1");

                string OPR = (ListTrxBapsData[0].Operator.Length >= 4) ? ListTrxBapsData[0].Operator.Substring(0, 4) : ListTrxBapsData[0].Operator;
                string Period = ListTrxBapsData[0].InvoiceTypeDesc[0].ToString();
                string CountSoNumb = ListTrxBapsData.Count.ToString("00000");
                string RowNumber = (TrxArHeaderRepo.GetList().Count() + 1).ToString("00000");
                #endregion

                #region 'INSERT ARHeader'
                decimal SumAmount = 0;
                foreach (trxBapsData item in ListTrxBapsData)
                {
                    SumAmount += item.InvoiceAmount.Value;
                }

                ARHeader.BatchNo = OPR + Period + CountSoNumb + RowNumber;
                ARHeader.SONumberAmount = ListTrxBapsData.Count;
                ARHeader.TotalAmount = SumAmount;
                ARHeader.Activity = "CONFIRM BAPS";
                ARHeader.Status = "WTG";
                ARHeader.CreatedBy = UserID;
                ARHeader.CreatedDate = Helper.GetDateTimeNow();

                ARHeader = TrxArHeaderRepo.Create(ARHeader);
                #endregion

                #region 'INSERT ARDetail dan Update mstInvoiceStatusId TrxBapsData'
                List<trxArDetail> ListARDetail = new List<trxArDetail>();
                var listLogAr = new List<logArActivity>();
                var LogArRepo = new logArActivityRepository(context);

                //Added by ASE enhance flow Confirm (Freeze, Unfreeze, POConfirm)

                List<trxArDetail> ListARDetailPOConfirm = new List<trxArDetail>();
                var mstInvoiceStatusRepo = new mstInvoiceStatusRepository(context);
                var mstBAPSConfirmValidateRepo = new mstBAPSConfirmValidateRepository(context);
                var mstPicaTypeRepo = new mstPICATypeRepository(context);
                List<mstBAPSConfirmValidate> ListmstBAPSConfirmValidate = mstBAPSConfirmValidateRepo.GetList("IsActive = 1 AND (TypeValidation = 'FREEZE' OR TypeValidation = 'PO_CONFIRM')");

                string validateType = "";
                //End ASE
                foreach (trxBapsData TrxBAPSData in ListTrxBapsData)
                {

                    ARDetail = new trxArDetail();
                    //Added by ASE validate Freeze/PO Confirm
                    if (ListmstBAPSConfirmValidate.Count(n => n.OperatorID == TrxBAPSData.Operator && n.InvoiceType == TrxBAPSData.PeriodInvoice
                                         && n.TypeValidation == "FREEZE") > 0)
                    {
                        if (TrxBAPSData.BapsType.ToUpper().Trim() == "TOWER")
                        {
                            if (TrxBAPSData.StartDateInvoice != null)
                            {
                                double datedif = (Convert.ToDateTime(TrxBAPSData.StartDateInvoice) - DateTime.Now).TotalDays;
                                //Check apakah datedif lebih besar dari maxValidationDays 
                                var a = ListmstBAPSConfirmValidate.Where(x => x.OperatorID.ToString() == TrxBAPSData.Operator).FirstOrDefault().MinValidateDay;
                                if (datedif > ListmstBAPSConfirmValidate.Where(x => x.OperatorID.ToString() == TrxBAPSData.Operator).FirstOrDefault().MinValidateDay)
                                {
                                    if (ListmstBAPSConfirmValidate.Where(x => x.OperatorID.ToString() == TrxBAPSData.Operator).FirstOrDefault().StipSiroId == "0" && TrxBAPSData.StipSiroId == 0)
                                        validateType = "FREEZE";
                                    else if (ListmstBAPSConfirmValidate.Where(x => x.OperatorID.ToString() == TrxBAPSData.Operator).FirstOrDefault().StipSiroId == "<>0" && TrxBAPSData.StipSiroId > 0)
                                        validateType = "FREEZE";
                                }
                            }
                        }


                    }
                    if (ListmstBAPSConfirmValidate.Count(n => n.OperatorID == TrxBAPSData.Operator && n.InvoiceType == TrxBAPSData.PeriodInvoice && n.TypeValidation == "PO_CONFIRM" && n.BapsType.ToUpper().Trim() == TrxBAPSData.BapsType.ToUpper().Trim()) > 0)
                    {
                        //Checking Baps Confirm dan PO confirm sudah ter confirm dua2 nya
                        vwBapsData = vwBapsDataRepo.GetList("trxBapsDataId = " + TrxBAPSData.trxBapsDataId).FirstOrDefault();
                        if ((isPOConfirm != "" && vwBapsData.BapsConfirmDate == null) || isPOConfirm == "" && vwBapsData.POConfirmDate == null)
                            validateType = "PO CONFIRM";

                    }
                    // end by ASE
                    if (validateType == "")
                        ARDetail.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.NotProcessed;
                    else
                        ARDetail.mstInvoiceStatusId = mstInvoiceStatusRepo.GetList("Description = '" + validateType + "'").FirstOrDefault().mstInvoiceStatusId;
                    ARDetail.TrxArHeaderId = ARHeader.trxArHeaderId;
                    ARDetail.SONumber = TrxBAPSData.SONumber;
                    ARDetail.SiteIdOld = TrxBAPSData.SiteIdOld;
                    ARDetail.SiteIdOpr = TrxBAPSData.SiteIdOpr;
                    ARDetail.SiteName = TrxBAPSData.SiteName;
                    ARDetail.SpkDate = TrxBAPSData.SpkDate;
                    ARDetail.RfiDate = TrxBAPSData.RfiDate;
                    ARDetail.BapsPeriod = TrxBAPSData.BapsPeriod;
                    ARDetail.PoNumber = TrxBAPSData.PoNumber;
                    ARDetail.CompanyInvoice = TrxBAPSData.CompanyInvoice;
                    ARDetail.CompanyId = TrxBAPSData.CompanyId;
                    ARDetail.Operator = TrxBAPSData.Operator;
                    ARDetail.OperatorAsset = TrxBAPSData.OperatorAsset;
                    ARDetail.Type = TrxBAPSData.Type;
                    ARDetail.BapsNo = TrxBAPSData.BapsNo;
                    ARDetail.PeriodInvoice = TrxBAPSData.PeriodInvoice;
                    ARDetail.InvoiceTypeId = TrxBAPSData.InvoiceTypeId;
                    ARDetail.InvoiceTypeDesc = TrxBAPSData.InvoiceTypeDesc;
                    ARDetail.BapsType = TrxBAPSData.BapsType;
                    ARDetail.StipSiro = TrxBAPSData.StipSiro;
                    ARDetail.StipSiroId = TrxBAPSData.StipSiroId.Value;
                    ARDetail.BapsDone = TrxBAPSData.BapsDone;
                    ARDetail.PowerType = TrxBAPSData.PowerType;
                    ARDetail.PowerTypeCode = TrxBAPSData.PowerTypeCode;
                    ARDetail.Currency = TrxBAPSData.Currency;
                    ARDetail.StartDate = TrxBAPSData.StartDate;
                    ARDetail.EndDate = TrxBAPSData.EndDate;
                    ARDetail.SPK = TrxBAPSData.SPK;
                    ARDetail.Kontrak = TrxBAPSData.Kontrak;
                    ARDetail.Rekon = TrxBAPSData.Rekon;
                    ARDetail.BAPS = TrxBAPSData.BAPS;
                    ARDetail.BAK = TrxBAPSData.BAK;
                    ARDetail.SSR = TrxBAPSData.SSR;
                    ARDetail.PO = TrxBAPSData.PO;
                    ARDetail.BAUF = TrxBAPSData.BAUF;
                    ARDetail.Regional = TrxBAPSData.Regional;
                    ARDetail.InvoiceAmount = TrxBAPSData.InvoiceAmount.Value;
                    ARDetail.AmountRental = TrxBAPSData.AmountRental.Value;
                    ARDetail.AmountService = TrxBAPSData.AmountService.Value;
                    ARDetail.AmountInvoicePeriod = TrxBAPSData.AmountInvoicePeriod.Value;
                    ARDetail.AmountPenaltyPeriod = TrxBAPSData.AmountPenaltyPeriod.Value;
                    ARDetail.AmountOverdaya = TrxBAPSData.AmountOverdaya.Value;
                    ARDetail.AmountOverblast = TrxBAPSData.AmountOverblast.Value;
                    ARDetail.StartDateInvoice = TrxBAPSData.StartDateInvoice;
                    ARDetail.EndDateInvoice = TrxBAPSData.EndDateInvoice;
                    ARDetail.IsLossPPN = TrxBAPSData.IsLossPPN;
                    ARDetail.IsPartial = TrxBAPSData.IsPartial;
                    ARDetail.AmountPPN = TrxBAPSData.AmountPPN;
                    ARDetail.AmountLossPPN = TrxBAPSData.AmountLossPPN;
                    ARDetail.IsPPHFinal = TrxBAPSData.IsPPHFinal;
                    ARDetail.SiteNameOpr = TrxBAPSData.SiteNameOpr;
                    ARDetail.ContractNumber = TrxBAPSData.ContractNumber;
                    ARDetail.ReslipNumber = TrxBAPSData.ReslipNumber;
                    ARDetail.CreatedBy = UserID;
                    ARDetail.CreatedDate = Helper.GetDateTimeNow();
                    if (isPOConfirm != "")
                        ListARDetailPOConfirm.Add(ARDetail);

                    //Ketika PO Confirm tidak insert ke trxARHeader dan trxArDetail, hnya update status di trxBapsData (data yg sdh terlanjur insert ke trxARHeader di hapus by PK)
                    if (validateType == "PO CONFIRM")
                        TrxArHeaderRepo.DeleteByPK(ARHeader.trxArHeaderId);
                    else
                        ARDetail = TrxArDetailRepo.Create(ARDetail);


                    trxBapsData BapsData = new trxBapsData();
                    BapsData = TrxBapsDataRepo.GetByPK(TrxBAPSData.trxBapsDataId);
                    if (isPOConfirm != "")
                        BapsData.mstInvoiceStatusIdPOConfirm = (int)StatusHelper.InvoiceStatus.StateBAPSConfirm;//CONFIRMED NOT SHOWN IN BAPS CONFIRM MENU
                    else
                        BapsData.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateBAPSConfirm;//CONFIRMED NOT SHOWN IN BAPS CONFIRM MENU

                    BapsData.UpdatedBy = UserID;
                    BapsData.UpdatedDate = Helper.GetDateTimeNow();
                    TrxBapsDataRepo.Update(BapsData);

                    //INSERT INTO LOGARACTIVITY
                    logArActivity logAr = new logArActivity();
                    if (isPOConfirm != "")
                        logAr.mstInvoiceStatusId = mstInvoiceStatusRepo.GetList("Description = 'PO CONFIRM'").FirstOrDefault().mstInvoiceStatusId;
                    else
                        logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.BAPSConfirm;
                    //else
                    //    logAr.mstInvoiceStatusId = mstInvoiceStatusRepo.GetList("Description = '" + validateType + "'").FirstOrDefault().mstInvoiceStatusId;

                    logAr.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                    if (validateType != "PO CONFIRM")
                        logAr.trxArDetailId = ARDetail.trxArDetailId;
                    logAr.trxBapsDataId = TrxBAPSData.trxBapsDataId;
                    logAr.CreatedBy = UserID;
                    logAr.CreatedDate = Helper.GetDateTimeNow();
                    listLogAr.Add(logAr);
                }

                LogArRepo.CreateBulky(listLogAr);
                #endregion



                //Add by ASE
                #region Send email Waiting PO Confirm Or PO Confirmed
                List<mstPICAType> ListmstPICAType = mstPicaTypeRepo.GetList("IsActive = 1");
                string emailDevelopment = "information@internal.m-tbig.com";
                string emailTo = "";
                string emailCc = "";
                string emailBcc = "";
                string Subject = "";
                string Body = "";
                string emailReplyTo = "";
                var tblHeader = "";
                var tblRow = "";
                if (isPOConfirm != "")
                {
                    foreach (trxArDetail BAPSData in ListARDetailPOConfirm)
                    {
                        if (BAPSData.StartDateInvoice == null)
                            tblRow += "<tr><td>" + BAPSData.SONumber + "</td><td>" + BAPSData.SiteIdOld + "</td><td>" + BAPSData.SiteName + "</td><td>" + BAPSData.PoNumber + "</td><td>" + BAPSData.Operator + "</td><td>" + "" + "</td><td>" + "" + "</td><td>" + BAPSData.InvoiceAmount.ToString() + "</td><td></td></tr>";

                        else
                            tblRow += "<tr><td>" + BAPSData.SONumber + "</td><td>" + BAPSData.SiteIdOld + "</td><td>" + BAPSData.SiteName + "</td><td>" + BAPSData.PoNumber + "</td><td>" + BAPSData.Operator + "</td><td>" + Convert.ToDateTime(BAPSData.StartDateInvoice).ToString("dd MMMM yyyy") + "</td><td>" + Convert.ToDateTime(BAPSData.EndDateInvoice).ToString("dd MMMM yyyy") + "</td><td>" + BAPSData.InvoiceAmount.ToString() + "</td><td></td></tr>";

                    }
                    Subject = "[ARSystem] - Notification PO Done";
                    emailTo = ListmstPICAType.Where(x => x.Description.ToString() == "External - Marketing").FirstOrDefault().Recipient;
                    emailCc += ListmstPICAType.Where(x => x.Description.ToString() == "External - RA New").FirstOrDefault().Recipient;
                    emailCc += ";" + ListmstPICAType.Where(x => x.Description.ToString() == "Internal - AR Data").FirstOrDefault().Recipient;

                    tblHeader = "Dear all,<br><br>Berikut Data BAPS yang berstatus PO Confirmed.<br><table border=0 cellpadding=4 cellspacing=1><tr bgcolor=#CCCCCC><th align=center><b>SO Number</b></th><th align=center><b>Site ID</b></th><th align=center><b>Site Name</b></th><th align=center><b>PO Number</b></th><th align=center><b>Operator</b></th><th align=center><b>Start Date Receivable</b></th><th align=center><b>End Date Receivable</b></th><th align=center><b>Amount Invoice</b></th></tr>";
                    Body = tblHeader + tblRow + "</table><br><br><br><b>ARSystem Notification</b><br><br><b><i>*This is an ARSystem automatic e-mail, please do not reply</i></b><br>";

                    EmailHelper.SendEmail(emailTo, emailCc, emailBcc, Subject, Body, emailReplyTo, UserID);

                    //Only for testing development 
                    //Body += "<br> Recipient : " + emailTo + "<br>  Cc : " + emailCc;
                    //emailTo = emailDevelopment;
                    //emailCc = emailDevelopment;

                }

                #endregion
                //End Add ASE
                uow.SaveChanges();
                #region 'UPDATE DATA UNTUK ExistingTBGSys'
                //string tokenWebService = "bbac92c7-ff22-45f6-8298-dfcfb1df68ad";
                //string xml = ARSystem.Domain.Repositories.Helper.XmlSerializer<List<trxBapsData>>(ListTrxBapsData);
                //string result = "";
                //using (var client = new TBGSysService.Service())
                //{
                //    result = client.ConfirmBAPS(tokenWebService, xml, UserID,0,false);

                //}
                //if (!string.IsNullOrEmpty(result))
                //{
                //    return new trxBapsData((int)Helper.ErrorType.Error, result);
                //}
                #endregion
                return new trxBapsData();

            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxBapsData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBAPSDataService", "ConfirmTrxBAPSData", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }
        /// <summary>
        /// Ketika User Melakukan Action Reject, Data dari BAPS yang ada di TBG_Sys di Update
        /// Data yang di Reject disimpan ke trxBapsReject dan Data dari TrxBAPSData di Delete
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ListRejectBAPS"></param>
        /// <param name="ListRejectBAPSLog"></param>
        /// <returns></returns>
        public trxBapsData RejectBAPS(string UserID, List<int> id, string Remarks, int MstRejectDtlId, string statusRejectPOConfirm)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var context2 = new DbContext(Helper.GetConnection("TBGSysV1"));
            var contextARTBiG = new DbContext(Helper.GetConnection("ARTBiG"));
            //var vmRejectBapsExisting = new ARSystem.Domain.Models.vmRejectBAPSExisting();
            //var vmRejectBapsExisting = new TBGSysService.vmRejectBAPSExisting();
            var trxBapsDataReject = new trxBapsReject();

            var ExistingRepo = new ExistingRepository(context);
            var TrxBapsRejectRepo = new trxBapsRejectRepository(context);
            var TrxBapsDataRepo = new trxBapsDataRepository(context);
            var TrxBapsDataRepo2 = new trxBapsDataRepository(context);
            var invoiceStatusRepo = new mstInvoiceStatusRepository(context);
            var ReturnPOMarketingRepo = new ReturnPORepository(context2);
            var bapsStagingRepo = new TrxBapsStagingRepository(contextARTBiG);

            trxBapsData RejectV1Result = RejectBAPSARv1(UserID, id, Remarks, MstRejectDtlId, UserID);
            if (!string.IsNullOrEmpty(RejectV1Result.ErrorMessage))
                return new trxBapsData((int)Helper.ErrorType.Error, "Failed to Reject BAPS. Please try again in a few minutes, or call IT Helpdesk for assistance.");

            List<trxRANewBapsActivity> BapsActivity = new List<trxRANewBapsActivity>();
            string filter = "";
            string filterreject = "trxBapsDataId IN ( 0";
            string filterrejectactivity = "ID IN ( 0";
            int rejectsts = 1;

            var uow = context.CreateUnitOfWork();
            var uowARTBiG = contextARTBiG.CreateUnitOfWork();

            try
            {
                List<trxBapsData> ListTrxBapsData = new List<trxBapsData>();
                ListTrxBapsData = GetTrxBapsListDataToList(UserID, id);
                List<trxBapsReject> ListTrxBapsDataReject = new List<trxBapsReject>();

                List<int> ListTrxReconcileID = new List<int>();
                List<int> ListTrxPOID = new List<int>();

                mstPICADetail picaDetail = new mstPICADetail();
                mstPICADetailRepository picaDetailRepo = new mstPICADetailRepository(context);
                picaDetail = picaDetailRepo.GetByPK(MstRejectDtlId);
                //TO DECIDE WHERE THE SONUMBER WILL BE REJECTED TO (MARKETING -> Z ELSE -> R)
                var RejectDestination = picaDetail.mstPICATypeID == (int)Constants.PICATypeIDARData.ExtMarketing ? "Z" : "R";

                foreach (trxBapsData TrxBapsData in ListTrxBapsData)
                {

                    //INSERT KE LOG TRXBAPSREJECT DAN DELETE TRXBAPSDATA
                    #region 'INSERT DATA UNTUK TrxBapsReject'
                    trxBapsDataReject = new trxBapsReject();
                    trxBapsDataReject.SONumber = TrxBapsData.SONumber;
                    trxBapsDataReject.SiteIdOld = TrxBapsData.SiteIdOld;
                    trxBapsDataReject.SiteIdOpr = TrxBapsData.SiteIdOpr;
                    trxBapsDataReject.SiteName = TrxBapsData.SiteName;
                    trxBapsDataReject.CompanyInvoice = TrxBapsData.CompanyInvoice;
                    trxBapsDataReject.CompanyId = TrxBapsData.CompanyId;
                    trxBapsDataReject.Operator = TrxBapsData.Operator;
                    trxBapsDataReject.OperatorAsset = TrxBapsData.OperatorAsset;
                    trxBapsDataReject.Type = TrxBapsData.Type;
                    trxBapsDataReject.SpkDate = TrxBapsData.SpkDate;
                    trxBapsDataReject.RfiDate = TrxBapsData.RfiDate;
                    trxBapsDataReject.BapsPeriod = TrxBapsData.BapsPeriod;
                    trxBapsDataReject.BapsNo = TrxBapsData.BapsNo;
                    trxBapsDataReject.PoNumber = TrxBapsData.PoNumber;
                    trxBapsDataReject.Status = TrxBapsData.Status;
                    trxBapsDataReject.StatusDescription = TrxBapsData.StatusDescription;
                    trxBapsDataReject.PeriodInvoice = TrxBapsData.PeriodInvoice;
                    trxBapsDataReject.InvoiceAmount = TrxBapsData.InvoiceAmount.Value;
                    trxBapsDataReject.AmountRental = TrxBapsData.AmountRental.Value;
                    trxBapsDataReject.AmountService = TrxBapsData.AmountService.Value;
                    trxBapsDataReject.InvoiceTypeId = TrxBapsData.InvoiceTypeId;
                    trxBapsDataReject.InvoiceTypeDesc = TrxBapsData.InvoiceTypeDesc;
                    trxBapsDataReject.AmountInvoicePeriod = TrxBapsData.AmountInvoicePeriod.Value;
                    trxBapsDataReject.AmountPenaltyPeriod = TrxBapsData.AmountPenaltyPeriod.Value;
                    trxBapsDataReject.AmountOverdaya = TrxBapsData.AmountOverdaya.Value;
                    trxBapsDataReject.AmountOverblast = TrxBapsData.AmountOverblast.Value;
                    trxBapsDataReject.BapsType = TrxBapsData.BapsType;
                    trxBapsDataReject.StipSiro = TrxBapsData.StipSiro;
                    trxBapsDataReject.StipSiroId = TrxBapsData.StipSiroId.Value;
                    trxBapsDataReject.BapsDone = TrxBapsData.BapsDone;
                    trxBapsDataReject.PowerType = TrxBapsData.PowerType;
                    trxBapsDataReject.PowerTypeCode = TrxBapsData.PowerTypeCode;
                    trxBapsDataReject.Currency = TrxBapsData.Currency;
                    trxBapsDataReject.StartDate = TrxBapsData.StartDate;
                    trxBapsDataReject.EndDate = TrxBapsData.EndDate;
                    trxBapsDataReject.StartDateInvoice = TrxBapsData.StartDateInvoice;
                    trxBapsDataReject.EndDateInvoice = TrxBapsData.EndDateInvoice;
                    trxBapsDataReject.SPK = TrxBapsData.SPK;
                    trxBapsDataReject.Kontrak = TrxBapsData.Kontrak;
                    trxBapsDataReject.Rekon = TrxBapsData.Rekon;
                    trxBapsDataReject.BAPS = TrxBapsData.BAPS;
                    trxBapsDataReject.BAK = TrxBapsData.BAK;
                    trxBapsDataReject.SSR = TrxBapsData.SSR;
                    trxBapsDataReject.PO = TrxBapsData.PO;
                    trxBapsDataReject.BAUF = TrxBapsData.BAUF;
                    trxBapsDataReject.Regional = TrxBapsData.Regional;
                    trxBapsDataReject.IsLossPPN = TrxBapsData.IsLossPPN;
                    trxBapsDataReject.IsPartial = TrxBapsData.IsPartial;
                    trxBapsDataReject.AmountPPN = TrxBapsData.AmountPPN;
                    trxBapsDataReject.AmountLossPPN = TrxBapsData.AmountLossPPN;
                    trxBapsDataReject.MstRejectDtlId = MstRejectDtlId;
                    trxBapsDataReject.RejectRemarks = Remarks;
                    trxBapsDataReject.RejectDestination = RejectDestination;
                    trxBapsDataReject.mstInvoiceStatusId = TrxBapsData.mstInvoiceStatusId;
                    trxBapsDataReject.AmountRounding = TrxBapsData.AmountRounding;
                    trxBapsDataReject.IsPPHFinal = TrxBapsData.IsPPHFinal;
                    trxBapsDataReject.SiteNameOpr = TrxBapsData.SiteNameOpr;
                    trxBapsDataReject.ContractNumber = TrxBapsData.ContractNumber;
                    trxBapsDataReject.ReslipNumber = TrxBapsData.ReslipNumber;
                    trxBapsDataReject.CreatedBy = UserID;
                    trxBapsDataReject.CreatedDate = Helper.GetDateTimeNow();
                    trxBapsDataReject.trxReconcileID = TrxBapsData.trxReconcileID;
                    trxBapsDataReject.trxBapsDataId = TrxBapsData.trxBapsDataId;

                    ListTrxBapsDataReject.Add(trxBapsDataReject);

                    #endregion
                    //add by ASE Update remark reject PO , Return to PO Process Marketing TBG Sys v1
                    #region Update remark reject PO , Return to PO Process Marketing TBG Sys v1
                    if (statusRejectPOConfirm == "RejectPOCONFIRM")
                    {
                        trxBapsData bapsData = new trxBapsData();
                        bapsData = TrxBapsDataRepo.GetByPK(TrxBapsData.trxBapsDataId);
                        bapsData.mstInvoiceStatusIdPOConfirm = invoiceStatusRepo.GetList("Description = 'REJECT PO CONFIRM'").FirstOrDefault().mstInvoiceStatusId;
                        bapsData.RemarksRejectPO = picaDetail.Description + " | " + Remarks;
                        bapsData.UpdatedDate = Helper.GetDateTimeNow();
                        bapsData.UpdatedBy = UserID;

                        TrxBapsDataRepo2.Update(bapsData);
                        ReturnPOMarketingRepo.ReturnToPOProcessMarketing(bapsData.SONumber, bapsData.BapsType, bapsData.StipSiroId, 1, UserID);

                    }

                    #endregion

                    if (TrxBapsData.BAPS != null && TrxBapsData.trxReconcileID > 0 && TrxBapsData.BAPS.Length > 1)
                    {
                        BapsActivity.Add(new trxRANewBapsActivity
                        {
                            ID = Int32.Parse(TrxBapsData.BAPS)
                        });
                        filterrejectactivity = filterrejectactivity + "," + TrxBapsData.BAPS;
                        filterreject = filterreject + "," + TrxBapsData.trxBapsDataId.ToString();
                    }
                    else
                    {
                        //Add
                        ListTrxReconcileID.Add(TrxBapsData.trxReconcileID);
                        //  TrxBapsData.PO = TrxBapsData.PO == "" || TrxBapsData.PO == null ? "0" : TrxBapsData.PO;
                        if (TrxBapsData.trxReconcileID > 0 && TrxBapsData.PO != null)
                        {
                            ListTrxPOID.Add(Convert.ToInt32(TrxBapsData.PO));
                        }

                        filter = "[SONumber]='" + TrxBapsData.SONumber + "' AND [BapsNo]='" + TrxBapsData.BapsNo + "'" +
                            "AND [BapsType] = '" + TrxBapsData.BapsType + "' AND [StipSiro]='" + TrxBapsData.StipSiro + "'" +
                            "AND [BapsPeriod]='" + TrxBapsData.BapsPeriod + "' AND [Currency]='" + TrxBapsData.Currency + "'";

                        if (statusRejectPOConfirm != "RejectPOCONFIRM")
                            TrxBapsDataRepo.DeleteByFilter(filter);
                    }

                    if (TrxBapsData.trxBapsDataId != null)
                    {
                        bapsStagingRepo.DeleteByPK(TrxBapsData.trxBapsDataId);
                    }
                }

                if (BapsActivity.Count > 0)
                {
                    filterreject = filterreject + ")";
                    filterrejectactivity = filterrejectactivity + ")";
                    if (statusRejectPOConfirm != "RejectPOCONFIRM")
                    {
                        TrxBapsDataRepo.DeleteByFilter(filterreject);
                        rejectsts = RejectNewBAPS(UserID, filterrejectactivity);
                    }

                }

                if (statusRejectPOConfirm != "RejectPOCONFIRM")
                    TrxBapsRejectRepo.CreateBulky(ListTrxBapsDataReject);


                //Add by RBA

                var trxReconcileRepo = new trxReconcileRepository(context);

                string IDs;
                IDs = string.Join(",", ListTrxReconcileID);

                if (string.IsNullOrEmpty(IDs))
                    IDs = "0";

                string IDPo;
                IDPo = string.Join(",", ListTrxPOID);

                if (string.IsNullOrEmpty(IDPo))
                    IDPo = "0";

                var jobReturnToBapsRepo = new JobReturnToBAPSRepository(context);
                jobReturnToBapsRepo.ReturnToBAPSV2(Convert.ToString((int)Constants.RAActivity.BAPS_RETURN), IDs, IDPo);


                //Add by ASE
                #region Send email Reject PO
                if (statusRejectPOConfirm == "RejectPOCONFIRM") //Reject PO Confirm
                {
                    var mstPicaTypeRepo = new mstPICATypeRepository(context);
                    List<mstPICAType> ListmstPICAType = mstPicaTypeRepo.GetList("IsActive = 1");
                    string emailDevelopment = "information@internal.m-tbig.com";
                    string emailTo = "";
                    string emailCc = "";
                    string emailBcc = "";
                    string Subject = "";
                    string Body = "";
                    string emailReplyTo = "";
                    var tblHeader = "";
                    var tblRow = "";
                    foreach (trxBapsReject BAPSData in ListTrxBapsDataReject)
                    {
                        if (BAPSData.StartDateInvoice == null)
                            tblRow += "<tr><td>" + BAPSData.SONumber + "</td><td>" + BAPSData.SiteIdOld + "</td><td>" + BAPSData.SiteName + "</td><td>" + BAPSData.PoNumber + "</td><td>" + BAPSData.Operator + "</td><td>" + "" + "</td><td>" + "" + "</td><td>" + BAPSData.InvoiceAmount.ToString() + "</td><td></td></tr>";

                        else
                            tblRow += "<tr><td>" + BAPSData.SONumber + "</td><td>" + BAPSData.SiteIdOld + "</td><td>" + BAPSData.SiteName + "</td><td>" + BAPSData.PoNumber + "</td><td>" + BAPSData.Operator + "</td><td>" + Convert.ToDateTime(BAPSData.StartDateInvoice).ToString("dd MMMM yyyy") + "</td><td>" + Convert.ToDateTime(BAPSData.EndDateInvoice).ToString("dd MMMM yyyy") + "</td><td>" + BAPSData.InvoiceAmount.ToString() + "</td><td></td></tr>";

                    }

                    Subject = "[ARSystem] - Notification Reject PO Confirm";
                    emailTo = ListmstPICAType.Where(x => x.mstPICATypeID.ToString() == Convert.ToInt32(picaDetail.mstPICATypeID).ToString()).FirstOrDefault().Recipient;
                    emailCc += ListmstPICAType.Where(x => x.mstPICATypeID.ToString() == Convert.ToInt32(picaDetail.mstPICATypeID).ToString()).FirstOrDefault().CC;

                    tblHeader = "Dear all,<br><br>Berikut Data BAPS yang berstatus Reject PO Confirm.<br> <table border=0 cellpadding=4 cellspacing=1><tr bgcolor=#CCCCCC><th align=center><b>SO Number</b></th><th align=center><b>Site ID</b></th><th align=center><b>Site Name</b></th><th align=center><b>PO Number</b></th><th align=center><b>Operator</b></th><th align=center><b>Start Date Receivable</b></th><th align=center><b>End Date Receivable</b></th><th align=center><b>Amount Invoice</b></th></tr>";
                    Body = tblHeader + tblRow + "</table><br><br><b> PICA Category </b> : " + picaDetail.Description + "<br><b> PICA </b> : " + Remarks + "<br><b> Rejected Date </b> : " + DateTime.Now.ToString("dd MMMM yyyy") + "<br><br><br><br><b>ARSystem Notification</b><br><br><b><i>*This is an ARSystem automatic e-mail, please do not reply</i></b><br>";

                    EmailHelper.SendEmail(emailTo, emailCc, emailBcc, Subject, Body, emailReplyTo, UserID);

                    //Only for testing development 
                    //Body += "<br> Recipient : " + emailTo + "<br>  Cc : " + emailCc;
                    //emailTo = emailDevelopment;
                    //emailCc = emailDevelopment;
                }


                #endregion
                //End Add ASE

                //if (rejectsts > 0)
                uow.SaveChanges();
                uowARTBiG.SaveChanges();

                return new trxBapsData();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                uowARTBiG.Dispose();
                return new trxBapsData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBAPSDataService", "RejectTrxBAPSData", UserID));
            }
            finally
            {
                context.Dispose();
                context2.Dispose();
                contextARTBiG.Dispose();
            }
        }

        /// <summary>
        /// Used by RejectBAPS, call this method first which is calling TBGSysService then call v2 method. 
        /// if not when there is error in TBGSysService v2 code will still running
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="id"></param>
        /// <param name="Remarks"></param>
        /// <param name="Department"></param>
        /// <param name="MstRejectDtlId"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        private trxBapsData RejectBAPSARv1(string UserID, List<int> id, string Remarks, int MstRejectDtlId, string userID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxBapsRejectRepo = new trxBapsRejectRepository(context);
            var TrxBapsDataRepo = new trxBapsDataRepository(context);
            var trxBapsDataReject = new trxBapsReject();
            try
            {
                List<trxBapsData> ListTrxBapsData = new List<trxBapsData>();
                ListTrxBapsData = GetTrxBapsListDataToList(UserID, id);
                List<trxBapsReject> ListTrxBapsDataReject = new List<trxBapsReject>();

                mstPICADetail picaDetail = new mstPICADetail();
                mstPICADetailRepository picaDetailRepo = new mstPICADetailRepository(context);
                picaDetail = picaDetailRepo.GetByPK(MstRejectDtlId);
                //TO DECIDE WHERE THE SONUMBER WILL BE REJECTED TO (MARKETING -> Z ELSE -> R)
                var RejectDestination = picaDetail.mstPICATypeID == (int)Constants.PICATypeIDARData.ExtMarketing ? "Z" : "R";

                #region Get List Of Rejected SO Number
                foreach (trxBapsData TrxBapsData in ListTrxBapsData)
                {
                    trxBapsDataReject = new trxBapsReject();
                    trxBapsDataReject.SONumber = TrxBapsData.SONumber;
                    trxBapsDataReject.SiteIdOld = TrxBapsData.SiteIdOld;
                    trxBapsDataReject.SiteIdOpr = TrxBapsData.SiteIdOpr;
                    trxBapsDataReject.SiteName = TrxBapsData.SiteName;
                    trxBapsDataReject.CompanyInvoice = TrxBapsData.CompanyInvoice;
                    trxBapsDataReject.CompanyId = TrxBapsData.CompanyId;
                    trxBapsDataReject.Operator = TrxBapsData.Operator;
                    trxBapsDataReject.OperatorAsset = TrxBapsData.OperatorAsset;
                    trxBapsDataReject.Type = TrxBapsData.Type;
                    trxBapsDataReject.SpkDate = TrxBapsData.SpkDate;
                    trxBapsDataReject.RfiDate = TrxBapsData.RfiDate;
                    trxBapsDataReject.BapsPeriod = TrxBapsData.BapsPeriod;
                    trxBapsDataReject.BapsNo = TrxBapsData.BapsNo;
                    trxBapsDataReject.PoNumber = TrxBapsData.PoNumber;
                    trxBapsDataReject.Status = TrxBapsData.Status;
                    trxBapsDataReject.StatusDescription = TrxBapsData.StatusDescription;
                    trxBapsDataReject.PeriodInvoice = TrxBapsData.PeriodInvoice;
                    trxBapsDataReject.InvoiceAmount = TrxBapsData.InvoiceAmount.Value;
                    trxBapsDataReject.AmountRental = TrxBapsData.AmountRental.Value;
                    trxBapsDataReject.AmountService = TrxBapsData.AmountService.Value;
                    trxBapsDataReject.InvoiceTypeId = TrxBapsData.InvoiceTypeId;
                    trxBapsDataReject.InvoiceTypeDesc = TrxBapsData.InvoiceTypeDesc;
                    trxBapsDataReject.AmountInvoicePeriod = TrxBapsData.AmountInvoicePeriod.Value;
                    trxBapsDataReject.AmountPenaltyPeriod = TrxBapsData.AmountPenaltyPeriod.Value;
                    trxBapsDataReject.AmountOverdaya = TrxBapsData.AmountOverdaya.Value;
                    trxBapsDataReject.AmountOverblast = TrxBapsData.AmountOverblast.Value;
                    trxBapsDataReject.BapsType = TrxBapsData.BapsType;
                    trxBapsDataReject.StipSiro = TrxBapsData.StipSiro;
                    trxBapsDataReject.StipSiroId = TrxBapsData.StipSiroId.Value;
                    trxBapsDataReject.BapsDone = TrxBapsData.BapsDone;
                    trxBapsDataReject.PowerType = TrxBapsData.PowerType;
                    trxBapsDataReject.PowerTypeCode = TrxBapsData.PowerTypeCode;
                    trxBapsDataReject.Currency = TrxBapsData.Currency;
                    trxBapsDataReject.StartDate = TrxBapsData.StartDate;
                    trxBapsDataReject.EndDate = TrxBapsData.EndDate;
                    trxBapsDataReject.StartDateInvoice = TrxBapsData.StartDateInvoice;
                    trxBapsDataReject.EndDateInvoice = TrxBapsData.EndDateInvoice;
                    trxBapsDataReject.SPK = TrxBapsData.SPK;
                    trxBapsDataReject.Kontrak = TrxBapsData.Kontrak;
                    trxBapsDataReject.Rekon = TrxBapsData.Rekon;
                    trxBapsDataReject.BAPS = TrxBapsData.BAPS;
                    trxBapsDataReject.BAK = TrxBapsData.BAK;
                    trxBapsDataReject.SSR = TrxBapsData.SSR;
                    trxBapsDataReject.PO = TrxBapsData.PO;
                    trxBapsDataReject.BAUF = TrxBapsData.BAUF;
                    trxBapsDataReject.Regional = TrxBapsData.Regional;
                    trxBapsDataReject.IsLossPPN = TrxBapsData.IsLossPPN;
                    trxBapsDataReject.IsPartial = TrxBapsData.IsPartial;
                    trxBapsDataReject.AmountPPN = TrxBapsData.AmountPPN;
                    trxBapsDataReject.AmountLossPPN = TrxBapsData.AmountLossPPN;
                    trxBapsDataReject.MstRejectDtlId = MstRejectDtlId;
                    trxBapsDataReject.RejectRemarks = Remarks;
                    trxBapsDataReject.RejectDestination = RejectDestination;
                    trxBapsDataReject.mstInvoiceStatusId = TrxBapsData.mstInvoiceStatusId;
                    trxBapsDataReject.AmountRounding = TrxBapsData.AmountRounding;
                    trxBapsDataReject.IsPPHFinal = TrxBapsData.IsPPHFinal;
                    trxBapsDataReject.SiteNameOpr = TrxBapsData.SiteNameOpr;
                    trxBapsDataReject.ContractNumber = TrxBapsData.ContractNumber;
                    trxBapsDataReject.ReslipNumber = TrxBapsData.ReslipNumber;
                    trxBapsDataReject.CreatedBy = userID;
                    trxBapsDataReject.CreatedDate = Helper.GetDateTimeNow();
                    trxBapsDataReject.trxReconcileID = TrxBapsData.trxReconcileID;
                    ListTrxBapsDataReject.Add(trxBapsDataReject);

                }
                #endregion

                #region 'UPDATE DATA UNTUK ExistingTBGSys'
                string tokenWebService = "bbac92c7-ff22-45f6-8298-dfcfb1df68ad";


                string xml = Helper.XmlSerializer<List<trxBapsReject>>(ListTrxBapsDataReject);
                string result = "";

                using (var client = new TBGSysService.Service())
                {
                    result = client.RejectBAPS(tokenWebService, xml, userID, 0, false);

                }
                if (!string.IsNullOrEmpty(result))
                {
                    Helper.logError(result, "trxBAPSDataService", "RejectBAPSARv1", userID);
                    using (var client = new TBGSysService.Service())
                    {
                        //result = client.DeleteJobRow(tokenWebService);

                    }
                    return new trxBapsData((int)Helper.ErrorType.Error, result);
                }
                return new trxBapsData();
                #endregion
            }
            catch (Exception ex)
            {
                return new trxBapsData((int)Helper.ErrorType.Error, ex.Message);
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<mstPICAType> GetRejectHdrToList(string UserID, string strOrderBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var mstPICATypeRepo = new mstPICATypeRepository(context);
            List<mstPICAType> listHdr = new List<mstPICAType>();

            try
            {
                string strWhereClause = "IsActive = '" + 1 + "'AND mstUserGroupId = " + (int)Constants.UserGroup.ARData +
                    " AND mstPICATypeID > 2 AND mstPICATypeID < 8 AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                listHdr = mstPICATypeRepo.GetList(strWhereClause, strOrderBy);

                return listHdr;
            }
            catch (Exception ex)
            {
                listHdr.Add(new mstPICAType((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBAPSDataService", "GetmstPICATypeToList", UserID)));
                return listHdr;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<mstPICADetail> GetRejectDtlToList(string UserID, int HdrId, string strOrderBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var mstRejectDtlRepo = new mstPICADetailRepository(context);
            List<mstPICADetail> listDtl = new List<mstPICADetail>();

            try
            {
                string strWhereClause = "IsActive = '" + 1 + "' AND ";
                if (!string.IsNullOrWhiteSpace(HdrId.ToString()))
                {
                    strWhereClause += "mstPICATypeID = '" + HdrId + "' AND ";
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                listDtl = mstRejectDtlRepo.GetList(strWhereClause, strOrderBy);


                return listDtl;
            }
            catch (Exception ex)
            {
                listDtl.Add(new mstPICADetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBAPSDataService", "GetmstPICADetailToList", UserID)));
                return listDtl;
            }
            finally
            {
                context.Dispose();
            }
        }

        public trxBapsData UpdateInvoiceAmount(string UserID, int trxBapsDataId, decimal AmountRounding)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();

            var TrxBapsDataRepo = new trxBapsDataRepository(context);

            try
            {
                #region 'Update InvoiceHeader Status'

                trxBapsData BapsData = TrxBapsDataRepo.GetByPK(trxBapsDataId);
                BapsData.InvoiceAmount = BapsData.InvoiceAmount + AmountRounding;
                BapsData.UpdatedBy = UserID;
                BapsData.UpdatedDate = Helper.GetDateTimeNow();
                BapsData.AmountRounding = (BapsData.AmountRounding.HasValue) ? (BapsData.AmountRounding.Value + AmountRounding) : AmountRounding;
                BapsData = TrxBapsDataRepo.Update(BapsData);
                #endregion

                uow.SaveChanges();

                return BapsData;

            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxBapsData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBAPSDataService", "UpdateRentalAmount", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        private List<trxBapsData> GetTrxBapsListDataToList(string UserID, List<int> id)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxDataBapsRepo = new trxBapsDataRepository(context);
            List<trxBapsData> listDataBAPS = new List<trxBapsData>();


            try
            {
                string strWhereClause = "";
                if (id.Count() != 0)
                {
                    string listId = string.Empty;
                    foreach (int trxBapsDataId in id)
                    {
                        if (listId == string.Empty)
                            listId = "'" + trxBapsDataId.ToString() + "'";
                        else
                            listId += ",'" + trxBapsDataId.ToString() + "'";
                    }
                    strWhereClause += "trxBapsDataId IN (" + listId + ") AND ";
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                listDataBAPS = TrxDataBapsRepo.GetList(strWhereClause, "");

                return listDataBAPS;
            }
            catch (Exception ex)
            {
                listDataBAPS.Add(new trxBapsData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GetTrxBapsListDataToList", UserID)));
                return listDataBAPS;
            }
            finally
            {
                context.Dispose();
            }
        }

        public vwBAPSData ReceiveBAPS(string UserID, List<int> id)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();
            var ListBAPSData = new List<trxBapsData>();
            var BAPSDataRepo = new trxBapsDataRepository(context);
            var EmployeeRepo = new mstEmployeeRepository(context);
            var listLogAr = new List<logArActivity>();
            var LogArRepo = new logArActivityRepository(context);

            try
            {
                ListBAPSData = GetTrxBapsListDataToList(UserID, id);
                //Added by ASE enhance flow Confirm (Freeze, Unfreeze, POConfirm)

                List<trxBapsData> ListBAPSwaitingPO = new List<trxBapsData>();
                var mstInvoiceStatusRepo = new mstInvoiceStatusRepository(context);
                var mstBAPSConfirmValidateRepo = new mstBAPSConfirmValidateRepository(context);
                var mstPicaTypeRepo = new mstPICATypeRepository(context);
                List<mstBAPSConfirmValidate> ListmstBAPSConfirmValidate = mstBAPSConfirmValidateRepo.GetList("IsActive = 1 AND TypeValidation = 'PO_CONFIRM'");

                string validateType = "";
                //End ASE
                foreach (trxBapsData BAPSData in ListBAPSData)
                {
                    if (ListmstBAPSConfirmValidate.Count(n => n.OperatorID == BAPSData.Operator && n.InvoiceType == BAPSData.PeriodInvoice && n.TypeValidation == "PO_CONFIRM" && n.BapsType.ToUpper().Trim() == BAPSData.BapsType.ToUpper().Trim()) > 0)
                    {
                        validateType = "PO CONFIRM";
                        BAPSData.mstInvoiceStatusIdPOConfirm = mstInvoiceStatusRepo.GetList("Description = 'PO CONFIRM'").FirstOrDefault().mstInvoiceStatusId;
                    }
                    BAPSData.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateBAPSReceive;//RECEIVED, NOT SHOW IN RECEIVE TABLE
                    BAPSData.UpdatedBy = UserID;
                    BAPSData.UpdatedDate = Helper.GetDateTimeNow();
                    BAPSDataRepo.Update(BAPSData);
                    if (validateType == "PO CONFIRM")
                        ListBAPSwaitingPO.Add(BAPSData);


                    logArActivity logAr = new logArActivity();
                    logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.BAPSReceive;
                    logAr.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                    logAr.trxBapsDataId = BAPSData.trxBapsDataId;
                    logAr.CreatedBy = UserID;
                    logAr.CreatedDate = Helper.GetDateTimeNow();
                    listLogAr.Add(logAr);

                }
                LogArRepo.CreateBulky(listLogAr);
                uow.SaveChanges();
                //Add by ASE
                #region Send email Waiting PO Confirm Or PO Confirmed
                List<mstPICAType> ListmstPICAType = mstPicaTypeRepo.GetList("IsActive = 1");
                string emailDevelopment = "information@internal.m-tbig.com";
                string emailTo = "";
                string emailCc = "";
                string emailBcc = "";
                string Subject = "";
                string Body = "";
                string emailReplyTo = "";
                var Header = "";
                var Row = "";
                if (validateType == "PO CONFIRM")
                {
                    foreach (trxBapsData BAPSData in ListBAPSwaitingPO)
                    {
                        if (BAPSData.StartDateInvoice == null)
                            Row += "<tr><td>" + BAPSData.SONumber + "</td><td>" + BAPSData.SiteIdOld + "</td><td>" + BAPSData.SiteName + "</td><td>" + BAPSData.PoNumber + "</td><td>" + BAPSData.Operator + "</td><td>" + "" + "</td><td>" + "" + "</td><td>" + BAPSData.InvoiceAmount.ToString() + "</td><td></td></tr>";

                        else
                            Row += "<tr><td>" + BAPSData.SONumber + "</td><td>" + BAPSData.SiteIdOld + "</td><td>" + BAPSData.SiteName + "</td><td>" + BAPSData.PoNumber + "</td><td>" + BAPSData.Operator + "</td><td>" + Convert.ToDateTime(BAPSData.StartDateInvoice).ToString("dd MMMM yyyy") + "</td><td>" + Convert.ToDateTime(BAPSData.EndDateInvoice).ToString("dd MMMM yyyy") + "</td><td>" + BAPSData.InvoiceAmount.ToString() + "</td><td></td></tr>";

                    }

                    Subject = "[ARSystem] - Notification Waiting PO Input";
                    emailTo = ListmstPICAType.Where(x => x.Description.ToString() == "External - Marketing").FirstOrDefault().Recipient;
                    emailCc += ListmstPICAType.Where(x => x.Description.ToString() == "External - RA New").FirstOrDefault().Recipient;
                    emailCc += ";" + ListmstPICAType.Where(x => x.Description.ToString() == "Internal - AR Data").FirstOrDefault().Recipient;


                    Header = "Dear all,<br><br>Berikut Data BAPS yang berstatus Waiting PO Input.<br><table border=0 cellpadding=4 cellspacing=1><tr bgcolor=#CCCCCC><th align=center><b>SO Number</b></th><th align=center><b>Site ID</b></th><th align=center><b>Site Name</b></th><th align=center><b>PO Number</b></th><th align=center><b>Operator</b></th><th align=center><b>Start Date Receivable</b></th><th align=center><b>End Date Receivable</b></th><th align=center><b>Amount Invoice</b></th></tr>";
                    Body = Header + Row + "</table><br><br><br><b>ARSystem Notification</b><br><br><b><i>*This is an ARSystem automatic e-mail, please do not reply</i></b><br>";


                    EmailHelper.SendEmail(emailTo, emailCc, emailBcc, Subject, Body, emailReplyTo, UserID);
                }

                #endregion
                //End Add ASE

                //Automatic Email for BAPS Service
                //Grab Email List New and Recurring 
                var strWhere = "DepartmentCode='290' OR DepartmentCode='362'";
                var ListEmail = EmployeeRepo.GetList(strWhere);

                var stMail = "";
                foreach (mstEmployee dtEmail in ListEmail)
                {
                    stMail = stMail + dtEmail.Email + ";";
                }
                //Grab Email List Recurring
                var strWhere2 = "DepartmentCode='290'";
                var ListEmail2 = EmployeeRepo.GetList(strWhere2);
                var stMail2 = "";
                foreach (mstEmployee dtEmail2 in ListEmail2)
                {
                    stMail2 = stMail2 + dtEmail2.Email + ";";
                }

                var tblHeader = "Dear all,<br><br>Berikut Data BAPS Receive.<br><table border=0 cellpadding=4 cellspacing=1><tr bgcolor=#CCCCCC><th align=center><b>SO Number</b></th><th align=center><b>Site ID</b></th><th align=center><b>Site Name</b></th><th align=center><b>Site Type</b></th><th align=center><b>Operator</b></th><th align=center><b>Company</b></th><th align=center><b>Regional</b></th><th align=center><b>Site ID Operator</b></th><th align=center><b>Start Date Contract</b></th><th align=center><b>End Date Contract</b></th><th align=center><b>Start Date Invoice</b></th><th align=center><b>End Date Invoice</b></th><th align=center><b>Basic Amount</b></th><th align=center><b>AMonthly</b></th><th align=center><b>Service Amount</b></th><th align=center><b>Amonthly</b></th><th align=center><b>Total Amount</b></th><th align=center><b>Yearly</b></th></tr>";

                var tblRow = "";
                var tblRow2 = "";

                foreach (trxBapsData BAPSData in ListBAPSData)
                {
                    if (BAPSData.PeriodInvoice == "NEW")
                    {
                        tblRow = tblRow + "<tr><td>" + BAPSData.SONumber + "</td><td>" + BAPSData.SiteIdOld + "</td><td>" + BAPSData.SiteName + "</td><td>" + BAPSData.Type + "</td><td>" + BAPSData.Operator + "</td><td>" + BAPSData.CompanyId + "</td><td>" + BAPSData.Regional + "</td><td>" + BAPSData.SiteIdOpr + "</td><td>" + BAPSData.StartDate + "</td><td>" + BAPSData.EndDate + "</td><td>" + BAPSData.StartDateInvoice + "</td><td>" + BAPSData.EndDateInvoice + "</td><td>" + BAPSData.AmountRental + "</td><td>" + BAPSData.AmountOverblast + "</td><td>" + BAPSData.AmountService + "</td><td>" + BAPSData.AmountOverdaya + "</td><td>" + BAPSData.AmountInvoicePeriod + "</td><td></td></tr>";
                    }

                    if (BAPSData.PeriodInvoice == "RENEWAL")
                    {
                        tblRow2 = tblRow2 + "<tr><td>" + BAPSData.SONumber + "</td><td>" + BAPSData.SiteIdOld + "</td><td>" + BAPSData.SiteName + "</td><td>" + BAPSData.Type + "</td><td>" + BAPSData.Operator + "</td><td>" + BAPSData.CompanyId + "</td><td>" + BAPSData.Regional + "</td><td>" + BAPSData.SiteIdOpr + "</td><td>" + BAPSData.StartDate + "</td><td>" + BAPSData.EndDate + "</td><td>" + BAPSData.StartDateInvoice + "</td><td>" + BAPSData.EndDateInvoice + "</td><td>" + BAPSData.AmountRental + "</td><td>" + BAPSData.AmountOverblast + "</td><td>" + BAPSData.AmountService + "</td><td>" + BAPSData.AmountOverdaya + "</td><td>" + BAPSData.AmountInvoicePeriod + "</td><td></td></tr>";
                    }
                }

                if (tblRow != "")
                {
                    var Isi = tblHeader + tblRow + "</table><br><br><br><b>TBGSys</b><br>";
                    EmailHelper.SendEmail(stMail, "information@internal.m-tbig.com", "information@internal.m-tbig.com", "BAPS Receive NEW", Isi, "information@internal.m-tbig.com", "555502180010");
                }

                if (tblRow2 != "")
                {
                    var Isi2 = tblHeader + tblRow2 + "</table><br><br><br><b>TBGSys</b><br>";
                    EmailHelper.SendEmail(stMail2, "information@internal.m-tbig.com", "information@internal.m-tbig.com", "BAPS Receive RENEWAL", Isi2, "information@internal.m-tbig.com", "555502180010");
                }

                return new vwBAPSData();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new vwBAPSData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBAPSDataService", "ReceiveBAPSData", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<int> GetBAPSDataListId(string UserID, string strCompanyId, string strOperator, string strStatusBAPS, string strPeriodInvoice, string strInvoiceType, string strCurrency, string strPONumber, string strBAPSNumber, string strSONumber, string strBapsType, string strSiteIdOld, string strStartPeriod, string strEndPeriod, int isReceive)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var CustomRepo = new GetListIdRepository(context);
            List<int> ListId = new List<int>();
            var mstInvoiceStatusRepo = new mstInvoiceStatusRepository(context);
            try
            {
                string strWhereClause = "";
                if (isReceive == 1)
                    strWhereClause = "mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.NotProcessed + " AND "; //TAB BAPS RECEIVE 
                else if (isReceive == 18)
                    strWhereClause = "mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.StateBAPSReceive + " AND "; //TAB BAPS CONFIRM 
                else if (isReceive == 0)
                    strWhereClause = "mstInvoiceStatusIdPOConfirm = " + mstInvoiceStatusRepo.GetList("Description = 'PO CONFIRM'").FirstOrDefault().mstInvoiceStatusId + " AND PoNumber <> '' AND PoNumber IS NOT NULL AND "; //TAB PO Confirm
                if (!string.IsNullOrWhiteSpace(strCompanyId))
                {
                    strWhereClause += "CompanyId = '" + strCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += "Operator = '" + strOperator + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStatusBAPS))
                {
                    strWhereClause += "Status = '" + strStatusBAPS + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPeriodInvoice))
                {
                    strWhereClause += "PeriodInvoice = '" + strPeriodInvoice + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strInvoiceType))
                {
                    strWhereClause += "InvoiceTypeId = '" + strInvoiceType + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strCurrency))
                {
                    strWhereClause += "Currency = '" + strCurrency + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPONumber))
                {
                    strWhereClause += "PoNumber LIKE '%" + strPONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBAPSNumber))
                {
                    strWhereClause += "BapsNo LIKE '%" + strBAPSNumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSONumber))
                {
                    strWhereClause += "SONumber LIKE '%" + strSONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBapsType))
                {
                    strWhereClause += "BapsType LIKE '%" + strBapsType + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteIdOld))
                {
                    strWhereClause += "SiteIdOld LIKE '%" + strSiteIdOld + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "StartDateInvoice >= '" + DateTime.Parse(strStartPeriod).ToString("yyyy-MM-dd") + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "EndDateInvoice <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                ListId = CustomRepo.GetListId("TrxBapsDataId", "vwBapsData", strWhereClause);
                return ListId;
            }
            catch (Exception ex)
            {
                return ListId;
            }
            finally
            {
                context.Dispose();
            }

        }

        public List<vwBAPSDataReject> GetTrxBAPSRejectToList(string UserID, string strCompanyId, string strOperator, string strStatusBAPS, string strPeriodInvoice, string strInvoiceType, string strCurrency, string strPONumber, string strBAPSNumber, string strSONumber, string strBapsType, string strSiteIdOld, string strStartPeriod, string strEndPeriod, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxBAPSRejectRepo = new vwBAPSDataRejectRepository(context);
            List<vwBAPSDataReject> listBAPSData = new List<vwBAPSDataReject>();

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(strCompanyId))
                {
                    strWhereClause += "CompanyId = '" + strCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += "Operator = '" + strOperator + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStatusBAPS))
                {
                    strWhereClause += "Status = '" + strStatusBAPS + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPeriodInvoice))
                {
                    strWhereClause += "PeriodInvoice = '" + strPeriodInvoice + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strInvoiceType))
                {
                    strWhereClause += "InvoiceTypeId = '" + strInvoiceType + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strCurrency))
                {
                    strWhereClause += "Currency = '" + strCurrency + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPONumber))
                {
                    strWhereClause += "PoNumber LIKE '%" + strPONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBAPSNumber))
                {
                    strWhereClause += "BapsNo LIKE '%" + strBAPSNumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSONumber))
                {
                    strWhereClause += "SONumber LIKE '%" + strSONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBapsType))
                {
                    strWhereClause += "BapsType LIKE '%" + strBapsType + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteIdOld))
                {
                    strWhereClause += "SiteIdOld LIKE '%" + strSiteIdOld + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "CONVERT(VARCHAR, StartDateInvoice, 106) LIKE '%" + strStartPeriod.Replace('-', ' ') + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "CONVERT(VARCHAR, EndDateInvoice, 106) LIKE '%" + strEndPeriod.Replace('-', ' ') + "%' AND ";
                    //strWhereClause += "EndDateInvoice <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                //set order by
                if (string.IsNullOrWhiteSpace(strOrderBy))
                {
                    strOrderBy = "CreatedDate DESC";

                }

                if (intPageSize > 0)
                    listBAPSData = TrxBAPSRejectRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    listBAPSData = TrxBAPSRejectRepo.GetList(strWhereClause, strOrderBy);

                return listBAPSData;
            }
            catch (Exception ex)
            {
                listBAPSData.Add(new vwBAPSDataReject((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GetTrxBAPSRejectToList", UserID)));
                return listBAPSData;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetTrxBapsRejectCount(string UserID, string strCompanyId, string strOperator, string strStatusBAPS, string strPeriodInvoice, string strInvoiceType, string strCurrency, string strPONumber, string strBAPSNumber, string strSONumber, string strBapsType, string strSiteIdOld, string strStartPeriod, string strEndPeriod)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxBAPSRejectRepo = new vwBAPSDataRejectRepository(context);
            List<vwBAPSDataReject> listBAPSData = new List<vwBAPSDataReject>();

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(strCompanyId))
                {
                    strWhereClause += "CompanyId = '" + strCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += "Operator = '" + strOperator + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStatusBAPS))
                {
                    strWhereClause += "Status = '" + strStatusBAPS + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPeriodInvoice))
                {
                    strWhereClause += "PeriodInvoice = '" + strPeriodInvoice + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strInvoiceType))
                {
                    strWhereClause += "InvoiceTypeId = '" + strInvoiceType + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strCurrency))
                {
                    strWhereClause += "Currency = '" + strCurrency + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPONumber))
                {
                    strWhereClause += "PoNumber LIKE '%" + strPONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBAPSNumber))
                {
                    strWhereClause += "BapsNo LIKE '%" + strBAPSNumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSONumber))
                {
                    strWhereClause += "SONumber LIKE '%" + strSONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBapsType))
                {
                    strWhereClause += "BapsType LIKE '%" + strBapsType + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteIdOld))
                {
                    strWhereClause += "SiteIdOld LIKE '%" + strSiteIdOld + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "CONVERT(VARCHAR, StartDateInvoice, 106) LIKE '%" + strStartPeriod.Replace('-', ' ') + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "CONVERT(VARCHAR, EndDateInvoice, 106) LIKE '%" + strEndPeriod.Replace('-', ' ') + "%' AND ";
                    //strWhereClause += "EndDateInvoice <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return TrxBAPSRejectRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GetTrxBapsRejectCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int RejectNewBAPS(string UserID, string Where)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repository = new RevenueAssuranceRepository(context);
            List<mstDropdown> list = new List<mstDropdown>();

            try
            {

                var command = context.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "UPDATE trxRANewBapsActivity SET mstRAActivityID = 14 Where " + Where;

                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxBapsDataService", "RejectNewBAPS", "");
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int UpdateInflationAmount(string UserID, long ID, decimal InflationAmount)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repository = new RevenueAssuranceRepository(context);

            try
            {
                return repository.UpdateInflationAmount(ID, InflationAmount);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxBapsDataService", "UpdateInflationAmount", "");
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }


        #region Added by ASE - enhance flow Baps confirm (freeze, unfreeze, Po confirm)
        public List<vwDataBAPSConfirm> GetTrxBapsConfirmNewFlowToList(string UserID, string strCompanyId, string strOperator, string strBAPSType, string strPeriodInvoice, string strInvoiceType, string strSoNumber, string strPONumber, string strBAPSNumber, string strSiteIdOld, string strStartPeriod, string strEndPeriod, int isFreeze, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxDataBapsConfirmRepo = new vwDataBAPSConfirmRepository(context);
            List<vwDataBAPSConfirm> listDataBAPSConfirm = new List<vwDataBAPSConfirm>();
            var mstInvoiceStatusRepo = new mstInvoiceStatusRepository(context);
            try
            {
                string strWhereClause = "";
                if (isFreeze == 1)
                    strWhereClause = "mstInvoiceStatusId = " + mstInvoiceStatusRepo.GetList("Description = 'FREEZE'").FirstOrDefault().mstInvoiceStatusId + " AND "; //TAB Freeze 
                else
                    strWhereClause = "mstInvoiceStatusId = " + mstInvoiceStatusRepo.GetList("Description = 'PO CONFIRM'").FirstOrDefault().mstInvoiceStatusId + " AND "; //TAB PO Confirm
                if (!string.IsNullOrWhiteSpace(strCompanyId))
                {
                    strWhereClause += "CompanyId = '" + strCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += "Operator = '" + strOperator + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBAPSType))
                {
                    strWhereClause += "BapsType = '" + strBAPSType + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPeriodInvoice))
                {
                    strWhereClause += "PeriodInvoice = '" + strPeriodInvoice + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strInvoiceType))
                {
                    strWhereClause += "InvoiceTypeId = '" + strInvoiceType + "' AND ";
                }

                if (!string.IsNullOrWhiteSpace(strSoNumber))
                {
                    strWhereClause += "SONumber LIKE '%" + strSoNumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPONumber))
                {
                    strWhereClause += "PoNumber LIKE '%" + strPONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBAPSNumber))
                {
                    strWhereClause += "BapsNo LIKE '%" + strBAPSNumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteIdOld))
                {
                    strWhereClause += "SiteIdOld LIKE '%" + strSiteIdOld + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "CONVERT(VARCHAR, StartDateInvoice, 106) LIKE '%" + strStartPeriod.Replace('-', ' ') + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "CONVERT(VARCHAR, EndDateInvoice, 106) LIKE '%" + strEndPeriod.Replace('-', ' ') + "%' AND ";
                    //strWhereClause += "EndDateInvoice <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }


                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                if (string.IsNullOrWhiteSpace(strOrderBy))
                {
                    strOrderBy = "CreatedDate DESC";
                }
                if (intPageSize > 0)
                    listDataBAPSConfirm = TrxDataBapsConfirmRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    listDataBAPSConfirm = TrxDataBapsConfirmRepo.GetList(strWhereClause, strOrderBy);

                return listDataBAPSConfirm;
            }
            catch (Exception ex)
            {
                listDataBAPSConfirm.Add(new vwDataBAPSConfirm((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBAPSDataService", "GetTrxBapsConfirmNewFlowToList", UserID)));
                return listDataBAPSConfirm;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetTrxBapsConfirmNewFlowCount(string UserID, string strCompanyId, string strOperator, string strBapsType, string strPeriodInvoice, string strInvoiceType, string strSoNumber, string strPONumber, string strBAPSNumber, string strSiteIdOld, string strStartPeriod, string strEndPeriod, int isFreeze)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxDataBapsConfirmRepo = new vwDataBAPSConfirmRepository(context);
            List<vwDataBAPSConfirm> listDataBAPSConfirm = new List<vwDataBAPSConfirm>();
            var mstInvoiceStatusRepo = new mstInvoiceStatusRepository(context);
            try
            {
                string strWhereClause = "";
                if (isFreeze == 1)
                    strWhereClause = "mstInvoiceStatusId = " + mstInvoiceStatusRepo.GetList("Description = 'FREEZE'").FirstOrDefault().mstInvoiceStatusId + " AND "; //TAB Freeze 
                else
                    strWhereClause = "mstInvoiceStatusId = " + mstInvoiceStatusRepo.GetList("Description = 'PO CONFIRM'").FirstOrDefault().mstInvoiceStatusId + " AND "; //TAB PO Confirm

                if (!string.IsNullOrWhiteSpace(strCompanyId))
                {
                    strWhereClause += "CompanyId = '" + strCompanyId + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += "Operator = '" + strOperator + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBapsType))
                {
                    strWhereClause += "BapsType = '" + strBapsType + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPeriodInvoice))
                {
                    strWhereClause += "PeriodInvoice = '" + strPeriodInvoice + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strInvoiceType))
                {
                    strWhereClause += "InvoiceTypeId = '" + strInvoiceType + "' AND ";
                }

                if (!string.IsNullOrWhiteSpace(strSoNumber))
                {
                    strWhereClause += "SONumber LIKE '%" + strSoNumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strPONumber))
                {
                    strWhereClause += "PoNumber LIKE '%" + strPONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strBAPSNumber))
                {
                    strWhereClause += "BapsNo LIKE '%" + strBAPSNumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strSiteIdOld))
                {
                    strWhereClause += "SiteIdOld LIKE '%" + strSiteIdOld + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strStartPeriod))
                {
                    strWhereClause += "CONVERT(VARCHAR, StartDateInvoice, 106) LIKE '%" + strStartPeriod.Replace('-', ' ') + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strEndPeriod))
                {
                    strWhereClause += "CONVERT(VARCHAR, EndDateInvoice, 106) LIKE '%" + strEndPeriod.Replace('-', ' ') + "%' AND ";
                    //strWhereClause += "EndDateInvoice <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return TrxDataBapsConfirmRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxBAPSDataService", "GetTrxBapsConfirmNewFlowCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public trxBapsData ValidateModalReject(string UserID, List<int> id, string isPOConfirm)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));

            try
            {
                List<trxBapsData> ListTrxBapsData = new List<trxBapsData>();
                ListTrxBapsData = GetTrxBapsListDataToList(UserID, id);
                //Validation
                if (ListTrxBapsData.Count(n => n.Operator.Trim().ToUpper() == "HCPT" && n.PeriodInvoice.Trim().ToUpper() == "NEW" && n.BapsType.Trim().ToUpper() == "TOWER") > 0)
                {
                    string validateReject = "";
                    foreach (trxBapsData var in ListTrxBapsData)
                    {
                        if (var.Operator.Trim().ToUpper() != "HCPT" && var.PeriodInvoice.Trim().ToUpper() != "NEW" && var.BapsType.Trim().ToUpper() != "TOWER")
                        {
                            validateReject = "Operator HCPT New , Type BAPS Tower can't Merge Reject with another Operator";
                            break;
                        }

                    }
                    if (validateReject == "")
                        return new trxBapsData((int)Helper.ErrorType.Validation, "HCPTNewTower");
                    else
                        return new trxBapsData((int)Helper.ErrorType.Validation, validateReject);
                }
                else
                    return new trxBapsData();

            }
            catch (Exception ex)
            {
                return new trxBapsData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBAPSDataService", "ValidateModalReject", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion
    }
}
