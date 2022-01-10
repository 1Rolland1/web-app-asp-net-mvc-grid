using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using ClosedXML.Excel;
using Rotativa;
using web_app_asp_net_mvc_grid.Models;
using web_app_asp_net_mvc_grid.Models.Xml;

namespace web_app_asp_net_mvc_grid.Controllers
{
    [Authorize]
    public class LessonsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new TimetableContext();
            var lessons = db.Lessons.ToList();

            return View(lessons);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var lesson = new Lesson();
            return View(lesson);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(Lesson model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var db = new TimetableContext();

            if (model.GroupIds != null && model.GroupIds.Any())
            {
                var group = db.Groups.Where(s => model.GroupIds.Contains(s.Id)).ToList();
                model.Groups = group;
            }

            db.Lessons.Add(model);
            db.SaveChanges();

            return RedirectPermanent("/Lessons/Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var db = new TimetableContext();
            var lesson = db.Lessons.FirstOrDefault(x => x.Id == id);
            if (lesson == null)
                return RedirectPermanent("/Lessons/Index");

            db.Lessons.Remove(lesson);
            db.SaveChanges();

            return RedirectPermanent("/Lessons/Index");
        }


        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var db = new TimetableContext();
            var lesson = db.Lessons.FirstOrDefault(x => x.Id == id);
            if (lesson == null)
                return RedirectPermanent("/Lessons/Index");

            return View(lesson);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Lesson model)
        {
            var db = new TimetableContext();
            var lesson = db.Lessons.FirstOrDefault(x => x.Id == model.Id);
            if (lesson == null)
                ModelState.AddModelError("Id", "Пара не найдена");

            if (!ModelState.IsValid)
                return View(model);

            MappingLesson(model, lesson, db);

            db.Entry(lesson).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectPermanent("/Lessons/Index");
        }

        private void MappingLesson(Lesson sourse, Lesson destination, TimetableContext db)
        {
            destination.Number = sourse.Number;
            destination.DisciplineId = sourse.DisciplineId;
            destination.Discipline = sourse.Discipline;
            destination.TeacherId = sourse.TeacherId;
            destination.Teacher = sourse.Teacher;



            if (destination.Groups != null)
                destination.Groups.Clear();

            if (sourse.GroupIds != null && sourse.GroupIds.Any())
                destination.Groups = db.Groups.Where(s => sourse.GroupIds.Contains(s.Id)).ToList();
        }

        [HttpGet]
        public ActionResult GetImage(int id)
        {
            var db = new TimetableContext();
            var image = db.TeacherImages.FirstOrDefault(x => x.Id == id);
            if (image == null)
            {
                FileStream fs = System.IO.File.OpenRead(Server.MapPath(@"~/Content/Images/not-foto.png"));
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                fs.Close();

                return File(new MemoryStream(fileData), "image/jpeg");
            }

            return File(new MemoryStream(image.Data), image.ContentType);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var db = new TimetableContext();
            var lesson = db.Lessons.FirstOrDefault(x => x.Id == id);
            if (lesson == null)
                return RedirectPermanent("/Lessons/Index");

            return View(lesson);
        }

        [HttpGet]
        public ActionResult GetXml()
        {
            var db = new TimetableContext();
            var lessons = db.Lessons.ToList().Select(x => new XmlLesson()
            {
                Number = x.Number,
                TeacherId = x.TeacherId,
                DisciplineId = x.DisciplineId,
                Groups = x.Groups.Select(y => new XmlGroup() { Id = y.Id }).ToList()
            }).ToList();

            XmlSerializer xml = new XmlSerializer(typeof(List<XmlLesson>));
            var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var ms = new MemoryStream();
            xml.Serialize(ms, lessons, ns);
            ms.Position = 0;

            return File(new MemoryStream(ms.ToArray()), "text/xml");
        }

        [HttpGet]
        public ActionResult Pdf(int id)
        {
            var db = new TimetableContext();
            var lesson = db.Lessons.FirstOrDefault(x => x.Id == id);
            if (lesson == null)
                return RedirectPermanent("/Lessons/Index");

            var pdf = new ViewAsPdf("Pdf", lesson);
            var data = pdf.BuildFile(this.ControllerContext);


            return File(new MemoryStream(data), "application/pdf", "document.pdf");
        }

        [HttpGet]
        public FileResult GetXlsx(Lesson model)
        {
            var db = new TimetableContext();
            var values = db.Lessons.ToList();
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Data");


            ws.Cell("A" + 1).Value = "Id";
            ws.Cell("B" + 1).Value = "Номер пары";
            ws.Cell("C" + 1).Value = "Дисциплина";
            ws.Cell("D" + 1).Value = "Группа(ы)";
            ws.Cell("E" + 1).Value = "Преподаватель";

            int row = 2;
            foreach (var value in values)
            {
                ws.Cell("A" + row).Value = value.Id;
                ws.Cell("B" + row).Value = value.Number;
                ws.Cell("C" + row).Value = value.Discipline.Name;
                ws.Cell("D" + row).Value = string.Join(", ", value.Groups.Select(x => $"{x.GroupName}"));
                ws.Cell("E" + row).Value = value.Teacher.Name;
                row++;
            };
            var rngHead = ws.Range("A1:E" + 1);
            rngHead.Style.Fill.BackgroundColor = XLColor.AshGrey;

            var rngTable = ws.Range("A1:E" + 10);
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            ws.Columns().AdjustToContents();



            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Timetable.xlsx");
            }
        }

    }
}