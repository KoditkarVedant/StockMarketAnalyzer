namespace StockMarketAnalyzer.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Analysis : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "IncreaseNextDayIncreasePercentages", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "IncreaseNextDayDecreasePercentages", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "IncreaseNextDayNoChangePercentages", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "DecreaseNextDayIncreasePercentages", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "DecreaseNextDayDecreasePercentages", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "DecreaseNextDayNoChangePercentages", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "NoChangeNextDayIncreasePercentages", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "NoChangeNextDayDecreasePercentages", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "NoChangeNextDayNoChangePercentages", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "NoChangeNextDayNoChangePercentages");
            DropColumn("dbo.Companies", "NoChangeNextDayDecreasePercentages");
            DropColumn("dbo.Companies", "NoChangeNextDayIncreasePercentages");
            DropColumn("dbo.Companies", "DecreaseNextDayNoChangePercentages");
            DropColumn("dbo.Companies", "DecreaseNextDayDecreasePercentages");
            DropColumn("dbo.Companies", "DecreaseNextDayIncreasePercentages");
            DropColumn("dbo.Companies", "IncreaseNextDayNoChangePercentages");
            DropColumn("dbo.Companies", "IncreaseNextDayDecreasePercentages");
            DropColumn("dbo.Companies", "IncreaseNextDayIncreasePercentages");
        }
    }
}
