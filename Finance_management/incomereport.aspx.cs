using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Finance_management
{
    public partial class incomereport : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["loginid"] == null)
            {

                Response.Redirect("login.aspx");
            }
            

        }

        protected DataSet getdata()
        {
            //DateTime dt = DateTime.Now;
            double total = 0;
            DataSet ds = new DataSet();
            con.Close();
            SqlCommand cmd = new SqlCommand("Select * from income where in_date BETWEEN @fromdate and @enddate and loginid=@id order by Srno", con);
            DateTime dt1 = DateTime.Parse(txtfrom.Text);
            DateTime dt2 = DateTime.Parse(txtto.Text);
            cmd.Parameters.AddWithValue("@fromdate", dt1);
            cmd.Parameters.AddWithValue("@enddate", dt2);
            cmd.Parameters.AddWithValue("@id", Session["loginid"]);
            con.Open();

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            con.Close();

            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                total = total + Convert.ToDouble(dr["in_amount"].ToString());
            }
            lbltotal.Text = total.ToString();
            return ds;
        }



        protected void btnIncome_Click(object sender, EventArgs e)
        {
            if (txtfrom.Text != "" && txtto.Text != "")
            {
                DateTime inforvalid = DateTime.Parse(txtfrom.Text);
                DateTime outforvalid = DateTime.Parse(txtto.Text);
                if (inforvalid > outforvalid)
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "swal('Select valid date range...!','','error');", true);
                }
                else
                {
                    gvRecords.DataSource = getdata();
                    gvRecords.DataBind();

                }
            }
        }
    }
}