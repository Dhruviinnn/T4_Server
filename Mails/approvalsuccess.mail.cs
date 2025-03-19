namespace TimeFourthe.Mails
{
    public class ApprovalSuccess
    {
        public static void Mail(string orgName, string email)
        {
            string title = "Authentication of Organization";
            string[] recipients = [email];
            string html = @$"<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>Request Approval</title>
</head>
<body style=' color:#D1D5DB; font-family:Arial, sans-serif; margin:0; padding:0;'>
  <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' align='center'>
    <tr>
      <td align='center' style='padding:15px;'>
        <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' style='
          max-width: 600px;
          background: rgba(19, 32, 50, 0.9);;
          backdrop-filter: blur(10px);
          border-radius: 15px;
          overflow:hidden;
          box-shadow: 0px 4px 30px rgba(0, 0, 0, 0.2);
          width: 90%;'>

          <!-- Header -->
          <tr>
            <td style='padding:15px; text-align:center;'>
              <span style='font-size:20px; font-weight:600; color:#fff;'>✅ Web University</span>
            </td>
          </tr>

          <tr>
            <td style='
              background: linear-gradient(to right, #8B5CF6, #6366F1);
              padding:20px;
              text-align:center;
              color:#fff;'>
              <h1 style='font-size:24px; margin:0; font-weight:bold; text-shadow: 2px 2px 4px rgba(0,0,0,0.4);'>
                🎉 Approval Successful
              </h1>
            </td>
          </tr>

         
          <tr>
            <td style='padding:20px; text-align:center;'>
              <h2 style='color:#fff; font-size:20px; margin-bottom:10px;'>
                Request Approval Confirmation
              </h2>

              <p style='
                background: rgba(255,255,255,0.1);
                padding:12px;
                border-radius:8px;
                font-size:16px;
                font-weight:500;
                color:#dfdbeb;
                display:inline-block;'>
                Dear <span style='color:#A78BFA; font-weight:600;'>[User's Name]</span>, your request has been successfully approved.
              </p>

              <div style='background:#1a1f2b; padding:20px; border-radius:12px; text-align:left;'>
                <p style='font-size:14px; margin-bottom:10px;'>✅ <b>Requestor Name:</b>{orgName}</p>
                <p style='font-size:14px; margin-bottom:10px;'>✅ <b>Request Details:</b> [Request Details]</p>
                <p style='font-size:14px;'>✅ <b>Approval Date:</b> [Approval Date]</p>
              </div>

              <p style='font-size:14px; margin-top:12px;'>
                Now that your request is approved, you can proceed with the next steps. Click the button below to access your account:
              </p>

              <div style='text-align:center; margin:20px;'>
                <a href='http://localhost:5173/login' style='
                  display:inline-block;
                  background: linear-gradient(to right, #8B5CF6, #6366F1);
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

              <div style='padding-top:20px; border-top:1px solid #374151; margin-top:20px; text-align:center;'>
                <p style='color:#A78BFA; font-weight:500; margin-bottom:5px;'>Best Regards</p>
                <p style='color:#fff;'>Web University</p>
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

            MailSender.SendMail(recipients, html, title,"Approval Success Mail");
        }
    }
}