using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ARSystemFrontEnd.Providers;
using System.Data;
using ARSystemFrontEnd.Helper;
using System.Collections.Specialized;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using ClosedXML.Excel;
using ARSystem.Domain.Models;
using ARSystem.Service;
using ARSystemFrontEnd.Models;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("RevenueSystem")]
    public class RevenueSystemController : BaseController
    {
        private readonly RevenueSystemService _revenueService;
        public RevenueSystemController()
        {
            _revenueService = new RevenueSystemService();
        }

        [Authorize]
        [Route("")]
        // GET: RevenueSystem
        public ActionResult Index()
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");

            return View();
        }


        [Authorize]
        [Route("UploadAccruePerDept")]
        public ActionResult UploadAccruePerDept()
        {
            string actionTokenView = "696d4e01-14c5-4e67-8a8e-059be6f12b5b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("AccrueRevenue")]
        public ActionResult AccrueRevenue()
        {
            string actionTokenView = "696d4e01-14c5-4e67-8a8e-059be6f12b5b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("AccrueRevenueDetail")]
        public ActionResult AccrueRevenueDetail()
        {
            string actionTokenView = "696d4e01-14c5-4e67-8a8e-059be6f12b5b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }


        [Route("DownloadPageAccruePerDept")]
        public ActionResult DownloadPageAccruePerDept()
        {
            string fileName = Request.QueryString["fileName"].ToString().Trim();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/templates/RevenueSystem/UploadAccruePerDept/" + fileName));

            return File(bytes, contentType, fileName);
        }

        [Authorize]
        [Route("MasterlistAccrue")]
        public ActionResult MasterlistAccrue()
        {
            string actionTokenView = "696d4e01-14c5-4e67-8a8e-059be6f12b5b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("DetailInvoice")]
        public ActionResult DetailInvoice()
        {
            string actionTokenView = "696d4e01-14c5-4e67-8a8e-059be6f12b5b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }


        [Authorize]
        [Route("AccruePerPIC")]
        public ActionResult AccruePerPIC()
        {
            string actionTokenView = "696d4e01-14c5-4e67-8a8e-059be6f12b5b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [HttpPost, Route("RevSys/UploadExcel")]
        public ActionResult RevSysUploadExcel()
        {
            try
            {
                NameValueCollection nvc = System.Web.HttpContext.Current.Request.Form;
                var cat = nvc.Get("Category").ToString().Trim();
                HttpPostedFileBase files = Request.Files[0]; //Read the first Posted Excel File  
                ISheet sheet; //Create the ISheet object to read the sheet cell values  
                string filename = Path.GetFileName(Server.MapPath(files.FileName)); //get the uploaded file name  
                var fileExt = Path.GetExtension(filename); //get the extension of uploaded excel file  
                if (fileExt == ".xls")
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(files.InputStream); //HSSWorkBook object will read the Excel 97-2000 formats  
                    sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook  
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(files.InputStream); //XSSFWorkBook will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook   
                }
                //List<ARSystemService.vmProduct> products = new List<ARSystemService.vmProduct>();
                //ARSystemService.vmProduct tempProduct;
                string sheetName = string.Empty;
                string soNumber = string.Empty;
                int startIndex = 1;
                var d = sheet.GetRow(1);
                string ds = d.Cells[0].ToString();
                List<ARSystemService.vmRevSysAccruePerDept> dtAccruePerDept = new List<ARSystemService.vmRevSysAccruePerDept>();
                ARSystemService.vmRevSysAccruePerDept tempData;
                for (int i = startIndex; i <= sheet.LastRowNum; i++) //Loop the records upto filled row, starts from content row (row 3)
                {
                    if (cat == "Accrue")
                    {
                        if (sheet.GetRow(i) != null) //null is when the row only contains empty cells   
                        {
                            tempData = new ARSystemService.vmRevSysAccruePerDept();
                            tempData.Sonumb = sheet.GetRow(i).Cells[0].StringCellValue.Trim();
                            tempData.Division = sheet.GetRow(i).Cells[1].StringCellValue.Trim();
                            tempData.PICA = sheet.GetRow(i).Cells[2].StringCellValue.Trim();
                            tempData.PicaDetail = sheet.GetRow(i).Cells[3].StringCellValue.Trim();
                            tempData.PicaSuggest = sheet.GetRow(i).Cells[4].StringCellValue.Trim();
                            tempData.year = sheet.GetRow(i).Cells[5].StringCellValue.Trim();
                            tempData.month = sheet.GetRow(i).Cells[6].StringCellValue.Trim();
                            tempData.week = sheet.GetRow(i).Cells[7].StringCellValue.Trim();
                            dtAccruePerDept.Add(tempData);
                        }
                    }
                    else
                    {
                        if (sheet.GetRow(i) != null) //null is when the row only contains empty cells   
                        {
                            tempData = new ARSystemService.vmRevSysAccruePerDept();
                            tempData.Sonumb = sheet.GetRow(i).Cells[0].StringCellValue.Trim();
                            tempData.PeriodeStartDate = Convert.ToDateTime(sheet.GetRow(i).Cells[1].ToString().Trim()).ToString("yyyy-MM-dd");
                            tempData.PeriodeEndDate = Convert.ToDateTime(sheet.GetRow(i).Cells[2].ToString().Trim()).ToString("yyyy-MM-dd");
                            tempData.amount = Convert.ToDecimal(sheet.GetRow(i).Cells[3].ToString().Trim());
                            tempData.currency = sheet.GetRow(i).Cells[4].ToString().Trim();
                            tempData.type = cat;

                            dtAccruePerDept.Add(tempData);
                        }
                    }
                }

                List<ARSystemService.vmRevSysAccruePerDept> dat;
                using (var client = new ARSystemService.RevenueSystemServiceClient())
                {
                    ARSystemService.vmRevSysParamMasterListAccrue param = new ARSystemService.vmRevSysParamMasterListAccrue();

                    if (cat == "Accrue")
                    {
                        param.flag = "ShowAndValidasiAll";
                        param.month = sheet.GetRow(1).Cells[6].StringCellValue.Trim();
                        param.year = sheet.GetRow(1).Cells[5].StringCellValue.Trim();

                        client.InsertRevSysAccruePerDept(UserManager.User.UserToken, dtAccruePerDept.ToArray()).ToList();
                        dat = client.GetRevSysDataLoadAccruePerDept(UserManager.User.UserToken, param).ToList();
                    }
                    else
                    {
                        param.flag = "ShowUploadTempOverAll";
                        param.Category = cat.ToString().Trim();

                        client.InsertTempOverAll(UserManager.User.UserToken, dtAccruePerDept.ToArray()).ToString();
                        dat = client.ShowUploadTempOver(UserManager.User.UserToken, param).ToList();
                    }
                }

                return Json(dat, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json("Exception");

            }
        }


        [Route("MasterListAccrue/Export")]
        public void getRevSysMasterListAccrue_Export()
        {
            ARSystemService.vmRevSysParamMasterListAccrue param = new ARSystemService.vmRevSysParamMasterListAccrue();
            List<ARSystemService.vmRevSysDataHoldStopAccrue> RevSysdataHoldStopAccrue = new List<ARSystemService.vmRevSysDataHoldStopAccrue>();
            List<ARSystemService.vmRevSysDataHoldStopAccrue> list = new List<ARSystemService.vmRevSysDataHoldStopAccrue>();
            param.company = Request.QueryString["companyID"];
            param.OperatorId = Request.QueryString["operatorID"];
            param.sonumb = Request.QueryString["sonumb"];
            param.Startidx = 0;
            using (var client = new ARSystemService.RevenueSystemServiceClient())
            {
                param.flag = "CountData";
                param.Endidx = client.GetRevSysCountHoldStopAccrue(UserManager.User.UserToken, param);
                var batch = param.Endidx / 5000;
                for (int i = 0; i <= batch; i++)
                {
                    param.Startidx = 5000 * i;
                    param.Endidx = param.Startidx + 5000;
                    param.flag = "Show";
                    RevSysdataHoldStopAccrue = client.GetRevSysDataHoldStopAccrue(UserManager.User.UserToken, param).ToList();
                    list.AddRange(RevSysdataHoldStopAccrue);
                }

                string[] fieldList = new string[] {
                "Sonumb",
              "SiteName" ,
              "SiteID" ,
              "Operator" ,
              "Company" ,
              "RFI_Date" ,
              "SLD_Date" ,
              "Status_Tenant" ,
              "Is_Tagih" ,
              "date_start" ,
              "date_END"
                 };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(list, fieldList);
                table.Load(reader);

                ExportToExcelHelper.Export("HoldStopAccrue_" + DateTime.Now.ToString("yyyyMMddHHmmss"), table);
            }

        }

        [Route("RevSysAccruePerPIC/Export")]
        public void getRevSysAccruePerPIC_Export()
        {
            ARSystemService.vmRevSysParamMasterListAccrue param = new ARSystemService.vmRevSysParamMasterListAccrue();
            param.year = Request.QueryString["year"];
            param.month = Request.QueryString["month"];
            param.week = Request.QueryString["week"];

            using (var client = new ARSystemService.RevenueSystemServiceClient())
            {
                List<ARSystemService.vmRevSysAccruePerPIC> list = new List<ARSystemService.vmRevSysAccruePerPIC>();
                param.flag = "AccruePerPIC";
                List<ARSystemService.vmRevSysAccruePerPIC> dt = client.ShowAccruePerDept(UserManager.User.UserToken, param).ToList();
                list.AddRange(dt);
                string[] fieldList = new string[] {
                    "ID",
                    "PIC",
                    "ACCOUNTING" ,
                    "FINANCE" ,
                    "FLEXIUPDATE" ,
                   "LEGAL" ,
                    "PDIMARKETING" ,
                   "RAREC" ,
                   "ASSET" ,
                     "MARKETING" ,
                     "PDI" ,
                     "PROJECT" ,
                     "RANEW" ,
                     "XLMINICME" ,
                     "TOTAL"
        };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(list, fieldList);
                table.Load(reader);

                ExportToExcelHelper.Export("AccruePerPIC_" + DateTime.Now.ToString("yyyyMMddHHmmss"), table);
            }


        }
        [Route("RevSysDetail/ExportDetail")]
        public void getRevsysDetailExport()
        {
            ARSystemService.vmRevSysParameters param = new ARSystemService.vmRevSysParameters();
            param.Sonumb = Request.QueryString["comSonumb"].ToString().Trim();
            param.KategoryRevenue = Request.QueryString["comKategoryRevenue"].ToString().Trim();
            param.SpParam = "LoadDetailRevSys";
            List<ARSystemService.vmRevSysDataDetail> dt = new List<ARSystemService.vmRevSysDataDetail>();
            using (var client = new ARSystemService.RevenueSystemServiceClient())
            {
                dt = client.GetRevSysDataDetail(UserManager.User.UserToken, param).ToList();
            }

            string[] fieldList = new string[] {
                "StartDueDateSLD",
                "EndDueDateSLD",
                "AmountSLD",
                "StartDueDate",
                "EndDueDate",
                "InvoiceCount",
                "AmountTotalInvoice",
                "AmountAmortSite",
                "AmountAccrue",
                "AmountInflasi",
                "AmountInvoiced",
                "AdjUpdateHarga",
                "AdjSLD",
                "AdjCleansing",
                "Balance"
            };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dt, fieldList);
            table.Load(reader);


            try
            {
                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                IXLWorksheet ws = wbook.Worksheets.Add("Sheet 1");

                ws.Cell(1, 1).Value = "Sonumb : ";
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 2).Value = param.Sonumb.ToString().Trim();
                ws.Cell(1, 2).DataType = XLCellValues.Text;
                ws.Cell(2, 1).Value = "Site ID : ";
                ws.Cell(2, 1).Style.Font.Bold = true;
                ws.Cell(2, 2).Value = Request.QueryString["comSiteID"].ToString().Trim();
                ws.Cell(2, 2).DataType = XLCellValues.Text;
                ws.Cell(3, 1).Value = "Site Name : ";
                ws.Cell(3, 1).Style.Font.Bold = true;
                ws.Cell(3, 2).Value = Request.QueryString["comSiteName"].ToString().Trim();
                ws.Cell(4, 1).Value = "Operator : ";
                ws.Cell(4, 1).Style.Font.Bold = true;
                ws.Cell(4, 2).Value = Request.QueryString["comOperator"].ToString().Trim();
                ws.Cell(5, 1).Value = "Tenant Type : ";
                ws.Cell(5, 1).Style.Font.Bold = true;
                ws.Cell(5, 2).Value = Request.QueryString["comTenantType"].ToString().Trim();
                ws.Cell(6, 1).Value = "Company Asset : ";
                ws.Cell(6, 1).Style.Font.Bold = true;
                ws.Cell(6, 2).Value = Request.QueryString["comCompanyAsset"].ToString().Trim();
                ws.Cell(7, 1).Value = "Periode : ";
                ws.Cell(7, 1).Style.Font.Bold = true;
                ws.Cell(7, 2).Value = Request.QueryString["comPeriode"].ToString().Trim();

                ws.Cell(1, 4).Value = "Rental Start : ";
                ws.Cell(1, 4).Style.Font.Bold = true;
                ws.Cell(1, 5).Value = Request.QueryString["comRentalStart"].ToString().Trim();
                ws.Cell(2, 4).Value = "Rental End : ";
                ws.Cell(2, 4).Style.Font.Bold = true;
                ws.Cell(2, 5).Value = Request.QueryString["comRentalEnd"].ToString().Trim();
                ws.Cell(3, 4).Value = "RFI Date : ";
                ws.Cell(3, 4).Style.Font.Bold = true;
                ws.Cell(3, 5).Value = Request.QueryString["comRFIDate"].ToString().Trim();
                ws.Cell(4, 4).Value = "SLD Date : ";
                ws.Cell(4, 4).Style.Font.Bold = true;
                ws.Cell(4, 5).Value = Request.QueryString["comSLDDate"].ToString().Trim();
                ws.Cell(5, 4).Value = "BAPS Date : ";
                ws.Cell(5, 4).Style.Font.Bold = true;
                ws.Cell(5, 5).Value = Request.QueryString["comBAPSDate"].ToString().Trim();
                ws.Cell(6, 4).Value = "Regional : ";
                ws.Cell(6, 4).Style.Font.Bold = true;
                ws.Cell(6, 5).Value = Request.QueryString["comRegional"].ToString().Trim();

                ws.Cell(1, 7).Value = "Total Amortsite BAPS : ";
                ws.Cell(1, 7).Style.Font.Bold = true;
                ws.Cell(1, 8).Value = Request.QueryString["comTotalAmortsiteBAPS"].ToString().Trim();
                ws.Cell(2, 7).Value = "Total Accrue : ";
                ws.Cell(2, 7).Style.Font.Bold = true;
                ws.Cell(2, 8).Value = Request.QueryString["comTotalAccrue"].ToString().Trim();
                ws.Cell(3, 7).Value = "Adjusment SLD : ";
                ws.Cell(3, 7).Style.Font.Bold = true;
                ws.Cell(3, 8).Value = Request.QueryString["comAdjusmentSLD"].ToString().Trim();
                ws.Cell(4, 7).Value = "Total Invoice : ";
                ws.Cell(4, 7).Style.Font.Bold = true;
                ws.Cell(4, 8).Value = Request.QueryString["comTotalInvoice"].ToString().Trim();
                ws.Cell(5, 7).Value = "Total Balance : ";
                ws.Cell(5, 7).Style.Font.Bold = true;
                ws.Cell(5, 8).Value = Request.QueryString["comTotalBalance"].ToString().Trim();
                ws.Cell(6, 7).Value = "Kategory Revenue : ";
                ws.Cell(6, 7).Style.Font.Bold = true;
                ws.Cell(6, 8).Value = Request.QueryString["comKategoryRevenue"].ToString().Trim();

                // From a DataTable
                var dataTable = table;
                IXLTable tableWithData = ws.Cell(9, 1).InsertTable(dataTable.AsEnumerable());

                //Start Width Column
                ws.Column(1).AdjustToContents();
                ws.Column(2).AdjustToContents();
                ws.Column(3).AdjustToContents();
                ws.Column(4).AdjustToContents();
                ws.Column(5).AdjustToContents();
                ws.Column(6).AdjustToContents();
                ws.Column(7).AdjustToContents();
                ws.Column(8).AdjustToContents();
                ws.Column(9).AdjustToContents();
                ws.Column(10).AdjustToContents();
                ws.Column(11).AdjustToContents();
                ws.Column(12).AdjustToContents();
                ws.Column(13).AdjustToContents();
                ws.Column(14).AdjustToContents();
                ws.Column(15).AdjustToContents();
                //End Width Column

                HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=" + "Detail-" + param.Sonumb.ToString().Trim() + ".xlsx");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
            catch (Exception ex)
            {

            }

            //ExportToExcelHelper.Export("DetailRevSys-"+param.Sonumb.ToString().Trim(), table);
        }



        [Route("RevSysHeader/Export")]
        public void getRevSysHeaderToExport()
        {
            ARSystemService.vmRevSysParameters param = new ARSystemService.vmRevSysParameters();

            param.CompanyAssetID = Request.QueryString["comAsset"];
            param.CompanyInvoiceID = Request.QueryString["comInv"];
            param.OperatorID = Request.QueryString["OperatorID"];
            param.RegionID = Request.QueryString["region"];
            param.KategoryRevenue = Request.QueryString["KategoryRevenue"];
            param.Year = Request.QueryString["MonthYear"].ToString().Substring(0, 4).Trim();
            param.Month = Request.QueryString["MonthYear"].ToString().Substring(5).Trim();
            //GetRevSysDataHeader
            using (var client = new ARSystemService.RevenueSystemServiceClient())
            {
                param.SpParam = "LoadGridCountAccrueRevenue";
                param.Endidx = client.GetRevSysDataHeaderCount(UserManager.User.UserToken, param);

                int batch = param.Endidx / 5000;
                List<ARSystemService.vmRevSysDataHeader> list = new List<ARSystemService.vmRevSysDataHeader>();
                param.SpParam = "LoadGridAccrueRevenue";
                for (int i = 0; i <= batch; i++)
                {
                    param.Startidx = 5000 * i;
                    param.Endidx = param.Startidx + 5000;
                    List<ARSystemService.vmRevSysDataHeader> dtHeader = client.GetRevSysDataHeader(UserManager.User.UserToken, param).ToList();
                    try
                    {
                        list.AddRange(dtHeader);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                string[] fieldList = new string[] {
                        "asatdate",
                        "sonumb",
                        "KategoriR",
                        "SiteID",
                        "SiteName",
                        "RegionalName",
                        "Province",
                        "OperatorId",
                        "status",
                        "DismantleDate",
                        "statusAccrue",
                        "company",
                        "CompanyInv",
                        "RFIDate",
                        "SLDDate",
                        "BAPSDate",
                        "RentalStart",
                        "RentalEnd",
                        "TenantType",
                        "TenancyWeight",
                        "RFCurr",
                        "MFCurr",
                        "RentalAmount",
                        "ServiceFeeAmount",
                        "Serviceandinflation",
                        "Overblast",
                        "Overdaya",
                        "NormalRev",
                        "RentalAmountInv",
                        "OMAmountInv",
                        "InflationInv",
                        "OverblastInv",
                        "OverdayaInv",
                        "PenaltyInv",
                        "DiscountPaidInv",
                        "AmountInv",
                        "NetRevenue",
                        "AccrueSld",
                        "adjDismantle",
                        "AdjUpdateHarga",
                        "AdjAccrue",
                        "SharingRevenue",
                        "AdjProRate",
                        "TotalAdjustment",
                        "BalanceAccured",
                        "BalanceDI"
                    };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(list, fieldList);
                table.Load(reader);

                ExportToExcelHelper.Export("HeaderRevSys", table);
            }
        }

        #region RevenuePerSonumb
        [Authorize]
        [Route("RevenuePerSonumb")]
        public ActionResult RevenuePerSonumb()
        {
            string actionTokenView = "AF1446E5-783D-43B4-88AB-26EF44C0D826";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Route("RevenuePerSonumb/Export")]
        public void RevenuePerSonumbExport()
        {
            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            //Parameter
            string strAccount = Request.QueryString["strAccount"].ToString();
            string strPeriode = Request.QueryString["strPeriode"];
            string strPeriodeTo = Request.QueryString["strPeriodeTo"];
            string strCompany = Request.QueryString["strCompany"].ToString();
            string strRegionName = Request.QueryString["strRegionName"].ToString();
            string strOperator = Request.QueryString["strOperator"].ToString();
            string strProduct = Request.QueryString["strProduct"].ToString();
            string schSoNumber = Request.QueryString["schSoNumber"].ToString();

            string[] fieldList = new string[] { };
            if(strAccount == "REVENUE")
            {
                fieldList = new string[]{
                    "SoNumber",
                        "SiteName",
                        "Operator",
                        "RegionName",
                        "Jan_RevenueNormal","Jan_Inflasi","Jan_Overblast","Jan_NetRevenue","Jan_PPN",
                        "Feb_RevenueNormal","Feb_Inflasi","Feb_Overblast","Feb_NetRevenue","Feb_PPN",
                        "Mar_RevenueNormal","Mar_Inflasi","Mar_Overblast","Mar_NetRevenue","Mar_PPN",
                        "Apr_RevenueNormal","Apr_Inflasi","Apr_Overblast","Apr_NetRevenue","Apr_PPN",
                        "May_RevenueNormal","May_Inflasi","May_Overblast","May_NetRevenue","May_PPN",
                        "Jun_RevenueNormal","Jun_Inflasi","Jun_Overblast","Jun_NetRevenue","Jun_PPN",
                        "Jul_RevenueNormal","Jul_Inflasi","Jul_Overblast","Jul_NetRevenue","Jul_PPN",
                        "Aug_RevenueNormal","Aug_Inflasi","Aug_Overblast","Aug_NetRevenue","Aug_PPN",
                        "Sep_RevenueNormal","Sep_Inflasi","Sep_Overblast","Sep_NetRevenue","Sep_PPN",
                        "Oct_RevenueNormal","Oct_Inflasi","Oct_Overblast","Oct_NetRevenue","Oct_PPN",
                        "Nov_RevenueNormal","Nov_Inflasi","Nov_Overblast","Nov_NetRevenue","Nov_PPN",
                        "Dec_RevenueNormal","Dec_Inflasi","Dec_Overblast","Dec_NetRevenue","Dec_PPN",
                        "TotalBalance"
                    };
            }
            else
            {
                fieldList = new string[]{
                    "SoNumber",
                        "SiteName",
                        "Operator",
                        "RegionName",
                        "Jan",
                        "Feb",
                        "Mar",
                        "Apr",
                        "May",
                        "Jun",
                        "Jul",
                        "Aug",
                        "Sep",
                        "Oct",
                        "Nov",
                        "Dec",
                        "TotalBalance"
                    };

            }
            

            var  count = _revenueService.GetvwARRevSysPerSonumbCount(strAccount, strPeriode, strPeriodeTo, strCompany, strRegionName, strOperator, strProduct, schSoNumber);
            var dataList = new List<vwARRevSysPerSonumb>();

            int batch = count / 5000;
            for (int i = 0; i <= batch; i++)
            {
                var skip = 5000 * i;
                var pageSize = 5000;
                var result = _revenueService.GetvwARRevSysPerSonumbList(strAccount, strPeriode, strPeriodeTo, strCompany, strRegionName, strOperator, strProduct, schSoNumber, "", skip, pageSize);
                try
                {
                    dataList.AddRange(result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataList, fieldList);
            table.Load(reader);
            if (strAccount == "REVENUE")
            {

                table.Columns["Jan_RevenueNormal"].ColumnName = "Revenue Normal Jan";
                table.Columns["Feb_RevenueNormal"].ColumnName = "Revenue Normal Feb";
                table.Columns["Mar_RevenueNormal"].ColumnName = "Revenue Normal Mar";
                table.Columns["Apr_RevenueNormal"].ColumnName = "Revenue Normal Apr";
                table.Columns["May_RevenueNormal"].ColumnName = "Revenue Normal May";
                table.Columns["Jun_RevenueNormal"].ColumnName = "Revenue Normal Jun";
                table.Columns["Jul_RevenueNormal"].ColumnName = "Revenue Normal Jul";
                table.Columns["Aug_RevenueNormal"].ColumnName = "Revenue Normal Aug";
                table.Columns["Sep_RevenueNormal"].ColumnName = "Revenue Normal Sep";
                table.Columns["Oct_RevenueNormal"].ColumnName = "Revenue Normal Oct";
                table.Columns["Nov_RevenueNormal"].ColumnName = "Revenue Normal Nov";
                table.Columns["Dec_RevenueNormal"].ColumnName = "Revenue Normal Dec";

                table.Columns["Jan_Inflasi"].ColumnName = "Inflasi Jan";
                table.Columns["Feb_Inflasi"].ColumnName = "Inflasi Feb";
                table.Columns["Mar_Inflasi"].ColumnName = "Inflasi Mar";
                table.Columns["Apr_Inflasi"].ColumnName = "Inflasi Apr";
                table.Columns["May_Inflasi"].ColumnName = "Inflasi May";
                table.Columns["Jun_Inflasi"].ColumnName = "Inflasi Jun";
                table.Columns["Jul_Inflasi"].ColumnName = "Inflasi Jul";
                table.Columns["Aug_Inflasi"].ColumnName = "Inflasi Aug";
                table.Columns["Sep_Inflasi"].ColumnName = "Inflasi Sep";
                table.Columns["Oct_Inflasi"].ColumnName = "Inflasi Oct";
                table.Columns["Nov_Inflasi"].ColumnName = "Inflasi Nov";
                table.Columns["Dec_Inflasi"].ColumnName = "Inflasi Dec";

                table.Columns["Jan_Overblast"].ColumnName = "Overblast Jan";
                table.Columns["Feb_Overblast"].ColumnName = "Overblast Feb";
                table.Columns["Mar_Overblast"].ColumnName = "Overblast Mar";
                table.Columns["Apr_Overblast"].ColumnName = "Overblast Apr";
                table.Columns["May_Overblast"].ColumnName = "Overblast May";
                table.Columns["Jun_Overblast"].ColumnName = "Overblast Jun";
                table.Columns["Jul_Overblast"].ColumnName = "Overblast Jul";
                table.Columns["Aug_Overblast"].ColumnName = "Overblast Aug";
                table.Columns["Sep_Overblast"].ColumnName = "Overblast Sep";
                table.Columns["Oct_Overblast"].ColumnName = "Overblast Oct";
                table.Columns["Nov_Overblast"].ColumnName = "Overblast Nov";
                table.Columns["Dec_Overblast"].ColumnName = "Overblast Dec";

                table.Columns["Jan_NetRevenue"].ColumnName = "Net Revenue Jan";
                table.Columns["Feb_NetRevenue"].ColumnName = "Net Revenue Feb";
                table.Columns["Mar_NetRevenue"].ColumnName = "Net Revenue Mar";
                table.Columns["Apr_NetRevenue"].ColumnName = "Net Revenue Apr";
                table.Columns["May_NetRevenue"].ColumnName = "Net Revenue May";
                table.Columns["Jun_NetRevenue"].ColumnName = "Net Revenue Jun";
                table.Columns["Jul_NetRevenue"].ColumnName = "Net Revenue Jul";
                table.Columns["Aug_NetRevenue"].ColumnName = "Net Revenue Aug";
                table.Columns["Sep_NetRevenue"].ColumnName = "Net Revenue Sep";
                table.Columns["Oct_NetRevenue"].ColumnName = "Net Revenue Oct";
                table.Columns["Nov_NetRevenue"].ColumnName = "Net Revenue Nov";
                table.Columns["Dec_NetRevenue"].ColumnName = "Net Revenue Dec";

                table.Columns["Jan_PPN"].ColumnName = "PPN Jan";
                table.Columns["Feb_PPN"].ColumnName = "PPN Feb";
                table.Columns["Mar_PPN"].ColumnName = "PPN Mar";
                table.Columns["Apr_PPN"].ColumnName = "PPN Apr";
                table.Columns["May_PPN"].ColumnName = "PPN May";
                table.Columns["Jun_PPN"].ColumnName = "PPN Jun";
                table.Columns["Jul_PPN"].ColumnName = "PPN Jul";
                table.Columns["Aug_PPN"].ColumnName = "PPN Aug";
                table.Columns["Sep_PPN"].ColumnName = "PPN Sep";
                table.Columns["Oct_PPN"].ColumnName = "PPN Oct";
                table.Columns["Nov_PPN"].ColumnName = "PPN Nov";
                table.Columns["Dec_PPN"].ColumnName = "PPN Dec";

            }

            //Export to Excel
            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            var ws = wbook.Worksheets.Add(table, "List Revenue Per Sonumb");

            ws.Column(5).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(6).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(7).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(8).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(9).Style.NumberFormat.Format = "#,##0.00";

            ws.Column(10).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(11).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(12).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(13).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(14).Style.NumberFormat.Format = "#,##0.00";

            ws.Column(15).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(16).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(17).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(18).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(19).Style.NumberFormat.Format = "#,##0.00";

            ws.Column(20).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(21).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(22).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(23).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(24).Style.NumberFormat.Format = "#,##0.00";

            ws.Column(25).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(26).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(27).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(28).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(29).Style.NumberFormat.Format = "#,##0.00";

            ws.Column(30).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(31).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(32).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(33).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(34).Style.NumberFormat.Format = "#,##0.00";

            ws.Column(35).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(36).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(37).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(38).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(39).Style.NumberFormat.Format = "#,##0.00";

            ws.Column(40).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(41).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(42).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(43).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(44).Style.NumberFormat.Format = "#,##0.00";
                      
            ws.Column(45).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(46).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(47).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(48).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(49).Style.NumberFormat.Format = "#,##0.00";
                      
            ws.Column(50).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(51).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(52).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(53).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(54).Style.NumberFormat.Format = "#,##0.00";
                      
            ws.Column(55).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(56).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(57).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(58).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(60).Style.NumberFormat.Format = "#,##0.00";

            ws.Column(61).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(62).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(63).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(64).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(65).Style.NumberFormat.Format = "#,##0.00";

            ws.Column(66).Style.NumberFormat.Format = "#,##0.00";

            HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=" + "List Revenue Per Sonumb" + ".xlsx");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }
        #endregion

        #region Summary
        //[Authorize]
        [Route("ReportSummary")]
        public ActionResult ReportSummary()
        {
            string actionTokenView = "06C865A1-E058-4585-BE86-0D76AFED9CE4";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (!client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Route("Summary/Export")]
        public void Summary()
        {
            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            //Parameter
            string strViewBy = Request.QueryString["strViewBy"].ToString();
            string strGroupBy = Request.QueryString["strGroupBy"].ToString();
            string strAccount = Request.QueryString["strAccount"].ToString();
            string intYear = Request.QueryString["intYear"];
            string strCompanyId = Request.QueryString["strCompanyId"].ToString();
            string strRegionName = Request.QueryString["strRegionName"].ToString();
            string strOperatorId = Request.QueryString["strOperatorId"].ToString();
            string strProduct = Request.QueryString["strProduct"].ToString();

            string[] fieldList = new string[] {
                        "Description",
                        "Jan",
                        "Feb",
                        "Mar",
                        "Apr",
                        "May",
                        "Jun",
                        "Jul",
                        "Aug",
                        "Sep",
                        "Oct",
                        "Nov",
                        "Dec",
                        "Total",
                        "TotalInPercent"
                    };

            var param = new vmRevenueSummaryParameters();
            param.vAccount = strAccount;
            param.vGroupBy = strGroupBy;
            param.vViewBy = strViewBy;
            if (!string.IsNullOrEmpty(intYear))
            {
                param.vYear = Convert.ToInt16(intYear);
            }
            if (!string.IsNullOrEmpty(strCompanyId))
            {
                param.vCompanyId = strCompanyId;
            }
            if (!string.IsNullOrEmpty(strRegionName))
            {
                param.vRegionalName = strRegionName;
            }
            if (!string.IsNullOrEmpty(strOperatorId))
            {
                param.vOperatorId = strOperatorId;
            }

            if (param.vViewBy == "Amount")
            {
                var dataList = new List<vmRevenueSummaryAmount>();
                var result = _revenueService.GetSummaryAmount(param, userCredential.UserID);
                dataList.AddRange(result);
                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(dataList, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("List SummaryInAmmount", table);
            }
            else
            {
                var dataList = new List<vmRevenueSummaryUnit>();
                var result = _revenueService.GetSummaryUnit(param, userCredential.UserID);
                dataList.AddRange(result);
                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(dataList, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("List SummaryinUnit", table);
            }
        }

        [Route("SoNumberList/Export")]
        public void SoNumberList()
        {
            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            //Parameter
            string strGroupBy = Request.QueryString["strGroupBy"].ToString();
            string strAccount = Request.QueryString["strAccount"].ToString();
            string intYear = Request.QueryString["intYear"];
            string intMonth = Request.QueryString["intMonth"];
            string strCompanyId = Request.QueryString["strCompanyId"].ToString();
            string strRegionName = Request.QueryString["strRegionName"].ToString();
            string strOperatorId = Request.QueryString["strOperatorId"].ToString();
            string strProduct = Request.QueryString["strProduct"].ToString();
            string strDesc = Request.QueryString["strDesc"].ToString();

            string[] fieldList = new string[] {
                        "SoNumber",
                        "SiteID",
                        "SiteName",
                        "CustomerID",
                        "RegionName",
                        "ProductName",
                        "Status",
                        "StipSiro",
                        "StipID",
                        "StipCategory",
                        "TotalAccrued",
                        "TotalUnearned",
                        "AmountRevenue"
                    };

            var param = new vmRevenueSummaryParameters();
            param.vGroupBy = strGroupBy;
            param.vAccount = strAccount;
            if (!string.IsNullOrEmpty(intYear))
            {
                param.vYear = Convert.ToInt16(intYear);
            }
            if (!string.IsNullOrEmpty(strCompanyId))
            {
                param.vCompanyId = strCompanyId;
            }
            if (!string.IsNullOrEmpty(strRegionName))
            {
                param.vRegionalName = strRegionName;
            }
            if (!string.IsNullOrEmpty(strOperatorId))
            {
                param.vOperatorId = strOperatorId;
            }
            if (!string.IsNullOrEmpty(intMonth))
            {
                param.vMonth = Convert.ToInt16(intMonth);
            }
            if (!string.IsNullOrEmpty(strDesc))
            {
                param.vGroupValue = strDesc;
            }

            var dataList = new List<vmRevenueSoNumberList>();
            var result = _revenueService.GetSoNumberList(param, userCredential.UserID);
            dataList.AddRange(result);
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataList, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("Summary - SoNumberList", table);
        }

        [Route("SoNumberList")]
        public ActionResult SonumberList()
        {
            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            if (userCredential.ErrorType > 0)
            {
                return RedirectToAction("Logout", "Login");
            }

            return View();
        }

        [Route("SonumberDetail")]
        public ActionResult SonumberDetail()
        {
            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            if (userCredential.ErrorType > 0)
            {
                return RedirectToAction("Logout", "Login");
            }

            return View();
        }
        #endregion

        #region Movement
        [Route("ReportMovement")]
        public ActionResult ReportMovement()
        {
            string actionTokenView = "28928589-B750-4DD7-B4A2-0627B12F693F";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (!client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Route("SummaryMovement/Export")]
        public void SummaryMovement()
        {
            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            //Parameter
            string strViewBy = Request.QueryString["strViewBy"].ToString();
            string strGroupBy = Request.QueryString["strGroupBy"].ToString();
            string intYear = Request.QueryString["intYear"];
            string intMonth = Request.QueryString["intMonth"];
            string strCompanyId = Request.QueryString["strCompanyId"].ToString();
            string strRegionName = Request.QueryString["strRegionName"].ToString();
            string strOperatorId = Request.QueryString["strOperatorId"].ToString();
            string strProduct = Request.QueryString["strProduct"].ToString();

            string[] fieldList = new string[] {
                        "Description",
                        "Previous",
                        "Current",
                        "Movement",
                        "Percentage",
                        "prevAccrueOverquotaISAT",
                        "prevCancelDiscount",
                        "prevDiffMLnv",
                        "prevDiskon",
                        "prevHoldAccrue",
                        "prevInflasi",
                        "prevKurs",
                        "prevNewPriceNew",
                        "prevNewPriceRenewal",
                        "prevNewSLD",
                        "prevOverBlast",
                        "prevReAktifHoldAccrue",
                        "prevRelokasi",
                        "prevReStopAccrue",
                        "prevSharingRevenue",
                        "prevSLDBAPSInvoice",
                        "prevStopAccrue",
                        "prevTemporaryFree",
                        "AccrueOverquotaISAT",
                        "CancelDiscount",
                        "DiffMLnv",
                        "Diskon",
                        "HoldAccrue",
                        "Inflasi",
                        "Kurs",
                        "NewPriceNew",
                        "NewPriceRenewal",
                        "NewSLD",
                        "OverBlast",
                        "ReAktifHoldAccrue",
                        "Relokasi",
                        "ReStopAccrue",
                        "SharingRevenue",
                        "SLDBAPSInvoice",
                        "StopAccrue",
                        "TemporaryFree",
                        "Total"
                    };

            var param = new vmRevenueSummaryParameters();
            param.vGroupBy = strGroupBy;
            param.vViewBy = strViewBy;
            if (!string.IsNullOrEmpty(intYear))
            {
                param.vYear = Convert.ToInt16(intYear);
            }
            if (!string.IsNullOrEmpty(intMonth))
            {
                param.vMonth = Convert.ToInt16(intMonth);
            }
            if (!string.IsNullOrEmpty(strCompanyId))
            {
                param.vCompanyId = strCompanyId;
            }
            if (!string.IsNullOrEmpty(strRegionName))
            {
                param.vRegionalName = strRegionName;
            }
            if (!string.IsNullOrEmpty(strOperatorId))
            {
                param.vOperatorId = strOperatorId;
            }

            if (param.vViewBy == "Amount")
            {
                var dataList = new List<vmRevenueMovementAmount>();
                var result = _revenueService.GetSummaryMovementAmount(param, userCredential.UserID);
                dataList.AddRange(result);
                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(dataList, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("List MovementinAmount", table);
            }
            else
            {
                var dataList = new List<vmRevenueMovementUnit>();
                var result = _revenueService.GetSummaryMovementUnit(param, userCredential.UserID);
                dataList.AddRange(result);
                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(dataList, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("List MovementinUnit", table);
            }
        }

        [Route("SummaryMovementDetail/Export")]
        public void SummaryMovementDetail()
        {
            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            //Parameter
            string strViewBy = Request.QueryString["strViewBy"].ToString();
            string strGroupBy = Request.QueryString["strGroupBy"].ToString();
            string intYear = Request.QueryString["intYear"];
            string intMonth = Request.QueryString["intMonth"];
            string strCompanyId = Request.QueryString["strCompanyId"].ToString();
            string strRegionName = Request.QueryString["strRegionName"].ToString();
            string strOperatorId = Request.QueryString["strOperatorId"].ToString();
            string strProduct = Request.QueryString["strProduct"].ToString();
            string strDesc = Request.QueryString["strDesc"].ToString();
            string strSoNumber = Request.QueryString["strSoNumber"].ToString();
            string strSiteID = Request.QueryString["strSiteID"].ToString();
            string strSiteName = Request.QueryString["strSiteName"].ToString();

            string[] fieldList = new string[] {
                        "SoNumber",
                        "SiteID",
                        "SiteName",
                        "RegionName",
                        "CustomerID",
                        "CompanyID",
                        "RFIDate",
                        "SLDDate",
                        "StartBapsDate",
                        "PreviousMonth",
                        "CurrentMonth",
                        "Movement"
                    };

            var param = new vmRevenueSummaryParameters();
            param.vGroupBy = strGroupBy;
            param.vViewBy = strViewBy;
            param.vDesc = strDesc;
            if (!string.IsNullOrEmpty(intYear))
            {
                param.vYear = Convert.ToInt16(intYear);
            }
            if (!string.IsNullOrEmpty(intMonth))
            {
                param.vMonth = Convert.ToInt16(intMonth);
            }
            if (!string.IsNullOrEmpty(strCompanyId))
            {
                param.vCompanyId = strCompanyId;
            }
            if (!string.IsNullOrEmpty(strRegionName))
            {
                param.vRegionalName = strRegionName;
            }
            if (!string.IsNullOrEmpty(strOperatorId))
            {
                param.vOperatorId = strOperatorId;
            }
            if (!string.IsNullOrEmpty(strProduct))
            {
                param.vProduct = strProduct;
            }
            if (!string.IsNullOrEmpty(strSoNumber))
            {
                param.vSoNumber = strSoNumber;
            }
            if (!string.IsNullOrEmpty(strSiteID))
            {
                param.vSiteID = strSiteID;
            }
            if (!string.IsNullOrEmpty(strSiteName))
            {
                param.vSiteName = strSiteName;
            }

            if (param.vViewBy == "Amount")
            {
                var dataList = new List<vmRevenueDetailMovement>();
                var result = _revenueService.GetDetailMovementByAmount(param, userCredential.UserID);
                dataList.AddRange(result);
                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(dataList, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("List MovementinDetailAmount", table);
            }
            else
            {
                var dataList = new List<vmRevenueDetailMovementUnit>();
                var result = _revenueService.GetDetailMovementByUnit(param, userCredential.UserID);
                dataList.AddRange(result);
                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(dataList, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("List MovementinDetailUnit", table);
            }
        }
        #endregion
        #region Dashboard Revenue
        [Route("DatabaseAudit")]
        public ActionResult DatabaseAudit()
        {

            string actionTokenView = "BFA7A4DB-4789-4596-861A-C4963AF5AF95";
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {

                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }


        [Route("ListDashboardRevenue/Export")]
        public void GetRevenueDashboardToExport()
        {
            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            dwhARRevenueSysSummary post = new dwhARRevenueSysSummary();
            post.fDataYear = Convert.ToInt32(Request.QueryString["strYear"]);
            post.RegionName = Request.QueryString["strRegionName"];
            post.CompanyID = Request.QueryString["strCompany"];
            post.CustomerID = Request.QueryString["strCustomer"];

            var listDataPostedDashboardRevenue = _revenueService.GetListArRevenueSummary(post, 0, 0, "");

            //Convert to DataTable

            DataTable table = new DataTable();
            string[] ColumsShow = new string[]
            {
                "SONumber",
                "SiteName",
                "CustomerID",
                "RegionName",
                "JanSourceAccrued",
                "JanSourceAmrUnearned",
                "JanSourceBalance",
                "JanBalanceAccrued",
                "JanBalanceUnearned",
                "FebSourceAccrued",
                "FebSourceAmrUnearned",
                "FebSourceBalance",
                "FebBalanceAccrued",
                "FebBalanceUnearned",
                "MarSourceAccrued",
                "MarSourceAmrUnearned",
                "MarSourceBalance",
                "MarBalanceAccrued",
                "MarBalanceUnearned",
                "AprSourceAccrued",
                "AprSourceAmrUnearned",
                "AprSourceBalance",
                "AprBalanceAccrued",
                "AprBalanceUnearned",
                "MaySourceAccrued",
                "MaySourceAmrUnearned",
                "MaySourceBalance",
                "MayBalanceAccrued",
                "MayBalanceUnearned",
                "JunSourceAccrued",
                "JunSourceAmrUnearned",
                "JunSourceBalance",
                "JunBalanceAccrued",
                "JunBalanceUnearned",
                "JulSourceAccrued",
                "JulSourceAmrUnearned",
                "JulSourceBalance",
                "JulBalanceAccrued",
                "JulBalanceUnearned",
                "AugSourceAccrued",
                "AugSourceAmrUnearned",
                "AugSourceBalance",
                "AugBalanceAccrued",
                "AugBalanceUnearned",
                "SepSourceAccrued",
                "SepSourceAmrUnearned",
                "SepSourceBalance",
                "SepBalanceAccrued",
                "SepBalanceUnearned",
                "OctSourceAccrued",
                "OctSourceAmrUnearned",
                "OctSourceBalance",
                "OctBalanceAccrued",
                "OctBalanceUnearned",
                "NovSourceAccrued",
                "NovSourceAmrUnearned",
                "NovSourceBalance",
                "NovBalanceAccrued",
                "NovBalanceUnearned",
                "DecSourceAccrued",
                "DecSourceAmrUnearned",
                "DecSourceBalance",
                "DecBalanceAccrued",
                "DecBalanceUnearned"

            };

            var reader = FastMember.ObjectReader.Create(listDataPostedDashboardRevenue.Select(i => new
            {
                i.SONumber,
                i.SiteName,
                i.CustomerID,
                i.RegionName,
                i.JanSourceAccrued,
                i.JanSourceAmrUnearned,
                i.JanSourceBalance,
                i.JanBalanceAccrued,
                i.JanBalanceUnearned,
                i.FebSourceAccrued,
                i.FebSourceAmrUnearned,
                i.FebSourceBalance,
                i.FebBalanceAccrued,
                i.FebBalanceUnearned,
                i.MarSourceAccrued,
                i.MarSourceAmrUnearned,
                i.MarSourceBalance,
                i.MarBalanceAccrued,
                i.MarBalanceUnearned,
                i.AprSourceAccrued,
                i.AprSourceAmrUnearned,
                i.AprSourceBalance,
                i.AprBalanceAccrued,
                i.AprBalanceUnearned,
                i.MaySourceAccrued,
                i.MaySourceAmrUnearned,
                i.MaySourceBalance,
                i.MayBalanceAccrued,
                i.MayBalanceUnearned,
                i.JunSourceAccrued,
                i.JunSourceAmrUnearned,
                i.JunSourceBalance,
                i.JunBalanceAccrued,
                i.JunBalanceUnearned,
                i.JulSourceAccrued,
                i.JulSourceAmrUnearned,
                i.JulSourceBalance,
                i.JulBalanceAccrued,
                i.JulBalanceUnearned,
                i.AugSourceAccrued,
                i.AugSourceAmrUnearned,
                i.AugSourceBalance,
                i.AugBalanceAccrued,
                i.AugBalanceUnearned,
                i.SepSourceAccrued,
                i.SepSourceAmrUnearned,
                i.SepSourceBalance,
                i.SepBalanceAccrued,
                i.SepBalanceUnearned,
                i.OctSourceAccrued,
                i.OctSourceAmrUnearned,
                i.OctSourceBalance,
                i.OctBalanceAccrued,
                i.OctBalanceUnearned,
                i.NovSourceAccrued,
                i.NovSourceAmrUnearned,
                i.NovSourceBalance,
                i.NovBalanceAccrued,
                i.NovBalanceUnearned,
                i.DecSourceAccrued,
                i.DecSourceAmrUnearned,
                i.DecSourceBalance,
                i.DecBalanceAccrued,
                i.DecBalanceUnearned

            }), ColumsShow);
            table.Load(reader);

            ExportToExcelHelper.Export("ListDashboardRevenue", table);

        }
        #endregion

        #region RevenueReport Description
        [Authorize]
        [Route("RevenueDesc")]
        public ActionResult RevenueDesc()
        {
            string actionTokenView = "EF53C8A5-2536-4A21-BBC6-772D8F6F32A2";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View("~/Views/RevenueSystem/RevenueReportDescription.cshtml");
        }

        [Authorize]
        [HttpGet, Route("RevenueDescExport")]
        public void RevenueDescExport(PostDwhARRevSysDescription post)
        {

            string actionTokenView = "EF53C8A5-2536-4A21-BBC6-772D8F6F32A2";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    DataTable table = new DataTable();
                    try
                    {

                        var service = new RevenueSystemService();
                        var listData = new List<dwhARRevSysDescription>();
                        var postParam = new dwhARRevSysDescription();

                        postParam.CustomerId = post.CustomerId;
                        postParam.CompanyId = post.CompanyId;
                        postParam.RegionName = post.RegionName;
                        postParam.RegionId = post.RegionId;
                        postParam.StartSLDDate = post.StartSLDDate;
                        postParam.EndSLDDate = post.EndSLDDate;
                        postParam.RfiStartDate = post.RfiStartDate;
                        postParam.RfiEndDate = post.RfiEndDate;
                        postParam.DepartmentName = post.DepartmentName;
                        postParam.SiteId = post.SiteId;
                        postParam.SiteName = post.SiteName;
                        postParam.SoNumber = post.SoNumber;


                        listData = service.GetRevenueReportDescriptionList(postParam);

                        string[] fieldList = new string[] {
                                                              "Number",
                                                              "SONumb",
                                                              "SiteId" ,
                                                              "SiteName" ,
                                                              "STIP" ,
                                                              "Regional" ,
                                                              "UserNumber" ,
                                                              "Customer" ,
                                                              "SiteStatus" ,
                                                              "ContractStatus" ,
                                                              "DocumentProcess",
                                                              "Company",
                                                              "RFIDate",
                                                              "SLDDate",
                                                              "BAPSDate",
                                                              "RentalStart",
                                                              "RentalEnd",
                                                              "TenantType",
                                                              "RFInv",
                                                               "MFInv",
                                                              "PriceBaseOnML",
                                                              "PriceBaseOnInvoice",
                                                              "LastInvPeriod",
                                                              "ContractStart",
                                                              "ContractEnd",
                                                              "BalanceAccrued",
                                                              "Aging",
                                                             };



                        var reader = FastMember.ObjectReader.Create(listData.Select(i => new
                        {
                            Number = i.RowIndex,
                            SONumb = i.SoNumber,
                            SiteId = i.SiteId,
                            SiteName = i.SiteName,
                            STIP = i.StipCategory,
                            Regional = i.RegionName,
                            UserNumber = i.UserNumber,
                            Customer = i.CustomerId,
                            SiteStatus = i.SiteStatus,
                            ContractStatus = i.ContractStatus,
                            DocumentProcess = i.DocumentProcess,
                            Company = i.CompanyId,
                            RFIDate = i.RfiDate,
                            SLDDate = i.SLDDate,
                            BAPSDate = i.BapsDate,
                            RentalStart = i.RentalStartDate,
                            RentalEnd = i.RentalEndDate,
                            TenantType = i.TenantType,
                            RFInv = i.RfiDate,
                            MFInv = i.MFInvoice,
                            PriceBaseOnML = i.PriceBaseOnML,
                            PriceBaseOnInvoice = i.PriceBaseOnInvoice,
                            LastInvPeriod = i.LastInvoicePeriod,
                            ContractStart = i.ContractStartDate,
                            ContractEnd = i.ContractEndDate,
                            BalanceAccrued = i.BalanceAccrued,
                            Aging = i.AgingDays
                        }), fieldList);
                        table.Load(reader);

                        ExportToExcelHelper.Export("ReportSummaryDescription", table);


                    }
                    catch (Exception ex)
                    {
                        ExportToExcelHelper.Export("ReportSummaryDescription", table);
                    }
                }

            }
        }
        #endregion
    }

}