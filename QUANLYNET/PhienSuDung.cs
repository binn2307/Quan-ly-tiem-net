using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYNET
{
    public class PhienSuDung
    {
        [Key]
        public string Id_phien { get; set; }
        public DateTime BatDau { get; set; }
        public DateTime? KetThuc { get; set; }
        public decimal TongTien { get; set; }

        public string Id_May { get; set; }
        [ForeignKey("Id_May")]
        public virtual MayTinh maytinh { get; set; }
        public string Id_User { get; set; }
        [ForeignKey("Id_User")]
        public virtual User user { get; set; }

    }
}
