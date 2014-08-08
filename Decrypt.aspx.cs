using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WordSoapWF01
{
    public partial class Decrypt : System.Web.UI.Page
    {
        Utilities utilities = new Utilities();

        #region - Accessors -

        protected Guid _UserGUID { get; set; }

        protected string _IPAddress { get; set; }

        protected string _Action { get; set; }

        protected string _Key { get; set; }
        protected string _Key_rm { get; set; }
        protected string _Key_ck { get; set; }

        protected string _StrToDecrypt { get; set; }
        protected string _StrToDecrypt_rm { get; set; }
        protected string _StrToDecrypt_ck { get; set; }

        protected string _ClearText { get; set; }
        //public string _ClearText_rm { get; set; }
        //public string _ClearText_ck { get; set; }

        protected bool _IsValid { get; set; }

        protected string _Device = "Web";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region - Check if the user is authenticated -

            // Temp GUID 135CB7C8-8FAA-43F0-956E-B99CCC92C8D0

            //if (!Request.IsAuthenticated) // Not being implemented
            //{
            //    //string loginPage = ConfigurationManager.AppSettings["AuthenticationChallengeRedirect"];
            //    //Response.Redirect(loginPage);
            //    btnScramble.Enabled = false;
            //    lblInfo.Text = "Please Login or Sign Up to Unscramble";
            //}
            //else
            //{
            //    MembershipUser myObject = Membership.GetUser();
            //    _UserGUID = (Guid)myObject.ProviderUserKey;
            //    btnScramble.Enabled = true;
            //    lblInfo.Text = String.Empty;
            //}
            #endregion
        }

        protected void btnUnscramble_Click(object sender, EventArgs e)
        {
            this.ValidateFormEntries_Decrypt();
        }

        private void ValidateFormEntries_Decrypt()
        {
            int count = 0;

            #region - tbxKeyword - Key1 -

            if (tbxKeyword.Text == String.Empty)
            {
                tbxKeyword.CssClass = "textboxYellow";
                lblKeyword.Text = "*";
            }
            else
            {
                _Key = tbxKeyword.Text;
                _Key_rm = utilities.StripSpaces(_Key);

                // Test for SQL Injection Test
                bool injectionTest = utilities.TestSearchStringForSQLInjection(_IPAddress, _Key_rm);
                if (injectionTest) // SQL Injection Detected
                {
                    _Key_ck = String.Empty;
                    this.Send_SQLInjection_Notification_Email(_IPAddress);
                    // Noting else happens
                }
                else // SQL Injection NOT Detected
                {
                    _Key_ck = _Key;
                    tbxKeyword.CssClass = "textbox";
                    //tbxKeyword.Text = String.Empty;
                    count++;
                }
            }

            #endregion

            #region - tbxTextEntry - StrToDecrypt -

            if (tbxTextEntry.Text == String.Empty)
            {
                tbxTextEntry.CssClass = "textboxYellow";
                lblTextEntry.Text = "*";
            }
            else
            {
                _StrToDecrypt = tbxTextEntry.Text;
                _StrToDecrypt_rm = utilities.StripSpaces(_StrToDecrypt);

                // Test for SQL Injection Test
                bool injectionTest = utilities.TestSearchStringForSQLInjection(_IPAddress, _StrToDecrypt_rm);
                //bool injectionTest = false;
                if (injectionTest) // SQL Injection Detected
                {
                    _StrToDecrypt_ck = String.Empty;
                    this.Send_SQLInjection_Notification_Email(_IPAddress);
                    // Noting else happens
                }
                else // SQL Injection NOT Detected
                {
                    _StrToDecrypt_ck = _StrToDecrypt;
                    tbxTextEntry.CssClass = "textbox";
                    //tbxTextEntry.Text = String.Empty;
                    count++;
                }
            }

            #endregion

            if (count > 1)
            {
                _ClearText = this.Decryption(_StrToDecrypt_ck, _Key_ck);
                tbxResult.Text = _ClearText;
                _Action = "Decryption";
                _IPAddress = this.GetIpAddress();
                if (_IsValid)
                {
                    // Temp GUID 
                    _UserGUID = new Guid("135CB7C8-8FAA-43F0-956E-B99CCC92C8D0");
                    bool result = data.Create_Data(_UserGUID, _IPAddress, _Action, _Key_ck, _ClearText, _Device);
                }
            }
        }

        protected string Decryption(string strEncrypted, string strKey)
        {
            try
            {
                TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
                byte[] byteHash, byteBuff;
                string strTempKey = strKey;
                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
                byteBuff = Convert.FromBase64String(strEncrypted);
                string strDecrypted = ASCIIEncoding.ASCII.GetString(objDESCrypto.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                objDESCrypto = null;
                _IsValid = true;
                return strDecrypted;
            }
            catch (Exception ex)
            {
                _IsValid = false;
                return "Nope! Is your Keyword correct? ";
            }
        }

        public static String CleanInput(string strIn)
        {
            return Regex.Replace(strIn, @"<", " <<<BAD>>> ");
        }

        public string GetIpAddress()
        {
            string clientIP;
            try
            {
                clientIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            catch (Exception ex)
            {
                clientIP = "ip-not-acquired";
            }
            finally { }
            return clientIP;
        }

        private void Send_SQLInjection_Notification_Email(string injectionString)
        {
            Email eMsg = new Email();
            eMsg.To = ConfigurationManager.AppSettings["To_Nimda"].ToString();
            eMsg.From = ConfigurationManager.AppSettings["From_Nimda"].ToString();
            eMsg.Cc = ConfigurationManager.AppSettings["Cc_Nimda"].ToString();
            eMsg.Bcc = ConfigurationManager.AppSettings["Bcc_Nimda"].ToString();
            eMsg.IPAddress = _IPAddress;
            Uri TheWholeUrl = Request.Url;
            eMsg.Url = TheWholeUrl.ToString(); // Get the URL of the Webpage where the attack was attempted.
            eMsg.InjectionString = injectionString;
            eMsg.SendEmail_ToAdmins("SQLInjectionAttempt!");
        }
    }
}