using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tela
{
    class Quadrado : System.Windows.Forms.PictureBox
    {
        public enum Cores
        {
            Branco = 0,
            Azul = 1,
            Verde = 2,
            Rosa = 3
        }

        public int Pos_X = 0;
        public int Pos_Y = 0;

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
                    case Cores.Branco:
                        this.Load(AppDomain.CurrentDomain.BaseDirectory + "branco.jpg");
                        break;
                    case Cores.Azul:
                        this.Load(AppDomain.CurrentDomain.BaseDirectory + "azul.jpg");
                        break;
                    case Cores.Verde:
                        this.Load(AppDomain.CurrentDomain.BaseDirectory + "verde.jpg");
                        break;
                    case Cores.Rosa:
                        this.Load(AppDomain.CurrentDomain.BaseDirectory + "rosa.jpg");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
