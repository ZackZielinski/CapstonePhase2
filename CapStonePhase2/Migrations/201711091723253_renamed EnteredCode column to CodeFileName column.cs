namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamedEnteredCodecolumntoCodeFileNamecolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students_Lectures", "CodeFileName", c => c.String());
            DropColumn("dbo.Students_Lectures", "EnteredCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students_Lectures", "EnteredCode", c => c.String());
            DropColumn("dbo.Students_Lectures", "CodeFileName");
        }
    }
}
