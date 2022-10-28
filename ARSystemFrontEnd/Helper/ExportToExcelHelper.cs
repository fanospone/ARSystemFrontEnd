using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ARSystemFrontEnd.Helper
{
    public static class ExportToExcelHelper
    {
        public static void Export(string strTitle, DataTable dt)
        {
            try
            {
                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(dt, strTitle);

                HttpResponse httpResponse = HttpContext.Current.Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=" + strTitle + ".xlsx");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
            catch(Exception ex)
            {

            }
        }

        public static bool CheckDate(string str)
        {
            try
            {
                DateTime.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ExportExcelMultiSheet(string filename, DataTable dt, int batchSize = 200000)
        {
            try
            {
                OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();

                var dtList = SplitsData(dt, batchSize);
                var num = 1;
                foreach (var item in dtList)
                {
                    var sheetName = num == 1 ? filename : filename + num.ToString();
                    var workSheet = excel.Workbook.Worksheets.Add(sheetName);
                    workSheet.TabColor = System.Drawing.Color.Blue;
                    workSheet.DefaultRowHeight = 12;
                    workSheet.Cells["A1"].LoadFromDataTable(item, true);
                    num++;
                }

                HttpResponse httpResponse = HttpContext.Current.Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=" + filename + ".xlsx");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
            catch (Exception ex)
            {

            }
        }

        private static List<DataTable> SplitsData(DataTable dt, int batcSize)
        {
            int n = 0;
            List<DataTable> dtList = new List<DataTable>();
            while ((dt.AsEnumerable().Skip(n).Take(batcSize).Count() > 0))
            {
                IEnumerable<DataRow> zIE = dt.AsEnumerable().Skip(n).Take(batcSize).AsEnumerable();
                //  Collect 10000 recs
                DataTable zDTtmp = zIE.CopyToDataTable();
                //  Place all in Datatable
                dtList.Add(zDTtmp);
                //  Add to Datasource
                n = (n + batcSize);
            }
            return dtList;
        }
    }
}