using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class Controll : Form
{
    public Controll()
    {
        InitializeComponent();
    }

    // Hàm khởi tạo giao diện
    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.ClientSize = new System.Drawing.Size(278, 244);
        this.Name = "Controll";
        this.Load += new System.EventHandler(this.Controll_Load);
        this.ResumeLayout(false);
    }

    private void Controll_Load(object sender, EventArgs e)
    {
        // Xử lý khi form được tải
    }

    public void ShowErrorMessage(string message)
    {
        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public void ShowSuccessMessage(string message)
    {
        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void InsertProduct(
        string loai,
        string ten,
        string chatLieu,
        decimal giaNhap,
        int soLuong,
        string maDacDiem,
        string maMau,
        string maNuocSX,
        int thoiGianBaoHanh,
        string anh = null,
        string ghiChu = null
    )
    {
        if (!IsMaLoaiValid(loai))
        {
            ShowErrorMessage("Mã loại không hợp lệ.");
            return;
        }

        string maHang = GenerateMaHang(loai);
        string query = @"INSERT INTO DMHangHoa 
                    (MaHang, TenHangHoa, MaLoai, MaChatLieu, SoLuong, DonGiaNhap, DonGiaBan, 
                     MaNuocSX, ThoiGianBaoHanh, MaDacDiem, MaMau, Anh, GhiChu) 
                    VALUES 
                    (@MaHang, @TenHangHoa, @MaLoai, @MaChatLieu, @SoLuong, @DonGiaNhap, 
                     @DonGiaBan, @MaNuocSX, @ThoiGianBaoHanh, @MaDacDiem, @MaMau, @Anh, @GhiChu)";

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@MaHang", maHang),
            new SqlParameter("@TenHangHoa", ten),
            new SqlParameter("@MaLoai", loai),
            new SqlParameter("@MaChatLieu", chatLieu),
            new SqlParameter("@SoLuong", soLuong),
            new SqlParameter("@DonGiaNhap", giaNhap),
            new SqlParameter("@DonGiaBan", giaNhap * 1.2m),
            new SqlParameter("@MaNuocSX", string.IsNullOrEmpty(maNuocSX) ? (object)DBNull.Value : maNuocSX),
            new SqlParameter("@ThoiGianBaoHanh", thoiGianBaoHanh > 0 ? thoiGianBaoHanh : (object)DBNull.Value),
            new SqlParameter("@MaDacDiem", string.IsNullOrEmpty(maDacDiem) ? (object)DBNull.Value : maDacDiem),
            new SqlParameter("@MaMau", string.IsNullOrEmpty(maMau) ? (object)DBNull.Value : maMau),
            new SqlParameter("@Anh", string.IsNullOrEmpty(anh) ? (object)DBNull.Value : anh),
            new SqlParameter("@GhiChu", string.IsNullOrEmpty(ghiChu) ? (object)DBNull.Value : ghiChu)
        };

        try
        {
            DatabaseManager.Instance.ExecuteNonQuery(query, parameters);
            ShowSuccessMessage("Dữ liệu đã được thêm thành công.");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Không thể thêm dữ liệu. Lỗi: " + ex.Message);
        }
    }

    private bool IsMaLoaiValid(string maLoai)
    {
        string query = "SELECT COUNT(*) FROM Loai WHERE MaLoai = @MaLoai";
        SqlParameter parameter = new SqlParameter("@MaLoai", maLoai);

        int count = (int)DatabaseManager.Instance.ExecuteScalar(query, parameter);
        return count > 0;
    }

    private string GenerateMaHang(string loai)
    {
        int count = 0;
        string prefix = loai == "Bàn" ? "BAN" : "GHE";

        string query = $"SELECT COUNT(*) FROM DMHangHoa WHERE MaHang LIKE '{prefix}%'";

        count = (int)DatabaseManager.Instance.ExecuteScalar(query); // Lấy số lượng sản phẩm

        return prefix + (count + 1).ToString(); // Tăng lên 1 để tạo mã mới
    }
}
