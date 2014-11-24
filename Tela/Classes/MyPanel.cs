using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tela.Classes
{
    public class MyPanel : Panel
    {
        private Guid _Guid;
        private bool _Agua;
        private bool _Inimigo;

        private TimerPiscar _Timer;

        public Guid Guid { get { return _Guid; } }
        public bool Agua { get { return _Agua; } }
        public bool Inimigo { get { return _Inimigo; } }

        public MyPanel()
        {
            this._Guid = Guid.NewGuid();
            this._Timer = new TimerPiscar(this);
        }

        public void SetAgua(bool agua)
        {
            this._Agua = agua;            
        }

        public void SetInimigo(bool inimigo)
        {
            this._Inimigo = inimigo;
        }

        public void Animar()
        {
            if (!_Agua)
            {
                TimerIndicador.Start(this);
                //_Timer.Start();
            }
        }

        public void Desanimar()
        {
            TimerIndicador.Stop();
            //_Timer.Stop();
        }
    }
}
