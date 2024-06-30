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
    public partial class FrmBolum : Form
    {
        public FrmBolum()
        {
            InitializeComponent();
        }

        sqlbaglanti baglan = new sqlbaglanti();
        void listele()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter("select * from TBL_BOLUM ", baglan.baglanti());
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

        }

        private void bttnEkle_Click(object sender, EventArgs e)
        {

            listele();
            if ((string.IsNullOrEmpty(textBox1.Text)) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                SqlCommand komut = new SqlCommand("insert into TBL_BOLUM (BADI, BACIKLAMA ,BPOSTA) values ('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "')", baglan.baglanti());
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
                SqlCommand komut = new SqlCommand("select * from TBL_BOLUM  where  BID='" + textBox0.Text + "'", baglan.baglanti());
                komut.ExecuteNonQuery();
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    SqlCommand cm = new SqlCommand("delete from TBL_BOLUM where  BID = '" + textBox0.Text + "'", baglan.baglanti());
                    int basari = cm.ExecuteNonQuery();
                    baglan.baglanti().Close();
                    if (basari == 1)
                        MessageBox.Show("BOLUM SİLİNDİ", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("BOLUM SİLİNMEDİ ", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("BÖYLE BİR BOLUM BULUNAMADI", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
        }

        private void bttnGuncelle_Click(object sender, EventArgs e)
        {
            listele();

            SqlCommand cm = new SqlCommand("UPDATE TBL_BOLUM SET BADI=@p1, BACIKLAMA=@p2, BPOSTA=@p3 WHERE BID=@p4", baglan.baglanti());

            cm.Parameters.AddWithValue("p1", textBox1.Text);
            cm.Parameters.AddWithValue("p2", textBox2.Text);
            cm.Parameters.AddWithValue("p3", textBox3.Text);
            cm.Parameters.AddWithValue("@p4", textBox0.Text);

            int basari = cm.ExecuteNonQuery();
            baglan.baglanti().Close();

            if (basari == 1)
                MessageBox.Show("KAYIT GÜNCELLENDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("KAYIT GÜNCELLENEMEDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox0.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void FRM_BOLUM_Load(object sender, EventArgs e)
        {
            listele();
        }
    }
}
