﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OctsProjectMvcTest.Models
{
    [Table("Semester")]
    public class Semester
    {
        public int SemesterID { set; get; }
        public string SemesterName { set; get; }
        public virtual List<Course> CourseList { set; get; }
    }
}
