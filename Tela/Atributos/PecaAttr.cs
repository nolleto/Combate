using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tela.Atributos
{
    public class AtributosAttribute : Attribute
    {
        public static int ForcaMaxima = 10;

        private int _Forca;
        private int _Quantidade;
        private bool _Spy;
        private bool _Bomba;
        private bool _Bandeira;
        private bool _PassoLargo;
        private bool _Desamador;

        public int Forca { get { return _Forca; } }
        public int Quantidade { get { return _Quantidade; } }
        public bool Spy { get { return _Spy; } }
        public bool Bomba { get { return _Bomba; } }
        public bool Bandeira { get { return _Bandeira; } }
        public bool Anda { get { return !_Bomba && !_Bandeira; } }
        public bool PassoLargo { get { return _PassoLargo; } }
        public bool Desamador { get { return _Desamador; } }

        public AtributosAttribute(int forca, int quantidade, bool spy = false, bool passoLargo = false, bool desarmador = false)
        {
            this._Forca = ForcaMaxima - forca;
            this._Quantidade = quantidade;
            this._Spy = spy;
            this._PassoLargo = passoLargo;
            this._Desamador = desarmador;
        }

        public AtributosAttribute(int quantidade, bool bomba = false, bool bandeira = false)
        {
            this._Quantidade = quantidade;
            this._Bomba = bomba;
            this._Bandeira = bandeira;
            this._Forca = 0;
        }
    }
}
