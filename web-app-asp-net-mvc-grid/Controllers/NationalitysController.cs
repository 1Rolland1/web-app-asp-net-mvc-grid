using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_app_asp_net_mvc_grid.Extensions;
using web_app_asp_net_mvc_grid.Models;

namespace web_app_asp_net_mvc_grid.Controllers
{
    [Authorize]
    public class NationalitysController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new TimetableContext();
            var nationalitys = db.Nationalitys.ToList();

            return View(nationalitys);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var nationality = new Nationality();
            return View(nationality);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(Nationality model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var db = new TimetableContext();

            db.Nationalitys.Add(model);
            db.SaveChanges();

            return RedirectPermanent("/Nationalitys/Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var db = new TimetableContext();
            var nationality = db.Nationalitys.FirstOrDefault(x => x.Id == id);
            if (nationality == null)
                return RedirectPermanent("/Nationalitys/Index");

            db.Nationalitys.Remove(nationality);
            db.SaveChanges();

            return RedirectPermanent("/Nationalitys/Index");
        }


        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var db = new TimetableContext();
            var nationality = db.Nationalitys.FirstOrDefault(x => x.Id == id);
            if (nationality == null)
                return RedirectPermanent("/Nationalitys/Index");

            return View(nationality);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Nationality model)
        {
            var db = new TimetableContext();
            var nationality = db.Nationalitys.FirstOrDefault(x => x.Id == model.Id);
            if (nationality == null)
                ModelState.AddModelError("Id", "Национальность не найден");

            if (!ModelState.IsValid)
                return View(model);

            MappingNationality(model, nationality);

            db.Entry(nationality).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectPermanent("/Nationalitys/Index");
        }

        private void MappingNationality(Nationality sourse, Nationality destination)
        {
            destination.Name = sourse.Name;
        }

        [HttpGet]
        public ActionResult GetXlsx()
        {
            var db = new TimetableContext();
            var xlsx = db.Nationalitys.ToXlsx();

            return File(xlsx.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Nationalities.xlsx");
        }
    }
}