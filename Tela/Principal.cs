using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Tela.Classes;

namespace Tela
{
    public partial class Principal : FormBase
    {
        private TabuleiroController _Tabuleiro { get; set; }
        
        public const int TamanhoQuadrado = 70;
        public const int Quadrados = 10;
        
        public Principal()
        {
            InitializeComponent();
            LblStatus = LblStatus_View;
            BtnIniciar = BtnStart_View;
            LblPortStatus = LblPortStatus_View;
        }

        private void Principal_Load(object sender, EventArgs e)
        {            
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            
            UpdateStatus("Posicionando");

            _Tabuleiro = new TabuleiroController(this, BtnStart_View);
            _Tabuleiro.SetSerialController(this.PortaSerial);
            _Tabuleiro.Desenhar();            
        }

        private void Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.PortaSerial.Saindo();
        }

        private void BtnPosicionarTeste_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            _Tabuleiro.PosicionarTeste();
        }

        private void BtnForcarMinhaVez_Click(object sender, EventArgs e)
        {
            _Tabuleiro.SetMinhaRodada();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            _Tabuleiro.IniciarPartida();
            _Tabuleiro.AtualizarTabuleiro_Received();
            this.BtnStart_View.Enabled = false;
        }
    }
}
