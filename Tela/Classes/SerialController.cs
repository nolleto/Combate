using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO.Ports;
using Newtonsoft.Json;
using Tela.Enums;
using System.Text.RegularExpressions;

namespace Tela.Classes
{
    public class SerialController : System.IO.Ports.SerialPort
    {
        private bool _InimigoEncontrado;
        private string _Dados = string.Empty;

        private System.Timers.Timer _Timer;

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
           
            Open();
            EnviarPacote(new SerialPacote());
        }

        public void SetEvents(TabuleiroBase tb)
        {
            _Events = tb;
        }

        public void TerminarPosicionamento()
        {
        }

        public void EnviarPosicionamento(Posicao posicao, Peca peca)
        {
            EnviarPacote(new SerialPacote()
            {
                Posicao = posicao,
                PecaEnum = peca.Type,
                Info = SerialPacoteEnum.Posicionamento
            });
        }

        public void EnviarMovimento(Posicao posicaoAntiga, Posicao posicaoNova, Peca peca)
        {
            EnviarPacote(new SerialPacote()
            {
                Posicao = posicaoAntiga,
                PosicaoAux = posicaoNova,
                PecaEnum = peca.Type,
                Info = SerialPacoteEnum.Movimento
            });
        }

        public void MatarInimigo(Posicao posicao)
        {
            EnviarPacote(new SerialPacote()
            {
                Posicao = posicao,
                Info = SerialPacoteEnum.Morte
            });
        }

        public void MatarMinhaPeca(Posicao posicao)
        {
            EnviarPacote(new SerialPacote()
            {
                Posicao = posicao,
                Info = SerialPacoteEnum.Morte,
                Inimgo = true
            });
        }

        public void EspiarPeca(Posicao posicaoEspia, Posicao posicaoEspiada)
        {
            EnviarPacote(new SerialPacote()
            {
                Posicao = posicaoEspia,
                PosicaoAux = posicaoEspiada,
                Info = SerialPacoteEnum.Espiao
            });
        }

        public void DeclararVitoria()
        {
            EnviarPacote(new SerialPacote()
            {
                Info = SerialPacoteEnum.Vitoria
            });
        }

        public void Saindo()
        {
            EnviarPacote(new SerialPacote()
            {
                Info = SerialPacoteEnum.Saindo
            });
        }

        private void OnDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            _PacoteRecebido = GetFromSerial();

            if (!_InimigoEncontrado && _PacoteRecebido != null)
            {
                //Inimigo encontrado
                _InimigoEncontrado = true;
            }
            else if (_PacoteRecebido != null && _Events != null)
            {
                //Tratamento
                switch (_PacoteRecebido.Info)
                {
                    case SerialPacoteEnum.Posicionamento:
                        _Events.PosicionarInimigo(_PacoteRecebido.Posicao, new Peca(_PacoteRecebido.PecaEnum));
                        break;
                    case SerialPacoteEnum.Movimento:
                        _Events.MovimentarInimigo(_PacoteRecebido.Posicao, _PacoteRecebido.PosicaoAux, new Peca(_PacoteRecebido.PecaEnum));
                        break;
                    case SerialPacoteEnum.Morte:
                        if (_PacoteRecebido.Inimgo)
                        {
                            _Events.MatarPecaInimiga(_PacoteRecebido.Posicao);
                        }
                        else
                        {
                            _Events.MatarPecaAmiga(_PacoteRecebido.Posicao);
                        }
                        break;
                    case SerialPacoteEnum.Saindo:
                        _InimigoEncontrado = false;
                        EnviarPacote(new SerialPacote());
                        break;
                    case SerialPacoteEnum.Vitoria:
                        //Você foi derrotado
                        break;
                    default://Espiao
                        _Events.EspiarPeca(_PacoteRecebido.Posicao, _PacoteRecebido.PosicaoAux);
                        break;
                }

                _PacoteRecebido = null;
            }
        }
        
        private SerialPacote GetFromSerial()
        {
            string dados = this.ReadExisting();
            if (dados.StartsWith(SerialPacote.INICIO))
            {
                _Dados = string.Empty;
            }
            _Dados = _Dados + dados;
            if (dados.Contains(SerialPacote.FIM))
            {
                _Dados = _Dados.Replace(SerialPacote.INICIO, "").Replace(SerialPacote.FIM, "");
                return SerialPacote.ConvertFromString(_Dados);
            }

            return null;
        }
        
        private void EnviarPacote(SerialPacote sp)
        {           
            _PacoteEnviado = sp;
            this.Write(sp.ToJsonString());
        }
    }
}
