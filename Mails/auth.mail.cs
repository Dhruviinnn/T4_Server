
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
<html>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>Organization Signup Request</title>
</head>
<body  color:#D1D5DB; font-family:Arial, sans-serif; margin:0; padding:0;'>

  <table role='presentation'  cellspacing='0' cellpadding='0' border='0' align='center'>
    <tr>
      <td align='center' style='padding:15px;'>
        <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' style='
          max-width: 600px;
          background: rgba(19, 32, 50, 0.9);
          backdrop-filter: blur(10px);
          border-radius: 15px;
          overflow:hidden;
          box-shadow: 0px 4px 30px rgba(0, 0, 0, 0.2);
          width: 90%;'>

          <!-- Header -->
          <tr>
            <td style='padding:15px; text-align:center;'>
              <span style='font-size:20px; font-weight:600; color:#fff;'>🔐 Web University</span>
            </td>
          </tr>

          <!-- Banner -->
          <tr>
            <td style='
              background: linear-gradient(to right, #8B5CF6, #6366F1);
              padding:20px;
              text-align:center;
              color:#fff;'>
              <h1 style='font-size:24px; margin:0; font-weight:bold; text-shadow: 2px 2px 4px rgba(0,0,0,0.4);'>
                🚀 Signup Request
              </h1>
            </td>
          </tr>

          <!-- Content -->
          <tr>
            <td style='padding:20px; text-align:center;'>
              <h2 style='color:#fff; font-size:20px; margin-bottom:10px;'>
                Organization Authorization
              </h2>
              
              <p style='
                background: rgba(255,255,255,0.1);
                padding:12px;
                border-radius:8px;
                font-size:20px;
                font-weight:350;
                color:#dfdbeb;
                display:inline-block;'>
                <strong><span style='font-weight: bold; font-size:25px;'>{orgName}</span></strong> is requesting to sign up for our platform.
              </p>

              <p style='font-size:14px; margin-top:12px; color:rgba(255, 255, 255, 0.9);'>
                Please review the request and choose whether to approve or deny this organization's signup request.
              </p>

              <!-- Buttons -->
              <div style='text-align:center; margin-top:20px;'>
                <a href='http://localhost:3000/api/get/auth?id={orgId}&answer=true' 
                   style='
                     display:inline-block;
                     background: linear-gradient(to right, #34D399, #059669);
                     padding:10px 30px;
                     color:#fff;
                     text-decoration:none;
                     font-weight:bold;
                     border-radius:6px;
                     font-size:14px;
                     box-shadow: 0px 4px 15px rgba(52, 211, 153, 0.3);
                     transition: all 0.3s ease;'>
                  ✅ Approve
                </a>

                <a href='http://localhost:3000/api/get/auth?id={orgId}&answer=false' 
                   style='
                     display:inline-block;
                     background: linear-gradient(to right, #EF4444, #991B1B);
                     padding:10px 30px;
                     color:#fff;
                     text-decoration:none;
                     font-weight:bold;
                     border-radius:6px;
                     font-size:14px;
                     box-shadow: 0px 4px 15px rgba(239, 68, 68, 0.3);
                     transition: all 0.3s ease;
                     margin-left:10px;'>
                  ❌ Deny
                </a>
              </div>

              <!-- Logo -->
              <div style='text-align:center; margin-top:20px;'>
                <img src='https://gateway.pinata.cloud/ipfs/bafkreieeqg2h74jc3y5veh4k7ekcyuhkcbgk3wlnuiv6eigflow4x6i3xu' width='60' height='60' alt='Logo'>
              </div>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>

</body>
</html>


";

      MailSender.SendMail(recipients, html, title,"Authentication Mail");
    }
  }
}