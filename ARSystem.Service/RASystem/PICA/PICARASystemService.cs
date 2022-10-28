using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Service
{
    public class PICARASystemService
    {
        public List<GetDropDownList> GetDdlCustomer()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<GetDropDownList> data = new List<GetDropDownList>();

            try
            {
                var repo = new GetDropDownListRepository(context);
                data = repo.GetDdlCustomer();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new GetDropDownList { ErrorMessage = ex.Message });
            }
            return data;
        }

        public List<GetDropDownList> GetDdlCompany()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<GetDropDownList> data = new List<GetDropDownList>();

            try
            {
                var repo = new GetDropDownListRepository(context);
                data = repo.GetDdlCompany();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new GetDropDownList { ErrorMessage = ex.Message });
            }
            return data;
        }

        public List<GetDropDownList> GetDdlProduct()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<GetDropDownList> data = new List<GetDropDownList>();

            try
            {
                var repo = new GetDropDownListRepository(context);
                data = repo.GetDdlProduct();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new GetDropDownList { ErrorMessage = ex.Message });
            }
            return data;
        }

        public List<GetDropDownList> GetDdlStipCategory()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<GetDropDownList> data = new List<GetDropDownList>();

            try
            {
                var repo = new GetDropDownListRepository(context);
                data = repo.GetDdlStipCategory();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new GetDropDownList { ErrorMessage = ex.Message });
            }
            return data;
        }

        public List<GetDropDownList> GetDdlBapsType()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<GetDropDownList> data = new List<GetDropDownList>();

            try
            {
                var repo = new GetDropDownListRepository(context);
                data = repo.GetDdlBapsType();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new GetDropDownList { ErrorMessage = ex.Message });
            }
            return data;
        }

        public List<GetDropDownList> GetDdlActivityStatus()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<GetDropDownList> data = new List<GetDropDownList>();

            try
            {
                var repo = new GetDropDownListRepository(context);
                data = repo.GetDdlActivityStatus();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new GetDropDownList { ErrorMessage = ex.Message });
            }
            return data;
        }

        public List<GetDropDownList> GetDdlCategoryPICA()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<GetDropDownList> data = new List<GetDropDownList>();

            try
            {
                var repo = new GetDropDownListRepository(context);
                data = repo.GetDdlCategoryPICA();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new GetDropDownList { ErrorMessage = ex.Message });
            }
            return data;
        }

        public List<GetDropDownList> GetDdlPICA(string PICA)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<GetDropDownList> data = new List<GetDropDownList>();

            try
            {
                var repo = new GetDropDownListRepository(context);
                data = repo.GetDdlPICA(PICA);
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new GetDropDownList { ErrorMessage = ex.Message });
            }
            return data;
        }

        public List<vwRADashboardPICA> GetListHeader(vwRADashboardPICA post, int intRowStart = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwRADashboardPICA> data = new List<vwRADashboardPICA>();

            try
            {
                var repo = new RADashboardPICARepository(context);
                //post.CustomerId = null;
                if(post.CompanyId != null && post.CompanyId != "ALL")
                {
                    post.CompanyId = post.CompanyId.Replace(" ", "");
                }
                data = repo.GetListHeader(post, intRowStart, intPageSize).ToList();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwRADashboardPICA { ErrorMessage = ex.Message });
            }
            return data;
        }

        public int GetCountListHeader(vwRADashboardPICA post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repository = new RADashboardPICARepository(context);
            try
            {
                if (post.CompanyId != null && post.CompanyId != "ALL")
                {
                    post.CompanyId = post.CompanyId.Replace(" ", "");
                }

                return repository.GetCountListHeader(post);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwRADashboardPICA> GetListDetail(vwRADashboardPICA post, int intRowStart = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwRADashboardPICA> data = new List<vwRADashboardPICA>();

            try
            {
                var repo = new RADashboardPICARepository(context);
                if (post.CompanyId != null && post.CompanyId != "ALL")
                {
                    post.CompanyId = post.CompanyId.Replace(" ", "");
                }

                data = repo.GetListDetail(post, intRowStart, intPageSize).ToList();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwRADashboardPICA { ErrorMessage = ex.Message });
            }
            return data;
        }

        public int GetCountListDetail(vwRADashboardPICA post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repository = new RADashboardPICARepository(context);
            try
            {
                if (post.CompanyId != null && post.CompanyId != "ALL")
                {
                    post.CompanyId = post.CompanyId.Replace(" ", "");
                }
                return repository.GetCountDetail(post);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwRADashboardPICA> GetHistoryPICA(vwRADashboardPICA post, int intRowStart = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwRADashboardPICA> data = new List<vwRADashboardPICA>();

            try
            {
                var repo = new RADashboardPICARepository(context);

                data = repo.GetViewHistoryPICA(post, intRowStart, intPageSize).ToList();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwRADashboardPICA { ErrorMessage = ex.Message });
            }
            return data;
        }


        public int GetCountHistoryPICA(vwRADashboardPICA post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repository = new RADashboardPICARepository(context);
            try
            {
                return repository.GetCountHistoryPICA(post);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public vwRADashboardPICA InsertPICA(string userID, vwRADashboardPICA post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new RADashboardPICARepository(context);
            

            try
            {
                return repo.InsertPICA(userID, post);
            }
            catch (Exception ex)
            {
                return new vwRADashboardPICA();
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
