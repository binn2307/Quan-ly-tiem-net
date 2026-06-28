using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QUANLYNET
{
    public partial class UserService : Form
    {
        public delegate void MyDel();
        public MyDel d { get; set; }
        public string ID { get; set; }
        public bool IsEdit =false;
        public UserService(string id)
        {
            InitializeComponent();
            ID = id;
            SetUI();
        }
        public void SetUI()
        {
            if (ID != null && ID != "")
            {
                IsEdit = true;
                BLL bll = new BLL();
                User us = bll.GetUserById(ID);
                if(us != null)
                {
                    txtName.Text = us.HoTen;
                    txtSDT.Text = us.SDT;
                    txtPass.Text = us.MatKhau;
                    txtNap.Text = us.TaiKhoan.ToString();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsEdit)
            {
                BLL bll = new BLL();
                User us = bll.GetUserById(ID);
                if(us != null)
                {
                    us.HoTen = txtName.Text;
                    us.MatKhau = txtPass.Text;
                    us.SDT = txtSDT.Text;

                    // Cộng thêm tiền nạp vào tài khoản hiện có
                    decimal napTien = decimal.Parse(txtNap.Text);
                    us.TaiKhoan += napTien;

                    bll.SaveDB();
                    MessageBox.Show("Nạp tiền thành công! Số tiền nạp: " + napTien.ToString("N0") + " VND\nSố dư mới: " + us.TaiKhoan.ToString("N0") + " VND");
                    d?.Invoke();
                    this.Close();
                }
            }
            else
            {
                User us = new User
                {
                    HoTen = txtName.Text,
                    TaiKhoan = decimal.Parse(txtNap.Text),
                    SDT = txtSDT.Text,
                    MatKhau = txtPass.Text
                };
                BLL bll = new BLL();
                bll.AddUser(us);
                MessageBox.Show("Thêm user thành công!");
                d?.Invoke();
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
    }
    
}
