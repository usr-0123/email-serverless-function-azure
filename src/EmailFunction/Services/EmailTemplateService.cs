using System.Collections.Generic;

namespace EmailFunction.Services 
{
    public static class EmailTemplateService
    {
        public static string GetTemplate(string templateType, Dictionary<string, string> data)
        {
            return templateType switch
            {
                "booking_configuration" => BookingConfirmation(data),
                "event_reminder" => EventReminder(data),
                "password_reset" => PasswordReset(data),
                _ => DefaultTemplate(data)
            };
        }

        private static string BookingConfirmation(Dictionary<string, string> data)
        {
            return $@"
        <html>
        <body>
            <h2>Event Reminder</h2>
            <p>Hello {data["name"]},</p>
            <p>Your ticket for <b>{data["event"]}</b> has been confirmed.</p>
            <p>Date: {data["date"]}</p>
            <p>Ticket ID: {data["ticketId"]}</p>
        </body>
        </html>";
        }

        private static string EventReminder(Dictionary<string, string> data)
        {
            return $@"
            <html>
            <body>
                <h2>Event Reminder</h2>
                <p>Hello {data["name"]},</p>
                <p>This is a reminder for <b>{data["event"]}</b>.</p>
                <p>Date: {data["date"]}</p>
            </body>
            </html>";
        }

        private static string PasswordReset(Dictionary<string, string> data)
        {
            return $@"
        <html>
        <body>
            <h2>Password Reset</h2>
            <p>Click the link below to reset your password:</p>
            <a href='{data["link"]}'>Reset Password</a>
        </body>
        </html>";
        }

        private static string DefaultTemplate(Dictionary<string, string> data)
        {
            return "<h3>Email notification</h3>";
        }
    }
}