using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class AboutDTO
    {
        public int ID { get; set; }
        public string ImageFile { get; set; }
        public HttpPostedFileBase ImageFileBase { get; set; }
        public string Heading { get; set; }
        public string Paragraph { get; set; }

        public string Service { get; set; }
        public List<Servicesdetail> Services { get; set; }
    }

    public class Servicesdetail
    {
        public int ServiceID { get; set; }
        public Nullable<int> AboutContentID { get; set; }
        public string Service { get; set; }
    }
}