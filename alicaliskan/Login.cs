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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        Class.Context db = new Class.Context();
        Kullanici Kisi = new Kullanici();
        private void button1_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                Properties.Settings.Default["KullaniciAdi"] = txtKullaniciAdi.Text;
                Properties.Settings.Default["Sifre"] = txtSifre.Text;
            }
            else
            {
                Properties.Settings.Default["KullaniciAdi"] = "";
                Properties.Settings.Default["Sifre"] = "";
            }
            Properties.Settings.Default.Save();
            Kisi = null;
            
            if(txtKullaniciAdi.Text != "")
            {
                Kisi = db.Kullanicis.Where(
                x => x.KullaniciAdi == KullaniciSecimleri.Kullanici_Adi).FirstOrDefault();

                if(Kisi != null)
                {
                    if(KullaniciSecimleri.Kullanici_Sifre == Kisi.Sifre)
                    {
                        KrediKarti anaekran = new KrediKarti();
                        anaekran.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Girilen Şifre yanlış");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı Adı Kayıtlı Değil");
                    return;
                }
            }          

        }

        private void Login_Load(object sender, EventArgs e)
        {
            db.Database.CreateIfNotExists();
            txtKullaniciAdi.Text = Properties.Settings.Default["KullaniciAdi"].ToString();
            txtSifre.Text = Properties.Settings.Default["Sifre"].ToString();

            if (txtKullaniciAdi.Text.Count() > 1)
            {
                checkBox1.Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                Properties.Settings.Default["KullaniciAdi"] = txtKullaniciAdi.Text;
                Properties.Settings.Default["Sifre"] = txtSifre.Text;
            }
            else
            {
                Properties.Settings.Default["KullaniciAdi"] = "";
                Properties.Settings.Default["Sifre"] = "";
            }
            Properties.Settings.Default.Save();
            Kisi = null;

            if (txtKullaniciAdi.Text != "")
            {
                Kisi = db.Kullanicis.Where(
                x => x.KullaniciAdi == KullaniciSecimleri.Kullanici_Adi).FirstOrDefault();

                if (Kisi != null)
                {
                    if (KullaniciSecimleri.Kullanici_Sifre == Kisi.Sifre)
                    {
                        GelirEkrani ge = new GelirEkrani();
                        ge.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Girilen Şifre yanlış");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı Adı Kayıtlı Değil");
                    return;
                }
            }


            
        }

        private void txtKullaniciAdi_TextChanged(object sender, EventArgs e)
        {
            KullaniciSecimleri.Kullanici_Adi = txtKullaniciAdi.Text;
        }

        private void txtSifre_TextChanged(object sender, EventArgs e)
        {
            KullaniciSecimleri.Kullanici_Sifre = txtSifre.Text;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            sifremiunuttum frm = new sifremiunuttum();
            frm.Show();
        }
    }
}
