namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsToAdminTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Administrators", "Email", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Administrators", "Birthdate", c => c.DateTime());
            AddColumn("dbo.Administrators", "Education", c => c.String(maxLength: 200));
            AddColumn("dbo.Administrators", "Country", c => c.String(maxLength: 20));
            AddColumn("dbo.Administrators", "Bio", c => c.String(maxLength: 500));
            AddColumn("dbo.Administrators", "PicUrl", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Administrators", "PicUrl");
            DropColumn("dbo.Administrators", "Bio");
            DropColumn("dbo.Administrators", "Country");
            DropColumn("dbo.Administrators", "Education");
            DropColumn("dbo.Administrators", "Birthdate");
            DropColumn("dbo.Administrators", "Email");
        }
    }
}
