using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OctsProjectMvcTest.Models
{
    public class Grade
    {
        public int GradeID { set; get; }
        public string GradeLetter { set; get; }
        public virtual List<Enrollment> Enrollments { set; get; } 
    }
}