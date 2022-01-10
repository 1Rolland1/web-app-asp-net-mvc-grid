using System.Xml.Serialization;

namespace web_app_asp_net_mvc_grid.Models.Xml
{
    public class XmlTeacher
    {
        /// <summary>
        /// Id
        /// </summary> 
        [XmlElement("Id")]
        public int Id { get; set; }
    }
}