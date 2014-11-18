using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tela.Classes
{
    public class PanelController
    {
        private List<_PanelPosicionamento> _PanelsTabuleiro = new List<_PanelPosicionamento>();
        private List<_PanelPosicionamento> _PanelsPosicionamento = new List<_PanelPosicionamento>();

        private Panel _PanelTabuleiroParent;
        private Panel _PanelPosicionarParent;

        private int _Quadrados;
        private int _Tamanho;
        private Color _Background;

        public Panel PanelTabuleiroParent { get { return _PanelTabuleiroParent; } }
        public Panel PanelPosicionarParent { get { return _PanelPosicionarParent; } }

        public PanelController(Form form, int tamQuadrado, int quantQuadrado, Color background)
        {
            this._Tamanho = tamQuadrado;
            this._Quadrados = quantQuadrado;
            this._Background = background;
            this._PanelPosicionarParent = new Panel()
            {
                AutoScroll = true,
                Size = new Size(form.Width - 15, _Tamanho + 20),
                Location = new Point(0, (_Tamanho * _Quadrados) + 10)
            };
            this._PanelTabuleiroParent = new Panel()
            {
                Location = new Point(0, 0),
                Size = new Size(_Tamanho * _Quadrados, _Tamanho * _Quadrados),
            };
        }

        public void AddTabuleiroPanel(Panel panel, int x, int y)
        {
            _PanelsTabuleiro.Add(new _PanelPosicionamento()
            {
                Panel = panel,
                Posicao = new Posicao(x, y)
            });
            _PanelTabuleiroParent.Controls.Add(panel);
        }

        public void AddPosicionarPanel(Panel panel, Peca peca)
        {
            _PanelsPosicionamento.Add(new _PanelPosicionamento()
            {
                Panel = panel,
                Peca = peca
            });
            _PanelPosicionarParent.Controls.Add(panel);
        }

        public void LimparTabuleiroPanel()
        {
            foreach (var pp in _PanelsTabuleiro)
            {
                _PanelTabuleiroParent.Controls.Remove(pp.Panel);
            }
            _PanelsTabuleiro = new List<_PanelPosicionamento>();
        }

        public void LimparPosicionarPanel()
        {
            foreach (var pp in _PanelsPosicionamento)
            {
                _PanelPosicionarParent.Controls.Remove(pp.Panel);
            }
            _PanelsPosicionamento = new List<_PanelPosicionamento>();
        }

        public Panel GetPanelTabuleiro(Posicao posicao)
        {
            return _PanelsTabuleiro.Where(p => p.Posicao.X == posicao.X && p.Posicao.Y == posicao.Y).FirstOrDefault().Panel;
        }

        public Posicao GetPosicaoTabuleiro(Panel panel)
        {
            return _PanelsTabuleiro.Where(p => p.Panel == panel).FirstOrDefault().Posicao;
        }

        public Peca GetPecaPosicionamento(Panel panel)
        {
            return _PanelsPosicionamento.Where(p => p.Panel == panel).FirstOrDefault().Peca;
        }
    }
    
    public class _PanelPosicionamento
    {
        public Panel Panel { get; set; }
        public Posicao Posicao { get; set; }
        public Peca Peca { get; set; }
    }
}
