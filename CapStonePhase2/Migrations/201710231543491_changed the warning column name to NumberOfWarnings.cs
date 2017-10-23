namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedthewarningcolumnnametoNumberOfWarnings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students_Lectures", "NumberOfWarnings", c => c.Int(nullable: false));
            DropColumn("dbo.Students_Lectures", "Warnings");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students_Lectures", "Warnings", c => c.Int(nullable: false));
            DropColumn("dbo.Students_Lectures", "NumberOfWarnings");
        }
    }
}
