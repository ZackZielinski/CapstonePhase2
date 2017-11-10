namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdaUnitTestmodelforinstructors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UnitTests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstructorId = c.Int(nullable: false),
                        TestTitle = c.String(),
                        UnitTestFileName = c.String(),
                        UnitTestCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Instructors", t => t.InstructorId, cascadeDelete: true)
                .Index(t => t.InstructorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UnitTests", "InstructorId", "dbo.Instructors");
            DropIndex("dbo.UnitTests", new[] { "InstructorId" });
            DropTable("dbo.UnitTests");
        }
    }
}
