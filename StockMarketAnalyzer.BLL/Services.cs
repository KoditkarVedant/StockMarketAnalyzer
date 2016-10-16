using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockMarketAnalyzer.BLL.BusinessModel;
using StockMarketAnalyzer.BLL.Interfaces;
using StockMarketAnalyzer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockMarketAnalyzer.BLL.Mapper;

namespace StockMarketAnalyzer.BLL
{
    public class Services : IServices
    {
        private readonly IDataAccessor _dataAccessor;

        static Services()
        {
            AutoMapperConfiguration.Configure();
        }
        public Services(IDataAccessor dataAccessor)
        {
            _dataAccessor = dataAccessor;
        }

        public CompanyDetail GetCompanyDetails(string ticker)
        {
            var data = _dataAccessor.GetCompanyDetails(ticker);

            if (data != null) return AutoMapperConfiguration.Mapper.Map<CompanyDetail>(data);


            var company = _dataAccessor.GetCompanyDetailYahoo(ticker);
            company.HistoricalDatas = _dataAccessor.GetHistoricalData(ticker);
            
            return AutoMapperConfiguration.Mapper.Map<CompanyDetail>(company);
        }

        public CompanyFeeds GetCompanyFeeds(string ticker)
        {
            throw new NotImplementedException();
        }

        public HistoricalData GetHistoricalData(string ticker)
        {
            throw new NotImplementedException();
        }

        public List<CompanyDetail> GetStockGainer()
        {
            throw new NotImplementedException();
        }

        public List<CompanyDetail> GetStockLooser()
        {
            throw new NotImplementedException();
        }

        public void GetCurrencyRates()
        {
            throw new NotImplementedException();
        }

        public List<CompanyDetail> SearchCompany(string query)
        {
            var data = _dataAccessor.SearchCompany(query);

            var json = JObject.Parse(data);

            var companies = JsonConvert.DeserializeObject<List<CompanyDetail>>(json.GetValue("items").ToString(Formatting.None));

            return companies;
        }
    }
}
