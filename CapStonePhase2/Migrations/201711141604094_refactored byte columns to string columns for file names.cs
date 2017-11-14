namespace CapStonePhase2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactoredbytecolumnstostringcolumnsforfilenames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lectures", "CodeFileName", c => c.String());
            AddColumn("dbo.Students_Lectures", "CodeFileName", c => c.String());
            DropColumn("dbo.Lectures", "CodeFile");
            DropColumn("dbo.Students_Lectures", "CodeFile");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students_Lectures", "CodeFile", c => c.Binary());
            AddColumn("dbo.Lectures", "CodeFile", c => c.Binary());
            DropColumn("dbo.Students_Lectures", "CodeFileName");
            DropColumn("dbo.Lectures", "CodeFileName");
        }
    }
}
