using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYNET
{
    [Table("Computer")]
    public class MayTinh
    {
        public MayTinh()
        {
            PhienSuDungs = new HashSet<PhienSuDung>();
        }
        [Key]
        [Column("MaMay")]
        
        public string Id_May { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        //Gia tinh theo h
        public decimal Price { get; set; } 
        public virtual ICollection<PhienSuDung> PhienSuDungs { get; set; }

    }
}
