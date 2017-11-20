﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CapStonePhase2.Models
{
    public class Methods
    {
        public int Id { get; set; }

        [Display(Name = "Method Name")]
        public string MethodName { get; set; }

        [Display(Name = "Return Value")]
        public string ReturnValueType { get; set; }

        [ForeignKey("Lectureid")]
        public Lectures Lecture { get; set; }

        public int Lectureid { get; set; }

    }
}