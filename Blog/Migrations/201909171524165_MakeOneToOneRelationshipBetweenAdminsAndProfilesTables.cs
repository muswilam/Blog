namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeOneToOneRelationshipBetweenAdminsAndProfilesTables : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AdminProfiles");
            AlterColumn("dbo.AdminProfiles", "AdminProfileId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.AdminProfiles", "AdminProfileId");
            CreateIndex("dbo.AdminProfiles", "AdminProfileId");
            AddForeignKey("dbo.AdminProfiles", "AdminProfileId", "dbo.Administrators", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdminProfiles", "AdminProfileId", "dbo.Administrators");
            DropIndex("dbo.AdminProfiles", new[] { "AdminProfileId" });
            DropPrimaryKey("dbo.AdminProfiles");
            AlterColumn("dbo.AdminProfiles", "AdminProfileId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.AdminProfiles", "AdminProfileId");
        }
    }
}
