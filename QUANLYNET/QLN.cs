using System;
using System.Data.Entity;
using System.Linq;

namespace QUANLYNET
{
    public class QLN : DbContext
    {
        public QLN()
            : base("name=QLN")
        {
            Database.SetInitializer(new CreateDB());
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MayTinh> MayTinhs { get; set; }
        public virtual DbSet<PhienSuDung> PhienSuDungs { get; set; }
    }
}