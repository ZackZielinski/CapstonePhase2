namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdaforeignKeyfromstudentstoapplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Userid", c => c.String(maxLength: 128));
            CreateIndex("dbo.Students", "Userid");
            AddForeignKey("dbo.Students", "Userid", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "Userid", "dbo.AspNetUsers");
            DropIndex("dbo.Students", new[] { "Userid" });
            DropColumn("dbo.Students", "Userid");
        }
    }
}
