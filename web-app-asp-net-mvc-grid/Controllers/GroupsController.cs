using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using web_app_asp_net_mvc_grid.Models;
using web_app_asp_net_mvc_grid.Extensions;

namespace web_app_asp_net_mvc_grid.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new TimetableContext();
            var groups = db.Groups.ToList();
            return View(groups);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var group = new Group();

            return View(group);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(Group model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var db = new TimetableContext();

            if (model.NationalityIds != null && model.NationalityIds.Any())
            {
                var nationality = db.Nationalitys.Where(s => model.NationalityIds.Contains(s.Id)).ToList();
                model.Nationalitys = nationality;
            }

            db.Groups.Add(model);
            db.SaveChanges();


            return RedirectPermanent("/Groups/Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var db = new TimetableContext();
            var group = db.Groups.FirstOrDefault(x => x.Id == id);
            if (group == null)
                return RedirectPermanent("/Groups/Index");

            db.Groups.Remove(group);
            db.SaveChanges();

            return RedirectPermanent("/Groups/Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var db = new TimetableContext();
            var group = db.Groups.FirstOrDefault(x => x.Id == id);
            if (group == null)
                return RedirectPermanent("/Groups/Index");

            return View(group);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Group model)
        {

            var db = new TimetableContext();
            var group = db.Groups.FirstOrDefault(x => x.Id == model.Id);
            if (group == null)
            {
                ModelState.AddModelError("Id", "Группа не найдена");
            }
            if (!ModelState.IsValid)
                return View(model);

            MappingGroup(model, group, db);


            db.Entry(group).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectPermanent("/Groups/Index");
        }

        private void MappingGroup(Group sourse, Group destination, TimetableContext db)
        {
            destination.GroupName = sourse.GroupName;
            destination.NumberOfStudents = sourse.NumberOfStudents;

            if (destination.Nationalitys != null)
                destination.Nationalitys.Clear();

            if (sourse.NationalityIds != null && sourse.NationalityIds.Any())
                destination.Nationalitys = db.Nationalitys.Where(s => sourse.NationalityIds.Contains(s.Id)).ToList();
        }

        [HttpGet]
        public ActionResult GetXlsx()
        {
            var db = new TimetableContext();
            var xlsx = db.Groups.ToXlsx();

            return File(xlsx.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Groups.xlsx");
        }
    }
}