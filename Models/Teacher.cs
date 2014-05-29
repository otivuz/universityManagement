using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace OctsProjectMvcTest.Models
{
    [Table("Teacher")]
    public class Teacher
    {
        public int TeacherID { set; get; }

        [Required(ErrorMessage = "Error : Teacher Name can not be empty !!!")]
        [Display(Name = "Teacher Name :- ")]
        public string TeacherName { set; get; }

        public string Address { set; get; }

        [Required(ErrorMessage = "Error : Email can not be empty !!!")]
        [Remote("Check_Email", "Teachers",
            ErrorMessage = "Error : this email Id already exists !!!")]
        public string Email { set; get; }

        public string ContactNo { set; get; }

        [Display(Name = "Credits  to  take  :- ")]
        public double CreditsToBeTaken { set; get; }
        public double CreditsHaveTaken { set; get; }
        public double CreditsRemaining { set; get; }

        public virtual Designation Designation { set; get; }
        public int DesignationID { set; get; }

        public virtual Department Department { set; get; }
        public int DepartmentID { set; get; }
    }
}
