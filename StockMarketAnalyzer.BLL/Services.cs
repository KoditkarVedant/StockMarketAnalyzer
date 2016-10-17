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
using StockMarketAnalyzer.DAL.Persitence;
using StockMarketAnalyzer.DAL.Core;
using StockMarketAnalyzer.DAL.DataModels;

namespace StockMarketAnalyzer.BLL
{
    public class Services : IServices
    {
        private readonly IUnitOfWork _unitOfWork;

        static Services()
        {
            AutoMapperConfiguration.Configure();
        }
        public Services()
        {
            _unitOfWork = new UnitOfWork(new DAL.StockMarketDbContext());
        }

        public CompanyDetail GetCompanyDetails(string ticker)
        {
            var data = _unitOfWork.Companies.GetCompanyWithTicker(ticker);

            if (data != null) return AutoMapperConfiguration.Mapper.Map<CompanyDetail>(data);


            var company = _unitOfWork.Companies.GetCompanyFromYahoo(ticker);
            company.HistoricalDatas = _unitOfWork.HistoricalDatas.GetHistoricalDataFromYahoo(ticker);
            company.Symbol = ticker;

            var companyAnalysis = CompanyAnalysis(company.HistoricalDatas.ToList());

            company.IncreaseNextDayIncreasePercentages = companyAnalysis.IncreaseNextDayIncreasePercentages;
            company.IncreaseNextDayDecreasePercentages = companyAnalysis.IncreaseNextDayDecreasePercentages;
            company.IncreaseNextDayNoChangePercentages = companyAnalysis.IncreaseNextDayNoChangePercentages;

            company.DecreaseNextDayIncreasePercentages = companyAnalysis.DecreaseNextDayIncreasePercentages;
            company.DecreaseNextDayDecreasePercentages = companyAnalysis.DecreaseNextDayDecreasePercentages;
            company.DecreaseNextDayNoChangePercentages = companyAnalysis.DecreaseNextDayNoChangePercentages;

            company.NoChangeNextDayIncreasePercentages = companyAnalysis.NoChangeNextDayIncreasePercentages;
            company.NoChangeNextDayDecreasePercentages = companyAnalysis.NoChangeNextDayDecreasePercentages;
            company.NoChangeNextDayNoChangePercentages = companyAnalysis.NoChangeNextDayNoChangePercentages;

            _unitOfWork.Companies.Add(company);
            _unitOfWork.Complete();
            return AutoMapperConfiguration.Mapper.Map<CompanyDetail>(company);
        }

        private static CompanyAnalysis CompanyAnalysis(List<DAL.DataModels.HistoricalData> historicalDatas)
        {
            var total = historicalDatas.Count;

            var increaseNextDayIncrease = 0; // Today Increase Next day Increase
            var increaseNextDayDecrease = 0; // Today Increase Next day Decrease
            var increaseNextDayNoChange = 0; // Today Increase Next day NoChange
            var decreaseNextDayIncrease = 0; // Today Decrease Next day Increase
            var decreaseNextDayDecrease = 0; // Today Decrease Next day Decrease
            var decreaseNextDayNoChange = 0; // Today Decrease Next day NoChange
            var noChangeNextDayIncrease = 0;
            var noChangeNextDayDecrease = 0;
            var noChangeNextDayNoChange = 0;

            for (var i = 0; i < total; i++)
            {
                var todayHistoricalData = historicalDatas[i];

                todayHistoricalData.Change = todayHistoricalData.Open - todayHistoricalData.AdjClose;
                todayHistoricalData.PercentageChange = todayHistoricalData.Change * 100.00M / todayHistoricalData.Open;

                if (i + 1 >= total) break;

                var nextDayHistoricalData = historicalDatas[i + 1];
                if (todayHistoricalData.Change > 0) // increased today
                {
                    if (nextDayHistoricalData.Open > nextDayHistoricalData.Close)
                    {
                        increaseNextDayIncrease++;
                    }
                    else if (nextDayHistoricalData.Open < nextDayHistoricalData.Close)
                    {
                        increaseNextDayDecrease++;
                    }
                    else
                    {
                        increaseNextDayNoChange++;
                    }
                }
                else if (todayHistoricalData.Change < 0) // decreased today
                {
                    if (nextDayHistoricalData.Open > nextDayHistoricalData.Close)
                    {
                        decreaseNextDayIncrease++;
                    }
                    else if (nextDayHistoricalData.Open < nextDayHistoricalData.Close)
                    {
                        decreaseNextDayDecrease++;
                    }
                    else
                    {
                        decreaseNextDayNoChange++;
                    }
                }
                else
                {
                    if (nextDayHistoricalData.Open > nextDayHistoricalData.Close)
                    {
                        noChangeNextDayIncrease++;
                    }
                    else if (nextDayHistoricalData.Open < nextDayHistoricalData.Close)
                    {
                        noChangeNextDayDecrease++;
                    }
                    else
                    {
                        noChangeNextDayNoChange++;
                    }
                }
            }

            var increaseNextDayIncreasePercentage = increaseNextDayIncrease * 100.00M / (increaseNextDayIncrease + increaseNextDayDecrease + increaseNextDayNoChange);
            var increaseNextDayDecreasePercentage = increaseNextDayDecrease * 100.00M / (increaseNextDayIncrease + increaseNextDayDecrease + increaseNextDayNoChange);
            var increaseNextDayNoChangePercentage = increaseNextDayNoChange * 100.00M / (increaseNextDayIncrease + increaseNextDayDecrease + increaseNextDayNoChange);

            var decreaseNextDayIncreasePercentage = decreaseNextDayIncrease * 100.00M / (decreaseNextDayIncrease + decreaseNextDayDecrease + decreaseNextDayNoChange);
            var decreaseNextDayDecreasePercentage = decreaseNextDayDecrease * 100.00M / (decreaseNextDayIncrease + decreaseNextDayDecrease + decreaseNextDayNoChange);
            var decreaseNextDayNoChangePercentage = decreaseNextDayNoChange * 100.00M / (decreaseNextDayIncrease + decreaseNextDayDecrease + decreaseNextDayNoChange);

            var noChangeNextDayIncreasePercentage = noChangeNextDayIncrease * 100.00M / (noChangeNextDayIncrease + noChangeNextDayDecrease + noChangeNextDayNoChange);
            var noChangeNextDayDecreasePercentage = noChangeNextDayDecrease * 100.00M / (noChangeNextDayIncrease + noChangeNextDayDecrease + noChangeNextDayNoChange);
            var noChangeNextDayNoChangePercentage = noChangeNextDayNoChange * 100.00M / (noChangeNextDayIncrease + noChangeNextDayDecrease + noChangeNextDayNoChange);


            var companyAnalysis = new CompanyAnalysis
            {
                IncreaseNextDayIncreasePercentages = increaseNextDayIncreasePercentage,
                IncreaseNextDayDecreasePercentages = increaseNextDayDecreasePercentage,
                IncreaseNextDayNoChangePercentages = increaseNextDayNoChangePercentage,
                DecreaseNextDayIncreasePercentages = decreaseNextDayIncreasePercentage,
                DecreaseNextDayDecreasePercentages = decreaseNextDayDecreasePercentage,
                DecreaseNextDayNoChangePercentages = decreaseNextDayNoChangePercentage,
                NoChangeNextDayIncreasePercentages = noChangeNextDayIncreasePercentage,
                NoChangeNextDayDecreasePercentages = noChangeNextDayDecreasePercentage,
                NoChangeNextDayNoChangePercentages = noChangeNextDayNoChangePercentage
            };



            //            StringBuilder json = new StringBuilder("{");
            //            json.AppendFormat(@"IncreaseNextDayIncreasePercentages = {0}, 
            //                                            IncreaseNextDayDecreasePercentages = {1},
            //                                            IncreaseNextDayNoChangePercentages = {2},
            //                                            DecreaseNextDayIncreasePercentages = {3},
            //                                            DecreaseNextDayDecreasePercentages = {4},
            //                                            DecreaseNextDayNoChangePercentages = {5},
            //                                            NoChangeNextDayIncreasePercentages = {6},
            //                                            NoChangeNextDayDecreasePercentages = {7},
            //                                            NoChangeNextDayNoChangePercentages = {8}",
            //                                            increaseNextDayIncreasePercentage,
            //                                            increaseNextDayDecreasePercentage,
            //                                            increaseNextDayNoChangePercentage,
            //                                            decreaseNextDayIncreasePercentage,
            //                                            decreaseNextDayDecreasePercentage,
            //                                            decreaseNextDayNoChangePercentage,
            //                                            noChangeNextDayIncreasePercentage,
            //                                            noChangeNextDayDecreasePercentage,
            //                                            noChangeNextDayNoChangePercentage);
            //            json.Append("}");

            return companyAnalysis;
        }

        public CompanyFeeds GetCompanyFeeds(string ticker)
        {
            throw new NotImplementedException();
        }

        public BLL.BusinessModel.HistoricalData GetHistoricalData(string ticker)
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
            var data = _unitOfWork.Companies.SearchCompany(query);

            var json = JObject.Parse(data);

            var companies = JsonConvert.DeserializeObject<List<CompanyDetail>>(json.GetValue("items").ToString(Formatting.None));

            return companies;
        }
    }
}
