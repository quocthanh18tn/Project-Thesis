using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace KFLOP
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

        public Form1 frmForm3;
        string ConfigKFLOPPath, ConfigsPathName, MainPathC;
        public bool CheckInOutMode;

        private void Config_Load(object sender, EventArgs e)
        {
            string strnumX, strdenX;
            string strnumY, strdenY;
            
            MainPathC = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            ConfigKFLOPPath = MainPathC + "\\Factors";
            ConfigsPathName = MainPathC + "\\ScaleFactor.txt";
            CheckInOutMode = frmForm3.CheckEnaInOut;
            if (CheckInOutMode == true)
            {
                cbEnaInOut.Checked = true;
            }
            else cbEnaInOut.Checked = false;

            try
            {
                FileStream ofs = new FileStream(ConfigsPathName, FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader rd = new StreamReader(ofs);
                strnumX = rd.ReadLine();
                strdenX = rd.ReadLine();
                strnumY = rd.ReadLine();
                strdenY = rd.ReadLine();

                rd.Close();
                ofs.Close();

                txtNumX.Text = strnumX;
                txtDenX.Text = strdenX;
                txtNumY.Text = strnumY;
                txtDenY.Text = strdenY;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtNumX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtDenX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtNumY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtDenY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void butConfigOK_Click(object sender, EventArgs e)
        {
            if (cbEnaInOut.Checked == true)
            {
                CheckInOutMode = true;
            }
            else CheckInOutMode = false;
            try
            {
                if ((Convert.ToDouble(txtDenX.Text) != 0.0) && (Convert.ToDouble(txtDenY.Text) != 0.0))
                {
                    FileStream rfs = new FileStream(ConfigsPathName, FileMode.Create, FileAccess.Write, FileShare.None);
                    StreamWriter wr = new StreamWriter(rfs);
                    wr.WriteLine(txtNumX.Text);
                    wr.WriteLine(txtDenX.Text);
                    wr.WriteLine(txtNumY.Text);
                    wr.WriteLine(txtDenY.Text);

                    wr.Flush();
                    wr.Close();
                    rfs.Close();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Denominator is zero!");
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }                    
        }

        private void butConfigCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
