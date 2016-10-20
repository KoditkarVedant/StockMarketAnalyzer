using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockMarketAnalyzer.BLL.Interfaces;
using StockMarketAnalyzer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockMarketAnalyzer.DAL.Persitence;
using StockMarketAnalyzer.DAL.Core;
using StockMarketAnalyzer.DAL.DataModels;
using StockMarketAnalyzer.BO;
using System.Xml;

namespace StockMarketAnalyzer.BLL
{
    public class Services : IServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public Services()
        {
            _unitOfWork = new UnitOfWork(new DAL.StockMarketDbContext());
        }

        public Company GetCompanyDetails(string ticker)
        {
            var data = _unitOfWork.Companies.GetCompanyWithTicker(ticker);

            if (data != null)
            {
                data.CompanyFeeds = GetCompanyFeeds(ticker);
                return data;
            }

            var company = _unitOfWork.Companies.GetCompanyFromYahoo(ticker);
            company.HistoricalDatas = _unitOfWork.HistoricalDatas.GetHistoricalDataFromYahoo(ticker);
            company.Symbol = ticker;

            // check this deserialization
            var companyData = SearchCompany(ticker).FirstOrDefault(x => x.Symbol.Equals(ticker));

            company.Name = companyData.Name;
            company.Type = companyData.Type;
            company.TypeDisp = companyData.TypeDisp;
            company.Exch = companyData.Exch;
            company.ExchDisp = companyData.ExchDisp;



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

            company.CompanyFeeds = GetCompanyFeeds(ticker);

            _unitOfWork.Companies.Add(company);
            _unitOfWork.Complete();

            return company;
        }

        private static CompanyAnalysis CompanyAnalysis(List<HistoricalData> historicalDatas)
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

                todayHistoricalData.Change = todayHistoricalData.Open - todayHistoricalData.Close;
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

            var totalIncrease = (increaseNextDayIncrease + increaseNextDayDecrease + increaseNextDayNoChange);
            if (totalIncrease <= 0) totalIncrease = 1;
            var increaseNextDayIncreasePercentage = increaseNextDayIncrease * 100.00M / totalIncrease;
            var increaseNextDayDecreasePercentage = increaseNextDayDecrease * 100.00M / totalIncrease;
            var increaseNextDayNoChangePercentage = increaseNextDayNoChange * 100.00M / totalIncrease;

            var totalDecrease = (decreaseNextDayIncrease + decreaseNextDayDecrease + decreaseNextDayNoChange);
            if (totalDecrease <= 0) totalDecrease = 1;
            var decreaseNextDayIncreasePercentage = decreaseNextDayIncrease * 100.00M / totalDecrease;
            var decreaseNextDayDecreasePercentage = decreaseNextDayDecrease * 100.00M / totalDecrease;
            var decreaseNextDayNoChangePercentage = decreaseNextDayNoChange * 100.00M / totalDecrease;

            var totalNoChange = (noChangeNextDayIncrease + noChangeNextDayDecrease + noChangeNextDayNoChange);
            if (totalNoChange <= 0) totalNoChange = 1;

            var noChangeNextDayIncreasePercentage = noChangeNextDayIncrease * 100.00M / totalNoChange;
            var noChangeNextDayDecreasePercentage = noChangeNextDayDecrease * 100.00M / totalNoChange;
            var noChangeNextDayNoChangePercentage = noChangeNextDayNoChange * 100.00M / totalNoChange;



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

        public List<CompanyFeeds> GetCompanyFeeds(string ticker)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_unitOfWork.Companies.GetCompanyFeeds(ticker));
            // doc.LoadXml(doc.InnerXml);
            var d = doc.SelectNodes("//item");
            List<CompanyFeeds> companyFeeds = new List<CompanyFeeds>(d.Count);
            foreach (XmlNode item in d)
            {
                companyFeeds.Add(new CompanyFeeds()
                {
                    title = item.SelectSingleNode("title").InnerText,
                    link = item.SelectSingleNode("link").InnerText,
                    description = item.SelectSingleNode("description").InnerText,
                    guid = item.SelectSingleNode("guid").InnerText,
                    pubDate = DateTime.Parse(item.SelectSingleNode("pubDate").InnerText)
                });
            }

            return companyFeeds;
        }

        public HistoricalData GetHistoricalData(string ticker)
        {
            throw new NotImplementedException();
        }

        public List<Company> GetStockGainer()
        {
            throw new NotImplementedException();
        }

        public List<Company> GetStockLooser()
        {
            throw new NotImplementedException();
        }

        public void GetCurrencyRates()
        {
            throw new NotImplementedException();
        }

        public List<Company> SearchCompany(string query)
        {
            var data = _unitOfWork.Companies.SearchCompany(query);

            var json = JObject.Parse(data);

            var companies = JsonConvert.DeserializeObject<List<Company>>(json.GetValue("items").ToString(Newtonsoft.Json.Formatting.None));

            return companies;
        }


        public bool UpdateUserProfile(UserProfile profile)
        {
            var profileDM = profile;

            _unitOfWork.Users.UpdateProfile(profileDM);

            return true;
        }


        public UserProfile GetUserProfile(int UserId)
        {
            //var user = new User()
            //{
            //    Username = "Vedant",
            //    EmailAddress = "vedkoditkar@gmail.com",
            //    PhoneNumber = "8412013051",
            //    UserProfile = new UserProfile()
            //    {
            //        FirstName = "Vedant",
            //        LastName = "Koditkar",
            //        Address = "Pune"
            //    }
            //};

            //_unitOfWork.Users.Add(user);
            //_unitOfWork.Complete();

            var profile = _unitOfWork.Users.Get(UserId).UserProfile;

            return profile;
        }

        public bool Authenticate(User user)
        {
            return _unitOfWork.Users.Authenticate(user);
        }

        public bool CheckUserAvailability(User user)
        {
            throw new NotImplementedException();
        }

        public bool RegisterNewUser(Register user)
        {
            return _unitOfWork.Users.Register(user);
        }
    }
}
