using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Stripe.Checkout;

using Stripe;
using Hombay.Models;

using SessionService = Stripe.Checkout.SessionService;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using Session = Stripe.Checkout.Session;
using log4net;
using System.Data.Entity;
namespace Hombay.Controllers
{
    public class ServicerController : Controller
    {
        // GET: Servicer
        public ActionResult Index(int? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var mybid = db.sp_BuyerAccBid(Convert.ToInt32(Session["UserID"])).ToList();
            Session["mybid"] = mybid[0];
            var myproject = db.sp_Myprojects(Convert.ToInt32(Session["UserID"])).ToList();
            Session["myproject"] = myproject[0];
            var mycomplaints = db.sp_Usercomplaints(Convert.ToInt32(Session["UserID"])).ToList();
            Session["mycomplaint"] = mycomplaints[0];
            var myreview = db.sp_UserReviews(Convert.ToInt32(Session["UserID"])).ToList();
            Session["myreview"] = myreview[0];
            var myquery = db.sp_UserQuery(Convert.ToInt32(Session["UserID"])).ToList();
            Session["myquery"] = myquery[0];


            int id = Convert.ToInt32(Session["UserID"]);
            var result = db.tbl_new_construction.ToList().ToPagedList(i ?? 1, 5);

            return View(result);
        }
        public ActionResult New_bidding()
        {


            return View();
        }
        [HttpPost]
        public ActionResult New_bidding(User_Bidding tbl_Items)
        {
            string filename = "~" + TempData["Data1"].ToString();
            tbl_Items.Image = filename;

            //  tbl_Items.ImageFile.SaveAs(filename);
            HomeBeyDBEntities appEntities = new HomeBeyDBEntities();
            appEntities.User_Bidding.Add(tbl_Items);
            appEntities.SaveChanges();
            TempData["Success"] = "Success! Your query is been executed.";
            return RedirectToAction("New_Bidding");

           
        }

        

        public ActionResult Update_ProProgress(int Pro_id)
        {
            ViewBag.Success = TempData["Success"];
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_proprogress.Find(Pro_id);

            return View(result);


        }
        [HttpPost]
        public ActionResult Update_ProProgress(tbl_proprogress tbl_)
        {
            HomeBeyDBEntities appEntities = new HomeBeyDBEntities();
            appEntities.Entry(tbl_).State = System.Data.Entity.EntityState.Modified;
            appEntities.SaveChanges();
            TempData["Success"] = true;
            return RedirectToAction("Update_ProProgress", new { Pro_id = tbl_.Pro_id });


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


        public ActionResult Project_Progres(int Id)
        {
            ViewBag.Success = TempData["Success"];
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.User_Bidding.Find(Id);
            
            return View(result);


        }
        [HttpPost]
        public ActionResult Project_Progres(tbl_proprogress tbl_Pro)
        {
            string filename = "~" + TempData["Data1"].ToString();
            tbl_Pro.Image = filename;
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            db.tbl_proprogress.Add(tbl_Pro);
            db.SaveChanges();
            TempData["Success"] = "Success! Your query is been executed.";
            return RedirectToAction("Project_Progres");


        }
        public ActionResult Bidding_list(int? i)
        {

            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.User_Bidding.ToList().ToPagedList(i ?? 1, 5);

            return View(result);


        }
        public ActionResult Add_to_Cart(tbl_Order _Order)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();


            var userid = Convert.ToInt32(Session["UserID"]);
            List<User_Bidding> items = db.User_Bidding.Where(x=> x.Approve=="Payment" & x.User_Id== userid).ToList();

            if (items != null)
            {

                ViewBag.Id = items;
                ViewBag.Item_Name = items;
                ViewBag.price = items;
               
            }


            var mypaidprice = db.sp_paidItemprice(Convert.ToInt32(Session["UserID"])).ToList();
            Session["mypaid"] = mypaidprice[0];


            //Session["Ids"] = model.Id;

            //Session["IdofItemUser"] = model.User_id;
            //Session["Item_price"] = model.Item_price;
            //long Itmprice = long.Parse(model.Item_price.ToString()+"00");
            //Session["Item_titles"] = model.Item_title;




            // Set your secret key. Remember to switch to your live secret key in production!
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.ApiKey = "sk_test_51HrzKFJWAkWQReajWOSIktUeDnTgiJGp9ITWrFBwR0AJilWV8mumjJc1KhsDHpyAJzRyiU9RypxoERNl9ZTsIFsm00Nf69z591";



            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>
              {
              new SessionLineItemOptions
              {
              PriceData = new SessionLineItemPriceDataOptions
              {
              UnitAmount =40000,
              Currency = "PKR",
              ProductData = new SessionLineItemPriceDataProductDataOptions
              {
                Name = "hi",
              },

              },
            Quantity = 1,

            },
            },
                Mode = "payment",
                SuccessUrl = "http://localhost:52354/Seller/Thanks",


                CancelUrl = "https://example.com/cancel",
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = new Dictionary<string, string>
                    {
                        {"Order_ID","12345" },
                        {"Description","homebey Fazal Abbas Your enter Destription" },
                    }
                }
            };

            var service = new SessionService();
            Session session = service.Create(options);
            var id = session.PaymentIntentId;

            if (session != null)
            {
                HomeBeyDBEntities NWEntities = new HomeBeyDBEntities();

                foreach (var price in items)
                {
                    tbl_Order newProduct = new tbl_Order();
                    newProduct.Product_price = price.price;
                    newProduct.Id_Number = id;
                    newProduct.Item_Userid = price.Buyer_Id;
                    newProduct.Product_name = price.Item_Name;
                    newProduct.Date = DateTime.Now;
                    newProduct.User_id = Convert.ToInt32(Session["UserID"]);
                    NWEntities.tbl_Order.Add(newProduct);
                }

                NWEntities.SaveChanges();

                //var deleteid = Convert.ToInt32(Session["UserID"]);
                //tbl_cart_item deptDelete = NWEntities.tbl_cart_item.Find(deleteid);
                //NWEntities.Entry(deptDelete).State = EntityState.Deleted;

                //NWEntities.SaveChanges();


            }













            return View(session);

            //HomeBeyDBEntities db = new HomeBeyDBEntities();
            //var result = db.tbl_items.Find(Id);



        }
        public ActionResult Complaint()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Complaint(tbl_contact _Contact)
        {
            _Contact.Email = Session["Email"].ToString();
            _Contact.User_id = Convert.ToInt32(Session["UserID"]);
            _Contact.Name = Session["UserName"].ToString();
            HomeBeyDBEntities home = new HomeBeyDBEntities();
            home.tbl_contact.Add(_Contact);
            home.SaveChanges();
            TempData["Success"] = "Success! Your query is been executed.";
            return View();

        }
        public ActionResult Item_List(int? i)
        {

            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_new_construction.ToList().ToPagedList(i ?? 1, 5);
            return View(result);


        }
        public ActionResult Inprogress_Project(int ? i)
        {

            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_proprogress.ToList().ToPagedList(i ?? 1, 5); 
            return View(result);


        }

        public ActionResult New_Bid(int Id)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_new_construction.Find(Id);

            return View(result);
        }
        [HttpPost]
        public ActionResult New_Bid(User_Bidding bidding)
        {
            HomeBeyDBEntities homeBeyDB = new HomeBeyDBEntities();
            int id =Convert.ToInt32(Session["UserID"]);
            var Record = homeBeyDB.User_Bidding.Where(o => o.Id == id & o.Date == DateTime.Today).Select(o => o.Date).Count();




                if (Record<10)
                {
                    bidding.Date = DateTime.Today;
                    string filename = "~" + TempData["Data1"].ToString();
                    bidding.Image = filename;

                    //  tbl_Items.ImageFile.SaveAs(filename);
                    HomeBeyDBEntities appEntities = new HomeBeyDBEntities();
                    appEntities.User_Bidding.Add(bidding);
                    appEntities.SaveChanges();
                TempData["Success"] = "Success! Your query is been executed.";
                return RedirectToAction("New_Bid");
                }
                else
                {


                }


                return View();

            
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
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ServicerLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ServicerLogin(tbl_users users)
        {
            users.Role = "Servicer";

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
                return RedirectToAction("Index", "Servicer");

            }
            ViewBag.exm = "Login Details are Incorrect!";
            return View(users);
        }
    }
}