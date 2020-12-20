using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Stripe.Infrastructure;
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
    public class BuyerController : Controller
    {
        private ILog log = LogManager.GetLogger("BuyerController");

        // GET: Buyer
        public ActionResult Index(int ? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var myaccbid = db.sp_BuyerAccBid(Convert.ToInt32(Session["UserID"])).ToList();
            Session["myaccbid"] = myaccbid[0];
            var mybid = db.sp_AddContructionAddBid(Convert.ToInt32(Session["UserID"])).ToList();
            Session["mybid"] = mybid[0];
            var mycomplaints = db.sp_Usercomplaints(Convert.ToInt32(Session["UserID"])).ToList();
            Session["mycomplaint"] = mycomplaints[0];
            var mypurchase = db.sp_MypurchaseItem(Convert.ToInt32(Session["UserID"])).ToList();
            Session["mypurchase"] = mypurchase[0];
            var myquery = db.sp_UserQuery(Convert.ToInt32(Session["UserID"])).ToList();
            Session["myquery"] = myquery[0];


            int id = Convert.ToInt32(Session["UserID"]);
            var result = db.tbl_items.ToList().ToPagedList(i ?? 1, 5);

            return View(result);





        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Reviewrating()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Reviewrating(tbl_Rating rating)
        {
            rating.Item_Name = "Ai";


            return View();
        }

        public ActionResult LoginBuyer()
        {

            return View();
        }

        [HttpPost]
        public ActionResult LoginBuyer(tbl_users users)
        {
            users.Role = "Buyer";
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
                return RedirectToAction("Index", "Buyer");

            }

            return View(users);

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
                return RedirectToAction("LoginBuyer");
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
        public ActionResult My_Bidding()
        {
            HomeBeyDBEntities home = new HomeBeyDBEntities();
          
            var result = home.User_Bidding.ToList();
            return View(result);
        }
        public ActionResult Item_List(int? i)
        {

            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_items.ToList().ToPagedList(i ?? 1, 5);
            return View(result);


        }
        [HttpPost]
        public ActionResult Item_List( string search, int? i)
        {

            HomeBeyDBEntities db = new HomeBeyDBEntities();
            return View(db.tbl_items.Where(x => x.Name.Contains(search)).ToList().ToPagedList(i ?? 1, 5));


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
            tbl_Items.Buyer_Id =Convert.ToInt32(Session["UserID"]);
            //  tbl_Items.ImageFile.SaveAs(filename);
            HomeBeyDBEntities appEntities = new HomeBeyDBEntities();
            appEntities.Entry(tbl_Items).State = System.Data.Entity.EntityState.Modified;

            appEntities.SaveChanges();
            TempData["Success"] = "Success! Your query is been executed.";
            return RedirectToAction("Approve", new { id = tbl_Items.Id });
        }

        public ActionResult Inprogress_Project(int? i)
        {

            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_proprogress.ToList().ToPagedList(i ?? 1, 5);
            return View(result);


        }
        public ActionResult My_Purchase(int? i)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_Rating.ToList().ToPagedList(i ?? 1, 5);

            return View(result);

        }
        HomeBeyDBEntities db = new HomeBeyDBEntities();
        public ActionResult New_Cart(int? i)
        {
            int id = Convert.ToInt32(Session["UserID"]);
            var items = db.tbl_cart_item.Where(x=>x.user_id==id).ToList();

            if (items != null)
            {

                ViewBag.data = items;
            }

            var result = db.tbl_items.ToList().ToPagedList(i ?? 1, 8);
            return View(result);


        }
        [HttpPost]
        public ActionResult New_Cart(tbl_cart_item _Cart_Item, string search,int ? i)
        {
            if(search ==null && _Cart_Item.Item_title!=null)
            {

           
            var title=_Cart_Item.Item_title;
            var deleteid = db.tbl_cart_item.FirstOrDefault(x => x.Item_title == title);
            var newdeleteid = deleteid.Id;

            using (HomeBeyDBEntities Context = new HomeBeyDBEntities())
            {
                tbl_cart_item deptDelete = Context.tbl_cart_item.Find(newdeleteid);
                Context.Entry(deptDelete).State = EntityState.Deleted;
                    TempData["Success"] = "Success! Your query is been executed.";
                    Context.SaveChanges();
            }
            }

            var id = _Cart_Item.product_id;
            if(id !=null)
            {
                var model = db.tbl_items.FirstOrDefault(x => x.Id == id);

                _Cart_Item.Item_title = model.Name;
                _Cart_Item.product_id = model.Id;
                _Cart_Item.Item_price = model.Item_price;
                _Cart_Item.quantity = Convert.ToInt32(model.Quantity);
                _Cart_Item.user_id = Convert.ToInt32(Session["UserID"]);
                db.tbl_cart_item.Add(_Cart_Item);
                db.SaveChanges();
            }
            int Myid = Convert.ToInt32(Session["UserID"]);
            var items = db.tbl_cart_item.Where(x => x.user_id == Myid).ToList();

            if (items != null)
            {

                ViewBag.data = items;
            }
            if(search !=null)
            {
                return View(db.tbl_items.Where(x => x.Name.Contains(search)).ToList().ToPagedList(i ?? 1, 5));
            }

           
            
           
            //Session["Ids"] =

            //Session["IdofItemUser"] = model.User_id;
            //Session["Item_price"] =
            //long Itmprice = long.Parse(model.Item_price.ToString() + "00");
            //Session["Item_titles"] =


            return View();
        }
        public ActionResult Mainpurchase(int? i)
        {
            int id = Convert.ToInt32(Session["UserID"]);
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_Order.Where(z=>z.User_id==id).ToList().ToPagedList(i ?? 1, 8);
            return View(result);


        }
        public ActionResult Item_Detail(int Id)
        {


            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_Order.Find(Id);
            return View(result);

           
        }
        [HttpPost]
        public ActionResult Item_Detail(tbl_Rating tbl)
        {
          var names=  tbl.Item_Name;
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_items.FirstOrDefault(x => x.Name == names);
            int idofitem = result.Id;
            int Rating = Convert.ToInt32(tbl.Rating);
           db.tbl_Rating.Add(tbl);
            db.SaveChanges();
            TempData["Success"] = "Success! Your query is been executed.";
            ChangePassword(idofitem, Rating);
            return RedirectToAction("Item_Detail");


        }
       
        public void ChangePassword(int userId, int Rating)
        {
          
            var user = new tbl_items() { Id = userId, Rating = Rating };
            using (var db = new HomeBeyDBEntities())
            {
                db.tbl_items.Attach(user);
                db.Entry(user).Property(x => x.Rating).IsModified = true;
                TempData["Success"] = "Success! Your query is been executed.";
                db.SaveChanges();
            }
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
        public ActionResult AjaxMethod(string name)
        {
            var sender = Session["UserName"].ToString();
            var Receiver = name;

            var result = db.tbl_Chat.Where(x => x.Sender_user.Equals(sender) & x.Receiver_user.Equals(Receiver)).ToList();

            live_Chat();

            return View(result);
        }


        [HttpPost]
        public ActionResult live_Chat(tbl_Chat tbl_, string value)
        {

            tbl_.Time = DateTime.Now;
            tbl_.Sender_user = Session["UserName"].ToString();
            tbl_.Sender_id = Convert.ToInt32(Session["UserID"]);
            tbl_.Sender= Session["UserID"].ToString();
            var value1 = Session["UserName"].ToString();
            var value2 = tbl_.Receiver_user;
            HomeBeyDBEntities db = new HomeBeyDBEntities();

            string slt = "SELECT Sender FROM tbl_Chat WHERE Sender_user='" + value1 + "' and Receiver_user='"+ value2 + "'";
            var sltresult = db.tbl_Chat.SqlQuery(slt);

            if (sltresult != null)
            {
                var sendid= Session["UserID"].ToString();
                var getid = db.tbl_Chat.FirstOrDefault(x => x.Sender == sendid);
                var idofsender = getid.Sender;

                tbl_.Sender = idofsender;

                db.tbl_Chat.Add(tbl_);
                db.SaveChanges();


            }
            else if(sltresult == null)
            {
                tbl_.Sender= Session["UserID"].ToString();
                db.tbl_Chat.Add(tbl_);
                db.SaveChanges();

            }

            






            
            live_Chat();


            // var result = db.sp_Userchats(tbl_.Sender_user,tbl_.Receiver_user);

            // var result = db.tbl_Chat.Where(x => x.Sender_user.Equals(tbl_.Sender_user) & x.Receiver_user.Equals(tbl_.Receiver_user)).ToList();
            //  string query = "SELECT * FROM tbl_Chat WHERE Sender_user IN ( '"+ tbl_.Sender_user + "', '"+ tbl_.Receiver_user + "')";
           
            string query = "SELECT * FROM tbl_Chat WHERE Sender_user in ('" + value1 + "' ,'" + value2 + "')";
            var result = db.tbl_Chat.SqlQuery(query);
            return View(result);


        }

        [HttpPost]
        public ActionResult GetData(Student Student)
        {
            var StudentId = Student.Recevier_user.ToString();
          
            return Json(JsonRequestBehavior.AllowGet);
        }

        public string getlivechat(tbl_Chat tbl)
        {
            tbl.Sender_user = "Seller";
            tbl.Receiver_user = "Buyer";
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_Chat.OrderByDescending(x => x.Sender_user.Equals(tbl.Sender_user) && x.Receiver_user.Equals(tbl.Receiver_user)).ToList();

            return ("Ok");
        }

        public ActionResult Add_to_Cart(tbl_Order _Order)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
         

           
            List<tbl_cart_item> items = db.tbl_cart_item.ToList();

            if (items != null)
            {

                ViewBag.product_id = items;
                ViewBag.Item_title = items;
                ViewBag.Item_price = items;
                ViewBag.quantity = items;
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

            if(session!=null)
            {
                HomeBeyDBEntities NWEntities = new HomeBeyDBEntities();

                foreach (var price in items)
                {
                    tbl_Order newProduct = new tbl_Order();
                    newProduct.Product_price = price.Item_price;
                    newProduct.Id_Number = id;
                    newProduct.Item_Userid = price.Id;
                    newProduct.Product_name = price.Item_title;
                    newProduct.Date = DateTime.Now;
                    newProduct.User_id = price.user_id;
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



        public ActionResult New_Construction()
        {
            ViewBag.Success = TempData["Success"];
            return View();
        }
      
        [HttpPost]
        public ActionResult New_Construction(tbl_new_construction _Items)
        {



            if (_Items.ImageFile != null)
            {

                _Items.Title = _Items.Name;

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
                TempData["Success"] = "Success! Your query is been executed.";
                appEntities.SaveChanges();
            }

          
            return RedirectToAction("New_Construction", new { id = _Items.Id });

        }
        public ActionResult New_Bid(int Id)
        {
            HomeBeyDBEntities db = new HomeBeyDBEntities();
            var result = db.tbl_items.Find(Id);

            return View(result);
        }
        [HttpPost]
        public ActionResult New_Bid(User_Bidding tbl_Items )
        {
           
           
           
                string filename = "~"+TempData["Data1"].ToString();
                tbl_Items.Image = filename;
               
              //  tbl_Items.ImageFile.SaveAs(filename);
                HomeBeyDBEntities appEntities = new HomeBeyDBEntities();
                appEntities.User_Bidding.Add(tbl_Items);
                appEntities.SaveChanges();
            TempData["Success"] = "Success! Your query is been executed.";
            return RedirectToAction("New_Bid");

           
           
        }
       
        public ActionResult CheckoutBuyer()
        {

            Stripe.StripeConfiguration.SetApiKey("sk_test_51HrzKFJWAkWQReajWOSIktUeDnTgiJGp9ITWrFBwR0AJilWV8mumjJc1KhsDHpyAJzRyiU9RypxoERNl9ZTsIFsm00Nf69z591");
           
            Stripe.CreditCardoptions card = new Stripe.CreditCardoptions();
            card.Name = "Muhammad Fazal" + " " + "Abbas";
            card.Number = "4649510301037203";
            card.ExpYear = "2022";
            card.ExpMonth = "11";
            card.Cvc = "639";

            //Assign Card to Token Object and create Token  
            Stripe.TokenCreateOptions token = new Stripe.TokenCreateOptions();
            token.Card = card.ToString();
            Stripe.TokenService serviceToken = new Stripe.TokenService();
            Stripe.Token newToken = serviceToken.Create(token);


          
            return View();
        }

        public ActionResult Charges()
        {
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
              UnitAmount = (long)Session["Item_price"],
              Currency = "PKR",
              ProductData = new SessionLineItemPriceDataProductDataOptions
              {
                Name =Session["Item_title"].ToString(),
              },

              },
            Quantity = 1,
           
            },
            },
                Mode = "payment",
                SuccessUrl = "https://example.com/success",
                CancelUrl = "https://example.com/cancel",
                PaymentIntentData =new SessionPaymentIntentDataOptions
                {
                    Metadata =new Dictionary<string,string>
                    {
                        {"Order_ID","12345" },
                        {"Description","homebey Fazal Abbas Your enter Destription" },
                    }
                }
            };

            var service = new SessionService();
            Session session = service.Create(options);






            return View(session);
        }


        public ActionResult Thanks()
        {

            return View();
        }


        [HttpPost]
        public ActionResult Charges(string stripeToken, string stripeEmail)
        {

            StripeConfiguration.ApiKey = "sk_test_51HrzKFJWAkWQReajWOSIktUeDnTgiJGp9ITWrFBwR0AJilWV8mumjJc1KhsDHpyAJzRyiU9RypxoERNl9ZTsIFsm00Nf69z591";

            var options = new PaymentIntentCreateOptions
            {
                Amount = 50000,
                Currency = "pkr",
              
               
            PaymentMethodTypes = new List<string>
                       {
                        "card",
                       },
                ReceiptEmail = "mfazalabbas5@gmail.com",
            };


            Stripe.TokenCreateOptions token = new Stripe.TokenCreateOptions();
            token.Card = options.ToString();
            Stripe.TokenService serviceToken = new Stripe.TokenService();
            Stripe.Token newToken = serviceToken.Create(token);


            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);


            //ViewBag.StripePublishKey = ConfigurationManager.AppSettings["pk_test_51HrzKFJWAkWQReajLesoTWvF3tGF0iOZCpDVpkwJrcUrhcLHf3jCriPbMvIG9dLLZKTFQnShS3RUis2koJ6i4qGJ00MgENXbP9"];
            //Stripe.StripeConfiguration.SetApiKey("pk_test_51HrzKFJWAkWQReajLesoTWvF3tGF0iOZCpDVpkwJrcUrhcLHf3jCriPbMvIG9dLLZKTFQnShS3RUis2koJ6i4qGJ00MgENXbP9");
            //Stripe.StripeConfiguration.ApiKey = "sk_test_51HrzKFJWAkWQReajWOSIktUeDnTgiJGp9ITWrFBwR0AJilWV8mumjJc1KhsDHpyAJzRyiU9RypxoERNl9ZTsIFsm00Nf69z591";

            //var myCharge = new Stripe.ChargeCreateOptions();
            //// always set these properties
            //myCharge.Amount = 1;
            //myCharge.Currency = "USD";
            //myCharge.ReceiptEmail = stripeEmail;
            //myCharge.Description = "Sample Charge";
            //myCharge.Source = "pi_1HsPrxJWAkWQReajOgTfBbWK";
            //myCharge.Capture = true;
            //var chargeService = new Stripe.ChargeService();
            //Charge stripeCharge = chargeService.Create(myCharge);


            


          //  Stripe.StripeConfiguration.SetApiKey("sk_test_51HrzKFJWAkWQReajWOSIktUeDnTgiJGp9ITWrFBwR0AJilWV8mumjJc1KhsDHpyAJzRyiU9RypxoERNl9ZTsIFsm00Nf69z591");

            //  Stripe.CreditCardoptions card = new Stripe.CreditCardoptions();
            ////  card.Name = "Iqra" + " " + "Rubab";
            //  card.Number = "4242424242424242";
            //  card.ExpYear = "2020";
            //  card.ExpMonth = "12";
            //  card.Cvc = "123";
            //// card.Email = "mfazalabbas5@gmail.com";
            // // card.Currency = "Pkr";

            //  //Assign Card to Token Object and create Token  
            //  Stripe.TokenCreateOptions token = new Stripe.TokenCreateOptions();
            //  token.Card = card.ToString();
            //  Stripe.TokenService serviceToken = new Stripe.TokenService();
            //  Stripe.Token newToken = serviceToken.Create(token);


            return View();
        }

       public ActionResult UpdatePaymentStatus()
        {
            try
            {
                StripeConfiguration.ApiKey = "sk_test_51HrzKFJWAkWQReajWOSIktUeDnTgiJGp9ITWrFBwR0AJilWV8mumjJc1KhsDHpyAJzRyiU9RypxoERNl9ZTsIFsm00Nf69z591";
                Stream req = Request.InputStream;
                req.Seek(0 , System.IO.SeekOrigin.Begin );
                string json = new StreamReader(req).ReadToEnd();
                log.Info("Stripe live callback :" + json);

                var stripeEvent = EventUtility.ParseEvent(json);
                string stripejson = stripeEvent.Data.RawObject + string.Empty;
                var childData = Charge.FromJson(stripejson);
                var metadata = childData.Metadata;


                int orderID = -1;
                string strOrderID = string.Empty;
                if(metadata.TryGetValue("order_id", out strOrderID))
                {
                    int.TryParse(strOrderID, out orderID);


                }

                switch (stripeEvent.Type)
                {
                    case Events.ChargeCaptured:
                    case Events.ChargeFailed:
                    case Events.ChargePending:
                    case Events.ChargeRefunded:
                    case Events.ChargeSucceeded:
                    case Events.ChargeUpdated:
                        var charge = Charge.FromJson(stripejson);
                        string amountBuyer = ((double)childData.Amount / 100.0).ToString();
                        if(childData.BalanceTransactionId!=null)
                        {
                            long transactionAmount = 0;
                            long transactionFee = 0;
                            if(childData.BalanceTransactionId!=null)
                            {
                                var balanceService = new BalanceTransactionService();
                                BalanceTransaction transaction = balanceService.Get(childData.BalanceTransactionId);
                                transactionAmount = transaction.Amount;
                                transactionFee = transaction.Fee;


                            }

                            int status = 0;
                            double transactionRefund = 0;
                            if (stripeEvent.Type == Events.ChargeFailed)
                                status = -1;
                            if (stripeEvent.Type == Events.ChargePending)
                                status = -2;
                            if (stripeEvent.Type == Events.ChargeRefunded)
                            {
                                status = -3;
                                transactionRefund = ((double)childData.AmountRefunded / 100.0);

                            }

                            if (stripeEvent.Type == Events.ChargeSucceeded)
                                status = 1;

                        }

                        break;
                    default:

                        break;

                }


                return Json(new
                {
                    Code = -1,
                    Message = "Update Failed."

                });
            }
            catch (Exception ex)
            {

                log.Error("UpdatePaymentStatus: " + ex.Message);
                return Json(new
                {
                    Code = -100,
                    Message = "Error"

                });

            }
        }







    }
}