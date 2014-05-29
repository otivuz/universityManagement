using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace OctsProjectMvcTest.Models
{
    [Table("Department")]
    public class Department
    {
        public int DepartmentID { set; get; }

        [Required(ErrorMessage = "Department Code can not be empty!")]
        [Remote("Check_DeptCode", "Departments", ErrorMessage = "Department code already exists!")]
        [Display(Name = "Department Code")]
        public string DeptCode { set; get; }

        [Required(ErrorMessage = "Department Name can not be empty!")]
        [Remote("Check_DeptName", "Departments", ErrorMessage = "Department name already exists!")]
        [Display(Name = "Name")]
        public string DeptName { set; get; }

        public virtual List<Course> CourseList { set; get; }
        public virtual List<Teacher> TeacherList { set; get; }
        public virtual List<Student> StudentList { set; get; }
    }
}
