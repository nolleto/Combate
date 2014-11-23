using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tela.Classes
{
    public class MyPanel : Panel
    {
        private Guid _Guid { get; set; }
        private bool _Agua;

        private TimerPiscar _Timer;

        public Guid Guid { get { return _Guid; } }
        public bool Agua { get { return _Agua; } }

        public MyPanel()
        {
            this._Guid = Guid.NewGuid();
            this._Timer = new TimerPiscar(this);
        }

        public void SetAgua(bool agua)
        {
            this._Agua = agua;            
        }

        public void Piscar()
        {
            if (!_Agua)
            {
                _Timer.Start();
            }
        }

        public void PararDePiscar()
        {
            _Timer.Stop();
        }
    }
}
