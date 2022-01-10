using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace web_app_asp_net_mvc_grid.Models
{
    public class TeacherImage
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }
        public DateTime? DateChanged { get; set; }
        public string FileName { get; set; }

    }
}