using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS_3sem.userpage
{
    public partial class mywinitem : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindOrderData();
            }
        }
        private void BindOrderData()
        {
            // Ensure UserID session is valid
            if (Session["UserID"] == null)
            {
                // Redirect to login if UserID is not found in session
                Response.Redirect("~/Login.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection connection = new SqlConnection("Data Source=AK\\SQLEXPRESS;Initial Catalog=AMS;Integrated Security=True"))
            {
                string query = "SELECT * FROM tbl_orders WHERE aid = @uid";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@uid", userId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        AllorderTableRecord.DataSource = reader;
                        AllorderTableRecord.DataBind();
                    }
                    else
                    {
                        // Update UI or display a message if no orders are found
                        ScriptManager.RegisterStartupScript(this, GetType(), "NoRowsAlert", "alert('No orders found.');", true);
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception and display an error message
                    // Consider logging the exception details to a file or monitoring system
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErrorAlert", $"alert('Error fetching orders: {ex.Message}');", true);
                }
            }
        }




        protected void payment(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            RepeaterItem item = (RepeaterItem)btn.NamingContainer;

            // Assuming the Repeater is bound to a DataSource, you can access the DataItem directly
            var dataItem = item.DataItem;
            var transactionId = DataBinder.Eval(dataItem, "transactionid");

            if (transactionId == DBNull.Value || string.IsNullOrEmpty(transactionId?.ToString()))
            {
                HiddenField HiddenField1 = (HiddenField)item.FindControl("HiddenField1");
                HiddenField HiddenField2 = (HiddenField)item.FindControl("HiddenField2");

                string orderNo = HiddenField1.Value;
                string amount = HiddenField2.Value;

                string url =$"{Request.Url.GetLeftPart(UriPartial.Authority)}/payment.aspx?orderno={orderNo}&Amount={amount}";

                Response.Redirect(url);
            }
        }




    }
}