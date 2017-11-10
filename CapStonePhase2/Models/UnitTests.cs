using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CapStonePhase2.Models
{
    public class UnitTests
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("InstructorId")]
        public Instructors Instructor { get; set; }

        public int InstructorId { get; set; }

        [Display(Name = "Name of Unit Test:")]
        public string UnitTestFileName { get; set; }

        public string UnitTestCode { get; set; }
    }
}