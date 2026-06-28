using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYNET
{
    public class User
    {
        public User()
        {
            PhienSuDungs = new HashSet<PhienSuDung>();
        }
        [Column("MaNguoiDung")]
        [Key]
        public string Id_User { get; set; }
        public string HoTen { get; set; }
        public string MatKhau { get; set; }
        public string SDT { get; set; }
        public decimal TaiKhoan { get; set; }
        public virtual ICollection<PhienSuDung> PhienSuDungs { get; set; }
    }
}
