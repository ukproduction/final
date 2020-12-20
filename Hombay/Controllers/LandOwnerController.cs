using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hombay.Controllers
{
    public class LandOwnerController : Controller
    {
        // GET: LandOwner
        public ActionResult Index(int ? i)
        {



            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var consconunt = db.sp_landownerConscount(Convert.ToInt32(Session["UserID"])).ToList();
            Session["consconunt"] = consconunt[0];
            var quansum = db.sp_landownerquansum(Convert.ToInt32(Session["UserID"])).ToList();
            Session["quansum"] = quansum[0];
            var mycomplaints = db.sp_Usercomplaints(Convert.ToInt32(Session["UserID"])).ToList();
            Session["mycomplaint"] = mycomplaints[0];
            var myreview = db.sp_UserReviews(Convert.ToInt32(Session["UserID"])).ToList();
            Session["myreview"] = myreview[0];
            var myquery = db.sp_UserQuery(Convert.ToInt32(Session["UserID"])).ToList();
            Session["myquery"] = myquery[0];


            int id = Convert.ToInt32(Session["UserID"]);
            var result = db.tbl_items.Where(x => x.User_id == id).ToList().ToPagedList(i ?? 1, 5);

            return View(result);

        
        }
        public ActionResult Profile()
        {
            return View();
        }
        public ActionResult Construction_list(int ? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_new_construction.ToList().ToPagedList(i ?? 1, 5);

            return View(result);
        }
        public ActionResult LandownerLogin()
        {
            return View();
        }
        public ActionResult New_Construction()
        {
            return View();
        }
        [HttpPost]
        public ActionResult New_Construction(tbl_new_construction _Items)
        {



            if (_Items.ImageFile != null)
            {



                string filename = Path.GetFileNameWithoutExtension(_Items.ImageFile.FileName);
                string extension = Path.GetExtension(_Items.ImageFile.FileName);
                filename = filename + DateTime.Now.ToString("yyyymmssff") + extension;
                _Items.Image = "~/Images/" + filename;
                filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                _Items.ImageFile.SaveAs(filename);
            }
            using (HomeBeyDBEntities appEntities = new HomeBeyDBEntities())
            {
                appEntities.tbl_new_construction.Add(_Items);

                appEntities.SaveChanges();
            }

            ViewBag.exms = "Success!!! New Item added Successfully!";
            return RedirectToAction("New_Construction", new { id = _Items.Id });

        }

        [HttpPost]
        public ActionResult LandownerLogin(tbl_users users)
        {

            HomeBeyDBEntities webApp = new HomeBeyDBEntities();
            var result = webApp.tbl_users.Where(x => x.Username.Equals(users.Username) && x.Password.Equals(users.Password) && x.Role.Equals(users.Role)).FirstOrDefault();

            if (result != null)
            {
                Session["UserID"] = result.Id.ToString();
                Session["UserName"] = result.Username.ToString();
                Session["Email"] = result.Email.ToString();
                Session["Mobile"] = result.Mobile.ToString();
                Session["Password"] = result.Password.ToString();
                Session["Address"] = result.Address.ToString();
                return RedirectToAction("Index", "LandOwner");

            }
            ViewBag.exm = "Login Details are Incorrect!";
            return View(users);

        }
    }
}