using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OctsProjectMvcTest.Models
{
    public class SampleData : DropCreateDatabaseIfModelChanges<RootProjDbContext>
    {
        protected override void Seed(RootProjDbContext context)
        {
            context.DepartmentDbSet.Add(new Department { DeptCode = "CSE", DeptName = "Computer Science and Engineering" });
            context.DepartmentDbSet.Add(new Department { DeptCode = "EEE", DeptName = "Electrical & Electronics Engineering" });

            context.SemesterDbSet.Add(new Semester { SemesterName = "First" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "Second" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "Third" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "Forth" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "Fifth" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "Sixth" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "Seventh" });
            context.SemesterDbSet.Add(new Semester { SemesterName = "Eighth" });

            context.DesignationDbSet.Add(new Designation { DsgName = "Lecturer" });
            context.DesignationDbSet.Add(new Designation { DsgName = "Assistant Profrssor" });
            context.DesignationDbSet.Add(new Designation { DsgName = "Associate Professor" });
            context.DesignationDbSet.Add(new Designation { DsgName = "Professor" });

            context.SaveChanges();

            context.CourseDbSet.Add(new Course
            {
                CourseCode = "CSE-115",
                CourseName = "C Programming",
                Credit = 3.0000,
                Description = "Introductino to Programming",
                DepartmentID = 1,
                SemesterID = 1,
                AssignedTo = "Yet Not Assigned "
            });
            context.CourseDbSet.Add(new Course
            {
                CourseCode = "CSE-135",
                CourseName = "Java Programming",
                Credit = 3.0000,
                Description = "OOP Programming",
                DepartmentID = 1,
                SemesterID = 2,
                AssignedTo = "Yet Not Assigned "
            });
            context.CourseDbSet.Add(new Course
            {
                CourseCode = "CSE-173",
                CourseName = "Discrete Mathematics",
                Credit = 3.0000,
                Description = "Discrete Mathematics",
                DepartmentID = 1,
                SemesterID = 3,
                AssignedTo = "Yet Not Assigned "
            });
            context.CourseDbSet.Add(new Course
            {
                CourseCode = "EEE-101",
                CourseName = "Electronics",
                Credit = 3.0000,
                Description = "Introduction to Electronics",
                DepartmentID = 2,
                SemesterID = 1,
                AssignedTo = "Yet Not Assigned "
            });

            context.TeacherDbSet.Add(new Teacher
            {
                TeacherName = "Abul L. Haque",
                Address = "Bashundhara",
                Email = "alh@nsu.com",
                ContactNo = "11111111111",
                CreditsToBeTaken = 16.0000,
                CreditsHaveTaken = 0.0000,
                CreditsRemaining = 16.0000,
                DepartmentID = 1,
                DesignationID = 4
            });
            context.TeacherDbSet.Add(new Teacher
            {
                TeacherName = "Lutfe Elahi",
                Address = "Shantinagar",
                Email = "mle@nsu.com",
                ContactNo = "22222222222",
                CreditsToBeTaken = 10.0000,
                CreditsHaveTaken = 0.0000,
                CreditsRemaining = 10.0000,
                DepartmentID = 1,
                DesignationID = 1
            });
            context.TeacherDbSet.Add(new Teacher
            {
                TeacherName = "Rashedur Rahman",
                Address = "Dhanmondi",
                Email = "rrn@nsu.com",
                ContactNo = "33333333333",
                CreditsToBeTaken = 17.0000,
                CreditsHaveTaken = 0.0000,
                CreditsRemaining = 17.0000,
                DepartmentID = 1,
                DesignationID = 3
            });

            context.SaveChanges();

            context.StudentDbSet.Add(new Student
                {
                    Address = "Panchogor",
                    AdmissionDate = Convert.ToDateTime("10/04/2014"),
                    ContactNo = "44444444444",
                    DepartmentID = 1,
                    Email = "sa@nsu.com",
                    RegNo = "CSE2014001",
                    StudentName = "Sohan Abrar"
                });
            context.StudentDbSet.Add(new Student
            {
                Address = "Rajshashi",
                AdmissionDate = Convert.ToDateTime("10/04/2014"),
                ContactNo = "55555555555",
                DepartmentID = 1,
                Email = "kr@nsu.com",
                RegNo = "CSE2014002",
                StudentName = "Kollol Roy"
            });
            context.StudentDbSet.Add(new Student
            {
                Address = "Sylhet",
                AdmissionDate = Convert.ToDateTime("10/12/2013"),
                ContactNo = "66666666666",
                DepartmentID = 2,
                Email = "hm@nsu.com",
                RegNo = "EEE2013001",
                StudentName = "Hemon Babu"
            });
            context.StudentDbSet.Add(new Student
            {
                Address = "Comilla",
                AdmissionDate = Convert.ToDateTime("10/05/2012"),
                ContactNo = "77777777777",
                DepartmentID = 2,
                Email = "ba@nsu.com",
                RegNo = "EEE2012002",
                StudentName = "Hasin Taufiq"
            });
            context.SaveChanges();

            context.GradeDbSet.Add(new Grade { GradeLetter = "Result not published yet" });
            context.GradeDbSet.Add(new Grade {GradeLetter = "A+"});
            context.GradeDbSet.Add(new Grade { GradeLetter = "A" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "A-" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "B+" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "B" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "B-" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "C+" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "C" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "D" });
            context.GradeDbSet.Add(new Grade { GradeLetter = "F" });
            context.SaveChanges();

            context.WeekDayDbSet.Add(new WeekDay { DayName = "Sun" });
            context.WeekDayDbSet.Add(new WeekDay { DayName = "Mon" });
            context.WeekDayDbSet.Add(new WeekDay { DayName = "Tue" });
            context.WeekDayDbSet.Add(new WeekDay { DayName = "Wed" });
            context.WeekDayDbSet.Add(new WeekDay { DayName = "Thu" });
            context.WeekDayDbSet.Add(new WeekDay { DayName = "Fri" });
            context.WeekDayDbSet.Add(new WeekDay { DayName = "Sat" });

            context.RoomDbSet.Add(new Room { RoomNo = "A-101" });
            context.RoomDbSet.Add(new Room { RoomNo = "A-201" });
            context.RoomDbSet.Add(new Room { RoomNo = "A-301" });
            context.RoomDbSet.Add(new Room { RoomNo = "B-102" });
            context.RoomDbSet.Add(new Room { RoomNo = "B-201" });
            context.RoomDbSet.Add(new Room { RoomNo = "B-202" });
            context.SaveChanges();
        }
    }
}