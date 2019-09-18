namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdminProfileIdForeignKeyColumnInAdminsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Administrators", "AdminProfileId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Administrators", "AdminProfileId");
        }
    }
}
