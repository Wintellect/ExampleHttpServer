namespace CustomWebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedCompletedColumnName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoes", "Completed", c => c.Boolean(nullable: false));
            DropColumn("dbo.ToDoes", "Complete");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ToDoes", "Complete", c => c.Boolean(nullable: false));
            DropColumn("dbo.ToDoes", "Completed");
        }
    }
}
