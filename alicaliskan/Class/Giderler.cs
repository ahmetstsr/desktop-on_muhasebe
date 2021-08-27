using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alicaliskan.Class
{
    class Giderler
    {
        [Key]
        public int Gider_ID { get; set; }
        public DateTime Islem_Tarihi { get; set; }       
        public decimal Islem_Tutari { get; set; }
        public string Islem_Kalemi { get; set; }
        public byte Islem_Taksit_Saysi { get; set; }
        public string Islem_Sube_Adi { get; set; }       
        public string Islem_Aciklama { get; set; }      
        public string CalisilanFirma { get; set; }
        public string BankaAdi { get; set; }
        public string Odeme_Sekli { get; set; }
        public virtual KrediKartlari SecilenKart { get; set; }
    }
}
