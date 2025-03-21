namespace TimeFourthe.Mails
{
    public class Forgetpass
    {
        
        public async static void Mail()
        {
            
            string title = "Reset your Password";
            string senderName = "Web University";
            string[] recipients = ["vasavadhruvin123@gmail.com"];
            string html = @$"<!DOCTYPE html>
<html>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>Password Reset Request</title>
</head>
<body style='color:#222; font-family:Arial, sans-serif; margin:0; padding:0; background:#f5f5f5;'>

  <table role='presentation' cellspacing='0' cellpadding='0' border='0' align='center' width='100%'>
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
              <span style='font-size:20px; font-weight:600; color:#222;'> 🔒 Secure Access</span>
            </td>
          </tr>

          <!-- Banner -->
          <tr>
            <td style='
              background: #ddd;
              padding:20px;
              text-align:center;
              color:#222;'>
              
              <h1 style='font-size:24px; margin:0; font-weight:bold;'>
              🔑 Reset Your Password
              </h1>
            </td>
          </tr>

          <!-- Content -->
          <tr>
            <td style='padding:20px; text-align:center;'>
              <h2 style='color:#222; font-size:20px; margin-bottom:10px;'>
                Forgot Your Password?
              </h2>
              
              <p style='
                background: #eee;
                padding:12px;
                border-radius:8px;
                font-size:16px;
                font-weight:350;
                color:#222;'>
                Dear <strong>Dhruvin</strong>, we received a request to reset your password
              </p>

              <p style='font-size:14px; margin-top:12px; color:#333;'>
                Click the button below to reset your password. If you did not request this, please ignore this email
              </p>

              <!-- Reset Button -->
              <div style='text-align:center; margin-top:20px;'>
                <a href='https://yourresetlink.com' 
                   style='
                     display:inline-block;
                     background: #444;
                     padding:12px 30px;
                     color:#fff;
                     text-decoration:none;
                     font-weight:bold;
                     border-radius:6px;
                     font-size:14px;
                     box-shadow: 0px 4px 15px rgba(0, 0, 0, 0.3);
                     transition: all 0.3s ease;'>
                  🔑 Reset Password
                </a>
              </div>

              <!-- Footer -->
              <div style='text-align:center; margin-top:20px; padding-top:15px; border-top:1px solid #bbb;'>
                <p style='color:#222; font-weight:500; margin-bottom:8px;'>Best Regards,</p>
                <p style='color:#222;'>Secure Access Team</p>

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

            await MailSender.SendMail(recipients, html, title, senderName);
        }
    }
}
