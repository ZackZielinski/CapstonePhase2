namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactoredmethodstabletoobtainalistofmethodsinStudent_Lectures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Methods", "Students_Lectures_StudentId", c => c.Int());
            AddColumn("dbo.Methods", "Students_Lectures_LectureId", c => c.Int());
            CreateIndex("dbo.Methods", new[] { "Students_Lectures_StudentId", "Students_Lectures_LectureId" });
            AddForeignKey("dbo.Methods", new[] { "Students_Lectures_StudentId", "Students_Lectures_LectureId" }, "dbo.Students_Lectures", new[] { "StudentId", "LectureId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Methods", new[] { "Students_Lectures_StudentId", "Students_Lectures_LectureId" }, "dbo.Students_Lectures");
            DropIndex("dbo.Methods", new[] { "Students_Lectures_StudentId", "Students_Lectures_LectureId" });
            DropColumn("dbo.Methods", "Students_Lectures_LectureId");
            DropColumn("dbo.Methods", "Students_Lectures_StudentId");
        }
    }
}
