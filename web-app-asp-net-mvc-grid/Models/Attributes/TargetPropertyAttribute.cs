using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_app_asp_net_mvc_grid.Models.Attributes
{
    public class TargetPropertyAttribute : Attribute
    {
        public TargetPropertyAttribute(string targetProperty)
        {
            TargetProperty = targetProperty;
        }

        public string TargetProperty { get; set; }
    }
}