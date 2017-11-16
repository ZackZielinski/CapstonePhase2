using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CapStonePhase2.Models
{
    public class Lectures
    {
        public Lectures()
        {
            ListOfMethodNames = new List<string>();
            MethodsAndReturnValues = new Dictionary<string, string>();
        }

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

        [Display(Name = "Methods found with Respective Return Values")]
        public Dictionary<string,string> MethodsAndReturnValues { get; set; }

        [Display(Name = "Methods Found")]
        public List<string> ListOfMethodNames { get; set; }

        public IList<Students_Lectures> Student_Lectures { get; set; }

    }
}