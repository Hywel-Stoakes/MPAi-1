using System;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace MPAi.Cores
{
    /// <summary>
    /// Class to handle email sending operations.
    /// </summary>
    class MailSender
    {
        private bool IsEmailValid;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MailSender() {}
        /// <summary>
        /// Verifies that the input string is a valid email address.
        /// </summary>
        /// <param name="strIn">The string to validate.</param>
        /// <returns>Boolean true if the string is a valid email, false otherwise</returns>
        public bool ValidateEmail(string strIn)
        {
            IsEmailValid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (IsEmailValid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        /// <summary>
        /// Creates a valid ASCII domain name out of a Unicode one.
        /// </summary>
        /// <param name="match">Used by the Regex class.</param>
        /// <returns>A valid domain, if one can be generated.</returns>
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                IsEmailValid = true;    // This causes ValidateEmail to fail.
            }
            return match.Groups[1].Value + domainName;
        }
        /// <summary>
        /// Overload of the Send method allowing string values to be input.
        /// These string values are used to create a new MailMessage, which is put into the other Send method.
        /// </summary>
        /// <param name="from">The sender, as a string.</param>
        /// <param name="to">The recipient, as a string.</param>
        /// <param name="caption">The subject, as a string.</param>
        /// <param name="content">The content of the message, as a string.</param>
        public void Send(string from, string to, string caption, string content)
        {
            Send(new MailMessage(from, to, caption, content));
        }
        /// <summary>
        /// Invokes the SMTP server to send the input message.
        /// </summary>
        /// <param name="msg">The message to send, as a MailMessage object.</param>
        public void Send(MailMessage msg)
        {
            using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
            {
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("MPAicustomer@gmail.com", "Crusader1");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(msg);
            } 
        }
    }
}
