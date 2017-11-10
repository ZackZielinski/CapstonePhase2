namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedauserIdcolumnforinstructors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Instructors", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Instructors", "UserId");
        }
    }
}
