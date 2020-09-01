using System;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.Text;

namespace EmailSlicer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter To Address:");
            string to = Console.ReadLine().Trim();

            string emailUsername = ""; // Username string.
            string emailDomeinName = ""; // Domein name string.

            StringBuilder sb = new StringBuilder();

            for(int i = 0; i <= to.Length; i++)
            {
                if(to[i] != '@')
                {
                    sb.Insert(i, to[i]);
                } else if(to[i] == '@')
                {
                    break;
                }
            }

            emailUsername = sb.ToString();
            sb.Clear();
            bool flag = false;
            int j = 0;
            for (int i = 0; i <= to.Length; i++)
            {
                if (to[i] != '@' && flag == false)
                {
                    continue;
                }
                else if (to[i] == '@')
                {
                    flag = true;
                    continue;
                }
                else if (flag == true)
                {
                    sb.Insert(j, to[i]);
                    j++;
                    if (i == to.Length-1) break;
                }
            }

            emailDomeinName = sb.ToString();

            Console.WriteLine(emailUsername);
            Console.WriteLine(emailDomeinName);

            Console.WriteLine("Enter Subject:");
            string subject = Console.ReadLine().Trim();

            Console.WriteLine("Enter Body:");
            string body = Console.ReadLine().Trim();
            body += " Your username is " + emailUsername + " in domein " + emailDomeinName;

            using (MailMessage mm = new MailMessage(ConfigurationManager.AppSettings["FromEmail"], to))
            {
                mm.Subject = subject;
                mm.Body = body;
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["Host"];
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                NetworkCredential NetworkCred = new NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
                smtp.Credentials = NetworkCred;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                Console.WriteLine("Sending Email......");
                smtp.Send(mm);
                System.Threading.Thread.Sleep(3000);
                Console.WriteLine("Email Sent.");
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(0);
            }
        }
    }
}
