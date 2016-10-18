using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.BO
{
    public class Register
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public enum UserType
    {
        Admin,
        User
    }

    public class User
    {
        public int UserId { get; set; }
        public UserType UserType { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<UserPortfolio> UserPortfolios { get; set; }
    }

    public class UserProfile
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }

        public virtual User User { get; set; }
    }

    public class UserPortfolio
    {
        public int UserPortfolioId { get; set; }
        public int UserId { get; set; }
        public string Symbol { get; set; }

        public Decimal BuyRate { get; set; }
        public Decimal SaleRate { get; set; }


        public virtual User User { get; set; }
        public virtual Company Company { get; set; }
    }

    public class Company : CompanyAnalysis
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Exch { get; set; }
        public string ExchDisp { get; set; }
        public string Type { get; set; }
        public string TypeDisp { get; set; }
        public virtual ICollection<CompanyOfficer> CompanyOfficers { get; set; }
        public virtual ICollection<HistoricalData> HistoricalDatas { get; set; }
        public virtual CompanyProfile CompanyProfile { get; set; }
        public virtual ICollection<UserPortfolio> UserPortfolios { get; set; }
    }

    public class CompanyAnalysis
    {
        public Decimal IncreaseNextDayIncreasePercentages { get; set; }
        public Decimal IncreaseNextDayDecreasePercentages { get; set; }
        public Decimal IncreaseNextDayNoChangePercentages { get; set; }
        public Decimal DecreaseNextDayIncreasePercentages { get; set; }
        public Decimal DecreaseNextDayDecreasePercentages { get; set; }
        public Decimal DecreaseNextDayNoChangePercentages { get; set; }
        public Decimal NoChangeNextDayIncreasePercentages { get; set; }
        public Decimal NoChangeNextDayDecreasePercentages { get; set; }
        public Decimal NoChangeNextDayNoChangePercentages { get; set; }
    }

    public partial class CompanyOfficer
    {
        public int CompanyOfficerId { get; set; }

        public string Name { get; set; }
        public string Title { get; set; }
        public int? Age { get; set; }
        public int? FiscalYear { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? ExercisedValue { get; set; }
        public decimal? UnexercisedValue { get; set; }

        public string Symbol { get; set; }

        public virtual Company Company { get; set; }
    }

    public partial class CompanyProfile
    {
        public string Symbol { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public string Sector { get; set; }
        public string LongBusinessSummary { get; set; }
        public long? FullTimeEmployees { get; set; }

        public virtual Company Company { get; set; }
    }

    public partial class HistoricalData
    {
        public int HistoricalDataId { get; set; }
        public System.DateTime Date { get; set; }
        public decimal? Open { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public decimal? Change { get; set; }
        public decimal? PercentageChange { get; set; }
        public decimal? Volume { get; set; }
        public decimal? AdjClose { get; set; }

        public string Symbol { get; set; }
        public virtual Company Company { get; set; }
    }

    public partial class CompanyFeeds
    {

    }
}
