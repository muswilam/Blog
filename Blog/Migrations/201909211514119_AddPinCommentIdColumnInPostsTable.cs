namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPinCommentIdColumnInPostsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "PinCommentId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "PinCommentId");
        }
    }
}
