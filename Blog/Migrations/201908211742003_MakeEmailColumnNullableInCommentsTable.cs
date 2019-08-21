namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeEmailColumnNullableInCommentsTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "Email", c => c.String(nullable: false));
        }
    }
}
