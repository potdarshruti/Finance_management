using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_management
{
    public partial class incomereport : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"] != null ? ConfigurationManager.ConnectionStrings["connstr"].ConnectionString : "");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["loginid"] == null)
            {
                Response.Redirect("login.aspx");
            }
        }

        protected DataSet getdata()
        {
            double total = 0;
            DataSet ds = new DataSet();
            con.Close();
            SqlCommand cmd = new SqlCommand("SELECT * FROM income WHERE in_date BETWEEN @fromdate AND @enddate AND loginid=@id ORDER BY srno", con);
            DateTime dt1 = DateTime.Parse(txtfrom.Text);
            DateTime dt2 = DateTime.Parse(txtto.Text);
            cmd.Parameters.AddWithValue("@fromdate", dt1);
            cmd.Parameters.AddWithValue("@enddate", dt2);
            cmd.Parameters.AddWithValue("@id", Session["loginid"]);
            con.Open();

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            con.Close();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    total += Convert.ToDouble(dr["in_amount"].ToString());
                }
            }
            lbltotal.Text = total.ToString("N2");
            return ds;
        }

        protected void btnIncome_Click(object sender, EventArgs e)
        {
            DateTime fromDate;
            DateTime toDate;
            if (!ValidationHelper.IsValidDate(txtfrom.Text, out fromDate) ||
                !ValidationHelper.IsValidDate(txtto.Text, out toDate))
            {
                ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('Please select both dates..!','','warning');", true);
                return;
            }

            if (fromDate > toDate)
            {
                ClientScript.RegisterStartupScript(GetType(), "SweetAlert", "swal('Select valid date range...!','','error');", true);
                return;
            }

            gvRecords.DataSource = getdata();
            gvRecords.DataBind();
        }
    }
}
