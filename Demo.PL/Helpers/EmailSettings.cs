using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var Client = new SmtpClient("smtp.gmail.com", 587);
            Client.EnableSsl = true;// Must SSL Certificate installed in the server!
            /*
             * MyMvcFirstApplication
             * ufgcliunjylgpnch
             */
            Client.Credentials = new NetworkCredential("yahiadakhly2004@gmail.com", "ufgcliunjylgpnch");
            Client.Send("yahiadakhly2004@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
