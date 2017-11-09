namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingandrefactoringcolumnstostartsavingtextintoafile : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Students", "Userid", "dbo.AspNetUsers");
            DropIndex("dbo.Students", new[] { "Userid" });
            AddColumn("dbo.Students_Lectures", "EnteredCode", c => c.String());
            AlterColumn("dbo.Students", "Userid", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "Userid", c => c.String(maxLength: 128));
            DropColumn("dbo.Students_Lectures", "EnteredCode");
            CreateIndex("dbo.Students", "Userid");
            AddForeignKey("dbo.Students", "Userid", "dbo.AspNetUsers", "Id");
        }
    }
}
