namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserNameColumnInAdminTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Administrators", "UserName", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Administrators", "UserName");
        }
    }
}
