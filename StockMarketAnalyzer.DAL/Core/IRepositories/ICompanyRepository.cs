using StockMarketAnalyzer.BO;
using StockMarketAnalyzer.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL.Core.IRepositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Company GetCompanyWithTicker(string ticker);
        Company GetCompanyFromYahoo(string ticker);
        string SearchCompany(string ticker);
        IEnumerable<Company> GetPopularCompanyInPortfolio(int count);
        string GetCompanyFeeds(string ticker);
    }
}
