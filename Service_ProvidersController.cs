using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using HealthCare.Models;

namespace HealthCare.Controllers
{
    public class Service_ProvidersController : Controller
    {
        private HealthCareDBContext db = new HealthCareDBContext();

        #region Service Provider Home page
        /// <summary>
        /// Service Provider Home page
        /// </summary>
        /// <returns></returns>
        // GET: Service_Providers
        public ActionResult Index()
        {
            if(Session["PkDocID"]==null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                return View(db.Service_Providers.ToList());
            }
        }
        #endregion

        #region Service Page
        public ActionResult AddService()//Add Service Page
        {
            if (Session["PkDocID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                var data = from p in db.Service_Categories
                           select new
                           {
                               ServiceId=p.Service_Category_Id,
                               ServiceName = p.Service_Category_Service_Name
                           };
                SelectList list = new SelectList(data, "ServiceId", "ServiceName");
                ViewBag.Roles = list;
                int count = (from row in db.Services select row).Count();


                if (count > 0)
                {
                    TempData["docid"] = "S_" + (count + 1);
                }
                else
                {
                    TempData["docid"] = "S_1";
                }

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddService(Service service)
        {
            if (Session["PkDocID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {

                if (ModelState.IsValid)
                {
                    var data = from p in db.Service_Categories
                               select new
                               {
                                   ServiceId = p.Service_Category_Id,
                                   ServiceName = p.Service_Category_Service_Name
                               };
                    SelectList list = new SelectList(data, "ServiceId", "ServiceName");
                    ViewBag.Roles = list;
                    int i = Convert.ToInt32(Session["PkDocID"].ToString());
                    //string name = service.Service_Name;
                    service.Service_Category = db.Service_Categories.Where(x => x.Service_Category_Id == service.Service_Name).FirstOrDefault();
                    //service.FK_Service_Category_Service_Id = i;
                    service.Service_Provider = db.Service_Providers.Where(x => x.PK_Service_Provider_Id.Equals(i)).FirstOrDefault();
                    db.Services.Add(service);
                    //db.Entry(service).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.Message = String.Format("Your Details Are Submited Successfully");
                return View();
            }

        }

        public ActionResult ServiceDetails()
        {
            if (Session["PkDocID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                var services = (from p in db.Services
                                join f in db.Service_Categories on p.FK_Service_Category_Service_Id equals f.PK_Service_Category_Id
                                join g in db.Service_Providers on p.FK_Service_Provider_Service_Id equals g.PK_Service_Provider_Id
                                select new 
                                {
                                    PK_Service_Id= p.PK_Service_Id,
                                    Service_ID=p.Service_ID,
                                    Service_Name=f.Service_Category_Service_Name,
                                    FK_Service_Provider_Service_Id=p.FK_Service_Provider_Service_Id,
                                    FK_Service_Category_Service_Id=p.FK_Service_Category_Service_Id
                                }).ToList()
                                .Select(x=>new Service()
                                {
                                    PK_Service_Id = x.PK_Service_Id,
                                    Service_ID = x.Service_ID,
                                    Service_Name = x.Service_Name,
                                    FK_Service_Provider_Service_Id = x.FK_Service_Provider_Service_Id,
                                    FK_Service_Category_Service_Id = x.FK_Service_Category_Service_Id
                                });

                //return View(db.Services.ToList());
                return View(services.ToList());
            }
        }

        #endregion

        //GET: Service_Providers/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["PkDocID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Service service = db.Services.Find(id);
                if (service == null)
                {
                    return HttpNotFound();
                }

                var data = from p in db.Service_Categories
                           select new
                           {
                               ServiceId = p.Service_Category_Id,
                               ServiceName = p.Service_Category_Service_Name
                           };
                SelectList list = new SelectList(data, "ServiceId", "ServiceName");
                ViewBag.Roles = list;
                Service_Category service_Category = db.Service_Categories.Find(service.FK_Service_Category_Service_Id);
                service.Service_Name = service_Category.Service_Category_Service_Name;
                return View(service);
            }
        }
        #region Registration Page for Service Provider
        // GET: Service_Providers/Create
        public ActionResult Create()//Registration Page for Service Provider
        {
            int count = (from row in db.Service_Providers select row).Count();
            if (count > 0)
            {
                TempData["docid"] = "SP_" + (count + 1);
            }
            else
            {
                TempData["docid"] = "SP_1";
            }
            //ViewBag.Success =TempData["Success"];
            return View();
        }

        // POST: Service_Providers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "PK_Service_Provider_Id,Service_Provider_First_Name,Service_Provider_Last_Name,Service_Provider_Email,Service_Provider_DOB,Service_Provider_Contact_No,Service_Provider_ID,Service_Provider_Password,Service_Provider_Address,Service_Provider_Map_Location,Service_Provider_Isactive,Service_Provider_Deny")] Service_Provider service_Provider)
        public ActionResult Create(Service_Provider service_Provider)
        {
            string f_name = Request.Form["Service_Provider_First_Name"];
            string l_name = Request.Form["Service_Provider_Last_Name"];
            string email = Request.Form["Service_Provider_Email"];
            //DateTime dob = Convert.ToDateTime(Request.Form["Service_Provider_DOB"]);
            string c_no = Request.Form["Service_Provider_Contact_No"];
            string d_id = Request.Form["Service_Provider_ID"];
            string pwd = Request.Form["Service_Provider_Password"];
            string adrs = Request.Form["Service_Provider_Address"];
            string d_quali = Request.Form["Service_Provider_Map_Location"];
            string amb = Request.Form["Service_Provider_Ambulance"];
            bool a_avail = amb=="false"?false:true;
            
                //if (ModelState.IsValid)
                //{
                //try
                //{

                    
                //    TempData["Success"] = true;
                //    return RedirectToAction("Create");
                //    //TempData["UserMessage"] = new AlertMessage() { CssClassName = "alert-sucess", Title = "Success!", Message = "Operation Done." };
                //    //return RedirectToAction("Success");
                //}
                //catch (Exception)
                //{

                //    //TempData["UserMessage"] = new AlertMessage() { CssClassName = "alert-error", Title = "Error!", Message = "Operation Failed." };
                //    //return RedirectToAction("Error");
                //    //throw;
                //}
                if (ModelState.IsValid)
                {
                    Service_Provider ob = new Service_Provider();
                    ob.Service_Provider_First_Name = f_name;
                    ob.Service_Provider_Last_Name = l_name;
                    //ob.Service_Provider_DOB = dob;
                    ob.Service_Provider_Email = email;
                    ob.Service_Provider_Contact_No = c_no;
                    ob.Service_Provider_ID = d_id;
                    ob.Service_Provider_Password = pwd;
                    ob.Service_Provider_Address = adrs;
                    ob.Service_Provider_Map_Location = d_quali;
                    ob.Service_Provider_Confirm_Password = pwd;
                ob.Service_Provider_Ambulance = a_avail;
                    db.Service_Providers.Add(ob);

                    db.SaveChanges();
                }
            ViewBag.Message = String.Format("Your Details Are Submited Successfully");
            //return RedirectToAction("Login", "Home");
            //return RedirectToAction("Index");
            return View();
        }

        #endregion

        // GET: Service_Providers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["PkDocID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }           
                Service service = db.Services.Find(id);
                if (service == null)
                {
                    return HttpNotFound();
                }
                var data = from p in db.Service_Categories
                           select new
                           {
                               ServiceId = p.Service_Category_Id,
                               ServiceName = p.Service_Category_Service_Name
                           };
                SelectList list = new SelectList(data, "ServiceId", "ServiceName");
                ViewBag.Roles = list;
                Service_Category service_Category = db.Service_Categories.Find(service.FK_Service_Category_Service_Id);
                service.Service_Name = service_Category.Service_Category_Service_Name;
                return View(service);
            }
        }

        // POST: Service_Providers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "PK_Service_Provider_Id,Service_Provider_First_Name,Service_Provider_Last_Name,Service_Provider_Email,Service_Provider_DOB,Service_Provider_Contact_No,Service_Provider_ID,Service_Provider_Password,Service_Provider_Address,Service_Provider_Map_Location,Service_Provider_Isactive,Service_Provider_Deny")] Service_Provider service_Provider)
        public ActionResult Edit(Service service)
        {
            if (Session["PkDocID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {

                if (ModelState.IsValid)
                {

                    //User emp = db.Users.Where(x => x.Patient_ID == patient.Patient_ID).FirstOrDefault();
                    Service emp = db.Services.Find(service.PK_Service_Id);
                    var data = from p in db.Service_Categories
                               select new
                               {
                                   ServiceId = p.Service_Category_Id,
                                   ServiceName = p.Service_Category_Service_Name
                               };
                    SelectList list = new SelectList(data, "ServiceId", "ServiceName");
                    ViewBag.Roles = list;
                    int i = Convert.ToInt32(Session["PkDocID"].ToString());
                    //string name = service.Service_Name;
                    Service_Category service_Category= db.Service_Categories.Where(x => x.Service_Category_Id == service.Service_Name).FirstOrDefault();
                    emp.Service_Category = service_Category;
                    //service.FK_Service_Category_Service_Id = i;
                    emp.Service_Provider = db.Service_Providers.Where(x => x.PK_Service_Provider_Id.Equals(i)).FirstOrDefault();
                    emp.Service_Name = service_Category.Service_Category_Service_Name;
                    emp.Service_ID = service.Service_ID;
                    //emp.Service_Name = service.Service_Name;
                    //db.Entry(service).State = EntityState.Modified;
                    //db.As.Attach(emp.a);

                    db.SaveChanges();
                    //return RedirectToAction("Index");
                }
                ViewBag.Message = String.Format("Service Details Are Changed Successfully");
                return View(service);
                
            }

                
        }

        // GET: Service_Providers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["PkDocID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Service_Provider service_Provider = db.Service_Providers.Find(id);
                Service service = db.Services.Find(id);
                if (service== null)
                {
                    return HttpNotFound();
                }
                var data = from p in db.Service_Categories
                           select new
                           {
                               ServiceId = p.Service_Category_Id,
                               ServiceName = p.Service_Category_Service_Name
                           };
                SelectList list = new SelectList(data, "ServiceId", "ServiceName");
                ViewBag.Roles = list;
                Service_Category service_Category = db.Service_Categories.Find(service.FK_Service_Category_Service_Id);
                service.Service_Name = service_Category.Service_Category_Service_Name;
                return View(service);
                //return View(service);
            }
                
        }

        // POST: Service_Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["PkDocID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                Service service = db.Services.Find(id);
                
                //Service emp = db.Services.Find(service.PK_Service_Id);
                var data = from p in db.Service_Categories
                           select new
                           {
                               ServiceId = p.Service_Category_Id,
                               ServiceName = p.Service_Category_Service_Name
                           };
                SelectList list = new SelectList(data, "ServiceId", "ServiceName");
                ViewBag.Roles = list;
                //int i = Convert.ToInt32(Session["PkDocID"].ToString());
                ////string name = service.Service_Name;
                //Service_Category service_Category = db.Service_Categories.Where(x => x.Service_Category_Id == service.Service_Name).FirstOrDefault();
                //emp.Service_Category = service_Category;
                ////service.FK_Service_Category_Service_Id = i;
                //emp.Service_Provider = db.Service_Providers.Where(x => x.PK_Service_Provider_Id.Equals(i)).FirstOrDefault();
                //emp.Service_Name = service_Category.Service_Category_Service_Name;
                //emp.Service_ID = service.Service_ID;
                db.Services.Remove(service);
                db.SaveChanges();
                ViewBag.Message = String.Format("Services are Deleted Successfully");
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
    }

    
}
