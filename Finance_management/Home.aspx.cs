using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_management
{
    public partial class Home : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"] != null ? 
            ConfigurationManager.ConnectionStrings["connstr"].ConnectionString : "");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["loginid"] != null)
            {
                pnlDashboard.Visible = true;
                if (!IsPostBack)
                {
                    LoadDashboard();
                    GridView1.DataSource = getReminders();
                    GridView1.DataBind();
                }
            }
        }

        protected void LoadDashboard()
        {
            DateTime now = DateTime.Now;
            DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime startOfNextMonth = startOfMonth.AddMonths(1);
            lblMonth.Text = now.ToString("MMMM yyyy");

            double income = GetMonthlyTotal("income", "in_amount", "in_date", startOfMonth, startOfNextMonth);
            double expense = GetMonthlyTotal("expense", "ex_amount", "ex_date", startOfMonth, startOfNextMonth);
            double balance = income - expense;

            lblIncome.Text = income.ToString("N2");
            lblExpense.Text = expense.ToString("N2");
            lblBalance.Text = balance.ToString("N2");
            lblBalance.CssClass = balance >= 0 ? "card-value text-success" : "card-value text-danger";

            gvRecent.DataSource = GetRecentTransactions();
            gvRecent.DataBind();

            gvBudget.DataSource = GetBudgetProgress(startOfMonth, startOfNextMonth);
            gvBudget.DataBind();

            DataTable expenseByCat = GetExpenseByCategory(startOfMonth, startOfNextMonth);
            litCharts.Text = ChartHelper.BuildBarChart("chartIncomeExpense", "Income", income, "Expense", expense);
            if (expenseByCat.Rows.Count > 0)
            {
                litCharts.Text += ChartHelper.BuildDoughnutChart("chartExpenseCat", "Expense by category", expenseByCat, "Category", "Total");
            }
        }

        protected double GetMonthlyTotal(string table, string amountColumn, string dateColumn, DateTime start, DateTime end)
        {
            con.Close();
            string sql = string.Format(
                "SELECT ISNULL(SUM(CAST({0} AS FLOAT)), 0) FROM {1} WHERE loginid=@loginid AND {2} >= @start AND {2} < @end",
                amountColumn, table, dateColumn);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);
            con.Open();
            object result = cmd.ExecuteScalar();
            con.Close();
            return Convert.ToDouble(result);
        }

        protected DataTable GetExpenseByCategory(DateTime start, DateTime end)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand(@"
                SELECT ex_category AS Category, SUM(CAST(ex_amount AS FLOAT)) AS Total
                FROM expense WHERE loginid=@loginid AND ex_date >= @start AND ex_date < @end
                GROUP BY ex_category ORDER BY Total DESC", con);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        protected DataTable GetBudgetProgress(DateTime start, DateTime end)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand(@"
                SELECT b.ex_category, b.budget_amount,
                       ISNULL(SUM(CAST(e.ex_amount AS FLOAT)), 0) AS spent_amount
                FROM budgets b
                LEFT JOIN expense e ON e.loginid = b.loginid
                    AND e.ex_category = b.ex_category
                    AND e.ex_date >= b.budget_month AND e.ex_date < @end
                WHERE b.loginid = @loginid AND b.budget_month = @start
                GROUP BY b.ex_category, b.budget_amount
                ORDER BY b.ex_category", con);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();

            dt.Columns.Add("remaining", typeof(double));
            dt.Columns.Add("percent", typeof(double));
            foreach (DataRow row in dt.Rows)
            {
                double budgetAmt = Convert.ToDouble(row["budget_amount"]);
                double spent = Convert.ToDouble(row["spent_amount"]);
                row["remaining"] = budgetAmt - spent;
                row["percent"] = budgetAmt > 0 ? Math.Min(100, Math.Round(spent / budgetAmt * 100, 0)) : 0;
            }
            return dt;
        }

        protected string GetProgressClass(object percentObj)
        {
            double percent = Convert.ToDouble(percentObj);
            if (percent >= 100) return "bg-danger";
            if (percent >= 80) return "bg-warning";
            return "bg-success";
        }

        protected DataSet GetRecentTransactions()
        {
            con.Close();
            SqlCommand cmd = new SqlCommand(@"
                SELECT TOP 5 TransDate, TransType, Category, Amount, Remark FROM (
                    SELECT in_date AS TransDate, 'Income' AS TransType, in_category AS Category,
                           CAST(in_amount AS FLOAT) AS Amount, in_remark AS Remark
                    FROM income WHERE loginid=@loginid
                    UNION ALL
                    SELECT ex_date, 'Expense', ex_category,
                           CAST(ex_amount AS FLOAT), ex_remark
                    FROM expense WHERE loginid=@loginid
                ) t ORDER BY TransDate DESC", con);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            con.Close();
            return ds;
        }

        protected DataSet getReminders()
        {
            DateTime today = DateTime.Now.Date;
            con.Close();
            SqlCommand cmd = new SqlCommand(
                "SELECT re_tittle, re_remark FROM reminders WHERE loginid=@loginid AND re_date=@today", con);
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            cmd.Parameters.AddWithValue("@today", today);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            con.Close();
            return ds;
        }
    }
}
