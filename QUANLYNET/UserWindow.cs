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
    public partial class UserWindow : Form
    {
        public delegate void MyDel();
        public MyDel d { get; set; }
        public UserWindow()
        {
            InitializeComponent();
            SetGUi();
        }
        public void SetGUi()
        {
            BLL bll = new BLL();
            dataGridView1.DataSource = bll.GetAllUser();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string userId = selectedRow.Cells["Id_User"].Value.ToString();

                BLL bll = new BLL();
                User user = bll.GetUserById(userId);

                if (user != null)
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa user này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        bll.DelUser(user);
                        SetGUi();
                        MessageBox.Show("Xóa user thành công!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một user để xóa!");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserService us = new UserService(null);
            us.d += new UserService.MyDel(SetGUi);
            us.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                string id = dataGridView1.SelectedRows[0].Cells["Id_User"].Value.ToString();
                UserService us = new UserService(id);
                us.d += new UserService.MyDel(SetGUi);
                us.ShowDialog();
            }
        }
    }
}
