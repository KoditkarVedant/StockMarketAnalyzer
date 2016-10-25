namespace StockMarketAnalyzer.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFBandTwitterField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyProfiles", "FBHandle", c => c.String());
            AddColumn("dbo.CompanyProfiles", "TWHandle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyProfiles", "TWHandle");
            DropColumn("dbo.CompanyProfiles", "FBHandle");
        }
    }
}
