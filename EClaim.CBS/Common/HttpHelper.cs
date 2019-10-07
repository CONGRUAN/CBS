using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.CBS.Common
{
    public class HttpHelper
    {
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Post(string url, string param)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            //var data = Encoding.ASCII.GetBytes(param);
            var data = Encoding.GetEncoding("gbk").GetBytes(param);

            request.Method = "POST";
            SetHeaderValue(request.Headers, "Accept", "*/*");
            SetHeaderValue(request.Headers, "Connection", "Keep-Alive");
            SetHeaderValue(request.Headers, "UserAgent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
            SetHeaderValue(request.Headers, "ContentType", "application/x-www-form-urlencoded");
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new System.IO.StreamReader(response.GetResponseStream(), Encoding.Default).ReadToEnd();
            return responseString;
        }

        private static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }
    }
}
