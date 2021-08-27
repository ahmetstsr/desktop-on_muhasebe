using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alicaliskan.Class
{
    class POSCihazi
    {
        [Key]
        public int KartID { get; set; }
        public string BankaAdi { get; set; }        
        public byte Odeme_Gunu { get; set; }
    }
}
