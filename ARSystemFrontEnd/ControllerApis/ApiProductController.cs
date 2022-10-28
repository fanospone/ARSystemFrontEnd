using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System.Web;
using System.Collections.Specialized;
using ARSystemFrontEnd.Helper;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/Product")]
    public class ApiProductController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetProductToList(string productName = "", int productTypeId = 0, int isOperator = -1, int productId = 0)
        {
            try
            {
                List<ARSystemService.vmProduct> products = new List<ARSystemService.vmProduct>();
                using (var client = new ARSystemService.ImstProductServiceClient())
                {
                    products = client.GetMstProductToList(UserManager.User.UserToken, productName, productTypeId, productId, isOperator, "ProductId", 0, 0).ToList();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("")]
        public IHttpActionResult CreateProduct()
        {
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files["File"];
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                //ARSystemService.vmProduct product = MappingHelper<ARSystemService.vmProduct>.MappingModel(nvc);
                ARSystemService.vmProduct product = new ARSystemService.vmProduct();
                using (var client = new ARSystemService.ImstProductServiceClient())
                {
                    product = MapModel(product, nvc, postedFile);
                    product = client.CreateProduct(UserManager.User.UserToken, product);
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult UpdateProduct(int id)
        {
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files["File"];
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                ARSystemService.vmProduct product = new ARSystemService.vmProduct();
                using (var client = new ARSystemService.ImstProductServiceClient())
                {
                    product = MapModel(product, nvc, postedFile);
                    product = client.UpdateProduct(UserManager.User.UserToken, id, product);
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetProductToGrid(PostProductView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmProduct> products = new List<ARSystemService.vmProduct>();
                using (var client = new ARSystemService.ImstProductServiceClient())
                {
                    intTotalRecord = client.GetProductCount(UserManager.User.UserToken, post.ProductName, post.ProductTypeId, post.ProductId, post.IsOperator);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    products = client.GetMstProductToList(UserManager.User.UserToken, post.ProductName, post.ProductTypeId, post.ProductId, post.IsOperator, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = products });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("BulkCreate")]
        public IHttpActionResult BulkCreateProduct(List<ARSystemService.vmProduct> products)
        {
            try
            {
                using (var client = new ARSystemService.ImstProductServiceClient())
                {
                    products = client.CreateBulkProduct(UserManager.User.UserToken, products.ToArray()).ToList();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        private ARSystemService.vmProduct MapModel(ARSystemService.vmProduct product, NameValueCollection nvc, HttpPostedFile postedFile)
        {
            string nullString = "null";
            product.ProductName = nvc.Get("ProductName");
            product.ProductTypeId = Convert.ToInt32(nvc.Get("ProductTypeId"));
            product.CompanyID = nvc.Get("CompanyID");

            string customerID = nvc.Get("CustomerID");
            if (customerID != nullString)
                product.CustomerID = Convert.ToInt32(customerID);

            product.IsOperator = Convert.ToBoolean(nvc.Get("IsOperator"));

            if (nvc.Get("OperatorCode") != nullString)
                product.OperatorCode = nvc.Get("OperatorCode");

            if (nvc.Get("OperatorID") != nullString)
                product.OperatorID = nvc.Get("OperatorID");

            product.StartLeaseDate = DateTime.Parse(nvc.Get("StartLeaseDate"));
            product.EndLeaseDate = DateTime.Parse(nvc.Get("EndLeaseDate"));

            if(nvc.Get("ProjectInformations") != nullString && !string.IsNullOrEmpty(nvc.Get("ProjectInformations")))
            {
                List<ARSystemService.vwProjectInformation> projectInformations = new List<ARSystemService.vwProjectInformation>();
                List<string> projectInformationIDs = nvc.Get("ProjectInformations").Split(',').ToList();
                ARSystemService.vwProjectInformation tmp;
                foreach (string projectInformationID in projectInformationIDs)
                {
                    tmp = new ARSystemService.vwProjectInformation();
                    tmp.ProjectInformationID = int.Parse(projectInformationID);
                    projectInformations.Add(tmp);
                }
                product.ProjectInformations = projectInformations.ToArray();
            }

            if(postedFile != null)
            {
                product.AgreementDoc = postedFile.FileName;
                product.FilePath = "\\Product\\" + Helper.Helper.GetFileTimeStamp(postedFile.FileName);
                product.ContentType = postedFile.ContentType;

                postedFile.SaveAs(Helper.Helper.GetDocPath() + product.FilePath);
            }

            return product;
        }
    }
}