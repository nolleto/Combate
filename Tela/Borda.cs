using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tela
{
    class Borda : System.Windows.Forms.PictureBox
    {
        public enum Cores
        {
            Azul = 1,
            Rosa = 2,
            Branco = 3
        }

        private Cores _Cor;

        public Cores Cor
        {
            get
            {
                return _Cor;
            }
            set
            {
                _Cor = value;
                switch (_Cor)
                {
                    case Cores.Azul:
                        this.Load(AppDomain.CurrentDomain.BaseDirectory + "azul.jpg");
                        break;
                    case Cores.Rosa:
                        this.Load(AppDomain.CurrentDomain.BaseDirectory + "rosa.jpg");
                        break;
                    case Cores.Branco:
                        this.Load(AppDomain.CurrentDomain.BaseDirectory + "branco.jpg");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
