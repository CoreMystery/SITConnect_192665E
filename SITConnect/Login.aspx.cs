using SITConnect.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class Login : System.Web.UI.Page
    {
        protected string countdown;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                Response.Clear();
                Response.StatusCode = 403;
                Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            if (Session["Registration"] != null)
            {
                Session.Remove("Registration");
                lbFlashMessage.Text = "Registration successful! You may login.";
                LoginPanel.Visible = true;
            }
        }

        // Button - Login
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (validateInput() && ValidateCaptcha())
            {
                string email = tbEmail.Text.ToString().Trim();
                string password = tbPassword.Text.ToString();

                Account user = Account.RetrieveByEmail(email);

                try
                {
                    if (user != null && password != null)
                    {
                        SHA512Managed hashing = new SHA512Managed();
                        string pwdWithSalt = password + user.PasswordSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);
                        if (user.LockedFrom != null)
                        {
                            TimeSpan timeRemaining = (TimeSpan)(DateTime.Now - user.LockedFrom);
                            if (timeRemaining.TotalMinutes >= 5)
                            {
                                user.FailedLoginAttempts = 0;
                                user.LockedFrom = null;
                                Account.Update(user);
                            }
                            else
                            {
                                errorMsg.Text = $"Too many failed login attempts. Try again in {timeRemaining.Subtract(TimeSpan.FromMinutes(5)):mm\\:ss}.";
                                countdown = (timeRemaining.Subtract(TimeSpan.FromMinutes(5)).TotalSeconds * -1).ToString();
                            }
                        }
                        if (user.LockedFrom == null)
                        {
                            if (userHash.Equals(user.PasswordHash))
                            {
                                user.FailedLoginAttempts = 0;
                                Account.Update(user);

                                TimeSpan passwordAge = DateTime.Now - user.PasswordAge;

                                Session["UserID"] = user.Id;

                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;
                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                if (passwordAge.TotalMinutes >= 15)
                                {
                                    Session["ChangePassword"] = true;
                                    Response.Redirect("ChangePassword.aspx", false);
                                }
                                else
                                {
                                    Response.Redirect("Profile.aspx", false);
                                }
                            }
                            else
                            {
                                int failedLoginAttempts = 1 + user.FailedLoginAttempts;

                                if (failedLoginAttempts >= 3)
                                {
                                    user.LockedFrom = DateTime.Now;
                                    Account.Update(user);

                                    errorMsg.Text = "Account has been locked due to too many failed login attempts.";
                                }
                                else
                                {
                                    user.FailedLoginAttempts = failedLoginAttempts;
                                    Account.Update(user);

                                    errorMsg.Text = "Email or Password is not valid. Please try again.";
                                }
                            }
                        }
                    }
                    else
                    {
                        errorMsg.Text = "Email or Password is not valid. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }

        // Input Validation
        protected bool validateInput()
        {
            bool validation  = true;
            if (string.IsNullOrEmpty(tbEmail.Text))
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
            if (string.IsNullOrEmpty(tbPassword.Text))
            {
                lbPassword.Text = "Password cannot be empty.";
                validation = false;
            }
            else
                lbPassword.Text = "";

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
    }
}