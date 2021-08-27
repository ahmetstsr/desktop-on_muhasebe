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
    public partial class SubelerEkrani : Form
    {
        public SubelerEkrani()
        {
            InitializeComponent();
        }
        Class.Context db = new Class.Context(); 
        private void SubelerEkrani_Load(object sender, EventArgs e)
        {
            VeritabaniniGoruntule();
        }

        void VeritabaniniGoruntule()
        {
            List<Subeler> SubeListesi = db.Subelers.ToList();
            dataGridView1.DataSource = SubeListesi;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilialan = dataGridView1.SelectedCells[0].RowIndex;
            string ID = dataGridView1.Rows[secilialan].Cells[0].Value.ToString();
            string SubeAdi = dataGridView1.Rows[secilialan].Cells[1].Value.ToString();           

            txtSubeAdi.Text = SubeAdi;            
            txtID.Text = ID;
        }

        void Temizle()
        {
            txtID.Text = "";
            txtSubeAdi.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Temizle
            Temizle();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Güncelle
            if (txtID.Text == "")
                MessageBox.Show("Bir Şube Seçin");
            else
            {
                int secilenID = int.Parse(txtID.Text);
                Subeler SecilenSube = db.Subelers.Where(x => x.SubeID == secilenID).FirstOrDefault();

                SecilenSube.SubeAdi = txtSubeAdi.Text;
                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                    MessageBox.Show("Günceleme Başarılı");
                else
                    MessageBox.Show("Günceleme Başarısız");
                VeritabaniniGoruntule();
                Temizle();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {       //Yeni Şube Ekle
            if (txtSubeAdi.Text == "")
                MessageBox.Show("Bir Şube Adı Girin");
            else
            {
                string GirilenSubeAdi = txtSubeAdi.Text;
                List<Subeler> KayitliSubeler = db.Subelers.ToList();
                int Deger = 0;
                foreach (var item in KayitliSubeler)
                {
                    if (item.SubeAdi == GirilenSubeAdi)
                        Deger++;
                }
                if(Deger == 0)
                {
                    Subeler yeniSube = new Subeler();
                    yeniSube.SubeAdi = GirilenSubeAdi;
                    db.Subelers.Add(yeniSube);
                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                        MessageBox.Show(txtSubeAdi.Text + " Şubesi Eklendi");
                    else
                        MessageBox.Show("Ekleme İşlemi Başarısız");
                    VeritabaniniGoruntule();
                    Temizle();
                }
                else
                {
                    MessageBox.Show("\""+GirilenSubeAdi +"\""+ " adlı şube zaten kayıtlı");
                    return;
                }
                
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
                MessageBox.Show("Silmek için bir şube seçin");
            else
            {
                int secilenID = int.Parse(txtID.Text);
                Subeler SilinecekSube = db.Subelers.Where(x => x.SubeID == secilenID).FirstOrDefault();
                db.Subelers.Remove(SilinecekSube);
                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                    MessageBox.Show(txtSubeAdi.Text + " Şubesi Silindi");
                else
                    MessageBox.Show("Silme İşlemi Başarısız");
                VeritabaniniGoruntule();
                Temizle();
            }
        }
    }
}
