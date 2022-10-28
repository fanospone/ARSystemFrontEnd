using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

namespace ARSystem.Service
{
    public class TemplateService
    {
        public List<vwReportTemplate> GetReport()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var data = new vwReportTemplateRepository(context);
            List<vwReportTemplate> model = new List<vwReportTemplate>();

            try
            {
                model = data.GetList(" PrintType='header' ", "");

                return model;
            }
            catch (Exception ex)
            {
                model.Add(new vwReportTemplate((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "TemplateService", "GetReport", "")));
                return model;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<mstRAGeneratorPDF> GetTemplate()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var data = new mstRAGeneratorPDFRepository(context);
            List<mstRAGeneratorPDF> model = new List<mstRAGeneratorPDF>();

            try
            {
                model = data.GetList(" PrintType='header' ", "");

                return model;
            }
            catch (Exception ex)
            {
                model.Add(new mstRAGeneratorPDF((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "TemplateService", "GetTemplate", "")));
                return model;
            }
            finally
            {
                context.Dispose();
            }
        }

        public mstRAGeneratorPDF SaveReport(mstRAGeneratorPDF post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var data = new mstRAGeneratorPDFRepository(context);
            List<mstRAGeneratorPDF> model = new List<mstRAGeneratorPDF>();

            try
            {
                if (post.ID != 0)
                {
                    post = data.Update(post);
                }
                else
                    post = data.Create(post);

                return post;
            }
            catch (Exception ex)
            {
                model.Add(new mstRAGeneratorPDF((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "BAPSValidationService", "GetReport", "")));
                return model.FirstOrDefault(); ;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
