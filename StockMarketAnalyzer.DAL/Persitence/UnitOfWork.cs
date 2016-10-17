using StockMarketAnalyzer.DAL.Core;
using StockMarketAnalyzer.DAL.Mapper;
using StockMarketAnalyzer.DAL.Persitence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL.Persitence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StockMarketDbContext _context;

        public UnitOfWork(StockMarketDbContext context)
        {
            _context = context;
            Companies = new CompanyRepository(_context);
            HistoricalDatas = new HistoricalDataRepository(_context);
            Users = new UserRepository(_context);
            UserPortfolios = new UserPortfolioRepository(_context);
        }

        static UnitOfWork()
        {
            AutoMapperConfiguration.Configure();
        }

        public Core.IRepositories.ICompanyRepository Companies { get; private set; }
        public Core.IRepositories.IHistoricalDataRepository HistoricalDatas { get; private set; }
        public Core.IRepositories.IUserPortfolioRepository UserPortfolios { get; private set; }
        public Core.IRepositories.IUserRepository Users { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
