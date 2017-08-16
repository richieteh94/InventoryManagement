using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class HomePage : System.Web.UI.Page
    {
        public class Product
        {
            public string barcode { get; set; }
            public string name { get; set; }
            public int quantity { get; set; }
            public double price { get; set; }
            public string status { get; set; }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)Session["username"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
            else
            {
                string UserName = (string)Session["username"];
                username.InnerHtml = UserName;
                addTableData();
            }
            
            //ClientScript.RegisterStartupScript(this.GetType(), "alertme", "alertMe()", true);

        }

        public void addTableData()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection("server=localhost;userid=root;password=;database=psm");
                conn.Open();

                string statement = "SELECT * FROM product ORDER BY status ASC, quantity ASC";

                MySqlCommand cmd = new MySqlCommand(statement, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string barcode = rdr.GetString(0);
                    string name = rdr.GetString(1);
                    double price = rdr.GetDouble(2);
                    int quantity = rdr.GetInt32(3);
                    string status = rdr.GetString(4);

                    TableRow row = new TableRow();
                    TableCell barcodeCell = new TableCell();
                    TableCell nameCell = new TableCell();
                    TableCell priceCell = new TableCell();
                    TableCell quantityCell = new TableCell();
                    TableCell statusCell = new TableCell();

                    barcodeCell.Text = barcode;
                    nameCell.Text = name;
                    priceCell.Text = price.ToString("0.00");
                    quantityCell.Text = quantity.ToString();
                    statusCell.Text = status;      

                    row.Cells.Add(barcodeCell);
                    row.Cells.Add(nameCell);
                    row.Cells.Add(priceCell);
                    row.Cells.Add(quantityCell);
                    row.Cells.Add(statusCell);

                    if (quantity <= 0)
                    {
                        row.BackColor = ColorTranslator.FromHtml("#FF6666");
                    }

                    searchTable.Rows.Add(row);

                }
                rdr.Close();
                conn.Close();
            }
            catch (MySqlException mysqlex)
            {
                if (mysqlex.Number == 1042)
                {
                    Response.Write("<script>alert('Could not connect to database.');</script>");
                }
                else
                {
                    Response.Write("<script>alert('" + mysqlex.Number + "');</script>");
                }
            }
        }

        public void clearTable(Table table)
        {
            //Response.Write("<script>alert('"+table.Rows.Count+"')</script>");
            for (int i = table.Rows.Count-1; i > 0; i--)
            {
                table.Rows.RemoveAt(i);
            }
        }

        /*private void searchItem(string searchInput)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection("server=localhost;userid=root;password=;database=psm");
                conn.Open();
                
                string statement = "SELECT * FROM product WHERE barcode='" + searchInput + "' ORDER BY status ASC, quantity ASC";

                MySqlCommand cmd = new MySqlCommand(statement, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string barcode = rdr.GetString(0);
                    string name = rdr.GetString(1);
                    double price = rdr.GetDouble(2);
                    int quantity = rdr.GetInt32(3);
                    string status = rdr.GetString(4);

                    TableRow row = new TableRow();
                    TableCell barcodeCell = new TableCell();
                    TableCell nameCell = new TableCell();
                    TableCell priceCell = new TableCell();
                    TableCell quantityCell = new TableCell();
                    TableCell statusCell = new TableCell();

                    barcodeCell.Text = barcode;
                    nameCell.Text = name;
                    priceCell.Text = price.ToString("0.00");
                    quantityCell.Text = quantity.ToString();
                    statusCell.Text = status;

                    row.Cells.Add(barcodeCell);
                    row.Cells.Add(nameCell);
                    row.Cells.Add(priceCell);
                    row.Cells.Add(quantityCell);
                    row.Cells.Add(statusCell);

                    searchTable.Rows.Add(row);
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
        }*/
        
        /*protected void searchClicked(object sender, EventArgs e)
        {
            clearTable(searchTable);

            string searchBox = String.Format("{0}", Request.Form["search"]);    
            //Response.Write("<script>alert('"+searchBox+"')</script>");

            if (searchBox == "")
            {
                addTableData();
            }
            else
            {
                searchItem(searchBox);
            }
            
        }*/

        [WebMethod]
        public static string refillItems(List<Product> products)
        {
            string responseString = "";
            try
            {
                MySqlConnection conn = new MySqlConnection("server=localhost;userid=root;password=;database=psm");
                conn.Open();

                foreach (Product product in products)
                {
                    string statement = "UPDATE product SET quantity = quantity + " 
                        + product.quantity+" WHERE barcode = '"+product.barcode+"'";

                    MySqlCommand cmd = new MySqlCommand(statement, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        responseString += product.name + " added " + product.quantity + " pcs.\n";
                    }
                    else
                    {
                        responseString += product.name + "failed to add stock.";
                    }
                }

                conn.Close();
            }
            catch (MySqlException mysqlex)
            {
                //Console.Write("failed : "+ex.ToString());
                if (mysqlex.Number == 1042)
                {
                    responseString = "Could not connect to database.";
                }
                else
                {
                    responseString = mysqlex.Number+"";
                }

            }
            return responseString;
        }

        [WebMethod]
        public static string searchItemInModal(string Num,string Input)
        {
            string num = Num;
            string searchInput = Input;

            string jsonString = "{\"Barcode\":\"\",\"Name\":\"\",\"Row\":" 
                + num + ",\"SearchInput\":\"" + searchInput + "\"}";
            //Console.Write(jsonString);
            try
            {
                MySqlConnection conn = new MySqlConnection("server=localhost;userid=root;password=;database=psm");
                conn.Open();

                string statement = "SELECT * FROM product WHERE barcode='" + searchInput+"'";

                MySqlCommand cmd = new MySqlCommand(statement, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string barcode = rdr.GetString(0);
                    string name = rdr.GetString(1);

                    jsonString = "{\"Barcode\":\"" +barcode+ "\",\"Name\":\""+name+"\",\"Row\":"+num+",\"SearchInput\":\""+searchInput+ "\"}";
                    

                }
                rdr.Close();
                conn.Close();
            }
            catch (MySqlException mysqlex)
            {
                //Console.Write("failed : "+ex.ToString());
                jsonString = "{\"Barcode\":\"\",\"Name\":\"\",\"Row\":"
                + num + ",\"SearchInput\":\"" + searchInput + "\"}";
            }
            return jsonString;
        }

        [WebMethod]
        public static string addModalConfirm(Product product)
        {
            string responseString = "";

            try
            {
                MySqlConnection conn = new MySqlConnection("server=localhost;userid=root;password=;database=psm");
                conn.Open();

                string statement = "INSERT INTO product (barcode,name,price,quantity,status) VALUES ('" 
                    + product.barcode + "','" + product.name + "'," + product.price + "," 
                    + product.quantity + ",'"+product.status+"')";

                MySqlCommand cmd = new MySqlCommand(statement, conn);

                if (cmd.ExecuteNonQuery() == 1)
                {
                    responseString = product.name + " with barcode : " + product.barcode + " added into database.";
                    //Response.Redirect("HomePage.aspx");
                }
                else
                {
                    responseString = product.name + "failed to add into database.";
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                responseString = ex.ToString();
            }
            return responseString;
        }

        /*protected void addModalConfirm(object sender, EventArgs e)
        {
            string barcode = String.Format("{0}", Request.Form["addBarcode"]);
            string name = String.Format("{0}", Request.Form["addName"]);
            double price = Convert.ToDouble(String.Format("{0}", Request.Form["addPrice"]));
            string quantity = String.Format("{0}", Request.Form["addAmount"]);

            try
            {
                MySqlConnection conn = new MySqlConnection("server=localhost;userid=root;password=;database=psm");
                conn.Open();

                string statement = "INSERT INTO product (barcode,name,price,quantity) VALUES ('"+barcode 
                                    +"','" + name + "'," + price + "," + quantity+ ")";



                MySqlCommand cmd = new MySqlCommand(statement, conn);

                if (cmd.ExecuteNonQuery() == 1)
                {
                    Response.Write("<script>alert('" + name + " with barcode : " + barcode + " added into database.')</script>");
                    //Response.Redirect("HomePage.aspx");
                }
                else
                {
                    Response.Write("<script>alert('" + name + " failed to add into database')</script>");
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.ToString() + "')</script>");
            }
        }*/

        [WebMethod]
        public static string searchItemEditModal(string searchInput)
        {

            string jsonString = "{\"Barcode\":\"\",\"Name\":\"\",\"Price\":\"\",\"SearchInput\":\"" 
                                + searchInput + "\",\"Status\": \"\"}";
            //Console.Write(jsonString);
            try
            {
                MySqlConnection conn = new MySqlConnection("server=localhost;userid=root;password=;database=psm");
                conn.Open();

                string statement = "SELECT * FROM product WHERE barcode='" + searchInput + "'";



                MySqlCommand cmd = new MySqlCommand(statement, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string barcode = rdr.GetString(0);
                    string name = rdr.GetString(1);
                    double price = rdr.GetDouble(2);
                    string status = rdr.GetString(4);

                    jsonString = "{\"Barcode\":\"" + barcode + "\",\"Name\":\"" + name + "\",\"Price\":" 
                                + price + ",\"SearchInput\":\"" + searchInput + "\",\"Status\": \"" + status + "\"}";

                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                //jsonString = ex.ToString();
                jsonString = "{\"Barcode\":\"\",\"Name\":\"\",\"Price\":\"\",\"SearchInput\":\""
                                + searchInput + "\",\"Status\": \"\"}";

            }
            return jsonString;
        }

        [WebMethod]
        public static string searchEditModalConfirm(Product product)
        {
            string responseString = "";
            //Console.Write(jsonString);
             try
             {
                 MySqlConnection conn = new MySqlConnection("server=localhost;userid=root;password=;database=psm");
                 conn.Open();

                 string statement = "UPDATE product SET name = '" + product.name + "',price= " 
                                    + product.price + ",status ='" + product.status 
                                    + "' WHERE barcode = '" + product.barcode + "' ";

                MySqlCommand cmd = new MySqlCommand(statement, conn);

                if (cmd.ExecuteNonQuery() == 1)
                {
                    responseString = "Barcode : " + product.barcode + " updated.";
                }
                else
                {
                    responseString = "Barcode : " + product.barcode + " failed to update.";
                }
                conn.Close();
            }
             catch (Exception ex)
             {
                 responseString = ex.ToString();
             }
             return responseString;
        }

        protected void LogOut(object sender, EventArgs e)
        {
            Session["username"] = null;
            Response.Redirect("LoginPage.aspx");
        }
    }
}