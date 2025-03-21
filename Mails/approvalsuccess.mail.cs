
namespace TimeFourthe.Mails
{
    public class ApprovalSuccess
    {
        public static void Mail(string orgName, string email)
        {
            string title = "Authentication of Organization";
            string[] recipients = ["vasavadhruvin123@gmail.com"];
            string html = @$"<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>Request Approval</title>
</head>
<body style='color:#222; font-family:Arial, sans-serif; margin:0; padding:0; background:#f5f5f5;'>
  <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' align='center'>
    <tr>
      <td align='center' style='padding:15px;'>
        <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' style='
          max-width: 600px;
          background: #fff;
          border-radius: 15px;
          overflow:hidden;
          box-shadow: 0px 4px 30px rgba(0, 0, 0, 0.1);
          width: 90%;'>

          <!-- Header -->
          <tr>
            <td style='padding:15px; text-align:center;'>
              <span style='font-size:20px; font-weight:600; color:#222;'>✅ Web University</span>
            </td>
          </tr>

          <tr>
            <td style='background: #ddd; padding:20px; text-align:center; color:#222;'>
              <h1 style='font-size:24px; margin:0; font-weight:bold;'>🎉 Approval Successful</h1>
            </td>
          </tr>

          <tr>
            <td style='padding:20px; text-align:center;'>
              <h2 style='color:#222; font-size:20px; margin-bottom:10px;'>Request Approval Confirmation</h2>
              
              <p style='background: #eee; padding:12px; border-radius:8px; font-size:16px; font-weight:500; color:#222; display:inline-block;'>
                Dear <span style='color:#6366F1; font-weight:600;'>[User's Name]</span>, your request has been successfully approved.
              </p>

              <div style='background:#f0f0f0; padding:20px; border-radius:12px; text-align:left;'>
                <p style='font-size:14px; margin-bottom:10px;'>✅ <b>Requestor Name:</b> [Authorizer Name]</p>
                <p style='font-size:14px;'>✅ <b>Approval Date:</b> [Approval Date]</p>
              </div>

              <p style='font-size:14px; margin-top:12px;'>
                Now that your request is approved, you can proceed with the next steps. Click the button below to access your account:
              </p>

              <div style='text-align:center; margin:20px;'>
                <a href='https://yourloginpage.com' style='
                  display:inline-block;
                  background: #6366F1;
                  color: white;
                  text-decoration: none;
                  padding:12px 24px;
                  border-radius:6px;
                  font-weight:600;
                  transition: opacity 0.3s;'>
                  Go to Login Page
                </a>
              </div>

              <p style='font-size:14px; margin-top:15px;'>
                If you need assistance, our support team is happy to help.
              </p>

              <div style='padding-top:20px; border-top:1px solid #bbb; margin-top:20px; text-align:center;'>
                <p style='color:#6366F1; font-weight:500; margin-bottom:5px;'>Best Regards</p>
                <p style='color:#222;'>Web University</p>
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

            MailSender.SendMail(recipients, html, title);
        }
    }
}
