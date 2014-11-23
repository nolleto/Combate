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
        private TabuleiroController _Tabuleiro;
        private System.Windows.Forms.Timer _Timer = new System.Windows.Forms.Timer();

        public TabuleiroController Tabuleiro { get { return _Tabuleiro; } }

        public SerialController(System.ComponentModel.IContainer iContainer)
            : base(iContainer)
        {
            this.PortName = "COM1";
            this.BaudRate = 9600;
            this.DataBits = 8;
            this.Parity = System.IO.Ports.Parity.None;
            this.StopBits = System.IO.Ports.StopBits.One;
            this.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);

            Open();
        }
    
        public void SetTabulerio(TabuleiroController tabuleiro)
        {
            this._Tabuleiro = tabuleiro;
        }

        private void IniciarProcuraPorAdversario()
        {
            _Timer.Interval = 1000;
            _Timer.Tick += new EventHandler(ProcuraAdversario_Event);
            _Timer.Start();
        }

        private void PararProcuraPorAdversario()
        {
        }

        private void ProcuraAdversario_Event(object sender, EventArgs e)
        {
        }

        private void OnDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(70);

            if (false && _Tabuleiro != null)
            {
                string Dado = this.ReadExisting();
                var pacote = SerialPacote.ConvertFromString(Dado);

                if (pacote != null)
                {
                }
            }
        }
    }
}
