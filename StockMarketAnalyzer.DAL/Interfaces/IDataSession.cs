using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL.Interfaces
{
    public interface IDataSession : IDisposable
    {
        IDbSet<T> Set<T>() where T : class;

        // We also obviously need to persist our changes, so:
        void SaveChanges();

        // I also have use cases where I need to drop down to raw SQL, so:
        int SqlCommand(string sql, params object[] parameters);
    }
}
