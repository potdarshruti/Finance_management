using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Finance_management
{
    public partial class Home : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["loginid"] != null)
            {
                if (!IsPostBack)
                {
                    GridView1.DataSource = getList();
                    GridView1.DataBind();
                }
            }
            else
            {
                headrem.Visible = false;
            }
            

        }

        
        protected DataSet getList()
        {
            DateTime today = DateTime.Now.Date;
            con.Close();
            SqlCommand cmd = new SqlCommand("select re_tittle,re_remark  from reminders where loginid=@loginid and re_date=@today", con);
            
            
            cmd.Parameters.AddWithValue("@loginid", Session["loginid"]);
            cmd.Parameters.AddWithValue("@today", today);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
    }
}