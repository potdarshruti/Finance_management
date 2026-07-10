using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Finance_management
{
    public partial class login : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"] != null ? ConfigurationManager.ConnectionStrings["connstr"].ConnectionString : "");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RegisterSuccess"] != null)
            {
                Session.Remove("RegisterSuccess");
                ShowAlert("Registered successfully. Please log in.", "success");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.IsValidEmail(txtEmail.Text))
            {
                ShowAlert("Please enter a valid email address.", "warning");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowAlert("Please enter your password.", "warning");
                return;
            }

            con.Close();
            SqlCommand cmd = new SqlCommand("SELECT srno, password FROM registration WHERE email=@email", con);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            if (sdr.Read())
            {
                string storedPassword = sdr["password"].ToString();
                if (PasswordHelper.VerifyPassword(txtPassword.Text, storedPassword))
                {
                    int userId = Convert.ToInt32(sdr["srno"]);
                    sdr.Close();

                    if (PasswordHelper.NeedsRehash(storedPassword))
                    {
                        UpgradePasswordHash(userId, txtPassword.Text);
                    }

                    Session["loginid"] = userId;
                    Response.Redirect("Home.aspx");
                    return;
                }
            }

            sdr.Close();
            con.Close();
            ShowAlert("Invalid email or password..!", "error");
        }

        protected void UpgradePasswordHash(int userId, string plainPassword)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("UPDATE registration SET password=@pwd WHERE srno=@id", con);
            cmd.Parameters.AddWithValue("@pwd", PasswordHelper.HashPassword(plainPassword));
            cmd.Parameters.AddWithValue("@id", userId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        protected void ShowAlert(string message, string type)
        {
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('" + message + "','','" + type + "');", true);
        }
    }
}
