using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapStonePhase2.Models
{
    public class Students
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "First Name:")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name:")]
        public string LastName { get; set; }

        [Display(Name = "Number of Completed Lectures")]
        public int CompletedLectures { get; set; }

        //Lecture Id placeholder
        public int LectureId { get; set; }

        public IList<Students_Lectures> Students_Lecture { get; set; }
    }
}