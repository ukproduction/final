using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hombay.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_new_construction.ToList().ToPagedList(i ?? 1, 4);
            return View(result);
        }
        public ActionResult Login()
        {
            return View();
        }
       


      

        [HttpPost]
        public ActionResult Login_Admin(tbl_users users)
        {
            HomeBeyDBEntities webApp = new HomeBeyDBEntities();
            var result = webApp.tbl_users.Where(x => x.Username == users.Username && x.Password == users.Password).Count();

            if (result > 0)
            {
                return RedirectToAction("listuser");

            }
            else
            {
                return View();

            }


        }



        public ActionResult About()
        {
            return View("~/Views/Home/About.cshtml");
        }
        public ActionResult Contact()
        {
            ViewBag.Success = TempData["Success"];
            return View();
        }
        [HttpPost]
        public ActionResult Contact(tbl_contact contact)
        {
            
            HomeBeyDBEntities homeBeyDB = new HomeBeyDBEntities();
            homeBeyDB.tbl_contact.Add(contact);
            homeBeyDB.SaveChanges();
            TempData["Success"] = true;
            return View("Contact");
        }
        public ActionResult Products(int? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_items.ToList().ToPagedList(i ?? 1, 12);

            return View(result);
        }
        [HttpPost]
        public ActionResult Products(string search, int? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            return View(db.tbl_items.Where(x => x.Name.Contains(search)).ToList().ToPagedList(i ?? 1, 5));
        }
        public ActionResult Services()
        {
            return View("~/Views/Account/Services.cshtml");
        }
        public ActionResult Project_Details(int? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_proprogress.ToList().ToPagedList(i ?? 1, 8);

            return View(result);

        }
        public ActionResult Platforms()
        {
            return View("~/Views/Account/Platforms.cshtml");
        }
        public ActionResult x()
        {
            return View("~/Views/Account/x.cshtml");
        }
        public ActionResult landowner_form()
        {
            return View("~/Views/Account/landowner_form.cshtml");
        }
        public ActionResult servicer_form()
        {
            return View("~/Views/Account/servicer_form.cshtml");
        }
        public ActionResult seller_form()
        {

            return View("~/Views/Account/seller_form.cshtml");
        }
       
    }
}