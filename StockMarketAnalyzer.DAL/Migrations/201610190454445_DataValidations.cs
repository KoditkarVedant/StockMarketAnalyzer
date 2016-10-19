namespace StockMarketAnalyzer.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataValidations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserPortfolios", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Users", "EmailAddress", c => c.String(nullable: false));
            AlterColumn("dbo.UserProfiles", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.UserProfiles", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.UserProfiles", "PhoneNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfiles", "PhoneNumber", c => c.String());
            AlterColumn("dbo.UserProfiles", "LastName", c => c.String());
            AlterColumn("dbo.UserProfiles", "FirstName", c => c.String());
            AlterColumn("dbo.Users", "EmailAddress", c => c.String());
            AlterColumn("dbo.Users", "Password", c => c.String());
            DropColumn("dbo.UserPortfolios", "Quantity");
        }
    }
}
