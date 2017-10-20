namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedaCodeFilecolumninStudents_Lecturesmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students_Lectures", "CodeFile", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students_Lectures", "CodeFile");
        }
    }
}
