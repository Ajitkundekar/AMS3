using System;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web.UI;



namespace AMS_3sem
{
    public partial class Forgotpass1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        string connectionString = "Data Source=AK\\SQLEXPRESS;Initial Catalog=AMS;Integrated Security=True;";

        private bool IsEmailExists(string email)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT email FROM tbl_user WHERE email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim().ToString();

            if (IsEmailExists(email))
            {
                string resetToken = Guid.NewGuid().ToString();

                UpdateResetToken(email, resetToken);

                SendPasswordResetLink(email, resetToken);

                Page.ClientScript.RegisterStartupScript(this.GetType(), "toasterScript", "showToaster('Link sent to your email address..' , 'blue')", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toasterScript", "showToaster('Please enter a valid email address.' , 'red')", true);
            }
        }

        private void UpdateResetToken(string email, string resetToken)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE tbl_user SET password_reset_token = @Token WHERE email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Token", resetToken);
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void SendPasswordResetLink(string email, string resetToken)
        {
            string resetLink = $"{Request.Url.GetLeftPart(UriPartial.Authority)}/UpdatePassword.aspx?email={email}&token={resetToken}";

            MailMessage message = new MailMessage
            {
                To = { email },
                From = new MailAddress("amsbyajit@gmail.com"),
                Subject = "Password Reset Link (Auction Management System)",
                Body = $"Dear User,\n\nPlease click on the following link to reset your password:\n{resetLink}\n\nThanks & Regards\nAuction Management System"
            };

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("amsbyajit@gmail.com", "hnafplsmztphlywx");
            smtp.EnableSsl = true;



            try
            {
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toasterScript", "showToaster('Error:- ' " + ex + ", 'red')", true);
            }
        }
    }
}

