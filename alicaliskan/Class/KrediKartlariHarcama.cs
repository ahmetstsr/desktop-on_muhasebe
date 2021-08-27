using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alicaliskan.Class
{
    class KrediKartlariHarcama
    {
        [Key]
        public int HarcamaID { get; set; }
        public DateTime IslemTarihi { get; set; }
        public string CalisilanFirma { get; set; }
        public decimal IslemTutari { get; set; }
        public int TaksitSaysi { get; set; }
        public string SubeAdi { get; set; }
        public string BankaAdi { get; set; }
    }
}
