﻿
using StockMarketAnalyzer.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.BLL.Interfaces
{
    public interface IServices : ILogin
    {

        Company GetCompanyDetails(string ticker);
        List<CompanyFeeds> GetCompanyFeeds(string ticker);
        HistoricalData GetHistoricalData(string ticker);
        List<Company> GetStockGainer();
        List<Company> GetStockLooser();
        void GetCurrencyRates();
        List<Company> SearchCompany(string query);

        bool UpdateUserProfile(UserProfile profile);
        UserProfile GetUserProfile(int UserId);
    }
}
