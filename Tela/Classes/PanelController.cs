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

        private MyPanel _PanelTabuleiroParent;
        private MyPanel _PanelPosicionarParent;

        private int _Quadrados;
        private int _Tamanho;
        private Color _Background;

        public List<_PanelPosicionamento> PanelsTabuleiro { get { return _PanelsTabuleiro; } }
        public List<_PanelPosicionamento> PanelsPosicionamento { get { return _PanelsPosicionamento; } }

        public MyPanel PanelTabuleiroParent { get { return _PanelTabuleiroParent; } }
        public MyPanel PanelPosicionarParent { get { return _PanelPosicionarParent; } }

        public PanelController(Form form, int tamQuadrado, int quantQuadrado, Color background)
        {
            this._Tamanho = tamQuadrado;
            this._Quadrados = quantQuadrado;
            this._Background = background;
            this._PanelPosicionarParent = new MyPanel()
            {
                AutoScroll = true,
                Size = new Size(form.Width - 15, _Tamanho + 20),
                Location = new Point(0, (_Tamanho * _Quadrados) + 10)
            };
            this._PanelTabuleiroParent = new MyPanel()
            {
                Location = new Point(0, 0),
                Size = new Size(_Tamanho * _Quadrados, _Tamanho * _Quadrados),
            };
        }

        public void AddTabuleiroPanel(MyPanel panel, int x, int y)
        {
            _PanelsTabuleiro.Add(new _PanelPosicionamento()
            {
                Panel = panel,
                Posicao = new Posicao(x, y)
            });
            _PanelTabuleiroParent.Controls.Add(panel);
        }

        public void AddPosicionarPanel(MyPanel panel, Peca peca)
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

        public _PanelPosicionamento GetPanelInfoTabuleiro(int x, int y)
        {
            return _PanelsTabuleiro.Where(p => p.Posicao.X == x && p.Posicao.Y == y).FirstOrDefault();
        }

        public MyPanel GetPanelTabuleiro(Posicao posicao)
        {
            return _PanelsTabuleiro.Where(p => p.Posicao.X == posicao.X && p.Posicao.Y == posicao.Y).FirstOrDefault().Panel;
        }

        public _PanelPosicionamento GetTabuleiroInfoByGuid(Guid guid)
        {
            return _PanelsTabuleiro.Where(p => p.Panel.Guid == guid).FirstOrDefault();
        }

        public Peca GetPecaPosicionamento(Guid guid)
        {
            return _PanelsPosicionamento.Where(p => p.Panel.Guid == guid).FirstOrDefault().Peca;
        }

        public bool ExistePecaNoPosicionamento(MyPanel panel)
        {
            return _PanelsPosicionamento.Any(p => p.Panel == panel);
        }

        public _PanelPosicionamento GetPecaMovimentoInfo(Guid guid)
        {            
            return _PanelsTabuleiro.Where(p => p.Panel.Guid == guid).FirstOrDefault();
        }

        public void SetPecaTabuleiro(Guid guid, Peca peca, bool inimigo = false)
        {
            var info = _PanelsTabuleiro.Where(p => p.Panel != null && p.Panel.Guid == guid).FirstOrDefault();
            info.Peca = peca;
            info.Inimigo = inimigo;
        }

        public void MovimentarPeca(_PanelPosicionamento antiga, _PanelPosicionamento nova)
        {
            nova.Peca = antiga.Peca;
            antiga.Peca = null;
        }
    }
    
    public class _PanelPosicionamento
    {
        public MyPanel Panel { get; set; }
        public Posicao Posicao { get; set; }
        public Peca Peca { get; set; }
        public bool Inimigo { get; set; }
    }
}
