
namespace TimeFourthe.Mails
{
    public class ApprovalSuccess
    {
        public static void MailOtp(string orgName,string email)
        {
            string title = "Authentication of Organization";
            string[] recipients = [email];
            string html = @$"<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Approval Successful</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #121212;
            margin: 0;
            padding: 20px;
            color: #E0E0E0;
        }}
        .email-container {{
            background-color: #1F1F1F;
            border-radius: 8px;
            padding: 30px;
            max-width: 600px;
            margin: 0 auto;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
        }}
        h2 {{
            color: #FFFFFF;
            text-align: center;
        }}
        p {{
            font-size: 16px;
            line-height: 1.5;
            color: #D1D1D1;
        }}
        ul {{
            color: #B0B0B0;
            padding-left: 20px;
        }}
        li {{
            margin-bottom: 8px;
        }}
        .button {{
            display: inline-block;
            background-color: #4CAF50;
            color: white;
            padding: 12px 24px;
            font-size: 16px;
            text-decoration: none;
            border-radius: 4px;
            text-align: center;
            margin-top: 20px;
            transition: background-color 0.3s ease;
        }}
        .button:hover {{
            background-color: #45a049;
        }}
        .footer {{
            text-align: center;
            font-size: 12px;
            color: #888;
            margin-top: 20px;
        }}
        .footer a {{
            color: #4CAF50;
            text-decoration: none;
        }}
    </style>
</head>
<body>

    <div class='email-container'>
        <h2>Approval Successfully Granted</h2>
        <p>Dear [User's Name],</p>

        <p>We are excited to inform you that your request has been successfully approved. Here are the details of your request:</p>
        
        <ul>
            <li><strong>Requestor Name:</strong> [User's Name]</li>
            <li><strong>Request Details:</strong> [Request Details]</li>
            <li><strong>Approval Date:</strong> [Approval Date]</li>
        </ul>
        
        <p>Now that your request has been approved, you can proceed with the next steps. Please click the button below to log in to your account and take further action:</p>

        <p><a href='https://yourloginpage.com' class='button'>Go to Login Page</a></p>

        <p>If you need any further assistance or have questions, feel free to reach out to us.</p>

        <p>Thank you for your patience, and we look forward to your continued engagement with us!</p>

        <p>Best regards,</p>
        <p>[Your Organization Name]</p>

        <div class='footer'>
            <p>© [Current Year] [Your Organization]. All rights reserved. <br><a href='https://yourwebsite.com'>Visit Our Website</a></p>
        </div>
    </div>

</body>
</html>
";

            MailSender.SendMail(recipients, html, title);
        }
    }
}
