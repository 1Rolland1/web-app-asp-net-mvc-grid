using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using web_app_asp_net_mvc_grid.Extensions;
using web_app_asp_net_mvc_grid.Models;

namespace web_app_asp_net_mvc_grid.Controllers
{
    public class ImportDisciplinesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new ImportDisciplineViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Import(ImportDisciplineViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var log = ProceedImport(model);

            return View("Log", log);
        }

        public ActionResult GetExample()
        {
            return File("~/Content/Files/ImportDisciplinesExample.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ImportDisciplinesExample.xlsx");
        }

        private ImportDisciplineLog ProceedImport(ImportDisciplineViewModel model)
        {
            var startTime = DateTime.Now;

            var workBook = new XLWorkbook(model.FileToImport.InputStream);
            var workSheet = workBook.Worksheet(1);
            var rows = workSheet.RowsUsed().Skip(1).ToList();

            var logs = new List<ImportDisciplineRowLog>();
            var data = ParseRows(rows, logs);
            ApplyImported(data);

            var successCount = data.Count();
            var failedCount = rows.Count() - successCount;
            var finishTime = DateTime.Now;

            var result = new ImportDisciplineLog()
            {
                StartImport = startTime,
                EndImport = finishTime,
                SuccessCount = successCount,
                FailedCount = failedCount,
                Logs = logs
            };

            return result;
        }

        private List<ImportDisciplineData> ParseRows(IEnumerable<IXLRow> rows, List<ImportDisciplineRowLog> logs)
        {
            var result = new List<ImportDisciplineData>();
            int index = 1;
            foreach(var row in rows)
            {
                try
                {
                    var data = new ImportDisciplineData()
                    {
                        Name = ConvertToString(row.Cell("A").GetValue<string>().Trim()),
                        DisciplineGoal = ConvertToString(row.Cell("B").GetValue<string>().Trim()),
                        DisciplineObjectives = ConvertToString(row.Cell("C").GetValue<string>().Trim()),
                        MainSections = ConvertToString(row.Cell("D").GetValue<string>().Trim()),
                    };

                    result.Add(data);
                    logs.Add(new ImportDisciplineRowLog()
                    {
                        Id = index,
                        Message = $"ОК",
                        Type = ImportDisciplineRowLogType.Success 
                    }); ;

                }
                catch (Exception ex)
                {
                    logs.Add(new ImportDisciplineRowLog()
                    {
                        Id = index,
                        Message = $"Error: {ex.GetBaseException().Message}",
                        Type = ImportDisciplineRowLogType.ErrorParsed
                    }); ;
                }

                index++;
            }


            return result;
        }

        private void ApplyImported(List<ImportDisciplineData> data)
        {
            var db = new TimetableContext();
                       
            foreach (var value in data)
            {
                var model = new Discipline()
                {
                    Name = value.Name,
                    DisciplineGoal = value.DisciplineGoal,
                    DisciplineObjectives = value.DisciplineObjectives,
                    MainSections = value.MainSections,
                    
                };

                db.Disciplines.Add(model);
                db.SaveChanges();
            }
        }

        private string ConvertToString(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new Exception("Значение не определено");

            var result = HandleInjection(value);

            return result;
        }
        private string HandleInjection(string value)
        {
            var badSymbols = new Regex(@"^[+=@-].*");
            return Regex.IsMatch(value, badSymbols.ToString()) ? string.Empty : value;
        }

        private DateTime? ConvertToDateTime(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            DateTime result = default;

            if (DateTime.TryParse(value, out DateTime temp))
                result = temp;

            if (result == default)
                return null;

            return result;
        }

       

    }
}