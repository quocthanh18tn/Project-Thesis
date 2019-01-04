namespace KFLOP
{
    partial class Command
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.comboCode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCoorY = new System.Windows.Forms.TextBox();
            this.txtCoorX = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtVel = new System.Windows.Forms.TextBox();
            this.ChePerforate = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNumLoop = new System.Windows.Forms.TextBox();
            this.txtLoop = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Code";
            // 
            // comboCode
            // 
            this.comboCode.AllowDrop = true;
            this.comboCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboCode.FormattingEnabled = true;
            this.comboCode.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.comboCode.Location = new System.Drawing.Point(169, 21);
            this.comboCode.MaxLength = 1;
            this.comboCode.Name = "comboCode";
            this.comboCode.Size = new System.Drawing.Size(66, 33);
            this.comboCode.TabIndex = 1;
            this.comboCode.TextChanged += new System.EventHandler(this.comboCode_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(26, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Coordinate X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(297, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "Coordinate Y";
            // 
            // txtCoorY
            // 
            this.txtCoorY.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCoorY.Location = new System.Drawing.Point(484, 78);
            this.txtCoorY.MaxLength = 7;
            this.txtCoorY.Name = "txtCoorY";
            this.txtCoorY.Size = new System.Drawing.Size(100, 31);
            this.txtCoorY.TabIndex = 3;
            this.txtCoorY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCoorY_KeyPress);
            // 
            // txtCoorX
            // 
            this.txtCoorX.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCoorX.Location = new System.Drawing.Point(169, 78);
            this.txtCoorX.MaxLength = 7;
            this.txtCoorX.Name = "txtCoorX";
            this.txtCoorX.Size = new System.Drawing.Size(100, 31);
            this.txtCoorX.TabIndex = 2;
            this.txtCoorX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCoorX_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(27, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 25);
            this.label4.TabIndex = 6;
            this.label4.Text = "Velocity";
            // 
            // txtVel
            // 
            this.txtVel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVel.Location = new System.Drawing.Point(169, 133);
            this.txtVel.MaxLength = 2;
            this.txtVel.Name = "txtVel";
            this.txtVel.Size = new System.Drawing.Size(100, 31);
            this.txtVel.TabIndex = 4;
            this.txtVel.TextChanged += new System.EventHandler(this.txtVel_TextChanged);
            this.txtVel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVel_KeyPress);
            // 
            // ChePerforate
            // 
            this.ChePerforate.AutoSize = true;
            this.ChePerforate.Checked = true;
            this.ChePerforate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChePerforate.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChePerforate.Location = new System.Drawing.Point(301, 135);
            this.ChePerforate.Name = "ChePerforate";
            this.ChePerforate.Size = new System.Drawing.Size(119, 29);
            this.ChePerforate.TabIndex = 5;
            this.ChePerforate.Text = "Perforate";
            this.ChePerforate.UseVisualStyleBackColor = true;
            this.ChePerforate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChePerforate_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(297, 191);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(176, 25);
            this.label5.TabIndex = 11;
            this.label5.Text = "Number of Loops";
            // 
            // txtNumLoop
            // 
            this.txtNumLoop.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumLoop.Location = new System.Drawing.Point(484, 187);
            this.txtNumLoop.MaxLength = 3;
            this.txtNumLoop.Name = "txtNumLoop";
            this.txtNumLoop.Size = new System.Drawing.Size(100, 31);
            this.txtNumLoop.TabIndex = 7;
            this.txtNumLoop.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumLoop_KeyPress);
            // 
            // txtLoop
            // 
            this.txtLoop.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoop.FormattingEnabled = true;
            this.txtLoop.Items.AddRange(new object[] {
            "None",
            "Start",
            "End"});
            this.txtLoop.Location = new System.Drawing.Point(169, 187);
            this.txtLoop.Name = "txtLoop";
            this.txtLoop.Size = new System.Drawing.Size(100, 33);
            this.txtLoop.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(27, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 25);
            this.label6.TabIndex = 14;
            this.label6.Text = "Loop";
            // 
            // butOK
            // 
            this.butOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butOK.Location = new System.Drawing.Point(271, 245);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(145, 45);
            this.butOK.TabIndex = 8;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butCancel.Location = new System.Drawing.Point(439, 245);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(145, 45);
            this.butCancel.TabIndex = 9;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // Command
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(624, 312);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtLoop);
            this.Controls.Add(this.txtNumLoop);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ChePerforate);
            this.Controls.Add(this.txtVel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCoorX);
            this.Controls.Add(this.txtCoorY);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboCode);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(640, 350);
            this.MinimumSize = new System.Drawing.Size(640, 350);
            this.Name = "Command";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Command";
            this.Load += new System.EventHandler(this.Command_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCoorY;
        private System.Windows.Forms.TextBox txtCoorX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtVel;
        private System.Windows.Forms.CheckBox ChePerforate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNumLoop;
        private System.Windows.Forms.ComboBox txtLoop;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
    }
}