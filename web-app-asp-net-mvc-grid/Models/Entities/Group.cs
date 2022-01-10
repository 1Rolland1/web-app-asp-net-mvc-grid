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
    public class Group
    {
        /// <summary>
        /// Id
        /// </summary> 
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>    
        [Required]
        [Display(Name = "Название группы", Order = 5)]
        public string GroupName { get; set; }

        // <summary>
        /// Количество студентов
        /// </summary>    
        [Required]
        [Display(Name = "Количество студентов", Order = 10)]
        public int? NumberOfStudents { get; set; }

        /// <summary>
        /// Национальности
        /// </summary> 
        [ScaffoldColumn(false)]
        public virtual ICollection<Nationality> Nationalitys { get; set; }

        [ScaffoldColumn(false)]
        public List<int> NationalityIds { get; set; }

        [Display(Name = "Национальности", Order = 55)]
        [UIHint("MultipleSelect")]
        [TargetProperty("NationalityIds")]
        [NotMapped]
        public IEnumerable<SelectListItem> NationalityDictionary
        {
            get
            {
                var db = new TimetableContext();
                var query = db.Nationalitys;

                if (query != null)
                {
                    var Ids = query.Where(s => s.Groups.Any(ss => ss.Id == Id)).Select(s => s.Id).ToList();
                    var dictionary = new List<SelectListItem>();
                    dictionary.AddRange(query.ToSelectList(c => c.Id, c => $"{c.Name}", c => Ids.Contains(c.Id)));
                    return dictionary;
                }

                return new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            }
        }

        /// <summary>
        /// Предметы
        /// </summary> 
        [ScaffoldColumn(false)]
        public virtual ICollection<Lesson> Lessons { get; set; }

    }
}