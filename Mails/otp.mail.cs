namespace TimeFourthe.Mails {
    public class Otp {
        private static string GenerateOtp(){
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Generate 6-digit OTP
        }
        public async static void MailOtp() {
            string otpCode=GenerateOtp();
            string title = "Your One-Time Password (OTP) 🔐";
            string senderName="Web University";
            string[] recipients =["cclab01234@gmail.com"];
            string html = @$"<!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>OTP Verification</title>
                        <style>
                            body {{font-family: Arial, sans-serif; background-color: #121212; color: #ffffff; text-align: center; padding: 20px;}}
                            .container {{max-width: 400px; margin: auto; background-color: #1e1e1e; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(255, 255, 255, 0.1);}}
                            h2 {{color: #ffffff;}}
                            p {{color: #bbbbbb; font-size: 16px;}}
                            .otp-code {{font-size: 24px; font-weight: bold; color: #00ffcc; margin: 20px auto; padding: 10px; border: 2px dashed #00ffcc; display: inline-block;}}
                            .footer {{margin-top: 20px; font-size: 14px; color: #888888;}}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>Your OTP Code</h2>
                            <p>Use the OTP below to complete your verification:</p>
                            <div class='otp-code'>{otpCode}</div>
                            <p>This OTP is valid for 10 minutes.</p>
                            <p>If you did not request this, please ignore this email.</p>
                        </div>
                    </body>
                    </html>";
        
        await MailSender.SendMail(recipients, html, title,senderName);
        }
    }
}
