namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEditTimeColumnAndMakeTimeColumnNotNullableInCommentsTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "Time", c => c.DateTime(nullable: false));
            DropColumn("dbo.Comments", "EditTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "EditTime", c => c.DateTime());
            AlterColumn("dbo.Comments", "Time", c => c.DateTime());
        }
    }
}
