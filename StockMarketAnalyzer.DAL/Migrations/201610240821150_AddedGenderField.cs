namespace StockMarketAnalyzer.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGenderField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "Gender");
        }
    }
}
