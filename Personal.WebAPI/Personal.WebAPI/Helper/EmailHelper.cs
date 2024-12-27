using Personal.WebAPI.Configurations;
using System.Net.Mail;

namespace Personal.WebAPI.Helper
{
    public class EmailHelper
    {
        private SmtpConfig _smtpConfig;
        public EmailHelper(SmtpConfig smtpConfig) 
        {
            _smtpConfig = smtpConfig;
        }

        public void SendEmailForgotPassword(string username, string email, string temppass)
        {
            try
            {
                SmtpClient mailServer = new SmtpClient(_smtpConfig.smtpHost, _smtpConfig.smtpPort);
                mailServer.EnableSsl = true;
                mailServer.Credentials = new System.Net.NetworkCredential(_smtpConfig.username, _smtpConfig.password);

                string from = _smtpConfig.username;
                string to = email;
                MailMessage msg = new MailMessage(from, to);
                msg.Subject = "[Test Account] Password Reset for Test Login";
                msg.Body = "<html><body><p>Greetings,<br><br>" +
                    "A password reset request was made in your account. Below are your credentials: <br><br><b>" +
                    "Username : </b> " + username + "<br><b> " +
                    "Temporary Password: </b> " + temppass +
                    "</span><br><br>Please login to your new account using " + _smtpConfig.loginUrl
                    + ".</p></body></html>";
                msg.IsBodyHtml = true;
                mailServer.Send(msg);
                mailServer.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void SendEmailPassword(string username, string email, string temppass)
        {
            try
            {
                SmtpClient mailServer = new SmtpClient(_smtpConfig.smtpHost, _smtpConfig.smtpPort);
                mailServer.EnableSsl = true;
                mailServer.Credentials = new System.Net.NetworkCredential(_smtpConfig.username, _smtpConfig.password);

                string from = _smtpConfig.username;
                string to = email;
                MailMessage msg = new MailMessage(from, to);
                msg.Subject = "[Test Account] Temporary Password for Test Login";
                msg.Body = "<html><body><p>Greetings,<br><br>" +
                    "Your account received a request for a temporary password. Below are your credentials: <br><br><b>" +
                    "Username : </b> " + username + "<br><b> " +
                    "Temporary Password: </b> " + temppass +
                    "</span><br><br>Please login to your new account using " + _smtpConfig.loginUrl
                    + ".</p></body></html>";
                msg.IsBodyHtml = true;
                mailServer.Send(msg);
                mailServer.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
