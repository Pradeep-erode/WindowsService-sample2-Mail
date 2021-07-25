using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace WindowsServiceProject1
{
    class SendMailService
    {
        // This function write log to LogFile.text when some error occurs.  
        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        // This function write Message to log file.
        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        // This function contains the logic to send mail.
        public static void SendEmail(String ToEmail, String Subj, string Message)
        {
            try
            {
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 200000;
                MailMessage MailMsg = new MailMessage();
                System.Net.Mime.ContentType HTMLType = new System.Net.Mime.ContentType("text/html");

                string strBody = "This is a test mail.";

                MailMsg.BodyEncoding = System.Text.Encoding.Default;
                MailMsg.To.Add(ToEmail);
                MailMsg.Priority = System.Net.Mail.MailPriority.High;
                MailMsg.Subject = "Subject - Window Service";
                MailMsg.Body = strBody;
                MailMsg.IsBodyHtml = true;
                AlternateView HTMLView = AlternateView.CreateAlternateViewFromString(strBody, HTMLType);

                smtpClient.Send(MailMsg);
                WriteErrorLog("Mail sent successfully!");
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message);
                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SimpleService"))
                {
                    serviceController.Stop();
                }
                throw;
            }
        }
    }
}
