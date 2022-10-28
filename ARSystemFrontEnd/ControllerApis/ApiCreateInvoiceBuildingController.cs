using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/CreateInvoiceBuilding")]
    public class ApiCreateInvoiceBuildingController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetInvoiceBuildingDetailToList(string companyName, string invoiceTypeId, string invNo,string InvoiceCategory)
        {
            try
            {
                List<ARSystemService.vwInvoiceBuildingDetail> data = new List<ARSystemService.vwInvoiceBuildingDetail>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    data = client.GetTrxInvoiceBuildingDetailToList(UserManager.User.UserToken, companyName, invoiceTypeId, -1, invNo, "trxInvoiceBuildingDetailID", InvoiceCategory, 0, 0).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetInvoiceBuildingDetailDataToGrid(PostTrxInvoiceBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwInvoiceBuildingDetail> data = new List<ARSystemService.vwInvoiceBuildingDetail>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    intTotalRecord = client.GetInvoiceBuildingDetailCount(UserManager.User.UserToken, post.companyName, post.invoiceTypeId, -1, post.invNo, post.InvoiceCategory);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetTrxInvoiceBuildingDetailToList(UserManager.User.UserToken, post.companyName, post.invoiceTypeId, -1, post.invNo, post.InvoiceCategory, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("")]
        public IHttpActionResult CreateTrxInvoiceBuilding(ARSystemService.vwInvoiceBuildingDetail invoice)
        {
            try
            {
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    invoice = client.CreateInvoiceBuildingDetail(UserManager.User.UserToken, invoice);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpGet, Route("GetProrateTotalPrice")]
        public IHttpActionResult GetProrateTotalPrice( int CatagoryBuildingID, DateTime startDate, DateTime endDate, decimal Area)
        {

            try
            {
                ARSystemService.mstCategoryBuilding data = new ARSystemService.mstCategoryBuilding();
                using (var client = new ARSystemService.ImstCategoryBuildingServiceClient())
                {
                    data = client.GetProrateTotalPrice(UserManager.User.UserToken, CatagoryBuildingID,startDate,endDate, Area);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
    }
}