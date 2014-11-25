using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Tela.Enums;


namespace Tela.Classes
{
    public class TabuleiroController : TabuleiroBase
    {
        private bool _MinhaRodada = false;

        private Color _Background = Color.DarkGray;
        private Color _BackgroundPosicionar = Color.LightGreen;
        private Color _BackgroundInimigo = Color.Red;

        private delegate void _AtualizarTabuleiroCallback();

        private SerialController _Serial;

        private PanelController _PanelController;
                   
        private PosicaoController _Amigo = new PosicaoController();
        private PosicaoController _Inimigo = new PosicaoController();

        private PosicaoMovimentosController _Movimentos;

        private bool _PosicionandoPecas = true;
        private bool _PosicionamentoOK = false;

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
        

        public PosicaoController Amigo { get { return _Amigo; } }
        public PosicaoController Inimigo { get { return _Inimigo; } }
        public Posicao[] Obstaculos { get { return _Aguas; } }
        
        public TabuleiroController(Form form)
        {
            this.Form = form;            
            this._Amigo.SetPosicoesPosicionamento();
            this._PanelController = new PanelController(form, _Background);
            this.Form.Controls.Add(this._PanelController.PanelTabuleiroParent);
            this.Form.Controls.Add(this._PanelController.PanelPosicionarParent);
            this._Movimentos = new PosicaoMovimentosController(this);
            DesenharTabulerio();
        }

        public void Desenhar()
        {
            DesenharPosicionamento();
            AtualizarTabuleiro();
        }

        private void DesenharTabulerio()
        {
            LimparTabuleiro();
            var posicoes = _Amigo.Posicoes;

            for (var x = 0; x < Principal.Quadrados; x++)
            {
                for (var y = 0; y < Principal.Quadrados; y++)
                {                    
                    var newPanel = new MyPanel
                    {
                        Location = new Point(Principal.TamanhoQuadrado * x, Principal.TamanhoQuadrado * y),
                        Size = new Size(Principal.TamanhoQuadrado, Principal.TamanhoQuadrado),                        
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
            if (Form.InvokeRequired)
            {
                _AtualizarTabuleiroCallback d = new _AtualizarTabuleiroCallback(AtualizarTabuleiro);
                Form.Invoke(d, new object[] { });
            }
            else
            {
                var panels = _PanelController.PanelsTabuleiro;

                for (var x = 0; x < Principal.Quadrados; x++)
                {
                    for (var y = 0; y < Principal.Quadrados; y++)
                    {
                        var info = panels.Where(p => p.Posicao.X == x && p.Posicao.Y == y).FirstOrDefault();

                        if (info.Inimigo)
                        {
                            info.Panel.BackgroundImage = null;
                            info.Panel.BorderStyle = BorderStyle.None;
                            info.Panel.BackColor = Color.Black;
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
                    Size = new Size(Principal.TamanhoQuadrado, Principal.TamanhoQuadrado),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    BackgroundImage = peca.Image,
                    Location = new Point(Principal.TamanhoQuadrado * i, 0)
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
                    panel.BackColor = _Background;
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
            if (_PosicionandoPecas)
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
                        _PosicionandoPecas = _Amigo.FaltaPosicionar;
                        
                        Desenhar();
                        AdicionarPosicionamento(info.Posicao, info.Peca);
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
                    if (infoNova.Peca.Anda && !panel.Inimigo)
                    {
                        _MovimentoPanel = panel;
                        _Amigo.IniciarMovimento(infoNova.Peca);
                        panel.BorderStyle = BorderStyle.FixedSingle;
                        MostrarOndeMover(infoNova);
                    }
                }
                else if (infoNova.Peca != null && !panel.Inimigo)
                {
                    _Amigo.CancelarMovimento();
                    _MovimentoPanel.BorderStyle = BorderStyle.None;
                    EsconderOndeMover(infoNova);
                    _MovimentoPanel = null;
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
                        EsconderOndeMover(infoNova);

                        var inimigo = _Inimigo.GetPecaByPosicao(infoNova.Posicao.X, infoNova.Posicao.Y);
                        if (inimigo != null)
                        {
                            var status = Duelo(infoAntiga.Peca, inimigo);
                            switch (status)
                            {
                                case DueloEnum.Vitoria:
                                    MatarInimigo(infoAntiga, infoNova, infoNova.Posicao);
                                    break;
                                case DueloEnum.Derrota:
                                    MatarMinhaPeca(infoAntiga.Posicao);
                                    break;
                                case DueloEnum.Empate:
                                    MatarInimigo(infoAntiga, infoNova, infoNova.Posicao);
                                    MatarMinhaPeca(infoNova.Posicao);
                                    break;
                                case DueloEnum.VenceuJogo:
                                    MatarInimigo(infoAntiga, infoNova, infoNova.Posicao);
                                    DeclararVitoria();
                                    break;
                                default://Espiao
                                    EspiarPeca(infoAntiga, infoNova);
                                    break;
                            }
                        }
                        else
                        {
                            _PanelController.MovimentarPeca(infoAntiga, infoNova);
                            _Amigo.TerminarMovimento(infoAntiga.Posicao);                            
                            TerminarRodada(infoAntiga.Posicao, infoNova.Posicao, infoNova.Peca);
                        }
                        AtualizarTabuleiro();
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
            for (int x = Principal.Quadrados - 1; x >= 0; x--)
            {
                for (int y = Principal.Quadrados - 1; y >= 0; y--)
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
            _PosicionandoPecas = _Amigo.FaltaPosicionar;
            //PosicionarInimigos();
        }

        public void PosicionarInimigosTeste()
        {
            throw new NotImplementedException();

            Peca peca;
            int faltantes = _Inimigo.APosicionar.Count() - 1;
            for (int y = 0; y < Principal.Quadrados; y++)
            {
                for (int x = Principal.Quadrados - 1; x >= 0; x--)
                {
                    peca = _Inimigo.PecasPosicionamento.ElementAt(faltantes);
                    _Inimigo.IniciarPosicionamento(peca);
                    var retorno = _Inimigo.SetPosicaoPecaInimigo(_Inimigo.PecaPosicionando, x, y);
                    if (retorno.Sucesso)
                    {
                        var info = _PanelController.GetPanelInfoTabuleiro(x, y);
                        //_PanelController.SetPecaTabuleiro(info.Panel.Guid, _Inimigo.PecaPosicionando, true);
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
            _PosicionandoPecas = _Inimigo.FaltaPosicionar;
        }

        private void AdicionarPosicionamento(Posicao posicao, Peca peca)
        {            
            _Serial.EnviarPosicionamento(posicao.ToEnemy(), peca);
            if (_Amigo.APosicionar.Count() > 0)
            {
                Principal.UpdateStatus("Posicionando");
            }
            else if (_Amigo.APosicionar.Count() > 0)
            {
                Principal.UpdateStatus("Aguardando outro player");
            }
            else
            {
                Principal.UpdateStatus("Aguardando inicio da partida");
            }
        }

        private void TerminarPosicionamento()
        {            
            _Serial.TerminarPosicionamento();
        }

        private void EspiarPeca(_PanelPosicionamento espia, _PanelPosicionamento espiada)
        {
            _Amigo.CancelarMovimento();
            _Serial.EspiarPeca(espia.Posicao.ToEnemy(), espiada.Posicao.ToEnemy());
            //_MinhaRodada = false;

            MessageBox.Show(string.Format(
                "({0}){1}", 
                espiada.Posicao.ToPosicaoTabuleiro().ToInfo(),
                espiada.Peca.GetInfo()
            ));
        }

        private void TerminarRodada(Posicao posicaoAntiga, Posicao posicaoNova, Peca peca)
        {
            _Serial.EnviarMovimento(posicaoAntiga.ToEnemy(), posicaoNova.ToEnemy(), peca);
            //_MinhaRodada = false;
        }

        public void MatarInimigo(_PanelPosicionamento infoAntiga, _PanelPosicionamento infoNova, Posicao posicao)
        {
            _PanelController.MatarPeca(posicao);
            _PanelController.MovimentarPeca(infoAntiga, infoNova);
            _Amigo.TerminarMovimento(infoAntiga.Posicao);
            _Inimigo.MatarPeca(posicao);

            _Serial.MatarInimigo(posicao);
        }

        public void MatarMinhaPeca(Posicao posicao)
        {
            _Amigo.CancelarMovimento();
            _PanelController.MatarPeca(posicao);
            _Serial.MatarMinhaPeca(posicao);
        }

        public void DeclararVitoria()
        {
            _Serial.DeclararVitoria();
        }

        public void SetMinhaRodada()
        {
            _MinhaRodada = true;
        }

        public void SetSerialController(SerialController sc)
        {
            _Serial = sc;
            _Serial.SetEvents(this);
        }

        private DueloEnum Duelo(Peca atacando, Peca defendendo)
        {
            if (defendendo.Bandeira)
            {
                return DueloEnum.VenceuJogo;
            }
            else if (defendendo.Bomba)
            {
                return DueloEnum.Derrota;
            }
            else if (atacando.IsSpy)
            {
                return DueloEnum.Espiao;
            }
            else if (atacando.Forca < defendendo.Forca)
            {
                return DueloEnum.Derrota;
            }
            else if (atacando.Forca > defendendo.Forca)
            {
                return DueloEnum.Vitoria;
            }
            else
            {
                return DueloEnum.Empate;
            }            
        }

        public override void PosicionarInimigo(Posicao posicao, Peca peca)
        {
            _Inimigo.IniciarPosicionamento(peca);
            var retorno = _Inimigo.SetPosicaoPecaInimigo(peca, posicao.X, posicao.Y);
            if (retorno.Sucesso)
            {
                _PanelController.SetPecaTabuleiroInimigo(posicao, peca);
                _Inimigo.TerminarPosicionamento(posicao);

                AtualizarTabuleiro();
            }
            else
            {
                MessageBox.Show("Algo deu muito errado!!!1!eleven!!");
            }            
        }

        public override void MovimentarInimigo(Posicao posicaoAntiga, Posicao posicaoNova, Peca peca)
        {
            var info = _PanelController.GetPanelInfoTabuleiro(posicaoAntiga.X, posicaoAntiga.Y);
            _Inimigo.IniciarMovimento(peca);
            var retorno = _Inimigo.SetPosicaoPecaMovimentoInimiga(info, posicaoNova);
            if (retorno.Sucesso)
            {
                _PanelController.MovimentarPecaInimigo(info, posicaoNova);
                _Amigo.TerminarMovimento(posicaoAntiga);

                AtualizarTabuleiro();
                _MinhaRodada = true;
            }
            else
            {
                MessageBox.Show("Algo deu muito errado!!!1!eleven!!");
            } 
        }

        public override void MatarPecaInimiga(Posicao posicao)
        {
            _PanelController.MatarPeca(posicao);
            _Inimigo.MatarPeca(posicao);
            AtualizarTabuleiro();
        }

        public override void MatarPecaAmiga(Posicao posicao)
        {
            _PanelController.MatarPeca(posicao);
            _Amigo.MatarPeca(posicao);
            AtualizarTabuleiro();
        }

        public override void EspiarPeca(Posicao posicaoEspia, Posicao posicaoEspiada)
        {
            MessageBox.Show(string.Format(
                "A peca da posição {0} acabou de espiar sua peça({1}).",
                posicaoEspia.ToPosicaoTabuleiro().ToInfo(),
                posicaoEspiada.ToPosicaoTabuleiro().ToInfo()
            ));
        }
    }
}
