using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC3UniversityMVC3App1.Models;
using OctsProjectMvcTest.Models;

namespace OctsProjectMvcTest.Controllers
{ 
    public class AllocatedRoomsController : Controller
    {
        private RootProjDbContext db = new RootProjDbContext();

        //
        // GET: /AllocatedRooms/

        public ViewResult Index()
        {
            var allocatedroomdbset = db.AllocatedRoomDbSet.Include(a => a.Course).Include(a => a.Room).Include(a => a.WeekDay);
            return View(allocatedroomdbset.ToList());
        }

        //
        // GET: /AllocatedRooms/Details/5

        public ViewResult Details(int id)
        {
            AllocatedRoom allocatedroom = db.AllocatedRoomDbSet.Find(id);
            return View(allocatedroom);
        }

        //
        // GET: /AllocatedRooms/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            ViewBag.CourseID = new SelectList("", "CourseID", "CourseCode");
            ViewBag.RoomID = new SelectList(db.RoomDbSet, "RoomID", "RoomNo");
            ViewBag.WeekDayID = new SelectList(db.WeekDayDbSet, "WeekDayID", "DayName");
            return View();
        } 

        //
        // POST: /AllocatedRooms/Create

        [HttpPost]

        public ActionResult Create(AllocatedRoom allocatedroom)
        {
            Room room = db.RoomDbSet.Find(allocatedroom.RoomID);
            Course course = db.CourseDbSet.Find(allocatedroom.CourseID);
            WeekDay day = db.WeekDayDbSet.Find(allocatedroom.WeekDayID); 

            if (ModelState.IsValid)
            {                  
                double startTime, endTime;

                try
                {
                    startTime = ConvertTimeIntoDouble(allocatedroom.StartTime);
                }
                catch (Exception anException)
                {
                    ViewBag.ErrorMessage1 = anException.Message;
                    ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
                    ViewBag.CourseID = new SelectList(db.CourseDbSet.Where(c => c.DepartmentID == course.DepartmentID), "CourseID", "CourseCode", allocatedroom.CourseID);
                    ViewBag.RoomID = new SelectList(db.RoomDbSet, "RoomID", "RoomNo", allocatedroom.RoomID);
                    ViewBag.WeekDayID = new SelectList(db.WeekDayDbSet, "WeekDayID", "DayName", allocatedroom.WeekDayID);
                    return View(allocatedroom);
                }

                try
                {
                    endTime = ConvertTimeIntoDouble(allocatedroom.EndTime);
                }
                catch (Exception anException)
                {
                    ViewBag.ErrorMessage2 = anException.Message;
                    ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
                    ViewBag.CourseID = new SelectList(db.CourseDbSet.Where(c => c.DepartmentID == course.DepartmentID), "CourseID", "CourseCode", allocatedroom.CourseID);
                    ViewBag.RoomID = new SelectList(db.RoomDbSet, "RoomID", "RoomNo", allocatedroom.RoomID);
                    ViewBag.WeekDayID = new SelectList(db.WeekDayDbSet, "WeekDayID", "DayName", allocatedroom.WeekDayID);
                    return View(allocatedroom);
                }

                if (endTime < startTime)
                {
                    ViewBag.Message = "Start Time must be before End Time (within 24 hours)";
                    ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
                    ViewBag.CourseID = new SelectList(db.CourseDbSet, "CourseID", "CourseCode", allocatedroom.CourseID);
                    ViewBag.RoomID = new SelectList(db.RoomDbSet, "RoomID", "RoomNo", allocatedroom.RoomID);
                    ViewBag.WeekDayID = new SelectList(db.WeekDayDbSet, "WeekDayID", "DayName", allocatedroom.WeekDayID);
                    return View(allocatedroom);
                }

                if ((startTime<0)||(endTime >= 24))
                {
                    ViewBag.Message = "Please input time within 24 hours .";
                    ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
                    ViewBag.CourseID = new SelectList(db.CourseDbSet.Where(c => c.DepartmentID == course.DepartmentID), "CourseID", "CourseCode", allocatedroom.CourseID);
                    ViewBag.RoomID = new SelectList(db.RoomDbSet, "RoomID", "RoomNo", allocatedroom.RoomID);
                    ViewBag.WeekDayID = new SelectList(db.WeekDayDbSet, "WeekDayID", "DayName", allocatedroom.WeekDayID);
                    return View(allocatedroom);
                }

                List<AllocatedRoom> overlappedRoomList = new List<AllocatedRoom>();

                foreach (var anAllocatedRoom in 
                    db.AllocatedRoomDbSet.Include(a => a.Course).Include(a => a.Room).Include(a => a.WeekDay).Where(a=>(a.RoomID==allocatedroom.RoomID)&&(a.WeekDayID==allocatedroom.WeekDayID))
                    )
                {
                    double allocatedStartTime = ConvertTimeIntoDouble(anAllocatedRoom.StartTime);
                    double allocatedEndTime = ConvertTimeIntoDouble(anAllocatedRoom.EndTime);
                    if (
                            ((allocatedStartTime <= startTime) && (startTime < allocatedEndTime))
                            || ((allocatedStartTime < endTime) && (endTime <= allocatedEndTime))
                            || ((startTime <= allocatedStartTime) && (allocatedEndTime <= endTime))
                        )
                    {
                        overlappedRoomList.Add(anAllocatedRoom); 
                    }
                }

                if (overlappedRoomList.Count() > 0)
                {
                    string message = "Room :- " + room.RoomNo + " is Overlapped with : ";
                    foreach (var anOverlappedRoom in overlappedRoomList)
                    {
                        message += " Course : " + anOverlappedRoom.Course.CourseCode + " Start Time : "
                            + anOverlappedRoom.StartTime + " and End Time : "
                            + anOverlappedRoom.EndTime + " overlapped from : ";

                        double allocatedStartTime = ConvertTimeIntoDouble(anOverlappedRoom.StartTime);
                        double allocatedEndTime = ConvertTimeIntoDouble(anOverlappedRoom.EndTime);
                        
                        string overlappingStart, overlappingEnd;

                        if ((allocatedStartTime <= startTime) && (startTime < allocatedEndTime))
                            overlappingStart = allocatedroom.StartTime;
                        else
                            overlappingStart = anOverlappedRoom.StartTime;

                        if ((allocatedStartTime < endTime) && (endTime <= allocatedEndTime))
                            overlappingEnd = allocatedroom.EndTime;
                        else
                            overlappingEnd = anOverlappedRoom.EndTime;

                        message += overlappingStart + " to " + overlappingEnd;
                    }

                    ViewBag.Message = message + " on " + day.DayName + "Day.";

                    ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
                    ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
                    ViewBag.CourseID = new SelectList(db.CourseDbSet.Where(c => c.DepartmentID == course.DepartmentID), "CourseID", "CourseCode", allocatedroom.CourseID);
                    ViewBag.RoomID = new SelectList(db.RoomDbSet, "RoomID", "RoomNo", allocatedroom.RoomID);
                    ViewBag.WeekDayID = new SelectList(db.WeekDayDbSet, "WeekDayID", "DayName", allocatedroom.WeekDayID);
                    return View(allocatedroom);
                }

                db.AllocatedRoomDbSet.Add(allocatedroom);
                if (db.SaveChanges() > 0)
                    ViewBag.Message = "Room : " + room.RoomNo + " has been successfully allocated "
                        + " for course : " + course.CourseCode + " From " + allocatedroom.StartTime
                        + " to " + allocatedroom.EndTime + " on " + day.DayName + "Day";

                ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
                ViewBag.CourseID = new SelectList(db.CourseDbSet.Where(c => c.DepartmentID == course.DepartmentID), "CourseID", "CourseCode", allocatedroom.CourseID);
                ViewBag.RoomID = new SelectList(db.RoomDbSet, "RoomID", "RoomNo", allocatedroom.RoomID);
                ViewBag.WeekDayID = new SelectList(db.WeekDayDbSet, "WeekDayID", "DayName", allocatedroom.WeekDayID);
                return View(allocatedroom);  
            }

            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
            ViewBag.CourseID = new SelectList(db.CourseDbSet.Where(c => c.DepartmentID == course.DepartmentID), "CourseID", "CourseCode", allocatedroom.CourseID);
            ViewBag.RoomID = new SelectList(db.RoomDbSet, "RoomID", "RoomNo", allocatedroom.RoomID);
            ViewBag.WeekDayID = new SelectList(db.WeekDayDbSet, "WeekDayID", "DayName", allocatedroom.WeekDayID);
            return View(allocatedroom);
        }

        public PartialViewResult LoadCourseDropDown(int? departmentID)
        {
            if (departmentID != null)
                ViewBag.CourseID = new SelectList(db.CourseDbSet.Where(c => c.DepartmentID == departmentID), "CourseID", "CourseCode");
            return PartialView("~/Views/AllocatedRooms/_LoadCourseDropDown.cshtml");
        }

        private double ConvertTimeIntoDouble(string timeStr)
        {
            try
            {
                if (timeStr.Length == 4)
                    timeStr = "0" + timeStr;
                string hour = timeStr[0].ToString() + timeStr[1];
                string minute = timeStr[3].ToString() + timeStr[4];
                double time = Convert.ToDouble(hour);
                time += (Convert.ToDouble(minute) / 60.0000);
                return time;
            }

            catch (FormatException aFormatException)
            {
                throw new FormatException(
                    "Please input time ONLY in this format HH:mm (within 24 hours)", aFormatException);
            }

            catch (IndexOutOfRangeException anIndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException(
                    "Please input time ONLY in this format HH:mm (within 24 hours)", anIndexOutOfRangeException);
            }

            catch (Exception anException)
            {
                throw new Exception(
                    "An Unknown Error has Occurred", anException);
            }
        }

        public ActionResult ViewRoomAllocation()
        {
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            List<Course> CourseList = db.CourseDbSet.ToList();

            foreach (var aCourse in CourseList)
            {
                aCourse.AllocatedRoomList
                    = db.AllocatedRoomDbSet.Include(a => a.Room).Include(a => a.WeekDay)
                    .Where(a => a.CourseID == aCourse.CourseID).ToList();
            }

            return View(CourseList);
        }

        public PartialViewResult RoomFilter(int? departmentID)
        {
            List<Course> CourseList;
            if (departmentID != null)
            {
                CourseList = db.CourseDbSet.Where(c => c.DepartmentID == departmentID).ToList(); 
            }
            else
            {
                CourseList = db.CourseDbSet.ToList();
            }

            foreach (var aCourse in CourseList)
            {
                aCourse.AllocatedRoomList
                    = db.AllocatedRoomDbSet.Include(a => a.Room).Include(a => a.WeekDay)
                    .Where(a => a.CourseID == aCourse.CourseID).ToList();
            }

            return PartialView("~/Views/AllocatedRooms/_RoomFilter.cshtml", CourseList);
        }

        //
        // GET: /AllocatedRooms/Edit/5
 
        public ActionResult Edit(int id)
        {
            AllocatedRoom allocatedroom = db.AllocatedRoomDbSet.Find(id);
            ViewBag.CourseID = new SelectList(db.CourseDbSet, "CourseID", "CourseCode", allocatedroom.CourseID);
            ViewBag.RoomID = new SelectList(db.RoomDbSet, "RoomID", "RoomNo", allocatedroom.RoomID);
            ViewBag.WeekDayID = new SelectList(db.WeekDayDbSet, "WeekDayID", "DayName", allocatedroom.WeekDayID);
            return View(allocatedroom);
        }

        //
        // POST: /AllocatedRooms/Edit/5

        [HttpPost]
        public ActionResult Edit(AllocatedRoom allocatedroom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(allocatedroom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.CourseDbSet, "CourseID", "CourseCode", allocatedroom.CourseID);
            ViewBag.RoomID = new SelectList(db.RoomDbSet, "RoomID", "RoomNo", allocatedroom.RoomID);
            ViewBag.WeekDayID = new SelectList(db.WeekDayDbSet, "WeekDayID", "DayName", allocatedroom.WeekDayID);
            return View(allocatedroom);
        }

        //
        // GET: /AllocatedRooms/Delete/5
 
        public ActionResult Delete(int id)
        {
            AllocatedRoom allocatedroom = db.AllocatedRoomDbSet.Find(id);
            return View(allocatedroom);
        }

        //
        // POST: /AllocatedRooms/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            AllocatedRoom allocatedroom = db.AllocatedRoomDbSet.Find(id);
            db.AllocatedRoomDbSet.Remove(allocatedroom);
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