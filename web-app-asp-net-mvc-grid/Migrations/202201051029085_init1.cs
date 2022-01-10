namespace web_app_asp_net_mvc_grid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Disciplines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DisciplineGoal = c.String(nullable: false),
                        DisciplineObjectives = c.String(nullable: false),
                        MainSections = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        TeacherId = c.Int(nullable: false),
                        DisciplineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disciplines", t => t.DisciplineId, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId)
                .Index(t => t.DisciplineId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false),
                        NumberOfStudents = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Nationalities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Sex = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeacherImages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Guid = c.Guid(nullable: false),
                        Data = c.Binary(nullable: false),
                        ContentType = c.String(maxLength: 100),
                        DateChanged = c.DateTime(),
                        FileName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GroupLessons",
                c => new
                    {
                        Group_Id = c.Int(nullable: false),
                        Lesson_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.Lesson_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.Lessons", t => t.Lesson_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.Lesson_Id);
            
            CreateTable(
                "dbo.NationalityGroups",
                c => new
                    {
                        Nationality_Id = c.Int(nullable: false),
                        Group_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Nationality_Id, t.Group_Id })
                .ForeignKey("dbo.Nationalities", t => t.Nationality_Id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.Nationality_Id)
                .Index(t => t.Group_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TeacherImages", "Id", "dbo.Teachers");
            DropForeignKey("dbo.Lessons", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.NationalityGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.NationalityGroups", "Nationality_Id", "dbo.Nationalities");
            DropForeignKey("dbo.GroupLessons", "Lesson_Id", "dbo.Lessons");
            DropForeignKey("dbo.GroupLessons", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Lessons", "DisciplineId", "dbo.Disciplines");
            DropIndex("dbo.NationalityGroups", new[] { "Group_Id" });
            DropIndex("dbo.NationalityGroups", new[] { "Nationality_Id" });
            DropIndex("dbo.GroupLessons", new[] { "Lesson_Id" });
            DropIndex("dbo.GroupLessons", new[] { "Group_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.TeacherImages", new[] { "Id" });
            DropIndex("dbo.Lessons", new[] { "DisciplineId" });
            DropIndex("dbo.Lessons", new[] { "TeacherId" });
            DropTable("dbo.NationalityGroups");
            DropTable("dbo.GroupLessons");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TeacherImages");
            DropTable("dbo.Teachers");
            DropTable("dbo.Nationalities");
            DropTable("dbo.Groups");
            DropTable("dbo.Lessons");
            DropTable("dbo.Disciplines");
        }
    }
}
