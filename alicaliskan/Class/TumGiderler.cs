using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alicaliskan.Class
{
    class TumGiderler
    {
        [Key]
        public int Gider_ID { get; set; }
        public DateTime Gider_Tarihi { get; set; }
        public decimal Gider_Tutari { get; set; }
        public string Gider_Kalemi { get; set; }
        public int Gider_Taksit_Saysi { get; set; }
        public string Gider_Sube_Adi { get; set; }
        public bool Gider_Odeme_Durumu { get; set; }
        public string Gider_Aciklama { get; set; }
        public virtual SabitGiderler SabitGider { get; set; }
    }
}
