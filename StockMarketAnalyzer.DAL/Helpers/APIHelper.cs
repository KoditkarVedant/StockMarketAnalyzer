using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL.Helpers
{
    public interface IApiHelper
    {
        string GetResponse(string url, string method = "GET", Dictionary<string, string> param = null);
    }

    public class ApiHelper : IApiHelper
    {
        /// <summary>
        /// this is method makes the call and return the reponse
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetResponse(string url, string method = "GET", Dictionary<string, string> param = null)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    var request = System.Net.WebRequest.Create(url);

                    using (var response = request.GetResponse() as HttpWebResponse)
                    {
                        var reader = new StreamReader(response.GetResponseStream());

                        var vals = reader.ReadToEnd();

                        return vals;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }
    }
}
