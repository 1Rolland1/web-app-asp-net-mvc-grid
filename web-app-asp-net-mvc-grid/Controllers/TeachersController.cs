using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using web_app_asp_net_mvc_grid.Extensions;
using web_app_asp_net_mvc_grid.Models;

namespace web_app_asp_net_mvc_grid.Controllers
{
    [Authorize]
    public class TeachersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new TimetableContext();
            return View(db.Set<Teacher>());
            //var teachers = db.Teachers.ToList();

            //return View(teachers);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var teacher = new Teacher();
            return View(teacher);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(Teacher model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var db = new TimetableContext();

            if (model.TeacherImageFile != null)
            {
                var data = new byte[model.TeacherImageFile.ContentLength];
                model.TeacherImageFile.InputStream.Read(data, 0, model.TeacherImageFile.ContentLength);

                model.TeacherImage = new TeacherImage()
                {
                    Guid = Guid.NewGuid(),
                    DateChanged = DateTime.Now,
                    Data = data,
                    ContentType = model.TeacherImageFile.ContentType,
                    FileName = model.TeacherImageFile.FileName
                };
            }


            db.Teachers.Add(model);
            db.SaveChanges();

            return RedirectPermanent("/Teachers/Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var db = new TimetableContext();
            var teacher = db.Teachers.FirstOrDefault(x => x.Id == id);
            if (teacher == null)
                return RedirectPermanent("/Teachers/Index");

            db.Teachers.Remove(teacher);
            db.SaveChanges();

            return RedirectPermanent("/Teachers/Index");
        }


        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var db = new TimetableContext();
            var teacher = db.Teachers.FirstOrDefault(x => x.Id == id);
            if (teacher == null)
                return RedirectPermanent("/Teachers/Index");

            return View(teacher);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Teacher model)
        {
            var db = new TimetableContext();
            var teacher = db.Teachers.FirstOrDefault(x => x.Id == model.Id);
            if (teacher == null)
                ModelState.AddModelError("Id", "Преподаватель не найден");

            if (!ModelState.IsValid)
                return View(model);

            MappingTeacher(model, teacher, db);

            db.Entry(teacher).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectPermanent("/Teachers/Index");
        }

        private void MappingTeacher(Teacher sourse, Teacher destination, TimetableContext db)
        {
            destination.Name = sourse.Name;
            destination.Sex = sourse.Sex;
            destination.Position = sourse.Position;

            if (sourse.TeacherImageFile != null)
            {
                var image = db.TeacherImages.FirstOrDefault(x => x.Id == sourse.Id);
                if (image != null)
                    db.TeacherImages.Remove(image);

                var data = new byte[sourse.TeacherImageFile.ContentLength];
                sourse.TeacherImageFile.InputStream.Read(data, 0, sourse.TeacherImageFile.ContentLength);

                destination.TeacherImage = new TeacherImage()
                {
                    Guid = Guid.NewGuid(),
                    DateChanged = DateTime.Now,
                    Data = data,
                    ContentType = sourse.TeacherImageFile.ContentType,
                    FileName = sourse.TeacherImageFile.FileName
                };
            }

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
        public ActionResult GetXlsx()
        {
            var db = new TimetableContext();
            var xlsx = db.Teachers.ToXlsx();

            return File(xlsx.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Teachers.xlsx");
        }
    }
}