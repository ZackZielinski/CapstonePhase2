﻿using System.ComponentModel.DataAnnotations;

namespace CapStonePhase2.Models
{
    public class Instructors
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "First Name:")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name:")]
        public string LastName { get; set; }

    }
}