using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StockMarketAnalyzer.DAL.Interfaces
{
    public class DataSession : IDataSession
    {
        // This is the EF class, not ours:
        private readonly DbContext _dbContext;

        public DataSession(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IDbSet<T> Set<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public void SaveChanges()
        {
            var x = _dbContext.SaveChanges();
        }

        public int SqlCommand(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }
    }
}
