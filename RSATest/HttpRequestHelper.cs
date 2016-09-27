using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace RSATest
{
    class HttpRequestHelper
    {
        private static string USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.124 Safari/537.36";
        private static string ACCEPT = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        private static string CONTENT_TYPE = "application/x-www-form-urlencoded; encoding=UTF-8";

        /// <summary>
        /// 单例
        /// </summary>
        private static volatile HttpRequestHelper instance;
        private static object syncRoot = new object();
        private HttpRequestHelper() { }

        public static HttpRequestHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new HttpRequestHelper();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="queryString">url参数</param>
        /// <returns>json格式结果</returns>
        public string Get(string URL)
        {
            string ret = string.Empty;            
            var request = CreateRequest("GET", URL);
            var response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8);
            ret = readStream.ReadToEnd();
            readStream.Close();
            return ret;
        }

        public string GetToken(string URL)
        {
            string ret = string.Empty;
            var request = CreateRequest("GET", URL);
            var response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8);
            ret = readStream.ReadToEnd();
            readStream.Close();
            return ret;
        }

        private HttpWebRequest CreateRequest(string method, string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Accept = ACCEPT;
            request.UserAgent = USER_AGENT;
            request.Method = method;
            request.ContentType = CONTENT_TYPE;
            request.Proxy = null;
            return request;
        }
    }
}
