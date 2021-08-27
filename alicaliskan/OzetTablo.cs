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
    public partial class OzetTablo : Form
    {
        public OzetTablo()
        {
            InitializeComponent();
        }

        Class.Context db = new Class.Context();
      
        string BuAy;
        int BuYil;

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            OzetTablo ot = new OzetTablo();
            ot.Show();
            this.Close();
        }

        private void cbAy_SelectedIndexChanged(object sender, EventArgs e)
        {
            KullaniciSecimleri.Secilen_Ay = int.Parse(cbAy.SelectedItem.ToString());
        }

        private void cbYil_SelectedIndexChanged(object sender, EventArgs e)
        {
            KullaniciSecimleri.Secilen_Yil = int.Parse(cbYil.SelectedItem.ToString());
        }

        private void OzetTablo_Load(object sender, EventArgs e)
        {
            BuAy = DateTime.Now.ToString("MMMM");
            BuYil = DateTime.Now.Year;
            cbAy.Text = KullaniciSecimleri.Secilen_Ay.ToString();
            cbYil.Text = KullaniciSecimleri.Secilen_Yil.ToString();

            
            for (int i = BuYil; i > BuYil-2; i--)
            {
                cbYil.Items.Add(i.ToString());
            }

            for (int i = 1; i <= 12; i++)
            {
                cbAy.Items.Add(i.ToString());
            }

            btnOdemeGenelBaslik.Text = KullaniciSecimleri.Secilen_Yil + " Yılı " + KullaniciSecimleri.Secilen_Ay + ". Ay Yapılacak Toplam Ödeme Tutarları";
            var Kartlar = db.KrediKartlaris.ToList();
            var Subeler = db.Subelers.ToList();
            var Odemeler = db.Odemelers.ToList();
            var SabitGiderler = db.SabitGiderlers.ToList();
           
            string[] SubeIsimleri = new string[Subeler.Count];
            int sayac = 0;
            foreach (var item in Subeler)
            {
                SubeIsimleri[sayac] = item.SubeAdi;
                sayac++;
            }

            //Kredi Kart Ödeme Tablosu
            var AylikKartOdemeleri = db.Odemelers.Where(
                x => x.IlkOdemeTarihi.Month == KullaniciSecimleri.Secilen_Ay &&
                x.IlkOdemeTarihi.Year == KullaniciSecimleri.Secilen_Yil &&
                x.Odeme_Sekli == "Kredi Kartı").
                GroupBy(x => x.BankaAdi).
                Select(y => new {
                    Banka = y.Key,
                    Nur_Cerez = y.Where(
                        x => x.Odeme_Sube_Adi == "Nur Çerez (Simav)").Sum(a => (decimal?)a.Odeme_Tutari),
                    Nur_Peynir = y.Where(
                        x => x.Odeme_Sube_Adi == "Nur Peynir (Simav)").Sum(a => (decimal?)a.Odeme_Tutari),
                    Haci_Mumtaz_Simav = y.Where(
                        x => x.Odeme_Sube_Adi == "Hacı Mümtaz (Simav)").Sum(a => (decimal?)a.Odeme_Tutari),
                    Haci_Mumtaz_Kutahya = y.Where(
                        x => x.Odeme_Sube_Adi == "Hacı Mümtaz (Kütahya)").Sum(a => (decimal?)a.Odeme_Tutari),
                    Ortak_Harcama = y.Where(
                        x => x.Odeme_Sube_Adi == "Ortak Harcama" &&
                         x.Odeme_Kalemi != "yatırım").Sum(a => (decimal?)a.Odeme_Tutari),
                    Yatirim = y.Where(
                        x => x.Odeme_Sube_Adi == "Ortak Harcama" &&
                        x.Odeme_Kalemi == "yatırım").Sum(a => (decimal?)a.Odeme_Tutari),
                    Fatih = y.Where(
                        x => x.Odeme_Sube_Adi == "Fatih").Sum(a => (decimal?)a.Odeme_Tutari),
                    Muhlis = y.Where(
                        x => x.Odeme_Sube_Adi == "Muhlis").Sum(a => (decimal?)a.Odeme_Tutari),

                    Toplam_Odeme = y.Sum(a => a.Odeme_Tutari),
                    Hesap_Kesim_Tarihi = y.Select(x => x.IlkOdemeTarihi).FirstOrDefault(),
                    Son_Odeme_Tarihi = y.Select(x => x.SonOdemeTarihi).FirstOrDefault()

                }).OrderBy(x => x.Son_Odeme_Tarihi);
            dataGridView1.DataSource = AylikKartOdemeleri.ToList();

            //Çek Ödeme Tablosu
            var AylikCekOdemeleri = db.Odemelers.Where(
                x => x.IlkOdemeTarihi.Month == KullaniciSecimleri.Secilen_Ay &&
                x.IlkOdemeTarihi.Year == KullaniciSecimleri.Secilen_Yil &&
                x.Odeme_Sekli == "Çek").
                GroupBy(x => x.Odeme_ID).
                Select(y => new {
                    OdemeID = y.Key,
                    Nur_Cerez = y.Where(
                        x => x.Odeme_Sube_Adi == "Nur Çerez (Simav)").Sum(a => (decimal?)a.Odeme_Tutari),
                    Nur_Peynir = y.Where(
                        x => x.Odeme_Sube_Adi == "Nur Peynir (Simav)").Sum(a => (decimal?)a.Odeme_Tutari),
                    Haci_Mumtaz_Simav = y.Where(
                        x => x.Odeme_Sube_Adi == "Hacı Mümtaz (Simav)").Sum(a => (decimal?)a.Odeme_Tutari),
                    Haci_Mumtaz_Kutahya = y.Where(
                        x => x.Odeme_Sube_Adi == "Hacı Mümtaz (Kütahya)").Sum(a => (decimal?)a.Odeme_Tutari),
                    Ortak_Harcama = y.Where(
                        x => x.Odeme_Sube_Adi == "Ortak Harcama" &&
                         x.Odeme_Kalemi != "yatırım").Sum(a => (decimal?)a.Odeme_Tutari),
                    Yatirim = y.Where(
                        x => x.Odeme_Sube_Adi == "Ortak Harcama" &&
                        x.Odeme_Kalemi == "yatırım").Sum(a => (decimal?)a.Odeme_Tutari),
                    Fatih = y.Where(
                        x => x.Odeme_Sube_Adi == "Fatih").Sum(a => (decimal?)a.Odeme_Tutari),
                    Muhlis = y.Where(
                        x => x.Odeme_Sube_Adi == "Muhlis").Sum(a => (decimal?)a.Odeme_Tutari),
                    Toplam_Odeme = y.Sum(a => a.Odeme_Tutari),
                    Hesap_Kesim_Tarihi = y.Select(x => x.IlkOdemeTarihi).FirstOrDefault(),
                    Son_Odeme_Tarihi = y.Select(x => x.SonOdemeTarihi).FirstOrDefault()

                }).OrderBy(x=> x.Son_Odeme_Tarihi);
            dataGridView2.DataSource = AylikCekOdemeleri.ToList();

            //Sabit Gider Tablosu
            var AylikSabitGiderler = db.SabitGiderlers.Where(
                x => x.Sabit_Gider_Tutari != 0).
                GroupBy(x => x.Sabit_Gider_Kalemi).
                Select(y => new {
                    SabitGider = y.Key,
                    Nur_Cerez = y.Where(x => x.Sube.SubeAdi == "Nur Çerez (Simav)").Sum(a => (decimal?)a.Sabit_Gider_Tutari),                    
                    Nur_Peynir = y.Where(x => x.Sube.SubeAdi == "Nur Peynir (Simav)").Sum(a => (decimal?)a.Sabit_Gider_Tutari),                   
                    Haci_Mumtaz_Simav = y.Where(x => x.Sube.SubeAdi == "Hacı Mümtaz (Simav)").Sum(a => (decimal?)a.Sabit_Gider_Tutari),                    
                    Haci_Mumtaz_Kutahya = y.Where(x => x.Sube.SubeAdi == "Hacı Mümtaz (Kütahya)").Sum(a => (decimal?)a.Sabit_Gider_Tutari),                    
                    Toplam_Odeme = y.Sum(a => a.Sabit_Gider_Tutari),                   

                });
            dataGridView3.DataSource = AylikSabitGiderler.ToList();

            //Gelir Tablosu
            var Gelirler = db.Gelirlers.Where(
                x => x.Gelir_Odenecek_Tarih.Month == KullaniciSecimleri.Secilen_Ay &&
                x.Gelir_Odenecek_Tarih.Year == KullaniciSecimleri.Secilen_Yil).
                GroupBy(x => x.Odeme_Sekli).
                Select(y => new {
                    Banka = y.Key,
                    Nur_Cerez = y.Where(
                        x => x.Islem_Sube_Adi == "Nur Çerez (Simav)").Sum(a => (decimal?)a.Gelir_Tutari),
                    Nur_Peynir = y.Where(
                        x => x.Islem_Sube_Adi == "Nur Peynir (Simav)").Sum(a => (decimal?)a.Gelir_Tutari),
                    Haci_Mumtaz_Simav = y.Where(
                        x => x.Islem_Sube_Adi == "Hacı Mümtaz (Simav)").Sum(a => (decimal?)a.Gelir_Tutari),
                    Haci_Mumtaz_Kutahya = y.Where(
                        x => x.Islem_Sube_Adi == "Hacı Mümtaz (Kütahya)").Sum(a => (decimal?)a.Gelir_Tutari),                  

                    Toplam_Gelir = y.Sum(a => a.Gelir_Tutari),
                    //Islem_Tarihi = y.Select(x => x.Gelir_Islem_Tarihi).FirstOrDefault(),
                    //Odenecek_Tarih = y.Select(x => x.Gelir_Odenecek_Tarih).FirstOrDefault()

                })/*.OrderBy(x => x.Odenecek_Tarih)*/;
            dataGridView4.DataSource = Gelirler.ToList();

            //Toplam Ödeme Bilgileri
            decimal TopOdemeNurCerez = 0, TopOdemeNurPeynir= 0, TopOdemeHaciSimav = 0, TopOdemeHaciKutayha = 0;
            decimal TopOdemeOrtak = 0, TopOdemeYatirim = 0, TopOdemeFatih = 0, TopOdemeMuhlis = 0;
            decimal GenelToplamOdeme = 0;
            decimal NurPeynirGelir = 0, NurCerezGelir = 0, HaciSimavGelir = 0, HaciKutahyaGelir = 0;
            List<Gelirler> GelirListesi = db.Gelirlers.Where(
                x => x.Gelir_Odenecek_Tarih.Year == KullaniciSecimleri.Secilen_Yil &&
                x.Gelir_Odenecek_Tarih.Month == KullaniciSecimleri.Secilen_Ay).ToList();
            NurPeynirGelir = GelirListesi.Where(x => x.Islem_Sube_Adi == "Nur Peynir (Simav)").Sum(x => x.Gelir_Tutari);
            NurCerezGelir = GelirListesi.Where(x => x.Islem_Sube_Adi == "Nur Çerez (Simav)").Sum(x => x.Gelir_Tutari);
            HaciSimavGelir = GelirListesi.Where(x => x.Islem_Sube_Adi == "Hacı Mümtaz (Simav)").Sum(x => x.Gelir_Tutari);
            HaciKutahyaGelir = GelirListesi.Where(x => x.Islem_Sube_Adi == "Hacı Mümtaz (Kütahya)").Sum(x => x.Gelir_Tutari);

           
            //DataGridView Sütunlarının Toplamları
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                TopOdemeNurCerez += Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value);
                TopOdemeNurPeynir += Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value);
                TopOdemeHaciSimav += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
                TopOdemeHaciKutayha += Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value);
                TopOdemeOrtak += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
                TopOdemeYatirim += Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value);
                TopOdemeFatih += Convert.ToDecimal(dataGridView1.Rows[i].Cells[7].Value);
                TopOdemeMuhlis += Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value);

            }

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                TopOdemeNurCerez += Convert.ToDecimal(dataGridView2.Rows[i].Cells[1].Value);
                TopOdemeNurPeynir += Convert.ToDecimal(dataGridView2.Rows[i].Cells[2].Value);
                TopOdemeHaciSimav += Convert.ToDecimal(dataGridView2.Rows[i].Cells[3].Value);
                TopOdemeHaciKutayha += Convert.ToDecimal(dataGridView2.Rows[i].Cells[4].Value);
                TopOdemeOrtak += Convert.ToDecimal(dataGridView2.Rows[i].Cells[5].Value);
                TopOdemeYatirim += Convert.ToDecimal(dataGridView2.Rows[i].Cells[6].Value);
                TopOdemeFatih += Convert.ToDecimal(dataGridView2.Rows[i].Cells[7].Value);
                TopOdemeMuhlis += Convert.ToDecimal(dataGridView2.Rows[i].Cells[8].Value);
            }

            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                TopOdemeNurCerez += Convert.ToDecimal(dataGridView3.Rows[i].Cells[1].Value);
                TopOdemeNurPeynir += Convert.ToDecimal(dataGridView3.Rows[i].Cells[2].Value);
                TopOdemeHaciSimav += Convert.ToDecimal(dataGridView3.Rows[i].Cells[3].Value);
                TopOdemeHaciKutayha += Convert.ToDecimal(dataGridView3.Rows[i].Cells[4].Value);
            }

            //TextBox'ları Doldurma
            GenelToplamOdeme = TopOdemeHaciKutayha + TopOdemeHaciSimav + TopOdemeNurCerez + 
                TopOdemeNurPeynir + TopOdemeFatih + TopOdemeMuhlis + TopOdemeOrtak + TopOdemeYatirim;
            txtHaciKutahya.Text = TopOdemeHaciKutayha.ToString();
            txtHaciSimav.Text = TopOdemeHaciSimav.ToString();
            txtNurCerez.Text = TopOdemeNurCerez.ToString();
            txtNurPeynir.Text = TopOdemeNurPeynir.ToString();
            txtFatih.Text = TopOdemeFatih.ToString();
            txtMuhlis.Text = TopOdemeMuhlis.ToString();
            txtOrtak.Text = TopOdemeOrtak.ToString();
            txtYatirim.Text = TopOdemeYatirim.ToString();
            txtToplamOdeme.Text = GenelToplamOdeme.ToString();

            txtNurCerezGelir.Text = NurCerezGelir.ToString();
            txtNurPeynirGelir.Text = NurPeynirGelir.ToString();
            txtHaciSimavGelir.Text = HaciSimavGelir.ToString();
            txtHaciKutahyaGelir.Text = HaciKutahyaGelir.ToString();

            //Farklar
            decimal FarkHaciKutahya = (HaciKutahyaGelir - TopOdemeHaciKutayha);
            decimal FarkHaciSimav = (HaciSimavGelir - TopOdemeHaciSimav);
            decimal FarkNurCerez = (NurCerezGelir - TopOdemeNurCerez);
            decimal FarkNurPeynir = (NurPeynirGelir - TopOdemeNurPeynir);

            decimal ToplamGelir = NurPeynirGelir + NurCerezGelir + HaciSimavGelir + HaciKutahyaGelir;
            decimal FarklarToplami = FarkHaciKutahya + FarkHaciSimav + FarkNurCerez + FarkNurPeynir;

            txtFarkHaciKutahya.Text = FarkHaciKutahya.ToString();
            txtFarkHaciSimav.Text = FarkHaciSimav.ToString();
            txtFarkNurCerez.Text = FarkNurCerez.ToString();
            txtFarkNurPeynir.Text = FarkNurPeynir.ToString();

            txtToplamGelir.Text = ToplamGelir.ToString();
            txtGelirGiderFarki.Text = (ToplamGelir - GenelToplamOdeme).ToString();
        }      
    }
}
