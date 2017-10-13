namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movedcompletionfromstudent_lecturemodeltolectures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lectures", "CompletedCourse", c => c.Boolean(nullable: false));
            DropColumn("dbo.Students_Lectures", "CompletedCourse");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students_Lectures", "CompletedCourse", c => c.Boolean(nullable: false));
            DropColumn("dbo.Lectures", "CompletedCourse");
        }
    }
}
