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
    public partial class Principal : Form
    {
        const int LINHAS_COLUNAS = 10;
        Quadrado.Cores MinhaCor = Quadrado.Cores.Branco;
        Borda IndicacaoMinhaCor;

        System.Windows.Forms.Timer tmrProcuraAdversario = new System.Windows.Forms.Timer();

        bool ControleMinhaVez = true;
        bool Ganhou = false;

        private TabuleiroController _Tabuleiro { get; set; }

        public Principal()
        {
            InitializeComponent();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            _Tabuleiro = new TabuleiroController(this, 40, 10, Color.DarkGray);            
            DesenhaTabuleiro();

            PortaSerial.PortName = "COM1";
            PortaSerial.BaudRate = 9600;
            PortaSerial.DataBits = 8;
            PortaSerial.Parity = System.IO.Ports.Parity.None;
            PortaSerial.StopBits = System.IO.Ports.StopBits.One;

            try
            {

                PortaSerial.Open();
                tmrProcuraAdversario.Interval = 1000;
                tmrProcuraAdversario.Tick += new EventHandler(tmrProcuraAdversario_Tick);
                tmrProcuraAdversario.Start();
                lblInformacoes.Text = "Aguardando jogador...";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void tmrProcuraAdversario_Tick(object sender, EventArgs e)
        {
            //#01 - JOGADOR 1 CHAMANDO
            PortaSerial.WriteLine("JOGADOR1");
        }

        #region Tabuleiro
        private Panel[,] _tabuleiro;
        private void DesenhaTabuleiro()
        {
            _Tabuleiro.Desenhar();
        }

        void Img_Click(object sender, EventArgs e)
        {
            Quadrado Img = ((Quadrado)sender);
            if (!Ganhou)
            {
                if (ControleMinhaVez)
                {
                    if (Img.Cor == Quadrado.Cores.Branco)
                    {
                        //Img.BackColor = MinhaCor;
                        Img.Cor = MinhaCor;

                        ControleMinhaVez = false;
                        SetText("Vez do outro jogador");
                        PortaSerial.WriteLine("JOGADA_" + Img.Pos_X + "#" + Img.Pos_Y);

                        VerificaVencedor(MinhaCor);

                    }
                    else
                    {
                        // System.Media.SystemSounds.Beep.Play();
                        MessageBox.Show("Não pode!");
                    }
                }
                else
                {
                    //System.Media.SystemSounds.Beep.Play();
                    MessageBox.Show("Não é sua vez!");
                }
            }
            else
            {
                //System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("Jogo terminou!");
            }
        }

        private void VerificaVencedor(Quadrado.Cores cor)
        {
            List<Quadrado> marcados = (from x in this.Controls.OfType<Quadrado>()
                                       where x.Cor == cor
                                       select x).ToList();

            if (marcados.Count < LINHAS_COLUNAS)
            {
                return;
            }
            if (cor == Quadrado.Cores.Azul)
            {
                if (!marcados.Exists(m => m.Pos_X == 0) && !marcados.Exists(m => m.Pos_X == LINHAS_COLUNAS - 1))
                {
                    return;
                }
            }
            else
            {
                if (!marcados.Exists(m => m.Pos_Y == 0) && !marcados.Exists(m => m.Pos_Y == LINHAS_COLUNAS - 1))
                {
                    return;
                }
            }
    
            List<Quadrado> caminho = new List<Quadrado>();

            foreach (var item in (from x in marcados 
                                  where (x.Pos_X == 0 && x.Cor == Quadrado.Cores.Azul) ||
                                        (x.Pos_Y == 0 && x.Cor == Quadrado.Cores.Rosa)
                                  select x))
            {
                caminho.Add(item);

                VerificaCaminho(ref caminho, marcados, cor);

                if (Ganhou)
                {
                    Thread.Sleep(100);
                    PortaSerial.WriteLine("GANHEI");
                    SetText("Fim de jogo.");
                    MessageBox.Show("Uhuuuuuuulll.. Você ganhooou!!");
                }
                else
                {
                    caminho.Remove(item);
                }
            }
        }

        private void VerificaCaminho(ref List<Quadrado> caminho, List<Quadrado> marcados, Quadrado.Cores cor)
        {
            Quadrado atual = caminho.Last();

            if ((atual.Pos_X == LINHAS_COLUNAS - 1 && atual.Cor == Quadrado.Cores.Azul) || (atual.Pos_Y == LINHAS_COLUNAS - 1 && atual.Cor == Quadrado.Cores.Rosa))
            {
                Ganhou = true;
                return;
            }

            foreach (var item in (from x in marcados
                                  where x.Pos_X == atual.Pos_X - 1 && x.Pos_Y == atual.Pos_Y //a
                                  || x.Pos_X == atual.Pos_X - 1 && x.Pos_Y == atual.Pos_Y + 1 //b
                                  || x.Pos_X == atual.Pos_X && x.Pos_Y == atual.Pos_Y + 1 //c
                                  || x.Pos_X == atual.Pos_X + 1 && x.Pos_Y == atual.Pos_Y //d
                                  || x.Pos_X == atual.Pos_X + 1 && x.Pos_Y == atual.Pos_Y - 1 //e
                                  || x.Pos_X == atual.Pos_X && x.Pos_Y == atual.Pos_Y - 1 //f
                                  select x).Except(caminho))
            {
                caminho.Add(item);

                VerificaCaminho(ref caminho, marcados, cor);

                if (Ganhou)
                {
                    return;
                }
                else
                {
                    caminho.Remove(item);
                }
            }
        }
        #endregion

        #region Serial
        private void PortaSerial_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(70);

            string Dado = PortaSerial.ReadExisting();
            Dado = Dado.Replace("\n", "");

            //#01 - JOGADOR 1 CHAMANDO - Jogador 2 responde
            if (Dado == "JOGADOR1")
            {
                MinhaCor = Quadrado.Cores.Rosa;
                IndicacaoMinhaCor.Cor = Borda.Cores.Rosa;
                SetVisibilidade(true);
                PortaSerial.WriteLine("JOGADOR2");
                SetText("Vez do outro jogador");
            }

            //#02 - JOGADOR 2 RESPONDENDO - Jogador 1 inicia o jogo
            if (Dado == "JOGADOR2")
            {
                MinhaCor = Quadrado.Cores.Azul;
                IndicacaoMinhaCor.Cor = Borda.Cores.Azul;
                SetVisibilidade(true);
                ControleMinhaVez = true;
                tmrProcuraAdversario.Stop();
                SetText("Minha vez");
            }

            //#03 - JOGADOR RECEBENDO UMA JOGADA DO OUTRO
            if (Dado.StartsWith("JOGADA"))
            {
                int Pos_X = int.Parse(Dado.Split('_').ToList()[1].Split('#').ToList()[0]);
                int Pos_Y = int.Parse(Dado.Split('_').ToList()[1].Split('#').ToList()[1]);
                
                ControleMinhaVez = true;
                SetText("Minha vez");

                Quadrado Img = (from x in this.Controls.OfType<Quadrado>()
                                where x.Pos_X == Pos_X && x.Pos_Y == Pos_Y
                                select x).First();

                Img.Cor = MinhaCor == Quadrado.Cores.Rosa ? Quadrado.Cores.Azul : Quadrado.Cores.Rosa;
            }

            //#04 - JOGADOR RECEBENDO AVISO QUE PERDEU
            if (Dado.StartsWith("GANHEI"))
            {
                ControleMinhaVez = false;
                SetText("Você perdeu :(");
                MessageBox.Show("Você perdeu :(");
            }

            //#05 - JOGADOR AVISO QUE O OUTRO PIPOCOU
            if (Dado.StartsWith("SAINDO"))
            {
                ControleMinhaVez = false;
                Ganhou = true;
                SetText("O outro jogador abandonou o jogo...");
                MessageBox.Show("O outro jogador abandonou o jogo...");
            }

            //#06 - CHAT
            if (Dado.StartsWith("CHAT"))
            {
                string Mensagem = Dado.Split('|')[1];
                SetTextChat("Adversario: " + Mensagem);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnviarMensagemChat();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                EnviarMensagemChat();
            }
        }

        private void EnviarMensagemChat()
        {
            if (textBox1.Text.Trim().Length > 0)
            {
                SetTextChat("Eu: " + textBox1.Text);
                PortaSerial.WriteLine("CHAT|" + textBox1.Text);
                textBox1.Text = "";
            }
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblInformacoes.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblInformacoes.Text = text;
            }
        }

        delegate void SetVisibilidadeCallback(bool visivel);

        private void SetVisibilidade(bool visivel)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.IndicacaoMinhaCor.InvokeRequired)
            {
                SetVisibilidadeCallback d = new SetVisibilidadeCallback(SetVisibilidade);
                this.Invoke(d, new object[] { visivel });
            }
            else
            {
                this.IndicacaoMinhaCor.Visible = visivel;
            }
        }

        delegate void SetTextChatCallback(string text);

        private void SetTextChat(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox2.InvokeRequired)
            {
                SetTextChatCallback d = new SetTextChatCallback(SetTextChat);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox2.Text = text + Environment.NewLine + this.textBox2.Text;
            }
        }

        private void Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
            PortaSerial.Write("SAINDO");
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            _Tabuleiro.PosicionarTeste();
        }
    }
}
