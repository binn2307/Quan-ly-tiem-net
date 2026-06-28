using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYNET
{
    public class BLL
    {
        QLN db = new QLN();
        public User GetUserById(string id)
        {
            User us = db.Users.Where(u => u.Id_User == id).FirstOrDefault();
            return us;
        }

        public User GetUserByName(string name)
        {
            return db.Users.Where(u => u.HoTen == name).FirstOrDefault();
        }

        /// <summary>
        /// Refresh user data từ database (lấy dữ liệu mới nhất)
        /// </summary>
        public User RefreshUser(string id)
        {
            // Xóa entry cũ khỏi cache
            var entry = db.Users.FirstOrDefault(u => u.Id_User == id);
            if (entry != null)
            {
                db.Entry(entry).Reload();
            }
            return entry;
        }
        public List<MayTinh> GetAllMay()
        {
            return db.MayTinhs.ToList();
        }
        public MayTinh GetMayTinhById(string id)
        {
            return db.MayTinhs.Where(m => m.Id_May == id).FirstOrDefault();
        }
        public void SaveDB()
        {
            db.SaveChanges();
        }
        public void AddMayTinh(MayTinh mt)
        {
            db.MayTinhs.Add(mt);
            db.SaveChanges();
        }
        public void DelMayTinh(MayTinh mt)
        {
            db.MayTinhs.Remove(mt);
            db.SaveChanges();
        }
        public List<User> GetAllUser()
        {
            return db.Users.ToList().OrderBy(u => int.Parse(u.Id_User)).ToList();
        }

        public void AddUser(User user)
        {
            if (db.Users.Count() == 0)
            {
                user.Id_User = "1";
            }
            else
            {
                int maxId = db.Users.Max(u => int.Parse(u.Id_User));
                user.Id_User = (maxId + 1).ToString();
            }
            db.Users.Add(user);
            db.SaveChanges();
        }

        public void DelUser(User user)
        {
            var userToDelete = db.Users.FirstOrDefault(u => u.Id_User == user.Id_User);
            if (userToDelete != null)
            {
                db.Users.Remove(userToDelete);
                db.SaveChanges();

                // Sắp xếp lại ID
                var allUsers = db.Users.OrderBy(u => int.Parse(u.Id_User)).ToList();
                for (int i = 0; i < allUsers.Count; i++)
                {
                    allUsers[i].Id_User = (i + 1).ToString();
                }
                db.SaveChanges();
            }
        }

        // Tính chi phí phiên sử dụng
        public decimal CalculateSessionCost(PhienSuDung phien)
        {
            if (phien.KetThuc == null)
                return 0;

            TimeSpan duration = phien.KetThuc.Value - phien.BatDau;
            decimal hours = (decimal)duration.TotalHours;

            var maytinh = db.MayTinhs.Where(m => m.Id_May == phien.Id_May).FirstOrDefault();
            if (maytinh == null)
                return 0;

            decimal cost = hours * maytinh.Price;
            return cost;
        }

        // Kết thúc phiên sử dụng và trừ tiền tài khoản
        public bool EndSession(string idPhien)
        {
            var phien = db.PhienSuDungs.Where(p => p.Id_phien == idPhien).FirstOrDefault();
            if (phien == null)
                return false;

            // Cập nhật thời gian kết thúc
            phien.KetThuc = DateTime.Now;

            // Tính chi phí
            decimal cost = CalculateSessionCost(phien);
            phien.TongTien = cost;

            // Lấy thông tin người dùng
            var user = db.Users.FirstOrDefault(u => u.Id_User == phien.Id_User);
            if (user == null)
                return false;

            // Kiểm tra tài khoản có đủ tiền không
            if (user.TaiKhoan < cost)
                return false;

            // Trừ tiền tài khoản
            user.TaiKhoan -= cost;

            // Lưu thay đổi
            db.SaveChanges();
            return true;
        }

        // Lấy danh sách máy tính không hoạt động (IsActive = false)
        public List<MayTinh> GetAvailableMayTinh()
        {
            return db.MayTinhs.Where(m => m.IsActive == false).ToList();
        }

        // Tạo phiên sử dụng mới
        public string CreateNewSession(string userId, string mayTinhId)
        {
            try
            {
                var user = db.Users.FirstOrDefault(u => u.Id_User == userId);
                var maytinh = db.MayTinhs.FirstOrDefault(m => m.Id_May == mayTinhId);

                if (user == null || maytinh == null)
                    return null;

                // Kiểm tra máy đã được sử dụng hay chưa
                var existingSession = db.PhienSuDungs.FirstOrDefault(p => p.Id_May == mayTinhId && p.KetThuc == null);
                if (existingSession != null)
                    return null; // Máy đang được sử dụng

                // Tạo ID phiên mới (có thể dùng GUID hoặc timestamp)
                string sessionId = Guid.NewGuid().ToString();

                PhienSuDung phien = new PhienSuDung
                {
                    Id_phien = sessionId,
                    Id_User = userId,
                    Id_May = mayTinhId,
                    BatDau = DateTime.Now,
                    KetThuc = null,
                    TongTien = 0
                };

                // Cập nhật trạng thái máy thành active
                maytinh.IsActive = true;

                db.PhienSuDungs.Add(phien);
                db.SaveChanges();

                return sessionId;
            }
            catch
            {
                return null;
            }
        }

        // Lấy phiên sử dụng đang hoạt động của user
        public PhienSuDung GetActiveSessionByUserId(string userId)
        {
            return db.PhienSuDungs.FirstOrDefault(p => p.Id_User == userId && p.KetThuc == null);
        }
        public PhienSuDung GetSessionById(string idPhien)
        {
            return db.PhienSuDungs.Where(p => p.Id_phien == idPhien).FirstOrDefault();
        }
    }
}
