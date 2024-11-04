using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace tablechair
{
    public partial class LoginForm : Controll
    {
        public LoginForm()
        {
            InitializeComponent();
            txtPass.UseSystemPasswordChar = true;
        }

        // Xử lý sự kiện khi người dùng nhấn nút đăng nhập
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string username = txtEmail.Text;
            string password = txtPass.Text;

            try
            {
                string query = "SELECT * FROM login WHERE email = @Email AND password = @Password";

                // Thực hiện truy vấn bằng DatabaseManager
                DataTable dt = DatabaseManager.Instance.ExecuteQuery(query,
                    new SqlParameter("@Email", username),
                    new SqlParameter("@Password", password));

                if (dt.Rows.Count > 0)
                {
                    MenuForm menuForm = new MenuForm();
                    menuForm.StartPosition = FormStartPosition.CenterScreen; // Đặt vị trí nếu cần
                    menuForm.ShowDialog(); // Hoặc ShowDialog()
                    this.Hide();  // Ẩn form hiện tại
                }
                else
                {
                    ShowErrorMessage("Sai mật khẩu hoặc email.");
                    txtEmail.Clear();
                    txtPass.Clear();
                    txtEmail.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi: " + ex.Message);
            }
        }

        // Xử lý sự kiện khi người dùng nhấn nút đăng ký
        private void btnSignup_Click(object sender, EventArgs e)
        {
            SignUpForm signUpForm = new SignUpForm();
            signUpForm.ShowDialog();
            this.Hide();
        }

        // Xử lý sự kiện bật/tắt hiển thị mật khẩu
        private void ShowPass_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ShowPass.Checked)
            {
                txtPass.UseSystemPasswordChar = false;  // Hiện mật khẩu
                txtPass.ForeColor = Color.White;        // Đổi màu chữ thành trắng
            }
            else
            {
                txtPass.UseSystemPasswordChar = true;   // Ẩn mật khẩu
                txtPass.ForeColor = SystemColors.WindowText;  // Đổi màu chữ về mặc định
            }
        }
    }
}
