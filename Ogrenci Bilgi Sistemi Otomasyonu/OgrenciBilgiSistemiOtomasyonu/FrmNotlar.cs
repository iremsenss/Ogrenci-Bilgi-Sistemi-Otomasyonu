using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SirketOtomasyonu
{
    public partial class FrmNotlar : Form
    {
        public FrmNotlar()
        {
            InitializeComponent();
        }

        sqlbaglanti baglan = new sqlbaglanti();

        private void FrmNotlar_Load(object sender, EventArgs e)
        {
            ListeleTBL_NOTLAR();
            FillComboBox();
            FillDersler();

        }

        private void ListeleTBL_NOTLAR()
        {
            using (SqlConnection con = baglan.baglanti())
            {
                try
                {
                    string query = "SELECT * FROM TBL_NOTLAR";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veri çekme hatası: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ListeleTBL_NOTLAR();
        }


        void FillComboBox()
        {
            using (SqlConnection connection = baglan.baglanti())
            {
                SqlCommand cmd = new SqlCommand("SELECT OGRID, OGRAD FROM TBL_OGRENCILER", connection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "OGRAD";
                comboBox1.ValueMember = "OGRID";
            }
        }

        void FillDersler()
        {
            using (SqlConnection connection = baglan.baglanti())
            {
                SqlCommand cmd = new SqlCommand("SELECT DID, DADI FROM TBL_DERS", connection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "DADI";
                comboBox2.ValueMember = "DID";
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            // Önce formdaki değerleri alalım
            int ogrenciID = Convert.ToInt32(comboBox1.SelectedValue);
            int dersID = Convert.ToInt32(comboBox2.SelectedValue);
            string Vnotu = txtVize.Text;
            string Fnotu = txtFinal.Text;

            using (SqlConnection connection = baglan.baglanti())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO TBL_NOTLAR (OGRID, DID, VizeNotu,FinalNotu) VALUES (@ogrenciID, @dersID, @Vnotu ,@Fnotu)", connection);
                cmd.Parameters.AddWithValue("@ogrenciID", ogrenciID);
                cmd.Parameters.AddWithValue("@dersID", dersID);
                cmd.Parameters.AddWithValue("@Vnotu", Vnotu);
                cmd.Parameters.AddWithValue("@Fnotu", Fnotu);


                // Komutu çalıştıralım

                cmd.ExecuteNonQuery();

                baglan.baglanti().Close();
            }

            // Not ekledikten sonra kullanıcıya bir mesaj gösterelim
            MessageBox.Show("Not başarıyla eklendi.");


            try
            {
                // Vize ve final notlarını al
                double vize = Convert.ToDouble(txtVize.Text);
                double final = Convert.ToDouble(txtFinal.Text);

                // Not ortalamasını hesapla
                double ortalama = (vize + final) / 2;

                // Sonucu "Not Ortalama" label'ına yaz
                lblNotOrtalama.Text = "Ortalama: " + ortalama.ToString("0.00");
            }
            catch (FormatException)
            {
                MessageBox.Show("Lütfen geçerli bir not değeri giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListeleTBL_NOTLAR();

            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Öğrenci seçiniz.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = baglan.baglanti())
                {
                    int ogrenciID;
                    if (!int.TryParse(comboBox1.SelectedValue.ToString(), out ogrenciID))
                    {
                        MessageBox.Show("Geçersiz öğrenci ID.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Önce belirli bir öğrencinin notlarını kontrol edelim
                    string query = "SELECT * FROM TBL_NOTLAR WHERE OGRID = @ogrID";
                    using (SqlCommand komut = new SqlCommand(query, connection))
                    {
                        komut.Parameters.AddWithValue("@ogrID", ogrenciID);

                        using (SqlDataReader dr = komut.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                dr.Close();

                                // Öğrencinin notlarını sil
                                string deleteQuery = "DELETE FROM TBL_NOTLAR WHERE OGRID = @ogrID";
                                using (SqlCommand cm = new SqlCommand(deleteQuery, connection))
                                {
                                    cm.Parameters.AddWithValue("@ogrID", ogrenciID);
                                    int basari = cm.ExecuteNonQuery();

                                    if (basari == 1)
                                        MessageBox.Show("Ders başarıyla silindi.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    else
                                        MessageBox.Show("Ders silinemedi.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Böyle bir ders bulunamadı.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                comboBox1.SelectedValue = row.Cells["OGRID"].Value.ToString();
                comboBox2.SelectedValue = row.Cells["DID"].Value.ToString();
                txtVize.Text = row.Cells["VizeNotu"].Value.ToString();
                txtFinal.Text = row.Cells["FinalNotu"].Value.ToString();
            }

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {

                int ogrenciID = Convert.ToInt32(comboBox1.SelectedValue);
                int dersID = Convert.ToInt32(comboBox2.SelectedValue);
                string Vnotu = txtVize.Text;
                string Fnotu = txtFinal.Text;

                SqlCommand cmd = new SqlCommand("update TBL_NOTLAR set VizeNotu = @Vnotu, FinalNotu = @Fnotu WHERE OGRID = @ogrenciID AND DID = @dersID", baglan.baglanti());

                cmd.Parameters.AddWithValue("@ogrenciID", ogrenciID);
                cmd.Parameters.AddWithValue("@dersID", dersID);
                cmd.Parameters.AddWithValue("@Vnotu", Vnotu);
                cmd.Parameters.AddWithValue("@Fnotu", Fnotu);

                int basari = cmd.ExecuteNonQuery();
                baglan.baglanti().Close();

                if (basari > 0)
                    MessageBox.Show("Not başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Not güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
           
        }
    }
}

