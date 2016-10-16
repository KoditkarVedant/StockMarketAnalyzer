using StockMarketAnalyzer.BLL.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.BLL.Interfaces
{
    public interface IServices
    {

        CompanyDetail GetCompanyDetails(string ticker);
        CompanyFeeds GetCompanyFeeds(string ticker);
        HistoricalData GetHistoricalData(string ticker);
        List<CompanyDetail> GetStockGainer();
        List<CompanyDetail> GetStockLooser();
        void GetCurrencyRates();
        List<CompanyDetail> SearchCompany(string query);

    }
}
