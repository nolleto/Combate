using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tela.Classes
{
    public class FormBase : Form
    {
        private delegate void _UpdateStatusCallback(string status);
        private delegate void _UpdateStatusPortCallback(string status);
        private delegate void _AtivarIniciarCallback();
        private delegate void _DesativarBtnIniciarCallback();

        public Label LblStatus { get; set; }
        public Button BtnIniciar { get; set; }
        public Label LblPortStatus { get; set; }

        public void UpdateStatus(string status)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    _UpdateStatusCallback d = new _UpdateStatusCallback(UpdateStatus);
                    this.Invoke(d, new object[] { status });
                }
                catch (Exception)
                {

                }                
            }
            else
            {
                LblStatus.Text = string.Concat(
                    "Status: ",
                    status
                );
            }
        }

        public void AtivarBtnIniciar()
        {
            if (this.InvokeRequired)
            {
                _AtivarIniciarCallback d = new _AtivarIniciarCallback(AtivarBtnIniciar);
                this.Invoke(d, new object[] { });
            }
            else
            {
                BtnIniciar.Enabled = true;
            }
        }

        public void DesativarBtnIniciar()
        {
            if (this.InvokeRequired)
            {
                _DesativarBtnIniciarCallback d = new _DesativarBtnIniciarCallback(DesativarBtnIniciar);
                this.Invoke(d, new object[] { });
            }
            else
            {
                BtnIniciar.Enabled = false;
            }
        }

        public void UpdateStatusPort(string status)
        {
            if (this.InvokeRequired)
            {
                _UpdateStatusPortCallback d = new _UpdateStatusPortCallback(UpdateStatusPort);
                this.Invoke(d, new object[] { status });
            }
            else
            {
                LblPortStatus.Text = status;
            }
        }
    }
}
