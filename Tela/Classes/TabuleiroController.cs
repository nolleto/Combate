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
        private bool _MinhaRodada = false;

        private Color _Background;
        private Color _BackgroundPosicionar = Color.LightGreen;
        private Color _BackgroundInimigo = Color.Red;        

        private PanelController _PanelController;
                   
        private PosicaoController _Amigo = new PosicaoController();
        private PosicaoController _Inimigo = new PosicaoController();

        private PosicaoMovimentosController _Movimentos;

        private bool _Posicionar = true;

        private MyPanel _PosicionamentoPanel;
        private MyPanel _MovimentoPanel;

        private List<MyPanel> _PanelsPodeMover = new List<MyPanel>();

        private Posicao[] _Aguas = new Posicao[] {
            new Posicao(2, 4),
            new Posicao(3, 4),
            new Posicao(6, 4),
            new Posicao(7, 4),
            new Posicao(2, 5),
            new Posicao(3, 5),
            new Posicao(6, 5),
            new Posicao(7, 5),
        };

        public Form Form { get; set; }

        public int Quadrados { get { return _Quadrados; } }

        public PosicaoController Amigo { get { return _Amigo; } }
        public PosicaoController Inimigo { get { return _Inimigo; } }
        public Posicao[] Obstaculos { get { return _Aguas; } }
        
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
            this._Movimentos = new PosicaoMovimentosController(this);
            DesenharTabulerio();
        }

        public void Desenhar()
        {
            if (_Posicionar)
            {
                DesenharPosicionamento();
            }
            AtualizarTabuleiro();
        }

        private void DesenharTabulerio()
        {
            LimparTabuleiro();
            var posicoes = _Amigo.Posicoes;
            
            for (var x = 0; x < _Quadrados; x++)
            {
                for (var y = 0; y < _Quadrados; y++)
                {                    
                    var newPanel = new MyPanel
                    {
                        Location = new Point(_Tamanho * x, _Tamanho * y),
                        Size = new Size(_Tamanho, _Tamanho),                        
                        BackgroundImageLayout = ImageLayout.Stretch,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = _Background
                    };
                    if (!_Aguas.Any(p => p.X == x && p.Y == y))
                    {
                        newPanel.Click += new EventHandler(Panel_Click);
                    }
                    else
                    {
                        newPanel.SetAgua(true);
                    }
                 
                    _PanelController.AddTabuleiroPanel(newPanel, x, y);        
                }
            }
        }

        private void AtualizarTabuleiro()
        {
            var panels = _PanelController.PanelsTabuleiro;
            
            for (var x = 0; x < _Quadrados; x++)
            {
                for (var y = 0; y < _Quadrados; y++)
                {
                    var info = panels.Where(p => p.Posicao.X == x && p.Posicao.Y == y).FirstOrDefault();

                    if (info.Inimigo)
                    {
                        info.Panel.BackgroundImage = null;
                        info.Panel.BorderStyle = BorderStyle.None;
                        info.Panel.BackColor = Color.Black;
                        info.Panel.Enabled = false;
                    }
                    else if (info.Peca != null)
                    {
                        info.Panel.BackgroundImage = info.Peca.Image;
                        info.Panel.BorderStyle = BorderStyle.None;
                    }
                    else if (_Aguas.Any(p => p.X == x && p.Y == y))
                    {
                        info.Panel.BackgroundImage = Properties.Resources.backgroundblue;
                        info.Panel.BorderStyle = BorderStyle.FixedSingle;
                    }
                    else
                    {
                        info.Panel.BackgroundImage = Properties.Resources.backgroundgreen;
                        info.Panel.BorderStyle = BorderStyle.FixedSingle;
                    }
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
                var panel = new MyPanel()
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

        /*private void AtualizarPosicionamento()
        {
            bool reposicionar = false;
            for (int i = 0; i < _PanelController.PanelPosicionarParent.Controls.Count; i++)
            {
                var obj = _PanelController.PanelPosicionarParent.Controls[i];
                if (obj is MyPanel)
                {
                    MyPanel panel = (MyPanel)obj;                    
                    if (!reposicionar && !_PanelController.ExistePecaNoPosicionamento(panel))
                    {
                        reposicionar = true;
                    }
                    if (reposicionar)
                    {
                        MyPanel.loca
                    }
                }
            }
        }*/

        private void MostrarOndePosicionar()
        {
            var posicoes = _Amigo.PosicoesPosicionamento;
            foreach (var posicaoValida in posicoes)
            {
                MyPanel panel = _PanelController.GetPanelTabuleiro(posicaoValida);
                panel.Animar();
            }
        }

        private void EsconderOndePosicionar()
        {
            var posicoes = _Amigo.PosicoesPosicionamento;
            foreach (var posicaoValida in posicoes)
            {
                MyPanel panel = _PanelController.GetPanelTabuleiro(posicaoValida);
                panel.Desanimar();
            }
        }

        private void MostrarOndeMover(_PanelPosicionamento info)
        {
            _PanelsPodeMover = new List<MyPanel>();
            var posicoes = _Movimentos.GetPosicoesMovimento(info);

            foreach (var posicao in posicoes)
            {
                var panel = _PanelController.GetPanelTabuleiro(posicao);

                if (posicao.Amigo)
                {
                    panel.BackColor = _Background;
                }
                else if (posicao.Inimigo)
                {
                    panel.BackColor = _BackgroundInimigo;
                }
                else if (posicao.Move)
                {
                    _PanelsPodeMover.Add(panel);
                    //panel.BackColor = _BackgroundPosicionar;
                    panel.Animar();
                }
            }
        }
        
        private void EsconderOndeMover(_PanelPosicionamento info)
        {
            foreach (var panel in _PanelsPodeMover)
            {
                //panel.BackColor = _Background;
                panel.Desanimar();
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
            var panel = (MyPanel)sender;            
            if (_Posicionar)
            {
                if (_Amigo.Posicionando)
                {
                    var info = _PanelController.GetTabuleiroInfoByGuid(panel.Guid);
                    var retorno = _Amigo.SetPosicaoPeca(_Amigo.PecaPosicionando, info.Posicao.X, info.Posicao.Y);
                    if (retorno.Sucesso)
                    {
                        EsconderOndePosicionar();
                        _PanelController.SetPecaTabuleiro(panel.Guid, _Amigo.PecaPosicionando);
                        _Amigo.TerminarPosicionamento(info.Posicao);                        
                        _Posicionar = _Amigo.FaltaPosicionar;
                        
                        Desenhar();
                    }
                    else
                    {
                        MessageBox.Show(retorno.Mensagem);
                    }
                }
            }
            else if (_MinhaRodada)
            {
                var infoNova = _PanelController.GetPecaMovimentoInfo(panel.Guid);                
                if (!_Amigo.Movimentando && infoNova.Peca != null )
                {
                    if (infoNova.Peca.Anda)
                    {
                        _MovimentoPanel = panel;
                        _Amigo.IniciarMovimento(infoNova.Peca);
                        panel.BorderStyle = BorderStyle.FixedSingle;
                        MostrarOndeMover(infoNova);
                    }
                }
                else if (infoNova.Peca != null)
                {
                    _Amigo.CancelarMovimento();
                    _MovimentoPanel.BorderStyle = BorderStyle.None;
                    EsconderOndeMover(infoNova);
                }
                else if (_MovimentoPanel != null)
                {
                    var infoAntiga = _PanelController.GetTabuleiroInfoByGuid(_MovimentoPanel.Guid);
                    var posicoes = _Movimentos.GetPosicoesMovimento(infoAntiga);
                    var retorno = _Amigo.SetPosicaoPecaMovimento(infoAntiga, infoNova.Posicao.X, infoNova.Posicao.Y, posicoes);
                    if (retorno.Sucesso)
                    {
                        _MovimentoPanel.BorderStyle = BorderStyle.None;
                        _MovimentoPanel = null;
                        _PanelController.MovimentarPeca(infoAntiga, infoNova);
                        _Amigo.TerminarMovimento(infoAntiga.Posicao);
                        EsconderOndeMover(infoNova);

                        AtualizarTabuleiro();
                        TerminarRodada();
                    }
                    else
                    {
                        MessageBox.Show(retorno.Mensagem);
                    }
                }
            }
        }

        private void Escolha_Click(object sender, EventArgs e)
        {
            var panel = (MyPanel)sender;
            var peca = _PanelController.GetPecaPosicionamento(panel.Guid);

            if (!_Amigo.Posicionando)
            {
                _PosicionamentoPanel = panel;
                _Amigo.IniciarPosicionamento(peca);
                panel.BorderStyle = BorderStyle.FixedSingle;
                MostrarOndePosicionar();
            }
            else
            {                
                _Amigo.CancelarPosicionamento();
                _PosicionamentoPanel.BorderStyle = BorderStyle.None;
                EsconderOndePosicionar();
            }
        }

        public void PosicionarTeste()
        {
            Peca peca;
            int faltantes = _Amigo.APosicionar.Count() - 1;
            for (int x = _Quadrados - 1; x >= 0; x--)
            {
                for (int y = _Quadrados - 1; y >= 0; y--)
                {
                    peca = _Amigo.PecasPosicionamento.ElementAt(faltantes);
                    _Amigo.IniciarPosicionamento(peca);
                    var retorno = _Amigo.SetPosicaoPeca(_Amigo.PecaPosicionando, x, y);
                    if (retorno.Sucesso)
                    {
                        var info = _PanelController.GetPanelInfoTabuleiro(x, y);
                        _PanelController.SetPecaTabuleiro(info.Panel.Guid, _Amigo.PecaPosicionando);
                        _Amigo.TerminarPosicionamento(info.Posicao);
                        if (--faltantes < 0)
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
            PosicionarInimigos();
        }

        public void PosicionarInimigos(Tela.Classes.PosicaoController.PosicaoPeca[] itens = null)
        {
            if (itens == null)
            {
                
            }

            Peca peca;
            int faltantes = _Inimigo.APosicionar.Count() - 1;
            for (int y = 0; y < _Quadrados; y++)
            {
                for (int x = _Quadrados - 1; x >= 0; x--)
                {
                    peca = _Inimigo.PecasPosicionamento.ElementAt(faltantes);
                    _Inimigo.IniciarPosicionamento(peca);
                    var retorno = _Inimigo.SetPosicaoPecaInimigo(_Inimigo.PecaPosicionando, x, y);
                    if (retorno.Sucesso)
                    {
                        var info = _PanelController.GetPanelInfoTabuleiro(x, y);
                        _PanelController.SetPecaTabuleiro(info.Panel.Guid, _Inimigo.PecaPosicionando, true);
                        _Inimigo.TerminarPosicionamento(info.Posicao);
                        if (--faltantes < 0)
                        {
                            y = 999999;
                            break;
                        }
                    }
                    else
                    {
                        _Inimigo.CancelarPosicionamento();
                    }
                }
            }

            Desenhar();
            _Posicionar = _Inimigo.FaltaPosicionar;
        }

        private void TerminarRodada()
        {
            //_MinhaRodada = false;
        }

        public void SetMinhaRodada()
        {
            _MinhaRodada = true;
        }

        
    }
}
