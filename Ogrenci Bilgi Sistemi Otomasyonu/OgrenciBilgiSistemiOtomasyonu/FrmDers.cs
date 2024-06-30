using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace SirketOtomasyonu
{
    public partial class FrmDersler : Form
    {
        public FrmDersler()
        {
            InitializeComponent();
        }
        sqlbaglanti baglan = new sqlbaglanti();

        void listele()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter("select * from TBL_DERS ", baglan.baglanti());
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

        }

        void BolumleriYukle()
        {
            SqlCommand cmd = new SqlCommand("SELECT BID, BADI FROM TBL_BOLUM", baglan.baglanti());
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cmbBolum.Items.Add(new { Text = dr["BADI"].ToString(), Value = dr["BID"] });
            }
            baglan.baglanti().Close();

            cmbBolum.DisplayMember = "Text";
            cmbBolum.ValueMember = "Value";
        }


        private void bttnEkle_Click(object sender, EventArgs e)
        {
            listele();
            if ((string.IsNullOrEmpty(textBox1.Text))  || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                

                SqlCommand komut = new SqlCommand("insert into TBL_DERS (DADI, DKODU ,DKREDI ,BID) values ('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','"+ (cmbBolum.SelectedItem as dynamic).Value + "')", baglan.baglanti());
                int basari = komut.ExecuteNonQuery();
                baglan.baglanti().Close();

                if (basari == 1)
                    MessageBox.Show("DERS EKLENDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("DERS EKLENEMEDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);


                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
    

            }
        }

        private void bttnSil_Click(object sender, EventArgs e)
        {
            listele();

            int hata = 0;
            if (textBox0.Text == string.Empty)
                hata = 1;
            if (hata == 1)
                MessageBox.Show("ALANI DOLDURUNUZ", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                SqlCommand komut = new SqlCommand("select * from TBL_DERS  where  DID='" + textBox0.Text + "'", baglan.baglanti());
                komut.ExecuteNonQuery();
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    SqlCommand cm = new SqlCommand("delete from TBL_DERS where  DID = '" + textBox0.Text + "'", baglan.baglanti());
                    int basari = cm.ExecuteNonQuery();
                    baglan.baglanti().Close();
                    if (basari == 1)
                        MessageBox.Show("DERS SİLİNDİ", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("DERS SİLİNMEDİ ", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("BÖYLE BİR DERS BULUNAMADI", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
    }

        private void bttnGuncelle_Click(object sender, EventArgs e)   
        {
            listele();
            if (string.IsNullOrEmpty(textBox0.Text) || string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || cmbBolum.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int bolumId = (cmbBolum.SelectedItem as dynamic).Value;
                SqlCommand cm = new SqlCommand("UPDATE TBL_DERS SET DADI=@dadi, DKODU=@dkodu, DKREDI=@dkredi, BID=@bolumid WHERE DID=@did", baglan.baglanti());
                cm.Parameters.AddWithValue("@dadi", textBox1.Text);
                cm.Parameters.AddWithValue("@dkodu", textBox2.Text);
                cm.Parameters.AddWithValue("@dkredi", textBox3.Text);
                cm.Parameters.AddWithValue("@bolumid", bolumId);
                cm.Parameters.AddWithValue("@did", textBox0.Text);

                int basari = cm.ExecuteNonQuery();
                baglan.baglanti().Close();

                if (basari == 1)
                    MessageBox.Show("KAYIT GÜNCELLENDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("KAYIT GÜNCELLENEMEDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox0.Clear();
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                cmbBolum.SelectedIndex = -1;
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox0.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text= dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void FrmDersler_Load(object sender, EventArgs e)
        {
            listele();
            BolumleriYukle();
        }
    }
}

