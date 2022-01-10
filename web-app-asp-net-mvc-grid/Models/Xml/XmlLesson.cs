using System.Collections.Generic;
using System.Xml.Serialization;
using web_app_asp_net_mvc_grid.Models.Xml;

namespace web_app_asp_net_mvc_grid.Models
{
    [XmlRoot("Lesson")]
    public class XmlLesson
    {
        /// <summary>
        /// Порядковый номер пары в расписании
        /// </summary>    
        [XmlElement("Number")]
        public int Number { get; set; }

        /// <summary>
        /// Группа(ы), у которой(ых) будет пара
        /// </summary> 
        [XmlArray("Groups")]
        [XmlArrayItem(typeof(XmlGroup), ElementName = "Group")]
        public virtual List<XmlGroup> Groups { get; set; }

        /// <summary>
        /// Преподаватель
        /// </summary> 
        [XmlElement("TeacherId")]
        public int TeacherId { get; set; }

        /// <summary>
        /// Дисциплина
        /// </summary> 
        [XmlElement("DisciplineId")]
        public int DisciplineId { get; set; }

    }
}