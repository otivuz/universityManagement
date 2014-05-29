using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using MVC3UniversityMVC3App1.Models;

namespace OctsProjectMvcTest.Models
{
    [Table("WeekDay")]
    public class WeekDay
    {
        public int WeekDayID { set; get; }
        public string DayName { set; get; }
        public virtual List<AllocatedRoom> AllocatedRoomList { set; get; }
    }
}