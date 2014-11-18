using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Tela.Enums;


namespace Tela.Classes
{
    public class TabuleiroController
    {
        private int _Tamanho;
        private int _Quadrados;
        private Color _Background;

        private PanelController _PanelController;
                   
        private PosicaoController _Amigo = new PosicaoController();
        private PosicaoController _Inimigo = new PosicaoController();
        private bool _Posicionar = true;

        private Panel _PosicionamentoPanel;

        private delegate void _PanelClick();

        public Form Form { get; set; }
        
        public TabuleiroController(Form form, int tamQuadrado, int quantQuadrado, Color background)
        {
            this.Form = form;
            this._Tamanho = tamQuadrado;
            this._Quadrados = quantQuadrado;
            this._Background = background;
            this._Amigo.SetPosicoesPosicionamento(quantQuadrado);
            this._PanelController = new PanelController(form, tamQuadrado, quantQuadrado, background);
            this.Form.Controls.Add(this._PanelController.PanelTabuleiroParent);
            this.Form.Controls.Add(this._PanelController.PanelPosicionarParent);
        }

        public void Desenhar()
        {
            if (_Posicionar)
            {
                DesenharPosicionamento();
            }
            DesenharTabulerio();
        }

        private void DesenharTabulerio()
        {
            LimparTabuleiro();
            var posicoes = _Amigo.Posicoes;
           
            for (var y = 0; y < _Quadrados; y++)
            {
                for (var x = 0; x < _Quadrados; x++)
                {
                    var posicao = posicoes.Where(po => po.PosX == x && po.PosY == y).FirstOrDefault();
                    var newPanel = new Panel
                    {
                        Location = new Point(_Tamanho * y, _Tamanho * x),
                        Size = new Size(_Tamanho, _Tamanho),                        
                        BackgroundImageLayout = ImageLayout.Stretch,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = _Background
                    };                    
                    newPanel.Click += new EventHandler(Panel_Click);

                    if (posicao != null)
                    {
                        newPanel.BackgroundImage = posicao.Peca.Image;
                        newPanel.BorderStyle = BorderStyle.None;
                    }

                    _PanelController.AddTabuleiroPanel(newPanel, x, y);        
                }
            }
        }

        private void DesenharPosicionamento()
        {
            LimparPosicionar();
            var pecas = _Amigo.APosicionar;                       
            
            for (int i = 0; i < pecas.Count(); i++)
            {
                var peca = pecas.ElementAt(i);
                var panel = new Panel()
                {
                    Size = new Size(_Tamanho, _Tamanho),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    BackgroundImage = peca.Image,                    
                    Location = new Point(_Tamanho * i, 0)
                };
                panel.Click += new EventHandler(Escolha_Click);                

                _PanelController.AddPosicionarPanel(panel, peca);
            }
        }

        private void MostrarOndePosicionar()
        {
            foreach (var posicaoValida in _Amigo.PosicoesPosicionamento)
            {
                Panel panel = _PanelController.GetPanelTabuleiro(posicaoValida);
                panel.BackColor = Color.Green;                           
            }
        }

        private void EsconderOndePosicionar()
        {
            foreach (var posicaoValida in _Amigo.PosicoesPosicionamento)
            {
                Panel panel = _PanelController.GetPanelTabuleiro(posicaoValida);
                panel.BackColor = _Background;
            }
        }

        private void LimparTabuleiro()
        {
            _PanelController.LimparTabuleiroPanel();            
        }

        private void LimparPosicionar()
        {
            _PanelController.LimparPosicionarPanel();
        }

        private void Panel_Click(object sender, EventArgs e)
        {
            var panel = (Panel)sender;
            if (_Posicionar)
            {
                if (_Amigo.Posicionando)
                {
                    var p = _PanelController.GetPosicaoTabuleiro(panel);
                    var retorno = _Amigo.SetPeca(_Amigo.PecaPosicionando, p.X, p.Y);
                    if (retorno.Sucesso)
                    {
                        _Amigo.TerminarPosicionamento();
                        _Posicionar = _Amigo.FaltaPosicionar;

                        Desenhar();
                    }
                    else
                    {
                        MessageBox.Show(retorno.Mensagem);
                    }
                }
            }
            else
            {
                panel.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private void Escolha_Click(object sender, EventArgs e)
        {
            var panel = (Panel)sender;
            var peca = _PanelController.GetPecaPosicionamento(panel);

            if (!_Amigo.Posicionando)
            {
                _PosicionamentoPanel = panel;
                _Amigo.IniciarPosicionamento(peca);
                panel.BorderStyle = BorderStyle.FixedSingle;
                MostrarOndePosicionar();
            }
            else if (_Amigo.PecaPosicionando.Guid == peca.Guid)
            {                
                _Amigo.CancelarPosicionamento();
                _PosicionamentoPanel.BorderStyle = BorderStyle.None;
                EsconderOndePosicionar();
            }
        }

        public void PosicionarTeste()
        {
            Peca peca;
            int faltantes = _Amigo.APosicionar.Count();
            for (int x = _Quadrados - 1; x >= 0; x--)
            {
                for (int y = _Quadrados - 1; y >= 0; y--)
                {
                    peca = _Amigo.PecasPosicionamento.ElementAt(0);
                    _Amigo.IniciarPosicionamento(peca);
                    var retorno = _Amigo.SetPeca(_Amigo.PecaPosicionando, x, y);
                    if (retorno.Sucesso)
                    {
                        _Amigo.TerminarPosicionamento();
                        if (--faltantes <= 0)
                        {
                            x = -1;
                            break;
                        }
                    }
                    else
                    {
                        _Amigo.CancelarPosicionamento();
                    }
                }
            }

            Desenhar();
            _Posicionar = _Amigo.FaltaPosicionar;
        }
    }
}
