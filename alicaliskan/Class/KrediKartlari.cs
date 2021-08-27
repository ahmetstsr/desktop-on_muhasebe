using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alicaliskan.Class
{
    class KrediKartlari
    {
        [Key]
        public int KartID { get; set; }
        public string BankaAdi { get; set; }
        public byte Hesap_Kesim_Gunu { get; set; }
        public byte Odeme_Gunu { get; set; }    

    }
}
