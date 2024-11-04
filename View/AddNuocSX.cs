using System;
using System.Windows.Forms;

namespace tablechair
{
    public partial class AddNuocSX : Form
    {
        private DataBaseHelper dbHelper = new DataBaseHelper(); // Sử dụng DataBaseHelper

        public AddNuocSX()
        {
            InitializeComponent();
        }

        // Khi người dùng nhấn nút "Thêm"
        private void btnThem_Click(object sender, EventArgs e)
        {
            string tenNuocSX = txtTenNuocSx.Text.Trim();

            if (string.IsNullOrWhiteSpace(tenNuocSX))
            {
                MessageBox.Show("Vui lòng nhập tên nước sản xuất.");
                return;
            }

            // Kiểm tra xem nước sản xuất đã tồn tại chưa
            string maNuocSX = dbHelper.CheckNuocSX(tenNuocSX);
            if (maNuocSX != null)
            {
                MessageBox.Show("Nước sản xuất đã tồn tại với mã: " + maNuocSX);
                return;
            }

            // Thêm nước sản xuất vào database
            maNuocSX = dbHelper.AddNuocSX(tenNuocSX);
            if (maNuocSX != null)
            {
                MessageBox.Show("Thêm nước sản xuất thành công với mã: " + maNuocSX);
                this.DialogResult = DialogResult.OK;
                this.Close(); // Đóng form sau khi thêm thành công
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi thêm nước sản xuất.");
            }
        }
    }
}
