
namespace TimeFourthe.Mails
{
  public class Auth
  {
    public static void Mail(List<string> org)
    {
      Console.WriteLine($"Authentication of {org[1]} has been sent !");
      string orgName = org[1];
      string orgId = org[0];
      string title = "Authentication of Organization";
      string[] recipients = ["timefourthe@gmail.com"];
      string html = @$"<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>Organization Signup Request</title>
</head>
<body>

  <div style='max-width: 600px; margin: 0 auto; background-color: #1F2937; border-radius: 8px; overflow: hidden; padding: 20px;'>
    
    <div style='background: linear-gradient(to right, #6B21A8, #1E3A8A); padding: 21px; border-radius: 12px; margin: 20px 0;'>
      <h1 style='color: #ffffff; font-size: 24px; margin: 0;'>Signup Request</h1>
    </div>

    <h2 style='font-size: 20px; color: #ffffff;'>Organization Authorization</h2>

    <div style='background-color: #1a1f2b; padding: 20px; border-radius: 12px; text-align: center;'>
      <p style='font-size: 16px; margin-bottom: 10px;'>
        <span style='color: #A78BFA; font-weight: bold;'>{orgName}</span> is requesting to sign up for our platform.
      </p>

      <p style='font-size: 14px; margin-bottom: 20px; color: #D1D5DB;'>
        Please review the request and choose whether to approve or deny this organization's signup request.
      </p>

      <!-- Centered Buttons -->
      <div style='display: flex; justify-content: center;'>
        <a href='http://localhost:3000/api/get/auth?id={orgId}&answer=true' 
           style='background: #059669; color: #fff; text-decoration: none; padding: 10px 20px; border-radius: 6px; display: inline-block; margin: 5px;'>
          Approve
        </a>

        <a href='http://localhost:3000/api/get/auth?id={orgId}&answer=false' 
           style='background: #dc2626; color: #fff; text-decoration: none; padding: 10px 20px; border-radius: 6px; display: inline-block; margin: 5px;'>
          Deny
        </a>
      </div>

    </div>

  </div>

</body>
</html>

";

      MailSender.SendMail(recipients, html, title);
    }
  }
}
