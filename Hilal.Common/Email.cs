using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Hilal.Common
{
    public class Email
    {
        public static bool SendEmail(string subject, string body, string from, string password, string to)
        {
            try
            {
                bool res = false;

                string[] tempfrom = from.Split('@');

                MailMessage mail = new MailMessage();
                mail.To.Add(to);
                mail.From = new MailAddress(from, "FalconOffer", Encoding.UTF8);
                mail.Subject = subject;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.Body = body;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                SmtpClient client = new SmtpClient();
                //Add the Creddentials- use your own email id and password

                client.Credentials = new System.Net.NetworkCredential(from, password);

                // Gmail works on this port
                if (tempfrom[1] == "gmail.com")
                {
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                }
                else if (tempfrom[1] == "yahoo.com")
                {
                    client.Port = 465;
                    client.Host = "smtp.mail.yahoo.com";
                }
                client.EnableSsl = true; //Gmail works on Server Secured Layer
                client.Send(mail);
                res = true;
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
