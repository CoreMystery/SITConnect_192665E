using SITConnect.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Clear();
                Response.StatusCode = 401;
                Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            if (Session["PasswordChanged"] != null)
            {
                Session.Remove("PasswordChanged");
                lbFlashMessage.Text = "Password has been changed.";
                ProfilePanel.Visible = true;
            }
            if (Session["UserID"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                if (Session["ChangePassword"] != null)
                {
                    Response.Redirect("ChangePassword.aspx", false);
                }
                else
                {
                    Account user = Account.RetrieveById(Session["UserID"].ToString());

                    lbFirstName.Text = HttpUtility.HtmlEncode(user.FirstName);
                    lbLastName.Text = HttpUtility.HtmlEncode(user.LastName);
                    lbEmail.Text = HttpUtility.HtmlEncode(user.Email);
                    lbDateOfBirth.Text = HttpUtility.HtmlEncode(Convert.ToDateTime(user.DateOfBirth).ToString("dd/MM/yyyy"));
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            };
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx", false);
        }
    }
}