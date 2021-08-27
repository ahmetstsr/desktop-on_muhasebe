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
    public partial class GelirEkrani : Form
    {
        public GelirEkrani()
        {
            InitializeComponent();
        }
        Class.Context db = new Class.Context();
        private void GelirEkrani_Load(object sender, EventArgs e)
        {
            var Subeler = db.Subelers.Where(
                x=> x.SubeAdi != "Fatih" &&
                x.SubeAdi != "Muhlis" &&
                x.SubeAdi != "Ortak Harcama").ToList();

            cbSube.DataSource = Subeler;
            cbSube.DisplayMember = "SubeAdi";
            cbSube.ValueMember = "SubeAdi";

            var GelirSekli = db.PosCihazi.ToList();

            cbGelirSekli.DataSource = GelirSekli;
            cbGelirSekli.DisplayMember = "BankaAdi";
            cbGelirSekli.ValueMember = "BankaAdi";

            VeritabaniniGoruntule();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            EskiKayitlariSil(); //Sadece son 2 yılın kayıtlarını saklamak istiyoruz
            Gelirler YeniGelir = new Gelirler();

            //Kredi kartı ile yapılan ödemelerde karta göre ödeme günü değişiyor
            int eklenecekGun = db.PosCihazi.Where(
                x => x.BankaAdi == cbGelirSekli.SelectedValue.ToString()).FirstOrDefault().Odeme_Gunu;

            YeniGelir.Gelir_Islem_Tarihi = dateTimePicker1.Value.Date;
            YeniGelir.Gelir_Odenecek_Tarih = dateTimePicker1.Value.Date.AddDays(eklenecekGun);
            YeniGelir.Gelir_Tutari = Convert.ToDecimal(txtIslemTutari.Text);
            YeniGelir.Islem_Sube_Adi = cbSube.SelectedValue.ToString();
            YeniGelir.Odeme_Sekli = cbGelirSekli.SelectedValue.ToString();
            if(txtAciklama.Text != "")
            YeniGelir.Islem_Aciklama = txtAciklama.Text;

            db.Gelirlers.Add(YeniGelir);
            int sonuc = db.SaveChanges();
            if (sonuc > 0)
            {
                MessageBox.Show("Kayıt eklendi.");
                txtAciklama.Text = "";
                txtIslemTutari.Text = "";
            }
            else
            {
                MessageBox.Show("Kayıt eklenemedi.");
            }

                VeritabaniniGoruntule();

        }

        private void cbGelirSekli_SelectedIndexChanged(object sender, EventArgs e)
        {            
               
        }

        void VeritabaniniGoruntule()
        {
            var GelirListesi = db.Gelirlers.ToList();
            
            dataGridView1.DataSource = GelirListesi;
        }

        void EskiKayitlariSil()
        {
            int SilinecekYil = (DateTime.Now.Year)-2;

            List<Giderler> EskiGiderler = db.Giderlers.Where(
                x=> x.Islem_Tarihi.Year <= SilinecekYil).ToList();
            List<Odemeler> EskiOdemeler = db.Odemelers.Where(
                x => x.IlkOdemeTarihi.Year <= SilinecekYil).ToList();
            List<Gelirler> EskiGelirler = db.Gelirlers.Where(
                x => x.Gelir_Islem_Tarihi.Year == SilinecekYil).ToList();

            foreach (var item in EskiGiderler)
            {
                db.Giderlers.Remove(item);
            }

            foreach (var item in EskiOdemeler)
            {
                db.Odemelers.Remove(item);
            }
            foreach (var item in EskiGelirler)
            {
                db.Gelirlers.Remove(item);
            }
            db.SaveChanges();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            KrediKarti kk = new KrediKarti();
            kk.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Raporlar rpr = new Raporlar();
            rpr.Show();
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SabitGiderlerEkrani sge = new SabitGiderlerEkrani();
            sge.Show();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            KrediKartlariEkrani kke = new KrediKartlariEkrani();
            kke.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SubelerEkrani se = new SubelerEkrani();
            se.Show();
        }
    }
    
}
