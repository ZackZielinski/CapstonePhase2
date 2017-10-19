namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedaCodeRewardcolumninStudent_Lecturesmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students_Lectures", "CodeRewards", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students_Lectures", "CodeRewards");
        }
    }
}
