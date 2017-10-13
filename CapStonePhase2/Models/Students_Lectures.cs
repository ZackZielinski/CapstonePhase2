﻿using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapStonePhase2.Models
{
    public class Students_Lectures
    {
        [ForeignKey("StudentId")]
        public Students Student { get; set; }

        [Key, Column(Order = 1)]
        public int StudentId { get; set; }

        [ForeignKey("LectureId")]
        public Lectures Lecture { get; set; }

        [Key, Column(Order = 2)]
        public int LectureId { get; set; }


        [Display(Name = "Short Answer:")]
        public string ShortAnswer { get; set; }

        [Display(Name = "Is that Correct?")]
        public bool IsShortAnswerCorrect { get; set; }

        [Display(Name = "Student Code File")]
        public string CodeFileName { get; set; }

        public CompilerErrorCollection ListOfErrors { get; set; }

        [Display(Name = "Is the Code Error-free?")]
        public bool IsCodeCorrect { get; set; }

        [Display(Name = "Completed")]
        public bool CompletedCourse { get; set; }
    }
}