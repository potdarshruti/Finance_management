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
    public partial class income : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["loginid"] != null)
            {
                if (!IsPostBack)
                {
                    gvdata.DataSource = getList();
                    gvdata.DataBind();
                }
                
            }
            else
            {
                Response.Redirect("login.aspx");
            }


        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("insert into income (in_date,in_category,in_amount,in_remark,loginid) values (@in_date,@in_category,@in_amount,@in_remark,@loginid)", con);
            cmd.Parameters.AddWithValue("@in_date", Indate.Text);
            cmd.Parameters.AddWithValue("@in_category", Incat.SelectedItem.Text );
            cmd.Parameters.AddWithValue("@in_amount", txtInAmt.Text);
            cmd.Parameters.AddWithValue("@in_remark", InRemark.Text);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            cmd.ExecuteNonQuery();

            Indate.Text = "";
            txtInAmt.Text = "";
            InRemark.Text = "";
            Incat.SelectedIndex = 0;
            gvdata.DataSource = getList();
            gvdata.DataBind();

            this.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "swal('Registered Successfully..!','','success');", true);
        }
        protected DataSet getList()
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from income where loginid = @loginid order by Srno DESC", con);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
        protected void gvdata_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvdata.Rows[e.RowIndex];

            con.Close();
            SqlCommand cmd = new SqlCommand("Delete from income where srno='" + gvdata.DataKeys[e.RowIndex].Value + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();

            gvdata.EditIndex = -1;
            gvdata.DataSource = getList();
            gvdata.DataBind();
            this.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "swal('Entry deleted successfully..!','','success');", true);

        }

    }
}
    