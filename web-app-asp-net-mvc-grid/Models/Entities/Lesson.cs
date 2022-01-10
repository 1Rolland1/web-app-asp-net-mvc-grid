using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_app_asp_net_mvc_grid.Extensions;
using web_app_asp_net_mvc_grid.Models.Attributes;

namespace web_app_asp_net_mvc_grid.Models
{
    public class Lesson
    {
        /// <summary>
        /// Id
        /// </summary> 
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Порядковый номер пары в расписании
        /// </summary>    
        [Required]
        [Display(Name = "Номер пары в расписании", Order = 1)]
        [Range(1, 8, ErrorMessage = "Недопустимое значение")]
        public int Number { get; set; }

        /// <summary>
        /// Название пары
        /// </summary>    
        ///[Required]
        ///[Display(Name = "Пара", Order = 2)]
        ///public String Name { get; set; }

        /// <summary>
        /// Группа, у которой будет пара
        /// </summary> 
        [ScaffoldColumn(false)]
        public virtual ICollection<Group> Groups { get; set; }

        [ScaffoldColumn(false)]
        public List<int> GroupIds { get; set; }

        [Display(Name = "Группы", Order = 70)]
        [UIHint("MultipleDropDownList")]
        [TargetProperty("GroupIds")]
        [NotMapped]
        public IEnumerable<SelectListItem> GroupDictionary
        {
            get
            {
                var db = new TimetableContext();
                var query = db.Groups;

                if (query != null)
                {
                    var Ids = query.Where(s => s.Lessons.Any(ss => ss.Id == Id)).Select(s => s.Id).ToList();
                    var dictionary = new List<SelectListItem>();
                    dictionary.AddRange(query.ToSelectList(c => c.Id, c => $"{c.GroupName}", c => Ids.Contains(c.Id)));
                    return dictionary;
                }

                return new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            }
        }

        /// <summary>
        /// Преподаватель
        /// </summary> 
        [ScaffoldColumn(false)]
        public int TeacherId { get; set; }
        [ScaffoldColumn(false)]
        public virtual Teacher Teacher { get; set; }
        [Display(Name = "Преподаватель", Order = 50)]
        [UIHint("DropDownList")]
        [TargetProperty("TeacherId")]
        [NotMapped]
        public IEnumerable<SelectListItem> TeacherDictionary
        {
            get
            {
                var db = new TimetableContext();
                var query = db.Teachers;

                if (query != null)
                {
                    var dictionary = new List<SelectListItem>();
                    dictionary.AddRange(query.OrderBy(d => d.Name).ToSelectList(c => c.Id, c => c.Name, c => c.Id == TeacherId));
                    return dictionary;
                }

                return new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            }
        }

        /// <summary>
        /// Дисциплина
        /// </summary> 
        [ScaffoldColumn(false)]
        public int DisciplineId { get; set; }
        [ScaffoldColumn(false)]
        public virtual Discipline Discipline { get; set; }
        [Display(Name = "Дисциплина", Order = 2)]
        [UIHint("RadioList")]
        [TargetProperty("DisciplineId")]
        [NotMapped]
        public IEnumerable<SelectListItem> DisciplineDictionary
        {
            get
            {
                var db = new TimetableContext();
                var query = db.Disciplines;

                if (query != null)
                {
                    var dictionary = new List<SelectListItem>();
                    dictionary.AddRange(query.OrderBy(d => d.Name).ToSelectList(c => c.Id, c => c.Name, c => c.Id == DisciplineId));
                    return dictionary;
                }

                return new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            }
        }





    }
}