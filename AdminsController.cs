using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;

using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HealthCare.Models;

namespace HealthCare.Controllers
{
    public class AdminsController : Controller
    {
        private HealthCareDBContext db = new HealthCareDBContext();
        #region Admin
        // GET: Admins
        public ActionResult Index()
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            return View();
        }
        //Logout
        public ActionResult Logout()
        {

            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.Expires = -1500;
            Response.CacheControl = "no-cache";
            FormsAuthentication.SignOut();
            HttpContext.Session["AdminID"] = null;
            HttpContext.Session["PkDocID"] = null;
            HttpContext.Session["PkPatID"] = null;
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            HttpContext.Session.Clear();
            Session.RemoveAll();
            HttpContext.Session.Abandon();
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            return RedirectToAction("index", "Home");

        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult AddServiceCategory()
        {

            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                int count = (from row in db.Service_Categories select row).Count();

                if (count > 0)
                {
                    TempData["patid"] = "SC_" + (count + 1);
                }
                else
                {
                    TempData["patid"] = "SC_1";
                }

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddServiceCategory(Service_Category service_Category)
        {

            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                //string sci= Request.Form["Service_Category_Id"];
                //string scsn= Request.Form["Service_Category_Service_Name"];
                //string scst= Request.Form["Service_Category_Service_Type"];


                if (ModelState.IsValid)
                {
                    //Service_Category ob = new Service_Category();
                    //ob.Service_Category_Id = sci;
                    //ob.Service_Category_Service_Name = (name)Enum.Parse(typeof(name), scsn);
                    //ob.Service_Category_Service_Type = (type)Enum.Parse(typeof(type), scst);

                    db.Service_Categories.Add(service_Category);
                    db.SaveChanges();
                }
                ViewBag.Message = String.Format("Your Details Are Submited Successfully");
                return View();
            }
        }

                #endregion

                #region Service Provider

        public ActionResult AproveServiceProviders()
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                return View(db.Service_Providers.Where(a => a.Service_Provider_Isactive == false));
            }

        }

        public ActionResult AproveServiceProvidersDetails(int? id)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Service_Provider service_Provider = db.Service_Providers.Find(id);
                if (service_Provider == null)
                {
                    return HttpNotFound();
                }
                return View(service_Provider);
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AproveServiceProvidersDetails(int id)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                Service_Provider exp = db.Service_Providers.Where(x => x.PK_Service_Provider_Id == id).FirstOrDefault();



                if (exp == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    //string pwd = Request.Form["Service_Provider_Password"];
                    exp.Service_Provider_Confirm_Password = exp.Service_Provider_Password;
                    exp.Service_Provider_Isactive = true;
                    exp.Service_Provider_Deny = false;
                    db.Entry(exp).State = EntityState.Modified;
                    db.SaveChanges();

                    return this.RedirectToAction("AproveServiceProviders", "Admins");

                }
            }



        }

        public ActionResult DenyServiceProvidersDetails(int? id)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Service_Provider service_Provider = db.Service_Providers.Find(id);
                if (service_Provider == null)
                {
                    return HttpNotFound();
                }
                return View(service_Provider);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DenyServiceProvidersDetails(int id)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                Service_Provider exp = db.Service_Providers.Where(x => x.PK_Service_Provider_Id == id).FirstOrDefault();



                if (exp == null)
                {
                    return HttpNotFound();
                }
                else
                {

                    //string pwd = Request.Form["Service_Provider_Password"];
                    exp.Service_Provider_Confirm_Password = exp.Service_Provider_Password;
                    exp.Service_Provider_Deny = true;
                    exp.Service_Provider_Isactive = false;
                    db.Entry(exp).State = EntityState.Modified;
                    db.SaveChanges();

                    return this.RedirectToAction("AproveServiceProviders", "Admins");

                }
            }

        }

        #endregion


        #region User

        public ActionResult UserDetails()
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                return View(db.Users.ToList());
            }
        }

        public ActionResult AddUser()
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                int count = (from row in db.Users select row).Count();

                if (count > 0)
                {
                    TempData["patid"] = "U_" + (count + 1);
                }
                else
                {
                    TempData["patid"] = "U_1";
                }

                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(User user)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                

                string f_name = Request.Form["User_First_Name"];
                string l_name = Request.Form["User_Last_Name"];
                string email = Request.Form["User_Email"];
                DateTime dob = Convert.ToDateTime(Request.Form["User_DOB"]);
                string gender = Request.Form["User_Gender"];
                string c_no = Request.Form["User_Contact_No"];
                string p_id = Request.Form["User_ID"];
                string pwd = Request.Form["User_Password"];
                string adrs = Request.Form["User_Address"];

                if (ModelState.IsValid)
                {
                    User ob = new User();
                    ob.User_First_Name = f_name;
                    ob.User_Last_Name = l_name;
                    ob.User_Email = email;
                    ob.User_DOB = dob;
                    ob.User_Gender = gender;
                    ob.User_Contact_No = c_no;
                    ob.User_ID = p_id;
                    ob.User_Password = pwd;
                    ob.User_Address = adrs;
                    ob.User_Confirm_Password = pwd;

                    db.Users.Add(user);
                    db.SaveChanges();
                }
                ViewBag.Message = String.Format("Your Details Are Submited Successfully");
                return View();
            }
        }

        public ActionResult EditUser(int? id)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user= db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult EditPatient([Bind(Include = "PK_Patient_ID,Patient_First_Name,Patient_Last_Name,Patient_DOB,Patient_Gender,Patient_Contact_No,Patient_ID,Patient_Password,Patient_Address")] Patient patient)
        public ActionResult EditUser(User user)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {

                    //User emp = db.Users.Where(x => x.Patient_ID == patient.Patient_ID).FirstOrDefault();
                    User emp=db.Users.Where(x=>x.User_ID==user.User_ID).FirstOrDefault();

                    emp.User_First_Name = user.User_First_Name;
                    emp.User_Last_Name = user.User_Last_Name;
                    emp.User_DOB = user.User_DOB;
                    emp.User_Email = user.User_Email;
                    emp.User_Gender = user.User_Gender;
                    //emp.User_ID = user.User_ID;
                    emp.User_Password = user.User_Password;
                    emp.User_Address = user.User_Address;
                    emp.User_Confirm_Password = user.User_Password;
                    db.Entry(emp).State = EntityState.Modified;

                    db.SaveChanges();
                    //return RedirectToAction("Index");
                }
                ViewBag.Message = String.Format("User Details Are Changed Successfully");
                return View(user);
            }
        }
        #endregion


        #region Others
        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }

        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                return View();
            }
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_Admin_Id,Admin_User_Name,Admin_Password")] Admin admin)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Admins.Add(admin);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(admin);
            }
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }

        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_Admin_Id,Admin_User_Name,Admin_Password")] Admin admin)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(admin);
            }

        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                Admin admin = db.Admins.Find(id);
                db.Admins.Remove(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

    }
}
