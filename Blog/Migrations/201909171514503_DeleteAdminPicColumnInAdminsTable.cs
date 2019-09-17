namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteAdminPicColumnInAdminsTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Administrators", "PicUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Administrators", "PicUrl", c => c.String(maxLength: 200));
        }
    }
}
