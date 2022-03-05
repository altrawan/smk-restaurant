using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SMK_Restaurant
{
    class Class
    {
        public static string orderid;
        public static string total;

        //this function Convert password to MD5 
        public string Hash(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            UTF8Encoding encode = new UTF8Encoding();
            //encrypt the given password string into Encrypted data  
            byte[] encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
        }

        //this function to check email address
        public bool IsValidEmail(string email)
        {
            //This Pattern is use to verify the email
            string strRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            Regex re = new Regex(strRegex, RegexOptions.IgnoreCase);

            if (re.IsMatch(email))
                return (true);
            else
                return (false);
        }

        //this function to check correct format password
        public bool IsValidPassword(string password)
        {
            string strRegex = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,9}$";
            Regex r = new Regex(strRegex);
            if (r.IsMatch(password))
                return (true);
            else
                return (false);
        }

        
    }
}
