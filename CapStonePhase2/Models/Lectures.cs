using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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

        [Display(Name = "Code Assignment:")]
        public string CodeAssignment { get; set; }

        //temporary placeholder for junction table
        public int StudentId { get; set; }
    }
}