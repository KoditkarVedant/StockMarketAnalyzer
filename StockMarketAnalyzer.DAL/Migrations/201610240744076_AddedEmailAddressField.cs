namespace StockMarketAnalyzer.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEmailAddressField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "EmailAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "EmailAddress");
        }
    }
}
