namespace StockMarketAnalyzer.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProfilePicField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "ProfileUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "ProfileUrl");
        }
    }
}
