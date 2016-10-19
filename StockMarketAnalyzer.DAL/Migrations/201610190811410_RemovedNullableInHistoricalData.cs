namespace StockMarketAnalyzer.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedNullableInHistoricalData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HistoricalDatas", "Open", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "High", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "Low", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "Close", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "Change", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "PercentageChange", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "Volume", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "AdjClose", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HistoricalDatas", "AdjClose", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "Volume", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "PercentageChange", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "Change", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "Close", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "Low", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "High", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.HistoricalDatas", "Open", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
