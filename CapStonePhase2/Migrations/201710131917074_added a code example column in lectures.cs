namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedacodeexamplecolumninlectures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lectures", "CodeExample", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lectures", "CodeExample");
        }
    }
}
