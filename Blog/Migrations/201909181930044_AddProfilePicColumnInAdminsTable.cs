namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfilePicColumnInAdminsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Administrators", "ProfilePic", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Administrators", "ProfilePic");
        }
    }
}
