using System;
using System.Web.Mvc;
using System.Web;
using ARSystemFrontEnd.Providers;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using ARSystemFrontEnd.Models;
using System.Linq;
using FastMember;
using ARSystemFrontEnd.Helper;
using ARSystem.Domain.Models;
using System.Threading.Tasks;
using ARSystem.Service;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("DashboardRA")]
    public class DashboardRAController : BaseController
    {
        // GET: DashboardRA
        #region Dashboard
        [Authorize]
        [Route("MonitoringBAPSDone")]
        public ActionResult MonitoringBAPSDone()
        {
            string actionTokenView = "394B2F4D-3A08-4EEB-AEAB-4F4616EC3157";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("MasterPICA")]
        public ActionResult MasterPICA()
        {
            string actionTokenView = "01951206-2C8D-4D6C-BB43-736BDBF29D71";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("MasterEmailNotification")]
        public ActionResult MasterEmailNotification()
        {
            string actionTokenView = "01951206-2C8D-4D6C-BB41-736BDBF29D71";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("MasterEmail")]
        public ActionResult MasterEmail()
        {
            string actionTokenView = "01951206-2C8D-4D6C-BB40-736BDBF29D71";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("DashboardXL")]
        public ActionResult DashboardXL()
        {
            string actionTokenView = "EB2DA255-D683-46EA-8A8F-E8AD889D1DE8";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("BapsReport")]
        public ActionResult BapsReport()
        {
            string actionTokenView = "01951206-2C8D-4D6C-BB43-736BDBF29D71";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }


        [Route("MonitoringBapsDoneHeader/Export")]
        public async Task MonitoringBapsDoneHeaderExport()
        {
            //Parameter
            string groupBy = Request.QueryString["groupBy"];
            string bapsType = Request.QueryString["bapsType"];
            string customerID = Request.QueryString["customerID"];
            string companyID = Request.QueryString["companyID"];
            int year = int.Parse(Request.QueryString["year"]);
            int stipID = int.Parse(Request.QueryString["stipID"] == "" ? "0" : Request.QueryString["stipID"]);
            int regionID = int.Parse(Request.QueryString["regionID"] == "" ? "0" : Request.QueryString["regionID"]);
            int provinceID = int.Parse(Request.QueryString["provinceID"] == "" ? "0" : Request.QueryString["provinceID"]);
            int productID = int.Parse(Request.QueryString["productID"] == "" ? "0" : Request.QueryString["productID"]);
            string powerTypeID = Request.QueryString["powerTypeID"];
            string DataType = Request.QueryString["Datatype"];

            MonitoringBapsDoneHeaderParam param = new MonitoringBapsDoneHeaderParam();
            param.BapsType = bapsType;
            param.GroupBy = groupBy;
            param.CustomerID = customerID;
            param.CompanyId = companyID;
            param.Year = year;
            param.StipID = stipID;
            param.RegionID = regionID;
            param.ProvinceID = provinceID;
            param.ProductID = productID;
            param.PowerTypeID = powerTypeID;
            param.DataType = DataType;

            List<vwMonitoringBapsDoneHeader> headerList = new List<vwMonitoringBapsDoneHeader>();
            var client = new RADashboardMonitoringBAPSDoneService();

            headerList = await client.GetMonitoringBapsDoneHeaderList(UserManager.User.UserToken, param, 0, 99999);

            //var data = new List<vwMonitoringBapsDoneHeader>();
            //foreach (var item in headerList)
            //{
            //    data.Add(new vwMonitoringBapsDoneHeader
            //    {
            //        RowIndex = item.RowIndex,
            //        Descrip = item.Descrip.Replace("<b>", "").Replace("</b>", ""),
            //        Jan = item.Jan.Replace("<b>", "").Replace("</b>", ""),
            //        Feb = item.Feb.Replace("<b>", "").Replace("</b>", ""),
            //        Mar = item.Mar.Replace("<b>", "").Replace("</b>", ""),
            //        Apr = item.Apr.Replace("<b>", "").Replace("</b>", ""),
            //        Mei = item.Mei.Replace("<b>", "").Replace("</b>", ""),
            //        Jun = item.Jun.Replace("<b>", "").Replace("</b>", ""),
            //        Jul = item.Jul.Replace("<b>", "").Replace("</b>", ""),
            //        Agu = item.Agu.Replace("<b>", "").Replace("</b>", ""),
            //        Sep = item.Sep.Replace("<b>", "").Replace("</b>", ""),
            //        Okt = item.Okt.Replace("<b>", "").Replace("</b>", ""),
            //        Nov = item.Nov.Replace("<b>", "").Replace("</b>", ""),
            //        Des = item.Des.Replace("<b>", "").Replace("</b>", ""),
            //        TotalSite = item.TotalSite.Replace("<b>", "").Replace("</b>", ""),
            //    });
            //}
            //headerList = new List<vwMonitoringBapsDoneHeader>();
            //headerList = data.ToList();


            string[] fieldList = new string[] {
                "RowIndex",
                "Descrip",
                "Jan",
                "Feb",
                "Mar",
                "Apr",
                "Mei",
                "Jun",
                "Jul",
                "Agu",
                "Sep",
                "Okt",
                "Nov",
                "Des",
                "TotalSite"
            };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(headerList.Select(i => new
            {
                i.RowIndex,
                i.Descrip,
                i.Jan,
                i.Feb,
                i.Mar,
                i.Apr,
                i.Mei,
                i.Jun,
                i.Jul,
                i.Agu,
                i.Sep,
                i.Okt,
                i.Nov,
                i.Des,
                i.TotalSite
            }), fieldList);
            table.Load(reader);


            //Export to Excel
            ExportToExcelHelper.Export("MonitoringBapsDone", table);
        }
        [Route("MonitoringBapsDoneDetail/Export")]
        public async Task MonitoringBapsDoneDetailExport()
        {
            //Parameter
            string groupBy = Request.QueryString["groupBy"];
            string bapsType = Request.QueryString["bapsType"];
            string customerID = Request.QueryString["customerID"];
            string companyID = Request.QueryString["companyID"];
            int year = int.Parse(Request.QueryString["year"]);
            int stipID = int.Parse(Request.QueryString["stipID"] == "" ? "0" : Request.QueryString["stipID"]);
            int month = int.Parse(Request.QueryString["month"] == "" ? "0" : Request.QueryString["month"]);
            int regionID = int.Parse(Request.QueryString["regionID"] == "" ? "0" : Request.QueryString["regionID"]);
            int provinceID = int.Parse(Request.QueryString["provinceID"] == "" ? "0" : Request.QueryString["provinceID"]);
            int productID = int.Parse(Request.QueryString["productID"] == "" ? "0" : Request.QueryString["productID"]);
            string descID = Request.QueryString["descID"];
            string powerTypeID = Request.QueryString["powerTypeID"];
            
            List<vwMonitoringBapsDoneDetail> detailList = new List<vwMonitoringBapsDoneDetail>();
            var client = new RADashboardMonitoringBAPSDoneService();

            vwMonitoringBapsDoneDetail data = new vwMonitoringBapsDoneDetail();
            data.ProductID = productID;
            data.STIPID = stipID;
            data.Year = year;
            data.Month = month;
            data.PowerTypeID = int.Parse(powerTypeID == "" ? "0" : powerTypeID);
            data.CustomerId = customerID;
            data.CompanyInvoiceId = companyID;
            data.RegionID = regionID;
            data.ProvinceID = provinceID;


            if (descID == "Total")
            {
                data.CustomerId = null;
                data.CompanyInvoiceId = null;
                data.RegionID = null;
                data.ProvinceID = null;
            }
            else
            {
                if (groupBy == "customer")
                    data.CustomerId = descID;
                else if (groupBy == "company")
                    data.CompanyInvoiceId = descID;
                else if (groupBy == "region")
                    data.RegionID = int.Parse(descID == "" ? "0" : descID);
                else if (groupBy == "province")
                    data.ProvinceID = int.Parse(descID == "" ? "0" : descID);
            }

            if (month == 13)
            {
                data.Month = null;
            }
            //Added by ASE
            data.SoNumber = Request.QueryString["schSONumber"];
            data.SiteID = Request.QueryString["schSiteID"];
            data.SiteName = Request.QueryString["schSiteName"];
            data.CustomerSiteID = Request.QueryString["schSiteCustID"];
            data.CustomerSiteName = Request.QueryString["schSiteCustName"];
            //end

            detailList = await client.GetMonitoringBAPSDoneDetailList(UserManager.User.UserToken, data, groupBy, bapsType, 0, 999999);



            string[] fieldList = new string[] {
                 "RowIndex",
                "SoNumber",
                "SiteID",
                "SiteName",
                "CustomerSiteID",
                "CustomerSiteName",
                "TenantType",
                "StipSiro",
                "StipCategory",
                "CompanyName",
                "CustomerId",
                "RegionName",
                "ProvinceName",
                "RFIDate",
                "BAUKDone",
                "PODone",
                "PONumber", //Add by ASE
                "BapsDate",
                "BapsDoneDate",
                "BapsAmount",
                "BAPSNumber",
                //Added ASE
                "StartLeasedDate",
                "EndLeasedDate",
                "StartInvoiceDate",
                "EndInvoiceDate",
                "BAPSReceiveDate",
                "BAPSConfirmDate",
                "CreateInvoiceDate",
                "PostingInvoiceDate",
                "InvoiceNumber",
            };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(detailList.Select(i => new
            {
                i.RowIndex,
                i.SoNumber,
                i.SiteID,
                i.SiteName,
                i.CustomerSiteID,
                i.CustomerSiteName,
                i.TenantType,
                i.StipSiro,
                i.StipCategory,
                i.CompanyName,
                i.CustomerId,
                i.RegionName,
                i.ProvinceName,
                i.ResidenceName,
                i.RFIDate,
                i.BAUKDone,
                i.PODone,
                i.PONumber, //Add by ASE
                i.BapsDate,
                i.BapsDoneDate,
                i.BapsAmount,
                i.BAPSNumber,
                //Added by ASE
                i.StartLeasedDate,
                i.EndLeasedDate,
                i.StartInvoiceDate,
                i.EndInvoiceDate,
                i.BAPSReceiveDate,
                i.BAPSConfirmDate,
                i.CreateInvoiceDate,
                i.PostingInvoiceDate,
                i.InvoiceNumber

            }), fieldList);
            table.Load(reader);


            //Export to Excel
            ExportToExcelHelper.Export("MonitoringBapsDone", table);
        }

        [Route("ExportToExcelPICA")]
        public void ExportToExcelPICA()
        {

            List<ARSystemService.TrxRANewPICA> picadata = new List<ARSystemService.TrxRANewPICA>();
            //Call Service
            using (var client = new ARSystemService.RADashboardServiceClient())
            {

                var data = client.GetTrxRANewPICAToList(UserManager.User.UserToken).ToList();
                picadata.AddRange(data);

                string[] fieldList = new string[] {
                        "ID","SONumber","SiteID","SiteName","CustomerSiteID","CustomerSiteName","Process",
                        "TypePICA","CategoryPICA","Description"
                    };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(picadata, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("Data PICA", table);
            }
        }

        #endregion

        [Authorize]
        [Route("OverdueRTI")]
        public ActionResult OverdueRTI()
        {
            string actionTokenView = "01951206-2C8D-4D6C-BB43-736BDBF29D71";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                    if (userCredential.ErrorType > 0)
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    var userGroupService = new MstRAUserGroupService();
                    ViewBag.UserGroup = userGroupService.GetUserGroup(userCredential.UserID);

                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Route("OverdueRTI/Export")]
        public async Task OverdueRTIExpost()
        {
            //Parameter
            string dataType = Request.QueryString["DataType"];
            string strmonth = Request.QueryString["Month"];
            int year = int.Parse(Request.QueryString["Year"]);
            string customerId = Request.QueryString["Customer"];

            int month = 0;
            switch (strmonth)
            {
                case "JAN":
                    month = 1;
                    break;
                case "FEB":
                    month = 2;
                    break;
                case "MAR":
                    month = 3;
                    break;
                case "APR":
                    month = 4;
                    break;
                case "MAY":
                    month = 5;
                    break;
                case "JUN":
                    month = 6;
                    break;
                case "JUL":
                    month = 7;
                    break;
                case "AUG":
                    month = 8;
                    break;
                case "SEP":
                    month = 9;
                    break;
                case "OCT":
                    month = 10;
                    break;
                case "NOV":
                    month = 11;
                    break;
                case "DEC":
                    month = 12;
                    break;
            }

            RTIDoneNOverdueService service = new RTIDoneNOverdueService();
            List<RTINOverdueDetailModel> dataList = new List<RTINOverdueDetailModel>();
            dataList = await service.GetDataDetail(dataType, year, month, customerId, 0, 0, "");

            dataList = dataList.OrderBy(x => x.DataType).ToList();
            string[] fieldList = new string[] {
                "RowIndex",
                "SoNumber",
                "CustomerId",
                "CompanyId",
                "CustomerSiteID",
                "CustomerSiteName",
                "SiteId",
                "SiteName",
                "StartBapsDate",
                "EndBapsDate",
                "StartInvoiceDate",
                "EndInvoiceDate",
                "InvoiceAmount",
                "Currency",
                "CurrentStatus",
                "DataType"
            };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataList.Select(i => new
            {
                i.RowIndex,
                i.SoNumber,
                i.CustomerId,
                i.CompanyId,
                i.CustomerSiteID,
                i.CustomerSiteName,
                i.SiteId,
                i.SiteName,
                i.StartBapsDate,
                i.EndBapsDate,
                i.StartInvoiceDate,
                i.EndInvoiceDate,
                i.InvoiceAmount,
                i.Currency,
                i.CurrentStatus,
                i.DataType
            }), fieldList);
            table.Load(reader);


            //Export to Excel
            ExportToExcelHelper.Export("MonitoringRTIvsOverdue", table);
        }

        [Authorize]
        [Route("FulfilmentRTI")]
        public ActionResult FulfilmentRTI()
        {
            string actionTokenView = "2C0A46A0-1C3C-4ADF-8221-EA5EADD4C148";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                    if (userCredential.ErrorType > 0)
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    var userGroupService = new MstRAUserGroupService();
                    ViewBag.UserGroup = userGroupService.GetUserGroup(userCredential.UserID);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("MasterAmountTarget")]
        public ActionResult AmountTarget()
        {
            string actionTokenView = "2C0A46A0-1C3C-4ADF-8221-EA5EADD4C148";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //string UserRole = "";
                    //using (var client2 = new ARSystemService.UserServiceClient())
                    //{
                    //    UserRole = client2.GetARUserPosition(UserManager.User.UserToken).Result;
                    //}

                    ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                    using (var client2 = new ARSystemService.UserServiceClient())
                    {
                        Result = client2.GetARUserPosition(UserManager.User.UserToken);
                        ViewBag.UserRole = Result.Result.ToString().ToUpper();
                        //if (Result.Result.ToString().ToUpper() == "DEPT HEAD")
                        //{
                        //    ViewBag.SummaryEdit = true;
                        //    ViewBag.Request = false;
                        //}
                        //else
                        //{
                        //    ViewBag.SummaryEdit = false;
                        //    ViewBag.Request = true;
                        //}
                    }


                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                   
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        private int convertMonth(string strmonth)
        {
            int month = 0;
            switch (strmonth)
            {
                case "JAN":
                    month = 1;
                    break;
                case "FEB":
                    month = 2;
                    break;
                case "MAR":
                    month = 3;
                    break;
                case "APR":
                    month = 4;
                    break;
                case "MAY":
                    month = 5;
                    break;
                case "JUN":
                    month = 6;
                    break;
                case "JUL":
                    month = 7;
                    break;
                case "AUG":
                    month = 8;
                    break;
                case "SEP":
                    month = 9;
                    break;
                case "OCT":
                    month = 10;
                    break;
                case "NOV":
                    month = 11;
                    break;
                case "DEC":
                    month = 12;
                    break;
            }
            return month;
        }

        [Route("FulfilmentRTI/Export")]
        public async Task FulfilmentRTIExpost()
        {
            //Parameter
            string customer = Request.QueryString["CustomerID"];
            int Year = int.Parse(Request.QueryString["Year"]);
            string month = Request.QueryString["Month"];
            string department = Request.QueryString["DepartmentCode"];
            string exportType = Request.QueryString["ExportType"];

            int monthInt = (exportType == "All") ? 0 : convertMonth(month);
            //string vWhereClause = " 1=1 ";
            var listCust = (customer != "") ? customer.Split(',').ToList() : null;
            var listDept = (department != "") ? department.Split(',').ToList() : null;
            //if (listCust != null && listCust.Count() > 0)
            //{
            //    var customerIds = string.Join(", ", listCust.Select(cust => "'" + cust + "'"));
            //    if (!listCust.Any(cust => cust == "ALL"))
            //    {
            //        vWhereClause += " AND CustomerID in (" + customerIds + ")";
            //    }
            //}
            //if (listDept != null && listDept.Count() > 0)
            //{
            //    var deptCodes = string.Join(", ", listDept.Select(dept => "'" + dept + "'"));
            //    vWhereClause += " AND DepartmentCode in (" + deptCodes + ")";
            //}

            RTIAchievementService service = new RTIAchievementService();
            List<RTINOverdueDetailModel> dataListTarget = new List<RTINOverdueDetailModel>();
            List<RTINOverdueDetailModel> dataListAchivement = new List<RTINOverdueDetailModel>();


            //Export to Excel
            //ExportToExcelHelper.Export("MonitoringFulfilmentRTI", table);
            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            if(exportType == "Target" || exportType == "All")
            {
            dataListTarget = await service.GetRTIAchivementDetailByCustomer(listCust, monthInt, Year, "Target", listDept, 0, 0, string.Empty);
            dataListTarget = dataListTarget.OrderBy(x => x.MonthTarget).ToList();
            string[] fieldListTarget = new string[] {
                "SoNumber",
                "CustomerId",
                "CompanyId",
                "CustomerSiteID",
                "CustomerSiteName",
                "SiteId",
                "SiteName",
                "MonthTarget",
                "YearTarget",
                "StartInvoiceDate",
                "EndInvoiceDate",
                "InvoiceAmount",
                "Currency",
                "TypeBaps"
            };
            DataTable tableTarget = new DataTable();
            var readerTarget = FastMember.ObjectReader.Create(dataListTarget.Select(i => new
            {
                i.SoNumber,
                i.CustomerId,
                i.CompanyId,
                i.CustomerSiteID,
                i.CustomerSiteName,
                i.SiteId,
                i.SiteName,
                i.MonthTarget,
                i.YearTarget,
                i.StartInvoiceDate,
                i.EndInvoiceDate,
                i.InvoiceAmount,
                i.Currency,
                i.TypeBaps
            }), fieldListTarget);
            tableTarget.Load(readerTarget);
            wbook.Worksheets.Add(tableTarget, "Target");
            }

            if (exportType == "Achivement" || exportType == "All")
            {
                dataListAchivement = await service.GetRTIAchivementDetailByCustomer(listCust, monthInt, Year, "Achivement", listDept, 0, 0, String.Empty);
                dataListAchivement = dataListAchivement.OrderBy(x => x.DataType).ToList();
                string[] fieldListAchivement = new string[] {
                "SoNumber",
                "CustomerId",
                "CompanyId",
                "CustomerSiteID",
                "CustomerSiteName",
                "SiteId",
                "SiteName",
                "StartBapsDate",
                "EndBapsDate",
                "StartInvoiceDate",
                "EndInvoiceDate",
                "InvoiceAmount",
                "Currency",
                "CurrentStatus",
                "RTIDate",
                "FinanceConfirmDate",
                "CreateInvoiceDate",
                "BillingCycle",
                "TypeBaps"
            };
                DataTable tableAchivement = new DataTable();
                var readerAchivement = FastMember.ObjectReader.Create(dataListAchivement.Select(i => new
                {
                    i.SoNumber,
                    i.CustomerId,
                    i.CompanyId,
                    i.CustomerSiteID,
                    i.CustomerSiteName,
                    i.SiteId,
                    i.SiteName,
                    i.StartBapsDate,
                    i.EndBapsDate,
                    i.StartInvoiceDate,
                    i.EndInvoiceDate,
                    i.InvoiceAmount,
                    i.Currency,
                    i.CurrentStatus,
                    i.RTIDate,
                    i.FinanceConfirmDate,
                    i.CreateInvoiceDate,
                    i.BillingCycle,
                    i.TypeBaps
                }), fieldListAchivement);
                tableAchivement.Load(readerAchivement);
                wbook.Worksheets.Add(tableAchivement, "Achivement");
            }

            HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=" + "MonitoringFulfilmentRTI" + ".xlsx");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();
        }


        [Authorize]
        [Route("LeadTimeRTI")]
        public ActionResult LeadTimeRTI()
        {
            string actionTokenView = "C5482E46-793F-4E74-8A3C-75E66CB334B5";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                    if (userCredential.ErrorType > 0)
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    var userGroupService = new MstRAUserGroupService();
                    ViewBag.UserGroup = userGroupService.GetUserGroup(userCredential.UserID);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        [Route("LeadTimeRTI/Export")]
        public async Task LeadTimeRTIExpost()
        {
            //Parameter
            string LeadTime = Request.QueryString["LeadTime"];
            int year = int.Parse(Request.QueryString["Year"]);
            string customerId = Request.QueryString["Customer"];



            RTILeadTimeService service = new RTILeadTimeService();
            List<RTINOverdueDetailModel> dataList = new List<RTINOverdueDetailModel>();
            dataList = await service.GetDataLeadTimeDetail(LeadTime, year, customerId, 0, 0);

            dataList = dataList.OrderBy(x => x.DataType).ToList();
            string[] fieldList = new string[] {
                "SoNumber",
                "CustomerId",
                "CompanyId",
                "CustomerSiteID",
                "CustomerSiteName",
                "SiteId",
                "SiteName",
                "StartBapsDate",
                "EndBapsDate",
                "StartInvoiceDate",
                "EndInvoiceDate",
                "InvoiceAmount",
                "Currency",
                "CurrentStatus"
            };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataList.Select(i => new
            {
                i.SoNumber,
                i.CustomerId,
                i.CompanyId,
                i.CustomerSiteID,
                i.CustomerSiteName,
                i.SiteId,
                i.SiteName,
                i.StartBapsDate,
                i.EndBapsDate,
                i.StartInvoiceDate,
                i.EndInvoiceDate,
                i.InvoiceAmount,
                i.Currency,
                i.CurrentStatus
            }), fieldList);
            table.Load(reader);


            //Export to Excel
            ExportToExcelHelper.Export("MonitoringLeadTimeRTI", table);
        }

        [Route("LeadTimeRTI/ExportByStatus")]
        public async Task LeadTimeRTIExpostByStatus()
        {
            string customerId = Request.QueryString["Customer"];
            int year = int.Parse(Request.QueryString["Year"]);
            string currentStatus = Request.QueryString["currentStatus"];




            RTILeadTimeService service = new RTILeadTimeService();
            List<RTINOverdueDetailModel> dataList = new List<RTINOverdueDetailModel>();
            dataList = await service.GetListStatusReconcileDetail(customerId, year, currentStatus, 0, 0);

            dataList = dataList.OrderBy(x => x.DataType).ToList();
            string[] fieldList = new string[] {
                "SoNumber",
                "CustomerId",
                "CompanyId",
                "CustomerSiteID",
                "CustomerSiteName",
                "SiteId",
                "SiteName",
                "StartBapsDate",
                "EndBapsDate",
                "StartInvoiceDate",
                "EndInvoiceDate",
                "InvoiceAmount",
                "Currency",
                "CurrentStatus"
            };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataList.Select(i => new
            {
                i.SoNumber,
                i.CustomerId,
                i.CompanyId,
                i.CustomerSiteID,
                i.CustomerSiteName,
                i.SiteId,
                i.SiteName,
                i.StartBapsDate,
                i.EndBapsDate,
                i.StartInvoiceDate,
                i.EndInvoiceDate,
                i.InvoiceAmount,
                i.Currency,
                i.CurrentStatus
            }), fieldList);
            table.Load(reader);


            //Export to Excel
            ExportToExcelHelper.Export("MonitoringLeadTimeRTI", table);
        }

        [Authorize]
        [Route("SAPXLIntegration")]
        public ActionResult SAPXLIntegration()
        {
            string actionTokenView = "C5482E46-793F-4E74-8A3C-75E66CB334B5";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }


        [Authorize]
        [Route("")]
        public ActionResult Index()
        {
            string actionTokenView = "C5482E46-793F-4E74-8A3C-75E66CB334B5";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                    if (userCredential.ErrorType > 0)
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    var userGroupService = new MstRAUserGroupService();
                    ViewBag.UserGroup = userGroupService.GetUserGroup(userCredential.UserID);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }


        #region Dashboard RA Potential 
        [Authorize]
        
        [Route("DashboardOutstandingPotentialRecurring")]
        public ActionResult DashboardOutstandingPotentialRecurring()
        {
            string actionTokenView = "866377ea-758a-4aa9-b920-1917da948362";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserToken = UserManager.User.UserToken;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        [Authorize]
         
        [Route("DashboardOutstandingPotentialSIRO")]
        public ActionResult DashboardOutstandingPotentialSIRO()
        {
            string actionTokenView = "8fddb243-dca8-45b0-87fb-4343639ac668";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserToken = UserManager.User.UserToken;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        [Route("DashboardOutstandingPotentialRFI")]
        public ActionResult DashboardOutstandingPotentialRFI()
        {
            string actionTokenView = "3df7e76e-f10e-45c1-a559-85c01e3e190c";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserToken = UserManager.User.UserToken;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Route("ExportDetailPotentialRecurring")]
        public void ExportDetailPotentialRecurring()
        {
            ARSystem.Service.DashboardPotentialRecurringService client = new ARSystem.Service.DashboardPotentialRecurringService();
            PostRecurring post = new PostRecurring();
            //Parameter
            post.type = Request.QueryString["strtype"].ToString();
            post.paramRow = Request.QueryString["strparamRow"].ToString();
            post.paramColumn = Request.QueryString["strparamColumn"].ToString();
            post.YearBill = Request.QueryString["strYearBill"].ToString();
            post.SoNumber = Request.QueryString["SoNumber"].ToString();
            post.SiteID = Request.QueryString["SiteID"].ToString();
            post.SiteName = Request.QueryString["SiteName"].ToString();

            if (Request.QueryString["strCompanyID"] != "")
                post.CompanyID = Request.QueryString["strCompanyID"].ToString();
            if (Request.QueryString["strRegionalID"] != "")
                post.RegionalID = Convert.ToInt32(Request.QueryString["strRegionalID"].ToString());
            if (Request.QueryString["strSTIPID"] != "")
                post.STIPID = Convert.ToInt32(Request.QueryString["strSTIPID"].ToString());
            if (Request.QueryString["strProductID"] != "")
                post.ProductID = Convert.ToInt32(Request.QueryString["strProductID"].ToString());
            if (Request.QueryString["strSOWID"] != "")
                post.SOWName = Request.QueryString["strSOWID"].ToString();
            if (Request.QueryString["strSectionID"] != "")
                post.SecName = Request.QueryString["strSectionID"].ToString();
            if (Request.QueryString["strRFIDate"] != "")
                post.RFIDate = Convert.ToInt32(Request.QueryString["strRFIDate"].ToString());
            if (Request.QueryString["strSTIPDate"] != "")
                post.STIPDate = Convert.ToInt32(Request.QueryString["strSTIPDate"].ToString());

            string strWhereClause = " 1=1 AND  DepartmentName !='Not Mapping'  AND CustomerID IS NOT NULL AND TenantType IS NOT NULL ";


            if (post.paramRow != null)
            {
                if (post.type == "GroupBySection")
                    strWhereClause += " AND DepartmentName = '" + post.paramRow.Trim() + "'";
                else if (post.type == "GroupBySOW")
                    strWhereClause += " AND TenantType = '" + post.paramRow.Trim() + "'";
                else if (post.type == "GroupByProduct")
                    strWhereClause += " AND ProductName = '" + post.paramRow + "'";
                else if (post.type == "GroupBySTIPCategory")
                    strWhereClause += " AND StipCategory = '" + post.paramRow + "'";
                else if (post.type == "GroupByRegional")
                    strWhereClause += " AND RegionName = '" + post.paramRow + "'";
                else if (post.type == "GroupByOperator")
                    strWhereClause += " AND CustomerInvoice = '" + post.paramRow + "'";
                else if (post.type == "GroupByCompany")
                    strWhereClause += " AND CustomerID = '" + post.paramRow + "'";

            }
            if (post.YearBill != null)
            {
                strWhereClause += " AND YearInvoiceCategory = '" + post.YearBill + "'";
            }
            if (post.paramColumn != null && post.paramColumn != "13" && post.paramColumn.Length != 0)
            {
                strWhereClause += " AND MONTH(StartInvoiceDate) = '" + fnGetMonthHeaderString(post.paramColumn) + "'";
            }

            if (post.STIPDate != null)
                strWhereClause += " AND YEAR(StipDate) = '" + post.STIPDate.ToString() + "'";
            if (post.RFIDate != null)
                strWhereClause += " AND YEAR(RFIDate) = '" + post.RFIDate.ToString() + "'";
            if (post.SecName != null)
                strWhereClause += " AND DepartmentCode = '" + post.SecName.ToString().Trim() + "'";
            if (post.SOWName != null)
                strWhereClause += " AND TenantType = '" + post.SOWName.ToString().Trim() + "'";
            if (post.ProductID != null)
                strWhereClause += " AND ProductID = " + post.ProductID;
            if (post.STIPID != null)
                strWhereClause += " AND STIPID = " + post.STIPID;
            if (post.RegionalID != null)
                strWhereClause += " AND RegionID = " + post.RegionalID;
            if (post.CompanyID != null)
                strWhereClause += " AND CompanyInvoice = '" + post.CompanyID + "'";
            if (post.Customer != null)
                strWhereClause += " AND CustomerID = '" + post.Customer + "'";

            if (post.SoNumber != null && post.SoNumber != "")
            {
                strWhereClause += " AND SoNumber = '" + post.SoNumber + "'";
            }

            if (post.SiteID != null && post.SiteID != "")
            {
                strWhereClause += " AND SiteID = '" + post.SiteID + "'";
            }

            if (post.SiteName != null && post.SiteName != "")
            {
                strWhereClause += " AND SiteName LIKE '%" + post.SiteName + "%'";
            }
            var dataresult = client.GetDetailList(strWhereClause, 0, 0, "");

            string[] fieldList = new string[] {

                        "SoNumber",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "CustomerID",
                        "RegionName",
                        "ProvinceName",
                        "ResidenceName",
                        "RFIDate",
                        "FirstBAPSDone",
                        "StipCategory",
                        "PONumber",
                        "MLANumber",
                        "StartBapsDate",
                        "EndBapsDate",
                        "Term",
                        "BapsType",
                        "CustomerInvoice",
                        "CompanyInvoice",
                        "Company",
                        "StipSiro",
                        "Currency",
                        "StartInvoiceDate",
                        "EndInvoiceDate",
                        "BaseLeasePrice",
                        "ServicePrice",
                        "DeductionAmount",
                        "TotalAmount"
                    };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataresult, fieldList);
            table.Load(reader);

            ExportToExcelHelper.Export("Potential Recurring Detail List", table);

        }
        [Route("ExportDetailPotentialSIRO")]
        public void ExportDetailPotentialSIRO()
        {
            ARSystem.Service.DashboardPotentialSIROService client = new ARSystem.Service.DashboardPotentialSIROService();
            PostSIRO post = new PostSIRO();
            //Parameter

            if (Request.QueryString["strtype"] != "" && Request.QueryString["strtype"] != "null")
                post.Type = Request.QueryString["strtype"].ToString();


            if (Request.QueryString["strMonth"] != "" && Request.QueryString["strMonth"] != "null")
                post.Month = Request.QueryString["strMonth"].ToString();



            if (Request.QueryString["strDesc"] != "" && Request.QueryString["strDesc"] != "null")
                post.Desc = Request.QueryString["strDesc"].ToString();



            if (Request.QueryString["strYear"] != "" && Request.QueryString["strYear"] != "null")
                post.Year = Request.QueryString["strYear"].ToString();



            if (Request.QueryString["SoNumber"] != "" && Request.QueryString["SoNumber"] != "null")
                post.SoNumber = Request.QueryString["SoNumber"].ToString();

            if (Request.QueryString["SiteID"] != "" && Request.QueryString["SiteID"] != "null")
                post.SiteID = Request.QueryString["SiteID"].ToString();


            if (Request.QueryString["SiteName"] != "" && Request.QueryString["SiteName"] != "null")
                post.SiteName = Request.QueryString["SiteName"].ToString();

            if (Request.QueryString["Step"] != "" && Request.QueryString["Step"] != "null")
                post.Step = Request.QueryString["Step"].ToString();
            if (Request.QueryString["strCompanyID"] != "")
                post.CompanyID = Request.QueryString["strCompanyID"].ToString();
            if (Request.QueryString["strSTIPID"] != "")
                post.STIPID = Request.QueryString["strSTIPID"].ToString();
            if (Request.QueryString["strProductID"] != "")
                post.ProductID = Request.QueryString["strProductID"].ToString();
            if (Request.QueryString["strCustomer"] != "")
                post.Customer = Request.QueryString["strCustomer"].ToString();



            string strWhereClause = " 1=1  AND CustomerID IS NOT NULL AND YearCategory != 'Others'";

            if (post.Desc != null)
            {
                if (post.Type == "SIROGroupByOperator")
                    strWhereClause += " AND CustomerID = '" + post.Desc.Trim() + "'";
                else if (post.Type == "SIROGroupByProduct")
                    strWhereClause += " AND ProductName  = '" + post.Desc.Trim() + "'";
                else if (post.Type == "SIROGroupByStipCategory")
                    strWhereClause += " AND StipCategory  = '" + post.Desc + "'";
                else if (post.Type == "SIROGroupByRegional")
                    strWhereClause += " AND RegionName  = '" + post.Desc + "'";
                else if (post.Type == "SIROGroupByCompany")
                    strWhereClause += " AND  CompanyInvoice = '" + post.Desc + "'";

            }

            if (post.Year != null)
            {
                strWhereClause += " AND YearCategory = '" + post.Year + "'";
            }
            if (post.Month != null && post.Month != "13" && post.Step == null)
            {
                strWhereClause += " AND MONTH(EndBapsDate) = '" + fnGetMonthHeaderString(post.Month) + "'";
            }


            if (post.ProductID != null)
                strWhereClause += " AND ProductID = " + post.ProductID;
            if (post.STIPID != null)
                strWhereClause += " AND STIPID = " + post.STIPID;
            if (post.Customer != null)
                strWhereClause += " AND CustomerID = '" + post.Customer + "' "; ;
            if (post.CompanyID != null)
                strWhereClause += " AND Company = '" + post.CompanyID + "'";
            if (post.SoNumber != null && post.SoNumber != "")
            {
                strWhereClause += " AND SoNumber = '" + post.SoNumber + "'";
            }

            if (post.SiteID != null && post.SiteID != "")
            {
                strWhereClause += " AND SiteID = '" + post.SiteID + "'";
            }

            if (post.SiteName != null && post.SiteName != "")
            {
                strWhereClause += " AND SiteName LIKE '%" + post.SiteName + "%'";
            }
            if (post.Step != null)
            {
                strWhereClause += " AND SIROProgress ='" + post.Step + "' ";
            }

            var dataresult = client.GetDetailList(strWhereClause, 0, 0, "");

            string[] fieldList = new string[] {

                        "SoNumber",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "CustomerID",
                        "RegionName",
                        "ProvinceName",
                        "ResidenceName",
                        "RFIDate",
                        "FirstBAPSDone",
                        "StipCategory",
                        "PONumber",
                        "MLANumber",
                        "StartBapsDate",
                        "EndBapsDate",
                        "Term",
                        "BapsType",
                        "CustomerInvoice",
                        "CompanyInvoice",
                        "Company",
                        "StipSiro",
                        "Currency",
                        "StartInvoiceDate",
                        "EndInvoiceDate",
                        "BaseLeasePrice",
                        "ServicePrice",
                        "DeductionAmount",
                        "TotalAmount"
                    };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataresult, fieldList);
            table.Load(reader);

            ExportToExcelHelper.Export("Potential SIRO Detail List", table);

        }
        [Route("ExportDetailPotentialRFI")]
        public void ExportDetailPotentialRFI()
        {
            ARSystem.Service.DashboardPotentialRFITo1stBAPSBillingService client = new ARSystem.Service.DashboardPotentialRFITo1stBAPSBillingService();
            ARSystemFrontEnd.Models.PostDashboardPotential post = new ARSystemFrontEnd.Models.PostDashboardPotential();
            //Parameter

            string strWhereClause = " 1=1 ";
            post.Type = Request.QueryString["strType"].ToString();
            post.STIPCategory = Request.QueryString["strSTIPCategory"].ToString();
            post.RFIDateYear = Request.QueryString["strRFIDateYear"].ToString();
            post.RFIDateMonth = Request.QueryString["RFIDateMonth"].ToString();
            post.CustomerID = Request.QueryString["CustomerID"].ToString();
            post.Step = Request.QueryString["Step"].ToString();
            post.RFIDateWeek = Request.QueryString["Week"].ToString();
            post.SoNumber = Request.QueryString["SoNumber"].ToString();
            post.SiteID = Request.QueryString["SiteID"].ToString();
            post.SiteName = Request.QueryString["SiteName"].ToString();



            if (post.RFIDateYear != null)
            {
                strWhereClause += " AND YEAR(RFIDate) = '" + post.RFIDateYear + "'";
            }
            if (post.STIPCategory != null)
            {
                strWhereClause += " AND STIPCategory = '" + post.STIPCategory + "'";
            }
            if (post.SoNumber != null && post.SoNumber != "")
            {
                strWhereClause += " AND SoNumber = '" + post.SoNumber + "'";
            }

            if (post.SiteID != null && post.SiteID != "")
            {
                strWhereClause += " AND SiteName LIKE '%" + post.SiteName + "%'";
            }

            if (post.SiteName != null && post.SiteName != "")
            {
                strWhereClause += " AND SiteName = '" + post.SiteName + "'";
            }



            if (post.Type == "A")
                strWhereClause += " AND POStep IN ('PO DONE', 'PO PROCESS')";
            else if (post.Type == "B")
                strWhereClause += " AND DashboardStep IN ('RFI', 'CME', 'SITAC' ,'BAUK SUBMIT' )";
            else if (post.Type == "C")
                strWhereClause += " AND DashboardStep IN ('BAUK REVIEW', 'BAUK DONE','CASH IN First BAPS BILLING', 'RTI', 'BAPS PRODUCTION', 'BAPS VALIDATION')";

            else if (post.Type == "A1")
                strWhereClause += " AND POStep IN ('PO PROCESS') ";
            else if (post.Type == "A2")
                strWhereClause += " AND POStep IN ('PO DONE') ";

            else if (post.Type == "B1")
                strWhereClause += " AND DashboardStep IN ('SITAC') ";
            else if (post.Type == "B2")
                strWhereClause += " AND DashboardStep IN ('CME') ";
            else if (post.Type == "B3")
                strWhereClause += " AND DashboardStep IN ('RFI') ";
            else if (post.Type == "B4")
                strWhereClause += " AND DashboardStep = 'BAUK SUBMIT' ";

            else if (post.Type == "C1")
                strWhereClause += " AND DashboardStep IN ('BAUK REVIEW') ";
            else if (post.Type == "C2")
                strWhereClause += " AND DashboardStep IN ('BAUK DONE') ";
            else if (post.Type == "C3")
                strWhereClause += " AND DashboardStep IN ('BAPS PRODUCTION') ";
            else if (post.Type == "C4")
                strWhereClause += " AND DashboardStep IN ('BAPS VALIDATION') ";
            else if (post.Type == "C5")
                strWhereClause += " AND DashboardStep IN ('RTI') ";
            else if (post.Type == "C6")
                strWhereClause += " AND DashboardStep IN ('CASH IN First BAPS BILLING') ";



            if (post.RFIDateMonth != null && post.Step != "FooterTotal" && fnCheckingValue(post.RFIDateMonth) == true)
                strWhereClause += " AND MONTH(DATEADD(DD,SetDayForecasting,RFIDate)) = '" + post.RFIDateMonth + "'";
            if (post.CustomerID != null && fnCheckingValue(post.CustomerID) == true)
                strWhereClause += " AND ISNULL(CustomerID,'NOT MAPPING') = '" + post.CustomerID + "' ";
            if (post.RFIDateWeek != null && fnCheckingValue(post.RFIDateWeek) == true)
                strWhereClause += " AND  CONVERT(VARCHAR(15), DATEPART(DAY, DATEDIFF(DAY, 0, DATEADD(DD, SetDayForecasting, RFIDate)) / 7 * 7) / 7 + 1) = '" + post.RFIDateWeek.Substring(1) + "' ";
            if (post.Step != null && fnCheckingValue(post.Step) == true)
            {
                if (post.Step.Contains("BAUK"))
                {
                    strWhereClause += " AND DashboardStep ='" + post.Step + "' ";
                }
                else if (post.Step.Contains("BAPS"))
                {
                    strWhereClause += " AND DashboardStep ='" + post.Step + "' ";
                }
                else if (post.Step.Contains("RTI"))
                {
                    strWhereClause += " AND DashboardStep ='" + post.Step + "' ";
                }
                else if (post.Step == "Total")
                {
                    strWhereClause += " AND DashboardStep NOT IN ( '-', 'CASH IN First BAPS BILLING' ) ";
                }
                else if (post.Step == "FooterTotal" && post.RFIDateMonth != null && fnCheckingValue(post.RFIDateMonth) == true)
                {
                    strWhereClause += " AND DashboardStep NOT IN ( '-', 'CASH IN First BAPS BILLING') AND MONTH(DATEADD(DD,SetDayForecasting,RFIDate)) = '" + fnGetMonthHeaderString(post.RFIDateMonth) + "' ";
                }
                else if (post.Step == "FooterTotal" && post.RFIDateMonth == null && fnCheckingValue(post.RFIDateMonth) == true)
                {
                    strWhereClause += " AND DashboardStep NOT IN ( '-', 'CASH IN First BAPS BILLING') ";
                }
                else if (post.Step == "WeekTotal")
                {
                    strWhereClause += " AND DashboardStep NOT IN ( '-', 'CASH IN First BAPS BILLING') ";
                }

            }






            var dataresult = client.GetDetailList(strWhereClause, 0, 0, "");

            string[] fieldList = new string[] {

            "SoNumber",
            "SiteID",
            "SiteName",
            "CustomerSiteID",
            "CustomerSiteName",
            "CustomerID",
            "RegionName",
            "ProvinceName",
            "ResidenceName",
            "po_number",
            "MLANumber",
            "StartLeaseDate",
            "EndLeaseDate",
            "BapsType",
            "CustomerInvoice",
            "CompanyInvoiceName",
            "CompanyID",
            "Currency",
            "InvoiceStartDate",
            "InvoiceEndDate",
            "BaseLeasePrice",
            "ServicePrice",
            "DeductionAmount",
            "AmountTotal"
            };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataresult, fieldList);
            table.Load(reader);

            ExportToExcelHelper.Export("Potential RFI Detail List", table);

        }

        static string fnGetMonthHeaderString(string MonthText)
        {
            string month = "";
            switch (MonthText)
            {
                case "JAN":
                    month = "1";
                    break;
                case "FEB":
                    month = "2";
                    break;
                case "MAR":
                    month = "3";
                    break;
                case "APR":
                    month = "4";
                    break;
                case "MAY":
                    month = "5";
                    break;
                case "JUN":
                    month = "6";
                    break;
                case "JUL":
                    month = "7";
                    break;
                case "AUG":
                    month = "8";
                    break;
                case "SEP":
                    month = "9";
                    break;
                case "OCT":
                    month = "10";
                    break;
                case "NOV":
                    month = "11";
                    break;
                case "DEC":
                    month = "12";
                    break;
                default:
                    month = MonthText;
                    break;
            }
            return month;
        }

        static bool fnCheckingValue(string value)
        {
            bool val = true;
            switch (value)
            {
                case "null":
                    val = false;
                    break;
                case "NULL":
                    val = false;
                    break;
                case "undefined":
                    val = false;
                    break;
                default:
                    val = true;
                    break;
            }
            return val;
        }
        #endregion
    }


}