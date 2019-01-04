namespace KFLOP
{
    partial class Review
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
            this.components = new System.ComponentModel.Container();
            this.panDisplay = new System.Windows.Forms.Panel();
            this.timerRev = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // panDisplay
            // 
            this.panDisplay.BackColor = System.Drawing.Color.White;
            this.panDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panDisplay.Location = new System.Drawing.Point(12, 15);
            this.panDisplay.Name = "panDisplay";
            this.panDisplay.Size = new System.Drawing.Size(900, 120);
            this.panDisplay.TabIndex = 1;
            // 
            // timerRev
            // 
            this.timerRev.Enabled = true;
            this.timerRev.Tick += new System.EventHandler(this.timerRev_Tick);
            // 
            // Review
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 151);
            this.Controls.Add(this.panDisplay);
            this.MaximumSize = new System.Drawing.Size(940, 190);
            this.MinimumSize = new System.Drawing.Size(940, 190);
            this.Name = "Review";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Review Busbar";
            this.Load += new System.EventHandler(this.Review_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panDisplay;
        private System.Windows.Forms.Timer timerRev;
    }
}