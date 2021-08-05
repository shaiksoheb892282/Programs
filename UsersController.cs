using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HealthCare.Models;
using PagedList;

namespace HealthCare.Controllers
{
    public class UsersController : Controller
    {
        private HealthCareDBContext db = new HealthCareDBContext();
        #region User page
        // GET: Users
        public ActionResult Index()
        {
            if (Session["PkPatID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                return View(db.Users.ToList());
            }
        }

        public ActionResult Search(string option, string search, int? pageNumber)
        {
            if (Session["PkPatID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                //ViewBag.SortByName = string.IsNullOrEmpty(sort) ? "descending name" : "";
                ////if the sort value is gender then we are initializing the value as descending gender  
                //ViewBag.SortByGender = sort == "Gender" ? "descending gender" : "Gender";
                //if (search == null)
                //{
                //    return View();
                //}

                //else
                //{
                if (option == "Hospital type")
                {
                    //Index action method will return a view with a student records based on what a user specify the value in textbox  
                    //return View(db.Students.Where(x = > x.Subjects == search || search == null).ToList());


                    if (String.IsNullOrEmpty(search))
                    {
                        var data = (from sp in db.Service_Providers
                                    join s in db.Services on sp.PK_Service_Provider_Id equals s.FK_Service_Provider_Service_Id
                                    select new
                                    {
                                        PK_Service_Provider_Id = sp.PK_Service_Provider_Id,
                                        Service_Provider_First_Name = sp.Service_Provider_First_Name,
                                        Service_Provider_Last_Name = sp.Service_Provider_Last_Name,
                                        Service_Provider_Email = sp.Service_Provider_Email,
                                        Service_Provider_ID = sp.Service_Provider_ID,
                                        Service_Provider_Contact_No = sp.Service_Provider_Contact_No,
                                        Service_Provider_Address = sp.Service_Provider_Address,
                                        Service_Provider_Map_Location = sp.Service_Provider_Map_Location
                                    }).Distinct().ToList()
                                  .Select(x => new Service_Provider()
                                  {
                                      PK_Service_Provider_Id = x.PK_Service_Provider_Id,
                                      Service_Provider_First_Name = x.Service_Provider_First_Name,
                                      Service_Provider_Last_Name = x.Service_Provider_Last_Name,
                                      Service_Provider_Email = x.Service_Provider_Email,
                                      Service_Provider_ID = x.Service_Provider_ID,
                                      Service_Provider_Contact_No = x.Service_Provider_Contact_No,
                                      Service_Provider_Address = x.Service_Provider_Address,
                                      Service_Provider_Map_Location = x.Service_Provider_Map_Location
                                  });
                        return View(data.ToList().ToPagedList(pageNumber ?? 1, 3));
                    }
                    else
                    {
                        var data = (from sc in db.Service_Categories
                                    join s in db.Services on sc.PK_Service_Category_Id equals s.FK_Service_Category_Service_Id
                                    join sp in db.Service_Providers on s.FK_Service_Provider_Service_Id equals sp.PK_Service_Provider_Id
                                    where sc.Service_Category_Service_Name.StartsWith(search) || search == null
                                    select new
                                    {
                                        PK_Service_Provider_Id = sp.PK_Service_Provider_Id,
                                        Service_Provider_First_Name = sp.Service_Provider_First_Name,
                                        Service_Provider_Last_Name = sp.Service_Provider_Last_Name,
                                        Service_Provider_Email = sp.Service_Provider_Email,
                                        Service_Provider_ID = sp.Service_Provider_ID,
                                        Service_Provider_Contact_No = sp.Service_Provider_Contact_No,
                                        Service_Provider_Address = sp.Service_Provider_Address,
                                        Service_Provider_Map_Location = sp.Service_Provider_Map_Location,
                                        Service_Category_Service_Name = sc.Service_Category_Service_Name
                                    }).ToList()
                                    .Select(x => new Service_Provider()
                                    {
                                        PK_Service_Provider_Id = x.PK_Service_Provider_Id,
                                        Service_Provider_First_Name = x.Service_Provider_First_Name,
                                        Service_Provider_Last_Name = x.Service_Provider_Last_Name,
                                        Service_Provider_Email = x.Service_Provider_Email,
                                        Service_Provider_ID = x.Service_Provider_ID,
                                        Service_Provider_Contact_No = x.Service_Provider_Contact_No,
                                        Service_Provider_Address = x.Service_Provider_Address,
                                        Service_Provider_Map_Location = x.Service_Provider_Map_Location

                                    }).Distinct();
                        return View(data.ToList().ToPagedList(pageNumber ?? 1, 3));
                    }
                }

                else if (option == "Location")
                {
                    return View(db.Service_Providers.Where(x => x.Service_Provider_Map_Location.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 3));
                }
                else if (option == "Auxilary Services")
                {
                    //return View(db.Service_Providers.Where(x => x.Services.ForEach(x=>x.Service_Category.Service_Category_Service_Name.StartsWith(search) || search == null) ).ToList());
                    //return View(db.Service_Providers.ToList());
                    //if (search == null)
                    //{
                    //    return View();
                    //}
                    if (String.IsNullOrEmpty(search))
                    {
                        var data = (from sp in db.Service_Providers
                                    join s in db.Services on sp.PK_Service_Provider_Id equals s.FK_Service_Provider_Service_Id
                                    select new
                                    {
                                        PK_Service_Provider_Id = sp.PK_Service_Provider_Id,
                                        Service_Provider_First_Name = sp.Service_Provider_First_Name,
                                        Service_Provider_Last_Name = sp.Service_Provider_Last_Name,
                                        Service_Provider_Email = sp.Service_Provider_Email,
                                        Service_Provider_ID = sp.Service_Provider_ID,
                                        Service_Provider_Contact_No = sp.Service_Provider_Contact_No,
                                        Service_Provider_Address = sp.Service_Provider_Address,
                                        Service_Provider_Map_Location = sp.Service_Provider_Map_Location
                                    }).Distinct().ToList()
                                  .Select(x => new Service_Provider()
                                  {
                                      PK_Service_Provider_Id = x.PK_Service_Provider_Id,
                                      Service_Provider_First_Name = x.Service_Provider_First_Name,
                                      Service_Provider_Last_Name = x.Service_Provider_Last_Name,
                                      Service_Provider_Email = x.Service_Provider_Email,
                                      Service_Provider_ID = x.Service_Provider_ID,
                                      Service_Provider_Contact_No = x.Service_Provider_Contact_No,
                                      Service_Provider_Address = x.Service_Provider_Address,
                                      Service_Provider_Map_Location = x.Service_Provider_Map_Location
                                  });
                        return View(data.ToList().ToPagedList(pageNumber ?? 1, 3));
                    }
                    else
                    {
                        var data = (from sc in db.Service_Categories
                                    join s in db.Services on sc.PK_Service_Category_Id equals s.FK_Service_Category_Service_Id
                                    join sp in db.Service_Providers on s.FK_Service_Provider_Service_Id equals sp.PK_Service_Provider_Id
                                    where sc.Service_Category_Service_Type.StartsWith(search) || search == null
                                    select new
                                    {
                                        PK_Service_Provider_Id = sp.PK_Service_Provider_Id,
                                        Service_Provider_First_Name = sp.Service_Provider_First_Name,
                                        Service_Provider_Last_Name = sp.Service_Provider_Last_Name,
                                        Service_Provider_Email = sp.Service_Provider_Email,
                                        Service_Provider_ID = sp.Service_Provider_ID,
                                        Service_Provider_Contact_No = sp.Service_Provider_Contact_No,
                                        Service_Provider_Address = sp.Service_Provider_Address,
                                        Service_Provider_Map_Location = sp.Service_Provider_Map_Location,
                                        Service_Category_Service_Name = sc.Service_Category_Service_Name
                                    }).ToList()
                                .Select(x => new Service_Provider()
                                {
                                    PK_Service_Provider_Id = x.PK_Service_Provider_Id,
                                    Service_Provider_First_Name = x.Service_Provider_First_Name,
                                    Service_Provider_Last_Name = x.Service_Provider_Last_Name,
                                    Service_Provider_Email = x.Service_Provider_Email,
                                    Service_Provider_ID = x.Service_Provider_ID,
                                    Service_Provider_Contact_No = x.Service_Provider_Contact_No,
                                    Service_Provider_Address = x.Service_Provider_Address,
                                    Service_Provider_Map_Location = x.Service_Provider_Map_Location

                                });


                        return View(data.ToList().ToPagedList(pageNumber ?? 1, 3));
                    }
                }

                else
                {
                    //return View(db.Students.Where(x = > x.Name.StartsWith(search) || search == null).ToList());
                    return View();
                }
                    
                //}

            }
        }
        public ActionResult userSearch()
        {
            if (Session["PkPatID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                var data = from p in db.Service_Categories
                           select new
                           {
                               ServiceId = p.Service_Category_Id,
                               ServiceName = p.Service_Category_Service_Type
                           };
                SelectList list = new SelectList(data, "ServiceId", "ServiceName");
                ViewBag.Roles = list;

                return View();
            }
        }
        [HttpPost]
        public ActionResult userSearch(string Category,string Ambulance_Availability,string Hospital_Type,string search,int? pageNumber)
        {
            /*https://localhost:44311/Users/userSearch?Category=Location&Ambulance_Availability=True&Hospital_Type=&search=
            Hospital_Type*/
            if (Session["PkPatID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                bool avail;
                if (Ambulance_Availability == "False")
                {
                    avail = false;
                }
                else
                {
                    avail = true;
                }
                //bool avail = Ambulance_Availability == "False" ? true: false;
                var data = from p in db.Service_Categories
                           select new
                           {
                               ServiceId = p.Service_Category_Id,
                               ServiceName = p.Service_Category_Service_Type
                           };
                SelectList list = new SelectList(data, "ServiceId", "ServiceName");
                ViewBag.Roles = list;
                
                if (Category== "Hospital Type")
                {
                    if (Hospital_Type=="")
                    {
                        return View("UserSearchResult", db.Service_Providers.ToList());
                    }
                    else
                    {
                        var result = (from sp in db.Service_Providers
                                      join s in db.Services on sp.PK_Service_Provider_Id equals s.FK_Service_Provider_Service_Id
                                      join sc in db.Service_Categories on s.FK_Service_Category_Service_Id equals sc.PK_Service_Category_Id
                                      where sc.Service_Category_Id == Hospital_Type
                                      select new
                                      {
                                          PK_Service_Provider_Id = sp.PK_Service_Provider_Id,
                                          Service_Provider_First_Name = sp.Service_Provider_First_Name,
                                          Service_Provider_Last_Name = sp.Service_Provider_Last_Name,
                                          Service_Provider_Email = sp.Service_Provider_Email,
                                          Service_Provider_ID = sp.Service_Provider_ID,
                                          Service_Provider_Contact_No = sp.Service_Provider_Contact_No,
                                          Service_Provider_Address = sp.Service_Provider_Address,
                                          Service_Provider_Map_Location = sp.Service_Provider_Map_Location,
                                          Service_Provider_Ambulance = sp.Service_Provider_Ambulance
                                      }).ToList()
                                  .Select(x => new Service_Provider()
                                  {
                                      PK_Service_Provider_Id = x.PK_Service_Provider_Id,
                                      Service_Provider_First_Name = x.Service_Provider_First_Name,
                                      Service_Provider_Last_Name = x.Service_Provider_Last_Name,
                                      Service_Provider_Email = x.Service_Provider_Email,
                                      Service_Provider_ID = x.Service_Provider_ID,
                                      Service_Provider_Contact_No = x.Service_Provider_Contact_No,
                                      Service_Provider_Address = x.Service_Provider_Address,
                                      Service_Provider_Map_Location = x.Service_Provider_Map_Location,
                                      Service_Provider_Ambulance = x.Service_Provider_Ambulance
                                  });
                        //return View("UserSearchResult",result.ToList().ToPagedList(pageNumber ?? 1, 3));
                        return View("UserSearchResult", result.ToList());
                    }
                }
                else if (Category=="Location")
                {
                    if (Ambulance_Availability==null && String.IsNullOrEmpty(search))
                    {
                        return View("UserSearchResult", db.Service_Providers.ToList());
                    }
                    else if (String.IsNullOrEmpty(search))
                    {
                        var result = db.Service_Providers.Where(x => x.Service_Provider_Ambulance == avail).ToList();
                        //return View("UserSearchResult",result.ToPagedList(pageNumber ?? 1, 3));
                        return View("UserSearchResult", result.ToList());
                    }
                    else if (Ambulance_Availability == null)
                    {
                        var result = db.Service_Providers.Where(x => x.Service_Provider_Map_Location.StartsWith(search)).ToList();
                        //return View("UserSearchResult",result.ToPagedList(pageNumber ?? 1, 3));
                        return View("UserSearchResult", result.ToList());
                    }
                    else
                    {
                        var result = db.Service_Providers.Where(x => x.Service_Provider_Ambulance == avail).ToList();
                        result = result.Where(x => x.Service_Provider_Map_Location.StartsWith(search)).ToList();//x.Service_Provider_Ambulance == avail // x => x.Service_Provider_Map_Location.StartsWith(search)
                        //return View("UserSearchResult", result.ToPagedList(pageNumber ?? 1, 3));
                        return View("UserSearchResult", result.ToList());
                    }
                }
                return View("UserSearchResult");
            }

        }
        public ActionResult UserSearchResult()
        {
            if (Session["PkPatID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                var a = (from d in db.Service_Providers select d).ToList();
                /*
                //if the sort parameter is null or empty then we are initializing the value as descending name  
                ViewBag.SortByName = string.IsNullOrEmpty(sort) ? "descending name" : "";
                ViewBag.SortByLocaion = sort== "Service_Provider_Map_Location" ? "descending location" : "ascending location";
                //if the sort value is gender then we are initializing the value as descending gender  
                //ViewBag.SortByGender = sort == "Gender" ? "descending gender" : "Gender";
                var a = (from d in db.Service_Providers select d).ToList();
                var records = a.AsQueryable();
                switch (sort)
                {

                    case "descending location":
                        records = records.OrderByDescending(x => x.Service_Provider_Map_Location);
                        break;

                    case "ascending location":
                        records = records.OrderBy(x => x.Service_Provider_Map_Location);
                        break;

                    case "descending name":
                        records = records.OrderByDescending(x => x.Service_Provider_First_Name);
                        break;

                    default:
                        records = records.OrderBy(x => x.Service_Provider_First_Name);
                        break;

                }
                //return View(records.ToPagedList(pageNumber ?? 1, 3));*/
                return View(a);
            }
        }
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["PkPatID"] == null)
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

        #endregion
        #region Others



        // GET: Users/Create
        //User Registration
        public ActionResult Create()
        {
            int count= (from row in db.Users select row).Count();
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

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "PK_User_Id,User_First_Name,User_Last_Name,User_Email,User_DOB,User_Gender,User_Contact_No,User_ID,User_Password,User_Address")] User user)
        public ActionResult Create(User user)
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
                //return RedirectToAction("Login", "Home");
            }
            ViewBag.Message = String.Format("Your Details Are Submited Successfully");
            //return View(user);
            return View();
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["PkPatID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_User_Id,User_First_Name,User_Last_Name,User_Email,User_DOB,User_Gender,User_Contact_No,User_ID,User_Password,User_Address")] User user)
        {
            if (Session["PkPatID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(user);
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["PkPatID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["PkPatID"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                User user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        #endregion
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
