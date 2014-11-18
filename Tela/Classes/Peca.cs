using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Tela.Enums;

namespace Tela.Classes
{
    public class Peca
    {
        private AtributosAttribute _Attr { get; set; }
        private Bitmap _Image { get; set; }

        public PecaEnum Type { get; set; }
        public string Nome { get { return Type.ToString(); } }
        public int Forca { get { return _Attr.Forca; } }
        public bool IsSpy { get { return _Attr.Spy; } }
        private bool Bomba { get { return _Attr.Bomba; } }
        private bool Bandeira { get { return _Attr.Bandeira; } }
        private bool Anda { get { return !_Attr.Bomba && !_Attr.Bandeira; } }

        public Guid Guid = Guid.NewGuid();

        public Bitmap Image { get { return _Image; } }

        public Peca(PecaEnum type)
        {
            this.Type = type;
            this._Attr = this.Type.GetAttribute<AtributosAttribute>();

            switch (type)
            {
                case PecaEnum.Marshal:
                    this._Image = Properties.Resources.Marshal;
                    break;
                case PecaEnum.General:
                    this._Image = Properties.Resources.General;
                    break;
                case PecaEnum.Colonels:
                    this._Image = Properties.Resources.Colonel;
                    break;
                case PecaEnum.Majors:
                    this._Image = Properties.Resources.Major;
                    break;
                case PecaEnum.Captains:
                    this._Image = Properties.Resources.Captain;
                    break;
                case PecaEnum.Lieutenants:
                    this._Image = Properties.Resources.Lieutenant;
                    break;
                case PecaEnum.Sergeants:
                    this._Image = Properties.Resources.Sergeant;
                    break;
                case PecaEnum.Miners:
                    this._Image = Properties.Resources.Miner;
                    break;
                case PecaEnum.Scouts:
                    this._Image = Properties.Resources.Scout;
                    break;
                case PecaEnum.Spy:
                    this._Image = Properties.Resources.Spy;
                    break;
                case PecaEnum.Bombs:
                    this._Image = Properties.Resources.Bomb;
                    break;                
                default:
                    this._Image = Properties.Resources.Flag;
                    break;
            }
        }

    }
}
