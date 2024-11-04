using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tablechair
{
    public partial class AddChatLieuForm : Form
    {
        private DataBaseHelper databaseHelper;

        public AddChatLieuForm()
        {
            InitializeComponent();
            databaseHelper = new DataBaseHelper();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string tenChatLieu = txtTenChatLieu.Text.Trim();
            if (string.IsNullOrWhiteSpace(tenChatLieu))
            {
                MessageBox.Show("Vui lòng nhập tên chất liệu.");
                return;
            }
            if (IsChatLieuExists(tenChatLieu))
            {
                MessageBox.Show("Chất liệu này đã tồn tại trong cơ sở dữ liệu.");
                return;
            }

            string maChatLieu = databaseHelper.GenerateMaChatLieu(); 
            databaseHelper.AddChatLieuToDatabase(maChatLieu, tenChatLieu); 
            MessageBox.Show("Chất liệu đã được thêm thành công!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool IsChatLieuExists(string tenChatLieu)
        {
            string query = "SELECT COUNT(*) FROM ChatLieu WHERE TenChatLieu = @TenChatLieu";
            SqlParameter[] parameters =
            {
            new SqlParameter("@TenChatLieu", tenChatLieu)
        };
            int count = (int)DatabaseManager.Instance.ExecuteScalar(query, parameters);
            return count > 0;
        }
    }
}
