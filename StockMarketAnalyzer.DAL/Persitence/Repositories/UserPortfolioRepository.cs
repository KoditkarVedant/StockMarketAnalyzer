using StockMarketAnalyzer.DAL.Core.IRepositories;
using StockMarketAnalyzer.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL.Persitence.Repositories
{
    public class UserPortfolioRepository : Repository<UserPortfolio>, IUserPortfolioRepository
    {
        public UserPortfolioRepository(StockMarketDbContext context)
            : base(context)
        {
        }
    }
}
