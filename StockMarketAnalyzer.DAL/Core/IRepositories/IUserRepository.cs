using StockMarketAnalyzer.BO;
using StockMarketAnalyzer.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL.Core.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        void UpdateProfile(UserProfile profile);
        bool Authenticate(User user);
        bool Register(Register user);

        int getUserId(string p);

        UserType getUserRole(string p);
    }
}
