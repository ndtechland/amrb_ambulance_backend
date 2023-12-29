using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class TestimonialDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public IEnumerable<Testimoniallist> Testimoniallist { get; set; }
    }
    public class Testimoniallist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; } 
        public string Location { get; set; }
        public string Description { get; set; } 
    }
}