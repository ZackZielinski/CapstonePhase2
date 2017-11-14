using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapStonePhase2.Models
{
    public class Students_Lectures
    {
        public Students_Lectures()
        {
            ListOfErrors = new CompilerErrorCollection();
            ListOfWarnings = new CompilerErrorCollection();
            CodeRewards = "Awesome-sauce!";
        }

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

        [Display(Name = "Is Short Answer Correct?")]
        public bool IsShortAnswerCorrect { get; set; }

        public CompilerErrorCollection ListOfErrors { get; set; }

        public CompilerErrorCollection ListOfWarnings { get; set; }

        [Display(Name = "Does the Code have Zero Errors?")]
        public bool IsCodeCorrect { get; set; }

        [Display(Name = "Number of Errors")]
        public int NumberOfErrors { get; set; }

        [Display(Name ="Number of Warnings")]
        public int NumberOfWarnings { get; set; }

        [Display(Name = "Completed Course")]
        public bool CompletedCourse { get; set; }

        public string CodeRewards { get; set; }

        public string CodeFileName { get; set; }

        public string CodeFileText { get; set; }

        public IEnumerable<Students_Lectures> StudentInLectures { get; set; }
    }
}