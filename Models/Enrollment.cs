using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OctsProjectMvcTest.Models
{
    public class Enrollment
    {
        public int EnrollmentID { set; get; }

        public virtual Student Student { set; get; }
        public int StudentID { set; get; }

        public virtual Course Course { set; get; }
        public int CourseID { set; get; }

        public DateTime EnrollmentDate { set; get; }

        public virtual Grade Grade { set; get; }
        public int GradeID { set; get; }
    }
}