namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedfilenamecloumninStudent_Lecturesmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Lectureid", c => c.Int(nullable: false));
            DropColumn("dbo.Students_Lectures", "CodeFileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students_Lectures", "CodeFileName", c => c.String());
            DropColumn("dbo.Students", "Lectureid");
        }
    }
}
