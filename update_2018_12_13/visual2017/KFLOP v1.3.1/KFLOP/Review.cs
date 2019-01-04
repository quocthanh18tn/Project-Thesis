using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KFLOP
{
    public partial class Review : Form
    {
        public Review()
        {
            InitializeComponent();
        }

        public Form1 frmWFReview;
        public string tmptxtW, tmptxtH;
        public int tmpdatarev;
        public int[,] arrdatarev = new int[100, 2];
        double tmpmin;

        private void Review_Load(object sender, EventArgs e)
        {
            try
            {
                double tmpW = 900.0 / Convert.ToInt32(tmptxtW);
                double tmpH = 120.0 / Convert.ToInt32(tmptxtH);
                tmpmin = Math.Round(Math.Min(tmpW, tmpH), 1);
                int tmpsizeW = Convert.ToInt32(Convert.ToDouble(tmptxtW) * tmpmin);
                int tmpsizeH = Convert.ToInt32(Convert.ToDouble(tmptxtH) * tmpmin);
                panDisplay.Size = new Size(tmpsizeW, tmpsizeH);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timerRev_Tick(object sender, EventArgs e)
        {
            Graphics ReviewDisplay = panDisplay.CreateGraphics();
            for (int i = 0; i < tmpdatarev; i++)
            {
                int tmpX = Convert.ToInt32(Convert.ToDouble(arrdatarev[i, 0]) * tmpmin);
                int tmpY = Convert.ToInt32(Convert.ToDouble(arrdatarev[i, 1]) * tmpmin);
                ReviewDisplay.FillRectangle(Brushes.Blue, tmpX - 2, tmpY - 2, 5, 5);
            }
            ReviewDisplay.Dispose();
            timerRev.Enabled = false;
        }
    }
}
