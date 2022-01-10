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
    public class ImportDisciplineData
    {
        public string Name { get; set; }
        public string DisciplineGoal { get; set; }
        public string DisciplineObjectives { get; set; }
        public string MainSections { get; set; }
    }
}