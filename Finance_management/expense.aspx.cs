using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_management
{
    public partial class expense : System.Web.UI.Page
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
            double amount;
            if (!ValidationHelper.IsValidDate(ExDate.Text, out entryDate))
            {
                ShowAlert("Please enter a valid date.", "warning");
                return;
            }
            if (!ValidationHelper.IsCategorySelected(ExCat.SelectedValue))
            {
                ShowAlert("Please select a category.", "warning");
                return;
            }
            if (!ValidationHelper.IsValidAmount(ExAmt.Text, out amount))
            {
                ShowAlert("Please enter a valid amount.", "warning");
                return;
            }

            con.Close();
            SqlCommand cmd = new SqlCommand("insert into expense (ex_date,ex_category,ex_amount,ex_remark,loginid) values (@ex_date,@ex_category,@ex_amount,@ex_remark,@loginid)", con);
            cmd.Parameters.AddWithValue("@ex_date", ExDate.Text);
            cmd.Parameters.AddWithValue("@ex_category", ExCat.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@ex_amount", ExAmt.Text);
            cmd.Parameters.AddWithValue("@ex_remark", ExRemark.Text);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            ExDate.Text = "";
            ExAmt.Text = "";
            ExCat.SelectedIndex = 0;
            ExRemark.Text = "";
            BindGrid();
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('Expense saved successfully..!','','success');", true);
        }

        protected void BindGrid()
        {
            gvdata.DataSource = getList();
            gvdata.DataBind();
        }

        protected DataSet getList()
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from expense where loginid = @loginid order by Srno DESC", con);
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
            SqlCommand cmd = new SqlCommand("UPDATE expense SET ex_date=@date, ex_category=@cat, ex_amount=@amt, ex_remark=@remark WHERE srno=@srno AND loginid=@loginid", con);
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
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('Expense updated successfully..!','','success');", true);
        }

        protected void gvdata_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlEditCat");
                string cat = DataBinder.Eval(e.Row.DataItem, "ex_category").ToString();
                ListItem item = ddl.Items.FindByText(cat);
                if (item != null) item.Selected = true;
            }
        }

        protected void gvdata_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("DELETE FROM expense WHERE srno=@srno AND loginid=@loginid", con);
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
