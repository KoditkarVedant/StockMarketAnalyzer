using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL.DataModels.ApiModels
{
    public class RootObject
    {
        public Quotesummary QuoteSummary { get; set; }
    }

    public class Quotesummary
    {
        public Result[] Result { get; set; }
        public object Error { get; set; }
    }

    public class Result
    {
        public Assetprofile AssetProfile { get; set; }
        public Calendarevents CalendarEvents { get; set; }
        public Secfilings SecFilings { get; set; }
    }
    public class Assetprofile
    {
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public string IndustrySymbol { get; set; }
        public string Sector { get; set; }
        public string LongBusinessSummary { get; set; }
        public int FullTimeEmployees { get; set; }
        public Companyofficer[] CompanyOfficers { get; set; }
        public int AuditRisk { get; set; }
        public int BoardRisk { get; set; }
        public int CompensationRisk { get; set; }
        public int ShareHolderRightsRisk { get; set; }
        public int OverallRisk { get; set; }
        public int GovernanceEpochDate { get; set; }
        public int CompensationAsOfEpochDate { get; set; }
        public int MaxAge { get; set; }
    }
    public class Companyofficer
    {
        public int MaxAge { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Title { get; set; }
        public int FiscalYear { get; set; }
        public Totalpay TotalPay { get; set; }
        public Exercisedvalue ExercisedValue { get; set; }
        public Unexercisedvalue UnexercisedValue { get; set; }
    }

    public class Totalpay
    {
        public string Raw { get; set; }
        public string Fmt { get; set; }
        public string LongFmt { get; set; }
    }

    public class Exercisedvalue
    {
        public string Raw { get; set; }
        public string Fmt { get; set; }
        public string LongFmt { get; set; }
    }

    public class Unexercisedvalue
    {
        public string Raw { get; set; }
        public string Fmt { get; set; }
        public string LongFmt { get; set; }
    }

    public class Calendarevents
    {
        public int MaxAge { get; set; }
        public Earnings Earnings { get; set; }
        public Exdividenddate ExDividendDate { get; set; }
        public Dividenddate DividendDate { get; set; }
    }

    public class Earnings
    {
        public Earningsdate[] EarningsDate { get; set; }
        public Earningsaverage EarningsAverage { get; set; }
        public Earningslow EarningsLow { get; set; }
        public Earningshigh EarningsHigh { get; set; }
        public Revenueaverage RevenueAverage { get; set; }
        public Revenuelow RevenueLow { get; set; }
        public Revenuehigh RevenueHigh { get; set; }
    }

    public class Earningsaverage
    {
        public float Raw { get; set; }
        public string Fmt { get; set; }
    }

    public class Earningslow
    {
        public float Raw { get; set; }
        public string Fmt { get; set; }
    }

    public class Earningshigh
    {
        public float Raw { get; set; }
        public string Fmt { get; set; }
    }

    public class Revenueaverage
    {
        public long Raw { get; set; }
        public string Fmt { get; set; }
        public string LongFmt { get; set; }
    }

    public class Revenuelow
    {
        public long Raw { get; set; }
        public string Fmt { get; set; }
        public string LongFmt { get; set; }
    }

    public class Revenuehigh
    {
        public long Raw { get; set; }
        public string Fmt { get; set; }
        public string LongFmt { get; set; }
    }

    public class Earningsdate
    {
        public int Raw { get; set; }
        public string Fmt { get; set; }
    }

    public class Exdividenddate
    {
        public int Raw { get; set; }
        public string Fmt { get; set; }
    }

    public class Dividenddate
    {
        public int Raw { get; set; }
        public string Fmt { get; set; }
    }

    public class Secfilings
    {
        public Filing[] Filings { get; set; }
        public int MaxAge { get; set; }
    }

    public class Filing
    {
        public string Date { get; set; }
        public int EpochDate { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string EdgarUrl { get; set; }
        public int MaxAge { get; set; }
    }
}
