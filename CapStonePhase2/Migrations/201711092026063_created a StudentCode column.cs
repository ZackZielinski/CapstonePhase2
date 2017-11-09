namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdaStudentCodecolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students_Lectures", "StudentCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students_Lectures", "StudentCode");
        }
    }
}
