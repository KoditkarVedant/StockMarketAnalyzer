using StockMarketAnalyzer.DAL.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository Companies { get; }
        IHistoricalDataRepository HistoricalDatas { get; }
        IUserPortfolioRepository UserPortfolios { get; }
        IUserRepository Users { get; }

        int Complete();
    }
}
