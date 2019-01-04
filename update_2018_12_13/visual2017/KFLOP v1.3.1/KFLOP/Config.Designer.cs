namespace KFLOP
{
    partial class Config
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtNumX = new System.Windows.Forms.TextBox();
            this.txtDenX = new System.Windows.Forms.TextBox();
            this.butConfigOK = new System.Windows.Forms.Button();
            this.butConfigCancel = new System.Windows.Forms.Button();
            this.txtDenY = new System.Windows.Forms.TextBox();
            this.txtNumY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbEnaInOut = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Numerator:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 31);
            this.label2.TabIndex = 1;
            this.label2.Text = "Denominator:";
            // 
            // txtNumX
            // 
            this.txtNumX.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumX.Location = new System.Drawing.Point(207, 53);
            this.txtNumX.MaxLength = 5;
            this.txtNumX.Name = "txtNumX";
            this.txtNumX.Size = new System.Drawing.Size(111, 35);
            this.txtNumX.TabIndex = 1;
            this.txtNumX.Text = "1";
            this.txtNumX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumX_KeyPress);
            // 
            // txtDenX
            // 
            this.txtDenX.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDenX.Location = new System.Drawing.Point(207, 105);
            this.txtDenX.MaxLength = 5;
            this.txtDenX.Name = "txtDenX";
            this.txtDenX.Size = new System.Drawing.Size(111, 35);
            this.txtDenX.TabIndex = 2;
            this.txtDenX.Text = "1";
            this.txtDenX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDenX_KeyPress);
            // 
            // butConfigOK
            // 
            this.butConfigOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butConfigOK.Location = new System.Drawing.Point(137, 216);
            this.butConfigOK.Name = "butConfigOK";
            this.butConfigOK.Size = new System.Drawing.Size(145, 40);
            this.butConfigOK.TabIndex = 5;
            this.butConfigOK.Text = "OK";
            this.butConfigOK.UseVisualStyleBackColor = true;
            this.butConfigOK.Click += new System.EventHandler(this.butConfigOK_Click);
            // 
            // butConfigCancel
            // 
            this.butConfigCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butConfigCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butConfigCancel.Location = new System.Drawing.Point(305, 216);
            this.butConfigCancel.Name = "butConfigCancel";
            this.butConfigCancel.Size = new System.Drawing.Size(145, 40);
            this.butConfigCancel.TabIndex = 6;
            this.butConfigCancel.Text = "Cancel";
            this.butConfigCancel.UseVisualStyleBackColor = true;
            this.butConfigCancel.Click += new System.EventHandler(this.butConfigCancel_Click);
            // 
            // txtDenY
            // 
            this.txtDenY.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDenY.Location = new System.Drawing.Point(339, 105);
            this.txtDenY.MaxLength = 5;
            this.txtDenY.Name = "txtDenY";
            this.txtDenY.Size = new System.Drawing.Size(111, 35);
            this.txtDenY.TabIndex = 4;
            this.txtDenY.Text = "1";
            this.txtDenY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDenY_KeyPress);
            // 
            // txtNumY
            // 
            this.txtNumY.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumY.Location = new System.Drawing.Point(339, 53);
            this.txtNumY.MaxLength = 5;
            this.txtNumY.Name = "txtNumY";
            this.txtNumY.Size = new System.Drawing.Size(111, 35);
            this.txtNumY.TabIndex = 3;
            this.txtNumY.Text = "1";
            this.txtNumY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumY_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(205, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 38);
            this.label3.TabIndex = 7;
            this.label3.Text = "X axis";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(337, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 38);
            this.label4.TabIndex = 8;
            this.label4.Text = "Y axis";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cbEnaInOut
            // 
            this.cbEnaInOut.AutoSize = true;
            this.cbEnaInOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbEnaInOut.Location = new System.Drawing.Point(18, 159);
            this.cbEnaInOut.Name = "cbEnaInOut";
            this.cbEnaInOut.Size = new System.Drawing.Size(196, 28);
            this.cbEnaInOut.TabIndex = 9;
            this.cbEnaInOut.Text = "Enable Input/Output";
            this.cbEnaInOut.UseVisualStyleBackColor = true;
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butConfigCancel;
            this.ClientSize = new System.Drawing.Size(473, 272);
            this.Controls.Add(this.cbEnaInOut);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDenY);
            this.Controls.Add(this.txtNumY);
            this.Controls.Add(this.butConfigCancel);
            this.Controls.Add(this.butConfigOK);
            this.Controls.Add(this.txtDenX);
            this.Controls.Add(this.txtNumX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Config";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scale Factor";
            this.Load += new System.EventHandler(this.Config_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNumX;
        private System.Windows.Forms.TextBox txtDenX;
        private System.Windows.Forms.Button butConfigOK;
        private System.Windows.Forms.Button butConfigCancel;
        private System.Windows.Forms.TextBox txtDenY;
        private System.Windows.Forms.TextBox txtNumY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbEnaInOut;
    }
}