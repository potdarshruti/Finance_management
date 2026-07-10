using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_management
{
    public partial class income : System.Web.UI.Page
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime entryDate;
            double amount;
            if (!ValidationHelper.IsValidDate(Indate.Text, out entryDate))
            {
                ShowAlert("Please enter a valid date.", "warning");
                return;
            }
            if (!ValidationHelper.IsCategorySelected(Incat.SelectedValue))
            {
                ShowAlert("Please select a category.", "warning");
                return;
            }
            if (!ValidationHelper.IsValidAmount(txtInAmt.Text, out amount))
            {
                ShowAlert("Please enter a valid amount.", "warning");
                return;
            }

            con.Close();
            SqlCommand cmd = new SqlCommand("insert into income (in_date,in_category,in_amount,in_remark,loginid) values (@in_date,@in_category,@in_amount,@in_remark,@loginid)", con);
            cmd.Parameters.AddWithValue("@in_date", Indate.Text);
            cmd.Parameters.AddWithValue("@in_category", Incat.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@in_amount", txtInAmt.Text);
            cmd.Parameters.AddWithValue("@in_remark", InRemark.Text);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            Indate.Text = "";
            txtInAmt.Text = "";
            InRemark.Text = "";
            Incat.SelectedIndex = 0;
            BindGrid();
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('Income saved successfully..!','','success');", true);
        }

        protected void BindGrid()
        {
            gvdata.DataSource = getList();
            gvdata.DataBind();
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
            string category = ((DropDownList)row.FindControl("ddlEditCat")).SelectedItem.Text;
            string amount = ((TextBox)row.FindControl("txtEditAmt")).Text;
            string remark = ((TextBox)row.FindControl("txtEditRemark")).Text;

            con.Close();
            SqlCommand cmd = new SqlCommand("UPDATE income SET in_date=@date, in_category=@cat, in_amount=@amt, in_remark=@remark WHERE srno=@srno AND loginid=@loginid", con);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@cat", category);
            cmd.Parameters.AddWithValue("@amt", amount);
            cmd.Parameters.AddWithValue("@remark", remark);
            cmd.Parameters.AddWithValue("@srno", srno);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            gvdata.EditIndex = -1;
            BindGrid();
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('Income updated successfully..!','','success');", true);
        }

        protected void gvdata_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlEditCat");
                string cat = DataBinder.Eval(e.Row.DataItem, "in_category").ToString();
                ListItem item = ddl.Items.FindByText(cat);
                if (item != null) item.Selected = true;
            }
        }

        protected void gvdata_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("DELETE FROM income WHERE srno=@srno AND loginid=@loginid", con);
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
