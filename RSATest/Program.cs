using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using Org.BouncyCastle.Crypto.Parameters;

namespace RSATest
{
    class Program
    {
        private static void Main(string[] args)
        {
            string aesKey = "461c32501a4b11e5";
            string content = "{\"eid\":\"4494827\"}";
            string privateKey = AppDomain.CurrentDomain.BaseDirectory + "4494827.key";            
            using (System.IO.FileStream fs = System.IO.File.OpenRead(privateKey))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);//读取文件流
                string pKey = KeyConverter.RSAPrivateKeyJava2DotNet(Convert.ToBase64String(data));//转换java的privatekey为c#的key                   
                RSACryptoServiceProvider rsaCsp = new RSACryptoServiceProvider();
                rsaCsp.FromXmlString(pKey);
                byte[] encrytBytes = RSAHelper.encryptByPriKey(aesKey, rsaCsp);   //RSA加密             
                string encContent = AesUtil.Encrypt(content, aesKey);                //AES加密
                byte[] encContents = Convert.FromBase64String(encContent);
                int len = encrytBytes.Length + encContents.Length;
                byte[] lenArr = new byte[len];
                encrytBytes.CopyTo(lenArr, 0);
                encContents.CopyTo(lenArr, encrytBytes.Length);//把加密后的内容组装在一起
                //string resultStr = System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(lenArr));                //url加密
                //string resultcontent = "nonce=461c32501a4b11e5&eid=4494827&data=" + resultStr ;
                //Http Post请求
                string url = "https://www.yunzhijia.com/openaccess/input/dept/getall";
                var httpClient = new HttpClient(); 
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                paramList.Add(new KeyValuePair<string, string>("nonce", aesKey));
                paramList.Add(new KeyValuePair<string, string>("eid", eid));
                paramList.Add(new KeyValuePair<string, string>("data", Convert.ToBase64String(lenArr)));
                var result = httpClient.PostAsync(url, new FormUrlEncodedContent(paramList)).Result.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
