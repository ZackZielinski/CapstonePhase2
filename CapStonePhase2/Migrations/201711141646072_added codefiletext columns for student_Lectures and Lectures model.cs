namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedcodefiletextcolumnsforstudent_LecturesandLecturesmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lectures", "CodeFileText", c => c.String());
            AddColumn("dbo.Students_Lectures", "CodeFileText", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students_Lectures", "CodeFileText");
            DropColumn("dbo.Lectures", "CodeFileText");
        }
    }
}
