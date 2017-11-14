namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedacolumninstudentLecturestokeeptrackofwhatmethodsareneededtobeincludedinthecode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students_Lectures", "MethodsToIncludeInCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students_Lectures", "MethodsToIncludeInCode");
        }
    }
}
