using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OctsProjectMvcTest.Models;

namespace OctsProjectMvcTest.Controllers
{ 
    public class StudentsController : Controller
    {
        private RootProjDbContext db = new RootProjDbContext();

        //
        // GET: /Students/

        public ViewResult Index()
        {
            var studentdbset = db.StudentDbSet.Include(s => s.Department);
            return View(studentdbset.ToList());
        }

        //
        // GET: /Students/Details/5

        public ViewResult Details(int id)
        {
            Student student = db.StudentDbSet.Find(id);
            return View(student);
        }

        //
        // GET: /Students/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            return View();
        } 

        //
        // POST: /Students/Create

        [HttpPost]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                int id = db.StudentDbSet.Count(s => (s.DepartmentID == student.DepartmentID)
                    && (s.AdmissionDate.Year == student.AdmissionDate.Year)) + 1;
                Department aDepartment
                    = db.DepartmentDbSet.Where(d => d.DepartmentID == student.DepartmentID).FirstOrDefault();
                student.RegNo = aDepartment.DeptCode + student.AdmissionDate.Year.ToString();
                if (id < 10)
                    student.RegNo += "00" + id.ToString();
                else if (id < 100)
                    student.RegNo += "0" + id.ToString();
                else
                    student.RegNo += id.ToString();
                db.StudentDbSet.Add(student);
                if (db.SaveChanges() > 0)
                    ViewBag.Message = "Success !!! Student :- " + student.StudentName
                        + " has been saved successfully with Registration No : " + student.RegNo;
                ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", student.DepartmentID);
                return View(student);  
            }

            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", student.DepartmentID);
            return View(student);
        }
        
        //
        // GET: /Students/Edit/5
 
        public ActionResult Edit(int id)
        {
            Student student = db.StudentDbSet.Find(id);
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", student.DepartmentID);
            return View(student);
        }

        //
        // POST: /Students/Edit/5

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", student.DepartmentID);
            return View(student);
        }

        //
        // GET: /Students/Delete/5
 
        public ActionResult Delete(int id)
        {
            Student student = db.StudentDbSet.Find(id);
            return View(student);
        }

        //
        // POST: /Students/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Student student = db.StudentDbSet.Find(id);
            db.StudentDbSet.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}