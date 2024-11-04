using System;
using System.Windows.Forms;
using tablechair.UserControll;

namespace tablechair
{
    public partial class MenuForm : Form
    {
        private TongquanForm tongquanForm;
        private ProductControll productControll;
        private KhachHang customerControll;

        public MenuForm()
        {
            InitializeComponent();
            tongquanForm = new TongquanForm();
            productControll = new ProductControll();
            customerControll = new KhachHang(this);
            ShowControl(tongquanForm);
        }

        public void ShowControl(UserControl control)
        {
            panelParent.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelParent.Controls.Add(control);
            control.BringToFront();
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

        public void ShowKhachHangControl()
        {
            ShowControl(customerControll);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ShowControl(tongquanForm);
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
