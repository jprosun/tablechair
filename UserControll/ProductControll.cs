using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static tablechair.DataBaseHelper;

namespace tablechair.UserControll
{
    public partial class ProductControll : UserControl
    {
        private Controll controll;
        private string selectedProductId;
        private MenuForm parentForm; 
        private readonly DataBaseHelper databaseHelper = new DataBaseHelper();

        public ProductControll()
        {
            InitializeComponent();
            controll = new Controll();
            InitializeDataGridView();
            LoadDataIntoDataGridView();

            comboBoxLoaiSp.DataSource = databaseHelper.LoadLoaiSanPham();
            comboBoxNSX.DataSource = databaseHelper.LoadNuocSX();
            comboBoxChatLieu.DataSource = databaseHelper.LoadChatLieu();
            comboBoxChatLieu.SelectedIndexChanged += comboBoxChatLieu_SelectedIndexChanged;

            comboBoxNSX.SelectedIndexChanged += comboBoxNSX_SelectedIndexChanged;
            comboBoxNSX.DisplayMember = "Text"; // Hiển thị tên nước sản xuất
            comboBoxNSX.ValueMember = "Value";

            dataGridView1.CellClick += dataGridView1_CellClick;
        }
        private void InitializeDataGridView()
        {
            dataGridView1.ColumnCount = 9; 
            dataGridView1.Columns[0].Name = "MaHang"; 
            dataGridView1.Columns[0].Visible = false; 
            dataGridView1.Columns[1].Name = "Tên Hàng Hóa";
            dataGridView1.Columns[2].Name = "Chất Liệu";
            dataGridView1.Columns[3].Name = "Loại";
            dataGridView1.Columns[4].Name = "Nước Sản Xuất";
            dataGridView1.Columns[5].Name = "Thời Gian Bảo Hành";
            dataGridView1.Columns[6].Name = "Đơn Giá Bán";
            dataGridView1.Columns[7].Name = "Đơn Giá Nhập";
            dataGridView1.Columns[8].Name = "Số Lượng";
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;
        }

        private void LoadDataIntoDataGridView()
        {
            dataGridView1.Rows.Clear();

            string query = @"
SELECT 
    DMHangHoa.MaHang,
    DMHangHoa.TenHangHoa, 
    ChatLieu.TenChatLieu, 
    Loai.TenLoai, 
    NuocSX.TenNuocSX, 
    DMHangHoa.ThoiGianBaoHanh, 
    DMHangHoa.DonGiaBan, 
    DMHangHoa.DonGiaNhap, 
    DMHangHoa.SoLuong
FROM DMHangHoa
LEFT JOIN ChatLieu ON DMHangHoa.MaChatLieu = ChatLieu.MaChatLieu
LEFT JOIN Loai ON DMHangHoa.MaLoai = Loai.MaLoai
LEFT JOIN NuocSX ON DMHangHoa.MaNuocSX = NuocSX.MaNuocSX
/*WHERE DMHangHoa.IsDeleted = 0 OR DMHangHoa.IsDeleted IS NULL*/";


            try
            {
                DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string[] rowData = new string[]
                        {
                    row["MaHang"].ToString(),
                    row["TenHangHoa"].ToString(),
                    row["TenChatLieu"].ToString(),
                    row["TenLoai"].ToString(),
                    row["TenNuocSX"].ToString(),
                    row["ThoiGianBaoHanh"].ToString(),
                    row["DonGiaBan"].ToString(),
                    row["DonGiaNhap"].ToString(),
                    row["SoLuong"].ToString()
                        };
                        dataGridView1.Rows.Add(rowData);
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

        private void ProductControll_Load(object sender, EventArgs e)
        {
            SetPlaceholder(txtTenSP, "Nhập tên sản phẩm");
            SetPlaceholder(txtGiaN, "Nhập giá nhập SP");
            SetPlaceholder(txtGiaBan, "Nhập giá bán SP");
            SetPlaceholder(txtBaoHanh, "Nhập thời gian BH");
            SetPlaceholder(txtSoLuong, "Nhập số lượng SP");
        }

        private void SetPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedProductId = row.Cells["MaHang"].Value?.ToString();
                txtTenSP.Text = row.Cells[1].Value?.ToString();
                comboBoxChatLieu.Text = row.Cells[2].Value?.ToString();
                comboBoxLoaiSp.Text = row.Cells[3].Value?.ToString();
                comboBoxNSX.Text = row.Cells[4].Value?.ToString();
                txtBaoHanh.Text = row.Cells[5].Value?.ToString();
                txtGiaBan.Text = row.Cells[6].Value?.ToString();
                txtGiaN.Text = row.Cells[7].Value?.ToString();
                txtSoLuong.Text = row.Cells[8].Value?.ToString();
            }
        }

        private void gunaBtnThem_Click(object sender, EventArgs e)
        {
            string tenSP = txtTenSP.Text.Trim();
            string chatLieu = comboBoxChatLieu.Text.Trim();
            string nuocSX = comboBoxNSX.Text.Trim();
            int soLuong;
            decimal giaNhap;
            int thoiGianBaoHanh;

            if (!int.TryParse(txtSoLuong.Text.Trim(), out soLuong) ||
                !decimal.TryParse(txtGiaN.Text.Trim(), out giaNhap) ||
                !int.TryParse(txtBaoHanh.Text.Trim(), out thoiGianBaoHanh))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng cho các trường số!");
                return;
            }

            if (string.IsNullOrWhiteSpace(tenSP))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm.");
                return;
            }
            if (comboBoxLoaiSp.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm.");
                return;
            }
            if (comboBoxChatLieu.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn chất liệu.");
                return;
            }
            var selectedNuocSX = comboBoxNSX.SelectedItem as ComboBoxItem;
            string maNuocSX = selectedNuocSX?.Value;

            if (string.IsNullOrWhiteSpace(maNuocSX))
            {
                MessageBox.Show("Vui lòng chọn nước sản xuất.");
                return;
            }

            string maChatLieu = databaseHelper.CheckChatLieu(chatLieu);
            if (maChatLieu == null)
            {
                if (MessageBox.Show("Chất liệu không tồn tại. Bạn có muốn thêm chất liệu mới không?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    maChatLieu = databaseHelper.AddChatLieu(chatLieu);
                }
                else
                {
                    return;
                }
            }
            string maLoai = databaseHelper.GetMaLoaiFromTen(comboBoxLoaiSp.SelectedItem.ToString());
            if (maLoai == null)
            {
                MessageBox.Show("Mã loại không hợp lệ.");
                return;
            }
            try
            {
                controll.InsertProduct(maLoai, tenSP, maChatLieu, giaNhap, soLuong, "", "", maNuocSX, thoiGianBaoHanh);
                LoadDataIntoDataGridView();
                MessageBox.Show("Thêm sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private void comboBoxChatLieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxChatLieu.SelectedItem != null && comboBoxChatLieu.SelectedItem.ToString() == "Thêm chất liệu...")
            {

                AddChatLieuForm addChatLieuForm = new AddChatLieuForm();
                if (addChatLieuForm.ShowDialog() == DialogResult.OK)
                {
                    comboBoxChatLieu.DataSource = databaseHelper.LoadChatLieu();
                }
                comboBoxChatLieu.SelectedIndex = 0;
            }
        }

        private void comboBoxNSX_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = comboBoxNSX.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                string maNuocSX = selectedItem.Value; 

                if (string.IsNullOrEmpty(maNuocSX))
                {
                    AddNuocSX addNuocSXForm = new AddNuocSX();
                    addNuocSXForm.ShowDialog(); 
                    LoadNuocSXData();
                }
               
            }
        }

        private void LoadNuocSXData()
        {
            comboBoxNSX.DataSource = databaseHelper.LoadNuocSX();
            comboBoxNSX.DisplayMember = "Text";
            comboBoxNSX.ValueMember = "Value";
        }

        private void gunaBtnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string tenSP = dataGridView1.SelectedRows[0].Cells["Tên Hàng Hóa"].Value?.ToString();
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm: {tenSP}?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string query = "UPDATE DMHangHoa SET IsDeleted = 1 WHERE TenHangHoa = @TenHangHoa";

                    try
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                             {
                                new SqlParameter("@TenHangHoa", tenSP)
                             };
                        DatabaseManager.Instance.ExecuteNonQuery(query, parameters);

                        MessageBox.Show("Xóa sản phẩm thành công.");
                        LoadDataIntoDataGridView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.");
            }
        }

        private void gunaBtnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedProductId))
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để sửa.");
                return;
            }

            string tenSP = txtTenSP.Text.Trim();
            string chatLieu = comboBoxChatLieu.Text.Trim();
            string nuocSXTen = comboBoxNSX.Text.Trim();
            int soLuong;
            decimal giaNhap, giaBan;
            int thoiGianBaoHanh;

            if (!int.TryParse(txtSoLuong.Text.Trim(), out soLuong) ||
                !decimal.TryParse(txtGiaN.Text.Trim(), out giaNhap) ||
                !decimal.TryParse(txtGiaBan.Text.Trim(), out giaBan) ||
                !int.TryParse(txtBaoHanh.Text.Trim(), out thoiGianBaoHanh))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng cho các trường số!");
                return;
            }

            if (string.IsNullOrWhiteSpace(tenSP) || comboBoxLoaiSp.SelectedIndex < 0 ||
                comboBoxChatLieu.SelectedIndex < 0 || string.IsNullOrWhiteSpace(nuocSXTen))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            DataBaseHelper dbHelper = new DataBaseHelper();
            string maNuocSX = dbHelper.GetMaNuocSXFromTen(nuocSXTen);
            if (maNuocSX == null)
            {
                MessageBox.Show("Nước sản xuất không tồn tại.");
                return;
            }

            string maLoai = dbHelper.GetMaLoaiFromTen(comboBoxLoaiSp.SelectedItem.ToString());
            if (maLoai == null)
            {
                MessageBox.Show("Loại sản phẩm không hợp lệ.");
                return;
            }

            string maChatLieu = dbHelper.CheckChatLieu(chatLieu);
            if (dbHelper.UpdateProduct(selectedProductId, tenSP, maChatLieu, maLoai, maNuocSX, thoiGianBaoHanh, giaBan, giaNhap, soLuong))
            {
                MessageBox.Show("Sửa sản phẩm thành công.");
                LoadDataIntoDataGridView();
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm để sửa.");
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook|*.xlsx";
                sfd.Title = "Lưu file Excel";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        var workSheet = excel.Workbook.Worksheets.Add("Dữ liệu");

                        for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                        {
                            workSheet.Cells[1, i].Value = dataGridView1.Columns[i - 1].HeaderText;
                        }

                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                workSheet.Cells[i + 2, j + 1].Value = dataGridView1.Rows[i].Cells[j].Value;
                            }
                        }
                        FileInfo fi = new FileInfo(sfd.FileName);
                        excel.SaveAs(fi);
                    }
                    MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void gunaBtnChitiet_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string maHang = dataGridView1.SelectedRows[0].Cells["MaHang"].Value.ToString();
                MenuForm menuForm = this.ParentForm as MenuForm;
                if (menuForm != null)
                {
                    menuForm.ShowProductDetails(maHang);
                }
                else
                {
                    MessageBox.Show("Không thể tìm thấy MenuForm.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm.");
            }
        }

    }
}
