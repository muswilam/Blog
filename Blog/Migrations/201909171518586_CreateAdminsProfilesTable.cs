namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAdminsProfilesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminProfiles",
                c => new
                    {
                        AdminProfileId = c.Int(nullable: false, identity: true),
                        ProfileUrl = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.AdminProfileId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AdminProfiles");
        }
    }
}
