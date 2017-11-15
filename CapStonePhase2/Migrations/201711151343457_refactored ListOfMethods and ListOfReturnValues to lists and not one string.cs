namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactoredListOfMethodsandListOfReturnValuestolistsandnotonestring : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Lectures", "ListOfMethods");
            DropColumn("dbo.Lectures", "ListOfReturnValues");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lectures", "ListOfReturnValues", c => c.String());
            AddColumn("dbo.Lectures", "ListOfMethods", c => c.String());
        }
    }
}
