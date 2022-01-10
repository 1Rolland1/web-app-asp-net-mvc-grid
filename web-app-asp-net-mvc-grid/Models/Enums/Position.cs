using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace web_app_asp_net_mvc_grid.Models.Enums
{
	public enum Position
	{
		[Display(Name = "Профессор")]
		Professor = 1,

		[Display(Name = "Старший преподаватель")]
		SeniorLecturer = 2,

		[Display(Name = "Доцент")]
		AssistantProfessor = 3,
	}
}