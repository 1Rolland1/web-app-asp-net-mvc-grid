using web_app_asp_net_mvc_grid.Models.Attributes;
using web_app_asp_net_mvc_grid.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace web_app_asp_net_mvc_grid.Models
{
    public class ImportDisciplineLog
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Импорт начат в")]
        [UIHint("TextReadOnly")]
        public DateTime StartImport { get; set; }

        [Display(Name = "Импорт закончен в")]
        [UIHint("TextReadOnly")]
        public DateTime EndImport { get; set; }


        [Display(Name = "Распознанных строк")]
        [UIHint("TextReadOnly")]
        public int SuccessCount { get; set; }

        [Display(Name = "Не распознанных строк")]
        [UIHint("TextReadOnly")]
        public int FailedCount { get; set; }
       
        [Display(Name = "Отчет")]
        [UIHint("Logs")]
        public List<ImportDisciplineRowLog> Logs { get; set; }
    }
}