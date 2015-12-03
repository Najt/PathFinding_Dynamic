namespace MyEngine
{
    partial class Form1
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
            this.renderBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dynamicBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.renderBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dynamicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // renderBox
            // 
            this.renderBox.Location = new System.Drawing.Point(126, 83);
            this.renderBox.Name = "renderBox";
            this.renderBox.Size = new System.Drawing.Size(635, 377);
            this.renderBox.TabIndex = 0;
            this.renderBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // dynamicBox
            // 
            this.dynamicBox.Location = new System.Drawing.Point(166, 113);
            this.dynamicBox.Name = "dynamicBox";
            this.dynamicBox.Size = new System.Drawing.Size(100, 50);
            this.dynamicBox.TabIndex = 2;
            this.dynamicBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.dynamicBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.renderBox);
            this.Name = "Form1";
            this.Text = "Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.onClose);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeEnd += new System.EventHandler(this.resied);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyPressHandler);
            this.Resize += new System.EventHandler(this.resied);
            ((System.ComponentModel.ISupportInitialize)(this.renderBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dynamicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox renderBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox dynamicBox;
    }
}

