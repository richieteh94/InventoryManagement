using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class LoginPage : Page
    {
        public class Admin
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "alertme", "alertMe()", true);
        }

        protected void Login(object sender, EventArgs e)
        {
            try
            {
                string inputUser = String.Format("{0}", Request.Form["username"]);
                string inputPass = String.Format("{0}", Request.Form["password"]);
                Admin loginAdmin = new Admin();
                loginAdmin.username = "";
                loginAdmin.password = "";

                MySqlConnection conn = new MySqlConnection("server=localhost;userid=root;password=;database=psm");
                conn.Open();

                string statement = "SELECT * FROM admin WHERE username = '" + inputUser + "'";

                MySqlCommand cmd = new MySqlCommand(statement, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    loginAdmin.username = rdr.GetString(1);
                    loginAdmin.password = rdr.GetString(2);
                }
                rdr.Close();
                conn.Close();

                if (loginAdmin.username == "")
                {
                    Response.Write("<script>alert('" + inputUser + " does not exist.');</script>");
                }
                else
                {
                    if(loginAdmin.password == inputPass)
                    {
                        Session["username"] = inputUser;
                        Response.Redirect("HomePage.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('wrong password.');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
        }
    }
}