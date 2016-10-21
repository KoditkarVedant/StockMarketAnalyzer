using StockMarketAnalyzer.BO;
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
        public StockMarketDbContext StockMarketDbContext
        {
            get { return Context as StockMarketDbContext; }
        }

        public List<UserPortfolio> GetAll(int id)
        {
            return StockMarketDbContext.UserPortfolios.Where(x => x.UserId == id).ToList();
        }
    }
}
