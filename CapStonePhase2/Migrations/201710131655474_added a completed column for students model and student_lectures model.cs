namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedacompletedcolumnforstudentsmodelandstudent_lecturesmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "CompletedLectures", c => c.Int(nullable: false));
            AddColumn("dbo.Students_Lectures", "CompletedCourse", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students_Lectures", "CompletedCourse");
            DropColumn("dbo.Students", "CompletedLectures");
        }
    }
}
