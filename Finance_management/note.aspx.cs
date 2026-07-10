using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_management
{
    public partial class note : System.Web.UI.Page
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
            if (string.IsNullOrWhiteSpace(NTittle.Text))
            {
                ShowAlert("Please enter a note title.", "warning");
                return;
            }

            con.Close();
            SqlCommand cmd = new SqlCommand("insert into notes (note_tittle,note_remark,loginid) values (@note_tittle,@note_remark,@loginid)", con);
            cmd.Parameters.AddWithValue("@note_tittle", NTittle.Text);
            cmd.Parameters.AddWithValue("@note_remark", NDesc.Text);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            NTittle.Text = "";
            NDesc.Text = "";
            BindGrid();
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('Note saved successfully..!','','success');", true);
        }

        protected void BindGrid()
        {
            gvdata.DataSource = getList();
            gvdata.DataBind();
        }

        protected DataSet getList()
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from notes where loginid=@loginid order by Srno DESC", con);
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
            string title = ((TextBox)row.FindControl("txtEditTitle")).Text;
            string remark = ((TextBox)row.FindControl("txtEditRemark")).Text;

            con.Close();
            SqlCommand cmd = new SqlCommand("UPDATE notes SET note_tittle=@title, note_remark=@remark WHERE srno=@srno AND loginid=@loginid", con);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@remark", remark);
            cmd.Parameters.AddWithValue("@srno", srno);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            gvdata.EditIndex = -1;
            BindGrid();
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('Note updated successfully..!','','success');", true);
        }

        protected void gvdata_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("DELETE FROM notes WHERE srno=@srno AND loginid=@loginid", con);
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
