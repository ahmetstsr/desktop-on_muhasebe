using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alicaliskan.Class
{
    class Odemeler
    {
        [Key]
        public int Odeme_ID { get; set; }     
        //public Nullable<System.DateTime> Odeme_Tarihi { get; set; }
        //public Nullable<System.DateTime> Son_Odeme_Tarihi { get; set; }
        public DateTime SonOdemeTarihi { get; set; }
        public DateTime IlkOdemeTarihi { get; set; }
        public decimal Odeme_Tutari { get; set; }
        public string Odeme_Kalemi { get; set; }       
        public string Odeme_Sube_Adi { get; set; }
        public string Odeme_Aciklama { get; set; }
        public string CalisilanFirma { get; set; }
        public string BankaAdi { get; set; }
        public string Odeme_Sekli { get; set; }
        public string Taksit_Durumu { get; set; }
    }
}
