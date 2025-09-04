using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_management
{
    public partial class Index : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isLoggedIn = Session["loginid"] != null;

            logbutton.Visible = !isLoggedIn; // Hide Register if logged in
            loginbutton.Visible = !isLoggedIn;    // Hide Login if logged in
            logoutbutton.Visible = isLoggedIn; // Show Logout if logged in

        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("Home.aspx");

        }
    }
}