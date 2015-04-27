using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace HYUtility
{
   public class CryptographyHelper
    {
        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="str">要加密的string</param>
        /// <returns>密文</returns>
        public static string MD5Encrypt(string str)
        {
            string pwd = null;
            MD5 m = MD5.Create();
            byte[] s = m.ComputeHash(Encoding.Unicode.GetBytes(str));
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }
        /// <summary>
        ///  Md5加密
        /// </summary>
        /// <param name="pToEncrypt">要加密的string</param>
        /// <param name="sKey">要加密的key</param>
        /// <returns></returns>
        public static string MD5Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write); cs.Write(inputByteArray, 0, inputByteArray.Length); cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }
        //MD5解密 
        /// <summary>
        ///  Md5解密
        /// </summary>
        /// <param name="pToEncrypt">解密string</param>
        /// <param name="sKey">解密key(要8位数)</param>
        /// <returns></returns>
        public static string MD5Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

    }


   /// <summary>
   /// 类名：HashEncrypt
   /// 作用：对传入的字符串进行Hash运算，返回通过Hash算法加密过的字串。
   /// 属性：［无］
   /// 构造函数额参数：
   /// IsReturnNum:是否返回为加密后字符的Byte代码
   /// IsCaseSensitive：是否区分大小写。
   /// 方法：此类提供MD5，SHA1，SHA256，SHA512等四种算法，加密字串的长度依次增大。
   /// </summary>
   public class HashEncrypt
   {
       //private string strIN;
       private bool isReturnNum;
       private bool isCaseSensitive;

       /// <summary>
       /// 类初始化，此类提供MD5，SHA1，SHA256，SHA512等四种算法，加密字串的长度依次增大。
       /// </summary>
       /// <param name="IsCaseSensitive">是否区分大小写</param>
       /// <param name="IsReturnNum">是否返回为加密后字符的Byte代码</param>
       public HashEncrypt(bool IsCaseSensitive, bool IsReturnNum)
       {
           this.isReturnNum = IsReturnNum;
           this.isCaseSensitive = IsCaseSensitive;
       }

       private string getstrIN(string strIN)
       {
           //string strIN = strIN;
           if (strIN.Length == 0)
           {
               strIN = "~NULL~";
           }
           if (isCaseSensitive == false)
           {
               strIN = strIN.ToUpper();
           }
           return strIN;
       }

       public string MD5Encrypt(string strIN)
       {
           //string strIN = getstrIN(strIN);
           byte[] tmpByte;
           MD5 md5 = new MD5CryptoServiceProvider();
           tmpByte = md5.ComputeHash(GetKeyByteArray(getstrIN(strIN)));
           md5.Clear();

           return GetStringValue(tmpByte);

       }

       public string SHA1Encrypt(string strIN)
       {
           //string strIN = getstrIN(strIN);
           byte[] tmpByte;
           SHA1 sha1 = new SHA1CryptoServiceProvider();

           tmpByte = sha1.ComputeHash(GetKeyByteArray(strIN));
           sha1.Clear();

           return GetStringValue(tmpByte);

       }

       public string SHA256Encrypt(string strIN)
       {
           //string strIN = getstrIN(strIN);
           byte[] tmpByte;
           SHA256 sha256 = new SHA256Managed();

           tmpByte = sha256.ComputeHash(GetKeyByteArray(strIN));
           sha256.Clear();

           return GetStringValue(tmpByte);

       }

       public string SHA512Encrypt(string strIN)
       {
           //string strIN = getstrIN(strIN);
           byte[] tmpByte;
           SHA512 sha512 = new SHA512Managed();

           tmpByte = sha512.ComputeHash(GetKeyByteArray(strIN));
           sha512.Clear();

           return GetStringValue(tmpByte);

       }

       /// <summary>
       /// 使用DES加密（Added by niehl 2005-4-6）
       /// </summary>
       /// <param name="originalValue">待加密的字符串</param>
       /// <param name="key">密钥(最大长度8)</param>
       /// <param name="IV">初始化向量(最大长度8)</param>
       /// <returns>加密后的字符串</returns>
       public string DESEncrypt(string originalValue, string key, string IV)
       {
           //将key和IV处理成8个字符
           key += "12345678";
           IV += "12345678";
           key = key.Substring(0, 8);
           IV = IV.Substring(0, 8);

           SymmetricAlgorithm sa;
           ICryptoTransform ct;
           MemoryStream ms;
           CryptoStream cs;
           byte[] byt;

           sa = new DESCryptoServiceProvider();
           sa.Key = Encoding.UTF8.GetBytes(key);
           sa.IV = Encoding.UTF8.GetBytes(IV);
           ct = sa.CreateEncryptor();

           byt = Encoding.UTF8.GetBytes(originalValue);

           ms = new MemoryStream();
           cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
           cs.Write(byt, 0, byt.Length);
           cs.FlushFinalBlock();

           cs.Close();

           return Convert.ToBase64String(ms.ToArray());
       }

       public string DESEncrypt(string originalValue, string key)
       {
           return DESEncrypt(originalValue, key, key);
       }

       /// <summary>
       /// 使用DES解密（Added by niehl 2005-4-6）
       /// </summary>
       /// <param name="encryptedValue">待解密的字符串</param>
       /// <param name="key">密钥(最大长度8)</param>
       /// <param name="IV">m初始化向量(最大长度8)</param>
       /// <returns>解密后的字符串</returns>
       public string DESDecrypt(string encryptedValue, string key, string IV)
       {
           //将key和IV处理成8个字符
           key += "12345678";
           IV += "12345678";
           key = key.Substring(0, 8);
           IV = IV.Substring(0, 8);

           SymmetricAlgorithm sa;
           ICryptoTransform ct;
           MemoryStream ms;
           CryptoStream cs;
           byte[] byt;

           sa = new DESCryptoServiceProvider();
           sa.Key = Encoding.UTF8.GetBytes(key);
           sa.IV = Encoding.UTF8.GetBytes(IV);
           ct = sa.CreateDecryptor();

           byt = Convert.FromBase64String(encryptedValue);

           ms = new MemoryStream();
           cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
           cs.Write(byt, 0, byt.Length);
           cs.FlushFinalBlock();

           cs.Close();

           return Encoding.UTF8.GetString(ms.ToArray());

       }

       public string DESDecrypt(string encryptedValue, string key)
       {
           return DESDecrypt(encryptedValue, key, key);
       }

       private string GetStringValue(byte[] Byte)
       {
           string tmpString = "";
           if (this.isReturnNum == false)
           {
               ASCIIEncoding Asc = new ASCIIEncoding();
               tmpString = Asc.GetString(Byte);
           }
           else
           {
               int iCounter;
               for (iCounter = 0; iCounter < Byte.Length; iCounter++)
               {
                   tmpString = tmpString + Byte[iCounter].ToString();
               }
           }
           return tmpString;
       }

       private byte[] GetKeyByteArray(string strKey)
       {

           ASCIIEncoding Asc = new ASCIIEncoding();

           int tmpStrLen = strKey.Length;
           byte[] tmpByte = new byte[tmpStrLen - 1];

           tmpByte = Asc.GetBytes(strKey);

           return tmpByte;

       }

   }


   public class DESEncrypt
   {
       /// <summary>
       /// 构造方法
       /// </summary>
       public DESEncrypt()
       {

       }
       //默认密钥向量
       private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
       //密钥
       public static string KeyValue = "MJTPHYXT";
       /// DES加密
       /// <param >待加密的字符串</param>
       /// <param >加密密钥,要求为8位</param>
       /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
       public static string Encrypt(string encryptString)
       {
           try
           {
               string encryptKey = KeyValue;
               byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
               byte[] rgbIV = Keys;
               byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
               DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
               MemoryStream mStream = new MemoryStream();
               CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
               cStream.Write(inputByteArray, 0, inputByteArray.Length);
               cStream.FlushFinalBlock();
               Encoding ed = Encoding.UTF8;
               return Convert.ToBase64String(ed.GetBytes(Convert.ToBase64String(mStream.ToArray())));
           }
           catch
           {
               //return encryptString;
               return "Encrypt Failed!";
           }
       }
       /// DES解密
       /// <param >待解密的字符串</param>
       /// <param >解密密钥,要求为8位,和加密密钥相同</param>
       /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
       public static string Decrypt(string decryptString)
       {
           try
           {
               Encoding ed = Encoding.UTF8;
               decryptString = ed.GetString(Convert.FromBase64String(decryptString));
               string decryptKey = KeyValue;
               byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
               byte[] rgbIV = Keys;
               byte[] inputByteArray = Convert.FromBase64String(decryptString);
               DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
               MemoryStream mStream = new MemoryStream();
               CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
               cStream.Write(inputByteArray, 0, inputByteArray.Length);
               cStream.FlushFinalBlock();
               return Encoding.UTF8.GetString(mStream.ToArray());
           }
           catch
           {
               //return decryptString;
               return "Decrypt Failed!";
           }
       }
   }
}
