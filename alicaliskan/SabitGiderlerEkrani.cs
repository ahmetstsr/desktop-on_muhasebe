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
    public partial class SabitGiderlerEkrani : Form
    {
        public SabitGiderlerEkrani()
        {
            InitializeComponent();
        }
        Class.Context db = new Class.Context();

        private void SabitGiderlerEkrani_Load(object sender, EventArgs e)
        {
            //List<SabitGiderler> SGiderler = db.SabitGiderlers.ToList();

            VeritabaniniGoruntule();
            //dataGridView1.DataSource = SGiderler;

            var Subeler = db.Subelers.ToList();
            cbSubeAdi.DataSource = Subeler;
            cbSubeAdi.DisplayMember = "SubeAdi";
            cbSubeAdi.ValueMember = "SubeAdi";

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilialan = dataGridView1.SelectedCells[0].RowIndex;
            string ID = dataGridView1.Rows[secilialan].Cells[0].Value.ToString();
            string SabitGiderTutari = dataGridView1.Rows[secilialan].Cells[1].Value.ToString();
            string SabitGiderKalemi = dataGridView1.Rows[secilialan].Cells[2].Value.ToString();
            string SabitGiderAciklama = dataGridView1.Rows[secilialan].Cells[3].Value.ToString();
            string SabitGiderSubeAdi = dataGridView1.Rows[secilialan].Cells[4].Value.ToString();

            cbSubeAdi.Text = SabitGiderSubeAdi;
            txtAciklama.Text = SabitGiderAciklama;
            txtGiderKalemi.Text = SabitGiderKalemi;
            txtTutar.Text = SabitGiderTutari;
            txtID.Text = ID;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Sabit Gider Bilgi Güncelle
            if (txtID.Text == "")
            { MessageBox.Show("Güncellenecek bir kayıt seçmen gerekir"); return; }
            else
            {
                int SecilenID = int.Parse(txtID.Text);
                SabitGiderler YeniSabitGider = db.SabitGiderlers.Where(
                    x => x.Sabit_Gider_ID == SecilenID).FirstOrDefault();
                string secilenSube = cbSubeAdi.SelectedValue.ToString();
                Subeler YeniSube = db.Subelers.Where(x => x.SubeAdi == secilenSube).FirstOrDefault();
                YeniSabitGider.Sabit_Gider_Aciklama = txtAciklama.Text;
                YeniSabitGider.Sabit_Gider_Kalemi = txtGiderKalemi.Text;
                YeniSabitGider.Sabit_Gider_Tutari = Convert.ToDecimal(txtTutar.Text);
                YeniSabitGider.Sube = YeniSube;
                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                    MessageBox.Show("Günceleme Başarılı");
                else
                    MessageBox.Show("Günceleme Başarısız");
                Temizle();
                VeritabaniniGoruntule();

            }


        }

        void VeritabaniniGoruntule()
        {
            var SabitGiderListesi = db.SabitGiderlers.
               Select(y => new
               {
                   ID = y.Sabit_Gider_ID,
                   GiderTutari = y.Sabit_Gider_Tutari,
                   GiderKalemi = y.Sabit_Gider_Kalemi,
                   Aciklama = y.Sabit_Gider_Aciklama,
                   GiderSube = y.Sube.SubeAdi,
               }).OrderBy(x => x.GiderSube);

            dataGridView1.DataSource = SabitGiderListesi.ToList();

            //Şubelere Göre Kredi Kartı Ödemeleri
            var SubeSabitGiderToplamlari = db.SabitGiderlers.
                GroupBy(x => x.Sube.SubeAdi).
                Select(y => new
                {
                    SubeAdi = y.Key,
                    ToplamSabitGider = y.Sum(a => (decimal?)a.Sabit_Gider_Tutari),

                });
            dataGridView2.DataSource = SubeSabitGiderToplamlari.ToList();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Yeni Sabit Gider Ekle
            if (txtID.Text == "")
            {
                if (txtGiderKalemi.Text == "")
                { MessageBox.Show("Gider Kalemi Boş Bırakılamaz"); return; }
                else if (txtTutar.Text == "")
                { MessageBox.Show("Tutar ksımı boş bırakılamaz"); return; }
                else if (cbSubeAdi.Text == "")
                { MessageBox.Show("Şube seçimi boş bırakılamaz"); return; }
                else
                {
                    SabitGiderler sg = new SabitGiderler();
                    string secilenSube = cbSubeAdi.SelectedValue.ToString();
                    Subeler YeniSube = db.Subelers.Where(x => x.SubeAdi == secilenSube).FirstOrDefault();

                    sg.Sabit_Gider_Aciklama = txtAciklama.Text;
                    sg.Sabit_Gider_Kalemi = txtGiderKalemi.Text;
                    sg.Sabit_Gider_Tutari = Convert.ToDecimal(txtTutar.Text);
                    sg.Sube = YeniSube;

                    db.SabitGiderlers.Add(sg);
                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                        MessageBox.Show("Yeni Kayıt Başarılı");
                    else
                        MessageBox.Show("Yeni Kayıt Başarısız");
                    Temizle();
                    VeritabaniniGoruntule();
                }

            }
            else
            {
                MessageBox.Show("Bu bilgiler zaten kayıtlı");
                return;
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Sabit Gider Sil
            if (txtID.Text == "")
            { MessageBox.Show("Silinecek bir kayıt seçmen gerekir"); return; }
            else
            {
                int SecilenID = int.Parse(txtID.Text);
                SabitGiderler YeniSabitGider = db.SabitGiderlers.Where(
                    x => x.Sabit_Gider_ID == SecilenID).FirstOrDefault();
                db.SabitGiderlers.Remove(YeniSabitGider);

                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                    MessageBox.Show("Silme İşlemi Başarılı");
                else
                    MessageBox.Show("Silme İşlemi Başarısız");
                Temizle();
                VeritabaniniGoruntule();
            }

        }

        private void txtTutar_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',';
        }

        private void button1_Click(object sender, EventArgs e)
        {// Alanları Temizle
            Temizle();

        }
        void Temizle()
        {
            txtAciklama.Text = "";
            txtGiderKalemi.Text = "";
            txtID.Text = "";
            txtTutar.Text = "";
            cbSubeAdi.Text = "";
        }
    }
}
