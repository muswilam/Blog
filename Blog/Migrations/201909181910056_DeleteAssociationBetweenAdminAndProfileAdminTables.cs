namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteAssociationBetweenAdminAndProfileAdminTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdminProfiles", "AdminProfileId", "dbo.Administrators");
            DropIndex("dbo.AdminProfiles", new[] { "AdminProfileId" });
            DropPrimaryKey("dbo.AdminProfiles");
            AlterColumn("dbo.AdminProfiles", "AdminProfileId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.AdminProfiles", "AdminProfileId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AdminProfiles");
            AlterColumn("dbo.AdminProfiles", "AdminProfileId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.AdminProfiles", "AdminProfileId");
            CreateIndex("dbo.AdminProfiles", "AdminProfileId");
            AddForeignKey("dbo.AdminProfiles", "AdminProfileId", "dbo.Administrators", "Id");
        }
    }
}
