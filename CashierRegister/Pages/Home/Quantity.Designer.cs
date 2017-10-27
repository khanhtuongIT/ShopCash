namespace CashierRegister.Pages.Home
{
    partial class frmQuantity
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQuantity));
            this.txbQuantity = new System.Windows.Forms.TextBox();
            this.lblWarning = new System.Windows.Forms.Label();
            this.imgWarning = new System.Windows.Forms.PictureBox();
            this.lblNotication = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgWarning)).BeginInit();
            this.SuspendLayout();
            // 
            // txbQuantity
            // 
            this.txbQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbQuantity.Dock = System.Windows.Forms.DockStyle.Top;
            this.txbQuantity.Location = new System.Drawing.Point(0, 0);
            this.txbQuantity.MaxLength = 12;
            this.txbQuantity.Name = "txbQuantity";
            this.txbQuantity.Size = new System.Drawing.Size(168, 20);
            this.txbQuantity.TabIndex = 0;
            this.txbQuantity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbQuantity_KeyDown);
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.ForeColor = System.Drawing.Color.White;
            this.lblWarning.Location = new System.Drawing.Point(23, 23);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(91, 13);
            this.lblWarning.TabIndex = 1;
            this.lblWarning.Text = "Press enter to exit";
            // 
            // imgWarning
            // 
            this.imgWarning.ErrorImage = null;
            this.imgWarning.Image = ((System.Drawing.Image)(resources.GetObject("imgWarning.Image")));
            this.imgWarning.InitialImage = null;
            this.imgWarning.Location = new System.Drawing.Point(3, 23);
            this.imgWarning.Name = "imgWarning";
            this.imgWarning.Size = new System.Drawing.Size(18, 15);
            this.imgWarning.TabIndex = 2;
            this.imgWarning.TabStop = false;
            // 
            // lblNotication
            // 
            this.lblNotication.AutoSize = true;
            this.lblNotication.ForeColor = System.Drawing.Color.White;
            this.lblNotication.Location = new System.Drawing.Point(39, 24);
            this.lblNotication.Name = "lblNotication";
            this.lblNotication.Size = new System.Drawing.Size(91, 13);
            this.lblNotication.TabIndex = 3;
            this.lblNotication.Text = "Press enter to exit";
            // 
            // frmQuantity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Firebrick;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(168, 42);
            this.ControlBox = false;
            this.Controls.Add(this.lblNotication);
            this.Controls.Add(this.imgWarning);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.txbQuantity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmQuantity";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Quantity";
            this.Enter += new System.EventHandler(this.Quantity_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.imgWarning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbQuantity;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.PictureBox imgWarning;
        private System.Windows.Forms.Label lblNotication;

    }
}