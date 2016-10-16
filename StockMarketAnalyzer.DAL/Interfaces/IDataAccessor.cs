using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockMarketAnalyzer.DAL.DataModels;

namespace StockMarketAnalyzer.DAL.Interfaces
{
    public interface IDataAccessor
    {
        string SearchCompany(string query);
        CompanyDetail GetCompanyDetails(string ticker);
        string GetCompanyFeeds(string ticker);
        List<HistoricalData> GetHistoricalData(string ticker);
        string GetStockGainer();
        string GetStockLooser();
        string GetCurrencyRates();
        CompanyDetail GetCompanyDetailYahoo(string ticker);
    }
}
