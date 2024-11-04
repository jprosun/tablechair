using System;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Windows.Forms;
using System.Configuration;
using System.Drawing;

namespace tablechair
{
    public partial class SignUpForm : Controll
    {
        private string generatedOTP;  // Biến lưu OTP đã sinh ra

        public SignUpForm()
        {
            InitializeComponent();
            txtPass.UseSystemPasswordChar = true;
        }

        // Hàm sinh OTP ngẫu nhiên 4 chữ số
        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(1000, 9999).ToString(); // Sinh số ngẫu nhiên từ 1000 đến 9999
        }

        // Hàm xử lý khi bấm nút Send OTP
        private void btnSendOTP_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter your email.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo OTP và gửi qua email
            generatedOTP = GenerateOTP();
            SendOTPEmail(email, generatedOTP);  // Gửi OTP qua email
        }

        // Hàm gửi OTP qua email
        private void SendOTPEmail(string email, string otp)
        {
            try
            {
                // Lấy thông tin email gửi từ app.config
                string senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
                string senderPassword = ConfigurationManager.AppSettings["SenderPassword"];
                string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                message.From = new MailAddress(senderEmail);
                message.To.Add(new MailAddress(email));
                message.Subject = "Your OTP Code";
                message.Body = "Your OTP code is: " + otp;

                smtp.Port = smtpPort;
                smtp.Host = smtpHost;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(message);

                MessageBox.Show("OTP has been sent to your email.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send OTP. Error: " + ex.Message);
            }
        }

        // Hàm xử lý khi bấm nút Sign up
        private void btnSignup_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPass.Text;
            string username = txtUserName.Text;
            string otp = txtOTP.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(otp))
            {
                MessageBox.Show("Please fill all fields", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra xem OTP người dùng nhập có đúng không
            if (otp != generatedOTP)
            {
                MessageBox.Show("Incorrect OTP.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tiến hành đăng ký nếu OTP chính xác
            try
            {
                string query = "INSERT INTO login (email, password, username) VALUES (@Email, @Password, @UserName)";

                // Thực hiện truy vấn bằng DatabaseManager
                int result = DatabaseManager.Instance.ExecuteNonQuery(query,
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Password", password),
                    new SqlParameter("@UserName", username));

                if (result > 0)
                {
                    MessageBox.Show("Sign up successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MenuForm menuForm = new MenuForm();
                    menuForm.Show();
                    this.Hide(); // Ẩn form hiện tại
                }
                else
                {
                    ShowErrorMessage("Sign up failed.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error: " + ex.Message);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form = new Form1();
            form.Show();
        }

        private void ShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowPass.Checked) // Nếu ToggleSwitch được bật
            {
                txtPass.UseSystemPasswordChar = false; // Hiện mật khẩu
                txtPass.ForeColor = Color.White; // Đặt màu chữ thành trắng
            }
            else
            {
                txtPass.UseSystemPasswordChar = true; // Ẩn mật khẩu
                txtPass.ForeColor = SystemColors.WindowText; // Đặt màu chữ trở lại mặc định
            }
        }
    }
}
