using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_management
{
    public partial class reminder : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"] != null ? ConfigurationManager.ConnectionStrings["connstr"].ConnectionString : "");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["loginid"] == null)
            {
                Response.Redirect("login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        protected void btnclick_save(object sender, EventArgs e)
        {
            DateTime entryDate;
            if (!ValidationHelper.IsValidDate(ReDate.Text, out entryDate))
            {
                ShowAlert("Please enter a valid date.", "warning");
                return;
            }
            if (string.IsNullOrWhiteSpace(ReTittle.Text))
            {
                ShowAlert("Please enter a reminder title.", "warning");
                return;
            }

            con.Close();
            SqlCommand cmd = new SqlCommand("insert into reminders (re_date,re_tittle,re_remark,loginid) values (@re_date,@re_tittle,@re_remark,@loginid)", con);
            cmd.Parameters.AddWithValue("@re_date", ReDate.Text);
            cmd.Parameters.AddWithValue("@re_tittle", ReTittle.Text);
            cmd.Parameters.AddWithValue("@re_remark", ReRemark.Text);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            ReDate.Text = "";
            ReRemark.Text = "";
            ReTittle.Text = "";
            BindGrid();
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('Reminder saved successfully..!','','success');", true);
        }

        protected void BindGrid()
        {
            gvdata.DataSource = getList();
            gvdata.DataBind();
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
            con.Close();
            return ds;
        }

        protected void gvdata_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvdata.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvdata_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvdata.EditIndex = -1;
            BindGrid();
        }

        protected void gvdata_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int srno = Convert.ToInt32(gvdata.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvdata.Rows[e.RowIndex];
            string date = ((TextBox)row.FindControl("txtEditDate")).Text;
            string title = ((TextBox)row.FindControl("txtEditTitle")).Text;
            string remark = ((TextBox)row.FindControl("txtEditRemark")).Text;

            con.Close();
            SqlCommand cmd = new SqlCommand("UPDATE reminders SET re_date=@date, re_tittle=@title, re_remark=@remark WHERE srno=@srno AND loginid=@loginid", con);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@remark", remark);
            cmd.Parameters.AddWithValue("@srno", srno);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            gvdata.EditIndex = -1;
            BindGrid();
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('Reminder updated successfully..!','','success');", true);
        }

        protected void gvdata_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("DELETE FROM reminders WHERE srno=@srno AND loginid=@loginid", con);
            cmd.Parameters.AddWithValue("@srno", gvdata.DataKeys[e.RowIndex].Value);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            gvdata.EditIndex = -1;
            BindGrid();
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('Entry deleted successfully..!','','success');", true);
        }

        protected void ShowAlert(string message, string type)
        {
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('" + message + "','','" + type + "');", true);
        }
    }
}
