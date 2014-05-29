using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OctsProjectMvcTest.Models
{
    [Table("AssignedCourse")]
    public class AssignedCourse
    {
        public int AssignedCourseID { set; get; }

        public virtual Course Course { set; get; }
        public int CourseID { set; get; }

        public virtual Teacher Teacher { set; get; }
        public int TeacherID { set; get; }
    }
}
