using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_management
{
    public partial class Register : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"] != null ? ConfigurationManager.ConnectionStrings["connstr"].ConnectionString : "");

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ShowAlert("Please enter your name.", "warning");
                return;
            }

            if (!ValidationHelper.IsValidEmail(txtEmail.Text))
            {
                ShowAlert("Please enter a valid email address.", "warning");
                return;
            }

            if (!ValidationHelper.IsValidPhone(txtContact.Text))
            {
                ShowAlert("Please enter a valid 10-digit contact number.", "warning");
                return;
            }

            if (!ValidationHelper.IsCategorySelected(ddlcat.SelectedValue))
            {
                ShowAlert("Please select a category.", "warning");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text.Length < 6)
            {
                ShowAlert("Password must be at least 6 characters.", "warning");
                return;
            }

            if (EmailExists(txtEmail.Text.Trim()))
            {
                ShowAlert("This email is already registered.", "error");
                return;
            }

            con.Close();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO registration (name, email, contact, Category, password) VALUES (@name,@email,@contact,@Category,@pwd)", con);
            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@contact", txtContact.Text.Trim());
            cmd.Parameters.AddWithValue("@Category", ddlcat.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@pwd", PasswordHelper.HashPassword(txtPassword.Text));
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            Session["RegisterSuccess"] = true;
            Response.Redirect("login.aspx");
        }

        protected bool EmailExists(string email)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM registration WHERE email=@email", con);
            cmd.Parameters.AddWithValue("@email", email);
            con.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return count > 0;
        }

        protected void ShowAlert(string message, string type)
        {
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('" + message + "','','" + type + "');", true);
        }
    }
}
