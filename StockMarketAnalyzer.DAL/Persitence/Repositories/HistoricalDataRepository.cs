using Newtonsoft.Json;
using StockMarketAnalyzer.DAL.Core.IRepositories;
using StockMarketAnalyzer.DAL.DataModels;
using StockMarketAnalyzer.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL.Persitence.Repositories
{
    public class HistoricalDataRepository : Repository<HistoricalData>, IHistoricalDataRepository
    {
        public HistoricalDataRepository(StockMarketDbContext context)
            : base(context)
        {
        }

        public List<HistoricalData> GetHistoricalData(string ticker)
        {
            throw new NotImplementedException();
        }

        public List<HistoricalData> GetHistoricalDataFromYahoo(string ticker)
        {
            var data = ApiHelper.GetResponse(GetHistoricalDataUrl(ticker));

            return JsonConvert.DeserializeObject<List<HistoricalData>>(CsvToJsonConverter(data));
        }

        private static string GetHistoricalDataUrl(string symbol, bool onlyYesterdayData = false)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("http://chart.finance.yahoo.com/table.csv?s={0}", symbol);

            if (!onlyYesterdayData) return builder.ToString();

            var toDateTime = DateTime.Now;
            var fromDateTime = toDateTime.AddDays(-1);

            builder.AppendFormat("&a={0}&b={1}&c={2}&d={3}&e={4}&f={5}&g=d&ignore=.csv",
                fromDateTime.Month - 1,
                fromDateTime.Day,
                fromDateTime.Year,
                toDateTime.Month - 1,
                toDateTime.Day,
                toDateTime.Year);

            return builder.ToString();
        }

        private string GetHistoricalDataUrl(string symbol, DateTime fromPastDateTime, DateTime tillDateTime)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("http://chart.finance.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g=d&ignore=.csv",
                symbol,
                fromPastDateTime.Month - 1,
                fromPastDateTime.Day,
                fromPastDateTime.Year,
                tillDateTime.Month - 1,
                tillDateTime.Day,
                tillDateTime.Year);

            return builder.ToString();
        }

        private static string CsvToJsonConverter(string csvString)
        {
            var streamReader = csvString.Split(new[] { "\n" }, StringSplitOptions.None).Select(p => p.Trim()).ToArray();
            var delimeter = new[] { ',' };
            var keys = streamReader[0].Split(delimeter).Select(k => k.Trim().Replace(" ", "")).ToList();
            var stockDetails = new List<Dictionary<string, string>>(streamReader.Count() - 1);

            stockDetails.AddRange(from line in streamReader.Skip(1)
                                  where !string.IsNullOrWhiteSpace(line)
                                  select line.Trim().Split(delimeter).Select(p => p.Trim()).ToArray()
                                      into parts
                                      select new Dictionary<string, string>()
                {
                    {keys[0], parts[0]}, {keys[1], parts[1]}, {keys[2], parts[2]}, {keys[3], parts[3]}, {keys[4], parts[4]}, {keys[5], parts[5]}, {keys[6], parts[6]}
                });

            //stockDetails.AddRange(streamReader.Skip(1).Select(line => line.Split(delimeter).ToArray()).Select(parts => new Dictionary<string, string>()
            //{
            //    {keys[0], parts[0]}, {keys[1], parts[1]}, {keys[2], parts[2]}, {keys[3], parts[3]}, {keys[4], parts[4]}, {keys[5], parts[5]}, {keys[6], parts[6]}
            //}));

            return JsonConvert.SerializeObject(stockDetails);
        }
    }
}
