namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdStudentsLecturesInstructorsandStudents_Lecturesmodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Instructors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Topic = c.String(),
                        Description = c.String(),
                        ReviewQuestion = c.String(),
                        CodeAssignment = c.String(),
                        StudentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        LectureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students_Lectures",
                c => new
                    {
                        StudentId = c.Int(nullable: false),
                        LectureId = c.Int(nullable: false),
                        ShortAnswer = c.String(),
                        IsShortAnswerCorrect = c.Boolean(nullable: false),
                        CodeFileName = c.String(),
                        ListOfErrors_Capacity = c.Int(nullable: false),
                        IsCodeCorrect = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentId, t.LectureId });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Students_Lectures");
            DropTable("dbo.Students");
            DropTable("dbo.Lectures");
            DropTable("dbo.Instructors");
        }
    }
}
