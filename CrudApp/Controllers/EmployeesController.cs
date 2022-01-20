using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using CrudApp.Models;

namespace CrudApp.Controllers
{
    public class EmployeesController : Controller
    {
        private ProjectDB db = new ProjectDB();

        // GET: Employees
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,firstname,lastname,title,division,building,room")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,firstname,lastname,title,division,building,room")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }




        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase xmlFile)
        {
            if (xmlFile.ContentType.Equals("application/xml") || xmlFile.ContentType.Equals("text/xml"))
            {
                var xmlPath = Server.MapPath("~/FileUpload" + xmlFile.FileName);
                xmlFile.SaveAs(xmlPath);
                //XDocument xDoc = XDocument.Load(xmlPath);

                XDocument xDoc = null;

                using (StreamReader oReader = new StreamReader(xmlPath, Encoding.GetEncoding("ISO-8859-1")))
                {
                    xDoc = XDocument.Load(oReader);
                }

              



                List<Employee> EmployeeList = xDoc.Descendants("employees").Select(employee => new Employee {
                        //id = Convert.ToInt32(employee.Element("id").Value),
                        //firstname = employee.Element("firstname").Value,
                        lastname = employee.Element("lastname").Value,
                        title = employee.Element("title").Value
                    }).ToList();

                // using (DBModel db = new DBModel())  
                // {  
                foreach (var i in EmployeeList)
                {
                    var v = db.Employees.Where(a => a.id.Equals(i.id)).FirstOrDefault();

                    if (v != null)
                    {
                        v.id = i.id;
                        v.firstname = i.firstname;
                        v.lastname = i.lastname;
                        v.title = i.title;
                        v.division = i.division;
                        v.building = i.building;
                        v.room = i.room;
                    }
                    else
                    {
                        db.Employees.Add(i);
                        // db.Products.Add(i);  
                    }
                    db.SaveChanges();

                }
                // }  
                ViewBag.Success = "File uploaded successfully..";
            }
            else
            {
                ViewBag.Error = "Invalid file(Upload xml file only)";
            }
            return View("Index");
        }


        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public ActionResult Upload(HttpPostedFileBase xmlFile)
        //{
        //    if (xmlFile.ContentType.Equals("application/xml") || xmlFile.ContentType.Equals("text/xml"))
        //    {
        //        var xmlPath = Server.MapPath("~/FileUpload" + xmlFile.FileName);
        //        xmlFile.SaveAs(xmlPath);
        //        //XDocument xDoc = XDocument.Load(xmlPath);

        //        XDocument xDoc = null;

        //        using (StreamReader oReader = new StreamReader(xmlPath, Encoding.GetEncoding("ISO-8859-1")))
        //        {
        //            xDoc = XDocument.Load(oReader);
        //        }




        //        List<Employee> EmployeeList = xDoc.Descendants("employees").Select
        //            (employee => new Employee
        //            {
        //                id = Convert.ToInt32(employee.Element("id").Value),
        //                firstname = employee.Element("firstname").Value,
        //                lastname = employee.Element("lastname").Value,
        //                title = employee.Element("title").Value,
        //                division = employee.Element("division").Value,
        //                building = employee.Element("building").Value,
        //                room = employee.Element("room").Value
        //            }).ToList();

        //        // using (DBModel db = new DBModel())  
        //        // {  
        //        foreach (var i in EmployeeList)
        //        {
        //            var v = db.Employees.Where(a => a.id.Equals(i.id)).FirstOrDefault();

        //            if (v != null)
        //            {
        //                v.id = i.id;
        //                v.firstname = i.firstname;
        //                v.lastname = i.lastname;
        //                v.title = i.title;
        //                v.division = i.division;
        //                v.building = i.building;
        //                v.room = i.room;
        //            }
        //            else
        //            {
        //                db.Employees.Add(i);
        //                // db.Products.Add(i);  
        //            }
        //            db.SaveChanges();

        //        }
        //        // }  
        //        ViewBag.Success = "File uploaded successfully..";
        //    }
        //    else
        //    {
        //        ViewBag.Error = "Invalid file(Upload xml file only)";
        //    }
        //    return View("Index");
        //}




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
