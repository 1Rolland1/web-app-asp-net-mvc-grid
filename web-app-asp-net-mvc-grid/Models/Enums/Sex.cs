using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace web_app_asp_net_mvc_grid.Models.Enums
{
	public enum Sex
	{
		[Display(Name = "Женский")]
		Female = 1,

		[Display(Name = "Мужской")]
		Male = 2,
	}
}