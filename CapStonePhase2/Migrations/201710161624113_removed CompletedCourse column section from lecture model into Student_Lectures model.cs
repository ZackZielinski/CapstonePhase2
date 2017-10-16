namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedCompletedCoursecolumnsectionfromlecturemodelintoStudent_Lecturesmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students_Lectures", "CompletedCourse", c => c.Boolean(nullable: false));
            DropColumn("dbo.Lectures", "CompletedCourse");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lectures", "CompletedCourse", c => c.Boolean(nullable: false));
            DropColumn("dbo.Students_Lectures", "CompletedCourse");
        }
    }
}
