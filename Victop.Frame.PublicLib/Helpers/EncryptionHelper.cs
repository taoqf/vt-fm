using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Victop.Frame.PublicLib.Helpers
{
    /// <summary>
    /// 加密辅助类
    /// </summary>
    public static class EncryptionHelper
    {
        private const string desKey = "zzfeidao";
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="pToEncrypt">字符串</param>
        /// <returns></returns>
        public static string DESEncrypt(string pToEncrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
            des.Key = Encoding.UTF8.GetBytes(desKey);
            des.IV = Encoding.UTF8.GetBytes(desKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] EncryptData = (byte[])ms.ToArray();
            return Convert.ToBase64String(EncryptData, 0, EncryptData.Length);
        }
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="pToDecrypt">解密字符串</param>
        /// <returns></returns> 
        public static string DESDecrypt(string pToDecrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            des.Key = Encoding.UTF8.GetBytes(desKey);
            des.IV = Encoding.UTF8.GetBytes(desKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
