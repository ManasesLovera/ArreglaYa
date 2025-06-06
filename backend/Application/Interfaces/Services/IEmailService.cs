using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    /// <summary>
    /// Defines an abstraction for sending emails using different providers (e.g., SMTP, SendGrid).
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email asynchronously to the specified recipient.
        /// </summary>
        /// <param name="to">The recipient email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The HTML body content of the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendEmailAsync(string to, string subject, string body);
    }
}