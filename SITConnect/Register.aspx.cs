using SITConnect.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Registration : System.Web.UI.Page
    {
        byte[] Key;
        byte[] IV;
        public string currentYear = DateTime.Now.Year.ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                Response.Clear();
                Response.StatusCode = 403;
                Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                tbDOB.Attributes.Add("max", DateTime.Now.AddYears(-13).ToString("yyyy-MM-dd"));
            }
        }

        // Button - Register
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (validateInput() && ValidateCaptcha())
            {
                Account user = Account.RetrieveByEmail(tbEmail.Text.ToString().Trim());

                if (user == null)
                {
                    // Password Hashing
                    string password = tbPassword.Text.ToString();

                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    byte[] saltByte = new byte[8];

                    rng.GetBytes(saltByte);
                    string salt = Convert.ToBase64String(saltByte);

                    SHA512Managed hashing = new SHA512Managed();

                    byte[] saltedHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(password + salt));

                    string hashedPassword = Convert.ToBase64String(saltedHash);

                    RijndaelManaged cipher = new RijndaelManaged();
                    cipher.GenerateKey();
                    Key = cipher.Key;
                    IV = cipher.IV;

                    Account newUser = new Account(Guid.NewGuid().ToString(), tbFName.Text.Trim(), tbLName.Text.Trim(), tbEmail.Text.Trim(), tbDOB.Text.Trim(), hashedPassword, salt, Convert.ToBase64String(IV), Convert.ToBase64String(Key), Convert.ToBase64String(encryptData(tbCCNumber.Text.Trim())), Convert.ToBase64String(encryptData(tbCCExpiryDate.Text.Trim())), 0, null, null, null, null, null, DateTime.Now);
                    Account.Create(newUser);

                    Session["Registration"] = true;

                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    lbEmail.Text = "Email already exists!";
                }
            }
        }

        // Input Validation
        protected bool validateInput()
        {
            bool validation = true;
            if (string.IsNullOrWhiteSpace(tbFName.Text.Trim()))
            {
                lbFName.Text = "First Name cannot be empty.";
                validation = false;
            }
            else
            {
                if (!Regex.IsMatch(tbFName.Text.Trim(), @"^[a-zA-Z ]+$"))
                {
                    lbFName.Text = "First Name must only contain alphabets.";
                    validation = false;
                }
                else
                {
                    lbFName.Text = "";
                }
            }
            if (string.IsNullOrWhiteSpace(tbLName.Text.Trim()))
            {
                lbLName.Text = "Last Name cannot be empty.";
                validation = false;
            }
            else
            {
                if(!Regex.IsMatch(tbLName.Text.Trim(), @"^[a-zA-Z ]+$"))
                {
                    lbLName.Text = "Last Name must only contain alphabets.";
                    validation = false;
                }
                else
                {
                    lbLName.Text = "";
                }
            }
            if (string.IsNullOrWhiteSpace(tbEmail.Text.Trim()))
            {
                lbEmail.Text = "Email cannot be empty.";
                validation = false;
            }
            else
            {
                if (!Regex.IsMatch(tbEmail.Text.Trim(), @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                {
                    lbEmail.Text = "Email is in the wrong format. (E.g. SIT@mail.com)";
                    validation = false;
                }
                else
                {
                    lbEmail.Text = "";
                }

            }
            lbPassword.Text = "";
            if (string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                lbPassword.Text = "Password cannot be empty.<br>";
                validation = false;
            }
            else
            {
                if (passwordComplexity())
                {
                    lbPassword.Text = "";
                }
                else
                {
                    validation = false;
                }
            }
            if (string.IsNullOrWhiteSpace(tbConfirmPassword.Text))
            {
                lbConfirmPassword.Text = "Enter password again to confirm.";
                validation = false;
            }
            else
            {
                if (tbPassword.Text != tbConfirmPassword.Text)
                {
                    lbConfirmPassword.Text = "Password does not match.";
                    validation = false;
                }
                else
                {
                    lbConfirmPassword.Text = "";
                }
            }
            if (string.IsNullOrWhiteSpace(tbDOB.Text.Trim()) || !Regex.IsMatch(tbDOB.Text.Trim(), @"^([0-9]{4})[-]([0]?[1-9]|[1][0-2])[-]([0][1-9]|[1|2][0-9]|[3][0|1])$"))
            {
                lbDOB.Text = "Please select your date of birth.";
                validation = false;
            }
            else
            {
                lbDOB.Text = "";
            }
            if (string.IsNullOrWhiteSpace(tbCCNumber.Text.Trim()) || !Regex.IsMatch(tbCCNumber.Text.Trim(), @"^\d{4} \d{4} \d{4} \d{4}$"))
            {
                lbCCNumber.Text = "Please enter a valid card number.<br>(E.g. 1234 5678 1234 5678)";
                validation = false;
            }
            else
            {
                lbCCNumber.Text = "";
            }
            if (string.IsNullOrWhiteSpace(tbCCExpiryDate.Text.Trim()) || !Regex.IsMatch(tbCCExpiryDate.Text.Trim(), @"^([0][1-9]|[1][0-2]) [/] (\d{4})$"))
            {
                lbCCExpiryDate.Text = $"Please enter a valid expiry date.<br>(E.g. 01 / {DateTime.Now.Year})";
                validation = false;
            }
            else
            {
                int ccExpiryDateYear;
                if (Int32.TryParse(tbCCExpiryDate.Text.Trim().Substring(tbCCExpiryDate.Text.Trim().Length - 4), out ccExpiryDateYear))
                {
                    if (ccExpiryDateYear >= DateTime.Now.Year)
                    {
                        lbCCExpiryDate.Text = "";
                    }
                    else
                    {
                        lbCCExpiryDate.Text = $"Please enter a valid year.<br>({DateTime.Now.Year} and above)";
                        validation = false;
                    }
                }
                else
                {
                    lbCCExpiryDate.Text = $"Please enter a valid expiry date.<br>(E.g. 01 / {DateTime.Now.Year})";
                    validation = false;
                }
            }
            return validation;
        }

        // Password Complexity
        protected bool passwordComplexity()
        {
            var validation = true;
            lbPassword.Text += "Complexity Requirements:<br>";
            if (!(tbPassword.Text.Trim().Length >= 8))
            {
                lbPassword.Text += "Length should be at least 8 characters.<br>";
                validation = false;
            }
            if (!(Regex.IsMatch(tbPassword.Text, @"[a-z]+")))
            {
                lbPassword.Text += "Must include at least one lowercase character.<br>";
                validation = false;
            }
            if (!(Regex.IsMatch(tbPassword.Text, @"[A-Z]+")))
            {
                lbPassword.Text += "Must include at least one uppercase character.<br>";
                validation = false;
            }
            if (!(Regex.IsMatch(tbPassword.Text, @"\d+")))
            {
                lbPassword.Text += "Must include at least one numeric value.<br>";
                validation = false;
            }
            if (!(Regex.IsMatch(tbPassword.Text, @"[!@#$%^&*]+")))
            {
                lbPassword.Text += "Must include at least one special character (!@#$%^&*).<br>";
                validation = false;
            }
            return validation;
        }

        // Captcha
        public class CaptchaObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
            ("https://www.google.com/recaptcha/api/siteverify?secret=" + ConfigurationManager.AppSettings["RECAPTCHA_SECRET_KEY"].ToString() + "&response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        CaptchaObject jsonObject = js.Deserialize<CaptchaObject>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }

                if (!result)
                    errorMsg.Text = "Captcha failed.";

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        // Encryption
        protected byte[] encryptData(string data)
        {
            byte[] cipherText;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }
    }
}