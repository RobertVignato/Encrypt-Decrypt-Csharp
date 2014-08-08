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
    public partial class Encrypt : System.Web.UI.Page
    {
        Utilities utilities = new Utilities();
        Data data = new Data();

        #region - Accessors -

        public Guid _UserGUID { get; set; }
        public string _IPAddress { get; set; }

        public string _Action { get; set; }

        public string _Key { get; set; }
        public string _Key_rm { get; set; }
        public string _Key_ck { get; set; }

        public string _ClearText { get; set; }
        public string _ClearText_rm { get; set; }
        public string _ClearText_ck { get; set; }

        public string _EncryptedText { get; set; }
        //public string _EncryptedText_rm { get; set; }
        //public string _EncryptedText_ck { get; set; }

        protected string _Device = "Web";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region - Check if the user is authenticated -

            //if (!Request.IsAuthenticated)
            //{
            //    //string loginPage = ConfigurationManager.AppSettings["AuthenticationChallengeRedirect"];
            //    //Response.Redirect(loginPage);
            //    btnScramble.Enabled = false;
            //    lblInfo.Text = "Please Login or Sign Up to Scramble";
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

        protected void btnScramble_Click(object sender, EventArgs e)
        {
            this.ValidateFormEntries_Encrypt();
        }

        private void ValidateFormEntries_Encrypt()
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

            #region - tbxTextEntry - ClearText -

            if (tbxTextEntry.Text == String.Empty)
            {
                tbxTextEntry.CssClass = "textboxYellow";
                lblTextEntry.Text = "*";
            }
            else
            {
                _ClearText = tbxTextEntry.Text;
                _ClearText_rm = utilities.StripSpaces(_ClearText);

                // Test for SQL Injection Test
                bool injectionTest = utilities.TestSearchStringForSQLInjection(_IPAddress, _ClearText_rm);
                //bool injectionTest = false;
                if (injectionTest) // SQL Injection Detected
                {
                    _ClearText_ck = String.Empty;
                    this.Send_SQLInjection_Notification_Email(_IPAddress);
                    // Noting else happens
                }
                else // SQL Injection NOT Detected
                {
                    _ClearText_ck = _ClearText;
                    tbxTextEntry.CssClass = "textbox";
                    //tbxTextEntry.Text = String.Empty;
                    count++;
                }
            }

            #endregion

            if (count > 1)
            {
                _EncryptedText = this.Encryption(_ClearText_ck, _Key_ck);
                tbxResult.Text = _EncryptedText;
                _Action = "Encryption";
                _IPAddress = this.GetIpAddress();
                // Temp GUID 
                _UserGUID = new Guid("135CB7C8-8FAA-43F0-956E-B99CCC92C8D0");
                bool result = data.Create_Data(_UserGUID, _IPAddress, _Action, _Key_ck, _ClearText_ck, _Device);
            }
        }

        protected string Encryption(string ClearTextX, string strKey)
        {
            try
            {
                string ClearText;
                ClearText = CleanInput(ClearTextX);
                TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
                byte[] byteHash, byteBuff;
                string strTempKey = strKey;
                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
                byteBuff = ASCIIEncoding.ASCII.GetBytes(ClearText);
                return Convert.ToBase64String(objDESCrypto.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
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
            //eMsg.SendEmail_ToAdmins("SQLInjectionAttempt! Jumbuh SignUp");
        }
    }
}