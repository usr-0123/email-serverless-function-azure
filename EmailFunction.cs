using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using EmailFunction.Models;
using EmailFunction.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EmailFunction
{
    public static class EmailFunction
    {
        [FunctionName("EmailFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                EmailRequest data = JsonConvert.DeserializeObject<EmailRequest>(requestBody);
                
                string htmlTemplate = EmailTemplateService.GetTemplate(data.TemplateType, data.Data);
                
                var client = new SendGridClient(System.Environment.GetEnvironmentVariable("SENDGRID_API_KEY"));
                
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(data.From),
                    Subject = data.Subject,
                    HtmlContent = htmlTemplate
                };
                
                msg.AddTo(new EmailAddress(data.To));
                
                var response = await client.SendEmailAsync(msg);
                
                Console.WriteLine($"Message: {msg}");
                
                Console.Write($"Email sent successfully: {response}");

                return new OkObjectResult("Email sent successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An unexpected error occurred: {e.Message}");
                return new InternalServerErrorResult();
            }
        }
    }
}
