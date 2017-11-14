namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedmethodstoincludeduetonotfindingproperreturnvalues : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Students_Lectures", "MethodsToIncludeInCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students_Lectures", "MethodsToIncludeInCode", c => c.String());
        }
    }
}
