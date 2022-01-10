using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using web_app_asp_net_mvc_grid.Extensions;
using web_app_asp_net_mvc_grid.Models;

namespace web_app_asp_net_mvc_grid.Controllers
{
    public class ImportXmlLessonsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new ImportXmlLessonViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Import(ImportXmlLessonViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var file = new byte[model.FileToImport.InputStream.Length];
            model.FileToImport.InputStream.Read(file, 0, (int)model.FileToImport.InputStream.Length);

            XmlSerializer xml = new XmlSerializer(typeof(List<XmlLesson>));
            var lessons = (List<XmlLesson>)xml.Deserialize(new MemoryStream(file));
            var db = new TimetableContext();



            foreach (var lesson in lessons)
            {
                var input = new Lesson()
                {
                    Number = lesson.Number,
                    DisciplineId = lesson.DisciplineId,
                    TeacherId = lesson.TeacherId, 
                };
                var groupIds = lesson.Groups.Select(x => x.Id).ToList();
                var groups = db.Groups.Where(s => groupIds.Contains(s.Id)).ToList();
                input.Groups = groups;

                db.Lessons.Add(input);

                db.SaveChanges();
            }

            return RedirectPermanent("/Lessons/Index");
        }

        public ActionResult GetExample()
        {
            return File("~/Content/Files/ImportXmlLessonsExample.xml", "application/xml", "ImportXmlLessonsExample.xml");
        }

    }
}