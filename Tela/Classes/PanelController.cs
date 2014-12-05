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
        private List<_PanelPosicionamento> _PanelsModificados = new List<_PanelPosicionamento>();

        private MyPanel _PanelTabuleiroParent;
        private MyPanel _PanelPosicionarParent;

        private Form _Form;

        public List<_PanelPosicionamento> PanelsTabuleiro { get { return _PanelsTabuleiro; } }
        public List<_PanelPosicionamento> PanelsPosicionamento { get { return _PanelsPosicionamento; } }
        
        public MyPanel PanelTabuleiroParent { get { return _PanelTabuleiroParent; } }
        public MyPanel PanelPosicionarParent { get { return _PanelPosicionarParent; } }

        public PanelController(Form form)
        {
            this._Form = form;
            this._PanelPosicionarParent = new MyPanel()
            {
                AutoScroll = true,
                Size = new Size(this._Form.Width - 15, Principal.TamanhoQuadrado + 20),
                Location = new Point(0, (Principal.TamanhoQuadrado * Principal.Quadrados) + 75)
            };
            this._PanelTabuleiroParent = new MyPanel()
            {
                Location = new Point(Principal.TamanhoQuadrado, Principal.TamanhoQuadrado),
                Size = new Size(Principal.TamanhoQuadrado * Principal.Quadrados, Principal.TamanhoQuadrado * Principal.Quadrados),
            };

            MostrarIndicadorPoscoes();
        }

        public void AddTabuleiroPanel(MyPanel panel, int x, int y)
        {
            var info = new _PanelPosicionamento()
            {
                Panel = panel,
                Posicao = new Posicao(x, y)
            };
            _PanelsTabuleiro.Add(info);
            _PanelTabuleiroParent.Controls.Add(panel);
            _PanelsModificados.Add(info);
        }

        public void AddPosicionarPanel(MyPanel panel, Peca peca)
        {
            CustomTooltip.Create(panel, peca.Nome);
            _PanelsPosicionamento.Add(new _PanelPosicionamento()
            {
                Panel = panel,
                Peca = peca
            });
            _PanelPosicionarParent.Controls.Add(panel);
        }

        public void MostrarIndicadorPoscoes()
        {            
            var size = new Size(Principal.TamanhoQuadrado, Principal.TamanhoQuadrado);
            var font = new Font("Arial", 16);            
            List<Control> list = new List<Control>();
            
            for (int i = 0; i < 10; i++)
			{
                var a = PosicaoController.Colunas[i];
                list.Add(new Label()
                {
                    Location = new Point(0, (i + 1) * Principal.TamanhoQuadrado),
                    Text = (i + 1).ToString(),
                    Size = size,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = font,
                    BorderStyle = BorderStyle.FixedSingle
                });
                list.Add(new Label()
                {
                    Location = new Point((i + 1) * Principal.TamanhoQuadrado, 0),
                    Text = a,
                    Size = size,                    
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = font,
                    BorderStyle = BorderStyle.FixedSingle                    
                });
			}
            _Form.Controls.AddRange(list.ToArray());
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

        public _PanelPosicionamento GetPanelInfoTabuleiro(Posicao posicao)
        {
            return GetPanelInfoTabuleiro(posicao.X, posicao.Y);
        }

        public _PanelPosicionamento GetPanelInfoTabuleiro(int x, int y)
        {
            return _PanelsTabuleiro.Where(o => o.Posicao.Compare(x, y)).FirstOrDefault();
        }

        public MyPanel GetPanelTabuleiro(Posicao posicao)
        {
            return _PanelsTabuleiro.Where(o => o.Posicao.Compare(posicao)).FirstOrDefault().Panel;
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

        public void SetPecaTabuleiro(Guid guid, Peca peca)
        {
            var info = _PanelsTabuleiro.Where(p => p.Panel != null && p.Panel.Guid == guid).FirstOrDefault();
            info.Peca = peca;
            //CustomTooltip.Create(info.Panel, peca.Nome);
            _PanelsModificados.Add(info);
        }

        public void SetPecaTabuleiroInimigo(Posicao posicao, Peca peca) {
            var info = _PanelsTabuleiro.Where(p => p.Posicao != null && p.Posicao.Compare(posicao)).FirstOrDefault();
            info.Peca = peca;
            info.Inimigo = true;
            _PanelsModificados.Add(info);
        }

        public void MovimentarPeca(_PanelPosicionamento antiga, _PanelPosicionamento nova)
        {
            nova.Peca = antiga.Peca;
            antiga.Peca = null;
            //CustomTooltip.Remove(antiga.Panel);
            //CustomTooltip.Create(nova.Panel, nova.Peca.Nome);
            _PanelsModificados.Add(nova);
            _PanelsModificados.Add(antiga);
        }

        public void MovimentarPecaInimigo(_PanelPosicionamento antiga, Posicao posicaoNova)
        {
            var nova = GetPanelInfoTabuleiro(posicaoNova.X, posicaoNova.Y);
            nova.Peca = antiga.Peca;
            nova.Inimigo = true;
            antiga.Peca = null;
            antiga.Inimigo = false;
            _PanelsModificados.Add(nova);
            _PanelsModificados.Add(antiga);
        }

        public void MatarPeca(Posicao posicao)
        {
            var info = GetPanelInfoTabuleiro(posicao.X, posicao.Y);
            info.Peca = null;
            info.Inimigo = false;
            _PanelsModificados.Add(info);
        }

        public _PanelPosicionamento[] PanelsModificados()
        {
            var temp = _PanelsModificados.ToArray();
            _PanelsModificados = new List<_PanelPosicionamento>();
            return temp;
        }
    }
    
    public class _PanelPosicionamento
    {
        public MyPanel Panel { get; set; }
        public Posicao Posicao { get; set; }
        public Peca Peca { get; set; }
        public bool Inimigo
        {
            get { return Panel.Inimigo; }
            set
            {
                Panel.SetInimigo(value);
            }
        }
    }
}
