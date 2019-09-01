namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAdminColumnInCommentsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "IsAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "IsAdmin");
        }
    }
}
