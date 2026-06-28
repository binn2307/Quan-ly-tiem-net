using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYNET
{
    public class CreateDB : DropCreateDatabaseAlways<QLN>
    {
        protected override void Seed(QLN context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(new User[]
                {
                    new User{ Id_User = "1", HoTen = "admin", SDT = "0905000001",MatKhau = "0",  TaiKhoan = 0},
                    new User{Id_User = "2", HoTen = "NVB" , SDT = "0905000002", MatKhau = "0", TaiKhoan = 0}
                });
                context.SaveChanges();
            }
            if (!context.MayTinhs.Any())
            {
                context.MayTinhs.AddRange(new MayTinh[]
                {
                    new MayTinh{Id_May = "01", IsActive = false, Name = "Standar01", Price = 6},
                    new MayTinh{Id_May = "02", IsActive = false, Name = "Vip01", Price = 7},
                    new MayTinh{Id_May = "03", IsActive = false, Name = "SuperVip01", Price = 8},
                    new MayTinh{Id_May = "04", IsActive = false, Name = "SSVip01", Price = 9}
                });
                context.SaveChanges();
            }
        }
    }
}
