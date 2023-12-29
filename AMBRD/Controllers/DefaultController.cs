using AMBRD.Models;
using AMBRD.Models.ViewModels;
using AMBRD.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AMBRD.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default

        private abdul_amurdEntities11 ent = new abdul_amurdEntities11();
        public ActionResult Dashboard()
        {
            int hospitalCount = ent.Hospitals.Where(a => a.IsDeleted == false).Count();
            int AmbulanceCount = ent.Ambulances.Where(a => a.IsDeleted == false).Count();
            int DriverCount = ent.Drivers.Where(a => a.IsDeleted == false).Count();
            int PatientCount = ent.Patients.Where(a => a.IsDeleted == false).Count();

            ViewBag.HospitalCount = hospitalCount;
            ViewBag.AmbulanceCount = AmbulanceCount;
            ViewBag.DriverCount = DriverCount;
            ViewBag.PatientCount = PatientCount;

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            Session["user"] = "admin";
            try
            {
                var user = ent.Database.SqlQuery<AdminLogin>("select * from AdminLogin where UserID='" + model.UserID + "' and password='" + model.Password + "'").FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                    UserIdentity.UserRole = user.Role;
                    if (user.Role != "admin")
                    {
                        if (IsUserApproved(user.Role, user.Id))
                            return RedirectToAction("Dashboard");
                        else
                        {
                            //return IsUserApproved(user.Role, user.Id);
                        }
                        // TempData["msg"] = "You are not approved yet.";
                    }
                    return RedirectToAction("Dashboard");
                }
                TempData["msg"] = "Invalid User Name or Password.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
                TempData["msg"] = "Server Error.";
                return RedirectToAction("Login");
            }
        }

        bool IsUserApproved(string role, int loginId)
        {
            if (role == "hospital")
            {
                var hospital = ent.Hospitals.FirstOrDefault(a => a.AdminLogin_Id == loginId && a.IsDeleted == false);
                return (bool)hospital.IsApproved;
            }
            else if (role == "driver")
            {
                var driver = ent.Drivers.FirstOrDefault(a => a.AdminLogin_Id == loginId && a.IsDeleted == false);
                return (bool)driver.IsApproved;
            }
            else if (role == "patient")
            {
                var patient = ent.Patients.FirstOrDefault(a => a.AdminLogin_Id == loginId && a.IsDeleted == false);
                return (bool)patient.IsApproved;
            }
            else
                return false;

        }
        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();


            return RedirectToAction("Login");
            //FormsAuthentication.SignOut();
            ////UserIdentity.UnAssignUserRole();
            //return RedirectToAction("Login");
        }

        public ActionResult Profile()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login");
            return View();
        }

        public ActionResult Changepassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Changepassword(ChangepasswordDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    TempData["msg"] = "Any fields can not be blank.";
                if (model.Password != model.Confirmpassword)
                {
                    TempData["msg"] = "your password and confirm password are not same";
                    return RedirectToAction("Changepassword");
                }

                int id = Convert.ToInt32(User.Identity.Name);
                if (id > 0)
                {
                    var record = ent.AdminLogins.Find(id);
                    if (record != null)
                    {
                        record.Password = model.Password;
                        record.Confirmpassword = model.Confirmpassword;
                        ent.SaveChanges();
                        TempData["msg"] = "ok";
                    }
                }
                return RedirectToAction("Changepassword");
            }
            catch (Exception)
            {
                //log.Error(ex.Message + " and inner exception : " + ex.InnerException);
                return Content("Server error.");
            }
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string Username)
        {
            try
            {
                var data = ent.AdminLogins.FirstOrDefault(a => a.Username == Username);
                 
                string resetToken = Guid.NewGuid().ToString();
                string resetLink = Url.Action("ResetPassword", "Default", new { token = resetToken }, Request.Url.Scheme);

                Session["gettoken"] = resetToken;
                if (data != null)
                {
                    string qry = @"Update AdminLogin set Token='" + resetToken + "' where Username='" + Username + "'";
                    ent.Database.ExecuteSqlCommand(qry); 
                    SendPasswordResetEmail(Username, resetLink);
                    TempData["msg"] = "ok";
                    return RedirectToAction("ForgotPassword");
                } 
                TempData["msg"] = "Invalid email";

                return RedirectToAction("ForgotPassword");
            }
            catch (Exception)
            {
                // Log the error or handle it accordingly
                TempData["msg"] = "An error occurred while processing your request. Please try again later.";
                return RedirectToAction("ForgotPassword");
            }
        }

        private void SendPasswordResetEmail(string userEmail, string resetLink)
        {
            string sender = "cpdms1.gadigital@gmail.com";
            string password = "nvlpcqzbwksbsmih";
            MailMessage message = new MailMessage();
            message.From = new MailAddress(sender);
            message.To.Add(userEmail);
            message.Subject = "Password Reset";
            message.Body = $"Click the link below to reset your password: {resetLink}";
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(sender, password);
            client.EnableSsl = true;
            client.Send(message);




        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(AdminLoginDTO model)
        {
            try
            {
                if (model.Password != model.Confirmpassword)
                {
                    TempData["msg"] = "your password and confirm password are not match.";
                    return RedirectToAction("ResetPassword");
                }
                string userEmail = GetUserEmailFromToken(/*Request.QueryString["token"]*/);
                 
                var token = ent.AdminLogins.Where(a => a.Username == userEmail).Select(a => a.Token).FirstOrDefault();

                // Check if a record with the given userEmail is found
                if (token != null)
                {
                    string qry = @"Update AdminLogin set Password='" + model.Password + "' where Token='" + token + "'";
                    ent.Database.ExecuteSqlCommand(qry);
                }                
                TempData["msg"] = "Your password has been successfully reset. You can now login with your new password.";

                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                // Log the error or handle it accordingly
                TempData["msg"] = "An error occurred while resetting your password. Please try again later.";
                return View("ResetPassword");
            }
        }

        private bool ValidateResetToken(string token)
        {
            // Implement your logic to validate the reset token
            // For simplicity, assume all tokens are valid
            return true;
        }


        private string GetUserEmailFromToken()
        {
            try
            {
                string token = (string)Session["gettoken"];
                var userEmail = ent.AdminLogins.Where(t => t.Token == token).Select(t => t.Username).FirstOrDefault();

                if (string.IsNullOrEmpty(userEmail))
                {
                    TempData["msg"] = "Invalid or expired token";
                }
                return userEmail;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Invalid or expired token");
            }
        }
    }
}
