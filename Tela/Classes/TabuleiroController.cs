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
        private bool _PartidaIniciada = false;
        private bool _PartidaConluida = false;
        private bool _Vitoria = false;
        private bool _MinhaRodada = false;
        private bool _InimigoEncontrado = false;
        private bool _PosicionandoPecas = true;

        private delegate void _AtualizarTabuleiroCallback();

        private SerialController _Serial;
        private PanelController _PanelController;

        private PosicaoController _Amigo = new PosicaoController();
        private PosicaoController _Inimigo = new PosicaoController();

        private PosicaoMovimentosController _Movimentos;

        private MyPanel _PosicionamentoPanel;
        private MyPanel _MovimentoPanel;

        private List<MyPanel> _PanelsPodeMover = new List<MyPanel>();
        public FormBase _Form;

        private Posicao[] _Aguas = new Posicao[] {
            new Posicao(2, 4),
            new Posicao(3, 4),
            new Posicao(6, 4),
            new Posicao(7, 4),
            new Posicao(2, 5),
            new Posicao(3, 5),
            new Posicao(6, 5),
            new Posicao(7, 5)
        };

        public FormBase Form { get { return _Form; } }

        public PosicaoController Amigo { get { return _Amigo; } }
        public PosicaoController Inimigo { get { return _Inimigo; } }
        public Posicao[] Obstaculos { get { return _Aguas; } }

        public TabuleiroController(FormBase form, Button btnStart)
        {
            this._Form = form;
            this._Amigo.SetPosicoesPosicionamento();
            this._PanelController = new PanelController(form);
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
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    if (!_Aguas.Any(p => p.Compare(x, y)))
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
                AtualizarTabuleiroSmart();
                /*
                var panels = _PanelController.PanelsTabuleiro;

                for (var x = 0; x < Principal.Quadrados; x++)
                {
                    for (var y = 0; y < Principal.Quadrados; y++)
                    {
                        var info = panels.Where(p => p.Posicao.Compare(x, y)).FirstOrDefault();

                        if (info.Inimigo)
                        {
                            info.Panel.BackgroundImage = Properties.Resources.background;
                            info.Panel.BorderStyle = BorderStyle.FixedSingle;
                        }
                        else if (info.Peca != null)
                        {
                            info.Panel.BackgroundImage = info.Peca.Image;
                            info.Panel.BorderStyle = BorderStyle.None;
                        }
                        else if (_Aguas.Any(p => p.Compare(x, y)))
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
                */
            }
        }

        private void AtualizarTabuleiroSmart()
        {
            var panels = _PanelController.PanelsModificados();
            foreach (var info in panels)
            {
                if (info.Inimigo)
                {
                    info.Panel.BackgroundImage = Properties.Resources.background;
                    info.Panel.BorderStyle = BorderStyle.None;
                }
                else if (info.Peca != null)
                {
                    info.Panel.BackgroundImage = info.Peca.Image;
                    info.Panel.BorderStyle = BorderStyle.None;
                }
                else if (_Aguas.Any(p => p.Compare(info.Posicao)))
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

        private void MostrarOndePosicionar()
        {
            var posicoes = _Amigo.PosicoesValidasPosicionamento;
            foreach (var posicaoValida in posicoes)
            {
                MyPanel panel = _PanelController.GetPanelTabuleiro(posicaoValida);
                panel.Animar();
            }
        }

        private void EsconderOndePosicionar()
        {
            var posicoes = _Amigo.PosicoesValidasPosicionamento;
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

                if (panel.Inimigo || posicao.Move)
                {
                    _PanelsPodeMover.Add(panel);
                    panel.Animar();
                }
            }
        }

        private void EsconderOndeMover(_PanelPosicionamento info)
        {
            foreach (var panel in _PanelsPodeMover)
            {
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
                if (_Amigo.PosicionandoPeca)
                {
                    var info = _PanelController.GetTabuleiroInfoByGuid(panel.Guid);
                    var retorno = _Amigo.SetPosicaoPeca(_Amigo.PecaEmPosicionamento, info.Posicao.X, info.Posicao.Y);
                    if (retorno.Sucesso)
                    {
                        EsconderOndePosicionar();
                        _PanelController.SetPecaTabuleiro(panel.Guid, _Amigo.PecaEmPosicionamento);
                        _Amigo.TerminarPosicionamento(info.Posicao);
                        _PosicionandoPecas = _Amigo.PosicionandoPecas;

                        Desenhar();
                        AdicionarPosicionamento(info.Posicao, info.Peca);
                    }
                    else
                    {
                        CustomMessageBox.ShowMessageBoxAsync(retorno.Mensagem);
                    }
                }
            }
            else if (_MinhaRodada && _PartidaIniciada)
            {
                var infoNova = _PanelController.GetPecaMovimentoInfo(panel.Guid);
                if (!_Amigo.MovimentandoPeca && infoNova.Peca != null)
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
                            var status = infoAntiga.Peca.Atacar(inimigo);
                            switch (status)
                            {
                                case DueloEnum.Vitoria:
                                    MatarInimigo(infoAntiga, infoNova);
                                    break;
                                case DueloEnum.Derrota:
                                    MatarMinhaPeca(infoAntiga, infoNova);
                                    break;
                                case DueloEnum.Empate:
                                    MatarAmbasPeca(infoAntiga, infoNova);
                                    break;
                                case DueloEnum.VenceuJogo:
                                    MatarInimigo(infoAntiga, infoNova);
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
                        _MinhaRodada = false;
                        AtualizarTabuleiro();
                    }
                    else
                    {
                        CustomMessageBox.ShowMessageBoxAsync(retorno.Mensagem);
                    }
                }
            }
        }

        private void Escolha_Click(object sender, EventArgs e)
        {
            var panel = (MyPanel)sender;
            var peca = _PanelController.GetPecaPosicionamento(panel.Guid);

            if (!_Amigo.PosicionandoPeca)
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
                    var retorno = _Amigo.SetPosicaoPeca(_Amigo.PecaEmPosicionamento, x, y);
                    if (retorno.Sucesso)
                    {
                        var info = _PanelController.GetPanelInfoTabuleiro(x, y);
                        _PanelController.SetPecaTabuleiro(info.Panel.Guid, _Amigo.PecaEmPosicionamento);
                        _Amigo.TerminarPosicionamento(info.Posicao);
                        AdicionarPosicionamento(info.Posicao, info.Peca);
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
            _PosicionandoPecas = _Amigo.PosicionandoPecas;
        }

        private void AdicionarPosicionamento(Posicao posicao, Peca peca)
        {
            _Serial.EnviarPosicionamento(posicao.Inverter(), peca);
            if (_Inimigo.PecasPosicionadas && _Amigo.PecasPosicionadas)
            {
                Form.AtivarBtnIniciar();
            }
        }

        private void TerminarPosicionamento()
        {
            _Serial.TerminarPosicionamento();
        }

        private void EspiarPeca(_PanelPosicionamento espia, _PanelPosicionamento espiada)
        {
            _Amigo.CancelarMovimento();
            _Serial.EspiarPeca(espia.Posicao.Inverter(), espiada.Posicao.Inverter());

            CustomMessageBox.ShowMessageBoxAsync(string.Format(
                "Informações da peça {0}: {1}",
                espiada.Posicao.ToPosicaoTabuleiro().GetInfo(),
                espiada.Peca.GetInfo()
            ));
        }

        private void TerminarRodada(Posicao posicaoAntiga, Posicao posicaoNova, Peca peca)
        {
            _Serial.EnviarMovimento(posicaoAntiga.Inverter(), posicaoNova.Inverter(), peca);
        }

        private void MatarInimigo(_PanelPosicionamento infoAmigo, _PanelPosicionamento infoInimigo)
        {
            _PanelController.MatarPeca(infoInimigo.Posicao);
            _PanelController.MovimentarPeca(infoAmigo, infoInimigo);
            _Amigo.TerminarMovimento(infoAmigo.Posicao);
            _Inimigo.MatarPeca(infoInimigo.Posicao);

            _Serial.MatarInimigo(infoInimigo.Posicao.Inverter(), infoAmigo.Posicao.Inverter());

            CustomMessageBox.ShowMessageBoxAsync(string.Format(
                "Sua peça matou a peça {0}({1})",
                infoInimigo.Peca.GetInfo(),
                infoInimigo.Posicao.ToPosicaoTabuleiro().GetInfo()
            ));
        }

        private void MatarMinhaPeca(_PanelPosicionamento infoAmigo, _PanelPosicionamento infoInimigo)
        {            
            _Amigo.CancelarMovimento();
            _Amigo.MatarPeca(infoAmigo.Posicao);
            _Amigo.MatarPeca(infoInimigo.Posicao);
            _PanelController.MatarPeca(infoAmigo.Posicao);
            _Serial.MatarMinhaPeca(infoAmigo.Posicao.Inverter(), infoInimigo.Posicao.Inverter());

            CustomMessageBox.ShowMessageBoxAsync(string.Format(
                "Sua peça morreu ao atacar a peça {0}({1})",
                infoInimigo.Peca.GetInfo(),
                infoInimigo.Posicao.ToPosicaoTabuleiro().GetInfo()
            ));
        }

        private void MatarAmbasPeca(_PanelPosicionamento amigo, _PanelPosicionamento inimigo)
        {
            var pecaAmiga = _Amigo.GetPecaByPosicao(amigo.Posicao);
            var pecaInimiga = _Inimigo.GetPecaByPosicao(inimigo.Posicao);

            _PanelController.MatarPeca(amigo.Posicao);
            _PanelController.MatarPeca(inimigo.Posicao);
            _Amigo.CancelarMovimento();
            _Amigo.MatarPeca(amigo.Posicao);
            _Amigo.MatarPeca(inimigo.Posicao);
            _Inimigo.MatarPeca(inimigo.Posicao);

            _Serial.MatarAmbasPecas(amigo.Posicao.Inverter(), inimigo.Posicao.Inverter());

            CustomMessageBox.ShowMessageBoxAsync(string.Format(
                "Ambas as peças morreram no ataque\nVocê: {0}({1})\nInimigo: {2}({3})",
                pecaAmiga.GetInfo(),
                amigo.Posicao.ToPosicaoTabuleiro().GetInfo(),
                pecaInimiga.GetInfo(),
                inimigo.Posicao.ToPosicaoTabuleiro().GetInfo()
            ));
        }

        private void DeclararVitoria()
        {
            _PartidaIniciada = false;
            _PartidaConluida = true;
            _Vitoria = true;
            _Serial.DeclararVitoria();

            CustomMessageBox.ShowMessageBoxAsync("Você venceu a partida!");
        }

        public void IniciarPartida()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100);
            _MinhaRodada = (randomNumber % 2) == 0;
            _PartidaIniciada = true;
            _Serial.IniciarPartida(_MinhaRodada);

            if (_MinhaRodada)
            {
                CustomMessageBox.ShowMessageBoxAsync("Você inicia o jogo.");
            }
            else
            {
                CustomMessageBox.ShowMessageBoxAsync("O inimigo inicia o jogo.");
            }
        }

        public void SetMinhaRodada()
        {
            _MinhaRodada = true;
            _PartidaIniciada = true;
        }

        public void SetSerialController(SerialController sc)
        {
            _Serial = sc;
            _Serial.SetEvents(this);
        }

        #region Events

        public override void UpdateStatus_Received()
        {
            if (_PartidaConluida && _Vitoria)
            {
                Form.UpdateStatus("Vitória");
            }
            else if (_PartidaConluida && !_Vitoria)
            {
                Form.UpdateStatus("Derrota");
            }
            else if (_Amigo.PosicionandoPecas)
            {
                Form.UpdateStatus("Posicionando peças");
            }
            else if (_Amigo.PecasPosicionadas && _Inimigo.PosicionandoPecas)
            {
                Form.UpdateStatus("Aguardando outro player");
            }
            else if (!_PartidaIniciada)
            {
                Form.UpdateStatus("Aguardando inicio da partida");
            }
            else if (_MinhaRodada)
            {
                Form.UpdateStatus("Sua rodada");
            }
            else if (!_MinhaRodada)
            {
                Form.UpdateStatus("Rodada do inimigo");
            }            
            else
            {
                Form.UpdateStatus("Deu merda");
            }
        }

        public override void PosicionarInimigo_Received(Posicao posicao, Peca peca)
        {
            _Inimigo.IniciarPosicionamento(peca);
            var retorno = _Inimigo.SetPosicaoPecaInimigo(peca, posicao.X, posicao.Y);
            if (retorno.Sucesso)
            {
                _PanelController.SetPecaTabuleiroInimigo(posicao, peca);
                _Inimigo.TerminarPosicionamento(posicao);
            }
            else
            {
                CustomMessageBox.ShowMessageBoxAsync("Algo deu muito errado!!!1!eleven!!");
            }

            if (_Inimigo.PecasPosicionadas && _Amigo.PecasPosicionadas)
            {
                Form.AtivarBtnIniciar();
            }
        }

        public override void MovimentarInimigo_Received(Posicao posicaoAntiga, Posicao posicaoNova, Peca peca)
        {
            var info = _PanelController.GetPanelInfoTabuleiro(posicaoAntiga.X, posicaoAntiga.Y);
            _Inimigo.IniciarMovimento(peca);
            var retorno = _Inimigo.SetPosicaoPecaMovimentoInimiga(info, posicaoNova);
            if (retorno.Sucesso)
            {
                _PanelController.MovimentarPecaInimigo(info, posicaoNova);
                _Inimigo.TerminarMovimento(posicaoAntiga);
                _MinhaRodada = true;
            }
            else
            {
                CustomMessageBox.ShowMessageBoxAsync("Algo deu muito errado!!!1!eleven!!");
            }
        }

        public override void MatarPecaInimiga_Received(Posicao inimigo, Posicao amigo)
        {
            var pecaAmiga = _Amigo.GetPecaByPosicao(amigo);
            var pecaInimiga = _Inimigo.GetPecaByPosicao(inimigo);

            _PanelController.MatarPeca(inimigo);
            _Inimigo.MatarPeca(inimigo);

            CustomMessageBox.ShowMessageBoxAsync(string.Format(
                "Sua peça {0} matou a peça {1}({2}).",
                pecaAmiga.Nome,
                pecaInimiga.Nome,
                inimigo.ToPosicaoTabuleiro().GetInfo()
            ));
        }

        public override void MatarPecaAmiga_Received(Posicao amigo, Posicao inimigo)
        {
            var pecaAmiga = _Amigo.GetPecaByPosicao(amigo);
            var pecaInimiga = _Inimigo.GetPecaByPosicao(inimigo);

            _PanelController.MatarPeca(amigo);
            _Amigo.MatarPeca(amigo);

            var info = _PanelController.GetPanelInfoTabuleiro(inimigo);
            _Inimigo.IniciarMovimento(info.Peca);
            var retorno = _Inimigo.SetPosicaoPecaMovimentoInimiga(info, amigo);
            if (retorno.Sucesso)
            {
                _PanelController.MovimentarPecaInimigo(info, amigo);
                _Inimigo.TerminarMovimento(inimigo);
            }
            else
            {
                CustomMessageBox.ShowMessageBoxAsync("Algo deu muito errado!!!1!eleven!!");
            }

            CustomMessageBox.ShowMessageBoxAsync(string.Format(
                "Sua peça {0}({1}) foi morta pela peça {2}.",
                pecaAmiga.Nome,
                amigo.ToPosicaoTabuleiro().GetInfo(),
                pecaInimiga.Nome
            ));
        }

        public override void EspiarPeca_Received(Posicao posicaoEspia, Posicao posicaoEspiada)
        {
            CustomMessageBox.ShowMessageBoxAsync(string.Format(
                "A peca da posição {0} acabou de espiar sua peça({1}).",
                posicaoEspia.ToPosicaoTabuleiro().GetInfo(),
                posicaoEspiada.ToPosicaoTabuleiro().GetInfo()
            ));
        }

        public override void IniciarPartida_Received(bool minhaRodada = true)
        {
            _MinhaRodada = minhaRodada;
            _PartidaIniciada = true;
            if (_MinhaRodada)
            {
                CustomMessageBox.ShowMessageBoxAsync("Você inicia o jogo.");
            }
            else
            {
                CustomMessageBox.ShowMessageBoxAsync("O inimigo inicia o jogo.");
            }
            Form.DesativarBtnIniciar();
        }

        public override void SuaVez_Received()
        {
            _MinhaRodada = true;
        }

        public override void DeclararDerrota_Received()
        {
            _PartidaIniciada = false;
            _PartidaConluida = true;
            _Vitoria = false;

            CustomMessageBox.ShowMessageBoxAsync("Você foi derrotado!");
        }

        public override void AtualizarTabuleiro_Received()
        {
            AtualizarTabuleiro();
        }

        public override void MatarAmbasPeca_Received(Posicao amigo, Posicao inimigo)
        {
            var pecaAmiga = _Amigo.GetPecaByPosicao(amigo);
            var pecaInimiga = _Inimigo.GetPecaByPosicao(inimigo);

            _PanelController.MatarPeca(amigo);
            _PanelController.MatarPeca(inimigo);
            _Amigo.MatarPeca(amigo);
            _Inimigo.MatarPeca(inimigo);

            CustomMessageBox.ShowMessageBoxAsync(string.Format(
                "O inimigo da posição {0}({1}) atacou sua peça {2}({3}) e ambas morreram ='(",
                pecaInimiga.Nome,
                inimigo.ToPosicaoTabuleiro().GetInfo(),
                pecaAmiga.Nome,
                amigo.ToPosicaoTabuleiro().GetInfo()
            ));
        }

        public override void UpdateStatusSerial_Received()
        {
            if (_Serial.IsOpen && _Serial.InimigoEncontrado)
            {
                    Form.UpdateStatusPort("Porta Serial Aberta - Inimigo Encontrado.");
                    _InimigoEncontrado = true;
            }
            else if (_Serial.IsOpen && !_Serial.InimigoEncontrado)
            {
                Form.UpdateStatusPort("Porta Serial Aberta - Inimigo Não Encontrado.");
                if (_InimigoEncontrado)
                {
                    CustomMessageBox.ShowMessageBoxAsync("O inimigo saiu =(");
                }
                _InimigoEncontrado = false;                
            }
            else
            {
                Form.UpdateStatusPort("Porta Serial Fechada.");
            }
        }

        #endregion
    }
}
