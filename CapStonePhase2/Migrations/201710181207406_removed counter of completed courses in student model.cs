namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedcounterofcompletedcoursesinstudentmodel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Students", "CompletedLectures");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "CompletedLectures", c => c.Int(nullable: false));
        }
    }
}
