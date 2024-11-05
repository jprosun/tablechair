using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace tablechair.UserControll
{
    public partial class KhachHang : UserControl
    {
        private MenuForm parentForm;

        public KhachHang(MenuForm parent)
        {
            InitializeComponent();
            parentForm = parent ?? throw new ArgumentNullException(nameof(parent)); 
            InitializeCustomerDataGridView();
            LoadCustomerDataIntoDataGridView();
        }

        private void InitializeCustomerDataGridView()
        {
            dgvCustomer.ColumnCount = 5;
            dgvCustomer.Columns[0].Name = "Mã Khách";
            dgvCustomer.Columns[1].Name = "Tên Khách";
            dgvCustomer.Columns[2].Name = "Địa Chỉ";
            dgvCustomer.Columns[3].Name = "Điện Thoại";
            dgvCustomer.Columns[4].Name = "Tổng Tiền đã Chi Trả";

            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Chi tiết";
            buttonColumn.Name = "btnChiTiet";
            buttonColumn.Text = "Xem Chi tiết";
            buttonColumn.UseColumnTextForButtonValue = true;
            dgvCustomer.Columns.Add(buttonColumn);

            dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomer.ReadOnly = true;
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvCustomer.Columns["btnChiTiet"].Index && e.RowIndex >= 0)
            {
                var cellValue = dgvCustomer.Rows[e.RowIndex].Cells["Mã Khách"].Value;

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
            SUM(ChiTietHoaDonBan.ThanhTien) AS TongTien
        FROM KhachHang
        LEFT JOIN HoaDonBan ON KhachHang.MaKhach = HoaDonBan.MaKhach
        LEFT JOIN ChiTietHoaDonBan ON HoaDonBan.SoHDB = ChiTietHoaDonBan.SoHDB
        GROUP BY KhachHang.MaKhach, KhachHang.TenKhach, KhachHang.DiaChi, KhachHang.DienThoai";

            try
            {
                DataTable result = DatabaseManager.Instance.ExecuteQuery(query);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        string[] dataRow = new string[]
                        {
                    row["MaKhach"].ToString(),
                    row["TenKhach"].ToString(),
                    row["DiaChi"].ToString(),
                    row["DienThoai"].ToString(),
                    row["TongTien"] != DBNull.Value ? row["TongTien"].ToString() : "0"
                        };
                        dgvCustomer.Rows.Add(dataRow);
                    }
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu nào được tìm thấy.");
                }
            }
            catch (Exception ex)
            {
                DatabaseManager.Instance.ShowErrorMessage("Lỗi: " + ex.Message);
            }
        }

        private void iconButton1_Click_1(object sender, EventArgs e)
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
    }
}
