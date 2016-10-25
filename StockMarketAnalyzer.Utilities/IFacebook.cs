using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockMarketAnalyzer.Utilities
{
    public interface IFacebook
    {
        object GetFacebookHandle(string query);
        object GetFacebookPosts(string handle);
        object GetFacebookPostsTagged(string handle);
        object GetMoreFacebookHandles(string query);
        object GetMoreFacebookPosts(string query);
        object GetSpecificFacebookHandle(string handle);
    }
}
