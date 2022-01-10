using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace web_app_asp_net_mvc_grid.Models
{
    public class ImportXmlLessonViewModel
    {
        /// <summary>
        /// Id
        /// </summary> 
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }


        [Display(Name = "Файл импорта")]
        [Required(ErrorMessage = "Укажите файл импорта (.xml)")]
        public HttpPostedFileBase FileToImport { get; set; }
    }
}