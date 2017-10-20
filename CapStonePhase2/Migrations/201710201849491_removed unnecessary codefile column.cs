namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedunnecessarycodefilecolumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Students_Lectures", "CodeFile");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students_Lectures", "CodeFile", c => c.Binary());
        }
    }
}
