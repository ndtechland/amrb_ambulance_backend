using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class ServiceDTO
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public IEnumerable<ServeceList> ServeceList { get; set; }
    }
    public class ServeceList
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}