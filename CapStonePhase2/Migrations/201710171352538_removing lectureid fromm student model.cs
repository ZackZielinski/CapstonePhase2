namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removinglectureidfrommstudentmodel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Students", "LectureId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "LectureId", c => c.Int(nullable: false));
        }
    }
}
