using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class BlogDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string Title { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public IEnumerable<listBlog> Bloglist { get; set; }
    }

    public class listBlog
    {
        public int Id { get; set; }
        public string Image { get; set; } 
        public string Title { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; } 
    }
}
 