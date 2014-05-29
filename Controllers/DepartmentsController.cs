using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using OctsProjectMvcTest.Models;

namespace OctsProjectMvcTest.Controllers
{ 
    public class DepartmentsController : Controller
    {
        private RootProjDbContext db = new RootProjDbContext();

        //
        // GET: /Departments/

        public ViewResult Index()
        {
            return View(db.DepartmentDbSet.ToList());
        }

        //
        // GET: /Departments/Details/5

        public ViewResult Details(int id)
        {
            Department department = db.DepartmentDbSet.Find(id);
            return View(department);
        }

        //
        // GET: /Departments/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Departments/Create

        [HttpPost]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                db.DepartmentDbSet.Add(department);
                if (db.SaveChanges() > 0)
                    ViewBag.Message = "Department :- " + department.DeptCode
                        + " has been saved successfully .";
                return View(department);  
            }

            return View(department);
        }
        
        //
        // GET: /Departments/Edit/5
 
        public ActionResult Edit(int id)
        {
            Department department = db.DepartmentDbSet.Find(id);
            return View(department);
        }

        //
        // POST: /Departments/Edit/5

        [HttpPost]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        //
        // GET: /Departments/Delete/5
 
        public ActionResult Delete(int id)
        {
            Department department = db.DepartmentDbSet.Find(id);
            return View(department);
        }

        //
        // POST: /Departments/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.DepartmentDbSet.Find(id);
            List<Teacher> TeacherList
                = db.TeacherDbSet.Where(t => t.DepartmentID == id).ToList();
            foreach (var aTeacher in TeacherList)
            {
                db.TeacherDbSet.Remove(aTeacher);
            }
            List<Student> StudentList
                = db.StudentDbSet.Where(s => s.DepartmentID == id).ToList();
            foreach (var student in StudentList)
            {
                db.StudentDbSet.Remove(student);
            }

            db.SaveChanges();
            db.DepartmentDbSet.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult Check_DeptCode(string deptCode)
        {
            var result = db.DepartmentDbSet.Count(d => d.DeptCode == deptCode) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Check_DeptName(string deptName)
        {
            var result = db.DepartmentDbSet.Count(d => d.DeptName == deptName) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}