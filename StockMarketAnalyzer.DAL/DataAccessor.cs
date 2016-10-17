using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockMarketAnalyzer.DAL.DataModels;
using StockMarketAnalyzer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using StockMarketAnalyzer.DAL;
using System.Data.Entity;
using StockMarketAnalyzer.DAL.Helpers;
using StockMarketAnalyzer.DAL.Mapper;
using StockMarketAnalyzer.DAL.DataModels.ApiModels;

namespace StockMarketAnalyzer.DAL
{
    public class DataAccessor : IDataAccessor
    {
        private readonly DbContext _dbContext;

        private readonly StockMarketDbContext _db;

        static DataAccessor()
        {
            AutoMapperConfiguration.Configure();
        }

        public DataAccessor(DbContext dbContect)
        {
            _dbContext = dbContect;
            _db = new StockMarketDbContext();
        }

        //public CompanyDetail GetCompanyDetails(string ticker)
        //{
        //    var data = _dbContext.Set<CompanyDetail>().FirstOrDefault(x => x.Symbol.Equals(ticker));

        //    return data;
        //}

        //public CompanyDetail GetCompanyDetailYahoo(string ticker)
        //{
        //    var company = new CompanyDetail();

        //    var url = string.Format("https://query1.finance.yahoo.com/v10/finance/quoteSummary/{0}?formatted=true&crumb=jtwbxs1X9tZ&lang=en-US&region=US&modules=assetProfile%2CsecFilings%2CcalendarEvents&corsDomain=finance.yahoo.com", ticker);


        //    var companyDetailsJson = _apiHelper.GetResponse(url);

        //    var rootObject = JsonConvert.DeserializeObject<RootObject>(companyDetailsJson);
        //    company.CompanyProfile = AutoMapperConfiguration.Mapper.Map<CompanyProfile>(rootObject.QuoteSummary.Result.FirstOrDefault().AssetProfile);
        //    company.CompanyOfficers = AutoMapperConfiguration.Mapper.Map<List<DataModels.CompanyOfficer>>(rootObject.QuoteSummary.Result.FirstOrDefault().AssetProfile.CompanyOfficers.ToList());

        //    return company;
        //}

        //public bool AddCompany(CompanyDetail entity)
        //{
        //    try
        //    {
        //        _dbContext.Set<CompanyDetail>().Add(entity);
        //        _dbContext.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        //throw;
        //    }
        //    return true;
        //}

        //public string GetCompanyFeeds(string ticker)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<HistoricalData> GetHistoricalData(string ticker)
        //{
        //    var data = new ApiHelper().GetResponse(GetHistoricalDataUrl(ticker, true));

        //    return JsonConvert.DeserializeObject<List<HistoricalData>>(CsvToJsonConverter(data));
        //}

        //private static string GetHistoricalDataUrl(string symbol, bool onlyYesterdayData = false)
        //{
        //    var builder = new StringBuilder();
        //    builder.AppendFormat("http://chart.finance.yahoo.com/table.csv?s={0}", symbol);

        //    if (!onlyYesterdayData) return builder.ToString();

        //    var toDateTime = DateTime.Now;
        //    var fromDateTime = toDateTime.AddDays(-1);

        //    builder.AppendFormat("&a={0}&b={1}&c={2}&d={3}&e={4}&f={5}&g=d&ignore=.csv",
        //        fromDateTime.Month - 1,
        //        fromDateTime.Day,
        //        fromDateTime.Year,
        //        toDateTime.Month - 1,
        //        toDateTime.Day,
        //        toDateTime.Year);

        //    return builder.ToString();

        //}

        //private string GetHistoricalDataUrl(string symbol, DateTime fromPastDateTime, DateTime tillDateTime)
        //{
        //    var builder = new StringBuilder();

        //    builder.AppendFormat("http://chart.finance.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g=d&ignore=.csv",
        //        symbol,
        //        fromPastDateTime.Month - 1,
        //        fromPastDateTime.Day,
        //        fromPastDateTime.Year,
        //        tillDateTime.Month - 1,
        //        tillDateTime.Day,
        //        tillDateTime.Year);

        //    return builder.ToString();
        //}

        //private static string CsvToJsonConverter(string csvString)
        //{
        //    var streamReader = csvString.Split(new[] { "\n" }, StringSplitOptions.None).Select(p => p.Trim()).ToArray();
        //    var delimeter = new[] { ',' };
        //    var keys = streamReader[0].Split(delimeter).Select(k => k.Trim()).ToList();
        //    var stockDetails = new List<Dictionary<string, string>>(streamReader.Count() - 1);

        //    stockDetails.AddRange(from line in streamReader.Skip(1)
        //                          where !string.IsNullOrWhiteSpace(line)
        //                          select line.Trim().Split(delimeter).Select(p => p.Trim()).ToArray()
        //                              into parts
        //                              select new Dictionary<string, string>()
        //        {
        //            {keys[0], parts[0]}, {keys[1], parts[1]}, {keys[2], parts[2]}, {keys[3], parts[3]}, {keys[4], parts[4]}, {keys[5], parts[5]}, {keys[6], parts[6]}
        //        });

        //    //stockDetails.AddRange(streamReader.Skip(1).Select(line => line.Split(delimeter).ToArray()).Select(parts => new Dictionary<string, string>()
        //    //{
        //    //    {keys[0], parts[0]}, {keys[1], parts[1]}, {keys[2], parts[2]}, {keys[3], parts[3]}, {keys[4], parts[4]}, {keys[5], parts[5]}, {keys[6], parts[6]}
        //    //}));

        //    return JsonConvert.SerializeObject(stockDetails);
        //}

        //public string GetStockGainer()
        //{
        //    throw new NotImplementedException();
        //}

        //public string GetStockLooser()
        //{
        //    throw new NotImplementedException();
        //}

        //public string GetCurrencyRates()
        //{
        //    throw new NotImplementedException();
        //}

        //public string SearchCompany(string query)
        //{
        //    const string config = "%7B%22url%22%3A%7B%22host%22%3A%22s.yimg.com%22%2C%22path%22%3A%22%2Fxb%2Fv6%2Ffinance%2Fautocomplete%22%2C%22query%22%3A%7B%22appid%22%3A%22yahoo.com%22%2C%22nresults%22%3A10%2C%22output%22%3A%22yjsonp%22%2C%22region%22%3A%22US%22%2C%22lang%22%3A%22en-US%22%7D%2C%22protocol%22%3A%22https%22%7D%2C%22isJSONP%22%3Atrue%2C%22queryKey%22%3A%22query%22%2C%22resultAccessor%22%3A%22ResultSet.Result%22%2C%22suggestionTitleAccessor%22%3A%22symbol%22%2C%22suggestionMeta%22%3A%5B%22symbol%22%2C%22name%22%2C%22exch%22%2C%22type%22%2C%22exchDisp%22%2C%22typeDisp%22%5D%7D";
        //    var url = "https://finance.yahoo.com/_finance_doubledown/api/resource/searchassist;gossipConfig=" + HttpUtility.UrlDecode(config) + ";searchTerm=" + query; //+ "?bkt=finctrl&device=desktop&feature=&intl=us&lang=en-US&partner=none&region=US&site=finance&tz=Asia/Kolkata&ver=0.101.427&returnMeta=true";

        //    var data = _apiHelper.GetResponse(url);

        //    return data;
        //}
    }
}
