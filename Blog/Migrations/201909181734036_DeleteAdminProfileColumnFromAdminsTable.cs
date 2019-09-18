namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteAdminProfileColumnFromAdminsTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Administrators", "AdminProfileId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Administrators", "AdminProfileId", c => c.Int(nullable: false));
        }
    }
}
