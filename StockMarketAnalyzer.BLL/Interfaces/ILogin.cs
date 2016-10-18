using StockMarketAnalyzer.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.BLL.Interfaces
{
    public interface ILogin
    {
        bool Authenticate(User user);
        bool CheckUserAvailability(User user);
        bool RegisterNewUser(Register user);
    }
}
