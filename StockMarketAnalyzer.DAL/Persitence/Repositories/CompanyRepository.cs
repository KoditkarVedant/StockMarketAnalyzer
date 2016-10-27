using Newtonsoft.Json;
using StockMarketAnalyzer.BO;
using StockMarketAnalyzer.DAL.Core.IRepositories;
using StockMarketAnalyzer.DAL.DataModels;
using StockMarketAnalyzer.DAL.DataModels.ApiModels;
using StockMarketAnalyzer.DAL.Helpers;
using StockMarketAnalyzer.DAL.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StockMarketAnalyzer.DAL.Persitence.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(StockMarketDbContext context)
            : base(context)
        {
        }

        public StockMarketDbContext StockMarketDbContext
        {
            get { return Context as StockMarketDbContext; }
        }

        public Company GetCompanyWithTicker(string ticker)
        {
            return StockMarketDbContext.Companies.FirstOrDefault(x => x.Symbol == ticker);
        }

        public IEnumerable<Company> GetPopularCompanyInPortfolio(int count)
        {
            return StockMarketDbContext.Companies.OrderByDescending(x => x.UserPortfolios.Count).Take(count).ToList();
        }

        public Company GetCompanyFromYahoo(string ticker)
        {
            var company = new Company();

            var url = string.Format("https://query1.finance.yahoo.com/v10/finance/quoteSummary/{0}?formatted=true&crumb=jtwbxs1X9tZ&lang=en-US&region=US&modules=assetProfile%2CsecFilings%2CcalendarEvents&corsDomain=finance.yahoo.com", ticker);


            var companyDetailsJson = ApiHelper.GetResponse(url);

            var rootObject = JsonConvert.DeserializeObject<RootObject>(companyDetailsJson);
            company.CompanyProfile = AutoMapperConfiguration.Mapper.Map<CompanyProfile>(rootObject.QuoteSummary.Result.FirstOrDefault().AssetProfile);
            company.CompanyOfficers = AutoMapperConfiguration.Mapper.Map<List<CompanyOfficer>>(rootObject.QuoteSummary.Result.FirstOrDefault().AssetProfile.CompanyOfficers.ToList());

            return company;
        }

        public string SearchCompany(string query)
        {
            const string config = "%7B%22url%22%3A%7B%22host%22%3A%22s.yimg.com%22%2C%22path%22%3A%22%2Fxb%2Fv6%2Ffinance%2Fautocomplete%22%2C%22query%22%3A%7B%22appid%22%3A%22yahoo.com%22%2C%22nresults%22%3A10%2C%22output%22%3A%22yjsonp%22%2C%22region%22%3A%22US%22%2C%22lang%22%3A%22en-US%22%7D%2C%22protocol%22%3A%22https%22%7D%2C%22isJSONP%22%3Atrue%2C%22queryKey%22%3A%22query%22%2C%22resultAccessor%22%3A%22ResultSet.Result%22%2C%22suggestionTitleAccessor%22%3A%22symbol%22%2C%22suggestionMeta%22%3A%5B%22symbol%22%2C%22name%22%2C%22exch%22%2C%22type%22%2C%22exchDisp%22%2C%22typeDisp%22%5D%7D";
            var url = "https://finance.yahoo.com/_finance_doubledown/api/resource/searchassist;gossipConfig=" + HttpUtility.UrlDecode(config) + ";searchTerm=" + query; //+ "?bkt=finctrl&device=desktop&feature=&intl=us&lang=en-US&partner=none&region=US&site=finance&tz=Asia/Kolkata&ver=0.101.427&returnMeta=true";

            var data = ApiHelper.GetResponse(url);

            return data;
        }

        public string GetCompanyFeeds(string ticker)
        {
            string url = "http://finance.yahoo.com/rss/industry?s=" + ticker;

            var data = ApiHelper.GetResponse(url);

            return data;
        }

        public IEnumerable<Company> GetCompaniesWithoutHandle()
        {
            return StockMarketDbContext.Companies.Where(x => (x.CompanyProfile != null && x.CompanyProfile.FBHandle == null)).ToList();
        }
    }
}
