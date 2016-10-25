using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.Utilities
{
    public class FacebookWrapper : IFacebook
    {
        private readonly string _clientID;
        private readonly string _clientSecret;
        private readonly string _appAccessToken;
        private readonly ResourceManager _resourceManager;


        //static constuctor to initialize clientID,clientSecret,appAccessToken
        public FacebookWrapper()
        {
            Assembly assembly = this.GetType().Assembly;
            _resourceManager = new ResourceManager("StockMarketAnalyzer.Utilities.Properties.Resources", assembly);


            _clientID = _resourceManager.GetString("FacebookClientId");
            _clientSecret = _resourceManager.GetString("FacebookClientSecret"); // "500e5031aa34c0e8ee2b2d1a3c8bee77";
            _appAccessToken = _resourceManager.GetString("FacebookAppAccessToken");

            //get the token from the facebook
            _appAccessToken = GetAccessToken(_clientID, _clientSecret) ?? _appAccessToken;
        }

        /// <summary>
        /// Returns App Access Token for given client id and client secret
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="ClientSecret"></param>
        /// <returns></returns>
        private string GetAccessToken(string ClientID, string ClientSecret)
        {
            Dictionary<string, string> tokens = new Dictionary<string, string>();

            string getAccessTokenUrl = "https://graph.facebook.com/oauth/access_token?client_id=" + ClientID + "&client_secret=" + ClientSecret + "&grant_type=client_credentials";
            string AccessToken = null;

            string response = GetResponse(getAccessTokenUrl);
            if (!string.IsNullOrWhiteSpace(response))
            {
                try
                {
                    foreach (string token in response.Split('&'))
                    {
                        tokens.Add(token.Substring(0, token.IndexOf("=")),
                            token.Substring(token.IndexOf("=") + 1, token.Length - token.IndexOf("=") - 1));
                    }
                    AccessToken = tokens["access_token"];
                    return AccessToken;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// this is method makes the call and return the reponse
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetResponse(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                    
                    request.Proxy = new System.Net.WebProxy("10.0.10.169", 8080);

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream());

                        string vals = reader.ReadToEnd();

                        return vals;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// return the json object first 10 FB Handles
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public object GetFacebookHandle(string query)
        {
            string data = null;

            if (!string.IsNullOrWhiteSpace(query))
            {
                string url = "https://graph.facebook.com/search?access_token=" + _appAccessToken + "&q=" + query + "&type=page&fields=id,name,picture,link";

                data = GetResponse(url);
            }
            return data;
        }

        /// <summary>
        /// return the json object first 10 FB Posts
        /// </summary>
        /// <param name="AccessToken">App Access Token</param>
        /// <param name="handle">Facebook Handle</param>
        /// <returns></returns>
        public object GetFacebookPosts(string handle)
        {
            string data = null;

            if (!string.IsNullOrWhiteSpace(handle))
            {
                string url = "https://graph.facebook.com/" + handle + "/posts?fields=permalink_url,id,message&access_token=" + _appAccessToken;

                data = GetResponse(url);
            }
            return data;
        }

        /// <summary>
        /// return the json object first 10 FB Posts in which the person/bussiness is tagged
        /// </summary>
        /// <param name="AccessToken">App Access Token</param>
        /// <param name="handle">Facebook Handle</param>
        /// <returns></returns>
        public object GetFacebookPostsTagged(string handle)
        {
            string data = null;

            if (!string.IsNullOrWhiteSpace(handle))
            {
                string url = "https://graph.facebook.com/" + handle + "/tagged?fields=permalink_url,id,message&access_token=" + _appAccessToken + "&format=json";

                data = GetResponse(url);
            }
            return data;
        }

        public object GetMoreFacebookHandles(string query)
        {
            string data = null;
            if (!string.IsNullOrWhiteSpace(query))
            {
                data = GetResponse(query);
            }
            return data;
        }

        public object GetMoreFacebookPosts(string query)
        {
            string data = null;
            if (!string.IsNullOrWhiteSpace(query))
            {
                data = GetResponse(query);
            }
            return data;
        }

        public object GetSpecificFacebookHandle(string handle)
        {
            string data = null;

            if (!string.IsNullOrWhiteSpace(handle))
            {
                string url = "https://graph.facebook.com/" + handle + "?access_token=" + _appAccessToken + "&fields=id,name,picture,link";
                data = GetResponse(url);
            }
            return data;
        }
    }
}
