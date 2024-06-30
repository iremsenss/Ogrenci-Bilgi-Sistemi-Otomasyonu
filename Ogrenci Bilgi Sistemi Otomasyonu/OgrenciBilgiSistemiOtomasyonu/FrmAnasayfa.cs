using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SirketOtomasyonu
{
    public partial class FrmAnasayfa : Form
    {
        public FrmAnasayfa()
        {
            InitializeComponent();
        }

        private void FrmOgretmenlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var urun = new Öğretmenler
            {
                ShowInTaskbar = false,
                MinimizeBox = false,
                MaximizeBox = false
            };
            urun.StartPosition = FormStartPosition.CenterParent;
            urun.ShowDialog(this);

        }

        private void FrmOgrencilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ogrenci = new Öğrenciler 
            {
                ShowInTaskbar = false,
                MinimizeBox = false,
                MaximizeBox = false
            };
            ogrenci.StartPosition = FormStartPosition.CenterParent;
            ogrenci.ShowDialog(this);

        }

        private void stokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ders = new FrmDersler
            {
                ShowInTaskbar = false,
                MinimizeBox = false,
                MaximizeBox = false
            };
            ders.StartPosition = FormStartPosition.CenterParent;
            ders.ShowDialog(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void KarneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bolum = new FrmBolum
            {
                ShowInTaskbar = false,
                MinimizeBox = false,
                MaximizeBox = false
            };
            bolum.StartPosition = FormStartPosition.CenterParent;
            bolum.ShowDialog(this);
        }

        private void ayarlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var notlar = new FrmNotlar
            {
                ShowInTaskbar = false,
                MinimizeBox = false,
                MaximizeBox = false
            };
            notlar.StartPosition = FormStartPosition.CenterParent;
            notlar.ShowDialog(this);
        }

        private void eRDiyagramıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var diyagram = new FrmDiyagram
            {
                ShowInTaskbar = false,
                MinimizeBox = false,
                MaximizeBox = false
            };
            diyagram.StartPosition = FormStartPosition.CenterParent;
            diyagram.ShowDialog(this);
        }
    }
    }

