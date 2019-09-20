namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsMasterColumnInAdminsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Administrators", "IsMaster", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Administrators", "IsMaster");
        }
    }
}
