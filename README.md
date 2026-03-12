# Serverless Email Sender (Azure Functions + SendGrid)

A lightweight **serverless email microservice** built with **.NET and Azure Functions** that sends templated emails using SendGrid.
This service is designed to be triggered via HTTP requests from applications such as booking systems, ticket platforms, or notification services.

The function receives email details, selects an **HTML template based on a template type**, injects dynamic data, and sends the email.

---

## Features

* Serverless architecture
* Built with **.NET and Azure Functions**
* Email delivery via **SendGrid**
* Dynamic HTML email templates
* Template selection via `templateType`
* Easily extendable for new notification types
* Stateless and scalable

---

## Architecture

```
Client Application
        │
        │ HTTP POST
        ▼
Azure Function (SendBookingEmail)
        │
        │ Select Template
        ▼
Template Service
        │
        │ Generate HTML
        ▼
SendGrid API
        │
        ▼
Recipient Email Inbox
```

This architecture ensures:

* minimal infrastructure management
* automatic scaling
* separation of email logic from the main application

---

## Project Structure

```
EmailFunction
│
├── SendBookingEmail.cs
│
├── Models
│   └── EmailRequest.cs
│
├── Services
│   └── EmailTemplateService.cs
│
├── host.json
├── local.settings.json
└── EmailFunction.csproj
```

---

## Request Payload

The function expects a JSON payload containing the email metadata and template data.

```json
{
  "from": "tickets@events.com",
  "to": "user@email.com",
  "subject": "Booking Confirmed",
  "templateType": "booking_confirmation",
  "data": {
    "name": "Ian",
    "event": "Tech Summit",
    "date": "June 20",
    "ticketId": "834292"
  }
}
```

### Fields

| Field        | Description                                      |
| ------------ | ------------------------------------------------ |
| from         | Sender email address                             |
| to           | Recipient email address                          |
| subject      | Email subject                                    |
| templateType | Determines which template will be used           |
| data         | Key-value object injected into the HTML template |

---

## Supported Template Types

| Template Type          | Purpose                                       |
| ---------------------- | --------------------------------------------- |
| `booking_confirmation` | Sent after a user successfully books an event |
| `event_reminder`       | Reminder email before an event                |
| `password_reset`       | Password reset email                          |

New templates can be added easily in the **EmailTemplateService**.

---

## Local Development

### Prerequisites

* .NET 8 SDK
* Azure Functions Core Tools
* Azure CLI
* SendGrid account

---

### Install Dependencies

```
dotnet restore
```

---

### Configure Environment Variables

Edit `local.settings.json`.

```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "SENDGRID_API_KEY": "YOUR_SENDGRID_API_KEY"
  }
}
```

---

### Run the Function Locally

```
func start
```

The function will be available at:

```
http://localhost:7071/api/SendBookingEmail
```

---

## Testing the API

Example request using curl:

```
curl -X POST http://localhost:7071/api/SendBookingEmail \
-H "Content-Type: application/json" \
-d '{
  "from":"tickets@events.com",
  "to":"user@email.com",
  "subject":"Booking Confirmed",
  "templateType":"booking_confirmation",
  "data":{
      "name":"Ian",
      "event":"Tech Summit",
      "date":"June 20",
      "ticketId":"834292"
  }
}'
```

---

## Deployment

Login to Azure:

```
az login
```

Deploy the function:

```
func azure functionapp publish <FUNCTION_APP_NAME>
```

After deployment the endpoint will be available at:

```
https://<FUNCTION_APP_NAME>.azurewebsites.net/api/SendBookingEmail
```

---

## Extending the Service

To add a new email type:

1. Add a new template method in `EmailTemplateService`
2. Add the template key to the template selector
3. Send the new `templateType` in the request

Example:

```
"templateType": "payment_receipt"
```

---

## Use Cases

This service can be used for:

* booking confirmations
* event reminders
* payment receipts
* password reset emails
* welcome emails
* system notifications

---

## Benefits of Serverless Email Services

* automatic scaling
* reduced infrastructure management
* cost efficiency (pay-per-execution)
* easy integration with existing systems
* stateless and fault-tolerant design

---

## License

MIT License
