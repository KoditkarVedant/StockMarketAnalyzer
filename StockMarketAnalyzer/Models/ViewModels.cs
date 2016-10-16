using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockMarketAnalyzer.Models
{
    public class CompanyDetail
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

        public virtual CompanyDetail CompanyDetail { get; set; }
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

        public virtual CompanyDetail CompanyDetail { get; set; }
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
        public virtual CompanyDetail CompanyDetail { get; set; }
    }
    public class CompanyFeeds
    {
        string Title;
        string Link;
        string Description;
        string Guid;
        DateTime PubDate;
    }
}