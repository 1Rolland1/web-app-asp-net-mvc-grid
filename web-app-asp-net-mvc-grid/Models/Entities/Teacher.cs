using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;
using web_app_asp_net_mvc_grid.Extensions;
using web_app_asp_net_mvc_grid.Models.Attributes;
using web_app_asp_net_mvc_grid.Models.Enums;

namespace web_app_asp_net_mvc_grid.Models
{
    public class Teacher
    {
        ///<summary>
        /// Id
        /// </summary> 
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        ///<summary>
        /// Имя преподавателя
        /// </summary> 
        [Required]
        [Display(Name = "Преподаватель", Order = 5)]
        public string Name { get; set; }

        /// <summary>
        /// Фото преподавателя
        /// </summary> 
        [ScaffoldColumn(false)]
        public virtual TeacherImage TeacherImage { get; set; }

        [Display(Name = "Фото преподавателя", Order = 6)]
        [NotMapped]
        public HttpPostedFileBase TeacherImageFile { get; set; }




        /// <summary>
        /// Пол
        /// </summary> 
        [ScaffoldColumn(false)]
        public Sex Sex { get; set; }

        [Display(Name = "Пол", Order = 7)]
        [UIHint("DropDownList")]
        [TargetProperty("Sex")]
        [NotMapped]
        public IEnumerable<SelectListItem> SexDictionary
        {
            get
            {
                var dictionary = new List<SelectListItem>();

                foreach (Sex type in Enum.GetValues(typeof(Sex)))
                {
                    dictionary.Add(new SelectListItem
                    {
                        Value = ((int)type).ToString(),
                        Text = type.GetDisplayValue(),
                        Selected = type == Sex
                    });
                }

                return dictionary;
            }
        }



        /// <summary>
        /// Должность
        /// </summary> 
        [ScaffoldColumn(false)]
        public Position Position { get; set; }

        [Display(Name = "Должность", Order = 7)]
        [UIHint("RadioList")]
        [TargetProperty("Position")]
        [NotMapped]
        public IEnumerable<SelectListItem> PositionDictionary
        {
            get
            {
                var dictionary = new List<SelectListItem>();

                foreach (Position type in Enum.GetValues(typeof(Position)))
                {
                    dictionary.Add(new SelectListItem
                    {
                        Value = ((int)type).ToString(),
                        Text = type.GetDisplayValue(),
                        Selected = type == Position
                    });
                }

                return dictionary;
            }
        }


        ///<summary>
        /// Список предметов, которые ведет преподаватель
        /// </summary> 
        [ScaffoldColumn(false)]
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}