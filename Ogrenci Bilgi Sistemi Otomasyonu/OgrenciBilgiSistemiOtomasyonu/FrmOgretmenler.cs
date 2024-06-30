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
using System.Data.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.Mime.MediaTypeNames;

namespace SirketOtomasyonu
{
    public partial class Öğretmenler : Form
    {
        public Öğretmenler()
        {
            InitializeComponent();
        }

        sqlbaglanti baglan = new sqlbaglanti();

        void listele()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Ogretmenler", baglan.baglanti());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        void ilekle()
        {
            SqlCommand cmd = new SqlCommand("Select *from TBL_ILLER",baglan.baglanti());
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cmbIl.Items.Add(dr[1].ToString());
            }
            baglan.baglanti().Close();

        }

        private void Ürünler_Load(object sender, EventArgs e)
        {
            listele();
            ilekle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAd.Text) || string.IsNullOrEmpty(txtSoyad.Text) || string.IsNullOrEmpty(mtxtTel.Text) || string.IsNullOrEmpty(mtxtTc.Text) || string.IsNullOrEmpty(txtMail.Text) || string.IsNullOrEmpty(cmbIl.Text) || string.IsNullOrEmpty(cmbIlce.Text) || string.IsNullOrEmpty(rtxtAdres.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SqlCommand komut = new SqlCommand("insert into TBL_OGRETMENLER (OGRTAD,OGRTSOYAD,OGRTTC,OGRTTEL,OGRTMAIL,OGRTIL,OGRTILCE,OGRTADRES,OGRTBRANS) values ('" + txtAd.Text + "','" + txtSoyad.Text + "','" + mtxtTc.Text + "','" + mtxtTel.Text + "','" + txtMail.Text + "','" + cmbIl.Text + "','" + cmbIlce.Text + "','" + rtxtAdres.Text + "','" + cmbBrans.Text + "')", baglan.baglanti());
                int basari = komut.ExecuteNonQuery();
                baglan.baglanti().Close();

                if (basari == 1)
                    MessageBox.Show("KAYIT EKLENDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("KAYIT EKLENEMEDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);

                listele();
                txtAd.Clear();
                txtSoyad.Clear();
                mtxtTc.Clear();
                mtxtTel.Clear();
                txtMail.Clear();
                cmbIl.SelectedIndex = -1;
                cmbIlce.SelectedIndex = -1;
                rtxtAdres.Clear();
                cmbBrans.SelectedIndex = -1;

            }



        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int hata = 0;
            if (txtID.Text == string.Empty)
                hata = 1;
            if (hata == 1)
                MessageBox.Show("ALANI DOLDURUNUZ", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                SqlCommand komut = new SqlCommand("select * from TBL_OGRETMENLER where OGRTID='" + txtID.Text + "'", baglan.baglanti());
                komut.ExecuteNonQuery();
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    SqlCommand cm = new SqlCommand("delete from TBL_OGRETMENLER where OGRTID = '" + txtID.Text + "'", baglan.baglanti());
                    int basari = cm.ExecuteNonQuery();
                    baglan.baglanti().Close();
                    if (basari == 1)
                        MessageBox.Show("ÖĞRETMEN SİLİNDİ", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("ÖĞRETMEN SİLİNMEDİ ", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("BÖYLE BİR ÖĞRETMEN BULUNAMADI", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            txtID.Clear();
            listele();
            txtAd.Clear();
            txtSoyad.Clear();
            mtxtTc.Clear();
            mtxtTel.Clear();
            txtMail.Clear();
            cmbIl.SelectedIndex = -1;
            cmbIlce.SelectedIndex = -1;
            rtxtAdres.Clear();
            cmbBrans.SelectedIndex = -1;
        }
            private void btnGuncelle_Click(object sender, EventArgs e)
            {
                SqlCommand cm = new SqlCommand("update TBL_OGRETMENLER  set OGRTAD=@p1,OGRTSOYAD=@p2,OGRTTC=@p3,OGRTTEL=@p4,OGRTMAIL=@p5,OGRTIL=@p6,OGRTILCE=@p7,OGRTADRES=@p8,OGRTBRANS=@p9 where OGRTID=@p10", baglan.baglanti());

                cm.Parameters.AddWithValue("p1", txtAd.Text);
                cm.Parameters.AddWithValue("p2", txtSoyad.Text);
                cm.Parameters.AddWithValue("p3", mtxtTc.Text);
                cm.Parameters.AddWithValue("p4", mtxtTel.Text);
                cm.Parameters.AddWithValue("p5", txtMail.Text);
                cm.Parameters.AddWithValue("p6", cmbIl.Text);
                cm.Parameters.AddWithValue("p7", cmbIlce.Text);
                cm.Parameters.AddWithValue("p8", rtxtAdres.Text);
                cm.Parameters.AddWithValue("p9", cmbBrans.Text);
                cm.Parameters.AddWithValue("@p10", txtID.Text);

                int basari = cm.ExecuteNonQuery();
                baglan.baglanti().Close();

                if (basari == 1)
                    MessageBox.Show("KAYIT GÜNCELLENDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("KAYIT GÜNCELLENEMEDİ!!", "UYARI,", MessageBoxButtons.OK, MessageBoxIcon.Information);

                listele();
                txtAd.Clear();
                txtSoyad.Clear();
                mtxtTc.Clear();
                mtxtTel.Clear();
                txtMail.Clear();
                cmbIl.SelectedIndex = -1;
                cmbIlce.SelectedIndex = -1;
                rtxtAdres.Clear();
                cmbBrans.SelectedIndex = -1;


            }

            private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {
                txtID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtAd.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtSoyad.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                mtxtTc.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                mtxtTel.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                txtMail.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                cmbIlce.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                cmbIlce.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                rtxtAdres.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                cmbBrans.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();

            }

        private void cmbIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbIlce.Items.Clear();
            SqlCommand cmd = new SqlCommand("Select *from TBL_ILCELER where sehir=@p1", baglan.baglanti());
            cmd.Parameters.AddWithValue("@p1", cmbIl.SelectedIndex+1);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cmbIlce.Items.Add(dr[1].ToString());
            }
            baglan.baglanti().Close();
        }
    }

    }



