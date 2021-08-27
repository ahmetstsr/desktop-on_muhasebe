using alicaliskan.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alicaliskan
{
    public partial class Raporlar : Form
    {
        public Raporlar()
        {
            InitializeComponent();
        }
        string BuAy;
        int BuYil;
        Class.Context db = new Class.Context(); 
        private void Raporlar_Load(object sender, EventArgs e)
        {
            BuAy = DateTime.Now.ToString("MMMM");
            BuYil = DateTime.Now.Year;

            cbAy.Text = KullaniciSecimleri.Secilen_Ay.ToString();
            cbYil.Text = KullaniciSecimleri.Secilen_Yil.ToString();

           
            for (int i = BuYil; i > 2019; i--)
            {
                cbYil.Items.Add(i.ToString());
            }

            for (int i = 1; i <=12; i++)
            {
                cbAy.Items.Add(i.ToString());
            }
            var OdemeListesi = db.Odemelers.Where(
                x => x.IlkOdemeTarihi.Year == KullaniciSecimleri.Secilen_Yil &&
                x.IlkOdemeTarihi.Month == KullaniciSecimleri.Secilen_Ay &&
                x.Odeme_Sekli != "Nakit"
            ).OrderBy(x => x.SonOdemeTarihi).ToList();
            dataGridView1.DataSource = OdemeListesi;
        }

        private void btnBuAy_Click(object sender, EventArgs e)
        {
            var GiderListesi = db.Giderlers.Where(
                x => x.Islem_Tarihi.Month == KullaniciSecimleri.Secilen_Ay &&
                x.Islem_Tarihi.Year == KullaniciSecimleri.Secilen_Yil &&
                x.Odeme_Sekli == "Kredi Kartı").ToList();            
            dataGridView1.DataSource = GiderListesi;
        }

        private void button1_Click(object sender, EventArgs e)
        {           
            var KartlaraGoreToplam = db.Giderlers.Where(
                x => x.Islem_Tarihi.Month == KullaniciSecimleri.Secilen_Ay &&
                x.Odeme_Sekli == "Kredi Kartı" &&
                x.Islem_Tarihi.Year == KullaniciSecimleri.Secilen_Yil).
                GroupBy(x => x.BankaAdi).
                Select(y => new {
                    Banka = y.Key,
                    Toplam_Harcama = y.Sum(a => a.Islem_Tutari),
                });
            dataGridView1.DataSource = KartlaraGoreToplam.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Bu Ay Tüm Giderler
            var AylikTumGgiderler = db.Giderlers.Where(
                x => x.Islem_Tarihi.Month == KullaniciSecimleri.Secilen_Ay &&
                x.Islem_Tarihi.Year == KullaniciSecimleri.Secilen_Yil).
                OrderBy(x=> x.Islem_Tarihi);
            dataGridView1.DataSource = AylikTumGgiderler.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var GiderListesi = db.Giderlers.Where(
                x => x.Islem_Tarihi.Month == KullaniciSecimleri.Secilen_Ay &&
                x.Islem_Tarihi.Year == KullaniciSecimleri.Secilen_Yil).ToList();
            dataGridView1.DataSource = GiderListesi;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var OdemeListesi = db.Odemelers.ToList();
            dataGridView1.DataSource = OdemeListesi;
        }

        private void cbYil_SelectedIndexChanged(object sender, EventArgs e)
        {
            KullaniciSecimleri.Secilen_Yil = int.Parse(cbYil.SelectedItem.ToString());
        }

        private void cbAy_SelectedIndexChanged(object sender, EventArgs e)
        {
            KullaniciSecimleri.Secilen_Ay = int.Parse(cbAy.SelectedItem.ToString());
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            var OdemeListesi = db.Odemelers.Where(
                x=> x.IlkOdemeTarihi.Year == KullaniciSecimleri.Secilen_Yil &&
                x.IlkOdemeTarihi.Month == KullaniciSecimleri.Secilen_Ay &&
                x.Odeme_Sekli != "Nakit"
            ).OrderBy(x=> x.SonOdemeTarihi).ToList();
            dataGridView1.DataSource = OdemeListesi;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OzetTablo ot = new OzetTablo();
            ot.Show();
        }
    }
}
