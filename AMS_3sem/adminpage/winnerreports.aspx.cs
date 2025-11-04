using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS_3sem.adminpage
{
    public partial class winnerreports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateWinnerReportsTable();
        }

        protected void PopulateWinnerReportsTable()
        {
            string connectionString = "Data Source=AK\\SQLEXPRESS; Initial Catalog=AMS; Integrated Security=True";
            string query = @"
        SELECT 
            wr.AuctionItemId, 
            u.fullname AS WinnerName, 
            u.mobile AS Contact, 
            ai.ProductName AS Product, 
            ai.MinPrice AS MinPrice, 
            wr.WinningBidAmount AS WinningPrice,
            ai.FileName 
        FROM 
             WinnerReports wr
            INNER JOIN tbl_user u ON wr.WinningBidder = u.uid
            INNER JOIN AuctionItems ai ON wr.AuctionItemId = ai.AuctionItemId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    WinnerreportDataTable.DataSource = dataTable;
                    WinnerreportDataTable.DataBind();
                }
            }
        }



        protected void btnorder_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=AK\\SQLEXPRESS; Initial Catalog=AMS; Integrated Security=True";
            string insertQuery = @"
    INSERT INTO tbl_orders (Date,CustomerName, EmailAddress, PhoneNumber, Product, Amount, Status, aid)
    SELECT 
        GETDATE() AS Date, 
       
        u.fullname AS CustomerName, 
        u.email AS EmailAddress,  
        u.mobile AS PhoneNumber, 
        ai.ProductName AS Product, 
        wr.WinningBidAmount AS Amount,
        '1' AS Status,
        u.uid AS aid
    FROM 
        WinnerReports wr
    INNER JOIN 
        tbl_user u ON wr.WinningBidder = u.uid
    INNER JOIN 
        AuctionItems ai ON wr.AuctionItemId = ai.AuctionItemId
    WHERE 
        NOT EXISTS (
            SELECT 1 
            FROM tbl_orders o 
            WHERE 
                o.Amount = wr.WinningBidAmount 
                AND o.Product = ai.ProductName 
                AND o.CustomerName = u.fullname
        )";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // Optionally, display a message indicating success or failure
                    if (rowsAffected > 0)
                    {
                        // Insert was successful
                        exequery.Text = "Records inserted successfully.";
                    }
                    else
                    {
                        // No records were inserted
                        exequery.Text = "No records were inserted.";
                    }
                }
            }

        }


        protected void btnDownloadReport_Click(object sender, EventArgs e)
        {
            ReportDocument reportDocument = new ReportDocument();
            try
            {
                // Your SQL query
                string query = @"SELECT
    wr.AuctionItemId, 
    u.fullname AS WinnerName, 
    u.mobile AS Contact, 
    ai.ProductName AS Product, 
    ai.MinPrice AS MinPrice, 
    wr.WinningBidAmount AS WinningPrice,
    ai.FileNamehttps://localhost:44342/adminpage/winnerreports.aspx.cs
FROM
    WinnerReports wr
INNER JOIN
    tbl_user u ON wr.WinningBidder = u.uid
INNER JOIN
    AuctionItems ai ON wr.AuctionItemId = ai.AuctionItemId";
                // Replace with your actual query
                ;


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
                reportDocument.Load(Server.MapPath("~/adminpage/winner1.rpt"));

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