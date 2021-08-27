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
    public partial class KrediKarti : Form
    {
        Class.Context db = new Class.Context();
        
        public KrediKarti()
        {
            InitializeComponent();
        }

        private void KrediKarti_Load(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            var Subeler = db.Subelers.ToList();
            cbSube.DataSource = Subeler;
            cbSube.DisplayMember = "SubeAdi";
            cbSube.ValueMember = "SubeAdi";

            var Kartlar = db.KrediKartlaris.ToList();
            cbBankaAdi.DataSource = Kartlar;
            cbBankaAdi.DisplayMember = "BankaAdi";
            cbBankaAdi.ValueMember = "BankaAdi";
            VeritabaniniGoruntule();
        }

        void EskiKayitlariSil()
        {
            int SilinecekYil = (DateTime.Now.Year) - 2;

            List<Giderler> EskiGiderler = db.Giderlers.Where(
                x => x.Islem_Tarihi.Year <= SilinecekYil).ToList();
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
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            EskiKayitlariSil();
            dataGridView1.Visible = true;
            BosKontrol();
            if (KullaniciSecimleri.Secilen_ID == 0) // Kayıtlardan herhangi biri seçilmemişse yeni kayıt oluştur
            {
                if (txtIslemTutari.Text != "")
                {
                    Giderler YeniGider = new Giderler();
                    Odemeler YeniOdeme = new Odemeler();
                    //Ortak Bilgiler Girilmesi Zorunlu
                    YeniGider.Islem_Sube_Adi = cbSube.SelectedValue.ToString();
                    YeniGider.Islem_Tarihi = dateTimePicker1.Value.Date;
                    YeniGider.Islem_Tutari = Convert.ToDecimal(txtIslemTutari.Text);
                    YeniGider.Odeme_Sekli = cbOdemeSekli.SelectedItem.ToString();

                    //Ortak Bilgiler girilmesi Zorunlu Değil
                    YeniGider.CalisilanFirma = txtCalisilanFirma.Text;
                    YeniGider.Islem_Aciklama = txtAciklama.Text;
                    YeniGider.Islem_Kalemi = txtGiderKalemi.Text;

                    if (cbOdemeSekli.SelectedIndex == 1)
                        YeniGider.BankaAdi = cbBankaAdi.Text;

                    if (txtTaksitSayisi.Text != "")//Taksit Sayısı
                        YeniGider.Islem_Taksit_Saysi = Convert.ToByte(txtTaksitSayisi.Text);
                    else
                        YeniGider.Islem_Taksit_Saysi = 1;
                   

                    db.Giderlers.Add(YeniGider);
                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                    {
                        MessageBox.Show("Kayıt eklendi.");
                        //Taksit Sayısına Göre Ödeme Planı Oluşturuluyor
                        for (int i = 0; i < YeniGider.Islem_Taksit_Saysi; i++)
                        {
                            YeniOdeme.CalisilanFirma = YeniGider.CalisilanFirma;
                            YeniOdeme.Odeme_Sube_Adi = YeniGider.Islem_Sube_Adi;
                            YeniOdeme.Odeme_Aciklama = YeniGider.Islem_Aciklama;
                            YeniOdeme.Odeme_Kalemi = YeniGider.Islem_Kalemi;
                            YeniOdeme.Odeme_Sekli = YeniGider.Odeme_Sekli;

                            YeniOdeme.Odeme_Tutari = YeniGider.Islem_Tutari / YeniGider.Islem_Taksit_Saysi;

                            //Ödeme Tarihlerini Oluşturuyorum
                            if (cbOdemeSekli.SelectedIndex == 2)//Çek Ödemesi
                            {
                                YeniOdeme.IlkOdemeTarihi = dateTimePicker2.Value.Date;
                                YeniOdeme.SonOdemeTarihi = dateTimePicker2.Value.Date;
                                YeniOdeme.Taksit_Durumu = "1. Taksit";
                                YeniGider.Islem_Taksit_Saysi = 1;
                            }
                            else if (cbOdemeSekli.SelectedIndex == 1)//Kredi Kartı ile ödeme
                            {
                                int Ay;
                                int HesapKesimGun = db.KrediKartlaris.Where(
                                      x => x.BankaAdi == YeniGider.BankaAdi).FirstOrDefault().Hesap_Kesim_Gunu;
                                int Yil = YeniGider.Islem_Tarihi.Year;
                                if (YeniGider.Odeme_Sekli == "Kredi Kartı" && HesapKesimGun > YeniGider.Islem_Tarihi.Day)
                                    Ay = YeniGider.Islem_Tarihi.Month;
                                else
                                    Ay = YeniGider.Islem_Tarihi.Month + 1;
                                //Son ödeme günü diğer aya sarkıyorsa ay ve yıl değişikliklerini yapıyorum
                                int SonOdemeGun = db.KrediKartlaris.Where(
                                    x => x.BankaAdi == YeniGider.BankaAdi).FirstOrDefault().Odeme_Gunu;
                                int SonOdemeAyFark, SonOdemeYilFark;

                                if (SonOdemeGun < HesapKesimGun)
                                {
                                    SonOdemeAyFark = 1;
                                    if (Ay == 12)
                                        SonOdemeYilFark = 1;
                                    else
                                        SonOdemeYilFark = 0;
                                }
                                else
                                {
                                    SonOdemeAyFark = 0;
                                    SonOdemeYilFark = 0;
                                }
                                //YeniOdeme.Odeme_Tarihi = OdemeTarihi.AddMonths(i).Date;
                                //YeniOdeme.Son_Odeme_Tarihi = SonOdemeTarihi.AddMonths(i).Date;
                                DateTime OdemeTarihi = new DateTime(Yil, Ay, HesapKesimGun);
                                DateTime SonOdemeTarihi = new DateTime(Yil + SonOdemeYilFark, Ay + SonOdemeAyFark, SonOdemeGun);
                                YeniOdeme.BankaAdi = YeniGider.BankaAdi;

                                YeniOdeme.Taksit_Durumu = (i + 1).ToString() + "/" + YeniGider.Islem_Taksit_Saysi.ToString() + ". Taksit";
                                YeniOdeme.IlkOdemeTarihi = OdemeTarihi.AddMonths(i).Date;
                                YeniOdeme.SonOdemeTarihi = SonOdemeTarihi.AddMonths(i).Date;
                                if (txtTaksitSayisi.Text == "")
                                    YeniGider.Islem_Taksit_Saysi = 1;
                            }
                            else //Nakit Ödeme
                            {
                                YeniOdeme.IlkOdemeTarihi = dateTimePicker1.Value.Date;
                                YeniOdeme.SonOdemeTarihi = dateTimePicker1.Value.Date;
                                YeniOdeme.Taksit_Durumu = "1. Taksit";
                                YeniGider.Islem_Taksit_Saysi = 1;
                            }
                            db.Odemelers.Add(YeniOdeme);
                            db.SaveChanges();
                        }
                        BoxTemizle();
                    }
                    else
                    {
                        MessageBox.Show("Kayıt eklenemedi.");
                    }
                    VeritabaniniGoruntule();
                }
               
            }
            else
            {
                MessageBox.Show("Zaten Kayıtlı");
                return;
            }

        }
        void VeritabaniniGoruntule()
        {
            dataGridView1.Visible = true;
           
            var GiderListesi = db.Giderlers.ToList();
            dataGridView1.DataSource = GiderListesi;
        }
        void BoxTemizle()
        {
            txtCalisilanFirma.Text = "";
            txtIslemTutari.Text = "";
            txtTaksitSayisi.Text = "";
            txtGiderKalemi.Text = "";
            txtAciklama.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            KullaniciSecimleri.Secilen_ID = 0;

        }
        void BosKontrol()
        {
            if (txtIslemTutari.Text == "")
            { MessageBox.Show("İşlem tutarı boş geçilemez"); return; }

        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            BoxTemizle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Raporlar rprlr = new Raporlar();
            rprlr.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Datagridview üzerinden bir hücre seçildiyse box'ları doldur
            
            int secilialan = dataGridView1.SelectedCells[0].RowIndex;
            string ID = dataGridView1.Rows[secilialan].Cells[0].Value.ToString();
            string IslemTarihi = dataGridView1.Rows[secilialan].Cells[1].Value.ToString();
           
            string IslemTutari = dataGridView1.Rows[secilialan].Cells[2].Value.ToString();
            string IslemKalemi = dataGridView1.Rows[secilialan].Cells[3].Value.ToString();
            string TaksitSayisi = dataGridView1.Rows[secilialan].Cells[4].Value.ToString();
            string SubeAdi = dataGridView1.Rows[secilialan].Cells[5].Value.ToString();
            string Aciklama = dataGridView1.Rows[secilialan].Cells[6].Value.ToString();
            string CalisilanFirma = dataGridView1.Rows[secilialan].Cells[7].Value.ToString();
            if(dataGridView1.Rows[secilialan].Cells[8].Value != null)
            {
                string BankaAdi = dataGridView1.Rows[secilialan].Cells[8].Value.ToString();
                cbBankaAdi.Text = BankaAdi;
            }                        
            string OdemeSekli = dataGridView1.Rows[secilialan].Cells[9].Value.ToString();

            KullaniciSecimleri.Secilen_ID = int.Parse(ID);
            cbSube.Text = SubeAdi;
            dateTimePicker1.Text = IslemTarihi;
            txtIslemTutari.Text = IslemTutari;
            cbOdemeSekli.Text = OdemeSekli;
            txtCalisilanFirma.Text = CalisilanFirma;
            txtGiderKalemi.Text = IslemKalemi;
            txtAciklama.Text = Aciklama;
            
            txtTaksitSayisi.Text = TaksitSayisi;
        }

        private void txtIslemTutari_KeyPress(object sender, KeyPressEventArgs e)
        {
            //İşlem tutarı için sadece rakam ve virgüle izin ver
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',';
        }

        private void txtTaksitSayisi_KeyPress(object sender, KeyPressEventArgs e)
        {   //Taksit sayısı için sadece rakam ve virgüle izin ver
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',';
        }

        private void cbOdemeSekli_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOdemeSekli.SelectedIndex == 0) //Nakit
            {
                cbBankaAdi.Visible = false;
                label2.Visible = false;

                label5.Visible = false;
                txtTaksitSayisi.Visible = false;


                label9.Visible = false;
                dateTimePicker2.Visible = false;
            }
            else if (cbOdemeSekli.SelectedIndex == 1)//Kredi kartı
            {
                cbBankaAdi.Visible = true;
                label2.Visible = true;

                label5.Visible = true;
                txtTaksitSayisi.Visible = true;

                label9.Visible = false;
                dateTimePicker2.Visible = false;
            }
            else if (cbOdemeSekli.SelectedIndex == 2)//Çek
            {
                cbBankaAdi.Visible = false;
                label2.Visible = false;

                label5.Visible = false;
                txtTaksitSayisi.Visible = false;

                label9.Visible = true;
                dateTimePicker2.Visible = true;

                label9.Location = new Point(16, 448);
                dateTimePicker2.Location = new Point(11, 467);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GelirEkrani ge = new GelirEkrani();
            ge.Show();
            this.Hide();
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
