using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using QRCoder;

namespace AMS_3sem
{
    public partial class payment : System.Web.UI.Page
    {
        private static bool isQRCodeScanned = false;
        private string orderno;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Retrieve OrderID from the URL
                orderno = Request.QueryString["orderno"];
                if (string.IsNullOrEmpty(orderno))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "toasterScript", "showToaster('Order ID is missing in the URL.', 'red')", true);
                    return;
                }

             
                btnGenerateQr_Click();
            }
        }

        protected void btnGenerateQr_Click()
        {
            if (isQRCodeScanned)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toasterScript", "showToaster('QR Code already scanned.', 'red')", true);
                return;
            }

            string upiId = "8275008417@ybl";
            string amount = orderno = Request.QueryString["Amount"];

            string orderId = Request.QueryString["orderno"];


            if (string.IsNullOrEmpty(upiId) || string.IsNullOrEmpty(amount))
            {
                // Handle validation error
                return;
            }

            // Generate the UPI string with the OrderID
            string upiString = $"upi://pay?pa={upiId}&pn=YourName&am={amount}";

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(upiString, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            qrCodeImage.Save(ms, ImageFormat.Png);
                            byte[] byteImage = ms.ToArray();
                            string base64Image = Convert.ToBase64String(byteImage);
                            imgQrCode.ImageUrl = "data:image/png;base64," + base64Image;
                            imgQrCode.Visible = true;
                            btnConfirmPayment.Visible = true; // Show the Confirm Payment button
                        }
                    }
                }
            }
        }

        protected void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            string Address = txtAddress.Text.Trim();
            string Transactionid = txttId.Text.Trim();

            string orderId = Request.QueryString["orderno"];


            if (string.IsNullOrEmpty(Transactionid) || string.IsNullOrEmpty(Address))
            {
                // Handle validation error
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toasterScript", "showToaster('fill the field!', 'red')", true);

                return;
            }

            // Simulate payment confirmation
            bool paymentSuccessful = true; // Here, you assume the payment was successful


            if (paymentSuccessful)
            {
                update_address(Address, Transactionid, orderId);
                   isQRCodeScanned = true; // Set the flag to true after first scan/confirmation
                
                DestroyQRCode();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toasterScript", "showToaster('Payment confirmed successfully!', 'green')", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toasterScript", "showToaster('Payment confirmation failed. Please try again.', 'red')", true);
            }
        }


         protected void update_address(string address, string id ,string orderId)
        {
            string connectionString = "Data Source=AK\\SQLEXPRESS; Initial Catalog=AMS; Integrated Security=True";
            string updateQuery = @"UPDATE tbl_orders SET Address = @address  , transactionid=@id  WHERE OrderNo = @orderId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@orderId", orderId);
                    command.Parameters.AddWithValue("@id", id);



                    int rowsAffected = command.ExecuteNonQuery();

                    // Optionally, display a message indicating success or failure
                    if (rowsAffected > 0)
                    {
                        // Insert was successful
                        //  exequery.Text = "Records inserted successfully.";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "toasterScript", "showToaster('transaction is complete', 'red')", true);


                    }
                    else
                    {
                        // No records were inserted
                        //exequery.Text = "No records were inserted.";

                    }
                }
            }
        }

        private void DestroyQRCode()
        {
            imgQrCode.ImageUrl = string.Empty; // Clear the QR code image
            imgQrCode.Visible = false; // Hide the QR code
            btnConfirmPayment.Visible = false; // Hide the Confirm Payment button
        }
    }
}
