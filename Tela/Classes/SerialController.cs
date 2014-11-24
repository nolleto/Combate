using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO.Ports;
using Newtonsoft.Json;
using Tela.Enums;

namespace Tela.Classes
{
    public class SerialController : System.IO.Ports.SerialPort
    {
        private bool _InimigoEncontrado;

        private TabuleiroBase _Events;
        private SerialPacote _PacoteEnviado;
        private SerialPacote _PacoteRecebido;

        public SerialController(System.ComponentModel.IContainer iContainer)
            : base(iContainer)
        {
            this.PortName = "COM1";
            this.BaudRate = 9600;
            this.DataBits = 8;
            this.Parity = System.IO.Ports.Parity.None;
            this.StopBits = System.IO.Ports.StopBits.One;
            this.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);

            //Open();
            //EnviarPacote(new SerialPacote());
        }

        public void TerminarPosicionamento()
        {
        }

        public void EnviarPosicionamento(Posicao posicao, Peca peca)
        {
        }

        public void EnviarMovimento(Posicao posicaoAntiga, Posicao posicaoNova, Peca peca)
        {
        }

        public void MatarInimigo(Posicao posicao)
        {
        }

        public void MatarMinhaPeca(Posicao posicao)
        {
        }

        public void DeclararVitoria()
        {
        }

        private void OnDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(70);

            string dados = this.ReadExisting();
            _PacoteRecebido = SerialPacote.ConvertFromString(dados);

            if (!_InimigoEncontrado && _PacoteRecebido != null)
            {
                //Inimigo encontrado
            }
            else if (_PacoteRecebido != null && _Events != null)
            {
                //Tratamento
                _PacoteRecebido = null;
            }
        }

        private void EnviarPacote(SerialPacote sp)
        {
            _PacoteEnviado = sp;
        }
    }
}
