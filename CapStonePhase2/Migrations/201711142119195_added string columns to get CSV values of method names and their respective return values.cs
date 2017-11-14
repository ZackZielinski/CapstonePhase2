namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedstringcolumnstogetCSVvaluesofmethodnamesandtheirrespectivereturnvalues : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lectures", "ListOfMethods", c => c.String());
            AddColumn("dbo.Lectures", "ListOfReturnValues", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lectures", "ListOfReturnValues");
            DropColumn("dbo.Lectures", "ListOfMethods");
        }
    }
}
