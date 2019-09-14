namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHeadlineColumnToAdministratorsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Administrators", "Headline", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Administrators", "Headline");
        }
    }
}
