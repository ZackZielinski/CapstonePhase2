namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdamethodmodelwithmethodnamereturnvalueandlectureforeignKeycolumns : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Methods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MethodName = c.String(),
                        ReturnValueType = c.String(),
                        Lectureid = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lectures", t => t.Lectureid, cascadeDelete: true)
                .Index(t => t.Lectureid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Methods", "Lectureid", "dbo.Lectures");
            DropIndex("dbo.Methods", new[] { "Lectureid" });
            DropTable("dbo.Methods");
        }
    }
}
