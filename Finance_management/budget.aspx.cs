using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_management
{
    public partial class budget : System.Web.UI.Page
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
                txtMonth.Text = DateTime.Now.ToString("yyyy-MM");
                BindBudgetGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.IsCategorySelected(ddlCategory.SelectedValue))
            {
                ShowAlert("Please select a category.", "warning");
                return;
            }

            double amount;
            if (!ValidationHelper.IsValidAmount(txtAmount.Text, out amount))
            {
                ShowAlert("Please enter a valid budget amount.", "warning");
                return;
            }

            DateTime monthStart = GetSelectedMonthStart();

            con.Close();
            SqlCommand cmd = new SqlCommand(@"
                IF EXISTS (SELECT 1 FROM budgets WHERE loginid=@loginid AND budget_month=@month AND ex_category=@cat)
                    UPDATE budgets SET budget_amount=@amt WHERE loginid=@loginid AND budget_month=@month AND ex_category=@cat
                ELSE
                    INSERT INTO budgets (loginid, budget_month, ex_category, budget_amount)
                    VALUES (@loginid, @month, @cat, @amt)", con);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            cmd.Parameters.AddWithValue("@month", monthStart);
            cmd.Parameters.AddWithValue("@cat", ddlCategory.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@amt", amount);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            txtAmount.Text = "";
            ddlCategory.SelectedIndex = 0;
            BindBudgetGrid();
            ShowAlert("Budget saved successfully..!", "success");
        }

        protected void gvBudget_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("DELETE FROM budgets WHERE srno=@srno AND loginid=@loginid", con);
            cmd.Parameters.AddWithValue("@srno", gvBudget.DataKeys[e.RowIndex].Value);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            BindBudgetGrid();
            ShowAlert("Budget deleted successfully..!", "success");
        }

        protected void BindBudgetGrid()
        {
            DateTime monthStart = GetSelectedMonthStart();
            DateTime monthEnd = monthStart.AddMonths(1);
            lblMonthDisplay.Text = monthStart.ToString("MMMM yyyy");

            con.Close();
            SqlCommand cmd = new SqlCommand(@"
                SELECT b.srno, b.ex_category, b.budget_amount,
                       ISNULL(SUM(CAST(e.ex_amount AS FLOAT)), 0) AS spent_amount
                FROM budgets b
                LEFT JOIN expense e ON e.loginid = b.loginid
                    AND e.ex_category = b.ex_category
                    AND e.ex_date >= b.budget_month AND e.ex_date < @monthEnd
                WHERE b.loginid = @loginid AND b.budget_month = @monthStart
                GROUP BY b.srno, b.ex_category, b.budget_amount
                ORDER BY b.ex_category", con);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            cmd.Parameters.AddWithValue("@monthStart", monthStart);
            cmd.Parameters.AddWithValue("@monthEnd", monthEnd);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            dt.Columns.Add("remaining", typeof(double));
            dt.Columns.Add("percent", typeof(double));
            foreach (DataRow row in dt.Rows)
            {
                double budgetAmt = Convert.ToDouble(row["budget_amount"]);
                double spent = Convert.ToDouble(row["spent_amount"]);
                row["remaining"] = budgetAmt - spent;
                row["percent"] = budgetAmt > 0 ? Math.Min(100, Math.Round(spent / budgetAmt * 100, 0)) : 0;
            }

            gvBudget.DataSource = dt;
            gvBudget.DataBind();
        }

        protected DateTime GetSelectedMonthStart()
        {
            if (string.IsNullOrWhiteSpace(txtMonth.Text))
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }

            DateTime parsed;
            if (DateTime.TryParseExact(txtMonth.Text, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
            {
                return new DateTime(parsed.Year, parsed.Month, 1);
            }

            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        protected string GetProgressClass(object percentObj)
        {
            double percent = Convert.ToDouble(percentObj);
            if (percent >= 100) return "bg-danger";
            if (percent >= 80) return "bg-warning";
            return "bg-success";
        }

        protected void ShowAlert(string message, string type)
        {
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('" + message + "','','" + type + "');", true);
        }
    }
}
