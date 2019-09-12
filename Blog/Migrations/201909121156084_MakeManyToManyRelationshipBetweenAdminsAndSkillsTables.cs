namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeManyToManyRelationshipBetweenAdminsAndSkillsTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdministratorSkills",
                c => new
                    {
                        Administrator_Id = c.Int(nullable: false),
                        Skill_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Administrator_Id, t.Skill_Id })
                .ForeignKey("dbo.Administrators", t => t.Administrator_Id, cascadeDelete: true)
                .ForeignKey("dbo.Skills", t => t.Skill_Id, cascadeDelete: true)
                .Index(t => t.Administrator_Id)
                .Index(t => t.Skill_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdministratorSkills", "Skill_Id", "dbo.Skills");
            DropForeignKey("dbo.AdministratorSkills", "Administrator_Id", "dbo.Administrators");
            DropIndex("dbo.AdministratorSkills", new[] { "Skill_Id" });
            DropIndex("dbo.AdministratorSkills", new[] { "Administrator_Id" });
            DropTable("dbo.AdministratorSkills");
        }
    }
}
