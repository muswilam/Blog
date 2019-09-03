namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeTimeColumnNullableInPostsTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "Time", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "Time", c => c.DateTime(nullable: false));
        }
    }
}
