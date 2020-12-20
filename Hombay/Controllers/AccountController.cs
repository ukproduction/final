using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Hombay.Models;
using PagedList;

namespace Hombay.Controllers { 

    public class AccountController: Controller
    {

        public ActionResult ForgotPassword()
        {
            return View();
        }


        [NonAction]
        public string UpdatePassword()
        {
            HomeBeyDBEntities homes = new HomeBeyDBEntities();
            string usrEmail = Session["Mail"].ToString();
            var users = homes.tbl_users.Where(x => x.Email == usrEmail).Select(x => x.Id ).FirstOrDefault();






            var user = new tbl_users() { Id =Convert.ToInt32(users), Password = Session["Password"].ToString() };
            HomeBeyDBEntities home = new HomeBeyDBEntities();
            home.tbl_users.Attach(user);
            home.Entry(user).Property(x => x.Password).IsModified = true;
            home.Configuration.ValidateOnSaveEnabled = false;
            home.SaveChanges();
            
            return "ok";
        }

        [HttpPost]
      
        public ActionResult ForgotPassword(MessageModel model)
        {

            HomeBeyDBEntities webApp = new HomeBeyDBEntities();
            var result = webApp.tbl_users.Where(x => x.Email.Equals(model.To)).FirstOrDefault();

            if (result != null)
            {

                using (MailMessage mm = new MailMessage("mfazalabbas5@gmail.com", model.To))
                {




                    mm.Subject = "Your Account Password";
                    mm.Body = "Xife" + DateTime.Now.Second;

                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("mfazalabbas5@gmail.com", "ldguhytvxgcamsvh");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    Session["Mail"] = mm.To;
                    Session["Password"] = mm.Body;
                    UpdatePassword();
                    ViewBag.Message = "Success! Your Password Send to Email.";
                }
                
            }

            return View();
        }
        public ActionResult SuccessMessage()
        {
            return View();
        }
        public ActionResult Project_Details(int? i)
        {

            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_proprogress.ToList().ToPagedList(i ?? 1, 8);

            return View(result);
           
        }
        public ActionResult Index()
        {


            return View("~/Views/Home/Index.cshtml");
        }
        public ActionResult SignUp()
        {
            return View("~/Views/Account/SignUp.cshtml");
        }
        public ActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }
        public ActionResult About()
        {
            return View("~/Views/Account/About.cshtml");
        }
        public ActionResult Gallery()
        {
            return View("~/Views/Account/Gallery.cshtml");
        }

      
        public ActionResult seller_form()
        {
          

            return View();
        }

        [HttpPost]
        public ActionResult seller_form(tbl_users tbl_Users)
        {
            HomeBeyDBEntities dBEntities = new HomeBeyDBEntities();
            dBEntities.tbl_users.Add(tbl_Users);
            dBEntities.SaveChanges();

            return View();
        }

    }
}