// <auto-generated />
namespace Blog.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class RemoveEditTimeColumnAndMakeTimeColumnNotNullableInCommentsTable : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(RemoveEditTimeColumnAndMakeTimeColumnNotNullableInCommentsTable));
        
        string IMigrationMetadata.Id
        {
            get { return "201909031239232_RemoveEditTimeColumnAndMakeTimeColumnNotNullableInCommentsTable"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
