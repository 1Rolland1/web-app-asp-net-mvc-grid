using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_app_asp_net_mvc_grid.Models
{
    public class Nationality
    {
        /// <summary>
        /// Id
        /// </summary> 
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>    
        [Required]
        [Display(Name = "Название", Order = 5)]
        public string Name { get; set; }

        /// <summary>
        /// Список книг
        /// </summary> 
        [ScaffoldColumn(false)]
        public virtual ICollection<Group> Groups { get; set; }
    }
}