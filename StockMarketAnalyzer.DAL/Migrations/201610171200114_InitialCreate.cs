namespace StockMarketAnalyzer.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Symbol = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Exch = c.String(),
                        ExchDisp = c.String(),
                        Type = c.String(),
                        TypeDisp = c.String(),
                    })
                .PrimaryKey(t => t.Symbol);
            
            CreateTable(
                "dbo.CompanyOfficers",
                c => new
                    {
                        CompanyOfficerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Title = c.String(),
                        Age = c.Int(),
                        FiscalYear = c.Int(),
                        TotalPay = c.Decimal(precision: 18, scale: 2),
                        ExercisedValue = c.Decimal(precision: 18, scale: 2),
                        UnexercisedValue = c.Decimal(precision: 18, scale: 2),
                        Symbol = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CompanyOfficerId)
                .ForeignKey("dbo.Companies", t => t.Symbol, cascadeDelete: true)
                .Index(t => t.Symbol);
            
            CreateTable(
                "dbo.CompanyProfiles",
                c => new
                    {
                        Symbol = c.String(nullable: false, maxLength: 128),
                        Address1 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Country = c.String(),
                        Phone = c.String(),
                        Website = c.String(),
                        Industry = c.String(),
                        Sector = c.String(),
                        LongBusinessSummary = c.String(),
                        FullTimeEmployees = c.Long(),
                    })
                .PrimaryKey(t => t.Symbol)
                .ForeignKey("dbo.Companies", t => t.Symbol)
                .Index(t => t.Symbol);
            
            CreateTable(
                "dbo.HistoricalDatas",
                c => new
                    {
                        HistoricalDataId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Open = c.Decimal(precision: 18, scale: 2),
                        High = c.Decimal(precision: 18, scale: 2),
                        Low = c.Decimal(precision: 18, scale: 2),
                        Close = c.Decimal(precision: 18, scale: 2),
                        Change = c.Decimal(precision: 18, scale: 2),
                        PercentageChange = c.Decimal(precision: 18, scale: 2),
                        Volume = c.Decimal(precision: 18, scale: 2),
                        AdjClose = c.Decimal(precision: 18, scale: 2),
                        Symbol = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.HistoricalDataId)
                .ForeignKey("dbo.Companies", t => t.Symbol, cascadeDelete: true)
                .Index(t => t.Symbol);
            
            CreateTable(
                "dbo.UserPortfolios",
                c => new
                    {
                        UserPortfolioId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Symbol = c.String(nullable: false, maxLength: 128),
                        BuyRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaleRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.UserPortfolioId)
                .ForeignKey("dbo.Companies", t => t.Symbol, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.Symbol);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        UserType = c.Int(nullable: false),
                        Username = c.String(),
                        EmailAddress = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.UserProfiles", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        Zip = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPortfolios", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.UserPortfolios", "Symbol", "dbo.Companies");
            DropForeignKey("dbo.HistoricalDatas", "Symbol", "dbo.Companies");
            DropForeignKey("dbo.CompanyProfiles", "Symbol", "dbo.Companies");
            DropForeignKey("dbo.CompanyOfficers", "Symbol", "dbo.Companies");
            DropIndex("dbo.Users", new[] { "UserId" });
            DropIndex("dbo.UserPortfolios", new[] { "Symbol" });
            DropIndex("dbo.UserPortfolios", new[] { "UserId" });
            DropIndex("dbo.HistoricalDatas", new[] { "Symbol" });
            DropIndex("dbo.CompanyProfiles", new[] { "Symbol" });
            DropIndex("dbo.CompanyOfficers", new[] { "Symbol" });
            DropTable("dbo.UserProfiles");
            DropTable("dbo.Users");
            DropTable("dbo.UserPortfolios");
            DropTable("dbo.HistoricalDatas");
            DropTable("dbo.CompanyProfiles");
            DropTable("dbo.CompanyOfficers");
            DropTable("dbo.Companies");
        }
    }
}
