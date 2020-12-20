using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hombay.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index(int? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var user = db.sp_gettotaluser().ToList();
            Session["totaluser"] = user[0];
            var servicer = db.sp_gettotalservicer().ToList();
            Session["totalservicer"] = servicer[0];
            var landowner = db.sp_gettotallandowner().ToList();
            Session["totallandowner"] = landowner[0];
            var seller = db.sp_gettotalSeller().ToList();
            Session["totalSeller"] = seller[0];
            var Buyer = db.sp_gettotalBuyer().ToList();
            Session["totalBuyer"] = Buyer[0];
            var Bidding = db.sp_gettotalbidding().ToList();
            Session["totalBidding"] = Bidding[0];
            var Items = db.sp_gettotalItem().ToList();
            Session["totalItem"] = Items[0];
            var Complaint = db.sp_gettotalcomplaint().ToList();
            Session["totalcomplaint"] = Complaint[0];
            var LandConstruction = db.sp_gettotalconstruction().ToList();
            Session["totalConstruction"] = LandConstruction[0];


            var result = db.tbl_users.ToList().ToPagedList(i ?? 1, 5);

            return View(result);
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index","Home");
        }
        public ActionResult Profile()
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_users.Find(Convert.ToInt32(Session["UserID"]));

            return View(result);
        }
        [HttpPost]
        public ActionResult Profile(tbl_users _Users)
        {
            if (_Users.ImageFile != null)
            {



                string filename = Path.GetFileNameWithoutExtension(_Users.ImageFile.FileName);
                string extension = Path.GetExtension(_Users.ImageFile.FileName);
                filename = filename + DateTime.Now.ToString("yyyymmssff") + extension;
                _Users.Image = "~/Images/" + filename;
                filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                _Users.ImageFile.SaveAs(filename);

                HomeBeyDBEntities appEntities = new HomeBeyDBEntities();
             
                appEntities.Entry(_Users).State = System.Data.Entity.EntityState.Modified;
                appEntities.SaveChanges();
                TempData["Success"] = "Success! Your query is been executed.";
                return RedirectToAction("Profile", new { id = _Users.Id });
            }
            else if(_Users.ImageFile==null)
            {
                string filename = "~" + TempData["Data1"].ToString();
                _Users.Image = filename;

                HomeBeyDBEntities appEntities = new HomeBeyDBEntities();
                appEntities.Entry(_Users).State = System.Data.Entity.EntityState.Modified;
                appEntities.SaveChanges();
                TempData["Success"] = "Success! Your query is been executed.";
                return RedirectToAction("Profile", new { id = _Users.Id });
            }


            return View();



        }
        public ActionResult Delete_Bidding(int Id)
        {
            ViewBag.Success = TempData["Success"];
            using (HomeBeyDBEntities Context = new HomeBeyDBEntities())
            {
                User_Bidding deptDelete = Context.User_Bidding.Find(Id);
                Context.Entry(deptDelete).State = EntityState.Deleted;
                TempData["Success"] = "Success! Your query is been executed.";
                Context.SaveChanges();
            }

            return RedirectToAction("Bidding_list");
        }
        public ActionResult User_delete(int Id)
        {
            ViewBag.Success = TempData["Success"];
            using (HomeBeyDBEntities Context = new HomeBeyDBEntities())
            {
                tbl_users deptDelete = Context.tbl_users.Find(Id);
                Context.Entry(deptDelete).State = EntityState.Deleted;
                TempData["Success"] = "Success! Your query is been executed.";
                Context.SaveChanges();
            }

            return RedirectToAction("User_list");
        }
        public ActionResult complaint_delete(int Id)
        {
            ViewBag.Success = TempData["Success"];
            using (HomeBeyDBEntities Context = new HomeBeyDBEntities())
            {
                tbl_contact deptDelete = Context.tbl_contact.Find(Id);
                Context.Entry(deptDelete).State = EntityState.Deleted;
                TempData["Success"] = true;
                Context.SaveChanges();
            }

            return RedirectToAction("Complaint_list");
        }
        public ActionResult Delete_Item(int Id)
        {
            ViewBag.Success = TempData["Success"];
            using (HomeBeyDBEntities Context = new HomeBeyDBEntities())
            {
                tbl_items deptDelete = Context.tbl_items.Find(Id);
                Context.Entry(deptDelete).State = EntityState.Deleted;
                TempData["Success"] = "Success! Your query is been executed.";

                Context.SaveChanges();
            }

            return RedirectToAction("Item_list");
        }
        

            
       
        public ActionResult DeleteReview(int Id)
        {

            ViewBag.Success = TempData["Success"];
            using (HomeBeyDBEntities Context = new HomeBeyDBEntities())
            {
                tbl_Rating deptDelete = Context.tbl_Rating.Find(Id);
                Context.Entry(deptDelete).State = EntityState.Deleted;
                TempData["Success"] = "Success! Your query is been executed.";
                Context.SaveChanges();
            }

            return RedirectToAction("My_Purchase");
        }
        public ActionResult Add_Category()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Add_Category(tbl_Category _Category)
        {

            HomeBeyDBEntities homeBeyDBEntities = new HomeBeyDBEntities();
            homeBeyDBEntities.tbl_Category.Add(_Category);
            homeBeyDBEntities.SaveChanges();


            return View();
        }
        public ActionResult Edit_User(int Id)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_users.Find(Id);

            return View(result);
        }
        [HttpPost]
        public ActionResult Edit_User(tbl_users _Users)
        {
            if (_Users.ImageFile != null)
            {



                string filename = Path.GetFileNameWithoutExtension(_Users.ImageFile.FileName);
                string extension = Path.GetExtension(_Users.ImageFile.FileName);
                filename = filename + DateTime.Now.ToString("yyyymmssff") + extension;
                _Users.Image = "~/Images/" + filename;
                filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                _Users.ImageFile.SaveAs(filename);

                HomeBeyDBEntities appEntities = new HomeBeyDBEntities();

                appEntities.Entry(_Users).State = System.Data.Entity.EntityState.Modified;
                appEntities.SaveChanges();
                TempData["Success"] = "Success! Your query is been executed.";
                return RedirectToAction("Edit_User", new { id = _Users.Id });
            }
            else if (_Users.ImageFile == null)
            {
                string filename = "~" + TempData["Data1"].ToString();
                _Users.Image = filename;

                HomeBeyDBEntities appEntities = new HomeBeyDBEntities();
                appEntities.Entry(_Users).State = System.Data.Entity.EntityState.Modified;
                appEntities.SaveChanges();
                TempData["Success"] = "Success! Your query is been executed.";
                return RedirectToAction("Edit_User", new { id = _Users.Id });
            }


            return View();
        }


        public ActionResult Complaint_list(int? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_contact.ToList().ToPagedList(i ?? 1, 5);

            return View(result);
        }
        public ActionResult Item_list(int? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_items.ToList().ToPagedList(i ?? 1, 5);

            return View(result);
        }
        [HttpPost]
        public ActionResult Item_list(string search, int? i)
        {

            HomeBeyDBEntities db = new HomeBeyDBEntities();
            return View(db.tbl_items.Where(x => x.Name.Contains(search)).ToList().ToPagedList(i ?? 1, 5));


        }
        public ActionResult My_Purchase(int? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_Rating.ToList().ToPagedList(i ?? 1, 5);

            return View(result);

        }
        public ActionResult User_list(int? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_users.ToList().ToPagedList(i ?? 1, 5);

            return View(result);
        }
        public ActionResult Bidding_list(int? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.User_Bidding.ToList().ToPagedList(i ?? 1, 5);

            return View(result);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tbl_users users)
        {
            users.Role = "Admin";
            HomeBeyDBEntities webApp = new HomeBeyDBEntities();
            var result = webApp.tbl_users.Where(x => x.Username.Equals(users.Username) && x.Password.Equals(users.Password) && x.Role.Equals(users.Role)).FirstOrDefault();

            if (result != null)
            {
                Session["UserID"] = result.Id.ToString();
                Session["UserName"] = result.Username.ToString();
                if (result.Email != null)
                {
                    Session["Email"] = result.Email.ToString();
                }
                if (result.Mobile != null)
                {
                    Session["Mobile"] = result.Mobile.ToString();

                }

                Session["Password"] = result.Password.ToString();
                if (result.Address != null)
                {
                    Session["Address"] = result.Address.ToString();
                }
                return RedirectToAction("Index", "Admin");

            }
            ViewBag.exm = "Login Details are Incorrect!";
            return View(users);

           
        }
    }
}