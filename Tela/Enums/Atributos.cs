using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tela.Enums
{
    public class AtributosAttribute : Attribute
    {
        private int _Forca;
        private int _Quantidade;
        private bool _Spy;
        private bool _Bomba;
        private bool _Bandeira;

        public int Forca { get { return _Forca; } }
        public int Quantidade { get { return _Quantidade; } }
        public bool Spy { get { return _Spy; } }
        public bool Bomba { get { return _Bomba; } }
        public bool Bandeira { get { return _Bandeira; } }
        public bool Anda { get { return !_Bomba && !_Bandeira; } }

        public AtributosAttribute(int forca, int quantidade, bool spy = false)
        {
            this._Forca = forca;
            this._Quantidade = quantidade;
            this._Spy = spy;
        }

        public AtributosAttribute(int quantidade, bool bomba = false, bool bandeira = false)
        {
            this._Quantidade = quantidade;
            this._Bomba = bomba;
            this._Bandeira = bandeira;
        }
    }
}
