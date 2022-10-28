using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System.Data;

namespace ARSystem.Service
{
    public class ElectricityService
    {
        public List<vwElectricityData> GetDataToList(string UserID, string strWhereClause, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxBAPSDataRepo = new vwElectricityDataRepository(context);
            List<vwElectricityData> listBAPSData = new List<vwElectricityData>();

            try
            {

                //set order by
                if (string.IsNullOrWhiteSpace(strOrderBy))
                {
                    strOrderBy = "CreatedDate DESC";

                }

                if (intPageSize > 0)
                    listBAPSData = TrxBAPSDataRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    listBAPSData = TrxBAPSDataRepo.GetList(strWhereClause, strOrderBy);

                return listBAPSData;
            }
            catch (Exception ex)
            {
                listBAPSData.Add(new vwElectricityData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ElectricityService", "GetDataToList", UserID)));
                return listBAPSData;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetDataCount(string UserID, string strWhereClause = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxBAPSDataRepo = new vwElectricityDataRepository(context);
            List<vwElectricityData> listBAPSData = new List<vwElectricityData>();

            try
            {
                return TrxBAPSDataRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "ElectricityService", "GetDataCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<int> GetElectricityDataListId(string UserID, string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var CustomRepo = new GetListIdRepository(context);
            List<int> ListId = new List<int>();

            try
            {
                ListId = CustomRepo.GetListId("TrxBapsDataId", "vwElectricityData", strWhereClause);
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

        public List<vwElectricityDataReject> GetTrxElectricityRejectToList(string UserID, string strWhereClause, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxBAPSRejectRepo = new vwElectricityDataRejectRepository(context);
            List<vwElectricityDataReject> listBAPSData = new List<vwElectricityDataReject>();

            try
            {
                
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
                listBAPSData.Add(new vwElectricityDataReject((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ElectricityService", "GetTrxElectricityRejectToList", UserID)));
                return listBAPSData;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetTrxElectricityRejectCount(string UserID, string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxBAPSRejectRepo = new vwElectricityDataRejectRepository(context);
            List<vwElectricityDataReject> listBAPSData = new List<vwElectricityDataReject>();

            try
            {
                return TrxBAPSRejectRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "ElectricityService", "GetTrxElectricityRejectCount", UserID);
                return 0;
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
                string strWhereClause = "IsActive = '" + 1 + "'AND mstUserGroupId = " + (int)Constants.UserGroup.Electricity;

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

        public List<vwElectricityData> GetTrxElectricityListData(string UserID, List<int> id)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var TrxDataBapsRepo = new vwElectricityDataRepository(context);
            List<vwElectricityData> listDataBAPS = new List<vwElectricityData>();


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
                listDataBAPS.Add(new vwElectricityData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ElectricityService", "GetTrxElectricityListData", UserID)));
                return listDataBAPS;
            }
            finally
            {
                context.Dispose();
            }
        }

        public vwElectricityData EmailDataReject(string UserID, List<vwElectricityData> data, string RejectType, string RejectRemarks,string Recipient, string CC)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var emailRepo = new mstEmailRepository(context);
            var notifRepo = new NotificationRepository(context);
            var customRepo = new GetListIdRepository(context);

            List<Notification> notif = new List<Notification>();

            try
            {
                var EmailTemplate = emailRepo.GetList("EmailName = 'Reject Electricity'", "").FirstOrDefault().Body;

                var ListUserReject = customRepo.GetListString("UserID", "vwElectricityUsers", " Dept = 'AQA' ");

                foreach(var row in data)
                {
                    //Loop Email Reject
                    string BodyMail = EmailTemplate;

                    string BapsPeriods = row.StartDateInvoice == null ? "-" : FormattedDateToString((DateTime)row.StartDateInvoice, "BapsPeriod");

                    BodyMail = BodyMail.Replace("#SoNumber", row.SONumber);
                    BodyMail = BodyMail.Replace("#ExpenseNumber", row.PoNumber);
                    BodyMail = BodyMail.Replace("#SiteName", row.SiteName);
                    BodyMail = BodyMail.Replace("#Operator", row.CustomerName);
                    BodyMail = BodyMail.Replace("#DescriptionTagihan", row.ExpDescription);
                    BodyMail = BodyMail.Replace("#AmountInvoice", row.AmountString);
                    BodyMail = BodyMail.Replace("#PicaType", RejectType);
                    BodyMail = BodyMail.Replace("#Remarks", RejectRemarks);
                    BodyMail = BodyMail.Replace("#DateRejected", FormattedDateToString((DateTime)row.CreatedDate, "RejectDate"));
                    BodyMail = BodyMail.Replace("#BapsPeriod", BapsPeriods);

                    foreach(var user in ListUserReject)
                    {
                        notif.Add(new Notification
                        {
                            Code = "Electricity Reject",
                            Date = DateTime.Now,
                            Details = row.SONumber + " ( " + row.BapsPeriod + " ) ",
                            DetailsURL = @"""../../../InvoiceTransaction/TrxElectricityConfirm?Tab=tabBAPSReject&Bind=Reject&SoNumber=" + row.SONumber + @"""",
                            NotificationType = "Danger",
                            Title = "Trx Electricity Reject",
                            Type = 1,
                            SentTo = user
                        });
                    }
                    

                    EmailHelper.SendEmail(Recipient, CC, "information@internal.m-tbig.com", "Electricity Transaction Reject " + row.SONumber, BodyMail, "information@internal.m-tbig.com", UserID);
                }

                notifRepo.CreateBulky(notif);

                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new vwElectricityData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ElectricityService", "RejectTrxElectricityData", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public Notification SendTask(string UserID, List<vwElectricityData> data, int isReceive)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var notifRepo = new NotificationRepository(context);
            var customRepo = new GetListIdRepository(context);

            List<Notification> notif = new List<Notification>();

            try
            {
                var ListUser = customRepo.GetListString("UserID", "vwElectricityUsers", " Dept = 'ARD' ");

                string DetailURL = "";

                if (isReceive == 1)
                    DetailURL = @"""../../../InvoiceTransaction/TrxElectricityConfirm?Tab=tabBAPSConfirm&Bind=Confirm&SoNumber=";
                else
                    DetailURL = @"""../../../InvoiceTransaction/TrxCreateInvoiceTower?&SoNumber=";

                foreach (var row in data)
                {
                    notif.Add(new Notification
                    {
                        Code = "Electricity Task",
                        Date = DateTime.Now,
                        Details = row.SONumber + " ( " + row.BapsPeriod + " ) ",
                        DetailsURL = DetailURL + row.SONumber + @"""",
                        NotificationType = "Warning",
                        Title = isReceive == 1 ? "Electricity Confirm Task" : "Create Invoice Task",
                        Type = 2,
                        SentTo = string.Join(",", ListUser)
                    });
                }

                return notifRepo.CreateBulky(notif).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new Notification((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ElectricityService", "RejectTrxElectricityData", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        private string FormattedDateToString(DateTime dates,string Type)
        {
            string result = "";

            string[] monthstr = new string[] { "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };

            if (Type == "BapsPeriod")
            {
                result = monthstr[dates.Month - 1] + " " + dates.Year;
            }

            if (Type == "RejectDate")
            {
                result = dates.Day + " " + monthstr[dates.Month - 1] + " " + dates.Year;
            }

            return result;
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

                foreach (trxBapsData BAPSData in ListBAPSData)
                {
                    BAPSData.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateBAPSReceive;//RECEIVED, NOT SHOW IN RECEIVE TABLE
                    BAPSData.UpdatedBy = UserID;
                    BAPSData.UpdatedDate = Helper.GetDateTimeNow();
                    BAPSDataRepo.Update(BAPSData);


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
                return new vwBAPSData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ElectricityService", "ReceiveBAPSData", UserID));
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
                listDataBAPS.Add(new trxBapsData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ElectricityService", "GetTrxBapsListDataToList", UserID)));
                return listDataBAPS;
            }
            finally
            {
                context.Dispose();
            }
        }

        public trxBapsData ConfirmBAPS(string UserID, List<int> id)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();

            var TrxArHeaderRepo = new trxArHeaderRepository(context);
            var TrxArDetailRepo = new trxArDetailRepository(context);
            var TrxArActivityLogRepo = new trxArActivityLogRepository(context);
            var mstBapsPowerTypeRepo = new mstBapsPowerTypeRepository(context);
            var TrxBapsDataRepo = new trxBapsDataRepository(context);

            var ARHeader = new trxArHeader();
            var ARDetail = new trxArDetail();
            var ARActivityLog = new trxArActivityLog();

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

                foreach (trxBapsData TrxBAPSData in ListTrxBapsData)
                {
                    ARDetail = new trxArDetail();
                    ARDetail.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.NotProcessed;
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
                    ARDetail = TrxArDetailRepo.Create(ARDetail);

                    trxBapsData BapsData = new trxBapsData();
                    BapsData = TrxBapsDataRepo.GetByPK(TrxBAPSData.trxBapsDataId);

                    BapsData.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.StateBAPSConfirm; //CONFIRMED NOT SHOWN IN BAPS CONFIRM MENU
                    BapsData.UpdatedBy = UserID;
                    BapsData.UpdatedDate = Helper.GetDateTimeNow();
                    TrxBapsDataRepo.Update(BapsData);

                    //INSERT INTO LOGARACTIVITY
                    logArActivity logAr = new logArActivity();
                    logAr.mstInvoiceStatusId = (int)StatusHelper.InvoiceStatus.BAPSConfirm;
                    logAr.LogWeek = LogHelper.GetLogWeek(Helper.GetDateTimeNow());
                    logAr.trxArDetailId = ARDetail.trxArDetailId;
                    logAr.trxBapsDataId = TrxBAPSData.trxBapsDataId;
                    logAr.CreatedBy = UserID;
                    logAr.CreatedDate = Helper.GetDateTimeNow();
                    listLogAr.Add(logAr);
                }

                LogArRepo.CreateBulky(listLogAr);
                #endregion

                uow.SaveChanges();
                
                return new trxBapsData();

            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxBapsData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ElectricityService", "ConfirmTrxBAPSData", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
