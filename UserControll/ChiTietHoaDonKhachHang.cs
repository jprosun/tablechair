using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tablechair.UserControll
{
    public partial class ChiTietHoaDonKhachHang : UserControl
    {
        private string maKhach;
        private MenuForm parentForm;

        public ChiTietHoaDonKhachHang()
        {
            InitializeComponent();
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click_1);
        }

        public ChiTietHoaDonKhachHang(MenuForm parent, string maKhach) : this() // Gọi constructor không tham số
        {
            this.maKhach = maKhach;
            parentForm = parent; // Gán parentForm
            LoadCustomerDetails();
        }

        private void LoadCustomerDetails()
        {
            string query = @"
        SELECT 
            KhachHang.TenKhach,
            KhachHang.DiaChi,
            KhachHang.DienThoai,
            HoaDonBan.NgayBan,
            HoaDonBan.MaNV,
            ChiTietHoaDonBan.MaHang,
            ChiTietHoaDonBan.ThanhTien,
            DMHangHoa.TenHangHoa
        FROM KhachHang
        LEFT JOIN HoaDonBan ON KhachHang.MaKhach = HoaDonBan.MaKhach
        LEFT JOIN ChiTietHoaDonBan ON HoaDonBan.SoHDB = ChiTietHoaDonBan.SoHDB
        LEFT JOIN DMHangHoa ON ChiTietHoaDonBan.MaHang = DMHangHoa.MaHang
        WHERE KhachHang.MaKhach = @MaKhach";

            HashSet<DateTime> addedDates = new HashSet<DateTime>();
            HashSet<string> addedProductNames = new HashSet<string>(); // Set để tránh tên sản phẩm trùng lặp

            try
            {
                // Cập nhật phương thức ExecuteQuery để hỗ trợ nhiều tham số
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@MaKhach", maKhach)
                };

                DataTable result = DatabaseManager.Instance.ExecuteQuery(query, parameters);

                if (result.Rows.Count > 0)
                {
                    DataRow row = result.Rows[0];
                    // Điền thông tin vào các trường
                    txtMaKH.Text = maKhach;
                    txtTenKH.Text = row["TenKhach"].ToString();
                    txtDiaChi.Text = row["DiaChi"].ToString();
                    txtSoDT.Text = row["DienThoai"].ToString();
                    txtTenNhanVien.Text = row["MaNV"].ToString();
                    txtGiaTien.Text = row["ThanhTien"].ToString();

                    // Thêm ngày bán vào comboboxNgayMua, kiểm tra trùng lặp
                    if (row["NgayBan"] != DBNull.Value)
                    {
                        DateTime ngayMua = Convert.ToDateTime(row["NgayBan"]);
                        if (!addedDates.Contains(ngayMua.Date))
                        {
                            comboboxNgayMua.Items.Add(ngayMua.Date);
                            addedDates.Add(ngayMua.Date);
                        }
                    }

                    // Thêm tên mặt hàng vào comboboxMatHang, kiểm tra trùng lặp
                    if (row["TenHangHoa"] != DBNull.Value)
                    {
                        string tenHang = row["TenHangHoa"].ToString();
                        if (!addedProductNames.Contains(tenHang))
                        {
                            comboBoxMatHang.Items.Add(tenHang);
                            addedProductNames.Add(tenHang);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin khách hàng.");
                }
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi: " + ex.Message);
            }
        }


        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            parentForm.ShowKhachHangControl();
        }

      
    }
}
