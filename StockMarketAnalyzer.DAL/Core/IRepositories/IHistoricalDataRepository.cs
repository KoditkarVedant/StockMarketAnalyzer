using StockMarketAnalyzer.BO;
using StockMarketAnalyzer.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL.Core.IRepositories
{
    public interface IHistoricalDataRepository : IRepository<HistoricalData>
    {
        List<HistoricalData> GetHistoricalData(string ticker);
        List<HistoricalData> GetHistoricalDataFromYahoo(string ticker);
    }
}
