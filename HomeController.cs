using System;
using HealthCare.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthCare.Controllers
{
    public class HomeController : Controller
    {
        HealthCareDBContext objDataContext = new HealthCareDBContext();
        public ActionResult Index()//Navigation page to Registration Page
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(Admin admin)
        {

            string role = Request.Form["role"];
            string Id = Request.Form["Id"];
            string password = Request.Form["Password"];

            if (role == "")
            {
                ViewBag.Message = String.Format("Please Select Role");
            }
            else if (Id == "")
            {
                ViewBag.Message = String.Format("ID Should Not be Empty");
            }
            else if (password == "")
            {
                ViewBag.Message = String.Format("password Should Not be Empty");
            }
            else
            if (role == "Admin")
            {
                var obj = objDataContext.Admins.Where(a => a.Admin_User_Name.Equals(Id) && a.Admin_Password.Equals(password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["AdminID"] = obj.PK_Admin_Id;
                    return this.RedirectToAction("Index", "Admins");
                }
                else
                {
                    ViewBag.Message = String.Format("Invalid User ID (or) Incorrect Password");
                }

            }
            else if (role == "User")
            {
                var obj = objDataContext.Users.Where(a => a.User_ID.Equals(Id) && a.User_Password.Equals(password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["PkPatID"] = obj.PK_User_Id;
                    Session["PatID"] = obj.User_ID;
                    Session["Patphone"] = obj.User_Contact_No;
                    return this.RedirectToAction("Index", "Users");
                }
                else
                {
                    ViewBag.Message = String.Format("Invalid User ID (or) Incorrect Password");
                }

            }
            
            else if (role == "Service Provider")
            {
                //var obj = objDataContext.Clerks.Where(a => a.Clerk_Id == Id && a.Clerk_Password == password && a.Clerk_Isactive == true && a.Clerk_Deny == false).FirstOrDefault();
                 var obj = objDataContext.Service_Providers.Where(a => a.Service_Provider_ID.Equals(Id) && a.Service_Provider_Password.Equals(password) && a.Service_Provider_Isactive.Equals(true) && a.Service_Provider_Deny.Equals(false)).FirstOrDefault();
                if (obj != null)
                {
                    Session["DocID"] = obj.Service_Provider_ID;
                    Session["PkDocID"] = obj.PK_Service_Provider_Id;
                    Session["Docphone"] = obj.Service_Provider_Contact_No;
                    return this.RedirectToAction("Index", "Service_Providers");
                }
                else
                {
                    obj= objDataContext.Service_Providers.Where(a => a.Service_Provider_ID.Equals(Id) && a.Service_Provider_Password.Equals(password) && a.Service_Provider_Isactive.Equals(false) && a.Service_Provider_Deny.Equals(true)).FirstOrDefault();
                    
                    if (obj!=null)
                    {
                        ViewBag.Message = String.Format("Your Registration is Rejected");
                        return View();
                    }
                    else
                    {
                        obj = objDataContext.Service_Providers.Where(a => a.Service_Provider_ID.Equals(Id) && a.Service_Provider_Password.Equals(password) && a.Service_Provider_Isactive.Equals(false) && a.Service_Provider_Deny.Equals(false)).FirstOrDefault();
                        if (obj!= null)
                        {
                            ViewBag.Message = String.Format("Your Registration is Waiting for Admin Approval");
                            return View();
                        }
                        else
                        {
                            ViewBag.Message = String.Format("Invalid User ID (or) Incorrect Password");
                        }
                    }
                    
                }
            }


            return View();
        }
        #region others


        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        #endregion
    }
}