namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdalistofwarningscolumnandanumberofwarningscolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students_Lectures", "ListOfWarnings_Capacity", c => c.Int(nullable: false));
            AddColumn("dbo.Students_Lectures", "Warnings", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students_Lectures", "Warnings");
            DropColumn("dbo.Students_Lectures", "ListOfWarnings_Capacity");
        }
    }
}
