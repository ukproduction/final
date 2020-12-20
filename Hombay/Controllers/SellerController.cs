using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Data.Entity;

namespace Hombay.Controllers
{
    public class SellerController : Controller
    {
        // GET: Seller
        public ActionResult Index(int ? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var selleritem = db.sp_SellerItemcount(Convert.ToInt32(Session["UserID"])).ToList();
            Session["selleritem"] = selleritem[0];
           
            var itemprice = db.sp_SellerItemprice(Convert.ToInt32(Session["UserID"])).ToList();
          if(itemprice !=null)
            {
                Session["itemprice"] = itemprice[0];
            }
            else
            {
                Session["itemprice"] = 0;

            }
            
           
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

        public ActionResult Delete(int Id)
        {
            using (HomeBeyDBEntities Context = new HomeBeyDBEntities())
            {
                tbl_items deptDelete = Context.tbl_items.Find(Id);
                Context.Entry(deptDelete).State = EntityState.Deleted;
                Context.SaveChanges();
            }

            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Deleteproduct(int Id)
        {

            ViewBag.Success = TempData["Success"];
            using (HomeBeyDBEntities Context = new HomeBeyDBEntities())
            {
                tbl_items deptDelete = Context.tbl_items.Find(Id);
                Context.Entry(deptDelete).State = EntityState.Deleted;
                TempData["Success"] = "Success! Your query is been executed.";
                Context.SaveChanges();
            }

            return RedirectToAction("Item_List");
        }
        public ActionResult Approve(int Id)
        {

            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.User_Bidding.Find(Id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Approve(User_Bidding tbl_Items)
        {


            string filename = "~" + TempData["Data1"].ToString();
            tbl_Items.Image = filename;

            //  tbl_Items.ImageFile.SaveAs(filename);
            HomeBeyDBEntities appEntities = new HomeBeyDBEntities();
            appEntities.Entry(tbl_Items).State = System.Data.Entity.EntityState.Modified;
          
            appEntities.SaveChanges();
            TempData["Success"] = "Success! Your query is been executed.";
            return RedirectToAction("Approve", new { id = tbl_Items.Id });
        }


        public ActionResult Bidding_list(int? i)
        {
         
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.User_Bidding.ToList().ToPagedList(i?? 1,5);

            return View(result);
          
        }
        public ActionResult profile()
        {
            if (Session["UserName"] != null)
            {

                HomeBeyDBEntities db = new HomeBeyDBEntities();
                var result = db.tbl_users.Find(Convert.ToInt32(Session["UserID"]));

                return View(result);



               
            }
            else
            {
                return RedirectToAction("LoginSeller");
            }



           
        }
        [HttpPost]
        public ActionResult profile(tbl_users _Users)
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
            else if (_Users.ImageFile == null)
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
        public ActionResult Complaint()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Complaint(tbl_contact _Contact)
        {
            _Contact.Email = Session["Email"].ToString();
            _Contact.User_id =Convert.ToInt32(Session["UserID"]);
            _Contact.Name = Session["UserName"].ToString();
            HomeBeyDBEntities home = new HomeBeyDBEntities();
            home.tbl_contact.Add(_Contact);
            home.SaveChanges();
            TempData["Success"] = "Success! Your query is been executed.";
            return View();

        }
        public ActionResult LoginSeller()
        {
            return View();

        }


        [HttpPost]
        public ActionResult LoginSeller(tbl_users users)
        {
            users.Role = "Seller";
            HomeBeyDBEntities webApp = new HomeBeyDBEntities();
            var result = webApp.tbl_users.Where(x => x.Username.Equals(users.Username) && x.Password.Equals(users.Password) && x.Role.Equals(users.Role)).FirstOrDefault();

            if (result != null)
            {
                Session["UserID"] = result.Id.ToString();
                Session["UserName"] = result.Username.ToString();
                if(result.Email !=null)
                {
                    Session["Email"] = result.Email.ToString();
                }
               if(result.Mobile !=null)
                {
                    Session["Mobile"] = result.Mobile.ToString();

                }
               
                Session["Password"] = result.Password.ToString();
                if(result.Address !=null)
                {
                    Session["Address"] = result.Address.ToString();
                }
               
               ViewBag.img = Url.Content(result.Image);
                return RedirectToAction("Index", "Seller");

            }
            ViewBag.exm = "Login Details are Incorrect!";
            return View(users);


        }
        public ActionResult New_item()
        {
            HomeBeyDBEntities dBEntities = new HomeBeyDBEntities();
            var items = dBEntities.tbl_Category.ToList();

            if (items != null)
            {

                ViewBag.data = items;
            }
           

            return View();
        }

            [HttpPost]
        public ActionResult New_item(tbl_items _Items)
        {

            _Items.Item_title = _Items.Name;

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
                appEntities.tbl_items.Add(_Items);
               
                appEntities.SaveChanges();
                TempData["Success"] = "Success! Your query is been executed.";
            }

            return RedirectToAction("New_item");

            //return View();
        }
        [HttpPost]
        public ActionResult Add_Item()
        {
           
            return View();
        }
        public ActionResult Sold_Item(int ? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            int id = Convert.ToInt32(Session["UserID"]);
            var result = db.tbl_Order.Where(x => x.Item_Userid == id).ToList().ToPagedList(i ?? 1, 5);

            return View(result);
        }
        public ActionResult live_Chat()
        {

            HomeBeyDBEntities dBEntities = new HomeBeyDBEntities();
            List<tbl_users> items = dBEntities.tbl_users.ToList();

            if (items != null)
            {

                ViewBag.data = items;
            }
            else
            {

            }

            return View();



        }
        [HttpPost]
        public ActionResult live_Chat(tbl_Chat tbl_)
        {





            tbl_.Time = DateTime.Now;
            tbl_.Sender_user = Session["UserName"].ToString();
            tbl_.Sender_id = Convert.ToInt32(Session["UserID"]);

            HomeBeyDBEntities db = new HomeBeyDBEntities();
            db.tbl_Chat.Add(tbl_);
            db.SaveChanges();




            var result = db.tbl_Chat.Where(x => x.Sender_user.Equals(tbl_.Sender_user) & x.Receiver_user.Equals(tbl_.Receiver_user)).ToList();

            live_Chat();

            return View(result);


        }
        public ActionResult Add_Category()
        {

            return View();
        }

        public ActionResult Item_list(int ? i)
        {
            int id = Convert.ToInt32(Session["UserID"]);
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_items.Where(x => x.User_id == id).ToList().ToPagedList(i ?? 1, 5);
            return View(result);

        }
        [HttpPost]
        public ActionResult Item_list(string search, int? i)
        {
            int id = Convert.ToInt32(Session["UserID"]);
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            return View(db.tbl_items.Where(x => x.Name.Contains(search) && x.User_id == id).ToList().ToPagedList(i ?? 1, 5));


        }
        public ActionResult Update_Item(int Id)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_items.Find(Id);

            return View(result);
        }
        [HttpPost]
        public ActionResult Update_Item(tbl_items tbl_Items)
        {
            if (tbl_Items.ImageFile != null)
            {



                string filename = Path.GetFileNameWithoutExtension(tbl_Items.ImageFile.FileName);
                string extension = Path.GetExtension(tbl_Items.ImageFile.FileName);
                filename = filename + DateTime.Now.ToString("yyyymmssff") + extension;
                tbl_Items.Image = "~/Images/" + filename;
                filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                tbl_Items.ImageFile.SaveAs(filename);
                HomeBeyDBEntities appentity = new HomeBeyDBEntities();
                appentity.Entry(tbl_Items).State = System.Data.Entity.EntityState.Modified;
                appentity.SaveChanges();
                TempData["Success"] = "Success! Your query is been executed.";
                return RedirectToAction("Update_Item", new { id = tbl_Items.Id });
            }
            else
            {
                string filename = "~" + TempData["Data1"].ToString();
                tbl_Items.Image = filename;

                HomeBeyDBEntities appEntities = new HomeBeyDBEntities();
                appEntities.Entry(tbl_Items).State = System.Data.Entity.EntityState.Modified;
                appEntities.SaveChanges();
                TempData["Success"] = "Success! Your query is been executed.";
                return RedirectToAction("Update_Item", new { id = tbl_Items.Id });

            }
         


          
        }

        [HttpPost]
        public ActionResult Add_Category(tbl_Category _Category)
        {

            HomeBeyDBEntities homeBeyDBEntities = new HomeBeyDBEntities();
            homeBeyDBEntities.tbl_Category.Add(_Category);
            homeBeyDBEntities.SaveChanges();
            TempData["Success"] = "Success! Your query is been executed.";

            return View();
        }
    }
}