
namespace TimeFourthe.Mails
{
    public class Auth
    {
        public static void MailOtp(List<string> org)
        {
            string orgName = org[1];
            string orgId=org[0];
            string title = "Authentication of Organization";
            string[] recipients = ["timeforthe@gmail.com"];
            string html = @$"<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>Organization Signup Request</title>
  <style>
    /* Reset styles */
    body, p, h1, h2, div {{
      margin: 0;
      padding: 0;
      box-sizing: border-box;
    }}

    body {{
      font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
      background-color: #111827;
      color: #D1D5DB;
      line-height: 1.5;
      padding: 24px;
    }}

    .container {{
      max-width: 600px;
      margin: 0 auto;
      background-color: #1F2937;
      border-radius: 8px;
      overflow: hidden;
    }}

    .header {{
      padding: 24px;
      border-bottom: 1px solid #374151;
      display: flex;
      justify-content: space-between;
      align-items: center;
    }}

    .logo {{
      display: flex;
      align-items: center;
      gap: 8px;
      color: #fff;
      text-decoration: none;
    }}

    .logo-text {{
      font-size: 20px;
      font-weight: 600;
      color: #fff;
    }}

    .portal-link {{
      color: #9CA3AF;
      text-decoration: none;
      padding: 8px 16px;
      border-radius: 6px;
      transition: all 0.2s ease;
    }}

    .portal-link:hover {{
      color: #fff;
      background-color: #374151;
    }}

    .banner {{
      background: linear-gradient(to right, #6B21A8, #1E3A8A);
      padding: 21px 24px;
      margin: 24px 0;
      border-radius: 12px;
    }}

    .banner-title {{
      font-size: 36px;
      font-weight: bold;
      color: #fff;
    }}

    .content {{
      padding: 0 24px 24px;
    }}

    .subtitle {{
      font-size: 24px;
      font-weight: 600;
      color: #fff;
      margin-bottom: 24px;
      margin-left: 7px;
    }}

    .text {{
      background-color: #1a1f2b;
      padding: 24px;
      border-radius: 12px;
    }}

    .highlight {{
      color: #A78BFA;
      font-weight: 500;
    }}

    .paragraph {{
      margin-bottom: 16px;
      text-align: center;
      font-size: 18px;
    }}

    .paragraph:last-child {{
      margin-bottom: 0;
    }}

    .signature {{
      padding-top: 24px;
      border-top: 1px solid #374151;
      margin-top: 32px;
      text-align: center;
    }}

    .button-container {{
      display: flex;
      justify-content: center;
      gap: 16px;
      margin-top: 24px;
    }}

    .action-button {{
      display: inline-block;
      color: white;
      text-decoration: none;
      padding: 12px 36px;
      border-radius: 6px;
      font-weight: 500;
      transition: opacity 0.2s ease;
    }}

    .action-button:hover {{
      opacity: 0.9;
    }}

    .action-button.yes {{
      background: linear-gradient(to right, #059669, #065f46);
    }}

    .action-button.no {{
      background: linear-gradient(to right, #dc2626, #991b1b);
    }}

    #t4logo{{
      width: 60px;
      height: 60px;
    }}

    @media (max-width: 640px) {{
      body {{
        padding: 12px;
      }}

      .container {{
        border-radius: 12px;
      }}

      .header {{
        padding: 16px;
      }}

      .banner {{
        margin: 16px;
        padding: 24px 20px;
      }}

      .banner-title {{
        font-size: 28px;
      }}

      .content {{
        padding: 0 16px 16px;
      }}

      .subtitle {{
        font-size: 20px;
        margin-bottom: 20px;
      }}

      .text {{
        padding: 20px;
      }}

      .button-container {{
        flex-direction: column;
      }}

      .action-button {{
        width: 100%;
        text-align: center;
      }}
    }}
  </style>
</head>
<body>
  <div class='container'>
    <!-- Header -->
    <div class='header'>
      <a href='#' class='logo'>
        <p class='logo-text'>Web University</p>
      </a>
    </div>

    <!-- Main Content -->
    <div class='content'>
      <div class='banner'>
        <h1 class='banner-title'>Signup Request</h1>
      </div>

      <h2 class='subtitle'>Organization Authorization</h2>

      <div class='text'>
        <p class='paragraph'>
          <span class='highlight'>{orgName}</span> is requesting to sign up for our platform.
        </p>

        <p class='paragraph'>
          Please review the request and choose whether to approve or deny this organization's signup request.
        </p>

        <div class='button-container'>
          <a href='http://localhost:5140/api/get/auth?id={orgId}&answer=true' class='action-button yes'>
            Approve
          </a>
          <a href='http://localhost:5140/api/get/auth?id={orgId}&answer=false' class='action-button no'>
            Deny
          </a>
        </div>

        <div class='signature'>
          <img id='t4logo' src='../Assets/Logo.png'></img>
        </div>
      </div>
    </div>
  </div>
</body>
</html>";

            MailSender.SendMail(recipients, html, title);
        }
    }
}
