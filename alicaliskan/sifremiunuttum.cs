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
using System.Net.Mail;

namespace alicaliskan
{
    public partial class sifremiunuttum : Form
    {
        public sifremiunuttum()
        {
            InitializeComponent();
        }

        Class.Context db = new Class.Context();
        Kullanici Kisi = new Kullanici();        
        public bool MailGonder(string konu, string icerik)
        {
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress("emailadresin@gmail.com");
            ePosta.To.Add(txtMail.Text); //göndereceğimiz mail adresi

            ePosta.Subject = konu; //mail konusu
            ePosta.Body = icerik; //mail içeriği 

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("emailadresin@gmail.com", "mail_sifren");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Send(ePosta);
            object userState = true;
            bool kontrol = true;
            try
            {
                client.SendAsync(ePosta, (object)ePosta);
            }
            catch (SmtpException ex)
            {
                kontrol = false;
                MessageBox.Show(ex.Message);
            }
            return kontrol;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Kisi = null;
                Kisi = db.Kullanicis.Where(
                    x => x.Mail == KullaniciSecimleri.Kullanici_Mail).FirstOrDefault();
                if (Kisi != null)
                {
                    lblHata.Visible = true;
                    lblHata.ForeColor = Color.Green;
                    lblHata.Text = "Girmiş Olduğunuz Bilgiler Uyuşuyor Şifreniz Mail Olarak Gönderildi";

                    progressBar1.Visible = true;
                    progressBar1.Maximum = 900000;
                    progressBar1.Minimum = 90;

                    for (int j = 90; j < 900000; j++)
                    {
                        progressBar1.Value = j;
                    }

                    MailGonder("ŞİFRE HATIRLATMA", "Şifreniz: " + Kisi.Sifre);
                }
                else
                {
                    lblHata.Visible = true;
                    lblHata.ForeColor = Color.Red;
                    lblHata.BackColor = Color.WhiteSmoke;
                    lblHata.Text = "Girmiş Olduğunuz Bilgiler Uyuşmuyor";

                }
            }
            catch (Exception)
            {
                lblHata.Visible = true;
                lblHata.BackColor = Color.WhiteSmoke;
                lblHata.ForeColor = Color.Red;
                lblHata.Text = "Mail Gönderme Hatası";
            }
            
        }

        private void txtMail_TextChanged(object sender, EventArgs e)
        {
            KullaniciSecimleri.Kullanici_Mail = txtMail.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login lgn = new Login();
            lgn.Show();
            this.Close();
        }
    }
}
