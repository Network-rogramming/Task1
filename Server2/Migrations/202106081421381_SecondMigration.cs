namespace Server2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "Faculty_Id", "dbo.Faculties");
            DropForeignKey("dbo.Students", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Groups", new[] { "Faculty_Id" });
            DropIndex("dbo.Students", new[] { "Group_Id" });
            RenameColumn(table: "dbo.Groups", name: "Faculty_Id", newName: "FacultyId");
            RenameColumn(table: "dbo.Students", name: "Group_Id", newName: "GroupId");
            AlterColumn("dbo.Groups", "FacultyId", c => c.Int(nullable: false));
            AlterColumn("dbo.Students", "GroupId", c => c.Int(nullable: false));
            CreateIndex("dbo.Groups", "FacultyId");
            CreateIndex("dbo.Students", "GroupId");
            AddForeignKey("dbo.Groups", "FacultyId", "dbo.Faculties", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Students", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "FacultyId", "dbo.Faculties");
            DropIndex("dbo.Students", new[] { "GroupId" });
            DropIndex("dbo.Groups", new[] { "FacultyId" });
            AlterColumn("dbo.Students", "GroupId", c => c.Int());
            AlterColumn("dbo.Groups", "FacultyId", c => c.Int());
            RenameColumn(table: "dbo.Students", name: "GroupId", newName: "Group_Id");
            RenameColumn(table: "dbo.Groups", name: "FacultyId", newName: "Faculty_Id");
            CreateIndex("dbo.Students", "Group_Id");
            CreateIndex("dbo.Groups", "Faculty_Id");
            AddForeignKey("dbo.Students", "Group_Id", "dbo.Groups", "Id");
            AddForeignKey("dbo.Groups", "Faculty_Id", "dbo.Faculties", "Id");
        }
    }
}
