using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class BannerDTO
    {
        public int Id { get; set; }
        public string BannerImage { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string Heading { get; set; }
        public string Paragraph { get; set; }
        public IEnumerable<listBanner> listBanner { get; set; }
    }
    public class listBanner
    {
        public int ID { get; set; }
        public string BannerImage { get; set; } 
        public string Heading { get; set; }
        public string Paragraph { get; set; } 
    }
}