using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hombay.models;
using Hombay.Models;

namespace Hombay.Controllers
{
    public class SignUpController : Controller
    {

      
        
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(tbl_users tbl_Users)
        {
            if (tbl_Users.ImageFile != null)
            {



                string filename = Path.GetFileNameWithoutExtension(tbl_Users.ImageFile.FileName);
                string extension = Path.GetExtension(tbl_Users.ImageFile.FileName);
                filename = filename + DateTime.Now.ToString("yyyymmssff") + extension;
                tbl_Users.Image = "~/Images/" + filename;
                filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                tbl_Users.ImageFile.SaveAs(filename);
            }

             HomeBeyDBEntities dBEntities = new HomeBeyDBEntities();
            dBEntities.tbl_users.Add(tbl_Users);
            dBEntities.SaveChanges();
            TempData["Success"] = "Success! Your query is been executed.";
            return View();
        }

        [HttpGet]
        public ActionResult AddorEdit(int id = 0)
        {
            SignUp usermodel = new SignUp();
            return View(usermodel);
            /*
            [HttpPost]
            public ActionResult AddorEdit(SignUp SignUpModel)
            {
                using (DbModels dbmodel = new DbModels())
                {
                    dbmodel.SignUp.Add(SignUpModel);
                    dbmodel.savechanges();
                }
                ModelState.Clear();

                return View("AddorEdit", new SignUp());
            }*/
        }
    }
}