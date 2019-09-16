namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostImageUrlColumnInPostsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "PostImageUrl", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "PostImageUrl");
        }
    }
}
