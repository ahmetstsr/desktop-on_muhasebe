using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alicaliskan.Class
{
    class Gelirler
    {
        [Key]
        public int Gelir_ID { get; set; }
        public DateTime Gelir_Islem_Tarihi { get; set; }
        public DateTime Gelir_Odenecek_Tarih { get; set; }
        public decimal Gelir_Tutari { get; set; }        
        public string Islem_Sube_Adi { get; set; }
        public string Islem_Aciklama { get; set; }      
        public string Odeme_Sekli { get; set; }
        
    }
}
