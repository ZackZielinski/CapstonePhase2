namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedacolumndeterminingnumberoferrors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students_Lectures", "NumberOfErrors", c => c.Int(nullable: false));
            DropColumn("dbo.Students_Lectures", "TotalErrors");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students_Lectures", "TotalErrors", c => c.Int(nullable: false));
            DropColumn("dbo.Students_Lectures", "NumberOfErrors");
        }
    }
}
