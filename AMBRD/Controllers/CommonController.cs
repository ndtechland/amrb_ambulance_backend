using AMBRD.Models;
using AMBRD.Models.ViewModels;
using AMBRD.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using System.Net.Mail;
using Microsoft.Web.Infrastructure;
using System.Web.Http.Results;
using AMBRD.Repositories;
using System.Globalization;

namespace AMBRD.Controllers
{
    [Authorize]
    public class CommonController : Controller
    {
        // GET: Common
        CommonRepository repos = new CommonRepository();
        private abdul_amurdEntities11 ent = new abdul_amurdEntities11();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddBanner()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");
            return View();
        }
        [HttpPost]
        [Obsolete]
        public ActionResult AddBanner(BannerDTO model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var ban = Mapper.CreateMap<BannerDTO, Banner>();
                var banner = Mapper.Map<Banner>(model);

                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }

                    banner.BannerImage = uploadResult;

                }

                ent.Banners.Add(banner);
                ent.SaveChanges();
                TempData["msg"] = "Saved Successfully.";
            }
            catch
            {
                TempData["msg"] = "Error has been occured.";

            }
            return RedirectToAction("AddBanner");

        }

        public ActionResult Bannerlist()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");

            var model = new BannerDTO();
            string q = @"select * from Banner";
            var data = ent.Database.SqlQuery<listBanner>(q).ToList();
            if (data.Count() == 0)
            {
                TempData["msg"] = "No Record";
                return View(model);
            }
            model.listBanner = data;
            return View(model);
        }

        public ActionResult DeleteBanner(int Id)
        {
            try
            {
                var result = ent.Banners.FirstOrDefault(x => x.Id == Id);
                if (result != null)
                {
                    ent.Banners.Remove(result);
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "Deleted successfully";

                }
                return RedirectToAction("BannerList");
            }
            catch(Exception ex)
            {
                //string sender = "cpdms1.gadigital@gmail.com";
                //string password = "nvlpcqzbwksbsmih";
                //MailMessage message = new MailMessage();
                //message.From = new MailAddress(sender);
                //message.To.Add("rakeshguptainbox@gmail.com");
                //message.Subject = "error";
                //message.Body = ex.ToString();
                //message.IsBodyHtml = true;
                //SmtpClient client = new SmtpClient();
                //client.Host = "smtp.gmail.com";
                //client.Port = 587;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential(sender, password);
                //client.EnableSsl = true;
                //client.Send(message);
                //return View ();
                throw new Exception(ex.ToString());
            }
        }

        public ActionResult About()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");
            return View();  
        }
        [HttpPost]

        public ActionResult About(AboutDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Create a new AboutContent record
                var aboutContent = new AboutContent
                { 
                    Heading = model.Heading,
                    Paragraph = model.Paragraph,
                };

                if (model.ImageFileBase != null)
                {
                    if (model.ImageFileBase.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not exceed 2 MB";
                        return View(model);
                    }

                    var uploadResult = FileOperation.UploadImage(model.ImageFileBase, "Images");

                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg, .jpeg, .png, and .gif files are allowed";
                        return View(model);
                    }

                    aboutContent.ImageFile = uploadResult;
                }

                ent.AboutContents.Add(aboutContent);
                ent.SaveChanges();

                // Create AboutContentServices records if model.Services is not null or empty
                if (model.Services != null && model.Services.Any())
                {

                    foreach (var item in model.Services)
                    {
                        var service = new AboutContentService
                        {
                            AboutContentID = aboutContent.ID, // Use the generated AboutContent ID
                            Service = item.Service
                        };
                        ent.AboutContentServices.Add(service);
                    }
                    ent.SaveChanges();
                }

                TempData["msg"] = "About content has been added successfully.";
                return RedirectToAction("About");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error occurred: " + ex.Message;
                return View(model);
            }
        } 
        public ActionResult Ourservice()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");
            return View();
        }
        [HttpPost]
        public ActionResult Ourservice(ServiceDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var data = new OurService()
                {
                    ServiceName = model.ServiceName,
                    Description = model.Description,
                };
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }

                    data.Image = uploadResult;

                }

                ent.OurServices.Add(data);
                ent.SaveChanges();
                TempData["msg"] = "ok";
            }
            catch (Exception)
            {
                TempData["msg"] = "Error has been occured.";
            }
            return RedirectToAction("Ourservice");
        }        
        public ActionResult ServiceList()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default"); 

            var model = new ServiceDTO();
            string q = @"select * from OurService";
            var data = ent.Database.SqlQuery<ServeceList>(q).ToList();
            if (data.Count() == 0)
            {
                TempData["msg"] = "No Record";
                return View(model);
            }
            model.ServeceList = data;
            return View(model);
        }
        public ActionResult EditService(int id)
        {
            var data = ent.OurServices.Find(id);
            Mapper.CreateMap<OurService, ServiceDTO>();
            var model = Mapper.Map<ServiceDTO>(data);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditService(ServiceDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                var data = ent.OurServices.Find(model.Id);
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }

                    data.Image = uploadResult;

                }
                if (data != null)
                {
                    data.ServiceName = model.ServiceName;
                    data.Description = model.Description;
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "Edit successfully";
                }

                ent.SaveChanges();
                return RedirectToAction("ServiceList");
            }
            catch (Exception)
            {
                //log.Error(ex.Message);
                TempData["msg"] = "Server Error";
                return RedirectToAction("ServiceList", model.Id);
            }
        }
        public ActionResult Deleteservice(int id)
        {
            try
            {
                var data = ent.OurServices.FirstOrDefault(x => x.Id == id);
                if (data != null)
                {
                    ent.OurServices.Remove (data); 
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "Service deleted successfully";
                }
                return RedirectToAction("ServiceList");
            }
            catch
            {
                throw new Exception("Server error");
            }

        }

        public ActionResult OtherService()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");
            return View();
        }
        [HttpPost]
        public ActionResult OtherService(ServiceDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var data = new OtherService()
                {
                    ServiceName = model.ServiceName,
                    Description = model.Description,
                };
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }

                    data.Image = uploadResult;

                }

                ent.OtherServices.Add(data);
                ent.SaveChanges();
                TempData["msg"] = "ok";
            }
            catch (Exception)
            {
                TempData["msg"] = "Error has been occured.";
            }
            return RedirectToAction("OtherService");
        }

        public ActionResult ListOtherService()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");

            var model = new ServiceDTO();
            string q = @"select * from OtherService";
            var data = ent.Database.SqlQuery<ServeceList>(q).ToList();
            if (data.Count() == 0)
            {
                TempData["msg"] = "No Record";
                return View(model);
            }
            model.ServeceList = data;
            return View(model);
        }

        public ActionResult EditOtherService(int id)
        {
            var data = ent.OtherServices.Find(id);
            Mapper.CreateMap<OtherService, ServiceDTO>();
            var model = Mapper.Map<ServiceDTO>(data);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditOtherService(ServiceDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                var data = ent.OtherServices.Find(model.Id);
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }

                    data.Image = uploadResult;

                }
                if (data != null)
                {
                    data.ServiceName = model.ServiceName;
                    data.Description = model.Description;
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "Edit successfully";
                }

                ent.SaveChanges();
                return RedirectToAction("ListOtherService");
            }
            catch (Exception)
            {
                //log.Error(ex.Message);
                TempData["msg"] = "Server Error";
                return RedirectToAction("ListOtherService", model.Id);
            }
        }
        public ActionResult DeleteOtherservice(int id)
        {
            try
            {
                var data = ent.OtherServices.FirstOrDefault(x => x.Id == id);
                if (data != null)
                {
                    ent.OtherServices.Remove(data);
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "Service deleted successfully";
                }
                return RedirectToAction("ListOtherService");
            }
            catch
            {
                throw new Exception("Server error");
            }

        }
        public ActionResult AddBlog()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");
            return View();
        }
        [HttpPost]
        public ActionResult AddBlog(BlogDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                } 
                var data = new Blog()
                {
                    Title = model.Title,
                    Heading = model.Heading,
                    Description = model.Description, 
                };
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }

                    data.Image = uploadResult;

                }

                ent.Blogs.Add(data);
                ent.SaveChanges();
                TempData["msg"] = "Saved Successfully.";
            }
            catch
            {
                TempData["msg"] = "Error has been occured.";

            }
            return RedirectToAction("AddBlog");
        }
        public ActionResult BlogList()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");

            var model = new BlogDTO();
            string q = @"select * from Blog";
            var data = ent.Database.SqlQuery<listBlog>(q).ToList();
            if (data.Count() == 0)
            {
                TempData["msg"] = "No Record";
                return View(model);
            }
            model.Bloglist = data;
            return View(model);
        }
        public ActionResult DeleteBlog(int id)
        {
            try
            {
                var result = ent.Blogs.FirstOrDefault(x => x.Id == id);
                if (result != null)
                {
                    ent.Blogs.Remove(result);
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "Deleted successfully";
                }
                return RedirectToAction("BlogList");
            }
            catch
            {
                throw new Exception("Server error");
            }
        }

        [Obsolete]
        public ActionResult EditBlog(int id)
        {
            var data = ent.Blogs.Find(id);
            Mapper.CreateMap<Blog, BlogDTO>();
            var model = Mapper.Map<BlogDTO>(data);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditBlog(BlogDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model); 
                var data = ent.Blogs.Find(model.Id);
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }

                    data.Image = uploadResult;

                }
                if (data != null)
                { 
                    data.Title = model.Title;
                    data.Heading = model.Heading;
                    data.Description = model.Description;
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "Edit successfully";
                }

                ent.SaveChanges();
                return RedirectToAction("BlogList");
            }       
            catch (Exception)
            {
                //log.Error(ex.Message);
                TempData["msg"] = "Server Error";
                return RedirectToAction("BlogList", model.Id);
            }
        }

        public ActionResult AddGallery()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");
            return View();
        }

        [HttpPost]
        public ActionResult AddGallery(GalleryDTO model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                 
                var data = new Gallery()
                {
                    Content =model .Content,
                };
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }

                    data.Image = uploadResult;

                }
                
                ent.Galleries.Add(data);
                ent.SaveChanges();
                TempData["msg"] = "Saved Successfully.";
            }
            catch
            {
                TempData["msg"] = "Error has been occured.";

            }
            return RedirectToAction("AddGallery");

        }

        public ActionResult GalleryList()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");

            var model = new GalleryDTO();
            string q = @"select * from Gallery";
            var data = ent.Database.SqlQuery<listGallery>(q).ToList();
            if (data.Count() == 0)
            {
                TempData["msg"] = "No Record";
                return View(model);
            }
            model.Gallerylist = data;
            return View(model);
        }

        public ActionResult EditGallery(int id)
        {
            var data = ent.Galleries.Find(id);
            Mapper.CreateMap<Gallery, GalleryDTO>();
            var model = Mapper.Map<GalleryDTO>(data);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditGallery(GalleryDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                var data = ent.Galleries.Find(model.Id);
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }

                    data.Image = uploadResult;

                }
                if (data != null)
                {
                    data.Content = model.Content;
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "Edit successfully";
                }

                ent.SaveChanges();
                return RedirectToAction("GalleryList");
            }
            catch (Exception)
            {
                //log.Error(ex.Message);
                TempData["msg"] = "Server Error";
                return RedirectToAction("GalleryList", model.Id);
            }
        }

        public ActionResult DeleteGallery(int id)
        {
            try
            {
                var result = ent.Galleries.FirstOrDefault(x => x.Id == id);
                if (result != null)
                {
                    ent.Galleries.Remove(result);
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "Deleted successfully";
                }
                return RedirectToAction("GalleryList");
            }
            catch
            {
                throw new Exception("Server error");
            }
        }

        public ActionResult AddTestimonial()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");
            return View();
        }
        [HttpPost]
        public ActionResult AddTestimonial(TestimonialDTO model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                //var ban = Mapper.CreateMap<BannerDTO, Banner>();
                //var banner = Mapper.Map<Banner>(model);
                var data = new Testimonial()
                {
                    Name = model.Name,
                    Location = model.Location,
                    Description = model.Description,
                };
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }

                    data.Image = uploadResult;

                }

                ent.Testimonials.Add(data);
                ent.SaveChanges();
                TempData["msg"] = "Saved Successfully.";
            }
            catch
            {
                TempData["msg"] = "Error has been occured.";

            }
            return RedirectToAction("AddTestimonial");

        }
        public ActionResult TestimonialList()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");

            var model = new TestimonialDTO();
            string q = @"select * from Testimonial";
            var data = ent.Database.SqlQuery<Testimoniallist>(q).ToList();
            if (data.Count() == 0)
            {
                TempData["msg"] = "No Record";
                return View(model);
            }
            model.Testimoniallist = data;
            return View(model);
        }
        public ActionResult DeleteTestimonial(int id)
        {
            try
            {
                var result = ent.Testimonials.FirstOrDefault(x => x.Id == id);
                if (result != null)
                {
                    ent.Testimonials.Remove(result);
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "Deleted successfully";
                }
                return RedirectToAction("TestimonialList");
            }
            catch
            {
                throw new Exception("Server error");
            }
        }

        [Obsolete]
        public ActionResult GetCitiesByState(int? stateId)
        {
            var cities = repos.GetCitiesByState(stateId);
            Mapper.CreateMap<CityMaster, CityDTO>();
            var c = Mapper.Map<IEnumerable<CityDTO>>(cities);
            return Json(c, JsonRequestBehavior.AllowGet);
        }

        [Obsolete]
        public ActionResult GetVehicleTypeByCat(int? CatId)
        {
            var vehicleTypes = repos.GetVehicleTypeByCat(CatId);
            Mapper.CreateMap<VehicleType, VehicleTypeDTO>();
            //var data = Mapper.Map<IEnumerable<VehicleTypeDTO>>(ent.VehicleTypes.Where(A => A.IsDeleted == false && A.VehicleCatId == CatId).ToList());
            
            var c = Mapper.Map<IEnumerable<VehicleTypeDTO>>(vehicleTypes);
            return Json(c, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Commision()
        {
            var model = new CommissionDTO();
            var Q = @"select * from CommissionMaster where IsDelete=0 order by Name asc";
            var data = ent.Database.SqlQuery<CommissionList>(Q).ToList();
            model.CommissionList = data;
            return View(model);
        }

        [HttpPost]
        public ActionResult AddCommision(string Name = "", string Commission = "")
        {
            if (!ModelState.IsValid)
            {
                TempData["msg"] = "Some Error";
                return RedirectToAction("Commision");
            }
            if (ent.CommissionMasters.Any(a => a.Name == Name && a.IsDelete == false))
            {
                TempData["msg"] = "Already Exist with this Name";
                return RedirectToAction("Commision");
            }
            NumberFormatInfo provider = new NumberFormatInfo();
            var data = new CommissionMaster();
            data.Name = Name;
            data.Commission = Convert.ToDouble(Commission);
            data.IsDelete = false;
            ent.CommissionMasters.Add(data);
            ent.SaveChanges();
            TempData["msg"] = "ok";
            return RedirectToAction("Commision");
        }

        [HttpGet]
        public ActionResult EditCommission(int id)
        {
            Mapper.CreateMap<CommissionMaster, CommissionDTO>();
            var data = ent.CommissionMasters.Find(id);
            var model = Mapper.Map<CommissionDTO>(data);
            return View(model);
        }


        [HttpPost]
        public ActionResult EditCommission(CommissionDTO model)
        { 
            
            var data = ent.CommissionMasters.Find(model.Id);
            if(data != null)
            {
                data.Commission = model.Commission;
                TempData["SuccessMessage"] = "Commission edit successfully";
            }
            ent.SaveChanges();
            
            return RedirectToAction("Commision");
        }


        public ActionResult DeleteCommission(int Id)
        {
            var data = ent.CommissionMasters.Find(Id);
            data.IsDelete = true;
            TempData["SuccessMessage"] = "Commission delete successfully";
            ent.SaveChanges();
            return RedirectToAction("Commision");
        }
    }
}