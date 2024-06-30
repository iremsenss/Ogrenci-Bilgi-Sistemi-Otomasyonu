using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;

namespace SirketOtomasyonu
{
    public partial class Öğrenciler : Form
    {
        public Öğrenciler()
        {
            InitializeComponent();
        }

        sqlbaglanti baglan = new sqlbaglanti();

        void listele()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter("select * from TBL_OGRENCILER ", baglan.baglanti());
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];

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



        private void Ogrenciler_Load(object sender, EventArgs e)
        {
            listele();
            ilekle();
        }

        void ilekle()
        {
            SqlCommand cmd = new SqlCommand("Select *from TBL_ILLER", baglan.baglanti());
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cmBoxil.Items.Add(dr[1].ToString());
            }
            baglan.baglanti().Close();

        }
        private void cmBoxil_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmBoxilce.Items.Clear();
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM TBL_ILCELER WHERE sehir=@p1", baglan.baglanti()))
            {
                cmd.Parameters.AddWithValue("@p1", cmBoxil.SelectedIndex + 1);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        cmBoxilce.Items.Add(dr[1].ToString());
                    }
                }
            }
        }



        private void bttnEkle_Click_1(object sender, EventArgs e)
        {
            listele();
            if (string.IsNullOrEmpty(textAd.Text) || string.IsNullOrEmpty(textSoyadı.Text) || string.IsNullOrEmpty(maskedTextTC.Text) || string.IsNullOrEmpty(dateTimePicker1.Text) || string.IsNullOrEmpty(cmBoxil.Text) || string.IsNullOrEmpty(cmBoxilce.Text) || string.IsNullOrEmpty(richTextBoxAdres.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                SqlCommand komut = new SqlCommand("insert into TBL_OGRENCILER (OGRAD,OGRSOYAD,OGRTC,OGRIL,OGRILCE,OGRADRES,BOLUMID) values ('" + textAd.Text + "','" + textSoyadı.Text + "','" + maskedTextTC.Text + "','" + cmBoxil.Text + "','" + cmBoxilce.Text + "','" + richTextBoxAdres.Text + "' , '"+ (cmbBolum.SelectedItem as dynamic).Value + "')", baglan.baglanti());
                int basari = komut.ExecuteNonQuery();
                baglan.baglanti().Close();

                if (basari == 1)
                    MessageBox.Show("Ogrenci EKLENDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Ogrenci EKLENEMEDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);


                textAd.Clear();
                textSoyadı.Clear();
                maskedTextTC.Clear();
                //   dateTimePicker1.Clear();
                cmBoxil.SelectedIndex = -1;
                cmBoxilce.SelectedIndex = -1;
                richTextBoxAdres.Clear();


            }

        }

        private void bttnSil_Click_1(object sender, EventArgs e)
        {
            listele();

            if (string.IsNullOrEmpty(textID.Text))
            {
                MessageBox.Show("ALANI DOLDURUNUZ", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlCommand komut = new SqlCommand("SELECT * FROM TBL_OGRENCILER WHERE OGRID=@p1", baglan.baglanti()))
            {
                komut.Parameters.AddWithValue("@p1", textID.Text);
                SqlDataReader dr = komut.ExecuteReader();

                if (dr.Read())
                {
                    dr.Close(); // Okumayı kapat

                    // İlk olarak log tablosundaki ilgili kayıtları sil
                    using (SqlCommand cmLog = new SqlCommand("DELETE FROM TBL_OGRENCI_ADRES_LOG WHERE OGRID=@p1", baglan.baglanti()))
                    {
                        cmLog.Parameters.AddWithValue("@p1", textID.Text);
                        cmLog.ExecuteNonQuery();
                    }

                    // Daha sonra TBL_NOTLAR tablosundaki ilgili kayıtları sil
                    using (SqlCommand cmNotlar = new SqlCommand("DELETE FROM TBL_NOTLAR WHERE OGRID=@p1", baglan.baglanti()))
                    {
                        cmNotlar.Parameters.AddWithValue("@p1", textID.Text);
                        cmNotlar.ExecuteNonQuery();
                    }

                    // En son olarak öğrenciyi sil
                    using (SqlCommand cm = new SqlCommand("DELETE FROM TBL_OGRENCILER WHERE OGRID=@p1", baglan.baglanti()))
                    {
                        cm.Parameters.AddWithValue("@p1", textID.Text);
                        int basari = cm.ExecuteNonQuery();
                        baglan.baglanti().Close();

                        if (basari == 1)
                            MessageBox.Show("ÖĞRENCİ SİLİNDİ", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("ÖĞRENCİ SİLİNMEDİ", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("BÖYLE BİR ÖĞRENCİ BULUNAMADI", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            textAd.Clear();
            textSoyadı.Clear();
            maskedTextTC.Clear();
            cmBoxil.SelectedIndex = -1;
            cmBoxilce.SelectedIndex = -1;
            richTextBoxAdres.Clear();
        }


        private void bttnGuncelle_Click_1(object sender, EventArgs e)
        {
            using (SqlCommand cm = new SqlCommand("UPDATE TBL_OGRENCILER SET OGRAD=@p1, OGRSOYAD=@p2, OGRTC=@p3, OGRIL=@p4, OGRILCE=@p5, OGRADRES=@p6 WHERE OGRID=@p7", baglan.baglanti()))
            {
                cm.Parameters.AddWithValue("@p1", textAd.Text);
                cm.Parameters.AddWithValue("@p2", textSoyadı.Text);
                cm.Parameters.AddWithValue("@p3", maskedTextTC.Text);
                cm.Parameters.AddWithValue("@p4", cmBoxil.Text);
                cm.Parameters.AddWithValue("@p5", cmBoxilce.Text);
                cm.Parameters.AddWithValue("@p6", richTextBoxAdres.Text);
                cm.Parameters.AddWithValue("@p7", textID.Text);

                int basari = cm.ExecuteNonQuery();
                baglan.baglanti().Close();

                if (basari == 1)
                    MessageBox.Show(" GÜNCELLENDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(" GÜNCELLENEMEDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textAd.Clear();
                textSoyadı.Clear();
                maskedTextTC.Clear();
                cmBoxil.SelectedIndex = -1;
                cmBoxilce.SelectedIndex = -1;
                richTextBoxAdres.Clear();
            }
        }




        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textID.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            textAd.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            textSoyadı.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            maskedTextTC.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            cmBoxil.Text = dataGridView2.CurrentRow.Cells[4].Value.ToString();
            cmBoxilce.Text = dataGridView2.CurrentRow.Cells[5].Value.ToString();
            richTextBoxAdres.Text = dataGridView2.CurrentRow.Cells[6].Value.ToString();
        }

        private void Öğrenciler_Load(object sender, EventArgs e)
        {
            listele();
            ilekle();
            BolumleriYukle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = baglan.baglanti();
            try
            {
                conn.Open();
                string sql = "SELECT * FROM TBL_OGRENCILER WHERE OGRID = @p1";
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@p1",textID.Text);
                adapter.Fill(table);
                dataGridView2.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
                string ad = txtAdi.Text.Trim();

                try
                {
                    using (SqlConnection connection = baglan.baglanti())
                    {
                        string query = "SELECT * FROM TBL_OGRENCILER";

                        if (!string.IsNullOrEmpty(ad))
                            query += " AND OGRAD LIKE @ad";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            if (!string.IsNullOrEmpty(ad))
                                command.Parameters.AddWithValue("@ad", "%" + ad + "%");

                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // DataGridView kontrolünü formunuzda tanımlamanız gerekmektedir.
                            dataGridView2.DataSource = dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void button2_Click(object sender, EventArgs e)
        {
            string ad = txtBolum.Text.Trim();

            try
            {
                using (SqlConnection connection = baglan.baglanti())
                {
                    string query = "SELECT * FROM TBL_OGRENCILER WHERE 1=1";

                    if (!string.IsNullOrEmpty(ad))
                        query += " AND OGRIL LIKE @ad";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(ad))
                            command.Parameters.AddWithValue("@ad", "%" + ad + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // DataGridView kontrolünü formunuzda tanımlamanız gerekmektedir.
                        dataGridView2.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

