using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alicaliskan.Class
{
    class Context:DbContext
    {        
        public DbSet<KrediKartlariHarcama> KrediKartlariHarcamas { get; set; }
        public DbSet<SabitGiderler> SabitGiderlers { get; set; }
        public DbSet<TumGiderler> TumGiderlers { get; set; }
        public DbSet<KrediKartlari> KrediKartlaris { get; set; }
        public DbSet<Subeler> Subelers { get; set; }
        public DbSet<Giderler> Giderlers { get; set; }
        public DbSet<Odemeler> Odemelers { get; set; }
        public DbSet<Gelirler> Gelirlers { get; set; }
        public DbSet<POSCihazi> PosCihazi { get; set; }
        public DbSet<Kullanici> Kullanicis { get; set; }



    }
}
