using Tela.Classes;
namespace Tela
{
    partial class Principal
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
            this.PortaSerial = new Tela.Classes.SerialController(this.components);
            this.LblStatus_View = new System.Windows.Forms.Label();
            this.BtnPosicionarTeste = new System.Windows.Forms.Button();
            this.BtnForcarMinhaVez = new System.Windows.Forms.Button();
            this.BtnStart_View = new System.Windows.Forms.Button();
            this.LblPortStatus_View = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LblStatus_View
            // 
            this.LblStatus_View.Location = new System.Drawing.Point(798, 70);
            this.LblStatus_View.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblStatus_View.Name = "LblStatus_View";
            this.LblStatus_View.Size = new System.Drawing.Size(85, 44);
            this.LblStatus_View.TabIndex = 0;
            this.LblStatus_View.Text = "informações";
            this.LblStatus_View.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnPosicionarTeste
            // 
            this.BtnPosicionarTeste.Location = new System.Drawing.Point(801, 235);
            this.BtnPosicionarTeste.Name = "BtnPosicionarTeste";
            this.BtnPosicionarTeste.Size = new System.Drawing.Size(75, 23);
            this.BtnPosicionarTeste.TabIndex = 5;
            this.BtnPosicionarTeste.Text = "Posicionar";
            this.BtnPosicionarTeste.UseVisualStyleBackColor = true;
            this.BtnPosicionarTeste.Click += new System.EventHandler(this.BtnPosicionarTeste_Click);
            // 
            // BtnForcarMinhaVez
            // 
            this.BtnForcarMinhaVez.Location = new System.Drawing.Point(801, 265);
            this.BtnForcarMinhaVez.Name = "BtnForcarMinhaVez";
            this.BtnForcarMinhaVez.Size = new System.Drawing.Size(75, 23);
            this.BtnForcarMinhaVez.TabIndex = 6;
            this.BtnForcarMinhaVez.Text = "Minha Vez";
            this.BtnForcarMinhaVez.UseVisualStyleBackColor = true;
            this.BtnForcarMinhaVez.Click += new System.EventHandler(this.BtnForcarMinhaVez_Click);
            // 
            // BtnStart_View
            // 
            this.BtnStart_View.Enabled = false;
            this.BtnStart_View.Location = new System.Drawing.Point(801, 32);
            this.BtnStart_View.Name = "BtnStart_View";
            this.BtnStart_View.Size = new System.Drawing.Size(75, 23);
            this.BtnStart_View.TabIndex = 10;
            this.BtnStart_View.Text = "Iniciar Partida";
            this.BtnStart_View.UseVisualStyleBackColor = true;
            this.BtnStart_View.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // LblPortStatus_View
            // 
            this.LblPortStatus_View.Location = new System.Drawing.Point(798, 114);
            this.LblPortStatus_View.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblPortStatus_View.Name = "LblPortStatus_View";
            this.LblPortStatus_View.Size = new System.Drawing.Size(85, 71);
            this.LblPortStatus_View.TabIndex = 11;
            this.LblPortStatus_View.Text = "status porta";
            this.LblPortStatus_View.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(885, 883);
            this.Controls.Add(this.LblPortStatus_View);
            this.Controls.Add(this.BtnStart_View);
            this.Controls.Add(this.BtnForcarMinhaVez);
            this.Controls.Add(this.BtnPosicionarTeste);
            this.Controls.Add(this.LblStatus_View);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Principal";
            this.Text = "Combate";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Principal_FormClosing);
            this.Load += new System.EventHandler(this.Principal_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public SerialController PortaSerial;
        private System.Windows.Forms.Label LblStatus_View;
        private System.Windows.Forms.Button BtnPosicionarTeste;
        private System.Windows.Forms.Button BtnForcarMinhaVez;
        private System.Windows.Forms.Button BtnStart_View;
        private System.Windows.Forms.Label LblPortStatus_View;
    }
}