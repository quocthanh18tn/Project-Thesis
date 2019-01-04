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
    public partial class Command : Form
    {
        public Command()
        {
            InitializeComponent();
        }
        
        public Form1 frmForm1;
        public bool CheckOK, F2TmpPerfor;
        public byte F2TmpCmd;//Check what button pushed (New, Edit or Insert)
        public string F2Code;
        public string F2CoorX, F2CoorY;
        public string F2Velocity;
        public string F2Loop, F2NumLoop;
        public string F2Perfor;


        private void Command_Load(object sender, EventArgs e)
        {
            CheckOK = false;
            if ((F2TmpCmd == 0) || (F2TmpCmd == 1))
            {
                txtLoop.Text = "None";
                comboCode.Text = "1";
            }
            else if (F2TmpCmd == 2)
            {
                comboCode.Text = F2Code;
                txtCoorX.Text = F2CoorX;
                txtCoorY.Text = F2CoorY;
                txtVel.Text = F2Velocity;
                if (F2Perfor == "X") ChePerforate.Checked = true;
                else ChePerforate.Checked = false;
                txtLoop.Text = F2Loop;
                txtNumLoop.Text = F2NumLoop;
            }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (((comboCode.Text == "0") || (comboCode.Text == "1") || (comboCode.Text == "2") || (comboCode.Text == "3"))
                & ((txtLoop.Text == "None") || (txtLoop.Text == "Start") || (txtLoop.Text == "End")))
            {
                if (txtLoop.Text == "Start")
                {
                    if (txtNumLoop.Text == "")
                    {
                        MessageBox.Show("Error. Please check Code or Loop!");
                    }
                    else
                    {
                        CheckOK = true;
                        F2Code = comboCode.Text;
                        F2CoorX = txtCoorX.Text;
                        F2CoorY = txtCoorY.Text;
                        F2Velocity = txtVel.Text;
                        F2TmpPerfor = ChePerforate.Checked;
                        F2Loop = txtLoop.Text;
                        F2NumLoop = txtNumLoop.Text;
                        this.Close();
                    }
                }
                else
                {
                    CheckOK = true;
                    F2Code = comboCode.Text;
                    F2CoorX = txtCoorX.Text;
                    F2CoorY = txtCoorY.Text;
                    F2Velocity = txtVel.Text;
                    F2TmpPerfor = ChePerforate.Checked;
                    F2Loop = txtLoop.Text;
                    F2NumLoop = txtNumLoop.Text;
                    this.Close();
                } 
            }
            else
            {
                MessageBox.Show("Error. Please check Code or Loop!");
            }           
        }

        private void txtCoorX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }
            else if ((e.KeyChar == '.') && (txtCoorX.Text.IndexOf('.'.ToString()) == -1)) { }
            else if (e.KeyChar == '-') { }
            else
            {
                e.Handled = true;
            }
        }

        private void txtCoorY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }
            else if ((e.KeyChar == '.') && (txtCoorX.Text.IndexOf('.'.ToString()) == -1)) { }
            else if (e.KeyChar == '-') { }
            else
            {
                e.Handled = true;
            }
        }

        private void txtVel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtNumLoop_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboCode_TextChanged(object sender, EventArgs e)
        {
            if (comboCode.Text == "0")
            {
                txtCoorX.Enabled = false;
                txtCoorY.Enabled = false;
                txtVel.Enabled = false;
                txtNumLoop.Enabled = false;
                txtLoop.Enabled = false;
                ChePerforate.Enabled = false;
                txtCoorX.Text = "";
                txtCoorY.Text = "";
                txtVel.Text = "";
                txtNumLoop.Text = "";
                txtLoop.Text = "None";
                ChePerforate.Checked = false;
            }
            else if(comboCode.Text == "3")
            {
                txtCoorX.Enabled = false;
                txtCoorY.Enabled = false;
                txtVel.Enabled = false;
                ChePerforate.Enabled = false;
                txtNumLoop.Enabled = true;
                txtLoop.Enabled = true;
                txtCoorX.Text = "-";
                txtCoorY.Text = "-";
                txtVel.Text = "";
                txtNumLoop.Text = "";
                txtLoop.Text = "None";
                ChePerforate.Checked = false;
            }
            else
            {
                txtCoorX.Enabled = true;
                txtCoorY.Enabled = true;
                txtVel.Enabled = true;
                txtNumLoop.Enabled = true;
                txtLoop.Enabled = true;
                ChePerforate.Enabled = true;
                txtCoorX.Text = "";
                txtCoorY.Text = "";
                txtVel.Text = "";
                txtNumLoop.Text = "";
                txtLoop.Text = "None";
                ChePerforate.Checked = true;
            }
        }

        private void ChePerforate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ChePerforate.Checked == true)
                    ChePerforate.Checked = false;
                else ChePerforate.Checked = true;
            }
        }

        private void txtVel_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtVel.Text != "")
                {
                    if (Convert.ToDouble(txtVel.Text) > 80)
                    {
                        MessageBox.Show("Please enter Velocity: 0-80");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }         
        }
    }
}
