using SITConnect.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected string countdown;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("Login.aspx", false);

            if (Session["ChangePassword"] != null)
            {
                lbError.Text = "You must change your password before proceeding.";
                PanelErrorResult.Visible = true;
            }
        }

        // Button - Change Password
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                Account user = Account.RetrieveById(Session["UserID"].ToString());

                if (user != null)
                {
                    if (validateInput())
                    {
                        string password = tbNewPassword.Text.ToString();
                        SHA512Managed hashing = new SHA512Managed();

                        TimeSpan passwordAge = DateTime.Now - user.PasswordAge;

                        if (user.PasswordHash != Convert.ToBase64String(hashing.ComputeHash(Encoding.UTF8.GetBytes(tbCurrentPassword.Text.ToString() + user.PasswordSalt))))
                        {
                            lbError.Text = $"Your current password is invalid!";
                            PanelErrorResult.Visible = true;
                        }
                        else if (passwordAge.TotalMinutes <= 5)
                        {
                            lbError.Text = $"You can't change your password within 5 minutes from the last change of password. Time Remaining: {passwordAge.Subtract(TimeSpan.FromMinutes(5)):mm\\:ss}";
                            PanelErrorResult.Visible = true;
                            countdown = (passwordAge.Subtract(TimeSpan.FromMinutes(5)).TotalSeconds * -1).ToString();
                        }

                        else if (user.PasswordHash == Convert.ToBase64String(hashing.ComputeHash(Encoding.UTF8.GetBytes(password + user.PasswordSalt))))
                        {
                            lbError.Text = $"You cannot change to your current password!";
                            PanelErrorResult.Visible = true;
                        }

                        else if (user.OldPassword1 == Convert.ToBase64String(hashing.ComputeHash(Encoding.UTF8.GetBytes(password + user.OldSalt1))) || user.OldPassword2 == Convert.ToBase64String(hashing.ComputeHash(Encoding.UTF8.GetBytes(password + user.OldSalt2))))
                        {
                            lbError.Text = $"Password has been used in the last two passwords you had. Please type another password.";
                            PanelErrorResult.Visible = true;
                        }

                        else
                        {
                            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                            byte[] saltByte = new byte[8];

                            rng.GetBytes(saltByte);
                            string salt = Convert.ToBase64String(saltByte);

                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                            string finalHash = Convert.ToBase64String(hashWithSalt);

                            if (user.OldPassword1 == null && user.OldPassword2 == null)
                            {
                                user.OldPassword2 = user.PasswordHash;
                                user.OldSalt2 = user.PasswordSalt;
                            }
                            else if (user.OldPassword1 == null || user.OldPassword2 == null)
                            {
                                user.OldPassword1 = user.PasswordHash;
                                user.OldSalt1 = user.PasswordSalt;
                            }
                            else
                            {
                                user.OldPassword2 = user.OldPassword1;
                                user.OldSalt2 = user.OldSalt1;
                                user.OldPassword1 = user.PasswordHash;
                                user.OldSalt1 = user.PasswordSalt;
                            }

                            user.PasswordHash = finalHash;
                            user.PasswordSalt = salt;
                            user.PasswordAge = DateTime.Now;

                            Account.Update(user);
                            Session.Remove("ChangePassword");
                            Session["PasswordChanged"] = true;

                            Response.Redirect("Profile.aspx", false);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        // Input Validation
        protected bool validateInput()
        {
            lbError.Text = "";
            PanelErrorResult.Visible = false;
            bool validation = true;
            if (string.IsNullOrWhiteSpace(tbCurrentPassword.Text))
            {
                lbCurrentPassword.Text = "Current password cannot be empty.";
                validation = false;
            }
            if (string.IsNullOrWhiteSpace(tbNewPassword.Text))
            {
                lbNewPassword.Text = "New password cannot be empty.";
                validation = false;
            }
            else
            {
                if (passwordComplexity())
                {
                    lbNewPassword.Text = "";
                    if (string.IsNullOrWhiteSpace(tbConfirmPassword.Text))
                    {
                        lbConfirmPassword.Text = "Enter new password again to confirm.";
                        validation = false;
                    }
                    else
                    {
                        if (tbNewPassword.Text != tbConfirmPassword.Text)
                        {
                            lbConfirmPassword.Text = "Password does not match.";
                            validation = false;
                        }
                        else
                            lbConfirmPassword.Text = "";
                    }
                }
                else
                {
                    validation = false;
                }
            }

            return validation;
        }

        // Password Complexity
        protected bool passwordComplexity()
        {
            var validation = true;
            lbNewPassword.Text += "Complexity Requirements:<br>";
            if (!(tbNewPassword.Text.Length >= 8))
            {
                lbNewPassword.Text += "Length should be at least 8 characters.<br>";
                validation = false;
            }
            if (!(Regex.IsMatch(tbNewPassword.Text, @"[a-z]+")))
            {
                lbNewPassword.Text += "Should contain at least one lowercase character.<br>";
                validation = false;
            }
            if (!(Regex.IsMatch(tbNewPassword.Text, @"[A-Z]+")))
            {
                lbNewPassword.Text += "Should contain at least one uppercase character.<br>";
                validation = false;
            }
            if (!(Regex.IsMatch(tbNewPassword.Text, @"\d+")))
            {
                lbNewPassword.Text += "Should contain at least one numeric value.<br>";
                validation = false;
            }
            if (!(Regex.IsMatch(tbNewPassword.Text, @"[!@#$%^&*]+")))
            {
                lbNewPassword.Text += "Should contain at least one special character (!@#$%^&*).<br>";
                validation = false;
            }
            return validation;
        }
    }
}