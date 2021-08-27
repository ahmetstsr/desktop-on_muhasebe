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
    public partial class KrediKartlariEkrani : Form
    {
        public KrediKartlariEkrani()
        {
            InitializeComponent();
        }
        Class.Context db = new Class.Context();

        private void KrediKartlariEkrani_Load(object sender, EventArgs e)
        {
            VeritabaniniGoruntule();
            
        }


        void VeritabaniniGoruntule()
        {
            var KrediKartlari = db.KrediKartlaris.ToList();

            dataGridView1.DataSource = KrediKartlari.ToList();
            Temizle();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilialan = dataGridView1.SelectedCells[0].RowIndex;
            string ID = dataGridView1.Rows[secilialan].Cells[0].Value.ToString();
            string BankaAdi = dataGridView1.Rows[secilialan].Cells[1].Value.ToString();
            string HesapKesim = dataGridView1.Rows[secilialan].Cells[2].Value.ToString();
            string SonOdeme = dataGridView1.Rows[secilialan].Cells[3].Value.ToString();            

            txtBankaAdi.Text = BankaAdi;            
            txtHesapKesimTarihi.Text = HesapKesim;
            txtSonOdemeTarihi.Text = SonOdeme;
            txtID.Text = ID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //KrediKarti Kartı Bilgi Güncelle
            if (txtID.Text == "")
            { MessageBox.Show("Güncellenecek bir kayıt seçmen gerekir"); return; }
            else
            {
                int SecilenID = int.Parse(txtID.Text);
                KrediKartlari YeniKrediKarti = db.KrediKartlaris.Where(
                    x => x.KartID == SecilenID).FirstOrDefault();

                YeniKrediKarti.BankaAdi = txtBankaAdi.Text;
                YeniKrediKarti.Hesap_Kesim_Gunu = Convert.ToByte(txtHesapKesimTarihi.Text);
                YeniKrediKarti.Odeme_Gunu = Convert.ToByte(txtSonOdemeTarihi.Text);

                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                    MessageBox.Show("Günceleme Başarılı");
                else
                    MessageBox.Show("Günceleme Başarısız");
                VeritabaniniGoruntule();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Yeni Kredi Kartı Ekle
            if(txtBankaAdi.Text != "" && txtHesapKesimTarihi.Text != "" && txtSonOdemeTarihi.Text != "")
            {
                KrediKartlari YeniKrediKarti = new KrediKartlari();

                YeniKrediKarti.BankaAdi = txtBankaAdi.Text;
                YeniKrediKarti.Hesap_Kesim_Gunu = Convert.ToByte(txtHesapKesimTarihi.Text);
                YeniKrediKarti.Odeme_Gunu = Convert.ToByte(txtSonOdemeTarihi.Text);

                db.KrediKartlaris.Add(YeniKrediKarti);

                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                    MessageBox.Show("Kredi Kartı Eklendi");
                else
                    MessageBox.Show("Kredi Kartı Eklenemedi");
                VeritabaniniGoruntule();
            }
            else
            {
                MessageBox.Show("Tüm alanları doldurman gerekir");
                return;
            }
           

        }

        void Temizle()
        {
            txtID.Text = "";
            txtBankaAdi.Text = "";
            txtHesapKesimTarihi.Text = "";
            txtSonOdemeTarihi.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Kredi Karti Silme
            if(txtID.Text != "")
            {
                int SecilenID = int.Parse(txtID.Text);
                KrediKartlari YeniKrediKarti = db.KrediKartlaris.Where(
                    x => x.KartID == SecilenID).FirstOrDefault();

                db.KrediKartlaris.Remove(YeniKrediKarti);

                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                    MessageBox.Show("Kredi Kartı Silindi");
                else
                    MessageBox.Show("Kredi Kartı Silinemedi");
                VeritabaniniGoruntule();
            }
           else
            {
                MessageBox.Show("Silinecek bir kayıt seçmen gerekir"); return;
            }
        }
        
        private void txtHesapKesimTarihi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtSonOdemeTarihi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
