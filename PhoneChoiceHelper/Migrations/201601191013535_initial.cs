
namespace PhoneChoiceHelper.Migrations
{
    using System;
	using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
	
	[GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public partial class initial : DbMigration, IMigrationMetadata
    {        
        string IMigrationMetadata.Id
        {
            get { return "201601191013535_initial"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return "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"; }
        }

        public override void Up()
        {
            CreateTable(
                "DNE.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 4000),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "DNE.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 4000),
                        RoleId = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("DNE.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("DNE.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "DNE.Setting",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 4000),
                        Value = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "DNE.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 4000),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(maxLength: 4000),
                        SecurityStamp = c.String(maxLength: 4000),
                        PhoneNumber = c.String(maxLength: 4000),
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
                "DNE.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 4000),
                        ClaimType = c.String(maxLength: 4000),
                        ClaimValue = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DNE.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "DNE.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 4000),
                        ProviderKey = c.String(nullable: false, maxLength: 4000),
                        UserId = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("DNE.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DNE.AspNetUserRoles", "UserId", "DNE.AspNetUsers");
            DropForeignKey("DNE.AspNetUserLogins", "UserId", "DNE.AspNetUsers");
            DropForeignKey("DNE.AspNetUserClaims", "UserId", "DNE.AspNetUsers");
            DropForeignKey("DNE.AspNetUserRoles", "RoleId", "DNE.AspNetRoles");
            DropIndex("DNE.AspNetUserLogins", new[] { "UserId" });
            DropIndex("DNE.AspNetUserClaims", new[] { "UserId" });
            DropIndex("DNE.AspNetUsers", "UserNameIndex");
            DropIndex("DNE.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("DNE.AspNetUserRoles", new[] { "UserId" });
            DropIndex("DNE.AspNetRoles", "RoleNameIndex");
            DropTable("DNE.AspNetUserLogins");
            DropTable("DNE.AspNetUserClaims");
            DropTable("DNE.AspNetUsers");
            DropTable("DNE.Setting");
            DropTable("DNE.AspNetUserRoles");
            DropTable("DNE.AspNetRoles");
        }
    }
}
