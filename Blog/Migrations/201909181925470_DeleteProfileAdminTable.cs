namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteProfileAdminTable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.AdminProfiles");
        }
        
        public override void Down()
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
    }
}
