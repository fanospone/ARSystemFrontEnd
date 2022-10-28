using System;
using System.Web.Mvc;
using System.Web;
using ARSystemFrontEnd.Providers;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections.Generic;
using ARSystemFrontEnd.Models;
using System.Linq;
using ARSystemFrontEnd.Helper;
using System.Text;
using System.Web.UI;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("RANewBaps")]
    public class RANewBapsController : BaseController
    {
        // GET: RANewBaps

        [Authorize]
        [Route("CheckingDocument")]
        public ActionResult CheckingDocument()
        {
            string actionTokenView = "17B039D8-6431-47B4-BD54-F11194899C8C";
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
        }

        [Authorize]
        [Route("StartBaps")]
        public ActionResult StartBaps()
        {
            string actionTokenView = "D31A32FD-E2D5-47F7-B5A0-839EA6636DC7";
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
        }

        [Authorize]
        [Route("BapsInput")]
        public ActionResult BapsInput()
        {
            string actionTokenView = "D31A32FD-E2D5-47F7-B5A0-839EA6636DC7";
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
        }

        [Authorize]
        [Route("printPDF")]
        public ActionResult PrintPDF(string SoNumber, int BulkID, string CustomerID, string PrintType, string StipCategory, string ProductType)
        {
            string htmlElements = "";
            string headerHtml = "";
            string footerHtml = "";
            string fileName = "";
            //===========================================================================================================//
            string actionTokenView = "D31A32FD-E2D5-47F7-B5A0-839EA6636DC7";
            List<ARSystemService.trxRABapsMaterials> dataList = new List<ARSystemService.trxRABapsMaterials>();
            ARSystemService.trxRABapsMaterials post = new ARSystemService.trxRABapsMaterials();
            ARSystemService.mstRAGeneratorPDF resultHtml = new ARSystemService.mstRAGeneratorPDF();
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    using (var client2 = new ARSystemService.NewBapsStartBapsServiceClient())
                    {
                        // get data //
                        fileName = SoNumber == "" ? BulkID.ToString() : SoNumber;
                        resultHtml = client2.GetHtmlElement(UserManager.User.UserToken, SoNumber, CustomerID, StipCategory, BulkID, PrintType, ProductType);
                        // ====== set header page ================================//
                        if (PrintType.ToLower() == "header")
                        {
                            headerHtml += "<!DOCTYPE html><html><body>";
                            headerHtml += "<table style='width:100%'>";
                            headerHtml += "<tr>";
                            headerHtml += "<td> <img src='" + Server.MapPath(resultHtml.LogoPathLeft) + "' width='" + resultHtml.LogoLeftWidth + "px' height='" + resultHtml.LogoLeftHeight + "px' />";
                            headerHtml += "</td>";
                            headerHtml += "<td> <img src='" + Server.MapPath(resultHtml.LogoPathRight) + "'  width='" + resultHtml.LogoRightWidth + "px' height='" + resultHtml.LogoRightHeight + "px' style='float:right' />";
                            headerHtml += "</td>";
                            headerHtml += "</tr>";
                            headerHtml += "</table>";
                            headerHtml += "</body></html>";
                            // ====== set footer page ================================//
                            // === javascript for add page number == //
                            string jsScript = "<script>function subst() { var vars = { };var x = document.location.search.substring(1).split('&');";
                            jsScript += "for(var i in x) { var z = x[i].split('=', 2); vars[z[0]] = unescape(z[1]); }";
                            jsScript += "var x =['frompage', 'topage', 'page', 'webpage', 'section', 'subsection', 'subsubsection'];";
                            jsScript += "for(var i in x){";
                            jsScript += "var y = document.getElementsByClassName(x[i]);";
                            jsScript += "for (var j = 0; j < y.length; ++j) y[j].textContent = vars[x[i]];}}</script > ";
                            //==================================================================================================//
                            footerHtml += "<!DOCTYPE html><html><head>";
                            footerHtml += jsScript + "</head><body onload='subst()'>";
                            footerHtml += "<table style='width:100%'>";
                            footerHtml += "<tr>";
                            footerHtml += "<td valign='top' style='padding-right:10px;'>";
                            footerHtml += resultHtml.FooterText;
                            footerHtml += "</tr><tr>";
                            footerHtml += "</td>";
                            footerHtml += "<td style='width:" + resultHtml.QRCodeWidth + "px'> <img src='" + GenerateQRCode(resultHtml.TextQrCode, "baps_" + fileName + ".jpg") + "' width='" + resultHtml.QRCodeWidth + "px' height='" + resultHtml.QRCodeHeight + "px' style='float:right'/>";
                            footerHtml += "</td>";
                            footerHtml += "</tr>";
                            footerHtml += "</table>";
                            footerHtml += "</body></html>";
                            //================================================================================================//
                            htmlElements += resultHtml.HtmlString;
                        }
                        else if (CustomerID.ToUpper() == "ISAT" && PrintType.ToLower() == "details")
                        {
                            post.BulkyID = BulkID;
                            dataList = client2.GetListBapsMaterials(UserManager.User.UserToken, post, 0, 0).ToList();
                            htmlElements += "<div style='font-size:14px; font-family:Calibri; width:100%;'>";
                            htmlElements += GenerateSiteListHtml(dataList, resultHtml.HtmlString, resultHtml.LogoPathLeft, resultHtml.LogoPathRight, resultHtml.LogoLeftWidth, resultHtml.LogoLeftHeight, resultHtml.LogoRightWidth, resultHtml.LogoRightHeight, resultHtml.QRCodeWidth, resultHtml.QRCodeHeight);
                            htmlElements += GenerateMaterialHtml(dataList, resultHtml.HtmlString, resultHtml.LogoPathLeft, resultHtml.LogoPathRight, resultHtml.LogoLeftWidth, resultHtml.LogoLeftHeight, resultHtml.LogoRightWidth, resultHtml.LogoRightHeight, resultHtml.QRCodeWidth, resultHtml.QRCodeHeight);
                            htmlElements += "</div>";
                        }
                        else if (CustomerID.ToUpper() == "XL" && PrintType.ToLower() == "details" && StipCategory.ToUpper() == "NON-ADDITIONAL")
                        {
                            List<ARSystemService.trxRABapsPrintXLBulky> XLBulkyPrint = new List<ARSystemService.trxRABapsPrintXLBulky>();
                            XLBulkyPrint = client2.GetListDetailXLBulky(UserManager.User.UserToken, BulkID, 0, 0).ToList();
                            htmlElements = GenerateHtmlXLBulky(XLBulkyPrint, resultHtml.HtmlString, resultHtml.LogoPathLeft, resultHtml.LogoLeftWidth, resultHtml.LogoLeftHeight, resultHtml.LogoPathRight, resultHtml.LogoRightWidth, resultHtml.LogoRightHeight, resultHtml.QRCodeWidth, resultHtml.QRCodeHeight);
                        }
                    }
                }
            }

            BapsPrintPDF BapsPrintPDF = new BapsPrintPDF();
            string customeSwitches = "";
            //  if (CustomerID.ToUpper() == "ISAT" && PrintType.ToLower() == "details" && StipCategory.ToUpper() == "BULKY")
            //{
            if (CustomerID == "ISAT" && PrintType == "details")
            {
                customeSwitches = "--footer-left \"version : 3.0\" --footer-center \"halaman [page] of [topage]\" --footer-right \"tanggal: [date]\" --footer-font-size \"6\"";
            }
            else
            {
                string headerPath = Server.MapPath("~/Views/RANewBaps/Header.html");
                System.IO.File.WriteAllText(headerPath, "");
                string footerPath = Server.MapPath("~/Views/RANewBaps/Footer.html");
                System.IO.File.WriteAllText(footerPath, "");

                System.IO.File.WriteAllText(headerPath, headerHtml);
                System.IO.File.WriteAllText(footerPath, footerHtml);
                customeSwitches = string.Format("--print-media-type --header-html  \"{0}\" --header-spacing \"3\" --footer-html \"{1}\"", headerPath, footerPath);
            }
            BapsPrintPDF.htmlElements = htmlElements;

            return new Rotativa.ViewAsPdf("~/Views/RANewBaps/PrintDetails.cshtml", BapsPrintPDF)
            {
                FileName = CustomerID + "_" + fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = PrintType == "details" ? Rotativa.Options.Orientation.Landscape : Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(Convert.ToInt16(resultHtml.MarginTop), Convert.ToInt16(resultHtml.MarginRight), Convert.ToInt16(resultHtml.MarginBottom), Convert.ToInt16(resultHtml.MarginLeft)),
                CustomSwitches = customeSwitches
            };
        }

        private string GenerateSiteListHtml(List<ARSystemService.trxRABapsMaterials> dataList, string approval, string logoLeftPath, string logoRightPath, int logoLeftWidth, int logoLeftHeight, int logoRightWidth, int logoRightHeight, int QrCodeWidth, int QrCodeHeight)
        {
            string htmlElements = "";
            string headerTbSiteList = "";
            string rowDataSiteList = "";
            string headerSiteListAttach = "";
            string qrCodeImage = "";
            string borderPage = "<div style='page-break-after:always; border:2px solid black; padding:10px; 20px; 0px; 20px;'>";
            headerTbSiteList += "<table style='border-collapse:collapse; width:100%'><tr>";
            headerTbSiteList += "<th style='border:1px solid black;' rowspan='2'>No</th>";
            headerTbSiteList += "<th style='border:1px solid black;' rowspan='2'>Nomor PO/Amd PO</th>";
            headerTbSiteList += "<th style='border:1px solid black;' rowspan='2'>RFS PO</th>";
            headerTbSiteList += "<th style='border:1px solid black;' rowspan='2'>Nama Site</th>";
            headerTbSiteList += "<th style='border:1px solid black;' rowspan='2'>Site ID</th>";
            headerTbSiteList += "<th style='border:1px solid black;' rowspan='2'>Tanggal Pemeriksaan</th>";
            headerTbSiteList += "<th style='border:1px solid black;' rowspan='2'>Nomor Berita Acara Uji Fungsi(BAUF)</th>";
            headerTbSiteList += "<th style='border:1px solid black' colspan='2'>Goods Reciept</th>";
            headerTbSiteList += "<th style='border:1px solid black;' rowspan='2'>Minor Pending ITem (%)</th>";
            headerTbSiteList += "<th style='border:1px solid black' colspan='2'>Keterlambatan (hari)</th></tr><tr>";
            headerTbSiteList += "<th style='border:1px solid black;'>No</th>";
            headerTbSiteList += "<th style='border:1px solid black;'>Tanggal</th>";
            headerTbSiteList += "<th style='border:1px solid black;'>ISAT</th>";
            headerTbSiteList += "<th style='border:1px solid black;'>Vendor</th></tr>";

            headerSiteListAttach += "<table style='width:100%'>";
            headerSiteListAttach += "<tr>";
            headerSiteListAttach += "<td> <img src='" + Server.MapPath(logoLeftPath) + "' width='" + logoLeftWidth + "px' height='" + logoLeftHeight + "px' />";
            headerSiteListAttach += "</td>";
            headerSiteListAttach += "<td> <div style='text-align:center; line-height:1; font-family:Arial'><h3>LAMPIRAN 1. BERITA ACARA PENGGUNAAN SITE</h3>";
            headerSiteListAttach += "No: ..................../100-10AG/PRJ/BAPS/" + System.DateTime.Now.Year.ToString().Substring(2, 2) + "</div><br/>";
            headerSiteListAttach += "</td>";
            headerSiteListAttach += "<td> <img src='" + Server.MapPath(logoRightPath) + "'  width='" + logoRightWidth + "px' height='" + logoRightHeight + "px' style='float:right' />";
            headerSiteListAttach += "</td>";
            headerSiteListAttach += "</tr>";
            headerSiteListAttach += "</table>";

            qrCodeImage += "<table style='width:100%'>";
            qrCodeImage += "<tr>";
            qrCodeImage += "<td> <img src='" + GenerateQRCode("ISATBut", "ISATBulky") + "' width='" + QrCodeWidth + "px' height='" + QrCodeHeight + "px' style='float:right'/>";
            qrCodeImage += "</td>";
            qrCodeImage += "</tr>";
            qrCodeImage += "</table>";

            int rowPerpage = 35;
            int totalRow = dataList.Count();
            int totalPage = 0;
            int startRow = 1;
            int endRow = rowPerpage;
            if (totalRow > rowPerpage)
            {
                totalPage = totalRow / rowPerpage;
                if ((totalPage * rowPerpage) < totalRow)
                {
                    totalPage += 1;
                }
                for (int i = 1; i <= totalPage; i++)
                {

                    List<ARSystemService.trxRABapsMaterials> dataList2 = new List<ARSystemService.trxRABapsMaterials>();
                    dataList2 = (from a in dataList
                                 where a.RowIndex >= startRow && a.RowIndex <= endRow
                                 select a).ToList();
                    rowDataSiteList = GenerateSiteListData(dataList2);

                    startRow = startRow + rowPerpage;
                    endRow = endRow + rowPerpage;

                    if (i == 1)
                    {
                        htmlElements += borderPage + headerSiteListAttach + headerTbSiteList + rowDataSiteList + "</table></div>";
                        continue;
                    }
                    else if (i == totalPage)
                    {
                        if (dataList2.Count() > 32)
                        {
                            htmlElements += borderPage + headerSiteListAttach + headerTbSiteList + rowDataSiteList + "</table></div>";
                            htmlElements += borderPage + approval + qrCodeImage + "</div>";
                            continue;
                        }
                        else
                        {
                            htmlElements += borderPage + headerTbSiteList + rowDataSiteList + "</table> <br> " + approval + qrCodeImage + "</div>";
                            continue;
                        }
                        //htmlElements += borderPage + headerTbSiteList + rowDataSiteList + "</table> <br> " + approval + qrCodeImage + "</div>";
                        
                    }
                    else
                    {
                        htmlElements += borderPage + headerTbSiteList + rowDataSiteList + "</table></div>";
                        continue;
                    }

                }
            }
            else
            {
                rowDataSiteList = GenerateSiteListData(dataList);
                if (totalRow > 30)
                {

                    htmlElements += borderPage + headerSiteListAttach + headerTbSiteList + rowDataSiteList + "</table></div>";
                    htmlElements += borderPage + approval + qrCodeImage + "</div>";
                }
                else
                {
                    htmlElements += borderPage + headerSiteListAttach + headerTbSiteList + rowDataSiteList + "</table><br />" + approval + qrCodeImage + "</div>";
                }
            }
            return htmlElements;
        }
        private string GenerateMaterialHtml(List<ARSystemService.trxRABapsMaterials> dataList, string approval, string logoLeftPath, string logoRightPath, int logoLeftWidth, int logoLeftHeight, int logoRightWidth, int logoRightHeight, int QrCodeWidth, int QrCodeHeight)
        {
            // html elements for site material list //
            string headerTbMaterial = "";
            string htmlElements = "";
            string rowDataMaterial = "";
            string rowEndDataMaterial = "";
            string headerMaterialAttach = "";
            string qrCodeImage = "";
            string borderPage = "<div style='page-break-after:always; border:2px solid black; padding:10px; 20px; 0px; 20px;'>";
            headerTbMaterial += "<table style='border-collapse:collapse; width:100%'><tr>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center' rowspan='3'>No</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center' rowspan='3'>Item PO</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center' rowspan='3'>Material</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center'  rowspan='3'>Uraian Pekerjaan</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center' rowspan='3'>Unit Net Price</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center' colspan='8'>Volume Pekerjaan / Material</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center' rowspan='3'>Hasil</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center' rowspan='3'>Progress Fisik</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center' rowspan='3'>Keterangan</th></tr><tr>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center' colspan='2'>Kontrak/SPK</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center' colspan='2'>Realisasi Sebelumnya</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center' colspan='2'>Realisasi Saat ini</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center' colspan='2'>Sisa</th></tr><tr>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center'>QTY</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center'>Unit Net Price</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center'>QTY</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center'>Unit Net Price</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center'>QTY</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center'>Unit Net Price</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center'>QTY</th>";
            headerTbMaterial += "<th style='border:1px solid black; text-align:center'>Unit Net Price</th></tr>";

            headerMaterialAttach += "<table style='width:100%'>";
            headerMaterialAttach += "<tr>";
            headerMaterialAttach += "<td> <img src='" + Server.MapPath(logoLeftPath) + "' width='" + logoLeftWidth + "px' height='" + logoLeftHeight + "px' />";
            headerMaterialAttach += "</td>";
            headerMaterialAttach += "<td> <div style='text-align:center; line-height:1; font-family:Arial''><h3>LAMPIRAN 2. BERITA ACARA PENGGUNAAN SITE</h3>";
            headerMaterialAttach += "No: ..................../100-10AG/PRJ/BAPS/" + System.DateTime.Now.Year.ToString().Substring(2, 2) + "</div><br/>";
            headerMaterialAttach += "</td>";
            headerMaterialAttach += "<td> <img src='" + Server.MapPath(logoRightPath) + "'  width='" + logoRightWidth + "px' height='" + logoRightHeight + "px' style='float:right' />";
            headerMaterialAttach += "</td>";
            headerMaterialAttach += "</tr>";
            headerMaterialAttach += "</table>";

            qrCodeImage += "<table style='width:100%'>";
            qrCodeImage += "<tr>";
            qrCodeImage += "<td> <img src='" + GenerateQRCode("ISATBut", "ISATBulky") + "' width='" + QrCodeWidth + "px' height='" + QrCodeHeight + "px' style='float:right'/>";
            qrCodeImage += "</td>";
            qrCodeImage += "</tr>";
            qrCodeImage += "</table>";

            ARSystemService.trxRABapsMaterials model = new ARSystemService.trxRABapsMaterials();

            model.UnitNetPrice = dataList.Sum(x => x.UnitNetPrice);
            model.SpkQty = dataList.Sum(x => x.SpkQty);
            model.SpkUNP = dataList.Sum(x => x.SpkUNP);
            model.RealisationBeforeQty = dataList.Sum(x => x.RealisationBeforeQty);
            model.RealisationBeforeUNP = dataList.Sum(x => x.RealisationBeforeUNP);
            model.RealisationCurrentQty = dataList.Sum(x => x.RealisationCurrentQty);
            model.RealisationCurrentUNP = dataList.Sum(x => x.RealisationCurrentUNP);
            model.TheRestQty = dataList.Sum(x => x.TheRestQty);
            model.TheRestUNP = dataList.Sum(x => x.TheRestUNP);


            rowEndDataMaterial += "<tr>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>Subtotal</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.UnitNetPrice) + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>" + model.SpkQty + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.SpkUNP) + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>" + model.RealisationBeforeQty + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.RealisationBeforeUNP) + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>" + model.RealisationCurrentQty + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.RealisationCurrentUNP) + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>" + model.TheRestQty + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.TheRestUNP) + "</td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "</tr>";

            rowEndDataMaterial += "<tr>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>Discount</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'> 0% </td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'></td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>0</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'></td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>0</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'></td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>0</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'></td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>0</td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "</tr>";

            rowEndDataMaterial += "<tr>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>Price</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'></td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>" + model.SpkQty + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.SpkUNP) + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>" + model.RealisationBeforeQty + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.RealisationBeforeUNP) + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>" + model.RealisationCurrentQty + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.RealisationCurrentUNP) + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>" + model.TheRestQty + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.TheRestUNP) + "</td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "</tr>";

            rowEndDataMaterial += "<tr>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>CAC</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>100%</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>" + model.SpkQty + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.SpkUNP) + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>" + model.RealisationBeforeQty + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.RealisationBeforeUNP) + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>" + model.RealisationCurrentQty + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.RealisationCurrentUNP) + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:center'>" + model.TheRestQty + "</td>";
            rowEndDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(model.TheRestUNP) + "</td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "<td></td>";
            rowEndDataMaterial += "</tr>";

            int rowPerpage = 10;
            int totalPage = 0;
            int startRow = 1;
            int endRow = rowPerpage;
            int totalRow = dataList.Count();

            if (totalRow > rowPerpage)
            {
                totalPage = totalRow / rowPerpage;
                if ((totalPage * rowPerpage) < totalRow)
                {
                    totalPage += 1;
                }
                for (int i = 1; i <= totalPage; i++)
                {

                    List<ARSystemService.trxRABapsMaterials> dataList2 = new List<ARSystemService.trxRABapsMaterials>();
                    dataList2 = (from a in dataList
                                 where a.RowIndex >= startRow && a.RowIndex <= endRow
                                 select a).ToList();
                    rowDataMaterial = GenerateMaterialData(dataList2);
                    startRow = startRow + rowPerpage;
                    endRow = endRow + rowPerpage;
                    if (i == 1)
                    {
                        htmlElements += borderPage + headerMaterialAttach + headerTbMaterial + rowDataMaterial + "</table></div>";
                        continue;
                    }
                    else if (i == totalPage)
                    {
                        if (dataList2.Count() >= rowPerpage)
                        {
                            htmlElements += borderPage + headerTbMaterial + rowDataMaterial + rowEndDataMaterial + "</table></div>";
                            htmlElements += "<div style='border:2px solid black; padding:10px; 20px; 0px; 20px;'>" + approval + qrCodeImage + "</div>";
                            continue;
                        }
                        else
                        {
                            htmlElements += "<div style='border:2px solid black; padding:10px; 20px; 0px; 20px;'>" + headerTbMaterial + rowDataMaterial + rowEndDataMaterial + "</table> <br> " + approval + qrCodeImage + "</div>";
                            continue;
                        }
                        
                    }
                    else
                    {
                        htmlElements += borderPage + headerTbMaterial + rowDataMaterial + "</table></div>";
                        continue;
                    }

                }
            }
            else
            {
                rowDataMaterial = GenerateMaterialData(dataList);
                if (totalRow >= rowPerpage)
                {

                    htmlElements += borderPage + headerMaterialAttach + headerTbMaterial + rowDataMaterial + rowEndDataMaterial + "</table></div>";
                    htmlElements += "<div style='border:2px solid black; padding:10px; 20px; 0px; 20px;'>" + approval + qrCodeImage + "</div></div>";
                }
                else
                {
                    htmlElements += "<div style='border:2px solid black; padding:10px; 20px; 0px; 20px;'>" + headerMaterialAttach + headerTbMaterial + rowDataMaterial + rowEndDataMaterial + "</table><br />" + approval + qrCodeImage + "</div>";
                }
            }
            return htmlElements;
        }
        private string GenerateSiteListData(List<ARSystemService.trxRABapsMaterials> dataList)
        {
            string rowDataSiteList = "";
            string rfsDate = "";
            string checkedDate = "";
            foreach (var item in dataList)
            {
                rfsDate = item.RfsPoDate.ToString() != "" ? Convert.ToDateTime(item.RfsPoDate.ToString()).ToString("dd-MMM-yyyy") : "";
                checkedDate = item.CheckedDate.ToString() != "" ? Convert.ToDateTime(item.CheckedDate.ToString()).ToString("dd-MMM-yyyy") : "";
                rowDataSiteList += "<tr>";
                rowDataSiteList += "<td style='border:1px solid black; text-align:center'>" + item.RowIndex + "</td>";
                rowDataSiteList += "<td style='border:1px solid black; text-align:center'>" + item.PoNumber + "</td>";
                rowDataSiteList += "<td style='border:1px solid black; text-align:center'>" + rfsDate + "</td>";
                rowDataSiteList += "<td style='border:1px solid black; text-align:center'>" + item.SiteNameOpr + "</td>";
                rowDataSiteList += "<td style='border:1px solid black; text-align:center'>" + item.SiteIdOpr + "</td>";
                rowDataSiteList += "<td style='border:1px solid black; text-align:center'>" + checkedDate + "</td>";
                rowDataSiteList += "<td style='border:1px solid black; text-align:center'>" + item.BaufNumber + "</td>";
                rowDataSiteList += "<td style='border:1px solid black'></td>";
                rowDataSiteList += "<td style='border:1px solid black'></td>";
                rowDataSiteList += "<td style='border:1px solid black'></td>";
                rowDataSiteList += "<td style='border:1px solid black'></td>";
                rowDataSiteList += "<td style='border:1px solid black'></td>";
                rowDataSiteList += "</tr>";

            }
            return rowDataSiteList;
        }
        private string GenerateMaterialData(List<ARSystemService.trxRABapsMaterials> dataList)
        {
            string rowDataMaterial = "";

            foreach (var item in dataList)
            {
                rowDataMaterial += "<tr>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:center'>" + item.RowIndex + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; width:3%; text-align:center'>" + item.RowIndex + ".1" + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:center'>" + item.Material + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; width:30%; padding:1px; 5px; 1px; 3px'>" + item.JobDesc + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(item.UnitNetPrice) + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:center'>" + item.SpkQty + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(item.SpkUNP) + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:center'>" + item.RealisationBeforeQty + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(item.RealisationBeforeUNP) + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:center'>" + item.RealisationCurrentQty + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(item.RealisationCurrentUNP) + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:center'>" + item.TheRestQty + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:right'>" + ConvertToMoeny(item.TheRestUNP) + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:center'>" + item.Result + "</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:center'>" + item.PhysicalProgress + " %</td>";
                rowDataMaterial += "<td style='border:1px solid black; text-align:center'>" + item.Description + "</td>";
                rowDataMaterial += "</tr>";

            }
            return rowDataMaterial;
        }
        private string GenerateQRCode(string textQr, string fileName)
        {
            string pathDOc;
            string fileDoc;
            //=====================================================================//
            pathDOc = Server.MapPath(Helper.Helper.GetDocPath() + "RABAPSPrint\\QRCode");
            ThoughtWorks.QRCode.Codec.QRCodeEncoder qrCode = new ThoughtWorks.QRCode.Codec.QRCodeEncoder();
            System.Drawing.Image bitMap;
            //=====================================================================//
            qrCode.QRCodeEncodeMode = ThoughtWorks.QRCode.Codec.QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCode.QRCodeScale = 7;
            qrCode.QRCodeVersion = 0;
            qrCode.QRCodeErrorCorrect = ThoughtWorks.QRCode.Codec.QRCodeEncoder.ERROR_CORRECTION.Q;
            bitMap = qrCode.Encode(textQr);
            // =====================================================================//
            fileDoc = pathDOc + "\\" + fileName;
            if (Directory.Exists(pathDOc) == false)
            {
                Directory.CreateDirectory(pathDOc);
            }
            //  ==== save file ====== //
            if (System.IO.File.Exists(fileDoc) == true)
            {
                System.IO.File.Delete(fileDoc);
                bitMap.Save(fileDoc);
            }
            else
            {
                bitMap.Save(fileDoc);
            }
            bitMap.Dispose();
            return fileDoc;
        }
        private string GenerateHtmlXLBulky(List<ARSystemService.trxRABapsPrintXLBulky> list, string strAppr, string logoLeftPath, int logoLeftWidth, int logoLeftHeight, string logoRightPath, int logoRightWidth, int logoRightHeight, int QrCodeWidth, int QrCodeHeight)
        {
            string htmlString = "";
            try
            {
                // ==== html string ====//
                string htmlDiv = "<div style='width:100%; page-break-after:always'>";
                string headerTable = "<table style='width:99%; border-collapse: collapse; font-size:10px; font-family:Calibri'>";
                headerTable += "<tr>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>No</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>PROJ TYPE</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>EWO</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>PO</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>ITEM</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>PO ITEM</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>PO DATE</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>MATERIAL</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>MATERIAL DESCRIPTION</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>QTY</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>PRICE</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>PO VALUE</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>CRY</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>#ESS</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>SITE ID</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>SITE NAME</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>SITE STATUS</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>ATP DATE</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>PIC FOP</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>REMARK FOP</th>";
                headerTable += "<th style='border: 1px solid black; text-align:center; background-color:Gray'>REMARK MML</th>";
                headerTable += "</tr>";

                string headerPage = "";
                headerPage += "<table style='width:100%'>";
                headerPage += "<tr>";
                headerPage += "<td> <img src='" + Server.MapPath(logoLeftPath) + "' width='" + logoLeftWidth + "px' height='" + logoLeftHeight + "px' />";
                headerPage += "</td>";
                headerPage += "<td> <div style='text-align:center; line-height:1; font-family:Arial'><h3>LAMPIRAN 1. BERITA ACARA PENGGUNAAN SITE</h3>";
                headerPage += "No: ..................../100-10AG/PRJ/BAPS/" + System.DateTime.Now.Year.ToString().Substring(2, 2) + "</div><br/>";
                headerPage += "</td>";
                headerPage += "<td> <img src='" + Server.MapPath(logoRightPath) + "'  width='" + logoRightWidth + "px' height='" + logoRightHeight + "px' style='float:right' />";
                headerPage += "</td>";
                headerPage += "</tr>";
                headerPage += "</table>";

                string qrCodeImage = "";
                qrCodeImage += "<table style='width:100%'>";
                qrCodeImage += "<tr>";
                qrCodeImage += "<td> <img src='" + GenerateQRCode("ISATBut", "ISATBulky") + "' width='" + QrCodeWidth + "px' height='" + QrCodeHeight + "px' style='float:right'/>";
                qrCodeImage += "</td>";
                qrCodeImage += "</tr>";
                qrCodeImage += "</table>";
                //===  end header table ===//
                if (list.Count > 0)
                {
                    int firstRow = 0;
                    int endRow = 0;
                    int rowPerPage = 50;
                    int countData = list.Count();
                    string currency = (from x in list select x.Currency).FirstOrDefault();
                    decimal? sumPoValue = (from x in list select x.PoValue).Sum();
                    string totalPovalue = String.Format("{0:#,#.}", sumPoValue);
                    // === add grand total == //
                    string htmltotalPoValue = "<tr><td style='text-align:right; padding-right:30px' colspan='11'><br /><b>GRAND TOTAL</b></td>";
                    htmltotalPoValue += "<td style='text-align:center;'><br /><b>" + totalPovalue + "</b></td>";
                    htmltotalPoValue += "<td style='text-align:center;'><br /><b>" + currency + "</b></td>";
                    htmltotalPoValue += "<td style='text-align:center;' colspan='8'></td></tr></table>";
                    //============================//
                    if (countData > rowPerPage)
                    {
                        int totalPage = countData / rowPerPage;

                        if ((totalPage * rowPerPage) < countData)
                        {
                            totalPage += 1;
                        }
                        firstRow = 1;
                        endRow = rowPerPage;
                        for (int i = 1; i <= totalPage; i++)
                        {
                            List<ARSystemService.trxRABapsPrintXLBulky> listLq = new List<ARSystemService.trxRABapsPrintXLBulky>();
                            listLq = (from a in list
                                      where a.RowIndex >= firstRow && a.RowIndex <= endRow
                                      select a).ToList();
                            if (i == 1)
                            {
                                htmlString += htmlDiv + headerTable + headerPage + GenerateRowTableXLBulky(listLq) + "</table></div>";
                            }
                            else if (i == totalPage)
                            {
                                if (listLq.Count > 45)
                                {
                                    // === jika halaman terkakhir lebih dari > 15 maka ttd di halaman selanjuta====//

                                    htmlString += htmlDiv + headerTable + GenerateRowTableXLBulky(listLq) + htmltotalPoValue;
                                    htmlString += "</table>";
                                    htmlString += "</div>";
                                    htmlString += "<div style='width:100%'>";
                                    htmlString += strAppr;
                                    htmlString += "</div>";
                                }
                                else
                                {
                                    htmlString += "<div style='width:100%;'>";
                                    htmlString += headerTable + GenerateRowTableXLBulky(listLq) + htmltotalPoValue;
                                    htmlString += strAppr;
                                    htmlString += "</div>";
                                }
                            }
                            else
                            {
                                htmlString += htmlDiv + headerTable + GenerateRowTableXLBulky(listLq) + "</table></div>";
                            }

                            firstRow = firstRow + rowPerPage;
                            endRow = endRow + rowPerPage;
                        }

                    }
                    else //  jika data kurang dari jumlah halaman per page //
                    {
                        if (list.Count > 45)
                        {
                            // === jika halaman terkakhir lebih dari > 15 maka ttd di halaman selanjuta====//

                            htmlString += htmlDiv + headerTable + GenerateRowTableXLBulky(list) + htmltotalPoValue;
                            htmlString += "</div>";
                            htmlString += "<div style='width:100%'>";
                            htmlString += strAppr;
                            htmlString += "</div>";
                        }
                        else
                        {
                            htmlString += "<div style='width:100%;'>";
                            htmlString += headerTable + GenerateRowTableXLBulky(list) + htmltotalPoValue;
                            htmlString += strAppr;
                            htmlString += "</div>";
                        }
                    }

                }
                else // == jika rows = 0 //
                {
                    htmlString += "</table>";
                    htmlString += "</div>";
                }
                return htmlString;
            }
            catch (Exception ex)
            {
                return htmlString = ex.Message;
            }

        }
        private string GenerateRowTableXLBulky(List<ARSystemService.trxRABapsPrintXLBulky> list)
        {
            string result = "";
            foreach (var item in list)
            {
                string rowTable = "";
                string atpDate = "";
                string poDate = "";
                if (item.AtpDate.ToString() != "")
                {
                    atpDate = Convert.ToDateTime(item.AtpDate.ToString()).ToString("dd-MMM-yy");
                }
                if (item.PoDate.ToString() != "")
                {
                    poDate = Convert.ToDateTime(item.PoDate.ToString()).ToString("dd-MMM-yy");
                }
                rowTable += "<tr>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.RowIndex + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.ProjectType + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.Ewo + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.Po + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.Item + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.PoItem + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + poDate + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.Material + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.MaterialDescription + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.Qty + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + String.Format("{0:#,#.}", item.Price) + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + String.Format("{0:#,#.}", item.PoValue) + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.Currency + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.Ess + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.SiteIDCustomer + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.SiteNameCustomer + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.SiteStatus + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + atpDate + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.PicFop + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.RemarkFop + "</td>";
                rowTable += "<td style='border: 1px solid black; text-align:center'>" + item.RemarkMML + "</td>";
                rowTable += "</tr>";
                result = result + rowTable;
            }
            return result;
        }

        protected string ConvertToMoeny(decimal? amount)
        {

            if (amount.ToString() != "0.00" && amount>0)
            {
                var n = decimal.Parse(amount.ToString()).ToString("N2").ToString().Split('.');
                //Comma-fies the first part
                n[0] = n[0].Replace(@"/\B(?= (\d{ 3})+(? !\d))/ g", ",");
                //Combines the two sections
                return n[0]; //String.Join("", n);
            }
            else
            {
                return "";
            }
        }

        //[Route("Download")]
        //public ActionResult DownloadAgreementDoc()
        //{
        //    string path = Request.QueryString["path"];
        //    string fileName = Request.QueryString["fileName"];
        //    string contentType = Request.QueryString["contentType"];
        //    byte[] bytes = System.IO.File.ReadAllBytes(path + fileName);
        //    return File(bytes, contentType, fileName);
        //}

        //[Authorize]
        //[Route("PrintBAPS")]
        //public ActionResult PrintPDF(string SoNumber, string CustomerID, int BulkID, string PrintType, string StipCategory, string ProductType)
        //{
        //    // ======= HTML ELEMENT ======= //
        //    string actionTokenView = "D31A32FD-E2D5-47F7-B5A0-839EA6636DC7";
        //    FileResult file;
        //    using (var client = new SecureAccessService.UserServiceClient())
        //    {
        //        if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
        //        {
        //            ARSystemService.mstRAGeneratorPDF resultHtml = new ARSystemService.mstRAGeneratorPDF();

        //            using (var client2 = new ARSystemService.NewBapsStartBapsServiceClient())
        //            {
        //                resultHtml = client2.GetHtmlElement(UserManager.User.UserToken, SoNumber, CustomerID, StipCategory, BulkID, PrintType, ProductType);

        //                if (PrintType.TrimStart().TrimEnd() == "details" && CustomerID.TrimStart().TrimEnd() == "XL" && StipCategory.TrimStart().TrimEnd() == "NON-ADDITIONAL")
        //                {
        //                    List<ARSystemService.trxRABapsPrintXLBulky> XLBulkyPrint = new List<ARSystemService.trxRABapsPrintXLBulky>();
        //                    XLBulkyPrint = client2.GetListDetailXLBulky(UserManager.User.UserToken, BulkID, 0, 0).ToList();
        //                   // resultHtml.HtmlString = GenerateHtmlXLBulky(XLBulkyPrint, resultHtml.HtmlString);
        //                }

        //                resultHtml.HtmlString = resultHtml.HtmlString == null ? "Template Not Found" : resultHtml.HtmlString;
        //                resultHtml.TextQrCode = resultHtml.TextQrCode == null ? "baps" : resultHtml.TextQrCode;
        //                resultHtml.FileName = resultHtml.FileName == null ? "_baps.pdf" : resultHtml.FileName; ;


        //            }
        //            // ================ set up page pdf ===========================//
        //            HtmlToPdf converter = new HtmlToPdf();
        //            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
        //            if (PrintType.TrimStart().TrimEnd() == "details")
        //            {
        //                converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
        //            }
        //            converter.Options.PdfPageSize = PdfPageSize.A4;
        //            converter.Options.MarginTop = Convert.ToInt16(resultHtml.MarginTop);
        //            converter.Options.MarginLeft = Convert.ToInt16(resultHtml.MarginLeft);
        //            converter.Options.MarginRight = Convert.ToInt16(resultHtml.MarginRight);
        //            converter.Options.MarginBottom = Convert.ToInt16(resultHtml.MarginBottom);
        //            converter.Options.KeepTextsTogether = true;
        //            converter.Options.KeepImagesTogether = true;
        //            converter.Options.DisplayHeader = true;
        //            converter.Header.DisplayOnFirstPage = true;
        //            converter.Header.DisplayOnOddPages = true;
        //            converter.Header.DisplayOnEvenPages = true;
        //            converter.Header.Height = Convert.ToInt16(resultHtml.HeaderHeight);
        //            converter.Options.DisplayFooter = true;
        //            converter.Footer.DisplayOnEvenPages = true;
        //            converter.Footer.DisplayOnFirstPage = true;
        //            converter.Footer.DisplayOnOddPages = true;
        //            converter.Footer.Height = Convert.ToInt16(resultHtml.FooterHeight);
        //            // ==========================================================================//
        //            resultHtml.LogoPathLeft = Server.MapPath(resultHtml.LogoPathLeft);//Server.MapPath("~\\Content\\images\\TBGLogo.png");
        //            resultHtml.LogoPathRight = Server.MapPath(resultHtml.LogoPathRight);// Server.MapPath("~\\Content\\images\\smartFren.png");
        //            // ==== get QR Code ===== //
        //            string qrCodeNumber = SoNumber;
        //            if (SoNumber == null || SoNumber == "")
        //                qrCodeNumber = BulkID.ToString();

        //            string qrCodePath = GenerateQRCode(resultHtml.TextQrCode, "baps_" + qrCodeNumber + ".jpg");
        //            PdfImageSection img = new PdfImageSection(Convert.ToInt16(resultHtml.LogoLeftXpos), Convert.ToInt16(resultHtml.LogoLeftYpos), 0, resultHtml.LogoPathLeft);
        //            img.Width = Convert.ToInt16(resultHtml.LogoLeftWidth);
        //            img.Height = Convert.ToInt16(resultHtml.LogoLeftHeight);
        //            converter.Header.Add(img);
        //            // == right logo ===//
        //            img = new PdfImageSection(Convert.ToInt16(resultHtml.LogoRightXpos), Convert.ToInt16(resultHtml.LogoRightYpos), 0, resultHtml.LogoPathRight);
        //            img.Width = Convert.ToInt16(resultHtml.LogoRightWidth);
        //            img.Height = Convert.ToInt16(resultHtml.LogoRightHeight);
        //            converter.Header.Add(img);
        //            // == left logo === //

        //            img = new PdfImageSection(Convert.ToInt16(resultHtml.QRCodeXpos), Convert.ToInt16(resultHtml.QRCodeYpos), 0, qrCodePath);
        //            img.Width = Convert.ToInt16(resultHtml.QRCodeWidth);
        //            img.Height = Convert.ToInt16(resultHtml.QRCodeHeight);
        //            converter.Footer.Add(img);

        //            if (resultHtml.PageNumberShow == 1)
        //            {
        //                PdfTextSection text = new PdfTextSection(0, 18, "Page: {page_number} of {total_pages}", new System.Drawing.Font("Arial", 6));
        //                text.HorizontalAlign = PdfTextHorizontalAlign.Right;
        //                converter.Footer.Add(text);
        //            }

        //            if (CustomerID.TrimStart().TrimEnd() == "SMART")
        //            {
        //                PdfTextSection text = new PdfTextSection(0, 0, "___________________________________________________________________________________________________", new System.Drawing.Font("Arial", 8));
        //                text.HorizontalAlign = PdfTextHorizontalAlign.Right;
        //                converter.Footer.Add(text);

        //                PdfTextSection text2 = new PdfTextSection(0, 10, resultHtml.FooterText, new System.Drawing.Font("Arial", 8));
        //                text2.HorizontalAlign = PdfTextHorizontalAlign.Right;
        //                converter.Footer.Add(text2);
        //            }

        //            StringBuilder sb = new StringBuilder(resultHtml.HtmlString);
        //            TextWriter tw = new StringWriter(sb);
        //            HtmlTextWriter htw;
        //            htw = new HtmlTextWriter(tw);

        //            PdfDocument pdfDoc = converter.ConvertHtmlString(resultHtml.HtmlString, Request.Url.AbsoluteUri);
        //            byte[] pdf = pdfDoc.Save();
        //            file = new FileContentResult(pdf, "application/pdf");
        //            file.FileDownloadName = resultHtml.FileName + ".pdf";
        //            tw.Dispose();
        //            pdfDoc.Close();
        //            System.Drawing.Image image = System.Drawing.Image.FromFile(qrCodePath);
        //            image.Dispose();
        //            return file;

        //        }
        //        else
        //        {
        //            return RedirectToAction("Logout", "Login");
        //        }

        //    }

        //}

        //[Authorize]
        //[Route("PrintBAPSXlBulky")]
        //public ActionResult PrintPDF(int BulkID, string CustomerID, string PrintType, string StipCategory, string ProductType)
        //{
        //    string actionTokenView = "D31A32FD-E2D5-47F7-B5A0-839EA6636DC7";

        //    PostNewBapsStartBaps model = new PostNewBapsStartBaps();
        //    ARSystemService.mstRAGeneratorPDF resultHtml = new ARSystemService.mstRAGeneratorPDF();
        //    List<ARSystemService.trxRABapsPrintXLBulky> XLBulkyPrint = new List<ARSystemService.trxRABapsPrintXLBulky>();
        //    using (var client = new SecureAccessService.UserServiceClient())
        //    {
        //        if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
        //        {

        //            using (var client2 = new ARSystemService.NewBapsStartBapsServiceClient())
        //            {

        //                resultHtml = client2.GetHtmlElement(UserManager.User.UserToken, "", CustomerID, StipCategory, BulkID, PrintType, ProductType);
        //                XLBulkyPrint = client2.GetListDetailXLBulky(UserManager.User.UserToken, BulkID, 0, 0).ToList();
        //                model.XlBulkyList = XLBulkyPrint;
        //                model.generatePdf = resultHtml;
        //            }

        //        }
        //    }
        //    string header = Url.Action("HeaderPdf", "RaNewBaps", new { Areas = "" }, Request.Url.Scheme);
        //    string footer = Url.Action("FooterPdf", "RaNewBaps", new { Areas = "" }, Request.Url.Scheme);

        //    string customSwitches = string.Format("--header-html {0} --header-spacing 10 --footer-html {1} --header-spacing 10",
        //    header, footer);


        //    //string cusomtSwitches = string.Format("--print-media-type --allow {0} --footer-html {0}",Url.Action("Footer", "Document", new { area = "" }, "https"));

        //    return new Rotativa.ViewAsPdf("~/Views/RaNewBaps/PrintXlBulky.cshtml", model)
        //    {
        //        FileName = resultHtml.FileName,
        //        PageSize = Rotativa.Options.Size.A4,
        //        MinimumFontSize = 7,
        //        PageOrientation = Rotativa.Options.Orientation.Landscape,
        //        PageMargins = { Left = resultHtml.MarginLeft, Right = resultHtml.MarginRight, Top = resultHtml.MarginTop, Bottom = resultHtml.MarginTop },
        //        //CustomSwitches = cusomtSwitches,
        //        CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 12"
        //    };

        //    // return new  Rotativa.ViewAsPdf("~/Views/RaNewBaps/PrintXlBulky.cshtml", model);
        //}
    }
}