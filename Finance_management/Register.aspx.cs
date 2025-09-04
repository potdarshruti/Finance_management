using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_management
{
    public partial class Register : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e, txtName, GetTxtEmail(), GetTxtContact(), GetDdlcat());
        }

        protected TextBox GetTxtEmail()
        {
            return txtEmail;
        }

        protected TextBox GetTxtContact()
        {
            return txtContact;
        }

        protected DropDownList GetDdlcat()
        {
            return ddlcat;
        }
        protected void btnSave_Click(object sender, EventArgs e, TextBox txtName, TextBox txtEmail, TextBox txtContact, DropDownList ddlcat)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("insert into registration values (@name,@email,@contact,@Category,@pwd)", con);
            cmd.Parameters.AddWithValue("@name", txtName.Text);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@contact", txtContact.Text);
            cmd.Parameters.AddWithValue("@Category", ddlcat.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@pwd", txtPassword.Text);
            con.Open();
            cmd.ExecuteNonQuery();

            txtName.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            ddlcat.SelectedIndex = 0;
            Response.Redirect("login.aspx");

            this.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "swal('Registered Successfully..!','','success');", true);
        }
        protected DataSet getList()
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from reminders where loginid=@loginid order by Srno DESC", con);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }

    }
}