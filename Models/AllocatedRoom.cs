using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OctsProjectMvcTest.Models;

namespace MVC3UniversityMVC3App1.Models
{
    [Table("AllocatedRoom")]
    public class AllocatedRoom
    {
        public int AllocatedRoomID { set; get; }

        public virtual Course Course { set; get; }
        public int CourseID { set; get; }

        public virtual Room Room { set; get; }
        public int RoomID { set; get; }

        public virtual WeekDay WeekDay { set; get; }
        public int WeekDayID { set; get; }

        [Required(ErrorMessage = "Start time is required")]
        [Display(Name = "Start Time - HH:mm (24 hours)")]
        public string StartTime { set; get; }

        [Required(ErrorMessage = "End time is required")]
        [Display(Name = "End Time - HH:mm (24 hours)")]
        public string EndTime { set; get; }
    }
}