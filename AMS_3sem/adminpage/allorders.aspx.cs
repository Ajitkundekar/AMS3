using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Data;

namespace AMS_3sem.adminpage
{
    public partial class allorders : System.Web.UI.Page
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
            using (SqlConnection connection = new SqlConnection("Data Source=AK\\SQLEXPRESS;Initial Catalog=AMS;Integrated Security=True"))
            {
                string query = "SELECT * FROM tbl_orders";
                SqlCommand command = new SqlCommand(query, connection);

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
                        ScriptManager.RegisterStartupScript(this, GetType(), "NoRowsAlert", "alert('No orders found.');", true);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErrorAlert", $"alert('Error fetching orders: {ex.Message}');", true);
                }
            }
        }

        protected void confirmDeliveryButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            RepeaterItem item = (RepeaterItem)btn.NamingContainer;

            HiddenField orderNoField = (HiddenField)item.FindControl("OrderNoHiddenField");
            HiddenField statusField = (HiddenField)item.FindControl("StatusHiddenField");
            string state;
            string orderno = orderNoField.Value;
            string status = statusField.Value;

            state = status == "1" ? "2" : "1";

            bool updateSuccess = UpdateStatus(orderno, state);

            if (updateSuccess)
            {
                Response.Redirect(Request.Url.ToString(), true);
            }
            else
            {
                // Update failed, handle the error or show an error message
                ScriptManager.RegisterStartupScript(this, GetType(), "UpdateErrorAlert", "alert('Failed to update the status.');", true);
            }
        }

        protected void mailsend(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            RepeaterItem item = (RepeaterItem)btn.NamingContainer;

            HiddenField emailhiddenfield = (HiddenField)item.FindControl("emailhiddenfield");
            HiddenField ordernofield = (HiddenField)item.FindControl("ordernofield");
            HiddenField datefield = (HiddenField)item.FindControl("datefield");
            HiddenField Productfield = (HiddenField)item.FindControl("Productfield");
            HiddenField Amountfield = (HiddenField)item.FindControl("Amountfield");
            HiddenField customernamefield = (HiddenField)item.FindControl("customernamefield");

            string email = emailhiddenfield.Value;
            string orderno = ordernofield.Value;
            string date = datefield.Value;
            string Product = Productfield.Value;
            string Amount = Amountfield.Value;
            string customername = customernamefield.Value;
            string resetLink = $"{Request.Url.GetLeftPart(UriPartial.Authority)}/payment.aspx?orderno={orderno}&Amount={Amount}";

            MailMessage message = new MailMessage
            {
                To = { email },
                From = new MailAddress("amsbyajit@gmail.com"),
                Subject = "Congratulations on Winning the Auction!",
                Body = $@"
                <body>
                    <p>Dear {customername},</p>
                    <p>Congratulations on winning the auction! We are thrilled to inform you that you have successfully won the bid.</p>
                    <p>Details of your winning bid:</p>
                    <ul>
                        <li><strong>Auction Date:</strong> {date}</li>
                        <li><strong>Order Number:</strong> {orderno}</li>
                        <li><strong>Product Name:</strong> {Product}</li>
                        <li><strong>Price:</strong> {Amount}</li>
                    </ul>
                    <p>To pay click the below link:</p>
                    <p><strong>{resetLink}</strong></p>
                    <p>Please pay the amount and collect your product. Our office will send the product to your respective address.</p>
                    <p>We appreciate your participation and hope you enjoy your new purchase.</p>
                    <p>Should you have any questions or need further assistance, please do not hesitate to contact us.</p>
                    <p><strong>[At-Mouje Shirgaon tal-chandgad Dist-Kolhapur<br>pin-416509<br>contact:02320-000000]</strong></p>
                    <p>Thanks & Regards,<br>Auction Management System</p>
                </body>",
                IsBodyHtml = true
            };

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new System.Net.NetworkCredential("amsbyajit@gmail.com", "hnafplsmztphlywx"),
                EnableSsl = true
            };

            try
            {
                smtp.Send(message);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('Mail sent to user.');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", $"alert('Error sending email: {ex.Message}');", true);
            }
        }

        private bool UpdateStatus(string orderno, string state)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=AK\\SQLEXPRESS;Initial Catalog=AMS;Integrated Security=True"))
                {
                    connection.Open();

                    string updateQuery = "UPDATE tbl_orders SET Status = @state WHERE orderno = @orderno";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@orderno", orderno);
                        command.Parameters.AddWithValue("@state", state);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", $"alert('Error updating status: {ex.Message}');", true);
                return false;
            }
        }

        protected void btnDownloadReport_Click(object sender, EventArgs e)
        {
            ReportDocument reportDocument = new ReportDocument();
            try
            {
                // Your SQL query
                string query = "SELECT * FROM tbl_orders"; // Replace with your actual query

                // Execute the query and load the data into a DataTable
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection("Data Source = AK\\SQLEXPRESS; Initial Catalog = AMS; Integrated Security = True"))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }

                // Load the report
                reportDocument.Load(Server.MapPath("~/adminpage/allorder.rpt"));

                // Set the data source for the report
                reportDocument.SetDataSource(dt);

                // Export the report to a byte array
                byte[] reportBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    reportDocument.ExportToStream(ExportFormatType.PortableDocFormat).CopyTo(memoryStream);
                    reportBytes = memoryStream.ToArray();
                }

                // Send the report to the browser
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=allorder.pdf");
                Response.BinaryWrite(reportBytes);
                Response.End();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", $"alert('Error generating report: {ex.Message}');", true);
            }
            finally
            {
                reportDocument.Close();
                reportDocument.Dispose();
            }
        }


        protected void btnDownloadReport_Click0(object sender, EventArgs e)
{
    ReportDocument reportDocument = new ReportDocument();
    try
    {
        // Your SQL query to get records where Address and transactionid are null
        string query = "SELECT * FROM tbl_orders WHERE Address IS NULL AND transactionid IS NULL";

        // Execute the query and load the data into a DataTable
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection("Data Source = AK\\SQLEXPRESS; Initial Catalog = AMS; Integrated Security = True"))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }

        // Load the report
        reportDocument.Load(Server.MapPath("~/adminpage/allorder.rpt"));

        // Set the data source for the report
        reportDocument.SetDataSource(dt);

        // Export the report to a byte array
        byte[] reportBytes;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            reportDocument.ExportToStream(ExportFormatType.PortableDocFormat).CopyTo(memoryStream);
            reportBytes = memoryStream.ToArray();
        }

        // Send the report to the browser
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=allorder.pdf");
        Response.BinaryWrite(reportBytes);
        Response.End();
    }
    catch (Exception ex)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", $"alert('Error generating report: {ex.Message}');", true);
    }
    finally
    {
        reportDocument.Close();
        reportDocument.Dispose();
    }
}

    }
}
