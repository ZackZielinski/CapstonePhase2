namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedforeignkeysthatwereplacedinthestudentandinstructormodels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Instructors", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Students", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Instructors", new[] { "UserId" });
            DropIndex("dbo.Students", new[] { "UserId" });
            DropColumn("dbo.Instructors", "UserId");
            DropColumn("dbo.Students", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Instructors", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Students", "UserId");
            CreateIndex("dbo.Instructors", "UserId");
            AddForeignKey("dbo.Students", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Instructors", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
