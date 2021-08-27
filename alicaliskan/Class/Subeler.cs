using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alicaliskan.Class
{
    class Subeler
    {
        [Key]
        public int SubeID { get; set; }
        public string SubeAdi { get; set; }
    }
}
