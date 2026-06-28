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
    public partial class 
        UserComputerForm : Form
    {
        private BLL bll = new BLL();

        public UserComputerForm()
        {
            InitializeComponent();
        }

        private void UserComputerForm_Load(object sender, EventArgs e)
        {
            LoadAvailableComputers();
            UpdateUserInfo();
        }

        /// <summary>
        /// Tải danh sách máy tính có sẵn (IsActive = false)
        /// </summary>
        private void LoadAvailableComputers()
        {
            try
            {
                var availableComputers = bll.GetAvailableMayTinh();
                dataGridView1.DataSource = availableComputers;

                // Tùy chỉnh cột hiển thị
                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["Id_May"].HeaderText = "Mã Máy";
                    dataGridView1.Columns["Name"].HeaderText = "Tên Máy";
                    dataGridView1.Columns["Price"].HeaderText = "Giá/Giờ";
                    dataGridView1.Columns["IsActive"].Visible = false;
                    dataGridView1.Columns["PhienSuDungs"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách máy: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cập nhật thông tin user hiển thị
        /// </summary>
        private void UpdateUserInfo()
        {
            lblUserName.Text = "Tên: " + UserSession.FullName;
            lblBalance.Text = "Số dư: " + UserSession.Balance.ToString("N0") + " VND";
        }

        /// <summary>
        /// Nút Play - Tạo phiên sử dụng mới
        /// </summary>
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một máy tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy thông tin máy tính được chọn
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string mayTinhId = selectedRow.Cells["Id_May"].Value.ToString();
                string mayTinhName = selectedRow.Cells["Name"].Value.ToString();

                // Kiểm tra tài khoản có đủ tiền không
                if (UserSession.Balance <= 0)
                {
                    MessageBox.Show("Tài khoản của bạn không có tiền! Vui lòng nạp tiền.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo phiên sử dụng mới
                string sessionId = bll.CreateNewSession(UserSession.UserId, mayTinhId);

                if (sessionId != null)
                {
                    // Lưu ID phiên vào session
                    UserSession.CurrentSessionId = sessionId;

                    MessageBox.Show("Bắt đầu sử dụng máy " + mayTinhName + " thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Làm mới danh sách máy
                    LoadAvailableComputers();

                    // Có thể mở form hiển thị thông tin phiên đang chạy
                    // SessionForm sessionForm = new SessionForm();
                    // sessionForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Không thể tạo phiên sử dụng! Máy có thể đang được sử dụng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Nút Stop - Kết thúc phiên sử dụng
        /// </summary>
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserSession.CurrentSessionId))
            {
                MessageBox.Show("Không có phiên sử dụng đang chạy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool success = bll.EndSession(UserSession.CurrentSessionId);

                if (success)
                {
                    MessageBox.Show("Phiên kết thúc! Tiền đã bị trừ từ tài khoản.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Cập nhật thông tin tài khoản
                    var user = bll.GetUserById(UserSession.UserId);
                    UserSession.Balance = user.TaiKhoan;
                    UserSession.CurrentSessionId = null;

                    UpdateUserInfo();
                    LoadAvailableComputers();
                }
                else
                {
                    MessageBox.Show("Kết thúc phiên thất bại! Tài khoản có thể không đủ tiền.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Nút Logout - Đăng xuất
        /// </summary>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UserSession.CurrentSessionId))
            {
                MessageBox.Show("Vui lòng kết thúc phiên sử dụng trước khi đăng xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                UserSession.Clear();
                Login loginForm = new Login();
                loginForm.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Nút Refresh - Làm mới danh sách
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAvailableComputers();
            UpdateUserInfo();
        }

        private void UserComputerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Nếu đóng form mà còn phiên đang chạy
            if (!string.IsNullOrEmpty(UserSession.CurrentSessionId))
            {
                DialogResult result = MessageBox.Show("Bạn còn phiên sử dụng đang chạy! Bạn có muốn kết thúc phiên trước khi đóng?", "Cảnh báo", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Cancel)
                {
                    // Hủy đóng form
                    e.Cancel = true;
                }
                else if (result == DialogResult.Yes)
                {
                    // Kết thúc phiên trước khi đóng
                    try
                    {
                        bool success = bll.EndSession(UserSession.CurrentSessionId);
                        if (success)
                        {
                            // Cập nhật thông tin người dùng
                            var user = bll.GetUserById(UserSession.UserId);
                            UserSession.Balance = user.TaiKhoan;
                            UserSession.CurrentSessionId = null;

                            MessageBox.Show("Phiên đã kết thúc, tiền đã trừ. Tạm biệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không thể kết thúc phiên! Tài khoản có thể không đủ tiền.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Cancel = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi kết thúc phiên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                }
                else if (result == DialogResult.No)
                {
                    // Chọn No - Cảnh báo và hủy đóng form
                    DialogResult confirmClose = MessageBox.Show(
                        "⚠️ CẢNH BÁO!\n\n" +
                        "Nếu bạn đóng form mà không kết thúc phiên:\n" +
                        "• Phiên sẽ tiếp tục chạy và tính tiền\n" +
                        "• Tiền sẽ bị trừ từ tài khoản\n" +
                        "• Bạn sẽ không thể kiểm soát được chi phí\n\n" +
                        "Bạn có chắc chắn muốn tiếp tục?",
                        "Xác nhận lần nữa",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (confirmClose == DialogResult.No)
                    {
                        // Quay lại dialog chính
                        e.Cancel = true;
                    }
                    else
                    {
                        // Ghi log phiên "bỏ lơ"
                        try
                        {
                            var session = bll.GetSessionById(UserSession.CurrentSessionId);
                            if (session != null)
                            {
                                // Có thể ghi vào log file hoặc database
                                System.IO.File.AppendAllText(
                                    "abandoned_sessions.log",
                                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] User: {UserSession.UserId}, Session: {UserSession.CurrentSessionId}, Computer: {session.Id_May}\n");
                            }
                        }
                        catch { }

                        MessageBox.Show("Form sẽ đóng. Phiên vẫn đang chạy!\nHãy liên hệ Admin nếu có vấn đề.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UserSession.CurrentSessionId = null;
                        // Form được phép đóng
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
