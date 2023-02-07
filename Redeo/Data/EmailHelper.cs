using System.Net.Mail;

namespace Redeo.Data
{
    public class EmailHelper
    {
        public bool SendEmailConfirmation(string userEmail, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("ek51840@ubt-uni.net");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<h4>Hello " + userEmail + ",</h4><br><strong><p>Thank you for joing Redeo<br>Please verify your email address to continue!</p></strong><br><a href="+ confirmationLink + ">Verify Email Address </a><br><h4>Thank you,</h4><h5>Redeo Team</h5>";

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("ek51840@ubt-uni.net", "ek-UBT2020");
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                try
                {
                    client.Send(mailMessage);
                    return true;
                }
                catch (Exception ex)
                {

                }
                return false;
            }
        }

        public bool SendEmailPasswordReset(string userEmail, string link)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("ek51840@ubt-uni.net");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Password Reset";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<h4>Hello " + userEmail + ",</h4><br><strong><p>A request has been received to change your password for your Redeo account.</p></strong><br><a href=" + link +">Reset Password</a><br><span>If you did not initiate this request, please contact us immediately at <a href=\"mailto:contact@redeo.tv\">contact@redeo.tv</a></span><br><h4>Thank you,</h4><h5>Redeo Team</h5>";

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("ek51840@ubt-uni.net", "ek-UBT2020");
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                try
                {
                    client.Send(mailMessage);
                    return true;
                }
                catch (Exception ex)
                {

                }
                return false;
            }
        }
    }
}
