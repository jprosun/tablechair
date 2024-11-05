using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using tablechair.UserControll;

namespace tablechair
{
    public partial class MenuForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private TongquanForm tongquanForm;
        private ProductControll productControll;
        private KhachHang customerControll;
        private HomePage homePage;

        public MenuForm()
        {
            InitializeComponent();

            homePage = new HomePage();
            tongquanForm = new TongquanForm();
            productControll = new ProductControll();
            customerControll = new KhachHang(this);

            ToolTip.SetToolTip(UserIcon, "Thông tin cá nhân");
            ToolTip.SetToolTip(HomeBtn, "Trang chủ");
            ToolTip.SetToolTip(BillBtn, "Hóa đơn");
            ToolTip.SetToolTip(ProductBtn, "Danh sách sản phẩm");
            ToolTip.SetToolTip(CustomerBtn, "Danh sách khách hàng");
            ToolTip.SetToolTip(OverallBtn, "Tổng quan");
            ToolTip.SetToolTip(EmployeeBtn, "Danh sách nhân viên");
            ToolTip.SetToolTip(LogoutBtn, "Đăng xuất");
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            HomeBtn_Click(HomeBtn, e);
        }

        private void ShowControl(UserControl control)
        {
            panelParent.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelParent.Controls.Add(control);
            control.BringToFront();
        }

        public void ShowKhachHangControl()
        {
            ShowControl(customerControll);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ShowControl(tongquanForm);
            moveEffect(sender);
        }

        private void moveEffect(object sender)
        {
            Control btn = (Control)sender;
            btnEffect.Location = new Point()
            {
                X = btnEffect.Location.X,
                Y = btn.Location.Y - (btnEffect.Height - btn.Height) / 2 + 1
            };
            btnEffect.BringToFront();
            btnEffect.Visible = true;
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void RevenueBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            ShowControl(tongquanForm);
            HomeLabel.Text = "Tổng quan";
        }

        private void EmployeeBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            HomeLabel.Text = "Danh sách nhân viên";
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            ShowControl(homePage);
            HomeLabel.Text = "Trang chủ";
        }

        private void UserIcon_Click(object sender, EventArgs e)
        {
            btnEffect.Visible = false;
            foreach (Control item in AsidePanel.Controls)
            {
                if (item is Guna2Button && item != sender)
                {
                    ((Guna2Button)item).Checked = false;
                }
            }
            HomeLabel.Text = "Thông tin cá nhân";
        }

        private void BillBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            HomeLabel.Text = "Hóa đơn";
        }

        private void ProductBtn_Click(object sender, EventArgs e)
        {
            ShowControl(productControll);
            moveEffect(sender);
            HomeLabel.Text = "Danh sách sản phẩm";
        }

        private void CustomerBtn_Click(object sender, EventArgs e)
        {
            ShowControl(customerControll);
            moveEffect(sender);
            HomeLabel.Text = "Danh sách khách hàng";
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {

        }

        public void ShowOrderDetails(string maKhach)
        {
            ChiTietHoaDonKhachHang chiTietControl = new ChiTietHoaDonKhachHang(this, maKhach);
            ShowControl(chiTietControl);
        }

        public void ShowProductDetails(string maHang)
        {
            var chitietSanPham = new ChitietSanPham(this, productControll, maHang);
            ShowControl(chitietSanPham);
        }

        public void ShowSanPhamControl()
        {
            ShowControl(productControll);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ShowControl(productControll);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            ShowControl(customerControll);
        }
    }
}
