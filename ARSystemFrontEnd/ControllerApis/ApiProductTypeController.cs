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
    [RoutePrefix("api/MstProductType")]
    public class ApiProductTypeController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetProductTypeToList(string productType = "", string productCode = "", int isActive = -1)
        {
            try
            {
                List<ARSystemService.mstProductType> productTypes = new List<ARSystemService.mstProductType>();
                using (var client = new ARSystemService.ImstProductTypeServiceClient())
                {
                    productTypes = client.GetMstProductTypeToList(UserManager.User.UserToken, productType, productCode, isActive, "ProductTypeId", 0, 0).ToList();
                }

                return Ok(productTypes);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("")]
        public IHttpActionResult CreateProductType(ARSystemService.mstProductType ProductType)
        {
            try
            {
                using (var client = new ARSystemService.ImstProductTypeServiceClient())
                {
                    ProductType = client.CreateProductType(UserManager.User.UserToken, ProductType);
                }

                return Ok(ProductType);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult UpdateProductType(int id, ARSystemService.mstProductType ProductType)
        {
            try
            {
                using (var client = new ARSystemService.ImstProductTypeServiceClient())
                {
                    ProductType = client.UpdateProductType(UserManager.User.UserToken, id, ProductType);
                }

                return Ok(ProductType);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetProductTypeToGrid(PostProductTypeView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.mstProductType> productTypes = new List<ARSystemService.mstProductType>();
                using (var client = new ARSystemService.ImstProductTypeServiceClient())
                {
                    intTotalRecord = client.GetProductTypeCount(UserManager.User.UserToken, post.productCode, post.productType, post.intIsActive);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    productTypes = client.GetMstProductTypeToList(UserManager.User.UserToken, post.productType, post.productCode, post.intIsActive, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = productTypes });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}