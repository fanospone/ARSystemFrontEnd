using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System.Data;
using System.Reflection;
using ARSystem.Domain.Models.ViewModels;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.ViewModels.Datatable;

namespace ARSystem.Service.ARSystem.Dashboard
{
    public class InvoiceProductionService : BaseService
    {
        private DbContext _contextDWH;
        private DateTime _dtNow;
        public static string ServiceName = "InvoiceProductionService";

        private uspInvoiceProductionSummaryRepository _repo;

        public InvoiceProductionService() : base()
        {
            _contextDWH = new DbContext(Helper.GetConnection("ARSystemDWH"));
            _dtNow = Helper.GetDateTimeNow();

            _repo = new uspInvoiceProductionSummaryRepository(_contextDWH);
        }    

        public List<vmInvoiceProduction> GetSummaryOutStanding(string UserID, vmInvoiceProductionPost filter)
        {
            List<vmInvoiceProduction> result = new List<vmInvoiceProduction>();
            try
            {
                result = _repo.GetSummaryOutStanding(filter);
                return result;
            }
            catch (Exception ex)
            {
                result.Add(new vmInvoiceProduction((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return result;
            }
        }

        public List<vmInvoiceProduction> GetSummaryProduction(string UserID, vmInvoiceProductionPost filter)
        {
            List<vmInvoiceProduction> result = new List<vmInvoiceProduction>();
            try
            {
                result = _repo.GetSummaryProduction(filter);
                return result;
            }
            catch (Exception ex)
            {
                result.Add(new vmInvoiceProduction((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return result;
            }
        }

        public Datatable<vmInvoiceProduction> GetHeaderList(string UserID, vmInvoiceProductionPost filter)
        {
            Datatable<vmInvoiceProduction> header = new Datatable<vmInvoiceProduction>();

            try
            {
                if (filter.vGroup == 1)
                    header.List = _repo.GetHeaderOutStanding(filter.vType, filter);
                else
                    header.List = _repo.GetHeaderProduction(filter.vType, filter);

                return header;
            }
            catch (Exception ex)
            {
                header.List.Add(new vmInvoiceProduction((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return header;
            }
        }

        public Datatable<vmInvoiceProduction> GetDetailList(string UserID, vmInvoiceProductionPost filter)
        {
            Datatable<vmInvoiceProduction> detail = new Datatable<vmInvoiceProduction>();

            try
            {
                if (filter.vGroup == 1)
                    detail.List = _repo.GetDetailOutStanding(filter.vType, filter);
                else
                    detail.List = _repo.GetDetailProduction(filter.vType, filter);

                return detail;
            }
            catch (Exception ex)
            {
                detail.List.Add(new vmInvoiceProduction((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return detail;
            }
        }

        public Datatable<vmInvoiceProduction> GetAllData(string UserID, vmInvoiceProductionPost filter)
        {
            Datatable<vmInvoiceProduction> detail = new Datatable<vmInvoiceProduction>();

            try
            {
                if (filter.vGroup == 1)
                    detail.List = _repo.GetAllOutStanding(filter);
                else
                    detail.List = _repo.GetAllProduction(filter);

                return detail;
            }
            catch (Exception ex)
            {
                detail.List.Add(new vmInvoiceProduction((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID)));
                return detail;
            }
        }
    }
}
