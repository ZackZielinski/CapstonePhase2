namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdforeignKeysfromStudents_LecturestoStudentsandtoLectures : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Students_Lectures", "StudentId");
            CreateIndex("dbo.Students_Lectures", "LectureId");
            AddForeignKey("dbo.Students_Lectures", "LectureId", "dbo.Lectures", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Students_Lectures", "StudentId", "dbo.Students", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students_Lectures", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Students_Lectures", "LectureId", "dbo.Lectures");
            DropIndex("dbo.Students_Lectures", new[] { "LectureId" });
            DropIndex("dbo.Students_Lectures", new[] { "StudentId" });
        }
    }
}
