using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using ARSystem.Domain.Models;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/MstDataSource")]
    public class ApiMstDataSourceController : ApiController
    {
        private readonly MasterService masterService;
        public ApiMstDataSourceController()
        {
            masterService = new MasterService();
        }

        [HttpGet, Route("Company")]
        public IHttpActionResult GetCompanyToList()
        {
            try
            {
                List<ARSystemService.mstCompany> company = new List<ARSystemService.mstCompany>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    company = client.GetCompanyToList(UserManager.User.UserToken, "").ToList();
                }

                // Modification Or Added By Ibnu Setiawan 04. September 2020
                // Only User Company to be display !
                if(UserManager.User.CompanyCode == "PKP")
                {
                    company = company.Where(w => w.CompanyId == UserManager.User.CompanyCode).ToList();
                }
                // End Modification Or Added By Ibnu Setiawan 04. September 2020

                return Ok(company);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("InvoiceType")]
        public IHttpActionResult GetInvoiceTypeToList()
        {
            try
            {
                List<ARSystemService.mstInvoiceType> invoiceType = new List<ARSystemService.mstInvoiceType>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    invoiceType = client.GetInvoiceTypeToList(UserManager.User.UserToken, "").ToList();
                }

                return Ok(invoiceType);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("Operator")]
        public IHttpActionResult GetOperatorToList()
        {
            try
            {
                List<ARSystemService.mstOperator> Operator = new List<ARSystemService.mstOperator>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    Operator = client.GetOperatorToList(UserManager.User.UserToken, "").ToList();
                }

                return Ok(Operator);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("BapsType")]
        public IHttpActionResult GetBapsTypeToList()
        {
            try
            {
                List<ARSystemService.mstBapsType> BapsTypeList = new List<ARSystemService.mstBapsType>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    BapsTypeList = client.GetBapsTypeToList(UserManager.User.UserToken, "").ToList();
                }

                return Ok(BapsTypeList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("Regional")]
        public IHttpActionResult GetRegionalToList()
        {
            try
            {
                List<ARSystemService.mstRegional> RegionalList = new List<ARSystemService.mstRegional>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    RegionalList = client.GetRegionalToList(UserManager.User.UserToken, "").ToList();
                }

                return Ok(RegionalList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("Department")]
        public IHttpActionResult GetDepartmentToList()
        {
            try
            {
                List<ARSystemService.mstDepartment> DeptList = new List<ARSystemService.mstDepartment>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    DeptList = client.GetDepartmentToList(UserManager.User.UserToken, "").ToList();
                }

                return Ok(DeptList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("CustomerType")]
        public IHttpActionResult GetCustomerTypeToList()
        {
            try
            {
                List<ARSystemService.mstCustomerType> list = new List<ARSystemService.mstCustomerType>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetCustomerTypeToList(UserManager.User.UserToken, "").ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("Province")]
        public IHttpActionResult GetProvinceToList()
        {
            try
            {
                List<ARSystemService.mstProvince> list = new List<ARSystemService.mstProvince>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetProvinceToList(UserManager.User.UserToken, "ProvinceName ASC").ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("TbgSysProvince")]
        public IHttpActionResult GetTbgSysProvinceToList()
        {
            try
            {
                List<mstProvince> list = new List<mstProvince>();
                list = masterService.GetTbgSysProvince(UserManager.User.UserID).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("Regency")]
        public IHttpActionResult GetRegencyToList()
        {
            try
            {
                var allUrlKeyValues = ControllerContext.Request.GetQueryNameValuePairs();
                int provinceId = int.Parse(allUrlKeyValues.LastOrDefault(x => x.Key == "provinceId").Value);
                List<ARSystemService.mstRegency> list = new List<ARSystemService.mstRegency>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetRegencyToList(UserManager.User.UserToken, provinceId, "").ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("Customer")]
        public IHttpActionResult GetCustomerToList()
        {
            try
            {
                List<ARSystemService.mstCustomer> list = new List<ARSystemService.mstCustomer>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetCustomerToList(UserManager.User.UserToken, "CustomerName ASC").ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("PowerType")]
        public IHttpActionResult GetPowerTypeToList()
        {
            try
            {
                List<ARSystemService.mstBapsPowerType> list = new List<ARSystemService.mstBapsPowerType>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetBapsPowerTypeToList(UserManager.User.UserToken, "PowerType ASC").ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("ChargeEntity")]
        public IHttpActionResult GetChargeEntityToList()
        {
            try
            {
                List<ARSystemService.mstChargeEntity> list = new List<ARSystemService.mstChargeEntity>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetChargeEntityToList(UserManager.User.UserToken, "ChargeEntityID DESC").ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("SignatureUser")]
        public IHttpActionResult GetSignatureUserToList()
        {
            try
            {
                List<ARSystemService.vwSignatureUser> list = new List<ARSystemService.vwSignatureUser>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetDataSignature(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("OperatorRegional")]
        public IHttpActionResult GetSignatureUserToList(string OperatorId)
        {
            try
            {
                List<ARSystemService.mstOperatorRegional> list = new List<ARSystemService.mstOperatorRegional>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetOperatorRegionalToList(UserManager.User.UserToken, OperatorId, "OprRegionId").ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("PicaReprint")]
        public IHttpActionResult GetPicaReprintToList()
        {
            try
            {
                List<ARSystemService.mstPICAReprint> list = new List<ARSystemService.mstPICAReprint>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetDataPICAReprint(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("InvoiceCategory")]
        public IHttpActionResult GetInvoiceCategoryToList()
        {
            try
            {
                List<ARSystemService.mstInvoiceCategory> list = new List<ARSystemService.mstInvoiceCategory>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetDataInvoiceCategory(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("PaymentBank")]
        public IHttpActionResult GetPaymentBankToList(string CompanyId, string Currency)
        {
            try
            {
                List<ARSystemService.mstPaymentBank> list = new List<ARSystemService.mstPaymentBank>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetArPaymentBankToList(UserManager.User.UserToken, CompanyId, Currency).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("PICAType")]
        public IHttpActionResult GetPICATypeToList()
        {
            try
            {
                List<ARSystemService.mstPICAType> list = new List<ARSystemService.mstPICAType>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetPICATypeToList(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("PICATypeARData")]
        public IHttpActionResult GetPICATypeARDataToList()
        {
            try
            {
                List<ARSystemService.mstPICAType> list = new List<ARSystemService.mstPICAType>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetPICATypeARDataToList(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("PICAMajor")]
        public IHttpActionResult GetPICAMajorToList()
        {
            try
            {
                List<ARSystemService.mstPICAMajor> list = new List<ARSystemService.mstPICAMajor>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetPICAMajorToList(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("PICADetail")]
        public IHttpActionResult GetPICADetailToList(int PICATypeId)
        {
            try
            {
                List<ARSystemService.mstPICADetail> list = new List<ARSystemService.mstPICADetail>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetPICADetailToList(UserManager.User.UserToken, PICATypeId).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("InternalPIC")]
        public IHttpActionResult GetInternalPICToList()
        {
            try
            {
                List<ARSystemService.vwInternalPIC> list = new List<ARSystemService.vwInternalPIC>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetInternalPICToList(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("DeductionType")]
        public IHttpActionResult GetDeductionTypeToList()
        {
            try
            {
                List<ARSystemService.mstDeductionType> list = new List<ARSystemService.mstDeductionType>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetDeductionTypeToList(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("Residence")]
        public IHttpActionResult GetResidenceToList()
        {
            try
            {
                List<ARSystemService.mstResidence> list = new List<ARSystemService.mstResidence>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetResidenceToList(UserManager.User.UserToken, "ResidenceName ASC").ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("NextActivity")]
        public IHttpActionResult GetNextActivity(string CurrentActivity,string CustomerID="")
        {
            try
            {
                List<ARSystemService.mstDropdown> list = new List<ARSystemService.mstDropdown>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetNextActivityList(UserManager.User.UserToken, Int32.Parse(CurrentActivity), CustomerID).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("ListActivity")]
        public IHttpActionResult ListActivity()
        {
            try
            {
                List<ARSystemService.mstDropdown> list = new List<ARSystemService.mstDropdown>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetActivityList(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("TenantType")]
        public IHttpActionResult TenantType()
        {
            try
            {
                List<ARSystemService.mstDropdown> list = new List<ARSystemService.mstDropdown>();
                using (var client = new ARSystemService.ReconcileDataServiceClient())
                {
                    list = client.GetProduct(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }



        /// <summary>
        /// method to get product type by operatorID, from table mstOperatorProduct
        /// </summary>
        /// <param name="operatorID"></param>
        [HttpGet, Route("TenantTypeByOperator")]
        public IHttpActionResult TenantTypeByOperator(string operatorID)
        {
            try
            {
                MasterService client = new MasterService();
                var list = client.GetTenantTypeByOperator(operatorID);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("AreaList")]
        public IHttpActionResult GetAreaToList()
        {
            try
            {
                List<ARSystemService.mstDropdown> Area = new List<ARSystemService.mstDropdown>();
                var a = new TBGSysService.vwAreaOperation();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    var data = client.GetAreaList(UserManager.User.UserToken);

                    foreach (var item in data)
                    {
                        Area.Add(new ARSystemService.mstDropdown
                        {
                            Text = item.Text,
                            Value = item.Value
                        });
                    }
                    //Operator = client.GetOperatorToList(UserManager.User.UserToken, "").ToList();
                }

                return Ok(Area);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("ListPO")]
        public IHttpActionResult ListPO(string param = "")
        {
            try
            {
                List<ARSystemService.mstDropdown> list = new List<ARSystemService.mstDropdown>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetPOList(UserManager.User.UserToken, param).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("ListBAPS")]
        public IHttpActionResult ListBAPS(string param="")
        {
            try
            {
                List<ARSystemService.mstDropdown> list = new List<ARSystemService.mstDropdown>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetBAPSList(UserManager.User.UserToken,param).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("ListCustomerRegion")]
        public IHttpActionResult ListCustomerRegion(string CustomerID)
        {
            try
            {
                List<ARSystemService.mstDropdown> list = new List<ARSystemService.mstDropdown>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetmstRARegion(UserManager.User.UserToken, CustomerID).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("TBGSys_Company")]
        public IHttpActionResult TBGSysGetCompanyToList()
        {
            try
            {
                List<ARSystemService.mstCompany> company = new List<ARSystemService.mstCompany>();
                ARSystem.Domain.Models.vmUserCredential userCredential = Helper.UserService.CheckUserToken(UserManager.User.UserToken);

                var data = masterService.GetCompanyList(userCredential.UserID);
                //company = client.GetCompanyToList(UserManager.User.UserToken, "").ToList();
                foreach (var item in data)
                {
                    company.Add(new ARSystemService.mstCompany
                    {
                        Company = item.Company,
                        CompanyId = item.CompanyID
                    });
                }

                return Ok(company);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("TBGSys_Operator")]
        public IHttpActionResult TBGSysGetOperatorToList()
        {
            try
            {
                List<ARSystemService.mstOperator> Operator = new List<ARSystemService.mstOperator>();
                ARSystem.Domain.Models.vmUserCredential userCredential = Helper.UserService.CheckUserToken(UserManager.User.UserToken);
                var data = masterService.GetCustomerList(userCredential.UserID);

                foreach (var item in data)
                {
                    Operator.Add(new ARSystemService.mstOperator
                    {
                        Operator = item.CustomerID,
                        PopularName = item.CustomerName,
                        OperatorCode = item.CustomerCode
                    });
                }

                return Ok(Operator);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpGet, Route("TBGSys_Area")]
        public IHttpActionResult TBGSysGetAreaToList()
        {
            try
            {
                List<ARSystemService.mstDropdown> Area = new List<ARSystemService.mstDropdown>();
                var a = new TBGSysService.vwAreaOperation();
                using (var client = new TBGSysService.MasterServiceClient())
                {
                    var data = client.GetAreaOperationList(UserManager.User.UserToken,a,"",0,9999);

                    foreach (var item in data)
                    {
                        Area.Add(new ARSystemService.mstDropdown
                        {
                            Text = item.AreaName,
                            Value = item.AreaID
                        });
                    }
                    //Operator = client.GetOperatorToList(UserManager.User.UserToken, "").ToList();
                }

                return Ok(Area);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("TBGSys_Stip")]
        public IHttpActionResult TBGSysGetStipToList()
        {
            try
            {
                List<ARSystemService.mstDropdown> Stip = new List<ARSystemService.mstDropdown>();
                ARSystem.Domain.Models.vmUserCredential userCredential = Helper.UserService.CheckUserToken(UserManager.User.UserToken);
                var data = masterService.GetSTIPList(userCredential.UserID);

                foreach (var item in data)
                {
                    Stip.Add(new ARSystemService.mstDropdown
                    {
                        Text = item.STIPCode,
                        Value = item.STIPID.ToString()
                    });
                }

                return Ok(Stip);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("TBGSys_Product")]
        public IHttpActionResult ListProduct()
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = Helper.UserService.CheckUserToken(UserManager.User.UserToken);
                List<mstProduct> data = new List<mstProduct>();
                data = masterService.GetProductList(userCredential.UserID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("PONewBaps")]
        public IHttpActionResult PONewBaps(string WhereClause)
        {
            try
            {
                ARSystemService.vmNewBapsData data = new ARSystemService.vmNewBapsData();
                
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    data = client.GetPOBapsNew(UserManager.User.UserToken, WhereClause);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("ListBapsTypeSoNumber")]
        public IHttpActionResult ListBapsTypeSoNumber(string SoNumber="")
        {
            try
            {
                List<ARSystemService.mstDropdown> list = new List<ARSystemService.mstDropdown>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetBapsTypeSONumber(UserManager.User.UserToken,SoNumber).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetSiroSoNumber")]
        public IHttpActionResult GetSiroSoNumber(string SoNumber = "")
        {
            try
            {
                List<ARSystemService.mstDropdown> list = new List<ARSystemService.mstDropdown>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    list = client.GetSIROSONumber(UserManager.User.UserToken, SoNumber).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        /*Edit By MTR*/
        [HttpGet, Route("Region")]
        public IHttpActionResult GetRegionToList()
        {
            try
            {
                List<ARSystemService.vwRegion> RegionalList = new List<ARSystemService.vwRegion>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    RegionalList = client.GetRegionToList(UserManager.User.UserToken).ToList();
                }

                return Ok(RegionalList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("Parameter")]
        public IHttpActionResult Parameter(string strParamterType)
        {

            try
            {
                List<ARSystemService.mstDropdown> Data = new List<ARSystemService.mstDropdown>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    Data = client.GetParameterType(UserManager.User.UserToken, strParamterType).ToList();
                }
                return Ok(Data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("CategoryBuilding")]
        public IHttpActionResult CategoryBuilding(string strBuildingType)
        {

            try
            {
                List<ARSystemService.mstDropdown> Data = new List<ARSystemService.mstDropdown>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    Data = client.GetCategoryBuilding(UserManager.User.UserToken, strBuildingType).ToList();

                }
                return Ok(Data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("CustomerByCustomerType")]
        public IHttpActionResult CustomerByCustomerType(int CustomerTypeID)
        {

            try
            {
                List<ARSystemService.mstDropdown> Data = new List<ARSystemService.mstDropdown>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    Data = client.GetCustomerByCustomerType(UserManager.User.UserToken, CustomerTypeID).ToList();
                }
                return Ok(Data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        /*Edit By MTR*/

        //Added By ASE
        [HttpGet, Route("FieldName")]
        public IHttpActionResult getFieldNameList()
        {

            try
            {
                List<ARSystemService.mstValidasiInvoiceManual> Data = new List<ARSystemService.mstValidasiInvoiceManual>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    Data = client.GetValidasiDropdownToList(UserManager.User.UserToken, "").ToList();
                }
                return Ok(Data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("STIPCategory")]
        public IHttpActionResult getSTIPCategoryList()
        {
            try
            {
                List<ARSystemService.vwmstSTIP> Data = new List<ARSystemService.vwmstSTIP>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    Data = client.GetStipCategoryList(UserManager.User.UserToken, "").ToList();
                }
                return Ok(Data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpGet, Route("GetSizeUpload")]
        public IHttpActionResult GetSizeUpload(string Module)
        {
            try
            {
                int Result = 0;
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    Result = client.GetSizeUpload(UserManager.User.UserToken,Module);
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("ListWeekARCBS")]
        public IHttpActionResult ListWeekARCBS(string DateInput, string Week)
        {
            try
            {
                List<ARSystemService.vmListWeekARCBS> Data = new List<ARSystemService.vmListWeekARCBS>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    Data = client.GetListWeek(UserManager.User.UserToken, DateInput, Week).ToList();
                }
                return Ok(Data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("ListRole")]

        public IHttpActionResult ListWeekARCBS()
        {
            try
            {
               List<SecureAccessService.mstRole> data = new List<SecureAccessService.mstRole>();
                using (var client = new SecureAccessService.RoleServiceClient())
                {
                    data = client.GetRoleToList(UserManager.User.UserToken, "", 1,"Role", 0, 1000).ToList();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        //Added By NHF
        [HttpGet, Route("DepartmentHumanCapital")]
        public IHttpActionResult GetDepartmentHumanCapital()
        {
            try
            {
                List<mstDepartment> DeptList = new List<mstDepartment>();

                DeptList = masterService.GetmstDepartmentList(UserManager.User.UserToken).ToList();

                return Ok(DeptList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}