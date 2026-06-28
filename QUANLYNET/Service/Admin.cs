using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QUANLYNET
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
            setui();
        }
        

        private void button2_Click(object sender, EventArgs e)
        {

        }
        public void setui()
        {
            BLL bll = new BLL();
            dataGridView1.DataSource = bll.GetAllMay();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            BLL bll = new BLL();
            dataGridView1.DataSource = bll.GetAllMay();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ComputerService cs = new ComputerService(null);
            cs.d += new ComputerService.MyDel(setui);
            cs.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                string id = dataGridView1.SelectedRows[0].Cells["Id_May"].Value.ToString();
                ComputerService cs = new ComputerService(id);
                cs.d += new ComputerService.MyDel(setui);
                cs.ShowDialog();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                foreach(DataGridViewRow may in dataGridView1.SelectedRows)
                {
                    string id = may.Cells["Id_May"].Value.ToString();
                    BLL bll = new BLL();
                    MayTinh mt = bll.GetMayTinhById(id);
                    bll.DelMayTinh(mt);
                    MessageBox.Show("Xoa thanh cong may" + mt.Name);
                }
            }
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            UserWindow uw = new UserWindow();
            uw.d += new UserWindow.MyDel(setui);
            uw.ShowDialog();
        }
    }
}
