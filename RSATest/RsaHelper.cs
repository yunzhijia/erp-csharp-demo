using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Math;

namespace RSATest
{
    /// <summary>
    /// 参考地址
    /// http://www.cnblogs.com/goodjin/archive/2012/03/30/2426120.html
    /// </summary>
    public static class RSAHelper
    {        
        /// <summary>
        /// 加密数据时加上PKCS1_padding，同java端保持一致
        /// </summary>
        /// <param name="oText"></param>
        /// <param name="blockLen"></param>
        /// <returns></returns>
        private static byte[] add_PKCS1_padding(byte[] oText, int blockLen)
        {
            byte[] result = new byte[blockLen];
            result[0] = 0x00;
            result[1] = 0x01;

            int padLen = blockLen - 3 - oText.Length;
            for (int i = 0; i < padLen; i++)
            {
                result[i + 2] = 0xff;
            }

            result[padLen + 2] = 0x00;

            int j = 0;
            for (int i = padLen + 3; i < blockLen; i++)
            {
                result[i] = oText[j++];
            }

            return result;
        }

        private static byte[] priEncrypt(byte[] block, RSACryptoServiceProvider key)
        {
            RSAParameters param = key.ExportParameters(true);
            BigInteger d = new BigInteger(param.D);
            BigInteger n = new BigInteger(param.Modulus);
            BigInteger biText = new BigInteger(block);
            BigInteger biEnText = biText.modPow(d, n);
            return biEnText.getBytes();
        }
        /// <summary>
        /// 用私匙结合PKCS1_padding加密数据
        /// </summary>
        /// <param name="src"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] encryptByPriKey(string src, RSACryptoServiceProvider key)
        {
            //获得明文字节数组
            byte[] oText = System.Text.Encoding.Default.GetBytes(src);
            //填充
            oText = add_PKCS1_padding(oText, 128);
            //加密
            byte[] result = priEncrypt(oText, key);
            return result;
        }  

    }
}
