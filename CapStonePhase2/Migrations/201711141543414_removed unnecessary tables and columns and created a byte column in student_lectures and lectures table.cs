namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedunnecessarytablesandcolumnsandcreatedabytecolumninstudent_lecturesandlecturestable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lectures", "CodeFile", c => c.Binary());
            AddColumn("dbo.Students_Lectures", "CodeFile", c => c.Binary());
            DropColumn("dbo.Students_Lectures", "CodeFileName");
            DropColumn("dbo.Students_Lectures", "StudentCode");
            DropTable("dbo.CodeTests");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CodeTests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TestFileName = c.String(),
                        TestFileCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Students_Lectures", "StudentCode", c => c.String());
            AddColumn("dbo.Students_Lectures", "CodeFileName", c => c.String());
            DropColumn("dbo.Students_Lectures", "CodeFile");
            DropColumn("dbo.Lectures", "CodeFile");
        }
    }
}
