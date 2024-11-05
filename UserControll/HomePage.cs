using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tablechair.UserControll
{
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            NameLb.Text = "Mai Việt Hùng";
            IDLb.Text = "B1809276";
            JobLb.Text = "Quản lý";

            //int monthInvoice = ;

            MonthInvoiceLb.Text = 30000000.ToString("N0") + " VND";
            BillTodayLb.Text = "10";
            ProductLb.Text = "100";
        }
    }
}

