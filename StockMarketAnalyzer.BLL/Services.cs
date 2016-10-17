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

        private CompanyAnalysis CompanyAnalysis(List<DAL.DataModels.HistoricalData> HistoricalDatas)
        {
            int total = HistoricalDatas.Count;

            int increaseNextDayIncrease = 0; // Today Increase Next day Increase
            int increaseNextDayDecrease = 0; // Today Increase Next day Decrease
            int increaseNextDayNoChange = 0; // Today Increase Next day NoChange
            int decreaseNextDayIncrease = 0; // Today Decrease Next day Increase
            int decreaseNextDayDecrease = 0; // Today Decrease Next day Decrease
            int decreaseNextDayNoChange = 0; // Today Decrease Next day NoChange
            int noChangeNextDayIncrease = 0;
            int noChangeNextDayDecrease = 0;
            int noChangeNextDayNoChange = 0;

            for (var i = 0; i < total; i++)
            {
                var TodayHistoricalData = HistoricalDatas[i];

                TodayHistoricalData.Change = TodayHistoricalData.Open - TodayHistoricalData.AdjClose;
                TodayHistoricalData.PercentageChange = TodayHistoricalData.Change * 100.00M / TodayHistoricalData.Open;

                if (i + 1 >= total) break;

                var NextDayHistoricalData = HistoricalDatas[i + 1];
                if (TodayHistoricalData.Change > 0) // increased today
                {
                    if (NextDayHistoricalData.Open > NextDayHistoricalData.Close)
                    {
                        increaseNextDayIncrease++;
                    }
                    else if (NextDayHistoricalData.Open < NextDayHistoricalData.Close)
                    {
                        increaseNextDayDecrease++;
                    }
                    else
                    {
                        increaseNextDayNoChange++;
                    }
                }
                else if (TodayHistoricalData.Change < 0) // decreased today
                {
                    if (NextDayHistoricalData.Open > NextDayHistoricalData.Close)
                    {
                        decreaseNextDayIncrease++;
                    }
                    else if (NextDayHistoricalData.Open < NextDayHistoricalData.Close)
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
                    if (NextDayHistoricalData.Open > NextDayHistoricalData.Close)
                    {
                        noChangeNextDayIncrease++;
                    }
                    else if (NextDayHistoricalData.Open < NextDayHistoricalData.Close)
                    {
                        noChangeNextDayDecrease++;
                    }
                    else
                    {
                        noChangeNextDayNoChange++;
                    }
                }
            }

            decimal increaseNextDayIncreasePercentage = increaseNextDayIncrease * 100.00M / (increaseNextDayIncrease + increaseNextDayDecrease + increaseNextDayNoChange);
            decimal increaseNextDayDecreasePercentage = increaseNextDayDecrease * 100.00M / (increaseNextDayIncrease + increaseNextDayDecrease + increaseNextDayNoChange);
            decimal increaseNextDayNoChangePercentage = increaseNextDayNoChange * 100.00M / (increaseNextDayIncrease + increaseNextDayDecrease + increaseNextDayNoChange);

            decimal decreaseNextDayIncreasePercentage = decreaseNextDayIncrease * 100.00M / (decreaseNextDayIncrease + decreaseNextDayDecrease + decreaseNextDayNoChange);
            decimal decreaseNextDayDecreasePercentage = decreaseNextDayDecrease * 100.00M / (decreaseNextDayIncrease + decreaseNextDayDecrease + decreaseNextDayNoChange);
            decimal decreaseNextDayNoChangePercentage = decreaseNextDayNoChange * 100.00M / (decreaseNextDayIncrease + decreaseNextDayDecrease + decreaseNextDayNoChange);

            decimal noChangeNextDayIncreasePercentage = noChangeNextDayIncrease * 100.00M / (noChangeNextDayIncrease + noChangeNextDayDecrease + noChangeNextDayNoChange);
            decimal noChangeNextDayDecreasePercentage = noChangeNextDayDecrease * 100.00M / (noChangeNextDayIncrease + noChangeNextDayDecrease + noChangeNextDayNoChange);
            decimal noChangeNextDayNoChangePercentage = noChangeNextDayNoChange * 100.00M / (noChangeNextDayIncrease + noChangeNextDayDecrease + noChangeNextDayNoChange);


            var companyAnalysis = new CompanyAnalysis();
            companyAnalysis.IncreaseNextDayIncreasePercentages = increaseNextDayIncreasePercentage;
            companyAnalysis.IncreaseNextDayDecreasePercentages = increaseNextDayDecreasePercentage;
            companyAnalysis.IncreaseNextDayNoChangePercentages = increaseNextDayNoChangePercentage;

            companyAnalysis.DecreaseNextDayIncreasePercentages = decreaseNextDayIncreasePercentage;
            companyAnalysis.DecreaseNextDayDecreasePercentages = decreaseNextDayDecreasePercentage;
            companyAnalysis.DecreaseNextDayNoChangePercentages = decreaseNextDayNoChangePercentage;

            companyAnalysis.NoChangeNextDayIncreasePercentages = noChangeNextDayIncreasePercentage;
            companyAnalysis.NoChangeNextDayDecreasePercentages = noChangeNextDayDecreasePercentage;
            companyAnalysis.NoChangeNextDayNoChangePercentages = noChangeNextDayNoChangePercentage;

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

        public StockMarketAnalyzer.BLL.BusinessModel.HistoricalData GetHistoricalData(string ticker)
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
