using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using ARSystem.Service;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("Admin")]
    public class AdminController : BaseController
    {
        [Authorize]
        [Route("UserProfile")]
        public ActionResult UserProfile()
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");

            return View();
        }

        [Authorize]
        [Route("CompanyInformation")]
        public ActionResult CompanyInformation()
        {
            string actionTokenView = "34778365-efb1-463c-bf9a-7bc2700cc226";
            string actionTokenAdd = "45bd8fba-552a-43cd-ba5d-3fc35afdecc5";
            string actionTokenEdit = "2fcae118-a05a-4b7d-a312-439dd7b98bf2";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("MasterReject")]
        public ActionResult MasterReject()
        {
            string actionTokenView = "86565b56-7e77-4cba-a2ad-5e2a300515ff";
            string actionTokenAdd = "1fdc8a97-5b68-4bf7-8d3d-b5d188e1ef3f";
            string actionTokenEdit = "3b259981-e6f1-4782-8d1f-f0670bba3adc";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("Email")]
        public ActionResult Email()
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");

            string actionTokenView = "e5941182-8c99-4d44-8797-96dca87b7efd";
            string actionTokenEdit = "5e9f465f-d2a6-445e-bfd6-c19266e4b9f1";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("ProductType")]
        public ActionResult ProductType()
        {
            string actionTokenView = "de4ca31d-868f-4ce5-bd7b-40d6de28c4e6";
            string actionTokenAdd = "5c9a8627-ddea-4de4-a8df-71310b005cc6";
            string actionTokenEdit = "8bec5bc1-13a2-4229-8b56-661ad4b34872";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("Customer")]
        public ActionResult Customer()
        {
            string actionTokenView = "41fa7e1b-3855-4480-a7dc-ca1a23622f01";
            string actionTokenAdd = "a9695d7f-8b27-4d9c-bbf1-4b0b5fef4a08";
            string actionTokenEdit = "ae1ab644-4757-46f4-a10d-375b1c7c67bc";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("Product")]
        public ActionResult Product()
        {
            string actionTokenView = "37cde9b0-84e9-4534-8877-e8a69df1dcd4";
            string actionTokenAdd = "e46260d4-9a90-49a8-916e-75280dcbbb64";
            string actionTokenEdit = "8a43e510-3286-454b-9097-b471d66f0470";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                    ViewBag.DocPath = Helper.Helper.GetDocPath();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("PICAReprint")]
        public ActionResult PICAReprint()
        {
            string actionTokenView = "6e9b1423-2ef8-4068-9848-9987a006c004";
            string actionTokenAdd = "891e715c-262f-42e0-8ce8-4bff138e7c72";
            string actionTokenEdit = "3c227d0c-97f8-4dd2-8138-c657ecd80f06";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("TaxInvoice")]
        public ActionResult TaxInvoice()
        {
            string actionTokenView = "e505daa6-6d38-4ea1-b8fd-2dac5105835d";
            string actionTokenAdd = "f7fd7b9e-339e-4630-a0d7-71fc87f7d8f8";
            string actionTokenEdit = "f62e4da2-a095-4095-bf02-e5392da7f8ae";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("Kurs")]
        public ActionResult Kurs()
        {
            string actionTokenView = "54fd5cd9-4f8f-4575-8991-89d6be0af7c7";
            string actionTokenAdd = "515afb41-689e-4576-a4bf-d5dd554b17bd";
            string actionTokenEdit = "7bdcbd5b-1016-416b-abf1-8a0fc88633d0";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("InvoiceDeduction")]
        public ActionResult InvoiceDeduction()
        {
            string actionTokenView = "e868def1-d495-4d4f-a44c-14b2fdbc428e";
            string actionTokenAdd = "c7c38b51-54da-4e9d-98fa-0e169cbdc607";
            string actionTokenEdit = "17166a8f-649c-4e4a-a8c4-5b4d2fcc2754";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                    ViewBag.DocPath = Helper.Helper.GetDocPath();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("PaymentBank")]
        public ActionResult PaymentBank()
        {
            string actionTokenView = "bc56e35a-483f-49b5-9fa3-6b49a775fcb0";
            string actionTokenAdd = "7431eda4-759f-4119-ac0b-90b80313bc16";
            string actionTokenEdit = "85a4bfc2-88a1-4879-abd5-cbed08df757a";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                    //Filter Company PKP Purpose
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("MasterApprovalBAPS")]
        public ActionResult ApprovalBAPS()
        {
            string actionTokenView = "3EC7B908-C442-44D3-AC81-F2B8AB90B054";
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    //ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("MappingRevenue")]
        public ActionResult MappingRevenue()
        {
            string actionTokenView = "4bc4d929-5927-49ea-a940-fb66823b4b8a";
            string actionTokenAdd = "c0463f2e-86b4-4005-a3cb-dcc083c7717e";
            string actionTokenEdit = "4d3b4cb9-d028-49db-a155-9774ded2fb50";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("ValidasiInvoiceManual")]
        public ActionResult ValidasiInvoiceManual()
        {
            string actionTokenView = "a5e0fe48-1f72-4ddb-a7f1-fd6df8534019";
            string actionTokenAdd = "9eeff619-07fc-414c-84e8-fd5969ae8651";
            string actionTokenEdit = "38881130-d487-4f2a-8b7e-c3623c9d1204";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("MappingOperator")]
        public ActionResult MappingOperator()
        {
            string actionTokenView = "edd3981b-dcf1-4e66-8ff8-8ed15c7e30eb";
            string actionTokenAdd = "e2e20394-bf5a-4211-8900-cb75d655a5eb";
            string actionTokenEdit = "e356cd6f-986b-43b5-8ac3-1f953113dcb8";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        /* edit By MTR */
        [Authorize]
        [Route("CategoryBuilding")]
        public ActionResult CategoryBuilding()
        {
            string actionTokenView = "EDE0C2C8-C976-49EB-B601-11FA14F50EA6";
            string actionTokenAdd = "6E5B1F5C-F4E2-491D-9D51-653E10096FAE";
            string actionTokenEdit = "23897C81-2976-4D44-8525-CF8CB77C76C6";
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("RESlip")]
        public ActionResult RESlip()
        {
            string actionTokenView = "B5FB335D-F7C1-4354-92A2-1C024BEABB1A";
            string actionTokenAdd = "A0FBB471-A954-412C-AE16-8BA8B9D32B6F";
            string actionTokenEdit = "554F6960-96B5-44D6-8927-A5DC7BE264C9";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("DataSourceDashboard")]

        public ActionResult DataSourceDashboard()
        {
            string actionTokenView = "45A3E284-45B0-4C5D-BE10-B4993396E0BF";
            string actionTokenAdd = "A0FBB471-A954-412C-AE16-8BA8B9D32B6F";
            string actionTokenEdit = "554F6960-96B5-44D6-8927-A5DC7BE264C9";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        /* edit By MTR */

        #region Export to Excel

        [Route("Customer/Export")]
        public void GetCustomerTenantToExport()
        {
            //Parameter
            string customerName = Request.QueryString["customerName"];
            int customerTypeId = int.Parse(Request.QueryString["customerType"]);
            int isActive = int.Parse(Request.QueryString["status"]);

            //Call Service
            List<ARSystemService.vmCustomerTenant> list = new List<ARSystemService.vmCustomerTenant>();
            using (var client = new ARSystemService.ImstCustomerTenantServiceClient())
            {
                int intTotalRecord = client.GetCustomerTenantCount(UserManager.User.UserToken, customerName, customerTypeId, isActive, string.Empty, 0);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vmCustomerTenant> listHolder = new List<ARSystemService.vmCustomerTenant>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetMstCustomerTenantToList(UserManager.User.UserToken, customerName, customerTypeId, isActive, string.Empty, 0, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "CustomerName",
                "CustomerType",
                "LegalAddress",
                "LegalProvince",
                "LegalRegion",
                "BillingAddress",
                "BillingProvince",
                "BillingRegion",
                "PhoneNumber",
                "Email",
                "NPWP",
                "ContactPerson"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("Customers", table);
        }

        [Route("ProductType/Export")]
        public void GetProductTypeToExport()
        {
            //Parameter
            string productCode = Request.QueryString["productCode"];
            string productType = Request.QueryString["productType"];
            int isActive = int.Parse(Request.QueryString["status"]);

            //Call Service
            List<ARSystemService.mstProductType> list = new List<ARSystemService.mstProductType>();
            using (var client = new ARSystemService.ImstProductTypeServiceClient())
            {
                int intTotalRecord = client.GetProductTypeCount(UserManager.User.UserToken, productCode, productType, isActive);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.mstProductType> listHolder = new List<ARSystemService.mstProductType>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetMstProductTypeToList(UserManager.User.UserToken, productCode, productType, isActive, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "ProductCode",
                "ProductType",
                "EffectiveDate",
                "RelatedToSonumb",
                "IsActive",
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ProductTypes", table);
        }

        [Route("Product/Export")]
        public void GetProductToExport()
        {
            //Parameter
            string productName = Request.QueryString["productCode"];
            int productTypeId = int.Parse(Request.QueryString["productTypeId"]);
            int isOperator = int.Parse(Request.QueryString["isOperator"]);

            //Call Service
            List<ARSystemService.vmProduct> list = new List<ARSystemService.vmProduct>();
            using (var client = new ARSystemService.ImstProductServiceClient())
            {
                int intTotalRecord = client.GetProductCount(UserManager.User.UserToken, productName, productTypeId, 0, isOperator);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vmProduct> listHolder = new List<ARSystemService.vmProduct>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetMstProductToList(UserManager.User.UserToken, productName, productTypeId, 0, isOperator, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "ProductType",
                "ProductName",
                "RelatedToSonumb",
                "CustomerCategory",
                "CustomerType",
                "CustomerName"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("Products", table);
        }

        [Route("CompanyInformation/Export")]
        public void GetCompanyInformationToExport()
        {
            //Parameter
            string strCompany = Request.QueryString["strCompany"];
            string strTerm = Request.QueryString["strTerm"];
            string strCompanyType = Request.QueryString["strCompanyType"];
            int intIsActive = int.Parse(Request.QueryString["intIsActive"]);

            //Call Service
            List<ARSystemService.mstCompanyInformation> listCompanyInformation = new List<ARSystemService.mstCompanyInformation>();
            using (var client = new ARSystemService.ImstCompanyInformationServiceClient())
            {
                int intTotalRecord = client.GetCompanyInformationCount(UserManager.User.UserToken, strCompany, strTerm, intIsActive, strCompanyType, 0);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.mstCompanyInformation> listCompanyInformationHolder = new List<ARSystemService.mstCompanyInformation>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listCompanyInformationHolder = client.GetMstCompanyToList(UserManager.User.UserToken, strCompany, strTerm, intIsActive, strCompanyType, 0, null, 50 * i, 50).ToList();
                    listCompanyInformation.AddRange(listCompanyInformationHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { "Tenant", "Company", "TermPeriod", "IsActive" };
            var reader = FastMember.ObjectReader.Create(listCompanyInformation.Select(i => new
            {
                Tenant = i.CompanyType + " " + i.Tenant,
                i.Company,
                i.TermPeriod,
                i.IsActive
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("CompanyInformation", table);
        }

        [Route("MasterRejectHdr/Export")]
        public void GetMasterRejectHdrToExport()
        {
            //Parameter
            string strHdr = Request.QueryString["strHdr"];
            string UserRole = "";
            int intIsActive = int.Parse(Request.QueryString["intIsActive"]);
            int mstUserGroupId = int.Parse(Request.QueryString["mstUserGroupId"]);

            //Call Service
            List<ARSystemService.vmMstRejectHdrDtl> listMasterReject = new List<ARSystemService.vmMstRejectHdrDtl>();
            using (var client = new ARSystemService.ImstRejectServiceClient())
            {
                int intTotalRecord = client.GetRejectHdrCount(UserManager.User.UserToken, strHdr, intIsActive, mstUserGroupId);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vmMstRejectHdrDtl> listMasterRejectHdrHolder = new List<ARSystemService.vmMstRejectHdrDtl>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listMasterRejectHdrHolder = client.GetMstHdrDtlToList(UserManager.User.UserToken, strHdr, intIsActive, mstUserGroupId, null, 50 * i, 50).ToList();
                    listMasterReject.AddRange(listMasterRejectHdrHolder);
                }
            }
            using (var client = new ARSystemService.UserServiceClient())
            {
                UserRole = client.GetARUserPosition(UserManager.User.UserToken).Result;
            }
            //Convert to DataTable
            DataTable table = new DataTable();
            FastMember.ObjectReader reader;

            string[] ColumsShow = new string[] { "Description", "Recipient", "CC", "isActive" };
            if (UserRole == "AR COLLECTION" || UserRole == "AR MONITORING")
            {
                reader = FastMember.ObjectReader.Create(listMasterReject.Select(i => new
                {
                    i.Description,
                    i.isActive
                }));
            }
            else
            {
                reader = FastMember.ObjectReader.Create(listMasterReject.Select(i => new
                {
                    i.Description,
                    i.Recipient,
                    i.CC,
                    i.isActive

                }), ColumsShow);
            }

            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("MasterRejectHdr", table);
        }

        [Route("MasterRejectDtl/Export")]
        public void GetMasterRejectDtlToExport()
        {
            //Parameter
            string strHdrId = Request.QueryString["strHdrId"];

            //Call Service
            List<ARSystemService.mstPICADetail> listMasterRejectDtl = new List<ARSystemService.mstPICADetail>();
            using (var client = new ARSystemService.ImstRejectServiceClient())
            {
                int intTotalRecord = client.GetRejectDtlCount(UserManager.User.UserToken, strHdrId);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.mstPICADetail> listMasterRejectDtlHolder = new List<ARSystemService.mstPICADetail>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listMasterRejectDtlHolder = client.GetMstRejectDtlToList(UserManager.User.UserToken, strHdrId, null, 50 * i, 50).ToList();
                    listMasterRejectDtl.AddRange(listMasterRejectDtlHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(listMasterRejectDtl.Select(i => new
            {
                i.Description,
                i.IsActive
            }));
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("MasterRejectDtl", table);
        }

        [Route("PICAReprint/Export")]
        public void GetPICAReprintToExport()
        {
            //Parameter
            string picaReprint = Request.QueryString["picaReprint"];
            int isActive = int.Parse(Request.QueryString["status"]);

            //Call Service
            List<ARSystemService.mstPICAReprint> list = new List<ARSystemService.mstPICAReprint>();
            using (var client = new ARSystemService.ImstPICAReprintServiceClient())
            {
                int intTotalRecord = client.GetPICAReprintCount(UserManager.User.UserToken, picaReprint, isActive);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.mstPICAReprint> listHolder = new List<ARSystemService.mstPICAReprint>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetMstPICAReprintToList(UserManager.User.UserToken, picaReprint, isActive, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "PICAReprint",
                "IsActive",
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("PICAReprints", table);
        }

        [Route("TaxInvoice/Export")]
        public void GetTaxInvoiceToExport()
        {
            //Parameter
            string invNo = Request.QueryString["invNo"];
            string taxInvoiceNo = Request.QueryString["taxInvoiceNo"];

            //Call Service
            List<ARSystemService.vwTaxInvoice> list = new List<ARSystemService.vwTaxInvoice>();
            using (var client = new ARSystemService.ImstTaxInvoiceServiceClient())
            {
                int intTotalRecord = client.GetTaxInvoiceCount(UserManager.User.UserToken, invNo, taxInvoiceNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwTaxInvoice> listHolder = new List<ARSystemService.vwTaxInvoice>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetMstTaxInvoiceToList(UserManager.User.UserToken, invNo, taxInvoiceNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }


            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"InvNo","TaxInvoiceNo","TaxInvoiceDate"
            };
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                InvNo = i.InvNo,
                TaxInvoiceNo = i.TaxInvoiceNo,
                TaxInvoiceDate = (i.TaxInvoiceDate.HasValue) ? DateTime.Parse(i.TaxInvoiceDate.ToString()).ToString("dd-MMM-yyyy") : ""

            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TaxInvoices", table);
        }

        [Route("Kurs/Export")]
        public void GetKursToExport()
        {
            //Call Service
            List<ARSystemService.mstKurs> list = new List<ARSystemService.mstKurs>();
            using (var client = new ARSystemService.ImstKursServiceClient())
            {
                int intTotalRecord = client.GetKursCount(UserManager.User.UserToken, null);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.mstKurs> listHolder = new List<ARSystemService.mstKurs>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetMstKursToList(UserManager.User.UserToken, "KursDate DESC", 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "KursDate",
                "KursTengahBI",
                "KursPajak"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("MasterKurs", table);
        }

        [Route("InvoiceDeductionSKB/Export")]
        public void GetInvoiceDeductionSKBToExport()
        {
            int deductionTypeId = int.Parse(Request.QueryString["deductionTypeId"]);
            string companyId = Request.QueryString["companyId"];
            string operatorId = Request.QueryString["operatorId"];

            //Call Service
            List<ARSystemService.vwInvoiceDeduction> list = new List<ARSystemService.vwInvoiceDeduction>();
            using (var client = new ARSystemService.ImstInvoiceDeductionServiceClient())
            {
                int intTotalRecord = client.GetInvoiceDeductionCount(UserManager.User.UserToken, deductionTypeId, companyId, operatorId);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwInvoiceDeduction> listHolder = new List<ARSystemService.vwInvoiceDeduction>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetMstInvoiceDeductionToList(UserManager.User.UserToken, deductionTypeId, companyId, operatorId, "mstInvoiceDeductionId DESC", 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "CompanyId",
                "StartPeriod",
                "EndPeriod"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("InvoiceDeduction(SKB)", table);
        }

        [Route("InvoiceDeductionWAPU/Export")]
        public void GetInvoiceDeductionWAPUToExport()
        {
            int deductionTypeId = int.Parse(Request.QueryString["deductionTypeId"]);
            string companyId = Request.QueryString["companyId"];
            string operatorId = Request.QueryString["operatorId"];

            //Call Service
            List<ARSystemService.vwInvoiceDeduction> list = new List<ARSystemService.vwInvoiceDeduction>();
            using (var client = new ARSystemService.ImstInvoiceDeductionServiceClient())
            {
                int intTotalRecord = client.GetInvoiceDeductionCount(UserManager.User.UserToken, deductionTypeId, companyId, operatorId);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwInvoiceDeduction> listHolder = new List<ARSystemService.vwInvoiceDeduction>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetMstInvoiceDeductionToList(UserManager.User.UserToken, deductionTypeId, companyId, operatorId, "mstInvoiceDeductionId DESC", 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "OperatorId",
                "AmountWAPU"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("InvoiceDeduction(WAPU)", table);
        }

        [Route("PaymentBank/Export")]
        public void GetPaymentBankToExport()
        {
            string companyId = Request.QueryString["companyId"];
            string bankGroupId = Request.QueryString["bankGroupId"];
            string accountNo = Request.QueryString["accountNo"];

            if (UserManager.User.CompanyCode == Constants.CompanyCode.PKP)
            {
                companyId = Constants.CompanyCode.PKP;
            }

            //Call Service
            List<ARSystemService.mstPaymentBank> list = new List<ARSystemService.mstPaymentBank>();
            using (var client = new ARSystemService.ImstPaymentBankServiceClient())
            {
                int intTotalRecord = client.GetPaymentBankCount(UserManager.User.UserToken, bankGroupId, companyId, accountNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.mstPaymentBank> listHolder = new List<ARSystemService.mstPaymentBank>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetPaymentBankToList(UserManager.User.UserToken, bankGroupId, companyId, accountNo, "mstPaymentBankId DESC", 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "CompanyId",
                "AccountId",
                "AccountName",
                "AccountNum",
                "BankGroupId",
                "Address",
                "Currency",
                "IsActive"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("MasterPaymentBank", table);
        }

        /* edt by MTR */
        [Authorize]
        [Route("CategoryBuilding/Export")]
        public void CategoryBuildingExport()
        {

            //Parameter
            string strCategoryBuilding = Request.QueryString["strCategoryBuilding"];

            int intTotalRecord = 0;
            List<ARSystemService.mstCategoryBuilding> repoList = new List<ARSystemService.mstCategoryBuilding>();
            ARSystemService.mstCategoryBuilding model = new ARSystemService.mstCategoryBuilding();

            using (var client = new ARSystemService.ImstCategoryBuildingServiceClient())
            {
                model.CategoryBuilding = strCategoryBuilding;
                intTotalRecord = client.GetCountCategoryBuilding(UserManager.User.UserToken, model);

                int intBatch = intTotalRecord / 50;
                for (int i = 0; i <= intBatch; i++)
                {

                    repoList = client.GetListCategoryBuilding(UserManager.User.UserToken, model, 50 * i, 50).ToList();
                }

            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { "CategoryBuilding", "PPHType", "PPNType" };
            var reader = FastMember.ObjectReader.Create(repoList.Select(i => new
            {
                i.CategoryBuilding,
                i.PPHType,
                i.PPNType
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("CategoryBuilding", table);

        }

        [Authorize]
        [Route("RESlip/Export")]
        public void RESlipExport()
        {

            //Parameter
            string strSONumber = Request.QueryString["strSONumber"];

            int intTotalRecord = 0;
            List<ARSystemService.mstRESlip> repoList = new List<ARSystemService.mstRESlip>();
            ARSystemService.mstRESlip model = new ARSystemService.mstRESlip();

            using (var client = new ARSystemService.ImstRESlipServiceClient())
            {

                intTotalRecord = client.GetRESlipCount(UserManager.User.UserToken, strSONumber);

                int intBatch = intTotalRecord / 50;
                for (int i = 0; i <= intBatch; i++)
                {

                    repoList = client.GetMstRESlipToList(UserManager.User.UserToken, strSONumber, 50 * i, 50).ToList();
                }

            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { "SONumber", "RESlip" };
            var reader = FastMember.ObjectReader.Create(repoList.Select(i => new
            {
                i.SONumber,
                i.RESlip,
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("RESlip", table);

        }

        [Route("RESlip/ImportExcel")]
        public ActionResult RESlipImportExcel()
        {

            var ErrorMsg = "";
            try
            {
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
                List<ARSystemService.mstRESlip> data = new List<ARSystemService.mstRESlip>();
                ARSystemService.mstRESlip temp;
                string sheetName = string.Empty;
                string soNumber = string.Empty;
                int startIndex = 1;
                IRow row;
                ICell cell;
                for (int i = startIndex; i <= sheet.LastRowNum; i++) //Loop the records upto filled row, starts from content row (row 3)
                {
                    if (sheet.GetRow(i) != null) //null is when the row only contains empty cells   
                    {
                        temp = new ARSystemService.mstRESlip();
                        row = sheet.GetRow(i);
                        if (row != null)
                        {
                            cell = row.GetCell(1);
                            if (cell != null)
                            {
                                temp.SONumber = sheet.GetRow(i).GetCell(1).StringCellValue;
                                if (temp.SONumber.TrimStart().TrimEnd() == "" || temp.SONumber == null)
                                {
                                    ErrorMsg ="Number "+ sheet.GetRow(i).GetCell(0).StringCellValue+ " SO Number is empty!";
                                    return Json(ErrorMsg);
                                }
                            }else
                            {
                                ErrorMsg = "Number " + sheet.GetRow(i).GetCell(0).StringCellValue +" SO Number is empty!";
                                return Json(ErrorMsg);
                            }

                            cell = row.GetCell(2);
                            if (cell != null)
                            {
                                temp.RESlip = sheet.GetRow(i).GetCell(2).StringCellValue;
                                if (temp.RESlip.TrimStart().TrimEnd() == "" || temp.RESlip == null)
                                {
                                    ErrorMsg = "Number " + sheet.GetRow(i).GetCell(0).StringCellValue + " RE Slip is empty!";
                                    return Json(ErrorMsg);
                                }
                            }
                            else
                            {
                                ErrorMsg = "Number " + sheet.GetRow(i).GetCell(0).StringCellValue + " RE Slip is empty!";
                                return Json(ErrorMsg);
                            }

                            if (!string.IsNullOrEmpty(temp.SONumber) && !string.IsNullOrEmpty(temp.RESlip))
                                data.Add(temp);
                        }
                        else
                        {
                            ErrorMsg = "Data is empty!";
                            return Json(ErrorMsg);
                        }
                    }
                }
                using (var client = new ARSystemService.ImstRESlipServiceClient())
                {
                    data = client.CreateBulkyRESlip(UserManager.User.UserToken, data.ToArray()).ToList();
                    var a = data.FirstOrDefault();
                    ErrorMsg = "";
                    if (a !=null && a.ErrorType > 0)
                        ErrorMsg = a.ErrorMessage;

                    return Json(ErrorMsg);
                }

                //return Json(data, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "Upload is failed!";
                return Json(ErrorMsg);
            }
        }

        [Route("ValidasiInvoiceManual/Export")]
        public void GetValidasiInvoiceManualToExport()
        {
            //Parameter
            string FieldName = Request.QueryString["FieldName"];
            int IsMandatory = int.Parse(Request.QueryString["IsMandatory"]);
            int IsActive = int.Parse(Request.QueryString["IsActive"]);

            //Call Service
            List<ARSystemService.mstValidasiInvoiceManual> list = new List<ARSystemService.mstValidasiInvoiceManual>();
            using (var client = new ARSystemService.ImstValidasiInvoiceManualServiceClient())
            {
                int intTotalRecord = client.GetValidasiCount(UserManager.User.UserToken, FieldName, IsMandatory, IsActive);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.mstValidasiInvoiceManual> listHolder = new List<ARSystemService.mstValidasiInvoiceManual>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetValidasiToList(UserManager.User.UserToken, FieldName, IsMandatory, IsActive, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "FieldName",
                "isMandatory",
                "isActive"

            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ValidasiInvoiceManual", table);
        }

        //[Route("MappingOperator/Export")]
        //public void GetMappingOperatorToExport()
        //{
        //    //Parameter
        //    string OperatorReport = Request.QueryString["OperatorReport"];
        //    string OperatorID = Request.QueryString["OperatorID"];
        //    int IsActive = int.Parse(Request.QueryString["IsActive"]);

        //    //Call Service
        //    List<ARSystemService.mstMappingOperatorReport> list = new List<ARSystemService.mstMappingOperatorReport>();
        //    using (var client = new ARSystemService.ImstMappingOperatorReportServiceClient())
        //    {
        //        int intTotalRecord = client.GetMappingOperatorCount(UserManager.User.UserToken, OperatorReport, OperatorID, IsActive);
        //        int intBatch = intTotalRecord / 50;
        //        List<ARSystemService.mstMappingOperatorReport> listHolder = new List<ARSystemService.mstMappingOperatorReport>();

        //        for (int i = 0; i <= intBatch; i++)
        //        {
        //            listHolder = client.GetMappingOperatorToList(UserManager.User.UserToken, OperatorReport, OperatorID, IsActive, null, 50 * i, 50).ToList();
        //            list.AddRange(listHolder);
        //        }
        //    }

        //    //Convert to DataTable
        //    string[] fieldList = new string[] {
        //        "OperatorReport",
        //        "OperatorId",
        //        "IsActive"

        //    };
        //    DataTable table = new DataTable();
        //    var reader = FastMember.ObjectReader.Create(list, fieldList);
        //    table.Load(reader);

        //    //Export to Excel
        //    ExportToExcelHelper.Export("MappingOperator", table);
        //}



        //[Route("MappingRevenue/Export")]
        //public void MappingRevenueToExport()
        //{
        //    //Parameter
        //    string STIPID = Request.QueryString["STIPID"];
        //    string BapsType = Request.QueryString["BapsType"];
        //    string RevenueType = Request.QueryString["RevenueType"];
        //    int IsActive = int.Parse(Request.QueryString["IsActive"]);

        //    //Call Service
        //    List<ARSystemService.vwmstMappingRevenue> list = new List<ARSystemService.vwmstMappingRevenue>();
        //    using (var client = new ARSystemService.ImstMappingRevenueServiceClient())
        //    {
        //        int intTotalRecord = client.GetMappingRevenueCount(UserManager.User.UserToken, STIPID, BapsType, RevenueType, IsActive);
        //        int intBatch = intTotalRecord / 50;
        //        List<ARSystemService.vwmstMappingRevenue> listHolder = new List<ARSystemService.vwmstMappingRevenue>();

        //        for (int i = 0; i <= intBatch; i++)
        //        {
        //            listHolder = client.GetMappingRevenueToList(UserManager.User.UserToken, STIPID, BapsType, RevenueType, IsActive, null, 50 * i, 50).ToList();
        //            list.AddRange(listHolder);
        //        }
        //    }

        //    //Convert to DataTable
        //    string[] fieldList = new string[] {
        //        "StipCode",
        //        "BapsType",
        //        "RevenueType",
        //        "IsActive"

        //    };
        //    DataTable table = new DataTable();
        //    var reader = FastMember.ObjectReader.Create(list, fieldList);
        //    table.Load(reader);

        //    //Export to Excel
        //    ExportToExcelHelper.Export("MappingRevenue", table);
        //}

        #endregion

        #region Product Data Excel Bulk Upload and Download Template

        [Route("Product/DownloadExcel")]
        public void DownloadProductExcel()
        {
            string filePath = "~/Content/templates/Product Excel Template.xlsx";
            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath(filePath));
            Stream stream = new MemoryStream(bytes);
            ISheet sheet;
            IRow row;

            XSSFWorkbook wb = new XSSFWorkbook(stream); //XSSFWorkBook will read 2007 Excel format 
            int startRowIndex = 2;
            if (wb != null)
            {
                #region Define Cell Style
                ICellStyle style = wb.CreateCellStyle();
                style.BorderTop = BorderStyle.Thin;
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.WrapText = true;
                style.VerticalAlignment = VerticalAlignment.Top;

                ICellStyle headerStyle = wb.CreateCellStyle();
                headerStyle.FillForegroundColor = IndexedColors.Yellow.Index;
                headerStyle.FillPattern = FillPattern.SolidForeground;
                headerStyle.BorderTop = BorderStyle.Thin;
                headerStyle.BorderBottom = BorderStyle.Thin;
                headerStyle.BorderLeft = BorderStyle.Thin;
                headerStyle.BorderRight = BorderStyle.Thin;
                headerStyle.WrapText = true;
                headerStyle.VerticalAlignment = VerticalAlignment.Top;
                #endregion

                sheet = wb.GetSheetAt(0);
                if (sheet != null)
                {
                    #region Generate Master Data Table in Excel

                    List<ARSystemService.mstProductType> productTypes = new List<ARSystemService.mstProductType>();
                    List<ARSystemService.mstCompany> companies = new List<ARSystemService.mstCompany>();
                    List<ARSystemService.mstOperator> operators = new List<ARSystemService.mstOperator>();
                    List<ARSystemService.mstCustomer> customers = new List<ARSystemService.mstCustomer>();

                    #region Product Type
                    // Populate Product Type List
                    int productTypeIndex = startRowIndex;
                    using (var client = new ARSystemService.ImstProductTypeServiceClient())
                    {
                        productTypes = client.GetMstProductTypeToList(UserManager.User.UserToken, "", "", 1, "ProductTypeId", 0, 0).ToList();

                        int productTypeIdCellIndex = 11;
                        int productTypeCellIndex = 12;
                        int relatedToSoNumberCellIndex = 13;
                        ICell productTypeIdCell;
                        ICell productTypeCell;
                        ICell relatedToSoNumberCell;
                        foreach (ARSystemService.mstProductType pt in productTypes)
                        {
                            row = sheet.GetRow(productTypeIndex);
                            if (row == null)
                                row = sheet.CreateRow(productTypeIndex);

                            productTypeIdCell = sheet.GetRow(productTypeIndex).GetCell(productTypeIdCellIndex);
                            if (productTypeIdCell == null)
                                productTypeIdCell = row.CreateCell(productTypeIdCellIndex);

                            productTypeIdCell.SetCellValue(pt.ProductTypeId);
                            productTypeIdCell.CellStyle = style;

                            productTypeCell = sheet.GetRow(productTypeIndex).GetCell(productTypeCellIndex);
                            if (productTypeCell == null)
                                productTypeCell = row.CreateCell(productTypeCellIndex);

                            productTypeCell.SetCellValue(pt.ProductType);
                            productTypeCell.CellStyle = style;

                            relatedToSoNumberCell = sheet.GetRow(productTypeIndex).GetCell(relatedToSoNumberCellIndex);
                            if (relatedToSoNumberCell == null)
                                relatedToSoNumberCell = row.CreateCell(relatedToSoNumberCellIndex);

                            relatedToSoNumberCell.SetCellValue(pt.RelatedToSonumb.Value);
                            relatedToSoNumberCell.CellStyle = style;

                            productTypeIndex++;
                        }
                    }
                    #endregion

                    #region Is Operator Guide

                    int operatorGuideIndex = productTypeIndex + 1;
                    int operatorGuideCellIndex = 11;

                    row = sheet.GetRow(operatorGuideIndex);
                    if (row == null)
                        row = sheet.CreateRow(operatorGuideIndex);

                    ICell operatorGuideCell = sheet.GetRow(operatorGuideIndex).GetCell(operatorGuideCellIndex);
                    if (operatorGuideCell == null)
                        operatorGuideCell = row.CreateCell(operatorGuideCellIndex);

                    operatorGuideCell.CellStyle = headerStyle;
                    operatorGuideCell.SetCellValue("Is Operator");

                    operatorGuideIndex++;
                    row = sheet.GetRow(operatorGuideIndex);
                    if (row == null)
                        row = sheet.CreateRow(operatorGuideIndex);

                    operatorGuideCell = sheet.GetRow(operatorGuideIndex).GetCell(operatorGuideCellIndex);
                    if (operatorGuideCell == null)
                        operatorGuideCell = row.CreateCell(operatorGuideCellIndex);

                    operatorGuideCell.CellStyle = style;
                    operatorGuideCell.SetCellValue(true);

                    operatorGuideIndex++;
                    row = sheet.GetRow(operatorGuideIndex);
                    if (row == null)
                        row = sheet.CreateRow(operatorGuideIndex);

                    operatorGuideCell = sheet.GetRow(operatorGuideIndex).GetCell(operatorGuideCellIndex);
                    if (operatorGuideCell == null)
                        operatorGuideCell = row.CreateCell(operatorGuideCellIndex);

                    operatorGuideCell.CellStyle = style;
                    operatorGuideCell.SetCellValue(false);

                    #endregion

                    #region Company
                    // Populate Company List
                    using (var client = new ARSystemService.ImstDataSourceServiceClient())
                    {
                        companies = client.GetCompanyToList(UserManager.User.UserToken, "CompanyID").ToList();
                        int companyIndex = startRowIndex;
                        int companyIdCellIndex = 15;
                        int companyCellIndex = 16;
                        ICell companyIdCell;
                        ICell companyCell;
                        foreach (ARSystemService.mstCompany c in companies)
                        {
                            row = sheet.GetRow(companyIndex);
                            if (row == null)
                                row = sheet.CreateRow(companyIndex);

                            companyIdCell = sheet.GetRow(companyIndex).GetCell(companyIdCellIndex);
                            if (companyIdCell == null)
                                companyIdCell = row.CreateCell(companyIdCellIndex);

                            companyIdCell.SetCellValue(c.CompanyId);
                            companyIdCell.CellStyle = style;

                            companyCell = sheet.GetRow(companyIndex).GetCell(companyCellIndex);
                            if (companyCell == null)
                                companyCell = row.CreateCell(companyCellIndex);

                            companyCell.SetCellValue(c.Company);
                            companyCell.CellStyle = style;

                            companyIndex++;
                        }
                    }
                    #endregion

                    #region Customer
                    // Populate Customer List
                    using (var client = new ARSystemService.ImstCustomerServiceClient())
                    {
                        customers = client.GetMstCustomerToList(UserManager.User.UserToken, "", 0, 1, "CustomerID", 0, 0).ToList();
                        int customerIndex = startRowIndex;
                        int customerIdCellIndex = 18;
                        int customerNameCellIndex = 19;
                        ICell customerIdCell;
                        ICell customerNameCell;
                        foreach (ARSystemService.mstCustomer c in customers)
                        {
                            row = sheet.GetRow(customerIndex);
                            if (row == null)
                                row = sheet.CreateRow(customerIndex);

                            customerIdCell = sheet.GetRow(customerIndex).GetCell(customerIdCellIndex);
                            if (customerIdCell == null)
                                customerIdCell = row.CreateCell(customerIdCellIndex);

                            customerIdCell.SetCellValue(c.CustomerID);
                            customerIdCell.CellStyle = style;

                            customerNameCell = sheet.GetRow(customerIndex).GetCell(customerNameCellIndex);
                            if (customerNameCell == null)
                                customerNameCell = row.CreateCell(customerNameCellIndex);

                            customerNameCell.SetCellValue(c.CustomerName);
                            customerNameCell.CellStyle = style;

                            customerIndex++;
                        }
                    }
                    #endregion

                    #region Operator
                    // Populate Operator List
                    using (var client = new ARSystemService.ImstDataSourceServiceClient())
                    {
                        operators = client.GetOperatorToList(UserManager.User.UserToken, "OperatorID").ToList();
                        int operatorIndex = startRowIndex;
                        int operatorIdCellIndex = 21;
                        int operatorCodeCellIndex = 22;
                        int operatorNameCellIndex = 23;
                        ICell operatorIdCell;
                        ICell operatorCodeCell;
                        ICell operatorNameCell;
                        foreach (ARSystemService.mstOperator o in operators)
                        {
                            row = sheet.GetRow(operatorIndex);
                            if (row == null)
                                row = sheet.CreateRow(operatorIndex);

                            operatorIdCell = sheet.GetRow(operatorIndex).GetCell(operatorIdCellIndex);
                            if (operatorIdCell == null)
                                operatorIdCell = row.CreateCell(operatorIdCellIndex);

                            operatorIdCell.SetCellValue(o.OperatorId);
                            operatorIdCell.CellStyle = style;

                            operatorCodeCell = sheet.GetRow(operatorIndex).GetCell(operatorCodeCellIndex);
                            if (operatorCodeCell == null)
                                operatorCodeCell = row.CreateCell(operatorCodeCellIndex);

                            operatorCodeCell.SetCellValue(o.OperatorCode);
                            operatorCodeCell.CellStyle = style;

                            operatorNameCell = sheet.GetRow(operatorIndex).GetCell(operatorNameCellIndex);
                            if (operatorNameCell == null)
                                operatorNameCell = row.CreateCell(operatorNameCellIndex);

                            operatorNameCell.SetCellValue(o.Operator);
                            operatorNameCell.CellStyle = style;

                            operatorIndex++;
                        }
                    }
                    #endregion

                    #endregion

                    #region Return the excel file to memoryStream
                    HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
                    httpResponse.Clear();
                    httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    httpResponse.AddHeader("content-disposition", "attachment;filename=Products_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx");

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wb.Write(memoryStream);
                        memoryStream.WriteTo(httpResponse.OutputStream);
                        memoryStream.Close();
                    }

                    httpResponse.End();
                    #endregion
                }
            }
        }

        [HttpPost, Route("Product/ImportExcel")]
        public ActionResult ImportProductExcel()
        {
            try
            {
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
                List<ARSystemService.vmProduct> products = new List<ARSystemService.vmProduct>();
                ARSystemService.vmProduct tempProduct;
                string sheetName = string.Empty;
                string soNumber = string.Empty;
                int startIndex = 2;
                IRow row;
                ICell cell;
                for (int i = startIndex; i <= sheet.LastRowNum; i++) //Loop the records upto filled row, starts from content row (row 3)
                {
                    if (sheet.GetRow(i) != null) //null is when the row only contains empty cells   
                    {
                        tempProduct = new ARSystemService.vmProduct();
                        row = sheet.GetRow(i);
                        if (row != null)
                        {
                            cell = row.GetCell(0);
                            if (cell != null)
                                tempProduct.ProductTypeId = int.Parse(sheet.GetRow(i).GetCell(0).NumericCellValue.ToString());

                            cell = row.GetCell(1);
                            if (cell != null)
                                tempProduct.ProductName = sheet.GetRow(i).GetCell(1).StringCellValue;

                            cell = row.GetCell(2);
                            if (cell != null)
                                tempProduct.SoNumber = sheet.GetRow(i).GetCell(2).StringCellValue;

                            cell = row.GetCell(3);
                            if (cell != null)
                                tempProduct.CompanyID = sheet.GetRow(i).GetCell(3).StringCellValue;

                            cell = row.GetCell(4);
                            if (cell != null)
                                tempProduct.IsOperator = sheet.GetRow(i).GetCell(4).BooleanCellValue;

                            cell = row.GetCell(5);
                            if (cell != null)
                                tempProduct.CustomerID = int.Parse(sheet.GetRow(i).GetCell(5).NumericCellValue.ToString());

                            cell = row.GetCell(6);
                            if (cell != null)
                                tempProduct.OperatorID = sheet.GetRow(i).GetCell(6).StringCellValue;

                            cell = row.GetCell(7);
                            if (cell != null)
                                tempProduct.OperatorCode = sheet.GetRow(i).GetCell(7).StringCellValue;

                            cell = row.GetCell(8);
                            if (cell != null)
                                tempProduct.StartLeaseDate = sheet.GetRow(i).GetCell(8).DateCellValue;

                            cell = row.GetCell(9);
                            if (cell != null)
                                tempProduct.EndLeaseDate = sheet.GetRow(i).GetCell(9).DateCellValue;

                            if (tempProduct.ProductName != null)
                                products.Add(tempProduct);
                        }
                    }
                }

                List<ARSystemService.vmProduct> validatedProducts;
                using (var client = new ARSystemService.ImstProductServiceClient())
                {
                    validatedProducts = client.ValidateProduct(UserManager.User.UserToken, products.ToArray()).ToList();
                }

                return Json(validatedProducts, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json("Exception");
            }
        }

        [Route("Email/Export")]
        public void GetEmailToExport()
        {
            //Parameter
            string strEmailName = Request.QueryString["strEmailName"];
            int intIsActive = int.Parse(Request.QueryString["intIsActive"]);

            //Call Service
            List<ARSystemService.vmEmailVariable> listEmail = new List<ARSystemService.vmEmailVariable>();
            using (var client = new ARSystemService.EmailServiceClient())
            {
                int intTotalRecord = client.GetEmailCount(UserManager.User.UserToken, strEmailName, intIsActive);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vmEmailVariable> listEmailHolder = new List<ARSystemService.vmEmailVariable>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listEmailHolder = client.GetEmailToList(UserManager.User.UserToken, strEmailName, intIsActive, null, 50 * i, 50).ToList();
                    listEmail.AddRange(listEmailHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { "EmailName", "IsActive" };
            var reader = FastMember.ObjectReader.Create(listEmail.Select(i => new
            {
                i.EmailName,
                i.IsActive
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("Email", table);
        }

        [Route("ApprovalBAPS/Export")]
        public void GetApprovalBAPSToExport()
        {
            //Parameter

            PostMstApprovalBAPS post = new PostMstApprovalBAPS();

            List<ARSystemService.MstApprovalBAPS> list = new List<ARSystemService.MstApprovalBAPS>();
            using (var client = new ARSystemService.ImstApprovalBAPSServiceClient())
            {
                list = client.GetListApprovalBAPS(UserManager.User.UserToken, post.ApprBaps, 0, 9999).ToList();
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { "RegionName", "CustomerName", "ApprovalName", "Position", "IsActive" };
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                i.RegionName,
                i.CustomerName,
                i.ApprovalName,
                i.Position,
                i.IsActive
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ApprovalBAPS", table);
        }

        #endregion

        #region Tax Invoice Bulk Upload and Download Template

        [Route("TaxInvoice/DownloadExcel")]
        public ActionResult DownloadTaxInvoiceExcel()
        {
            string fileName = "Tax Invoice Excel Template.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/templates/" + fileName));
            return File(bytes, contentType, fileName);
        }

        [Route("TaxInvoice/ImportExcel")]
        public ActionResult ImportTaxInvoiceExcel()
        {
            try
            {
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
                List<ARSystemService.vmTaxInvoice> data = new List<ARSystemService.vmTaxInvoice>();
                ARSystemService.vmTaxInvoice temp;
                string sheetName = string.Empty;
                string soNumber = string.Empty;
                int startIndex = 1;
                IRow row;
                ICell cell;
                for (int i = startIndex; i <= sheet.LastRowNum; i++) //Loop the records upto filled row, starts from content row (row 3)
                {
                    if (sheet.GetRow(i) != null) //null is when the row only contains empty cells   
                    {
                        temp = new ARSystemService.vmTaxInvoice();
                        row = sheet.GetRow(i);
                        if (row != null)
                        {
                            cell = row.GetCell(0);
                            if (cell != null)
                                temp.InvNo = sheet.GetRow(i).GetCell(0).StringCellValue;

                            cell = row.GetCell(1);
                            if (cell != null)
                                temp.TaxInvoiceNo = sheet.GetRow(i).GetCell(1).StringCellValue;

                            //cell = row.GetCell(2);
                            //if (cell != null)
                            //{
                            //    temp.TaxInvoiceDate = DateTime.Parse(Helper.Helper.ConvertDateTimeToSQLFormat(sheet.GetRow(i).GetCell(2).StringCellValue));
                            //    temp.strTaxInvoiceDate = sheet.GetRow(i).GetCell(2).StringCellValue;
                            //}

                            if (!string.IsNullOrEmpty(temp.InvNo) && !string.IsNullOrEmpty(temp.TaxInvoiceNo)) // && !string.IsNullOrEmpty(temp.strTaxInvoiceDate)
                                data.Add(temp);
                        }
                    }
                }

                List<ARSystemService.vmTaxInvoice> validatedData;
                using (var client = new ARSystemService.ImstTaxInvoiceServiceClient())
                {
                    validatedData = client.ValidateTaxInvoice(UserManager.User.UserToken, data.ToArray()).ToList();
                }

                return Json(validatedData, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json("Exception");
            }
        }

        #endregion

        #region Download Action

        [Route("Download")]
        public ActionResult DownloadAgreementDoc()
        {
            string path = Request.QueryString["path"];
            string fileName = Request.QueryString["fileName"];
            string contentType = Request.QueryString["contentType"];

            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath(path));

            return File(bytes, contentType, fileName);
        }

        [Route("RESlip/DownloadExcelTemplate")]
        public ActionResult DownloadExcelTemplate()
        {
            string fileName = "RE SLIP Excel Template.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/templates/" + fileName));
            return File(bytes, contentType, fileName);
        }

        #endregion


        [Authorize]
        [Route("MasterARRevSysParameter")]
        public ActionResult MasterARRevSysParameter()
        {
            string actionTokenView = "c5990e4f-fb92-42cc-9645-99290e667a80";
          

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                     
                    ViewBag.UserName = client.GetUserSingle(UserManager.User.UserToken);  
                    ViewBag.UsrFullName = client.GetUserSingle(UserManager.User.UserToken).FullName;
                    ViewBag.UsrUserID = client.GetUserSingle(UserManager.User.UserToken).UserID;
                    ViewBag.DocPath = Helper.Helper.GetDocPath();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
    }

}
