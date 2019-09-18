namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeAdminProfileIdIdentityNoneInAdminProfileTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AdminProfiles", new[] { "AdminProfileId" });
            DropPrimaryKey("dbo.AdminProfiles");
            AlterColumn("dbo.AdminProfiles", "AdminProfileId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.AdminProfiles", "AdminProfileId");
            CreateIndex("dbo.AdminProfiles", "AdminProfileId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AdminProfiles", new[] { "AdminProfileId" });
            DropPrimaryKey("dbo.AdminProfiles");
            AlterColumn("dbo.AdminProfiles", "AdminProfileId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.AdminProfiles", "AdminProfileId");
            CreateIndex("dbo.AdminProfiles", "AdminProfileId");
        }
    }
}
