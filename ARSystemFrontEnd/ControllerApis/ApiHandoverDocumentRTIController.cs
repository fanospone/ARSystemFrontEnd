using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using ARSystem.Domain.Models;
using ARSystemFrontEnd.Helper;
using ARSystem.Service.RevenueAssurance;
using System.Threading.Tasks;
using ARSystem.Domain.Models.ViewModels;
using MoreLinq;
using System.Globalization;
using Ionic.Zip;
using System.IO;
using Newtonsoft.Json;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/handoverDocRTI")]
    public class ApiHandoverDocumentRTIController : ApiController
    {
        private readonly vwRTIDataService _vwRTIDataService;
        private readonly vwRTIDataDoneService _vwRTIDataDoneService;

        trxBAPSDataService client = new trxBAPSDataService();
        BapsDataService service = new BapsDataService();

        public ApiHandoverDocumentRTIController()
        {
            _vwRTIDataService = new vwRTIDataService();
            _vwRTIDataDoneService = new vwRTIDataDoneService();
        }

        [HttpPost, Route("getBapsDoneTree")]
        public async Task<IHttpActionResult> GetBapsDoneTreeList(PostNewBapsCheckingDocument post)
        {
            var dataList = new List<vwRABapsDone>();
            int intTotalRecord = 0;

            try
            {
                var service = new BapsDoneService();
                var param = new vwRABapsDone();

                DateTime? startDate = null;
                DateTime? endDate = null;
                

                if (!string.IsNullOrEmpty(post.strStartDate) && !string.IsNullOrEmpty(post.strEndDate))
                {
                    startDate = DateTime.ParseExact(post.strStartDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    endDate = DateTime.ParseExact(post.strEndDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }

                param.CustomerID = post.strCustomerID;
                //NEWBAPS_DONE
                param.ActivityID = 20;
                param.SoNumber = post.strSoNumber;
                param.ProductID = int.Parse(post.strProductId == null ? "0" : post.strProductId);
                param.CompanyID = post.strCompanyId;
                param.SiteID = post.strSiteID;
                param.StartDate = startDate;
                param.EndDate = endDate;
                param.TowerTypeID = string.IsNullOrEmpty(post.strBAPSType) ? "TOWER" : post.strBAPSType;

                dataList = await service.BapsDoneTreeViewList(param);

                if (dataList.Count() == 0)
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, treeViewData = 0 });


                var dataGroup = dataList.GroupBy(x => x.Year);
                var treeDirectory = new List<vwTreeViewNodes>();

                string[] months = { "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };

                // Parent
                foreach (var group in dataGroup)
                {
                    // Add base directory parent
                    treeDirectory.Add(new vwTreeViewNodes()
                    {
                        id = group.Key.ToString(),
                        text = group.Key.ToString(),
                        parent = "#",
                        state = new vwTreeViewState()
                        {
                            disabled = false,
                            opened = false,
                            selected = false,
                        }
                    });

                    // Child
                    var dataGroupMRaw = group.OrderBy(x => x.Month).ToList();
                    var dataGroupMonths = dataGroupMRaw.GroupBy(x => x.Month);
                    foreach (var node in dataGroupMonths)
                    {
                        // Add child directory parent
                        treeDirectory.Add(new vwTreeViewNodes()
                        {
                            id = $"{months[node.Key.Value - 1]}-{group.Key}",
                            text = months[node.Key.Value - 1],
                            parent = group.Key.ToString(),
                            state = new vwTreeViewState()
                            {
                                disabled = false,
                                opened = false,
                                selected = false,
                            },
                        });

                        // Second Child
                        var dataGroupDays = node.OrderBy(x => x.Day).ToList();
                        foreach (var innerNode in dataGroupDays)
                        {
                            // Add second child directory parent
                            treeDirectory.Add(new vwTreeViewNodes()
                            {
                                id = $"{innerNode.Day}-{months[node.Key.Value - 1]}-{group.Key}",
                                text = innerNode.Day.ToString(),
                                parent = $"{months[node.Key.Value - 1]}-{group.Key}",
                                state = new vwTreeViewState()
                                {
                                    disabled = false,
                                    opened = false,
                                    selected = false,
                                },
                            });

                            // Add customer directory parent
                            treeDirectory.Add(new vwTreeViewNodes()
                            {

                                id = $"{innerNode.Day}-{months[node.Key.Value - 1]}-{group.Key}-{innerNode.CustomerID}",
                                text = $"{innerNode.CustomerID} (Document)",
                                parent = $"{innerNode.Day}-{months[node.Key.Value - 1]}-{group.Key}",
                                state = new vwTreeViewState()
                                {
                                    disabled = false,
                                    opened = false,
                                    selected = false,
                                },
                                li_attr = new vwTreeViewNodeData()
                                {
                                   data_node = "author",
                                   data_value = "MMH"
                                },
                                data = new List<vwRABapsDone>()
                                {
                                    new vwRABapsDone()
                                    {
                                       CustomerID = innerNode.CustomerID,
                                       DoneDate = innerNode.DoneDate,
                                       SoNumber = innerNode.SoNumber,
                                       ProductID = innerNode.ProductID,
                                    }
                                }
                            });

                            // Add company directory parent
                            treeDirectory.Add(new vwTreeViewNodes()
                            {
                                id = $"{innerNode.Day}-{months[node.Key.Value - 1]}-{group.Key}-{innerNode.CustomerID}-{innerNode.CompanyID}",
                                text = $"{innerNode.CompanyID} (Document)" ,
                                parent = $"{innerNode.Day}-{months[node.Key.Value - 1]}-{group.Key}-{innerNode.CustomerID}",
                                state = new vwTreeViewState()
                                {
                                    disabled = false,
                                    opened = false,
                                    selected = false,
                                },
                                data = new List<vwRABapsDone>()
                                {
                                    new vwRABapsDone()
                                    {
                                       CustomerID = innerNode.CustomerID,
                                       CompanyID = innerNode.CompanyID,
                                       DoneDate = innerNode.DoneDate,
                                       SoNumber = innerNode.SoNumber,
                                       ProductID = innerNode.ProductID,
                                    }
                                }
                            });

                        }
                    }
                }
                
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, treeViewData = treeDirectory.DistinctBy(x => x.id) });
            }
            catch (Exception ex)
            {
                dataList.Add(new vwRABapsDone { ErrorType = 1, ErrorMessage = ex.Message });
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
            }
        }

        [HttpPost, Route("getBapsDoneGrid")]
        public async Task<IHttpActionResult> GetBapsDoneGridList(PostNewBapsCheckingDocument post)
        {
            var dataList = new List<vwRABapsDone>();
            int intTotalRecord = 0;
            try
            {
                var service = new BapsDoneService();
                var param = new vwRABapsDone();

                DateTime? startDate = null;
                DateTime? endDate = null;

                if (!string.IsNullOrEmpty(post.strStartDate) && !string.IsNullOrEmpty(post.strEndDate))
                {
                    startDate = DateTime.ParseExact(post.strStartDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    endDate = DateTime.ParseExact(post.strEndDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }

                param.CustomerID = post.strCustomerID;
                //NEWBAPS_DONE
                param.ActivityID = 20;
                param.SoNumber = post.strSoNumber;
                param.ProductID = int.Parse(post.strProductId == null ? "0" : post.strProductId);
                param.CompanyID = post.strCompanyId;
                param.SiteID = post.strSiteID;
                param.TowerTypeID = string.IsNullOrEmpty(post.strBAPSType) ? "TOWER" : post.strBAPSType;
                param.StartDate = startDate;
                param.EndDate = endDate;
                intTotalRecord = service.BapsDoneTreeViewCount(param);
                dataList = await service.BapsDoneTreeViewList(param, post.start, post.length, TableViewTypes.GRID);
                //intTotalRecord = dataList.Count();
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
            }
            catch (Exception ex)
            {
                dataList.Add(new vwRABapsDone { ErrorType = 1, ErrorMessage = ex.Message });
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
            }
        }
        
        [HttpPost, Route("getArchive")]
        public async Task<IHttpActionResult> GetZipArchive(PostDocumentArchieve post)
        {
            Dictionary<string, string> docFiles = new Dictionary<string, string>();
            List<string> notFoundFiles = new List<string>();

            string zipFileName = string.Empty;
            var service = new BapsDoneService();
            var param = new vwRABapsDone();
            var dataList = new List<vwRABapsDone>();

            string comments = string.Empty;

            if (post.DoneDate.Length > 19)
                post.DoneDate = post.DoneDate.Remove(post.DoneDate.LastIndexOf('.'));

            DateTime doneDate = DateTime.Now;

            try
            {
              doneDate = DateTime.ParseExact(post.DoneDate, "yyyy-MM-ddTHH:mm:ss", null);
            }
            catch (Exception ex)
            {

            }

            //NEWBAPS_DONE
            param.ActivityID = 20;
            param.SoNumber = null;
            param.ProductID = int.Parse(post.ProductID == null ? "0" : post.ProductID);
            param.CompanyID = post.CompanyID;
            param.CustomerID = post.CustomerID;
            param.TowerTypeID = post.TowerTypeID;
            //param.SiteID = post.strSiteID;
            param.DoneDate = doneDate;
            dataList = await service.BapsDoneTreeViewList(param, type:TableViewTypes.TREE_ARCHIVE);

            int count = 0;

            if (dataList.Count == 0)
                return Ok(new { zipPath = "", message = "Document is not found!", innerMessage = notFoundFiles });

            foreach (var item in dataList)
            {
                if (string.IsNullOrEmpty(item.FilePath))
                {
                    notFoundFiles.Add($"Document is not found with SO Number: {item.SoNumber} and Transaction ID: {item.ID}");
                    continue;
                }
                else
                {
                    docFiles.Add($"{count}", item.FilePath);
                    count++;
                }
            }

            string fileName = $"archive-{UserManager.User.UserToken}.zip";
            if (docFiles.Count == 0)
                return Ok(new { zipPath = "", message = $"System cannot find any specific document!", innerMessage = notFoundFiles });

            try
            {
                int counter = 1;

                using (ZipFile zip = new ZipFile())
                {
                    foreach (var file in dataList)
                    {
                        if (string.IsNullOrEmpty(file.FilePath))
                            continue;

                        var costName = string.IsNullOrEmpty(file.CustomerSiteID) ? file.SiteID : file.CustomerSiteID;
                        zip.AddFile(System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + file.FilePath), $"Document_{DateTime.Now.ToString("yyyyMMdd")}").FileName = $"{counter}_{costName}.pdf";
                        counter++;
                    }

                    comments = $"Berkas dikompresi pada tanggal {DateTime.Now.ToString("yyyy MM dd HH:mm:ss")}";
                    comments += $"{comments} File tidak ditemukan: ${string.Join(Environment.NewLine, notFoundFiles.ToArray())}";
                    zip.Comment = comments;
                    zip.Save(System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath()) + fileName);
                }
            }
            catch(Exception ex)
            {
                return Ok(new { zipPath = "", message = ex.Message, innerMessage = notFoundFiles });
            }

            return Ok(new { zipPath = $"{Helper.Helper.GetDocPath().Replace("~","")}/{fileName}", message = "The data compressed successfully", innerMessage = notFoundFiles });
        }

        [HttpPost, Route("getArchiveBAPSByRange")]
        public async Task<IHttpActionResult> GetZipArchiveBPASByRange(PostDocumentArchieve post)
        {
            Dictionary<string, string> docFiles = new Dictionary<string, string>();
            List<string> notFoundFiles = new List<string>();

            string zipFileName = string.Empty;
            var service = new BapsDoneService();
            var param = new vwRABapsDone();
            var dataList = new List<vwRABapsDone>();

            string comments = string.Empty;

            DateTime? startDate = null;
            DateTime? endDate = null;
            int count = 0;

            try
            {
                if (!string.IsNullOrEmpty(post.StartDate) && !string.IsNullOrEmpty(post.EndDate))
                {
                    startDate = DateTime.ParseExact(post.StartDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    endDate = DateTime.ParseExact(post.EndDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }

                //NEWBAPS_DONE
                param.ActivityID = 20;
                param.SoNumber = null;
                param.ProductID = int.Parse(post.ProductID == null ? "0" : post.ProductID);
                param.CompanyID = post.CompanyID;
                param.CustomerID = post.CustomerID;
                param.TowerTypeID = post.TowerTypeID;
                //param.SiteID = post.strSiteID;
                param.StartDate = startDate;
                param.EndDate = endDate;

                dataList = await service.BapsDoneTreeViewList(param, type: TableViewTypes.COMPRESSED_ARCHIVE);

            }
            catch (Exception ex)
            {

            }

            if (dataList.Count == 0)
                return Ok(new { zipPath = "", message = "Document is not found!", innerMessage = notFoundFiles });

            foreach (var item in dataList)
            {
                if (string.IsNullOrEmpty(item.FilePath))
                {
                    notFoundFiles.Add($"Document is not found with SO Number: {item.SoNumber} and Transaction ID: {item.ID}");
                    continue;
                }
                else
                {
                    docFiles.Add($"{count}", item.FilePath);
                    count++;
                }
            }

            string fileName = $"archive-{UserManager.User.UserToken}.zip";
            if (docFiles.Count == 0)
                return Ok(new { zipPath = "", message = $"System cannot find any specific document!", innerMessage = notFoundFiles });

            try
            {
                int counter = 1;

                using (ZipFile zip = new ZipFile())
                {
                    foreach (var file in dataList)
                    {
                        if (string.IsNullOrEmpty(file.FilePath))
                            continue;

                        var costName = string.IsNullOrEmpty(file.CustomerSiteID) ? file.SiteID : file.CustomerSiteID;
                        zip.AddFile(System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + file.FilePath), $"Document_{DateTime.Now.ToString("yyyyMMdd")}").FileName = $"{counter}_{costName}.pdf";
                        counter++;
                    }

                    comments = $"Berkas dikompresi pada tanggal {DateTime.Now.ToString("yyyy MM dd HH:mm:ss")}";
                    comments += $"{comments} File tidak ditemukan: ${string.Join(Environment.NewLine, notFoundFiles.ToArray())}";
                    zip.Comment = comments;
                    zip.Save(System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath()) + fileName);
                }
            }
            catch (Exception ex)
            {
                return Ok(new { zipPath = "", message = ex.Message, innerMessage = notFoundFiles });
            }

            return Ok(new { zipPath = $"{Helper.Helper.GetDocPath().Replace("~", "")}/{fileName}", message = "The data compressed successfully", innerMessage = notFoundFiles });
        }

        [HttpPost, Route("getArchiveRTIByRange")]
        public async Task<IHttpActionResult> GetZipArchiveRTIByRange(trxRARTIPost post)
        {
            Dictionary<string, string> docFiles = new Dictionary<string, string>();
            List<string> notFoundFiles = new List<string>();

            // Handle init an object
            if (post.strBAPSNumber == null)
                post.strBAPSNumber = new List<string>() { null };                

            if(post.strPONumber == null)
                post.strPONumber = new List<string>() { null };

            if (post.strSONumber == null)
                post.strSONumber = new List<string>() { null };

            string zipFileName = string.Empty;
            var service = new BapsDoneService();
            var param = new vwRABapsDone();
            var dataList = new List<vwRTIDataDone>();

            string comments = string.Empty;

            DateTime? startDate = null;
            DateTime? endDate = null;

            try
            {
                if (!string.IsNullOrEmpty(post.strStartDate) && !string.IsNullOrEmpty(post.strEndDate))
                {
                    startDate = DateTime.ParseExact(post.strStartDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    endDate = DateTime.ParseExact(post.strEndDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }

                dataList = _vwRTIDataDoneService.GetDataDone(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.strBAPSNumber.ToList(), post.strPONumber.ToList(), post.strSONumber.ToList(), post.Year, post.Quartal, post.BapsType, post.PowerType, "", post.start, post.length, startDate, endDate).ToList();
            }
            catch (Exception ex)
            {

            }


            if (dataList.Count == 0)
                return Ok(new { zipPath = "", message = "Document is not found!", innerMessage = notFoundFiles });

            foreach (var item in dataList)
            {
                if (string.IsNullOrEmpty(item.FilePath))
                {
                    notFoundFiles.Add($"Document is not found with SO Number: {item.SONumber} and Transaction ID: {item.Id}");
                    continue;
                }
                else
                {
                    docFiles.Add(item.Id.ToString(), item.FilePath);
                }
            }

            string fileName = $"archive-{UserManager.User.UserToken}.zip";
            if (docFiles.Count == 0)
                return Ok(new { zipPath = "", message = $"System cannot find any specific document!", innerMessage = notFoundFiles });

            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    int counter = 1;

                    foreach (var file in dataList)
                    {
                        if (string.IsNullOrEmpty(file.FilePath))
                            continue;

                        var costName = string.IsNullOrEmpty(file.CustomerSiteID) ? file.SiteID : file.CustomerSiteID;
                        zip.AddFile(System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + file.FilePath), $"Document_{DateTime.Now.ToString("yyyyMMdd")}").FileName = $"{counter}_{costName}.pdf"; ;
                        counter++;
                    }

                    comments = $"Berkas dikompresi pada tanggal {DateTime.Now.ToString("yyyy MM dd HH:mm:ss")}";
                    comments += $"{comments} File tidak ditemukan: ${string.Join(Environment.NewLine, notFoundFiles.ToArray())}";
                    zip.Comment = comments;
                    zip.Save(System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath()) + fileName);
                }
            }
            catch (Exception ex)
            {
                return Ok(new { zipPath = "", message = ex.Message, innerMessage = notFoundFiles });
            }

            return Ok(new { zipPath = $"{Helper.Helper.GetDocPath().Replace("~", "")}/{fileName}", message = "The data compressed successfully", innerMessage = notFoundFiles });
        }

        [HttpPost, Route("gridReconcile")]
        public IHttpActionResult gridDone(trxRARTIPost post)
        {
            try
            {
                int intTotalRecord = 0;

                DateTime? startDate = null;
                DateTime? endDate = null;

                if (!string.IsNullOrEmpty(post.strStartDate) && !string.IsNullOrEmpty(post.strEndDate))
                {
                    startDate = DateTime.ParseExact(post.strStartDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    endDate = DateTime.ParseExact(post.strEndDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }

                List<vwRTIDataDone> Result = new List<vwRTIDataDone>();
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    Result.Add(new vwRTIDataDone(userCredential.ErrorType, userCredential.ErrorMessage));
                    return Ok(Result);
                }

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                intTotalRecord = _vwRTIDataDoneService.GetCountDone(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.strBAPSNumber.ToList(), post.strPONumber.ToList(), post.strSONumber.ToList(), post.Year, post.Quartal, post.BapsType, post.PowerType, startDate, endDate);
                Result = _vwRTIDataDoneService.GetDataDone(UserManager.User.UserToken, post.CompanyID, post.CustomerID, post.strBAPSNumber.ToList(), post.strPONumber.ToList(), post.strSONumber.ToList(), post.Year, post.Quartal, post.BapsType, post.PowerType, strOrderBy, post.start, post.length, startDate, endDate).ToList();

                if (post.IsOrder)
                 Result =  Result.OrderByDescending(x => x.CustomerSiteName).ToList();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = Result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

    }

}