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
        private string _Dados = string.Empty;
        private bool _InimigoEncontrado;

        private System.Timers.Timer _Timer;
        private System.Timers.Timer _TimerOpen;

        private TabuleiroBase _Events;
        private List<SerialPacote> _PacotesAEnviar = new List<SerialPacote>();
        private List<SerialPacote> _PacotesRecebido = new List<SerialPacote>();
        private SerialPacote _PacoteRecebido;

        public bool InimigoEncontrado { get { return _InimigoEncontrado; } }

        public SerialController(System.ComponentModel.IContainer iContainer)
            : base(iContainer)
        {
            this.PortName = "COM1";
            this.BaudRate = 9600;
            this.DataBits = 8;
            this.Parity = System.IO.Ports.Parity.None;
            this.StopBits = System.IO.Ports.StopBits.One;
            this.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);

            this._Timer = new System.Timers.Timer(500);
            this._Timer.Elapsed += new System.Timers.ElapsedEventHandler(_Timer_Elapsed);
            this._Timer.Start();

            this._TimerOpen = new System.Timers.Timer(500);
            this._TimerOpen.Elapsed += new System.Timers.ElapsedEventHandler(_TimerOpen_Elapsed);

            TryOpen();
        }

        public void TryOpen()
        {
            this._TimerOpen.Start();
        }

        public void SetEvents(TabuleiroBase tb)
        {
            _Events = tb;
            _Events.UpdateStatusSerial_Received();
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

        public void MatarInimigo(Posicao inimigo, Posicao amigo)
        {
            EnviarPacote(new SerialPacote()
            {
                Posicao = inimigo,
                PosicaoAux = amigo,
                Info = SerialPacoteEnum.Morte
            });
        }

        public void MatarMinhaPeca(Posicao amigo, Posicao inimigo)
        {
            EnviarPacote(new SerialPacote()
            {
                Posicao = amigo,
                PosicaoAux = inimigo,
                Info = SerialPacoteEnum.Morte,
                Inimgo = true
            });
        }

        public void MatarAmbasPecas(Posicao amigo, Posicao inimigo)
        {
            EnviarPacote(new SerialPacote()
            {
                Posicao = amigo,
                PosicaoAux = inimigo,
                Info = SerialPacoteEnum.MorteAmbos,
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

        public void IniciarPartida(bool euInicio)
        {
            EnviarPacote(new SerialPacote()
            {
                Info = SerialPacoteEnum.IniciarPartida,
                Inimgo = !euInicio
            });
        }

        public void Saindo()
        {
            EnviarPacote(new SerialPacote()
            {
                Info = SerialPacoteEnum.InimigoSaiu
            });
            Close();
        }

        private void _TimerOpen_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Open();
                if (IsOpen)
                {
                    _TimerOpen.Stop();
                }                    
            }
            catch (Exception)
            {
                Close();
            }
            if (_Events != null)
            {
                _Events.UpdateStatusSerial_Received();
            }
        }

        private void OnDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            _PacoteRecebido = GetFromSerial();

            if (_PacoteRecebido != null && _Events != null)
            {
                //Tratamento
                if (_PacoteRecebido.Info != SerialPacoteEnum.Vitoria &&
                    _PacoteRecebido.Info != SerialPacoteEnum.InimigoSaiu &&
                    _PacoteRecebido.Info != SerialPacoteEnum.InimigoEntrou)
                {
                    _Events.SuaVez_Received();
                }

                switch (_PacoteRecebido.Info)
                {
                    case SerialPacoteEnum.Posicionamento:
                        _Events.PosicionarInimigo_Received(_PacoteRecebido.Posicao, new Peca(_PacoteRecebido.PecaEnum));
                        break;
                    case SerialPacoteEnum.Movimento:
                        _Events.MovimentarInimigo_Received(_PacoteRecebido.Posicao, _PacoteRecebido.PosicaoAux, new Peca(_PacoteRecebido.PecaEnum));
                        break;
                    case SerialPacoteEnum.Morte:
                        if (_PacoteRecebido.Inimgo)
                        {
                            _Events.MatarPecaInimiga_Received(_PacoteRecebido.Posicao, _PacoteRecebido.PosicaoAux);
                        }
                        else
                        {
                            _Events.MatarPecaAmiga_Received(_PacoteRecebido.Posicao, _PacoteRecebido.PosicaoAux);
                        }
                        break;
                    case SerialPacoteEnum.MorteAmbos:
                        _Events.MatarAmbasPeca_Received(_PacoteRecebido.PosicaoAux, _PacoteRecebido.Posicao);
                        break;
                    case SerialPacoteEnum.InimigoSaiu:
                        _InimigoEncontrado = false;
                        _Events.UpdateStatusSerial_Received();
                        
                        break;
                    case SerialPacoteEnum.InimigoEntrou:
                        _InimigoEncontrado = true;
                        _Events.UpdateStatusSerial_Received();
                        EnviarPacote(new SerialPacote()
                        {
                            Info = SerialPacoteEnum.InimigoEntrou
                        });

                        break;
                    case SerialPacoteEnum.Vitoria:
                        _Events.DeclararDerrota_Received();
                        break;
                    case SerialPacoteEnum.IniciarPartida:
                        _Events.IniciarPartida_Received(_PacoteRecebido.Inimgo);
                        break;
                    default://Espiao
                        _Events.EspiarPeca_Received(_PacoteRecebido.Posicao, _PacoteRecebido.PosicaoAux);
                        break;
                }

                _Events.UpdateStatus_Received();
                _Events.AtualizarTabuleiro_Received();
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
                var sp = SerialPacote.ConvertFromString(_Dados);
                _PacotesRecebido.Add(sp);
                return sp;
            }

            return null;
        }

        private void EnviarPacote(SerialPacote sp)
        {
            _PacotesAEnviar.Add(sp);
        }

        private void _Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_InimigoEncontrado)
            {
                if (_PacotesAEnviar.Count() > 0)
                {
                    SerialPacote sp = _PacotesAEnviar.ElementAt(0);
                    _PacotesAEnviar.Remove(sp);
                    this.Write(sp.ToJsonString());
                    if (_Events != null)
                    {
                        _Events.UpdateStatus_Received();
                    }
                }
            }
            else if (IsOpen)
            {
                var sp = new SerialPacote()
                {
                    Info = SerialPacoteEnum.InimigoEntrou
                };
                this.Write(sp.ToJsonString());
            }
        }
    }
}
