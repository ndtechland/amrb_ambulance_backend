using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class GalleryDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string Content { get; set; }
        public IEnumerable<listGallery> Gallerylist { get; set; }
    }

    public class listGallery
    {
        public int Id { get; set; }
        public string Image { get; set; } 
        public string Content { get; set; } 
    }
}
 