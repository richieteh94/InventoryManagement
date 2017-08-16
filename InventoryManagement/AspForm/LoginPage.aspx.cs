using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            Session["username"] = "";

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

                if(inputUser=="" || inputPass == "")
                {
                    Response.Write("<script>alert('Please enter username field and password field.');</script>");
                }
                else
                {
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
                        if (loginAdmin.password == Encrypt(inputPass))
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
            }   
            catch (MySqlException mysqlex)
            {
                if (mysqlex.Number == 1042) {
                    Response.Write("<script>alert('Could not connect to database.');</script>");
                }
            }
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "teh2017psm";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "teh2017psm";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}