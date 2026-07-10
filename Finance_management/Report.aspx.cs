using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_management
{
    public partial class Report : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"] != null ? ConfigurationManager.ConnectionStrings["connstr"].ConnectionString : "");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["loginid"] == null)
            {
                Response.Redirect("login.aspx");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DateTime fromDate;
            DateTime toDate;
            if (!ValidationHelper.IsValidDate(txtfrom.Text, out fromDate) ||
                !ValidationHelper.IsValidDate(txtto.Text, out toDate))
            {
                ShowAlert("Please select both dates..!", "warning");
                return;
            }

            if (fromDate > toDate)
            {
                ShowAlert("Select valid date range...!", "error");
                return;
            }

            double income = GetTotal("income", "in_amount", "in_date", fromDate, toDate);
            double expense = GetTotal("expense", "ex_amount", "ex_date", fromDate, toDate);
            double savings = income - expense;

            lblIncome.Text = income.ToString("N2");
            lblExpense.Text = expense.ToString("N2");
            lblSavings.Text = savings.ToString("N2");
            lblSavings.CssClass = savings >= 0 ? "card-value text-success" : "card-value text-danger";

            DataTable incomeCat = GetCategoryBreakdown("income", "in_category", "in_amount", "in_date", fromDate, toDate);
            DataTable expenseCat = GetCategoryBreakdown("expense", "ex_category", "ex_amount", "ex_date", fromDate, toDate);

            gvIncomeCat.DataSource = incomeCat;
            gvIncomeCat.DataBind();

            gvExpenseCat.DataSource = expenseCat;
            gvExpenseCat.DataBind();

            litCharts.Text = ChartHelper.BuildBarChart("plChartSummary", "Income", income, "Expense", expense);
            if (expenseCat.Rows.Count > 0)
            {
                litCharts.Text += ChartHelper.BuildDoughnutChart("plChartExpense", "Expense by category", expenseCat, "Category", "Total");
            }
            if (incomeCat.Rows.Count > 0)
            {
                litCharts.Text += ChartHelper.BuildDoughnutChart("plChartIncome", "Income by category", incomeCat, "Category", "Total");
            }

            pnlResults.Visible = true;
        }

        protected double GetTotal(string table, string amountCol, string dateCol, DateTime fromDate, DateTime toDate)
        {
            con.Close();
            string sql = string.Format(
                "SELECT ISNULL(SUM(CAST({0} AS FLOAT)), 0) FROM {1} WHERE loginid=@id AND {2} BETWEEN @from AND @to",
                amountCol, table, dateCol);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", Session["loginid"]);
            cmd.Parameters.AddWithValue("@from", fromDate);
            cmd.Parameters.AddWithValue("@to", toDate);
            con.Open();
            object result = cmd.ExecuteScalar();
            con.Close();
            return Convert.ToDouble(result);
        }

        protected DataTable GetCategoryBreakdown(string table, string categoryCol, string amountCol, string dateCol, DateTime fromDate, DateTime toDate)
        {
            con.Close();
            string sql = string.Format(
                "SELECT {0} AS Category, SUM(CAST({1} AS FLOAT)) AS Total FROM {2} WHERE loginid=@id AND {3} BETWEEN @from AND @to GROUP BY {0} ORDER BY Total DESC",
                categoryCol, amountCol, table, dateCol);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", Session["loginid"]);
            cmd.Parameters.AddWithValue("@from", fromDate);
            cmd.Parameters.AddWithValue("@to", toDate);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        protected void ShowAlert(string message, string type)
        {
            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('" + message + "','','" + type + "');", true);
        }
    }
}
