namespace HelloEEG
{
    partial class Autonomy_UI
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
            this.connectUno = new System.Windows.Forms.Button();
            this.connectMindwave = new System.Windows.Forms.Button();
            this.unoStatus = new System.Windows.Forms.Label();
            this.mindwaveStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // connectUno
            // 
            this.connectUno.Location = new System.Drawing.Point(45, 28);
            this.connectUno.Name = "connectUno";
            this.connectUno.Size = new System.Drawing.Size(111, 49);
            this.connectUno.TabIndex = 0;
            this.connectUno.Text = "Uno Connect";
            this.connectUno.UseVisualStyleBackColor = true;
            // 
            // connectMindwave
            // 
            this.connectMindwave.Location = new System.Drawing.Point(224, 28);
            this.connectMindwave.Name = "connectMindwave";
            this.connectMindwave.Size = new System.Drawing.Size(111, 49);
            this.connectMindwave.TabIndex = 1;
            this.connectMindwave.Text = "Mindwave Connect";
            this.connectMindwave.UseVisualStyleBackColor = true;
            // 
            // unoStatus
            // 
            this.unoStatus.AutoSize = true;
            this.unoStatus.Location = new System.Drawing.Point(42, 90);
            this.unoStatus.Name = "unoStatus";
            this.unoStatus.Size = new System.Drawing.Size(56, 17);
            this.unoStatus.TabIndex = 2;
            this.unoStatus.Text = "Status: ";
            this.unoStatus.Click += new System.EventHandler(this.label1_Click);
            // 
            // mindwaveStatus
            // 
            this.mindwaveStatus.AutoSize = true;
            this.mindwaveStatus.Location = new System.Drawing.Point(221, 90);
            this.mindwaveStatus.Name = "mindwaveStatus";
            this.mindwaveStatus.Size = new System.Drawing.Size(56, 17);
            this.mindwaveStatus.TabIndex = 3;
            this.mindwaveStatus.Text = "Status: ";
            // 
            // Autonomy_UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 332);
            this.Controls.Add(this.mindwaveStatus);
            this.Controls.Add(this.unoStatus);
            this.Controls.Add(this.connectMindwave);
            this.Controls.Add(this.connectUno);
            this.Name = "Autonomy_UI";
            this.Text = "Autonomy_UI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectUno;
        private System.Windows.Forms.Button connectMindwave;
        private System.Windows.Forms.Label unoStatus;
        private System.Windows.Forms.Label mindwaveStatus;
    }
}