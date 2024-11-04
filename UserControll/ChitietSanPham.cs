using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace tablechair.UserControll
{
    public partial class ChitietSanPham : UserControl
    {
        private readonly DataBaseHelper databaseHelper = new DataBaseHelper();
        private MenuForm parentForm;
        private string maHang;
        private ProductControll productControl;
        private string imagePath;

        public ChitietSanPham(MenuForm parent, ProductControll productControl, string maHang)
        {
            InitializeComponent();
            parentForm = parent;
            this.productControl = productControl; // Lưu đối tượng ProductControll vào biến thành viên
            this.maHang = maHang;
            InitializeControls();
            LoadProductDetails();
        }

        private void InitializeControls()
        {
            LoadChatLieuComboBox();
            guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
        }

        private void LoadChatLieuComboBox()
        {
            List<string> chatLieuList = databaseHelper.LoadChatLieu();
            guna2ComboBox1.DataSource = chatLieuList;
        }

        public void LoadProductDetails()
        {
            string query = @"
                SELECT 
                    DMHangHoa.MaHang,
                    DMHangHoa.TenHangHoa,
                    Loai.TenLoai AS LoaiHang,
                    ChatLieu.TenChatLieu AS ChatLieu,
                    KichThuoc.TenKichThuoc AS KichThuoc,
                    HinhDang.TenHinhDang AS HinhDang,
                    NuocSX.TenNuocSX AS NuocSanXuat,
                    DacDiem.TenDacDiem AS DacDiem,
                    MauSac.TenMau AS Mau,
                    DMHangHoa.CongDung,
                    DMHangHoa.SoLuong,
                    DMHangHoa.DonGiaNhap,
                    DMHangHoa.DonGiaBan,
                    DMHangHoa.ThoiGianBaoHanh,
                    DMHangHoa.GhiChu,
                    DMHangHoa.Anh
                FROM DMHangHoa
                LEFT JOIN Loai ON DMHangHoa.MaLoai = Loai.MaLoai
                LEFT JOIN ChatLieu ON DMHangHoa.MaChatLieu = ChatLieu.MaChatLieu
                LEFT JOIN KichThuoc ON DMHangHoa.MaKichThuoc = KichThuoc.MaKichThuoc
                LEFT JOIN HinhDang ON DMHangHoa.MaHinhDang = HinhDang.MaHinhDang
                LEFT JOIN NuocSX ON DMHangHoa.MaNuocSX = NuocSX.MaNuocSX
                LEFT JOIN DacDiem ON DMHangHoa.MaDacDiem = DacDiem.MaDacDiem
                LEFT JOIN MauSac ON DMHangHoa.MaMau = MauSac.MaMau
                WHERE DMHangHoa.MaHang = @MaHang";

            SqlParameter[] parameters = { new SqlParameter("@MaHang", maHang) };

            DataTable result = DatabaseManager.Instance.ExecuteQuery(query, parameters);
            if (result.Rows.Count > 0)
            {
                FillProductDetails(result.Rows[0]);
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin sản phẩm.");
            }
        }

        private void FillProductDetails(DataRow row)
        {
            txtMahang.Text = row["MaHang"]?.ToString() ?? string.Empty;
            txtTenhang.Text = row["TenHangHoa"]?.ToString() ?? string.Empty;
            txtLoaihang.Text = row["LoaiHang"]?.ToString() ?? string.Empty;
            guna2ComboBox1.SelectedItem = row["ChatLieu"]?.ToString() ?? string.Empty;
            txtNuocsx.Text = row["NuocSanXuat"]?.ToString() ?? string.Empty;
            txtKichthuoc.Text = row["KichThuoc"]?.ToString() ?? string.Empty;
            txtDacdiem.Text = row["DacDiem"]?.ToString() ?? string.Empty;
            txtMau.Text = row["Mau"]?.ToString() ?? string.Empty;
            txtCongdung.Text = row["CongDung"]?.ToString() ?? string.Empty;
            txtSoluong.Text = row["SoLuong"]?.ToString() ?? string.Empty;
            txtDongianhap.Text = row["DonGiaNhap"]?.ToString() ?? string.Empty;
            txtDongiaban.Text = row["DonGiaBan"]?.ToString() ?? string.Empty;
            txtBaohanh.Text = row["ThoiGianBaoHanh"]?.ToString() ?? string.Empty;
            txtGhichu.Text = row["GhiChu"]?.ToString() ?? string.Empty;
            LoadProductImage(row["Anh"]?.ToString());
        }

        private void LoadProductImage(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                pictureboxanh.Image = Image.FromFile(imagePath);
            }
            else
            {
                pictureboxanh.Image = null;
            }
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedItem != null && guna2ComboBox1.SelectedItem.ToString() == "Thêm chất liệu...")
            {
                AddNewChatLieu();
            }
        }

        private void AddNewChatLieu()
        {
            using (var addChatLieuForm = new AddChatLieuForm())
            {
                if (addChatLieuForm.ShowDialog() == DialogResult.OK)
                {
                    guna2ComboBox1.DataSource = databaseHelper.LoadChatLieu();
                }
            }
            guna2ComboBox1.SelectedIndex = 0;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (productControl != null)
            {
                productControl.LoadDataIntoDataGridView();
            }
            else
            {
                MessageBox.Show("ProductControl chưa được khởi tạo.");
            }
            parentForm.ShowSanPhamControl();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string maHang = txtMahang.Text.Trim();
            string tenHangHoa = txtTenhang.Text.Trim();
            string chatLieu = guna2ComboBox1.SelectedItem?.ToString();
            string nuocSanXuat = txtNuocsx.Text.Trim();
            string kichThuoc = txtKichthuoc.Text.Trim();
            string dacDiem = txtDacdiem.Text.Trim();
            string mau = txtMau.Text.Trim();
            string congDung = txtCongdung.Text.Trim();
            int soLuong;
            decimal donGiaNhap, donGiaBan;
            int thoiGianBaoHanh;
            string ghiChu = txtGhichu.Text.Trim();
            string currentImagePath = imagePath ?? ""; // Sử dụng đường dẫn ảnh đã chọn, nếu không có sẽ để chuỗi rỗng

            if (!int.TryParse(txtSoluong.Text.Trim(), out soLuong) ||
                !decimal.TryParse(txtDongianhap.Text.Trim(), out donGiaNhap) ||
                !decimal.TryParse(txtDongiaban.Text.Trim(), out donGiaBan) ||
                !int.TryParse(txtBaohanh.Text.Trim(), out thoiGianBaoHanh))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng cho các trường số!");
                return;
            }

            if (string.IsNullOrEmpty(chatLieu) || chatLieu == "Thêm chất liệu...")
            {
                MessageBox.Show("Vui lòng chọn chất liệu hợp lệ.");
                return;
            }

            string query = @"
    UPDATE DMHangHoa
    SET TenHangHoa = @TenHangHoa,
        MaChatLieu = (SELECT MaChatLieu FROM ChatLieu WHERE TenChatLieu = @ChatLieu),
        MaNuocSX = (SELECT MaNuocSX FROM NuocSX WHERE TenNuocSX = @NuocSanXuat),
        MaKichThuoc = (SELECT MaKichThuoc FROM KichThuoc WHERE TenKichThuoc = @KichThuoc),
        MaDacDiem = (SELECT MaDacDiem FROM DacDiem WHERE TenDacDiem = @DacDiem),
        MaMau = (SELECT MaMau FROM MauSac WHERE TenMau = @Mau),
        CongDung = @CongDung,
        SoLuong = @SoLuong,
        DonGiaNhap = @DonGiaNhap,
        DonGiaBan = @DonGiaBan,
        ThoiGianBaoHanh = @ThoiGianBaoHanh,
        GhiChu = @GhiChu,
        Anh = @Anh
    WHERE MaHang = @MaHang";

            SqlParameter[] parameters = {
        new SqlParameter("@MaHang", maHang),
        new SqlParameter("@TenHangHoa", tenHangHoa),
        new SqlParameter("@ChatLieu", chatLieu),
        new SqlParameter("@NuocSanXuat", nuocSanXuat),
        new SqlParameter("@KichThuoc", kichThuoc),
        new SqlParameter("@DacDiem", dacDiem),
        new SqlParameter("@Mau", mau),
        new SqlParameter("@CongDung", congDung),
        new SqlParameter("@SoLuong", soLuong),
        new SqlParameter("@DonGiaNhap", donGiaNhap),
        new SqlParameter("@DonGiaBan", donGiaBan),
        new SqlParameter("@ThoiGianBaoHanh", thoiGianBaoHanh),
        new SqlParameter("@GhiChu", ghiChu),
        new SqlParameter("@Anh", currentImagePath)
    };

            try
            {
                int rowsAffected = DatabaseManager.Instance.ExecuteNonQuery(query, parameters);
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!");
                    productControl.LoadDataIntoDataGridView();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm để cập nhật.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }


        private void guna2Button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Title = "Chọn ảnh sản phẩm";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureboxanh.Image = Image.FromFile(openFileDialog.FileName);
                    imagePath = openFileDialog.FileName;
                }
            }
        }

        private void pictureboxanh_Click(object sender, EventArgs e)
        {

        }
    }
}
