namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamedUnitTeststoCodeTests : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UnitTests", "InstructorId", "dbo.Instructors");
            DropIndex("dbo.UnitTests", new[] { "InstructorId" });
            CreateTable(
                "dbo.CodeTests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TestFileName = c.String(),
                        TestFileCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.UnitTests");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UnitTests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstructorId = c.Int(nullable: false),
                        UnitTestFileName = c.String(),
                        UnitTestCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.CodeTests");
            CreateIndex("dbo.UnitTests", "InstructorId");
            AddForeignKey("dbo.UnitTests", "InstructorId", "dbo.Instructors", "Id", cascadeDelete: true);
        }
    }
}
