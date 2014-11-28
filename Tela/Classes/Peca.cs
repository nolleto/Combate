using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Tela.Enums;
using Tela.Atributos;

namespace Tela.Classes
{
    public class Peca
    {
        private static Dictionary<PecaEnum, Bitmap> _Imagens;
        private static Dictionary<PecaEnum, Bitmap> Imagens
        {
            get
            {
                if (Peca._Imagens == null) {
                    Peca._Imagens = new Dictionary<PecaEnum, Bitmap>();
                    Peca._Imagens.Add(PecaEnum.Marshal, Properties.Resources.Marshal);
                    Peca._Imagens.Add(PecaEnum.General, Properties.Resources.General);
                    Peca._Imagens.Add(PecaEnum.Colonels, Properties.Resources.Colonel);
                    Peca._Imagens.Add(PecaEnum.Majors, Properties.Resources.Major);
                    Peca._Imagens.Add(PecaEnum.Captains, Properties.Resources.Captain);
                    Peca._Imagens.Add(PecaEnum.Lieutenants, Properties.Resources.Lieutenant);
                    Peca._Imagens.Add(PecaEnum.Sergeants, Properties.Resources.Sergeant);
                    Peca._Imagens.Add(PecaEnum.Miners, Properties.Resources.Miner);
                    Peca._Imagens.Add(PecaEnum.Scouts, Properties.Resources.Scout);
                    Peca._Imagens.Add(PecaEnum.Spy, Properties.Resources.Spy);
                    Peca._Imagens.Add(PecaEnum.Bombs, Properties.Resources.Bomb);
                    Peca._Imagens.Add(PecaEnum.Flags, Properties.Resources.Flag);    
                }
                return Peca._Imagens;
            }
        }

        private AtributosAttribute _Attr { get; set; }
        private Bitmap _Image { get; set; }
        private List<Posicao> _Movimentos = new List<Posicao>();

        public PecaEnum Type { get; set; }
        public string Nome { get { return Type.ToString(); } }
        public int Forca { get { return _Attr.Forca; } }
        public bool IsSpy { get { return _Attr.Spy; } }
        public bool Bomba { get { return _Attr.Bomba; } }
        public bool Bandeira { get { return _Attr.Bandeira; } }
        public bool Anda { get { return !_Attr.Bomba && !_Attr.Bandeira; } }
        public bool PassoLargo { get { return _Attr.PassoLargo; } }
        public bool Desarmador { get { return _Attr.Desamador; } }

        public List<Posicao> Movimentos { get { return _Movimentos; } }

        public Guid Guid = Guid.NewGuid();

        public Bitmap Image { get { return _Image; } }

        public Peca(PecaEnum type)
        {
            this.Type = type;
            this._Attr = this.Type.GetAttribute<AtributosAttribute>();
            this._Image = Peca.Imagens[type];            
        }

        public void AddPosicao(Posicao posicao)
        {
            _Movimentos.Add(posicao);
        }

        public string GetInfo()
        {
            if (Bandeira)
            {
                return "Banderia";
            }
            else if (Bomba)
            {
                return "Bomba";
            }
            else
            {
                return string.Format(
                    "Nome: {0}, Força: {1}",
                    Nome,
                    Forca
                );
            }
        }

        public DueloEnum Atacar(Classes.Peca inimigo)
        {
            if (inimigo.Bandeira)
            {
                return DueloEnum.VenceuJogo;
            }
            else if (inimigo.Bomba && !Desarmador)
            {
                return DueloEnum.Derrota;
            }
            else if (inimigo.Bomba && Desarmador)
            {
                return DueloEnum.Vitoria;
            }
            else if (IsSpy)
            {
                return DueloEnum.Espiao;
            }
            else if (Forca < inimigo.Forca)
            {
                return DueloEnum.Derrota;
            }
            else if (Forca > inimigo.Forca)
            {
                return DueloEnum.Vitoria;
            }
            else
            {
                return DueloEnum.Empate;
            }
        }
    }
}
