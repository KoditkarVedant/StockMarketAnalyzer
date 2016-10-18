namespace StockMarketAnalyzer.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRegistertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Password", c => c.String());
            DropColumn("dbo.Users", "Username");
            DropColumn("dbo.Users", "PhoneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "PhoneNumber", c => c.String());
            AddColumn("dbo.Users", "Username", c => c.String());
            DropColumn("dbo.Users", "Password");
        }
    }
}
