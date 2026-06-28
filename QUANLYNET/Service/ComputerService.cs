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
    public partial class ComputerService : Form
    {
        public delegate void MyDel();
        public MyDel d { get; set; }
        public string ID { get; set; }
        public bool IsEdit = false;
        public ComputerService(string id)
        {
            InitializeComponent();
            ID = id;
            SetGUI();
        }

        public void SetGUI()
        {
            if(ID != null && ID != "")
            {
                IsEdit = true;
                BLL bll = new BLL();
                MayTinh mt = bll.GetMayTinhById(ID);
                if(mt != null)
                {
                    txtID.Text = mt.Id_May;
                    txtName.Text = mt.Name;
                    txtPrice.Text = mt.Price.ToString();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsEdit)
            {
                BLL bll = new BLL();
                MayTinh mt = bll.GetMayTinhById(ID);
                mt.Name = txtName.Text;
                mt.Id_May = txtID.Text;
                mt.Price = decimal.Parse(txtPrice.Text);
                mt.IsActive = false;
                bll.SaveDB();
                MessageBox.Show("Edit thanh cong");
                d?.Invoke();
                this.Close();
            }
            else
            {
                BLL bll = new BLL();
                MayTinh mt = new MayTinh();
                foreach(MayTinh item in bll.GetAllMay())
                {
                    if (item.Id_May == txtID.Text)
                    {
                        MessageBox.Show("ID may da ton tai");
                        return;
                    }
                }
                mt.Id_May = txtID.Text;
                mt.Name = txtName.Text;
                mt.Price = decimal.Parse(txtPrice.Text);
                mt.IsActive = false;
                bll.AddMayTinh(mt);
                MessageBox.Show("Them may moi thanh cong");
                d?.Invoke();
                this.Close();
            }
        }
    }
}
