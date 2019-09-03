namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEditTimeColumnInPostsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "EditTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "EditTime");
        }
    }
}
