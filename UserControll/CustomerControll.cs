using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace tablechair.UserControll
{
    public partial class CustomerControll : UserControl
    {
        private MenuForm parentForm;

        public CustomerControll(MenuForm parent)
        {
            InitializeComponent();
            parentForm = parent ?? throw new ArgumentNullException(nameof(parent)); // Kiểm tra xem parent có null không
            InitializeCustomerDataGridView();
            LoadCustomerDataIntoDataGridView();
        }

        public CustomerControll()
        {
            // Constructor rỗng nếu cần thiết
        }

        private void InitializeCustomerDataGridView()
        {
            dgvCustomer.ColumnCount = 8; // Tăng số lượng cột lên 9
            dgvCustomer.Columns[0].Name = "Mã Khách";
            dgvCustomer.Columns[1].Name = "Tên Khách";
            dgvCustomer.Columns[2].Name = "Địa Chỉ";
            dgvCustomer.Columns[3].Name = "Điện Thoại";
            dgvCustomer.Columns[4].Name = "Số HDB";
            dgvCustomer.Columns[5].Name = "Mã Hàng";
            dgvCustomer.Columns[6].Name = "Số Lượng";
            dgvCustomer.Columns[7].Name = "Thành Tiền";

            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Chi tiết";
            buttonColumn.Name = "btnChiTiet";
            buttonColumn.Text = "Xem Chi tiết";
            buttonColumn.UseColumnTextForButtonValue = true;
            dgvCustomer.Columns.Add(buttonColumn);
            dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomer.ReadOnly = true;
        }

        private void DgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvCustomer.Columns["btnChiTiet"].Index && e.RowIndex >= 0)
            {
                var cellValue = dgvCustomer.Rows[e.RowIndex].Cells["Mã Khách"].Value;

                // Kiểm tra giá trị của ô có null không
                if (cellValue != null)
                {
                    string maKhach = cellValue.ToString();
                    parentForm.ShowOrderDetails(maKhach);
                }
                else
                {
                    MessageBox.Show("Mã khách không hợp lệ.");
                }
            }
        }

        private void LoadCustomerDataIntoDataGridView()
        {
            dgvCustomer.Rows.Clear();
            string query = @"
                SELECT 
                    KhachHang.MaKhach,
                    KhachHang.TenKhach,
                    KhachHang.DiaChi,
                    KhachHang.DienThoai,
                    HoaDonBan.SoHDB,
                    ChiTietHoaDonBan.MaHang,
                    ChiTietHoaDonBan.SoLuong,
                    ChiTietHoaDonBan.ThanhTien
                FROM KhachHang
                LEFT JOIN HoaDonBan ON KhachHang.MaKhach = HoaDonBan.MaKhach
                LEFT JOIN ChiTietHoaDonBan ON HoaDonBan.SoHDB = ChiTietHoaDonBan.SoHDB";

            try
            {
                DataTable customerData = DatabaseManager.Instance.ExecuteQuery(query);
                if (customerData.Rows.Count > 0)
                {
                    foreach (DataRow row in customerData.Rows)
                    {
                        string[] data = new string[]
                        {
                            row["MaKhach"].ToString(),
                            row["TenKhach"].ToString(),
                            row["DiaChi"].ToString(),
                            row["DienThoai"].ToString(),
                            row["SoHDB"] != DBNull.Value ? row["SoHDB"].ToString() : "N/A",
                            row["MaHang"] != DBNull.Value ? row["MaHang"].ToString() : "N/A",
                            row["SoLuong"] != DBNull.Value ? row["SoLuong"].ToString() : "0",
                            row["ThanhTien"] != DBNull.Value ? row["ThanhTien"].ToString() : "0"
                        };
                        dgvCustomer.Rows.Add(data);
                    }
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu nào được tìm thấy.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel._Worksheet)excelApp.ActiveSheet;
            for (int i = 0; i < dgvCustomer.Columns.Count; i++)
            {
                workSheet.Cells[1, i + 1] = dgvCustomer.Columns[i].HeaderText; // Xuất tiêu đề
            }
            for (int i = 0; i < dgvCustomer.Rows.Count; i++)
            {
                for (int j = 0; j < dgvCustomer.Columns.Count; j++)
                {
                    var cellValue = dgvCustomer.Rows[i].Cells[j].Value;
                    workSheet.Cells[i + 2, j + 1] = cellValue != null ? cellValue.ToString() : ""; // Xuất dữ liệu, sử dụng "" nếu null
                }
            }
            excelApp.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }

        private void dgvCustomer_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
