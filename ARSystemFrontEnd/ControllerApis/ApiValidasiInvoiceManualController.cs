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
    [RoutePrefix("api/ValidasiInvoiceManual")]
    public class ApiValidasiInvoiceManualController : ApiController
    {
        // GET: ApiValidasiInvoiceManual
        [HttpPost, Route("grid")]
        public IHttpActionResult GetValidasiToGrid(PostValidasiInvoiceManual post)
        {
            try
            {
                int intTotalRecord = 0;
                
                List<ARSystemService.mstValidasiInvoiceManual> list = new List<ARSystemService.mstValidasiInvoiceManual>();
                using (var client = new ARSystemService.ImstValidasiInvoiceManualServiceClient())
                {
                    intTotalRecord = client.GetValidasiCount(UserManager.User.UserToken, post.FieldName, post.isMandatory, post.isActive);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    list = client.GetValidasiToList(UserManager.User.UserToken, post.FieldName, post.isMandatory, post.isActive, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Create")]
        public IHttpActionResult CreateValidasi(ARSystemService.mstValidasiInvoiceManual validasiParam)
        {
            try
            {
                using (var client = new ARSystemService.ImstValidasiInvoiceManualServiceClient())
                {
                    validasiParam = client.CreateValidasiInvoiceManual(UserManager.User.UserToken, validasiParam);
                }

                return Ok(validasiParam);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult UpdateValidasi(int id, ARSystemService.mstValidasiInvoiceManual ValidasiParam)
        {
            try
            {
                using (var client = new ARSystemService.ImstValidasiInvoiceManualServiceClient())
                {
                    ValidasiParam = client.UpdateValidasiInvoiceManual(UserManager.User.UserToken, id, ValidasiParam);
                }

                return Ok(ValidasiParam);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}