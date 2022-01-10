using System.ComponentModel.DataAnnotations;

namespace web_app_asp_net_mvc_grid.Models
{
    public enum ImportDisciplineRowLogType
    {
        [Display(Name = "Успешно")]
        Success = 1,

        [Display(Name = "Ошибка при парсинге строки")]
        ErrorParsed = 2,
    }
}