using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CapStonePhase2.Models
{
    public class Lectures
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Topic:")]
        public string Topic { get; set; }

        [Display(Name = "Description:")]
        public string Description { get; set; }

        [Display(Name = "Review Question:")]
        public string ReviewQuestion { get; set; }

        [Display(Name = "Coded Example:")]
        public string CodeExample { get; set; }

        [Display(Name = "Code Assignment:")]
        public string CodeAssignment { get; set; }

        //Student Id placeholder
        public int StudentId { get; set; }

        public string CodeFileName { get; set; }

        public string CodeFileText { get; set; }

        public IList<Students_Lectures> Student_Lectures { get; set; }

    }
}