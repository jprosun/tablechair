using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace tablechair
{
    public class DataBaseHelper
    {
        public string GetMaLoaiFromTen(string tenLoai)
        {
            string query = "SELECT MaLoai FROM Loai WHERE TenLoai = @TenLoai";
            SqlParameter parameter = new SqlParameter("@TenLoai", tenLoai);

            try
            {
                object result = DatabaseManager.Instance.ExecuteScalar(query, parameter);
                return result?.ToString();
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi: " + ex.Message);
            }

            return null;
        }


        public List<string> LoadLoaiSanPham()
        {
            string query = "SELECT TenLoai FROM Loai";
            List<string> loaiSanPhamList = new List<string>();

            try
            {
                DataTable loaiTable = DatabaseManager.Instance.ExecuteQuery(query);
                foreach (DataRow row in loaiTable.Rows)
                {
                    loaiSanPhamList.Add(row["TenLoai"].ToString());
                }
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi: " + ex.Message);
            }

            return loaiSanPhamList;
        }

        public List<string> LoadChatLieu()
        {
            List<string> chatLieuList = new List<string>();
            string query = "SELECT TenChatLieu FROM ChatLieu";

            try
            {
                DataTable chatLieuTable = DatabaseManager.Instance.ExecuteQuery(query);
                foreach (DataRow row in chatLieuTable.Rows)
                {
                    chatLieuList.Add(row["TenChatLieu"].ToString());
                }
                chatLieuList.Add("Thêm chất liệu...");
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi: " + ex.Message);
            }

            return chatLieuList;
        }

        public string CheckChatLieu(string tenChatLieu)
        {
            string query = "SELECT MaChatLieu FROM ChatLieu WHERE TenChatLieu = @TenChatLieu";
            SqlParameter parameter = new SqlParameter("@TenChatLieu", tenChatLieu);
            try
            {
                object result = DatabaseManager.Instance.ExecuteScalar(query, parameter);
                return result?.ToString();
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi: " + ex.Message);
                return null;
            }
        }

        public string AddChatLieu(string tenChatLieu)
        {
            string maChatLieu = GenerateMaChatLieu();
            string query = "INSERT INTO ChatLieu (MaChatLieu, TenChatLieu) VALUES (@MaChatLieu, @TenChatLieu)";
            SqlParameter[] parameters = {
                new SqlParameter("@MaChatLieu", maChatLieu),
                new SqlParameter("@TenChatLieu", tenChatLieu)
            };

            try
            {
                DatabaseManager.Instance.ExecuteNonQuery(query, parameters);
                DatabaseManager.Instance.ShowSuccessMessage("Chất liệu đã được thêm thành công.");
                return maChatLieu;
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi: " + ex.Message);
                return null;
            }
        }

        public string GenerateMaChatLieu()
        {
            string query = "SELECT TOP 1 MaChatLieu FROM ChatLieu ORDER BY MaChatLieu DESC";

            try
            {
                object result = DatabaseManager.Instance.ExecuteScalar(query);
                if (result != null)
                {
                    string lastMa = result.ToString();
                    int number = int.Parse(lastMa.Substring(2)) + 1;
                    return "CL" + number.ToString("D3");
                }
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi: " + ex.Message);
            }

            return "CL001";
        }

        public class ComboBoxItem
        {
            public string Value { get; set; } 
            public string Text { get; set; } 

            public override string ToString() => Text; 
        }

        public List<ComboBoxItem> LoadNuocSX()
        {
            List<ComboBoxItem> nuocSXList = new List<ComboBoxItem>();
            string query = "SELECT MaNuocSX, TenNuocSX FROM NuocSX";

            try
            {
                DataTable nuocSXTable = DatabaseManager.Instance.ExecuteQuery(query);
                foreach (DataRow row in nuocSXTable.Rows)
                {
                    string maNuocSX = row["MaNuocSX"].ToString();
                    string tenNuocSX = row["TenNuocSX"].ToString();
                    nuocSXList.Add(new ComboBoxItem { Value = maNuocSX, Text = tenNuocSX });
                }
                // Thêm tùy chọn "Thêm nước sản xuất..."
                nuocSXList.Add(new ComboBoxItem { Value = "", Text = "Thêm nước sản xuất..." });
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi: " + ex.Message);
            }

            return nuocSXList;
        }


        public string GetMaNuocSXFromTen(string tenNuocSX)
        {
            string query = "SELECT MaNuocSX FROM NuocSX WHERE TenNuocSX = @TenNuocSX";
            SqlParameter parameter = new SqlParameter("@TenNuocSX", tenNuocSX);

            try
            {
                object result = DatabaseManager.Instance.ExecuteScalar(query, parameter);
                return result?.ToString();
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi: " + ex.Message);
                return null;
            }
        }

        public bool UpdateProduct(string maHang, string tenSP, string maChatLieu, string maLoai, string maNuocSX,
                              int thoiGianBaoHanh, decimal giaBan, decimal giaNhap, int soLuong)
        {
            string query = @"
            UPDATE DMHangHoa 
            SET 
                TenHangHoa = @TenHangHoa,
                MaChatLieu = @MaChatLieu,
                MaLoai = @MaLoai,
                MaNuocSX = @MaNuocSX,
                ThoiGianBaoHanh = @ThoiGianBaoHanh,
                DonGiaBan = @DonGiaBan,
                DonGiaNhap = @DonGiaNhap,
                SoLuong = @SoLuong
            WHERE MaHang = @MaHang";

            SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@MaHang", maHang),
            new SqlParameter("@TenHangHoa", tenSP),
            new SqlParameter("@MaChatLieu", maChatLieu),
            new SqlParameter("@MaLoai", maLoai),
            new SqlParameter("@MaNuocSX", maNuocSX),
            new SqlParameter("@ThoiGianBaoHanh", thoiGianBaoHanh),
            new SqlParameter("@DonGiaBan", giaBan),
            new SqlParameter("@DonGiaNhap", giaNhap),
            new SqlParameter("@SoLuong", soLuong)
            };

            try
            {
                int rowsAffected = DatabaseManager.Instance.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating product: " + ex.Message);
                return false;
            }
        }

        public string CheckNuocSX(string tenNuocSX)
        {
            string query = "SELECT MaNuocSX FROM NuocSX WHERE TenNuocSX = @TenNuocSX";
            SqlParameter parameter = new SqlParameter("@TenNuocSX", tenNuocSX);

            try
            {
                object result = DatabaseManager.Instance.ExecuteScalar(query, parameter);
                return result?.ToString();
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi khi kiểm tra nước sản xuất: " + ex.Message);
                return null;
            }
        }

        public string AddNuocSX(string tenNuocSX)
        {
            string maNuocSX = GenerateMaNuocSX();
            string query = "INSERT INTO NuocSX (MaNuocSX, TenNuocSX) VALUES (@MaNuocSX, @TenNuocSX)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNuocSX", maNuocSX),
                new SqlParameter("@TenNuocSX", tenNuocSX)
            };

            try
            {
                DatabaseManager.Instance.ExecuteNonQuery(query, parameters);
                return maNuocSX;
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi khi thêm nước sản xuất: " + ex.Message);
                return null;
            }
        }

        private string GenerateMaNuocSX()
        {
            string query = "SELECT TOP 1 MaNuocSX FROM NuocSX ORDER BY MaNuocSX DESC";

            try
            {
                object result = DatabaseManager.Instance.ExecuteScalar(query);
                if (result != null)
                {
                    string lastMa = result.ToString();
                    int number = int.Parse(lastMa.Substring(3)) + 1;
                    return "NSX" + number.ToString("D3");
                }
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi khi tạo mã nước sản xuất: " + ex.Message);
            }
            return "NSX001";
        }


        public void AddChatLieuToDatabase(string maChatLieu, string tenChatLieu)
        {
            string query = "INSERT INTO ChatLieu (MaChatLieu, TenChatLieu) VALUES (@MaChatLieu, @TenChatLieu)";
            SqlParameter[] parameters =
            {
            new SqlParameter("@MaChatLieu", maChatLieu),
            new SqlParameter("@TenChatLieu", tenChatLieu)
        };
            DatabaseManager.Instance.ExecuteNonQuery(query, parameters);
        }

    }
}

