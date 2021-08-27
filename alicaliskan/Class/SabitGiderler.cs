using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alicaliskan.Class
{
    class SabitGiderler
    {
        [Key]
        public int Sabit_Gider_ID { get; set; }        
        public decimal Sabit_Gider_Tutari { get; set; }
        public string Sabit_Gider_Kalemi { get; set; }   
        public string Sabit_Gider_Aciklama { get; set; }
        public virtual Subeler Sube { get; set; }
    }
}
