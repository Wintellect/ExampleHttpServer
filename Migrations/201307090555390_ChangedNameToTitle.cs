namespace CustomWebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedNameToTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoes", "Title", c => c.String());
            DropColumn("dbo.ToDoes", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ToDoes", "Name", c => c.String());
            DropColumn("dbo.ToDoes", "Title");
        }
    }
}
