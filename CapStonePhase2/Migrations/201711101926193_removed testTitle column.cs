namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedtestTitlecolumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UnitTests", "TestTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UnitTests", "TestTitle", c => c.String());
        }
    }
}
