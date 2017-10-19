using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("Userid")]
        public ApplicationUser Usertype { get; set; }

        public string Userid { get; set; }

        //Lecture id placeholder
        public int Lectureid { get; set; }

        public IList<Students_Lectures> Students_Lecture { get; set; }
    }
}