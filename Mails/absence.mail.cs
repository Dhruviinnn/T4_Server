using Org.BouncyCastle.Crypto.Modes;

namespace TimeFourthe.Mails
{
  public class Absence
  {
    public static void Mail(string[] recipients, string teacher, string subject, string orgName, string date)
    {
      string title = "Urgent : No Class Today – Teacher Absent";
      string html = @$"<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>Teacher Absence Notification</title>
</head>
<body style='color:#222; font-family:Arial, sans-serif; margin:0; padding:0; background:#f5f5f5;'>

  <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' align='center'>
    <tr>
      <td align='center' style='padding:15px;'>
        <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' style='
          max-width: 600px;
          background: #ffffff;
          border-radius: 15px;
          overflow:hidden;
          box-shadow: 0px 4px 30px rgba(0, 0, 0, 0.1);
          width: 90%;'>

          <!-- Header -->
          <tr>
            <td style='padding:15px; text-align:center;'>
              <span style='font-size:20px; font-weight:600; color:#222;'>📢 Web University</span>
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
                🚨 Absence Notice
              </h1>
            </td>
          </tr>

          <!-- Content -->
          <tr>
            <td style='padding:20px; text-align:center;'>
              <h2 style='color:#222; font-size:20px; margin-bottom:10px;'>
                Important Teacher Absence Notice
              </h2>

              <p style='
                background: #eee;
                padding:12px;
                border-radius:8px;
                font-size:16px;
                font-weight:500;
                color:#222;
                display:inline-block;'>
                This is to inform you that <span style='color:#A78BFA; font-weight:600;'>{teacher}</span>, your 
                <span style='color:#A78BFA; font-weight:600;'>{subject}</span> teacher, will be unavailable on 
                <span style='color:#A78BFA; font-weight:600;'>{date}</span>.
              </p>

              <p style='font-size:14px; margin-top:15px;'>
                We appreciate your understanding. Regular classes will resume as scheduled from next lecture onwards.
              </p>

              <div style='padding-top:20px; border-top:1px solid #374151; margin-top:20px; text-align:center;'>
                <p style='color:#A78BFA; font-weight:500; margin-bottom:5px;'>Best Regards</p>
                <p style='color:#fff;'>{orgName}</p>
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

      MailSender.SendMail(recipients, html, title, orgName, "Absence Mail");
    }
  }
}
