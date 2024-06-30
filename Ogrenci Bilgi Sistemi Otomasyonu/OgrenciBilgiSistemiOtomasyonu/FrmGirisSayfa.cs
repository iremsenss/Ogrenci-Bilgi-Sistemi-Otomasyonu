using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SirketOtomasyonu
{
    public partial class FrmGirisSayfa : Form
    {
        public FrmGirisSayfa()
        {
            InitializeComponent();
        }
        sqlbaglanti baglan = new sqlbaglanti();
        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text;
            string sifre = txtSifre.Text;

            // Veritabanı bağlantısı
            using (SqlConnection conn = baglan.baglanti())
            {
                

                
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM TBL_GİRİS WHERE KullaniciAd = @kullaniciAdi AND Sifre = @sifre", conn);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                cmd.Parameters.AddWithValue("@sifre", sifre);

                int kullaniciSayisi = (int)cmd.ExecuteScalar();

                if (kullaniciSayisi > 0)
                {
                    MessageBox.Show("Giriş başarılı!");
                    FrmAnasayfa anaForm = new FrmAnasayfa();
                    anaForm.FormClosed += (s, args) => this.Close(); 
                    anaForm.Show();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı!");
                }
                
            }
        }
    }
    
}
