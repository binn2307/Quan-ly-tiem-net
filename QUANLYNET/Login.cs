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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtUser != null && txtPass != null)
            {
                string username = txtUser.Text.Trim();
                string password = txtPass.Text;

                // Kiểm tra đăng nhập admin
                if(username == "admin" && password == "0")
                {
                    // Đăng nhập thành công với quyền admin
                    MessageBox.Show("Đăng nhập thành công với quyền Admin!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Admin adminform = new Admin();
                    adminform.Show();
                    this.Hide();
                }
                else if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    // Kiểm tra user bình thường trong database
                    BLL bll = new BLL();
                    User user = bll.GetUserByName(username);

                    if(user != null && user.MatKhau == password)
                    {
                        // Đăng nhập thành công với quyền user bình thường
                        MessageBox.Show("Đăng nhập thành công với quyền User!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh user data từ database (lấy dữ liệu mới nhất, đặc biệt là TaiKhoan)
                        user = bll.RefreshUser(user.Id_User);

                        // Lưu thông tin user vào session
                        UserSession.UserId = user.Id_User;
                        UserSession.FullName = user.HoTen;
                        UserSession.Balance = user.TaiKhoan;

                        // Mở form chính với quyền user
                        UserComputerForm userForm = new UserComputerForm();
                        userForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
