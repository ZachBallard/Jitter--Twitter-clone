namespace Jitter.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JitterFollowers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FollowerId = c.String(maxLength: 128),
                        FollowingId = c.String(maxLength: 128),
                        IsBlocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowerId)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowingId)
                .Index(t => t.FollowerId)
                .Index(t => t.FollowingId);
            
            CreateTable(
                "dbo.Tweaks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TweakDate = c.DateTime(nullable: false),
                        Body = c.String(maxLength: 250),
                        JitterUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.JitterUser_Id)
                .Index(t => t.JitterUser_Id);
            
            AddColumn("dbo.AspNetUsers", "Handle", c => c.String());
            AddColumn("dbo.AspNetUsers", "PhotoURL", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tweaks", "JitterUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.JitterFollowers", "FollowingId", "dbo.AspNetUsers");
            DropForeignKey("dbo.JitterFollowers", "FollowerId", "dbo.AspNetUsers");
            DropIndex("dbo.Tweaks", new[] { "JitterUser_Id" });
            DropIndex("dbo.JitterFollowers", new[] { "FollowingId" });
            DropIndex("dbo.JitterFollowers", new[] { "FollowerId" });
            DropColumn("dbo.AspNetUsers", "PhotoURL");
            DropColumn("dbo.AspNetUsers", "Handle");
            DropTable("dbo.Tweaks");
            DropTable("dbo.JitterFollowers");
        }
    }
}
